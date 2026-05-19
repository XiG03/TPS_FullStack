import React, { useState } from 'react';
import { NavLink, Outlet } from 'react-router-dom';
import { BookOpen, Users, UserCog, FileText, Menu, X, CalendarDays } from 'lucide-react';
import './AdminLayout.css';

const AdminLayout = () => {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);

  const toggleSidebar = () => {
    setIsSidebarOpen(!isSidebarOpen);
  };

  const navItems = [
    { path: '/schedules', name: 'Schedules', icon: <CalendarDays size={20} /> },
    { path: '/courses', name: 'Courses', icon: <BookOpen size={20} /> },
    { path: '/students', name: 'Students', icon: <Users size={20} /> },
    { path: '/teachers', name: 'Teachers', icon: <UserCog size={20} /> },
    { path: '/topics', name: 'Topics', icon: <FileText size={20} /> },
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
