//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import com.pdftron.common.PDFNetException;
import com.pdftron.pdf.*;

public class ElementReaderTest {

    static void ProcessElements(ElementReader reader) throws PDFNetException {
        for (Element element = reader.next(); element != null; element = reader.next())    // Read page contents
        {
			switch (element.getType()) 
			{
				case Element.e_path:                 // Process path data...
				{
					PathData data = element.getPathData();
					byte[] operators = data.getOperators();
					double[] points = data.getPoints();
				}
				break;
				case Element.e_text:                // Process text strings...
				{
					String data = element.getTextString();
					System.out.println(data); 
				}
				break;
				case Element.e_form:                // Process form XObjects
				{
					reader.formBegin();
					ProcessElements(reader);
					reader.end();
				}
				break;
			}
        }
    }

    public static void main(String[] args) {
        PDFNet.initialize(PDFTronLicense.Key());

        // Relative path to the folder containing test files.
        String input_path = "../../TestFiles/";
        String output_path = "../../TestFiles/Output/";

        System.out.println("-------------------------------------------------");
        System.out.println("Sample 1 - Extract text data from all pages in the document.");
        System.out.println("Opening the input pdf...");

        try (PDFDoc doc = new PDFDoc(input_path + "newsletter.pdf"))    // Extract text data from all pages in the document
        {
            doc.initSecurityHandler();

            int pgnum = doc.getPageCount();

            PageIterator itr;
            ElementReader page_reader = new ElementReader();

            for (itr = doc.getPageIterator(); itr.hasNext(); )        //  Read every page
            {
                page_reader.begin(itr.next());
                ProcessElements(page_reader);
                page_reader.end();
            }
            System.out.println("Done.");
        } catch (Exception e) {
            System.out.println(e);
        }

        PDFNet.terminate();
    }
}
