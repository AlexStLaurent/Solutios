Create Database ProjetSolutios
Go

Use ProjetSolutios
GO

Create Table Users(
"user_id" int PRIMARY KEY,
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
project_suivi nvarchar(max)
)
Go