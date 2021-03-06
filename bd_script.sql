USE [master]
GO
/****** Object:  Database [ControleTarefas]    Script Date: 15/05/2019 23:38:55 ******/
CREATE DATABASE [ControleTarefas]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ControleTarefas', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ControleTarefas.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ControleTarefas_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\ControleTarefas_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ControleTarefas] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ControleTarefas].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ControleTarefas] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ControleTarefas] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ControleTarefas] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ControleTarefas] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ControleTarefas] SET ARITHABORT OFF 
GO
ALTER DATABASE [ControleTarefas] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ControleTarefas] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ControleTarefas] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ControleTarefas] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ControleTarefas] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ControleTarefas] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ControleTarefas] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ControleTarefas] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ControleTarefas] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ControleTarefas] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ControleTarefas] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ControleTarefas] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ControleTarefas] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ControleTarefas] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ControleTarefas] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ControleTarefas] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ControleTarefas] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ControleTarefas] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ControleTarefas] SET  MULTI_USER 
GO
ALTER DATABASE [ControleTarefas] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ControleTarefas] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ControleTarefas] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ControleTarefas] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ControleTarefas] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ControleTarefas] SET QUERY_STORE = OFF
GO
USE [ControleTarefas]
GO
/****** Object:  Table [dbo].[FimTarefas]    Script Date: 15/05/2019 23:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FimTarefas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTarefa] [int] NULL,
	[dataHora] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InicioTarefas]    Script Date: 15/05/2019 23:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InicioTarefas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTarefa] [int] NULL,
	[dataHora] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SessaoTarefas]    Script Date: 15/05/2019 23:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SessaoTarefas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdInicioTarefas] [int] NULL,
	[IdFimTarefas] [int] NULL,
	[Ativo] [bit] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tarefas]    Script Date: 15/05/2019 23:38:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tarefas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NULL
) ON [PRIMARY]
GO
USE [master]
GO
ALTER DATABASE [ControleTarefas] SET  READ_WRITE 
GO
