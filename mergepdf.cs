using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp;
using iTextSharp.text.pdf;

namespace PDFMerge
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        string FileName;
        string infile;

        public static void CombineMultiplePDFs(string[] fileNames, string outFile)
        {

            string[] extensions = { ".pdf", ".PDF", };

            Document document = new Document();

            PdfCopy writer = new PdfCopy(document, new FileStream(outFile, FileMode.Create));
            if (writer == null)
            {
                return;
            }

            document.Open();

            foreach (string fileName in fileNames)
            {

                PdfReader reader = new PdfReader(fileName);

                reader.ConsolidateNamedDestinations();

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                    writer.AddPage(page);
                }

                writer.FreeReader(reader);
                reader.Close();
 
            }            

            writer.Close();
            document.Close();
            
        }

        private void Main_Load(object sender, EventArgs e)
        {
            notifyIcon.BalloonTipText = "Application minimized";
            notifyIcon.BalloonTipTitle = "Multiple PDF Merge";
            notifyIcon.Text = "Multiple PDF Merge";
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon.Visible = false;
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            //This will only work if the window is minimized
            if (WindowState == FormWindowState.Minimized)
            {
                //Don't show in the taskbar
                ShowInTaskbar = false;
                
                //Don't show the icon
                ShowIcon = false;
                
                //Show in the notification area
                notifyIcon.Visible = true;
                
                //Display the balloon tip
                notifyIcon.ShowBalloonTip(1000);
            }
        }      
        
        private void btnMergeFiles_Click(object sender, EventArgs e)
        {
            if (txtFilesIn.Text.Length == 0)
            {                
                MessageBox.Show("Please select some files to merge"); 
                return;
            }
            else
            {
                infile = txtFilesIn.Text + "\\";
            }                        

            if (txtFileName.Text.Length == 0)
            {                
                MessageBox.Show("Please enter a file name);            
            }
            else
            {
                FileName = txtFileName.Text;
            }

            if (File.Exists(infile + FileName + "-Merged" + ".pdf"))
            {
                MessageBox.Show("File already exists with the specified filename " + FileName + ".pdf"); 
            }
            else
            {
                string[] filePaths = Directory.GetFiles(infile, "*.pdf");

                Directory.CreateDirectory(infile + "MergeFile");

                string NewfileName = infile + FileName + "-Merged" + ".pdf";

                CombineMultiplePDFs(filePaths, NewfileName);

                if (chkDelete.Checked)
                {
                    foreach (var filestodelete in filePaths)
                    {
                        File.Delete(filestodelete);
                    }
                }

                //Put the filename on the clipboard 
                System.Windows.Forms.Clipboard.SetText(FileName + "-Merged" +  ".pdf");

                //Set the status label on screen to say the file has been created
                MessageBox.Show("Merged PDF created as requested");

                txtFileName.Text = string.Empty;
            }

        }

        private void btnFilesIn_Click(object sender, EventArgs e)
        {

            lblStatus.Text = string.Empty;

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {

                //Set the path of the source files and put it in the textbox
                txtFilesIn.Text = folderBrowserDialog1.SelectedPath;

                //Build a string of accepted file formats 
                string[] extensions = { ".pdf", ".PDF" };
                
                //Find all the files that are in the directory and have the accepted extension
                string[] files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.*").Where(f => extensions.Contains(System.IO.Path.GetExtension(f).ToLower())).ToArray();
                
                //Display how many files have been found in the label
                lblFilesFound.Text = ("Documents found: " + files.Length.ToString());
            }
        }        

        private void btnOpnEpr_Click(object sender, EventArgs e)
        {
            string link = "https://somelink" + txtFileName.Text;

            if (txtFileName.Text.Length == 0)
            {
                MessageBox.Show("System can't be launched as no filename is specified");
                return;
            }
            else
            {
                System.Diagnostics.Process.Start(link);
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //Show the application in the taskbar
            ShowInTaskbar = true;

            //Set the window state to normal
            WindowState = FormWindowState.Normal;
        }
        
    }
}
