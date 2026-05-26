import { useParams } from 'react-router-dom';
import { useStudentDetail } from '../hooks/useStudentDetail';
import StudentDetailUI from '../components/StudentManagement/StudentDetailUI';

const StudentDetailPage = () => {
    const { id } = useParams();
    const { detail, isLoading, error } = useStudentDetail(id);

    return (
        <StudentDetailUI 
            detail={detail} 
            isLoading={isLoading} 
            error={error} 
        />
    );
};

export default StudentDetailPage;
