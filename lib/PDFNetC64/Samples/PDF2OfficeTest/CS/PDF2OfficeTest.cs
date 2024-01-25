//
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
//

using System;
using pdftron;
using pdftron.Common;
using pdftron.PDF;

namespace PDF2OfficeTestCS
{
	/// <summary>
	// The following sample illustrates how to use the PDF::Convert utility class to convert 
	// documents and files to Office.
	//
	// The Structured Output module is an optional PDFNet Add-on that can be used to convert PDF
	// and other documents into Word, Excel, PowerPoint and HTML format.
	//
	// The Apryse SDK Structured Output add-on module can be downloaded from
	// https://docs.apryse.com/documentation/core/info/modules/
	//
	// Please contact us if you have any questions.	
	/// </summary>

	class Class1
	{
		private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();

		static Class1() {}

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

			PDFNet.AddResourceSearchPath("../../../Lib/");

			if (!StructuredOutputModule.IsModuleAvailable())
			{
				Console.WriteLine();
				Console.WriteLine("Unable to run the sample: Apryse SDK Structured Output module not available.");
				Console.WriteLine("-----------------------------------------------------------------------------");
				Console.WriteLine("The Structured Output module is an optional add-on, available for download");
				Console.WriteLine("at https://docs.apryse.com/documentation/core/info/modules/. If you have already");
				Console.WriteLine("downloaded this module, ensure that the SDK is able to find the required files");
				Console.WriteLine("using the PDFNet::AddResourceSearchPath() function.");
				Console.WriteLine();
				return 0;
			}

			bool err = false;

			//////////////////////////////////////////////////////////////////////////
			// Word
			//////////////////////////////////////////////////////////////////////////

			try
			{
				// Convert PDF document to Word
				Console.WriteLine("Converting PDF to Word");

				string outputFile = outputPath + "paragraphs_and_tables.docx";

				pdftron.PDF.Convert.ToWord(inputPath + "paragraphs_and_tables.pdf", outputFile);

				Console.WriteLine("Result saved in " + outputFile);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine("Unable to convert PDF document to Word, error: " + e.Message);
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
				// Convert PDF document to Word with options
				Console.WriteLine("Converting PDF to Word with options");

				string outputFile = outputPath + "paragraphs_and_tables_first_page.docx";

				pdftron.PDF.Convert.WordOutputOptions wordOutputOptions = new pdftron.PDF.Convert.WordOutputOptions();

				// Convert only the first page
				wordOutputOptions.SetPages(1, 1);

				pdftron.PDF.Convert.ToWord(inputPath + "paragraphs_and_tables.pdf", outputFile, wordOutputOptions);

				Console.WriteLine("Result saved in " + outputFile);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine("Unable to convert PDF document to Word, error: " + e.Message);
				err = true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Unknown Exception, error: ");
				Console.WriteLine(e);
				err = true;
			}

			//////////////////////////////////////////////////////////////////////////
			// Excel
			//////////////////////////////////////////////////////////////////////////

			try
			{
				// Convert PDF document to Excel
				Console.WriteLine("Converting PDF to Excel");

				string outputFile = outputPath + "paragraphs_and_tables.xlsx";

				pdftron.PDF.Convert.ToExcel(inputPath + "paragraphs_and_tables.pdf", outputFile);

				Console.WriteLine("Result saved in " + outputFile);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine("Unable to convert PDF document to Excel, error: " + e.Message);
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
				// Convert PDF document to Excel with options
				Console.WriteLine("Converting PDF to Excel with options");

				string outputFile = outputPath + "paragraphs_and_tables_second_page.xlsx";

				pdftron.PDF.Convert.ExcelOutputOptions excelOutputOptions = new pdftron.PDF.Convert.ExcelOutputOptions();

				// Convert only the second page
				excelOutputOptions.SetPages(2, 2);

				pdftron.PDF.Convert.ToExcel(inputPath + "paragraphs_and_tables.pdf", outputFile, excelOutputOptions);

				Console.WriteLine("Result saved in " + outputFile);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine("Unable to convert PDF document to Excel, error: " + e.Message);
				err = true;
			}
			catch (Exception e)
			{
				Console.WriteLine("Unknown Exception, error: ");
				Console.WriteLine(e);
				err = true;
			}

			//////////////////////////////////////////////////////////////////////////
			// PowerPoint
			//////////////////////////////////////////////////////////////////////////

			try
			{
				// Convert PDF document to PowerPoint
				Console.WriteLine("Converting PDF to PowerPoint");

				string outputFile = outputPath + "paragraphs_and_tables.pptx";

				pdftron.PDF.Convert.ToPowerPoint(inputPath + "paragraphs_and_tables.pdf", outputFile);

				Console.WriteLine("Result saved in " + outputFile);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine("Unable to convert PDF document to PowerPoint, error: " + e.Message);
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
				// Convert PDF document to PowerPoint with options
				Console.WriteLine("Converting PDF to PowerPoint with options");

				string outputFile = outputPath + "paragraphs_and_tables_first_page.pptx";

				pdftron.PDF.Convert.PowerPointOutputOptions powerPointOutputOptions = new pdftron.PDF.Convert.PowerPointOutputOptions();

				// Convert only the first page
				powerPointOutputOptions.SetPages(1, 1);

				pdftron.PDF.Convert.ToPowerPoint(inputPath + "paragraphs_and_tables.pdf", outputFile, powerPointOutputOptions);

				Console.WriteLine("Result saved in " + outputFile);
			}
			catch (PDFNetException e)
			{
				Console.WriteLine("Unable to convert PDF document to PowerPoint, error: " + e.Message);
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
