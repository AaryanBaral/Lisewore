CREATE PROCEDURE GetAllEmployees
AS
BEGIN
    SELECT EmployeeId, FirstName, LastName, Email, Department, HireDate FROM Employees;
END
GO

CREATE PROCEDURE GetEmployeeById
    @Id NVARCHAR(50)
AS
BEGIN
    SELECT EmployeeId, FirstName, LastName, Email, Department, HireDate 
    FROM Employees WHERE EmployeeId = @Id;
END
GO

CREATE PROCEDURE AddEmployee
    @EmployeeId NVARCHAR(50),
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Department NVARCHAR(100),
    @HireDate DATETIME
AS
BEGIN
    INSERT INTO Employees(EmployeeId, FirstName, LastName, Email, Department, HireDate)
    VALUES (@EmployeeId, @FirstName, @LastName, @Email, @Department, @HireDate);
END
GO

CREATE PROCEDURE UpdateEmployee
    @EmployeeId NVARCHAR(50),
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Department NVARCHAR(100),
    @HireDate DATETIME
AS
BEGIN
    UPDATE Employees
    SET FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        Department = @Department,
        HireDate = @HireDate
    WHERE EmployeeId = @EmployeeId;
END
GO

CREATE PROCEDURE DeleteEmployee
    @EmployeeId NVARCHAR(50)
AS
BEGIN
    DELETE FROM Employees WHERE EmployeeId = @EmployeeId;
END
GO

CREATE PROCEDURE GetEmployeeByEmail
    @Email NVARCHAR(100)
AS
BEGIN
    SELECT EmployeeId, FirstName, LastName, Email, Department, HireDate 
    FROM Employees WHERE Email = @Email;
END
GO






