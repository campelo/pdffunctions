//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import com.pdftron.common.PDFNetException;
import com.pdftron.pdf.*;
import com.pdftron.sdf.SDFDoc;
import java.util.ArrayList;


// This sample illustrates the basic text highlight capabilities of PDFNet.
// It simulates a full-text search engine that finds all occurrences of the word 'Federal'.
// It then highlights those words on the page.
//
// Note: The TextSearch class is the preferred solution for searching text within a single
// PDF file. TextExtractor provides search highlighting capabilities where a large number
// of documents are indexed using a 3rd party search engine.
public class HighlightsTest {

    public static void main(String[] args) {
        PDFNet.initialize(PDFTronLicense.Key());

        // Relative path to the folder containing test files.
        String input_path = "../../TestFiles/";
        String output_path = "../../TestFiles/Output/";

        // Sample code showing how to use high-level text highlight APIs.
        try (PDFDoc doc = new PDFDoc(input_path + "paragraphs_and_tables.pdf")) {
            doc.initSecurityHandler();

            Page page = doc.getPage(1); // first page
            if (page == null) {
                System.out.println("Page not found.");
                PDFNet.terminate();
                return;
            }

            TextExtractor txt = new TextExtractor();
            txt.begin(page);  // read the page

            // Do not dehyphenate; that would interfere with character offsets
            boolean dehyphen = false;
            // Retrieve the page text
            String page_text = txt.getAsText(dehyphen);

            // Simulating a full-text search engine that finds all occurrences of the word 'Federal'.
            // In a real application, plug in your own search engine here.
            String search_text = "Federal";
            ArrayList<TextExtractor.CharRange> char_ranges_list = new ArrayList<TextExtractor.CharRange>();
            int ofs = page_text.indexOf(search_text);
            while (ofs >= 0) {
                char_ranges_list.add(txt.new CharRange(ofs, search_text.length())); // character offset + length
                ofs = page_text.indexOf(search_text, ofs + 1);
            }

            // Convert ArrayList to array, as required by TextExtractor.getHighlights()
            TextExtractor.CharRange[] char_ranges = new TextExtractor.CharRange[char_ranges_list.size()];
            char_ranges_list.toArray(char_ranges);

            // Retrieve Highlights object and apply annotations to the page
            Highlights hlts = txt.getHighlights(char_ranges);
            hlts.begin(doc);
            while (hlts.hasNext()) {
                double[] q = hlts.getCurrentQuads();
                int quad_count = q.length / 8;
                for (int i = 0; i < quad_count; ++i) {
                    // assume each quad is an axis-aligned rectangle
                    int offset = 8 * i;
                    double x1 = Math.min(Math.min(Math.min(q[offset + 0], q[offset + 2]), q[offset + 4]), q[offset + 6]);
                    double x2 = Math.max(Math.max(Math.max(q[offset + 0], q[offset + 2]), q[offset + 4]), q[offset + 6]);
                    double y1 = Math.min(Math.min(Math.min(q[offset + 1], q[offset + 3]), q[offset + 5]), q[offset + 7]);
                    double y2 = Math.max(Math.max(Math.max(q[offset + 1], q[offset + 3]), q[offset + 5]), q[offset + 7]);
                    com.pdftron.pdf.annots.Highlight highlight = com.pdftron.pdf.annots.Highlight.create(doc, new Rect(x1, y1, x2, y2));
                    highlight.refreshAppearance();
                    page.annotPushBack(highlight);

                    System.out.println(String.format("[%.2f, %.2f, %.2f, %.2f]", x1, y1, x2, y2));
                }
                hlts.next();
            }

            // Output highlighted PDF doc
            doc.save(output_path + "search_highlights.pdf", SDFDoc.SaveMode.LINEARIZED, null);
        } catch (PDFNetException e) {
            System.out.println(e);
        }

        PDFNet.terminate();
    }
}
