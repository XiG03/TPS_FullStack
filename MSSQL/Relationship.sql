USE db_TPS;
GO

-- 1. Bảng Giaovien_Chungchi nối với Giaovien
ALTER TABLE Giaovien_Chungchi
ADD CONSTRAINT FK_GiaovienChungchi_Giaovien FOREIGN KEY (GiaovienID) 
REFERENCES Giaovien(GiaovienID);

-- 2. Bảng Cauhinhchungchi nối với Khoahoc
ALTER TABLE Cauhinhchungchi
ADD CONSTRAINT FK_Cauhinhchungchi_Khoahoc FOREIGN KEY (KhoahocID) 
REFERENCES Khoahoc(KhoahocID);

-- 3. Bảng Hocvien_Khoahoc nối với Hocvien và Khoahoc
ALTER TABLE Hocvien_Khoahoc
ADD CONSTRAINT FK_HocvienKhoahoc_Hocvien FOREIGN KEY (HocvienID) 
REFERENCES Hocvien(HocvienID);

ALTER TABLE Hocvien_Khoahoc
ADD CONSTRAINT FK_HocvienKhoahoc_Khoahoc FOREIGN KEY (KhoahocID) 
REFERENCES Khoahoc(KhoahocID);

-- 4. Bảng Giaovien_Khoahoc nối với Giaovien và Khoahoc
ALTER TABLE Giaovien_Khoahoc
ADD CONSTRAINT FK_GiaovienKhoahoc_Giaovien FOREIGN KEY (GiaovienID) 
REFERENCES Giaovien(GiaovienID);

ALTER TABLE Giaovien_Khoahoc
ADD CONSTRAINT FK_GiaovienKhoahoc_Khoahoc FOREIGN KEY (KhoahocID) 
REFERENCES Khoahoc(KhoahocID);

-- 5. Bảng Giaovien_Chuyende nối với Giaovien và Chuyende
ALTER TABLE Giaovien_Chuyende
ADD CONSTRAINT FK_GiaovienChuyende_Giaovien FOREIGN KEY (GiaovienID) 
REFERENCES Giaovien(GiaovienID);

ALTER TABLE Giaovien_Chuyende
ADD CONSTRAINT FK_GiaovienChuyende_Chuyende FOREIGN KEY (ChuyendeID) 
REFERENCES Chuyende(ChuyendeID);

-- 6. Bảng Tailieu nối với Chuyende
ALTER TABLE Tailieu
ADD CONSTRAINT FK_Tailieu_Chuyende FOREIGN KEY (ChuyendeID) 
REFERENCES Chuyende(ChuyendeID);

-- 7. Bảng Cauhoi nối với Chuyende
ALTER TABLE Cauhoi
ADD CONSTRAINT FK_Cauhoi_Chuyende FOREIGN KEY (ChuyendeID) 
REFERENCES Chuyende(ChuyendeID);

-- 8. Bảng Cauhoi_Dapan nối với Cauhoi
ALTER TABLE Cauhoi_Dapan
ADD CONSTRAINT FK_CauhoiDapan_Cauhoi FOREIGN KEY (CauhoiID) 
REFERENCES Cauhoi(CauhoiID);

-- 9. Bảng Lichhoc nối với Khoahoc, Chuyende và Giaovien
ALTER TABLE Lichhoc
ADD CONSTRAINT FK_Lichhoc_Khoahoc FOREIGN KEY (KhoahocID) 
REFERENCES Khoahoc(KhoahocID);

ALTER TABLE Lichhoc
ADD CONSTRAINT FK_Lichhoc_Chuyende FOREIGN KEY (ChuyendeID) 
REFERENCES Chuyende(ChuyendeID);

ALTER TABLE Lichhoc
ADD CONSTRAINT FK_Lichhoc_Giaovien FOREIGN KEY (GiaovienID) 
REFERENCES Giaovien(GiaovienID);

-- 10. Bảng Cauhinhbaithuhoach nối với Khoahoc
ALTER TABLE Cauhinhbaithuhoach
ADD CONSTRAINT FK_Cauhinhbaithuhoach_Khoahoc FOREIGN KEY (KhoahocID) 
REFERENCES Khoahoc(KhoahocID);

-- 11. Bảng Chitiecauhinhbaithuhoach nối với Cauhinhbaithuhoach (MaID) và Chuyende
ALTER TABLE Chitiecauhinhbaithuhoach
ADD CONSTRAINT FK_Chitiecauhinh_Cauhinh FOREIGN KEY (CauhinhID) 
REFERENCES Cauhinhbaithuhoach(MaID);

ALTER TABLE Chitiecauhinhbaithuhoach
ADD CONSTRAINT FK_Chitiecauhinh_Chuyende FOREIGN KEY (ChuyendeID) 
REFERENCES Chuyende(ChuyendeID);

