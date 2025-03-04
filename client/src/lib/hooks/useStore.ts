import { useContext } from "react";
import { StoreContext } from "../store/store";

export default function useStore() {
    return useContext(StoreContext);
}