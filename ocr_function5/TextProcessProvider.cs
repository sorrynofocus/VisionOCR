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

// Hack to get C# language version
// uncomment and hover mouse over version to get c# lang ver.
// #error version
// or you could use csc -langversion:?  at command developer prompt.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

/*
  During text analysis, OCR engines often include extraneous text such as UI elements or instructions.
 In my case, I needed to remove UI prompts like "Show Answer", "Reset", and keyboard instructions from the OCR output.
This worked, and can be turned into a reusable function for options.

Legacy OCR should not be used for production systems. Newer OCR engines provide better accuracy and layout analysis.

*** Azure.AI.Vision.ImageAnalysis - The Azure AI Vision SDK offers advanced OCR capabilities with improved accuracy and 
layout understanding. It can better handle complex documents, multi-column layouts, and various fonts. This is the 
newer SDK that Microsoft is promoting for vision tasks.

ImageAnalysisClient with features: ImageAnalysisFeature.Read, ImageAnalysisFeature.Text, ImageAnalysisOptions, 
AnalyzeAsync() are considered the modern approach

*** Microsoft.Azure.CognitiveServices.Vision.ComputerVision - The older Computer Vision SDK and Microsoft suggested migrating 
to the newer Azure.AI.Vision SDK for better performance and features.

ComputerVisionClient, ReadAsync, RecognizeTextAsync, OcrAsync are older methods that may not provide the best results compared to the newer SDK.


Other issues like detecting list items, paragraph reconstruction, and whitespace normalization are referenced in below articles:
    https://learn.microsoft.com/en-us/azure/cognitive-services/computer-vision/overview-ocr

    References:

    Why Stop at Words? Unveiling the Bigger Picture through Line-Level OCR
    https://arxiv.org/html/2508.21693v1

    How OCR Works — Line Segmentation
    https://how-ocr-works.com/OCR/line-segmentation.html

    Sentence Boundary Detection in OCR Text
    https://aclanthology.org/W19-4006.pdf

    GCP Vision OCR Line Segmentation Example
    https://github.com/7codeRO/line-segmentation-gpc-vision-ocr?
 */


namespace ocr_function5
{
    /// <summary>
    /// Text processing utilities for cleaning and formatting OCR output.
    /// </summary>
    internal static class TextProcessProvider
    {
        // Words phrases to drop from OCR output 
        // TODO: Add options to customize this list
        // See RemoveDropPhraseLines()
        private static readonly string[] DefaultDropPhrases =
        {
            "Show Answer",
            "Reset",
            "Hide Answer",
            "Keyboard Instructions",
            "Press Escape",
            "Press Enter",
            "Press Space",
            "Use Tab key",
            "Open in Browser"
        };

        /// <summary>
        /// Removes lines containing common unwanted phrases from OCR output.
        /// TODO: Make the phrases customizable by options.
        /// </summary>
        /// <param name="srcLines"></param>
        /// <returns></returns>
        public static List<string> RemoveDropPhraseLines(IEnumerable<string> srcLines)
        {
            string[]? phrases = DefaultDropPhrases.ToArray();
            List<string>? output = new List<string>();

            foreach (string line in srcLines)
            {
                string? trimmedLine = line.Trim();
                bool dropItLikeItsHot = false;

                if (trimmedLine.Length == 0) dropItLikeItsHot = true;

                for (int i = 0; i < phrases.Length && !dropItLikeItsHot; i++)
                {
                    if (trimmedLine.IndexOf(phrases[i], StringComparison.OrdinalIgnoreCase) >= 0)
                        dropItLikeItsHot = true;
                }

                if (!dropItLikeItsHot && 
                    trimmedLine.Length <= 2 
                    && !char.IsLetterOrDigit(trimmedLine[0]))
                    dropItLikeItsHot = true;
                
                if (!dropItLikeItsHot) output.Add(line);
            }
            return (output);
        }


