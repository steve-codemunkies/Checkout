-- Set date format to ensure text dates are parsed correctly
SET DATEFORMAT ymd

-- Turn off affected rows being returned
SET NOCOUNT ON

PRINT 'Updating static data table [dbo].[SkuWithMultiBuy]'

-- 1: Define table variable
DECLARE @tblTempTable TABLE (
[Id] int,
[Item] nvarchar(13),
[UnitPrice] int,
[MultiBuyItemCount] int NULL,
[MultiBuyPrice] int NULL
)

-- 2: Populate the table variable with data
INSERT INTO @tblTempTable ([Id], [Item], [UnitPrice], [MultiBuyItemCount], [MultiBuyPrice]) VALUES (1, 'A', 50, 3, 130)
INSERT INTO @tblTempTable ([Id], [Item], [UnitPrice], [MultiBuyItemCount], [MultiBuyPrice]) VALUES (2, 'B', 30, 2, 45)
INSERT INTO @tblTempTable ([Id], [Item], [UnitPrice], [MultiBuyItemCount], [MultiBuyPrice]) VALUES (3, 'C', 20, null, null)
INSERT INTO @tblTempTable ([Id], [Item], [UnitPrice], [MultiBuyItemCount], [MultiBuyPrice]) VALUES (4, 'D', 15, null, null)

-- 3: Insert any new items into the table from the table variable
IF $(InsertData) = 1
BEGIN
	PRINT N'Inserting data into data table [dbo].[SkuWithMultiBuy]';
		
	SET IDENTITY_INSERT [dbo].[SkuWithMultiBuy] ON

	INSERT INTO [dbo].[SkuWithMultiBuy] ([Id], [Item], [UnitPrice], [MultiBuyItemCount], [MultiBuyPrice])
	SELECT tmp.[Id], tmp.[Item], tmp.[UnitPrice], tmp.[MultiBuyItemCount], tmp.[MultiBuyPrice]
	FROM @tblTempTable tmp
	LEFT JOIN [dbo].[SkuWithMultiBuy] tbl ON tbl.[Id] = tmp.[Id]
	WHERE tbl.[Id] IS NULL

	SET IDENTITY_INSERT [dbo].[SkuWithMultiBuy] OFF
END

-- 4: Update any modified values with the values from the table variable
IF $(UpdateData) = 1 AND $(ProductionDeployment) = 0
BEGIN
	PRINT N'Updating data into data table [dbo].[SkuWithMultiBuy]';
		
	UPDATE LiveTable SET
	LiveTable.[Item] = tmp.[Item],
	LiveTable.[UnitPrice] = tmp.[UnitPrice],
	LiveTable.[MultiBuyItemCount] = tmp.[MultiBuyItemCount],
	LiveTable.[MultiBuyPrice] = tmp.[MultiBuyPrice]
	FROM [dbo].[SkuWithMultiBuy] LiveTable 
	INNER JOIN @tblTempTable tmp ON LiveTable.[Id] = tmp.[Id]
END

-- 5: Delete any missing records from the target
IF $(DeleteData) = 1 AND $(ProductionDeployment) = 0
BEGIN
	PRINT N'Deleting data into data table [dbo].[SkuWithMultiBuy]';
		
		DELETE FROM [dbo].[SkuWithMultiBuy] FROM [dbo].[SkuWithMultiBuy] LiveTable
		LEFT JOIN @tblTempTable tmp ON LiveTable.[Id] = tmp.[Id]
		WHERE tmp.[Id] IS NULL
END

PRINT 'Finished updating static data table [dbo].[SkuWithMultiBuy]'

-- Note: If you are not using the new GDR version of DBPro
-- then remove this go command.
GO