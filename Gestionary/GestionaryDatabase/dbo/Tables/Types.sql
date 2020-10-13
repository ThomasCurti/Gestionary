CREATE TABLE [dbo].[Types] (
    [id]   BIGINT       IDENTITY (1, 1) NOT NULL,
    [name] VARCHAR (64) NOT NULL,
    CONSTRAINT [PK_Types] PRIMARY KEY CLUSTERED ([id] ASC)
);

