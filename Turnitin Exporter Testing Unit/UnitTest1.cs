using System;
using Xunit;
using Turnitin_Exporter;
using System.Windows.Forms;

namespace Turnitin_Exporter_Testing_Unit
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }
        private void ChooseFolder()
        {
            FolderBrowserDialog ChooseFilesFolder = new FolderBrowserDialog();
            DialogResult result = ChooseFilesFolder.ShowDialog();
            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(ChooseFilesFolder.SelectedPath))
            {
                string[] files = System.IO.Directory.GetFiles(ChooseFilesFolder.SelectedPath);
                Assert.NotEqual("C:\\Users\\James\\Desktop", ChooseFilesFolder.SelectedPath);
            }
        }
    }
}
