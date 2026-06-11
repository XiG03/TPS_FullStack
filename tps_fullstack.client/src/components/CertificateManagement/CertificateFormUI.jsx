import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeft, Save } from 'lucide-react';
import { getAvailableCertificateCourses } from '../../services/certificateService';
import './CertificateManagement.css';

const buildInitialForm = (detail) => {
    const info = detail?.certificateInfo;

    return {
        maID: info?.maID || '',
        ten: info?.ten || '',
        mota: info?.mota || '',
        donvicap: info?.donvicap || '',
        thoigiansudung: info?.thoigiansudung ?? 12,
        courseIds: (detail?.certificateCourses || []).map((course) => course.khoahocID).filter(Boolean)
    };
};

const CertificateFormUI = ({ detail, isLoading, error, onSave, isSaving }) => {
    const navigate = useNavigate();
    const [formData, setFormData] = useState(() => buildInitialForm(detail));
    const [courses, setCourses] = useState([]);
    const [isLoadingCourses, setIsLoadingCourses] = useState(false);
    const [courseError, setCourseError] = useState('');

    const isEditMode = Boolean(detail?.certificateInfo?.maID);

    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
        setFormData(buildInitialForm(detail));
    }, [detail]);

    useEffect(() => {
        let isMounted = true;

        const loadCourses = async () => {
            setIsLoadingCourses(true);
            setCourseError('');

            try {
                const response = await getAvailableCertificateCourses();
                if (isMounted) setCourses(Array.isArray(response.data) ? response.data : []);
            } catch (err) {
                console.error(err);
                if (isMounted) setCourseError('Không tải được danh sách khóa học.');
            } finally {
                if (isMounted) setIsLoadingCourses(false);
            }
        };

        loadCourses();

        return () => {
            isMounted = false;
        };
    }, []);

    const selectedCourseIds = useMemo(() => new Set(formData.courseIds), [formData.courseIds]);

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleCourseToggle = (courseId) => {
        setFormData((prev) => {
            const nextCourseIds = prev.courseIds.includes(courseId)
                ? prev.courseIds.filter((id) => id !== courseId)
                : [...prev.courseIds, courseId];

            return { ...prev, courseIds: nextCourseIds };
        });
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        if (!formData.ten.trim()) {
            alert('Vui lòng nhập tên chứng chỉ.');
            return;
        }

        onSave({
            ...formData,
            thoigiansudung: Number(formData.thoigiansudung) || 0
        });
    };

    if (isLoading) {
        return (
            <div className="certificate-screen">
                <div className="certificate-shell">
                    <div className="certificate-loading">Đang tải thông tin chứng chỉ...</div>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="certificate-screen">
                <div className="certificate-shell">
                    <div className="certificate-error">{error}</div>
                </div>
            </div>
        );
    }

    return (
        <div className="certificate-screen">
            <div className="certificate-shell">
                <header className="certificate-header">
                    <div>
                        <p className="certificate-kicker">api/v1/certificate</p>
                        <h1 className="certificate-title">{isEditMode ? 'Chỉnh sửa chứng chỉ' : 'Thêm chứng chỉ'}</h1>
                        <p className="certificate-subtitle">Cấu hình thông tin cấp chứng chỉ và các khóa học liên quan.</p>
                    </div>
                    <button type="button" className="certificate-button ghost" onClick={() => navigate('/certificates')}>
                        <ArrowLeft size={18} />
                        Quay lại
                    </button>
                </header>

                <form className="certificate-form" onSubmit={handleSubmit}>
                    <section className="certificate-panel">
                        <h2 className="certificate-section-title">Thông tin chứng chỉ</h2>
                        <div className="certificate-form-grid">
                            <div className="certificate-field">
                                <label className="certificate-label" htmlFor="certificate-id">Mã chứng chỉ</label>
                                <input
                                    id="certificate-id"
                                    className="certificate-input"
                                    name="maID"
                                    value={formData.maID}
                                    onChange={handleInputChange}
                                    disabled={isEditMode}
                                    placeholder="Để trống để hệ thống tự tạo"
                                />
                            </div>
                            <div className="certificate-field">
                                <label className="certificate-label" htmlFor="certificate-duration">Thời gian sử dụng (tháng)</label>
                                <input
                                    id="certificate-duration"
                                    className="certificate-input"
                                    type="number"
                                    min="0"
                                    step="1"
                                    name="thoigiansudung"
                                    value={formData.thoigiansudung}
                                    onChange={handleInputChange}
                                    required
                                />
                            </div>
                            <div className="certificate-field full">
                                <label className="certificate-label" htmlFor="certificate-name">Tên chứng chỉ *</label>
                                <input
                                    id="certificate-name"
                                    className="certificate-input"
                                    name="ten"
                                    value={formData.ten}
                                    onChange={handleInputChange}
                                    placeholder="Ví dụ: Chứng chỉ Fresher Software Engineer"
                                    required
                                />
                            </div>
                            <div className="certificate-field full">
                                <label className="certificate-label" htmlFor="certificate-issuer">Đơn vị cấp</label>
                                <input
                                    id="certificate-issuer"
                                    className="certificate-input"
                                    name="donvicap"
                                    value={formData.donvicap}
                                    onChange={handleInputChange}
                                    placeholder="TPS Software"
                                />
                            </div>
                            <div className="certificate-field full">
                                <label className="certificate-label" htmlFor="certificate-description">Mô tả</label>
                                <textarea
                                    id="certificate-description"
                                    className="certificate-textarea"
                                    name="mota"
                                    value={formData.mota}
                                    onChange={handleInputChange}
                                    placeholder="Mô tả điều kiện, phạm vi hoặc giá trị của chứng chỉ"
                                />
                            </div>
                        </div>
                    </section>

                    <section className="certificate-panel">
                        <h2 className="certificate-section-title">Khóa học áp dụng</h2>
                        {courseError ? (
                            <div className="certificate-error">{courseError}</div>
                        ) : isLoadingCourses ? (
                            <div className="certificate-loading">Đang tải khóa học...</div>
                        ) : courses.length === 0 ? (
                            <div className="certificate-empty">Chưa có khóa học để gắn chứng chỉ.</div>
                        ) : (
                            <div className="certificate-course-grid">
                                {courses.map((course) => {
                                    const courseId = course.khoahocID || course.maID;
                                    if (!courseId) return null;

                                    return (
                                        <label className="certificate-check-item" key={courseId}>
                                            <input
                                                type="checkbox"
                                                checked={selectedCourseIds.has(courseId)}
                                                onChange={() => handleCourseToggle(courseId)}
                                            />
                                            <span>{course.ten || courseId}</span>
                                        </label>
                                    );
                                })}
                            </div>
                        )}
                    </section>

                    <div className="certificate-actions">
                        <button type="button" className="certificate-button secondary" onClick={() => navigate('/certificates')} disabled={isSaving}>
                            Hủy
                        </button>
                        <button type="submit" className="certificate-button primary" disabled={isSaving}>
                            <Save size={18} />
                            {isSaving ? 'Đang lưu...' : 'Lưu chứng chỉ'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default CertificateFormUI;
