import StudentWorkspaceUI from '../components/StudentWorkspace/StudentWorkspaceUI';
import { useStudentWorkspace } from '../hooks/useStudentWorkspace';

const StudentWorkspacePage = () => {
    const {
        student,
        schedules,
        exams,
        selectedExam,
        isLoading,
        isExamLoading,
        error,
        successMessage,
        actionId,
        examActionId,
        refetch,
        checkin,
        openExam,
        closeExam,
        submitExam
    } = useStudentWorkspace();

    return (
        <StudentWorkspaceUI
            student={student}
            schedules={schedules}
            exams={exams}
            selectedExam={selectedExam}
            isLoading={isLoading}
            isExamLoading={isExamLoading}
            error={error}
            successMessage={successMessage}
            actionId={actionId}
            examActionId={examActionId}
            onRefresh={refetch}
            onCheckin={checkin}
            onOpenExam={openExam}
            onCloseExam={closeExam}
            onSubmitExam={submitExam}
        />
    );
};

export default StudentWorkspacePage;
