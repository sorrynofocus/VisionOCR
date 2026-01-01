namespace ocr_function5
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            btn_Start = new System.Windows.Forms.Button();
            txt_Endpoint = new System.Windows.Forms.TextBox();
            txt_Key = new System.Windows.Forms.TextBox();
            lb_Endpoint = new System.Windows.Forms.Label();
            lb_Key = new System.Windows.Forms.Label();
            SettingsOperationPanel = new System.Windows.Forms.Panel();
            btn_RST = new System.Windows.Forms.Button();
            lb_Status = new System.Windows.Forms.Label();
            btn_SaveAll = new System.Windows.Forms.Button();
            listBox_Status = new System.Windows.Forms.ListBox();
            lb_InstructionsToUse = new System.Windows.Forms.Label();
            btn_AddFile = new System.Windows.Forms.Button();
            listBox_SrcImg = new System.Windows.Forms.ListBox();
            contextMenu_SrcImg = new System.Windows.Forms.ContextMenuStrip(components);
            menuItem_Remove = new System.Windows.Forms.ToolStripMenuItem();
            checkBox_SaveSettings = new System.Windows.Forms.CheckBox();
            lb_SettingsOverview = new System.Windows.Forms.Label();
            progressBar_Operation = new System.Windows.Forms.ProgressBar();
            panel_Status = new System.Windows.Forms.Panel();
            richTextBox_DumpText = new System.Windows.Forms.RichTextBox();
            panel_ImgPreview = new System.Windows.Forms.Panel();
            pictureBox_Preview = new System.Windows.Forms.PictureBox();
            panel_Main = new System.Windows.Forms.Panel();
            textBox_IntroInstructions = new System.Windows.Forms.TextBox();
            SettingsOperationPanel.SuspendLayout();
            contextMenu_SrcImg.SuspendLayout();
            panel_Status.SuspendLayout();
            panel_ImgPreview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox_Preview).BeginInit();
            panel_Main.SuspendLayout();
            SuspendLayout();
            // 
            // btn_Start
            // 
            btn_Start.Location = new System.Drawing.Point(364, 333);
            btn_Start.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_Start.Name = "btn_Start";
            btn_Start.Size = new System.Drawing.Size(98, 43);
            btn_Start.TabIndex = 0;
            btn_Start.Text = "Start";
            btn_Start.UseVisualStyleBackColor = true;
            btn_Start.Click += btn_Start_Click;
            // 
            // txt_Endpoint
            // 
            txt_Endpoint.Location = new System.Drawing.Point(7, 90);
            txt_Endpoint.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txt_Endpoint.Name = "txt_Endpoint";
            txt_Endpoint.Size = new System.Drawing.Size(454, 23);
            txt_Endpoint.TabIndex = 1;
            txt_Endpoint.Text = "<endpoint>";
            // 
            // txt_Key
            // 
            txt_Key.Location = new System.Drawing.Point(8, 44);
            txt_Key.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txt_Key.Name = "txt_Key";
            txt_Key.Size = new System.Drawing.Size(453, 23);
            txt_Key.TabIndex = 2;
            txt_Key.Text = "<key>";
            // 
            // lb_Endpoint
            // 
            lb_Endpoint.AutoSize = true;
            lb_Endpoint.Location = new System.Drawing.Point(405, 117);
            lb_Endpoint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lb_Endpoint.Name = "lb_Endpoint";
            lb_Endpoint.Size = new System.Drawing.Size(55, 15);
            lb_Endpoint.TabIndex = 3;
            lb_Endpoint.Text = "Endpoint";
            // 
            // lb_Key
            // 
            lb_Key.AutoSize = true;
            lb_Key.Location = new System.Drawing.Point(422, 70);
            lb_Key.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lb_Key.Name = "lb_Key";
            lb_Key.Size = new System.Drawing.Size(26, 15);
            lb_Key.TabIndex = 4;
            lb_Key.Text = "Key";
            // 
            // SettingsOperationPanel
            // 
            SettingsOperationPanel.Controls.Add(btn_RST);
            SettingsOperationPanel.Controls.Add(lb_Status);
            SettingsOperationPanel.Controls.Add(btn_SaveAll);
            SettingsOperationPanel.Controls.Add(listBox_Status);
            SettingsOperationPanel.Controls.Add(lb_InstructionsToUse);
            SettingsOperationPanel.Controls.Add(btn_AddFile);
            SettingsOperationPanel.Controls.Add(listBox_SrcImg);
            SettingsOperationPanel.Controls.Add(checkBox_SaveSettings);
            SettingsOperationPanel.Controls.Add(lb_SettingsOverview);
            SettingsOperationPanel.Controls.Add(progressBar_Operation);
            SettingsOperationPanel.Controls.Add(lb_Endpoint);
            SettingsOperationPanel.Controls.Add(btn_Start);
            SettingsOperationPanel.Controls.Add(txt_Key);
            SettingsOperationPanel.Controls.Add(lb_Key);
            SettingsOperationPanel.Controls.Add(txt_Endpoint);
            SettingsOperationPanel.Location = new System.Drawing.Point(20, 201);
            SettingsOperationPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SettingsOperationPanel.Name = "SettingsOperationPanel";
            SettingsOperationPanel.Size = new System.Drawing.Size(465, 576);
            SettingsOperationPanel.TabIndex = 5;
            // 
            // btn_RST
            // 
            btn_RST.Location = new System.Drawing.Point(371, 202);
            btn_RST.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_RST.Name = "btn_RST";
            btn_RST.Size = new System.Drawing.Size(91, 25);
            btn_RST.TabIndex = 14;
            btn_RST.Text = "CLR/RST";
            btn_RST.UseVisualStyleBackColor = true;
            btn_RST.Click += btn_RST_Click;
            // 
            // lb_Status
            // 
            lb_Status.AutoSize = true;
            lb_Status.Location = new System.Drawing.Point(5, 393);
            lb_Status.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lb_Status.Name = "lb_Status";
            lb_Status.Size = new System.Drawing.Size(39, 15);
            lb_Status.TabIndex = 13;
            lb_Status.Text = "Status";
            // 
            // btn_SaveAll
            // 
            btn_SaveAll.Location = new System.Drawing.Point(374, 541);
            btn_SaveAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_SaveAll.Name = "btn_SaveAll";
            btn_SaveAll.Size = new System.Drawing.Size(88, 27);
            btn_SaveAll.TabIndex = 12;
            btn_SaveAll.Text = "Save all";
            btn_SaveAll.UseVisualStyleBackColor = true;
            btn_SaveAll.Click += btn_SaveAll_Click;
            // 
            // listBox_Status
            // 
            listBox_Status.FormattingEnabled = true;
            listBox_Status.Location = new System.Drawing.Point(7, 414);
            listBox_Status.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBox_Status.Name = "listBox_Status";
            listBox_Status.Size = new System.Drawing.Size(454, 79);
            listBox_Status.TabIndex = 6;
            // 
            // lb_InstructionsToUse
            // 
            lb_InstructionsToUse.AutoSize = true;
            lb_InstructionsToUse.Location = new System.Drawing.Point(4, 208);
            lb_InstructionsToUse.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lb_InstructionsToUse.Name = "lb_InstructionsToUse";
            lb_InstructionsToUse.Size = new System.Drawing.Size(302, 15);
            lb_InstructionsToUse.TabIndex = 11;
            lb_InstructionsToUse.Text = "Add source image files to analyze. CLR/RST will reset all.";
            // 
            // btn_AddFile
            // 
            btn_AddFile.Location = new System.Drawing.Point(8, 333);
            btn_AddFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btn_AddFile.Name = "btn_AddFile";
            btn_AddFile.Size = new System.Drawing.Size(154, 43);
            btn_AddFile.TabIndex = 10;
            btn_AddFile.Text = "Add files...";
            btn_AddFile.UseVisualStyleBackColor = true;
            // 
            // listBox_SrcImg
            // 
            listBox_SrcImg.AllowDrop = true;
            listBox_SrcImg.ContextMenuStrip = contextMenu_SrcImg;
            listBox_SrcImg.FormattingEnabled = true;
            listBox_SrcImg.HorizontalScrollbar = true;
            listBox_SrcImg.Location = new System.Drawing.Point(4, 232);
            listBox_SrcImg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            listBox_SrcImg.Name = "listBox_SrcImg";
            listBox_SrcImg.ScrollAlwaysVisible = true;
            listBox_SrcImg.Size = new System.Drawing.Size(458, 94);
            listBox_SrcImg.Sorted = true;
            listBox_SrcImg.TabIndex = 9;
            listBox_SrcImg.Click += listBox_SrcImg_Click;
            listBox_SrcImg.SelectedIndexChanged += listBox_SrcImg_SelectedIndexChanged;
            listBox_SrcImg.DragDrop += listBox_SrcImg_DragDrop;
            listBox_SrcImg.DragEnter += listBox_SrcImg_DragEnter;
            listBox_SrcImg.KeyUp += listBox_SrcImg_KeyUp;
            // 
            // contextMenu_SrcImg
            // 
            contextMenu_SrcImg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { menuItem_Remove });
            contextMenu_SrcImg.Name = "contextMenu_SrcImg";
            contextMenu_SrcImg.Size = new System.Drawing.Size(118, 26);
            // 
            // menuItem_Remove
            // 
            menuItem_Remove.Name = "menuItem_Remove";
            menuItem_Remove.Size = new System.Drawing.Size(117, 22);
            menuItem_Remove.Text = "Remove";
            menuItem_Remove.Click += menuItem_Remove_Click;
            // 
            // checkBox_SaveSettings
            // 
            checkBox_SaveSettings.AutoSize = true;
            checkBox_SaveSettings.Location = new System.Drawing.Point(8, 145);
            checkBox_SaveSettings.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBox_SaveSettings.Name = "checkBox_SaveSettings";
            checkBox_SaveSettings.Size = new System.Drawing.Size(197, 19);
            checkBox_SaveSettings.TabIndex = 10;
            checkBox_SaveSettings.Text = "Save settings  -not implemented";
            checkBox_SaveSettings.UseVisualStyleBackColor = true;
            // 
            // lb_SettingsOverview
            // 
            lb_SettingsOverview.AutoSize = true;
            lb_SettingsOverview.Location = new System.Drawing.Point(4, 13);
            lb_SettingsOverview.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lb_SettingsOverview.Name = "lb_SettingsOverview";
            lb_SettingsOverview.Size = new System.Drawing.Size(298, 15);
            lb_SettingsOverview.TabIndex = 9;
            lb_SettingsOverview.Text = "Enter Azure settings to access Azure Cognitives services";
            // 
            // progressBar_Operation
            // 
            progressBar_Operation.Location = new System.Drawing.Point(8, 501);
            progressBar_Operation.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar_Operation.Name = "progressBar_Operation";
            progressBar_Operation.Size = new System.Drawing.Size(454, 27);
            progressBar_Operation.Step = 2;
            progressBar_Operation.TabIndex = 5;
            // 
            // panel_Status
            // 
            panel_Status.Controls.Add(richTextBox_DumpText);
            panel_Status.Location = new System.Drawing.Point(1146, 201);
            panel_Status.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel_Status.Name = "panel_Status";
            panel_Status.Size = new System.Drawing.Size(331, 576);
            panel_Status.TabIndex = 7;
            // 
            // richTextBox_DumpText
            // 
            richTextBox_DumpText.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            richTextBox_DumpText.Location = new System.Drawing.Point(4, 9);
            richTextBox_DumpText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            richTextBox_DumpText.Name = "richTextBox_DumpText";
            richTextBox_DumpText.Size = new System.Drawing.Size(322, 562);
            richTextBox_DumpText.TabIndex = 0;
            richTextBox_DumpText.Text = "";
            // 
            // panel_ImgPreview
            // 
            panel_ImgPreview.Controls.Add(pictureBox_Preview);
            panel_ImgPreview.Location = new System.Drawing.Point(492, 201);
            panel_ImgPreview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel_ImgPreview.Name = "panel_ImgPreview";
            panel_ImgPreview.Size = new System.Drawing.Size(646, 576);
            panel_ImgPreview.TabIndex = 8;
            // 
            // pictureBox_Preview
            // 
            pictureBox_Preview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            pictureBox_Preview.Location = new System.Drawing.Point(4, 13);
            pictureBox_Preview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox_Preview.Name = "pictureBox_Preview";
            pictureBox_Preview.Size = new System.Drawing.Size(627, 554);
            pictureBox_Preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox_Preview.TabIndex = 0;
            pictureBox_Preview.TabStop = false;
            // 
            // panel_Main
            // 
            panel_Main.Controls.Add(textBox_IntroInstructions);
            panel_Main.Controls.Add(panel_Status);
            panel_Main.Controls.Add(panel_ImgPreview);
            panel_Main.Controls.Add(SettingsOperationPanel);
            panel_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            panel_Main.Location = new System.Drawing.Point(0, 0);
            panel_Main.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel_Main.Name = "panel_Main";
            panel_Main.Size = new System.Drawing.Size(1592, 808);
            panel_Main.TabIndex = 9;
            // 
            // textBox_IntroInstructions
            // 
            textBox_IntroInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textBox_IntroInstructions.Enabled = false;
            textBox_IntroInstructions.Location = new System.Drawing.Point(14, 3);
            textBox_IntroInstructions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBox_IntroInstructions.Multiline = true;
            textBox_IntroInstructions.Name = "textBox_IntroInstructions";
            textBox_IntroInstructions.Size = new System.Drawing.Size(1458, 178);
            textBox_IntroInstructions.TabIndex = 10;
            textBox_IntroInstructions.Text = resources.GetString("textBox_IntroInstructions.Text");
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1592, 808);
            Controls.Add(panel_Main);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "Form1";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Azure Vision OCR Analyze/Extract - function5";
            SettingsOperationPanel.ResumeLayout(false);
            SettingsOperationPanel.PerformLayout();
            contextMenu_SrcImg.ResumeLayout(false);
            panel_Status.ResumeLayout(false);
            panel_ImgPreview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox_Preview).EndInit();
            panel_Main.ResumeLayout(false);
            panel_Main.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.TextBox txt_Endpoint;
        private System.Windows.Forms.TextBox txt_Key;
        private System.Windows.Forms.Label lb_Endpoint;
        private System.Windows.Forms.Label lb_Key;
        private System.Windows.Forms.Panel SettingsOperationPanel;
        private System.Windows.Forms.ProgressBar progressBar_Operation;
        private System.Windows.Forms.ListBox listBox_Status;
        private System.Windows.Forms.Panel panel_Status;
        private System.Windows.Forms.Panel panel_ImgPreview;
        private System.Windows.Forms.Button btn_AddFile;
        private System.Windows.Forms.ListBox listBox_SrcImg;
        private System.Windows.Forms.PictureBox pictureBox_Preview;
        private System.Windows.Forms.Label lb_SettingsOverview;
        private System.Windows.Forms.CheckBox checkBox_SaveSettings;
        private System.Windows.Forms.Panel panel_Main;
        private System.Windows.Forms.Label lb_InstructionsToUse;
        private System.Windows.Forms.Button btn_SaveAll;
        private System.Windows.Forms.Label lb_Status;
        private System.Windows.Forms.TextBox textBox_IntroInstructions;
        private System.Windows.Forms.RichTextBox richTextBox_DumpText;
        private System.Windows.Forms.Button btn_RST;
        private System.Windows.Forms.ContextMenuStrip contextMenu_SrcImg;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Remove;


    }
}

