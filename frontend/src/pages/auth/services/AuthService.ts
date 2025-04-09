import axiosClient from "@/api/axiosClient";
import { AuthEndpoints } from '@/api/endpoints';
import type { Credentials } from '@/types';

/**
 * Service class for handling auth-related API calls
 */
export default class AuthService {
    /**
    Submit login POST request
    **/
    static async login(credentials: Credentials): Promise<any> {
        const response = await axiosClient.post(AuthEndpoints.login(), credentials);
        const authToken = response.data.token;
        sessionStorage.setItem("authToken", authToken);
        axiosClient.defaults.headers.common.Authorization = `Bearer ${authToken}`;
        console.log(response.data);

        return response.data;
    }

    /**
    Logout of application
    **/
    static async logout(): Promise<any> {
        sessionStorage.removeItem("authToken");
        axiosClient.defaults.headers.common.Authorization = null;
        return;
    };

}