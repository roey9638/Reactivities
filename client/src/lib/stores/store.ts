import { createContext } from "react";
import CounterStore from "./counterStore";
import UiStore from "./uiStore";

// Here i will [use] the [Stores] that will hold my [States]
// And Make a [Global] [Context / (StoreContext)] to access those [Stores] that has [States]


interface Store {
    counterStore: CounterStore;
    uiStore: UiStore;
}

export const store: Store = {
    counterStore: new CounterStore(),
    uiStore: new UiStore()
}

export const StoreContext = createContext(store);
