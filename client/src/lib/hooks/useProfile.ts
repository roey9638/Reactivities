import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import agent from "../api/agent"
import { useMemo } from "react";

export const useProfile = (id?: string) => {

    const queryClient = useQueryClient();

    const { data: profile, isLoading: loadingProfile } = useQuery<Profile>({
        queryKey: ['profile', id],
        queryFn: async () => {
            /* 
                This [`/profiles/${id}`] will [Go] [into] the [API] to the [ProfilesController] VVV
                And will Go to the [GetProfile()] [Function]
            */
            const response = await agent.get<Profile>(`/profiles/${id}`);
            return response.data;
        },
        enabled: !!id
    })


    const { data: photos, isLoading: loadingPhotos } = useQuery<Photo[]>({
        queryKey: ['photos', id],
        queryFn: async () => {
            /* 
                This [`/profiles/${id}/photos`] will [Go] [into] the [API] to the [ProfilesController] VVV
                And will Go to the [GetPhotosForUser()] [Function]
            */
            const response = await agent.get<Photo[]>(`/profiles/${id}/photos`);
            return response.data;
        },
        enabled: !!id
    })


    const uploadPhoto = useMutation({
        mutationFn: async (file: Blob) => {
            /* 
                We use this [new FormData()] Because in [order] to [send] a [file] 
                then we need to use form data.
            */
            const formData = new FormData();
            formData.append('file', file); // has to be [called] ['file']
            /* 
                This ['/profiles/add-photo'] will [Go] [into] the [API] to the [ProfilesController] VVV
                And will Go to the [AddPhoto()] [Function]
            */
            const response = await agent.post('/profiles/add-photo', formData, {
                headers: { 'Content-Type': 'multipart/form-data' }
            });
            return response.data;
        },
        onSuccess: async (photo: Photo) => {
            await queryClient.invalidateQueries({
                queryKey: ['photos', id]
            });
            // Here I'm [fetching] the [user] i want to [set] his [Data]
            queryClient.setQueryData(['user'], (data: User) => {
                if (!data) return data;
                return {
                    ...data,
                    // If the [data.imageUrl] of the [user] we just [grabbed] is [null]. Then [set] it to [photo.url].
                    imageUrl: data.imageUrl ?? photo.url
                }
            });
            queryClient.setQueryData(['profile', id], (data: Profile) => {
                if (!data) return data;
                return {
                    ...data,
                    // If the [data.imageUrl] of the [Profile] we just [grabbed] is [null]. Then [set] it to [photo.url].
                    imageUrl: data.imageUrl ?? photo.url
                }
            });
        }
    })


    const setMainPhoto = useMutation({
        mutationFn: async (photo: Photo) => {
            await agent.put(`/profiles/${photo.id}/setMain`)
        },
        onSuccess: (_, photo) => {
            queryClient.setQueryData(['user'], (userData: User) => {
                if (!userData) return userData;
                return {
                    ...userData,
                    imageUrl: photo.url
                }
            });
            queryClient.setQueryData(['profile', id], (profile: Profile) => {
                if (!profile) return profile;
                return {
                    ...profile,
                    imageUrl: photo.url
                }
            })
        }
    })


    const deletePhoto = useMutation({
        mutationFn: async (photoId: string) => {
            await agent.delete(`/profiles/${photoId}/photos`)
        },
        onSuccess: (_, photoId) => {
            queryClient.setQueryData(['photos', id], (photos: Photo[]) => {
                return photos?.filter(x => x.id !== photoId)
            })
        }
    });


    // This [useMemo] will only [recompute] the [memorized] [value] when [one] of the [dependencies] has [changed].
    const isCurrentUser = useMemo(() => {
        // This ['user'] will come from the [useAccount] [Hook] in the [Query] that has that [queryKey] with that [Name] => ['user']. VVV
        // It will [Basically] will [give] us the [User] [Object].
        return id === queryClient.getQueryData<User>(['user'])?.id
    }, [id, queryClient])


    return {
        profile,
        loadingProfile,
        photos,
        loadingPhotos,
        isCurrentUser,
        uploadPhoto,
        setMainPhoto,
        deletePhoto
    }
}