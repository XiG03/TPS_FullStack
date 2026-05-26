import { getHeaders } from './httpClient';
import { getTopics } from './topicService';
import { getTeachers } from './teacherService';
import { getStudents } from './studentService';

const API_BASE_URL = '/api/v1/course';

const readJson = async (response) => {
    const text = await response.text();
    return text ? JSON.parse(text) : null;
};

const getApiMessage = (payload, fallback) => {
    return payload?.message || payload?.Message || fallback;
};

const unwrapServiceResponse = (payload) => {
    if (
        payload &&
        typeof payload === 'object' &&
        Object.prototype.hasOwnProperty.call(payload, 'data') &&
        Object.prototype.hasOwnProperty.call(payload, 'statusCode')
    ) {
        return payload.data;
    }

    return payload;
};

const getValue = (source, ...keys) => {
    for (const key of keys) {
        if (source?.[key] !== undefined && source?.[key] !== null) return source[key];
    }
    return undefined;
};

const normalizeTopic = (topic) => ({
    chuyendeID: getValue(topic, 'chuyendeID', 'ChuyendeID', 'maID', 'MaID'),
    ten: getValue(topic, 'ten', 'Ten'),
    mota: getValue(topic, 'mota', 'Mota')
});

const normalizeTeacher = (teacher) => ({
    maID: getValue(teacher, 'maID', 'MaID', 'giangvienID', 'GiangvienID'),
    giangvienID: getValue(teacher, 'giangvienID', 'GiangvienID', 'maID', 'MaID'),
    hoten: getValue(teacher, 'hoten', 'Hoten'),
    email: getValue(teacher, 'email', 'Email'),
    dienthoai: getValue(teacher, 'dienthoai', 'Dienthoai')
});

const normalizeStudent = (student) => ({
    maID: getValue(student, 'maID', 'MaID', 'hocvienID', 'HocvienID'),
    hocvienID: getValue(student, 'hocvienID', 'HocvienID', 'maID', 'MaID'),
    hoten: getValue(student, 'hoten', 'Hoten'),
    email: getValue(student, 'email', 'Email'),
    dienthoai: getValue(student, 'dienthoai', 'Dienthoai')
});

const normalizeCourse = (course) => ({
    khoahocID: getValue(course, 'khoahocID', 'KhoahocID', 'maID', 'MaID'),
    ten: getValue(course, 'ten', 'Ten'),
    mota: getValue(course, 'mota', 'Mota')
});

const normalizeCourseDetail = (course) => {
    if (!course) return null;

    return {
        ...normalizeCourse(course),
        diemdat: getValue(course, 'diemdat', 'Diemdat'),
        chungchiID: getValue(course, 'chungchiID', 'ChungchiID'),
        thu: getValue(course, 'thu', 'Thu'),
        thoiluonghoc: getValue(course, 'thoiluonghoc', 'Thoiluonghoc'),
        batdaudukien: getValue(course, 'batdaudukien', 'Batdaudukien'),
        sobuoihoc: getValue(course, 'sobuoihoc', 'Sobuoihoc'),
        ngaybatdau: getValue(course, 'ngaybatdau', 'Ngaybatdau'),
        thoiluongthi: getValue(course, 'thoiluongthi', 'Thoiluongthi'),
        socauhoi: getValue(course, 'socauhoi', 'Socauhoi'),
        teachers: (getValue(course, 'teachers', 'Teachers') || []).map((teacher) => ({
            maID: getValue(teacher, 'maID', 'MaID'),
            giangvienID: getValue(teacher, 'giangvienID', 'GiangvienID')
        })),
        students: (getValue(course, 'students', 'Students') || []).map((student) => ({
            maID: getValue(student, 'maID', 'MaID'),
            hocvienID: getValue(student, 'hocvienID', 'HocvienID'),
            diem: getValue(student, 'diem', 'Diem'),
            dieuchinh: getValue(student, 'dieuchinh', 'Dieuchinh')
        })),
        topics: (getValue(course, 'topics', 'Topics') || []).map((topic) => ({
            maID: getValue(topic, 'maID', 'MaID'),
            chuyendeID: getValue(topic, 'chuyendeID', 'ChuyendeID'),
            socauhoi: getValue(topic, 'socauhoi', 'Socauhoi')
        }))
    };
};

export const getCourses = async () => {
    const response = await fetch(API_BASE_URL, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch courses'));
    const data = unwrapServiceResponse(payload) || [];
    return { data: Array.isArray(data) ? data.map(normalizeCourse) : [] };
};

export const getCourseDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch course detail'));
    return { data: normalizeCourseDetail(unwrapServiceResponse(payload)) };
};

export const createCourse = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to create course'));
    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const updateCourse = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to update course'));
    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const deleteCourse = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to delete course'));
    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const getAvailableTopics = async () => {
    const response = await getTopics();
    return {
        data: (response.data || []).map(normalizeTopic)
    };
};

export const getAvailableTeachers = async () => {
    const response = await getTeachers();
    return {
        data: (response.data || []).map(normalizeTeacher)
    };
};

export const getAvailableStudents = async () => {
    const response = await getStudents();
    return {
        data: (response.data || []).map(normalizeStudent)
    };
};
