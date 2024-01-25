//
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import com.pdftron.common.PDFNetException;
import com.pdftron.pdf.*;
import com.pdftron.sdf.Obj;
import com.pdftron.sdf.ObjSet;
import com.pdftron.sdf.SDFDoc;
import java.util.ArrayList;
//---------------------------------------------------------------------------------------
// The following sample illustrates how to use the PDF::Convert utility class to convert 
// documents and files to PDF, XPS, or SVG, or EMF. The sample also shows how to convert MS Office files 
// using our built in conversion.
//
// Certain file formats such as XPS, EMF, PDF, and raster image formats can be directly 
// converted to PDF or XPS. 
//
// Please contact us if you have any questions.	
//---------------------------------------------------------------------------------------
class Testfile
{
	public String inputFile, outputFile;
	public Testfile(String inFile, String outFile)
	{
		inputFile = inFile;
		outputFile = outFile;
	}
}

public class ConvertTest
{
	// Relative path to the folder containing test files.
	static String inputPath = "../../TestFiles/";
	static String outputPath = "../../TestFiles/Output/";

	static boolean ConvertSpecificFormats()
	{
		//////////////////////////////////////////////////////////////////////////
		boolean err = false;
		try (PDFDoc pdfdoc = new PDFDoc())
		{

			System.out.println("Converting from XPS");

			Convert.fromXps(pdfdoc, inputPath + "simple-xps.xps");
			pdfdoc.save(outputPath + "xps2pdf v2.pdf", SDFDoc.SaveMode.REMOVE_UNUSED, null);
			System.out.println("Saved xps2pdf v2.pdf");
		}
		catch (PDFNetException e)
		{
			System.out.println(e);
			err = true;
		}

		//////////////////////////////////////////////////////////////////////////
		try (PDFDoc pdfdoc = new PDFDoc())
		{
			// add a dictionary
			ObjSet set = new ObjSet();
			Obj options = set.createDict();

			// Put options
			options.putNumber("FontSize", 15);
			options.putBool("UseSourceCodeFormatting", true);
			options.putNumber("PageWidth", 12);
			options.putNumber("PageHeight", 6);

			// Convert from .txt file
			System.out.println("Converting from txt");
			Convert.fromText(pdfdoc, inputPath + "simple-text.txt", options);
			pdfdoc.save(outputPath + "simple-text.pdf", SDFDoc.SaveMode.REMOVE_UNUSED, null);
			System.out.println("Saved simple-text.pdf");
		}
		catch (PDFNetException e)
		{
			System.out.println(e);
			err = true;
		}
		
		//////////////////////////////////////////////////////////////////////////
		try (PDFDoc pdfdoc = new PDFDoc(inputPath + "newsletter.pdf"))
		{
			// Convert PDF document to SVG
			System.out.println("Converting pdfdoc to SVG");
			Convert.toSvg(pdfdoc, outputPath + "pdf2svg v2.svg");
			System.out.println("Saved pdf2svg v2.svg");
		}
		catch (PDFNetException e)
		{
			System.out.println(e);
			err = true;
		}

		//////////////////////////////////////////////////////////////////////////
		try
		{
			// Convert PNG image to XPS
			System.out.println("Converting PNG to XPS");
			Convert.toXps(inputPath + "butterfly.png", outputPath + "butterfly.xps");
			System.out.println("Saved butterfly.xps");
		}
		catch (PDFNetException e)
		{
			System.out.println(e);
			err = true;
		}

		//////////////////////////////////////////////////////////////////////////
		try
		{
			// Convert PDF document to XPS
			System.out.println("Converting PDF to XPS");
			Convert.toXps(inputPath + "newsletter.pdf", outputPath + "newsletter.xps");
			System.out.println("Saved newsletter.xps");
		}
		catch (PDFNetException e)
		{
			System.out.println(e);
			err = true;
		}

		//////////////////////////////////////////////////////////////////////////
		try
		{
			// Convert PDF document to HTML
			System.out.println("Converting PDF to HTML");
			Convert.toHtml(inputPath + "newsletter.pdf", outputPath + "newsletter");
			System.out.println("Saved newsletter as HTML");
		}
		catch (PDFNetException e)
		{
			System.out.println(e);
			err = true;
		}

		//////////////////////////////////////////////////////////////////////////
		try
		{
			// Convert PDF document to EPUB
			System.out.println("Converting PDF to EPUB");
			Convert.toEpub(inputPath + "newsletter.pdf", outputPath + "newsletter.epub");
			System.out.println("Saved newsletter.epub");
		}
		catch (PDFNetException e)
		{
			System.out.println(e);
			err = true;
		}

		//////////////////////////////////////////////////////////////////////////
		try
		{
			// Convert PDF document to multipage TIFF
			System.out.println("Converting PDF to multipage TIFF");
			Convert.TiffOutputOptions tiff_options = new Convert.TiffOutputOptions();
			tiff_options.setDPI(200);
			tiff_options.setDither(true);
			tiff_options.setMono(true);
			Convert.toTiff(inputPath + "newsletter.pdf", outputPath + "newsletter.tiff", tiff_options);
			System.out.println("Saved newsletter.tiff");
		}
		catch (PDFNetException e)
		{
			System.out.println(e);
			err = true;
		}

		//////////////////////////////////////////////////////////////////////////
		try (PDFDoc pdfdoc = new PDFDoc())
		{
			// Convert SVG file to PDF
			System.out.println("Converting SVG to PDF");
			Convert.fromSVG(pdfdoc, inputPath + "tiger.svg", null);
			pdfdoc.save(outputPath + "svg2pdf.pdf", SDFDoc.SaveMode.REMOVE_UNUSED, null);

			System.out.println("Saved svg2pdf.pdf");
		}
		catch (PDFNetException e)
		{
			System.out.println(e);
			err = true;
		}

		return err;
	}

