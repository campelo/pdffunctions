//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

#include <iostream>
#include <PDF/PDFNet.h>
#include <PDF/PDFDoc.h>
#include <PDF/HTML2PDF.h>
#include "../../LicenseKey/CPP/LicenseKey.h"

using namespace std;
using namespace pdftron;
using namespace Common;
using namespace SDF;
using namespace PDF;

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

int main(int argc, char * argv[])
{
	int ret = 0;

	std::string output_path = "../../TestFiles/Output/html2pdf_example";
	std::string host = "https://docs.apryse.com";
	std::string page0 = "/";
	std::string page1 = "/all-products/";
	std::string page2 = "/documentation/web/faq";
	
	
	// The first step in every application using PDFNet is to initialize the 
	// library and set the path to common PDF resources. The library is usually 
	// initialized only once, but calling Initialize() multiple times is also fine.
	PDFNet::Initialize(LicenseKey);

	// For HTML2PDF we need to locate the html2pdf module. If placed with the 
	// PDFNet library, or in the current working directory, it will be loaded
	// automatically. Otherwise, it must be set manually using HTML2PDF.SetModulePath().
	HTML2PDF::SetModulePath("../../../Lib");
	if (!HTML2PDF::IsModuleAvailable())
	{
		cout << endl;
		cout << "Unable to run HTMLPDFTest: Apryse SDK HTML2PDF module not available." << endl;
		cout << "---------------------------------------------------------------" << endl;
		cout << "The HTML2PDF module is an optional add-on, available for download" << endl;
		cout << "at https://www.pdftron.com/. If you have already downloaded this" << endl;
		cout << "module, ensure that the SDK is able to find the required files" << endl;
		cout << "using the HTML2PDF::SetModulePath() function." << endl << endl;
		return 1;
	}

	//--------------------------------------------------------------------------------
	// Example 1) Simple conversion of a web page to a PDF doc. 
	try
	{
		PDFDoc doc;

		// now convert a web page, sending generated PDF pages to doc
		HTML2PDF::Convert(doc, host + page0);
		doc.Save(output_path + "_01.pdf", SDFDoc::e_linearized, NULL);
	}
	catch (Common::Exception& e)
	{
		cout << e << endl;
		ret = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		ret = 1;
	}

	//--------------------------------------------------------------------------------
	// Example 2) Modify the settings of the generated PDF pages and attach to an
	// existing PDF document. 

	try
	{
		// open the existing PDF, and initialize the security handler
		PDFDoc doc("../../TestFiles/numbered.pdf");
		doc.InitSecurityHandler();

		// create the HTML2PDF converter object and modify the output of the PDF pages
		HTML2PDF converter;
		converter.SetPaperSize(PrinterMode::e_11x17);

		// insert the web page to convert
		converter.InsertFromURL(host + page0);

		// convert the web page, appending generated PDF pages to doc
		converter.Convert(doc);
		doc.Save(output_path + "_02.pdf", SDFDoc::e_linearized, NULL);
	}
	catch (Common::Exception& e)
	{
		cout << e << endl;
		ret = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		ret = 1;
	}


	//--------------------------------------------------------------------------------
	// Example 3) Convert multiple web pages
	try
	{
		// convert page 0 into pdf
		PDFDoc doc;
		HTML2PDF converter;
		UString header("<div style='width:15%;margin-left:0.5cm;text-align:left;font-size:10px;color:#0000FF'><span class='date'></span></div><div style='width:70%;direction:rtl;white-space:nowrap;overflow:hidden;text-overflow:clip;text-align:center;font-size:10px;color:#0000FF'><span>PDFTRON HEADER EXAMPLE</span></div><div style='width:15%;margin-right:0.5cm;text-align:right;font-size:10px;color:#0000FF'><span class='pageNumber'></span> of <span class='totalPages'></span></div>");
		UString footer("<div style='width:15%;margin-left:0.5cm;text-align:left;font-size:7px;color:#FF00FF'><span class='date'></span></div><div style='width:70%;direction:rtl;white-space:nowrap;overflow:hidden;text-overflow:clip;text-align:center;font-size:7px;color:#FF00FF'><span>PDFTRON FOOTER EXAMPLE</span></div><div style='width:15%;margin-right:0.5cm;text-align:right;font-size:7px;color:#FF00FF'><span class='pageNumber'></span> of <span class='totalPages'></span></div>");
		converter.SetHeader(header);
		converter.SetFooter(footer);
		converter.SetMargins("1cm", "2cm", ".5cm", "1.5cm");
		HTML2PDF::WebPageSettings settings;
		settings.SetZoom(0.5);
		converter.InsertFromURL(host + page0, settings);
		converter.Convert(doc);

		// convert page 1 with the same settings, appending generated PDF pages to doc
		converter.InsertFromURL(host + page1, settings);
		converter.Convert(doc);
	
		// convert page 2 with different settings, appending generated PDF pages to doc
		HTML2PDF another_converter;
		another_converter.SetLandscape(true);
		HTML2PDF::WebPageSettings another_settings;
		another_settings.SetPrintBackground(false);
		another_converter.InsertFromURL(host + page2, another_settings);
		another_converter.Convert(doc);

		doc.Save(output_path + "_03.pdf", SDFDoc::e_linearized, NULL);
	}
	catch (Common::Exception& e)
	{
		std::cout << e << endl;
		ret = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		ret = 1;
	}

	//--------------------------------------------------------------------------------
	// Example 4) Convert HTML string to PDF. 

	try
	{
		PDFDoc doc;

		HTML2PDF converter;
	
		// Our HTML data
		UString html("<html><body><h1>Heading</h1><p>Paragraph.</p></body></html>");
		
		// Add html data
		converter.InsertFromHtmlString(html);
		// Note, InsertFromHtmlString can be mixed with the other Insert methods.
		
		converter.Convert(doc);
		doc.Save(output_path + "_04.pdf", SDFDoc::e_linearized, NULL);
	}
	catch (Common::Exception& e)
	{
		std::cout << e << endl;
		ret = 1;		
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		ret = 1;
	}

	//--------------------------------------------------------------------------------
	// Example 5) Set the location of the log file to be used during conversion. 
	try
	{
		PDFDoc doc;
		HTML2PDF converter;
		converter.SetLogFilePath("../../TestFiles/Output/html2pdf.log");
		converter.InsertFromURL(host + page0);
		converter.Convert(doc);
		doc.Save(output_path + "_05.pdf", SDFDoc::e_linearized, NULL);
	}
	catch (Common::Exception& e)
	{
		cout << e << endl;
		ret = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		ret = 1;
	}

	PDFNet::Terminate();
	return ret;

}
