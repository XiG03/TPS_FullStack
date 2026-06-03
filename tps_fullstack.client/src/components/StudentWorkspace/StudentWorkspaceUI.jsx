import { format, isAfter, isSameDay } from 'date-fns';
import { vi } from 'date-fns/locale';
import {
    Award,
    BookOpen,
    CalendarDays,
    CheckCircle2,
    Clock,
    GraduationCap,
    LogIn,
    Mail,
    MapPin,
    Phone,
    RotateCcw,
    UserRound
} from 'lucide-react';
import './StudentWorkspaceUI.css';

const getValue = (source, ...keys) => {
    for (const key of keys) {
        if (source?.[key] !== undefined && source?.[key] !== null) return source[key];
    }
    return undefined;
};

const toDate = (value) => {
    if (!value) return null;
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? null : date;
};

const formatDate = (value) => {
    const date = toDate(value);
    return date ? format(date, 'EEEE, dd/MM/yyyy', { locale: vi }) : 'Chưa có ngày';
};

const formatShortDate = (value) => {
    const date = toDate(value);
    return date ? format(date, 'dd/MM/yyyy') : '--/--/----';
};

const formatTime = (value) => {
    const date = toDate(value);
    return date ? format(date, 'HH:mm') : '--:--';
};

const getInitials = (name) => {
    if (!name) return 'HV';
    const parts = name.trim().split(/\s+/).filter(Boolean);
    if (parts.length >= 2) return `${parts[0][0]}${parts[parts.length - 1][0]}`.toUpperCase();
    return name.substring(0, 2).toUpperCase();
};

const normalizeSchedule = (schedule) => ({
    id: getValue(schedule, 'lichhocID', 'LichhocID', 'maID', 'MaID'),
    courseId: getValue(schedule, 'khoahocID', 'KhoahocID'),
    courseName: getValue(schedule, 'tenKhoahoc', 'TenKhoahoc') || 'Khóa học chưa đặt tên',
    topicName: getValue(schedule, 'tenChuyende', 'TenChuyende') || 'Chuyên đề chưa đặt tên',
    teacherName: getValue(schedule, 'tenGiangvien', 'TenGiangvien') || getValue(schedule, 'giangvienID', 'GiangvienID'),
    expectedDate: getValue(schedule, 'ngaydukien', 'Ngaydukien'),
    expectedStart: getValue(schedule, 'batdaudukien', 'Batdaudukien'),
    expectedEnd: getValue(schedule, 'ketthucdukien', 'Ketthucdukien', 'kethucdukien', 'Kethucdukien'),
    actualStart: getValue(schedule, 'batdauthucte', 'Batdauthucte'),
    actualEnd: getValue(schedule, 'ketthucthucte', 'Ketthucthucte', 'kethucthucte', 'Kethucthucte'),
    attended: normalizeAttendanceStatus(getValue(schedule, 'trangthai', 'Trangthai', 'isCheckedIn', 'IsCheckedIn'))
});

function normalizeAttendanceStatus(value) {
    if (typeof value === 'boolean') return value;
    if (typeof value === 'number') return value === 1;
    if (typeof value === 'string') {
        const normalized = value.trim().toLowerCase();
        return normalized === 'true' || normalized === '1' || normalized === 'diem danh';
    }
    return false;
}

const getScheduleStatus = (schedule) => {
    if (schedule.attended) return 'attended';
    if (schedule.actualStart) return 'active';

    const startDate = toDate(schedule.expectedStart || schedule.expectedDate);
    if (startDate && isAfter(startDate, new Date())) return 'upcoming';
    return 'pending';
};

const getStatusLabel = (status) => {
    const labels = {
        attended: '\u0110\u00e3 \u0111i\u1ec3m danh',
        completed: '\u0110\u00e3 h\u1ecdc',
        active: '\u0110ang di\u1ec5n ra',
        upcoming: 'S\u1eafp h\u1ecdc',
        pending: 'Ch\u01b0a \u0111i\u1ec3m danh'
    };

    return labels[status] || labels.pending;
};

const uniqueCourses = (schedules) => {
    const seen = new Set();
    return schedules.filter((schedule) => {
        const key = schedule.courseId || schedule.courseName;
        if (!key || seen.has(key)) return false;
        seen.add(key);
        return true;
    });
};

