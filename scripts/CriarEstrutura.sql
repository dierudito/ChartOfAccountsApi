USE [master]
GO
CREATE DATABASE [ChartOfAccountsApi]
USE [ChartOfAccountsApi]
GO
CREATE TABLE [dbo].[Accounts](
	[Id] [uniqueidentifier] NOT NULL,
	[IdParentAccount] [uniqueidentifier] NULL,
	[AcceptEntries] [bit] NOT NULL,
	[IdAccountType] [uniqueidentifier] NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[code] [int] NOT NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[AccountTypes](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_AccountTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_Accounts] FOREIGN KEY([IdParentAccount])
REFERENCES [dbo].[Accounts] ([Id])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_Accounts]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_AccountTypes] FOREIGN KEY([IdAccountType])
REFERENCES [dbo].[AccountTypes] ([Id])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_AccountTypes]
GO

INSERT [dbo].[AccountTypes] ([Id], [Name]) VALUES ('34e4dcf9-7740-45e0-84c9-6cccb635fdb4', 'Despesa')
GO
INSERT [dbo].[AccountTypes] ([Id], [Name]) VALUES ('7872298c-cccd-42ad-b4c7-fc5ad7f042f5', 'Receita')
