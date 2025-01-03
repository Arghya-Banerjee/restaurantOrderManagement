USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_OrderHistory]    Script Date: 1/3/2025 4:09:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_OrderHistory]
@OpMode INT = 0,
@UserID NVARCHAR(50) = 'ALL',
@StartDate DATETIME = NULL,
@EndDate DATETIME = NULL,
@OrderID BIGINT = 0,
@InvoiceID BIGINT = 0,
@CreatedBy NVARCHAR(50) = NULL,
@CreatedOn DATETIME = NULL,
@AmountExcludingGST DECIMAL(18, 2) = 0,
@GSTAmount DECIMAL(18, 2) = 0,
@AmountIncludingGST DECIMAL(18, 2) = 0,
@PaymentMode INT = 0,
@ViewType INT = 0
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validate ViewType
        IF @ViewType NOT IN (0, 1)
        BEGIN
            RAISERROR('Invalid ViewType. Must be 0 (Aggregate) or 1 (Individual).', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        -- Fetch Order History
        IF @OpMode = 0
        BEGIN
            IF @ViewType = 0 -- Aggregate
            BEGIN
                IF @UserID = 'ALL'
                BEGIN
                    SELECT 
                        SUM(AmountExcludingGST) AS AmountExcludingGST,
                        SUM(AmountIncludingGST) AS AmountIncludingGST,
                        SUM(GSTAmount) AS GSTAmount,
                        PaymentMode,
                        CreatedBy
                    FROM OrderInvoice
                    WHERE (CreatedOn >= @StartDate OR @StartDate IS NULL)
                      AND (CreatedOn <= @EndDate OR @EndDate IS NULL)
                      --AND (PaymentMode = @PaymentMode OR @PaymentMode = 0)
                    GROUP BY CreatedBy, PaymentMode;
                END
                ELSE
                BEGIN
                    SELECT 
                        SUM(AmountExcludingGST) AS AmountExcludingGST,
                        SUM(AmountIncludingGST) AS AmountIncludingGST,
                        SUM(GSTAmount) AS GSTAmount,
                        PaymentMode
                    FROM OrderInvoice
                    WHERE CreatedBy = @UserID
                      AND (CreatedOn >= @StartDate OR @StartDate IS NULL)
                      AND (CreatedOn <= @EndDate OR @EndDate IS NULL)
                      --AND (PaymentMode = @PaymentMode OR @PaymentMode = 0)
                    GROUP BY PaymentMode;
                END
            END
            ELSE IF @ViewType = 1 -- Individual
            BEGIN
                IF @UserID = 'ALL'
                BEGIN
                    SELECT 
                        AmountExcludingGST,
                        AmountIncludingGST,
                        GSTAmount,
                        PaymentMode,
                        CreatedBy,
						CreatedOn
                    FROM OrderInvoice
                    WHERE (CreatedOn >= @StartDate OR @StartDate IS NULL)
                      AND (CreatedOn <= @EndDate OR @EndDate IS NULL)
                      --AND (PaymentMode = @PaymentMode OR @PaymentMode = 0)
					ORDER BY CreatedOn;
                END
                ELSE
                BEGIN
                    SELECT 
                        AmountExcludingGST,
                        AmountIncludingGST,
                        GSTAmount,
                        PaymentMode,
                        CreatedBy,
						CreatedOn
                    FROM OrderInvoice
                    WHERE CreatedBy = @UserID
                      AND (CreatedOn >= @StartDate OR @StartDate IS NULL)
                      AND (CreatedOn <= @EndDate OR @EndDate IS NULL)
                      --AND (PaymentMode = @PaymentMode OR @PaymentMode = 0)
					ORDER BY CreatedOn;
                END
            END
        END

        COMMIT TRANSACTION;
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
