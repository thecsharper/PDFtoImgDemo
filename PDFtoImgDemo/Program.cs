using System.Drawing;
using System.Drawing.Imaging;
using PdfiumViewer;

namespace PdfToPngConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string pdfPath = "sample.pdf"; // Replace with your PDF file path
            string outputDir = "OutputImages"; // Directory to save PNGs

            ConvertPdfToPng(pdfPath, outputDir);
            Console.WriteLine("PDF conversion completed!");
        }

        static void ConvertPdfToPng(string pdfPath, string outputDir)
        {
            if (!File.Exists(pdfPath))
            {
                Console.WriteLine("PDF file not found.");
                return;
            }

            // Ensure output directory exists
            Directory.CreateDirectory(outputDir);

            using (var pdfDocument = PdfDocument.Load(pdfPath))
            {
                for (int i = 0; i < pdfDocument.PageCount; i++)
                {
                    using (var image = RenderPage(pdfDocument, i, 300)) // Render at 300 DPI
                    {
                        string outputPath = Path.Combine(outputDir, $"Page-{i + 1}.png");
                        image.Save(outputPath, ImageFormat.Png);
                        Console.WriteLine($"Saved: {outputPath}");
                    }
                }
            }
        }

        static Image RenderPage(PdfDocument document, int pageNumber, int dpi)
        {
            var pageSize = document.PageSizes[pageNumber];
            float scale = dpi / 72f; // 72 DPI is the default for PDFs
            int width = (int)(pageSize.Width * scale);
            int height = (int)(pageSize.Height * scale);

            return document.Render(pageNumber, width, height, dpi, dpi, true);
        }
    }
}
