import { createContext } from "react";
import type { User } from "../types/types";

export const AuthContext = createContext<{
    user: User | null;
    login: (user: User) => void;
    logout: () => void;
}>({
    user: null,
    login: () => { },
    logout: () => { }
});