import { useEffect, useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeft, Edit3, RotateCcw, Send } from 'lucide-react';
import { getAvailableCertificateStudents } from '../../services/certificateService';
import './CertificateManagement.css';

const formatDate = (value) => {
    if (!value) return 'Chưa cập nhật';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return 'Chưa cập nhật';
    return date.toLocaleDateString('vi-VN');
};

const CertificateDetailUI = ({ detail, isLoading, error, onIssueCertificate, onRevokeCertificate }) => {
    const navigate = useNavigate();
    const [students, setStudents] = useState([]);
    const [selectedStudentId, setSelectedStudentId] = useState('');
    const [isLoadingStudents, setIsLoadingStudents] = useState(false);
    const [studentError, setStudentError] = useState('');
    const [isSubmitting, setIsSubmitting] = useState(false);

    const info = detail?.certificateInfo;
    const courses = useMemo(() => detail?.certificateCourses || [], [detail]);
    const issuedStudents = useMemo(() => detail?.certificateStudents || [], [detail]);

    useEffect(() => {
        let isMounted = true;

        const loadStudents = async () => {
            setIsLoadingStudents(true);
            setStudentError('');

            try {
                const response = await getAvailableCertificateStudents();
                if (isMounted) setStudents(Array.isArray(response.data) ? response.data : []);
            } catch (err) {
                console.error(err);
                if (isMounted) setStudentError('Không tải được danh sách học viên.');
            } finally {
                if (isMounted) setIsLoadingStudents(false);
            }
        };

        loadStudents();

        return () => {
            isMounted = false;
        };
    }, []);

    const issuedStudentIds = useMemo(() => new Set(issuedStudents.map((student) => student.hocvienID).filter(Boolean)), [issuedStudents]);
    const availableStudents = useMemo(() => students.filter((student) => !issuedStudentIds.has(student.hocvienID)), [students, issuedStudentIds]);

    const handleIssue = async () => {
        if (!selectedStudentId) {
            alert('Vui lòng chọn học viên.');
            return;
        }

        setIsSubmitting(true);
        const success = await onIssueCertificate(selectedStudentId);
        setIsSubmitting(false);

        if (success) {
            setSelectedStudentId('');
            alert('Đã cấp chứng chỉ cho học viên.');
        } else {
            alert('Không cấp được chứng chỉ. Vui lòng kiểm tra API hoặc dữ liệu học viên.');
        }
    };

    const handleRevoke = async (issuedCertificateId) => {
        if (!window.confirm('Bạn có chắc chắn muốn thu hồi chứng chỉ này?')) return;

        setIsSubmitting(true);
        const success = await onRevokeCertificate(issuedCertificateId);
        setIsSubmitting(false);
        alert(success ? 'Đã thu hồi chứng chỉ.' : 'Không thu hồi được chứng chỉ.');
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

    if (error || !info) {
        return (
            <div className="certificate-screen">
                <div className="certificate-shell">
                    <div className="certificate-error">{error || 'Không tìm thấy chứng chỉ.'}</div>
                </div>
            </div>
        );
    }

    return (
        <div className="certificate-screen">
            <div className="certificate-shell">
                <header className="certificate-header">
                    <div>
                        <p className="certificate-kicker">Chi tiết chứng chỉ</p>
                        <h1 className="certificate-title">{info.ten || 'Chưa cập nhật tên'}</h1>
                        <p className="certificate-subtitle">{info.mota || 'Chưa có mô tả.'}</p>
                    </div>
                    <div className="certificate-actions">
                        <button type="button" className="certificate-button ghost" onClick={() => navigate('/certificates')}>
                            <ArrowLeft size={18} />
                            Quay lại
                        </button>
                        <button type="button" className="certificate-button primary" onClick={() => navigate(`/certificates/${info.maID}/edit`)}>
                            <Edit3 size={18} />
                            Chỉnh sửa
                        </button>
                    </div>
                </header>

                <div className="certificate-detail-grid">
                    <aside className="certificate-panel">
                        <h2 className="certificate-section-title">Thông tin</h2>
                        <div className="certificate-info-list">
                            <div className="certificate-info-item">
                                <span>Mã chứng chỉ</span>
                                <strong>{info.maID}</strong>
                            </div>
                            <div className="certificate-info-item">
                                <span>Đơn vị cấp</span>
                                <strong>{info.donvicap || 'Chưa cập nhật'}</strong>
                            </div>
                            <div className="certificate-info-item">
                                <span>Thời hạn</span>
                                <strong>{Number(info.thoigiansudung) ? `${info.thoigiansudung} tháng` : 'Chưa cấu hình'}</strong>
                            </div>
                            <div className="certificate-info-item">
                                <span>Khóa học áp dụng</span>
                                <strong>{courses.length}</strong>
                            </div>
                            <div className="certificate-info-item">
                                <span>Đã cấp</span>
                                <strong>{issuedStudents.length}</strong>
                            </div>
                        </div>
                    </aside>

                    <main className="certificate-stack">
                        <section className="certificate-panel">
                            <h2 className="certificate-section-title">Khóa học gắn chứng chỉ</h2>
                            {courses.length === 0 ? (
                                <div className="certificate-empty">Chưa có khóa học nào được gắn chứng chỉ này.</div>
                            ) : (
                                <div className="certificate-list">
                                    {courses.map((course) => (
                                        <div className="certificate-list-item" key={course.khoahocID}>
                                            <div className="certificate-name">
                                                <strong>{course.ten || course.khoahocID}</strong>
                                                <span>{course.khoahocID}</span>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            )}
                        </section>

                        <section className="certificate-panel">
                            <h2 className="certificate-section-title">Cấp chứng chỉ cho học viên</h2>
                            {studentError && <div className="certificate-error">{studentError}</div>}
                            <div className="certificate-issue-bar">
                                <select
                                    className="certificate-select"
                                    value={selectedStudentId}
                                    onChange={(event) => setSelectedStudentId(event.target.value)}
                                    disabled={isLoadingStudents || isSubmitting}
                                >
                                    <option value="">{isLoadingStudents ? 'Đang tải học viên...' : '-- Chọn học viên --'}</option>
                                    {availableStudents.map((student) => (
                                        <option key={student.hocvienID} value={student.hocvienID}>
                                            {student.hoten || student.email || student.hocvienID}
                                        </option>
                                    ))}
                                </select>
                                <button type="button" className="certificate-button primary" onClick={handleIssue} disabled={isSubmitting || !selectedStudentId}>
                                    <Send size={18} />
                                    Cấp chứng chỉ
                                </button>
                            </div>
                            <p className="certificate-muted">Học viên đã được cấp chứng chỉ hiện tại sẽ không xuất hiện trong danh sách chọn.</p>
                        </section>

                        <section className="certificate-panel">
                            <h2 className="certificate-section-title">Học viên đã được cấp</h2>
                            {issuedStudents.length === 0 ? (
                                <div className="certificate-empty">Chưa có học viên nào được cấp chứng chỉ này.</div>
                            ) : (
                                <div className="certificate-list">
                                    {issuedStudents.map((student) => (
                                        <div className="certificate-list-item" key={student.maID}>
                                            <div className="certificate-name">
                                                <strong>{student.hocvienTen || student.hocvienID || 'Học viên'}</strong>
                                                <span>Cấp: {formatDate(student.ngaycap)} - Hết hạn: {formatDate(student.ngayhethan)}</span>
                                            </div>
                                            <button
                                                type="button"
                                                className="certificate-button danger"
                                                onClick={() => handleRevoke(student.maID)}
                                                disabled={isSubmitting}
                                            >
                                                <RotateCcw size={17} />
                                                Thu hồi
                                            </button>
                                        </div>
                                    ))}
                                </div>
                            )}
                        </section>
                    </main>
                </div>
            </div>
        </div>
    );
};

export default CertificateDetailUI;
