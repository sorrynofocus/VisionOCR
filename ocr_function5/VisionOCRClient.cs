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
using Azure;
using Azure.AI.Vision.ImageAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

// uncomment and hover mouse over version to get c# lang ver.
//#error version
// or you could use csc -langversion:?  at command developer prompt.



/*
Legacy OCR should not be used for production systems. Newer OCR engines provide better accuracy and layout analysis.

*** Azure.AI.Vision.ImageAnalysis - The Azure AI Vision SDK offers advanced OCR capabilities with improved accuracy and 
layout understanding. It can better handle complex documents, multi-column layouts, and various fonts. This is the 
newer SDK that Microsoft is promoting for vision tasks.

ImageAnalysisClient with features: ImageAnalysisFeature.Read, ImageAnalysisFeature.Text, ImageAnalysisOptions, 
AnalyzeAsync() are considered the modern approach

*** Microsoft.Azure.CognitiveServices.Vision.ComputerVision - The older Computer Vision SDK and Microsoft suggested migrating 
to the newer Azure.AI.Vision SDK for better performance and features.

ComputerVisionClient, ReadAsync, RecognizeTextAsync, OcrAsync are older methods that may not provide the best results compared to the newer SDK.

Reference guides:
https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/overview
https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/overview-image-analysis
https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/concept-ocr
https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/vision/Azure.AI.Vision.ImageAnalysis
https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/how-to/call-read-api



 */


namespace ocr_function5
{
    internal class VisionOcrClient
    {
        private readonly Uri? _endpoint;
        private readonly AzureKeyCredential? _credential;

        public VisionOcrClient(string? endpoint, string? key)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException($"Endpoint is required. {nameof(endpoint)}");
            
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"Key is required. {nameof(key)}");

            _endpoint = new Uri(endpoint);
            _credential = new AzureKeyCredential(key);
        }

        /// <summary>
        /// Calls Azure Vision OCR and returns ONLY the raw extracted lines.
        /// No cleanup, no markdown, no whitespace normalization.
        /// </summary>
        public async Task<List<string>> ExtractRawLinesAsync(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                throw new ArgumentException($"Image path is required. {nameof(imagePath)}");

            if (!File.Exists(imagePath))
                throw new FileNotFoundException($"Image not found. {imagePath}");

            ImageAnalysisClient? client = new ImageAnalysisClient(_endpoint, _credential);

            try
            {
                using (FileStream fileStream = File.OpenRead(imagePath))
                {
                    // Analyze the image for text using the Read feature
                    ImageAnalysisResult? result = await client.AnalyzeAsync( BinaryData.FromStream(fileStream),
                                                                            VisualFeatures.Read);

                    List<string>? lines = ExtractLines(result);
                    return (lines);
                }
            }
            catch (RequestFailedException ex)
            {
                throw new InvalidOperationException(
                    $"Vision OCR request failed. Status={ex.Status}, ErrorCode={ex.ErrorCode}, Message={ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Vision OCR failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Extracts text lines from Azure Vision result.
        /// This function is unchanged and remains the core OCR extraction logic.
        /// Helper func for ExtractRawLinesAsync()
        /// </summary>
        private static List<string> ExtractLines(ImageAnalysisResult? result)
        {

            //Sidenote:
            // Confidence scores are available for words:
            // result.Read.Blocks[i].Lines[j].Words[k].Confidence
            //
            // Line- or block-level confidence is not exposed the same way;
            // Block
            // └── Line
            //       └── Word (confidence and bounding box available here)
            // use word confidence if you need quality signals.
            //Those are commented out below.
            //Ref: https://learn.microsoft.com/en-us/azure/ai-services/computer-vision/concept-ocr
            // https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/vision/Azure.AI.Vision.ImageAnalysis


            List<string>? lines = new List<string>();

            if (result == null || 
                result.Read == null 
                || result.Read.Blocks == null)
                return (lines);

            foreach (DetectedTextBlock block in result.Read.Blocks)
            {
                if (block.Lines == null)
                    continue;

                foreach (DetectedTextLine line in block.Lines)
                {
                    if (line == null || string.IsNullOrWhiteSpace(line.Text))
                        continue;

                    // debug logging for line text
                    // Console.WriteLine("Line: " + line.Text);

                    if (line.Words != null)
                    {
                        foreach (DetectedTextWord word in line.Words)
                        {
                            if (word == null || string.IsNullOrWhiteSpace(word.Text))
                                continue;

                            // logging for word and text confidence
                            Console.WriteLine($"  Word: {word.Text} (Confidence: {word.Confidence})");
                        }
                    }

                    lines.Add(line.Text.Trim());
                }
            }
            return (lines);
        } //extractlines()
    }
}
