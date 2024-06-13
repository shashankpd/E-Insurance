use [E-Insurance-3]


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
	Location VARCHAR(255) NULL;
);

ALTER TABLE InsuranceAgentRegistration ADD Location VARCHAR(255) NULL;

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

8.-- Agent Commission table

CREATE TABLE Commissions (
    CommissionId INT IDENTITY(1,1) PRIMARY KEY,
    AgentId INT,
    PolicyId INT,
    CommissionRate DECIMAL(5, 2) NOT NULL,
    CommissionAmount DECIMAL(10, 2) NOT NULL,
    PaymentDate DATE NOT NULL,
    FOREIGN KEY (AgentId) REFERENCES InsuranceAgentRegistration(AgentId),
    FOREIGN KEY (PolicyId) REFERENCES Policies(PolicyId)
);

9.-- AgentPolicyCommissionRates table

CREATE TABLE AgentPolicyCommissionRates (
    AgentPolicyCommissionRateId INT IDENTITY(1,1) PRIMARY KEY,
    AgentId INT,
    PolicyId INT,
    CommissionRate DECIMAL(5, 2) NOT NULL,
    FOREIGN KEY (AgentId) REFERENCES InsuranceAgentRegistration(AgentId),
    FOREIGN KEY (PolicyId) REFERENCES Policies(PolicyId)
);

INSERT INTO AgentPolicyCommissionRates (AgentId, PolicyId, CommissionRate)
VALUES (3, 3, 0.05); 

10.--CommissionPayments Table

CREATE TABLE CommissionPayments (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    AgentId INT,
    Amount DECIMAL(10, 2) NOT NULL,
    PaymentDate DATE NOT NULL,
    FOREIGN KEY (AgentId) REFERENCES InsuranceAgentRegistration(AgentId)
);

EXEC sp_CalculateCommission @AgentId = 3, @PolicyId = 1, @PremiumAmount = 5000.00;

11.---premum calculation table----

CREATE TABLE PremiumCalculations (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PolicyId INT,
    CustomerAge INT,
    CoverageAmount DECIMAL(18, 2),
    PolicyType NVARCHAR(50),
    PaymentFrequency NVARCHAR(50),
    TermYears INT,
    PremiumAmount DECIMAL(10, 2),
    Status NVARCHAR(50) DEFAULT 'Pending',
    CreatedAt DATETIME DEFAULT GETDATE()
);



select * from AdminRegistration;
select * from CustomerRegistration;
select * from EmployeeRegistration;
select * from InsuranceAgentRegistration;
select * from Policies;
select * from CustomerPolicies;
select * from Payments;
select * from Commissions;
select * from AgentPolicyCommissionRates;
select * from CommissionPayments;
select * from PremiumCalculations;




-------------------------------------------------------------------------------------------------------------------------------
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
    
-----------------------------------------------------------------------------------------------------------------------------

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

ALTER PROCEDURE ViewCustomerPolicies
    @CustomerId INT
AS
BEGIN
    SELECT 
        cp.CustomerPolicyId,
        c.Name AS CustomerName,
        p.PolicyName,          -- Include PolicyName from Policies table
        pc.CoverageAmount, 
        pc.TermYears AS PolicyTerm, 
        pc.PolicyType, 
        pc.PaymentFrequency, 
        pc.PolicyId,
        pc.CreatedAt AS CalculationCreatedAt,
        pc.CustomerAge, 
        pc.PremiumAmount,
        a.Name AS AgentName, 
        ISNULL(a.Location, 'N/A') AS AgentLocation  -- Handle null locations
    FROM 
        CustomerPolicies cp
    INNER JOIN 
        PremiumCalculations pc ON cp.PolicyId = pc.PolicyId
    LEFT JOIN 
        InsuranceAgentRegistration a ON cp.AgentId = a.AgentId
    INNER JOIN 
        CustomerRegistration c ON cp.CustomerId = c.CustomerId
    INNER JOIN 
        Policies p ON cp.PolicyId = p.PolicyId    -- Join with Policies table to get PolicyName
    WHERE 
        cp.CustomerId = @CustomerId AND pc.Status = 'Purchased';
