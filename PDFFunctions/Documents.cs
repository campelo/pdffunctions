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

namespace PDFFunctions
{
    public class Documents(ILoggerFactory loggerFactory, IOptions<ApryseOptions> options)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<Documents>();

        [Function(nameof(Merge))]
        public HttpResponseData Merge(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Merge")]
            HttpRequestData req)
        {
            _logger.LogInformation($"C# HTTP trigger function {nameof(Merge)} processed a request.");
            MergePDFs(options);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString("All documents merged.");
            return response;
        }

        private void MergePDFs(IOptions<ApryseOptions> options)
        {
            PDFNet.Initialize(options.Value.Key);

            // Relative path to the folder containing test files.
            string input_path = Path.Combine(Path.GetTempPath(), "pdf_input");
            string output_path = Path.Combine(Path.GetTempPath(), "pdf_output");

            try
            {
                var files = Directory.GetFiles(input_path, "*.pdf");

                using (PDFDoc new_doc = new PDFDoc()) // Create a new document
                using (ElementBuilder builder = new ElementBuilder())
                using (ElementWriter writer = new ElementWriter())
                {
                    foreach (var file in files)
                    {
                        MergeFile(new_doc, builder, writer, file);
                    }
                    new_doc.Save(Path.Combine(output_path, "mergedfile.pdf"), SDFDoc.SaveOptions.e_linearized);
                    _logger.LogInformation("Done. Result saved in newsletter_booklet.pdf...");
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("Exception caught:\n{0}", e);
            }
            PDFNet.Terminate();
        }

        private static void MergeFile(PDFDoc new_doc, ElementBuilder builder, ElementWriter writer, string file)
        {
            ArrayList import_list = new ArrayList();
            var in_doc = new PDFDoc(file);
            in_doc.InitSecurityHandler();
            for (PageIterator itr = in_doc.GetPageIterator(); itr.HasNext(); itr.Next())
                import_list.Add(itr.Current());

            ArrayList imported_pages = new_doc.ImportPages(import_list);

            // Paper dimension for A3 format in points. Because one inch has 
            // 72 points, 11.69 inch 72 = 841.69 points
            Rect media_box = new Rect(0, 0, 1190.88, 841.69);
            double mid_point = media_box.Width() / 2;

            for (int i = 0; i < imported_pages.Count; ++i)
            {
                // Create a blank new A3 page and place on it two pages from the input document.
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

                    // Paper dimension for A3 format in points. Because one inch has 
                    // 72 points, 11.69 inch 72 = 841.69 points
                    Rect media_box = new Rect(0, 0, 1190.88, 841.69);
                    double mid_point = media_box.Width() / 2;

                    for (int i = 0; i < imported_pages.Count; ++i)
                    {
                        // Create a blank new A3 page and place on it two pages from the input document.
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
                    _logger.LogInformation("Done. Result saved in newsletter_booklet.pdf...");
                }
            }
        }
    }
}
