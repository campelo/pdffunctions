//
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import com.pdftron.common.PDFNetException;
import com.pdftron.pdf.*;

//---------------------------------------------------------------------------------------
// The following sample illustrates how to use the PDF::Convert utility class to convert 
// documents and files to Office.
//
// The Structured Output module is an optional PDFNet Add-on that can be used to convert PDF
// and other documents into Word, Excel, PowerPoint and HTML format.
//
// The Apryse SDK Structured Output module can be downloaded from
// https://docs.apryse.com/documentation/core/info/modules/
//
// Please contact us if you have any questions.
//---------------------------------------------------------------------------------------

public class PDF2OfficeTest 
{
    // Relative path to the folder containing test files.
    static String inputPath = "../../TestFiles/";
    static String outputPath = "../../TestFiles/Output/";

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public static void main(String[] args) 
    {
        // The first step in every application using PDFNet is to initialize the 
        // library. The library is usually initialized only once, but calling 
        // Initialize() multiple times is also fine.
        PDFNet.initialize(PDFTronLicense.Key());

        PDFNet.addResourceSearchPath("../../../Lib/");

        try {
            if (!StructuredOutputModule.isModuleAvailable()) {
                System.out.println();
                System.out.println("Unable to run the sample: Apryse SDK Structured Output module not available.");
                System.out.println("-----------------------------------------------------------------------------");
                System.out.println("The Structured Output module is an optional add-on, available for download");
                System.out.println("at https://docs.apryse.com/documentation/core/info/modules/. If you have already");
                System.out.println("downloaded this module, ensure that the SDK is able to find the required files");
                System.out.println("using the PDFNet::AddResourceSearchPath() function.");
                System.out.println();
                return;
            }
        } catch (PDFNetException e) {
            System.out.println(e);
            return;
        }  catch (Exception e) {
            System.out.println(e);
            return;
        }

        boolean err = false;

        //////////////////////////////////////////////////////////////////////////
        // Word
        //////////////////////////////////////////////////////////////////////////

        try {
            // Convert PDF document to Word
            System.out.println("Converting PDF to Word");

            String outputFile = outputPath + "paragraphs_and_tables.docx";

            Convert.toWord(inputPath + "paragraphs_and_tables.pdf", outputFile);

            System.out.println("Result saved in " + outputFile);
        } catch (PDFNetException e) {
            System.out.println("Unable to convert PDF document to Word, error: ");
            System.out.println(e);
            err = true;
        }  catch (Exception e) {
            System.out.println("Unknown Exception, error: ");
            System.out.println(e);
            err = true;
        }

        //////////////////////////////////////////////////////////////////////////

        try {
            // Convert PDF document to Word with options
            System.out.println("Converting PDF to Word with options");

            String outputFile = outputPath + "paragraphs_and_tables_first_page.docx";

            Convert.WordOutputOptions wordOutputOptions = new Convert.WordOutputOptions();

            // Convert only the first page
            wordOutputOptions.setPages(1, 1);

            Convert.toWord(inputPath + "paragraphs_and_tables.pdf", outputFile, wordOutputOptions);

            System.out.println("Result saved in " + outputFile);
        } catch (PDFNetException e) {
            System.out.println("Unable to convert PDF document to Word, error: ");
            System.out.println(e);
            err = true;
        }  catch (Exception e) {
            System.out.println("Unknown Exception, error: ");
            System.out.println(e);
            err = true;
        }

        //////////////////////////////////////////////////////////////////////////
        // Excel
        //////////////////////////////////////////////////////////////////////////

        try {
            // Convert PDF document to Excel
            System.out.println("Converting PDF to Excel");

            String outputFile = outputPath + "paragraphs_and_tables.xlsx";

            Convert.toExcel(inputPath + "paragraphs_and_tables.pdf", outputFile);

            System.out.println("Result saved in " + outputFile);
        } catch (PDFNetException e) {
            System.out.println("Unable to convert PDF document to Excel, error: ");
            System.out.println(e);
            err = true;
        }  catch (Exception e) {
            System.out.println("Unknown Exception, error: ");
            System.out.println(e);
            err = true;
        }

        //////////////////////////////////////////////////////////////////////////

        try (PDFDoc doc = new PDFDoc()) {
            // Convert PDF document to Excel with options
            System.out.println("Converting PDF to Excel with options");

            String outputFile = outputPath + "paragraphs_and_tables_second_page.xlsx";

            Convert.ExcelOutputOptions excelOutputOptions = new Convert.ExcelOutputOptions();

            // Convert only the second page
            excelOutputOptions.setPages(2, 2);

            Convert.toExcel(inputPath + "paragraphs_and_tables.pdf", outputFile, excelOutputOptions);

            System.out.println("Result saved in " + outputFile);
        } catch (PDFNetException e) {
            System.out.println("Unable to convert PDF document to Excel, error: ");
            System.out.println(e);
            err = true;
        }  catch (Exception e) {
            System.out.println("Unknown Exception, error: ");
            System.out.println(e);
            err = true;
        }

        //////////////////////////////////////////////////////////////////////////
        // PowerPoint
        //////////////////////////////////////////////////////////////////////////

        try {
            // Convert PDF document to PowerPoint
            System.out.println("Converting PDF to PowerPoint");

            String outputFile = outputPath + "paragraphs_and_tables.pptx";

            Convert.toPowerPoint(inputPath + "paragraphs_and_tables.pdf", outputFile);

            System.out.println("Result saved in " + outputFile);
        } catch (PDFNetException e) {
            System.out.println("Unable to convert PDF document to PowerPoint, error: ");
            System.out.println(e);
            err = true;
        }  catch (Exception e) {
            System.out.println("Unknown Exception, error: ");
            System.out.println(e);
            err = true;
        }

        //////////////////////////////////////////////////////////////////////////

        try {
            // Convert PDF document to PowerPoint with options
            System.out.println("Converting PDF to PowerPoint with options");

            String outputFile = outputPath + "paragraphs_and_tables_first_page.pptx";

            Convert.PowerPointOutputOptions powerPointOutputOptions = new Convert.PowerPointOutputOptions();

            // Convert only the first page
            powerPointOutputOptions.setPages(1, 1);

            Convert.toPowerPoint(inputPath + "paragraphs_and_tables.pdf", outputFile, powerPointOutputOptions);

            System.out.println("Result saved in " + outputFile);
        } catch (PDFNetException e) {
            System.out.println("Unable to convert PDF document to PowerPoint, error: ");
            System.out.println(e);
            err = true;
        }  catch (Exception e) {
            System.out.println("Unknown Exception, error: ");
            System.out.println(e);
            err = true;
        }

        //////////////////////////////////////////////////////////////////////////

        PDFNet.terminate();
        System.out.println("Done.");        
    }
}
