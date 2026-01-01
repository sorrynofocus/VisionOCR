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
// uncomment and hover mouse over version to get c# lang ver.
//#error version
// or you could use csc -langversion:?  at command developer prompt.

using System.Collections.Generic;
using System.Windows.Forms;

namespace ocr_function5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Store OCR results in memory for later saving
        private Dictionary<string, string> _ocrResults = new Dictionary<string, string>();

        //All events are in Form1-Events.cs
    }
}
