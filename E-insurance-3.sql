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
    Age int NOT NULL,
    Address VARCHAR(255) NOT NULL,
    AgentId INT,
    FOREIGN KEY (AgentId) REFERENCES InsuranceAgentRegistration(AgentId)
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


select * from AdminRegistration;
select * from CustomerRegistration;
select * from EmployeeRegistration;
select * from InsuranceAgentRegistration;
