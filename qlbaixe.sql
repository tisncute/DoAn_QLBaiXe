/****** Object:  Table [dbo].[__EFMigrationsHistory] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ParkingSessions] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParkingSessions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LicensePlate] [nvarchar](max) NOT NULL,
	[VehicleType] [nvarchar](max) NOT NULL,
	[CheckInTime] [datetime2](7) NOT NULL,
	[CheckOutTime] [datetime2](7) NULL,
	[ParkingFee] [decimal](18, 2) NOT NULL,
	[IsCompleted] [bit] NOT NULL,
	[SpotName] [nvarchar](max) NULL,
	[CustomerName] [nvarchar](max) NULL,
	[IsBooked] [bit] NOT NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[BookingTime] [datetime2](7) NULL,
 CONSTRAINT [PK_ParkingSessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260302034856_TaoDatabaseLanDau', N'8.0.24')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260302054059_InitialCreate', N'8.0.24')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260310064929_ThemCotViTri', N'8.0.24')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260311053800_ThemDatChoWeb', N'8.0.24')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260312152025_ThemGioHen', N'8.0.24')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20260312154825_ThemCotSoDienThoai', N'8.0.24')
GO

SET IDENTITY_INSERT [dbo].[ParkingSessions] ON 

INSERT [dbo].[ParkingSessions] ([Id], [LicensePlate], [VehicleType], [CheckInTime], [CheckOutTime], [ParkingFee], [IsCompleted], [SpotName], [CustomerName], [IsBooked], [PhoneNumber], [BookingTime]) VALUES (104, N'30-M3 555.55', N'XeMay', CAST(N'2026-03-14T20:35:07.0125296' AS DateTime2), CAST(N'2026-03-14T22:06:33.6736541' AS DateTime2), CAST(5000.00 AS Decimal(18, 2)), 1, N'A3', NULL, 0, NULL, NULL)
INSERT [dbo].[ParkingSessions] ([Id], [LicensePlate], [VehicleType], [CheckInTime], [CheckOutTime], [ParkingFee], [IsCompleted], [SpotName], [CustomerName], [IsBooked], [PhoneNumber], [BookingTime]) VALUES (105, N'59-S2 333.33', N'XeMay', CAST(N'2026-03-14T20:35:13.1056722' AS DateTime2), CAST(N'2026-03-14T20:35:22.2324309' AS DateTime2), CAST(5000.00 AS Decimal(18, 2)), 1, N'A1', NULL, 0, NULL, NULL)
INSERT [dbo].[ParkingSessions] ([Id], [LicensePlate], [VehicleType], [CheckInTime], [CheckOutTime], [ParkingFee], [IsCompleted], [SpotName], [CustomerName], [IsBooked], [PhoneNumber], [BookingTime]) VALUES (106, N'30F-686.86', N'OTo', CAST(N'2026-03-14T20:35:26.2769013' AS DateTime2), CAST(N'2026-03-14T22:06:30.8956696' AS DateTime2), CAST(30000.00 AS Decimal(18, 2)), 1, N'A1', NULL, 0, NULL, NULL)
INSERT [dbo].[ParkingSessions] ([Id], [LicensePlate], [VehicleType], [CheckInTime], [CheckOutTime], [ParkingFee], [IsCompleted], [SpotName], [CustomerName], [IsBooked], [PhoneNumber], [BookingTime]) VALUES (109, N'65-V1 8659', N'XeMay', CAST(N'2026-03-14T21:50:55.3219981' AS DateTime2), NULL, CAST(0.00 AS Decimal(18, 2)), 0, N'A4', N'Lê Dương Huỳnh Tín', 1, N'0762841300', CAST(N'2026-03-14T21:49:00.0000000' AS DateTime2))
INSERT [dbo].[ParkingSessions] ([Id], [LicensePlate], [VehicleType], [CheckInTime], [CheckOutTime], [ParkingFee], [IsCompleted], [SpotName], [CustomerName], [IsBooked], [PhoneNumber], [BookingTime]) VALUES (110, N'VINFAST-VF8', N'OTo', CAST(N'2026-03-14T22:25:13.1313694' AS DateTime2), NULL, CAST(0.00 AS Decimal(18, 2)), 0, N'A1', NULL, 0, NULL, NULL)

SET IDENTITY_INSERT [dbo].[ParkingSessions] OFF
GO

ALTER TABLE [dbo].[ParkingSessions] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsBooked]
GO