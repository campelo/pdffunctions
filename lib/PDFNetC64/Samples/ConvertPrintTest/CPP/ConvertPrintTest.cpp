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
// The following sample illustrates how to convert to PDF with virtual printer on Windows.
// It supports several input formats like docx, xlsx, rtf, txt, html, pub, emf, etc. For more details, visit 
// https://docs.apryse.com/documentation/windows/guides/features/conversion/convert-other/
// 
// To check if ToPDF (or ToXPS) require that PDFNet printer is installed use Convert::RequiresPrinter(filename). 
// The installing application must be run as administrator. The manifest for this sample 
// specifies appropriate the UAC elevation.
//
// Note: the PDFNet printer is a virtual XPS printer supported on Vista SP1 and Windows 7.
// For Windows XP SP2 or higher, or Vista SP0 you need to install the XPS Essentials Pack (or 
// equivalent redistributables). You can download the XPS Essentials Pack from:
//		http://www.microsoft.com/downloads/details.aspx?FamilyId=B8DCFFDD-E3A5-44CC-8021-7649FD37FFEE&displaylang=en
// Windows XP Sp2 will also need the Microsoft Core XML Services (MSXML) 6.0:
// 		http://www.microsoft.com/downloads/details.aspx?familyid=993C0BCF-3BCF-4009-BE21-27E85E1857B1&displaylang=en
//
// Note: Convert.FromEmf and Convert.ToEmf will only work on Windows and require GDI+.
//
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
	{ "simple-publisher.pub",		"pub2pdf.pdf"},
	{ "simple-text.txt",			"txt2pdf.pdf"},
	{ "simple-rtf.rtf",			"rtf2pdf.pdf"},
	{ "simple-emf.emf",			"emf2pdf.pdf"},
	{ "simple-webpage.mht",		"mht2pdf.pdf"},
	{ "simple-webpage.html",		"html2pdf.pdf"}
};

int ConvertSpecificFormats();  // convert to/from PDF, XPS, EMF, SVG
int ConvertToPdfFromFile();	   // convert from a file to PDF automatically

int main(int argc, char *argv[])
{	

//Virtual printer only available on Windows
#if defined(_WIN32)
	// The first step in every application using PDFNet is to initialize the 
	// library. The library is usually initialized only once, but calling 
	// Initialize() multiple times is also fine.
	int err = 0;

	PDFNet::Initialize(LicenseKey);

	// Demonstrate Convert::ToPdf and Convert::Printer
	err = ConvertToPdfFromFile();
	if (err)
	{
		cout << "ConvertFile failed" << endl;
	}
	else
	{
		cout << "ConvertFile succeeded" << endl;
	}

	// Demonstrate Convert::[FromEmf, FromXps, ToEmf, ToSVG, ToXPS]
	err = ConvertSpecificFormats();
	if (err)
	{
		cout << "ConvertSpecificFormats failed" << endl;
	}
	else
	{
		cout << "ConvertSpecificFormats succeeded" << endl;
	}

	if (Convert::Printer::IsInstalled())
	{
		try 
		{
			cout << "Uninstalling printer (requires Windows platform and administrator)" << endl;
			Convert::Printer::Uninstall();
			cout << "Uninstalled printer " << Convert::Printer::GetPrinterName().ConvertToAscii().c_str() << endl;;
		}
		catch (Common::Exception)
		{
			cout << "Unable to uninstall printer" << endl;
		}
	}

	PDFNet::Terminate();
	cout << "Done.\n";
	return err;
#else
	cout << "ConvertPrintTest only available on Windows\n";
#endif // defined(_WIN32)

}

int ConvertToPdfFromFile()
{
	int ret = 0;

	if( Convert::Printer::IsInstalled("PDFTron PDFNet") )
	{
		Convert::Printer::SetPrinterName("PDFTron PDFNet");
	}
	else if (!Convert::Printer::IsInstalled())
	{
		try
		{
			// This will fail if not run as administrator. Harmless if PDFNet 
			// printer already installed
			cout << "Installing printer (requires Windows platform and administrator)\n";
			Convert::Printer::Install();
			cout << "Installed printer " << Convert::Printer::GetPrinterName().ConvertToAscii().c_str() << endl;
		}
		catch (Common::Exception)
		{
			cout << "Unable to install printer" << endl;
		}
	}

	unsigned int ceTestfiles = sizeof (testfiles) / sizeof (Testfile);

	for (unsigned int i = 0; i < ceTestfiles; i++)
	{
		try
		{
			PDFDoc pdfdoc;
			UString inputFile = inputPath + testfiles[i].inputFile;
			UString outputFile = outputPath + testfiles[i].outputFile;
			if (Convert::RequiresPrinter(inputFile))
			{
				cout << "Using PDFNet printer to convert file " << testfiles[i].inputFile << endl;
			}
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

		cout << "Converting from EMF" << endl;
		Convert::FromEmf(pdfdoc, inputPath + "simple-emf.emf");
		pdfdoc.Save(outputPath + "emf2pdf v2.pdf", SDF::SDFDoc::e_remove_unused, NULL);
		cout << "Saved emf2pdf v2.pdf" << endl;
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
		// Convert MSWord document to XPS
		cout << "Converting DOCX to XPS" << endl;
		Convert::ToXps(inputPath + "simple-word_2007.docx", outputPath + "simple-word_2007.xps");
		cout << "Saved simple-word_2007.xps" << endl;
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
