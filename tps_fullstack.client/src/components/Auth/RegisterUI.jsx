import { useState } from 'react';
import { Link } from 'react-router-dom';

const RegisterUI = ({ onSubmit, isLoading, error, successMessage }) => {
    const [formData, setFormData] = useState({
        email: '',
        password: '',
        confirmPassword: ''
    });
    const [showPassword, setShowPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const [clientError, setClientError] = useState('');

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
        setClientError('');
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        if (formData.password !== formData.confirmPassword) {
            setClientError('Mật khẩu xác nhận không khớp.');
            return;
        }

        onSubmit(formData);
    };

    const visibleError = clientError || error;

    return (
        <div className="min-h-screen flex items-center justify-center p-4 lg:p-8 bg-surface-container-low relative overflow-hidden">
            <div className="absolute top-0 left-0 w-full h-full overflow-hidden z-0 pointer-events-none">
                <div className="absolute -top-[20%] -right-[10%] w-[60%] h-[60%] rounded-full opacity-10 blur-3xl bg-gradient-to-br from-primary to-primary-container"></div>
                <div className="absolute -bottom-[20%] -left-[10%] w-[50%] h-[50%] rounded-full opacity-[0.05] blur-3xl bg-primary"></div>
            </div>

            <div className="w-full max-w-6xl mx-auto flex flex-col lg:flex-row items-center justify-between gap-12 z-10 relative">
                <div className="hidden lg:flex flex-col w-1/2 pr-12">
                    <div className="mb-8">
                        <span className="font-headline text-3xl font-extrabold tracking-tighter text-primary">The Ledger</span>
                    </div>
                    <h1 className="font-headline text-5xl font-bold leading-tight mb-6 text-on-surface">
                        Join The<br />Academic Hub
                    </h1>
                    <p className="font-body text-lg text-on-surface-variant mb-10 max-w-md">
                        Tạo tài khoản để quản lý chứng chỉ, khóa học và lộ trình học tập trong cùng một hệ thống.
                    </p>
                    <div className="space-y-6">
                        <div className="flex items-start gap-4">
                            <div className="w-10 h-10 rounded-full bg-surface-container-lowest shadow-[0px_4px_12px_rgba(25,28,30,0.06)] flex items-center justify-center flex-shrink-0">
                                <span className="material-symbols-outlined text-primary">person_add</span>
                            </div>
                            <div>
                                <h3 className="font-headline font-semibold text-on-surface">Fast Access</h3>
                                <p className="font-body text-sm text-on-surface-variant">Đăng ký bằng email và bắt đầu sử dụng hệ thống ngay.</p>
                            </div>
                        </div>
                        <div className="flex items-start gap-4">
                            <div className="w-10 h-10 rounded-full bg-surface-container-lowest shadow-[0px_4px_12px_rgba(25,28,30,0.06)] flex items-center justify-center flex-shrink-0">
                                <span className="material-symbols-outlined text-primary">lock</span>
                            </div>
                            <div>
                                <h3 className="font-headline font-semibold text-on-surface">Secure Identity</h3>
                                <p className="font-body text-sm text-on-surface-variant">Thông tin đăng nhập được gửi qua API đăng ký của hệ thống.</p>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="w-full lg:w-[480px] flex-shrink-0">
                    <div className="bg-white/85 backdrop-blur-[20px] rounded-xl shadow-[0px_12px_32px_rgba(25,28,30,0.08)] p-8 lg:p-10 relative overflow-hidden">
                        <div className="absolute top-0 left-0 w-full h-1 bg-gradient-to-r from-primary to-primary-container"></div>

                        <div className="lg:hidden mb-8 text-center">
                            <span className="font-headline text-2xl font-extrabold tracking-tighter text-primary">The Ledger</span>
                        </div>

                        <div className="mb-8">
                            <h2 className="font-headline text-2xl font-bold text-on-surface mb-2">Đăng ký</h2>
                            <p className="font-body text-sm text-on-surface-variant">Tạo tài khoản mới bằng email của bạn</p>
                        </div>

                        {visibleError && (
                            <div className="mb-6 p-3 rounded-lg bg-error-container text-error font-body text-sm">
                                {visibleError}
                            </div>
                        )}

                        {successMessage && (
                            <div className="mb-6 p-3 rounded-lg bg-green-50 text-green-700 font-body text-sm">
                                {successMessage}
                            </div>
                        )}

                        <form onSubmit={handleSubmit} className="space-y-6">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-medium text-on-surface" htmlFor="email">
                                    Email
                                </label>
                                <div className="relative">
                                    <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                        <span className="material-symbols-outlined text-outline-variant text-sm">mail</span>
                                    </div>
                                    <input
                                        id="email"
                                        name="email"
                                        type="email"
                                        placeholder="name@example.com"
                                        required
                                        autoComplete="email"
                                        disabled={isLoading}
                                        value={formData.email}
                                        onChange={handleChange}
                                        className="w-full pl-10 pr-4 py-3 bg-surface-container-highest border-none rounded-md text-on-surface placeholder:text-outline focus:ring-2 focus:ring-primary/40 focus:bg-surface-container-lowest transition-all font-body text-sm"
                                    />
                                </div>
                            </div>

                            <div className="space-y-2">
                                <label className="font-label text-sm font-medium text-on-surface" htmlFor="password">
                                    Mật khẩu
                                </label>
                                <div className="relative">
                                    <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                        <span className="material-symbols-outlined text-outline-variant text-sm">lock</span>
                                    </div>
                                    <input
                                        id="password"
                                        name="password"
                                        type={showPassword ? 'text' : 'password'}
                                        placeholder="••••••••"
                                        required
                                        autoComplete="new-password"
                                        disabled={isLoading}
                                        value={formData.password}
                                        onChange={handleChange}
                                        className="w-full pl-10 pr-10 py-3 bg-surface-container-highest border-none rounded-md text-on-surface placeholder:text-outline focus:ring-2 focus:ring-primary/40 focus:bg-surface-container-lowest transition-all font-body text-sm"
                                    />
                                    <button
                                        type="button"
                                        onClick={() => setShowPassword(!showPassword)}
                                        className="absolute inset-y-0 right-0 pr-3 flex items-center text-outline-variant hover:text-on-surface-variant transition-colors"
                                        aria-label={showPassword ? 'Ẩn mật khẩu' : 'Hiện mật khẩu'}
                                    >
                                        <span className="material-symbols-outlined text-sm">
                                            {showPassword ? 'visibility' : 'visibility_off'}
                                        </span>
                                    </button>
                                </div>
                            </div>

                            <div className="space-y-2">
                                <label className="font-label text-sm font-medium text-on-surface" htmlFor="confirmPassword">
                                    Xác nhận mật khẩu
                                </label>
                                <div className="relative">
                                    <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                        <span className="material-symbols-outlined text-outline-variant text-sm">verified_user</span>
                                    </div>
                                    <input
                                        id="confirmPassword"
                                        name="confirmPassword"
                                        type={showConfirmPassword ? 'text' : 'password'}
                                        placeholder="••••••••"
                                        required
                                        autoComplete="new-password"
                                        disabled={isLoading}
                                        value={formData.confirmPassword}
                                        onChange={handleChange}
                                        className="w-full pl-10 pr-10 py-3 bg-surface-container-highest border-none rounded-md text-on-surface placeholder:text-outline focus:ring-2 focus:ring-primary/40 focus:bg-surface-container-lowest transition-all font-body text-sm"
                                    />
                                    <button
                                        type="button"
                                        onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                                        className="absolute inset-y-0 right-0 pr-3 flex items-center text-outline-variant hover:text-on-surface-variant transition-colors"
                                        aria-label={showConfirmPassword ? 'Ẩn mật khẩu xác nhận' : 'Hiện mật khẩu xác nhận'}
                                    >
                                        <span className="material-symbols-outlined text-sm">
                                            {showConfirmPassword ? 'visibility' : 'visibility_off'}
                                        </span>
                                    </button>
                                </div>
                            </div>

                            <button
                                type="submit"
                                disabled={isLoading}
                                className={`w-full py-3 px-4 text-white rounded-lg font-label font-medium text-base shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary transition-all uppercase tracking-wide bg-primary ${isLoading ? 'opacity-70 cursor-not-allowed' : 'hover:opacity-90'}`}
                            >
                                {isLoading ? 'Đang xử lý...' : 'Đăng ký'}
                            </button>
                        </form>

                        <p className="mt-6 text-center font-body text-sm text-on-surface-variant">
                            Đã có tài khoản?
                            <Link className="ml-1 font-medium text-primary hover:text-primary-container transition-colors" to="/login">
                                Đăng nhập
                            </Link>
                        </p>
                    </div>

                    <div className="mt-6 flex items-center justify-center gap-2 text-outline text-xs font-label">
                        <span className="material-symbols-outlined text-[16px]">lock</span>
                        <span>Secure, encrypted connection</span>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default RegisterUI;
