//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import java.io.FileWriter;
import java.io.BufferedWriter;
import java.io.FileNotFoundException;
import java.io.IOException;

import com.pdftron.common.PDFNetException;
import com.pdftron.pdf.*;
import com.pdftron.filters.*;

//---------------------------------------------------------------------------------------
// The Data Extraction suite is an optional PDFNet add-on collection that can be used to
// extract various types of data from PDF documents.
//
// The Apryse SDK Data Extraction suite can be downloaded from http://www.pdftron.com/
//---------------------------------------------------------------------------------------

public class DataExtractionTest {

	static void writeTextToFile(String filename, String text) throws IOException
	{
		BufferedWriter writer = new BufferedWriter(new FileWriter(filename));
		writer.write(text);
		writer.close();
	}

	//---------------------------------------------------------------------------------------
	// The following sample illustrates how to extract tables from PDF documents.
	//---------------------------------------------------------------------------------------
	static void testTabularData()
	{
		try {
			// Test if the add-on is installed
			if (!DataExtractionModule.isModuleAvailable(DataExtractionModule.DataExtractionEngine.e_tabular))
			{
				System.out.println();
				System.out.println("Unable to run Data Extraction: Apryse SDK Tabular Data module not available.");
				System.out.println("---------------------------------------------------------------");
				System.out.println("The Data Extraction suite is an optional add-on, available for download");
				System.out.println("at http://www.pdftron.com/. If you have already downloaded this");
				System.out.println("module, ensure that the SDK is able to find the required files");
				System.out.println("using the PDFNet.addResourceSearchPath() function." );
				System.out.println();
				return;
			}
		} catch (PDFNetException e) {
			System.out.println("Data Extraction module not available, error:");
			e.printStackTrace();
			System.out.println(e);
		}

		// Relative path to the folder containing test files.
		String input_path = "../../TestFiles/";
		String output_path = "../../TestFiles/Output/";

		try {
			// Extract tabular data as a JSON file
			DataExtractionModule.extractData(input_path + "table.pdf", output_path + "table.json", DataExtractionModule.DataExtractionEngine.e_tabular);

			// Extract tabular data as a JSON string
			String json = DataExtractionModule.extractData(input_path + "financial.pdf", DataExtractionModule.DataExtractionEngine.e_tabular);
			writeTextToFile(output_path + "financial.json", json);

			// Extract tabular data as an XLSX file
			DataExtractionModule.extractToXLSX(input_path + "table.pdf", output_path + "table.xlsx");

			// Extract tabular data as an XLSX stream (also known as filter)
			DataExtractionOptions options = new DataExtractionOptions();
			options.setPages("1");
			MemoryFilter output_xlsx_stream = new MemoryFilter(0, false);
			DataExtractionModule.extractToXLSX(input_path + "financial.pdf", output_xlsx_stream, options);
			output_xlsx_stream.setAsInputFilter();
			output_xlsx_stream.writeToFile(output_path + "financial.xlsx", false);

		} catch (PDFNetException e) {
			System.out.println(e);
		}
		catch (IOException e) {
			System.out.println(e);
		}
	}

	//---------------------------------------------------------------------------------------
	// The following sample illustrates how to extract document structure from PDF documents.
	//---------------------------------------------------------------------------------------
	static void testDocumentStructure()
	{
		// Test if the add-on is installed
		try {
			if (!DataExtractionModule.isModuleAvailable(DataExtractionModule.DataExtractionEngine.e_doc_structure))
			{
				System.out.println();
				System.out.println("Unable to run Data Extraction: Apryse SDK Structured Output module not available.");
				System.out.println("---------------------------------------------------------------");
				System.out.println("The Data Extraction suite is an optional add-on, available for download");
				System.out.println("at http://www.pdftron.com/. If you have already downloaded this");
				System.out.println("module, ensure that the SDK is able to find the required files");
				System.out.println("using the PDFNet.addResourceSearchPath() function." );
				System.out.println();
				return;
			}
		} catch (PDFNetException e) {
			System.out.println("Data Extraction module not available, error:");
			e.printStackTrace();
			System.out.println(e);
		}

		// Relative path to the folder containing test files.
		String input_path = "../../TestFiles/";
		String output_path = "../../TestFiles/Output/";

		try {
			// Extract document structure as a JSON file
			DataExtractionModule.extractData(input_path + "paragraphs_and_tables.pdf", output_path + "paragraphs_and_tables.json", DataExtractionModule.DataExtractionEngine.e_doc_structure);

			// Extract document structure as a JSON string
			String json = DataExtractionModule.extractData(input_path + "tagged.pdf", DataExtractionModule.DataExtractionEngine.e_doc_structure);
			writeTextToFile(output_path + "tagged.json", json);

		} catch (PDFNetException e) {
			System.out.println(e);
		}
		catch (IOException e) {
			System.out.println(e);
		}
	}

	//---------------------------------------------------------------------------------------
	// The following sample illustrates how to extract form fields from PDF documents.
	//---------------------------------------------------------------------------------------
	static void testFormFields()
	{
		try {
			// Test if the add-on is installed
			if (!DataExtractionModule.isModuleAvailable(DataExtractionModule.DataExtractionEngine.e_form))
			{
				System.out.println();
				System.out.println("Unable to run Data Extraction: Apryse SDK AIFormFieldExtractor module not available.");
				System.out.println("---------------------------------------------------------------");
				System.out.println("The Data Extraction suite is an optional add-on, available for download");
				System.out.println("at http://www.pdftron.com/. If you have already downloaded this");
				System.out.println("module, ensure that the SDK is able to find the required files");
				System.out.println("using the PDFNet.addResourceSearchPath() function." );
				System.out.println();
				return;
			}
		} catch (PDFNetException e) {
			System.out.println("Data Extraction module not available, error:");
			e.printStackTrace();
			System.out.println(e);
		}

		// Relative path to the folder containing test files.
		String input_path = "../../TestFiles/";
		String output_path = "../../TestFiles/Output/";

		try {
			// Extract form fields as a JSON file
			DataExtractionModule.extractData(input_path + "formfields-scanned.pdf", output_path + "formfields-scanned.json", DataExtractionModule.DataExtractionEngine.e_form);

			// Extract form fields as a JSON string
			String json = DataExtractionModule.extractData(input_path + "formfields.pdf", DataExtractionModule.DataExtractionEngine.e_form);
			writeTextToFile(output_path + "formfields.json", json);

		} catch (PDFNetException e) {
			System.out.println(e);
		}
		catch (IOException e) {
			System.out.println(e);
		}
	}

	public static void main(String[] args)
	{
		// The first step in every application using PDFNet is to initialize the 
		// library and set the path to common PDF resources. The library is usually 
		// initialized only once, but calling initialize() multiple times is also fine.
		PDFNet.initialize(PDFTronLicense.Key());
		PDFNet.addResourceSearchPath("../../../Lib/");

		testTabularData();
		testDocumentStructure();
		testFormFields();

		PDFNet.terminate();
	}
}
