//
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
//

using System;
using pdftron;
using pdftron.Common;
using pdftron.Filters;
using pdftron.SDF;
using pdftron.PDF;

namespace EncTestCS
{
	// A custom security handler used to obtain document password dynamically via user feedback. 
	/// <summary>
	//---------------------------------------------------------------------------------------
	// This sample shows encryption support in PDFNet. The sample reads an encrypted document and 
	// sets a new SecurityHandler. The sample also illustrates how password protection can 
	// be removed from an existing PDF document.
	//---------------------------------------------------------------------------------------
	/// </summary>
	class Class1
	{
		private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
		static Class1() {}
		
		static void Main(string[] args)
		{
			PDFNet.Initialize(PDFTronLicense.Key);

			// Relative path to the folder containing test files.
			string input_path =  "../../TestFiles/";
			string output_path = "../../TestFiles/Output/";

			// Example 1: Securing a document with password protection and adjusting permissions 
			// on the document.
			try
			{
				// Open the test file
				Console.WriteLine("-------------------------------------------------");
				Console.WriteLine("Securing an existing document...");
				using (PDFDoc doc = new PDFDoc(input_path + "fish.pdf"))
				{

					if (!doc.InitSecurityHandler()) 
					{
						Console.WriteLine("Document authentication error...");
						return;
					}
					
					// Perform some operation on the document. In this case we use low level SDF API
					// to replace the content stream of the first page with contents of file 'my_stream.txt'
					if (true)  // Optional
					{
						Console.WriteLine("Replacing the content stream, use flate compression...");

						// Get the first page dictionary using the following path: trailer/Root/Pages/Kids/0
						Obj page_dict = doc.GetTrailer().Get("Root").Value().
							Get("Pages").Value().Get("Kids").Value().GetAt(0);

						// Embed a custom stream (file mystream.txt) using Flate compression.
						MappedFile embed_file = new MappedFile(input_path + "my_stream.txt");
						FilterReader mystm = new FilterReader(embed_file);
						page_dict.Put("Contents", doc.CreateIndirectStream(mystm));
						embed_file.Close();
					}
				
					// Apply a new security handler with given security settings. 
					// In order to open saved PDF you will need a user password 'test'.
					SecurityHandler new_handler = new SecurityHandler();
					// Set a new password required to open a document
					string my_password = "test";				
					new_handler.ChangeUserPassword(my_password);

					// Set Permissions
					new_handler.SetPermission (SecurityHandler.Permission.e_print, true);
					new_handler.SetPermission (SecurityHandler.Permission.e_extract_content, false);

					// Note: document takes the ownership of new_handler.
					doc.SetSecurityHandler(new_handler);

					// Save the changes.
					Console.WriteLine("Saving modified file...");
					doc.Save(output_path + "secured.pdf", 0);
				}

				Console.WriteLine("Done. Result saved in secured.pdf");
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
			}

			// Example 2: Reading password protected document without user feedback.
			try
			{
				// In this sample case we will open an encrypted document that 
				// requires a user password in order to access the content.
				Console.WriteLine("-------------------------------------------------");
				Console.WriteLine("Open the password protected document from the first example...");
				using (PDFDoc doc = new PDFDoc(output_path + "secured.pdf"))	// Open the encrypted document that we saved in the first example. 
				{

					Console.WriteLine("Initializing security handler without any user interaction...");
					
					// At this point MySecurityHandler callbacks will be invoked. 
					// MySecurityHandler.GetAuthorizationData() should collect the password and 
					// AuthorizeFailed() is called if user repeatedly enters a wrong password.
					if (!doc.InitStdSecurityHandler("test")) 
					{
						Console.WriteLine("Document authentication error...");
						Console.WriteLine("The password is not valid.");
						return;
					}
					else 
					{
						Console.WriteLine("The password is correct! Document can now be used for reading and editing");

						// Remove the password security and save the changes to a new file.
						doc.RemoveSecurity();
						doc.Save(output_path + "secured_nomore1.pdf", 0);
						Console.WriteLine("Done. Result saved in secured_nomore1.pdf");
					}

				}
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
			}

			// Example 3:
			// Encrypt/Decrypt a PDF using PDFTron custom security handler
			try
			{
				Console.WriteLine("-------------------------------------------------");
				Console.WriteLine("Encrypt a document using PDFTron Custom Security handler with a custom id and password...");
				PDFDoc doc = new PDFDoc(input_path + "BusinessCardTemplate.pdf");

				// Create PDFTron custom security handler with a custom id. Replace this with your own integer
				int custom_id = 123456789;
				PDFTronCustomSecurityHandler custom_handler = new PDFTronCustomSecurityHandler(custom_id);

				// Add a password to the custom security handler
				String pass = "test";
				custom_handler.ChangeUserPassword(pass);

				// Save the encrypted document
				doc.SetSecurityHandler(custom_handler);
				doc.Save(output_path + "BusinessCardTemplate_enc.pdf", 0);

				Console.WriteLine("Decrypt the PDFTron custom security encrypted document above...");
				// Register the PDFTron Custom Security handler with the same custom id used in encryption
				PDFNet.AddPDFTronCustomHandler(custom_id);

				PDFDoc doc_enc = new PDFDoc(output_path + "BusinessCardTemplate_enc.pdf");
				doc_enc.InitStdSecurityHandler(pass);
				doc_enc.RemoveSecurity();
				// Save the decrypted document
				doc_enc.Save(output_path + "BusinessCardTemplate_enc_dec.pdf", 0);
				Console.WriteLine("Done. Result saved in BusinessCardTemplate_enc_dec.pdf");
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
			}

			Console.WriteLine("Tests completed.");

		}
	}
}
