USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_Menu]    Script Date: 1/3/2025 4:09:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_Menu]
@Opmode INT = 0,
@menuid INT = 0,
@foodname NVARCHAR(50) = NULL,
@fooddescription NVARCHAR(MAX) = NULL,
@foodcategoryid NCHAR(10) = 0,
@foodprice DECIMAL(18, 0) = 0,
@foodavailable INT = 1,
@catid INT = 0,
@categoryname NVARCHAR(50) = NULL,
@createdby NVARCHAR(50) = NULL,
@foodpictureurl NVARCHAR(MAX) = NULL

AS
BEGIN
    BEGIN TRY
        IF @Opmode = 0 -- Read all data for menu list
        BEGIN
            SELECT mt.menuid, mt.foodname, mt.fooddescription, ct.categoryname, mt.foodprice, mt.foodpictureurl
            FROM menutbl mt 
            LEFT JOIN categorymastertbl ct ON mt.foodcategoryid = ct.catid
            WHERE mt.foodavailable = 1
            ORDER BY ct.categoryname;
        END
        ELSE IF @Opmode = 1 -- Read categories
        BEGIN
            SELECT DISTINCT ct.categoryname
            FROM menutbl mt
            LEFT JOIN categorymastertbl ct ON mt.foodcategoryid = ct.catid
            WHERE mt.foodavailable = 1
            ORDER BY ct.categoryname ASC;
        END
        ELSE IF @Opmode = 2 -- Filter menu items by category
        BEGIN
            SELECT mt.foodname, mt.fooddescription, ct.categoryname, mt.foodprice
            FROM menutbl mt 
            LEFT JOIN categorymastertbl ct ON mt.foodcategoryid = ct.catid
            WHERE mt.foodavailable = 1 AND ct.categoryname = @categoryname;
        END
        ELSE IF @Opmode = 3 -- Insert new menu
        BEGIN
            INSERT INTO menutbl(foodname, fooddescription, foodcategoryid, foodprice, foodavailable, CreatedBy, CreatedOn)
            VALUES (@foodname, @fooddescription, @foodcategoryid, @foodprice, @foodavailable, @createdby, GETDATE());
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
