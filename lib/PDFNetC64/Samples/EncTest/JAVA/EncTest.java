//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;

import com.pdftron.common.PDFNetException;
import com.pdftron.filters.FilterReader;
import com.pdftron.filters.FlateEncode;
import com.pdftron.filters.MappedFile;
import com.pdftron.pdf.*;
import com.pdftron.sdf.*;


//---------------------------------------------------------------------------------------
// This sample shows encryption support in PDFNet. The sample reads an encrypted document and 
// sets a new SecurityHandler. The sample also illustrates how password protection can 
// be removed from an existing PDF document.
//---------------------------------------------------------------------------------------
public class EncTest {
    public static void main(String[] args) {
        PDFNet.initialize(PDFTronLicense.Key());

        // Relative path to the folder containing test files.
        String input_path = "../../TestFiles/";
        String output_path = "../../TestFiles/Output/";

        // Example 1:
        // secure a document with password protection and
        // adjust permissions

        // Open the test file
        System.out.println("Securing an existing document ...");
        try (PDFDoc doc = new PDFDoc((input_path + "fish.pdf"))) {
            doc.initSecurityHandler();

            // Perform some operation on the document. In this case we use low level SDF API
            // to replace the content stream of the first page with contents of file 'my_stream.txt'
            if (true)  // Optional
            {
                System.out.println("Replacing the content stream, use flate compression...");

                // Get the page dictionary using the following path: trailer/Root/Pages/Kids/0
                Obj page_dict = doc.getTrailer().get("Root").value()
                        .get("Pages").value()
                        .get("Kids").value()
                        .getAt(0);

                // Embed a custom stream (file mystream.txt) using Flate compression.
                MappedFile embed_file = new MappedFile((input_path + "my_stream.txt"));
                FilterReader mystm = new FilterReader(embed_file);
                page_dict.put("Contents",
                        doc.createIndirectStream(mystm,
                                new FlateEncode(null)));
            }

            //encrypt the document

            // Apply a new security handler with given security settings.
            // In order to open saved PDF you will need a user password 'test'.
            SecurityHandler new_handler = new SecurityHandler();

            // Set a new password required to open a document
            String user_password = "test";
            new_handler.changeUserPassword(user_password);

            // Set Permissions
            new_handler.setPermission(SecurityHandler.e_print, true);
            new_handler.setPermission(SecurityHandler.e_extract_content, false);

            // Note: document takes the ownership of new_handler.
            doc.setSecurityHandler(new_handler);

            // Save the changes.
            System.out.println("Saving modified file...");
            doc.save((output_path + "secured.pdf"), SDFDoc.SaveMode.NO_FLAGS, null);
        } catch (PDFNetException e) {
            e.printStackTrace();
        }

        // Example 2:
        // Opens the encrypted document and removes all of
        // its security.
        try (PDFDoc doc = new PDFDoc((output_path + "secured.pdf"))) {
            //If the document is encrypted prompt for the password
            if (!doc.initSecurityHandler()) {
                boolean success = false;
                System.out.println("The password is: test");
                for (int count = 0; count < 3; count++) {
                    BufferedReader r = new BufferedReader(new InputStreamReader(System.in));
                    System.out.println("A password required to open the document.");
                    System.out.print("Please enter the password: ");
                    String password = r.readLine();
                    if (doc.initStdSecurityHandler(password)) {
                        success = true;
                        System.out.println("The password is correct.");
                        break;
                    } else if (count < 3) {
                        System.out.println("The password is incorrect, please try again");
                    }
                }
                if (!success) {
                    System.out.println("Document authentication error....");
                    PDFNet.terminate();
                }
            }

            //remove all security on the document
            doc.removeSecurity();
            doc.save(output_path + "not_secured.pdf", SDFDoc.SaveMode.NO_FLAGS, null);
        } catch (Exception e) {
            e.printStackTrace();
        }

        // Example 3:
        // Encrypt/Decrypt a PDF using PDFTron custom security handler
        System.out.println("-------------------------------------------------");
        System.out.println("Encrypt a document using PDFTron Custom Security handler with a custom id and password...");
        try (PDFDoc doc = new PDFDoc(input_path + "BusinessCardTemplate.pdf"))
        {   
            // Create PDFTron custom security handler with a custom id. Replace this with your own integer
            int custom_id = 123456789;
            PDFTronCustomSecurityHandler custom_handler = new PDFTronCustomSecurityHandler(custom_id);

            // Add a password to the custom security handler
            String pass = "test";
            custom_handler.changeUserPassword(pass);

            // Save the encrypted document
            doc.setSecurityHandler(custom_handler);
            doc.save(output_path + "BusinessCardTemplate_enc.pdf", SDFDoc.SaveMode.NO_FLAGS, null);
                    
            System.out.println("Decrypt the PDFTron custom security encrypted document above...");
            // Register the PDFTron Custom Security handler with the same custom id used in encryption
            PDFNet.addPDFTronCustomHandler(custom_id);

            PDFDoc doc_enc = new PDFDoc(output_path + "BusinessCardTemplate_enc.pdf");
            doc_enc.initStdSecurityHandler(pass);
            doc_enc.removeSecurity();
            // Save the decrypted document
            doc_enc.save(output_path + "BusinessCardTemplate_enc_dec.pdf", SDFDoc.SaveMode.NO_FLAGS, null);
            System.out.println("Done. Result saved in BusinessCardTemplate_enc_dec.pdf");
        } catch (Exception e) {
            e.printStackTrace();
        }
        System.out.println("-------------------------------------------------");
        System.out.println("Tests completed.");
        PDFNet.terminate();
    }
}
