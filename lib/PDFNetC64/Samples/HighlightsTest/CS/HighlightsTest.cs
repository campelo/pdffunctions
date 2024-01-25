//
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
//

using System;
using System.Collections.Generic;
using pdftron;
using pdftron.Common;
using pdftron.Filters;
using pdftron.SDF;
using pdftron.PDF;


namespace HighlightsTestCS
{
	// This sample illustrates the basic text highlight capabilities of PDFNet.
	// It simulates a full-text search engine that finds all occurrences of the word 'Federal'.
	// It then highlights those words on the page.
	//
	// Note: The TextSearch class is the preferred solution for searching text within a single
	// PDF file. TextExtractor provides search highlighting capabilities where a large number
	// of documents are indexed using a 3rd party search engine.

	class Class1
	{		
		private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
		static Class1() {}
		
		static void Main(string[] args)
		{
			PDFNet.Initialize(PDFTronLicense.Key);

			// Relative path to the folder containing test files.
			string input_path = "../../TestFiles/";
			string output_path = "../../TestFiles/Output/";

			// Sample code showing how to use high-level text highlight APIs.
			try
			{
				using (PDFDoc doc = new PDFDoc(input_path + "paragraphs_and_tables.pdf"))
				{
					doc.InitSecurityHandler();

					Page page = doc.GetPage(1);
					if (page == null)
					{
						Console.WriteLine("Page not found.");
						PDFNet.Terminate();
						return;
					}

					using (TextExtractor txt = new TextExtractor())
					{
						txt.Begin(page);  // read the page

						// Do not dehyphenate; that would interfere with character offsets
						bool dehyphen = false;
						// Retrieve the page text
						string page_text = txt.GetAsText(dehyphen);

						// Simulating a full-text search engine that finds all occurrences of the word 'Federal'.
						// In a real application, plug in your own search engine here.
						string search_text = "Federal";
						List<TextExtractor.CharRange> char_ranges_list = new List<TextExtractor.CharRange>();
						int ofs = page_text.IndexOf(search_text);
						while (ofs >= 0)
						{
							char_ranges_list.Add(new TextExtractor.CharRange(ofs, search_text.Length)); // character offset + length
							ofs = page_text.IndexOf(search_text, ofs + 1);
						}

						// Convert List to array, as required by TextExtractor.GetHighlights()
						TextExtractor.CharRange[] char_ranges = char_ranges_list.ToArray();

						// Retrieve Highlights object and apply annotations to the page
						Highlights hlts = txt.GetHighlights(char_ranges);
						hlts.Begin(doc);
						while (hlts.HasNext())
						{
							double[] quads = hlts.GetCurrentQuads();
							int quad_count = quads.Length / 8;
							for (int i = 0; i < quad_count; ++i)
							{
								// assume each quad is an axis-aligned rectangle
								int offset = 8 * i;
								double x1 = Math.Min(Math.Min(Math.Min(quads[offset + 0], quads[offset + 2]), quads[offset + 4]), quads[offset + 6]);
								double x2 = Math.Max(Math.Max(Math.Max(quads[offset + 0], quads[offset + 2]), quads[offset + 4]), quads[offset + 6]);
								double y1 = Math.Min(Math.Min(Math.Min(quads[offset + 1], quads[offset + 3]), quads[offset + 5]), quads[offset + 7]);
								double y2 = Math.Max(Math.Max(Math.Max(quads[offset + 1], quads[offset + 3]), quads[offset + 5]), quads[offset + 7]);
								pdftron.PDF.Annots.Highlight highlight = pdftron.PDF.Annots.Highlight.Create(doc, new Rect(x1, y1, x2, y2));
								highlight.RefreshAppearance();
								page.AnnotPushBack(highlight);

								Console.WriteLine("[{0:N2}, {1:N2}, {2:N2}, {3:N2}]", x1, y1, x2, y2);
							}
							hlts.Next();
						}

						// Output highlighted PDF doc
						doc.Save(output_path + "search_highlights.pdf", SDFDoc.SaveOptions.e_linearized);

					}
				}
			}
			catch (PDFNetException e)
			{
				Console.WriteLine(e.Message);
			}

			PDFNet.Terminate();
		}
	}
}
