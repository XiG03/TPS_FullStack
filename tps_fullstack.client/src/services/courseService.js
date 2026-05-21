import { getHeaders } from './httpClient';

const API_BASE_URL = '/api/admin/course';

export const getCourses = async () => {
    const response = await fetch(API_BASE_URL, {
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to fetch courses');
    const responseBody = await response.json();
    return { data: responseBody.data ?? responseBody.Data ?? responseBody };
};

export const getCourseDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to fetch course detail');
    const responseBody = await response.json();
    return { data: responseBody.data ?? responseBody.Data ?? responseBody };
};

export const createCourse = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to create course');
    return { success: true };
};

export const updateCourse = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to update course');
    const data = await response.json();
    return { data };
};

export const deleteCourse = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to delete course');
    return { success: true };
};

export const updateStudentScore = async (payload) => {
    const response = await fetch(`${API_BASE_URL}/studentscore`, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to update student score');
    const data = await response.json();
    return { data };
};

// Lookup APIs - these call the same topic/teacher/student endpoints
export const getAvailableTopics = async () => {
    const response = await fetch('/api/admin/topic/getall', {
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to fetch topics');
    const data = await response.json();
    return { data };
};

export const getAvailableTeachers = async () => {
    const response = await fetch('/api/admin/teacher/teachers', {
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to fetch teachers');
    const data = await response.json();
    return { data };
};

export const getAvailableStudents = async () => {
    const response = await fetch('/api/admin/student/students', {
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to fetch students');
    const data = await response.json();
    return { data };
};
