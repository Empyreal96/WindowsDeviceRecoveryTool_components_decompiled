using System;
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
	// Token: 0x02000611 RID: 1553
	internal sealed class CellParaClient : SubpageParaClient
	{
		// Token: 0x0600676E RID: 26478 RVA: 0x001CEABA File Offset: 0x001CCCBA
		internal CellParaClient(CellParagraph cellParagraph, TableParaClient tableParaClient) : base(cellParagraph)
		{
			this._tableParaClient = tableParaClient;
		}

		// Token: 0x0600676F RID: 26479 RVA: 0x001CEACC File Offset: 0x001CCCCC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void Arrange(int du, int dv, PTS.FSRECT rcTable, FlowDirection tableFlowDirection, PageContext pageContext)
		{
			CalculatedColumn[] calculatedColumns = this._tableParaClient.CalculatedColumns;
			double internalCellSpacing = this.Table.InternalCellSpacing;
			double num = -internalCellSpacing;
			int num2 = this.Cell.ColumnIndex + this.Cell.ColumnSpan - 1;
			do
			{
				num += calculatedColumns[num2].DurWidth + internalCellSpacing;
			}
			while (--num2 >= this.ColumnIndex);
			if (tableFlowDirection != base.PageFlowDirection)
			{
				PTS.FSRECT pageRect = pageContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(base.PageFlowDirection), ref pageRect, ref rcTable, PTS.FlowDirectionToFswdir(tableFlowDirection), out rcTable));
			}
			this._rect.u = du + rcTable.u;
			this._rect.v = dv + rcTable.v;
			this._rect.du = TextDpi.ToTextDpi(num);
			this._rect.dv = TextDpi.ToTextDpi(this._arrangeHeight);
			if (tableFlowDirection != base.PageFlowDirection)
			{
				PTS.FSRECT pageRect2 = pageContext.PageRect;
				PTS.Validate(PTS.FsTransformRectangle(PTS.FlowDirectionToFswdir(tableFlowDirection), ref pageRect2, ref this._rect, PTS.FlowDirectionToFswdir(base.PageFlowDirection), out this._rect));
			}
			this._flowDirectionParent = tableFlowDirection;
			this._flowDirection = (FlowDirection)base.Paragraph.Element.GetValue(FrameworkElement.FlowDirectionProperty);
			this._pageContext = pageContext;
			this.OnArrange();
			if (this._paraHandle.Value != IntPtr.Zero)
			{
				PTS.Validate(PTS.FsClearUpdateInfoInSubpage(base.PtsContext.Context, this._paraHandle.Value), base.PtsContext);
			}
		}

		// Token: 0x06006770 RID: 26480 RVA: 0x001CEC5E File Offset: 0x001CCE5E
		internal void ValidateVisual()
		{
			this.ValidateVisual(PTS.FSKUPDATE.fskupdNew);
		}

		// Token: 0x06006771 RID: 26481 RVA: 0x001CEC68 File Offset: 0x001CCE68
		[SecurityCritical]
		internal void FormatCellFinite(Size subpageSize, IntPtr breakRecordIn, bool isEmptyOk, uint fswdir, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out int dvrUsed, out IntPtr breakRecordOut)
		{
			if (this.CellParagraph.StructuralCache.DtrList != null)
			{
				this.CellParagraph.InvalidateStructure(TextContainerHelper.GetCPFromElement(this.CellParagraph.StructuralCache.TextContainer, this.CellParagraph.Element, ElementEdge.BeforeStart));
			}
			PTS.FSPAP fspap = default(PTS.FSPAP);
			this.CellParagraph.GetParaProperties(ref fspap);
			PTS.FSRECT fsrect = default(PTS.FSRECT);
			fsrect.u = (fsrect.v = 0);
			fsrect.du = TextDpi.ToTextDpi(subpageSize.Width);
			fsrect.dv = TextDpi.ToTextDpi(subpageSize.Height);
			bool condition = breakRecordIn != IntPtr.Zero;
			IntPtr value;
			PTS.FSBBOX fsbbox;
			IntPtr zero;
			PTS.FSKCLEAR fskclear;
			int num;
			this.CellParagraph.FormatParaFinite(this, breakRecordIn, PTS.FromBoolean(true), IntPtr.Zero, PTS.FromBoolean(isEmptyOk), PTS.FromBoolean(condition), fswdir, ref fsrect, null, PTS.FSKCLEAR.fskclearNone, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out value, out breakRecordOut, out dvrUsed, out fsbbox, out zero, out fskclear, out num);
			if (zero != IntPtr.Zero)
			{
				MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState);
				dvrUsed += marginCollapsingState.Margin;
				marginCollapsingState.Dispose();
				zero = IntPtr.Zero;
			}
			this._paraHandle.Value = value;
		}

		// Token: 0x06006772 RID: 26482 RVA: 0x001CEDAC File Offset: 0x001CCFAC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void FormatCellBottomless(uint fswdir, double width, out PTS.FSFMTRBL fmtrbl, out int dvrUsed)
		{
			if (this.CellParagraph.StructuralCache.DtrList != null)
			{
				this.CellParagraph.InvalidateStructure(TextContainerHelper.GetCPFromElement(this.CellParagraph.StructuralCache.TextContainer, this.CellParagraph.Element, ElementEdge.BeforeStart));
			}
			PTS.FSPAP fspap = default(PTS.FSPAP);
			this.CellParagraph.GetParaProperties(ref fspap);
			IntPtr value;
			PTS.FSBBOX fsbbox;
			IntPtr zero;
			PTS.FSKCLEAR fskclear;
			int num;
			int num2;
			this.CellParagraph.FormatParaBottomless(this, PTS.FromBoolean(false), fswdir, 0, TextDpi.ToTextDpi(width), 0, null, PTS.FSKCLEAR.fskclearNone, PTS.FromBoolean(true), out fmtrbl, out value, out dvrUsed, out fsbbox, out zero, out fskclear, out num, out num2);
			if (zero != IntPtr.Zero)
			{
				MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState);
				dvrUsed += marginCollapsingState.Margin;
				marginCollapsingState.Dispose();
				zero = IntPtr.Zero;
			}
			this._paraHandle.Value = value;
		}

		// Token: 0x06006773 RID: 26483 RVA: 0x001CEE90 File Offset: 0x001CD090
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void UpdateBottomlessCell(uint fswdir, double width, out PTS.FSFMTRBL fmtrbl, out int dvrUsed)
		{
			PTS.FSPAP fspap = default(PTS.FSPAP);
			this.CellParagraph.GetParaProperties(ref fspap);
			PTS.FSBBOX fsbbox;
			IntPtr zero;
			PTS.FSKCLEAR fskclear;
			int num;
			int num2;
			this.CellParagraph.UpdateBottomlessPara(this._paraHandle.Value, this, PTS.FromBoolean(false), fswdir, 0, TextDpi.ToTextDpi(width), 0, null, PTS.FSKCLEAR.fskclearNone, PTS.FromBoolean(true), out fmtrbl, out dvrUsed, out fsbbox, out zero, out fskclear, out num, out num2);
			if (zero != IntPtr.Zero)
			{
				MarginCollapsingState marginCollapsingState = base.PtsContext.HandleToObject(zero) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState);
				dvrUsed += marginCollapsingState.Margin;
				marginCollapsingState.Dispose();
				zero = IntPtr.Zero;
			}
		}

		// Token: 0x06006774 RID: 26484 RVA: 0x001CEF30 File Offset: 0x001CD130
		internal Geometry GetTightBoundingGeometryFromTextPositions(ITextPointer startPosition, ITextPointer endPosition, Rect visibleRect)
		{
			Geometry geometry = null;
			if (endPosition.CompareTo(this.Cell.StaticElementEnd) >= 0)
			{
				geometry = new RectangleGeometry(this._rect.FromTextDpi());
			}
			else
			{
				SubpageParagraphResult subpageParagraphResult = (SubpageParagraphResult)this.CreateParagraphResult();
				ReadOnlyCollection<ColumnResult> columns = subpageParagraphResult.Columns;
				Transform transform = new TranslateTransform(-TextDpi.FromTextDpi(base.ContentRect.u), -TextDpi.FromTextDpi(base.ContentRect.v));
				visibleRect = transform.TransformBounds(visibleRect);
				geometry = TextDocumentView.GetTightBoundingGeometryFromTextPositionsHelper(columns[0].Paragraphs, subpageParagraphResult.FloatingElements, startPosition, endPosition, 0.0, visibleRect);
				if (geometry != null)
				{
					Rect viewport = new Rect(0.0, 0.0, TextDpi.FromTextDpi(base.ContentRect.du), TextDpi.FromTextDpi(base.ContentRect.dv));
					CaretElement.ClipGeometryByViewport(ref geometry, viewport);
					transform = new TranslateTransform(TextDpi.FromTextDpi(base.ContentRect.u), TextDpi.FromTextDpi(base.ContentRect.v));
					CaretElement.AddTransformToGeometry(geometry, transform);
				}
			}
			return geometry;
		}

		// Token: 0x06006775 RID: 26485 RVA: 0x001CF048 File Offset: 0x001CD248
		internal double CalculateCellWidth(TableParaClient tableParaClient)
		{
			CalculatedColumn[] calculatedColumns = tableParaClient.CalculatedColumns;
			double internalCellSpacing = this.Table.InternalCellSpacing;
			double num = -internalCellSpacing;
			int num2 = this.Cell.ColumnIndex + this.Cell.ColumnSpan - 1;
			do
			{
				num += calculatedColumns[num2].DurWidth + internalCellSpacing;
			}
			while (--num2 >= this.Cell.ColumnIndex);
			return num;
		}

		// Token: 0x170018FD RID: 6397
		// (get) Token: 0x06006776 RID: 26486 RVA: 0x001CF0A9 File Offset: 0x001CD2A9
		internal TableCell Cell
		{
			get
			{
				return this.CellParagraph.Cell;
			}
		}

		// Token: 0x170018FE RID: 6398
		// (get) Token: 0x06006777 RID: 26487 RVA: 0x001CF0B6 File Offset: 0x001CD2B6
		internal Table Table
		{
			get
			{
				return this.Cell.Table;
			}
		}

		// Token: 0x170018FF RID: 6399
		// (get) Token: 0x06006778 RID: 26488 RVA: 0x001CF0C3 File Offset: 0x001CD2C3
		internal CellParagraph CellParagraph
		{
			get
			{
				return (CellParagraph)this._paragraph;
			}
		}

		// Token: 0x17001900 RID: 6400
		// (get) Token: 0x06006779 RID: 26489 RVA: 0x001CF0D0 File Offset: 0x001CD2D0
		internal int ColumnIndex
		{
			get
			{
				return this.Cell.ColumnIndex;
			}
		}

		// Token: 0x17001901 RID: 6401
		// (set) Token: 0x0600677A RID: 26490 RVA: 0x001CF0DD File Offset: 0x001CD2DD
		internal double ArrangeHeight
		{
			set
			{
				this._arrangeHeight = value;
			}
		}

		// Token: 0x17001902 RID: 6402
		// (get) Token: 0x0600677B RID: 26491 RVA: 0x001CF0E6 File Offset: 0x001CD2E6
		internal TableParaClient TableParaClient
		{
			get
			{
				return this._tableParaClient;
			}
		}

		// Token: 0x0400336E RID: 13166
		private double _arrangeHeight;

		// Token: 0x0400336F RID: 13167
		private TableParaClient _tableParaClient;
	}
}
