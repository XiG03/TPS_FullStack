import { useEffect, useState } from 'react';
import { getCourses } from '../../services/courseService';
import { getTeachers } from '../../services/teacherService';
import { getTopics } from '../../services/topicService';

const emptyForm = {
    lichhocID: '',
    khoahocID: '',
    chuyendeID: '',
    giangvienID: '',
    ngaydukien: '',
    batdaudukien: '',
    ketthucdukien: '',
    ngaythucte: '',
    batdauthucte: '',
    ketthucthucte: ''
};

const getValue = (source, ...keys) => {
    for (const key of keys) {
        if (source?.[key] !== undefined && source?.[key] !== null) return source[key];
    }
    return undefined;
};

const toDateInput = (value) => {
    if (!value) return '';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '';
    return date.toISOString().slice(0, 10);
};

const toTimeInput = (value) => {
    if (!value) return '';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '';
    return date.toISOString().slice(11, 16);
};

const mapInitialData = (initialData) => {
    if (!initialData) return emptyForm;

    return {
        lichhocID: getValue(initialData, 'lichhocID', 'LichhocID', 'maID', 'MaID') || '',
        khoahocID: getValue(initialData, 'khoahocID', 'KhoahocID') || '',
        chuyendeID: getValue(initialData, 'chuyendeID', 'ChuyendeID') || '',
        giangvienID: getValue(initialData, 'giangvienID', 'GiangvienID') || '',
        ngaydukien: toDateInput(getValue(initialData, 'ngaydukien', 'Ngaydukien', 'batdaudukien', 'Batdaudukien')),
        batdaudukien: toTimeInput(getValue(initialData, 'batdaudukien', 'Batdaudukien')),
        ketthucdukien: toTimeInput(getValue(initialData, 'ketthucdukien', 'Ketthucdukien', 'kethucdukien', 'Kethucdukien')),
        ngaythucte: toDateInput(getValue(initialData, 'ngaythucte', 'Ngaythucte', 'batdauthucte', 'Batdauthucte')),
        batdauthucte: toTimeInput(getValue(initialData, 'batdauthucte', 'Batdauthucte')),
        ketthucthucte: toTimeInput(getValue(initialData, 'ketthucthucte', 'Ketthucthucte', 'kethucthucte', 'Kethucthucte'))
    };
};

const combineDateTime = (dateValue, timeValue) => {
    if (!dateValue || !timeValue) return null;
    return new Date(`${dateValue}T${timeValue}:00`).toISOString();
};

const getCourseId = (course) => getValue(course, 'khoahocID', 'KhoahocID', 'maID', 'MaID');
const getTopicId = (topic) => getValue(topic, 'chuyendeID', 'ChuyendeID', 'maID', 'MaID');
const getTeacherId = (teacher) => getValue(teacher, 'maID', 'MaID', 'giangvienID', 'GiangvienID');

