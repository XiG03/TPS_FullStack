import { format, isSameDay } from 'date-fns';
import { vi } from 'date-fns/locale';
import {
    BookOpen,
    CalendarDays,
    CheckCircle2,
    Clock,
    LogIn,
    LogOut,
    Mail,
    MapPin,
    Phone,
    RotateCcw,
    UserRound
} from 'lucide-react';
import './TeacherWorkspaceUI.css';

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

const formatTime = (value) => {
    const date = toDate(value);
    return date ? format(date, 'HH:mm') : '--:--';
};

const getInitials = (name) => {
    if (!name) return 'GV';
    const parts = name.trim().split(/\s+/).filter(Boolean);
    if (parts.length >= 2) return `${parts[0][0]}${parts[parts.length - 1][0]}`.toUpperCase();
    return name.substring(0, 2).toUpperCase();
};

const normalizeSchedule = (schedule) => ({
    id: getValue(schedule, 'lichhocID', 'LichhocID', 'maID', 'MaID'),
    courseId: getValue(schedule, 'khoahocID', 'KhoahocID'),
    courseName: getValue(schedule, 'tenKhoahoc', 'TenKhoahoc') || 'Khóa học chưa đặt tên',
    topicName: getValue(schedule, 'tenChuyende', 'TenChuyende') || 'Chuyên đề chưa đặt tên',
    expectedDate: getValue(schedule, 'ngaydukien', 'Ngaydukien'),
    expectedStart: getValue(schedule, 'batdaudukien', 'Batdaudukien'),
    expectedEnd: getValue(schedule, 'ketthucdukien', 'Ketthucdukien', 'kethucdukien', 'Kethucdukien'),
    actualStart: getValue(schedule, 'batdauthucte', 'Batdauthucte'),
    actualEnd: getValue(schedule, 'ketthucthucte', 'Ketthucthucte', 'kethucthucte', 'Kethucthucte')
});

const getStatus = (schedule) => {
    if (schedule.actualStart && schedule.actualEnd) return 'completed';
    if (schedule.actualStart) return 'teaching';
    return 'pending';
};

const uniqueBy = (items, keySelector) => {
    const seen = new Set();
    return items.filter((item) => {
        const key = keySelector(item);
        if (!key || seen.has(key)) return false;
        seen.add(key);
        return true;
    });
};

