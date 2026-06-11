import { useEffect, useMemo, useRef, useState } from 'react';
import { Link } from 'react-router-dom';
import {
    ArrowRight,
    Award,
    BookOpen,
    Building2,
    CheckCircle2,
    Clock3,
    GraduationCap,
    LogIn,
    ShieldCheck,
    Users
} from 'lucide-react';
import { getCertificateDetail, getCertificates } from '../services/certificateService';
import { getCourseDetail, getCourses } from '../services/courseService';
import { getToken } from '../services/httpClient';
import heroImage from '../assets/hero.png';
import './HomePage.css';

const getInitials = (name) => {
    if (!name) return 'TP';

    return name
        .split(' ')
        .filter(Boolean)
        .slice(0, 2)
        .map((word) => word[0])
        .join('')
        .toUpperCase();
};

const formatDuration = (value) => {
    const duration = Number(value);
    return duration > 0 ? `${duration} tháng` : 'Đang cập nhật';
};

const getShortText = (value, fallback) => {
    const text = String(value || '').trim();
    return text || fallback;
};

const HomePage = () => {
    const [courses, setCourses] = useState([]);
    const [certificates, setCertificates] = useState([]);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState('');
    const [activeDetail, setActiveDetail] = useState(null);
    const [isDetailLoading, setIsDetailLoading] = useState(false);
    const [detailError, setDetailError] = useState('');

    const isAuthenticated = Boolean(getToken());
    const overviewRef = useRef(null);
    const programsRef = useRef(null);
    const certificatesRef = useRef(null);

    const scrollToSection = (event, section) => {
        event?.preventDefault();

        const target = {
            overview: overviewRef,
            programs: programsRef,
            certificates: certificatesRef
        }[section]?.current;

        if (!target) return;

        const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
        target.scrollIntoView({
            behavior: prefersReducedMotion ? 'auto' : 'smooth',
            block: 'start'
        });

        window.history.replaceState(null, '', `#${section}`);
    };

    useEffect(() => {
        let isMounted = true;

        const loadHomepageData = async () => {
            setIsLoading(true);
            setError('');

            const [courseResult, certificateResult] = await Promise.allSettled([
                getCourses(),
                getCertificates()
            ]);

            if (!isMounted) return;

            const errors = [];

            if (courseResult.status === 'fulfilled') {
                setCourses(Array.isArray(courseResult.value.data) ? courseResult.value.data : []);
            } else {
                errors.push('Không tải được danh sách khóa học.');
            }

            if (certificateResult.status === 'fulfilled') {
                setCertificates(Array.isArray(certificateResult.value.data) ? certificateResult.value.data : []);
            } else {
                errors.push('Không tải được danh sách chứng chỉ.');
            }

            setError(errors.join(' '));
            setIsLoading(false);
        };

        loadHomepageData();

        return () => {
            isMounted = false;
        };
    }, []);

    const featuredCourses = useMemo(() => courses.slice(0, 3), [courses]);
    const featuredCertificates = useMemo(() => certificates.slice(0, 3), [certificates]);

    const stats = useMemo(() => {
        const issuingUnits = new Set(
            certificates
                .map((certificate) => certificate.donvicap)
                .filter(Boolean)
        );

        const configuredCertificates = certificates.filter(
            (certificate) => Number(certificate.thoigiansudung) > 0
        ).length;

        return [
            {
                label: 'Khóa học',
                value: courses.length,
                helper: 'đang vận hành',
                icon: BookOpen
            },
            {
                label: 'Chứng chỉ',
                value: certificates.length,
                helper: `${configuredCertificates} đã có thời hạn`,
                icon: Award
            },
            {
                label: 'Đơn vị cấp',
                value: issuingUnits.size,
                helper: 'trong hệ thống',
                icon: Building2
            }
        ];
    }, [certificates, courses.length]);

    const openCourseDetail = async (course) => {
        if (!course?.khoahocID) return;

        setIsDetailLoading(true);
        setDetailError('');
        setActiveDetail({
            type: 'course',
            title: getShortText(course.ten, 'Khóa học'),
            description: getShortText(course.mota, 'Đang tải thông tin chi tiết...'),
            items: []
        });

        try {
            const response = await getCourseDetail(course.khoahocID);
            const detail = response.data || {};

            setActiveDetail({
                type: 'course',
                title: getShortText(detail.ten, course.ten || 'Khóa học'),
                description: getShortText(detail.mota, course.mota || 'Chưa có mô tả khóa học.'),
                items: [
                    ['Điểm đạt', detail.diemdat || 'Đang cập nhật'],
                    ['Số buổi học', detail.sobuoihoc || 'Đang cập nhật'],
                    ['Thời lượng học', detail.thoiluonghoc || 'Đang cập nhật'],
                    ['Ngày bắt đầu', detail.ngaybatdau || detail.batdaudukien || 'Đang cập nhật'],
                    ['Số chuyên đề', detail.topics?.length || 0],
                    ['Số học viên', detail.students?.length || 0]
                ]
            });
        } catch (err) {
            console.error(err);
            setDetailError('Không tải được chi tiết khóa học.');
        } finally {
            setIsDetailLoading(false);
        }
    };

    const openCertificateDetail = async (certificate) => {
        if (!certificate?.maID) return;

        setIsDetailLoading(true);
        setDetailError('');
        setActiveDetail({
            type: 'certificate',
            title: getShortText(certificate.ten, 'Chứng chỉ'),
            description: getShortText(certificate.mota, 'Đang tải thông tin chi tiết...'),
            items: []
        });

        try {
            const response = await getCertificateDetail(certificate.maID);
            const detail = response.data || {};
            const info = detail.certificateInfo || certificate;

            setActiveDetail({
                type: 'certificate',
                title: getShortText(info.ten, certificate.ten || 'Chứng chỉ'),
                description: getShortText(info.mota, certificate.mota || 'Chưa có mô tả chứng chỉ.'),
                items: [
                    ['Đơn vị cấp', getShortText(info.donvicap, 'Đang cập nhật')],
                    ['Thời hạn', formatDuration(info.thoigiansudung)],
                    ['Khóa học liên kết', detail.certificateCourses?.length || 0],
                    ['Học viên đã cấp', detail.certificateStudents?.length || 0]
                ]
            });
        } catch (err) {
            console.error(err);
            setDetailError('Không tải được chi tiết chứng chỉ.');
        } finally {
            setIsDetailLoading(false);
        }
    };

    const closeDetail = () => {
        setActiveDetail(null);
        setDetailError('');
        setIsDetailLoading(false);
    };

    return (
        <div className="homepage-screen">
            <header className="homepage-topbar">
                <Link className="homepage-brand" to="/">
                    <span className="homepage-brand-mark">TPS</span>
                    <span>The Ledger</span>
                </Link>

                <nav className="homepage-nav" aria-label="Homepage navigation">
                    <a href="#programs" onClick={(event) => scrollToSection(event, 'programs')}>Khóa học</a>
                    <a href="#certificates" onClick={(event) => scrollToSection(event, 'certificates')}>Chứng chỉ</a>
                    <a href="#overview" onClick={(event) => scrollToSection(event, 'overview')}>Tổng quan</a>
                </nav>

                <div className="homepage-actions">
                    <Link
                        className="homepage-icon-link"
                        to={isAuthenticated ? '/courses' : '/login'}
                        aria-label={isAuthenticated ? 'Vào quản trị' : 'Đăng nhập'}
                    >
                        <LogIn size={20} />
                    </Link>
                    <Link className="homepage-primary-link" to={isAuthenticated ? '/courses' : '/login'}>
                        {isAuthenticated ? 'Quản trị' : 'Đăng nhập'}
                        <ArrowRight size={18} />
                    </Link>
                </div>
            </header>

            <main>
                <section className="homepage-hero">
                    <div className="homepage-hero-copy">
                        <span className="homepage-kicker">TPS Learning Platform</span>
                        <h1>Nền tảng khóa học và chứng chỉ chuyên nghiệp</h1>
                        <p>
                            Theo dõi chương trình đào tạo, cấu hình chứng chỉ và mở nhanh các
                            nghiệp vụ quản trị từ một màn hình tổng quan gọn gàng.
                        </p>

                        <div className="homepage-hero-actions">
                            <a
                                className="homepage-button primary"
                                href="#programs"
                                onClick={(event) => scrollToSection(event, 'programs')}
                            >
                                Xem khóa học
                                <ArrowRight size={19} />
                            </a>
                            <a
                                className="homepage-button secondary"
                                href="#certificates"
                                onClick={(event) => scrollToSection(event, 'certificates')}
                            >
                                Xem chứng chỉ
                            </a>
                        </div>
                    </div>

                    <aside className="homepage-hero-panel" aria-label="Tổng quan nhanh">
                        <div className="homepage-visual">
                            <img src={heroImage} alt="Khối nền tảng học tập TPS" />
                        </div>
                        <div className="homepage-panel-grid">
                            {stats.map((item) => {
                                const Icon = item.icon;

                                return (
                                    <div className="homepage-stat" key={item.label}>
                                        <span className="homepage-stat-icon">
                                            <Icon size={20} />
                                        </span>
                                        <strong>{isLoading ? '...' : item.value}</strong>
                                        <span>{item.label}</span>
                                        <small>{item.helper}</small>
                                    </div>
                                );
                            })}
                        </div>
                    </aside>
                </section>

                <section className="homepage-band" id="overview" ref={overviewRef}>
                    <div className="homepage-overview">
                        <article>
                            <ShieldCheck size={24} />
                            <h2>Dữ liệu từ API quản trị</h2>
                            <p>Homepage đọc trực tiếp từ `api/v1/course` và `api/v1/certificate` qua service hiện có.</p>
                        </article>
                        <article>
                            <GraduationCap size={24} />
                            <h2>Điều hướng nhanh</h2>
                            <p>Chuyển ngay sang màn quản lý khóa học, chứng chỉ hoặc các nghiệp vụ liên quan.</p>
                        </article>
                        <article>
                            <Users size={24} />
                            <h2>Sẵn sàng mở rộng</h2>
                            <p>Cấu trúc card có thể bổ sung giáo viên, học viên và thống kê chi tiết khi API mở thêm dữ liệu.</p>
                        </article>
                    </div>
                </section>

                <section className="homepage-section" id="programs" ref={programsRef}>
                    <div className="homepage-section-heading">
                        <div>
                            <span className="homepage-kicker">Programs</span>
                            <h2>Khóa học nổi bật</h2>
                        </div>
                        <a href="#programs" onClick={(event) => scrollToSection(event, 'programs')}>
                            Danh sách khóa học
                            <ArrowRight size={18} />
                        </a>
                    </div>

                    {error && <div className="homepage-alert">{error}</div>}

                    <div className="homepage-card-grid">
                        {isLoading ? (
                            Array.from({ length: 3 }).map((_, index) => (
                                <div className="homepage-card skeleton" key={index} />
                            ))
                        ) : featuredCourses.length > 0 ? (
                            featuredCourses.map((course, index) => (
                                <article className="homepage-card course-card" key={course.khoahocID || index}>
                                    <div className="homepage-card-top">
                                        <span className="homepage-avatar">{getInitials(course.ten)}</span>
                                        <span className="homepage-pill">Khóa học</span>
                                    </div>
                                    <h3>{getShortText(course.ten, 'Khóa học chưa đặt tên')}</h3>
                                    <p>{getShortText(course.mota, 'Chưa có mô tả cho khóa học này.')}</p>
                                    <button
                                        type="button"
                                        className="homepage-detail-button"
                                        onClick={() => openCourseDetail(course)}
                                    >
                                        Chi tiết
                                        <ArrowRight size={17} />
                                    </button>
                                </article>
                            ))
                        ) : (
                            <div className="homepage-empty">
                                Chưa có khóa học nào để hiển thị.
                            </div>
                        )}
                    </div>
                </section>

                <section className="homepage-section certificate-section" id="certificates" ref={certificatesRef}>
                    <div className="homepage-section-heading">
                        <div>
                            <span className="homepage-kicker">Credentials</span>
                            <h2>Chứng chỉ đang cấu hình</h2>
                        </div>
                        <a href="#certificates" onClick={(event) => scrollToSection(event, 'certificates')}>
                            Danh sách chứng chỉ
                            <ArrowRight size={18} />
                        </a>
                    </div>

                    <div className="homepage-certificate-list">
                        {isLoading ? (
                            Array.from({ length: 3 }).map((_, index) => (
                                <div className="homepage-certificate-row skeleton" key={index} />
                            ))
                        ) : featuredCertificates.length > 0 ? (
                            featuredCertificates.map((certificate, index) => (
                                <article
                                    className="homepage-certificate-row"
                                    key={certificate.maID || index}
                                >
                                    <span className="homepage-certificate-icon">
                                        <Award size={22} />
                                    </span>
                                    <div>
                                        <h3>{getShortText(certificate.ten, 'Chứng chỉ chưa đặt tên')}</h3>
                                        <p>{getShortText(certificate.mota, 'Chưa có mô tả chứng chỉ.')}</p>
                                    </div>
                                    <div className="homepage-certificate-meta">
                                        <span>
                                            <Building2 size={16} />
                                            {getShortText(certificate.donvicap, 'Chưa có đơn vị cấp')}
                                        </span>
                                        <span>
                                            <Clock3 size={16} />
                                            {formatDuration(certificate.thoigiansudung)}
                                        </span>
                                    </div>
                                    <button
                                        type="button"
                                        className="homepage-certificate-button"
                                        aria-label={`Xem chi tiết ${certificate.ten || 'chứng chỉ'}`}
                                        onClick={() => openCertificateDetail(certificate)}
                                    >
                                        <ArrowRight size={18} />
                                    </button>
                                </article>
                            ))
                        ) : (
                            <div className="homepage-empty">
                                Chưa có chứng chỉ nào để hiển thị.
                            </div>
                        )}
                    </div>
                </section>

                <section className="homepage-cta">
                    <div>
                        <CheckCircle2 size={26} />
                        <h2>Sẵn sàng quản lý chương trình đào tạo?</h2>
                        <p>Mở dashboard để thêm khóa học mới, gắn chứng chỉ và theo dõi dữ liệu học tập.</p>
                    </div>
                    <Link className="homepage-button primary" to={isAuthenticated ? '/courses/new' : '/login'}>
                        {isAuthenticated ? 'Tạo khóa học' : 'Đăng nhập quản trị'}
                        <ArrowRight size={19} />
                    </Link>
                </section>

                {activeDetail && (
                    <div className="homepage-detail-backdrop" role="presentation" onClick={closeDetail}>
                        <section
                            className="homepage-detail-panel"
                            role="dialog"
                            aria-modal="true"
                            aria-label={`Chi tiết ${activeDetail.title}`}
                            onClick={(event) => event.stopPropagation()}
                        >
                            <div className="homepage-detail-header">
                                <span className="homepage-kicker">
                                    {activeDetail.type === 'course' ? 'Course detail' : 'Certificate detail'}
                                </span>
                                <button type="button" onClick={closeDetail} aria-label="Đóng chi tiết">
                                    ×
                                </button>
                            </div>
                            <h2>{activeDetail.title}</h2>
                            <p>{activeDetail.description}</p>

                            {detailError && <div className="homepage-alert">{detailError}</div>}

                            <div className="homepage-detail-grid">
                                {isDetailLoading ? (
                                    Array.from({ length: 4 }).map((_, index) => (
                                        <div className="homepage-detail-item skeleton" key={index} />
                                    ))
                                ) : (
                                    activeDetail.items.map(([label, value]) => (
                                        <div className="homepage-detail-item" key={label}>
                                            <span>{label}</span>
                                            <strong>{value}</strong>
                                        </div>
                                    ))
                                )}
                            </div>

                            <Link
                                className="homepage-button secondary"
                                to={isAuthenticated ? (activeDetail.type === 'course' ? '/courses' : '/certificates') : '/login'}
                            >
                                {isAuthenticated ? 'Mở dashboard' : 'Đăng nhập để quản trị'}
                                <ArrowRight size={18} />
                            </Link>
                        </section>
                    </div>
                )}
            </main>
        </div>
    );
};

export default HomePage;
