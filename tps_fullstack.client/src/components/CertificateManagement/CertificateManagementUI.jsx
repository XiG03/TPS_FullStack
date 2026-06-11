import { useMemo, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Award, Building2, Clock, Edit3, Eye, Plus, Search, Trash2 } from 'lucide-react';
import './CertificateManagement.css';

const formatDuration = (value) => {
    const duration = Number(value);
    if (!duration) return 'Chưa cấu hình';
    return `${duration} tháng`;
};

const CertificateManagementUI = ({ certificates, isLoading, error, onDeleteCertificate }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    const filteredCertificates = useMemo(() => {
        const normalizedSearch = searchTerm.trim().toLowerCase();
        if (!normalizedSearch) return certificates;

        return certificates.filter((certificate) => (
            certificate.ten?.toLowerCase().includes(normalizedSearch) ||
            certificate.mota?.toLowerCase().includes(normalizedSearch) ||
            certificate.donvicap?.toLowerCase().includes(normalizedSearch)
        ));
    }, [certificates, searchTerm]);

    const configuredCount = certificates.filter((certificate) => Number(certificate.thoigiansudung) > 0).length;
    const longestDuration = certificates.reduce((max, certificate) => Math.max(max, Number(certificate.thoigiansudung) || 0), 0);

    return (
        <div className="certificate-screen">
            <div className="certificate-shell">
                <header className="certificate-header">
                    <div>
                        <p className="certificate-kicker">api/v1/certificate</p>
                        <h1 className="certificate-title">Quản lý chứng chỉ</h1>
                        <p className="certificate-subtitle">Tạo cấu hình chứng chỉ, gắn khóa học và theo dõi học viên đã được cấp.</p>
                    </div>
                    <button type="button" className="certificate-button primary" onClick={() => navigate('/certificates/new')}>
                        <Plus size={18} />
                        Thêm chứng chỉ
                    </button>
                </header>

                <section className="certificate-stat-grid" aria-label="Thống kê chứng chỉ">
                    <div className="certificate-stat">
                        <span className="certificate-stat-icon"><Award size={22} /></span>
                        <div>
                            <p className="certificate-stat-label">Tổng chứng chỉ</p>
                            <span className="certificate-stat-value">{certificates.length}</span>
                        </div>
                    </div>
                    <div className="certificate-stat">
                        <span className="certificate-stat-icon"><Clock size={22} /></span>
                        <div>
                            <p className="certificate-stat-label">Đã cấu hình hạn</p>
                            <span className="certificate-stat-value">{configuredCount}</span>
                        </div>
                    </div>
                    <div className="certificate-stat">
                        <span className="certificate-stat-icon"><Building2 size={22} /></span>
                        <div>
                            <p className="certificate-stat-label">Thời hạn dài nhất</p>
                            <span className="certificate-stat-value">{longestDuration ? `${longestDuration} tháng` : '-'}</span>
                        </div>
                    </div>
                </section>

                <section className="certificate-toolbar">
                    <div className="certificate-search">
                        <Search size={18} />
                        <input
                            className="certificate-input"
                            type="text"
                            value={searchTerm}
                            onChange={(event) => setSearchTerm(event.target.value)}
                            placeholder="Tìm theo tên, mô tả hoặc đơn vị cấp..."
                        />
                    </div>
                </section>

                <section className="certificate-table">
                    <div className="certificate-table-head">
                        <div>Chứng chỉ</div>
                        <div>Mô tả</div>
                        <div>Thời hạn</div>
                        <div>Thao tác</div>
                    </div>

                    {isLoading ? (
                        <div className="certificate-loading">Đang tải danh sách chứng chỉ...</div>
                    ) : error ? (
                        <div className="certificate-error">{error}</div>
                    ) : filteredCertificates.length === 0 ? (
                        <div className="certificate-empty">Không tìm thấy chứng chỉ nào.</div>
                    ) : (
                        filteredCertificates.map((certificate) => (
                            <article className="certificate-row" key={certificate.maID}>
                                <div className="certificate-name">
                                    <strong title={certificate.ten}>{certificate.ten || 'Chưa cập nhật tên'}</strong>
                                    <span>{certificate.donvicap || 'Chưa có đơn vị cấp'}</span>
                                </div>
                                <div className="certificate-description" title={certificate.mota}>
                                    {certificate.mota || 'Chưa có mô tả.'}
                                </div>
                                <div>
                                    <span className="certificate-pill">{formatDuration(certificate.thoigiansudung)}</span>
                                </div>
                                <div className="certificate-row-actions">
                                    <button type="button" className="certificate-icon-button" title="Xem chi tiết" onClick={() => navigate(`/certificates/${certificate.maID}`)}>
                                        <Eye size={18} />
                                    </button>
                                    <button type="button" className="certificate-icon-button" title="Chỉnh sửa" onClick={() => navigate(`/certificates/${certificate.maID}/edit`)}>
                                        <Edit3 size={18} />
                                    </button>
                                    <button type="button" className="certificate-icon-button danger" title="Xóa" onClick={() => onDeleteCertificate(certificate.maID)}>
                                        <Trash2 size={18} />
                                    </button>
                                </div>
                            </article>
                        ))
                    )}
                </section>
            </div>
        </div>
    );
};

export default CertificateManagementUI;
