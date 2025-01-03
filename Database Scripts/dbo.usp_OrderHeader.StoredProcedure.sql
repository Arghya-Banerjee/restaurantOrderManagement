USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_OrderHeader]    Script Date: 1/3/2025 4:09:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_OrderHeader]

@OpMode INT = 0,
@OrderID BIGINT = 0,
@OrderDate DATE = NULL,
@DeliveryMode INT = 0,
@TableNumber INT = 0,
@CreatedBy NVARCHAR(20) = NULL,
@CreatedOn DATETIME = NULL,
@OrderStatus INT = 0

AS
BEGIN
    BEGIN TRY
        IF @OpMode = 0 -- Insert header data
        BEGIN
            INSERT INTO OrderHeader (OrderDate, DeliveryMode, TableNumber, CreatedBy, CreatedOn, OrderStatus)
            VALUES (CONVERT(DATE, GETDATE()), @DeliveryMode, @TableNumber, @CreatedBy, GETDATE(), @OrderStatus);
        END
        ELSE IF @OpMode = 1 -- Get OrderID
        BEGIN
            SELECT OrderID
            FROM OrderHeader
            WHERE OrderStatus = 0 AND TableNumber = @TableNumber;
        END
        ELSE IF @OpMode = 2 -- Change order status
        BEGIN
            UPDATE OrderHeader
            SET OrderStatus = @OrderStatus
            WHERE TableNumber = @TableNumber AND OrderStatus = 0;
        END
		ELSE IF @OpMode = 3 -- Get OrderID and OrderTime
        BEGIN
            SELECT OrderID, CreatedOn
            FROM OrderHeader
            WHERE OrderStatus = 0 AND TableNumber = @TableNumber;
        END
		ELSE IF @OpMode = 4 -- Get Table Number
		BEGIN
			SELECT TableNumber
			FROM OrderHeader
			WHERE OrderID = @OrderID
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
