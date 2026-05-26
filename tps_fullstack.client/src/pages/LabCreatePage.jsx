import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import LabCreateUI from '../components/LabManagement/LabCreateUI';
import { createLab } from '../services/labService';

const LabCreatePage = () => {
    const navigate = useNavigate();
    const [isSaving, setIsSaving] = useState(false);

    const handleCreateLab = async (payload) => {
        setIsSaving(true);
        try {
            await createLab(payload);
            alert('Tạo buổi thực hành thành công!');
            navigate('/schedules');
        } catch (error) {
            alert(`Có lỗi xảy ra: ${error.message}`);
        } finally {
            setIsSaving(false);
        }
    };

    return <LabCreateUI onCreateLab={handleCreateLab} isSaving={isSaving} />;
};

export default LabCreatePage;
