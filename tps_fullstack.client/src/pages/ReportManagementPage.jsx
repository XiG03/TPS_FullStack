import ReportManagementUI from '../components/TopicManagement/ReportManagementUI';
import { useReport } from '../hooks/useReport';

const ReportManagementPage = () => {
    const {
        reports,
        selectedReport,
        isLoading,
        isLoadingDetail,
        error,
        detailError,
        handleViewReport,
        closeReportDetail
    } = useReport();

    return (
        <ReportManagementUI
            reports={reports}
            selectedReport={selectedReport}
            isLoading={isLoading}
            isLoadingDetail={isLoadingDetail}
            error={error}
            detailError={detailError}
            onViewReport={handleViewReport}
            onCloseDetail={closeReportDetail}
        />
    );
};

export default ReportManagementPage;
