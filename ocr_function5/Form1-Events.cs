/*
  Azure Cognitive Services Vision OCR Client
    ---------------------------------------
    Purpose:
    This class provides functionality to extract raw text lines from images

    Author:  CWinters / GaleHarper Desktop - Z790 DARK HERO+RTX 4070 Ti SUPER / Arizona / AZ / USA
    Assisted by: Azure AI Vision SDK
    
    Date: December 2025 

    See README.MD for setup and usage instructions.

 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ocr_function5
{
    public partial class Form1
    {

        private static readonly string[] SupportedImgExt = { ".jpg", ".jpeg", ".png", ".bmp" };

        /// <summary>
        /// Drag and drop handler for source image list box with file type filtering for supported formats
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_SrcImg_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (string file in files)
                {
                    string ext = Path.GetExtension(file).ToLowerInvariant();

                    if (SupportedImgExt.Contains(ext))
                        listBox_SrcImg.Items.Add(file);
                    else
                        listBox_Status.Items.Add("Skipped unsupported file: " + file);
                }
            }
        }

        /// <summary>
        /// Drag enter handler for source image list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_SrcImg_DragEnter(object sender, DragEventArgs e)
        {
            // Check if data being dragged is file drop
            //if (e.Data.GetDataPresent(DataFormats.FileDrop))
            //{
            //    string[]? files = (string[]?)e.Data.GetData(DataFormats.FileDrop);

            //    if (files == null)
            //    {
            //        e.Effect = DragDropEffects.None;
            //        return;
            //    }

            //    bool isValid = false;

            // Check if any of the files have supported image extensions
            //    foreach (string file in files)
            //    {
            //        string ext = Path.GetExtension(file).ToLowerInvariant();

            //        foreach (string extAllowed in SupportedImgExt)
            //        {
            //            if (ext == extAllowed)
            //            {
            //                isValid = true;
            //                break;
            //            }
            //        }

            //        if (isValid)
            //            break;
            //    }
            //    e.Effect = isValid ? DragDropEffects.Copy : DragDropEffects.None;
            //}
            //else
            //{
            //    e.Effect = DragDropEffects.None;
            //    return;
            //}

            // Simplified using LINQ
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[]? files = (string[]?)e.Data.GetData(DataFormats.FileDrop);

                bool anyValid = files?.Any(f => SupportedImgExt.Contains(Path.GetExtension(f).ToLowerInvariant())) ?? false;

                e.Effect = anyValid ? DragDropEffects.Copy : DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.None;
        }

        /// <summary>
        /// Start button click handler to process images
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btn_Start_Click(object sender, EventArgs e)
        {
            btn_Start.Enabled = false;
            progressBar_Operation.Value = 0;
            listBox_Status.Items.Clear();
            _ocrResults.Clear();

            richTextBox_DumpText.Text = string.Empty;
            pictureBox_Preview.Image = null;

            string? endpoint = txt_Endpoint.Text;
            string? key = txt_Key.Text;

            VisionOcrClient ocr = new VisionOcrClient(endpoint, key);

            int totalItems = listBox_SrcImg.Items.Count;

            if (totalItems == 0)
            {
                listBox_Status.Items.Add("No images to process.");
                btn_Start.Enabled = true;
                return;
            }

            // Configure progress bar
            progressBar_Operation.Minimum = 0;
            progressBar_Operation.Maximum = totalItems;
            progressBar_Operation.Step = 1;

            for (int i = 0; i < totalItems; i++)
            {
                string? imagePath = (string)listBox_SrcImg.Items[i];

                if (imagePath == null || !File.Exists(imagePath))
                {
                    listBox_Status.Items.Add("Skipping invalid file entry.");
                    continue;
                }

                listBox_Status.Items.Add($"Processing: {imagePath}");

                try
                {
                    listBox_Status.Items.Add($"Success: {imagePath}");

                    List<string>? rawLines = await ocr.ExtractRawLinesAsync(imagePath);
                    List<string>? cleanedLines = TextProcessProvider.RemoveDropPhraseLines(rawLines);
                    //This is not used currently, but here for personal notes).
                    //See TextProcessProvider.ConvertLinesToMarkdownNote() for details.
                    List<string>? markdownNoteFormat = TextProcessProvider.ConvertLinesToMarkdownNote(rawLines);

                    string? textRes = string.Join("\n", cleanedLines);
                    textRes = TextProcessProvider.NormalizeOCRText(textRes);
                    textRes = TextProcessProvider.ConstructPotentialParagraphs(textRes);
                    _ocrResults[imagePath] = textRes;
                }
                catch (Exception ex)
                {
                    listBox_Status.Items.Add($"Error: {ex.Message}");
                }

                // Step up progress bar
                progressBar_Operation.PerformStep();

                // Force UI refresh - process windows message pump
                Application.DoEvents();
            }

            listBox_Status.Items.Add("All operations completed.");
            btn_Start.Enabled = true;
        }


        /// <summary>
        /// Synchronize preview image and OCR text output with selected image. 
        /// Shared method to update preview and text when selection changes
        /// </summary>
        private void UpdateSelectedImagePreview()
        {
            string? fileRef = listBox_SrcImg.SelectedItem as string;

            // Nothing selected — clear preview and text 
            // fileRef will be null if "removed" via context menu
            // or if nothing is selected. The SelectedIndexChanged event
            // will fire using the context menu removal as well.
            if (fileRef == null)
            {
                if (pictureBox_Preview.Image != null)
                {
                    // Dispose image in case we get locks. Possible?
                    pictureBox_Preview.Image.Dispose();
                    pictureBox_Preview.Image = null;
                }

                richTextBox_DumpText.Clear();
                return;
            }

            btn_Start.Enabled = true;

            if (fileRef != null && File.Exists(fileRef))
                pictureBox_Preview.Image = System.Drawing.Image.FromFile(fileRef);

            // Load OCR
            if (_ocrResults.ContainsKey(key: fileRef))
                richTextBox_DumpText.Text = _ocrResults[fileRef];
            else richTextBox_DumpText.Text = "No OCR result yet";
        }

        /// <summary>
        /// List box click handler to update preview and text output
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_SrcImg_Click(object sender, EventArgs e)
        {
            UpdateSelectedImagePreview();
        }

        /// <summary>
        /// List box key up handler to update preview and text output
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_SrcImg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) UpdateSelectedImagePreview();
        }

        /// <summary>
        /// List box selected index changed handler to update preview and text output.
        /// Example: moving keys up and down will change selected index.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox_SrcImg_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelectedImagePreview();
        }

        /// <summary>
        /// Save all OCR results to text files in selected folder - event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SaveAll_Click(object sender, EventArgs e)
        {
            if (_ocrResults.Count == 0)
            {
                MessageBox.Show("No OCR results available. Run Start first after selecting source images.");
                return;
            }

            //Sorry. Got lazy and just added "you find/create output folder" dialog here.
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select folder to save OCR text files";

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string? outputFolder = dialog.SelectedPath;

            foreach (KeyValuePair<string, string> kvp in _ocrResults)
            {
                string? imagePath = kvp.Key;
                string? textContent = kvp.Value;

                string? fileNameOnly = Path.GetFileNameWithoutExtension(imagePath);
                string? savePath = Path.Combine(outputFolder, fileNameOnly + ".txt");

                try
                {
                    File.WriteAllText(savePath, textContent, Encoding.UTF8);
                    listBox_Status.Items.Add($"Saved: {savePath}");
                }
                catch (Exception ex)
                {
                    listBox_Status.Items.Add($"Error saving {savePath}: {ex.Message}");
                }
            }

            MessageBox.Show("All OCR results saved.");
        }

        /// <summary>
        /// Event handler for Clear/Reset button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_RST_Click(object sender, EventArgs e)
        {
            //Clear the following UI Components:
            // source image list
            // status messages
            // OCR results dictionary
            // preview image
            // OCR text output
            // reset progress bar
            // enable Start button
            // focus back on add files button
            listBox_SrcImg.Items.Clear();
            listBox_Status.Items.Clear();
            _ocrResults.Clear();
            pictureBox_Preview.Image = null;
            richTextBox_DumpText.Clear();
            progressBar_Operation.Value = 0;
            btn_Start.Enabled = true;
            btn_AddFile.Focus();
            listBox_Status.Items.Add("Clear/Reset...");
        }

        /// <summary>
        /// Event handler for Remove menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_Remove_Click(object sender, EventArgs e)
        {
            string? fileRef = listBox_SrcImg.SelectedItem as string;
            
            if (fileRef == null)
                return;

            // Remove from ListBox
            listBox_SrcImg.Items.Remove(fileRef);

            // Remove OCR result if it exists
            if (_ocrResults.ContainsKey(fileRef))
                _ocrResults.Remove(fileRef);

            // Clear preview + text if the removed item was selected
            pictureBox_Preview.Image = null;
            richTextBox_DumpText.Clear();

            listBox_Status.Items.Add($"Removed: {fileRef}");
        }

    }//End of class Form1
}
