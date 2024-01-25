//
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
//

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using pdftron;
using pdftron.Common;
using pdftron.Filters;
using pdftron.SDF;
using pdftron.PDF;

namespace ConvertPrintTestCS
{
	/// <summary>
	// The following sample illustrates how to convert to PDF with virtual printer on Windows.
	// It supports several input formats like docx, xlsx, rtf, txt, html, pub, emf, etc. For more details, visit 
	// https://docs.apryse.com/documentation/windows/guides/features/conversion/convert-other/
	// 
	// To check if ToPDF (or ToXPS) require that PDFNet printer is installed use Convert::RequiresPrinter(filename). 
	// The installing application must be run as administrator. The manifest for this sample 
	// specifies appropriate the UAC elevation.
	//
	// Note: the PDFNet printer is a virtual XPS printer supported on Vista SP1 and Windows 7, or .NET Framework
	// 3.x or higher. For older versions of .NET Framework running on Windows XP or Vista SP0 you need to install 
	// the XPS Essentials Pack (or equivalent redistributables). 
	//
	// Also note that conversion under ASP.NET can be tricky to configure. Please see the following document for advice: 
	// http://www.pdftron.com/pdfnet/faq_files/Converting_Documents_in_Windows_Service_or_ASP.NET_Application_using_PDFNet.pdf
	/// </summary>
	class Testfile
	{
		public string inputFile, outputFile;
		public Testfile(string inFile, string outFile)
		{
			inputFile = inFile;
			outputFile = outFile;
		}
	};

	class Class1
	{
		private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
		static Class1() {}

		// Relative path to the folder containing test files.
		const string inputPath = "../../TestFiles/";
		const string outputPath = "../../TestFiles/Output/";

		static bool ConvertSpecificFormats()
		{
			//////////////////////////////////////////////////////////////////////////
			bool err = false;
			try
			{
				// Convert MSWord document to XPS
				Console.WriteLine("Converting DOCX to XPS");
				pdftron.PDF.Convert.ToXps(inputPath + "simple-word_2007.docx", outputPath + "simple-word_2007.xps");
				Console.WriteLine("Saved simple-word_2007.xps");
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
				err = true;
			}


			//////////////////////////////////////////////////////////////////////////
			try
			{
				using (PDFDoc pdfdoc = new PDFDoc())
				{
					Console.WriteLine("Converting from EMF");
					pdftron.PDF.Convert.FromEmf(pdfdoc, inputPath + "simple-emf.emf");
					pdfdoc.Save(outputPath + "emf2pdf v2.pdf", SDFDoc.SaveOptions.e_remove_unused);
					Console.WriteLine("Saved emf2pdf v2.pdf");
				}
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
				err = true;
			}

			return err;
		}

		static Boolean ConvertToPdfFromFile()
		{
			System.Collections.ArrayList testfiles = new System.Collections.ArrayList();
			testfiles.Add(new ConvertPrintTestCS.Testfile("simple-powerpoint_2007.pptx", "pptx2pdf.pdf"));
			testfiles.Add(new ConvertPrintTestCS.Testfile("simple-word_2007.docx", "docx2pdf.pdf"));
			testfiles.Add(new ConvertPrintTestCS.Testfile("simple-excel_2007.xlsx", "xlsx2pdf.pdf"));
			testfiles.Add(new ConvertPrintTestCS.Testfile("simple-publisher.pub", "pub2pdf.pdf"));
			testfiles.Add(new ConvertPrintTestCS.Testfile("simple-text.txt", "txt2pdf.pdf"));
			testfiles.Add(new ConvertPrintTestCS.Testfile("simple-rtf.rtf", "rtf2pdf.pdf"));
			testfiles.Add(new ConvertPrintTestCS.Testfile("simple-emf.emf", "emf2pdf.pdf"));
			testfiles.Add(new ConvertPrintTestCS.Testfile("simple-webpage.mht", "mht2pdf.pdf"));
			testfiles.Add(new ConvertPrintTestCS.Testfile("simple-webpage.html", "html2pdf.pdf"));

			bool err = false; 
			try
			{
				if (pdftron.PDF.Convert.Printer.IsInstalled("PDFTron PDFNet"))
				{
					pdftron.PDF.Convert.Printer.SetPrinterName("PDFTron PDFNet");
				}
				else if (!pdftron.PDF.Convert.Printer.IsInstalled())
				{
					try
					{
						Console.WriteLine("Installing printer (requires Windows platform and administrator)");
						pdftron.PDF.Convert.Printer.Install();
						Console.WriteLine("Installed printer " + pdftron.PDF.Convert.Printer.GetPrinterName());
						// the function ConvertToXpsFromFile may require the printer so leave it installed
						// uninstallPrinterWhenDone = true;
					}
					catch (PDFNetException e)
					{
						Console.WriteLine("ERROR: Unable to install printer.");
						Console.WriteLine(e.Message);
						err = true;
					}
					catch
					{
						Console.WriteLine("ERROR: Unable to install printer. Make sure that the package's bitness matches your operating system's bitness and that you are running with administrator privileges.");
					}
				}
			}
			catch (PDFNetException e)
			{
				Console.WriteLine("ERROR: Unable to install printer.");
				Console.WriteLine(e.Message);
				err = true;
			}
			
			foreach (Testfile file in testfiles)
			{
				try
				{
					using (pdftron.PDF.PDFDoc pdfdoc = new PDFDoc())
					{

						if (pdftron.PDF.Convert.RequiresPrinter(inputPath + file.inputFile))
						{
							Console.WriteLine("Using PDFNet printer to convert file " + file.inputFile);
						}
						pdftron.PDF.Convert.ToPdf(pdfdoc, inputPath + file.inputFile);
						pdfdoc.Save(outputPath + file.outputFile, SDFDoc.SaveOptions.e_linearized);
						Console.WriteLine("Converted file: " + file.inputFile);
						Console.WriteLine("to: " + file.outputFile);
					}
				}
				catch (PDFNetException e)
				{
					Console.WriteLine("ERROR: on input file " + file.inputFile);
					Console.WriteLine(e.Message);
					err = true;
				}
			}

			return err;
		}


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32NT)
			{
				PDFNet.Initialize(PDFTronLicense.Key);
				bool err = false;

				err = ConvertToPdfFromFile();
				if (err)
				{
					Console.WriteLine("ConvertFile failed");
				}
				else
				{
					Console.WriteLine("ConvertFile succeeded");
				}

				err = ConvertSpecificFormats();
				if (err)
				{
					Console.WriteLine("ConvertSpecificFormats failed");
				}
				else
				{
					Console.WriteLine("ConvertSpecificFormats succeeded");
				}


				if (pdftron.PDF.Convert.Printer.IsInstalled())
				{
					try
					{
						Console.WriteLine("Uninstalling printer (requires Windows platform and administrator)");
						pdftron.PDF.Convert.Printer.Uninstall();
						Console.WriteLine("Uninstalled Printer " + pdftron.PDF.Convert.Printer.GetPrinterName());
					}
					catch
					{
						Console.WriteLine("Unable to uninstall printer");
					}
				}
			
				PDFNet.Terminate();
				Console.WriteLine("Done.");
			}
			else
			{
				Console.WriteLine("ConvertPrintTest only available on Windows");
			}
		}
	}
}
