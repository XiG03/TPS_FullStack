import { getToken } from '../services/httpClient';

const ROLE_CLAIM = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';

export const decodeTokenPayload = (token) => {
    try {
        const base64Payload = token.split('.')[1]?.replace(/-/g, '+').replace(/_/g, '/');
        return base64Payload ? JSON.parse(atob(base64Payload)) : {};
    } catch (err) {
        console.error(err);
        return {};
    }
};

export const getRoleFromPayload = (payload) => {
    const role = payload?.role || payload?.Role || payload?.[ROLE_CLAIM];
    return Array.isArray(role) ? role[0] : role;
};

export const normalizeRole = (role) => {
    const normalized = String(role || '').trim().toLowerCase();

    if (['admin', 'administrator'].includes(normalized)) return 'admin';
    if (['teacher', 'giangvien', 'giang vien', 'lecturer'].includes(normalized)) return 'teacher';
    if (['student', 'hocvien', 'hoc vien', 'learner'].includes(normalized)) return 'student';

    return normalized;
};

export const getCurrentUserRole = () => {
    const token = getToken();
    if (!token) return null;

    return normalizeRole(getRoleFromPayload(decodeTokenPayload(token)));
};

export const getDefaultPathForRole = (role) => {
    const normalized = normalizeRole(role);

    if (normalized === 'teacher') return '/teacher/me';
    if (normalized === 'student') return '/student/me';

    return '/';
};

export const hasAnyRole = (role, allowedRoles = []) => {
    const normalized = normalizeRole(role);
    if (normalized === 'admin') return true;

    return allowedRoles.map(normalizeRole).includes(normalized);
};
