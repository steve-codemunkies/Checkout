CREATE TABLE [dbo].[SkuWithMultiBuy]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [Item] NVARCHAR(13) NOT NULL, 
    [UnitPrice] INT NOT NULL, 
    [MultiBuyItemCount] INT NULL, 
    [MultiBuyPrice] INT NULL
)
