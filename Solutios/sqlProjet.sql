Create Database ProjetSolutios
Go

Use ProjetSolutios
GO

Create Table utilisateur(
id int PRIMARY KEY,
Nom varchar(50),
Prenom varchar(50),
email varchar(100),
mdp varchar(400),
detail nvarchar(max),
projet nvarchar(max)
)
Go

Create Table Projet(
id int PRIMARY KEY,
nom varchar(50),
debut date,
"status" int,
detail nvarchar(max),
suivi nvarchar(max)
)
Go