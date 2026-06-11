import { AlertCircle, ArrowLeft, CheckCircle2, Loader2, Plus } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

const getCourseName = (course) => course.ten || 'Khóa học chưa có tên';
const getStudentName = (student) => student.hoten || 'Học viên chưa có tên';

const ExamCreateUI = ({
    courses,
    students,
    formData,
    isLoadingLookups,
    isSaving,
    lookupError,
    submitError,
    successMessage,
    onChange,
    onSubmit
}) => {
    const navigate = useNavigate();

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-6 lg:p-12">
            <div className="mb-8 flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
                <div className="flex items-center gap-4">
                    <button
                        type="button"
                        onClick={() => navigate('/reports')}
                        className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                        title="Quay lại"
                    >
                        <ArrowLeft size={20} />
                    </button>
                    <div>
                        <h1 className="font-headline text-2xl lg:text-3xl font-extrabold text-on-surface tracking-tight m-0">
                            Thêm bài thu hoạch
                        </h1>
                        <p className="font-body text-sm text-on-surface-variant">
                            Chọn khóa học và học viên để tạo bài thu hoạch mới.
                        </p>
                    </div>
                </div>
            </div>

            <form onSubmit={onSubmit} className="max-w-4xl w-full flex flex-col gap-6">
                {(lookupError || submitError || successMessage) && (
                    <div
                        className={`rounded-lg border px-4 py-3 font-body text-sm flex items-start gap-3 ${
                            successMessage
                                ? 'border-emerald-200 bg-emerald-50 text-emerald-700'
                                : 'border-error-container bg-error-container/60 text-error'
                        }`}
                    >
                        {successMessage ? <CheckCircle2 size={18} /> : <AlertCircle size={18} />}
                        <span>{successMessage || submitError || lookupError}</span>
                    </div>
                )}

                <section className="bg-surface-container-lowest rounded-lg p-6 lg:p-8 shadow-sm border border-surface-container">
                    <h2 className="font-headline text-xl font-bold text-on-surface mb-6">
                        Thông tin bài thu hoạch
                    </h2>

                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="khoahocID">
                                Tên khóa học *
                            </label>
                            <select
                                id="khoahocID"
                                name="khoahocID"
                                required
                                disabled={isLoadingLookups}
                                value={formData.khoahocID}
                                onChange={onChange}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all disabled:opacity-70 disabled:cursor-not-allowed"
                            >
                                <option value="">Chọn khóa học</option>
                                {courses.map((course) => (
                                    <option key={course.khoahocID} value={course.khoahocID}>
                                        {getCourseName(course)}
                                    </option>
                                ))}
                            </select>
                        </div>

                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="hocvienID">
                                Tên học viên *
                            </label>
                            <select
                                id="hocvienID"
                                name="hocvienID"
                                required
                                disabled={isLoadingLookups}
                                value={formData.hocvienID}
                                onChange={onChange}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all disabled:opacity-70 disabled:cursor-not-allowed"
                            >
                                <option value="">Chọn học viên</option>
                                {students.map((student) => (
                                    <option key={student.hocvienID} value={student.hocvienID}>
                                        {getStudentName(student)}
                                    </option>
                                ))}
                            </select>
                        </div>
                    </div>
                </section>

                <div className="flex flex-col-reverse sm:flex-row justify-end gap-3 pb-12 pt-2">
                    <button
                        type="button"
                        onClick={() => navigate('/reports')}
                        disabled={isSaving}
                        className="px-6 py-3 rounded-lg font-label font-bold text-sm text-on-surface-variant hover:bg-surface-container-highest transition-colors disabled:opacity-60"
                    >
                        Hủy
                    </button>
                    <button
                        type="submit"
                        disabled={isSaving || isLoadingLookups}
                        className="px-8 py-3 rounded-lg font-label font-bold text-sm text-on-primary bg-primary shadow-md hover:-translate-y-0.5 transition-all disabled:opacity-70 disabled:cursor-not-allowed flex items-center justify-center gap-2"
                    >
                        {isSaving ? <Loader2 size={18} className="animate-spin" /> : <Plus size={18} />}
                        {isSaving ? 'Đang tạo...' : 'Tạo bài thu hoạch'}
                    </button>
                </div>
            </form>
        </div>
    );
};

export default ExamCreateUI;