-- 12. Bảng Baithi nối với Hocvien, Khoahoc và Cauhinhbaithuhoach (MaID)
ALTER TABLE Baithi
ADD CONSTRAINT FK_Baithi_Hocvien FOREIGN KEY (HocvienID) 
REFERENCES Hocvien(HocvienID);

ALTER TABLE Baithi
ADD CONSTRAINT FK_Baithi_Khoahoc FOREIGN KEY (KhoahocID) 
REFERENCES Khoahoc(KhoahocID);

ALTER TABLE Baithi
ADD CONSTRAINT FK_Baithi_Cauhinh FOREIGN KEY (CauhinhID) 
REFERENCES Cauhinhbaithuhoach(MaID);

-- 13. Bảng Baithi_Cauhoi nối với Baithi và Cauhoi
ALTER TABLE Baithi_Cauhoi
ADD CONSTRAINT FK_BaithiCauhoi_Baithi FOREIGN KEY (BaithiID) 
REFERENCES Baithi(BaithiID);

ALTER TABLE Baithi_Cauhoi
ADD CONSTRAINT FK_BaithiCauhoi_Cauhoi FOREIGN KEY (CauhoiID) 
REFERENCES Cauhoi(CauhoiID);

-- 14. Bảng Baithi_Cauhoi_Traloi nối với Baithi_Cauhoi (MaID) và Cauhoi_Dapan (Dapan_ID)
ALTER TABLE Baithi_Cauhoi_Traloi
ADD CONSTRAINT FK_BaithiTraloi_BaithiCauhoi FOREIGN KEY (Baithi_CauhoiID) 
REFERENCES Baithi_Cauhoi(MaID);

ALTER TABLE Baithi_Cauhoi_Traloi
ADD CONSTRAINT FK_BaithiTraloi_Dapan FOREIGN KEY (Cauhoi_DapanID) 
REFERENCES Cauhoi_Dapan(Dapan_ID);

-- 15. Bảng Cathuchanh nối với Khoahoc, Chuyende và Giaovien
ALTER TABLE Cathuchanh
ADD CONSTRAINT FK_Cathuchanh_Khoahoc FOREIGN KEY (KhoahocID) 
REFERENCES Khoahoc(KhoahocID);

ALTER TABLE Cathuchanh
ADD CONSTRAINT FK_Cathuchanh_Chuyende FOREIGN KEY (ChuyendeID) 
REFERENCES Chuyende(ChuyendeID);

ALTER TABLE Cathuchanh
ADD CONSTRAINT FK_Cathuchanh_Giaovien FOREIGN KEY (GiaovienID) 
REFERENCES Giaovien(GiaovienID);

-- 16. Bảng Cathuchanh_Dangky nối với Hocvien và Cathuchanh
ALTER TABLE Cathuchanh_Dangky
ADD CONSTRAINT FK_DangkyThuchanh_Hocvien FOREIGN KEY (HocvienID) 
REFERENCES Hocvien(HocvienID);

ALTER TABLE Cathuchanh_Dangky
ADD CONSTRAINT FK_DangkyThuchanh_Cathuchanh FOREIGN KEY (CathuchanhID) 
REFERENCES Cathuchanh(CathuchanhID);

-- 17. Bảng Chungchi nối với Hocvien, Khoahoc và Cauhinhchungchi
ALTER TABLE Chungchi
ADD CONSTRAINT FK_Chungchi_Hocvien FOREIGN KEY (HocvienID) 
REFERENCES Hocvien(HocvienID);

ALTER TABLE Chungchi
ADD CONSTRAINT FK_Chungchi_Khoahoc FOREIGN KEY (KhoahocID) 
REFERENCES Khoahoc(KhoahocID);

ALTER TABLE Chungchi
ADD CONSTRAINT FK_Chungchi_Cauhinh FOREIGN KEY (CauhinhchungchiID) 
REFERENCES Cauhinhchungchi(CauhinhID);

-- 18. Bảng Diemdanh nối với Giaovien, Hocvien và Khoahoc
ALTER TABLE Diemdanh
ADD CONSTRAINT FK_Diemdanh_Giaovien FOREIGN KEY (GiaovienID) 
REFERENCES Giaovien(GiaovienID);

ALTER TABLE Diemdanh
ADD CONSTRAINT FK_Diemdanh_Hocvien FOREIGN KEY (HocvienID) 
REFERENCES Hocvien(HocvienID);

ALTER TABLE Diemdanh
ADD CONSTRAINT FK_Diemdanh_Khoahoc FOREIGN KEY (KhoahocID) 
REFERENCES Khoahoc(KhoahocID);

