CREATE TABLE [dbo].[Users] (
    [id]            BIGINT        IDENTITY (1, 1) NOT NULL,
    [username]      VARCHAR (64)  NOT NULL,
    [password_hash] VARCHAR (MAX) NOT NULL,
    [role_id]       BIGINT        NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([role_id]) REFERENCES [dbo].[Roles] ([id])
);

