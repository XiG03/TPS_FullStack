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
import ReportManagementPage from './pages/ReportManagementPage';
import LabCreatePage from './pages/LabCreatePage';
import './App.css'; 

function App() {
  return (
    <Router>
      <Routes>
        {/* Public Routes */}
        <Route path="/login" element={<LoginPage />} />
        <Route path="/teacher/me" element={<TeacherWorkspacePage />} />
        <Route path="/student/me" element={<StudentWorkspacePage />} />

        {/* Main Admin Layout Route */}
        <Route path="/" element={<AdminLayout />}>
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
          <Route path="reports" element={<ReportManagementPage />} />
          
          {/* Catch-all route to redirect back to courses */}
          <Route path="*" element={<Navigate to="/courses" replace />} />
        </Route>
      </Routes>
    </Router>
  );
}

export default App;