	static boolean ConvertToPdfFromFile()
	{
		ArrayList<Testfile> testfiles = new ArrayList<Testfile>();
		testfiles.add(new Testfile("simple-word_2007.docx", "docx2pdf.pdf"));
		testfiles.add(new Testfile("simple-powerpoint_2007.pptx", "pptx2pdf.pdf"));
		testfiles.add(new Testfile("simple-excel_2007.xlsx", "xlsx2pdf.pdf"));
		testfiles.add(new Testfile("simple-text.txt", "txt2pdf.pdf"));
		testfiles.add(new Testfile("butterfly.png", "png2pdf.pdf"));
		testfiles.add(new Testfile("simple-xps.xps", "xps2pdf.pdf"));

		boolean err = false;
		
		for (Testfile file : testfiles)
		{
			try (PDFDoc pdfdoc = new PDFDoc())
			{
				//use built in converter
				ConvertPrinter.setMode(ConvertPrinter.e_convert_printer_prefer_builtin_converter);
				Convert.toPdf(pdfdoc, inputPath + file.inputFile);
				pdfdoc.save(outputPath + file.outputFile, SDFDoc.SaveMode.LINEARIZED, null);
				System.out.println("Converted file: " + file.inputFile);
				System.out.println("to: " + file.outputFile);
			}
			catch (PDFNetException e)
			{
				System.out.println("ERROR: on input file " + file.inputFile);
				System.out.println(e);
				err = true;
			}
		}

		return err;
	}

	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	public static void main(String[] args) 
	{
		PDFNet.initialize(PDFTronLicense.Key());
		boolean err = false;

		err = ConvertToPdfFromFile();
		if (err)
		{
			System.out.println("ConvertFile failed");
		}
		else
		{
			System.out.println("ConvertFile succeeded");
		}

		err = ConvertSpecificFormats();
		if (err)
		{
			System.out.println("ConvertSpecificFormats failed");
		}
		else
		{
			System.out.println("ConvertSpecificFormats succeeded");
		}

		System.out.println("Done.");

		PDFNet.terminate();
	}

}
