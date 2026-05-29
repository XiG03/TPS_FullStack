import StudentWorkspaceUI from '../components/StudentWorkspace/StudentWorkspaceUI';
import { useStudentWorkspace } from '../hooks/useStudentWorkspace';

const StudentWorkspacePage = () => {
    const {
        student,
        schedules,
        isLoading,
        error,
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
            actionId={actionId}
            onRefresh={refetch}
            onCheckin={checkin}
        />
    );
};

export default StudentWorkspacePage;
