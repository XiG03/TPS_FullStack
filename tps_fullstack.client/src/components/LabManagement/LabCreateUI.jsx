import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { getCourses, getAvailableTeachers, getAvailableTopics } from '../../services/courseService';

const emptyForm = {
    khoahocID: '',
    chuyendeID: '',
    giangvienID: '',
    diachi: '',
    soluongtoida: 20,
    ngaydukien: '',
    batdaudukien: '08:00',
    ketthucdukien: '10:00'
};

const getValue = (source, ...keys) => {
    for (const key of keys) {
        if (source?.[key] !== undefined && source?.[key] !== null) return source[key];
    }
    return undefined;
};

const toApiDate = (dateValue) => (dateValue ? `${dateValue}T00:00:00` : null);
const toApiDateTime = (dateValue, timeValue) => (dateValue && timeValue ? `${dateValue}T${timeValue}:00` : null);

const toNumberOrNull = (value) => {
    if (value === '' || value === null || value === undefined) return null;
    const numberValue = Number(value);
    return Number.isNaN(numberValue) ? null : numberValue;
};

const formatPreviewDate = (dateValue) => {
    if (!dateValue) return 'Chưa chọn ngày';
    const date = new Date(`${dateValue}T00:00:00`);
    if (Number.isNaN(date.getTime())) return 'Chưa chọn ngày';
    return date.toLocaleDateString('vi-VN', {
        weekday: 'long',
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
};

const getCourseId = (course) => getValue(course, 'khoahocID', 'KhoahocID', 'maID', 'MaID');
const getTopicId = (topic) => getValue(topic, 'chuyendeID', 'ChuyendeID', 'maID', 'MaID');
const getTeacherId = (teacher) => getValue(teacher, 'giangvienID', 'GiangvienID', 'maID', 'MaID');

const LabCreateUI = ({ onCreateLab, isSaving }) => {
    const navigate = useNavigate();
    const [courses, setCourses] = useState([]);
    const [topics, setTopics] = useState([]);
    const [teachers, setTeachers] = useState([]);
    const [isLoadingLookups, setIsLoadingLookups] = useState(true);
    const [lookupError, setLookupError] = useState('');
    const [formData, setFormData] = useState(emptyForm);

    useEffect(() => {
        let isMounted = true;

        const loadLookups = async () => {
            setIsLoadingLookups(true);
            setLookupError('');
            try {
                const [courseRes, topicRes, teacherRes] = await Promise.all([
                    getCourses(),
                    getAvailableTopics(),
                    getAvailableTeachers()
                ]);

                if (!isMounted) return;
                setCourses(Array.isArray(courseRes.data) ? courseRes.data : []);
                setTopics(Array.isArray(topicRes.data) ? topicRes.data : []);
                setTeachers(Array.isArray(teacherRes.data) ? teacherRes.data : []);
            } catch (err) {
                if (!isMounted) return;
                console.error(err);
                setLookupError('Không tải được danh sách khóa học, chuyên đề hoặc giảng viên.');
                setCourses([]);
                setTopics([]);
                setTeachers([]);
            } finally {
                if (isMounted) setIsLoadingLookups(false);
            }
        };

        loadLookups();

        return () => {
            isMounted = false;
        };
    }, []);

    const selectedCourse = useMemo(
        () => courses.find((course) => getCourseId(course) === formData.khoahocID),
        [courses, formData.khoahocID]
    );

    const selectedTopic = useMemo(
        () => topics.find((topic) => getTopicId(topic) === formData.chuyendeID),
        [topics, formData.chuyendeID]
    );

    const selectedTeacher = useMemo(
        () => teachers.find((teacher) => getTeacherId(teacher) === formData.giangvienID),
        [teachers, formData.giangvienID]
    );

    const handleChange = (event) => {
        const { name, value } = event.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        const start = toApiDateTime(formData.ngaydukien, formData.batdaudukien);
        const end = toApiDateTime(formData.ngaydukien, formData.ketthucdukien);

        if (start && end && new Date(end) <= new Date(start)) {
            alert('Giờ kết thúc phải sau giờ bắt đầu.');
            return;
        }

        const payload = {
            schedule: {
                LichhocID: null,
                KhoahocID: formData.khoahocID,
                Ngaydukien: toApiDate(formData.ngaydukien),
                Batdaudukien: start,
                Ketthucdukien: end
            },
            lab: {
                ThuchanhID: null,
                KhoahocID: formData.khoahocID,
                GiangvienID: formData.giangvienID || null,
                ChuyendeID: formData.chuyendeID || null,
                Diachi: formData.diachi.trim() || null,
                Soluongtoida: toNumberOrNull(formData.soluongtoida)
            }
        };

        onCreateLab(payload);
    };

    return (
        <div className="flex min-h-[calc(100vh-60px)] flex-1 flex-col overflow-y-auto bg-surface p-6 lg:p-12">
            <div className="mb-8 flex flex-col gap-4 lg:flex-row lg:items-end lg:justify-between">
                <div className="min-w-0">
                    <div className="mb-3 flex items-center gap-3">
                        <button
                            type="button"
                            onClick={() => navigate('/schedules')}
                            className="flex h-10 w-10 items-center justify-center rounded-full bg-surface-container-lowest text-on-surface shadow-sm transition-colors hover:bg-surface-container-highest"
                            title="Quay lại lịch học"
                        >
                            <span className="material-symbols-outlined text-xl">arrow_back</span>
                        </button>
                        <span className="rounded-full bg-surface-container-high px-3 py-1 font-label text-xs font-bold uppercase text-on-surface-variant">
                            api/v1/lab
                        </span>
                    </div>
                    <h1 className="m-0 font-headline text-2xl font-extrabold tracking-tight text-on-surface lg:text-3xl">
                        Tạo buổi thực hành
                    </h1>
                    <p className="mt-2 max-w-2xl font-body text-sm text-on-surface-variant">
                        Tạo lịch học mới và thông tin phòng/lớp thực hành trong cùng một lần gửi.
                    </p>
                </div>
            </div>

            {lookupError && (
                <div className="mb-6 rounded-lg border border-error-container bg-error-container/70 px-4 py-3 font-body text-sm text-error">
                    {lookupError}
                </div>
            )}

            <form onSubmit={handleSubmit} className="grid w-full max-w-6xl grid-cols-1 gap-8 xl:grid-cols-[minmax(0,1fr)_360px]">
                <div className="space-y-8">
                    <section className="rounded-lg border border-surface-container bg-surface-container-lowest p-6 shadow-sm lg:p-8">
                        <h2 className="mb-6 flex items-center gap-2 font-headline text-xl font-bold text-on-surface">
                            <span className="material-symbols-outlined text-primary">science</span>
                            Thông tin thực hành
                        </h2>

                        <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
                            <div className="space-y-2 md:col-span-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Khóa học *</label>
                                <select
                                    required
                                    name="khoahocID"
                                    value={formData.khoahocID}
                                    onChange={handleChange}
                                    disabled={isLoadingLookups}
                                    className="w-full cursor-pointer rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30 disabled:cursor-not-allowed disabled:opacity-60"
                                >
                                    <option value="">-- Chọn khóa học --</option>
                                    {courses.map((course) => {
                                        const id = getCourseId(course);
                                        return (
                                            <option key={id} value={id}>
                                                {getValue(course, 'ten', 'Ten') || id}
                                            </option>
                                        );
                                    })}
                                </select>
                            </div>

                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Chuyên đề</label>
                                <select
                                    name="chuyendeID"
                                    value={formData.chuyendeID}
                                    onChange={handleChange}
                                    disabled={isLoadingLookups}
                                    className="w-full cursor-pointer rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30 disabled:cursor-not-allowed disabled:opacity-60"
                                >
                                    <option value="">-- Chọn chuyên đề --</option>
                                    {topics.map((topic) => {
                                        const id = getTopicId(topic);
                                        return (
                                            <option key={id} value={id}>
                                                {getValue(topic, 'ten', 'Ten') || id}
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
                                    disabled={isLoadingLookups}
                                    className="w-full cursor-pointer rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30 disabled:cursor-not-allowed disabled:opacity-60"
                                >
                                    <option value="">-- Chọn giảng viên --</option>
                                    {teachers.map((teacher) => {
                                        const id = getTeacherId(teacher);
                                        return (
                                            <option key={id} value={id}>
                                                {getValue(teacher, 'hoten', 'Hoten') || getValue(teacher, 'email', 'Email') || id}
                                            </option>
                                        );
                                    })}
                                </select>
                            </div>

                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Địa chỉ / phòng thực hành *</label>
                                <input
                                    required
                                    type="text"
                                    name="diachi"
                                    value={formData.diachi}
                                    onChange={handleChange}
                                    placeholder="VD: Lab A301, cơ sở 1"
                                    className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all placeholder:text-outline focus:ring-2 focus:ring-primary/30"
                                />
                            </div>

                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Số lượng tối đa *</label>
                                <input
                                    required
                                    type="number"
                                    min="1"
                                    name="soluongtoida"
                                    value={formData.soluongtoida}
                                    onChange={handleChange}
                                    className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                                />
                            </div>
                        </div>
                    </section>

                    <section className="rounded-lg border border-surface-container bg-surface-container-lowest p-6 shadow-sm lg:p-8">
                        <h2 className="mb-6 flex items-center gap-2 font-headline text-xl font-bold text-on-surface">
                            <span className="material-symbols-outlined text-primary">event</span>
                            Lịch thực hành
                        </h2>

                        <div className="grid grid-cols-1 gap-6 md:grid-cols-3">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Ngày thực hành *</label>
                                <input
                                    required
                                    type="date"
                                    name="ngaydukien"
                                    value={formData.ngaydukien}
                                    onChange={handleChange}
                                    className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:ring-2 focus:ring-primary/30"
                                />
                            </div>

                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Giờ bắt đầu *</label>
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
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Giờ kết thúc *</label>
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
                </div>

                <aside className="h-fit rounded-lg border border-outline-variant/30 bg-surface-container-lowest p-6 shadow-sm">
                    <h2 className="mb-5 font-headline text-lg font-bold text-on-surface">Tóm tắt</h2>
                    <dl className="space-y-4 font-body text-sm">
                        <div>
                            <dt className="text-on-surface-variant">Khóa học</dt>
                            <dd className="mt-1 font-semibold text-on-surface">
                                {getValue(selectedCourse, 'ten', 'Ten') || 'Chưa chọn'}
                            </dd>
                        </div>
                        <div>
                            <dt className="text-on-surface-variant">Chuyên đề</dt>
                            <dd className="mt-1 font-semibold text-on-surface">
                                {getValue(selectedTopic, 'ten', 'Ten') || 'Chưa chọn'}
                            </dd>
                        </div>
                        <div>
                            <dt className="text-on-surface-variant">Giảng viên</dt>
                            <dd className="mt-1 font-semibold text-on-surface">
                                {getValue(selectedTeacher, 'hoten', 'Hoten') || getValue(selectedTeacher, 'email', 'Email') || 'Chưa chọn'}
                            </dd>
                        </div>
                        <div>
                            <dt className="text-on-surface-variant">Thời gian</dt>
                            <dd className="mt-1 font-semibold text-on-surface">
                                {formData.batdaudukien} - {formData.ketthucdukien}
                            </dd>
                            <dd className="mt-1 text-on-surface-variant">{formatPreviewDate(formData.ngaydukien)}</dd>
                        </div>
                        <div>
                            <dt className="text-on-surface-variant">Địa điểm</dt>
                            <dd className="mt-1 font-semibold text-on-surface">{formData.diachi || 'Chưa nhập'}</dd>
                        </div>
                    </dl>

                    <div className="mt-8 flex flex-col gap-3">
                        <button
                            type="submit"
                            disabled={isSaving || isLoadingLookups}
                            className="rounded-lg bg-primary px-6 py-3 font-label text-sm font-bold text-on-primary shadow-md transition-all hover:-translate-y-0.5 disabled:translate-y-0 disabled:cursor-not-allowed disabled:opacity-60"
                        >
                            {isSaving ? 'Đang tạo...' : 'Tạo buổi thực hành'}
                        </button>
                        <button
                            type="button"
                            onClick={() => navigate('/schedules')}
                            disabled={isSaving}
                            className="rounded-lg px-6 py-3 font-label text-sm font-bold text-on-surface-variant transition-colors hover:bg-surface-container-highest disabled:opacity-60"
                        >
                            Hủy
                        </button>
                    </div>
                </aside>
            </form>
        </div>
    );
};

export default LabCreateUI;
