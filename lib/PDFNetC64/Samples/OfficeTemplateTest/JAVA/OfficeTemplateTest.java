//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import com.pdftron.common.PDFNetException;
import com.pdftron.pdf.Convert;
import com.pdftron.pdf.TemplateDocument;
import com.pdftron.pdf.PDFDoc;
import com.pdftron.pdf.PDFNet;
import com.pdftron.pdf.OfficeToPDFOptions;
import com.pdftron.sdf.SDFDoc;

//---------------------------------------------------------------------------------------
// The following sample illustrates how to use the PDF::Convert utility class 
// to convert MS Office files to PDF and replace templated tags present in the document
// with content supplied via json
//
// For a detailed specification of the template format and supported features,
// see: https://docs.apryse.com/documentation/core/guides/generate-via-template/data-model/
//
// This conversion is performed entirely within the PDFNet and has *no* external or
// system dependencies dependencies -- Conversion results will be the same whether
// on Windows, Linux or Android.
//
// Please contact us if you have any questions. 
//---------------------------------------------------------------------------------------
public class OfficeTemplateTest {

    static String input_path = "../../TestFiles/";
    static String output_path = "../../TestFiles/Output/";
    static String input_filename = "SYH_Letter.docx";
    static String output_filename = "SYH_Letter.pdf";

    public static void main(String[] args) {
        PDFNet.initialize(PDFTronLicense.Key());
        PDFNet.setResourcesPath("../../../Resources");

        try {
            String json = new StringBuilder()
                .append("{\"dest_given_name\": \"Janice N.\", \"dest_street_address\": \"187 Duizelstraat\", \"dest_surname\": \"Symonds\", \"dest_title\": \"Ms.\", \"land_location\": \"225 Parc St., Rochelle, QC \",")
                .append("\"lease_problem\": \"According to the city records, the lease was initiated in September 2010 and never terminated\", \"logo\": {\"image_url\": \"" + input_path + "logo_red.png\", \"width\" : 64, \"height\" : 64},")
                .append("\"sender_name\": \"Arnold Smith\"}").toString();

            // Create a TemplateDocument object from an input office file.
            TemplateDocument template_doc = Convert.createOfficeTemplate(input_path + input_filename, null);

            // Fill the template with data from a JSON string, producing a PDF document.
            try (PDFDoc pdfdoc = template_doc.fillTemplateJson(json)) {

                // Save the PDF to a file.
                pdfdoc.save(output_path + output_filename, SDFDoc.SaveMode.INCREMENTAL, null);
            }
        }
        catch (PDFNetException e) {
            e.printStackTrace();
            System.out.println(e);
        }

        // And we're done!
        System.out.println("Done conversion " + output_path + output_filename);

        PDFNet.terminate();
    }

}
