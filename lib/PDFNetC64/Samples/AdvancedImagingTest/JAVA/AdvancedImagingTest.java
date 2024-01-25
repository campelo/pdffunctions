//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import com.pdftron.common.PDFNetException;
import com.pdftron.pdf.*;
import com.pdftron.sdf.Obj;
import com.pdftron.sdf.ObjSet;
import com.pdftron.sdf.SDFDoc;

//---------------------------------------------------------------------------------------
// The following sample illustrates how to convert AdvancedImaging documents to PDF format using
// the AdvancedImaging class.
// 
// 'pdftron.PDF.AdvancedImaging' is an optional PDFNet Add-On utility class that can be 
// used to convert AdvancedImaging documents into PDF documents by using an external module (AdvancedImaging).
//
// AdvancedImaging modules can be downloaded from http://www.pdftron.com/pdfnet/downloads.html.
//---------------------------------------------------------------------------------------
public class AdvancedImagingTest {

	public static void main(String[] args)
	{
		System.getProperty("sun.arch.data.model");

		PDFNet.initialize(PDFTronLicense.Key());
		try {
			PDFNet.addResourceSearchPath("../../../Lib/");
			if(!AdvancedImagingModule.isModuleAvailable())
			{
				System.out.println();
				System.out.println("Unable to run AdvancedImagingTest: Apryse SDK AdvancedImaging module not available.");
				System.out.println("---------------------------------------------------------------");
				System.out.println("The AdvancedImaging module is an optional add-on, available for download");
				System.out.println("at http://www.pdftron.com/. If you have already downloaded this");
				System.out.println("module, ensure that the SDK is able to find the required files");
				System.out.println("using the PDFNet::AddResourceSearchPath() function." );
				System.out.println();
			}
		} catch (PDFNetException e) {
			System.out.println("AdvancedImaging module not available, error:");
			e.printStackTrace();
			System.out.println(e);
		}

		// Relative path to the folder containing test files.
		String input_path = "../../TestFiles/AdvancedImaging/";
		String output_path = "../../TestFiles/Output/";
		// The input file names
		String dicom_input_file = "xray.dcm";
		String heic_input_file  = "jasper.heic";
		String psd_input_file = "tiger.psd";
		String outputFile;

		System.out.println("-------------------------------------------------");
		System.out.println("Converting DICOM document to PDF");
	
		try (PDFDoc doc = new PDFDoc()) {
			AdvancedImagingConvertOptions opts = new AdvancedImagingConvertOptions();
 			opts.setDefaultDPI(72.0);
			Convert.fromDICOM(doc, input_path + dicom_input_file, opts);
			outputFile = output_path + dicom_input_file + ".pdf";
			doc.save(outputFile, SDFDoc.SaveMode.LINEARIZED, null);
			System.out.println("Result saved in " + outputFile);
		} catch (PDFNetException e) {
			System.out.println("Unable to convert DICOM document, error:");
			e.printStackTrace();
			System.out.println(e);
		}
		System.out.println("Converting HEIC document to PDF");
		try (PDFDoc doc = new PDFDoc()) {
			Convert.toPdf(doc, input_path + heic_input_file);
			outputFile = output_path + heic_input_file + ".pdf";
			doc.save(outputFile, SDFDoc.SaveMode.LINEARIZED, null);
			System.out.println("Result saved in " + outputFile);
		} catch (PDFNetException e) {
			System.out.println("Unable to convert HEIC document, error:");
			e.printStackTrace();
			System.out.println(e);
		}

		System.out.println("Converting PSD document to PDF");
		try (PDFDoc doc = new PDFDoc()) {
			Convert.toPdf(doc, input_path + psd_input_file);
			outputFile = output_path + psd_input_file + ".pdf";
			doc.save(outputFile, SDFDoc.SaveMode.LINEARIZED, null);
			System.out.println("Result saved in " + outputFile);
		} catch (PDFNetException e) {
			System.out.println("Unable to convert PSD document, error:");
			e.printStackTrace();
			System.out.println(e);
		}

		System.out.println("Done.");
		PDFNet.terminate();
	}
}
