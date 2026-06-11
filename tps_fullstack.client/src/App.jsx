import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import AdminLayout from './components/Layout/AdminLayout';
import TopicManagementPage from './pages/TopicManagementPage';
import TopicFormPage from './pages/TopicFormPage';
import TopicDetailPage from './pages/TopicDetailPage';
import CourseManagementPage from './pages/CourseManagementPage';
import CourseFormPage from './pages/CourseFormPage';
import CourseDetailPage from './pages/CourseDetailPage';
import ScheduleManagementPage from './pages/ScheduleManagementPage';
import ScheduleDetailPage from './pages/ScheduleDetailPage';
import StudentManagementPage from './pages/StudentManagementPage';
import StudentDetailPage from './pages/StudentDetailPage';
import StudentWorkspacePage from './pages/StudentWorkspacePage';
import TeacherManagementPage from './pages/TeacherManagementPage';
import TeacherDetailPage from './pages/TeacherDetailPage';
import TeacherWorkspacePage from './pages/TeacherWorkspacePage';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import ReportManagementPage from './pages/ReportManagementPage';
import ExamCreatePage from './pages/ExamCreatePage';
import LabCreatePage from './pages/LabCreatePage';
import CertificateManagementPage from './pages/CertificateManagementPage';
import CertificateFormPage from './pages/CertificateFormPage';
import CertificateDetailPage from './pages/CertificateDetailPage';
import HomePage from './pages/HomePage';
import ProtectedRoute from './components/Auth/ProtectedRoute';
import './App.css'; 

function App() {
  return (
    <Router>
      <Routes>
        {/* Public Routes */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route
          path="/teacher/me"
          element={(
            <ProtectedRoute allowedRoles={['teacher']}>
              <TeacherWorkspacePage />
            </ProtectedRoute>
          )}
        />
        <Route
          path="/student/me"
          element={(
            <ProtectedRoute allowedRoles={['student']}>
              <StudentWorkspacePage />
            </ProtectedRoute>
          )}
        />
        <Route path="/" element={<HomePage />} />

        {/* Main Admin Layout Route */}
        <Route
          element={(
            <ProtectedRoute allowedRoles={['admin']}>
              <AdminLayout />
            </ProtectedRoute>
          )}
        >
          {/* Nested routes inside AdminLayout */}
          {/* <Route path="dashboard" element={<AdminDashboard />} /> */}
          <Route path="courses" element={<CourseManagementPage />} />
          <Route path="courses/new" element={<CourseFormPage />} />
          <Route path="courses/:id/edit" element={<CourseFormPage />} />
          <Route path="courses/:id" element={<CourseDetailPage />} />
          <Route path="schedules" element={<ScheduleManagementPage />} />
          <Route path="schedules/:id" element={<ScheduleDetailPage />} />
          <Route path="labs" element={<Navigate to="/labs/new" replace />} />
          <Route path="labs/new" element={<LabCreatePage />} />
          <Route path="students" element={<StudentManagementPage />} />
          <Route path="students/:id" element={<StudentDetailPage />} />
          <Route path="teachers" element={<TeacherManagementPage />} />
          <Route path="teachers/:id" element={<TeacherDetailPage />} />
          <Route path="topics" element={<TopicManagementPage />} />
          <Route path="topics/new" element={<TopicFormPage />} />
          <Route path="topics/:id/edit" element={<TopicFormPage />} />
          <Route path="topics/:id" element={<TopicDetailPage />} />
          <Route path="certificates" element={<CertificateManagementPage />} />
          <Route path="certificates/new" element={<CertificateFormPage />} />
          <Route path="certificates/:id/edit" element={<CertificateFormPage />} />
          <Route path="certificates/:id" element={<CertificateDetailPage />} />
          <Route path="reports" element={<ReportManagementPage />} />
          <Route path="reports/new" element={<ExamCreatePage />} />
          
          {/* Catch-all route to redirect back to courses */}
          <Route path="*" element={<Navigate to="/courses" replace />} />
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
