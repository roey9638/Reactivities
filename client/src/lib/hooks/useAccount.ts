import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { LoginSchema } from "../schemas/loginSchema"
import agent from "../api/agent"
import { useNavigate } from "react-router";
import { RegisterSchema } from "../schemas/registerSchema";
import { toast } from "react-toastify";

export const useAccount = () => {

    const queryClient = useQueryClient();
    const navigate = useNavigate();


    // After the [User] [Logs in] we want to [fetch] his [data]. So that's what the [onSuccess] doing.
    // It's [invalidating] the [user] by using the [queryKey] because the [data] is [cached] in there VVV
    // And then the becuase the [queryKey: ['user']] is inside a [useQuery] [Function]. VVV
    // The [Function] will [execute] [automatically] and [fetch] his [data] [again].
    const loginUser = useMutation({
        mutationFn: async (creds: LoginSchema) => {
            await agent.post('/login?useCookies=true', creds);
        },
        onSuccess: async () => {
            await queryClient.invalidateQueries({
                queryKey: ['user']
            });
        }
    });


    const registerUser = useMutation({
        mutationFn: async (creds: RegisterSchema) => {
            await agent.post('/account/register', creds)
        },
        onSuccess: () => {
            toast.success('Register successful - you can now login');
            navigate('/login');
        }
    })


    const logoutUser = useMutation({
        mutationFn: async () => {
            await agent.post('/account/logout');
        },
        onSuccess: () => {
            queryClient.removeQueries({ queryKey: ['user'] }); // I'm removing this because the [User] [logs out] 
                                                               // So i don't have [his] [data] [cached].

            queryClient.removeQueries({ queryKey: ['activities'] }); // I'm removing this because the [User] [logs out] 
                                                                     // So i don't have the [activities] [data] [cached].
            navigate('/');
        }
    })


    const { data: currentUser, isLoading: loadingUserInfo } = useQuery({
        queryKey: ['user'],
        queryFn: async () => {
            const response = await agent.get<User>('account/user-info');
            return response.data;
        },
        // Here i making sure that this [useQuery] is [executing] [ONLY] if 
        // 1) I don't have the [User] [data] [cached] already.
        enabled: !queryClient.getQueryData(['user'])
    })

    return {
        loginUser,
        currentUser,
        logoutUser,
        loadingUserInfo,
        registerUser
    }
}