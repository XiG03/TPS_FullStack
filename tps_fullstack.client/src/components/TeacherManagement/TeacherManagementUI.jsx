import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const TeacherManagementUI = ({ teachers, isLoading, error, onAddTeacher, onEditTeacher, onDeleteTeacher }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const [topicFilter, setTopicFilter] = useState('');
    const navigate = useNavigate();

    const filteredTeachers = teachers.filter(t => {
        const matchName = t.Hoten?.toLowerCase().includes(searchTerm.toLowerCase()) || t.Email?.toLowerCase().includes(searchTerm.toLowerCase());
        const matchTopic = topicFilter === '' || (t.Specialization && t.Specialization.toLowerCase().includes(topicFilter.toLowerCase()));
        return matchName && matchTopic;
    });

    const getInitials = (name) => {
        if (!name) return '??';
        const parts = name.trim().split(' ');
        if (parts.length >= 2) {
            return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
        }
        return name.substring(0, 2).toUpperCase();
    };

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)]">
            {/* Page Header */}
            <header className="flex justify-between items-end px-12 pt-10 pb-10">
                <div className="flex flex-col gap-2">
                    <h1 className="font-headline text-4xl font-extrabold text-on-surface tracking-tight">Quản lý Giáo viên</h1>
                    <p className="font-body text-on-surface-variant text-base">Danh sách chuyên gia và giảng viên thuộc hệ thống.</p>
                </div>
                <button 
                    onClick={onAddTeacher}
                    className="flex items-center gap-2 bg-gradient-to-br from-primary to-primary-container text-on-primary px-6 py-3.5 rounded-lg font-body font-semibold shadow-[0px_12px_32px_rgba(25,28,30,0.06)] hover:shadow-md transition-all hover:-translate-y-0.5"
                >
                    <span className="material-symbols-outlined text-xl">person_add</span>
                    + Thêm Giáo viên
                </button>
            </header>

            {/* Filters Section */}
            <section className="px-12 pb-8">
                <div className="bg-surface-container-low rounded-xl p-6 flex items-end gap-6 shadow-[inset_0_2px_4px_rgba(255,255,255,0.4)]">
                    {/* Search Input */}
                    <div className="flex flex-col gap-2 flex-1 max-w-sm">
                        <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="search">Tìm theo tên</label>
                        <div className="relative">
                            <span className="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-outline">search</span>
                            <input
                                id="search"
                                type="text"
                                placeholder="Nhập tên giáo viên..."
                                value={searchTerm}
                                onChange={e => setSearchTerm(e.target.value)}
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md pl-10 pr-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                            />
                        </div>
                    </div>

                    {/* Dropdown Filter */}
                    <div className="flex flex-col gap-2 w-64">
                        <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="topic">Chuyên đề</label>
                        <div className="relative">
                            <select
                                id="topic"
                                value={topicFilter}
                                onChange={e => setTopicFilter(e.target.value)}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md pl-4 pr-10 py-3 font-body text-sm appearance-none focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all cursor-pointer"
                            >
                                <option value="">Tất cả chuyên đề</option>
                                <option value="data">Data Architecture</option>
                                <option value="design">UI/UX Design</option>
                                <option value="leadership">Leadership & Ethics</option>
                                <option value="engineering">Systems Engineering</option>
                            </select>
                            <span className="material-symbols-outlined absolute right-3 top-1/2 -translate-y-1/2 text-outline pointer-events-none">expand_more</span>
                        </div>
                    </div>
                </div>
            </section>

            {/* List View */}
            <section className="px-12 pb-16 flex-1 flex flex-col">
                {/* List Header */}
                <div className="grid grid-cols-[2fr_1.5fr_1fr_1fr_auto] gap-4 px-6 pb-4 font-label text-xs font-bold text-on-surface-variant uppercase tracking-wider">
                    <div>Giáo viên / Email</div>
                    <div>Chuyên đề</div>
                    <div className="text-center">Số lớp đang dạy</div>
                    <div className="text-center">Trạng thái</div>
                    <div className="w-20 text-center">Actions</div>
                </div>

                {/* List Items Container */}
                <div className="flex flex-col gap-3">
                    {isLoading ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">Đang tải dữ liệu...</div>
                    ) : error ? (
                        <div className="text-center py-10 font-body text-error">{error}</div>
                    ) : filteredTeachers.length === 0 ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">Không tìm thấy giáo viên nào.</div>
                    ) : (
                        filteredTeachers.map((teacher) => (
                            <div 
                                key={teacher.MaID} 
                                className={`bg-surface-container-lowest rounded-xl p-5 flex items-center shadow-[0_4px_12px_rgba(25,28,30,0.02)] transition-transform hover:-translate-y-0.5 hover:shadow-[0_8px_24px_rgba(25,28,30,0.04)] grid grid-cols-[2fr_1.5fr_1fr_1fr_auto] gap-4 ${teacher.Status !== 'Active' ? 'opacity-75 hover:opacity-100' : ''}`}
                            >
                                {/* Avatar & Info */}
                                <div className="flex items-center gap-4">
                                    {teacher.Avatar ? (
                                        <img src={teacher.Avatar} alt="Avatar" className="w-12 h-12 rounded-full object-cover shadow-sm bg-surface-variant" />
                                    ) : (
                                        <div className="w-12 h-12 rounded-full bg-surface-variant flex items-center justify-center text-on-surface-variant font-headline font-bold text-lg shadow-sm">
                                            {getInitials(teacher.Hoten)}
                                        </div>
                                    )}
                                    <div className="flex flex-col overflow-hidden">
                                        <span className="font-headline font-bold text-on-surface text-lg truncate">{teacher.Hoten}</span>
                                        <span className="font-body text-sm text-on-surface-variant truncate">{teacher.Email}</span>
                                    </div>
                                </div>
                                {/* Specialization */}
                                <div className="flex items-center">
                                    <span className="bg-surface-container text-on-surface-variant px-4 py-1.5 rounded-full text-sm font-medium font-label inline-flex items-center gap-2">
                                        <span className={`w-1.5 h-1.5 rounded-full ${teacher.Status === 'Active' ? 'bg-secondary' : 'bg-outline-variant'}`}></span>
                                        {teacher.Specialization || 'Chưa phân công'}
                                    </span>
                                </div>
                                {/* Classes */}
                                <div className="flex items-center justify-center">
                                    <span className={`font-headline ${teacher.Status === 'Active' ? 'font-extrabold text-on-surface' : 'font-medium text-on-surface-variant'} text-xl`}>
                                        {teacher.ClassesCount || 0}
                                    </span>
                                </div>
                                {/* Status */}
                                <div className="flex items-center justify-center">
                                    {teacher.Status === 'Active' ? (
                                        <span className="bg-primary-fixed text-on-primary-fixed-variant px-3 py-1 rounded-md text-xs font-bold uppercase tracking-wide inline-flex items-center gap-1.5">
                                            <span className="material-symbols-outlined text-[14px]">check_circle</span>
                                            Active
                                        </span>
                                    ) : (
                                        <span className="bg-surface-container-highest text-on-surface-variant px-3 py-1 rounded-md text-xs font-bold uppercase tracking-wide inline-flex items-center gap-1.5">
                                            <span className="material-symbols-outlined text-[14px]">pause_circle</span>
                                            Inactive
                                        </span>
                                    )}
                                </div>
                                {/* Actions */}
                                <div className="flex items-center justify-end gap-1 w-[100px]">
                                    <button onClick={() => navigate(`/teachers/${teacher.MaID}`)} className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors" title="View Detail">
                                        <span className="material-symbols-outlined text-xl">visibility</span>
                                    </button>
                                    <button onClick={() => onEditTeacher(teacher)} className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors" title="Edit">
                                        <span className="material-symbols-outlined text-xl">edit</span>
                                    </button>
                                    <button onClick={() => onDeleteTeacher(teacher.MaID)} className="text-on-surface-variant hover:text-error hover:bg-error-container p-2 rounded-md transition-colors" title="Delete">
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

export default TeacherManagementUI;
