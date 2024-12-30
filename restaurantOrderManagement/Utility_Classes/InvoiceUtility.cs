using System;
using System.IO;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using restaurantOrderManagement.Models;
using restaurantOrderManagement.Utility_Classes;
using iText.Kernel.Pdf.Canvas;

public static class InvoiceUtility
{
    public static void GenerateBillInvoice(InvoiceGenerateModel invoiceDetails)
    {
        string directoryPath = "wwwroot/bills"; // Path to save the invoice
        string fileName = $"{GlobalVariables.InvoicePrefix}{invoiceDetails.InvoiceID}.pdf";
        string filePath = Path.Combine(directoryPath, fileName);

        try
        {
            // Ensure directory exists
            Directory.CreateDirectory(directoryPath);

            using (PdfWriter writer = new PdfWriter(filePath))
            using (PdfDocument pdf = new PdfDocument(writer))
            {
                pdf.AddNewPage();
                Document document = new Document(pdf);

                // Draw Page Border
                var canvas = new PdfCanvas(pdf.GetFirstPage());
                float pageWidth = pdf.GetDefaultPageSize().GetWidth();
                float pageHeight = pdf.GetDefaultPageSize().GetHeight();
                canvas.Rectangle(20, 20, pageWidth - 40, pageHeight - 40)
                      .SetStrokeColor(ColorConstants.BLACK)
                      .SetLineWidth(1) // Thin border
                      .Stroke();

                // Title
                document.Add(new Paragraph("Restaurant Bill")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(18)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetBackgroundColor(ColorConstants.BLUE)
                    .SetMarginBottom(10));

                // Restaurant Information
                document.Add(CreateParagraph(GlobalVariables.RestaurantName)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetFontSize(12)
                    .SetBold());
                document.Add(CreateParagraph($"GSTIN: {GlobalVariables.GSTNumber}"));
                document.Add(CreateParagraph(GlobalVariables.Address));
                document.Add(CreateParagraph($"Phone: {GlobalVariables.PhoneNumber}")
                    .SetMarginBottom(10));

                // Invoice Metadata
                Table metadataTable = new Table(new float[] { 1, 1 }).UseAllAvailableWidth();
                metadataTable.AddCell(CreateCell("Invoice No:", TextAlignment.LEFT, true));
                metadataTable.AddCell(CreateCell(invoiceDetails.InvoiceID.ToString(), TextAlignment.RIGHT, false));
                metadataTable.AddCell(CreateCell("Invoice Date:", TextAlignment.LEFT, true));
                metadataTable.AddCell(CreateCell(invoiceDetails.InvoiceDate.ToString("dd-MM-yyyy"), TextAlignment.RIGHT, false));
                metadataTable.AddCell(CreateCell("Order ID:", TextAlignment.LEFT, true));
                metadataTable.AddCell(CreateCell(invoiceDetails.OrderID.ToString(), TextAlignment.RIGHT, false));
                metadataTable.AddCell(CreateCell("Created By:", TextAlignment.LEFT, true));
                metadataTable.AddCell(CreateCell(invoiceDetails.CreatedBy, TextAlignment.RIGHT, false));
                document.Add(metadataTable);

                // Itemized Section
                Table itemsTable = new Table(new float[] { 1, 3, 1, 1, 1 }).UseAllAvailableWidth();
                itemsTable.SetMarginTop(10);
                itemsTable.SetBorder(new SolidBorder(ColorConstants.BLACK, 1)); // Add border to the entire table

                // Add header cells with borders
                itemsTable.AddHeaderCell(CreateHeaderCell("S.No").SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                itemsTable.AddHeaderCell(CreateHeaderCell("Item Name").SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                itemsTable.AddHeaderCell(CreateHeaderCell("Quantity").SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                itemsTable.AddHeaderCell(CreateHeaderCell("Unit Price").SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                itemsTable.AddHeaderCell(CreateHeaderCell("Total").SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));

                // Add data rows with borders
                int serialNumber = 1;
                foreach (var item in invoiceDetails.BillItems)
                {
                    itemsTable.AddCell(CreateCell(serialNumber.ToString(), TextAlignment.CENTER, false).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                    itemsTable.AddCell(CreateCell(item.foodname, TextAlignment.LEFT, false).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                    itemsTable.AddCell(CreateCell(item.Quantity.ToString("F2"), TextAlignment.CENTER, false).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                    itemsTable.AddCell(CreateCell(item.foodprice.ToString("F2"), TextAlignment.RIGHT, false).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                    itemsTable.AddCell(CreateCell((item.foodprice * item.Quantity).ToString("F2"), TextAlignment.RIGHT, false).SetBorder(new SolidBorder(ColorConstants.BLACK, 1)));
                    serialNumber++;
                }
                document.Add(itemsTable);

                // Summary Section
                Table summaryTable = new Table(new float[] { 3, 1 }).UseAllAvailableWidth();
                summaryTable.SetMarginTop(10);

                summaryTable.AddCell(CreateCell("Amount Excluding GST:", TextAlignment.LEFT, true));
                summaryTable.AddCell(CreateCell(invoiceDetails.AmountExcludingGST.ToString("F2"), TextAlignment.RIGHT, false));

                summaryTable.AddCell(CreateCell("GST Amount:", TextAlignment.LEFT, true));
                summaryTable.AddCell(CreateCell(invoiceDetails.GSTAmount.ToString("F2"), TextAlignment.RIGHT, false));

                summaryTable.AddCell(CreateCell("Total Amount (Incl. GST):", TextAlignment.LEFT, true));
                summaryTable.AddCell(CreateCell(invoiceDetails.AmountIncludingGST.ToString("F2"), TextAlignment.RIGHT, true));

                document.Add(summaryTable);

                // Footer
                document.Add(new Paragraph("Thank you for dining with us!")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(20)
                    .SetFontSize(12));
            }

            Console.WriteLine($"Invoice generated successfully: {filePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating invoice: {ex.Message}");
        }
    }

    private static Paragraph CreateParagraph(string content)
    {
        return new Paragraph(content);
    }

    private static Paragraph SetBold(this Paragraph paragraph)
    {
        return paragraph.SetFont(iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD));
    }

    private static Cell CreateCell(string content, TextAlignment alignment, bool isBold)
    {
        Cell cell = new Cell().Add(new Paragraph(content).SetTextAlignment(alignment));
        if (isBold)
        {
            cell.SetFont(iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD));
        }
        return cell.SetBorder(Border.NO_BORDER);
    }

    private static Cell CreateHeaderCell(string content)
    {
        return new Cell().Add(new Paragraph(content))
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFont(iText.Kernel.Font.PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD))
            .SetBackgroundColor(ColorConstants.BLUE)
            .SetFontColor(ColorConstants.WHITE)
            .SetBorder(new SolidBorder(ColorConstants.BLACK, 1)); // Header cell border
    }
}
