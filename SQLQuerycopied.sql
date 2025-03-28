USE [onlinenewsdb]
GO

/****** Object: Table [dbo].[Articles] Script Date: 27-01-2025 21:25:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Articles] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [PublishedDate]  DATETIME2 (7)  NOT NULL,
    [LinkText]       NVARCHAR (MAX) NOT NULL,
    [Headline]       NVARCHAR (100) NOT NULL,
    [ContentSummary] NVARCHAR (MAX) NOT NULL,
    [Content]        NVARCHAR (MAX) NOT NULL,
    [IsArchieved]    BIT            NOT NULL,
    [Views]          INT            NOT NULL,
    [Likes]          INT            NOT NULL,
    [ImageLink]      NVARCHAR (MAX) NOT NULL,
    [CategoryId]     INT            NOT NULL,
    [AuthorId]       NVARCHAR (450) NOT NULL,
    [EditorsChoice]  BIT            NOT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_Articles_AuthorId]
    ON [dbo].[Articles]([AuthorId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Articles_CategoryId]
    ON [dbo].[Articles]([CategoryId] ASC);


GO
ALTER TABLE [dbo].[Articles]
    ADD CONSTRAINT [PK_Articles] PRIMARY KEY CLUSTERED ([Id] ASC);


GO
ALTER TABLE [dbo].[Articles]
    ADD CONSTRAINT [FK_Articles_AspNetUsers_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE;


GO
ALTER TABLE [dbo].[Articles]
    ADD CONSTRAINT [FK_Articles_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE;


