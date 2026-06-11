import { getHeaders } from './httpClient';
import { getCourses } from './courseService';
import { getStudents } from './studentService';

const API_BASE_URL = '/api/v1/certificate';

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

    if (
        payload &&
        typeof payload === 'object' &&
        Object.prototype.hasOwnProperty.call(payload, 'Data') &&
        Object.prototype.hasOwnProperty.call(payload, 'statusCode')
    ) {
        return payload.Data;
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
    ten: getValue(certificate, 'ten', 'Ten'),
    mota: getValue(certificate, 'mota', 'Mota'),
    thoigiansudung: getValue(certificate, 'thoigiansudung', 'Thoigiansudung') ?? 0,
    donvicap: getValue(certificate, 'donvicap', 'Donvicap') || ''
});

const normalizeCertificateCourse = (course) => ({
    khoahocID: getValue(course, 'khoahocID', 'KhoahocID', 'maID', 'MaID'),
    ten: getValue(course, 'ten', 'Ten')
});

const normalizeCertificateStudent = (student) => ({
    maID: getValue(student, 'maID', 'MaID'),
    chungchiID: getValue(student, 'chungchiID', 'ChungchiID'),
    hocvienID: getValue(student, 'hocvienID', 'HocvienID'),
    hocvienTen: getValue(student, 'hocvienTen', 'HocvienTen', 'hoten', 'Hoten'),
    ngaycap: getValue(student, 'ngaycap', 'Ngaycap'),
    ngayhethan: getValue(student, 'ngayhethan', 'Ngayhethan')
});

const normalizeCertificateDetail = (detail) => {
    if (!detail) return null;

    const certificateInfo = getValue(detail, 'certificateInfo', 'CertificateInfo') || detail;

    return {
        certificateInfo: normalizeCertificate(certificateInfo),
        certificateCourses: (getValue(detail, 'certificateCourses', 'CertificateCourses') || []).map(normalizeCertificateCourse),
        certificateStudents: (getValue(detail, 'certificateStudents', 'CertificateStudents') || []).map(normalizeCertificateStudent)
    };
};

const toNumberOrZero = (value) => {
    const numberValue = Number(value);
    return Number.isNaN(numberValue) ? 0 : numberValue;
};

const toCertificatePayload = (formData) => ({
    certificateInfo: {
        MaID: formData.maID || '',
        Ten: formData.ten?.trim() || '',
        Mota: formData.mota?.trim() || '',
        Donvicap: formData.donvicap?.trim() || '',
        Thoigiansudung: toNumberOrZero(formData.thoigiansudung)
    },
    certificateCourses: (formData.courseIds || [])
        .filter(Boolean)
        .map((courseId) => ({ KhoahocID: courseId }))
});

export const getCertificates = async () => {
    const response = await fetch(API_BASE_URL, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch certificates'));

    const data = unwrapServiceResponse(payload) || [];
    return { data: Array.isArray(data) ? data.map(normalizeCertificate) : [] };
};

export const getCertificateDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch certificate detail'));

    return { data: normalizeCertificateDetail(unwrapServiceResponse(payload)) };
};

export const createCertificate = async (formData) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify(toCertificatePayload(formData))
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to create certificate'));

    return { data: unwrapServiceResponse(payload), message: getApiMessage(payload, 'Tao chung chi thanh cong') };
};

export const updateCertificate = async (formData) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify(toCertificatePayload(formData))
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to update certificate'));

    return { data: unwrapServiceResponse(payload), message: getApiMessage(payload, 'Cap nhat chung chi thanh cong') };
};

export const deleteCertificate = async (id) => {
    if (!id) throw new Error('Missing certificate id');

    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to delete certificate'));

    return { data: unwrapServiceResponse(payload), message: getApiMessage(payload, 'Xoa chung chi thanh cong') };
};

export const issueCertificate = async ({ certificate, studentId }) => {
    if (!certificate?.maID || !studentId) throw new Error('Missing certificate or student id');

    const response = await fetch(`${API_BASE_URL}/accept`, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify({
            MaID: '',
            ChungchiID: certificate.maID,
            HocvienID: studentId,
            Ten: certificate.ten,
            Mota: certificate.mota,
            Donvicap: certificate.donvicap,
            Thoigiansudung: toNumberOrZero(certificate.thoigiansudung)
        })
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to issue certificate'));

    return { data: unwrapServiceResponse(payload), message: getApiMessage(payload, 'Cap chung chi thanh cong') };
};

export const revokeCertificate = async (issuedCertificateId) => {
    if (!issuedCertificateId) throw new Error('Missing issued certificate id');

    const response = await fetch(`${API_BASE_URL}/student/${issuedCertificateId}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to revoke certificate'));

    return { data: unwrapServiceResponse(payload), message: getApiMessage(payload, 'Thu hoi chung chi thanh cong') };
};

export const getAvailableCertificateCourses = async () => {
    const response = await getCourses();
    return { data: response.data || [] };
};

export const getAvailableCertificateStudents = async () => {
    const response = await getStudents();
    return { data: response.data || [] };
};
