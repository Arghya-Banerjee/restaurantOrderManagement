USE [resturentfood]
GO
/****** Object:  StoredProcedure [dbo].[usp_Invoice]    Script Date: 1/3/2025 3:24:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[usp_Invoice]

@Opmode INT = 0,
@InvoiceID BIGINT = 0,
@OrderID BIGINT = 0,
@InvoiceDate DATETIME = NULL,
@AmountExcludingGST DECIMAL(18, 2) = 0,
@GSTAmount DECIMAL(18, 2) = 0,
@AmountIncludingGST DECIMAL(18, 2) = 0,
@PaymentMode INT = 0,
@CreatedBy NVARCHAR(20) = NULL,
@CreatedOn DATETIME = NULL

AS
BEGIN
	IF @OpMode = 0
	BEGIN
		SELECT TOP 1 InvoiceID
		FROM OrderInvoice
		WHERE OrderID = @OrderID;
	END
	ELSE IF @Opmode = 1 -- Insert Invoice Data
	BEGIN
		INSERT INTO OrderInvoice (InvoiceDate, OrderID, AmountExcludingGST, GSTAmount, AmountIncludingGST, PaymentMode, CreatedBy, CreatedOn)
		VALUES (CONVERT(DATE, @InvoiceDate), @OrderID, @AmountExcludingGST, @GSTAmount, @AmountIncludingGST, @PaymentMode, @CreatedBy, @CreatedOn);
	END
	ELSE IF @Opmode = 2 -- Get Invoice Details
	BEGIN
		SELECT InvoiceDate, AmountExcludingGST, GSTAmount, AmountIncludingGST, PaymentMode, CreatedBy, CreatedOn
		FROM OrderInvoice
		WHERE InvoiceID = @InvoiceID;
	END
END
GO
