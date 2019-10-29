Create Database ProjetSolutios
Go

Use ProjetSolutios
	GO

Create Table Users(
"user_id" int PRIMARY KEY Identity,
"user_name" varchar(50),
"user_firstName" varchar(50),
"user_email" varchar(100),
"user_role" varchar(100),
"user_mdp" varchar(400),
"user_phone" varchar(12),
"user_address" varchar(100),
"user_address2" varchar(100),
"user_zipcode" varchar(10),
"user_city" varchar(50),
"user_province" varchar(20),
"user_projet" nvarchar(max)
)
Go

Create Table Project(
project_id int PRIMARY KEY,
project_name varchar(50),
project_debut date,
project_fin date,
project_status int,
project_soumission nvarchar(max),
)
Go

Create Table FollowUp(
FU_id int PRIMARY KEY,
FU_Date date,
FU_Info nvarchar(max)
)
GO

Create Table ProjectFollowUP(
PF_FollowUp_id int FOREIGN KEY REFERENCES FollowUp(FU_id),
PF_Project_id int FOREIGN KEY REFERENCES Project(Project_id)
)
GO

Create Table Expense(
Expense_id int PRIMARY KEY,
Expense_Date date,
Expense_Info nvarchar(max)
)
GO

Create Table ProjectExpense(
PE_Expense_id int FOREIGN KEY REFERENCES Expense(Expense_id),
PE_Project_id int FOREIGN KEY REFERENCES Project(Project_id)
)
GO




