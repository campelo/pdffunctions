// Generated code. Do not modify!
//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.     
//---------------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using pdftron;
using pdftron.Common;
using pdftron.PDF;
using pdftron.SDF;

namespace AdvancedImagingTestCS
{
    /// <summary>
    //---------------------------------------------------------------------------------------
    // The following sample illustrates how to convert AdvancedImaging documents (such as dcm,
    // png) to pdf 
    //---------------------------------------------------------------------------------------
    /// </summary>
    class Class1
    {
        private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
        static Class1() {}


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            // The first step in every application using PDFNet is to initialize the 
            // library and set the path to common PDF resources. The library is usually 
            // initialized only once, but calling Initialize() multiple times is also fine.
            PDFNet.Initialize(PDFTronLicense.Key);
            PDFNet.AddResourceSearchPath("../../../Lib/");
            if (!AdvancedImagingModule.IsModuleAvailable())
            {
                Console.WriteLine();
                Console.WriteLine("Unable to run AdvancedImagingTest: Apryse SDK AdvancedImaging module not available.");
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("The AdvancedImaging module is an optional add-on, available for download");
                Console.WriteLine("at http://www.pdftron.com/. If you have already downloaded this");
                Console.WriteLine("module, ensure that the SDK is able to find the required files");
                Console.WriteLine("using the PDFNet::AddResourceSearchPath() function.");
                Console.WriteLine();
            }

            // Relative path to the folder containing test files.
            string input_path =  "../../TestFiles/AdvancedImaging/";
            string output_path = "../../TestFiles/Output/";

            string dicom_input_file = "xray.dcm";
            string heic_input_file = "jasper.heic";
            string psd_input_file = "tiger.psd";
            string output_ext = ".pdf";

            Console.WriteLine("Example of advanced imaging module:");
            try
            {
                using (PDFDoc pdfdoc = new PDFDoc())
                {
                    AdvancedImagingConvertOptions opts = new AdvancedImagingConvertOptions();
                    opts.SetDefaultDPI(72.0);

                    pdftron.PDF.Convert.FromDICOM(pdfdoc, input_path + dicom_input_file, opts);
                    pdfdoc.Save(output_path + dicom_input_file + output_ext, SDFDoc.SaveOptions.e_remove_unused);
                }

                using (PDFDoc pdfdoc = new PDFDoc())
                {
                    pdftron.PDF.Convert.ToPdf(pdfdoc, input_path + heic_input_file);
                    pdfdoc.Save(output_path + heic_input_file + output_ext, SDFDoc.SaveOptions.e_remove_unused);
                }

                using (PDFDoc pdfdoc = new PDFDoc())
                {
                    pdftron.PDF.Convert.ToPdf(pdfdoc, input_path + psd_input_file);
                    pdfdoc.Save(output_path + psd_input_file + output_ext, SDFDoc.SaveOptions.e_remove_unused);
                }

                Console.WriteLine("Done.");
            }
            catch (PDFNetException e)
            {
                Console.WriteLine(e.Message);
            }
            PDFNet.Terminate();
        }
    }
}
