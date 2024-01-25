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

namespace ConvertTestCS
{
	/// <summary>
	// The following sample illustrates how to use the PDF::Convert utility class to convert 
	// documents and files to PDF, XPS, or SVG, or EMF. The sample also shows how to convert MS Office files 
	// using our built in conversion.
	//
	// Certain file formats such as XPS, EMF, PDF, and raster image formats can be directly 
	// converted to PDF or XPS. 
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
		static Class1() { }

		// Relative path to the folder containing test files.
		const string inputPath = "../../TestFiles/";
		const string outputPath = "../../TestFiles/Output/";

		static bool ConvertSpecificFormats()
		{
			//////////////////////////////////////////////////////////////////////////
			bool err = false;
			try
			{
				using (PDFDoc pdfdoc = new PDFDoc())
				{
					Console.WriteLine("Converting from XPS");

					pdftron.PDF.Convert.FromXps(pdfdoc, inputPath + "simple-xps.xps");
					pdfdoc.Save(outputPath + "xps2pdf v2.pdf", SDFDoc.SaveOptions.e_remove_unused);
					Console.WriteLine("Saved xps2pdf v2.pdf");
				}
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
					// Add a dictionary
					ObjSet set = new ObjSet();
					Obj options = set.CreateDict();

					// Put options
					options.PutNumber("FontSize", 15);
					options.PutBool("UseSourceCodeFormatting", true);
					options.PutNumber("PageWidth", 12);
					options.PutNumber("PageHeight", 6);

					// Convert from .txt file
					Console.WriteLine("Converting from txt");
					pdftron.PDF.Convert.FromText(pdfdoc, inputPath + "simple-text.txt", options);
					pdfdoc.Save(outputPath + "simple-text.pdf", SDFDoc.SaveOptions.e_remove_unused);
					Console.WriteLine("Saved simple-text.pdf");
				}
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
				err = true;
			}

			//////////////////////////////////////////////////////////////////////////
			try
			{
				using (PDFDoc pdfdoc = new PDFDoc(inputPath + "newsletter.pdf"))
				{
					// Convert PDF document to SVG
					Console.WriteLine("Converting pdfdoc to SVG");
					pdftron.PDF.Convert.ToSvg(pdfdoc, outputPath + "pdf2svg v2.svg");
					Console.WriteLine("Saved pdf2svg v2.svg");
				}
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
				err = true;
			}

			//////////////////////////////////////////////////////////////////////////
			try
			{
				// Convert PNG image to XPS
				Console.WriteLine("Converting PNG to XPS");
				pdftron.PDF.Convert.ToXps(inputPath + "butterfly.png", outputPath + "butterfly.xps");
				Console.WriteLine("Saved butterfly.xps");
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
				err = true;
			}

			
			//////////////////////////////////////////////////////////////////////////
			try
			{
				// Convert PDF document to XPS
				Console.WriteLine("Converting PDF to XPS");
				pdftron.PDF.Convert.ToXps(inputPath + "newsletter.pdf", outputPath + "newsletter.xps");
				Console.WriteLine("Saved newsletter.xps");
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
				err = true;
			}

			//////////////////////////////////////////////////////////////////////////
			try
			{
				// Convert PDF document to HTML
				Console.WriteLine("Converting PDF to HTML");
				pdftron.PDF.Convert.ToHtml(inputPath + "newsletter.pdf", outputPath + "newsletter");
				Console.WriteLine("Saved newsletter as HTML");
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
				err = true;
			}

			//////////////////////////////////////////////////////////////////////////
			try
			{
				// Convert PDF document to EPUB
				Console.WriteLine("Converting PDF to EPUB");
				pdftron.PDF.Convert.ToEpub(inputPath + "newsletter.pdf", outputPath + "newsletter.epub");
				Console.WriteLine("Saved newsletter.epub");
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
				err = true;
			}

			//////////////////////////////////////////////////////////////////////////
			try
			{
				// Convert PDF document to multipage TIFF
				Console.WriteLine("Converting PDF to multipage TIFF");
				pdftron.PDF.Convert.TiffOutputOptions tiff_options = new pdftron.PDF.Convert.TiffOutputOptions();
				tiff_options.SetDPI(200);
				tiff_options.SetDither(true);
				tiff_options.SetMono(true);
				pdftron.PDF.Convert.ToTiff(inputPath + "newsletter.pdf", outputPath + "newsletter.tiff", tiff_options);
				Console.WriteLine("Saved newsletter.tiff");
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
					// Convert SVG file to PDF
					Console.WriteLine("Converting SVG to PDF");

					pdftron.PDF.Convert.FromSVG(pdfdoc, inputPath + "tiger.svg", null);
					pdfdoc.Save(outputPath + "svg2pdf.pdf", SDFDoc.SaveOptions.e_remove_unused);

					Console.WriteLine("Saved svg2pdf.pdf");
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
			testfiles.Add(new ConvertTestCS.Testfile("simple-word_2007.docx", "docx2pdf.pdf"));
			testfiles.Add(new ConvertTestCS.Testfile("simple-powerpoint_2007.pptx", "pptx2pdf.pdf"));
			testfiles.Add(new ConvertTestCS.Testfile("simple-excel_2007.xlsx", "xlsx2pdf.pdf"));
			testfiles.Add(new ConvertTestCS.Testfile("simple-text.txt", "txt2pdf.pdf"));
			testfiles.Add(new ConvertTestCS.Testfile("butterfly.png", "png2pdf.pdf"));
			testfiles.Add(new ConvertTestCS.Testfile("simple-xps.xps", "xps2pdf.pdf"));
			
			bool err = false;

			foreach (Testfile file in testfiles)
			{
				try
				{
					using (pdftron.PDF.PDFDoc pdfdoc = new PDFDoc())
					{
						pdftron.PDF.Convert.Printer.SetMode(pdftron.PDF.Convert.Printer.Mode.e_prefer_builtin_converter);
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

			PDFNet.Terminate();
			Console.WriteLine("Done.");
		}
	}
}
