import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import agent from "../api/agent";
import { useLocation } from "react-router";

export const useActivities = (id?: string) => {

    const queryClient = useQueryClient();
    const location = useLocation();

    // When we [Fetching] [Data] we [use] [useQuery]!!!
    const { data: activities, isPending } = useQuery({
        // The [queryKey] is used internally for refetching, caching, VVVV
        // And sharing your queries throughout your application.
        // Also Its job is to give the incoming data from the backend  VVVV
        // a suitable name to store the incoming data in the cache.
        queryKey: ['activities'],
        queryFn: async () => {
            const response = await agent.get<Activity[]>('/activities');
            return response.data;
        },
        enabled: !id && location.pathname === '/activities'
    });



    const {data: activity, isLoading: isLoadingActivity} = useQuery({
        queryKey:['activities', id],
        queryFn: async () => {
            const response = await agent.get<Activity>(`/activities/${id}`);
            return response.data;
        },
        // What the [enabled] does is [allowing] this [Query / Function] to [RUN] 
        // [Only] if we [Have] the [id].
        // The [double (!!)] will [cast] the [id] into a [boolean].
        enabled: !!id
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


    return {
        activities,
        isPending,
        updateActivity,
        createActivity,
        deleteActivity,
        activity,
        isLoadingActivity
    }
}