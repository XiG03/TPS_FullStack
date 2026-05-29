import { useState } from 'react';

const LoginUI = ({ onSubmit, isLoading, error, onGoogleSignIn }) => {
    const [formData, setFormData] = useState({
        username: '',
        password: '',
        rememberMe: false
    });
    const [showPassword, setShowPassword] = useState(false);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData(prev => ({
            ...prev,
            [name]: type === 'checkbox' ? checked : value
        }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(formData);
    };

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
                        The Academic<br />Authority
                    </h1>
                    <p className="font-body text-lg text-on-surface-variant mb-10 max-w-md">
                        Enter your credentials to access the premier certification management and learning experience.
                    </p>
                    <div className="space-y-6">
                        <div className="flex items-start gap-4">
                            <div className="w-10 h-10 rounded-full bg-surface-container-lowest shadow-[0px_4px_12px_rgba(25,28,30,0.06)] flex items-center justify-center flex-shrink-0">
                                <span className="material-symbols-outlined text-primary">verified</span>
                            </div>
                            <div>
                                <h3 className="font-headline font-semibold text-on-surface">Verified Achievements</h3>
                                <p className="font-body text-sm text-on-surface-variant">Immutable records of your academic progress.</p>
                            </div>
                        </div>
                        <div className="flex items-start gap-4">
                            <div className="w-10 h-10 rounded-full bg-surface-container-lowest shadow-[0px_4px_12px_rgba(25,28,30,0.06)] flex items-center justify-center flex-shrink-0">
                                <span className="material-symbols-outlined text-primary">menu_book</span>
                            </div>
                            <div>
                                <h3 className="font-headline font-semibold text-on-surface">Curated Pathways</h3>
                                <p className="font-body text-sm text-on-surface-variant">Structured learning designed for mastery.</p>
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
                            <h2 className="font-headline text-2xl font-bold text-on-surface mb-2">Đăng nhập</h2>
                            <p className="font-body text-sm text-on-surface-variant">Truy cập bảng điều khiển chứng chỉ của bạn</p>
                        </div>

                        {error && (
                            <div className="mb-6 p-3 rounded-lg bg-error-container text-error font-body text-sm">
                                {error}
                            </div>
                        )}

                        <form onSubmit={handleSubmit} className="space-y-6">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-medium text-on-surface" htmlFor="username">
                                    Tên đăng nhập
                                </label>
                                <div className="relative">
                                    <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                                        <span className="material-symbols-outlined text-outline-variant text-sm">person</span>
                                    </div>
                                    <input
                                        id="username"
                                        name="username"
                                        type="text"
                                        placeholder="Nhập tên đăng nhập"
                                        required
                                        autoComplete="username"
                                        disabled={isLoading}
                                        value={formData.username}
                                        onChange={handleChange}
                                        className="w-full pl-10 pr-4 py-3 bg-surface-container-highest border-none rounded-md text-on-surface placeholder:text-outline focus:ring-2 focus:ring-primary/40 focus:bg-surface-container-lowest transition-all font-body text-sm"
                                    />
                                </div>
                            </div>

                            <div className="space-y-2">
                                <div className="flex items-center justify-between">
                                    <label className="font-label text-sm font-medium text-on-surface" htmlFor="password">
                                        Mật khẩu
                                    </label>
                                    <a href="#" className="font-label text-sm font-medium text-primary hover:text-primary-container transition-colors">
                                        Quên mật khẩu?
                                    </a>
                                </div>
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
                                        autoComplete="current-password"
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

                            <div className="flex items-center">
                                <input
                                    id="remember-me"
                                    name="rememberMe"
                                    type="checkbox"
                                    checked={formData.rememberMe}
                                    onChange={handleChange}
                                    disabled={isLoading}
                                    className="h-4 w-4 rounded border-outline-variant text-primary focus:ring-primary/40 bg-surface-container-highest"
                                />
                                <label htmlFor="remember-me" className="ml-2 block font-body text-sm text-on-surface-variant">
                                    Duy trì đăng nhập
                                </label>
                            </div>

                            <button
                                type="submit"
                                disabled={isLoading}
                                className={`w-full py-3 px-4 text-white rounded-lg font-label font-medium text-base shadow-sm focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-primary transition-all uppercase tracking-wide bg-primary ${isLoading ? 'opacity-70 cursor-not-allowed' : 'hover:opacity-90'}`}
                            >
                                {isLoading ? 'Đang xử lý...' : 'Đăng nhập'}
                            </button>
                        </form>

                        <div className="mt-8 mb-6 flex items-center justify-center">
                            <div className="w-full h-px bg-surface-dim"></div>
                            <span className="px-4 font-label text-xs text-outline bg-surface-container-lowest">OR</span>
                            <div className="w-full h-px bg-surface-dim"></div>
                        </div>

                        <div className="mt-4">
                            <button
                                type="button"
                                onClick={onGoogleSignIn}
                                className="w-full flex items-center justify-center gap-3 py-3 px-4 bg-surface-container-lowest border border-outline-variant rounded-lg font-label font-medium text-sm text-on-surface shadow-sm hover:bg-surface-container-low transition-all focus:outline-none focus:ring-2 focus:ring-primary/20"
                            >
                                <svg className="w-5 h-5" viewBox="0 0 24 24">
                                    <path d="M22.56 12.25c0-.78-.07-1.53-.2-2.25H12v4.26h5.92c-.26 1.37-1.04 2.53-2.21 3.31v2.77h3.57c2.08-1.92 3.28-4.74 3.28-8.09z" fill="#4285F4"></path>
                                    <path d="M12 23c2.97 0 5.46-.98 7.28-2.66l-3.57-2.77c-.98.66-2.23 1.06-3.71 1.06-2.86 0-5.29-1.93-6.16-4.53H2.18v2.84C3.99 20.53 7.7 23 12 23z" fill="#34A853"></path>
                                    <path d="M5.84 14.09c-.22-.66-.35-1.36-.35-2.09s.13-1.43.35-2.09V7.07H2.18C1.43 8.55 1 10.22 1 12s.43 3.45 1.18 4.93l2.85-2.22.81-.62z" fill="#FBBC05"></path>
                                    <path d="M12 5.38c1.62 0 3.06.56 4.21 1.64l3.15-3.15C17.45 2.09 14.97 1 12 1 7.7 1 3.99 3.47 2.18 7.07l3.66 2.84c.87-2.6 3.3-4.53 6.16-4.53z" fill="#EA4335"></path>
                                    <path d="M1 1h22v22H1z" fill="none"></path>
                                </svg>
                                <span>Sign in with Google</span>
                            </button>
                        </div>

                        <p className="mt-6 text-center font-body text-sm text-on-surface-variant">
                            Don't have an account?
                            <a className="ml-1 font-medium text-primary hover:text-primary-container transition-colors" href="#">
                                Request access
                            </a>
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

export default LoginUI;
