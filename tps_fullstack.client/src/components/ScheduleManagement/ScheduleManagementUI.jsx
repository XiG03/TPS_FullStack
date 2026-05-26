import { format, getDay, isSameDay } from 'date-fns';
import { vi } from 'date-fns/locale';
import { useNavigate } from 'react-router-dom';

const dayNames = ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'];

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

const formatTime = (value) => {
    const date = toDate(value);
    return date ? format(date, 'HH:mm') : '--:--';
};

const combineDateAndTime = (dateValue, timeValue) => {
    const date = toDate(dateValue);
    if (!date) return toDate(timeValue);

    const combined = new Date(date);
    const time = toDate(timeValue);

    if (time) {
        combined.setHours(time.getHours(), time.getMinutes(), 0, 0);
    }

    return combined;
};

const getEventStart = (schedule) => (
    combineDateAndTime(
        getValue(schedule, 'ngaydukien', 'Ngaydukien'),
        getValue(schedule, 'batdaudukien', 'Batdaudukien')
    )
);

const getEventEnd = (schedule) => (
    combineDateAndTime(
        getValue(schedule, 'ngaydukien', 'Ngaydukien'),
        getValue(schedule, 'ketthucdukien', 'Ketthucdukien', 'kethucdukien', 'Kethucdukien') ||
            getValue(schedule, 'batdaudukien', 'Batdaudukien')
    )
);

const normalizeSchedule = (schedule) => ({
    raw: schedule,
    id: getValue(schedule, 'lichhocID', 'LichhocID', 'maID', 'MaID'),
    courseId: getValue(schedule, 'khoahocID', 'KhoahocID'),
    courseName: getValue(schedule, 'tenKhoahoc', 'TenKhoahoc') || 'Khóa học chưa đặt tên',
    topicId: getValue(schedule, 'chuyendeID', 'ChuyendeID'),
    topicName: getValue(schedule, 'tenChuyende', 'TenChuyende'),
    teacherId: getValue(schedule, 'giangvienID', 'GiangvienID'),
    teacherName: getValue(schedule, 'tenGiangvien', 'TenGiangvien'),
    expectedDate: getValue(schedule, 'ngaydukien', 'Ngaydukien'),
    expectedStart: getValue(schedule, 'batdaudukien', 'Batdaudukien'),
    expectedEnd: getValue(schedule, 'ketthucdukien', 'Ketthucdukien', 'kethucdukien', 'Kethucdukien'),
    actualStart: getValue(schedule, 'batdauthucte', 'Batdauthucte'),
    actualEnd: getValue(schedule, 'ketthucthucte', 'Ketthucthucte', 'kethucthucte', 'Kethucthucte')
});

const getCourseColor = (courseKey = '') => {
    const palette = [
        {
            bgColor: 'bg-[#0B4AA2]',
            textColor: 'text-white',
            tagBg: 'bg-white/20',
            tagText: 'text-white',
            subTextColor: 'text-blue-100',
            borderColor: 'border-[#083A80]'
        },
        {
            bgColor: 'bg-[#F2F7FF]',
            textColor: 'text-[#123B73]',
            tagBg: 'bg-[#DCEBFF]',
            tagText: 'text-[#0B4AA2]',
            subTextColor: 'text-[#4B678A]',
            borderColor: 'border-[#C7DBF7]'
        },
        {
            bgColor: 'bg-[#FFF3ED]',
            textColor: 'text-[#6F2C12]',
            tagBg: 'bg-[#FFE0D1]',
            tagText: 'text-[#7D2B0C]',
            subTextColor: 'text-[#8D5138]',
            borderColor: 'border-[#FFD1BC]'
        },
        {
            bgColor: 'bg-[#F0F7F3]',
            textColor: 'text-[#164A2E]',
            tagBg: 'bg-[#D7EEE0]',
            tagText: 'text-[#166236]',
            subTextColor: 'text-[#4D755D]',
            borderColor: 'border-[#C5E3D1]'
        }
    ];

    const key = String(courseKey || '');
    const index = Math.abs(key.split('').reduce((sum, char) => sum + char.charCodeAt(0), 0)) % palette.length;
    return palette[index];
};

