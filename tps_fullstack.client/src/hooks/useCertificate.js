import { useCallback, useEffect, useState } from 'react';
import {
    deleteCertificate,
    getCertificateDetail,
    getCertificates,
    issueCertificate,
    revokeCertificate
} from '../services/certificateService';

export const useCertificate = () => {
    const [certificates, setCertificates] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchCertificates = useCallback(async () => {
        setIsLoading(true);
        setError(null);

        try {
            const response = await getCertificates();
            setCertificates(Array.isArray(response.data) ? response.data : []);
        } catch (err) {
            setError('Lỗi khi tải danh sách chứng chỉ.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
        fetchCertificates();
    }, [fetchCertificates]);

    const handleDeleteCertificate = async (id) => {
        try {
            await deleteCertificate(id);
            setCertificates((prev) => prev.filter((certificate) => certificate.maID !== id));
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    return {
        certificates,
        isLoading,
        error,
        fetchCertificates,
        handleDeleteCertificate
    };
};

export const useCertificateDetail = (id) => {
    const [detail, setDetail] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchDetail = useCallback(async () => {
        if (!id) return;

        setIsLoading(true);
        setError(null);

        try {
            const response = await getCertificateDetail(id);
            setDetail(response.data || null);
        } catch (err) {
            setError('Lỗi khi tải thông tin chứng chỉ.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, [id]);

    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
        fetchDetail();
    }, [fetchDetail]);

    const handleIssueCertificate = async (studentId) => {
        if (!detail?.certificateInfo || !studentId) return false;

        try {
            await issueCertificate({
                certificate: detail.certificateInfo,
                studentId
            });
            await fetchDetail();
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleRevokeCertificate = async (issuedCertificateId) => {
        try {
            await revokeCertificate(issuedCertificateId);
            await fetchDetail();
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    return {
        detail,
        isLoading,
        error,
        fetchDetail,
        handleIssueCertificate,
        handleRevokeCertificate
    };
};
