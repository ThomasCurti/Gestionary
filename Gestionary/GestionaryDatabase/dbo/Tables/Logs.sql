CREATE TABLE [dbo].[Logs] (
    [id]      BIGINT        IDENTITY (1, 1) NOT NULL,
    [date]    DATETIME      NOT NULL,
    [class]   VARCHAR (MAX) NULL,
    [type]    VARCHAR (MAX) NULL,
    [logtype] VARCHAR (MAX) NOT NULL,
    [message] VARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED ([id] ASC)
);

