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
// documents and files to HTML.
//
// There are two HTML modules and one of them is an optional PDFNet Add-on.
// 1. The built-in HTML module is used to convert PDF documents to fixed-position HTML
//    documents.
// 2. The optional add-on module is used to convert PDF documents to HTML documents with
//    text flowing across the browser window.
//
// The Apryse SDK HTML add-on module can be downloaded from http://www.pdftron.com/
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

	int err = 0;

	//////////////////////////////////////////////////////////////////////////

	try
	{
		// Convert PDF document to HTML with fixed positioning option turned on (default)
		cout << "Converting PDF to HTML with fixed positioning option turned on (default)" << endl;

		UString outputFile = outputPath + "paragraphs_and_tables_fixed_positioning";

		// Convert PDF to HTML
		Convert::ToHtml(inputPath + "paragraphs_and_tables.pdf", outputFile);

		cout << "Result saved in " << outputFile.ConvertToUtf8().c_str() << endl;
	}
	catch (Common::Exception& e)
	{
		cout << "Unable to convert PDF document to HTML, error: " << e << endl;
		err = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		err = 1;
	}

	//////////////////////////////////////////////////////////////////////////

	PDFNet::AddResourceSearchPath("../../../Lib/");

	if (!StructuredOutputModule::IsModuleAvailable())
	{
		cout << endl;
		cout << "Unable to run part of the sample: Apryse SDK Structured Output module not available." << endl;
		cout << "-------------------------------------------------------------------------------------" << endl;
		cout << "The Structured Output module is an optional add-on, available for download" << endl;
		cout << "at https://docs.apryse.com/documentation/core/info/modules/. If you have already" << endl;
		cout << "downloaded this module, ensure that the SDK is able to find the required files" << endl;
		cout << "using the PDFNet::AddResourceSearchPath() function." << endl;
		cout << endl;
		return 0;
	}

	//////////////////////////////////////////////////////////////////////////

	try
	{
		// Convert PDF document to HTML with reflow full option turned on (1)
		cout << "Converting PDF to HTML with reflow full option turned on (1)" << endl;

		UString outputFile = outputPath + "paragraphs_and_tables_reflow_full.html";

		Convert::HTMLOutputOptions htmlOutputOptions;

		// Set e_reflow_full content reflow setting
		htmlOutputOptions.SetContentReflowSetting(Convert::HTMLOutputOptions::e_reflow_full);

		// Convert PDF to HTML
		Convert::ToHtml(inputPath + "paragraphs_and_tables.pdf", outputFile, htmlOutputOptions);

		cout << "Result saved in " << outputFile.ConvertToUtf8().c_str() << endl;
	}
	catch (Common::Exception& e)
	{
		cout << "Unable to convert PDF document to HTML, error: " << e << endl;
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
		// Convert PDF document to HTML with reflow full option turned on (only converting the first page) (2)
		cout << "Converting PDF to HTML with reflow full option turned on (only converting the first page) (2)" << endl;

		UString outputFile = outputPath + "paragraphs_and_tables_reflow_full_first_page.html";

		Convert::HTMLOutputOptions htmlOutputOptions;

		// Set e_reflow_full content reflow setting
		htmlOutputOptions.SetContentReflowSetting(Convert::HTMLOutputOptions::e_reflow_full);

		// Convert only the first page
		htmlOutputOptions.SetPages(1, 1);

		// Convert PDF to HTML
		Convert::ToHtml(inputPath + "paragraphs_and_tables.pdf", outputFile, htmlOutputOptions);

		cout << "Result saved in " << outputFile.ConvertToUtf8().c_str() << endl;
	}
	catch (Common::Exception& e)
	{
		cout << "Unable to convert PDF document to HTML, error: " << e << endl;
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
