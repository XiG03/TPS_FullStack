import { useMemo, useState } from 'react';
import {
    addDays,
    addMonths,
    endOfMonth,
    endOfWeek,
    format,
    isSameDay,
    isSameMonth,
    isToday,
    startOfMonth,
    startOfWeek,
    subMonths
} from 'date-fns';
import { vi } from 'date-fns/locale';
import {
    BookOpen,
    CalendarDays,
    CheckCircle2,
    ChevronLeft,
    ChevronRight,
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

const getStatusLabel = (status) => {
    const labels = {
        completed: 'Đã hoàn tất',
        teaching: 'Đang dạy',
        pending: 'Chưa check-in'
    };

    return labels[status] || labels.pending;
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

const getScheduleDate = (schedule) => toDate(schedule.expectedStart || schedule.expectedDate);

const WEEKDAYS = ['T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'CN'];

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
    const [activeTab, setActiveTab] = useState('profile');
    const [calendarMonth, setCalendarMonth] = useState(() => startOfMonth(new Date()));
    const [selectedDate, setSelectedDate] = useState(() => new Date());

    const teacherInfo = getValue(teacher, 'teacherInfo', 'TeacherInfo') || teacher || {};
    const teacherTopics = getValue(teacher, 'topics', 'Topics', 'teacherTopics', 'TeacherTopics') || [];
    const apiCourses = useMemo(() => getValue(teacher, 'teacherCourses', 'TeacherCourses') || [], [teacher]);
    const normalizedSchedules = useMemo(() => schedules.map(normalizeSchedule).sort((a, b) => {
        const left = getScheduleDate(a)?.getTime() || 0;
        const right = getScheduleDate(b)?.getTime() || 0;
        return left - right;
    }), [schedules]);

    const courseOptions = useMemo(() => uniqueBy(
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
    ), [apiCourses, normalizedSchedules]);

    const todaySchedules = normalizedSchedules.filter((schedule) => {
        const scheduleDate = getScheduleDate(schedule);
        return scheduleDate && isSameDay(scheduleDate, new Date());
    });
    const completedCount = normalizedSchedules.filter((schedule) => getStatus(schedule) === 'completed').length;
    const activeCount = normalizedSchedules.filter((schedule) => getStatus(schedule) === 'teaching').length;
    const nextSchedule = normalizedSchedules.find((schedule) => getStatus(schedule) !== 'completed') || normalizedSchedules[0];

    const calendarDays = useMemo(() => {
        const monthStart = startOfMonth(calendarMonth);
        const monthEnd = endOfMonth(calendarMonth);
        const calendarStart = startOfWeek(monthStart, { weekStartsOn: 1 });
        const calendarEnd = endOfWeek(monthEnd, { weekStartsOn: 1 });
        const days = [];

        for (let cursor = calendarStart; cursor <= calendarEnd; cursor = addDays(cursor, 1)) {
            days.push(cursor);
        }

        return days;
    }, [calendarMonth]);

    const selectedDateSchedules = normalizedSchedules.filter((schedule) => {
        const scheduleDate = getScheduleDate(schedule);
        return scheduleDate && isSameDay(scheduleDate, selectedDate);
    });

    const getSchedulesForDate = (date) => normalizedSchedules.filter((schedule) => {
        const scheduleDate = getScheduleDate(schedule);
        return scheduleDate && isSameDay(scheduleDate, date);
    });

    const handleCalendarMonthChange = (direction) => {
        const nextMonth = direction > 0 ? addMonths(calendarMonth, 1) : subMonths(calendarMonth, 1);
        setCalendarMonth(nextMonth);
        setSelectedDate(startOfMonth(nextMonth));
    };

    const canCheckinSchedule = (schedule) => Boolean(
        schedule?.id &&
        !schedule.actualStart &&
        actionId !== schedule.id
    );

    const canCheckoutSchedule = (schedule) => Boolean(
        schedule?.id &&
        schedule.actualStart &&
        !schedule.actualEnd &&
        actionId !== schedule.id
    );

    const tabs = [
        { id: 'profile', label: 'Thông tin giảng viên', icon: UserRound },
        { id: 'courses', label: 'Khóa học phụ trách', icon: BookOpen },
        { id: 'schedule', label: 'Lịch dạy của tôi', icon: CalendarDays }
    ];

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

            <div className="teacher-workspace-layout">
                <nav className="teacher-workspace-nav" aria-label="Điều hướng không gian giảng viên">
                    {tabs.map((tab) => {
                        const Icon = tab.icon;
                        return (
                            <button
                                key={tab.id}
                                type="button"
                                className={activeTab === tab.id ? 'active' : ''}
                                onClick={() => setActiveTab(tab.id)}
                            >
                                <Icon size={18} />
                                <span>{tab.label}</span>
                            </button>
                        );
                    })}
                </nav>

                <div className="teacher-workspace-main">
                    {activeTab === 'profile' && (
                        <section className="teacher-tab-panel">
                            <section className="teacher-overview">
                                <article className="teacher-profile">
                                    <div className="teacher-section-title">
                                        <UserRound size={19} />
                                        <h2>Thông tin chi tiết giảng viên</h2>
                                    </div>
                                    <dl>
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

                            <article className="teacher-panel teacher-next-panel">
                                <div className="teacher-section-title">
                                    <CalendarDays size={19} />
                                    <h2>Buổi dạy gần nhất</h2>
                                </div>

                                {nextSchedule ? (
                                    <div className="teacher-next-session">
                                        <div>
                                            <span className={`teacher-badge ${getStatus(nextSchedule)}`}>
                                                {getStatusLabel(getStatus(nextSchedule))}
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
                                                disabled={!canCheckinSchedule(nextSchedule)}
                                            >
                                                <LogIn size={17} />
                                                Check-in
                                            </button>
                                            <button
                                                type="button"
                                                className="teacher-secondary-action"
                                                onClick={() => onCheckout(nextSchedule.id)}
                                                disabled={!canCheckoutSchedule(nextSchedule)}
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
                        </section>
                    )}

                    {activeTab === 'courses' && (
                        <section className="teacher-tab-panel">
                            <section className="teacher-content-grid">
                                <article className="teacher-panel">
                                    <div className="teacher-section-title">
                                        <BookOpen size={19} />
                                        <h2>Khóa học phụ trách</h2>
                                    </div>
                                    <div className="teacher-course-list teacher-course-card-list">
                                        {courseOptions.length > 0 ? (
                                            courseOptions.map((course) => {
                                                const courseSchedules = normalizedSchedules.filter(
                                                    (schedule) => (schedule.courseId || schedule.courseName) === (course.id || course.name)
                                                );

                                                return (
                                                    <div key={course.id || course.name} className="teacher-course-item">
                                                        <span></span>
                                                        <div>
                                                            <p>{course.name || 'Khóa học'}</p>
                                                            <small>{courseSchedules.length} buổi dạy</small>
                                                        </div>
                                                    </div>
                                                );
                                            })
                                        ) : (
                                            <p className="teacher-empty">Chưa có khóa học.</p>
                                        )}
                                    </div>
                                </article>

                                <aside className="teacher-panel">
                                    <div className="teacher-section-title">
                                        <BookOpen size={19} />
                                        <h2>Chuyên đề phụ trách</h2>
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
                                </aside>
                            </section>
                        </section>
                    )}

                    {activeTab === 'schedule' && (
                        <section className="teacher-tab-panel">
                            <article className="teacher-panel teacher-calendar-panel">
                                <div className="teacher-calendar-header">
                                    <div className="teacher-section-title teacher-schedule-title">
                                        <CalendarDays size={19} />
                                        <h2>Lịch dạy của tôi</h2>
                                        {activeCount > 0 && <span className="teacher-live-count">{activeCount} đang dạy</span>}
                                    </div>
                                    <div className="teacher-calendar-controls">
                                        <button type="button" onClick={() => handleCalendarMonthChange(-1)} title="Tháng trước">
                                            <ChevronLeft size={18} />
                                        </button>
                                        <strong>{format(calendarMonth, 'MMMM yyyy', { locale: vi })}</strong>
                                        <button type="button" onClick={() => handleCalendarMonthChange(1)} title="Tháng sau">
                                            <ChevronRight size={18} />
                                        </button>
                                    </div>
                                </div>

                                <div className="teacher-calendar-layout">
                                    <div className="teacher-calendar">
                                        <div className="teacher-calendar-weekdays">
                                            {WEEKDAYS.map((day) => <span key={day}>{day}</span>)}
                                        </div>
                                        <div className="teacher-calendar-grid">
                                            {calendarDays.map((day) => {
                                                const daySchedules = getSchedulesForDate(day);
                                                const isSelected = isSameDay(day, selectedDate);

                                                return (
                                                    <button
                                                        key={day.toISOString()}
                                                        type="button"
                                                        className={[
                                                            'teacher-calendar-day',
                                                            !isSameMonth(day, calendarMonth) ? 'muted' : '',
                                                            isToday(day) ? 'today' : '',
                                                            isSelected ? 'selected' : '',
                                                            daySchedules.length > 0 ? 'has-event' : ''
                                                        ].filter(Boolean).join(' ')}
                                                        onClick={() => setSelectedDate(day)}
                                                    >
                                                        <span className="teacher-calendar-number">{format(day, 'd')}</span>
                                                        <div className="teacher-calendar-events">
                                                            {daySchedules.slice(0, 2).map((schedule) => (
                                                                <span key={schedule.id || `${schedule.courseName}-${schedule.topicName}`} className={getStatus(schedule)}>
                                                                    {formatTime(schedule.expectedStart)} {schedule.topicName}
                                                                </span>
                                                            ))}
                                                            {daySchedules.length > 2 && <em>+{daySchedules.length - 2} buổi</em>}
                                                        </div>
                                                    </button>
                                                );
                                            })}
                                        </div>
                                    </div>

                                    <aside className="teacher-day-agenda">
                                        <div className="teacher-section-title">
                                            <Clock size={19} />
                                            <h2>{format(selectedDate, 'dd/MM/yyyy')}</h2>
                                        </div>
                                        <div className="teacher-schedule-list">
                                            {selectedDateSchedules.length > 0 ? (
                                                selectedDateSchedules.map((schedule) => {
                                                    const status = getStatus(schedule);
                                                    return (
                                                        <article key={schedule.id} className="teacher-schedule-row">
                                                            <div className="teacher-date-block">
                                                                <strong>{formatTime(schedule.expectedStart)}</strong>
                                                                <span>{formatTime(schedule.expectedEnd)}</span>
                                                            </div>
                                                            <div className="teacher-schedule-main">
                                                                <span className={`teacher-badge ${status}`}>
                                                                    {getStatusLabel(status)}
                                                                </span>
                                                                <h3>{schedule.topicName}</h3>
                                                                <p>{schedule.courseName}</p>
                                                                <small>{formatDate(schedule.expectedDate || schedule.expectedStart)}</small>
                                                            </div>
                                                            <div className="teacher-row-actions">
                                                                <button
                                                                    type="button"
                                                                    onClick={() => onCheckin(schedule.id)}
                                                                    disabled={!canCheckinSchedule(schedule)}
                                                                    title="Check-in"
                                                                >
                                                                    <LogIn size={17} />
                                                                </button>
                                                                <button
                                                                    type="button"
                                                                    onClick={() => onCheckout(schedule.id)}
                                                                    disabled={!canCheckoutSchedule(schedule)}
                                                                    title="Check-out"
                                                                >
                                                                    <LogOut size={17} />
                                                                </button>
                                                            </div>
                                                        </article>
                                                    );
                                                })
                                            ) : (
                                                <p className="teacher-empty">Không có buổi dạy trong ngày này.</p>
                                            )}
                                        </div>
                                    </aside>
                                </div>
                            </article>
                        </section>
                    )}
                </div>
            </div>
        </main>
    );
};

export default TeacherWorkspaceUI;
