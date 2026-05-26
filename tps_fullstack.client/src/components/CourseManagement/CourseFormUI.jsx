import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getAvailableTopics, getAvailableTeachers, getAvailableStudents } from '../../services/courseService';

const STUDY_DAYS = [
    { value: '2', label: 'Thứ 2' },
    { value: '3', label: 'Thứ 3' },
    { value: '4', label: 'Thứ 4' },
    { value: '5', label: 'Thứ 5' },
    { value: '6', label: 'Thứ 6' },
    { value: '7', label: 'Thứ 7' },
    { value: '8', label: 'Chủ nhật' }
];

const toDateInput = (value) => {
    if (!value) return '';
    if (typeof value === 'string' && /^\d{4}-\d{2}-\d{2}/.test(value)) return value.slice(0, 10);

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '';
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
};

const toTimeInput = (value) => {
    if (!value) return '08:00';
    if (typeof value === 'string') {
        const match = value.match(/T(\d{2}:\d{2})/);
        if (match) return match[1];
    }

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '08:00';
    return `${String(date.getHours()).padStart(2, '0')}:${String(date.getMinutes()).padStart(2, '0')}`;
};

const toApiDate = (dateValue) => (dateValue ? `${dateValue}T00:00:00` : null);

const toApiDateTime = (dateValue, timeValue) => {
    const date = dateValue || new Date().toISOString().slice(0, 10);
    const time = timeValue || '08:00';
    return `${date}T${time}:00`;
};

const toNumberOrNull = (value) => {
    if (value === '' || value === null || value === undefined) return null;
    const numberValue = Number(value);
    return Number.isNaN(numberValue) ? null : numberValue;
};

const normalizeStudyDays = (value) => {
    if (!value) return [];

    return String(value)
        .replace(/,/g, '#')
        .split('#')
        .map((item) => item.trim())
        .filter(Boolean);
};

const createEmptyForm = () => ({
    khoahocID: '',
    ten: '',
    mota: '',
    diemdat: 5,
    chungchiID: '',
    thu: [],
    thoiluonghoc: 90,
    batdaudukien: '08:00',
    sobuoihoc: 1,
    ngaybatdau: '',
    thoiluongthi: 60,
    socauhoi: 0,
    teachers: [],
    students: [],
    topics: []
});

const buildFormData = (initialData) => {
    if (!initialData) return createEmptyForm();

    return {
        khoahocID: initialData.khoahocID || '',
        ten: initialData.ten || '',
        mota: initialData.mota || '',
        diemdat: initialData.diemdat ?? 5,
        chungchiID: initialData.chungchiID || '',
        thu: normalizeStudyDays(initialData.thu),
        thoiluonghoc: initialData.thoiluonghoc ?? 90,
        batdaudukien: toTimeInput(initialData.batdaudukien),
        sobuoihoc: initialData.sobuoihoc ?? 1,
        ngaybatdau: toDateInput(initialData.ngaybatdau),
        thoiluongthi: initialData.thoiluongthi ?? 60,
        socauhoi: initialData.socauhoi ?? 0,
        teachers: (initialData.teachers || []).map((teacher) => ({
            maID: teacher.maID || '',
            giangvienID: teacher.giangvienID || ''
        })),
        students: (initialData.students || []).map((student) => ({
            maID: student.maID || '',
            hocvienID: student.hocvienID || '',
            diem: student.diem ?? '',
            dieuchinh: student.dieuchinh ?? ''
        })),
        topics: (initialData.topics || []).map((topic) => ({
            maID: topic.maID || '',
            chuyendeID: topic.chuyendeID || '',
            socauhoi: topic.socauhoi ?? 0
        }))
    };
};

const optionId = (item, primaryKey, fallbackKey = 'maID') => item?.[primaryKey] || item?.[fallbackKey] || '';

