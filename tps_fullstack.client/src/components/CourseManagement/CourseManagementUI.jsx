import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const CourseManagementUI = ({ courses, isLoading, error, onDeleteCourse }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    const filteredCourses = courses.filter(c => 
        c.ten?.toLowerCase().includes(searchTerm.toLowerCase()) || 
        c.maID?.toLowerCase().includes(searchTerm.toLowerCase())
    );

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)]">
            <header className="flex justify-between items-end px-12 pt-10 pb-10">
                <div className="flex flex-col gap-2">
                    <h1 className="font-headline text-4xl font-extrabold text-on-surface tracking-tight">Quản lý Khóa học</h1>
                    <p className="font-body text-on-surface-variant text-base">Tổng quan các khóa học đang được vận hành trên hệ thống.</p>
                </div>
                <button 
                    onClick={() => navigate('/courses/new')}
                    className="flex items-center gap-2 bg-gradient-to-br from-primary to-primary-container text-on-primary px-6 py-3.5 rounded-lg font-body font-semibold shadow-[0px_12px_32px_rgba(25,28,30,0.06)] hover:shadow-md transition-all hover:-translate-y-0.5"
                >
                    <span className="material-symbols-outlined text-xl">add_box</span>
                    Thêm Khóa học
                </button>
            </header>

            <section className="px-12 pb-8">
                <div className="bg-surface-container-low rounded-xl p-6 flex flex-col gap-2 max-w-sm shadow-[inset_0_2px_4px_rgba(255,255,255,0.4)]">
                    <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="search">Tìm kiếm</label>
                    <div className="relative">
                        <span className="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-outline">search</span>
                        <input
                            id="search"
                            type="text"
                            placeholder="Nhập mã hoặc tên khóa học..."
                            value={searchTerm}
                            onChange={e => setSearchTerm(e.target.value)}
                            className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md pl-10 pr-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                        />
                    </div>
                </div>
            </section>

            <section className="px-12 pb-16 flex-1 flex flex-col">
                <div className="grid grid-cols-[1fr_3fr_3fr_auto] gap-4 px-6 pb-4 font-label text-xs font-bold text-on-surface-variant uppercase tracking-wider">
                    <div>Mã KH</div>
                    <div>Tên khóa học</div>
                    <div>Mô tả ngắn</div>
                    <div className="w-[120px] text-center">Thao tác</div>
                </div>

                <div className="flex flex-col gap-3">
                    {isLoading ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">Đang tải dữ liệu...</div>
                    ) : error ? (
                        <div className="text-center py-10 font-body text-error">{error}</div>
                    ) : filteredCourses.length === 0 ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">Không tìm thấy khóa học nào.</div>
                    ) : (
                        filteredCourses.map((course) => (
                            <div key={course.maID} className="bg-surface-container-lowest rounded-xl p-5 flex items-center shadow-[0_4px_12px_rgba(25,28,30,0.02)] transition-transform hover:-translate-y-0.5 hover:shadow-[0_8px_24px_rgba(25,28,30,0.04)] grid grid-cols-[1fr_3fr_3fr_auto] gap-4">
                                <div className="font-label font-bold text-sm text-outline bg-surface-container-highest px-2 py-1 rounded w-max">
                                    {course.maID}
                                </div>
                                <div className="font-headline font-bold text-on-surface text-lg truncate">
                                    {course.ten}
                                </div>
                                <div className="font-body text-sm text-on-surface-variant truncate" title={course.mota}>
                                    {course.mota || 'Không có mô tả'}
                                </div>
                                <div className="flex items-center justify-end gap-1 w-[120px]">
                                    <button onClick={() => navigate(`/courses/${course.maID}`)} className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors" title="View Detail">
                                        <span className="material-symbols-outlined text-xl">visibility</span>
                                    </button>
                                    <button onClick={() => navigate(`/courses/${course.maID}/edit`)} className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors" title="Edit">
                                        <span className="material-symbols-outlined text-xl">edit</span>
                                    </button>
                                    <button onClick={() => onDeleteCourse(course.maID)} className="text-on-surface-variant hover:text-error hover:bg-error-container p-2 rounded-md transition-colors" title="Delete">
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

export default CourseManagementUI;
