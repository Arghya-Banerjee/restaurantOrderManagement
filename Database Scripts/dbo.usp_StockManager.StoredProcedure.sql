USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_StockManager]    Script Date: 1/3/2025 3:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_StockManager]
    @OpMode INT = 0,
    @ProductID INT = 0,
    @ProductName VARCHAR(100) = '',
    @CategoryID INT = 0,
    @CategoryName VARCHAR(20) = '',
    @SupplierName VARCHAR(100) = '',
    @Price DECIMAL(10, 2) = 0,
    @Unit VARCHAR(50) = ''
AS
BEGIN
    BEGIN TRY
        IF @OpMode = 0 -- Fetch All Details
        BEGIN
            SELECT p.ProductID, p.ProductName, c.CategoryName, s.SupplierName, p.Price, p.Unit
            FROM StockProductMaster p
            INNER JOIN StockSupplierMaster s ON p.SupplierID = s.SupplierID
            INNER JOIN StockCategoryMaster c ON p.CategoryID = c.CategoryID;
        END
        ELSE IF @OpMode = 1 -- Fetch All CategoryNames
        BEGIN
            SELECT c.CategoryID, c.CategoryName 
            FROM StockCategoryMaster c;
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
END;
GO
