CREATE TABLE [dbo].[Chungchi](
	[MaID] [nvarchar](50) NOT NULL,
	[Ten] [nvarchar](200) NOT NULL,
	[Mota] [nvarchar](500) NOT NULL,
	[Donvicap] [nvarchar](500) NOT NULL,
	[Thoigiansudung] [decimal](18, 2) NOT NULL,
	[Khongsudung] [bit] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](450) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](450) NULL,
	[DeletedAt] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](450) NULL,
 CONSTRAINT [PK_Chungchi] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Chungchi_Hocvien](
	[MaID] [nvarchar](50) NOT NULL,
	[KhoahocID] [nvarchar](50) NOT NULL,
	[ChungchiID] [nvarchar](50) NOT NULL,
	[HocvienID] [nvarchar](50) NOT NULL,
	[Chungchi_Ten] [nvarchar](200) NOT NULL,
	[Chungchi_Mota] [nvarchar](500) NOT NULL,
	[Chungchi_Donvicap] [nvarchar](200) NOT NULL,
	[Chungchi_Thoigiansudung] [decimal](18, 2) NOT NULL,
	[Ngaycap] [datetime2](7) NOT NULL,
	[Ngayhethan] [datetime2](7) NOT NULL,
	[Khongsudung] [bit] NOT NULL,
 CONSTRAINT [PK_Chungchi_Hocvien] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Chuyende](
	[MaID] [nvarchar](50) NOT NULL,
	[Ten] [nvarchar](50) NOT NULL,
	[Mota] [nvarchar](500) NOT NULL,
	[Khongsudung] [bit] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](450) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](450) NULL,
	[DeletedAt] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](450) NULL,
 CONSTRAINT [PK_Chuyende] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Chuyende_Cauhoi](
	[MaID] [nvarchar](50) NOT NULL,
	[ChuyendeID] [nvarchar](50) NOT NULL,
	[Ten] [nvarchar](200) NOT NULL,
	[Diem] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Chuyende_Cauhoi] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Chuyende_Dapan](
	[MaID] [nvarchar](50) NOT NULL,
	[Chuyende_CauhoiID] [nvarchar](50) NOT NULL,
	[Ten] [nvarchar](200) NOT NULL,
	[Dung] [bit] NOT NULL,
	[ChuyendeID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Chuyende_Dapan] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Chuyende_Giangvien](
	[MaID] [nvarchar](50) NOT NULL,
	[ChuyendeID] [nvarchar](50) NOT NULL,
	[GiangvienID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Chuyende_Giangvien] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Chuyende_Tailieu](
	[MaID] [nvarchar](50) NOT NULL,
	[ChuyendeID] [nvarchar](50) NOT NULL,
	[Tieude] [nvarchar](200) NOT NULL,
	[Ngaytao] [datetime2](7) NOT NULL,
	[Loaitailieu] [nvarchar](max) NOT NULL,
	[Kichthuoc] [decimal](18, 2) NOT NULL,
	[Khongsudung] [bit] NULL,
 CONSTRAINT [PK_Chuyende_Tailieu] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[Giangvien](
	[MaID] [nvarchar](50) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[Hoten] [nvarchar](200) NOT NULL,
	[Ngaysinh] [datetime2](7) NOT NULL,
	[Gioitinh] [nvarchar](10) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Diachi] [nvarchar](200) NOT NULL,
	[Dienthoai] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](450) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](450) NULL,
	[DeletedAt] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](450) NULL,
	[Khongsudung] [bit] NULL,
 CONSTRAINT [PK_Giangvien] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Hocvien](
	[MaID] [nvarchar](50) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[Hoten] [nvarchar](200) NOT NULL,
	[Ngaysinh] [datetime2](7) NOT NULL,
	[Gioitinh] [nvarchar](10) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Diachi] [nvarchar](200) NOT NULL,
	[Dienthoai] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](450) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](450) NULL,
	[DeletedAt] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](450) NULL,
 CONSTRAINT [PK_Hocvien] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Khoahoc](
	[MaID] [nvarchar](50) NOT NULL,
	[Ten] [nvarchar](200) NOT NULL,
	[Mota] [nvarchar](500) NOT NULL,
	[Diemdat] [decimal](18, 2) NOT NULL,
	[ChungchiID] [nvarchar](50) NOT NULL,
	[LichhocID] [nvarchar](50) NOT NULL,
	[Khongsudung] [bit] NULL,
	[CreatedAt] [datetime2](7) NULL,
	[CreatedBy] [nvarchar](450) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[UpdatedBy] [nvarchar](450) NULL,
	[DeletedAt] [datetime2](7) NULL,
	[DeletedBy] [nvarchar](450) NULL,
 CONSTRAINT [PK_Khoahoc] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Khoahoc_DmTrangthai](
	[MaID] [nvarchar](50) NOT NULL,
	[KhoahocID] [nvarchar](50) NOT NULL,
	[Ten] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Khoahoc_DmTrangthai] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Khoahoc_Giangvien](
	[MaID] [nvarchar](50) NOT NULL,
	[KhoahocID] [nvarchar](50) NOT NULL,
	[GiangvienID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Khoahoc_Giangvien] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Khoahoc_Hocvien](
	[MaID] [nvarchar](50) NOT NULL,
	[KhoahocID] [nvarchar](50) NOT NULL,
	[HocvienID] [nvarchar](50) NOT NULL,
	[Diem] [decimal](18, 2) NOT NULL,
	[Dieuchinh] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Khoahoc_Hocvien] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Lichhoc](
	[MaID] [nvarchar](50) NOT NULL,
	[Thu] [nvarchar](100) NOT NULL,
	[Thoigianhoc] [decimal](4, 1) NOT NULL,
	[Sobuoihoc] [decimal](4, 1) NOT NULL,
 CONSTRAINT [PK_Lichhoc] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Lichhoc_Ct](
	[MaID] [nvarchar](50) NOT NULL,
	[LichhocID] [nvarchar](50) NOT NULL,
	[GiangvienID] [nvarchar](50) NOT NULL,
	[Ngay] [datetime2](7) NOT NULL,
	[Batdaudukien] [datetime2](7) NOT NULL,
	[Ketthucdukien] [datetime2](7) NOT NULL,
	[Lichhoc_Thoigianhoc] [decimal](4, 1) NOT NULL,
 CONSTRAINT [PK_Lichhoc_Ct] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Lichhoc_Ct_Diemdanh](
	[MaID] [nvarchar](50) NOT NULL,
	[Lichhoc_CtID] [nvarchar](50) NOT NULL,
	[HocvienID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Lichhoc_Ct_Diemdanh] PRIMARY KEY CLUSTERED 
(
	[MaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[RefreshTokens](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[refreshToken] [nvarchar](max) NOT NULL,
	[ExpiryTime] [datetime2](7) NOT NULL,
	[IsRevoked] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO