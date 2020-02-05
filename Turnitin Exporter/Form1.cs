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
            FilesText filesText = new FilesText();
            PDFParser pdfParser = new PDFParser();
            for(int i = 0; i < FilesToParse.Length; i++)
            { 
            filesText.SetTextFromFiles(pdfParser.ExtractTextFromPdf(FilesInFolder.files[i]));
            filesText.GetStudentIDAndFeedback();
            }
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
            if (pdfDoc.GetNumberOfPages() > 0)
            {
                //FilesText filesText = new FilesText();
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    text.Append(iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
                }
                return text.ToString();
            }
            else return null;
        }
    }
    public class FilesText
    {
        public string teststring { get; set; }
        public List<string> textFromFiles = new List<string>();
        public List<string> ID = new List<string>();
        public List<string> Feedback = new List<string>();

        public void SetTextFromFiles(string files)
        {
            teststring = files;
            textFromFiles.Add(teststring);
        }
        public void GetStudentIDAndFeedback()
        {
            for(int i = 0; i < textFromFiles.Count; i++)
            {
                
                int IDFrom = textFromFiles[i].IndexOf("SUBMISSION ID ") + "SUBMISSION ID ".Length;
                int IDTo = textFromFiles[i].LastIndexOf(" CHARACTER");
                ID.Add(textFromFiles[i].Substring(IDFrom, IDTo - IDFrom));
                Console.WriteLine(ID[i]);
                
                int stringFrom = textFromFiles[i].IndexOf("Instructor") + "Instructor".Length;
                int stringTo = textFromFiles[i].LastIndexOf("PAGE 1");
                Feedback.Add(textFromFiles[i].Substring(stringFrom, stringTo - stringFrom));
                Console.WriteLine(Feedback[i]);
                //return result[i];
            }
        }
    }
}
