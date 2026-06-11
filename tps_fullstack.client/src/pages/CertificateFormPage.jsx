import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CertificateFormUI from '../components/CertificateManagement/CertificateFormUI';
import { useCertificateDetail } from '../hooks/useCertificate';
import { createCertificate, updateCertificate } from '../services/certificateService';

const CertificateFormPage = () => {
    const { id } = useParams();
    const navigate = useNavigate();
    const { detail, isLoading, error } = useCertificateDetail(id);
    const [isSaving, setIsSaving] = useState(false);

    const handleSave = async (formData) => {
        setIsSaving(true);

        try {
            if (id) {
                await updateCertificate({ ...formData, maID: id });
            } else {
                await createCertificate(formData);
            }

            alert(id ? 'Đã cập nhật chứng chỉ.' : 'Đã tạo chứng chỉ.');
            navigate('/certificates');
        } catch (err) {
            console.error(err);
            alert('Không lưu được chứng chỉ. Vui lòng kiểm tra dữ liệu và API.');
        } finally {
            setIsSaving(false);
        }
    };

    return (
        <CertificateFormUI
            detail={id ? detail : null}
            isLoading={!!id && isLoading}
            error={id ? error : null}
            onSave={handleSave}
            isSaving={isSaving}
        />
    );
};

export default CertificateFormPage;
