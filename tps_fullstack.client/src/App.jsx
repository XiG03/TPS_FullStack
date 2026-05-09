import React from 'react';
import TopicManagement from './components/TopicManagement/TopicManagement';
import './App.css'; // Optional if you have global app styles, otherwise can be removed

function App() {
  return (
    <div className="app-layout">
      {/* Optional: Add a sidebar or navbar here later */}
      <main className="main-content">
        <TopicManagement />
      </main>
    </div>
  );
}

export default App;