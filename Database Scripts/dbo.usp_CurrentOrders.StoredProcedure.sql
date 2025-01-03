USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_CurrentOrders]    Script Date: 1/3/2025 4:09:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_CurrentOrders]

@OpMode INT = 0,
@OrderID BIGINT = 0,
@CreatedBy NVARCHAR(20) = NULL,
@TableNumber INT = 0,
@OrderStatus INT = 0

AS
BEGIN
    BEGIN TRY
        IF @OpMode = 1 -- Get current Orders
        BEGIN
            SELECT OrderID, TableNumber, CreatedBy, OrderStatus
            FROM OrderHeader
            WHERE OrderStatus = 0;
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
