//
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
//

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using pdftron;
using pdftron.Common;
using pdftron.Filters;
using pdftron.SDF;
using pdftron.PDF;

namespace OfficeTemplateTestCS
{
    /// <summary>
    ///---------------------------------------------------------------------------------------
    /// The following sample illustrates how to use the PDF::Convert utility class 
    /// to convert MS Office files to PDF and replace templated tags present in the document
    /// with content supplied via json
    ///
    /// For a detailed specification of the template format and supported features,
    /// see: https://docs.apryse.com/documentation/core/guides/generate-via-template/data-model/
    ///
    /// This conversion is performed entirely within the PDFNet and has *no* external or
    /// system dependencies dependencies
    ///
    /// Please contact us if you have any questions.    
    ///---------------------------------------------------------------------------------------
    /// </summary>



    class Class1
    {
        private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
        static Class1() { }

        static String input_path = "../../TestFiles/";
        static String output_path = "../../TestFiles/Output/";
        static String json = "{\"dest_given_name\": \"Janice N.\", \"dest_street_address\": \"187 Duizelstraat\", \"dest_surname\": \"Symonds\", \"dest_title\": \"Ms.\", \"land_location\": \"225 Parc St., Rochelle, QC \"," +
                "\"lease_problem\": \"According to the city records, the lease was initiated in September 2010 and never terminated\", \"logo\": {\"image_url\": \"" + input_path + "logo_red.png\", \"width\" : 64, \"height\" : 64}," +
                "\"sender_name\": \"Arnold Smith\"}";

        static String input_filename = "SYH_Letter.docx";
        static String output_filename = "SYH_Letter.pdf";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            PDFNet.Initialize(PDFTronLicense.Key);

            try
            {
                // Create a TemplateDocument object from an input office file.
                using (TemplateDocument template_doc = pdftron.PDF.Convert.CreateOfficeTemplate(input_path + input_filename, null))
                {
                    // Fill the template with data from a JSON string, producing a PDF document.
                    PDFDoc pdfdoc = template_doc.FillTemplateJson(json);

                    // Save the PDF to a file.
                    pdfdoc.Save(output_path + output_filename, SDFDoc.SaveOptions.e_linearized);

                    // And we're done!
                    Console.WriteLine("Saved " + output_filename);
                }
            }
            catch (pdftron.Common.PDFNetException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unrecognized Exception: " + e.Message);
            }

            PDFNet.Terminate();
            Console.WriteLine("Done.");
        }
    }
}
