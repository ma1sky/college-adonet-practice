SELECT @@VERSION AS ServerVersion;
GO

SELECT StudentId, FullName, GroupName, Email
FROM dbo.Students
ORDER BY StudentId;
GO

SELECT COUNT(*) AS TransferCount
FROM dbo.StudentTransfers;
GO
