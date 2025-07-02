import { useLocalObservable } from "mobx-react-lite"
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr'
import { useEffect, useRef } from "react";
import { runInAction } from "mobx";

export const useComments = (activityId?: string) => {

    const created = useRef(false);

    const commentStore = useLocalObservable(() => ({
        comments: [] as ChatComment[],
        hubConnection: null as HubConnection | null,

        // This function takes an [activityId], 
        // which [identifies] which [activity's comments] you want to [connect] to.
        createHubConnection(activityId: string) {

            // If no [activityId] is [provided], it [stops] and does nothing.
            if (!activityId) return;

            this.hubConnection = new HubConnectionBuilder()
                // It sets the [server URL] to [connect] to, and [includes] the [activityId] in the [URL]
                .withUrl(`${import.meta.env.VITE_COMMENTS_URL}?activityId=${activityId}`, {
                    withCredentials: true
                })
                .withAutomaticReconnect()
                .build();

            this.hubConnection.start()
                .catch(error => console.log('Error establishing connection: ', error))

            // The [LoadComments] has to be [exactly] the [Same] VVV
            // as we [called] it in the [CommentHub.] [Class] in the [OnConnectedAsync()] [Function]
            this.hubConnection.on('LoadComments', comments => {
                runInAction(() => {
                    this.comments = comments
                })
            })

            // The [ReceiveComment] has to be [exactly] the [Same] VVV
            // as we [called] it in the [CommentHub.] [Class] in the [SendComment()] [Function]
            this.hubConnection.on('ReceiveComment', comment => {
                runInAction(() => {
                    this.comments.unshift(comment);
                })
            })
        },

        stopHubConnection() {
            // Checks if there's an [existing connection], and if it's [currently connected].
            // If [connected], it [stops] the [connection]. If [something] goes [wrong], it logs the [error].
            if (this.hubConnection?.state === HubConnectionState.Connected) {
                this.hubConnection.stop()
                    .catch(error => console.log('Error Stoping Connection: ', error))
            }
        }
    }));


    // When this [hook] [useComments()] is [used] in a [component] VVV
    // 1) The [useEffect] [runs] when the [component] [loads] or when [activityId] [changes]
    // 2) When the [component] [unmounts] or [activityId] [changes], it [cleans up] by [stopping] the [connection].
    useEffect(() => {
        // The [!created.current] is to make this [Component] to [Run] [Once]
        if (activityId && !created.current) {
            commentStore.createHubConnection(activityId);
            created.current = true;
        }

        return () => {
            commentStore.stopHubConnection();
            commentStore.comments = [];
        }
    }, [activityId, commentStore])

    return {
        commentStore
    }
}