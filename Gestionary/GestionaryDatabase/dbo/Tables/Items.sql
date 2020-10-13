CREATE TABLE [dbo].[Items] (
    [id]       BIGINT          IDENTITY (1, 1) NOT NULL,
    [name]     VARCHAR (MAX)   NOT NULL,
    [price]    DECIMAL (18, 2) NOT NULL,
    [stock]    BIGINT          NOT NULL,
    [type_id]  BIGINT          NOT NULL,
    [pic_name] VARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Items_Types] FOREIGN KEY ([type_id]) REFERENCES [dbo].[Types] ([id])
);



