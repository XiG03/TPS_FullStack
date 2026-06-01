import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login, register } from '../services/authService';
import { setRefreshToken, setToken, removeToken } from '../services/httpClient';

const decodeTokenPayload = (token) => {
    try {
        const base64Payload = token.split('.')[1]?.replace(/-/g, '+').replace(/_/g, '/');
        return base64Payload ? JSON.parse(atob(base64Payload)) : {};
    } catch (err) {
        console.error(err);
        return {};
    }
};

const getRoleFromPayload = (payload) => {
    const role = payload?.role || payload?.Role || payload?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    return Array.isArray(role) ? role[0] : role;
};

const isTeacherRole = (role) => {
    const normalized = String(role || '').trim().toLowerCase();
    return ['teacher', 'giangvien', 'giảng viên', 'lecturer'].includes(normalized);
};

const isStudentRole = (role) => {
    const normalized = String(role || '').trim().toLowerCase();
    return ['student', 'hocvien', 'học viên', 'learner'].includes(normalized);
};

const getField = (source, ...keys) => {
    for (const key of keys) {
        if (source?.[key] != null) return source[key];
    }

    return null;
};

export const useAuth = () => {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const navigate = useNavigate();

    const handleLogin = async (username, password, rememberMe) => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await login(username, password, rememberMe);
            const authData = response.data || {};
            const tempToken = getField(authData, 'tempToken', 'TempToken');

            if (tempToken === 'First_Login') {
                setError('Tài khoản đăng nhập lần đầu và cần được kích hoạt trước khi vào hệ thống.');
                return authData;
            }

            const token = getField(authData, 'accessToken', 'AccessToken', 'token', 'Token');
            const refreshToken = getField(authData, 'refreshToken', 'RefreshToken');

            if (!token) {
                throw new Error(response.message || 'Không nhận được access token từ máy chủ.');
            }

            setToken(token, rememberMe);
            if (refreshToken) {
                setRefreshToken(refreshToken, rememberMe);
            }

            const role = getRoleFromPayload(decodeTokenPayload(token)) || getField(authData, 'role', 'Role');
            if (isTeacherRole(role)) {
                navigate('/teacher/me');
            } else if (isStudentRole(role)) {
                navigate('/student/me');
            } else {
                navigate('/courses');
            }

            return authData;
        } catch (err) {
            setError(err.message || 'Email hoặc mật khẩu không đúng. Vui lòng thử lại.');
            throw err;
        } finally {
            setIsLoading(false);
        }
    };

    const handleRegister = async (email, password, confirmPassword) => {
        setIsLoading(true);
        setError(null);
        try {
            const response = await register(email, password, confirmPassword);
            return response.data || {};
        } catch (err) {
            setError(err.message || 'Đăng ký thất bại. Vui lòng thử lại.');
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
        handleRegister,
        handleLogout,
    };
};
