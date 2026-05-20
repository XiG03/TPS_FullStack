import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { getAvailableTopics, getAvailableTeachers, getAvailableStudents } from '../../services/courseService';

const generateId = () => {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
};

const CourseFormUI = ({ initialData, onSave, isSaving }) => {
    const navigate = useNavigate();

    // Lookups data
    const [availableTopics, setAvailableTopics] = useState([]);
    const [availableTeachers, setAvailableTeachers] = useState([]);
    const [availableStudents, setAvailableStudents] = useState([]);

    // Form State mapped to CourseCreateDto shape
    const [formData, setFormData] = useState({
        courseInfo: {
            MaID: generateId(),
            Ten: '',
            Mota: '',
            Diemdat: 5.0,
            ChungchiID: '',
            Sobuoihoc: 0,
            Thu: '',
            Thoiluonghoc: 0,
            Ngaybatdau: '',
            Thoigianthi: 0,
            Thoiluongthi: 0,
            Socauhoi: 0
        },
        courseTopics: [],
        courseTeachers: [],
        courseStudents: []
        // we ignore schedule generation logic in form for simplicity
    });

    useEffect(() => {
        // Load lookups
        getAvailableTopics().then(res => setAvailableTopics(res.data));
        getAvailableTeachers().then(res => setAvailableTeachers(res.data));
        getAvailableStudents().then(res => setAvailableStudents(res.data));

        if (initialData) {
            setFormData({
                courseInfo: {
                    ...initialData.courseInfo,
                    Ngaybatdau: initialData.courseInfo?.Ngaybatdau?.substring(0, 10) || ''
                },
                courseTopics: initialData.courseTopics || [],
                courseTeachers: initialData.courseTeachers || [],
                courseStudents: initialData.courseStudents || []
            });
        }
    }, [initialData]);

    const handleInfoChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({
            ...prev,
            courseInfo: { ...prev.courseInfo, [name]: value }
        }));
    };

    // --- Dynamic Lists Handlers ---
    const handleAddTopic = () => {
        setFormData(prev => ({
            ...prev,
            courseTopics: [...prev.courseTopics, { MaID: generateId(), ChuyendeID: '', SoCauhoi: 0 }]
        }));
    };
    const handleTopicChange = (index, field, value) => {
        const newArr = [...formData.courseTopics];
        newArr[index][field] = value;
        setFormData(prev => ({ ...prev, courseTopics: newArr }));
    };
    const handleRemoveTopic = (index) => {
        setFormData(prev => ({ ...prev, courseTopics: prev.courseTopics.filter((_, i) => i !== index) }));
    };

    const handleAddTeacher = () => {
        setFormData(prev => ({
            ...prev,
            courseTeachers: [...prev.courseTeachers, { MaID: generateId(), GiangvienID: '' }]
        }));
    };
    const handleTeacherChange = (index, value) => {
        const newArr = [...formData.courseTeachers];
        newArr[index].GiangvienID = value;
        setFormData(prev => ({ ...prev, courseTeachers: newArr }));
    };
    const handleRemoveTeacher = (index) => {
        setFormData(prev => ({ ...prev, courseTeachers: prev.courseTeachers.filter((_, i) => i !== index) }));
    };

    const handleAddStudent = () => {
        setFormData(prev => ({
            ...prev,
            courseStudents: [...prev.courseStudents, { MaID: generateId(), HocvienID: '', Diem: 0, Hieuchinh: 0 }]
        }));
    };
    const handleStudentChange = (index, value) => {
        const newArr = [...formData.courseStudents];
        newArr[index].HocvienID = value;
        setFormData(prev => ({ ...prev, courseStudents: newArr }));
    };
    const handleRemoveStudent = (index) => {
        setFormData(prev => ({ ...prev, courseStudents: prev.courseStudents.filter((_, i) => i !== index) }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        onSave(formData);
    };

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-8 lg:p-12">
            <div className="mb-8 flex items-center gap-4">
                <button 
                    type="button"
                    onClick={() => navigate('/courses')}
                    className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                >
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <h1 className="font-headline text-3xl font-extrabold text-on-surface tracking-tight">
                    {initialData ? 'Chỉnh sửa Khóa học' : 'Tạo mới Khóa học'}
                </h1>
            </div>

            <form onSubmit={handleSubmit} className="flex flex-col gap-8 max-w-5xl mx-auto w-full">
                
                {/* 1. Thông tin chung */}
                <div className="bg-surface-container-lowest rounded-2xl p-8 shadow-sm border border-surface-container">
                    <h2 className="font-headline text-xl font-bold text-on-surface mb-6 flex items-center gap-2">
                        <span className="material-symbols-outlined text-primary">info</span>
                        Thông tin cơ bản
                    </h2>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <div className="space-y-2 md:col-span-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Tên khóa học *</label>
                            <input required type="text" name="Ten" value={formData.courseInfo.Ten} onChange={handleInfoChange}
                                className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                        </div>
                        <div className="space-y-2 md:col-span-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Mô tả</label>
                            <textarea name="Mota" value={formData.courseInfo.Mota} onChange={handleInfoChange} rows="3"
                                className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                        </div>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Điểm đạt tối thiểu</label>
                            <input required type="number" step="0.5" name="Diemdat" value={formData.courseInfo.Diemdat} onChange={handleInfoChange}
                                className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                        </div>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Chứng chỉ ID liên kết</label>
                            <input type="text" name="ChungchiID" value={formData.courseInfo.ChungchiID || ''} onChange={handleInfoChange}
                                className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                        </div>
                    </div>
                </div>

                {/* 2. Cấu hình Lịch học & Bài thi */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                    <div className="bg-surface-container-lowest rounded-2xl p-8 shadow-sm border border-surface-container">
                        <h2 className="font-headline text-xl font-bold text-on-surface mb-6 flex items-center gap-2">
                            <span className="material-symbols-outlined text-secondary">calendar_month</span>
                            Lịch học
                        </h2>
                        <div className="space-y-4">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Ngày bắt đầu</label>
                                <input required type="date" name="Ngaybatdau" value={formData.courseInfo.Ngaybatdau} onChange={handleInfoChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none" />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Số buổi học</label>
                                <input required type="number" name="Sobuoihoc" value={formData.courseInfo.Sobuoihoc} onChange={handleInfoChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none" />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Học vào thứ (VD: 2,4,6)</label>
                                <input required type="text" name="Thu" value={formData.courseInfo.Thu} onChange={handleInfoChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none" />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Thời lượng mỗi buổi (Phút)</label>
                                <input required type="number" name="Thoiluonghoc" value={formData.courseInfo.Thoiluonghoc} onChange={handleInfoChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none" />
                            </div>
                        </div>
                    </div>

                    <div className="bg-surface-container-lowest rounded-2xl p-8 shadow-sm border border-surface-container">
                        <h2 className="font-headline text-xl font-bold text-on-surface mb-6 flex items-center gap-2">
                            <span className="material-symbols-outlined text-tertiary">quiz</span>
                            Bài thi cuối khóa
                        </h2>
                        <div className="space-y-4">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Thi sau (ngày) kể từ kết thúc</label>
                                <input required type="number" name="Thoigianthi" value={formData.courseInfo.Thoigianthi} onChange={handleInfoChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none" />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Thời lượng làm bài thi (Phút)</label>
                                <input required type="number" name="Thoiluongthi" value={formData.courseInfo.Thoiluongthi} onChange={handleInfoChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none" />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Tổng số câu hỏi trong đề thi</label>
                                <input required type="number" name="Socauhoi" value={formData.courseInfo.Socauhoi} onChange={handleInfoChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none" />
                            </div>
                        </div>
                    </div>
                </div>

                {/* 3. Phân bổ Chuyên đề */}
                <div className="bg-surface-container-lowest rounded-2xl p-8 shadow-sm border border-surface-container">
                    <div className="flex items-center justify-between mb-6">
                        <h2 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                            <span className="material-symbols-outlined text-primary">menu_book</span> Chuyên đề cấu thành
                        </h2>
                        <button type="button" onClick={handleAddTopic} className="bg-surface-container-highest px-3 py-1.5 rounded-lg text-sm font-bold hover:text-primary transition-colors flex items-center gap-1">
                            <span className="material-symbols-outlined text-[16px]">add</span> Thêm chuyên đề
                        </button>
                    </div>
                    {formData.courseTopics.map((ct, idx) => (
                        <div key={ct.MaID} className="flex gap-4 items-center mb-3 bg-surface-container-low p-3 rounded-lg">
                            <select required value={ct.ChuyendeID} onChange={e => handleTopicChange(idx, 'ChuyendeID', e.target.value)}
                                className="flex-1 bg-surface-container-highest border-none py-2 px-3 rounded-md text-sm outline-none cursor-pointer">
                                <option value="">-- Chọn chuyên đề --</option>
                                {availableTopics.map(t => <option key={t.maID} value={t.maID}>{t.ten}</option>)}
                            </select>
                            <div className="flex items-center gap-2 w-32">
                                <span className="text-xs font-bold text-outline">Số câu:</span>
                                <input required type="number" value={ct.SoCauhoi} onChange={e => handleTopicChange(idx, 'SoCauhoi', parseInt(e.target.value))}
                                    className="w-full bg-surface-container-highest border-none py-2 px-3 rounded-md text-sm outline-none" />
                            </div>
                            <button type="button" onClick={() => handleRemoveTopic(idx)} className="text-error p-1 hover:bg-error-container rounded">
                                <span className="material-symbols-outlined">delete</span>
                            </button>
                        </div>
                    ))}
                    {formData.courseTopics.length === 0 && <p className="text-sm italic text-outline">Chưa chọn chuyên đề nào.</p>}
                </div>

                {/* 4. Giảng viên & Học viên */}
                <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                    <div className="bg-surface-container-lowest rounded-2xl p-8 shadow-sm border border-surface-container">
                        <div className="flex items-center justify-between mb-6">
                            <h2 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                                <span className="material-symbols-outlined text-primary">groups</span> Giảng viên
                            </h2>
                            <button type="button" onClick={handleAddTeacher} className="bg-surface-container-highest px-3 py-1.5 rounded-lg text-sm font-bold hover:text-primary transition-colors">Thêm</button>
                        </div>
                        {formData.courseTeachers.map((ct, idx) => (
                            <div key={ct.MaID} className="flex gap-2 items-center mb-2">
                                <select required value={ct.GiangvienID} onChange={e => handleTeacherChange(idx, e.target.value)}
                                    className="flex-1 bg-surface-container-highest border-none py-2 px-3 rounded-md text-sm outline-none cursor-pointer">
                                    <option value="">-- Chọn giảng viên --</option>
                                    {availableTeachers.map(t => <option key={t.maID} value={t.maID}>{t.ten}</option>)}
                                </select>
                                <button type="button" onClick={() => handleRemoveTeacher(idx)} className="text-error p-1"><span className="material-symbols-outlined text-lg">close</span></button>
                            </div>
                        ))}
                    </div>

                    <div className="bg-surface-container-lowest rounded-2xl p-8 shadow-sm border border-surface-container">
                        <div className="flex items-center justify-between mb-6">
                            <h2 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                                <span className="material-symbols-outlined text-primary">school</span> Ghi danh Học viên
                            </h2>
                            <button type="button" onClick={handleAddStudent} className="bg-surface-container-highest px-3 py-1.5 rounded-lg text-sm font-bold hover:text-primary transition-colors">Thêm</button>
                        </div>
                        {formData.courseStudents.map((cs, idx) => (
                            <div key={cs.MaID} className="flex gap-2 items-center mb-2">
                                <select required value={cs.HocvienID} onChange={e => handleStudentChange(idx, e.target.value)}
                                    className="flex-1 bg-surface-container-highest border-none py-2 px-3 rounded-md text-sm outline-none cursor-pointer">
                                    <option value="">-- Chọn học viên --</option>
                                    {availableStudents.map(t => <option key={t.MaID} value={t.MaID}>{t.Hoten}</option>)}
                                </select>
                                <button type="button" onClick={() => handleRemoveStudent(idx)} className="text-error p-1"><span className="material-symbols-outlined text-lg">close</span></button>
                            </div>
                        ))}
                    </div>
                </div>

                <div className="flex justify-end gap-4 pb-12 pt-4">
                    <button type="button" onClick={() => navigate('/courses')} disabled={isSaving} className="px-6 py-3 rounded-lg font-label font-bold text-sm text-on-surface-variant hover:bg-surface-container-highest transition-colors">Hủy</button>
                    <button type="submit" disabled={isSaving} className="px-8 py-3 rounded-lg font-label font-bold text-sm text-on-primary bg-primary shadow-md hover:-translate-y-0.5 transition-all">
                        {isSaving ? 'Đang lưu...' : 'Lưu khóa học'}
                    </button>
                </div>
            </form>
        </div>
    );
};

export default CourseFormUI;
