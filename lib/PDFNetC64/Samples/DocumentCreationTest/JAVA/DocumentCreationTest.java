//---------------------------------------------------------------------------------------
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
// Consult legal.txt regarding legal and license information.
//---------------------------------------------------------------------------------------

import com.pdftron.common.Matrix2D;
import com.pdftron.common.PDFNetException;
import com.pdftron.pdf.*;
import com.pdftron.sdf.SDFDoc;
import com.pdftron.filters.*;
import com.pdftron.layout.*;

public class DocumentCreationTest {

    // Iterate over all text runs of the document and make every second run
    // bold with smaller font size.
    public static void ModifyContentTree(ContentNode node) throws PDFNetException
    {
        boolean bold = false;

        ContentNodeIterator itr = node.getContentNodeIterator();
        while (itr.hasNext())
        {
            ContentElement el = itr.next();

            TextRun text_run = el.asTextRun();
            if (text_run != null)
            {
                if (bold)
                {
                    text_run.getTextStyledElement().setBold(true);
                    text_run.getTextStyledElement().setFontSize(text_run.getTextStyledElement().getFontSize() * 0.8);
                }
                bold = !bold;
                continue;
            }

            ContentNode content_node = el.asContentNode();
            if (content_node != null)
            {
                ModifyContentTree(content_node);
                continue;
            }
        }
    }

