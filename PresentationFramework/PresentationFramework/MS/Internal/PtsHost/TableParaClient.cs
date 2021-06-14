using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.PtsTable;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200064A RID: 1610
	internal sealed class TableParaClient : BaseParaClient
	{
		// Token: 0x06006A9F RID: 27295 RVA: 0x001CF362 File Offset: 0x001CD562
		internal TableParaClient(TableParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x06006AA0 RID: 27296 RVA: 0x001E681C File Offset: 0x001E4A1C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		protected override void OnArrange()
		{
			base.OnArrange();
			this._columnRect = base.Paragraph.StructuralCache.CurrentArrangeContext.ColumnRect;
			CalculatedColumn[] calculatedColumns = this.CalculatedColumns;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT rect;
			if (!this.QueryTableDetails(out array, out fskupdate, out rect) || fskupdate == PTS.FSKUPDATE.fskupdNoChange || fskupdate == PTS.FSKUPDATE.fskupdShifted)
			{
				return;
			}
			this._rect = rect;
			this.UpdateChunkInfo(array);
			MbpInfo mbpInfo = MbpInfo.FromElement(this.TableParagraph.Element, this.TableParagraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ParentFlowDirection != base.PageFlowDirection)
			{
				PTS.FSRECT pageRect = this._pageContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.ParentFlowDirection), ref pageRect, ref this._rect, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out this._rect));
				mbpInfo.MirrorMargin();
			}
			this._rect.u = this._rect.u + mbpInfo.MarginLeft;
			this._rect.du = this._rect.du - (mbpInfo.MarginLeft + mbpInfo.MarginRight);
			int num = this.GetTableOffsetFirstRowTop() + TextDpi.ToTextDpi(this.Table.InternalCellSpacing) / 2;
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSKUPDATE fskupdate2 = (array[i].fsupdinf.fskupd != PTS.FSKUPDATE.fskupdInherited) ? array[i].fsupdinf.fskupd : fskupdate;
				if (fskupdate2 == PTS.FSKUPDATE.fskupdNoChange)
				{
					num += array[i].u.dvrRow;
				}
				else
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					for (int j = 0; j < array2.Length; j++)
					{
						if (!(array2[j] == IntPtr.Zero) && (i == 0 || (array4[j] != PTS.FSTABLEKCELLMERGE.fskcellmergeMiddle && array4[j] != PTS.FSTABLEKCELLMERGE.fskcellmergeLast)))
						{
							CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							double urOffset = calculatedColumns[cellParaClient.ColumnIndex].UrOffset;
							cellParaClient.Arrange(TextDpi.ToTextDpi(urOffset), num, this._rect, base.ThisFlowDirection, this._pageContext);
						}
					}
					num += array[i].u.dvrRow;
					if (i == 0 && this.IsFirstChunk)
					{
						num -= mbpInfo.BPTop;
					}
				}
			}
		}

		// Token: 0x06006AA1 RID: 27297 RVA: 0x001E6A70 File Offset: 0x001E4C70
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
			Invariant.Assert(fskupdInherited > PTS.FSKUPDATE.fskupdInherited);
			Invariant.Assert(this.TableParagraph.Table != null && this.CalculatedColumns != null);
			Table table = this.TableParagraph.Table;
			this.Visual.Clip = new RectangleGeometry(this._columnRect.FromTextDpi());
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (!this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				this._visual.Children.Clear();
				return;
			}
			MbpInfo mbpInfo = MbpInfo.FromElement(this.TableParagraph.Element, this.TableParagraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			if (fskupdate == PTS.FSKUPDATE.fskupdInherited)
			{
				fskupdate = fskupdInherited;
			}
			if (fskupdate == PTS.FSKUPDATE.fskupdNoChange)
			{
				return;
			}
			if (fskupdate == PTS.FSKUPDATE.fskupdShifted)
			{
				fskupdate = PTS.FSKUPDATE.fskupdNew;
			}
			VisualCollection children = this._visual.Children;
			if (fskupdate == PTS.FSKUPDATE.fskupdNew)
			{
				children.Clear();
			}
			Brush backgroundBrush = (Brush)base.Paragraph.Element.GetValue(TextElement.BackgroundProperty);
			using (DrawingContext drawingContext = this._visual.RenderOpen())
			{
				Rect tableContentRect = this.GetTableContentRect(mbpInfo).FromTextDpi();
				this._visual.DrawBackgroundAndBorderIntoContext(drawingContext, backgroundBrush, mbpInfo.BorderBrush, mbpInfo.Border, this._rect.FromTextDpi(), this.IsFirstChunk, this.IsLastChunk);
				this.DrawColumnBackgrounds(drawingContext, tableContentRect);
				this.DrawRowGroupBackgrounds(drawingContext, array, tableContentRect, mbpInfo);
				this.DrawRowBackgrounds(drawingContext, array, tableContentRect, mbpInfo);
			}
			TableRow tableRow = null;
			for (int i = 0; i < array.Length; i++)
			{
				PTS.FSKUPDATE fskupdate2 = (array[i].fsupdinf.fskupd != PTS.FSKUPDATE.fskupdInherited) ? array[i].fsupdinf.fskupd : fskupdate;
				RowParagraph rowParagraph = (RowParagraph)base.PtsContext.HandleToObject(array[i].fsnmRow);
				TableRow row = rowParagraph.Row;
				if (fskupdate2 == PTS.FSKUPDATE.fskupdNew)
				{
					RowVisual visual = new RowVisual(row);
					children.Insert(i, visual);
				}
				else
				{
					this.SynchronizeRowVisualsCollection(children, i, row);
				}
				Invariant.Assert(((RowVisual)children[i]).Row == row);
				if (fskupdate2 == PTS.FSKUPDATE.fskupdNew || fskupdate2 == PTS.FSKUPDATE.fskupdChangeInside)
				{
					if (rowParagraph.Row.HasForeignCells && (tableRow == null || tableRow.RowGroup != row.RowGroup))
					{
						this.ValidateRowVisualComplex((RowVisual)children[i], array[i].pfstablerow, this.CalculatedColumns.Length, fskupdate2, this.CalculatedColumns);
					}
					else
					{
						this.ValidateRowVisualSimple((RowVisual)children[i], array[i].pfstablerow, fskupdate2, this.CalculatedColumns);
					}
				}
				tableRow = row;
			}
			if (children.Count > array.Length)
			{
				children.RemoveRange(array.Length, children.Count - array.Length);
			}
		}

		// Token: 0x06006AA2 RID: 27298 RVA: 0x001E6D60 File Offset: 0x001E4F60
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void UpdateViewport(ref PTS.FSRECT viewport)
		{
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				for (int i = 0; i < array.Length; i++)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					for (int j = 0; j < array2.Length; j++)
					{
						if (!(array2[j] == IntPtr.Zero))
						{
							CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							cellParaClient.UpdateViewport(ref viewport);
						}
					}
				}
			}
		}

		// Token: 0x06006AA3 RID: 27299 RVA: 0x001E6DE4 File Offset: 0x001E4FE4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override IInputElement InputHitTest(PTS.FSPOINT pt)
		{
			IInputElement inputElement = null;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				int num = this.GetTableOffsetFirstRowTop() + fsrect.v;
				for (int i = 0; i < array.Length; i++)
				{
					if (pt.v >= num && pt.v <= num + array[i].u.dvrRow)
					{
						IntPtr[] array2;
						PTS.FSKUPDATE[] array3;
						PTS.FSTABLEKCELLMERGE[] array4;
						this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
						for (int j = 0; j < array2.Length; j++)
						{
							if (!(array2[j] == IntPtr.Zero))
							{
								CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
								PTS.FSRECT rect = cellParaClient.Rect;
								if (cellParaClient.Rect.Contains(pt))
								{
									inputElement = cellParaClient.InputHitTest(pt);
									break;
								}
							}
						}
						break;
					}
					num += array[i].u.dvrRow;
				}
			}
			if (inputElement == null && this._rect.Contains(pt))
			{
				inputElement = this.TableParagraph.Table;
			}
			return inputElement;
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x001E6F08 File Offset: 0x001E5108
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override List<Rect> GetRectangles(ContentElement e, int start, int length)
		{
			List<Rect> list = new List<Rect>();
			if (this.TableParagraph.Table == e)
			{
				this.GetRectanglesForParagraphElement(out list);
			}
			else
			{
				list = new List<Rect>();
				PTS.FSTABLEROWDESCRIPTION[] array;
				PTS.FSKUPDATE fskupdate;
				PTS.FSRECT fsrect;
				if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
				{
					for (int i = 0; i < array.Length; i++)
					{
						IntPtr[] array2;
						PTS.FSKUPDATE[] array3;
						PTS.FSTABLEKCELLMERGE[] array4;
						this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
						for (int j = 0; j < array2.Length; j++)
						{
							if (!(array2[j] == IntPtr.Zero))
							{
								CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
								if (start < cellParaClient.Paragraph.ParagraphEndCharacterPosition)
								{
									list = cellParaClient.GetRectangles(e, start, length);
									Invariant.Assert(list != null);
									if (list.Count != 0)
									{
										break;
									}
								}
							}
						}
						if (list.Count != 0)
						{
							break;
						}
					}
				}
			}
			Invariant.Assert(list != null);
			return list;
		}

		// Token: 0x06006AA5 RID: 27301 RVA: 0x001E6FF6 File Offset: 0x001E51F6
		internal override ParagraphResult CreateParagraphResult()
		{
			return new TableParagraphResult(this);
		}

		// Token: 0x06006AA6 RID: 27302 RVA: 0x001E7000 File Offset: 0x001E5200
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override TextContentRange GetTextContentRange()
		{
			TextContentRange textContentRange = null;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				textContentRange = new TextContentRange();
				this.UpdateChunkInfo(array);
				TextElement textElement = base.Paragraph.Element as TextElement;
				if (this._isFirstChunk)
				{
					textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(textElement, ElementEdge.BeforeStart));
				}
				for (int i = 0; i < array.Length; i++)
				{
					TableRow row = ((RowParagraph)base.PtsContext.HandleToObject(array[i].fsnmRow)).Row;
					PTS.FSTABLEROWDETAILS fstablerowdetails;
					PTS.Validate(PTS.FsQueryTableObjRowDetails(base.PtsContext.Context, array[i].pfstablerow, out fstablerowdetails));
					if (fstablerowdetails.fskboundaryAbove != PTS.FSKTABLEROWBOUNDARY.fsktablerowboundaryBreak)
					{
						if (row.Index == 0)
						{
							textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(row.RowGroup, ElementEdge.BeforeStart));
						}
						textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(row, ElementEdge.BeforeStart));
					}
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					for (int j = 0; j < array2.Length; j++)
					{
						if (!(array2[j] == IntPtr.Zero))
						{
							CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							textContentRange.Merge(cellParaClient.GetTextContentRange());
						}
					}
					if (fstablerowdetails.fskboundaryBelow != PTS.FSKTABLEROWBOUNDARY.fsktablerowboundaryBreak)
					{
						textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(row, ElementEdge.AfterEnd));
						if (row.Index == row.RowGroup.Rows.Count - 1)
						{
							textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(row.RowGroup, ElementEdge.AfterEnd));
						}
					}
				}
				if (this._isLastChunk)
				{
					textContentRange.Merge(TextContainerHelper.GetTextContentRangeForTextElementEdge(textElement, ElementEdge.AfterEnd));
				}
			}
			if (textContentRange == null)
			{
				textContentRange = TextContainerHelper.GetTextContentRangeForTextElement(this.TableParagraph.Table);
			}
			return textContentRange;
		}

		// Token: 0x06006AA7 RID: 27303 RVA: 0x001E71BC File Offset: 0x001E53BC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal CellParaClient GetCellParaClientFromPoint(Point point, bool snapToText)
		{
			int num = TextDpi.ToTextDpi(point.X);
			int num2 = TextDpi.ToTextDpi(point.Y);
			CellParaClient cellParaClient = null;
			CellParaClient cellParaClient2 = null;
			int num3 = int.MaxValue;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				int num4 = 0;
				while (num4 < array.Length && cellParaClient == null)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[num4].pfstablerow, out array2, out array3, out array4);
					int num5 = 0;
					while (num5 < array2.Length && cellParaClient == null)
					{
						if (!(array2[num5] == IntPtr.Zero))
						{
							CellParaClient cellParaClient3 = (CellParaClient)base.PtsContext.HandleToObject(array2[num5]);
							PTS.FSRECT rect = cellParaClient3.Rect;
							if (num >= rect.u && num <= rect.u + rect.du && num2 >= rect.v && num2 <= rect.v + rect.dv)
							{
								cellParaClient = cellParaClient3;
							}
							else if (snapToText)
							{
								int num6 = Math.Min(Math.Abs(rect.u - num), Math.Abs(rect.u + rect.du - num));
								int num7 = Math.Min(Math.Abs(rect.v - num2), Math.Abs(rect.v + rect.dv - num2));
								if (num6 + num7 < num3)
								{
									num3 = num6 + num7;
									cellParaClient2 = cellParaClient3;
								}
							}
						}
						num5++;
					}
					num4++;
				}
			}
			if (snapToText && cellParaClient == null)
			{
				cellParaClient = cellParaClient2;
			}
			return cellParaClient;
		}

		// Token: 0x06006AA8 RID: 27304 RVA: 0x001E7344 File Offset: 0x001E5544
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal ReadOnlyCollection<ParagraphResult> GetChildrenParagraphResults(out bool hasTextContent)
		{
			MbpInfo mbpInfo = MbpInfo.FromElement(this.TableParagraph.Element, this.TableParagraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			Rect rect = this.GetTableContentRect(mbpInfo).FromTextDpi();
			double num = rect.Y;
			Rect rowRect = rect;
			hasTextContent = false;
			List<ParagraphResult> list = new List<ParagraphResult>(0);
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				for (int i = 0; i < array.Length; i++)
				{
					RowParagraph rowParagraph = (RowParagraph)base.PtsContext.HandleToObject(array[i].fsnmRow);
					rowRect.Y = num;
					rowRect.Height = this.GetActualRowHeight(array, i, mbpInfo);
					RowParagraphResult rowParagraphResult = new RowParagraphResult(this, i, rowRect, rowParagraph);
					if (rowParagraphResult.HasTextContent)
					{
						hasTextContent = true;
					}
					list.Add(rowParagraphResult);
					num += rowRect.Height;
				}
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06006AA9 RID: 27305 RVA: 0x001E7440 File Offset: 0x001E5640
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal ReadOnlyCollection<ParagraphResult> GetChildrenParagraphResultsForRow(int rowIndex, out bool hasTextContent)
		{
			List<ParagraphResult> list = new List<ParagraphResult>(0);
			hasTextContent = false;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				IntPtr[] array2;
				PTS.FSKUPDATE[] array3;
				PTS.FSTABLEKCELLMERGE[] array4;
				this.QueryRowDetails(array[rowIndex].pfstablerow, out array2, out array3, out array4);
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i] != IntPtr.Zero && (rowIndex == 0 || (array4[i] != PTS.FSTABLEKCELLMERGE.fskcellmergeMiddle && array4[i] != PTS.FSTABLEKCELLMERGE.fskcellmergeLast)))
					{
						CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[i]);
						ParagraphResult paragraphResult = cellParaClient.CreateParagraphResult();
						if (paragraphResult.HasTextContent)
						{
							hasTextContent = true;
						}
						list.Add(paragraphResult);
					}
				}
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06006AAA RID: 27306 RVA: 0x001E74F4 File Offset: 0x001E56F4
		internal ReadOnlyCollection<ParagraphResult> GetParagraphsFromPoint(Point point, bool snapToText)
		{
			CellParaClient cellParaClientFromPoint = this.GetCellParaClientFromPoint(point, snapToText);
			List<ParagraphResult> list = new List<ParagraphResult>(0);
			if (cellParaClientFromPoint != null)
			{
				list.Add(cellParaClientFromPoint.CreateParagraphResult());
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06006AAB RID: 27307 RVA: 0x001E7528 File Offset: 0x001E5728
		internal ReadOnlyCollection<ParagraphResult> GetParagraphsFromPosition(ITextPointer position)
		{
			CellParaClient cellParaClientFromPosition = this.GetCellParaClientFromPosition(position);
			List<ParagraphResult> list = new List<ParagraphResult>(0);
			if (cellParaClientFromPosition != null)
			{
				list.Add(cellParaClientFromPosition.CreateParagraphResult());
			}
			return new ReadOnlyCollection<ParagraphResult>(list);
		}

		// Token: 0x06006AAC RID: 27308 RVA: 0x001E755C File Offset: 0x001E575C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			Geometry geometry = null;
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				bool flag = false;
				int num = 0;
				while (num < array.Length && !flag)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[num].pfstablerow, out array2, out array3, out array4);
					for (int i = 0; i < array2.Length; i++)
					{
						if (!(array2[i] == IntPtr.Zero))
						{
							CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[i]);
							if (endPosition.CompareTo(cellParaClient.Cell.ContentStart) <= 0)
							{
								flag = true;
							}
							else if (startPosition.CompareTo(cellParaClient.Cell.ContentEnd) <= 0)
							{
								Geometry tightBoundingGeometryFromTextPositions = cellParaClient.GetTightBoundingGeometryFromTextPositions(startPosition, endPosition, visibleRect);
								CaretElement.AddGeometry(ref geometry, tightBoundingGeometryFromTextPositions);
							}
						}
					}
					num++;
				}
			}
			if (geometry != null)
			{
				geometry = Geometry.Combine(geometry, this.Visual.Clip, GeometryCombineMode.Intersect, null);
			}
			return geometry;
		}

		// Token: 0x06006AAD RID: 27309 RVA: 0x001E764C File Offset: 0x001E584C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal CellParaClient GetCellParaClientFromPosition(ITextPointer position)
		{
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				for (int i = 0; i < array.Length; i++)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					for (int j = 0; j < array2.Length; j++)
					{
						if (!(array2[j] == IntPtr.Zero))
						{
							CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							if (position.CompareTo(cellParaClient.Cell.ContentStart) >= 0 && position.CompareTo(cellParaClient.Cell.ContentEnd) <= 0)
							{
								return cellParaClient;
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06006AAE RID: 27310 RVA: 0x001E7700 File Offset: 0x001E5900
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal CellParaClient GetCellAbove(double suggestedX, int rowGroupIndex, int rowIndex)
		{
			int num = TextDpi.ToTextDpi(suggestedX);
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				MbpInfo mbpInfo = MbpInfo.FromElement(this.TableParagraph.Element, this.TableParagraph.StructuralCache.TextFormatterHost.PixelsPerDip);
				for (int i = array.Length - 1; i >= 0; i--)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					CellParaClient cellParaClient = null;
					int num2 = int.MaxValue;
					for (int j = array2.Length - 1; j >= 0; j--)
					{
						if (!(array2[j] == IntPtr.Zero))
						{
							CellParaClient cellParaClient2 = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							int num3 = cellParaClient2.Cell.RowIndex + cellParaClient2.Cell.RowSpan - 1;
							if ((num3 < rowIndex && cellParaClient2.Cell.RowGroupIndex == rowGroupIndex) || cellParaClient2.Cell.RowGroupIndex < rowGroupIndex)
							{
								if (num >= cellParaClient2.Rect.u && num <= cellParaClient2.Rect.u + cellParaClient2.Rect.du)
								{
									return cellParaClient2;
								}
								int num4 = Math.Abs(cellParaClient2.Rect.u + cellParaClient2.Rect.du / 2 - num);
								if (num4 < num2)
								{
									num2 = num4;
									cellParaClient = cellParaClient2;
								}
							}
						}
					}
					if (cellParaClient != null)
					{
						return cellParaClient;
					}
				}
			}
			return null;
		}

		// Token: 0x06006AAF RID: 27311 RVA: 0x001E7878 File Offset: 0x001E5A78
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal CellParaClient GetCellBelow(double suggestedX, int rowGroupIndex, int rowIndex)
		{
			int num = TextDpi.ToTextDpi(suggestedX);
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				for (int i = 0; i < array.Length; i++)
				{
					IntPtr[] array2;
					PTS.FSKUPDATE[] array3;
					PTS.FSTABLEKCELLMERGE[] array4;
					this.QueryRowDetails(array[i].pfstablerow, out array2, out array3, out array4);
					CellParaClient cellParaClient = null;
					int num2 = int.MaxValue;
					for (int j = array2.Length - 1; j >= 0; j--)
					{
						if (!(array2[j] == IntPtr.Zero))
						{
							CellParaClient cellParaClient2 = (CellParaClient)base.PtsContext.HandleToObject(array2[j]);
							if ((cellParaClient2.Cell.RowIndex > rowIndex && cellParaClient2.Cell.RowGroupIndex == rowGroupIndex) || cellParaClient2.Cell.RowGroupIndex > rowGroupIndex)
							{
								if (num >= cellParaClient2.Rect.u && num <= cellParaClient2.Rect.u + cellParaClient2.Rect.du)
								{
									return cellParaClient2;
								}
								int num3 = Math.Abs(cellParaClient2.Rect.u + cellParaClient2.Rect.du / 2 - num);
								if (num3 < num2)
								{
									num2 = num3;
									cellParaClient = cellParaClient2;
								}
							}
						}
					}
					if (cellParaClient != null)
					{
						return cellParaClient;
					}
				}
			}
			return null;
		}

		// Token: 0x06006AB0 RID: 27312 RVA: 0x001E79B4 File Offset: 0x001E5BB4
		internal CellInfo GetCellInfoFromPoint(Point point)
		{
			CellParaClient cellParaClientFromPoint = this.GetCellParaClientFromPoint(point, true);
			if (cellParaClientFromPoint != null)
			{
				return new CellInfo(this, cellParaClientFromPoint);
			}
			return null;
		}

		// Token: 0x06006AB1 RID: 27313 RVA: 0x001E79D8 File Offset: 0x001E5BD8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal Rect GetRectangleFromRowEndPosition(ITextPointer position)
		{
			PTS.FSTABLEROWDESCRIPTION[] array;
			PTS.FSKUPDATE fskupdate;
			PTS.FSRECT fsrect;
			if (this.QueryTableDetails(out array, out fskupdate, out fsrect))
			{
				int num = this.GetTableOffsetFirstRowTop() + fsrect.v;
				for (int i = 0; i < array.Length; i++)
				{
					TableRow row = ((RowParagraph)base.PtsContext.HandleToObject(array[i].fsnmRow)).Row;
					if (((TextPointer)position).CompareTo(row.ContentEnd) == 0)
					{
						return new Rect(TextDpi.FromTextDpi(fsrect.u + fsrect.du), TextDpi.FromTextDpi(num), 1.0, TextDpi.FromTextDpi(array[i].u.dvrRow));
					}
					num += array[i].u.dvrRow;
				}
			}
			return System.Windows.Rect.Empty;
		}

		// Token: 0x06006AB2 RID: 27314 RVA: 0x001E7AAC File Offset: 0x001E5CAC
		internal void AutofitTable(uint fswdirTrack, int durAvailableSpace, out int durTableWidth)
		{
			double availableWidth = TextDpi.FromTextDpi(durAvailableSpace);
			double d;
			this.Autofit(availableWidth, out d);
			durTableWidth = TextDpi.ToTextDpi(d);
		}

		// Token: 0x06006AB3 RID: 27315 RVA: 0x001E7AD4 File Offset: 0x001E5CD4
		internal void UpdAutofitTable(uint fswdirTrack, int durAvailableSpace, out int durTableWidth, out int fNoChangeInCellWidths)
		{
			double availableWidth = TextDpi.FromTextDpi(durAvailableSpace);
			double d;
			fNoChangeInCellWidths = this.Autofit(availableWidth, out d);
			durTableWidth = TextDpi.ToTextDpi(d);
		}

		// Token: 0x06006AB4 RID: 27316 RVA: 0x001E7AFC File Offset: 0x001E5CFC
		internal int Autofit(double availableWidth, out double tableWidth)
		{
			int result = 1;
			this.ValidateCalculatedColumns();
			if (!DoubleUtil.AreClose(availableWidth, this._previousAutofitWidth))
			{
				result = this.ValidateTableWidths(availableWidth, out tableWidth);
			}
			else
			{
				tableWidth = this._previousTableWidth;
			}
			this._previousAutofitWidth = availableWidth;
			this._previousTableWidth = tableWidth;
			return result;
		}

		// Token: 0x170019A5 RID: 6565
		// (get) Token: 0x06006AB5 RID: 27317 RVA: 0x001E7B42 File Offset: 0x001E5D42
		internal TableParagraph TableParagraph
		{
			get
			{
				return (TableParagraph)this._paragraph;
			}
		}

		// Token: 0x170019A6 RID: 6566
		// (get) Token: 0x06006AB6 RID: 27318 RVA: 0x001E7B4F File Offset: 0x001E5D4F
		internal Table Table
		{
			get
			{
				return this.TableParagraph.Table;
			}
		}

		// Token: 0x170019A7 RID: 6567
		// (get) Token: 0x06006AB7 RID: 27319 RVA: 0x001E7B5C File Offset: 0x001E5D5C
		internal double TableDesiredWidth
		{
			get
			{
				double num = 0.0;
				CalculatedColumn[] calculatedColumns = this.CalculatedColumns;
				for (int i = 0; i < calculatedColumns.Length; i++)
				{
					num += calculatedColumns[i].DurWidth + this.Table.InternalCellSpacing;
				}
				return num;
			}
		}

		// Token: 0x170019A8 RID: 6568
		// (get) Token: 0x06006AB8 RID: 27320 RVA: 0x001E7BA4 File Offset: 0x001E5DA4
		internal CalculatedColumn[] CalculatedColumns
		{
			get
			{
				return this._calculatedColumns;
			}
		}

		// Token: 0x170019A9 RID: 6569
		// (get) Token: 0x06006AB9 RID: 27321 RVA: 0x001E7BAC File Offset: 0x001E5DAC
		internal double AutofitWidth
		{
			get
			{
				return this._previousAutofitWidth;
			}
		}

		// Token: 0x170019AA RID: 6570
		// (get) Token: 0x06006ABA RID: 27322 RVA: 0x001E7BB4 File Offset: 0x001E5DB4
		internal override bool IsFirstChunk
		{
			get
			{
				return this._isFirstChunk;
			}
		}

		// Token: 0x170019AB RID: 6571
		// (get) Token: 0x06006ABB RID: 27323 RVA: 0x001E7BBC File Offset: 0x001E5DBC
		internal override bool IsLastChunk
		{
			get
			{
				return this._isLastChunk;
			}
		}

		// Token: 0x06006ABC RID: 27324 RVA: 0x001E7BC4 File Offset: 0x001E5DC4
		[SecurityCritical]
		private void UpdateChunkInfo(PTS.FSTABLEROWDESCRIPTION[] arrayTableRowDesc)
		{
			RowParagraph rowParagraph = (RowParagraph)base.PtsContext.HandleToObject(arrayTableRowDesc[0].fsnmRow);
			TableRow row = rowParagraph.Row;
			PTS.FSTABLEROWDETAILS fstablerowdetails;
			PTS.Validate(PTS.FsQueryTableObjRowDetails(base.PtsContext.Context, arrayTableRowDesc[0].pfstablerow, out fstablerowdetails));
			this._isFirstChunk = (fstablerowdetails.fskboundaryAbove == PTS.FSKTABLEROWBOUNDARY.fsktablerowboundaryOuter && row.Index == 0 && this.Table.IsFirstNonEmptyRowGroup(row.RowGroup.Index));
			row = ((RowParagraph)base.PtsContext.HandleToObject(arrayTableRowDesc[arrayTableRowDesc.Length - 1].fsnmRow)).Row;
			PTS.Validate(PTS.FsQueryTableObjRowDetails(base.PtsContext.Context, arrayTableRowDesc[arrayTableRowDesc.Length - 1].pfstablerow, out fstablerowdetails));
			this._isLastChunk = (fstablerowdetails.fskboundaryBelow == PTS.FSKTABLEROWBOUNDARY.fsktablerowboundaryOuter && row.Index == row.RowGroup.Rows.Count - 1 && this.Table.IsLastNonEmptyRowGroup(row.RowGroup.Index));
		}

		// Token: 0x06006ABD RID: 27325 RVA: 0x001E7CD8 File Offset: 0x001E5ED8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private unsafe bool QueryTableDetails(out PTS.FSTABLEROWDESCRIPTION[] arrayTableRowDesc, out PTS.FSKUPDATE fskupdTable, out PTS.FSRECT rect)
		{
			PTS.FSTABLEOBJDETAILS fstableobjdetails;
			PTS.Validate(PTS.FsQueryTableObjDetails(base.PtsContext.Context, this._paraHandle.Value, out fstableobjdetails));
			fskupdTable = fstableobjdetails.fskupdTableProper;
			rect = fstableobjdetails.fsrcTableObj;
			PTS.FSTABLEDETAILS fstabledetails;
			PTS.Validate(PTS.FsQueryTableObjTableProperDetails(base.PtsContext.Context, fstableobjdetails.pfstableProper, out fstabledetails));
			if (fstabledetails.cRows == 0)
			{
				arrayTableRowDesc = null;
				return false;
			}
			arrayTableRowDesc = new PTS.FSTABLEROWDESCRIPTION[fstabledetails.cRows];
			fixed (PTS.FSTABLEROWDESCRIPTION* ptr = arrayTableRowDesc)
			{
				int num;
				PTS.Validate(PTS.FsQueryTableObjRowList(base.PtsContext.Context, fstableobjdetails.pfstableProper, fstabledetails.cRows, ptr, out num));
			}
			return true;
		}

		// Token: 0x06006ABE RID: 27326 RVA: 0x001E7D94 File Offset: 0x001E5F94
		[SecurityCritical]
		private unsafe void QueryRowDetails(IntPtr pfstablerow, out IntPtr[] arrayFsCell, out PTS.FSKUPDATE[] arrayUpdate, out PTS.FSTABLEKCELLMERGE[] arrayTableCellMerge)
		{
			PTS.FSTABLEROWDETAILS fstablerowdetails;
			PTS.Validate(PTS.FsQueryTableObjRowDetails(base.PtsContext.Context, pfstablerow, out fstablerowdetails));
			arrayUpdate = new PTS.FSKUPDATE[fstablerowdetails.cCells];
			arrayFsCell = new IntPtr[fstablerowdetails.cCells];
			arrayTableCellMerge = new PTS.FSTABLEKCELLMERGE[fstablerowdetails.cCells];
			if (fstablerowdetails.cCells > 0)
			{
				fixed (PTS.FSKUPDATE* ptr = arrayUpdate)
				{
					fixed (IntPtr* ptr2 = arrayFsCell)
					{
						fixed (PTS.FSTABLEKCELLMERGE* ptr3 = arrayTableCellMerge)
						{
							int num;
							PTS.Validate(PTS.FsQueryTableObjCellList(base.PtsContext.Context, pfstablerow, fstablerowdetails.cCells, ptr, ptr2, ptr3, out num));
						}
					}
				}
			}
		}

		// Token: 0x06006ABF RID: 27327 RVA: 0x001E7E70 File Offset: 0x001E6070
		private void SynchronizeRowVisualsCollection(VisualCollection rowVisualsCollection, int firstIndex, TableRow row)
		{
			if (((RowVisual)rowVisualsCollection[firstIndex]).Row != row)
			{
				int num = firstIndex;
				int count = rowVisualsCollection.Count;
				while (++num < count)
				{
					RowVisual rowVisual = (RowVisual)rowVisualsCollection[num];
					if (rowVisual.Row == row)
					{
						break;
					}
				}
				rowVisualsCollection.RemoveRange(firstIndex, num - firstIndex);
			}
		}

		// Token: 0x06006AC0 RID: 27328 RVA: 0x001E7EC4 File Offset: 0x001E60C4
		private void SynchronizeCellVisualsCollection(VisualCollection cellVisualsCollection, int firstIndex, Visual visual)
		{
			if (cellVisualsCollection[firstIndex] != visual)
			{
				int num = firstIndex;
				int count = cellVisualsCollection.Count;
				while (++num < count && cellVisualsCollection[num] != visual)
				{
				}
				cellVisualsCollection.RemoveRange(firstIndex, num - firstIndex);
			}
		}

		// Token: 0x06006AC1 RID: 27329 RVA: 0x001E7F04 File Offset: 0x001E6104
		[SecurityCritical]
		private void ValidateRowVisualSimple(RowVisual rowVisual, IntPtr pfstablerow, PTS.FSKUPDATE fskupdRow, CalculatedColumn[] calculatedColumns)
		{
			IntPtr[] array;
			PTS.FSKUPDATE[] array2;
			PTS.FSTABLEKCELLMERGE[] array3;
			this.QueryRowDetails(pfstablerow, out array, out array2, out array3);
			VisualCollection children = rowVisual.Children;
			int num = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i] == IntPtr.Zero) && array3[i] != PTS.FSTABLEKCELLMERGE.fskcellmergeMiddle && array3[i] != PTS.FSTABLEKCELLMERGE.fskcellmergeLast)
				{
					PTS.FSKUPDATE fskupdate = (array2[i] != PTS.FSKUPDATE.fskupdInherited) ? array2[i] : fskupdRow;
					if (fskupdate != PTS.FSKUPDATE.fskupdNoChange)
					{
						CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array[i]);
						double urOffset = calculatedColumns[cellParaClient.ColumnIndex].UrOffset;
						cellParaClient.ValidateVisual();
						if (fskupdate == PTS.FSKUPDATE.fskupdNew || VisualTreeHelper.GetParent(cellParaClient.Visual) == null)
						{
							Visual visual = VisualTreeHelper.GetParent(cellParaClient.Visual) as Visual;
							if (visual != null)
							{
								ContainerVisual containerVisual = visual as ContainerVisual;
								Invariant.Assert(containerVisual != null, "parent should always derives from ContainerVisual");
								containerVisual.Children.Remove(cellParaClient.Visual);
							}
							children.Insert(num, cellParaClient.Visual);
						}
						else
						{
							this.SynchronizeCellVisualsCollection(children, num, cellParaClient.Visual);
						}
					}
					num++;
				}
			}
			if (children.Count > num)
			{
				children.RemoveRange(num, children.Count - num);
			}
		}

		// Token: 0x06006AC2 RID: 27330 RVA: 0x001E8048 File Offset: 0x001E6248
		[SecurityCritical]
		private void ValidateRowVisualComplex(RowVisual rowVisual, IntPtr pfstablerow, int tableColumnCount, PTS.FSKUPDATE fskupdRow, CalculatedColumn[] calculatedColumns)
		{
			IntPtr[] array;
			PTS.FSKUPDATE[] array2;
			PTS.FSTABLEKCELLMERGE[] array3;
			this.QueryRowDetails(pfstablerow, out array, out array2, out array3);
			TableParaClient.CellParaClientEntry[] array4 = new TableParaClient.CellParaClientEntry[tableColumnCount];
			for (int i = 0; i < array.Length; i++)
			{
				if (!(array[i] == IntPtr.Zero))
				{
					PTS.FSKUPDATE fskupdCell = (array2[i] != PTS.FSKUPDATE.fskupdInherited) ? array2[i] : fskupdRow;
					CellParaClient cellParaClient = (CellParaClient)base.PtsContext.HandleToObject(array[i]);
					int columnIndex = cellParaClient.ColumnIndex;
					array4[columnIndex].cellParaClient = cellParaClient;
					array4[columnIndex].fskupdCell = fskupdCell;
				}
			}
			VisualCollection children = rowVisual.Children;
			int num = 0;
			for (int j = 0; j < array4.Length; j++)
			{
				CellParaClient cellParaClient2 = array4[j].cellParaClient;
				if (cellParaClient2 != null)
				{
					PTS.FSKUPDATE fskupdCell2 = array4[j].fskupdCell;
					if (fskupdCell2 != PTS.FSKUPDATE.fskupdNoChange)
					{
						double urOffset = calculatedColumns[j].UrOffset;
						cellParaClient2.ValidateVisual();
						if (fskupdCell2 == PTS.FSKUPDATE.fskupdNew)
						{
							children.Insert(num, cellParaClient2.Visual);
						}
						else
						{
							this.SynchronizeCellVisualsCollection(children, num, cellParaClient2.Visual);
						}
					}
					num++;
				}
			}
			if (children.Count > num)
			{
				children.RemoveRange(num, children.Count - num);
			}
		}

		// Token: 0x06006AC3 RID: 27331 RVA: 0x001E8180 File Offset: 0x001E6380
		private void DrawColumnBackgrounds(DrawingContext dc, Rect tableContentRect)
		{
			double num = tableContentRect.X;
			Rect rectangle = tableContentRect;
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				for (int i = this.CalculatedColumns.Length - 1; i >= 0; i--)
				{
					Brush brush = (i < this.Table.Columns.Count) ? this.Table.Columns[i].Background : null;
					rectangle.Width = this.CalculatedColumns[i].DurWidth + this.Table.InternalCellSpacing;
					if (brush != null)
					{
						rectangle.X = num;
						dc.DrawRectangle(brush, null, rectangle);
					}
					num += rectangle.Width;
				}
				return;
			}
			for (int j = 0; j < this.CalculatedColumns.Length; j++)
			{
				Brush brush = (j < this.Table.Columns.Count) ? this.Table.Columns[j].Background : null;
				rectangle.Width = this.CalculatedColumns[j].DurWidth + this.Table.InternalCellSpacing;
				if (brush != null)
				{
					rectangle.X = num;
					dc.DrawRectangle(brush, null, rectangle);
				}
				num += rectangle.Width;
			}
		}

		// Token: 0x06006AC4 RID: 27332 RVA: 0x001E82BC File Offset: 0x001E64BC
		private double GetActualRowHeight(PTS.FSTABLEROWDESCRIPTION[] arrayTableRowDesc, int rowIndex, MbpInfo mbpInfo)
		{
			int num = 0;
			if (this.IsFirstChunk && rowIndex == 0)
			{
				num = -mbpInfo.BPTop;
			}
			if (this.IsLastChunk && rowIndex == arrayTableRowDesc.Length - 1)
			{
				num = -mbpInfo.BPBottom;
			}
			return TextDpi.FromTextDpi(arrayTableRowDesc[rowIndex].u.dvrRow + num);
		}

		// Token: 0x06006AC5 RID: 27333 RVA: 0x001E8310 File Offset: 0x001E6510
		[SecurityCritical]
		private void DrawRowGroupBackgrounds(DrawingContext dc, PTS.FSTABLEROWDESCRIPTION[] arrayTableRowDesc, Rect tableContentRect, MbpInfo mbpInfo)
		{
			double num = tableContentRect.Y;
			double num2 = 0.0;
			Rect rectangle = tableContentRect;
			if (arrayTableRowDesc.Length != 0)
			{
				TableRow row = ((RowParagraph)base.PtsContext.HandleToObject(arrayTableRowDesc[0].fsnmRow)).Row;
				TableRowGroup rowGroup = row.RowGroup;
				Brush brush;
				for (int i = 0; i < arrayTableRowDesc.Length; i++)
				{
					row = ((RowParagraph)base.PtsContext.HandleToObject(arrayTableRowDesc[i].fsnmRow)).Row;
					if (rowGroup != row.RowGroup)
					{
						brush = (Brush)rowGroup.GetValue(TextElement.BackgroundProperty);
						if (brush != null)
						{
							rectangle.Y = num;
							rectangle.Height = num2;
							dc.DrawRectangle(brush, null, rectangle);
						}
						num += num2;
						rowGroup = row.RowGroup;
						num2 = this.GetActualRowHeight(arrayTableRowDesc, i, mbpInfo);
					}
					else
					{
						num2 += this.GetActualRowHeight(arrayTableRowDesc, i, mbpInfo);
					}
				}
				brush = (Brush)rowGroup.GetValue(TextElement.BackgroundProperty);
				if (brush != null)
				{
					rectangle.Y = num;
					rectangle.Height = num2;
					dc.DrawRectangle(brush, null, rectangle);
				}
			}
		}

		// Token: 0x06006AC6 RID: 27334 RVA: 0x001E8430 File Offset: 0x001E6630
		[SecurityCritical]
		private void DrawRowBackgrounds(DrawingContext dc, PTS.FSTABLEROWDESCRIPTION[] arrayTableRowDesc, Rect tableContentRect, MbpInfo mbpInfo)
		{
			double num = tableContentRect.Y;
			Rect rectangle = tableContentRect;
			for (int i = 0; i < arrayTableRowDesc.Length; i++)
			{
				TableRow row = ((RowParagraph)base.PtsContext.HandleToObject(arrayTableRowDesc[i].fsnmRow)).Row;
				Brush brush = (Brush)row.GetValue(TextElement.BackgroundProperty);
				rectangle.Y = num;
				rectangle.Height = this.GetActualRowHeight(arrayTableRowDesc, i, mbpInfo);
				if (brush != null)
				{
					dc.DrawRectangle(brush, null, rectangle);
				}
				num += rectangle.Height;
			}
		}

		// Token: 0x06006AC7 RID: 27335 RVA: 0x001E84BC File Offset: 0x001E66BC
		private void ValidateCalculatedColumns()
		{
			int columnCount = this.Table.ColumnCount;
			if (this._calculatedColumns == null)
			{
				this._calculatedColumns = new CalculatedColumn[columnCount];
			}
			else if (this._calculatedColumns.Length != columnCount)
			{
				CalculatedColumn[] array = new CalculatedColumn[columnCount];
				Array.Copy(this._calculatedColumns, array, Math.Min(this._calculatedColumns.Length, columnCount));
				this._calculatedColumns = array;
			}
			if (this._calculatedColumns.Length != 0)
			{
				int i;
				for (i = 0; i < this._calculatedColumns.Length; i++)
				{
					if (i >= this.Table.Columns.Count)
					{
						break;
					}
					this._calculatedColumns[i].UserWidth = this.Table.Columns[i].Width;
				}
				while (i < this._calculatedColumns.Length)
				{
					this._calculatedColumns[i].UserWidth = TableColumn.DefaultWidth;
					i++;
				}
			}
			this._durMinWidth = (this._durMaxWidth = 0.0);
			for (int j = 0; j < this._calculatedColumns.Length; j++)
			{
				switch (this._calculatedColumns[j].UserWidth.GridUnitType)
				{
				case GridUnitType.Auto:
					this._calculatedColumns[j].ValidateAuto(1.0, 1000000.0);
					break;
				case GridUnitType.Pixel:
					this._calculatedColumns[j].ValidateAuto(this._calculatedColumns[j].UserWidth.Value, this._calculatedColumns[j].UserWidth.Value);
					break;
				case GridUnitType.Star:
					this._calculatedColumns[j].ValidateAuto(1.0, 1000000.0);
					break;
				}
				this._durMinWidth += this._calculatedColumns[j].DurMinWidth;
				this._durMaxWidth += this._calculatedColumns[j].DurMaxWidth;
			}
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			double num = this.Table.InternalCellSpacing * (double)this.Table.ColumnCount + mbpInfo.Margin.Left + mbpInfo.Border.Left + mbpInfo.Padding.Left + mbpInfo.Padding.Right + mbpInfo.Border.Right + mbpInfo.Margin.Right;
			this._durMinWidth += num;
			this._durMaxWidth += num;
		}

		// Token: 0x06006AC8 RID: 27336 RVA: 0x001E87A0 File Offset: 0x001E69A0
		private int ValidateTableWidths(double durAvailableWidth, out double durTableWidth)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			double internalCellSpacing = this.Table.InternalCellSpacing;
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			double num = internalCellSpacing * (double)this.Table.ColumnCount + TextDpi.FromTextDpi(mbpInfo.MBPLeft + mbpInfo.MBPRight);
			int result = 1;
			durTableWidth = 0.0;
			double num2 = 0.0;
			double num4;
			double num3 = num4 = 0.0;
			double num5 = 0.0;
			double num7;
			double num6 = num7 = 0.0;
			double num8 = 0.0;
			double num9 = 1.0;
			double num10;
			for (int i = 0; i < this._calculatedColumns.Length; i++)
			{
				if (this._calculatedColumns[i].UserWidth.IsAuto)
				{
					num7 += this._calculatedColumns[i].DurMinWidth;
					num6 += this._calculatedColumns[i].DurMaxWidth;
				}
				else if (this._calculatedColumns[i].UserWidth.IsStar)
				{
					num10 = this._calculatedColumns[i].UserWidth.Value;
					if (num10 < 0.0)
					{
						num10 = 0.0;
					}
					if (num2 + num10 > 100.0)
					{
						num10 = 100.0 - num2;
						num2 = 100.0;
						this._calculatedColumns[i].UserWidth = new GridLength(num10, GridUnitType.Star);
					}
					else
					{
						num2 += num10;
					}
					if (num10 == 0.0)
					{
						num10 = 1.0;
					}
					if (this._calculatedColumns[i].DurMaxWidth * num9 > num10 * num8)
					{
						num8 = this._calculatedColumns[i].DurMaxWidth;
						num9 = num10;
					}
					num5 += this._calculatedColumns[i].DurMinWidth;
				}
				else
				{
					num4 += this._calculatedColumns[i].DurMinWidth;
					num3 += this._calculatedColumns[i].DurMaxWidth;
				}
			}
			num10 = 100.0 - num2;
			double num11;
			if (flag)
			{
				num11 = durAvailableWidth;
				if (num11 < this._durMinWidth && !DoubleUtil.AreClose(num11, this._durMinWidth))
				{
					num11 = this._durMinWidth;
				}
			}
			else if (0.0 < num2)
			{
				if (0.0 < num10)
				{
					double num12 = (num3 + num6) * num9;
					double num13 = num10 * num8;
					if (num12 > num13 && !DoubleUtil.AreClose(num12, num13))
					{
						num8 = num3 + num6;
						num9 = num10;
					}
				}
				if (0.0 < num10 || DoubleUtil.IsZero(num3 + num6))
				{
					num11 = num8 * 100.0 / num9 + num;
					if (num11 > durAvailableWidth && !DoubleUtil.AreClose(num11, durAvailableWidth))
					{
						num11 = durAvailableWidth;
					}
				}
				else
				{
					num11 = durAvailableWidth;
				}
				if (num11 < this._durMinWidth && !DoubleUtil.AreClose(num11, this._durMinWidth))
				{
					num11 = this._durMinWidth;
				}
			}
			else if (this._durMaxWidth < durAvailableWidth && !DoubleUtil.AreClose(this._durMaxWidth, durAvailableWidth))
			{
				num11 = this._durMaxWidth;
			}
			else if (this._durMinWidth > durAvailableWidth && !DoubleUtil.AreClose(this._durMinWidth, durAvailableWidth))
			{
				num11 = this._durMinWidth;
			}
			else
			{
				num11 = durAvailableWidth;
			}
			if (num11 > num || DoubleUtil.AreClose(num11, num))
			{
				num11 -= num;
			}
			double num14;
			if (0.0 < num6 + num3 && !DoubleUtil.IsZero(num6 + num3))
			{
				num14 = num10 * num11 / 100.0;
				if (num14 < num4 + num7 && !DoubleUtil.AreClose(num14, num4 + num7))
				{
					num14 = num4 + num7;
				}
				if (num14 > num11 - num5 && !DoubleUtil.AreClose(num14, num11 - num5))
				{
					num14 = num11 - num5;
				}
			}
			else
			{
				num14 = 0.0;
			}
			double num15;
			double num16;
			if (0.0 < num3 && !DoubleUtil.IsZero(num3))
			{
				num15 = num3;
				if (num15 > num14 && !DoubleUtil.AreClose(num15, num14))
				{
					num15 = num14;
				}
				if (0.0 < num6 && !DoubleUtil.IsZero(num6))
				{
					num16 = num7;
					if (num15 + num16 < num14 || DoubleUtil.AreClose(num15 + num16, num14))
					{
						num16 = num14 - num15;
					}
					else
					{
						num15 = num4;
						if (num15 + num16 < num14 || DoubleUtil.AreClose(num15 + num16, num14))
						{
							num15 = num14 - num16;
						}
					}
				}
				else
				{
					num16 = 0.0;
					if (num15 < num14 && !DoubleUtil.AreClose(num15, num14))
					{
						num15 = num14;
					}
				}
			}
			else
			{
				num15 = 0.0;
				if (0.0 < num6 && !DoubleUtil.IsZero(num6))
				{
					num16 = num7;
					if (num16 < num14 && !DoubleUtil.AreClose(num16, num14))
					{
						num16 = num14;
					}
				}
				else
				{
					num16 = 0.0;
				}
			}
			if (num16 > num6 && !DoubleUtil.AreClose(num16, num6))
			{
				flag4 = true;
			}
			else if (DoubleUtil.AreClose(num16, num6))
			{
				flag2 = true;
			}
			else if (DoubleUtil.AreClose(num16, num7))
			{
				flag3 = true;
			}
			else if (num16 < num6 && !DoubleUtil.AreClose(num16, num6))
			{
				flag8 = true;
			}
			if (num15 > num3 && !DoubleUtil.AreClose(num15, num3))
			{
				flag7 = true;
			}
			else if (DoubleUtil.AreClose(num15, num3))
			{
				flag5 = true;
			}
			else if (DoubleUtil.AreClose(num15, num4))
			{
				flag6 = true;
			}
			else if (num15 < num3 && !DoubleUtil.AreClose(num15, num3))
			{
				flag9 = true;
			}
			double num17 = (0.0 < num11) ? (100.0 * (num11 - num15 - num16) / num11) : 0.0;
			bool flag10 = !DoubleUtil.AreClose(num3, num4);
			durTableWidth = TextDpi.FromTextDpi(mbpInfo.BPLeft);
			for (int j = 0; j < this._calculatedColumns.Length; j++)
			{
				if (this._calculatedColumns[j].UserWidth.IsAuto)
				{
					this._calculatedColumns[j].DurWidth = (flag8 ? (this._calculatedColumns[j].DurMaxWidth - (this._calculatedColumns[j].DurMaxWidth - this._calculatedColumns[j].DurMinWidth) * (num6 - num16) / (num6 - num7)) : (flag4 ? (this._calculatedColumns[j].DurMaxWidth + this._calculatedColumns[j].DurMaxWidth * (num16 - num6) / num6) : (flag2 ? this._calculatedColumns[j].DurMaxWidth : (flag3 ? this._calculatedColumns[j].DurMinWidth : ((0.0 < num6 && !DoubleUtil.IsZero(num6)) ? (this._calculatedColumns[j].DurMinWidth + this._calculatedColumns[j].DurMaxWidth * (num16 - num7) / num6) : 0.0)))));
				}
				else if (this._calculatedColumns[j].UserWidth.IsStar)
				{
					num14 = ((0.0 < num2) ? (num11 * (num17 * this._calculatedColumns[j].UserWidth.Value / num2) / 100.0) : 0.0);
					num14 -= this._calculatedColumns[j].DurMinWidth;
					if (num14 < 0.0 && !DoubleUtil.IsZero(num14))
					{
						num14 = 0.0;
					}
					this._calculatedColumns[j].DurWidth = this._calculatedColumns[j].DurMinWidth + num14;
				}
				else
				{
					this._calculatedColumns[j].DurWidth = (flag9 ? (flag10 ? (this._calculatedColumns[j].DurMaxWidth - (this._calculatedColumns[j].DurMaxWidth - this._calculatedColumns[j].DurMinWidth) * (num3 - num15) / (num3 - num4)) : (this._calculatedColumns[j].DurMaxWidth - this._calculatedColumns[j].DurMaxWidth * (num3 - num15) / num3)) : (flag7 ? (this._calculatedColumns[j].DurMaxWidth + this._calculatedColumns[j].DurMaxWidth * (num15 - num3) / num3) : (flag5 ? this._calculatedColumns[j].DurMaxWidth : (flag6 ? this._calculatedColumns[j].DurMinWidth : ((0.0 < num3 && !DoubleUtil.IsZero(num3)) ? (this._calculatedColumns[j].DurMinWidth + this._calculatedColumns[j].DurMaxWidth * (num15 - num4) / num3) : 0.0)))));
				}
				this._calculatedColumns[j].UrOffset = durTableWidth + internalCellSpacing / 2.0;
				durTableWidth += this._calculatedColumns[j].DurWidth + internalCellSpacing;
				if (this._calculatedColumns[j].PtsWidthChanged == 1)
				{
					result = 0;
				}
			}
			durTableWidth += mbpInfo.Margin.Left + TextDpi.FromTextDpi(mbpInfo.MBPRight);
			return result;
		}

		// Token: 0x06006AC9 RID: 27337 RVA: 0x001E9188 File Offset: 0x001E7388
		private PTS.FSRECT GetTableContentRect(MbpInfo mbpInfo)
		{
			int num = this.IsFirstChunk ? mbpInfo.BPTop : 0;
			int num2 = this.IsLastChunk ? mbpInfo.BPBottom : 0;
			return new PTS.FSRECT(this._rect.u + mbpInfo.BPLeft, this._rect.v + num, Math.Max(this._rect.du - (mbpInfo.BPRight + mbpInfo.BPLeft), 1), Math.Max(this._rect.dv - num2 - num, 1));
		}

		// Token: 0x06006ACA RID: 27338 RVA: 0x001E9214 File Offset: 0x001E7414
		private int GetTableOffsetFirstRowTop()
		{
			MbpInfo mbpInfo = MbpInfo.FromElement(this.TableParagraph.Element, this.TableParagraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (!this.IsFirstChunk)
			{
				return 0;
			}
			return mbpInfo.BPTop;
		}

		// Token: 0x0400344F RID: 13391
		private bool _isFirstChunk;

		// Token: 0x04003450 RID: 13392
		private bool _isLastChunk;

		// Token: 0x04003451 RID: 13393
		private PTS.FSRECT _columnRect;

		// Token: 0x04003452 RID: 13394
		private CalculatedColumn[] _calculatedColumns;

		// Token: 0x04003453 RID: 13395
		private double _durMinWidth;

		// Token: 0x04003454 RID: 13396
		private double _durMaxWidth;

		// Token: 0x04003455 RID: 13397
		private double _previousAutofitWidth;

		// Token: 0x04003456 RID: 13398
		private double _previousTableWidth;

		// Token: 0x02000A28 RID: 2600
		private struct CellParaClientEntry
		{
			// Token: 0x0400471B RID: 18203
			internal CellParaClient cellParaClient;

			// Token: 0x0400471C RID: 18204
			internal PTS.FSKUPDATE fskupdCell;
		}
	}
}
