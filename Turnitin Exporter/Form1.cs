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
using Microsoft.Office;
using Excel = Microsoft.Office.Interop.Excel;

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
                SubstringFromfilesText.GetStudentID(AllPdfFilesAllPagesList.ListOfListOfPagesOfPdfFiles[i]);
                SubstringFromfilesText.GetStudentFeedback(AllPdfFilesAllPagesList.ListOfListOfPagesOfPdfFiles[i]);
            }
            Writer textWriter = new Writer();
            ExcelFile newExcelFile = new ExcelFile();
            textWriter.WriteIDsToTextFile(SubstringFromfilesText.ID, SubstringFromfilesText.Feedback);
            newExcelFile.SaveExcelFile(textWriter.WriteFeedbackToExcelFile(newExcelFile.CreateNewExcelFile(), SubstringFromfilesText.ID, SubstringFromfilesText.Feedback));
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
        public List<string> ExtractTextFromPdf(string path)
        {
            List<string> PagesOfText = new List<string>();
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

        public void GetStudentID(List<string> textList)
        {
            //for (int i = 0; i < textList.Count; i++)
            //{
            var match = textList.FirstOrDefault(stringToCheck => stringToCheck.Contains("SUBMISSION ID "));
            IDfromString = "SUBMISSION ID ";
                IDtoString = " CHARACTER COUNT";
                int IDFrom = match.IndexOf(IDfromString) + IDfromString.Length;
                int IDTo = match.LastIndexOf(IDtoString);
                ID.Add(match.Substring(IDFrom, IDTo - IDFrom));
            //}
        }
        public void GetStudentFeedback(List<string> textList)
        {
            //for (int i = 0; i < textList.Count; i++)
            //{
                string match = textList.FirstOrDefault(stringToCheck => stringToCheck.Contains("Instructor"));
                FeedbackfromString = "Instructor";
                FeedbacktoString = "PAGE 1";
                int stringFrom = match.IndexOf(FeedbackfromString) + FeedbackfromString.Length;
                int stringTo = match.LastIndexOf(FeedbacktoString);
            if (stringTo < 0)
            {
                stringTo = match.Length;
            }
            Feedback.Add(match.Substring(stringFrom, stringTo - stringFrom));
            //}
        }
    }
    public class Writer
    {
        public void WriteIDsToTextFile(List<string> id, List<string> feedback)
        {
            //TextWriter tw = new StreamWriter(@"C:\Users\James\Documents\transferred files\IDs and Feedback\IDs and Feedback.txt");
            TextWriter tw = new StreamWriter(@"Z:\chemistry\2019-20\Blackboard\material chemistry essays\IDs and Feedback\IDs and Feedback.txt");
            for (int i = 0; i < feedback.Count; i++)
            {
                tw.WriteLine(id[i]);
                tw.WriteLine(feedback[i]);
            }
            tw.Close();
        }
        public Excel.Application WriteFeedbackToExcelFile(Excel.Application excelfile, List<string> id, List<string> feedback)
        {
            excelfile.ActiveSheet.Cells[1, 1] = "Paper ID";
            excelfile.ActiveSheet.Cells[1, 2] = "Feedback";
            for (int i = 0; i < feedback.Count; i++)
            {
                excelfile.ActiveSheet.Cells[i + 1, 1] = id[i];
                excelfile.ActiveSheet.Cells[i + 1, 2] = feedback[i];
            }
            return excelfile;
        }
    }
    public class ExcelFile
    {
        public Excel.Application CreateNewExcelFile()
        {
            Excel.Application ExcelFile = new Excel.Application();
            Excel._Workbook workbook = (Excel._Workbook)(ExcelFile.Workbooks.Add(""));
            Excel._Worksheet worksheet = (Excel._Worksheet)workbook.ActiveSheet;
            return ExcelFile;
        }
        public void SaveExcelFile(Excel.Application file)
        {
            //file.ActiveWorkbook.SaveAs(@"C:\Users\James\Documents\transferred files\IDs and Feedback\IDs and Feedback.xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
            //false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
            //Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            file.ActiveWorkbook.SaveAs(@"Z:\chemistry\2019-20\Blackboard\material chemistry essays\IDs and Feedback\IDs and Feedback.xlsx", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
            false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
            Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            file.ActiveWorkbook.Close();
            file.Quit();
        }
    }
}
