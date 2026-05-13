import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import AdminLayout from './components/Layout/AdminLayout';
import TopicManagement from './components/TopicManagement/TopicManagement';
import CourseManagement from './components/CourseManagement/CourseManagement';
import StudentManagement from './components/StudentManagement/StudentManagement';
import TeacherManagement from './components/TeacherManagement/TeacherManagement';
import './App.css'; 

function App() {
  return (
    <Router>
      <Routes>
        {/* Main Admin Layout Route */}
        <Route path="/" element={<AdminLayout />}>
          {/* Redirect from root to topics or courses */}
          <Route index element={<Navigate to="/courses" replace />} />
          
          <Route path="courses" element={<CourseManagement />} />
          <Route path="students" element={<StudentManagement />} />
          <Route path="teachers" element={<TeacherManagement />} />
          <Route path="topics" element={<TopicManagement />} />
          
          {/* Catch-all route to redirect back to courses */}
          <Route path="*" element={<Navigate to="/courses" replace />} />
        </Route>
      </Routes>
    </Router>
  );
}

export default App;