import { getHeaders } from './httpClient';

const API_BASE_URL = '/api/v1/student';

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

const normalizeCertificate = (certificate) => ({
    maID: getValue(certificate, 'maID', 'MaID'),
    chungchiID: getValue(certificate, 'chungchiID', 'ChungchiID'),
    tenChungchi: getValue(certificate, 'tenChungchi', 'TenChungchi', 'tenchungchi'),
    mota: getValue(certificate, 'mota', 'Mota'),
    donvicap: getValue(certificate, 'donvicap', 'Donvicap'),
    ngaycap: getValue(certificate, 'ngaycap', 'Ngaycap'),
    ngayhethan: getValue(certificate, 'ngayhethan', 'Ngayhethan')
});

const normalizeStudent = (student) => {
    const hocvienID = getValue(student, 'hocvienID', 'HocvienID', 'maID', 'MaID');

    return {
        hocvienID,
        maID: hocvienID,
        hoten: getValue(student, 'hoten', 'Hoten'),
        ngaysinh: getValue(student, 'ngaysinh', 'Ngaysinh'),
        gioitinh: getValue(student, 'gioitinh', 'Gioitinh'),
        email: getValue(student, 'email', 'Email'),
        diachi: getValue(student, 'diachi', 'Diachi'),
        dienthoai: getValue(student, 'dienthoai', 'Dienthoai'),
        certificates: (getValue(student, 'certificates', 'Certificates', 'studentCertificate') || []).map(normalizeCertificate)
    };
};

const toApiPayload = (payload, id) => ({
    hocvienID: id || payload.hocvienID || payload.maID || null,
    hoten: payload.hoten || '',
    ngaysinh: payload.ngaysinh || null,
    gioitinh: payload.gioitinh || null,
    email: payload.email || '',
    diachi: payload.diachi || null,
    dienthoai: payload.dienthoai || ''
});

export const getStudents = async () => {
    const response = await fetch(API_BASE_URL, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch students'));

    const data = unwrapServiceResponse(payload) || [];
    return { data: Array.isArray(data) ? data.map(normalizeStudent) : [] };
};

export const getStudentDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch student detail'));

    const data = unwrapServiceResponse(payload);
    return { data: data ? normalizeStudent(data) : null };
};

export const getStudentMe = async () => {
    const response = await fetch(`${API_BASE_URL}/me`, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch current student'));

    const data = unwrapServiceResponse(payload);
    return { data: data ? normalizeStudent(data) : null };
};

export const getStudentMeSchedules = async () => {
    const response = await fetch(`${API_BASE_URL}/me/schedules`, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch student schedules'));

    const data = unwrapServiceResponse(payload) || [];
    return { data: Array.isArray(data) ? data : [] };
};

export const getStudentMeScheduleDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/me/schedule/${id}`, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch student schedule detail'));

    return { data: unwrapServiceResponse(payload) };
};

export const studentCheckinSchedule = async (id) => {
    const response = await fetch(`${API_BASE_URL}/me/schedule/${id}/checkin`, {
        method: 'POST',
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to check in schedule'));

    return { data: unwrapServiceResponse(payload), message: getApiMessage(payload, '\u0110i\u1ec3m danh th\u00e0nh c\u00f4ng') };
};

export const createStudent = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify(toApiPayload(payload))
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to create student'));

    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const updateStudent = async (id, payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify(toApiPayload(payload, id))
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to update student'));

    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const deleteStudent = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to delete student'));

    return { data: unwrapServiceResponse(data), message: data?.message };
};
