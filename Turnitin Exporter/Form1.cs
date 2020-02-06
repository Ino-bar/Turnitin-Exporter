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
            PdfFilesContentsAsString OriginalText = new PdfFilesContentsAsString();
            for (int i = 0; i < FilesToParse.Length; i++)
            {
                OriginalText.SetText(pdfParser.ExtractTextFromPdf(FilesInFolder.files[i]));
                //SubstringFromfilesText.SetTextFromFiles(pdfParser.ExtractTextFromPdf(FilesInFolder.files[i]));
                SubstringFromfilesText.GetStudentIDAndFeedback(OriginalText.textFromFiles);
            //filesText.WriteIDsToFile();
            //filesText.WriteFeedbackToFile();
            }
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
    public class PdfFilesContentsAsString
    {
        public List<string> textFromFiles = new List<string>();
        public void SetText(string pageText)
        {
            textFromFiles.Add(pageText);
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
                    string fixedString = iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
                    text.Append(fixedString);
                }
                return text.ToString();
            }
            else return null;
        }
    }
    public class FilesTextSubstring
    {
        public List<string> ID = new List<string>();
        public List<string> Feedback = new List<string>();

        public void GetStudentIDAndFeedback(List<string> textList)
        {
            //TextWriter tw = new StreamWriter(@"C:\Users\James\Documents\transferred files\IDs and Feedback\IDs and Feedback.txt");
            for (int i = 0; i < textList.Count; i++)
            {
                
                int IDFrom = textList[i].IndexOf("SUBMISSION ID ") + "SUBMISSION ID ".Length;
                int IDTo = textList[i].LastIndexOf(" CHARACTER");
                ID.Add(textList[i].Substring(IDFrom, IDTo - IDFrom));
                Console.WriteLine(ID[i]);
                //tw.WriteLine(ID[i]);
                int stringFrom = textList[i].IndexOf("Instructor") + "Instructor".Length;
                int stringTo = textList[i].LastIndexOf("PAGE 1");
                Feedback.Add(textList[i].Substring(stringFrom, stringTo - stringFrom));
                Console.WriteLine(Feedback[i]);
                //tw.WriteLine(Feedback[i]);
                //return result[i];
            }
            //tw.Close();
        }
        public void WriteIDsToFile()
        {
            TextWriter tw = new StreamWriter(@"C:\Users\James\Documents\transferred files\IDs\IDs.txt");

            foreach (String s in ID)
                tw.WriteLine(s);

            tw.Close();
            //System.IO.File.WriteAllLines(@"C:\Users\James\Documents\transferred files\IDs\IDs.txt", ID);
            //System.IO.File.WriteAllLines(@"C:\Users\James\Documents\transferred files\Feedback\Feedback.txt", Feedback);
        }
        public void WriteFeedbackToFile()
        {
            TextWriter tw = new StreamWriter(@"C:\Users\James\Documents\transferred files\Feedback\Feedback.txt");

            foreach (String s in Feedback)
                tw.WriteLine(s);

            tw.Close();
            //System.IO.File.WriteAllLines(@"C:\Users\James\Documents\transferred files\IDs\IDs.txt", ID);
            //System.IO.File.WriteAllLines(@"C:\Users\James\Documents\transferred files\Feedback\Feedback.txt", Feedback);
        }
    }
}
