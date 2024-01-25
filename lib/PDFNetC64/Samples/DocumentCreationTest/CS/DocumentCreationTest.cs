//
// Copyright (c) 2001-2023 by Apryse Software Inc. All Rights Reserved.
//

using System;
using pdftron;
using pdftron.Common;
using pdftron.Filters;
using pdftron.SDF;
using pdftron.PDF;
using pdftron.Layout;

namespace DocumentCreationTestCS
{
	/// <summary>
	/// The sample code shows how to create document with the document creation API.
	/// </summary>
	class Class1
	{
		private static pdftron.PDFNetLoader pdfNetLoader = pdftron.PDFNetLoader.Instance();
		static Class1() {}

		// Iterate over all text runs of the document and make every second run
		// bold with smaller font size.
		static void ModifyContentTree(ContentNode node)
		{
			bool bold = false;

			for (ContentNodeIterator itr = node.GetContentNodeIterator();
					itr.HasNext();
					itr.Next())
			{
				ContentElement el = itr.Current();

				ContentNode content_node = el.AsContentNode();
				if (content_node != null)
				{
					ModifyContentTree(content_node);
					continue;
				}

				TextRun text_run = el.AsTextRun();
				if (text_run != null)
				{
					if (bold)
					{
						text_run.GetTextStyledElement().SetBold(true);
						text_run.GetTextStyledElement().SetFontSize(text_run.GetTextStyledElement().GetFontSize() * 0.8);
					}
					bold = !bold;
					continue;
				}
			} 
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			PDFNet.Initialize(PDFTronLicense.Key);

			// Relative path to the folder containing test files.
			string output_path = "../../TestFiles/Output/";

			string para_text =
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

			try
			{
				using (FlowDocument flowdoc = new FlowDocument())
				{
					Paragraph para = flowdoc.AddParagraph();
					TextStyledElement st_para = para.GetTextStyledElement();

					st_para.SetFontSize(24);
					st_para.SetTextColor(255, 0, 0);
					para.AddText("Start \tRed \tText\n");

					para.AddTabStop(150);
					para.AddTabStop(250);
					st_para.SetTextColor(0, 0, 255);
					para.AddText("Start \tBlue \tText\n");

					TextRun last_run = para.AddText("Start Green Text\n");

					ContentNodeIterator itr = para.GetContentNodeIterator();
					for (int i = 0; itr.HasNext(); itr.Next(), i++)
					{
						ContentElement el = itr.Current();

						TextRun text_run = el.AsTextRun();
						if (text_run != null)
						{
							text_run.GetTextStyledElement().SetFontSize(12);

							if (i == 0)
							{
								// Restore red color.
								text_run.SetText(text_run.GetText() + "(restored \tred \tcolor)\n");
								text_run.GetTextStyledElement().SetTextColor(255, 0, 0);
							}
						}
					}

					TextStyledElement st_last = last_run.GetTextStyledElement();

					st_last.SetTextColor(0, 255, 0);
					st_last.SetItalic(true);
					st_last.SetFontSize(18);

					para.GetTextStyledElement().SetBold(true);
					para.SetBorder(0.2, 0, 127, 0);
					st_last.SetBold(false);

                    {
                        flowdoc.AddParagraph("Simple list creation process. All elements are added in their natural order\n\n");

                        List list = flowdoc.AddList();
                        list.SetNumberFormat(List.NumberFormat.e_upper_letter);
                        list.SetStartIndex(4);

                        ListItem item = list.AddItem(); // creates "D."
                        item.AddParagraph("item 0[0]");
                        Paragraph px = item.AddParagraph("item 0[1]");
                        TextStyledElement xx_para = px.GetTextStyledElement();
                        xx_para.SetTextColor(255, 99, 71);
                        px.AddText(" Some More Text!");


                        ListItem item2 = list.AddItem(); // creates "E."
                        List item2List = item2.AddList();
                        item2List.SetStartIndex(0);
                        item2List.SetNumberFormat(List.NumberFormat.e_decimal, "", true);
                        item2List.AddItem().AddParagraph("item 1[0].0");
                        Paragraph pp = item2List.AddItem().AddParagraph("item 1[0].1");
                        TextStyledElement sx_para = pp.GetTextStyledElement();
                        sx_para.SetTextColor(0, 0, 255);
                        pp.AddText(" Some More Text");
                        item2List.AddItem().AddParagraph("item 1[0].2");
                        List item2List1 = item2List.AddItem().AddList();
                        item2List1.SetStartIndex(7);
                        item2List1.SetNumberFormat(List.NumberFormat.e_upper_roman, ")", true);
                        item2List1.AddItem().AddParagraph("item 1[0].3.0");
                        item2List1.AddItem().AddParagraph("item 1[0].3.1");

                        ListItem extraItem = item2List1.AddItem();
                        extraItem.AddParagraph("item 1[0].3.2[0]");
                        extraItem.AddParagraph("item 1[0].3.2[1]");
                        List fourth = extraItem.AddList();
                        fourth.SetNumberFormat(List.NumberFormat.e_decimal, "", true);
                        fourth.AddItem().AddParagraph("Fourth Level");

                        fourth = item2List1.AddItem().AddList();
                        fourth.SetNumberFormat(List.NumberFormat.e_lower_letter, "", true);
                        fourth.AddItem().AddParagraph("Fourth Level (again)");


                        item2.AddParagraph("item 1[1]");
                        List item2List2 = item2.AddList();
                        item2List2.SetStartIndex(10);
                        item2List2.SetNumberFormat(List.NumberFormat.e_lower_roman); //  , UString(""), true);
                        item2List2.AddItem().AddParagraph("item 1[2].0");
                        item2List2.AddItem().AddParagraph("item 1[2].1");
                        item2List2.AddItem().AddParagraph("item 1[2].2");

                        ListItem item3 = list.AddItem(); // creates "F."
                        item3.AddParagraph("item 2");

                        ListItem item4 = list.AddItem(); // creates "G."
                        item4.AddParagraph("item 3");

                        ListItem item5 = list.AddItem(); // creates "H."
                        item5.AddParagraph("item 4");
                    }

                    for (itr = flowdoc.GetBody().GetContentNodeIterator();
                        itr.HasNext();
                        itr.Next())
                    {
                        ContentElement el = itr.Current();

						List list = el.AsList();
						if (list != null)
                        {
                            if (list.GetIndentationLevel() == 1)
                            {
                                Paragraph p = list.AddItem().AddParagraph("Item added during iteration");
                                TextStyledElement ps = p.GetTextStyledElement();
                                ps.SetTextColor(0, 127, 0);
                            }
                        }

						ListItem list_item = el.AsListItem();
                        if (list_item != null)
                        {
                            if (list_item.GetIndentationLevel() == 2)
                            {
                                Paragraph p = list_item.AddParagraph("* Paragraph added during iteration");
                                TextStyledElement ps = p.GetTextStyledElement();
                                ps.SetTextColor(0, 0, 255);
                            }
                        }
                    }

                    flowdoc.AddParagraph("\f"); // page break

                    {
                        flowdoc.AddParagraph("Nonlinear list creation flow. Items are added randomly."
							+ " List body separated by a paragraph, does not belong to the list\n\n");

                        List list = flowdoc.AddList();
                        list.SetNumberFormat(List.NumberFormat.e_upper_letter);
                        list.SetStartIndex(4);

                        ListItem item = list.AddItem(); // creates "D."
                        item.AddParagraph("item 0[0]");
                        Paragraph px = item.AddParagraph("item 0[1]");
                        TextStyledElement xx_para = px.GetTextStyledElement();
                        xx_para.SetTextColor(255, 99, 71);
                        px.AddText(" Some More Text!");
                        item.AddParagraph("item 0[2]");
                        px = item.AddParagraph("item 0[3]");
                        item.AddParagraph("item 0[4]");
                        xx_para = px.GetTextStyledElement();
                        xx_para.SetTextColor(255, 99, 71);


                        ListItem item2 = list.AddItem(); // creates "E."
                        List item2List = item2.AddList();
                        item2List.SetStartIndex(0);
                        item2List.SetNumberFormat(List.NumberFormat.e_decimal, "", true);
                        item2List.AddItem().AddParagraph("item 1[0].0");
                        Paragraph pp = item2List.AddItem().AddParagraph("item 1[0].1");
                        TextStyledElement sx_para = pp.GetTextStyledElement();
                        sx_para.SetTextColor(0, 0, 255);
                        pp.AddText(" Some More Text");


                        ListItem item3 = list.AddItem(); // creates "F."
                        item3.AddParagraph("item 2");

                        item2List.AddItem().AddParagraph("item 1[0].2");

                        item2.AddParagraph("item 1[1]");
                        List item2List2 = item2.AddList();
                        item2List2.SetStartIndex(10);
                        item2List2.SetNumberFormat(List.NumberFormat.e_lower_roman); //  , UString(""), true);
                        item2List2.AddItem().AddParagraph("item 1[2].0");
                        item2List2.AddItem().AddParagraph("item 1[2].1");
                        item2List2.AddItem().AddParagraph("item 1[2].2");

                        ListItem item4 = list.AddItem(); // creates "G."
                        item4.AddParagraph("item 3");

                        List item2List1 = item2List.AddItem().AddList();
                        item2List1.SetStartIndex(7);
                        item2List1.SetNumberFormat(List.NumberFormat.e_upper_roman, ")", true);
                        item2List1.AddItem().AddParagraph("item 1[0].3.0");

                        flowdoc.AddParagraph("---------------------------------- splitting paragraph ----------------------------------");

                        item2List1.ContinueList();

                        item2List1.AddItem().AddParagraph("item 1[0].3.1 (continued)");
                        ListItem extraItem = item2List1.AddItem();
                        extraItem.AddParagraph("item 1[0].3.2[0]");
                        extraItem.AddParagraph("item 1[0].3.2[1]");
                        List fourth = extraItem.AddList();
                        fourth.SetNumberFormat(List.NumberFormat.e_decimal, "", true);
                        fourth.AddItem().AddParagraph("FOURTH LEVEL");

                        ListItem item5 = list.AddItem(); // creates "H."
                        item5.AddParagraph("item 4 (continued)");
                    }


                    flowdoc.AddParagraph(" ");

                    flowdoc.SetDefaultMargins(72.0, 72.0, 144.0, 228.0);
					flowdoc.SetDefaultPageSize(650, 750);

					flowdoc.AddParagraph(para_text);

					byte[] clr1 = new byte[] { 50, 50, 199 };
					byte[] clr2 = new byte[] { 30, 199, 30 };

					for (int i = 0; i < 50; i++)
					{
						para = flowdoc.AddParagraph();
						TextStyledElement st = para.GetTextStyledElement();

						int point_size = (17 * i * i * i) % 13 + 5;
						if (i % 2 == 0)
						{
							st.SetItalic(true);
							st.SetTextColor(clr1[0], clr1[1], clr1[2]);
							st.SetBackgroundColor(200, 200, 200);
							para.SetSpaceBefore(20);
							para.SetStartIndent(20);
							para.SetJustificationMode(Paragraph.TextJustification.e_left);
						}
						else
						{
							st.SetTextColor(clr2[0], clr2[1], clr2[2]);
							para.SetSpaceBefore(50);
							para.SetEndIndent(20);
							para.SetJustificationMode(Paragraph.TextJustification.e_right);
						}

						para.AddText(para_text);
						para.AddText(" " + para_text);
						st.SetFontSize(point_size);
					}

					// Table creation
					Table new_table = flowdoc.AddTable();
					new_table.SetDefaultColumnWidth(100);
					new_table.SetDefaultRowHeight(15);

					for (int i = 0; i < 4; i++)
					{
						TableRow new_row = new_table.AddTableRow();
						new_row.SetRowHeight(new_table.GetDefaultRowHeight() + i * 5);
						for (int j = 0; j < 5; j++)
						{
							TableCell cell = new_row.AddTableCell();
							cell.SetBorder(0.5, 255, 0, 0);

							if (i == 3)
							{
								if (j % 2 != 0)
								{
									cell.SetVerticalAlignment(TableCell.CellAlignmentVertical.e_center);
								}
								else
								{
									cell.SetVerticalAlignment(TableCell.CellAlignmentVertical.e_bottom);
								}
							}

							if ((i == 3) && (j == 4))
							{
								Paragraph para_title = cell.AddParagraph("Table title");
								para_title.SetJustificationMode(Paragraph.TextJustification.e_center);

								Table nested_table = cell.AddTable();
								nested_table.SetDefaultColumnWidth(33);
								nested_table.SetDefaultRowHeight(5);
								nested_table.SetBorder(0.5, 0, 0, 0);

								for (int nested_row_index = 0; nested_row_index < 3; nested_row_index++)
								{
									TableRow nested_row = nested_table.AddTableRow();
									for (int nested_column_index = 0; nested_column_index < 3; nested_column_index++)
									{
										string str = nested_row_index.ToString() + "/" + nested_column_index.ToString();
										TableCell nested_cell = nested_row.AddTableCell();
										nested_cell.SetBackgroundColor(200, 200, 255);
										nested_cell.SetBorder(0.1, 0, 255, 0);

										Paragraph new_para = nested_cell.AddParagraph(str);
										new_para.SetJustificationMode(Paragraph.TextJustification.e_right);
									}
								}
							}
							else
								if ((i == 2) && (j == 2))
								{
									para = cell.AddParagraph("Cell " + j.ToString() + " x " + i.ToString() + "\n");
									para.AddText("to be bold text 1\n");
									para.AddText("still normal text\n");
									para.AddText("to be bold text 2");
									cell.AddParagraph("Second Paragraph");
								}
								else
								{
									cell.AddParagraph("Cell " + j.ToString() + " x " + i.ToString());
								}
						}
					}

					// Walk the content tree and modify some text runs.
					ModifyContentTree(flowdoc.GetBody());


					// Merge cells
					TableCell merged_cell = new_table.GetTableCell(2, 0).MergeCellsRight(1);
					merged_cell.SetHorizontalAlignment(TableCell.CellAlignmentHorizontal.e_middle);

					new_table.GetTableCell(0, 0).MergeCellsDown(1)
						.SetVerticalAlignment(TableCell.CellAlignmentVertical.e_center);

					
					// Walk over the table and change the first cell in each row.
					int row = 0;
					byte[] clr_row1 = new byte[] { 175, 240, 240 };
					byte[] clr_row2 = new byte[] { 250, 250, 175 };

					for (ContentNodeIterator table_itr = new_table.GetContentNodeIterator();
							table_itr.HasNext();
							table_itr.Next())
					{
						TableRow table_row = table_itr.Current().AsTableRow();
						if (table_row != null)
						{
							for (ContentNodeIterator row_itr = table_row.GetContentNodeIterator();
									row_itr.HasNext();
									row_itr.Next())
							{
								TableCell table_cell = row_itr.Current().AsTableCell();
								if (table_cell != null)
								{
									if (row % 2 != 0)
									{
										table_cell.SetBackgroundColor(clr_row1[0], clr_row1[1], clr_row1[2]);
									}
									else
									{
										table_cell.SetBackgroundColor(clr_row2[0], clr_row2[1], clr_row2[2]);
									}

									for (ContentNodeIterator cell_itr = table_cell.GetContentNodeIterator();
											cell_itr.HasNext();
											cell_itr.Next())
									{
										Paragraph cell_para = cell_itr.Current().AsParagraph();
										if (cell_para != null)
										{
											TextStyledElement st = cell_para.GetTextStyledElement();
											st.SetTextColor(255, 0, 0);
											st.SetFontSize(12);
										}
										else
										{
											Table nested_table = cell_itr.Current().AsTable();
											if (nested_table != null)
											{
												TableCell nested_cell = nested_table.GetTableCell(1, 1);
												nested_cell.SetBackgroundColor(255, 127, 127);
											}
										}
									}
								}
							}
						}

						++row;
					}

					PDFDoc my_pdf = flowdoc.PaginateToPDF();
					my_pdf.Save(output_path + "created_doc.pdf", SDFDoc.SaveOptions.e_linearized);
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
