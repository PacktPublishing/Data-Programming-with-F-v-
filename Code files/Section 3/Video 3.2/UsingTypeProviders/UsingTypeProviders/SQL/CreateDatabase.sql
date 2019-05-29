SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

USE [master];
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'OrdersDatabase')
  DROP DATABASE OrdersDatabase;
GO

-- Create the OrdersDatabase database.
CREATE DATABASE OrdersDatabase;
GO

-- Specify a simple recovery model 
-- to keep the log growth to a minimum.
ALTER DATABASE OrdersDatabase 
SET RECOVERY SIMPLE;
GO

USE [OrdersDatabase]
GO
ALTER TABLE [dbo].[OrderItems] DROP CONSTRAINT [FK_OrderItems_PurchaseOrder]
GO
ALTER TABLE [dbo].[OrderItems] DROP CONSTRAINT [FK_OrderItems_Product]
GO
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 2/21/2018 2:26:32 AM ******/
DROP TABLE [dbo].[PurchaseOrder]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 2/21/2018 2:26:32 AM ******/
DROP TABLE [dbo].[Product]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 2/21/2018 2:26:32 AM ******/
DROP TABLE [dbo].[OrderItems]
GO
/****** Object:  Table [dbo].[OrderItems]    Script Date: 2/21/2018 2:26:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[PurchaseOrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_OrderItems] PRIMARY KEY CLUSTERED 
(
	[PurchaseOrderId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 2/21/2018 2:26:36 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] NOT NULL,
	[ProductCategory] [int] NULL,
	[Name] [nvarchar](60) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseOrder]    Script Date: 2/21/2018 2:26:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseOrder](
	[Id] [int] NOT NULL,
	[Orderdate] [datetime] NOT NULL,
	[DeliveryCode] [int] NOT NULL,
	[Customer] [nvarchar](60) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (1, 1, 1)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (1, 4, 3)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (1, 5, 6)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (2, 1, 4)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (2, 2, 2)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (2, 3, 1)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (3, 3, 3)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (3, 5, 7)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (4, 1, 3)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (4, 2, 4)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (4, 3, 1)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (4, 5, 24)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (200, 1, 3)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (200, 3, 12)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (200, 5, 60)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (300, 1, 3)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (300, 3, 12)
GO
INSERT [dbo].[OrderItems] ([PurchaseOrderId], [ProductId], [Quantity]) VALUES (300, 5, 60)
GO
INSERT [dbo].[Product] ([Id], [ProductCategory], [Name]) VALUES (1, 1, N'Garden Fertilizer')
GO
INSERT [dbo].[Product] ([Id], [ProductCategory], [Name]) VALUES (2, 1, N'Vegetable Fertilizer')
GO
INSERT [dbo].[Product] ([Id], [ProductCategory], [Name]) VALUES (3, 2, N'Lime')
GO
INSERT [dbo].[Product] ([Id], [ProductCategory], [Name]) VALUES (4, 2, N'Peat Moss')
GO
INSERT [dbo].[Product] ([Id], [ProductCategory], [Name]) VALUES (5, 3, N'Mulch')
GO
INSERT [dbo].[PurchaseOrder] ([Id], [Orderdate], [DeliveryCode], [Customer]) VALUES (1, CAST(N'2018-01-23T00:00:00.000' AS DateTime), 1, N'Frank Farmer')
GO
INSERT [dbo].[PurchaseOrder] ([Id], [Orderdate], [DeliveryCode], [Customer]) VALUES (2, CAST(N'2018-01-13T00:00:00.000' AS DateTime), 32, N'Ernest Sawyer')
GO
INSERT [dbo].[PurchaseOrder] ([Id], [Orderdate], [DeliveryCode], [Customer]) VALUES (3, CAST(N'2018-02-06T00:00:00.000' AS DateTime), 1, N'Sid Stackhorn')
GO
INSERT [dbo].[PurchaseOrder] ([Id], [Orderdate], [DeliveryCode], [Customer]) VALUES (4, CAST(N'2017-01-03T00:00:00.000' AS DateTime), 15, N'Felicity Rinaport')
GO
INSERT [dbo].[PurchaseOrder] ([Id], [Orderdate], [DeliveryCode], [Customer]) VALUES (200, CAST(N'2018-02-03T23:47:21.527' AS DateTime), 4, N'O. Henry')
GO
INSERT [dbo].[PurchaseOrder] ([Id], [Orderdate], [DeliveryCode], [Customer]) VALUES (300, CAST(N'2018-01-14T02:16:41.600' AS DateTime), 4, N'Booth Tarkington')
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_Product]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [FK_OrderItems_PurchaseOrder] FOREIGN KEY([PurchaseOrderId])
REFERENCES [dbo].[PurchaseOrder] ([Id])
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [FK_OrderItems_PurchaseOrder]
GO
