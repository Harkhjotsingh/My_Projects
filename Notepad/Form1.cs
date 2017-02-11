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

namespace Notepad
{
    public partial class MainNotepadForm : Form
    {
        bool isFileSaved;
        bool isFileContentChanged;
        string currentFileName;

        public MainNotepadForm()
        {
            isFileSaved = false;
            isFileContentChanged = false;
            currentFileName = "";

            InitializeComponent();
        }

        // Common Used Methods.
        #region SaveAs
        private void SaveAs()
        {
            SaveFileDialog saveAs = new SaveFileDialog();
            saveAs.Filter = "Text Document(*.txt)|*.txt|All Files(*.*)|*.*";
            if (saveAs.ShowDialog() == DialogResult.OK)
            {
                MainWritingWindow.SaveFile(saveAs.FileName, RichTextBoxStreamType.PlainText);
                this.Text = saveAs.FileName;

                isFileSaved = true;
                isFileContentChanged = false;
                currentFileName = saveAs.FileName;
            }

        }
        #endregion
        #region Disply do you want to keep changes? Message 
        private void DoYouWantToKeepChanges()
        {
            if (isFileContentChanged && MainWritingWindow.Text != "")
            {
                DialogResult res = MessageBox.Show("Do you want to save Changes" + currentFileName, "Notepad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                switch (res)
                {
                    case DialogResult.Yes:
                        Save();
                        MainWritingWindow.Clear();
                        this.Text = "Untiteled - Notepad";
                        isFileContentChanged = false;
                        break;
                    case DialogResult.No:
                        MainWritingWindow.Clear();
                        this.Text = "Untiteled - Notepad";
                        isFileContentChanged = false;
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
            else
            {
                MainWritingWindow.Clear();
                undoToolStripMenuItem.Enabled = false;
            }
        }

        #endregion
        #region Save File
        private void Save()
        {
            if (isFileSaved == true)
            {

                MainWritingWindow.SaveFile(currentFileName, RichTextBoxStreamType.PlainText);

                isFileSaved = true;
                isFileContentChanged = false;
            }
            else
            {
                if (isFileContentChanged == true)
                {
                    SaveFileDialog save = new SaveFileDialog();
                    save.Filter = "Text Document(*.txt)|*.txt|All Files(*.*)|*.*";
                    if (save.ShowDialog() == DialogResult.OK)
                    {
                        MainWritingWindow.SaveFile(save.FileName, RichTextBoxStreamType.PlainText);
                        this.Text = save.FileName;

                        isFileSaved = true;
                        isFileContentChanged = false;
                        currentFileName = save.FileName;
                    }
                }
                else
                {
                    MainWritingWindow.Clear();
                    this.Text = "Untiteled - Notepad";
                }
            }
        }
        #endregion
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            isFileContentChanged = true;

            if (MainWritingWindow.TextLength == 0)
            {
                undoToolStripMenuItem.Enabled = false;
                cutToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                selectAllToolStripMenuItem.Enabled = false;
                boldToolStripMenuItem.Enabled = false;
                italicToolStripMenuItem.Enabled = false;
            }
            else
            {
                undoToolStripMenuItem.Enabled = true;
                cutToolStripMenuItem.Enabled = true;
                copyToolStripMenuItem.Enabled = true;
                selectAllToolStripMenuItem.Enabled = true;
                boldToolStripMenuItem.Enabled = true;
                italicToolStripMenuItem.Enabled = true;
                underLineToolStripMenuItem.Enabled = true;
            }

            if (MainWritingWindow.SelectedText != "")
            { deleteToolStripMenuItem.Enabled = true; }
        }
        
        //MenuControls Logic
        #region FileControls        
        #region New File
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoYouWantToKeepChanges();
        }
        #endregion
        #region Open File
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoYouWantToKeepChanges();

            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text Files(*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|All Files(*.*)|*.*";
            if (open.ShowDialog() == DialogResult.OK)
            {
                MainWritingWindow.LoadFile(open.FileName, RichTextBoxStreamType.PlainText);
                this.Text = Path.GetFileName(open.FileName) + " - Notepad";

                isFileSaved = true;
                isFileContentChanged = false;
                currentFileName = open.FileName;
            }
        }
        #endregion
        #region Save File
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save();
        }
        #endregion
        #region SaveAs
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }
        #endregion
        // Print
        #region Close Editor
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DoYouWantToKeepChanges();
            Application.Exit();
        }
        #endregion
        #endregion

        #region FontControls     
        #region BoldFont
        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.SelectionFont = new Font(MainWritingWindow.Font, FontStyle.Bold);
        }
        #endregion
        #region Italic
        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.SelectionFont = new Font(MainWritingWindow.Font, FontStyle.Italic);
        }
        #endregion
        #region UnderLine
        private void underLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.SelectionFont = new Font(MainWritingWindow.Font, FontStyle.Underline);
        }
        #endregion
        #region Select
        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fonts = new FontDialog();
            fonts.Font = MainWritingWindow.SelectionFont;
            if (fonts.ShowDialog() == DialogResult.OK)
            {
                MainWritingWindow.SelectionFont = fonts.Font;
            }
        }
        #endregion
        #endregion

        #region EditControls
        #region Undo
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.Undo();
            redoToolStripMenuItem.Enabled = true;
        }
        #endregion
        #region Redo
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.Redo();
            undoToolStripMenuItem.Enabled = true;
        }
        #endregion
        #region Cut
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.Cut();
            undoToolStripMenuItem.Enabled = true;
        }
        #endregion
        #region Copy
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.Copy();
        }
        #endregion
        #region Paste
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.Paste();
        }
        #endregion
        #region Delete
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.SelectedText = "";
            undoToolStripMenuItem.Enabled = true;
        }
        #endregion
        #region SelectAll
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.SelectAll();
        }
        #endregion
        #region DateTime
        private void dateTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainWritingWindow.Text += DateTime.Now;
        }
        #endregion
        #endregion

        #region HelpControl
        #region About
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Notepad Project is developed by Harkhjot Singh aka Harry. If you any questions or concerns please send me an email at harkhjotsingh@gmail.com", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
        #endregion
    }
}
