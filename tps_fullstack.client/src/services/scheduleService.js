const API_BASE_URL = '/api/admin/schedule';

export const getSchedules = async () => {
    const response = await fetch(API_BASE_URL);
    if (!response.ok) throw new Error('Failed to fetch schedules');
    const data = await response.json();
    return { data };
};

export const getScheduleDetail = async (id) => {
    // Note: The ScheduleController doesn't have a GetById endpoint.
    // If you need this, add a [HttpGet("{MaID}")] action in ScheduleController.cs
    throw new Error('Schedule detail endpoint not yet implemented on backend');
};

export const createSchedule = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to create schedule');
    const data = await response.json();
    return { data };
};

export const updateSchedule = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to update schedule');
    const data = await response.json();
    return { data };
};

export const deleteSchedule = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE'
    });
    if (!response.ok) throw new Error('Failed to delete schedule');
    const data = await response.json();
    return { data };
};

export const saveTeacherAttendance = async (attendanceData) => {
    const response = await fetch(`${API_BASE_URL}/teacherattendance`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(attendanceData)
    });
    if (!response.ok) throw new Error('Failed to save teacher attendance');
    const data = await response.json();
    return { data };
};

export const saveStudentAttendance = async (attendanceData) => {
    const response = await fetch(`${API_BASE_URL}/studentattendance`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(attendanceData)
    });
    if (!response.ok) throw new Error('Failed to save student attendance');
    const data = await response.json();
    return { data };
};

// Backward-compatible alias for components using the old name
export const saveAttendance = saveTeacherAttendance;
