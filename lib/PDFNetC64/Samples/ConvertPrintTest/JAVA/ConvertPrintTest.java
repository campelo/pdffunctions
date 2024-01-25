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
// The following sample illustrates how to convert to PDF with virtual printer on Windows.
// It supports several input formats like docx, xlsx, rtf, txt, html, pub, emf, etc. For more details, visit 
// https://docs.apryse.com/documentation/windows/guides/features/conversion/convert-other/
//
// To check if ToPDF (or ToXPS) require that PDFNet printer is installed use Convert::RequiresPrinter(filename). 
// The installing application must be run as administrator. The manifest for this sample 
// specifies appropriate the UAC elevation.
//
// Note: the PDFNet printer is a virtual XPS printer supported on Vista SP1 and Windows 7.
// For Windows XP SP2 or higher, or Vista SP0 you need to install the XPS Essentials Pack (or 
// equivalent redistributables). You can download the XPS Essentials Pack from:
//		http://www.microsoft.com/downloads/details.aspx?FamilyId=B8DCFFDD-E3A5-44CC-8021-7649FD37FFEE&displaylang=en
// Windows XP Sp2 will also need the Microsoft Core XML Services (MSXML) 6.0:
// 		http://www.microsoft.com/downloads/details.aspx?familyid=993C0BCF-3BCF-4009-BE21-27E85E1857B1&displaylang=en
//
// Note: Convert.fromEmf and Convert.toEmf will only work on Windows and require GDI+.
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

public class ConvertPrintTest 
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
			System.out.println("Converting from EMF");
			Convert.fromEmf(pdfdoc, inputPath + "simple-emf.emf");
			pdfdoc.save(outputPath + "emf2pdf v2.pdf", SDFDoc.SaveMode.REMOVE_UNUSED, null);
			System.out.println("Saved emf2pdf v2.pdf");
		}
		catch (PDFNetException e)
		{
			System.out.println(e);
			err = true;
		}
		

		//////////////////////////////////////////////////////////////////////////
		try
		{
			// Convert MSWord document to XPS
			System.out.println("Converting DOCX to XPS");
			Convert.toXps(inputPath + "simple-word_2007.docx", outputPath + "simple-word_2007.xps");
			System.out.println("Saved simple-word_2007.xps");
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
		testfiles.add(new Testfile("simple-publisher.pub", "pub2pdf.pdf"));
		testfiles.add(new Testfile("simple-text.txt", "txt2pdf.pdf"));
		testfiles.add(new Testfile("simple-rtf.rtf", "rtf2pdf.pdf"));
		testfiles.add(new Testfile("simple-emf.emf", "emf2pdf.pdf"));
		testfiles.add(new Testfile("simple-webpage.mht", "mht2pdf.pdf"));
		testfiles.add(new Testfile("simple-webpage.html", "html2pdf.pdf"));

		boolean err = false;
		try{
			if (ConvertPrinter.isInstalled("PDFTron PDFNet"))
			{
				ConvertPrinter.setPrinterName("PDFTron PDFNet");
			}
			else if (!ConvertPrinter.isInstalled())
			{
				try
				{
					System.out.println("Installing printer (requires Windows platform and administrator)");
					ConvertPrinter.install();
					System.out.println("Installed printer " + ConvertPrinter.getPrinterName());
					// the function ConvertToXpsFromFile may require the printer so leave it installed
					// uninstallPrinterWhenDone = true;
				}
				catch (PDFNetException e)
				{
					System.out.println("ERROR: Unable to install printer.");
					System.out.println(e);
					err = true;
				}
				catch (Exception e)
				{
					System.out.println("ERROR: Unable to install printer. Make sure that the package's bitness matches your operating system's bitness and that you are running with administrator privileges.");
				}
			}
		}
		catch (PDFNetException e)
		{
			System.out.println("ERROR: Unable to install printer.");
			System.out.println(e);
			err = true;
		}
		
		for (Testfile file : testfiles)
		{
			try (PDFDoc pdfdoc = new PDFDoc())
			{
				if (Convert.requiresPrinter(inputPath + file.inputFile))
				{
					System.out.println("Using PDFNet printer to convert file " + file.inputFile);
				}
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
		if (System.getProperty("os.name").startsWith("Windows")) {
			
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
			
			try
			{
				System.out.println("Uninstalling printer (requires Windows platform and administrator)");
				ConvertPrinter.uninstall();
				System.out.println("Uninstalled printer " + ConvertPrinter.getPrinterName());
			}
			catch (Exception e)
			{
				System.out.println("Unable to uninstall printer");
				err = true;
			}
			
			System.out.println("Done.");

			PDFNet.terminate();
		}
		else {
			System.out.println("ConvertPrintTest only available on Windows");
		}
	}

}
