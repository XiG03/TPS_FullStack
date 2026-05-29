import TeacherWorkspaceUI from '../components/TeacherWorkspace/TeacherWorkspaceUI';
import { useTeacherWorkspace } from '../hooks/useTeacherWorkspace';

const TeacherWorkspacePage = () => {
    const {
        teacher,
        schedules,
        isLoading,
        error,
        actionId,
        refetch,
        checkin,
        checkout
    } = useTeacherWorkspace();

    return (
        <TeacherWorkspaceUI
            teacher={teacher}
            schedules={schedules}
            isLoading={isLoading}
            error={error}
            actionId={actionId}
            onRefresh={refetch}
            onCheckin={checkin}
            onCheckout={checkout}
        />
    );
};

export default TeacherWorkspacePage;
