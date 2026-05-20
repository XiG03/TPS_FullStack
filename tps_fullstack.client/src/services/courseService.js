const API_BASE_URL = '/api/admin/course';

export const getCourses = async () => {
    const response = await fetch(API_BASE_URL);
    if (!response.ok) throw new Error('Failed to fetch courses');
    const data = await response.json();
    return { data };
};

export const getCourseDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`);
    if (!response.ok) throw new Error('Failed to fetch course detail');
    const data = await response.json();
    return { data };
};

export const createCourse = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to create course');
    return { success: true };
};

export const updateCourse = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to update course');
    const data = await response.json();
    return { data };
};

export const deleteCourse = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE'
    });
    if (!response.ok) throw new Error('Failed to delete course');
    return { success: true };
};

export const updateStudentScore = async (payload) => {
    const response = await fetch(`${API_BASE_URL}/studentscore`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to update student score');
    const data = await response.json();
    return { data };
};

// Lookup APIs - these call the same topic/teacher/student endpoints
export const getAvailableTopics = async () => {
    const response = await fetch('/api/admin/topic/getall');
    if (!response.ok) throw new Error('Failed to fetch topics');
    const data = await response.json();
    return { data };
};

export const getAvailableTeachers = async () => {
    const response = await fetch('/api/admin/teacher/teachers');
    if (!response.ok) throw new Error('Failed to fetch teachers');
    const data = await response.json();
    return { data };
};

export const getAvailableStudents = async () => {
    const response = await fetch('/api/admin/student/students');
    if (!response.ok) throw new Error('Failed to fetch students');
    const data = await response.json();
    return { data };
};
