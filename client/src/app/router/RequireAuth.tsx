import { Navigate, Outlet, useLocation } from "react-router";
import { useAccount } from "../../lib/hooks/useAccount"
import { Typography } from "@mui/material";

export default function RequireAuth() {
    const { currentUser, loadingUserInfo } = useAccount();
    const location = useLocation();

    if (loadingUserInfo) return <Typography>Loading...</Typography>

    // If [!currentUser] it means the use hasn't [loged In]. 
    // i'll send them to the ['/login'] which is [LoginForm].
    // And the [state={{ from: location }}] I'm sending the [Info] of from where I'm Sending them to the [LoginForm] VVV
    // To redirect them back to where they came from.
    if (!currentUser) return <Navigate to='/login' state={{ from: location }} />

    return (
        <Outlet />
    )
}