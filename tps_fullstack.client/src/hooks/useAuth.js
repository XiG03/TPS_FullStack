import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../services/authService';
import { setToken, removeToken } from '../services/httpClient';

export const useAuth = () => {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleLogin = async (email, password, rememberMe) => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await login(email, password);
            const token = response.data?.AccessToken || response.data?.accessToken || response.data?.token || response.data?.Token;
            if (token) {
                setToken(token);
            }
            navigate('/courses');
            return response.data;
        } catch (err) {
            setError('Email hoặc mật khẩu không đúng. Vui lòng thử lại.');
            throw err;
        } finally {
            setIsLoading(false);
        }
    };

    const handleLogout = () => {
        removeToken();
        navigate('/login');
    };

    return {
        isLoading,
        error,
        handleLogin,
        handleLogout,
    };
};
