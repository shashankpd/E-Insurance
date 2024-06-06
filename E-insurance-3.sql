use [E-Insurance-3];


CREATE TABLE AdminRegistration (
    AdminId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20),
	Role VARCHAR(20),
    CreatedDate DATETIME NOT NULL
);

ALTER TABLE AdminRegistration
ADD CONSTRAINT DF_AdminRegistration_CreatedDate DEFAULT GETDATE() FOR CreatedDate;

CREATE TABLE CustomerRegistration (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20),
	Role VARCHAR(20),
	CreatedDate DATETIME NOT NULL,
    Age int NOT NULL,
    Address VARCHAR(255) NOT NULL,
);


CREATE TABLE InsuranceAgentRegistration (
    AgentId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20),
	Role VARCHAR(20),
    CreatedDate DATETIME NOT NULL
);

CREATE TABLE EmployeeRegistration (
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20),
	Role VARCHAR(20),
    CreatedDate DATETIME NOT NULL
);

5.-- Policies table

CREATE TABLE Policies (
    PolicyId INT IDENTITY(1,1) PRIMARY KEY,
    PolicyName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    PolicyType NVARCHAR(50) NOT NULL,
    TermLength INT NOT NULL,
    CoverageAmount DECIMAL(18, 2) NOT NULL,
	Premium DECIMAL(10, 2) NOT NULL,
	EntryAge INT NOT NULL,
    Status NVARCHAR(50) NOT NULL
);

6.-- CustomerPolicies table

CREATE TABLE CustomerPolicies (
    CustomerPolicyId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT,
    PolicyId INT,
    PurchaseDate DATE NOT NULL,
	AgentId INT,
    FOREIGN KEY (AgentId) REFERENCES InsuranceAgentRegistration(AgentId),
    FOREIGN KEY (CustomerId) REFERENCES CustomerRegistration(CustomerId),
    FOREIGN KEY (PolicyId) REFERENCES Policies(PolicyId)
);




select * from AdminRegistration;
select * from CustomerRegistration;
select * from EmployeeRegistration;
select * from InsuranceAgentRegistration;
select * from Policies;
select * from CustomerPolicies;


-------------------------------------------------------------------------------------------
------stored procedure for policy creation------

ALTER PROCEDURE CreatePolicy
    @PolicyName NVARCHAR(100),
    @Description NVARCHAR(255),
    @PolicyType NVARCHAR(50),
    @TermLength INT,
    @CoverageAmount DECIMAL(18, 2),
    @Premium DECIMAL(10, 2),
    @EntryAge INT
AS
BEGIN
    INSERT INTO Policies (PolicyName, Description, PolicyType, TermLength, CoverageAmount, Premium, EntryAge, Status)
    VALUES (@PolicyName, @Description, @PolicyType, @TermLength, @CoverageAmount, @Premium, @EntryAge, 'Created');
END;


--------trigger after creation-------
CREATE TRIGGER trg_AfterPolicyInsert
ON Policies
AFTER INSERT
AS
BEGIN
    UPDATE Policies
    SET Status = 'Active'
    WHERE PolicyId IN (SELECT PolicyId FROM inserted);
END;

-----procedure for viewig----
Alter PROCEDURE ViewPolicy
AS
BEGIN
    -- Retrieve policy information
    SELECT * FROM Policies;
END;

EXEC CreatePolicy 
    @PolicyName = 'Health Insurance', 
    @Description = 'Comprehensive health coverage', 
    @PolicyType = 'Health', 
    @TermLength = 12, 
    @CoverageAmount = 100000.00,
	@Premium=3000,
	@EntryAge=18;

	EXEC ViewPolicy 
    
------------------------------------------------------------------

----stored procedure for policy purchase------------

CREATE PROCEDURE sp_PurchasePolicy
    @CustomerId INT,
    @PolicyId INT,
    @PurchaseDate DATE,
    @AgentId INT
AS
BEGIN
    INSERT INTO CustomerPolicies (CustomerId, PolicyId, PurchaseDate, AgentId)
    VALUES (@CustomerId, @PolicyId, @PurchaseDate, @AgentId);
END;

-----stored procedure for view customers purchase details----------

CREATE PROCEDURE ViewCustomerPolicies
    @CustomerId INT
AS
BEGIN
    SELECT cp.CustomerPolicyId, c.Name AS CustomerName, a.AgentId, p.PolicyName, p.Description, p.PolicyType, 
           cp.PurchaseDate, cp.PremiumAmount, p.CoverageAmount, p.Premium, p.TermLength, p.EntryAge, p.Status
    FROM CustomerPolicies cp
    INNER JOIN CustomerRegistration c ON cp.CustomerId = c.CustomerId
    LEFT JOIN InsuranceAgentRegistration a ON cp.AgentId = a.AgentId
    INNER JOIN Policies p ON cp.PolicyId = p.PolicyId
    WHERE cp.CustomerId = @CustomerId;
END;