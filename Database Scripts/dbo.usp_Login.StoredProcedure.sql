USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_Login]    Script Date: 1/3/2025 3:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_Login]
    @Id BIGINT = NULL,
    @MobileNumber BIGINT = 0,
    @Email NVARCHAR(50) = NULL,
    @UserId NVARCHAR(50) = NULL,
    @PassCode NVARCHAR(15) = NULL,
    @IsActive INT = 0,
    @UserType INT = 0,
    @Opmode INT = 0
AS
BEGIN
    BEGIN TRY
        -- Check operation mode
        IF @Opmode = 0
        BEGIN
            -- Perform SELECT operation using UserId
            SELECT *
            FROM UserMaster
            WHERE UserId = @UserId;
        END
    END TRY
    BEGIN CATCH
        IF (@@TRANCOUNT > 0)
        BEGIN
            ROLLBACK TRAN;
        END
        DECLARE @err_msg NVARCHAR(MAX);
        SET @err_msg = ERROR_MESSAGE();
        RAISERROR(@err_msg, 16, 1);
    END CATCH
END
GO
