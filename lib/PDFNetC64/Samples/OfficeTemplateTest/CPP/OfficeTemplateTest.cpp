//------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//------------------------------------------------------------------------------

#include <iostream>
#include <PDF/PDFNet.h>
#include <PDF/Convert.h>
#include <PDF/OfficeToPDFOptions.h>
#include "../../LicenseKey/CPP/LicenseKey.h"

//------------------------------------------------------------------------------
// The following sample illustrates how to use the PDF::Convert utility class 
// to convert MS Office files to PDF and replace templated tags present in the document
// with content supplied via json
//
// For a detailed specification of the template format and supported features,
// see: https://docs.apryse.com/documentation/core/guides/generate-via-template/data-model/
//
// This conversion is performed entirely within the PDFNet and has *no* 
// external or system dependencies -- Conversion results will be
// the same whether on Windows, Linux or Android.
//
// Please contact us if you have any questions.	
//------------------------------------------------------------------------------

using namespace pdftron;
using namespace PDF;

UString input_path = "../../TestFiles/";
UString output_path = "../../TestFiles/Output/";


int main(int argc, char *argv[])
{
	// The first step in every application using PDFNet is to initialize the 
	// library. The library is usually initialized only once, but calling 
	// Initialize() multiple times is also fine.
	int ret = 0;

	PDFNet::Initialize(LicenseKey);
	PDFNet::SetResourcesPath("../../../Resources");

	UString input_filename = "SYH_Letter.docx";
	UString output_filename = "SYH_Letter.pdf";

	UString json(
		"{\"dest_given_name\": \"Janice N.\", \"dest_street_address\": \"187 Duizelstraat\", \"dest_surname\": \"Symonds\", \"dest_title\": \"Ms.\", \"land_location\": \"225 Parc St., Rochelle, QC \","
		"\"lease_problem\": \"According to the city records, the lease was initiated in September 2010 and never terminated\", \"logo\": {\"image_url\": \"" + input_path + "logo_red.png\", \"width\" : 64, \"height\" : 64},"
		"\"sender_name\": \"Arnold Smith\"}"
	);

	try
	{
		// Create a TemplateDocument object from an input office file.
		TemplateDocument template_doc = Convert::CreateOfficeTemplate(input_path + input_filename, NULL);

		// Fill the template with data from a JSON string, producing a PDF document.
		PDFDoc pdf = template_doc.FillTemplateJson(json);

		// Save the PDF to a file.
		pdf.Save(output_path + output_filename, SDF::SDFDoc::e_linearized, NULL);

		// And we're done!
		std::cout << "Saved " << output_filename << std::endl;
	}
	catch (Common::Exception& e)
	{
		std::cout << e << std::endl;
		ret = 1;
	}
	catch (...)
	{
		std::cout << "Unknown Exception" << std::endl;
		ret = 1;
	}

	PDFNet::Terminate();
	std::cout << "Done.\n";
	return ret;
}
