import React from 'react';
import { format, isSameDay, getDay } from 'date-fns';
import { vi } from 'date-fns/locale';
import { useNavigate } from 'react-router-dom';

const ScheduleManagementUI = ({ 
    schedules, isLoading, error, onAddSchedule,
    weekDays, currentDate, nextWeek, prevWeek, goToday 
}) => {
    const navigate = useNavigate();

    // Lấy tên tháng năm cho Header
    const monthYearStr = format(currentDate, "'Tháng' M, yyyy", { locale: vi });

    // Hàm phụ trợ map tên thứ tiếng Việt
    const getVietnameseDay = (date) => {
        const dayMap = ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'];
        return dayMap[getDay(date)];
    };

    // Tìm khung giờ nhỏ nhất và lớn nhất dựa trên dữ liệu lịch học của tuần
    let minHour = 8; // Mặc định 08:00
    let maxHour = 17; // Mặc định 17:00

    const currentWeekEvents = schedules.filter(s => {
        const d = new Date(s.Batdaudukien);
        return weekDays.some(wd => isSameDay(wd, d));
    });

    if (currentWeekEvents.length > 0) {
        currentWeekEvents.forEach(s => {
            const startH = new Date(s.Batdaudukien).getHours();
            const endH = new Date(s.Ketthucdukien).getHours();
            if (startH < minHour) minHour = startH;
            // Nếu kết thúc vào giờ lẻ (ví dụ 18:30) thì làm tròn lên 19:00 để hiển thị grid đầy đủ
            const endMinutes = new Date(s.Ketthucdukien).getMinutes();
            const adjustedEnd = endMinutes > 0 ? endH + 1 : endH;
            if (adjustedEnd > maxHour) maxHour = adjustedEnd;
        });
    }

    // Đảm bảo có ít nhất 1 giờ đệm ở cuối
    maxHour = Math.min(23, maxHour + 1);

    const timeSlots = [];
    for (let i = minHour; i <= maxHour; i++) {
        timeSlots.push(`${i.toString().padStart(2, '0')}:00`);
    }
    const numRows = timeSlots.length;

    // Cập nhật hàm renderEventsForDay để tính top dựa trên minHour
    const renderEventsForDay = (date) => {
        const dayEvents = schedules.filter(s => {
            const eventDate = new Date(s.Batdaudukien);
            return isSameDay(eventDate, date);
        });

        return dayEvents.map(event => {
            const startDate = new Date(event.Batdaudukien);
            const endDate = new Date(event.Ketthucdukien);
            
            // Tính toán vị trí top (px) dựa trên minHour làm mốc 0px
            const startHourValue = startDate.getHours() + startDate.getMinutes() / 60;
            const topPx = (startHourValue - minHour) * 90;

            const durationHours = (endDate.getTime() - startDate.getTime()) / (1000 * 60 * 60);
            const heightPx = durationHours * 90;

            // Xác định màu sắc theo ID khóa học
            let bgColor = 'bg-[#002B7A]';
            let textColor = 'text-white';
            let tagBg = 'bg-white/20';
            let tagText = 'text-white/90';
            let subTextColor = 'text-blue-200';
            let borderColor = 'border-[#001f5c]';

            if (event.KhoahocID === 'C002') {
                bgColor = 'bg-[#E8F0FE]';
                textColor = 'text-[#1a3a6e]';
                tagBg = 'bg-[#d0e1fb]';
                tagText = 'text-[#00328a]';
                subTextColor = 'text-[#4a6b9c]';
                borderColor = 'border-blue-200/60';
            } else if (event.KhoahocID === 'C003') {
                bgColor = 'bg-[#FFF0EB]';
                textColor = 'text-[#6c1f00]';
                tagBg = 'bg-[#ffdbcf]';
                tagText = 'text-[#6c1f00]';
                subTextColor = 'text-[#8a4224]';
                borderColor = 'border-[#ffdbcf]';
            }

            return (
                <div 
                    key={event.MaID}
                    onClick={() => navigate(`/schedules/${event.MaID}`)}
                    className={`absolute left-1.5 right-1.5 rounded-xl p-3 flex flex-col justify-between shadow-sm z-10 hover:shadow-md transition-all cursor-pointer group border ${bgColor} ${textColor} ${borderColor}`}
                    style={{ top: `${topPx}px`, height: `${heightPx}px` }}
                >
                    <div>
                        <div className="flex items-center justify-between mb-1.5">
                            <span className={`inline-block px-1.5 py-0.5 rounded text-[9px] font-manrope font-bold uppercase tracking-wider ${tagBg} ${tagText}`}>
                                {event.KhoahocID}
                            </span>
                        </div>
                        <h4 className="text-[13px] font-manrope font-bold leading-tight mb-1 truncate">
                            {event.TenKhoahoc}
                        </h4>
                        <p className={`text-[10px] font-inter ${subTextColor}`}>
                            {format(startDate, 'HH:mm')} - {format(endDate, 'HH:mm')}
                        </p>
                    </div>
                </div>
            );
        });
    };

    // Không còn cứng 10 timeSlots nữa
    return (
        <div className="pt-8 px-10 pb-20 mx-auto max-w-full">
            {/* Header Section */}
            <div className="flex flex-col xl:flex-row xl:items-end justify-between mb-10 gap-6">
                <div>
                    <h2 className="text-3xl font-headline font-extrabold text-on-surface tracking-tight mb-2">
                        Lịch Đào tạo Tổng thể
                    </h2>
                    <p className="text-on-surface-variant font-body text-sm">
                        {monthYearStr} • Quản lý vận hành giảng dạy
                    </p>
                </div>
                <div className="flex items-center gap-4 flex-wrap">
                    {/* Navigation */}
                    <div className="flex items-center bg-surface-container-lowest border border-outline-variant/30 rounded-xl p-1 shadow-sm">
                        <button onClick={prevWeek} className="p-1.5 hover:bg-surface-container-low rounded-lg transition-all text-on-surface-variant hover:text-on-surface">
                            <span className="material-symbols-outlined text-[20px]">chevron_left</span>
                        </button>
                        <button onClick={goToday} className="px-4 py-1.5 font-headline font-bold text-sm text-[#0047BB] hover:bg-blue-50/50 rounded-lg transition-all">
                            Hôm nay
                        </button>
                        <button onClick={nextWeek} className="p-1.5 hover:bg-surface-container-low rounded-lg transition-all text-on-surface-variant hover:text-on-surface">
                            <span className="material-symbols-outlined text-[20px]">chevron_right</span>
                        </button>
                    </div>
                    {/* View Switcher */}
                    <div className="flex items-center bg-surface-container-lowest border border-outline-variant/30 rounded-xl p-1 shadow-sm">
                        <button className="px-4 py-1.5 font-headline font-semibold text-sm text-on-surface-variant hover:text-on-surface hover:bg-surface-container-low rounded-lg transition-all">Ngày</button>
                        <button className="px-4 py-1.5 font-headline font-bold text-sm bg-surface-container-low text-[#0047BB] rounded-lg shadow-sm border border-outline-variant/20 transition-all">Tuần</button>
                        <button className="px-4 py-1.5 font-headline font-semibold text-sm text-on-surface-variant hover:text-on-surface hover:bg-surface-container-low rounded-lg transition-all">Tháng</button>
                    </div>
                    <button 
                        onClick={onAddSchedule}
                        className="bg-[#0047BB] hover:bg-[#00328a] text-white px-5 py-2.5 rounded-xl font-headline font-bold text-sm flex items-center space-x-2 transition-all shadow-sm"
                    >
                        <span className="material-symbols-outlined text-[18px]">add</span>
                        <span>Tạo lịch mới</span>
                    </button>
                </div>
            </div>

            {/* Stat Cards */}
            <div className="flex flex-wrap gap-4 mb-8">
                <div className="flex-1 min-w-[240px] bg-surface-container-lowest p-4 rounded-2xl shadow-sm border border-outline-variant/30 flex items-center gap-4">
                    <div className="w-10 h-10 rounded-xl bg-blue-50 text-[#0047BB] flex items-center justify-center">
                        <span className="material-symbols-outlined">person_pin</span>
                    </div>
                    <div>
                        <p className="text-[10px] font-headline font-extrabold text-outline uppercase tracking-wider">Giảng viên bận</p>
                        <div className="flex items-baseline gap-2">
                            <span className="text-xl font-headline font-extrabold text-on-surface">12</span>
                            <span className="text-[10px] font-semibold text-outline">/ 45 Tổng số</span>
                            <span className="text-[9px] text-green-600 font-bold ml-2">+2 (Hôm nay)</span>
                        </div>
                    </div>
                </div>
                <div className="flex-1 min-w-[240px] bg-surface-container-lowest p-4 rounded-2xl shadow-sm border border-outline-variant/30 flex items-center gap-4">
                    <div className="w-10 h-10 rounded-xl bg-blue-50 text-[#0047BB] flex items-center justify-center">
                        <span className="material-symbols-outlined">meeting_room</span>
                    </div>
                    <div className="flex-1">
                        <p className="text-[10px] font-headline font-extrabold text-outline uppercase tracking-wider">Phòng học trống</p>
                        <div className="flex items-center justify-between mb-1.5">
                            <span className="text-xl font-headline font-extrabold text-on-surface">
                                08 <span className="text-[10px] text-outline">/ 20</span>
                            </span>
                            <span className="text-[10px] font-bold text-[#0047BB]">40% khả dụng</span>
                        </div>
                        <div className="w-full bg-surface-container-low h-1.5 rounded-full overflow-hidden">
                            <div className="bg-[#0047BB] h-full w-[40%] rounded-full"></div>
                        </div>
                    </div>
                </div>
            </div>

            {isLoading && <div className="text-center font-body text-on-surface-variant mb-4">Đang tải dữ liệu...</div>}
            {error && <div className="text-center font-body text-error mb-4">{error}</div>}

            {/* Calendar Custom Grid */}
            <div className="flex-1 bg-surface-container-lowest rounded-2xl overflow-hidden flex flex-col border border-outline-variant/30 shadow-sm relative">
                {/* Header (Thứ) */}
                <div className="grid grid-cols-[60px_1fr_1fr_1fr_1fr_1fr_1fr_1fr] border-b border-outline-variant/30 bg-surface-container-low/50 sticky top-0 z-20">
                    <div className="p-3 border-r border-outline-variant/20 flex items-end justify-center pb-2">
                        <span className="text-[10px] font-body font-medium text-outline">Giờ</span>
                    </div>
                    {weekDays && weekDays.map((date, idx) => {
                        const isToday = isSameDay(date, new Date());
                        return (
                            <div key={idx} className={`p-3 text-center ${idx < 6 ? 'border-r border-outline-variant/20' : ''} ${isToday ? 'bg-blue-50/50 border-b-2 border-b-[#0047BB]' : ''}`}>
                                <p className={`text-[11px] font-headline font-bold uppercase tracking-wider mb-1 ${isToday ? 'text-[#0047BB]' : 'text-outline'}`}>
                                    {getVietnameseDay(date)}
                                </p>
                                <p className={`text-xl font-headline font-extrabold ${isToday ? 'text-[#0047BB]' : 'text-on-surface'}`}>
                                    {format(date, 'dd')}
                                </p>
                            </div>
                        );
                    })}
                </div>

                {/* Grid Body */}
                <div className="flex-1 overflow-y-auto max-h-[700px] relative calendar-scroll bg-surface-container-lowest">
                    <div className="grid grid-cols-[60px_repeat(7,1fr)] relative" style={{ minHeight: `${numRows * 90}px` }}>
                        {/* Background Lines */}
                        <div className="absolute inset-0 grid grid-cols-[60px_repeat(7,1fr)] pointer-events-none">
                            <div className="border-r border-outline-variant/20 bg-surface-container-low/20"></div>
                            {weekDays && weekDays.map((date, idx) => (
                                <div key={idx} className={`border-r border-outline-variant/20 ${isSameDay(date, new Date()) ? 'bg-blue-50/10' : ''}`}></div>
                            ))}
                        </div>

                        {/* Horizontal Time Lines */}
                        <div className="absolute inset-0 grid pointer-events-none z-0" style={{ gridTemplateRows: `repeat(${numRows}, 90px)` }}>
                            {timeSlots.map((_, i) => (
                                <div key={i} className="border-b border-outline-variant/10"></div>
                            ))}
                        </div>

                        {/* Time Column (Dynamic) */}
                        <div className="relative z-10 grid" style={{ gridTemplateRows: `repeat(${numRows}, 90px)` }}>
                            {timeSlots.map((time, i) => (
                                <div key={i} className="flex justify-center pt-2 text-[11px] font-body font-medium text-outline">
                                    {time}
                                </div>
                            ))}
                        </div>

                        {/* Events Columns */}
                        {weekDays && weekDays.map((date, idx) => (
                            <div key={idx} className="relative">
                                {renderEventsForDay(date)}
                            </div>
                        ))}
                    </div>
                </div>
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
