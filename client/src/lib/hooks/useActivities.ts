import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import { useLocation } from "react-router";
import { useAccount } from "./useAccount";

export const useActivities = (id?: string) => {

    const queryClient = useQueryClient();
    const { currentUser } = useAccount();
    const location = useLocation();

    // When we [Fetching] [Data] we [use] [useQuery]!!!
    const { data: activities, isLoading } = useQuery({
        // The [queryKey] is used internally for refetching, caching, VVVV
        // And sharing your queries throughout your application.
        // Also Its job is to give the incoming data from the backend  VVVV
        // a suitable name to store the incoming data in the cache.
        queryKey: ['activities'],
        queryFn: async () => {
            const response = await agent.get<Activity[]>('/activities');
            return response.data;
        },
        // The [enabled] allows this [useQuery] to [Execute] [Only if] VVVV
        // 1) We [Don't] have an [!id] [Passed] into the [useActivities()] [hook] in other [Components] that [use] that [hook].
        // 2) The [location] that the [user] is in is [location.pathname === '/activities'].
        // 3) If we [Don't have] the [currentUser] as in [!!currentUser]. Then Don't [Excute] this [useQuery].
        enabled: !id && location.pathname === '/activities' && !!currentUser,
        select: data => {
            return data.map(activity => {
                return {
                    ...activity, // [copies] all existing [properties] from each [activity] object.
                    isHost: currentUser?.id === activity.hostId, // checks if the [currentUser] is the [host] of the [activity].
                    isGoing: activity.attendees.some(x => x.id === currentUser?.id) // checks if the [currentUser] is [listed] in the [activity's] [attendees].
                }
            })
        }
    });



    const { data: activity, isLoading: isLoadingActivity } = useQuery({
        queryKey: ['activities', id],
        queryFn: async () => {
            const response = await agent.get<Activity>(`/activities/${id}`);
            return response.data;
        },
        // What the [enabled] does is [allowing] this [Query / Function] to [RUN] 
        // 1) [Only] if we [Have] the [id].
        // 2) If we [Don't have] the [currentUser] as in [!!currentUser]. Then Don't [Excute] this [useQuery].
        // The [double (!!)] will [cast] the [id] into a [boolean].
        enabled: !!id && !!currentUser,
        select: data => {
            return {
                ...data,
                isHost: currentUser?.id === data.hostId,
                isGoing: data.attendees.some(x => x.id === currentUser?.id)
            }
        }
    })


    // When we [Updating] [Data] we [use] [useMutation]!!!
    const updateActivity = useMutation({
        mutationFn: async (activity: Activity) => {
            await agent.put('/activities', activity)
        },

        // The [onSuccess] is to [invalidate/ Reset] a [Query]. Which in this case we chose the [activities].
        // We do that [Because] we [Updating] an [Activity] in this [Function] VVV
        // so the [activities] [query] isn't [valid / updated] [anymore].
        // [Maybe Not Correct] --> So then [next time] we [use] the [useQuery] from the [Function] [above] VVV
        // It will get the [Updated] [data].
        onSuccess: async () => {
            await queryClient.invalidateQueries({
                queryKey: ['activities']
            })
        }
    })


    const createActivity = useMutation({
        mutationFn: async (activity: Activity) => {
            const response = await agent.post('/activities', activity);
            return response.data;
        },

        onSuccess: async () => {
            await queryClient.invalidateQueries({
                queryKey: ['activities']
            })
        }
    })


    const deleteActivity = useMutation({
        mutationFn: async (id: string) => {
            await agent.delete(`/activities/${id}`)
        },

        onSuccess: async () => {
            await queryClient.invalidateQueries({
                queryKey: ['activities']
            })
        }
    })

    const updateAttendance = useMutation({
        mutationFn: async (id: string) => {
            await agent.post(`/activities/${id}/attend`)
        },
        onMutate: async (activityId: string) => {
            await queryClient.cancelQueries({ queryKey: ['activities', activityId] });

            /*  
                It looks in the cache for data related to an activity with the given activityId. 
                If it finds it, it returns that data and stores it in prevActivity. If not, prevActivity will be undefined 
            */
            const prevActivity = queryClient.getQueryData<Activity>(['activities', activityId]);

            /* The [oldActivity] is the [current] [cached value], which is the [same] as [prevActivity]. BUT VVV */
            /* 
               Here I'm [Modifing] the [oldActivity]. And if everything is [Ok] then will [Return] it
               If Not then will [Return] the [prevActivity]  
            */
            queryClient.setQueryData<Activity>(['activities', activityId], oldActivity => {
                if (!oldActivity || !currentUser) {
                    return oldActivity
                }

                const isHost = oldActivity.hostId === currentUser.id;
                const isAttending = oldActivity.attendees.some(x => x.id === currentUser.id);

                return {
                    ...oldActivity,
                    /*
                        // If [isHost] is [true] → set [isCancelled] to the [opposite] of [oldActivity.isCancelled]
                        // If [isHost] is [false] → set [isCancelled] to [oldActivity.isCancelled] (no change).
                    */
                    isCancelled: isHost ? !oldActivity.isCancelled : oldActivity.isCancelled,
                    /* Here I'm checking if the [currentUser] is [isAttending]  */
                    attendees: isAttending
                        // Then I'm checking If the [currentUser] is the [host] VVV
                        // If he is the [Host] then Just [keep] the [oldActivity.attendees] list the same. 
                        ? isHost
                            ? oldActivity.attendees
                            /* Here I'm removing the [currentUser] if is [Not] the [Host]  */
                            : oldActivity.attendees.filter(x => x.id !== currentUser.id)
                        // Here if the [currentUser] is [not] [attending] Then VVV
                        // [Add] the [currentUser] to the [list] of [attendees] VVV
                        // (by copying the old list and adding their info to the end).
                        : [...oldActivity.attendees, {
                            id: currentUser.id,
                            displayName: currentUser.displayName,
                            imageUrl: currentUser.imageUrl
                        }]
                }
            });

            return { prevActivity };
        },
        onError: (error, activityId, context) => {
            console.log('prevActivity' + context?.prevActivity);
            console.log(error);
            if (context?.prevActivity) {
                // If theres an [Error] will just [set] the [prevActivity]
                queryClient.setQueryData(['activities', activityId], context.prevActivity)
            }
        }
    })


    return {
        activities,
        isLoading,
        updateActivity,
        createActivity,
        deleteActivity,
        activity,
        isLoadingActivity,
        updateAttendance
    }
}