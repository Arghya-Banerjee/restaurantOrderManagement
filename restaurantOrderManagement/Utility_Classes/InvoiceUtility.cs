using System;
using System.IO;
using System.Collections.Generic;
using iText.Kernel.Pdf;
using iText.Kernel.Colors;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Barcodes;
using iText.Kernel.Pdf.Canvas;
using restaurantOrderManagement.Models;
using restaurantOrderManagement.Utility_Classes;

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

                // Title and Header
                Table headerTable = new Table(new float[] { 1 }).UseAllAvailableWidth();
                headerTable.SetMarginBottom(10);

                // Left side: Restaurant Name and GSTIN
                Cell leftCell = new Cell()
                    .Add(new Paragraph(GlobalVariables.RestaurantName).SetFontSize(14).SetBold())
                    .Add(new Paragraph($"GSTIN: {GlobalVariables.GSTNumber}").SetFontSize(10))
                    .Add(new Paragraph(GlobalVariables.Address).SetFontSize(10))
                    .Add(new Paragraph($"Phone: {GlobalVariables.PhoneNumber}").SetFontSize(10));
                leftCell.SetBorder(Border.NO_BORDER);
                headerTable.AddCell(leftCell);

                document.Add(headerTable);

                // Add the Barcode in Top-Right using Absolute Positioning
                Image barcodeImage = CreateBarcode(pdf, invoiceDetails.OrderID.ToString(), true);
                barcodeImage.SetFixedPosition(pageWidth - 140, pageHeight - 65); // Adjust position
                barcodeImage.ScaleAbsolute(100, 20); // Scale to desired size
                document.Add(barcodeImage);

                // Invoice Metadata
                Table metadataTable = new Table(new float[] { 1, 3 }).UseAllAvailableWidth();

                metadataTable.AddCell(CreateCell("Invoice No:", TextAlignment.LEFT, true));
                metadataTable.AddCell(CreateCell(FormatID(invoiceDetails.InvoiceID), TextAlignment.RIGHT, false));

                metadataTable.AddCell(CreateCell("Invoice Date:", TextAlignment.LEFT, true));
                metadataTable.AddCell(CreateCell(invoiceDetails.InvoiceDate.ToString("dd-MM-yyyy"), TextAlignment.RIGHT, false));

                metadataTable.AddCell(CreateCell("Order ID:", TextAlignment.LEFT, true));
                metadataTable.AddCell(CreateCell(FormatID(invoiceDetails.OrderID), TextAlignment.RIGHT, false));

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

    private static Image CreateBarcode(PdfDocument pdf, string value, bool hideText = false)
    {
        Barcode128 barcode = new Barcode128(pdf);
        barcode.SetCode(value);
        barcode.SetBarHeight(40); // Set height
        barcode.SetX(2.0f); // Adjust width scaling for longer barcode

        if (hideText)
        {
            barcode.SetFont(null); // Removes the numerical text below the barcode
        }

        return new Image(barcode.CreateFormXObject(pdf));
    }

    private static string FormatID(long id)
    {
        return $"#{id:D6}"; // Add '#' and pad to 6 digits
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