END;
----stored Procedure for gitting details of all the  customers-------
ALTER PROCEDURE ViewAllCustomerPolicies
AS
BEGIN
    SELECT 
	    
        cp.CustomerPolicyId,
		c.CustomerId,
        c.Name AS CustomerName,
        p.PolicyName,          -- Include PolicyName from Policies table
        pc.CoverageAmount, 
        pc.TermYears AS PolicyTerm, 
        pc.PolicyType, 
        pc.PaymentFrequency, 
        pc.PolicyId,
        pc.CreatedAt AS CalculationCreatedAt,
        pc.CustomerAge, 
        pc.PremiumAmount,
        a.Name AS AgentName, 
        ISNULL(a.Location, 'N/A') AS AgentLocation  -- Handle null locations
    FROM 
        CustomerPolicies cp
    INNER JOIN 
        PremiumCalculations pc ON cp.PolicyId = pc.PolicyId
    LEFT JOIN 
        InsuranceAgentRegistration a ON cp.AgentId = a.AgentId
    INNER JOIN 
        CustomerRegistration c ON cp.CustomerId = c.CustomerId
    INNER JOIN 
        Policies p ON cp.PolicyId = p.PolicyId    -- Join with Policies table to get PolicyName

		WHERE pc.Status = 'Purchased';
    
END;


----stored procedure for canceling the customers particular policy------

ALTER PROCEDURE DeletePolicy
    @CustomerPolicyId INT
AS
BEGIN
    BEGIN TRY
        -- Start a transaction
        BEGIN TRANSACTION;

        -- Delete the related records from the Payments table
        DELETE FROM Payments
        WHERE CustomerPolicyId = @CustomerPolicyId;

        -- Get the PolicyId associated with the CustomerPolicyId
        DECLARE @PolicyId INT;
        SELECT @PolicyId = PolicyId FROM CustomerPolicies WHERE CustomerPolicyId = @CustomerPolicyId;

        -- Delete the related records from the PremiumCalculations table
        DELETE FROM PremiumCalculations
        WHERE PolicyId = @PolicyId;

        -- Delete the policy from CustomerPolicies table
        DELETE FROM CustomerPolicies
        WHERE CustomerPolicyId = @CustomerPolicyId;

        -- Commit the transaction
        COMMIT TRANSACTION;

        PRINT 'Policy, related payments, and premium calculations deleted successfully.';
    END TRY
    BEGIN CATCH
        -- Rollback the transaction if an error occurs
        ROLLBACK TRANSACTION;

        -- Print error message
        PRINT 'An error occurred while deleting the policy, related payments, and premium calculations.';
        THROW;
    END CATCH;
END;
GO

-------------------------------------------------------------------------------------------------------------------------
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

-----------stored procedure for getallpayments---------------

CREATE PROCEDURE sp_GetAllPayments
AS
BEGIN
    -- Select all records from the Payments table
    SELECT PaymentId, CustomerPolicyId, PaymentDate, Amount, Status
    FROM Payments;
END;

-----stored procedure for getpaymentbyId,using customerid,joined to get customerid from customerplicy table which is refrencing customer id-----------

CREATE PROCEDURE sp_GetPaymentsByCustomerId
    @CustomerId INT
AS
BEGIN
    SELECT 
        p.PaymentId, 
        p.CustomerPolicyId, 
        p.PaymentDate, 
        p.Amount, 
        p.Status
    FROM 
        Payments p
    INNER JOIN 
        CustomerPolicies cp ON p.CustomerPolicyId = cp.CustomerPolicyId
    WHERE 
        cp.CustomerId = @CustomerId;
END;

-----Stored Pocedure for genrating Reciept after payment-------------

ALTER PROCEDURE sp_GenerateReceipt
    @PaymentId INT
AS
BEGIN
    SELECT 
        p.PaymentId,
        cp.CustomerPolicyId,
        cp.FirstName AS CustomerName,
        c.Email AS CustomerEmail,
        a.Name AS AgentName,
        pol.PolicyName,
        pol.CoverageAmount,
        p.PaymentDate,
        p.Amount,
        p.Status
    FROM 
        Payments p
    INNER JOIN 
        CustomerPolicies cp ON p.CustomerPolicyId = cp.CustomerPolicyId
    INNER JOIN 
        CustomerRegistration c ON cp.CustomerId = c.CustomerId
    LEFT JOIN 
        InsuranceAgentRegistration a ON cp.AgentId = a.AgentId
    INNER JOIN 
        Policies pol ON cp.PolicyId = pol.PolicyId
    WHERE 
        p.PaymentId = @PaymentId;
END;

------Stored Procedure for Calculating premiums----------

ALTER PROCEDURE CalculatePremium
    @PolicyId INT,
    @CustomerAge INT,
    @CoverageAmount DECIMAL(18, 2),
    @PolicyType NVARCHAR(50),
    @PaymentFrequency NVARCHAR(50),
    @TermYears INT
