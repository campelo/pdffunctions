//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

#include <PDF/DataExtractionModule.h>
#include <PDF/PDFNet.h>
#include <PDF/PDFDoc.h>
#include <PDF/Convert.h>
#include <Filters/MemoryFilter.h>
#include <string>
#include <iostream>
#include <fstream>
#include "../../LicenseKey/CPP/LicenseKey.h"

using namespace pdftron;
using namespace PDF;
using namespace Filters;
using namespace std;

//---------------------------------------------------------------------------------------
// The Data Extraction suite is an optional PDFNet add-on collection that can be used to
// extract various types of data from PDF documents.
//
// The Apryse SDK Data Extraction suite can be downloaded from http://www.pdftron.com/
//---------------------------------------------------------------------------------------

void WriteTextToFile(const std::string& filename, const UString& text)
{
	ofstream out_file(filename.c_str(), ofstream::binary);
	string out_buf = text.ConvertToUtf8();
	out_file.write(out_buf.c_str(), out_buf.size());
	out_file.close();
}


string input_path("../../TestFiles/");
string output_path("../../TestFiles/Output/");

//---------------------------------------------------------------------------------------
// The following sample illustrates how to extract tables from PDF documents.
//---------------------------------------------------------------------------------------
void TestTabularData()
{
	// Test if the add-on is installed
	if (!DataExtractionModule::IsModuleAvailable(DataExtractionModule::e_Tabular))
	{
		cout << endl;
		cout << "Unable to run Data Extraction: Apryse SDK Tabular Data module not available." << endl;
		cout << "---------------------------------------------------------------" << endl;
		cout << "The Data Extraction suite is an optional add-on, available for download" << endl;
		cout << "at http://www.pdftron.com/. If you have already downloaded this" << endl;
		cout << "module, ensure that the SDK is able to find the required files" << endl;
		cout << "using the PDFNet::AddResourceSearchPath() function." << endl << endl;
		return;
	}

	// Extract tabular data as a JSON file
	DataExtractionModule::ExtractData(input_path + UString("table.pdf"), output_path + UString("table.json"), DataExtractionModule::e_Tabular);

	// Extract tabular data as a JSON string
	UString json = DataExtractionModule::ExtractData(input_path + UString("financial.pdf"), DataExtractionModule::e_Tabular);
	WriteTextToFile((output_path + "financial.json").c_str(), json);

	// Extract tabular data as an XLSX file
	DataExtractionModule::ExtractToXLSX(input_path + UString("table.pdf"), output_path + UString("table.xlsx"));

	// Extract tabular data as an XLSX stream (also known as filter)
	MemoryFilter output_xlsx_stream(0, false);
	DataExtractionOptions options;
	options.SetPages("1"); // extract page 1
	DataExtractionModule::ExtractToXLSX(input_path + UString("financial.pdf"), output_xlsx_stream, &options);
	output_xlsx_stream.SetAsInputFilter();
	output_xlsx_stream.WriteToFile(output_path + UString("financial.xlsx"), false);
}

//---------------------------------------------------------------------------------------
// The following sample illustrates how to extract document structure from PDF documents.
//---------------------------------------------------------------------------------------
void TestDocumentStructure()
{
	// Test if the add-on is installed
	if (!DataExtractionModule::IsModuleAvailable(DataExtractionModule::e_DocStructure))
	{
		cout << endl;
		cout << "Unable to run Data Extraction: Apryse SDK Structured Output module not available." << endl;
		cout << "---------------------------------------------------------------" << endl;
		cout << "The Data Extraction suite is an optional add-on, available for download" << endl;
		cout << "at http://www.pdftron.com/. If you have already downloaded this" << endl;
		cout << "module, ensure that the SDK is able to find the required files" << endl;
		cout << "using the PDFNet::AddResourceSearchPath() function." << endl << endl;
		return;
	}

	// Extract document structure as a JSON file
	DataExtractionModule::ExtractData(input_path + UString("paragraphs_and_tables.pdf"), output_path + UString("paragraphs_and_tables.json"), DataExtractionModule::e_DocStructure);

	// Extract document structure as a JSON string
	UString json = DataExtractionModule::ExtractData(input_path + UString("tagged.pdf"), DataExtractionModule::e_DocStructure);
	WriteTextToFile((output_path + "tagged.json").c_str(), json);
}

//---------------------------------------------------------------------------------------
// The following sample illustrates how to extract form fields from PDF documents.
//---------------------------------------------------------------------------------------
void TestFormFields()
{
	// Test if the add-on is installed
	if (!DataExtractionModule::IsModuleAvailable(DataExtractionModule::e_Form))
	{
		cout << endl;
		cout << "Unable to run Data Extraction: Apryse SDK AIFormFieldExtractor module not available." << endl;
		cout << "---------------------------------------------------------------" << endl;
		cout << "The Data Extraction suite is an optional add-on, available for download" << endl;
		cout << "at http://www.pdftron.com/. If you have already downloaded this" << endl;
		cout << "module, ensure that the SDK is able to find the required files" << endl;
		cout << "using the PDFNet::AddResourceSearchPath() function." << endl << endl;
		return;
	}

	// Extract form fields as a JSON file
	DataExtractionModule::ExtractData(input_path + UString("formfields-scanned.pdf"), output_path + UString("formfields-scanned.json"), DataExtractionModule::e_Form);

	// Extract form fields as a JSON string
	UString json = DataExtractionModule::ExtractData(input_path + UString("formfields.pdf"), DataExtractionModule::e_Form);
	WriteTextToFile((output_path + "formfields.json").c_str(), json);
}

int main(int argc, char* argv[])
{
	// The first step in every application using PDFNet is to initialize the 
	// library and set the path to common PDF resources. The library is usually 
	// initialized only once, but calling Initialize() multiple times is also fine.
	PDFNet::Initialize(LicenseKey);

	int ret = 0;

	try
	{
		PDFNet::AddResourceSearchPath("../../../Lib/");

		TestTabularData();
		TestDocumentStructure();
		TestFormFields();
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