const ScheduleManagementUI = ({
    schedules,
    isLoading,
    error,
    onAddSchedule,
    onEditSchedule,
    onDeleteSchedule,
    weekDays,
    currentDate,
    nextWeek,
    prevWeek,
    goToday
}) => {
    const navigate = useNavigate();
    const monthYearStr = format(currentDate, "'Tháng' M, yyyy", { locale: vi });
    const normalizedSchedules = schedules.map(normalizeSchedule);

    let minHour = 8;
    let maxHour = 17;

    const currentWeekEvents = normalizedSchedules
        .filter((schedule) => {
            const startDate = getEventStart(schedule.raw);
            return startDate && weekDays.some((weekDay) => isSameDay(weekDay, startDate));
        })
        .sort((a, b) => getEventStart(a.raw) - getEventStart(b.raw));

    currentWeekEvents.forEach((schedule) => {
        const startDate = getEventStart(schedule.raw);
        const endDate = getEventEnd(schedule.raw) || startDate;
        if (!startDate) return;

        const startHour = startDate.getHours();
        const adjustedEnd = endDate.getHours() + (endDate.getMinutes() > 0 ? 1 : 0);

        minHour = Math.min(minHour, startHour);
        maxHour = Math.max(maxHour, adjustedEnd);
    });

    maxHour = Math.min(23, Math.max(minHour + 1, maxHour + 1));

    const timeSlots = [];
    for (let hour = minHour; hour <= maxHour; hour += 1) {
        timeSlots.push(`${hour.toString().padStart(2, '0')}:00`);
    }

    const numRows = timeSlots.length;
    const completedEvents = currentWeekEvents.filter((event) => event.actualStart || event.actualEnd).length;

    const renderEventCard = (event) => {
        const startDate = getEventStart(event.raw);
        const endDate = getEventEnd(event.raw) || (startDate ? new Date(startDate.getTime() + 60 * 60 * 1000) : null);
        if (!startDate || !endDate) return null;

        const startHourValue = startDate.getHours() + startDate.getMinutes() / 60;
        const topPx = Math.max(0, (startHourValue - minHour) * 88);
        const durationHours = Math.max(0.75, (endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60));
        const heightPx = Math.min(Math.max(durationHours * 88, 68), 180);
        const color = getCourseColor(event.courseId || event.courseName);
        const title = event.topicName || event.courseName;

        return (
            <div
                key={event.id}
                onClick={() => navigate(`/schedules/${event.id}`)}
                className={`absolute left-1.5 right-1.5 rounded-lg p-3 shadow-sm z-10 hover:shadow-md transition-all cursor-pointer group border ${color.bgColor} ${color.textColor} ${color.borderColor}`}
                style={{ top: `${topPx}px`, height: `${heightPx}px` }}
            >
                <div className="flex h-full min-h-0 flex-col justify-between gap-2">
                    <div className="min-w-0">
                        <div className="mb-1.5 flex items-center justify-between gap-2">
                            <span className={`inline-flex px-1.5 py-0.5 rounded text-[9px] font-label font-bold uppercase ${color.tagBg} ${color.tagText}`}>
                                {event.actualStart || event.actualEnd ? 'Đã học' : 'Dự kiến'}
                            </span>
                            <div className="flex items-center gap-1 opacity-100 sm:opacity-0 sm:group-hover:opacity-100 transition-opacity">
                                <button
                                    type="button"
                                    onClick={(clickEvent) => {
                                        clickEvent.stopPropagation();
                                        onEditSchedule(event.raw);
                                    }}
                                    className="w-6 h-6 rounded bg-white/20 flex items-center justify-center hover:bg-white/30"
                                    title="Chỉnh sửa"
                                >
                                    <span className="material-symbols-outlined text-[14px]">edit</span>
                                </button>
                                <button
                                    type="button"
                                    onClick={(clickEvent) => {
                                        clickEvent.stopPropagation();
                                        onDeleteSchedule(event.id);
                                    }}
                                    className="w-6 h-6 rounded bg-white/20 flex items-center justify-center hover:bg-white/30"
                                    title="Xóa"
                                >
                                    <span className="material-symbols-outlined text-[14px]">delete</span>
                                </button>
                            </div>
                        </div>
                        <h4 className="text-[13px] font-headline font-bold leading-tight truncate" title={title}>
                            {title}
                        </h4>
                        <p className={`mt-1 text-[10px] font-body truncate ${color.subTextColor}`} title={event.courseName}>
                            {event.courseName}
                        </p>
                    </div>
                    <div className={`text-[10px] font-body ${color.subTextColor}`}>
                        <p>{formatTime(event.expectedStart || event.expectedDate)} - {formatTime(event.expectedEnd)}</p>
                        <p className="truncate">{event.teacherName || (event.teacherId ? 'Đã phân công giảng viên' : 'Chưa phân công giảng viên')}</p>
                    </div>
                </div>
            </div>
        );
    };

    const renderEventsForDay = (date) => {
        const dayEvents = currentWeekEvents.filter((schedule) => {
            const eventDate = getEventStart(schedule.raw);
            return eventDate && isSameDay(eventDate, date);
        });

        return dayEvents.map(renderEventCard);
    };

    return (
        <div className="mx-auto flex max-w-full flex-col px-4 pb-20 pt-6 sm:px-6 lg:px-10">
            <div className="mb-8 flex flex-col gap-5 xl:flex-row xl:items-end xl:justify-between">
                <div className="min-w-0">
                    <h2 className="font-headline text-3xl font-extrabold tracking-tight text-on-surface">
                        Lịch dạy học
                    </h2>
                    <p className="mt-2 font-body text-sm text-on-surface-variant">
                        {monthYearStr} - theo dõi các buổi học trong tuần
                    </p>
                </div>
                <div className="flex flex-wrap items-center gap-3">
                    <div className="flex items-center rounded-lg border border-outline-variant/30 bg-surface-container-lowest p-1 shadow-sm">
                        <button onClick={prevWeek} className="p-1.5 hover:bg-surface-container-low rounded-md transition-all text-on-surface-variant hover:text-on-surface" title="Tuần trước">
                            <span className="material-symbols-outlined text-[20px]">chevron_left</span>
                        </button>
                        <button onClick={goToday} className="px-4 py-1.5 font-label font-bold text-sm text-[#0B4AA2] hover:bg-blue-50 rounded-md transition-all">
                            Hôm nay
                        </button>
                        <button onClick={nextWeek} className="p-1.5 hover:bg-surface-container-low rounded-md transition-all text-on-surface-variant hover:text-on-surface" title="Tuần sau">
                            <span className="material-symbols-outlined text-[20px]">chevron_right</span>
                        </button>
                    </div>
                    <button
                        onClick={onAddSchedule}
                        className="flex items-center gap-2 rounded-lg bg-[#0B4AA2] px-5 py-2.5 text-sm font-label font-bold text-white shadow-sm transition-all hover:bg-[#083A80]"
                    >
                        <span className="material-symbols-outlined text-[18px]">add</span>
                        <span>Tạo lịch mới</span>
                    </button>
                </div>
            </div>

            <div className="mb-6 grid grid-cols-1 gap-3 md:grid-cols-3">
                <div className="flex items-center gap-4 rounded-lg border border-outline-variant/30 bg-surface-container-lowest p-4 shadow-sm">
                    <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-blue-50 text-[#0B4AA2]">
                        <span className="material-symbols-outlined">event</span>
                    </div>
                    <div>
                        <p className="font-label text-[10px] font-extrabold uppercase text-outline">Buổi học trong tuần</p>
                        <span className="font-headline text-xl font-extrabold text-on-surface">{currentWeekEvents.length}</span>
                    </div>
                </div>
                <div className="flex items-center gap-4 rounded-lg border border-outline-variant/30 bg-surface-container-lowest p-4 shadow-sm">
                    <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-green-50 text-[#166236]">
                        <span className="material-symbols-outlined">task_alt</span>
                    </div>
                    <div>
                        <p className="font-label text-[10px] font-extrabold uppercase text-outline">Đã cập nhật thực tế</p>
                        <span className="font-headline text-xl font-extrabold text-on-surface">{completedEvents}</span>
                    </div>
                </div>
                <div className="flex items-center gap-4 rounded-lg border border-outline-variant/30 bg-surface-container-lowest p-4 shadow-sm">
                    <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-surface-container-high text-on-surface-variant">
                        <span className="material-symbols-outlined">database</span>
                    </div>
                    <div>
                        <p className="font-label text-[10px] font-extrabold uppercase text-outline">Tổng lịch học</p>
                        <span className="font-headline text-xl font-extrabold text-on-surface">{schedules.length}</span>
                    </div>
                </div>
            </div>

            {isLoading && <div className="mb-4 rounded-lg bg-surface-container-lowest p-4 text-center font-body text-on-surface-variant">Đang tải dữ liệu...</div>}
            {error && <div className="mb-4 rounded-lg bg-error-container p-4 text-center font-body text-error">{error}</div>}

            <div className="grid grid-cols-1 gap-6 2xl:grid-cols-[minmax(0,1fr)_360px]">
                <div className="rounded-lg border border-outline-variant/30 bg-surface-container-lowest shadow-sm">
                    <div className="overflow-x-auto calendar-scroll">
                        <div className="min-w-[980px]">
                            <div className="grid grid-cols-[64px_repeat(7,1fr)] border-b border-outline-variant/30 bg-surface-container-low/50">
                                <div className="flex items-end justify-center border-r border-outline-variant/20 p-3 pb-2">
                                    <span className="font-body text-[10px] font-medium text-outline">Giờ</span>
                                </div>
                                {weekDays.map((date, index) => {
                                    const isToday = isSameDay(date, new Date());
                                    return (
                                        <div key={date.toISOString()} className={`p-3 text-center ${index < 6 ? 'border-r border-outline-variant/20' : ''} ${isToday ? 'bg-blue-50 border-b-2 border-b-[#0B4AA2]' : ''}`}>
                                            <p className={`mb-1 font-label text-[11px] font-bold uppercase ${isToday ? 'text-[#0B4AA2]' : 'text-outline'}`}>
                                                {dayNames[getDay(date)]}
                                            </p>
                                            <p className={`font-headline text-xl font-extrabold ${isToday ? 'text-[#0B4AA2]' : 'text-on-surface'}`}>
                                                {format(date, 'dd')}
                                            </p>
                                        </div>
                                    );
                                })}
                            </div>

                            <div className="relative bg-surface-container-lowest" style={{ minHeight: `${numRows * 88}px` }}>
                                <div className="absolute inset-0 grid grid-cols-[64px_repeat(7,1fr)] pointer-events-none">
                                    <div className="border-r border-outline-variant/20 bg-surface-container-low/20"></div>
                                    {weekDays.map((date) => (
                                        <div key={date.toISOString()} className={`border-r border-outline-variant/20 ${isSameDay(date, new Date()) ? 'bg-blue-50/20' : ''}`}></div>
                                    ))}
                                </div>

                                <div className="absolute inset-0 grid pointer-events-none" style={{ gridTemplateRows: `repeat(${numRows}, 88px)` }}>
                                    {timeSlots.map((time) => (
                                        <div key={time} className="border-b border-outline-variant/10"></div>
                                    ))}
                                </div>

                                <div className="absolute inset-y-0 left-0 z-10 grid w-16" style={{ gridTemplateRows: `repeat(${numRows}, 88px)` }}>
                                    {timeSlots.map((time) => (
                                        <div key={time} className="flex justify-center pt-2 font-body text-[11px] font-medium text-outline">
                                            {time}
                                        </div>
                                    ))}
                                </div>

                                <div className="absolute inset-y-0 left-16 right-0 grid grid-cols-7">
                                    {weekDays.map((date) => (
                                        <div key={date.toISOString()} className="relative">
                                            {renderEventsForDay(date)}
                                        </div>
                                    ))}
                                </div>

                                {!isLoading && currentWeekEvents.length === 0 && (
                                    <div className="absolute inset-0 left-16 flex items-center justify-center">
                                        <div className="rounded-lg border border-dashed border-outline-variant bg-surface/90 px-6 py-5 text-center shadow-sm">
                                            <p className="font-headline font-bold text-on-surface">Chưa có buổi học trong tuần này</p>
                                            <p className="mt-1 font-body text-sm text-on-surface-variant">Tạo lịch mới hoặc chuyển sang tuần khác để xem dữ liệu.</p>
                                        </div>
                                    </div>
                                )}
                            </div>
                        </div>
                    </div>
                </div>

                <aside className="rounded-lg border border-outline-variant/30 bg-surface-container-lowest p-4 shadow-sm">
                    <div className="mb-4 flex items-center justify-between gap-3">
                        <h3 className="font-headline text-lg font-bold text-on-surface">Buổi học trong tuần</h3>
                        <span className="rounded-full bg-surface-container-high px-3 py-1 font-label text-xs font-bold text-on-surface-variant">
                            {currentWeekEvents.length}
                        </span>
                    </div>
                    {currentWeekEvents.length === 0 ? (
                        <p className="rounded-lg bg-surface-container-low p-4 font-body text-sm text-on-surface-variant">Không có lịch học để hiển thị.</p>
                    ) : (
                        <div className="max-h-[680px] space-y-3 overflow-y-auto pr-1 calendar-scroll">
                            {currentWeekEvents.map((event) => {
                                const expectedDate = toDate(event.expectedDate);
                                return (
                                    <button
                                        key={event.id}
                                        type="button"
                                        onClick={() => navigate(`/schedules/${event.id}`)}
                                        className="w-full rounded-lg border border-surface-container-highest bg-surface-container-low p-3 text-left transition-colors hover:bg-surface-container-high"
                                    >
                                        <div className="flex items-start justify-between gap-3">
                                            <div className="min-w-0">
                                                <p className="truncate font-label text-sm font-bold text-on-surface">{event.topicName || event.courseName}</p>
                                                <p className="mt-1 truncate font-body text-xs text-on-surface-variant">{event.courseName}</p>
                                            </div>
                                            <span className="shrink-0 rounded bg-surface px-2 py-1 font-label text-[10px] font-bold text-on-surface-variant">
                                                {expectedDate ? format(expectedDate, 'dd/MM') : '--/--'}
                                            </span>
                                        </div>
                                        <div className="mt-3 flex items-center justify-between gap-3 font-body text-xs text-on-surface-variant">
                                            <span>{formatTime(event.expectedStart || event.expectedDate)} - {formatTime(event.expectedEnd)}</span>
                                            <span className="truncate">{event.teacherName || 'Chưa phân công'}</span>
                                        </div>
                                    </button>
                                );
                            })}
                        </div>
                    )}
                </aside>
            </div>

            <style jsx="true">{`
                .calendar-scroll::-webkit-scrollbar {
                    width: 6px;
                    height: 6px;
                }
                .calendar-scroll::-webkit-scrollbar-track {
                    background: transparent;
                }
                .calendar-scroll::-webkit-scrollbar-thumb {
                    background-color: #cbd5e1;
                    border-radius: 20px;
                }
                .calendar-scroll:hover::-webkit-scrollbar-thumb {
                    background-color: #94a3b8;
                }
            `}</style>
        </div>
    );
};

export default ScheduleManagementUI;