    public static void main(String[] args) {
        PDFNet.initialize(PDFTronLicense.Key());

        // Relative path to the folder containing test files.
        String output_path = "../../TestFiles/Output/";
        String output_name = "created_doc.pdf";
        String para_text =
            "Lorem ipsum dolor "
            + "sit amet, consectetur adipisicing elit, sed "
            + "do eiusmod tempor incididunt ut labore "
            + "et dolore magna aliqua. Ut enim ad "
            + "minim veniam, quis nostrud exercitation "
            + "ullamco laboris nisi ut aliquip ex ea "
            + "commodo consequat. Duis aute irure "
            + "dolor in reprehenderit in voluptate velit "
            + "esse cillum dolore eu fugiat nulla pariatur. "
            + "Excepteur sint occaecat cupidatat "
            + "non proident, sunt in culpa qui officia "
            + "deserunt mollit anim id est laborum.";

        try {
            FlowDocument flowdoc = new FlowDocument();
            Paragraph para = flowdoc.addParagraph();
            TextStyledElement st_para = para.getTextStyledElement();

            st_para.setFontSize(24);
            st_para.setTextColor(255, 0, 0);
            para.addText("Start \tRed \tText\n");

		    para.addTabStop(150);
		    para.addTabStop(250);
            st_para.setTextColor(0, 0, 255);
            para.addText("Start \tBlue \tText\n");

            TextRun last_run = para.addText("Start Green Text\n");

            boolean start = true;
            ContentNodeIterator itr = para.getContentNodeIterator();
            while (itr.hasNext())
            {
                ContentElement el = itr.next();
                TextRun run = el.asTextRun();
                if (run != null)
                {
                    run.getTextStyledElement().setFontSize(12);
                    if (start)
                    {
                        // Restore red color.
                        start = false;
                        run.setText(run.getText() + "(restored \tred \tcolor)\n");
                        run.getTextStyledElement().setTextColor(255, 0, 0);
                    }
                }
            }

            TextStyledElement st_last = last_run.getTextStyledElement();
            st_last.setTextColor(0, 255, 0);
            st_last.setItalic(true);
            st_last.setFontSize(18);

            para.getTextStyledElement().setBold(true);
            para.setBorder(0.2, 0, 127, 0);
            st_last.setBold(false);

            {
                flowdoc.addParagraph("Simple list creation process. All elements are added in their natural order\n\n");

                List list = flowdoc.addList();
                list.setNumberFormat(List.ListItemNumberFormat.e_upper_letter);
                list.setStartIndex(4);

                ListItem item = list.addItem(); // creates "D."
                item.addParagraph("item 0[0]");
                Paragraph px = item.addParagraph("item 0[1]");
                TextStyledElement xx_para = px.getTextStyledElement();
                xx_para.setTextColor(255, 99, 71);
                px.addText(" Some More Text!");


                ListItem item2 = list.addItem(); // creates "E."
                List item2List = item2.addList();
                item2List.setStartIndex(0);
                item2List.setNumberFormat(List.ListItemNumberFormat.e_decimal, "", true);
                item2List.addItem().addParagraph("item 1[0].0");
                Paragraph pp = item2List.addItem().addParagraph("item 1[0].1");
                TextStyledElement sx_para = pp.getTextStyledElement();
                sx_para.setTextColor(0, 0, 255);
                pp.addText(" Some More Text");
                item2List.addItem().addParagraph("item 1[0].2");
                List item2List1 = item2List.addItem().addList();
                item2List1.setStartIndex(7);
                item2List1.setNumberFormat(List.ListItemNumberFormat.e_upper_roman, ")", true);
                item2List1.addItem().addParagraph("item 1[0].3.0");
                item2List1.addItem().addParagraph("item 1[0].3.1");

                ListItem extraItem = item2List1.addItem();
                extraItem.addParagraph("item 1[0].3.2[0]");
                extraItem.addParagraph("item 1[0].3.2[1]");
                List  fourth = extraItem.addList();
                fourth.setNumberFormat(List.ListItemNumberFormat.e_decimal, "", true);
                fourth.addItem().addParagraph("Fourth Level");

                fourth = item2List1.addItem().addList();
                fourth.setNumberFormat(List.ListItemNumberFormat.e_lower_letter, "", true);
                fourth.addItem().addParagraph("Fourth Level (again)");


                item2.addParagraph("item 1[1]");
                List item2List2 = item2.addList();
                item2List2.setStartIndex(10);
                item2List2.setNumberFormat(List.ListItemNumberFormat.e_lower_roman); //  , "", true);
                item2List2.addItem().addParagraph("item 1[2].0");
                item2List2.addItem().addParagraph("item 1[2].1");
                item2List2.addItem().addParagraph("item 1[2].2");

                ListItem item3 = list.addItem(); // creates "F."
                item3.addParagraph("item 2");

                ListItem item4 = list.addItem(); // creates "G."
                item4.addParagraph("item 3");

                ListItem item5 = list.addItem(); // creates "H."
                item5.addParagraph("item 4");
            }

            for (itr = flowdoc.getBody().getContentNodeIterator(); itr.hasNext();)
            {
                ContentElement el = itr.next();

                List list = el.asList();
                if (list != null)
                {
                    if (list.getIndentationLevel() == 1)
                    {
                        Paragraph p = list.addItem().addParagraph("Item added during iteration");
                        TextStyledElement ps = p.getTextStyledElement();
                        ps.setTextColor(0, 127, 0);
                    }
                }

                ListItem list_item = el.asListItem();
                if (list_item != null)
                {
                    if (list_item.getIndentationLevel() == 2)
                    {
                        Paragraph p = list_item.addParagraph("* Paragraph added during iteration");
                        TextStyledElement ps = p.getTextStyledElement();
                        ps.setTextColor(0, 0, 255);
                    }
                }
            }

            flowdoc.addParagraph("\f"); // page break

            {
                flowdoc.addParagraph("Nonlinear list creation flow. Items are added randomly. List body separated by a paragraph, does not belong to the list\n\n");

                List list = flowdoc.addList();
                list.setNumberFormat(List.ListItemNumberFormat.e_upper_letter);
                list.setStartIndex(4);

                ListItem item = list.addItem(); // creates "D."
                item.addParagraph("item 0[0]");
                Paragraph px = item.addParagraph("item 0[1]");
                TextStyledElement xx_para = px.getTextStyledElement();
                xx_para.setTextColor(255, 99, 71);
                px.addText(" Some More Text!");
                item.addParagraph("item 0[2]");
                px = item.addParagraph("item 0[3]");
                item.addParagraph("item 0[4]");
                xx_para = px.getTextStyledElement();
                xx_para.setTextColor(255, 99, 71);


                ListItem item2 = list.addItem(); // creates "E."
                List item2List = item2.addList();
                item2List.setStartIndex(0);
                item2List.setNumberFormat(List.ListItemNumberFormat.e_decimal, "", true);
                item2List.addItem().addParagraph("item 1[0].0");
                Paragraph pp = item2List.addItem().addParagraph("item 1[0].1");
                TextStyledElement sx_para = pp.getTextStyledElement();
                sx_para.setTextColor(0, 0, 255);
                pp.addText(" Some More Text");


                ListItem item3 = list.addItem(); // creates "F."
                item3.addParagraph("item 2");

                item2List.addItem().addParagraph("item 1[0].2");

                item2.addParagraph("item 1[1]");
                List item2List2 = item2.addList();
                item2List2.setStartIndex(10);
                item2List2.setNumberFormat(List.ListItemNumberFormat.e_lower_roman); //  , "", true);
                item2List2.addItem().addParagraph("item 1[2].0");
                item2List2.addItem().addParagraph("item 1[2].1");
                item2List2.addItem().addParagraph("item 1[2].2");

                ListItem item4 = list.addItem(); // creates "G."
                item4.addParagraph("item 3");

                List item2List1 = item2List.addItem().addList();
                item2List1.setStartIndex(7);
                item2List1.setNumberFormat(List.ListItemNumberFormat.e_upper_roman, ")", true);
                item2List1.addItem().addParagraph("item 1[0].3.0");

                flowdoc.addParagraph("---------------------------------- splitting paragraph ----------------------------------");

                item2List1.continueList();

                item2List1.addItem().addParagraph("item 1[0].3.1 (continued)");
                ListItem extraItem = item2List1.addItem();
                extraItem.addParagraph("item 1[0].3.2[0]");
                extraItem.addParagraph("item 1[0].3.2[1]");
                List fourth = extraItem.addList();
                fourth.setNumberFormat(List.ListItemNumberFormat.e_decimal, "", true);
                fourth.addItem().addParagraph("FOURTH LEVEL");

                ListItem item5 = list.addItem(); // creates "H."
                item5.addParagraph("item 4 (continued)");
            }


            flowdoc.addParagraph(" ");

            flowdoc.setDefaultMargins(72.0, 72.0, 144.0, 228.0);
            flowdoc.setDefaultPageSize(650, 750);
            flowdoc.addParagraph(para_text);

            int[] clr1 = new int[] {50, 50, 199};
            int[] clr2 = new int[] {30, 199, 30};

            for (int i = 0; i < 50; i++)
            {
                para = flowdoc.addParagraph();
                TextStyledElement st = para.getTextStyledElement();

                int point_size = (17*i*i*i)%13+5;
                if (i % 2 == 0)
                {
                    st.setItalic(true);
                    st.setTextColor(clr1[0], clr1[1], clr1[2]);
                    st.setBackgroundColor(200, 200, 200);
                    para.setSpaceBefore(20);
                    para.setStartIndent(20);
                    para.setJustificationMode(Paragraph.TextJustification.e_text_justify_left);
                }
                else
                {
                    st.setTextColor(clr2[0], clr2[1], clr2[2]);
                    para.setSpaceBefore(50);
                    para.setEndIndent(20);
                    para.setJustificationMode(Paragraph.TextJustification.e_text_justify_right);
                }

                para.addText(para_text);
                para.addText(" " + para_text);
                st.setFontSize(point_size);
            }

            // Table creation
            Table new_table = flowdoc.addTable();
            new_table.setDefaultColumnWidth(100);
            new_table.setDefaultRowHeight(15);

            for (int i = 0; i < 4; i++)
            {
                TableRow row = new_table.addTableRow();
                row.setRowHeight(new_table.getDefaultRowHeight() + i*5);
                for (int j = 0; j < 5; j++)
                {
                    TableCell cell = row.addTableCell();
                    cell.setBorder(0.5, 255, 0, 0);

                    if (i == 3)
                    {
                        if (j % 2 != 0)
                        {
                            cell.setVerticalAlignment(TableCell.CellAlignmentVerticalVals.e_alignment_center);
                        }
                        else
                        {
                            cell.setVerticalAlignment(TableCell.CellAlignmentVerticalVals.e_alignment_bottom);
                        }
                    }

                    if ((i == 3) && (j == 4))
                    {
                        Paragraph para_title = cell.addParagraph("Table title");
                        para_title.setJustificationMode(Paragraph.TextJustification.e_text_justify_center);

                        Table nested_table = cell.addTable();
                        nested_table.setDefaultColumnWidth(33);
                        nested_table.setDefaultRowHeight(5);
                        nested_table.setBorder(0.5, 0, 0, 0);

                        for (int nested_row_index = 0; nested_row_index < 3; nested_row_index++)
                        {
                            TableRow nested_row = nested_table.addTableRow();
                            for (int nested_column_index = 0; nested_column_index < 3; nested_column_index++)
                            {
                                String str = Integer.toString(nested_row_index) + "/" + Integer.toString(nested_column_index);
                                TableCell nested_cell = nested_row.addTableCell();	
                                nested_cell.setBackgroundColor(200, 200, 255);
                                nested_cell.setBorder(0.1, 0, 255, 0);

                                Paragraph new_para = nested_cell.addParagraph(str);
                                new_para.setJustificationMode(Paragraph.TextJustification.e_text_justify_right);	
                            }
                        }
                    }
                    else
                    if ((i == 2) && (j == 2))
                    {
                        Paragraph new_para = cell.addParagraph("Cell " + Integer.toString(j) + " x " + Integer.toString(i) + "\n");
                        new_para.addText("to be bold text 1\n");
                        new_para.addText("still normal text\n");
                        new_para.addText("to be bold text 2");
                        cell.addParagraph("Second Paragraph");
                    }
                    else
                    {
                        cell.addParagraph("Cell " + Integer.toString(j) + " x " + Integer.toString(i));
                    }
                }
            }

            // Walk the content tree and modify some text runs.
            ModifyContentTree(flowdoc.getBody());

            // Merge cells
            TableCell merged_cell = new_table.getTableCell(2, 0).mergeCellsRight(1);
            merged_cell.setHorizontalAlignment(TableCell.CellAlignmentHorizontalVals.e_alignment_middle);

            new_table.getTableCell(0, 0).mergeCellsDown(1).setVerticalAlignment(TableCell.CellAlignmentVerticalVals.e_alignment_center);

            // Walk over the table and change the first cell in each row.
            int row_idx = 0;
            int[] clr_row1 = new int[] {175, 240, 240};
            int[] clr_row2 = new int[] {250, 250, 175};

            for (ContentNodeIterator table_itr = new_table.getContentNodeIterator();
                    table_itr.hasNext();)
            {
                TableRow row = table_itr.next().asTableRow();
                if (row != null)
                {
                    for (ContentNodeIterator row_itr = row.getContentNodeIterator();
                            row_itr.hasNext();)
                    {
                        TableCell cell = row_itr.next().asTableCell();
                        if (cell != null)
                        {
                            if (row_idx % 2 != 0)
                            {
                                cell.setBackgroundColor(clr_row1[0], clr_row1[1], clr_row1[2]);
                            }
                            else
                            {
                                cell.setBackgroundColor(clr_row2[0], clr_row2[1], clr_row2[2]);
                            }

                            for (ContentNodeIterator cell_itr = cell.getContentNodeIterator();
                                    cell_itr.hasNext();)
                            {
                                ContentElement element = cell_itr.next();
                                para = element.asParagraph();
                                if (para != null)
                                {
                                    TextStyledElement st = para.getTextStyledElement();
                                    st.setTextColor(255, 0, 0);
                                    st.setFontSize(12);
                                }
                                else
                                {
                                    Table nested_table = element.asTable();
                                    if (nested_table != null)
                                    {
                                        TableCell nested_cell = nested_table.getTableCell(1, 1);
                                        nested_cell.setBackgroundColor(255, 127, 127);
                                    }
                                }
                            }
                        }
                    }
                }

                ++row_idx;
            }

            PDFDoc my_pdf = flowdoc.paginateToPDF();
            my_pdf.save(output_path+output_name, SDFDoc.SaveMode.LINEARIZED, null);
        }
        catch (Exception e)
        {
            e.printStackTrace();
        }

        PDFNet.terminate();
    }
}