const StudentWorkspaceUI = ({
    student,
    schedules,
    isLoading,
    error,
    successMessage,
    actionId,
    onRefresh,
    onCheckin
}) => {
    const certificates = getValue(student, 'certificates', 'Certificates') || [];
    const normalizedSchedules = schedules.map(normalizeSchedule).sort((a, b) => {
        const left = toDate(a.expectedStart || a.expectedDate)?.getTime() || 0;
        const right = toDate(b.expectedStart || b.expectedDate)?.getTime() || 0;
        return left - right;
    });

    const courses = uniqueCourses(normalizedSchedules);
    const todaySchedules = normalizedSchedules.filter((schedule) => {
        const scheduleDate = toDate(schedule.expectedDate || schedule.expectedStart);
        return scheduleDate && isSameDay(scheduleDate, new Date());
    });
    const attendedCount = normalizedSchedules.filter((schedule) => schedule.attended).length;
    const nextSchedule = normalizedSchedules.find((schedule) => !schedule.attended) || normalizedSchedules[0];
    const nextScheduleStatus = nextSchedule ? getScheduleStatus(nextSchedule) : null;
    const canCheckinNextSchedule = Boolean(nextSchedule?.id && !nextSchedule.attended && !nextSchedule.actualEnd && actionId !== nextSchedule.id);

    if (isLoading && !student) {
        return (
            <div className="student-workspace student-workspace-center">
                <Clock className="student-spin" size={28} />
                <p>Đang tải không gian học viên...</p>
            </div>
        );
    }

    return (
        <main className="student-workspace">
            <section className="student-topbar">
                <div className="student-identity">
                    <div className="student-avatar">{getInitials(student?.hoten)}</div>
                    <div>
                        <p className="student-eyebrow">Không gian Học viên</p>
                        <h1>{student?.hoten || 'Học viên'}</h1>
                    </div>
                </div>
                <button className="student-icon-button" type="button" onClick={onRefresh} title="Tải lại dữ liệu">
                    <RotateCcw size={18} />
                    <span>Tải lại</span>
                </button>
            </section>

            {error && <div className="student-alert">{error}</div>}
            {successMessage && (
                <div className="student-alert student-alert-success" role="status">
                    <CheckCircle2 size={18} />
                    <span>{successMessage}</span>
                </div>
            )}

            <section className="student-overview">
                <article className="student-profile">
                    <div className="student-section-title">
                        <UserRound size={19} />
                        <h2>Hồ sơ</h2>
                    </div>
                    <dl>
                        <div>
                            <dt>Mã học viên</dt>
                            <dd>{student?.hocvienID || 'Chưa có'}</dd>
                        </div>
                        <div>
                            <dt>Giới tính</dt>
                            <dd>{student?.gioitinh || 'Chưa cập nhật'}</dd>
                        </div>
                        <div>
                            <dt>Email</dt>
                            <dd><Mail size={15} />{student?.email || 'Chưa có email'}</dd>
                        </div>
                        <div>
                            <dt>Điện thoại</dt>
                            <dd><Phone size={15} />{student?.dienthoai || 'Chưa có SĐT'}</dd>
                        </div>
                        <div>
                            <dt>Địa chỉ</dt>
                            <dd><MapPin size={15} />{student?.diachi || 'Chưa có địa chỉ'}</dd>
                        </div>
                    </dl>
                </article>

                <div className="student-stats-grid">
                    <article className="student-stat">
                        <BookOpen size={21} />
                        <span>{courses.length}</span>
                        <p>Khóa học</p>
                    </article>
                    <article className="student-stat">
                        <CalendarDays size={21} />
                        <span>{todaySchedules.length}</span>
                        <p>Buổi học hôm nay</p>
                    </article>
                    <article className="student-stat">
                        <CheckCircle2 size={21} />
                        <span>{attendedCount}</span>
                        <p>{'\u0110\u00e3 \u0111i\u1ec3m danh'}</p>
                    </article>
                    <article className="student-stat">
                        <Award size={21} />
                        <span>{certificates.length}</span>
                        <p>Chứng chỉ</p>
                    </article>
                </div>
            </section>

            <section className="student-content-grid">
                <article className="student-panel student-next-panel">
                    <div className="student-section-title">
                        <CalendarDays size={19} />
                        <h2>Buổi học gần nhất</h2>
                    </div>

                    {nextSchedule ? (
                        <div className="student-next-session">
                            <div>
                                <span className={`student-badge ${nextScheduleStatus}`}>
                                    {getStatusLabel(nextScheduleStatus)}
                                </span>
                                <h3>{nextSchedule.topicName}</h3>
                                <p>{nextSchedule.courseName}</p>
                            </div>
                            <div className="student-session-time">
                                <strong>{formatDate(nextSchedule.expectedDate || nextSchedule.expectedStart)}</strong>
                                <span>{formatTime(nextSchedule.expectedStart)} - {formatTime(nextSchedule.expectedEnd)}</span>
                                <small>{nextSchedule.teacherName || 'Chưa phân công giảng viên'}</small>
                            </div>
                            {!nextSchedule.actualEnd && (
                                <button
                                    type="button"
                                    className={`student-primary-action ${nextSchedule.attended ? 'attended' : ''}`}
                                    onClick={() => onCheckin(nextSchedule.id)}
                                    disabled={!canCheckinNextSchedule}
                                >
                                    {nextSchedule.attended ? <CheckCircle2 size={17} /> : <LogIn size={17} />}
                                    {nextSchedule.attended ? '\u0110\u00e3 \u0111i\u1ec3m danh' : 'Check-in bu\u1ed5i h\u1ecdc'}
                                </button>
                            )}
                        </div>
                    ) : (
                        <p className="student-empty">Chưa có lịch học được phân công.</p>
                    )}
                </article>

                <aside className="student-panel">
                    <div className="student-section-title">
                        <Award size={19} />
                        <h2>Chứng chỉ</h2>
                    </div>
                    <div className="student-certificate-list">
                        {certificates.length > 0 ? (
                            certificates.map((certificate) => (
                                <article key={certificate.maID || certificate.chungchiID} className="student-certificate-item">
                                    <div className="student-certificate-icon">
                                        <GraduationCap size={18} />
                                    </div>
                                    <div>
                                        <h3>{certificate.tenChungchi || 'Chứng chỉ'}</h3>
                                        <p>{certificate.donvicap || 'Chưa có đơn vị cấp'}</p>
                                        <span>{formatShortDate(certificate.ngaycap)} - {formatShortDate(certificate.ngayhethan)}</span>
                                    </div>
                                </article>
                            ))
                        ) : (
                            <p className="student-empty">Chưa có chứng chỉ.</p>
                        )}
                    </div>
                </aside>
            </section>

            <section className="student-panel student-course-panel">
                <div className="student-section-title">
                    <BookOpen size={19} />
                    <h2>Khóa học đang theo dõi</h2>
                </div>
                <div className="student-course-list">
                    {courses.length > 0 ? (
                        courses.map((course) => (
                            <article key={course.courseId || course.courseName} className="student-course-card">
                                <span></span>
                                <h3>{course.courseName}</h3>
                                <p>{normalizedSchedules.filter((schedule) => (schedule.courseId || schedule.courseName) === (course.courseId || course.courseName)).length} buổi học</p>
                            </article>
                        ))
                    ) : (
                        <p className="student-empty">Chưa có khóa học.</p>
                    )}
                </div>
            </section>

            <section className="student-panel student-schedule-panel">
                <div className="student-section-title">
                    <CalendarDays size={19} />
                    <h2>Lịch học của tôi</h2>
                </div>

                <div className="student-schedule-list">
                    {normalizedSchedules.length === 0 ? (
                        <p className="student-empty">Chưa có lịch học.</p>
                    ) : (
                        normalizedSchedules.map((schedule) => {
                            const status = getScheduleStatus(schedule);
                            return (
                                <article key={schedule.id} className={`student-schedule-row ${schedule.attended ? 'attended' : ''}`}>
                                    <div className="student-date-block">
                                        <strong>{formatTime(schedule.expectedStart)}</strong>
                                        <span>{formatTime(schedule.expectedEnd)}</span>
                                    </div>
                                    <div className="student-schedule-main">
                                        <span className={`student-badge ${status}`}>
                                            {getStatusLabel(status)}
                                        </span>
                                        <h3>{schedule.topicName}</h3>
                                        <p>{schedule.courseName}</p>
                                        <small>{formatDate(schedule.expectedDate || schedule.expectedStart)} · {schedule.teacherName || 'Chưa phân công'}</small>
                                    </div>
                                    {!schedule.actualEnd && (
                                        <div className="student-row-actions">
                                            <button
                                                type="button"
                                                onClick={() => onCheckin(schedule.id)}
                                                disabled={!schedule.id || schedule.attended || actionId === schedule.id}
                                                title={schedule.attended ? '\u0110\u00e3 \u0111i\u1ec3m danh' : 'Check-in'}
                                            >
                                                {schedule.attended ? <CheckCircle2 size={17} /> : <LogIn size={17} />}
                                            </button>
                                        </div>
                                    )}
                                </article>
                            );
                        })
                    )}
                </div>
            </section>
        </main>
    );
};

export default StudentWorkspaceUI;
