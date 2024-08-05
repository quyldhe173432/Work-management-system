USE [master]
GO
Create database [Prn212]
GO
USE [Prn212]
GO

CREATE TABLE [dbo].[Customers](
	[CustomerId] [int] PRIMARY KEY IDENTITY(1,1),
	[CustomerName] [nvarchar](50) NOT NULL,
	[Password] [varchar](20) NOT NULL,
	[Birthdate] [smalldatetime] NOT NULL,
	[Gender] bit NOT NULL,
	[Address] [nvarchar](max) NULL,
);
GO

INSERT INTO [dbo].[Customers] ([CustomerName], [Password], [Birthdate], [Gender], [Address])
VALUES 
(N'quy', '123', '2003-06-07', 1, N'Thanh Hóa'),
(N'nam', '123', '2003-02-16', 1, N'Thanh Hóa');
GO

CREATE TABLE [dbo].[Status] (
    StatusID INT PRIMARY KEY,
    StatusName NVARCHAR(50)
);
GO

INSERT INTO [dbo].[Status] (StatusID, StatusName) VALUES 
(1, N'Dự định'), 
(2, N'Đang làm'), 
(3, N'Hoàn Thành');
GO

CREATE TABLE [dbo].[Actions](
    [ActionID] [int] PRIMARY KEY IDENTITY(1,1),
    [CustomerId] [int] NULL,
    [ActionName] [nvarchar](MAX) NULL,
    [ActionDescription] NVARCHAR(MAX) NULL,
    [dateAction] [nvarchar](max) NULL,
    [timeAction] [nvarchar](max) NULL,
    [StatusID] INT NULL,
    FOREIGN KEY (CustomerId) REFERENCES [dbo].[Customers](CustomerId),
    FOREIGN KEY (StatusID) REFERENCES [dbo].[Status](StatusID)
);
GO

SET IDENTITY_INSERT [dbo].[Actions] ON;
INSERT INTO [dbo].[Actions] ([ActionID],[CustomerId], [ActionName],[ActionDescription],[dateAction] ,[timeAction], [StatusID])
VALUES 
(1, 1, N'Action Name 1', N'Mô tả hành động 1', N'2023-07-10', N'08:00', 1),
(2, 1, N'Action Name 2', N'Mô tả hành động 2', N'2023-07-10', N'09:00', 2),
(3, 2, N'Action Name 3', N'Mô tả hành động 3', N'2023-07-11', N'10:00', 3);
SET IDENTITY_INSERT [dbo].[Actions] OFF;
GO