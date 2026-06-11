import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import ExamCreateUI from '../components/ExamManagement/ExamCreateUI';
import { getCourses } from '../services/courseService';
import { createExam } from '../services/examService';
import { getStudents } from '../services/studentService';

const ExamCreatePage = () => {
    const navigate = useNavigate();
    const [courses, setCourses] = useState([]);
    const [students, setStudents] = useState([]);
    const [formData, setFormData] = useState({ khoahocID: '', hocvienID: '' });
    const [isLoadingLookups, setIsLoadingLookups] = useState(true);
    const [isSaving, setIsSaving] = useState(false);
    const [lookupError, setLookupError] = useState('');
    const [submitError, setSubmitError] = useState('');
    const [successMessage, setSuccessMessage] = useState('');

    useEffect(() => {
        let isMounted = true;

        const loadLookups = async () => {
            setIsLoadingLookups(true);
            setLookupError('');

            try {
                const [courseResponse, studentResponse] = await Promise.all([
                    getCourses(),
                    getStudents()
                ]);

                if (!isMounted) return;
                setCourses(Array.isArray(courseResponse.data) ? courseResponse.data : []);
                setStudents(Array.isArray(studentResponse.data) ? studentResponse.data : []);
            } catch (error) {
                if (!isMounted) return;
                console.error(error);
                setLookupError('Không tải được danh sách khóa học hoặc học viên. Vui lòng tải lại trang và thử lại.');
            } finally {
                if (isMounted) setIsLoadingLookups(false);
            }
        };

        loadLookups();

        return () => {
            isMounted = false;
        };
    }, []);

    const handleChange = (event) => {
        const { name, value } = event.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
        setSubmitError('');
        setSuccessMessage('');
    };

    const handleSubmit = async (event) => {
        event.preventDefault();

        const khoahocID = formData.khoahocID.trim();
        const hocvienID = formData.hocvienID.trim();

        if (!khoahocID || !hocvienID) {
            setSubmitError('Vui lòng chọn đầy đủ khóa học và học viên.');
            return;
        }

        setIsSaving(true);
        setSubmitError('');
        setSuccessMessage('');

        try {
            const response = await createExam({ khoahocID, hocvienID });
            setSuccessMessage(response.message || 'Tạo bài thu hoạch thành công.');
            window.alert(response.message || 'Tạo bài thu hoạch thành công.');
            navigate('/reports');
        } catch (error) {
            console.error(error);
            setSubmitError(error.message || 'Không thể tạo bài thu hoạch. Vui lòng thử lại.');
        } finally {
            setIsSaving(false);
        }
    };

    return (
        <ExamCreateUI
            courses={courses}
            students={students}
            formData={formData}
            isLoadingLookups={isLoadingLookups}
            isSaving={isSaving}
            lookupError={lookupError}
            submitError={submitError}
            successMessage={successMessage}
            onChange={handleChange}
            onSubmit={handleSubmit}
        />
    );
};

export default ExamCreatePage;
