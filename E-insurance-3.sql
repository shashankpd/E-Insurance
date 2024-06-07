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


CREATE TABLE CustomerRegistration (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(20),
	Role VARCHAR(20),
	CreatedDate DATETIME NOT NULL,
   
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

    AnnualIncome DECIMAL(10, 2) NOT NULL,
    DateOfBirth DATE, -- updated to DATE data type
    FirstName VARCHAR(255),
    LastName VARCHAR(255),
    Gender VARCHAR(50),
    MobileNumber VARCHAR(20),
    Address VARCHAR(250),

    FOREIGN KEY (AgentId) REFERENCES InsuranceAgentRegistration(AgentId),
    FOREIGN KEY (CustomerId) REFERENCES CustomerRegistration(CustomerId),
    FOREIGN KEY (PolicyId) REFERENCES Policies(PolicyId)
);

7.-- Payments table

CREATE TABLE Payments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerPolicyId INT NOT NULL,
    PaymentDate DATE NOT NULL,
    Amount DECIMAL(10, 2) NOT NULL,
    Status VARCHAR(20) DEFAULT 'Pending', -- Status of the payment (e.g., Pending, Processed)
    CONSTRAINT FK_CustomerPolicyId FOREIGN KEY (CustomerPolicyId) REFERENCES CustomerPolicies(CustomerPolicyId)
);

	drop table Payments



select * from AdminRegistration;
select * from CustomerRegistration;
select * from EmployeeRegistration;
select * from InsuranceAgentRegistration;
select * from Policies;
select * from CustomerPolicies;
select * from Payments;




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

ALTER PROCEDURE sp_PurchasePolicy
    @CustomerId INT,
    @PolicyId INT,
    @PurchaseDate DATE,
    @AgentId INT,
    @AnnualIncome DECIMAL(10, 2),
    @DateOfBirth DATE,
    @FirstName VARCHAR(255),
    @LastName VARCHAR(255),
    @Gender VARCHAR(50),
    @MobileNumber VARCHAR(20),
	@Address  VARCHAR(250)
AS
BEGIN
    INSERT INTO CustomerPolicies (CustomerId, PolicyId, PurchaseDate, AgentId, AnnualIncome, DateOfBirth, FirstName, LastName, Gender, MobileNumber,Address)
    VALUES (@CustomerId, @PolicyId, @PurchaseDate, @AgentId, @AnnualIncome, @DateOfBirth, @FirstName, @LastName, @Gender, @MobileNumber,@Address);
END;

-----stored procedure for view customers purchase details----------

CREATE PROCEDURE ViewCustomerPolicies
    @CustomerId INT
AS
BEGIN
    SELECT 
        cp.CustomerPolicyId, 
        c.Name AS CustomerName, 
        a.AgentId, 
        a.Name AS AgentName, 
        p.PolicyName, 
        p.Description, 
        p.PolicyType, 
        cp.PurchaseDate, 
        p.CoverageAmount, 
        p.Premium, 
        p.TermLength, 
        p.EntryAge, 
        p.Status
    FROM 
        CustomerPolicies cp
    INNER JOIN 
        CustomerRegistration c ON cp.CustomerId = c.CustomerId
    LEFT JOIN 
        InsuranceAgentRegistration a ON cp.AgentId = a.AgentId
    INNER JOIN 
        Policies p ON cp.PolicyId = p.PolicyId
    WHERE 
        cp.CustomerId = @CustomerId;
END;

----stored procedure for canceling the customers particular policy------

CREATE PROCEDURE DeletePolicy
    @CustomerPolicyId INT
AS
BEGIN
    -- Delete the policy based on the CustomerPolicyId
    DELETE FROM CustomerPolicies
    WHERE CustomerPolicyId = @CustomerPolicyId;
END;

------------------------------------------------------------------------
-----stored procedure for adding Payment--------------

ALTER PROCEDURE sp_ValidatePayment
    @CustomerPolicyId INT,
    @PaymentDate DATE,
    @Amount DECIMAL(10, 2)
AS
BEGIN
    -- Validate payment information
    IF @Amount <= 0
    BEGIN
        -- Payment amount is not valid
        RAISERROR ('Invalid payment amount. Payment is not valid.', 16, 1);
        RETURN; -- Exit the stored procedure
    END

    -- Payment is valid, insert the payment
    INSERT INTO Payments (CustomerPolicyId, PaymentDate, Amount)
    VALUES (@CustomerPolicyId, @PaymentDate, @Amount);
    
    -- Check if the payment was inserted
    IF @@ROWCOUNT = 0
    BEGIN
        -- No rows were inserted, indicating that the payment insertion failed
        RAISERROR ('Payment could not be inserted.', 16, 1);
        RETURN; -- Exit the stored procedure
    END

    -- Update the payment status to 'Processed'
    UPDATE Payments
    SET Status = 'Processed'
    WHERE CustomerPolicyId = @CustomerPolicyId
      AND PaymentDate = @PaymentDate
      AND Amount = @Amount;
END;

EXEC sp_ValidatePayment 1, '2024-06-06', 100.00;

