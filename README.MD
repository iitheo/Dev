#Upload file to database#

This is a .NET MVC 5 project.


Objective
----------
The objective of this project is to upload and download files to your database.


Features
---------
It supports pdf attachments not more than 1MB. But the source code can easily be edited to accept other file types.
The file size limit can also be changed from the source code.
It supports dynamic number of file attachments. You can add or a remove a file attachment from the front end.
This is an MVC 5 project.


How to install
---------------
1. Download or clone the project.
2. The project folder 'CMK.FiletoDBMVC' contains the web application folder.
3. Open the file 'filetodb_script.txt'
4. Create a Database 'Customers' in your Microsoft SQL Server Database.
5. Run the script to create table in step 3.
6. Create the table structure as shown in the text file OR Copy and paste the following in your SQL Worksheet in 
SQL Server Management studio.

USE [Customers]
GO

/****** Object:  Table [dbo].[tblFilestoDb]    Script Date: 18-Feb-19 8:47:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblFilestoDb](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [varchar](200) NULL,
	[FileSize] [int] NULL,
	[ContentType] [varchar](200) NULL,
	[FileExtension] [varchar](10) NULL,
	[FileContent] [varbinary](max) NULL,
 CONSTRAINT [PK_tblFilestoDb] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO