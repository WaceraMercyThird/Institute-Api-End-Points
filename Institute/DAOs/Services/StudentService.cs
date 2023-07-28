using Institute.Actor;
using Institute.Datas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Xml.Linq;

namespace Institute.Datas.Services;

public class StudentService : IStudentService
{
    public readonly ApiDbContext _context;



    public StudentService(ApiDbContext context)
    {
        _context = context;
    }

    public async Task<List<Student>> GetStudents()
    {
        return await _context.Students.ToListAsync();
    }

    public async Task<Student> GetStudent(Guid id)
    {
        return await _context.Students.FindAsync(id);

    }

    public async Task<byte[]> GeneratePdf(List<Student> students)
    {
        // Create a new PDF document
        var document = new PdfDocument();

        // Add a new page to the document
        var page = document.AddPage();

        // Create a graphics object for the page
        var gfx = XGraphics.FromPdfPage(page);

        // Set up the font and text format for the report
        var titleFont = new XFont("Arial", 16, XFontStyle.Bold);
        var headerFont = new XFont("Arial", 12, XFontStyle.Bold);
        var contentFont = new XFont("Arial", 10);

        // Define left and right margins for the content area
        double leftMargin = 50;
        double rightMargin = 50;

        // Calculate available width for the content area
        double availableWidth = page.Width - (leftMargin + rightMargin);

        // Calculate the number of columns
        int numColumns = 4;

        // Calculate the initial column width based on the available width
        double columnWidth = availableWidth / numColumns;

        // Define the minimum column width to prevent excessive wrapping
        double minColumnWidth = 100;

        // If the calculated column width is less than the minimum, adjust the column widths
        if (columnWidth < minColumnWidth)
        {
            columnWidth = minColumnWidth;
            availableWidth = columnWidth * numColumns;
        }

        // Draw the report title in the center
        var reportTitle = "Institute Report";
        var titleRect = new XRect(leftMargin, 40, availableWidth, 30);
        gfx.DrawString(reportTitle, titleFont, XBrushes.Black, titleRect, XStringFormats.Center);

        // Draw the table headers
        double xOffset = leftMargin;
        var headerRect = new XRect(xOffset, 80, columnWidth, 30);
        gfx.DrawRectangle(XBrushes.LightGray, headerRect);
        gfx.DrawString("Name", headerFont, XBrushes.Black, headerRect, XStringFormats.CenterLeft);
        xOffset += columnWidth;

        headerRect = new XRect(xOffset, 80, columnWidth, 30);
        gfx.DrawRectangle(XBrushes.LightGray, headerRect);
        gfx.DrawString("Class Room", headerFont, XBrushes.Black, headerRect, XStringFormats.CenterLeft);
        xOffset += columnWidth;

        headerRect = new XRect(xOffset, 80, columnWidth, 30);
        gfx.DrawRectangle(XBrushes.LightGray, headerRect);
        gfx.DrawString("Books", headerFont, XBrushes.Black, headerRect, XStringFormats.CenterLeft);
        xOffset += columnWidth;

        headerRect = new XRect(xOffset, 80, columnWidth, 30);
        gfx.DrawRectangle(XBrushes.LightGray, headerRect);
        gfx.DrawString("Grades", headerFont, XBrushes.Black, headerRect, XStringFormats.CenterLeft);

        // Draw the report data
        var yOffset = 110;
        foreach (var stud in students)
        {
            xOffset = leftMargin;

            var studentRect = new XRect(xOffset, yOffset, columnWidth, 20);
            gfx.DrawString(stud.Name ?? "None", contentFont, XBrushes.Black, studentRect, XStringFormats.CenterLeft);
            xOffset += columnWidth;

            var classRoomRect = new XRect(xOffset, yOffset, columnWidth, 20);
            gfx.DrawString(stud.ClassRoom ?? "None", contentFont, XBrushes.Black, classRoomRect, XStringFormats.CenterLeft);
            xOffset += columnWidth;

            var booksRect = new XRect(xOffset, yOffset, columnWidth, 20);
            gfx.DrawString(stud.Novel ?? "None", contentFont, XBrushes.Black, booksRect, XStringFormats.CenterLeft);
            xOffset += columnWidth;

            var gradesRect = new XRect(xOffset, yOffset, columnWidth, 20);
            gfx.DrawString(stud.Grades.ToString() ?? "None", contentFont, XBrushes.Black, gradesRect, XStringFormats.CenterLeft);

            gfx.DrawLine(XPens.LightGray, leftMargin, yOffset + 20, page.Width - rightMargin, yOffset + 20); // Draw a horizontal separator line

            yOffset += 20; // Move to the next row
        }

        // Draw the footer with colorful design
        var footerText = "Generated on " + DateTime.Now.ToString("dd/MM/yyyy");
        var footerRect = new XRect(leftMargin, page.Height - 40, availableWidth, 40); // Reduce the width of the footer
        gfx.DrawRectangle(XBrushes.LightBlue, footerRect);
        gfx.DrawString(footerText, contentFont, XBrushes.Black, footerRect, XStringFormats.Center);

        // Save the PDF to a memory stream
        var memoryStream = new System.IO.MemoryStream();
        document.Save(memoryStream);


        // Return the PDF as a byte array
        return memoryStream.ToArray();
    }








}

