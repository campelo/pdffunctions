//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

#include <PDF/PDFNet.h>
#include <PDF/PDFDoc.h>
#include <PDF/Convert.h>
#include <PDF/AdvancedImagingModule.h>
#include <PDF/AdvancedImagingConvertOptions.h>
#include <string>
#include <iostream>
#include <stdio.h>
#include "../../LicenseKey/CPP/LicenseKey.h"

using namespace pdftron;
using namespace pdftron::SDF;
using namespace PDF;
using namespace std;


//---------------------------------------------------------------------------------------
// The following sample illustrates how to convert AdvancedImaging documents to PDF format 
// 
// The AdvancedImaging module is an optional PDFNet Add-on that can be used to convert AdvancedImaging
// documents into PDF documents
//
// The Apryse SDK AdvancedImaging module can be downloaded from http://www.pdftron.com/
//---------------------------------------------------------------------------------------

UString inputPath("../../TestFiles/AdvancedImaging/");
UString outputPath("../../TestFiles/Output/");


int main(int argc, char *argv[])
{
	int ret = 0;
	
	try
	{
		// The first step in every application using PDFNet is to initialize the 
		// library and set the path to common PDF resources. The library is usually 
		// initialized only once, but calling Initialize() multiple times is also fine.
		PDFNet::Initialize(LicenseKey);
		PDFNet::AddResourceSearchPath("../../../Lib/");

		if(!AdvancedImagingModule::IsModuleAvailable())
		{
			cout << endl;
			cout << "Unable to run AdvancedImagingTest: Apryse SDK AdvancedImaging module not available." << endl;
			cout << "---------------------------------------------------------------" << endl;
			cout << "The AdvancedImaging module is an optional add-on, available for download" << endl;
			cout << "at http://www.pdftron.com/. If you have already downloaded this" << endl;
			cout << "module, ensure that the SDK is able to find the required files" << endl;
			cout << "using the PDFNet::AddResourceSearchPath() function." << endl << endl;
			return 0;
		}

		typedef struct
		{
			UString inputFile, outputFile;
		} TestFile;
		
		UString dicom_input_file, heic_input_file, psd_input_file;

		dicom_input_file = "xray.dcm";
		heic_input_file = "jasper.heic";
		psd_input_file = "tiger.psd";

		try
		{
			PDFDoc pdfdoc_dicom;
			AdvancedImagingConvertOptions opts;
			opts.SetDefaultDPI(72);
			Convert::FromDICOM(pdfdoc_dicom, inputPath + dicom_input_file, &opts);
			pdfdoc_dicom.Save(outputPath + dicom_input_file + UString(".pdf"), SDF::SDFDoc::e_linearized, NULL);
		}
		catch (Common::Exception& e)
		{
			cout << "Unable to convert DICOM test file" << endl;
			cout << e << endl;
			ret = 1;
		}
		catch (...)
		{
			cout << "Unknown Exception" << endl;
			ret = 1;
		}

		try
		{
			PDFDoc pdfdoc_heic;
			Convert::ToPdf(pdfdoc_heic, inputPath + heic_input_file);
			pdfdoc_heic.Save(outputPath + heic_input_file + UString(".pdf"), SDF::SDFDoc::e_linearized, NULL);
		}
		catch (Common::Exception& e)
		{
			cout << "Unable to convert the HEIC test file" << endl;
			cout << e << endl;
			ret = 1;
		}
		catch (...)
		{
			cout << "Unknown Exception" << endl;
			ret = 1;
		}

		try
		{
			PDFDoc pdfdoc_psd;
			Convert::ToPdf(pdfdoc_psd, inputPath + psd_input_file);
			pdfdoc_psd.Save(outputPath + psd_input_file + UString(".pdf"), SDF::SDFDoc::e_linearized, NULL);
		}
		catch (Common::Exception& e)
		{
			cout << "Unable to convert the PSD test file" << endl;
			cout << e << endl;
			ret = 1;
		}
		catch (...)
		{
			cout << "Unknown Exception" << endl;
			ret = 1;
		}


        PDFNet::Terminate();
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
