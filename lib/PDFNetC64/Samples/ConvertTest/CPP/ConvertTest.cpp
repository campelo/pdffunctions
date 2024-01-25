//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

#include <iostream>
#include <sstream>
#include <PDF/PDFNet.h>
#include <PDF/Convert.h>
#include "../../LicenseKey/CPP/LicenseKey.h"

//---------------------------------------------------------------------------------------
// The following sample illustrates how to use the PDF::Convert utility class to convert 
// documents and files to PDF, XPS, or SVG. The sample also shows how to convert MS Office files 
// using our built in conversion.
//
// Certain file formats such as XPS, PDF, and raster image formats can be directly 
// converted to PDF or XPS. 

// Please contact us if you have any questions.	
//---------------------------------------------------------------------------------------

using namespace pdftron;
using namespace PDF;
using namespace std;

UString inputPath("../../TestFiles/");
UString outputPath("../../TestFiles/Output/");

typedef struct  
{
	UString inputFile, outputFile;
}
Testfile;

Testfile testfiles[] = 
{
	{ "simple-word_2007.docx",		"docx2pdf.pdf"},
	{ "simple-powerpoint_2007.pptx",	"pptx2pdf.pdf"},
	{ "simple-excel_2007.xlsx",		"xlsx2pdf.pdf"},
	{ "simple-text.txt",			"txt2pdf.pdf"},
	{ "butterfly.png",			"png2pdf.pdf"},
	{ "simple-xps.xps",			"xps2pdf.pdf"}
};

int ConvertSpecificFormats();  // convert to/from PDF, XPS, SVG
int ConvertToPdfFromFile();	   // convert from a file to PDF automatically

int main(int argc, char *argv[])
{	
	// The first step in every application using PDFNet is to initialize the 
	// library. The library is usually initialized only once, but calling 
	// Initialize() multiple times is also fine.
	int err = 0;

	PDFNet::Initialize(LicenseKey);

	// Demonstrate Convert::ToPdf
	err = ConvertToPdfFromFile();
	if (err)
	{
		cout << "ConvertFile failed" << endl;
	}
	else
	{
		cout << "ConvertFile succeeded" << endl;
	}

	// Demonstrate Convert::[FromXps, ToSVG, ToXPS]
	err = ConvertSpecificFormats();
	if (err)
	{
		cout << "ConvertSpecificFormats failed" << endl;
	}
	else
	{
		cout << "ConvertSpecificFormats succeeded" << endl;
	}

	PDFNet::Terminate();
	cout << "Done.\n";
	return err;
}

int ConvertToPdfFromFile()
{
	int ret = 0;

	unsigned int ceTestfiles = sizeof (testfiles) / sizeof (Testfile);

	for (unsigned int i = 0; i < ceTestfiles; i++)
	{

		try
		{
			PDFDoc pdfdoc;
			UString inputFile = inputPath + testfiles[i].inputFile;
			UString outputFile = outputPath + testfiles[i].outputFile;

			Convert::Printer::SetMode(Convert::Printer::e_prefer_builtin_converter);
			Convert::ToPdf(pdfdoc, inputFile);
			pdfdoc.Save(outputFile, SDF::SDFDoc::e_linearized, NULL);
			cout << "Converted file: " << testfiles[i].inputFile << endl << "to: " << testfiles[i].outputFile << endl;
		}
		catch (Common::Exception& e)
		{
			cout << "Unable to convert file " << testfiles[i].inputFile.ConvertToAscii().c_str() << endl;
			cout << e << endl;
			ret = 1;
		}
		catch (...)
		{
			cout << "Unknown Exception" << endl;
			ret = 1;
		}
	}

	return ret;
}

int ConvertSpecificFormats()
{
	//////////////////////////////////////////////////////////////////////////
	int ret = 0;
	try
	{
		PDFDoc pdfdoc;

		cout << "Converting from XPS" << endl;
		Convert::FromXps(pdfdoc, inputPath + "simple-xps.xps");
		pdfdoc.Save(outputPath + "xps2pdf v2.pdf", SDF::SDFDoc::e_remove_unused, NULL);
		cout << "Saved xps2pdf v2.pdf" << endl;
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

	//////////////////////////////////////////////////////////////////////////
	try
	{
		PDFDoc pdfdoc;

		// Add a dictionary
		SDF::ObjSet set;
		SDF::Obj options = set.CreateDict();

		// Put options
		options.PutNumber("FontSize", 15);
		options.PutBool("UseSourceCodeFormatting", true);
		options.PutNumber("PageWidth", 12);
		options.PutNumber("PageHeight", 6);

		// Convert from .txt file
		cout << "Converting from txt" << endl;
		Convert::FromText(pdfdoc, inputPath + "simple-text.txt", options);
		pdfdoc.Save(outputPath + "simple-text.pdf", SDF::SDFDoc::e_remove_unused, NULL);
		cout << "Saved simple-text.pdf" << endl;
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

	//////////////////////////////////////////////////////////////////////////
	try
	{
		PDFDoc pdfdoc(inputPath + "newsletter.pdf");

		// Convert PDF document to SVG
		cout << "Converting pdfdoc to SVG" << endl;
		Convert::ToSvg(pdfdoc, outputPath + "pdf2svg v2.svg");
		cout << "Saved pdf2svg v2.svg" << endl;
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

	//////////////////////////////////////////////////////////////////////////
	try
	{
		// Convert PNG image to XPS
		cout << "Converting PNG to XPS" << endl;
		Convert::ToXps(inputPath + "butterfly.png", outputPath + "butterfly.xps");
		cout << "Saved butterfly.xps" << endl;
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

	//////////////////////////////////////////////////////////////////////////
	try
	{
		// Convert PDF document to XPS
		cout << "Converting PDF to XPS" << endl;
		Convert::ToXps(inputPath + "newsletter.pdf", outputPath + "newsletter.xps");
		cout << "Saved newsletter.xps" << endl;
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

	//////////////////////////////////////////////////////////////////////////
	try
	{
		// Convert PDF document to HTML
		cout << "Converting PDF to HTML" << endl;
		Convert::ToHtml(inputPath + "newsletter.pdf", outputPath + "newsletter");
		cout << "Saved newsletter as HTML" << endl;
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

	//////////////////////////////////////////////////////////////////////////
	try
	{
		// Convert PDF document to EPUB
		cout << "Converting PDF to EPUB" << endl;
		Convert::ToEpub(inputPath + "newsletter.pdf", outputPath + "newsletter.epub");
		cout << "Saved newsletter.epub" << endl;
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

	//////////////////////////////////////////////////////////////////////////
	try
	{
		// Convert PDF document to multipage TIFF
		cout << "Converting PDF to multipage TIFF" << endl;
		Convert::TiffOutputOptions tiff_options;
		tiff_options.SetDPI(200);
		tiff_options.SetDither(true);
		tiff_options.SetMono(true);
		Convert::ToTiff(inputPath + "newsletter.pdf", outputPath + "newsletter.tiff", tiff_options);
		cout << "Saved newsletter.tiff" << endl;
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

	//////////////////////////////////////////////////////////////////////////
	try
	{
		PDFDoc pdfdoc;

		// Convert SVG file to PDF
		cout << "Converting SVG to PDF" << endl;

		Convert::FromSVG(pdfdoc, inputPath + "tiger.svg");
		pdfdoc.Save(outputPath + "svg2pdf.pdf", SDF::SDFDoc::e_remove_unused, NULL);

		cout << "Saved svg2pdf.pdf" << endl;
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

	return ret;
}
