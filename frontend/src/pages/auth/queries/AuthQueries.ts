import { useMutation } from '@tanstack/react-query';
import AuthService from '@/pages/auth/services/AuthService';
import type { Credentials } from '@/types';

export const useLogin = () => {
    return useMutation({
        mutationFn: async (credentials: Credentials) =>
            AuthService.login(credentials),
    });
};