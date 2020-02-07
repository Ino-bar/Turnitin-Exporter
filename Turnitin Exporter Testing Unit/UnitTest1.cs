using System;
using Xunit;
using Turnitin_Exporter;
using System.Windows.Forms;
using System.Linq;
using iText.Kernel.Pdf;
using System.Text;
using System.Collections.Generic;

namespace Turnitin_Exporter_Testing_Unit
{
    public class UnitTest1
    {
        string[] filesInFolder { get; set; }
        [Fact]
        public string[] getArrayOfPdfFilesInFolder()
        {
            filesInFolder = System.IO.Directory.GetFiles(@"C:\Users\James\Documents\transferred files\New folder (2)", "*.pdf");
            //Assert.Equal(75, filesInFolder.Length);
            Assert.NotEmpty(System.IO.Directory.GetFiles(@"C:\Users\James\Documents\transferred files\New folder (2)", "*.pdf"));
            return System.IO.Directory.GetFiles(@"C:\Users\James\Documents\transferred files\New folder (2)", "*.pdf");
        }
        [Fact]
        public void checkIfTextFileInFolder()
        {
            Assert.Empty(System.IO.Directory.GetFiles(@"C:\Users\James\Documents\transferred files\New folder (2)", "*.txt"));
        }
        [Fact]
        public void TestUnit1()
        {
            var files = getArrayOfPdfFilesInFolder();
            Directory FilesInFolder = new Directory();
            FilesInFolder.SetFilesInFolderToDirectory(files);
            Assert.Equal(FilesInFolder.files, files);
        }
    }
    public class Directory
    {
        public string[] files { get; set; }
        public void SetFilesInFolderToDirectory(string[] filesToParse)
        {
            files = filesToParse;
        }
    }
    public class PDFParser
    {
        [Theory]
        [InlineData(@"C:\Users\James\Documents\transferred files\New folder (2)\01180527.pdf")]
        [InlineData(@"C:\Users\James\Documents\transferred files\New folder (2)\01199862 materials coursework essay.pdf")]
        public string ExtractTextFromPdf(string path)
        {

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
            StringBuilder text = new StringBuilder();
            Assert.True(pdfDoc.GetNumberOfPages() > 0);
            
            if (pdfDoc.GetNumberOfPages() > 0)
            {
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    string fixedString = iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
                    text.Append(fixedString);
                }
                Assert.NotEqual(2, text.Length);
                return text.ToString();
            }
            
            else return null;
            
        }
    }
    public class PdfFilesContentsAsString
    {
        public List<string> textFromFiles = new List<string>();
        [Theory]
        [InlineData(@"C:\Users\James\Documents\transferred files\New folder (2)\01180527.pdf")]
        public void SetText(string pageText)
        {
            textFromFiles.Add(pageText);
        }
    }
}
