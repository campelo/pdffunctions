//
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
//

using System;
using pdftron;
using pdftron.Common;
using pdftron.PDF;

namespace PDF2HtmlTestCS
{
	/// <summary>
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
	/// </summary>

	class Class1
	{
		private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();

		static Class1() { }

		// Relative path to the folder containing test files.
		const string inputPath = "../../TestFiles/";
		const string outputPath = "../../TestFiles/Output/";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			// The first step in every application using PDFNet is to initialize the 
			// library. The library is usually initialized only once, but calling 
			// Initialize() multiple times is also fine.
			PDFNet.Initialize(PDFTronLicense.Key);

			bool err = false;

			//////////////////////////////////////////////////////////////////////////

			try
			{
				// Convert PDF document to HTML with fixed positioning option turned on (default)
				Console.WriteLine("Converting PDF to HTML with fixed positioning option turned on (default)");

				string outputFile = outputPath + "paragraphs_and_tables_fixed_positioning";

				pdftron.PDF.Convert.ToHtml(inputPath + "paragraphs_and_tables.pdf", outputFile);

				Console.WriteLine("Result saved in " + outputFile);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine("Unable to convert PDF document to HTML, error: " + e.Message);
				err = true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Unknown Exception, error: ");
				Console.WriteLine(e);
				err = true;
			}

			//////////////////////////////////////////////////////////////////////////

			PDFNet.AddResourceSearchPath("../../../Lib/");

			if (!StructuredOutputModule.IsModuleAvailable())
			{
				Console.WriteLine();
				Console.WriteLine("Unable to run part of the sample: Apryse SDK Structured Output module not available.");
				Console.WriteLine("-------------------------------------------------------------------------------------");
				Console.WriteLine("The Structured Output module is an optional add-on, available for download");
				Console.WriteLine("at https://docs.apryse.com/documentation/core/info/modules/. If you have already");
				Console.WriteLine("downloaded this module, ensure that the SDK is able to find the required files");
				Console.WriteLine("using the PDFNet::AddResourceSearchPath() function.");
				Console.WriteLine();
				return 0;
			}

			//////////////////////////////////////////////////////////////////////////

			try
			{
				// Convert PDF document to HTML with reflow full option turned on (1)
				Console.WriteLine("Converting PDF to HTML with reflow full option turned on (1)");

				string outputFile = outputPath + "paragraphs_and_tables_reflow_full.html";

				pdftron.PDF.Convert.HTMLOutputOptions htmlOutputOptions = new pdftron.PDF.Convert.HTMLOutputOptions();

				// Set e_reflow_full content reflow setting
				htmlOutputOptions.SetContentReflowSetting(pdftron.PDF.Convert.HTMLOutputOptions.ContentReflowSetting.e_reflow_full);

				pdftron.PDF.Convert.ToHtml(inputPath + "paragraphs_and_tables.pdf", outputFile, htmlOutputOptions);

				Console.WriteLine("Result saved in " + outputFile);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine("Unable to convert PDF document to HTML, error: " + e.Message);
				err = true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Unknown Exception, error: ");
				Console.WriteLine(e);
				err = true;
			}

			//////////////////////////////////////////////////////////////////////////

			try
			{
				// Convert PDF document to HTML with reflow full option turned on (only converting the first page) (2)
				Console.WriteLine("Converting PDF to HTML with reflow full option turned on (only converting the first page) (2)");

				string outputFile = outputPath + "paragraphs_and_tables_reflow_full_first_page.html";

				pdftron.PDF.Convert.HTMLOutputOptions htmlOutputOptions = new pdftron.PDF.Convert.HTMLOutputOptions();

				// Set e_reflow_full content reflow setting
				htmlOutputOptions.SetContentReflowSetting(pdftron.PDF.Convert.HTMLOutputOptions.ContentReflowSetting.e_reflow_full);

				// Convert only the first page
				htmlOutputOptions.SetPages(1, 1);

				pdftron.PDF.Convert.ToHtml(inputPath + "paragraphs_and_tables.pdf", outputFile, htmlOutputOptions);

				Console.WriteLine("Result saved in " + outputFile);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine("Unable to convert PDF document to HTML, error: " + e.Message);
				err = true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Unknown Exception, error: ");
				Console.WriteLine(e);
				err = true;
			}

			//////////////////////////////////////////////////////////////////////////

			PDFNet.Terminate();
			Console.WriteLine("Done.");
			return (err == false ? 0 : 1);
		}
	}
}
