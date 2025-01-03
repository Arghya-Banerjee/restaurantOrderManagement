USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_User]    Script Date: 1/3/2025 4:09:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_User]

@OpMode INT = 0,
@Id BIGINT = 0,
@UserID NVARCHAR(50) = NULL,
@Email NVARCHAR(50) = NULL,
@MobileNumber BIGINT = 0,
@UserType INT = 0

AS
BEGIN
	IF @OpMode = 0 -- Get UserID of all Users according to UserType
	BEGIN
		SELECT UserID 
		FROM UserMaster
		WHERE UserType = @UserType;
	END
END

GO