const CourseFormUI = ({ initialData, onSave, isSaving }) => {
    const navigate = useNavigate();
    const [availableTopics, setAvailableTopics] = useState([]);
    const [availableTeachers, setAvailableTeachers] = useState([]);
    const [availableStudents, setAvailableStudents] = useState([]);
    const [isLoadingLookups, setIsLoadingLookups] = useState(true);
    const [lookupError, setLookupError] = useState('');
    const [formData, setFormData] = useState(() => buildFormData(initialData));

    const isEditMode = Boolean(initialData?.khoahocID);

    useEffect(() => {
        let isMounted = true;

        const loadLookups = async () => {
            setIsLoadingLookups(true);
            setLookupError('');
            try {
                const [topics, teachers, students] = await Promise.all([
                    getAvailableTopics(),
                    getAvailableTeachers(),
                    getAvailableStudents()
                ]);

                if (!isMounted) return;
                setAvailableTopics(Array.isArray(topics.data) ? topics.data : []);
                setAvailableTeachers(Array.isArray(teachers.data) ? teachers.data : []);
                setAvailableStudents(Array.isArray(students.data) ? students.data : []);
            } catch (err) {
                if (!isMounted) return;
                console.error(err);
                setLookupError('Không tải được danh sách chuyên đề, giảng viên hoặc học viên.');
                setAvailableTopics([]);
                setAvailableTeachers([]);
                setAvailableStudents([]);
            } finally {
                if (isMounted) setIsLoadingLookups(false);
            }
        };

        loadLookups();

        return () => {
            isMounted = false;
        };
    }, []);

    const selectedTopicIds = useMemo(() => formData.topics.map((topic) => topic.chuyendeID).filter(Boolean), [formData.topics]);
    const selectedTeacherIds = useMemo(() => formData.teachers.map((teacher) => teacher.giangvienID).filter(Boolean), [formData.teachers]);
    const selectedStudentIds = useMemo(() => formData.students.map((student) => student.hocvienID).filter(Boolean), [formData.students]);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleStudyDayToggle = (dayValue) => {
        setFormData((prev) => {
            const nextDays = prev.thu.includes(dayValue)
                ? prev.thu.filter((value) => value !== dayValue)
                : [...prev.thu, dayValue];

            return {
                ...prev,
                thu: nextDays.sort((a, b) => Number(a) - Number(b))
            };
        });
    };

    const handleAddTopic = () => {
        setFormData((prev) => ({
            ...prev,
            topics: [...prev.topics, { maID: '', chuyendeID: '', socauhoi: 0 }]
        }));
    };

    const handleTopicChange = (index, field, value) => {
        setFormData((prev) => ({
            ...prev,
            topics: prev.topics.map((topic, currentIndex) => (
                currentIndex === index ? { ...topic, [field]: value } : topic
            ))
        }));
    };

    const handleRemoveTopic = (index) => {
        setFormData((prev) => ({
            ...prev,
            topics: prev.topics.filter((_, currentIndex) => currentIndex !== index)
        }));
    };

    const handleAddTeacher = () => {
        setFormData((prev) => ({
            ...prev,
            teachers: [...prev.teachers, { maID: '', giangvienID: '' }]
        }));
    };

    const handleTeacherChange = (index, value) => {
        setFormData((prev) => ({
            ...prev,
            teachers: prev.teachers.map((teacher, currentIndex) => (
                currentIndex === index ? { ...teacher, giangvienID: value } : teacher
            ))
        }));
    };

    const handleRemoveTeacher = (index) => {
        setFormData((prev) => ({
            ...prev,
            teachers: prev.teachers.filter((_, currentIndex) => currentIndex !== index)
        }));
    };

    const handleAddStudent = () => {
        setFormData((prev) => ({
            ...prev,
            students: [...prev.students, { maID: '', hocvienID: '', diem: '', dieuchinh: '' }]
        }));
    };

    const handleStudentChange = (index, field, value) => {
        setFormData((prev) => ({
            ...prev,
            students: prev.students.map((student, currentIndex) => (
                currentIndex === index ? { ...student, [field]: value } : student
            ))
        }));
    };

    const handleRemoveStudent = (index) => {
        setFormData((prev) => ({
            ...prev,
            students: prev.students.filter((_, currentIndex) => currentIndex !== index)
        }));
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        if (formData.thu.length === 0) {
            alert('Vui lòng chọn ít nhất một ngày học trong tuần.');
            return;
        }

        const courseId = formData.khoahocID.trim();
        const payload = {
            KhoahocID: courseId || null,
            Ten: formData.ten.trim(),
            Mota: formData.mota.trim() || null,
            Diemdat: toNumberOrNull(formData.diemdat),
            ChungchiID: formData.chungchiID.trim() || null,
            Thu: formData.thu.join('#'),
            Thoiluonghoc: toNumberOrNull(formData.thoiluonghoc),
            Batdaudukien: toApiDateTime(formData.ngaybatdau, formData.batdaudukien),
            Sobuoihoc: toNumberOrNull(formData.sobuoihoc),
            Ngaybatdau: toApiDate(formData.ngaybatdau),
            Thoiluongthi: toNumberOrNull(formData.thoiluongthi),
            Socauhoi: toNumberOrNull(formData.socauhoi),
            Teachers: formData.teachers
                .filter((teacher) => teacher.giangvienID)
                .map((teacher) => ({
                    MaID: teacher.maID || '',
                    KhoahocID: courseId || null,
                    GiangvienID: teacher.giangvienID
                })),
            Students: formData.students
                .filter((student) => student.hocvienID)
                .map((student) => ({
                    MaID: student.maID || '',
                    KhoahocID: courseId || null,
                    HocvienID: student.hocvienID,
                    Diem: toNumberOrNull(student.diem),
                    Dieuchinh: toNumberOrNull(student.dieuchinh)
                })),
            Topics: formData.topics
                .filter((topic) => topic.chuyendeID)
                .map((topic) => ({
                    MaID: topic.maID || '',
                    KhoahocID: courseId || null,
                    ChuyendeID: topic.chuyendeID,
                    Socauhoi: toNumberOrNull(topic.socauhoi)
                }))
        };

        onSave(payload);
    };

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-6 lg:p-12">
            <div className="mb-8 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
                <div className="flex items-center gap-4">
                    <button
                        type="button"
                        onClick={() => navigate('/courses')}
                        className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                        title="Quay lại"
                    >
                        <span className="material-symbols-outlined text-xl">arrow_back</span>
                    </button>
                    <div>
                        <h1 className="font-headline text-2xl lg:text-3xl font-extrabold text-on-surface tracking-tight m-0">
                            {isEditMode ? 'Chỉnh sửa khóa học' : 'Thêm khóa học'}
                        </h1>
                        <p className="font-body text-sm text-on-surface-variant">
                            Cấu hình theo API <span className="font-semibold">api/v1/course</span>
                        </p>
                    </div>
                </div>
            </div>

            {lookupError && (
                <div className="mb-6 max-w-5xl mx-auto w-full rounded-lg border border-error-container bg-error-container/60 px-4 py-3 font-body text-sm text-error">
                    {lookupError}
                </div>
            )}

            <form onSubmit={handleSubmit} className="flex flex-col gap-8 max-w-5xl mx-auto w-full">
                <section className="bg-surface-container-lowest rounded-lg p-6 lg:p-8 shadow-sm border border-surface-container">
                    <h2 className="font-headline text-xl font-bold text-on-surface mb-6 flex items-center gap-2">
                        <span className="material-symbols-outlined text-primary">info</span>
                        Thông tin khóa học
                    </h2>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Mã khóa học</label>
                            <input
                                type="text"
                                name="khoahocID"
                                value={formData.khoahocID}
                                onChange={handleInputChange}
                                disabled={isEditMode}
                                placeholder="Để trống để hệ thống tự tạo"
                                className="w-full bg-surface-container-highest disabled:text-outline disabled:cursor-not-allowed border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                            />
                        </div>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Mã chứng chỉ</label>
                            <input
                                type="text"
                                name="chungchiID"
                                value={formData.chungchiID}
                                onChange={handleInputChange}
                                placeholder="ChungchiID"
                                className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                            />
                        </div>
                        <div className="space-y-2 md:col-span-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Tên khóa học *</label>
                            <input
                                required
                                type="text"
                                name="ten"
                                value={formData.ten}
                                onChange={handleInputChange}
                                placeholder="Nhập tên khóa học"
                                className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                            />
                        </div>
                        <div className="space-y-2 md:col-span-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Mô tả</label>
                            <textarea
                                name="mota"
                                value={formData.mota}
                                onChange={handleInputChange}
                                rows="3"
                                placeholder="Mô tả ngắn về mục tiêu và nội dung khóa học"
                                className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all resize-y"
                            />
                        </div>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Điểm đạt tối thiểu *</label>
                            <input
                                required
                                type="number"
                                step="0.1"
                                min="0"
                                name="diemdat"
                                value={formData.diemdat}
                                onChange={handleInputChange}
                                className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                            />
                        </div>
                    </div>
                </section>

                <div className="grid grid-cols-1 lg:grid-cols-[1.2fr_0.8fr] gap-8">
                    <section className="bg-surface-container-lowest rounded-lg p-6 lg:p-8 shadow-sm border border-surface-container">
                        <h2 className="font-headline text-xl font-bold text-on-surface mb-6 flex items-center gap-2">
                            <span className="material-symbols-outlined text-primary">calendar_month</span>
                            Lịch học
                        </h2>
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Ngày bắt đầu *</label>
                                <input
                                    required
                                    type="date"
                                    name="ngaybatdau"
                                    value={formData.ngaybatdau}
                                    onChange={handleInputChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Giờ bắt đầu *</label>
                                <input
                                    required
                                    type="time"
                                    name="batdaudukien"
                                    value={formData.batdaudukien}
                                    onChange={handleInputChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Số buổi học *</label>
                                <input
                                    required
                                    type="number"
                                    min="1"
                                    name="sobuoihoc"
                                    value={formData.sobuoihoc}
                                    onChange={handleInputChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Thời lượng mỗi buổi (phút) *</label>
                                <input
                                    required
                                    type="number"
                                    min="1"
                                    name="thoiluonghoc"
                                    value={formData.thoiluonghoc}
                                    onChange={handleInputChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none"
                                />
                            </div>
                            <div className="space-y-3 md:col-span-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Học vào thứ *</label>
                                <div className="grid grid-cols-2 sm:grid-cols-4 lg:grid-cols-7 gap-2">
                                    {STUDY_DAYS.map((day) => (
                                        <label
                                            key={day.value}
                                            className={`flex items-center justify-center rounded-md border px-3 py-2 font-label text-sm font-semibold cursor-pointer transition-colors ${
                                                formData.thu.includes(day.value)
                                                    ? 'border-primary bg-primary text-on-primary'
                                                    : 'border-outline-variant bg-surface-container-highest text-on-surface-variant hover:border-primary'
                                            }`}
                                        >
                                            <input
                                                type="checkbox"
                                                checked={formData.thu.includes(day.value)}
                                                onChange={() => handleStudyDayToggle(day.value)}
                                                className="sr-only"
                                            />
                                            {day.label}
                                        </label>
                                    ))}
                                </div>
                            </div>
                        </div>
                    </section>

                    <section className="bg-surface-container-lowest rounded-lg p-6 lg:p-8 shadow-sm border border-surface-container">
                        <h2 className="font-headline text-xl font-bold text-on-surface mb-6 flex items-center gap-2">
                            <span className="material-symbols-outlined text-primary">quiz</span>
                            Bài thi cuối khóa
                        </h2>
                        <div className="space-y-5">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Thời lượng thi (phút) *</label>
                                <input
                                    required
                                    type="number"
                                    min="0"
                                    name="thoiluongthi"
                                    value={formData.thoiluongthi}
                                    onChange={handleInputChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Tổng số câu hỏi *</label>
                                <input
                                    required
                                    type="number"
                                    min="0"
                                    name="socauhoi"
                                    value={formData.socauhoi}
                                    onChange={handleInputChange}
                                    className="w-full bg-surface-container-highest border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none"
                                />
                            </div>
                        </div>
                    </section>
                </div>

                <section className="bg-surface-container-lowest rounded-lg p-6 lg:p-8 shadow-sm border border-surface-container">
                    <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between mb-6">
                        <h2 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                            <span className="material-symbols-outlined text-primary">menu_book</span>
                            Chuyên đề cấu thành
                        </h2>
                        <button
                            type="button"
                            onClick={handleAddTopic}
                            disabled={isLoadingLookups}
                            className="bg-surface-container-highest px-4 py-2 rounded-lg text-sm font-bold hover:text-primary transition-colors flex items-center justify-center gap-1 disabled:opacity-60"
                        >
                            <span className="material-symbols-outlined text-[18px]">add</span>
                            Thêm chuyên đề
                        </button>
                    </div>
                    <div className="space-y-3">
                        {formData.topics.map((topic, index) => (
                            <div key={`${topic.maID || 'new'}-${index}`} className="grid grid-cols-1 md:grid-cols-[minmax(0,1fr)_150px_auto] gap-3 items-end bg-surface-container-low p-3 rounded-lg">
                                <div className="space-y-1">
                                    <label className="font-label text-xs font-semibold text-on-surface-variant">Chuyên đề</label>
                                    <select
                                        required
                                        value={topic.chuyendeID}
                                        onChange={(event) => handleTopicChange(index, 'chuyendeID', event.target.value)}
                                        className="w-full bg-surface-container-highest border-none py-2 px-3 rounded-md text-sm outline-none cursor-pointer"
                                    >
                                        <option value="">-- Chọn chuyên đề --</option>
                                        {availableTopics.map((option) => {
                                            const value = optionId(option, 'chuyendeID');
                                            return (
                                                <option
                                                    key={value}
                                                    value={value}
                                                    disabled={selectedTopicIds.includes(value) && value !== topic.chuyendeID}
                                                >
                                                    {option.ten || value}
                                                </option>
                                            );
                                        })}
                                    </select>
                                </div>
                                <div className="space-y-1">
                                    <label className="font-label text-xs font-semibold text-on-surface-variant">Số câu</label>
                                    <input
                                        required
                                        type="number"
                                        min="0"
                                        value={topic.socauhoi}
                                        onChange={(event) => handleTopicChange(index, 'socauhoi', event.target.value)}
                                        className="w-full bg-surface-container-highest border-none py-2 px-3 rounded-md text-sm outline-none"
                                    />
                                </div>
                                <button type="button" onClick={() => handleRemoveTopic(index)} className="text-on-surface-variant hover:text-error p-2 hover:bg-error-container rounded-md" title="Xóa chuyên đề">
                                    <span className="material-symbols-outlined">delete</span>
                                </button>
                            </div>
                        ))}
                    </div>
                    {formData.topics.length === 0 && <p className="text-sm italic text-outline bg-surface-container-low rounded-lg p-4">Chưa chọn chuyên đề nào.</p>}
                </section>

                <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                    <section className="bg-surface-container-lowest rounded-lg p-6 lg:p-8 shadow-sm border border-surface-container">
                        <div className="flex items-center justify-between gap-3 mb-6">
                            <h2 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                                <span className="material-symbols-outlined text-primary">groups</span>
                                Giảng viên
                            </h2>
                            <button type="button" onClick={handleAddTeacher} disabled={isLoadingLookups} className="bg-surface-container-highest px-4 py-2 rounded-lg text-sm font-bold hover:text-primary transition-colors disabled:opacity-60">Thêm</button>
                        </div>
                        <div className="space-y-2">
                            {formData.teachers.map((teacher, index) => (
                                <div key={`${teacher.maID || 'new'}-${index}`} className="flex gap-2 items-center">
                                    <select
                                        required
                                        value={teacher.giangvienID}
                                        onChange={(event) => handleTeacherChange(index, event.target.value)}
                                        className="flex-1 bg-surface-container-highest border-none py-2 px-3 rounded-md text-sm outline-none cursor-pointer min-w-0"
                                    >
                                        <option value="">-- Chọn giảng viên --</option>
                                        {availableTeachers.map((option) => {
                                            const value = optionId(option, 'giangvienID');
                                            return (
                                                <option
                                                    key={value}
                                                    value={value}
                                                    disabled={selectedTeacherIds.includes(value) && value !== teacher.giangvienID}
                                                >
                                                    {option.hoten || option.email || value}
                                                </option>
                                            );
                                        })}
                                    </select>
                                    <button type="button" onClick={() => handleRemoveTeacher(index)} className="text-on-surface-variant hover:text-error p-2 rounded-md hover:bg-error-container" title="Xóa giảng viên">
                                        <span className="material-symbols-outlined text-lg">close</span>
                                    </button>
                                </div>
                            ))}
                            {formData.teachers.length === 0 && <p className="text-sm italic text-outline bg-surface-container-low rounded-lg p-4">Chưa phân công giảng viên.</p>}
                        </div>
                    </section>

                    <section className="bg-surface-container-lowest rounded-lg p-6 lg:p-8 shadow-sm border border-surface-container">
                        <div className="flex items-center justify-between gap-3 mb-6">
                            <h2 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                                <span className="material-symbols-outlined text-primary">school</span>
                                Ghi danh học viên
                            </h2>
                            <button type="button" onClick={handleAddStudent} disabled={isLoadingLookups} className="bg-surface-container-highest px-4 py-2 rounded-lg text-sm font-bold hover:text-primary transition-colors disabled:opacity-60">Thêm</button>
                        </div>
                        <div className="space-y-3">
                            {formData.students.map((student, index) => (
                                <div key={`${student.maID || 'new'}-${index}`} className="grid grid-cols-1 sm:grid-cols-[minmax(0,1fr)_84px_96px_auto] gap-2 items-center">
                                    <select
                                        required
                                        value={student.hocvienID}
                                        onChange={(event) => handleStudentChange(index, 'hocvienID', event.target.value)}
                                        className="bg-surface-container-highest border-none py-2 px-3 rounded-md text-sm outline-none cursor-pointer min-w-0"
                                    >
                                        <option value="">-- Chọn học viên --</option>
                                        {availableStudents.map((option) => {
                                            const value = optionId(option, 'hocvienID');
                                            return (
                                                <option
                                                    key={value}
                                                    value={value}
                                                    disabled={selectedStudentIds.includes(value) && value !== student.hocvienID}
                                                >
                                                    {option.hoten || option.email || value}
                                                </option>
                                            );
                                        })}
                                    </select>
                                    <input
                                        type="number"
                                        step="0.1"
                                        min="0"
                                        value={student.diem}
                                        onChange={(event) => handleStudentChange(index, 'diem', event.target.value)}
                                        className="bg-surface-container-highest border-none py-2 px-2 rounded-md text-sm outline-none"
                                        title="Điểm"
                                        placeholder="Điểm"
                                    />
                                    <input
                                        type="number"
                                        step="0.1"
                                        value={student.dieuchinh}
                                        onChange={(event) => handleStudentChange(index, 'dieuchinh', event.target.value)}
                                        className="bg-surface-container-highest border-none py-2 px-2 rounded-md text-sm outline-none"
                                        title="Điều chỉnh"
                                        placeholder="Điều chỉnh"
                                    />
                                    <button type="button" onClick={() => handleRemoveStudent(index)} className="text-on-surface-variant hover:text-error p-2 rounded-md hover:bg-error-container" title="Xóa học viên">
                                        <span className="material-symbols-outlined text-lg">close</span>
                                    </button>
                                </div>
                            ))}
                            {formData.students.length === 0 && <p className="text-sm italic text-outline bg-surface-container-low rounded-lg p-4">Chưa ghi danh học viên.</p>}
                        </div>
                    </section>
                </div>

                <div className="flex flex-col-reverse sm:flex-row justify-end gap-3 pb-12 pt-4">
                    <button type="button" onClick={() => navigate('/courses')} disabled={isSaving} className="px-6 py-3 rounded-lg font-label font-bold text-sm text-on-surface-variant hover:bg-surface-container-highest transition-colors disabled:opacity-60">
                        Hủy
                    </button>
                    <button type="submit" disabled={isSaving} className="px-8 py-3 rounded-lg font-label font-bold text-sm text-on-primary bg-primary shadow-md hover:-translate-y-0.5 transition-all disabled:opacity-70 disabled:cursor-not-allowed">
                        {isSaving ? 'Đang lưu...' : 'Lưu khóa học'}
                    </button>
                </div>
            </form>
        </div>
    );
};

export default CourseFormUI;
