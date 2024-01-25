//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import com.pdftron.pdf.*;
import com.pdftron.sdf.Obj;
import com.pdftron.sdf.SDFDoc;
import com.pdftron.common.PDFNetException;
import com.pdftron.filters.*;


public class U3DTest {

    static String input_path = "../../TestFiles/";

    static void Create3DAnnotation(PDFDoc doc, Obj annots) throws PDFNetException {
        // ---------------------------------------------------------------------------------
        // Create a 3D annotation based on U3D content. PDF 1.6 introduces the capability
        // for collections of three-dimensional objects, such as those used by CAD software,
        // to be embedded in PDF files.
        Obj link_3D = doc.createIndirectDict();
        link_3D.putName("Subtype", "3D");

        // Annotation location on the page
        Rect link_3D_rect = new Rect(25, 180, 585, 643);
        link_3D.putRect("Rect", link_3D_rect.getX1(), link_3D_rect.getY1(),
                link_3D_rect.getX2(), link_3D_rect.getY2());
        annots.pushBack(link_3D);

        // The 3DA entry is an activation dictionary (see Table 9.34 in the PDF Reference Manual)
        // that determines how the state of the annotation and its associated artwork can change.
        Obj activation_dict_3D = link_3D.putDict("3DA");

        // Set the annotation so that it is activated as soon as the page containing the
        // annotation is opened. Other options are: PV (page view) and XA (explicit) activation.
        activation_dict_3D.putName("A", "PO");

        // Embed U3D Streams (3D Model/Artwork).
        {
            MappedFile u3d_file = new MappedFile(input_path + "dice.u3d");
            FilterReader u3d_reader = new FilterReader(u3d_file);

            // To embed 3D stream without compression, you can omit the second parameter in CreateIndirectStream.
            Obj u3d_data_dict = doc.createIndirectStream(u3d_reader, new FlateEncode(null));
            u3d_data_dict.putName("Subtype", "U3D");
            link_3D.put("3DD", u3d_data_dict);
        }

        // Set the initial view of the 3D artwork that should be used when the annotation is activated.
        Obj view3D_dict = link_3D.putDict("3DV");
        {
            view3D_dict.putString("IN", "Unnamed");
            view3D_dict.putString("XN", "Default");
            view3D_dict.putName("MS", "M");
            view3D_dict.putNumber("CO", 27.5);

            // A 12-element 3D transformation matrix that specifies a position and orientation
            // of the camera in world coordinates.
            Obj tr3d = view3D_dict.putArray("C2W");
            tr3d.pushBackNumber(1);
            tr3d.pushBackNumber(0);
            tr3d.pushBackNumber(0);
            tr3d.pushBackNumber(0);
            tr3d.pushBackNumber(0);
            tr3d.pushBackNumber(-1);
            tr3d.pushBackNumber(0);
            tr3d.pushBackNumber(1);
            tr3d.pushBackNumber(0);
            tr3d.pushBackNumber(0);
            tr3d.pushBackNumber(-27.5);
            tr3d.pushBackNumber(0);

        }

        // Create annotation appearance stream, a thumbnail which is used during printing or
        // in PDF processors that do not understand 3D data.
        Obj ap_dict = link_3D.putDict("AP");
        {
            ElementBuilder builder = new ElementBuilder();
            ElementWriter writer = new ElementWriter();
            writer.begin(doc);

            String thumb_pathname = input_path + "dice.jpg";
            Image image = Image.create(doc, thumb_pathname);
            writer.writePlacedElement(builder.createImage(image, 0.0, 0.0, link_3D_rect.getWidth(), link_3D_rect.getHeight()));

            Obj normal_ap_stream = writer.end();
            normal_ap_stream.putName("Subtype", "Form");
            normal_ap_stream.putRect("BBox", 0, 0, link_3D_rect.getWidth(), link_3D_rect.getHeight());
            ap_dict.put("N", normal_ap_stream);
        }
    }

    public static void main(String[] args) {
        String output_path = "../../TestFiles/Output/";
        PDFNet.initialize(PDFTronLicense.Key());
        try (PDFDoc doc = new PDFDoc()) {
            Page page = doc.pageCreate();
            doc.pagePushBack(page);
            Obj annots = doc.createIndirectArray();
            page.getSDFObj().put("Annots", annots);

            Create3DAnnotation(doc, annots);
            doc.save(output_path + "dice_u3d.pdf", SDFDoc.SaveMode.LINEARIZED, null);
            System.out.println("Done");
        } catch (Exception e) {
            e.printStackTrace();
        }

        PDFNet.terminate();
    }
}
