using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
            FilesTextSubstring SubstringFromfilesText = new FilesTextSubstring();
            PDFParser pdfParser = new PDFParser();
            AllPdfFilesListsOfPages AllPdfFilesAllPagesList = new AllPdfFilesListsOfPages();
            for (int i = 0; i < FilesToParse.Length; i++)
            {
                AllPdfFilesAllPagesList.AddPdfFilePagesToList(pdfParser.ExtractTextFromPdf(FilesInFolder.files[i]));
                SubstringFromfilesText.GetStudentIDAndFeedback(AllPdfFilesAllPagesList.ListOfListOfPagesOfPdfFiles[i]);
            }
            Writer textWriter = new Writer();
            textWriter.WriteIDsToTextFile(SubstringFromfilesText.ID, SubstringFromfilesText.Feedback);
        }

        public string[] ChooseFolder()
        {
            FolderBrowserDialog ChooseFilesFolder = new FolderBrowserDialog();
            DialogResult result = ChooseFilesFolder.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ChooseFilesFolder.SelectedPath))
            {
                return System.IO.Directory.GetFiles(ChooseFilesFolder.SelectedPath, "*.pdf");
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
    public class AllPdfFilesListsOfPages
    {
        public List<List<string>> ListOfListOfPagesOfPdfFiles = new List<List<string>>();
        public void AddPdfFilePagesToList (List<string> pageList)
        {
            ListOfListOfPagesOfPdfFiles.Add(pageList);
        }
    }
    /*
    public class PagesOfPdfFileAsStrings
    {
        public List<string> textFromFiles { get; set; }
        public void SetText(List<string> pageText)
        {
            textFromFiles = pageText;
        }
    }
    */
    public class PDFParser
    {
        List<string> PagesOfText = new List<string>();
        public List<string> ExtractTextFromPdf(string path)
        {
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(path));
            if (pdfDoc.GetNumberOfPages() > 0)
            {
                for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
                {
                    StringBuilder text = new StringBuilder();
                    string pageOfText = iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
                    text.Append(pageOfText);
                    PagesOfText.Add(text.ToString());
                }
                return PagesOfText;
            }
            else return null;
        }
    }
    public class FilesTextSubstring
    {
        public List<string> ID = new List<string>();
        public List<string> Feedback = new List<string>();
        private string IDfromString {get; set;}
        private string IDtoString { get; set; }
        private string FeedbackfromString { get; set; }
        private string FeedbacktoString { get; set; }

        public void GetStudentIDAndFeedback(List<string> textList)
        {
            //TextWriter tw = new StreamWriter(@"C:\Users\James\Documents\transferred files\IDs and Feedback\IDs and Feedback.txt");
            for (int i = 0; i < textList.Count; i++)
            {
                IDfromString = "SUBMISSION ID ";
                IDtoString = " CHARACTER";
                FeedbackfromString = "Instructor";
                FeedbacktoString = "PAGE 1";
                if (textList[i].Contains(IDfromString) & textList[i].Contains(IDtoString))
                {
                    int IDFrom = textList[i].IndexOf(IDfromString) + IDfromString.Length;
                    int IDTo = textList[i].LastIndexOf(IDtoString);
                    ID.Add(textList[i].Substring(IDFrom, IDTo - IDFrom));
                }
                else if (textList[i].Contains(FeedbackfromString) & textList[i].Contains(FeedbacktoString))
                { 
                    int stringFrom = textList[i].IndexOf(FeedbackfromString) + FeedbackfromString.Length;
                    int stringTo = textList[i].LastIndexOf(FeedbacktoString);
                    Feedback.Add(textList[i].Substring(stringFrom, stringTo - stringFrom));
                }
            }
            /*
            for(int i = 0; i < Feedback.Count; i++)
            {
                tw.WriteLine(ID[i]);
                tw.WriteLine(Feedback[i]);
            }
            tw.Close();
            */
        }
    }
    public class Writer
    {
        public void WriteIDsToTextFile(List<string> id, List<string> feedback)
        {
            TextWriter tw = new StreamWriter(@"C:\Users\James\Documents\transferred files\IDs and Feedback\IDs and Feedback.txt");
            for (int i = 0; i < feedback.Count; i++)
            {
                tw.WriteLine(id[i]);
                tw.WriteLine(feedback[i]);
            }
            tw.Close();
        }
        public void WriteFeedbackToExcelFile(List<string> id, List<string> feedback)
        {

        }
    }
}
