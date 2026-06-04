import { Navigate, useLocation } from 'react-router-dom';
import { getCurrentUserRole, getDefaultPathForRole, hasAnyRole } from '../../utils/auth';
import { getToken } from '../../services/httpClient';

const ProtectedRoute = ({ allowedRoles, children }) => {
    const location = useLocation();
    const token = getToken();
    const role = getCurrentUserRole();

    if (!token) {
        return <Navigate to="/login" replace state={{ from: location.pathname }} />;
    }

    if (allowedRoles?.length && !hasAnyRole(role, allowedRoles)) {
        return <Navigate to={getDefaultPathForRole(role)} replace />;
    }

    return children;
};

export default ProtectedRoute;