        /// <summary>
        /// This function normalizes text with common text artifacts from OCR output.
        /// </summary>
        /// <param name="srcText"></param>
        /// <returns></returns>
        public static string NormalizeOCRText(string? srcText)
        {
            if (string.IsNullOrWhiteSpace(srcText))
                return (string.Empty);

            // carriage return newline to regular newline
            srcText = srcText.Replace("\r\n", "\n").Replace("\r", "\n");
            // https://unicode-explorer.com/c/00A0
            srcText = srcText.Replace('\u00A0', ' ');
            // multi space/tabs-> single space
            srcText = Regex.Replace(srcText, @"[ \t]{2,}", " ");
            // trim \t and \n around newlines
            srcText = Regex.Replace(srcText, @"[ \t]*\n[ \t]*", "\n");
            // multi newlines (vertical gaps) reduce to double
            // (change this if you want single -remove \n\n to \n)
            srcText = Regex.Replace(srcText, @"\n{3,}", "\n\n");
            
            return (srcText.Trim());
        }

        /// <summary>
        ///  To try reconstruct paragraphs from broken OCR output, making the text readable and suitable for further processing.
        ///  Decide if two lines should be joined by a [space] or a newline.
        /// </summary>
        /// <param name="srcText"></param>
        /// <returns>string</returns>
        public static string ConstructPotentialParagraphs(string? srcText)
        {
            if (string.IsNullOrWhiteSpace(srcText))
                return (string.Empty);

            string[]? lines = srcText.Split('\n');
            StringBuilder? sb = new StringBuilder();
            string? prev = null;

            for (int i = 0; i < lines.Length; i++)
            {
                string curr = lines[i];

                if (string.IsNullOrWhiteSpace(curr))
                {
                    if (sb.Length > 0 && sb[sb.Length - 1] != '\n')
                        sb.Append('\n');

                    sb.Append('\n');
                    prev = null;
                    continue;
                }

                curr = curr.Trim();

                if (prev == null)
                {
                    sb.Append(curr);
                    prev = curr;
                    continue;
                }

                bool join = !DetectBreaks(prev) &&
                            !DetectListItems(curr) &&
                            !DetectListItems(prev) &&
                            // Check for short lines with only alphanumeric and whitespace characters
                            !(curr.Length <= 40 && 
                            curr.All(
                                // Check if all characters are letters, digits, or whitespace
                                delegate (char c)
                                {
                                    return char.IsLetterOrDigit(c) || char.IsWhiteSpace(c);
                                }));
                //Sidenote:
                //CoPilot suggested during this func construction; and I smiled when using older
                // style delegates instead of lambdas. ah the older memory lanes...
                // A reminder of various ways to express the same logic.
                //
                // Using lambda (the more modern way C#/Pythonic)
                //
                // c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)
                //            
                // Same as using method (older C style):
                // private static bool IsChar(char c)
                // {
                //    return char.IsLetterOrDigit(c) || char.IsWhiteSpace(c);
                // }
                // 
                // !(curr.Length <= 40 && curr.All(IsSimpleChar));
                // 
                //Or LINQ
                // !(curr.Length <= 40 && from c in curr where char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) select c).Count() == curr.Length;
                // I'm not really "Pro" on LINQ, so I prefer the delegate or lambda style for clarity.

                if (join)
                    sb.Append(' ').Append(curr);
                else
                    sb.Append('\n').Append(curr);

                prev = curr;
            }
            return (sb.ToString().Trim());
        }

        /// <summary>
        /// Detects if a line ends with a hard break punctuation.
        /// 
        /// This helper detects punctuation that strongly indicates end of a sentence
        /// Don't join the next line if detected.
        /// 
        /// </summary>
        /// <param name="srcString"></param>
        /// <returns></returns>
        private static bool DetectBreaks(string? srcString)
        {
            if (string.IsNullOrEmpty(srcString) || srcString.Length == 0) return (false);

            return (srcString.EndsWith(".") ||
                    srcString.EndsWith("!") ||
                    srcString.EndsWith("?") ||
                    srcString.EndsWith(":") ||
                    srcString.EndsWith(";"));
        }

