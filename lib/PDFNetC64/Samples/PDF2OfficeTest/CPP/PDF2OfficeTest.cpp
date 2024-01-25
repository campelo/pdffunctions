//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

#include <iostream>
#include <sstream>
#include <PDF/PDFNet.h>
#include <PDF/Convert.h>
#include <PDF/StructuredOutputModule.h>
#include "../../LicenseKey/CPP/LicenseKey.h"

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

using namespace pdftron;
using namespace PDF;
using namespace std;

UString inputPath("../../TestFiles/");
UString outputPath("../../TestFiles/Output/");

int main(int argc, char *argv[])
{	
	// The first step in every application using PDFNet is to initialize the 
	// library. The library is usually initialized only once, but calling 
	// Initialize() multiple times is also fine.
	PDFNet::Initialize(LicenseKey);

	PDFNet::AddResourceSearchPath("../../../Lib/");

	if (!StructuredOutputModule::IsModuleAvailable())
	{
		cout << endl;
		cout << "Unable to run the sample: Apryse SDK Structured Output module not available." << endl;
		cout << "-----------------------------------------------------------------------------" << endl;
		cout << "The Structured Output module is an optional add-on, available for download" << endl;
		cout << "at https://docs.apryse.com/documentation/core/info/modules/. If you have already" << endl;
		cout << "downloaded this module, ensure that the SDK is able to find the required files" << endl;
		cout << "using the PDFNet::AddResourceSearchPath() function." << endl;
		cout << endl;
		return 1;
	}

	int err = 0;

	//////////////////////////////////////////////////////////////////////////
	// Word
	//////////////////////////////////////////////////////////////////////////

	try
	{
		// Convert PDF document to Word
		cout << "Converting PDF to Word" << endl;

		UString outputFile = outputPath + "paragraphs_and_tables.docx";

		Convert::ToWord(inputPath + "paragraphs_and_tables.pdf", outputFile);

		cout << "Result saved in " << outputFile.ConvertToUtf8().c_str() << endl;
	}
	catch (Common::Exception& e)
	{
		cout << "Unable to convert PDF document to Word, error: " << e << endl;
		err = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		err = 1;
	}

	//////////////////////////////////////////////////////////////////////////

	try
	{
		// Convert PDF document to Word with options
		cout << "Converting PDF to Word with options" << endl;

		UString outputFile = outputPath + "paragraphs_and_tables_first_page.docx";

		Convert::WordOutputOptions wordOutputOptions;

		// Convert only the first page
		wordOutputOptions.SetPages(1, 1);

		Convert::ToWord(inputPath + "paragraphs_and_tables.pdf", outputFile, wordOutputOptions);

		cout << "Result saved in " << outputFile.ConvertToUtf8().c_str() << endl;
	}
	catch (Common::Exception& e)
	{
		cout << "Unable to convert PDF document to Word, error: " << e << endl;
		err = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		err = 1;
	}

	//////////////////////////////////////////////////////////////////////////
	// Excel
	//////////////////////////////////////////////////////////////////////////

	try
	{
		// Convert PDF document to Excel
		cout << "Converting PDF to Excel" << endl;

		UString outputFile = outputPath + "paragraphs_and_tables.xlsx";

		Convert::ToExcel(inputPath + "paragraphs_and_tables.pdf", outputFile);

		cout << "Result saved in " << outputFile.ConvertToUtf8().c_str() << endl;
	}
	catch (Common::Exception& e)
	{
		cout << "Unable to convert PDF document to Excel, error: " << e << endl;
		err = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		err = 1;
	}

	//////////////////////////////////////////////////////////////////////////

	try
	{
		// Convert PDF document to Excel with options
		cout << "Converting PDF to Excel with options" << endl;

		UString outputFile = outputPath + "paragraphs_and_tables_second_page.xlsx";

		Convert::ExcelOutputOptions excelOutputOptions;

		// Convert only the second page
		excelOutputOptions.SetPages(2, 2);

		Convert::ToExcel(inputPath + "paragraphs_and_tables.pdf", outputFile, excelOutputOptions);

		cout << "Result saved in " << outputFile.ConvertToUtf8().c_str() << endl;
	}
	catch (Common::Exception& e)
	{
		cout << "Unable to convert PDF document to Excel, error: " << e << endl;
		err = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		err = 1;
	}

	//////////////////////////////////////////////////////////////////////////
	// PowerPoint
	//////////////////////////////////////////////////////////////////////////

	try
	{
		// Convert PDF document to PowerPoint
		cout << "Converting PDF to PowerPoint" << endl;

		UString outputFile = outputPath + "paragraphs_and_tables.pptx";

		Convert::ToPowerPoint(inputPath + "paragraphs_and_tables.pdf", outputFile);

		cout << "Result saved in " << outputFile.ConvertToUtf8().c_str() << endl;
	}
	catch (Common::Exception& e)
	{
		cout << "Unable to convert PDF document to PowerPoint, error: " << e << endl;
		err = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		err = 1;
	}

	//////////////////////////////////////////////////////////////////////////

	try
	{
		// Convert PDF document to PowerPoint with options
		cout << "Converting PDF to PowerPoint with options" << endl;

		UString outputFile = outputPath + "paragraphs_and_tables_first_page.pptx";

		Convert::PowerPointOutputOptions powerPointOutputOptions;

		// Convert only the first page
		powerPointOutputOptions.SetPages(1, 1);

		Convert::ToPowerPoint(inputPath + "paragraphs_and_tables.pdf", outputFile, powerPointOutputOptions);

		cout << "Result saved in " << outputFile.ConvertToUtf8().c_str() << endl;
	}
	catch (Common::Exception& e)
	{
		cout << "Unable to convert PDF document to PowerPoint, error: " << e << endl;
		err = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		err = 1;
	}

	//////////////////////////////////////////////////////////////////////////

	PDFNet::Terminate();
	cout << "Done.\n";
	return err;
}
