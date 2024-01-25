//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import com.pdftron.pdf.*;
import com.pdftron.sdf.*;

public class HTML2PDFTest {
    //---------------------------------------------------------------------------------------
    // The following sample illustrates how to convert HTML pages to PDF format using
    // the HTML2PDF class.
    //
    // 'pdftron.PDF.HTML2PDF' is an optional PDFNet Add-On utility class that can be
    // used to convert HTML web pages into PDF documents by using an external module (html2pdf).
    //
    // html2pdf modules can be downloaded from http://www.pdftron.com/pdfnet/downloads.html.
    //
    // Users can convert HTML pages to PDF using the following operations:
    // - Simple one line static method to convert a single web page to PDF.
    // - Convert HTML pages from URL or string, plus optional table of contents, in user defined order.
    // - Optionally configure settings for proxy, images, java script, and more for each HTML page.
    // - Optionally configure the PDF output, including page size, margins, orientation, and more.
    // - Optionally add table of contents, including setting the depth and appearance.
    //---------------------------------------------------------------------------------------

    public static void main(String[] args) {
        String output_path = "../../TestFiles/Output/html2pdf_example";
        String host = "https://docs.apryse.com";
        String page0 = "/";
        String page1 = "/all-products/";
        String page2 = "/documentation/web/faq";
        // The first step in every application using PDFNet is to initialize the
        // library and set the path to common PDF resources. The library is usually
        // initialized only once, but calling initialize() multiple times is also fine.
        PDFNet.initialize(PDFTronLicense.Key());
        // For HTML2PDF we need to locate the html2pdf module. If placed with the
        // PDFNet library, or in the current working directory, it will be loaded
        // automatically. Otherwise, it must be set manually using HTML2PDF.SetModulePath().
        try {
                HTML2PDF.setModulePath("../../../Lib");
                if(!HTML2PDF.isModuleAvailable())
                {
                    System.out.println();
                    System.out.println("Unable to run HTML2PDFTest: Apryse SDK HTML2PDF module not available.");
                    System.out.println("---------------------------------------------------------------");
                    System.out.println("The HTML2PDF module is an optional add-on, available for download");
                    System.out.println("at https://www.pdftron.com/. If you have already downloaded this");
                    System.out.println("module, ensure that the SDK is able to find the required files");
                    System.out.println("using the HTML2PDF.setModulePath() function." );
                    System.out.println();
                    return;
                }
        } catch (Exception e) {
            e.printStackTrace();
            return;
        }

        //--------------------------------------------------------------------------------
        // Example 1) Simple conversion of a web page to a PDF doc.

        try (PDFDoc doc = new PDFDoc()) {
            // now convert a web page, sending generated PDF pages to doc
            HTML2PDF.convert(doc, host + page0);
            doc.save(output_path + "_01.pdf", SDFDoc.SaveMode.LINEARIZED, null);
        } catch (Exception e) {
            e.printStackTrace();
            return;
        }

        //--------------------------------------------------------------------------------
        // Example 2) Modify the settings of the generated PDF pages and attach to an
        // existing PDF document.

        try (PDFDoc doc = new PDFDoc("../../TestFiles/numbered.pdf")) {
            // open the existing PDF, and initialize the security handler
            doc.initSecurityHandler();

            // create the HTML2PDF converter object and modify the output of the PDF pages
            HTML2PDF converter = new HTML2PDF();
            converter.setPaperSize(PrinterMode.e_11x17);

            // insert the web page to convert
            converter.insertFromURL(host + page0);

            // convert the web page, appending generated PDF pages to doc
            converter.convert(doc);
            doc.save(output_path + "_02.pdf", SDFDoc.SaveMode.LINEARIZED, null);
        } catch (Exception e) {
            e.printStackTrace();
            return;
        }

        //--------------------------------------------------------------------------------
        // Example 3) Convert multiple web pages

        try (PDFDoc doc = new PDFDoc()) {
            // convert page 0 into pdf

            HTML2PDF converter = new HTML2PDF();

            String header = "<div style='width:15%;margin-left:0.5cm;text-align:left;font-size:10px;color:#0000FF'><span class='date'></span></div><div style='width:70%;direction:rtl;white-space:nowrap;overflow:hidden;text-overflow:clip;text-align:center;font-size:10px;color:#0000FF'><span>PDFTRON HEADER EXAMPLE</span></div><div style='width:15%;margin-right:0.5cm;text-align:right;font-size:10px;color:#0000FF'><span class='pageNumber'></span> of <span class='totalPages'></span></div>";
            String footer = "<div style='width:15%;margin-left:0.5cm;text-align:left;font-size:7px;color:#FF00FF'><span class='date'></span></div><div style='width:70%;direction:rtl;white-space:nowrap;overflow:hidden;text-overflow:clip;text-align:center;font-size:7px;color:#FF00FF'><span>PDFTRON FOOTER EXAMPLE</span></div><div style='width:15%;margin-right:0.5cm;text-align:right;font-size:7px;color:#FF00FF'><span class='pageNumber'></span> of <span class='totalPages'></span></div>";
            converter.setHeader(header);
            converter.setFooter(footer);
            converter.setMargins("1cm", "2cm", ".5cm", "1.5cm");
            HTML2PDF.WebPageSettings settings = new HTML2PDF.WebPageSettings();
            settings.setZoom(0.5);
            converter.insertFromURL(host + page0, settings);
            converter.convert(doc);

            // convert page 1 with the same settings, appending generated PDF pages to doc
            converter.insertFromURL(host + page1, settings);
            converter.convert(doc);

            // convert page 2 with different settings, appending generated PDF pages to doc
            HTML2PDF another_converter = new HTML2PDF();;
            another_converter.setLandscape(true);
            HTML2PDF.WebPageSettings another_settings = new HTML2PDF.WebPageSettings();
            another_settings.setPrintBackground(false);
            another_converter.insertFromURL(host + page2, another_settings);
            another_converter.convert(doc);

            doc.save(output_path + "_03.pdf", SDFDoc.SaveMode.LINEARIZED, null);
        } catch (Exception e) {
            e.printStackTrace();
            return;
        }

        //--------------------------------------------------------------------------------
        // Example 4) Convert HTML string to PDF.

        try (PDFDoc doc = new PDFDoc()) {
            HTML2PDF converter = new HTML2PDF();

            // Our HTML data
            String html = "<html><body><h1>Heading</h1><p>Paragraph.</p></body></html>";

            // Add html data
            converter.insertFromHtmlString(html);
            // Note, InsertFromHtmlString can be mixed with the other Insert methods.

            converter.convert(doc);
            doc.save(output_path + "_04.pdf", SDFDoc.SaveMode.LINEARIZED, null);
        } catch (Exception e) {
            e.printStackTrace();
            return;
        }

        //--------------------------------------------------------------------------------
        // Example 5) Set the location of the log file to be used during conversion.

        try (PDFDoc doc = new PDFDoc()) {
            HTML2PDF converter = new HTML2PDF();
            converter.setLogFilePath("../../TestFiles/Output/html2pdf.log");
            converter.insertFromURL(host + page0);
            converter.convert(doc);
            doc.save(output_path + "_05.pdf", SDFDoc.SaveMode.LINEARIZED, null);
        } catch (Exception e) {
            e.printStackTrace();
            return;
        }

        PDFNet.terminate();
    }
}
