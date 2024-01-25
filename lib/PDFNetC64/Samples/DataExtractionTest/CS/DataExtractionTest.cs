//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

using System;

using pdftron;
using pdftron.Common;
using pdftron.PDF;
using pdftron.Filters;

namespace DataExtractionTestCS
{
	/// <summary>
	///---------------------------------------------------------------------------------------
	/// The Data Extraction suite is an optional PDFNet add-on collection that can be used to
	/// extract various types of data from PDF documents.
	///
	/// The Apryse SDK Data Extraction suite can be downloaded from http://www.pdftron.com/
	//---------------------------------------------------------------------------------------
	/// </summary>
	class Class1
	{
		private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
		static Class1() { }

		// Relative path to the folder containing test files.
		static string input_path = "../../TestFiles/";
		static string output_path = "../../TestFiles/Output/";


		/// <summary>
		/// The following sample illustrates how to extract tables from PDF documents.
		/// </summary>
		static void TestTabularData()
		{
			// Test if the add-on is installed
			if (!DataExtractionModule.IsModuleAvailable(DataExtractionModule.DataExtractionEngine.e_tabular))
			{
				Console.WriteLine();
				Console.WriteLine("Unable to run Data Extraction: Apryse SDK Tabular Data module not available.");
				Console.WriteLine("---------------------------------------------------------------");
				Console.WriteLine("The Data Extraction suite is an optional add-on, available for download");
				Console.WriteLine("at http://www.pdftron.com/. If you have already downloaded this");
				Console.WriteLine("module, ensure that the SDK is able to find the required files");
				Console.WriteLine("using the PDFNet.AddResourceSearchPath() function.");
				Console.WriteLine();
				return;
			}

			try
			{
				// Extract tabular data as a JSON file
				DataExtractionModule.ExtractData(input_path + "table.pdf", output_path + "table.json", DataExtractionModule.DataExtractionEngine.e_tabular);

				// Extract tabular data as a JSON string
				string json = DataExtractionModule.ExtractData(input_path + "financial.pdf", DataExtractionModule.DataExtractionEngine.e_tabular);
				System.IO.File.WriteAllText(output_path + "financial.json", json);

				// Extract tabular data as an XLSX file
				DataExtractionModule.ExtractToXLSX(input_path + "table.pdf", output_path + "table.xlsx");

				// Extract tabular data as an XLSX stream (also known as filter)
				MemoryFilter output_xlsx_stream = new MemoryFilter(0, false);
				DataExtractionModule.ExtractToXLSX(input_path + "financial.pdf", output_xlsx_stream);
				output_xlsx_stream.SetAsInputFilter();
				output_xlsx_stream.WriteToFile(output_path + "financial.xlsx", false);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
			}
		}


		/// <summary>
		// The following sample illustrates how to extract document structure from PDF documents.
		/// </summary>
		static void TestDocumentStructure()
		{
			// Test if the add-on is installed
			if (!DataExtractionModule.IsModuleAvailable(DataExtractionModule.DataExtractionEngine.e_doc_structure))
			{
				Console.WriteLine();
				Console.WriteLine("Unable to run Data Extraction: Apryse SDK Structured Output module not available.");
				Console.WriteLine("---------------------------------------------------------------");
				Console.WriteLine("The Data Extraction suite is an optional add-on, available for download");
				Console.WriteLine("at http://www.pdftron.com/. If you have already downloaded this");
				Console.WriteLine("module, ensure that the SDK is able to find the required files");
				Console.WriteLine("using the PDFNet.AddResourceSearchPath() function.");
				Console.WriteLine();
				return;
			}

			try
			{
				// Extract document structure as a JSON file
				DataExtractionModule.ExtractData(input_path + "paragraphs_and_tables.pdf", output_path + "paragraphs_and_tables.json", DataExtractionModule.DataExtractionEngine.e_doc_structure);

				// Extract document structure as a JSON string
				string json = DataExtractionModule.ExtractData(input_path + "tagged.pdf", DataExtractionModule.DataExtractionEngine.e_doc_structure);
				System.IO.File.WriteAllText(output_path + "tagged.json", json);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
			}
		}


		/// <summary>
		// The following sample illustrates how to extract form fields from PDF documents.
		/// </summary>
		static void TestFormFields()
		{
			// Test if the add-on is installed
			if (!DataExtractionModule.IsModuleAvailable(DataExtractionModule.DataExtractionEngine.e_form))
			{
				Console.WriteLine();
				Console.WriteLine("Unable to run Data Extraction: Apryse SDK AIFormFieldExtractor module not available.");
				Console.WriteLine("---------------------------------------------------------------");
				Console.WriteLine("The Data Extraction suite is an optional add-on, available for download");
				Console.WriteLine("at http://www.pdftron.com/. If you have already downloaded this");
				Console.WriteLine("module, ensure that the SDK is able to find the required files");
				Console.WriteLine("using the PDFNet.AddResourceSearchPath() function.");
				Console.WriteLine();
				return;
			}

			try
			{
				// Extract form fields as a JSON file
				DataExtractionModule.ExtractData(input_path + "formfields-scanned.pdf", output_path + "formfields-scanned.json", DataExtractionModule.DataExtractionEngine.e_form);

				// Extract form fields as a JSON string
				string json = DataExtractionModule.ExtractData(input_path + "formfields.pdf", DataExtractionModule.DataExtractionEngine.e_form);
				System.IO.File.WriteAllText(output_path + "formfields.json", json);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
			}
		}


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			// The first step in every application using PDFNet is to initialize the 
			// library and set the path to common PDF resources. The library is usually 
			// initialized only once, but calling Initialize() multiple times is also fine.
			PDFNet.Initialize(PDFTronLicense.Key);
			PDFNet.AddResourceSearchPath("../../../Lib/");

			TestTabularData();
			TestDocumentStructure();
			TestFormFields();

			PDFNet.Terminate();
		}
	}
}
