//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

#include <PDF/PDFNet.h>
#include <PDF/PDFDoc.h>
#include <PDF/TextExtractor.h>

// This sample illustrates the basic text highlight capabilities of PDFNet.
// It simulates a full-text search engine that finds all occurrences of the word 'Federal'.
// It then highlights those words on the page.
//
// Note: The TextSearch class is the preferred solution for searching text within a single
// PDF file. TextExtractor provides search highlighting capabilities where a large number
// of documents are indexed using a 3rd party search engine.

#include <iostream>
#include "../../LicenseKey/CPP/LicenseKey.h"

using namespace std;

using namespace pdftron;
using namespace PDF;
using namespace SDF;
using namespace Common;
using namespace Filters; 

#undef max
#undef min
#include <algorithm>

int main(int argc, char *argv[])
{
	int ret = 0;
	PDFNet::Initialize(LicenseKey);

	// Relative path to the folder containing test files.
	string output_path = "../../TestFiles/Output/";
	string input_path = "../../TestFiles/paragraphs_and_tables.pdf";
	const char* filein = argc > 1 ? argv[1] : input_path.c_str();

	// Sample code showing how to use high-level text highlight APIs.
	try
	{
		PDFDoc doc(filein);
		doc.InitSecurityHandler();

		Page page = doc.GetPage(1);
		if (!page)
		{
			cout << "Page not found." << endl;
			PDFNet::Terminate();
			return 1;
		}

		TextExtractor txt;
		txt.Begin(page); // read the page

		// Do not dehyphenate; that would interfere with character offsets
		const bool dehyphen = false;
		// Retrieve the page text
		UString text;
		txt.GetAsText(text, dehyphen);
		const basic_string<Unicode> page_text(text.CStr(), text.CStr() + text.GetLength());

		// Simulating a full-text search engine that finds all occurrences of the word 'Federal'.
		// In a real application, plug in your own search engine here.
		const  Unicode search_text_arr[] = { 'F', 'e', 'd', 'e', 'r', 'a', 'l', 0 };
		const basic_string<Unicode> search_text(search_text_arr, search_text_arr + 7);
		vector<CharRange> char_ranges;
		size_t ofs = page_text.find(search_text);
		while (ofs != string::npos)
		{
			CharRange range;
			range.index = (int)ofs;
			range.length = (int)search_text.size();
			char_ranges.push_back(range); // character offset + length
			ofs = page_text.find(search_text, ofs + 1);
		}

		// Retrieve Highlights object and apply annotations to the page
		Highlights hlts = txt.GetHighlights(char_ranges);
		hlts.Begin(doc);
		while (hlts.HasNext())
		{
			const double* quads;
			int quad_count = hlts.GetCurrentQuads(quads);
			for (int i = 0; i < quad_count; ++i)
			{
				// assume each quad is an axis-aligned rectangle
				const double* q = &quads[8 * i];
				double x1 = min(min(min(q[0], q[2]), q[4]), q[6]);
				double x2 = max(max(max(q[0], q[2]), q[4]), q[6]);
				double y1 = min(min(min(q[1], q[3]), q[5]), q[7]);
				double y2 = max(max(max(q[1], q[3]), q[5]), q[7]);
				Annots::Highlight highlight = Annots::Highlight::Create(doc, Rect(x1, y1, x2, y2));
				highlight.RefreshAppearance();
				page.AnnotPushBack(highlight);

				cout.setf(ios_base::fixed, ios_base::floatfield);
				cout.precision(2);
				cout << "[" << x1 << ", " << y1 << ", " << x2 << ", " << y2 << "]" << endl;
			}
			hlts.Next();
		}

		// Output highlighted PDF doc
		doc.Save((output_path + "search_highlights.pdf").c_str(), SDFDoc::e_linearized, 0);

	}
	catch (Exception& e)
	{
		cout << e << endl;
		ret = 1;
	}
	catch (...)
	{
		cout << "Unknown Exception" << endl;
		ret = 1;
	}

	PDFNet::Terminate();
	return ret;
}
