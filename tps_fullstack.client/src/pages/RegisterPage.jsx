import { useState } from 'react';
import RegisterUI from '../components/Auth/RegisterUI';
import { useAuth } from '../hooks/useAuth';

const RegisterPage = () => {
    const { handleRegister, isLoading, error } = useAuth();
    const [successMessage, setSuccessMessage] = useState('');

    const handleSubmit = async (credentials) => {
        setSuccessMessage('');
        try {
            await handleRegister(credentials.email, credentials.password, credentials.confirmPassword);
            setSuccessMessage('Đăng ký thành công. Bạn có thể đăng nhập bằng tài khoản vừa tạo.');
        } catch {
            // Lỗi đã được xử lý và hiển thị bởi hook useAuth.
        }
    };

    return (
        <RegisterUI
            onSubmit={handleSubmit}
            isLoading={isLoading}
            error={error}
            successMessage={successMessage}
        />
    );
};

export default RegisterPage;