const ScheduleFormModal = ({ isOpen, onClose, onSubmit, isLoading, initialData }) => {
    const [courses, setCourses] = useState([]);
    const [topics, setTopics] = useState([]);
    const [teachers, setTeachers] = useState([]);
    const [formData, setFormData] = useState(() => mapInitialData(initialData));

    useEffect(() => {
        const loadLookups = async () => {
            try {
                const [courseRes, topicRes, teacherRes] = await Promise.all([
                    getCourses(),
                    getTopics(),
                    getTeachers()
                ]);
                setCourses(Array.isArray(courseRes.data) ? courseRes.data : []);
                setTopics(Array.isArray(topicRes.data) ? topicRes.data : []);
                setTeachers(Array.isArray(teacherRes.data) ? teacherRes.data : []);
            } catch (err) {
                console.error(err);
                setCourses([]);
                setTopics([]);
                setTeachers([]);
            }
        };

        loadLookups();
    }, []);

    useEffect(() => {
        if (!isOpen) return;
        // eslint-disable-next-line react-hooks/set-state-in-effect
        setFormData(mapInitialData(initialData));
    }, [isOpen, initialData]);

    if (!isOpen) return null;

    const handleChange = (event) => {
        const { name, value } = event.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        const expectedStart = combineDateTime(formData.ngaydukien, formData.batdaudukien);
        const expectedEnd = combineDateTime(formData.ngaydukien, formData.ketthucdukien);
        const actualStart = combineDateTime(formData.ngaythucte, formData.batdauthucte);
        const actualEnd = combineDateTime(formData.ngaythucte, formData.ketthucthucte);

        const payload = {
            lichhocID: formData.lichhocID,
            khoahocID: formData.khoahocID,
            chuyendeID: formData.chuyendeID || null,
            giangvienID: formData.giangvienID || null,
            ngaydukien: formData.ngaydukien ? new Date(`${formData.ngaydukien}T00:00:00`).toISOString() : null,
            batdaudukien: expectedStart,
            ketthucdukien: expectedEnd,
            kethucdukien: expectedEnd,
            ngaythucte: formData.ngaythucte ? new Date(`${formData.ngaythucte}T00:00:00`).toISOString() : null,
            batdauthucte: actualStart,
            ketthucthucte: actualEnd,
            kethucthucte: actualEnd
        };

        onSubmit(payload);
    };

    return (
        <div className="fixed inset-0 z-[100] flex items-center justify-end bg-on-surface/25 backdrop-blur-sm">
            <div className="flex h-full w-full max-w-lg animate-slide-in-right flex-col bg-surface shadow-2xl">
                <div className="flex items-center justify-between border-b border-surface-container-highest bg-surface-container-lowest px-6 py-4">
                    <div>
                        <h2 className="font-headline text-xl font-bold text-on-surface">
                            {initialData ? 'Chỉnh sửa buổi học' : 'Thêm buổi học mới'}
                        </h2>
                        <p className="mt-1 font-body text-xs text-on-surface-variant">
                            Chọn khóa học, chuyên đề, giảng viên và thời gian dạy.
                        </p>
                    </div>
                    <button onClick={onClose} className="rounded-full p-2 text-on-surface-variant transition-colors hover:bg-error-container hover:text-error" title="Đóng">
                        <span className="material-symbols-outlined text-xl">close</span>
                    </button>
                </div>

                <form id="scheduleForm" onSubmit={handleSubmit} className="flex-1 space-y-6 overflow-y-auto p-6">
                    <section className="space-y-4">
                        <h3 className="font-headline text-base font-bold text-on-surface">Thông tin buổi học</h3>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Khóa học *</label>
                            <select
                                required
                                name="khoahocID"
                                value={formData.khoahocID}
                                onChange={handleChange}
                                className="w-full cursor-pointer rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                            >
                                <option value="">-- Chọn khóa học --</option>
                                {courses.map((course) => {
                                    const id = getCourseId(course);
                                    return (
                                        <option key={id} value={id}>
                                            {getValue(course, 'ten', 'Ten') || 'Khóa học chưa đặt tên'}
                                        </option>
                                    );
                                })}
                            </select>
                        </div>

                        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Chuyên đề</label>
                                <select
                                    name="chuyendeID"
                                    value={formData.chuyendeID}
                                    onChange={handleChange}
                                    className="w-full cursor-pointer rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                                >
                                    <option value="">-- Chọn chuyên đề --</option>
                                    {topics.map((topic) => {
                                        const id = getTopicId(topic);
                                        return (
                                            <option key={id} value={id}>
                                                {getValue(topic, 'ten', 'Ten') || 'Chuyên đề chưa đặt tên'}
                                            </option>
                                        );
                                    })}
                                </select>
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Giảng viên</label>
                                <select
                                    name="giangvienID"
                                    value={formData.giangvienID}
                                    onChange={handleChange}
                                    className="w-full cursor-pointer rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                                >
                                    <option value="">-- Chọn giảng viên --</option>
                                    {teachers.map((teacher) => {
                                        const id = getTeacherId(teacher);
                                        return (
                                            <option key={id} value={id}>
                                                {getValue(teacher, 'hoten', 'Hoten') || 'Giảng viên chưa đặt tên'}
                                            </option>
                                        );
                                    })}
                                </select>
                            </div>
                        </div>
                    </section>

                    <section className="space-y-4 border-t border-surface-container-highest pt-5">
                        <h3 className="font-headline text-base font-bold text-on-surface">Thời gian dự kiến</h3>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Ngày học *</label>
                            <input
                                required
                                type="date"
                                name="ngaydukien"
                                value={formData.ngaydukien}
                                onChange={handleChange}
                                className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                            />
                        </div>
                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Bắt đầu *</label>
                                <input
                                    required
                                    type="time"
                                    name="batdaudukien"
                                    value={formData.batdaudukien}
                                    onChange={handleChange}
                                    className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Kết thúc *</label>
                                <input
                                    required
                                    type="time"
                                    name="ketthucdukien"
                                    value={formData.ketthucdukien}
                                    onChange={handleChange}
                                    className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                                />
                            </div>
                        </div>
                    </section>

                    <section className="space-y-4 border-t border-surface-container-highest pt-5">
                        <h3 className="font-headline text-base font-bold text-on-surface">Thời gian thực tế</h3>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Ngày thực tế</label>
                            <input
                                type="date"
                                name="ngaythucte"
                                value={formData.ngaythucte}
                                onChange={handleChange}
                                className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                            />
                        </div>
                        <div className="grid grid-cols-2 gap-4">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Bắt đầu</label>
                                <input
                                    type="time"
                                    name="batdauthucte"
                                    value={formData.batdauthucte}
                                    onChange={handleChange}
                                    className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Kết thúc</label>
                                <input
                                    type="time"
                                    name="ketthucthucte"
                                    value={formData.ketthucthucte}
                                    onChange={handleChange}
                                    className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                                />
                            </div>
                        </div>
                    </section>
                </form>

                <div className="flex items-center justify-end gap-3 border-t border-surface-container-highest bg-surface-container-lowest p-6">
                    <button type="button" onClick={onClose} disabled={isLoading} className="rounded-lg px-5 py-2.5 font-label text-sm font-medium text-on-surface-variant transition-colors hover:bg-surface-container-highest disabled:opacity-60">
                        Hủy
                    </button>
                    <button type="submit" form="scheduleForm" disabled={isLoading} className="rounded-lg bg-primary px-6 py-2.5 font-label text-sm font-semibold text-on-primary shadow-md transition-all hover:-translate-y-0.5 disabled:translate-y-0 disabled:opacity-60">
                        {isLoading ? 'Đang lưu...' : 'Lưu lịch học'}
                    </button>
                </div>
            </div>
            <style jsx="true">{`
                @keyframes slideInRight {
                    from { transform: translateX(100%); }
                    to { transform: translateX(0); }
                }
                .animate-slide-in-right { animation: slideInRight 0.3s cubic-bezier(0.16, 1, 0.3, 1) forwards; }
            `}</style>
        </div>
    );
};

export default ScheduleFormModal;
