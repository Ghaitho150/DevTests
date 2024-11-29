CREATE TABLE [dbo].[Employee]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Monday] BIT NOT NULL DEFAULT 0, 
    [Tuesday] BIT NOT NULL DEFAULT 0, 
    [Wednesday] BIT NOT NULL DEFAULT 0, 
    [Thursday] BIT NOT NULL DEFAULT 0, 
    [Friday] BIT NOT NULL DEFAULT 0, 
    [Saturday] BIT NOT NULL DEFAULT 0, 
    [Sunday] BIT NOT NULL DEFAULT 0
)
