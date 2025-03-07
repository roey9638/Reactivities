import axios from "axios";
import { store } from "../store/store";

const sleep = (delay: number) => {
    return new Promise(resolve => {
        setTimeout(resolve, delay);
    })
}


const agent = axios.create({
    // This [import.meta.env.VITE_API_URL] will just take the [URL]
    // That is in the [.env.development] [File] in the [client]
    baseURL: import.meta.env.VITE_API_URL
});


agent.interceptors.request.use(config => {
    store.uiStore.isBusy();
    return config;
})


agent.interceptors.response.use(async response => {
    try {
        await sleep(1000);
        return response;
    } catch (error) {
        console.log(error);
        return Promise.reject(error);
    } finally {
        store.uiStore.isIdle();
    }
})

export default agent;