        /// <summary>
        /// This function detects lists, numbered or bullet
        /// If it looks like a list, the line should not be joined with the previous line, only starting a new line
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool DetectListItems(string? srcString)
        {
            if (string.IsNullOrEmpty(srcString) || srcString.Length == 0) return (false);

            return (Regex.IsMatch(srcString.TrimStart(), @"^(\d+[\).]|[-*•]|[A-D][\).])\s+"));
        }

        /// <summary>
        /// Takes raw lines from OCR and converts to cheap markdown note format.
        /// This is only good for simple for Q/A style notes.
        /// </summary>
        /// <param name="rawLines"></param>
        /// <returns>list of strings</returns>
        public static List<string> ConvertLinesToMarkdownNote(IEnumerable<string>? rawLines)
        {
            /*
              Experimental/Personal only!
              I've included this because I had s ome images with Q/A style content. I needed to pull 
              out the prompt and extract text for late reading or study cards. See README.MD for reason
              I started the project.
              For this application example, I have not included it in the saving process, only the "cleaned" lines
              of extracted text for done. but, I am keeping this in because _YOU_ might find it useful.

              Markdown note format (change to your liking):
                # OCR Notes
                ## Prompt
                [first line as prompt]
                ## Extracted text
                [rest of lines as extracted text]

                To save this instead of plain text, change:

                btn_Start_Click() event handler, the following line:

                From:    
                _ocrResults[imagePath] = textRes;

                To:
                _ocrResults[imagePath] = markdownNoteFormat;

                Better yet, add an option to toggle between plain text and markdown note format.

                In btn_SaveAll_Click(), change:

                From:
                string? savePath = Path.Combine(outputFolder, fileNameOnly + ".txt");

                To:
                string? savePath = Path.Combine(outputFolder, fileNameOnly + ".md");

                Side note:
                ConvertLinesToMarkdownText() is a helper that calls this function and returns a single string. 
                This function returns a list of strings.
             */


            if (rawLines == null)
                return (new List<string>());

            // Normalize, filter lines
            List<string> lines = new List<string>();

            foreach (string? lineItems in rawLines)
            {
                if (lineItems == null)
                    continue;

                string? trimmed = lineItems.Trim();

                if (trimmed.Length > 0)
                    lines.Add(trimmed);
            }

            if (lines.Count == 0)
                return(new List<string>());

            List<string> outputMD = new List<string>();

            // Header
            outputMD.Add("# OCR Notes");
            outputMD.Add("");
            outputMD.Add("## Prompt");
            outputMD.Add("");
            outputMD.Add(lines[0]);
            outputMD.Add("");
            
            // Extracted text is the remaining output
            List<string> extractedText = new List<string>();
            
            for (int i = 1; i < lines.Count; i++)
                extractedText.Add(lines[i]);

            if (extractedText.Count > 0)
            {
                outputMD.Add("## Extracted text");
                outputMD.Add("");

                foreach (string line in extractedText)
                {
                    if (Regex.IsMatch(line, @"^([A-D][\).]|[-*•]|\d+[\).])\s+")) outputMD.Add("- " + line);
                    else outputMD.Add(line);
                }
            }
            return (outputMD);
        }

        /// <summary>
        /// Takes raw lines from OCR and converts to cheap markdown note format.
        /// This is only good for simple for Q/A style notes.
        /// </summary>
        /// <param name="rawLines"></param>
        /// <returns>string</returns>
        public static string ConvertLinesToMarkdownText(IEnumerable<string>? rawLines)
        {
            List<string> lines = ConvertLinesToMarkdownNote(rawLines);

            if (lines.Count == 0) return (string.Empty);

            StringBuilder? sb = new StringBuilder();

            foreach (string? line in lines)
            {
                sb.AppendLine(line);
            }

            return (sb.ToString().Trim());
        }


    }
}
