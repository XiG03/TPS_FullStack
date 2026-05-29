import LoginUI from '../components/Auth/LoginUI';
import { useAuth } from '../hooks/useAuth';

const LoginPage = () => {
    const { handleLogin, isLoading, error } = useAuth();

    const handleSubmit = async (credentials) => {
        try {
            await handleLogin(credentials.username, credentials.password, credentials.rememberMe);
        } catch {
            // Lỗi đã được xử lý và hiển thị bởi hook useAuth.
        }
    };

    const handleGoogleSignIn = () => {
        console.log('User clicked Google Sign In');
        // TODO: Tích hợp logic đăng nhập bằng Google (ví dụ Firebase/OAuth).
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
