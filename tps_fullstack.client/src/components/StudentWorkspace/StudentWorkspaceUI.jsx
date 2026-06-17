import { useMemo, useState } from 'react';
import {
    addDays,
    addMonths,
    endOfMonth,
    endOfWeek,
    format,
    isAfter,
    isSameDay,
    isSameMonth,
    isToday,
    startOfMonth,
    startOfWeek,
    subMonths
} from 'date-fns';
import { vi } from 'date-fns/locale';
import {
    Award,
    ArrowLeft,
    BookOpen,
    CalendarDays,
    CheckCircle2,
    ChevronLeft,
    ChevronRight,
    ClipboardList,
    Clock,
    FileCheck2,
    GraduationCap,
    Loader2,
    LogIn,
    Mail,
    MapPin,
    Phone,
    RotateCcw,
    Send,
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

const getTimeParts = (value) => {
    if (!value) return null;

    if (value instanceof Date) {
        return {
            hours: value.getHours(),
            minutes: value.getMinutes()
        };
    }

    if (typeof value === 'string') {
        const trimmed = value.trim();
        const timeMatch = trimmed.match(/(?:T|\s|^)(\d{1,2}):(\d{2})(?::\d{2})?/);

        if (timeMatch) {
            return {
                hours: Number(timeMatch[1]),
                minutes: Number(timeMatch[2])
            };
        }
    }

    const date = toDate(value);
    return date
        ? {
            hours: date.getHours(),
            minutes: date.getMinutes()
        }
        : null;
};

const combineDateAndTime = (dateValue, timeValue) => {
    const date = toDate(dateValue);
    if (!date) return null;

    const timeParts = getTimeParts(timeValue);
    const combinedDate = new Date(date);

    if (timeParts) {
        combinedDate.setHours(timeParts.hours, timeParts.minutes, 0, 0);
    } else {
        combinedDate.setHours(0, 0, 0, 0);
    }

    return combinedDate;
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
    const timeParts = getTimeParts(value);
    if (!timeParts) return '--:--';

    return `${String(timeParts.hours).padStart(2, '0')}:${String(timeParts.minutes).padStart(2, '0')}`;
};

const getInitials = (name) => {
    if (!name) return 'HV';
    const parts = name.trim().split(/\s+/).filter(Boolean);
    if (parts.length >= 2) return `${parts[0][0]}${parts[parts.length - 1][0]}`.toUpperCase();
    return name.substring(0, 2).toUpperCase();
};

function normalizeAttendanceStatus(value) {
    if (typeof value === 'boolean') return value;
    if (typeof value === 'number') return value === 1;
    if (typeof value === 'string') {
        const normalized = value.trim().toLowerCase();
        return normalized === 'true' || normalized === '1' || normalized === 'diem danh';
    }
    return false;
}

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

const getScheduleStatus = (schedule) => {
    if (schedule.attended) return 'attended';
    if (schedule.actualStart) return 'active';

    const startDate = combineDateAndTime(schedule.expectedDate, schedule.expectedStart);
    if (startDate && isAfter(startDate, new Date())) return 'upcoming';
    return 'pending';
};

const getStatusLabel = (status) => {
    const labels = {
        attended: 'Đã điểm danh',
        completed: 'Đã học',
        active: 'Đang diễn ra',
        upcoming: 'Sắp học',
        pending: 'Chưa điểm danh'
    };

    return labels[status] || labels.pending;
};

const getExamStatusLabel = (status) => {
    const labels = {
        submitted: 'Đã nộp',
        in_progress: 'Đang làm',
        pending: 'Cần thực hiện'
    };

    return labels[status] || labels.pending;
};

const formatScore = (score) => {
    if (score === undefined || score === null || score === '') return '--';
    const numericScore = Number(score);
    return Number.isNaN(numericScore) ? score : numericScore.toFixed(2).replace(/\.00$/, '');
};

const getInitialSelectedAnswers = (exam) => {
    const initialAnswers = {};
    (exam?.questions || []).forEach((question) => {
        const selectedAnswer = (question.answers || []).find((answer) => answer.isSelected);
        if (selectedAnswer?.maID) initialAnswers[question.maID] = selectedAnswer.maID;
    });
    return initialAnswers;
};

const StudentExamDetail = ({
    selectedExam,
    isExamLoading,
    examActionId,
    onCloseExam,
    onSubmitExam
}) => {
    const [selectedAnswers, setSelectedAnswers] = useState(() => getInitialSelectedAnswers(selectedExam));
    const [submitWarning, setSubmitWarning] = useState(null);
    const isSubmitted = Boolean(selectedExam?.endedAt);

    const handleSelectAnswer = (questionId, answerId) => {
        if (isSubmitted) return;
        setSelectedAnswers((current) => ({
            ...current,
            [questionId]: answerId
        }));
        setSubmitWarning(null);
    };

    const handleSubmit = async () => {
        if (!selectedExam || isSubmitted) return;

        const unansweredQuestions = (selectedExam.questions || []).filter((question) => !selectedAnswers[question.maID]);
        if (unansweredQuestions.length > 0) {
            setSubmitWarning(`Bạn còn ${unansweredQuestions.length} câu chưa chọn đáp án.`);
            return;
        }

        const submitted = await onSubmitExam(selectedExam, selectedAnswers);
        if (submitted) setSubmitWarning(null);
    };

    const getQuestionResult = (question) => {
        if (!isSubmitted) return null;
        const selectedAnswer = (question.answers || []).find((answer) => selectedAnswers[question.maID] === answer.maID);
        if (!selectedAnswer) return 'missing';
        return selectedAnswer.isCorrect ? 'correct' : 'wrong';
    };

    return (
        <article className="student-panel student-exam-detail-panel">
            <div className="student-exam-detail-header">
                <button className="student-secondary-action" type="button" onClick={onCloseExam}>
                    <ArrowLeft size={17} />
                    <span>Danh sách</span>
                </button>
                <div>
                    <span className={`student-badge ${selectedExam.status}`}>
                        {getExamStatusLabel(selectedExam.status)}
                    </span>
                    <h2>{selectedExam.courseName}</h2>
                    <p>{(selectedExam.questions || []).length} câu hỏi</p>
                </div>
                <div className="student-exam-score">
                    <span>Điểm</span>
                    <strong>{formatScore(selectedExam.score)}</strong>
                </div>
            </div>

            {isExamLoading ? (
                <div className="student-exam-loading">
                    <Loader2 className="student-spin" size={24} />
                    <span>Đang tải bài thu hoạch...</span>
                </div>
            ) : (
                <>
                    <div className="student-exam-meta">
                        <span>Bắt đầu: {selectedExam.startedAt ? `${formatShortDate(selectedExam.startedAt)} ${formatTime(selectedExam.startedAt)}` : 'Chưa bắt đầu'}</span>
                        <span>Nộp bài: {selectedExam.endedAt ? `${formatShortDate(selectedExam.endedAt)} ${formatTime(selectedExam.endedAt)}` : 'Chưa nộp'}</span>
                    </div>

                    <div className="student-question-list">
                        {(selectedExam.questions || []).map((question, index) => (
                            <section key={question.maID} className="student-question-block">
                                <div className="student-question-title">
                                    <span>Câu {index + 1}</span>
                                    {isSubmitted && (
                                        <em className={`student-question-result ${getQuestionResult(question)}`}>
                                            {getQuestionResult(question) === 'correct' ? 'Đúng' : 'Sai'}
                                        </em>
                                    )}
                                    <h3>{question.questionText || 'Câu hỏi chưa có nội dung'}</h3>
                                </div>
                                <div className="student-answer-list" role="radiogroup" aria-label={`Câu ${index + 1}`}>
                                    {(question.answers || []).map((answer) => {
                                        const checked = selectedAnswers[question.maID] === answer.maID;
                                        const answerState = isSubmitted
                                            ? answer.isCorrect
                                                ? 'correct'
                                                : checked
                                                    ? 'wrong'
                                                    : ''
                                            : '';
                                        return (
                                            <label
                                                key={answer.maID}
                                                className={`student-answer-option ${checked ? 'selected' : ''} ${answerState}`}
                                            >
                                                <input
                                                    type="radio"
                                                    name={`question-${question.maID}`}
                                                    checked={checked}
                                                    disabled={isSubmitted}
                                                    onChange={() => handleSelectAnswer(question.maID, answer.maID)}
                                                />
                                                <span>{answer.answerText || 'Đáp án chưa có nội dung'}</span>
                                                {isSubmitted && answer.isCorrect && <strong>Đáp án đúng</strong>}
                                                {isSubmitted && checked && !answer.isCorrect && <strong>Bạn đã chọn</strong>}
                                            </label>
                                        );
                                    })}
                                </div>
                            </section>
                        ))}
                    </div>

                    {submitWarning && <div className="student-alert student-exam-warning">{submitWarning}</div>}

                    <div className="student-exam-footer">
                        {isSubmitted ? (
                            <span className="student-submitted-note">
                                <FileCheck2 size={18} />
                                Bài thu hoạch đã được nộp.
                            </span>
                        ) : (
                            <button
                                type="button"
                                className="student-primary-action"
                                onClick={handleSubmit}
                                disabled={examActionId === selectedExam.maID}
                            >
                                {examActionId === selectedExam.maID ? <Loader2 className="student-spin" size={17} /> : <Send size={17} />}
                                Nộp bài
                            </button>
                        )}
                    </div>
                </>
            )}
        </article>
    );
};

const StudentExamPanel = ({
    exams = [],
    selectedExam,
    isExamLoading,
    examActionId,
    onOpenExam,
    onCloseExam,
    onSubmitExam
}) => {
    if (selectedExam) {
        return (
            <StudentExamDetail
                key={selectedExam.maID}
                selectedExam={selectedExam}
                isExamLoading={isExamLoading}
                examActionId={examActionId}
                onCloseExam={onCloseExam}
                onSubmitExam={onSubmitExam}
            />
        );
    }

    return (
        <article className="student-panel student-exam-list-panel">
            <div className="student-section-title">
                <ClipboardList size={19} />
                <h2>Danh sách bài thu hoạch cần thực hiện</h2>
            </div>
            <div className="student-exam-list">
                {exams.length > 0 ? (
                    exams.map((exam) => (
                        <article key={exam.maID} className="student-exam-card">
                            <div className="student-exam-card-main">
                                <span className={`student-badge ${exam.status}`}>
                                    {getExamStatusLabel(exam.status)}
                                </span>
                                <h3>{exam.courseName}</h3>
                                <p>{exam.endedAt ? `Đã nộp lúc ${formatShortDate(exam.endedAt)} ${formatTime(exam.endedAt)}` : 'Chọn làm bài để xem câu hỏi và nộp bài.'}</p>
                            </div>
                            <div className="student-exam-card-side">
                                <span>Điểm</span>
                                <strong>{formatScore(exam.score)}</strong>
                                <button
                                    type="button"
                                    className="student-primary-action"
                                    onClick={() => onOpenExam(exam.maID, { viewResult: Boolean(exam.endedAt) })}
                                    disabled={examActionId === exam.maID}
                                >
                                    {examActionId === exam.maID ? <Loader2 className="student-spin" size={17} /> : <FileCheck2 size={17} />}
                                    {exam.endedAt ? 'Xem bài' : 'Làm bài'}
                                </button>
                            </div>
                        </article>
                    ))
                ) : (
                    <p className="student-empty">Chưa có bài thu hoạch được giao.</p>
                )}
            </div>
        </article>
    );
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

const getScheduleDate = (schedule) => {
    const date = toDate(schedule.expectedDate);
    if (!date) return null;

    const scheduleDate = new Date(date);
    scheduleDate.setHours(0, 0, 0, 0);
    return scheduleDate;
};

const getScheduleDateTime = (schedule) => combineDateAndTime(schedule.expectedDate, schedule.expectedStart);

const WEEKDAYS = ['T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'CN'];

const StudentWorkspaceUI = ({
    student,
    schedules,
    exams,
    selectedExam,
    isLoading,
    isExamLoading,
    error,
    successMessage,
    actionId,
    examActionId,
    onRefresh,
    onCheckin,
    onOpenExam,
    onCloseExam,
    onSubmitExam
}) => {
    const [activeTab, setActiveTab] = useState('profile');
    const [calendarMonth, setCalendarMonth] = useState(() => startOfMonth(new Date()));
    const [selectedDate, setSelectedDate] = useState(() => new Date());

    const certificates = getValue(student, 'certificates', 'Certificates') || [];
    const normalizedSchedules = useMemo(() => schedules.map(normalizeSchedule).sort((a, b) => {
        const left = getScheduleDate(a)?.getTime() || 0;
        const right = getScheduleDate(b)?.getTime() || 0;
        return left - right;
    }), [schedules]);

    const courses = useMemo(() => uniqueCourses(normalizedSchedules), [normalizedSchedules]);
    const todaySchedules = normalizedSchedules.filter((schedule) => {
        const scheduleDate = getScheduleDate(schedule);
        return scheduleDate && isSameDay(scheduleDate, new Date());
    });
    const attendedCount = normalizedSchedules.filter((schedule) => schedule.attended).length;
    const nextSchedule = normalizedSchedules.find((schedule) => !schedule.attended) || normalizedSchedules[0];
    const nextScheduleStatus = nextSchedule ? getScheduleStatus(nextSchedule) : null;

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
    const monthSchedules = normalizedSchedules.filter((schedule) => {
        const scheduleDate = getScheduleDate(schedule);
        return scheduleDate && isSameMonth(scheduleDate, calendarMonth);
    });
    const attendedInMonth = monthSchedules.filter((schedule) => schedule.attended).length;
    const upcomingMonthSchedules = monthSchedules.filter((schedule) => {
        const scheduleDate = getScheduleDate(schedule);
        return scheduleDate && (isAfter(scheduleDate, new Date()) || isSameDay(scheduleDate, new Date()));
    }).slice(0, 5);
    const agendaPreviewSchedules = upcomingMonthSchedules.length > 0 ? upcomingMonthSchedules : monthSchedules.slice(0, 5);

    const getSchedulesForDate = (date) => normalizedSchedules.filter((schedule) => {
        const scheduleDate = getScheduleDate(schedule);
        return scheduleDate && isSameDay(scheduleDate, date);
    });

    const canCheckinSchedule = (schedule) => Boolean(
        schedule?.id &&
        !schedule.attended &&
        !schedule.actualEnd &&
        actionId !== schedule.id
    );

    const handleCalendarMonthChange = (direction) => {
        const nextMonth = direction > 0 ? addMonths(calendarMonth, 1) : subMonths(calendarMonth, 1);
        setCalendarMonth(nextMonth);
        setSelectedDate(startOfMonth(nextMonth));
    };

    const tabs = [
        { id: 'profile', label: 'Thông tin học viên', icon: UserRound },
        { id: 'courses', label: 'Khóa học của tôi', icon: BookOpen },
        { id: 'schedule', label: 'Lịch học của tôi', icon: CalendarDays },
        { id: 'exams', label: 'Quản lý bài thu hoạch', icon: ClipboardList }
    ];

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

            <div className="student-workspace-layout">
                <nav className="student-workspace-nav" aria-label="Điều hướng không gian học viên">
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

                <div className="student-workspace-main">
                    {activeTab === 'profile' && (
                        <section className="student-tab-panel">
                    <div className="student-overview">
                        <article className="student-profile">
                            <div className="student-section-title">
                                <UserRound size={19} />
                                <h2>Thông tin chi tiết học viên</h2>
                            </div>
                            <dl>
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
                                <p>Đã điểm danh</p>
                            </article>
                            <article className="student-stat">
                                <Award size={21} />
                                <span>{certificates.length}</span>
                                <p>Chứng chỉ</p>
                            </article>
                        </div>
                    </div>

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
                                            disabled={!canCheckinSchedule(nextSchedule)}
                                        >
                                            {nextSchedule.attended ? <CheckCircle2 size={17} /> : <LogIn size={17} />}
                                            {nextSchedule.attended ? 'Đã điểm danh' : 'Check-in buổi học'}
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
                        </section>
                    )}

                    {activeTab === 'courses' && (
                        <section className="student-tab-panel">
                    <article className="student-panel student-course-panel">
                        <div className="student-section-title">
                            <BookOpen size={19} />
                            <h2>Khóa học của tôi</h2>
                        </div>
                        <div className="student-course-list">
                            {courses.length > 0 ? (
                                courses.map((course) => {
                                    const courseSchedules = normalizedSchedules.filter(
                                        (schedule) => (schedule.courseId || schedule.courseName) === (course.courseId || course.courseName)
                                    );
                                    const attendedInCourse = courseSchedules.filter((schedule) => schedule.attended).length;

                                    return (
                                        <article key={course.courseId || course.courseName} className="student-course-card">
                                            <span></span>
                                            <h3>{course.courseName}</h3>
                                            <p>{courseSchedules.length} buổi học</p>
                                            <small>{attendedInCourse} buổi đã điểm danh</small>
                                        </article>
                                    );
                                })
                            ) : (
                                <p className="student-empty">Chưa có khóa học.</p>
                            )}
                        </div>
                    </article>
                        </section>
                    )}

                    {activeTab === 'schedule' && (
                        <section className="student-tab-panel">
                    <article className="student-panel student-calendar-panel">
                        <div className="student-calendar-header">
                            <div className="student-section-title">
                                <CalendarDays size={19} />
                                <h2>Lịch học của tôi</h2>
                            </div>
                            <div className="student-calendar-controls">
                                <button type="button" onClick={() => handleCalendarMonthChange(-1)} title="Tháng trước">
                                    <ChevronLeft size={18} />
                                </button>
                                <strong>{format(calendarMonth, 'MMMM yyyy', { locale: vi })}</strong>
                                <button type="button" onClick={() => handleCalendarMonthChange(1)} title="Tháng sau">
                                    <ChevronRight size={18} />
                                </button>
                            </div>
                        </div>

                        <div className="student-calendar-summary" aria-label="Tổng quan lịch học trong tháng">
                            <div>
                                <span>Buổi trong tháng</span>
                                <strong>{monthSchedules.length}</strong>
                            </div>
                            <div>
                                <span>Ngày đang chọn</span>
                                <strong>{selectedDateSchedules.length}</strong>
                            </div>
                            <div>
                                <span>Đã điểm danh</span>
                                <strong>{attendedInMonth}</strong>
                            </div>
                        </div>

                        <div className="student-calendar-layout">
                            <div className="student-calendar">
                                <div className="student-calendar-weekdays">
                                    {WEEKDAYS.map((day) => <span key={day}>{day}</span>)}
                                </div>
                                <div className="student-calendar-grid">
                                    {calendarDays.map((day) => {
                                        const daySchedules = getSchedulesForDate(day);
                                        const isSelected = isSameDay(day, selectedDate);

                                        return (
                                            <button
                                                key={day.toISOString()}
                                                type="button"
                                                className={[
                                                    'student-calendar-day',
                                                    !isSameMonth(day, calendarMonth) ? 'muted' : '',
                                                    isToday(day) ? 'today' : '',
                                                    isSelected ? 'selected' : '',
                                                    daySchedules.length > 0 ? 'has-event' : ''
                                                ].filter(Boolean).join(' ')}
                                                onClick={() => setSelectedDate(day)}
                                            >
                                                <span className="student-calendar-number">{format(day, 'd')}</span>
                                                <div className="student-calendar-events">
                                                    {daySchedules.slice(0, 2).map((schedule) => (
                                                        <span key={schedule.id || `${schedule.courseName}-${schedule.topicName}`} className={getScheduleStatus(schedule)}>
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

                            <aside className="student-day-agenda">
                                <div className="student-agenda-heading">
                                    <div className="student-section-title">
                                        <Clock size={19} />
                                        <h2>{format(selectedDate, 'dd/MM/yyyy')}</h2>
                                    </div>
                                    <span>{selectedDateSchedules.length} buổi học</span>
                                </div>
                                <div className="student-schedule-list">
                                    {selectedDateSchedules.length > 0 ? (
                                        selectedDateSchedules.map((schedule) => {
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
                                                        <small>{schedule.teacherName || 'Chưa phân công'}</small>
                                                    </div>
                                                    {!schedule.actualEnd && (
                                                        <div className="student-row-actions">
                                                            <button
                                                                type="button"
                                                                onClick={() => onCheckin(schedule.id)}
                                                                disabled={!canCheckinSchedule(schedule)}
                                                                title={schedule.attended ? 'Đã điểm danh' : 'Check-in'}
                                                            >
                                                                {schedule.attended ? <CheckCircle2 size={17} /> : <LogIn size={17} />}
                                                            </button>
                                                        </div>
                                                    )}
                                                </article>
                                            );
                                        })
                                    ) : (
                                        <p className="student-empty">Không có buổi học trong ngày này.</p>
                                    )}
                                </div>

                                <div className="student-agenda-divider"></div>

                                <div className="student-agenda-heading">
                                    <div className="student-section-title">
                                        <CalendarDays size={19} />
                                        <h2>Buổi học sắp tới</h2>
                                    </div>
                                    <span>{monthSchedules.length} buổi trong tháng</span>
                                </div>
                                <div className="student-schedule-list compact">
                                    {agendaPreviewSchedules.length > 0 ? (
                                        agendaPreviewSchedules.map((schedule) => {
                                            const status = getScheduleStatus(schedule);
                                            return (
                                                <article key={`preview-${schedule.id}`} className={`student-schedule-row ${schedule.attended ? 'attended' : ''}`}>
                                                    <div className="student-date-block">
                                                        <strong>{formatShortDate(schedule.expectedDate || schedule.expectedStart)}</strong>
                                                        <span>{formatTime(schedule.expectedStart)}</span>
                                                    </div>
                                                    <div className="student-schedule-main">
                                                        <span className={`student-badge ${status}`}>
                                                            {getStatusLabel(status)}
                                                        </span>
                                                        <h3>{schedule.topicName}</h3>
                                                        <p>{schedule.courseName}</p>
                                                        <small>{schedule.teacherName || 'Chưa phân công'}</small>
                                                    </div>
                                                </article>
                                            );
                                        })
                                    ) : (
                                        <p className="student-empty">Chưa có buổi học trong tháng này.</p>
                                    )}
                                </div>
                            </aside>
                        </div>
                    </article>
                        </section>
                    )}

                    {activeTab === 'exams' && (
                        <section className="student-tab-panel">
                            <StudentExamPanel
                                exams={exams}
                                selectedExam={selectedExam}
                                isExamLoading={isExamLoading}
                                examActionId={examActionId}
                                onOpenExam={onOpenExam}
                                onCloseExam={onCloseExam}
                                onSubmitExam={onSubmitExam}
                            />
                        </section>
                    )}
                </div>
            </div>
        </main>
    );
};

export default StudentWorkspaceUI;
