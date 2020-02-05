using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Turnitin_Exporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void ChooseFolderButton_Click(object sender, EventArgs e)
        {
            var FilesToParse = ChooseFolder();
            Directory FilesInFolder = new Directory();
            FilesInFolder.SetFilesInFolderToDirectory(FilesToParse);
            PDFParser pdfParser = new PDFParser();
            Console.WriteLine(pdfParser.ExtractTextFromPdf(FilesInFolder.files[0]));
        }

        public string[] ChooseFolder()
        {
            FolderBrowserDialog ChooseFilesFolder = new FolderBrowserDialog();
            DialogResult result = ChooseFilesFolder.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ChooseFilesFolder.SelectedPath))
            {
                //NewDirectoryObject(ChooseFilesFolder);
                return System.IO.Directory.GetFiles(ChooseFilesFolder.SelectedPath);
            }
            return null;
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
        public string ExtractTextFromPdf(string path)
        {

            PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
                StringBuilder text = new StringBuilder();
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    text.Append(iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
                }
                return text.ToString();
        }
    }
}
