import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const TopicManagementUI = ({ topics, isLoading, error, onDeleteTopic }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    const normalizedSearch = searchTerm.trim().toLowerCase();
    const filteredTopics = topics.filter((topic) => {
        if (!normalizedSearch) return true;

        return (
            topic.ten?.toLowerCase().includes(normalizedSearch) ||
            topic.mota?.toLowerCase().includes(normalizedSearch)
        );
    });

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)]">
            <header className="flex flex-col gap-6 px-6 pt-8 pb-8 lg:flex-row lg:items-end lg:justify-between lg:px-12 lg:pt-10">
                <div className="flex flex-col gap-2">
                    <h1 className="font-headline text-3xl lg:text-4xl font-extrabold text-on-surface tracking-tight">
                        Quản lý Chuyên đề
                    </h1>
                    <p className="font-body text-on-surface-variant text-base">
                        Tạo, chỉnh sửa và quản lý chuyên đề học tập.
                    </p>
                </div>
                <button
                    onClick={() => navigate('/topics/new')}
                    className="flex items-center justify-center gap-2 bg-gradient-to-br from-primary to-primary-container text-on-primary px-6 py-3.5 rounded-lg font-body font-semibold shadow-[0px_12px_32px_rgba(25,28,30,0.06)] hover:shadow-md transition-all hover:-translate-y-0.5"
                >
                    <span className="material-symbols-outlined text-xl">add_circle</span>
                    Thêm chuyên đề
                </button>
            </header>

            <section className="px-6 pb-8 lg:px-12">
                <div className="bg-surface-container-low rounded-xl p-6 flex flex-col gap-2 max-w-md shadow-[inset_0_2px_4px_rgba(255,255,255,0.4)]">
                    <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="search">
                        Tìm kiếm
                    </label>
                    <div className="relative">
                        <span className="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-outline">
                            search
                        </span>
                        <input
                            id="search"
                            type="text"
                            placeholder="Nhập tên hoặc mô tả chuyên đề..."
                            value={searchTerm}
                            onChange={(event) => setSearchTerm(event.target.value)}
                            className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md pl-10 pr-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                        />
                    </div>
                </div>
            </section>

            <section className="px-6 pb-16 flex-1 flex flex-col lg:px-12">
                <div className="hidden md:grid grid-cols-[2fr_3fr_140px] gap-4 px-6 pb-4 font-label text-xs font-bold text-on-surface-variant uppercase tracking-wider">
                    <div>Tên chuyên đề</div>
                    <div>Mô tả</div>
                    <div className="text-center">Thao tác</div>
                </div>

                <div className="flex flex-col gap-3">
                    {isLoading ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">Đang tải dữ liệu...</div>
                    ) : error ? (
                        <div className="text-center py-10 font-body text-error">{error}</div>
                    ) : filteredTopics.length === 0 ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">
                            Không tìm thấy chuyên đề nào.
                        </div>
                    ) : (
                        filteredTopics.map((topic) => (
                            <div
                                key={topic.chuyendeID}
                                className="bg-surface-container-lowest rounded-xl p-5 grid grid-cols-1 gap-4 shadow-[0_4px_12px_rgba(25,28,30,0.02)] transition-transform hover:-translate-y-0.5 hover:shadow-[0_8px_24px_rgba(25,28,30,0.04)] md:grid-cols-[2fr_3fr_140px] md:items-center"
                            >
                                <div className="min-w-0">
                                    <div className="font-headline font-bold text-on-surface text-lg truncate">
                                        {topic.ten}
                                    </div>
                                </div>
                                <div className="font-body text-sm text-on-surface-variant line-clamp-2" title={topic.mota}>
                                    {topic.mota || 'Không có mô tả'}
                                </div>
                                <div className="flex items-center justify-end gap-1">
                                    <button
                                        onClick={() => navigate(`/topics/${topic.chuyendeID}`)}
                                        className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors"
                                        title="Xem chi tiết"
                                    >
                                        <span className="material-symbols-outlined text-xl">visibility</span>
                                    </button>
                                    <button
                                        onClick={() => navigate(`/topics/${topic.chuyendeID}/edit`)}
                                        className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors"
                                        title="Chỉnh sửa"
                                    >
                                        <span className="material-symbols-outlined text-xl">edit</span>
                                    </button>
                                    <button
                                        onClick={() => onDeleteTopic(topic.chuyendeID)}
                                        className="text-on-surface-variant hover:text-error hover:bg-error-container p-2 rounded-md transition-colors"
                                        title="Xóa"
                                    >
                                        <span className="material-symbols-outlined text-xl">delete</span>
                                    </button>
                                </div>
                            </div>
                        ))
                    )}
                </div>
            </section>
        </div>
    );
};

export default TopicManagementUI;
