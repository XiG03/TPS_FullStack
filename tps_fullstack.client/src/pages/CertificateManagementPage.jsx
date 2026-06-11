import CertificateManagementUI from '../components/CertificateManagement/CertificateManagementUI';
import { useCertificate } from '../hooks/useCertificate';

const CertificateManagementPage = () => {
    const { certificates, isLoading, error, handleDeleteCertificate } = useCertificate();

    const confirmDelete = async (id) => {
        if (!window.confirm('Bạn có chắc chắn muốn xóa chứng chỉ này? Các khóa học liên quan sẽ không còn dùng chứng chỉ này.')) return;

        const success = await handleDeleteCertificate(id);
        alert(success ? 'Đã xóa chứng chỉ thành công.' : 'Đã có lỗi xảy ra khi xóa chứng chỉ.');
    };

    return (
        <CertificateManagementUI
            certificates={certificates}
            isLoading={isLoading}
            error={error}
            onDeleteCertificate={confirmDelete}
        />
    );
};

export default CertificateManagementPage;
