import { useState } from 'react';
import { NavLink, Outlet } from 'react-router-dom';
import { Award, BookOpen, Users, UserCog, FileText, Menu, X, CalendarDays, ClipboardCheck, FlaskConical } from 'lucide-react';
import './AdminLayout.css';

const AdminLayout = () => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  const navItems = [
    { path: '/schedules', name: 'Lịch học', icon: <CalendarDays size={20} /> },
    { path: '/labs/new', name: 'Thực hành', icon: <FlaskConical size={20} /> },
    { path: '/courses', name: 'Khóa học', icon: <BookOpen size={20} /> },
    { path: '/students', name: 'Học viên', icon: <Users size={20} /> },
    { path: '/teachers', name: 'Giảng viên', icon: <UserCog size={20} /> },
    { path: '/topics', name: 'Chuyên đề', icon: <FileText size={20} /> },
    { path: '/reports', name: 'Báo cáo', icon: <ClipboardCheck size={20} /> },
    { path: '/certificates', name: 'Chứng chỉ', icon: <Award size={20} /> },
  ];

  return (
    <div className="admin-layout">
      {/* Sidebar */}
      <aside className={`sidebar ${isSidebarOpen ? 'open' : 'closed'}`}>
        <div className="sidebar-header">
          <div className="logo-container">
            <h2 className={`logo-text ${!isSidebarOpen ? 'hidden' : ''}`}>TPS Admin</h2>
          </div>
          <button className="toggle-btn" onClick={toggleSidebar}>
            {isSidebarOpen ? <X size={24} /> : <Menu size={24} />}
          </button>
        </div>

        <nav className="sidebar-nav">
          <ul className="nav-list">
            {navItems.map((item) => (
              <li key={item.path} className="nav-item">
                <NavLink 
                  to={item.path} 
                  className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}
                  title={!isSidebarOpen ? item.name : ''}
                >
                  <span className="nav-icon">{item.icon}</span>
                  <span className={`nav-text ${!isSidebarOpen ? 'hidden' : ''}`}>
                    {item.name}
                  </span>
                </NavLink>
              </li>
            ))}
          </ul>
        </nav>
      </aside>

      {/* Main Content Area */}
      <main className="main-content">
        {/* We use Outlet to render the active route's component here */}
        <Outlet />
      </main>
    </div>
  );
};

export default AdminLayout;
