import React from 'react';
import LoginUI from '../components/Auth/LoginUI';
import { useAuth } from '../hooks/useAuth';

const LoginPage = () => {
    const { handleLogin, isLoading, error } = useAuth();

    const handleSubmit = async (credentials) => {
        try {
            await handleLogin(credentials.email, credentials.password, credentials.rememberMe);
            // Xử lý sau khi đăng nhập thành công (ví dụ: chuyển hướng trang)
            alert("Đăng nhập thành công!");
        } catch (err) {
            // Lỗi đã được xử lý ở hook, có thể thêm logic phụ nếu cần
            console.error('Đăng nhập thất bại:', err);
        }
    };

    const handleGoogleSignIn = () => {
        console.log('User clicked Google Sign In');
        // TODO: Tích hợp logic đăng nhập bằng Google (ví dụ Firebase/OAuth)
        alert('Tính năng đăng nhập Google đang được phát triển');
    };

    return (
        <LoginUI 
            onSubmit={handleSubmit} 
            isLoading={isLoading} 
            error={error} 
            onGoogleSignIn={handleGoogleSignIn} 
        />
    );
};

export default LoginPage;
