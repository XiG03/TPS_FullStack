import { useState } from 'react';
import { login } from '../services/authService';

export const useAuth = () => {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const handleLogin = async (email, password, rememberMe) => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await login(email, password);
            // Có thể lưu token vào localStorage ở đây hoặc thông qua global state (Redux/Zustand)
            console.log('Login success:', response.data, 'Remember:', rememberMe);
            return response.data;
        } catch (err) {
            setError('Email hoặc mật khẩu không đúng. Vui lòng thử lại.');
            throw err;
        } finally {
            setIsLoading(false);
        }
    };

    return {
        isLoading,
        error,
        handleLogin
    };
};
