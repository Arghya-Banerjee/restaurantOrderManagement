USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_OrderDetails]    Script Date: 1/3/2025 3:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_OrderDetails]

@OpMode INT = 0,
@OrderDetailID BIGINT = 0,
@OrderID BIGINT = 0,
@MenuID INT = 0,
@Quantity DECIMAL(18, 0) = 0,
@CreatedBy NVARCHAR(20) = '',
@CreatedOn DATETIME = NULL

AS
BEGIN
    BEGIN TRY
        IF @OpMode = 0 -- Insert item details
        BEGIN
            INSERT INTO OrderDetail (OrderID, MenuID, Quantity, CreatedBy, CreatedOn)
            VALUES (@OrderID, @MenuID, @Quantity, @CreatedBy, GETDATE());
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
