import StudentWorkspaceUI from '../components/StudentWorkspace/StudentWorkspaceUI';
import { useStudentWorkspace } from '../hooks/useStudentWorkspace';

const StudentWorkspacePage = () => {
    const {
        student,
        schedules,
        isLoading,
        error,
        successMessage,
        actionId,
        refetch,
        checkin
    } = useStudentWorkspace();

    return (
        <StudentWorkspaceUI
            student={student}
            schedules={schedules}
            isLoading={isLoading}
            error={error}
            successMessage={successMessage}
            actionId={actionId}
            onRefresh={refetch}
            onCheckin={checkin}
        />
    );
};

export default StudentWorkspacePage;
