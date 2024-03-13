using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PDFFunctions.Options;
using pdftron;
using pdftron.PDF;
using pdftron.SDF;
using System.Collections;
using System.Net;

namespace PDFFunctions;

public class Documents
{
    private readonly ILogger logger;
    private readonly ApryseOptions options;

    public Documents(ILoggerFactory loggerFactory, IOptions<ApryseOptions> options)
    {
        this.options = options.Value;
        logger = loggerFactory.CreateLogger<Documents>();
        PDFNet.Initialize(this.options.Key);
    }

    [Function(nameof(Merge))]
    public HttpResponseData Merge(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = nameof(Merge))]
        HttpRequestData req)
    {
        logger.LogInformation($"C# HTTP trigger function {nameof(Merge)} processed a request.");
        MergePDFs();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString("All documents merged.");
        return response;
    }

    [Function(nameof(ConvertDoc))]
    public HttpResponseData ConvertDoc(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = $"{nameof(ConvertDoc)}")]
        HttpRequestData req)
    {
        logger.LogInformation($"C# HTTP trigger function {nameof(ConvertDoc)} processed a request.");
        ConvertDoc();

        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
        response.WriteString("Document converted.");
        return response;
    }

    private void ConvertDoc()
    {
        string input_path = Path.Combine(Path.GetTempPath(), "pdf_input");
        string output_path = Path.Combine(Path.GetTempPath(), "pdf_output", "converted");
        if (!Directory.Exists(output_path))
            Directory.CreateDirectory(output_path);
        foreach (string originalFile in Directory.GetFiles(input_path))
        {
            PDFDoc doc = new();
            pdftron.PDF.Convert.ToPdf(doc, originalFile);
            var pdfFile = Path.GetFileName(originalFile) + ".pdf";
            doc.Save(Path.Combine(output_path, pdfFile), SDFDoc.SaveOptions.e_linearized);

            //pdftron.PDF.Convert.WordOutputOptions wordOutputOptions = new();

            //// Optionally convert only the first page
            //wordOutputOptions.SetPages(1, 1);
            //var docFile = Path.GetFileNameWithoutExtension(originalFile);
            //// Requires the Structured Output module
            //pdftron.PDF.Convert.ToWord(pdfFile, Path.Combine(output_path, docFile), wordOutputOptions);
        }
    }

    private void MergePDFs()
    {
        // Relative path to the folder containing test files.
        string input_path = Path.Combine(Path.GetTempPath(), "pdf_input");
        string output_path = Path.Combine(Path.GetTempPath(), "pdf_output", "merged");
        string temp_pdf_folder_name = Path.Combine(input_path, Guid.NewGuid().ToString());

        try
        {
            if (!Directory.Exists(temp_pdf_folder_name))
                Directory.CreateDirectory(temp_pdf_folder_name);
            var files = Directory.GetFiles(input_path);

            using (PDFDoc new_doc = new()) // Create a new document
            using (ElementBuilder builder = new())
            using (ElementWriter writer = new())
            {
                int doc_order = 1;
                foreach (var file in files)
                {
                    MergeFile(new_doc, builder, writer, file, temp_pdf_folder_name, ref doc_order);
                }
                new_doc.Save(Path.Combine(output_path, "mergedfile.pdf"), SDFDoc.SaveOptions.e_linearized);
                logger.LogInformation("Done. Result saved in newsletter_booklet.pdf...");
            }
        }
        catch (Exception e)
        {
            logger.LogInformation("Exception caught:\n{0}", e);
        }
        finally
        {
            PDFNet.Terminate();
            try
            {
                if (Directory.Exists(temp_pdf_folder_name))
                    Directory.Delete(temp_pdf_folder_name, true);
            }
            catch
            {
            }
        }
    }

    private void MergeFile(PDFDoc new_doc, ElementBuilder builder, ElementWriter writer, string file, string temp_pdf_folder_name, ref int doc_order)
    {
        var extension = Path.GetExtension(file)?.ToLower();
        var new_file = extension switch
        {
            ".pdf" => file,
            _ => CreateTempPDF(file, temp_pdf_folder_name, ref doc_order)
        };
        MergePDFFile(new_doc, builder, writer, new_file);
    }

    private string CreateTempPDF(string file, string temp_pdf_folder_name, ref int doc_order)
    {
        var doc = new PDFDoc();
        pdftron.PDF.Convert.ToPdf(doc, file);
        var file_name = Path.Combine(temp_pdf_folder_name, $"doc_{doc_order++}.pdf");
        doc.Save(file_name, SDFDoc.SaveOptions.e_linearized);
        return file_name;
    }

    private static void MergePDFFile(PDFDoc new_doc, ElementBuilder builder, ElementWriter writer, string pdf_file)
    {
        FileStream fileStream = new FileStream(pdf_file, FileMode.Open, FileAccess.Read);
        MergePDFFile(new_doc, builder, writer, fileStream);
    }

    private static void MergePDFFile(PDFDoc new_doc, ElementBuilder builder, ElementWriter writer, Stream stream)
    {
        ArrayList import_list = new ArrayList();
        var in_doc = new PDFDoc(stream);
        in_doc.InitSecurityHandler();
        for (PageIterator itr = in_doc.GetPageIterator(); itr.HasNext(); itr.Next())
            import_list.Add(itr.Current());

        ArrayList imported_pages = new_doc.ImportPages(import_list);

        // Paper dimension for A3 format in points. Because one inch has 
        // 72 points, 11.69 inch 72 = 841.69 points vs 1190.88 points
        // https://community.apryse.com/t/how-do-i-control-paper-size-when-printing-pdf-using-startprintjob-in-c/1243
        /**
4A0 = 1682 mm x 2378 mm = 4768 pt x 6741 pt
2A0 = 1189 mm x 1682 mm = 3370 pt x 4768 pt
A0 = 841 mm x 1189 mm = 2384 pt x 3370 pt
A1 = 594 mm x 841 mm = 1684 pt x 2384 pt
A2 = 420 mm x 594 mm = 1191 pt x 1684 pt
A3 = 297 mm x 420 mm = 842 pt x 1191 pt
A4 = 210 mm x 297 mm = 595 pt x 842 pt
A5 = 148 mm x 210 mm = 420 pt x 595 pt
A6 = 105 mm x 148 mm = 298 pt x 420 pt
A7 = 74 mm x 105 mm = 210 pt x 298 pt
A8 = 52 mm x 74 mm = 147 pt x 210 pt
A9 = 37 mm x 52 mm = 105 pt x 147 pt
A10 = 26 mm x 37 mm = 74 pt x 105 pt

B0 = 1000 mm x 1414 mm = 2835 pt x 4008 pt
B1 = 707 mm x 1000 mm = 2004 pt x 2835 pt
B2 = 500 mm x 707 mm = 1417 pt x 2004 pt
B3 = 353 mm x 500 mm = 1001 pt x 1417 pt
B4 = 250 mm x 353 mm = 709 pt x 1001 pt
B5 = 176 mm x 250 mm = 499 pt x 709 pt
B6 = 125 mm x 176 mm = 354 pt x 499 pt
B7 = 88 mm x 125 mm = 249 pt x 354 pt
B8 = 62 mm x 88 mm = 176 pt x 249 pt
B9 = 44 mm x 62 mm = 125 pt x 176 pt
B10 = 31 mm x 44 mm = 88 pt x 125 pt

C0 = 917 mm x 1297 mm = 2599 pt x 3677 pt
C1 = 648 mm x 917 mm = 1837 pt x 2599 pt
C2 = 458 mm x 648 mm = 1298 pt x 1837 pt
C3 = 324 mm x 458 mm = 918 pt x 1298 pt
C4 = 229 mm x 324 mm = 649 pt x 918 pt
C5 = 162 mm x 229 mm = 459 pt x 649 pt
C6 = 114 mm x 162 mm = 323 pt x 459 pt
C7 = 81 mm x 114 mm = 230 pt x 323 pt
C8 = 57 mm x 81 mm = 162 pt x 230 pt
C9 = 40 mm x 57 mm = 113 pt x 162 pt
C10 = 28 mm x 40 mm = 79 pt x 113 pt

Letter 8.5 x 11 612 x 792
Legal 8.5 x 14 612 x 1008
Ledger 11 x 17 792 x 1224
Executive 7.25 x 10.5 522 x 756
A3 11.69 x 16.53 842 x 1190
A4 8.27 x 11.69 595 x 842
A5 5.83 x 8.27 420 x 595
A6 4.13 x 5.83 297 x 420
Photo 4 x 6 288 x 432
B4 10.126 x 14.342 729 x 1033
B5 7.17 x 10.126 516 x 729
Oufuku-Hagaki 5.83 x 7.87 420 x 567
Hagaki 3.94 x 5.83 284 x 420
Super B 13 x 19 936 x 1368
Flsa 8.5 x 13 612 x 936
Number 10 Envelope 4.12 x 9.5 297 x 684
A2 Envelope 4.37 x 5.75 315 x 414
C6 Envelope 4.49 x 6.38 323 x 459
DL Envelope 4.33 x 8.66 312 x 624
         */
        // letter 612 x 792
        Rect media_box = new Rect(0, 0, 612, 792);
        double mid_point = media_box.Width() / 2;

        for (int i = 0; i < imported_pages.Count; ++i)
        {
            // Create a blank new letter page and place on it two pages from the input document.
            Page new_page = new_doc.PageCreate(media_box);
            writer.Begin(new_page);

            // Place the first page
            Page src_page = (Page)imported_pages[i];
            Element element = builder.CreateForm(src_page);

            double sc_x = media_box.Width() / src_page.GetPageWidth();
            double sc_y = media_box.Height() / src_page.GetPageHeight();
            double scale = Math.Min(sc_x, sc_y);
            element.GetGState().SetTransform(scale, 0, 0, scale, 0, 0);
            writer.WritePlacedElement(element);

            writer.End();
            new_doc.PagePushBack(new_page);
        }
    }

    private void OriginalMethod(string input_path, string output_path)
    {
        using (PDFDoc in_doc = new PDFDoc(input_path + "newsletter.pdf"))
        {
            in_doc.InitSecurityHandler();

            // Create a list of pages to import from one PDF document to another.
            ArrayList import_list = new ArrayList();
            for (PageIterator itr = in_doc.GetPageIterator(); itr.HasNext(); itr.Next())
                import_list.Add(itr.Current());

            using (PDFDoc new_doc = new PDFDoc()) // Create a new document
            using (ElementBuilder builder = new ElementBuilder())
            using (ElementWriter writer = new ElementWriter())
            {
                ArrayList imported_pages = new_doc.ImportPages(import_list);

                // Paper dimension for letter format in points. Because one inch has 
                // 72 points, 11.69 inch 72 = 841.69 points
                Rect media_box = new Rect(0, 0, 1190.88, 841.69);
                double mid_point = media_box.Width() / 2;

                for (int i = 0; i < imported_pages.Count; ++i)
                {
                    // Create a blank new letter page and place on it two pages from the input document.
                    Page new_page = new_doc.PageCreate(media_box);
                    writer.Begin(new_page);

                    // Place the first page
                    Page src_page = (Page)imported_pages[i];
                    Element element = builder.CreateForm(src_page);

                    double sc_x = mid_point / src_page.GetPageWidth();
                    double sc_y = media_box.Height() / src_page.GetPageHeight();
                    double scale = Math.Min(sc_x, sc_y);
                    element.GetGState().SetTransform(scale, 0, 0, scale, 0, 0);
                    writer.WritePlacedElement(element);

                    // Place the second page
                    ++i;
                    if (i < imported_pages.Count)
                    {
                        src_page = (Page)imported_pages[i];
                        element = builder.CreateForm(src_page);
                        sc_x = mid_point / src_page.GetPageWidth();
                        sc_y = media_box.Height() / src_page.GetPageHeight();
                        scale = Math.Min(sc_x, sc_y);
                        element.GetGState().SetTransform(scale, 0, 0, scale, mid_point, 0);
                        writer.WritePlacedElement(element);
                    }

                    writer.End();
                    new_doc.PagePushBack(new_page);
                }
                new_doc.Save(output_path + "newsletter_booklet.pdf", SDFDoc.SaveOptions.e_linearized);
                logger.LogInformation("Done. Result saved in newsletter_booklet.pdf...");
            }
        }
    }
}
