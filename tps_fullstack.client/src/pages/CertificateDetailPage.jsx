import { useParams } from 'react-router-dom';
import CertificateDetailUI from '../components/CertificateManagement/CertificateDetailUI';
import { useCertificateDetail } from '../hooks/useCertificate';

const CertificateDetailPage = () => {
    const { id } = useParams();
    const {
        detail,
        isLoading,
        error,
        handleIssueCertificate,
        handleRevokeCertificate
    } = useCertificateDetail(id);

    return (
        <CertificateDetailUI
            detail={detail}
            isLoading={isLoading}
            error={error}
            onIssueCertificate={handleIssueCertificate}
            onRevokeCertificate={handleRevokeCertificate}
        />
    );
};

export default CertificateDetailPage;
