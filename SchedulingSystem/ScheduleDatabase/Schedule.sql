CREATE TABLE [dbo].[Schedule]
(
	[Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [EmployeeId] INT NULL, 
    [ShiftId] INT NULL, 
    CONSTRAINT [FK_Schedule_EmployeeID] FOREIGN KEY ([EmployeeId]) REFERENCES [Employee]([ID]), 
    CONSTRAINT [FK_Schedule_ShiftID] FOREIGN KEY ([ShiftId]) REFERENCES [Shift]([ID])
   
)