const TeacherWorkspaceUI = ({
    teacher,
    schedules,
    isLoading,
    error,
    actionId,
    onRefresh,
    onCheckin,
    onCheckout
}) => {
    const teacherInfo = getValue(teacher, 'teacherInfo', 'TeacherInfo') || teacher || {};
    const teacherTopics = getValue(teacher, 'topics', 'Topics', 'teacherTopics', 'TeacherTopics') || [];
    const apiCourses = getValue(teacher, 'teacherCourses', 'TeacherCourses') || [];
    const normalizedSchedules = schedules.map(normalizeSchedule).sort((a, b) => {
        const left = toDate(a.expectedStart || a.expectedDate)?.getTime() || 0;
        const right = toDate(b.expectedStart || b.expectedDate)?.getTime() || 0;
        return left - right;
    });

    const courseOptions = uniqueBy(
        [
            ...apiCourses.map((course) => ({
                id: getValue(course, 'khoahocID', 'KhoahocID', 'maID', 'MaID'),
                name: getValue(course, 'tenKhoahoc', 'TenKhoahoc')
            })),
            ...normalizedSchedules.map((schedule) => ({
                id: schedule.courseId,
                name: schedule.courseName
            }))
        ],
        (course) => `${course.id || course.name}`
    );

    const todaySchedules = normalizedSchedules.filter((schedule) => {
        const scheduleDate = toDate(schedule.expectedDate || schedule.expectedStart);
        return scheduleDate && isSameDay(scheduleDate, new Date());
    });
    const completedCount = normalizedSchedules.filter((schedule) => getStatus(schedule) === 'completed').length;
    const activeCount = normalizedSchedules.filter((schedule) => getStatus(schedule) === 'teaching').length;
    const nextSchedule = normalizedSchedules.find((schedule) => getStatus(schedule) !== 'completed') || normalizedSchedules[0];

    if (isLoading && !teacher) {
        return (
            <div className="teacher-workspace teacher-workspace-center">
                <Clock className="teacher-spin" size={28} />
                <p>Đang tải không gian giảng viên...</p>
            </div>
        );
    }

    return (
        <main className="teacher-workspace">
            <section className="teacher-topbar">
                <div className="teacher-identity">
                    <div className="teacher-avatar">{getInitials(getValue(teacherInfo, 'hoten', 'Hoten'))}</div>
                    <div>
                        <p className="teacher-eyebrow">Không gian Giảng viên</p>
                        <h1>{getValue(teacherInfo, 'hoten', 'Hoten') || 'Giảng viên'}</h1>
                    </div>
                </div>
                <button className="teacher-icon-button" type="button" onClick={onRefresh} title="Tải lại dữ liệu">
                    <RotateCcw size={18} />
                    <span>Tải lại</span>
                </button>
            </section>

            {error && <div className="teacher-alert">{error}</div>}

            <section className="teacher-overview">
                <article className="teacher-profile">
                    <div className="teacher-section-title">
                        <UserRound size={19} />
                        <h2>Hồ sơ</h2>
                    </div>
                    <dl>
                        <div>
                            <dt>Mã giảng viên</dt>
                            <dd>{getValue(teacherInfo, 'maID', 'MaID') || 'Chưa có'}</dd>
                        </div>
                        <div>
                            <dt>Giới tính</dt>
                            <dd>{getValue(teacherInfo, 'gioitinh', 'Gioitinh') || 'Chưa cập nhật'}</dd>
                        </div>
                        <div>
                            <dt>Email</dt>
                            <dd><Mail size={15} />{getValue(teacherInfo, 'email', 'Email') || 'Chưa có email'}</dd>
                        </div>
                        <div>
                            <dt>Điện thoại</dt>
                            <dd><Phone size={15} />{getValue(teacherInfo, 'dienthoai', 'Dienthoai') || 'Chưa có SĐT'}</dd>
                        </div>
                        <div>
                            <dt>Địa chỉ</dt>
                            <dd><MapPin size={15} />{getValue(teacherInfo, 'diachi', 'Diachi') || 'Chưa có địa chỉ'}</dd>
                        </div>
                    </dl>
                </article>

                <div className="teacher-stats-grid">
                    <article className="teacher-stat">
                        <CalendarDays size={21} />
                        <span>{normalizedSchedules.length}</span>
                        <p>Buổi dạy</p>
                    </article>
                    <article className="teacher-stat">
                        <Clock size={21} />
                        <span>{todaySchedules.length}</span>
                        <p>Hôm nay</p>
                    </article>
                    <article className="teacher-stat">
                        <CheckCircle2 size={21} />
                        <span>{completedCount}</span>
                        <p>Đã hoàn tất</p>
                    </article>
                    <article className="teacher-stat">
                        <BookOpen size={21} />
                        <span>{courseOptions.length}</span>
                        <p>Khóa học</p>
                    </article>
                </div>
            </section>

            <section className="teacher-content-grid">
                <article className="teacher-panel teacher-next-panel">
                    <div className="teacher-section-title">
                        <CalendarDays size={19} />
                        <h2>Buổi dạy gần nhất</h2>
                    </div>

                    {nextSchedule ? (
                        <div className="teacher-next-session">
                            <div>
                                <span className={`teacher-badge ${getStatus(nextSchedule)}`}>
                                    {getStatus(nextSchedule) === 'completed' ? 'Đã hoàn tất' : getStatus(nextSchedule) === 'teaching' ? 'Đang dạy' : 'Chưa check-in'}
                                </span>
                                <h3>{nextSchedule.topicName}</h3>
                                <p>{nextSchedule.courseName}</p>
                            </div>
                            <div className="teacher-session-time">
                                <strong>{formatDate(nextSchedule.expectedDate || nextSchedule.expectedStart)}</strong>
                                <span>{formatTime(nextSchedule.expectedStart)} - {formatTime(nextSchedule.expectedEnd)}</span>
                            </div>
                            <div className="teacher-action-row">
                                <button
                                    type="button"
                                    className="teacher-primary-action"
                                    onClick={() => onCheckin(nextSchedule.id)}
                                    disabled={!nextSchedule.id || Boolean(nextSchedule.actualStart) || actionId === nextSchedule.id}
                                >
                                    <LogIn size={17} />
                                    Check-in
                                </button>
                                <button
                                    type="button"
                                    className="teacher-secondary-action"
                                    onClick={() => onCheckout(nextSchedule.id)}
                                    disabled={!nextSchedule.id || !nextSchedule.actualStart || Boolean(nextSchedule.actualEnd) || actionId === nextSchedule.id}
                                >
                                    <LogOut size={17} />
                                    Check-out
                                </button>
                            </div>
                        </div>
                    ) : (
                        <p className="teacher-empty">Chưa có lịch dạy được phân công.</p>
                    )}
                </article>

                <aside className="teacher-panel">
                    <div className="teacher-section-title">
                        <BookOpen size={19} />
                        <h2>Phụ trách</h2>
                    </div>
                    <div className="teacher-chip-group">
                        {teacherTopics.length > 0 ? (
                            teacherTopics.map((topic) => (
                                <span key={getValue(topic, 'maID', 'MaID', 'chuyendeID', 'ChuyendeID', 'khoahocID', 'KhoahocID')} className="teacher-chip">
                                    {getValue(topic, 'tenChuyende', 'TenChuyende') || 'Chuyên đề'}
                                </span>
                            ))
                        ) : (
                            <p className="teacher-empty">Chưa có chuyên đề.</p>
                        )}
                    </div>
                    <div className="teacher-course-list">
                        {courseOptions.slice(0, 6).map((course) => (
                            <div key={course.id || course.name} className="teacher-course-item">
                                <span></span>
                                <p>{course.name || 'Khóa học'}</p>
                            </div>
                        ))}
                    </div>
                </aside>
            </section>

            <section className="teacher-panel teacher-schedule-panel">
                <div className="teacher-section-title teacher-schedule-title">
                    <CalendarDays size={19} />
                    <h2>Lịch dạy của tôi</h2>
                    {activeCount > 0 && <span className="teacher-live-count">{activeCount} đang dạy</span>}
                </div>

                <div className="teacher-schedule-list">
                    {normalizedSchedules.length === 0 ? (
                        <p className="teacher-empty">Chưa có lịch dạy.</p>
                    ) : (
                        normalizedSchedules.map((schedule) => {
                            const status = getStatus(schedule);
                            return (
                                <article key={schedule.id} className="teacher-schedule-row">
                                    <div className="teacher-date-block">
                                        <strong>{formatTime(schedule.expectedStart)}</strong>
                                        <span>{formatTime(schedule.expectedEnd)}</span>
                                    </div>
                                    <div className="teacher-schedule-main">
                                        <span className={`teacher-badge ${status}`}>
                                            {status === 'completed' ? 'Đã hoàn tất' : status === 'teaching' ? 'Đang dạy' : 'Chưa check-in'}
                                        </span>
                                        <h3>{schedule.topicName}</h3>
                                        <p>{schedule.courseName}</p>
                                        <small>{formatDate(schedule.expectedDate || schedule.expectedStart)}</small>
                                    </div>
                                    <div className="teacher-row-actions">
                                        <button
                                            type="button"
                                            onClick={() => onCheckin(schedule.id)}
                                            disabled={!schedule.id || Boolean(schedule.actualStart) || actionId === schedule.id}
                                            title="Check-in"
                                        >
                                            <LogIn size={17} />
                                        </button>
                                        <button
                                            type="button"
                                            onClick={() => onCheckout(schedule.id)}
                                            disabled={!schedule.id || !schedule.actualStart || Boolean(schedule.actualEnd) || actionId === schedule.id}
                                            title="Check-out"
                                        >
                                            <LogOut size={17} />
                                        </button>
                                    </div>
                                </article>
                            );
                        })
                    )}
                </div>
            </section>
        </main>
    );
};

export default TeacherWorkspaceUI;
