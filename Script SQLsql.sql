--Create Banco 

CREATE DATABASE [DBEspera]

--Create Table
USE [DBEspera]
GO
CREATE TABLE Espera (
 Id INT IDENTITY (1,1) NOT NULL PRIMARY KEY,
 TipoAtendimento INT NOT NULL,
 StatusPainel BIT NOT NULL,
 DtEmissao DATETIME2(7) NOT NULL,
);
GO
CREATE TABLE Atendimento (
 Id INT IDENTITY (1,1) NOT NULL,
 Mesa INT NOT NULL,
 IdEspera INT NOT NULL,
 DtAtendimento DATETIME2(7) NOT NULL,
 FOREIGN KEY (IdEspera) REFERENCES Espera(Id)
);
GO
CREATE TABLE Contagem (
 Id INT IDENTITY (1,1) NOT NULL,
 Ordem INT NOT NULL,
);
GO


--Insert values Table Espera
INSERT INTO [dbo].[Espera] VALUES (1, 1, '16-09-2022 00:37:06.376')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '17-09-2022 11:50:40.5040')
INSERT INTO [dbo].[Espera] VALUES (2, 1, '17-09-2022 15:18:09.189')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '17-09-2022 15:20:13.2013')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '17-09-2022 15:20:23.2023')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '17-09-2022 15:20:50.2050')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '17-09-2022 15:20:57.2057')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '17-09-2022 15:21:06.216')
INSERT INTO [dbo].[Espera] VALUES (2, 1, '17-09-2022 15:21:10.2110')
INSERT INTO [dbo].[Espera] VALUES (2, 1, '17-09-2022 15:21:47.2147')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '17-09-2022 15:30:43.3043')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '17-09-2022 15:36:06.366')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '17-09-2022 15:37:14.3714')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '17-09-2022 15:38:04.384')
INSERT INTO [dbo].[Espera] VALUES (1, 1, '18-09-2022 19:42:51.4251')
INSERT INTO [dbo].[Espera] VALUES (2, 1, '18-09-2022 19:43:07.437')

GO
--Insert values Table Atendimento

INSERT INTO [dbo].[Atendimento] VALUES (1, 1, '18-09-2022 11:49:42.4942')
INSERT INTO [dbo].[Atendimento] VALUES (2, 7, '18-09-2022 11:50:13.5013')
GO
--Insert values Table Contagem

INSERT INTO [dbo].[Contagem] VALUES (0)