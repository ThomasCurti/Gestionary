CREATE TABLE [dbo].[Roles] (
    [id]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [name] VARCHAR (32) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([id] ASC)
);

