-- Выполняется в базе CollegeAdo. Базу предварительно создаёт проект setup.
IF OBJECT_ID(N'dbo.Students', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Students
    (
        StudentId int IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_Students PRIMARY KEY,
        FullName nvarchar(100) NOT NULL,
        GroupName nvarchar(20) NOT NULL,
        Email nvarchar(150) NULL
    );

    INSERT INTO dbo.Students (FullName, GroupName, Email)
    VALUES
        (N'Анна Иванова', N'ИСП-21', N'anna@example.com'),
        (N'Борис Петров', N'ИСП-21', NULL),
        (N'Вера Соколова', N'ИСП-22', N'vera@example.com');
END;

IF OBJECT_ID(N'dbo.StudentTransfers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.StudentTransfers
    (
        TransferId int IDENTITY(1, 1) NOT NULL
            CONSTRAINT PK_StudentTransfers PRIMARY KEY,
        StudentId int NOT NULL,
        FromGroup nvarchar(20) NOT NULL,
        ToGroup nvarchar(20) NOT NULL,
        ChangedAt datetime2 NOT NULL
            CONSTRAINT DF_StudentTransfers_ChangedAt DEFAULT SYSUTCDATETIME(),
        CONSTRAINT FK_StudentTransfers_Students
            FOREIGN KEY (StudentId) REFERENCES dbo.Students(StudentId),
        CONSTRAINT CK_StudentTransfers_DifferentGroups
            CHECK (FromGroup <> ToGroup)
    );
END;
