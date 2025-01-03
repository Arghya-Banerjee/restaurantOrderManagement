USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_BillItems]    Script Date: 1/3/2025 3:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_BillItems]

@OpMode INT = 0,
@TableNumber INT = 0,
@foodname NVARCHAR(50) = NULL,
@foodprice DECIMAL(18, 0) = 0,
@Quantity INT = 0

AS
BEGIN
    BEGIN TRY
        IF @OpMode = 0 -- Get all current orders according to tableNumber
        BEGIN
            SELECT m.foodname, m.foodprice, od.Quantity
			FROM OrderHeader oh
			INNER JOIN OrderDetail od ON oh.OrderID = od.OrderID
			INNER JOIN menutbl m ON m.menuid = od.MenuID
			WHERE oh.TableNumber = @TableNumber AND oh.OrderStatus = 0;
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