AS	
BEGIN
    DECLARE @Premium DECIMAL(10, 2)
    DECLARE @MonthlyRate DECIMAL(5, 4)
    DECLARE @TotalPremium DECIMAL(10, 2)

    -- Determine the monthly rate based on the policy type
    IF @PolicyType = 'Health'
    BEGIN
        SET @MonthlyRate = 0.02 / 12 -- Annual rate divided by 12 for monthly rate
    END
    ELSE IF @PolicyType = 'Life'
    BEGIN
        SET @MonthlyRate = 0.01 / 12 -- Annual rate divided by 12 for monthly rate
    END
    ELSE
    BEGIN
        SET @MonthlyRate = 0.005 / 12 -- Annual rate divided by 12 for monthly rate
    END

    -- Calculate the total premium based on the term length
    SET @TotalPremium = @CoverageAmount * @MonthlyRate * @TermYears * 12 + (@CustomerAge / 10.0)

    -- Adjust the premium based on the payment frequency
    IF @PaymentFrequency = 'Monthly'
    BEGIN
        SET @Premium = @TotalPremium / (@TermYears * 12)
    END
    ELSE IF @PaymentFrequency = 'Half-Yearly'
    BEGIN
        SET @Premium = @TotalPremium / (@TermYears * 2)
    END
    ELSE IF @PaymentFrequency = 'Yearly'
    BEGIN
        SET @Premium = @TotalPremium / @TermYears
    END
    ELSE
    BEGIN
        RAISERROR('Invalid Payment Frequency', 16, 1)
        RETURN
    END

    -- Insert the calculated premium into the PremiumCalculations table
    INSERT INTO PremiumCalculations (PolicyId, CustomerAge, CoverageAmount, PolicyType, PaymentFrequency, TermYears, PremiumAmount, Status)
    VALUES (@PolicyId, @CustomerAge, @CoverageAmount, @PolicyType, @PaymentFrequency, @TermYears, @Premium, 'Pending')

    -- Return the calculated premium
    SELECT @Premium AS Premium;
END;

------stored procedure for finalizing the purchase-------

CREATE PROCEDURE FinalizePurchase
AS
BEGIN
    DECLARE @Id INT

    -- Select the most recent calculation to finalize
    SELECT TOP 1 @Id = Id
    FROM PremiumCalculations
    WHERE Status = 'Pending'
    ORDER BY Id DESC

    -- If no pending calculations are found, raise an error
    IF @Id IS NULL
    BEGIN
        RAISERROR('No pending premium calculations found.', 16, 1)
        RETURN
    END

    -- Update the status to 'Purchased'
    UPDATE PremiumCalculations
    SET Status = 'Purchased'
    WHERE Id = @Id

    -- No need to return anything
END;




--------------------------------------------------------------------------------------------------------------------------
--------stored procedure for Agent Commission Calculation--------------

ALTER PROCEDURE sp_CalculateCommission
    @AgentId INT,
    @PolicyId INT,
    @PremiumAmount DECIMAL(10, 2)
AS
BEGIN
    DECLARE @CommissionRate DECIMAL(5, 2);
    DECLARE @CommissionAmount DECIMAL(10, 2);

    -- Get the commission rate for the agent and policy
    SELECT @CommissionRate = CommissionRate
    FROM AgentPolicyCommissionRates
    WHERE AgentId = @AgentId AND PolicyId = @PolicyId;

    -- Calculate the commission amount
    SET @CommissionAmount = @PremiumAmount * @CommissionRate;

    -- Insert the commission record into the Commissions table
    INSERT INTO Commissions (AgentId, PolicyId, CommissionRate, CommissionAmount, PaymentDate)
    VALUES (@AgentId, @PolicyId, @CommissionRate, @CommissionAmount, GETDATE());

    -- Return the calculated commission amount
    SELECT @CommissionAmount AS CommissionAmount;
END;



--------------Stored Procedure for view Particular Agents Commission using AgentID-----------

CREATE PROCEDURE sp_ViewCommissions
    @AgentId INT
AS
BEGIN
    -- Retrieve commission information
    SELECT * FROM Commissions WHERE AgentId = @AgentId;

END;

-------------------stored procedure for calculating totalAmount and updating into table(CommissionPayments)-------------
CREATE PROCEDURE sp_PayAgentCommission
    @AgentId INT
AS
BEGIN
    DECLARE @TotalCommission DECIMAL(10, 2);

    -- Calculate the total commission for the agent
    SELECT @TotalCommission = SUM(CommissionAmount)
    FROM Commissions
    WHERE AgentId = @AgentId AND PaymentDate = CAST(GETDATE() AS DATE);

    -- Insert the payment record into the CommissionPayments table
    INSERT INTO CommissionPayments (AgentId, Amount, PaymentDate)
    VALUES (@AgentId, @TotalCommission, GETDATE());

END;

