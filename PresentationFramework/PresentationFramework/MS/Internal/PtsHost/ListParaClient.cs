using System;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200062D RID: 1581
	internal sealed class ListParaClient : ContainerParaClient
	{
		// Token: 0x060068B1 RID: 26801 RVA: 0x001D8B0A File Offset: 0x001D6D0A
		internal ListParaClient(ListParagraph paragraph) : base(paragraph)
		{
		}

		// Token: 0x060068B2 RID: 26802 RVA: 0x001D8B14 File Offset: 0x001D6D14
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override void ValidateVisual(PTS.FSKUPDATE fskupdInherited)
		{
			PTS.FSSUBTRACKDETAILS fssubtrackdetails;
			PTS.Validate(PTS.FsQuerySubtrackDetails(base.PtsContext.Context, this._paraHandle.Value, out fssubtrackdetails));
			MbpInfo mbpInfo = MbpInfo.FromElement(base.Paragraph.Element, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (base.ThisFlowDirection != base.PageFlowDirection)
			{
				mbpInfo.MirrorBP();
			}
			uint num = PTS.FlowDirectionToFswdir((FlowDirection)base.Paragraph.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			Brush backgroundBrush = (Brush)base.Paragraph.Element.GetValue(TextElement.BackgroundProperty);
			TextProperties defaultTextProperties = new TextProperties(base.Paragraph.Element, StaticTextPointer.Null, false, false, base.Paragraph.StructuralCache.TextFormatterHost.PixelsPerDip);
			if (fssubtrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] array;
				PtsHelper.ParaListFromSubtrack(base.PtsContext, this._paraHandle.Value, ref fssubtrackdetails, out array);
				using (DrawingContext drawingContext = this._visual.RenderOpen())
				{
					this._visual.DrawBackgroundAndBorderIntoContext(drawingContext, backgroundBrush, mbpInfo.BorderBrush, mbpInfo.Border, this._rect.FromTextDpi(), this.IsFirstChunk, this.IsLastChunk);
					ListMarkerLine listMarkerLine = new ListMarkerLine(base.Paragraph.StructuralCache.TextFormatterHost, this);
					int num2 = 0;
					for (int i = 0; i < fssubtrackdetails.cParas; i++)
					{
						List list = base.Paragraph.Element as List;
						BaseParaClient baseParaClient = base.PtsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
						PTS.ValidateHandle(baseParaClient);
						if (i == 0)
						{
							num2 = list.GetListItemIndex(baseParaClient.Paragraph.Element as ListItem);
						}
						if (baseParaClient.IsFirstChunk)
						{
							int firstTextLineBaseline = baseParaClient.GetFirstTextLineBaseline();
							if (base.PageFlowDirection != base.ThisFlowDirection)
							{
								drawingContext.PushTransform(new MatrixTransform(-1.0, 0.0, 0.0, 1.0, TextDpi.FromTextDpi(2 * baseParaClient.Rect.u + baseParaClient.Rect.du), 0.0));
							}
							int index;
							if (2147483647 - i < num2)
							{
								index = int.MaxValue;
							}
							else
							{
								index = num2 + i;
							}
							LineProperties lineProps = new LineProperties(base.Paragraph.Element, base.Paragraph.StructuralCache.FormattingOwner, defaultTextProperties, new MarkerProperties(list, index));
							listMarkerLine.FormatAndDrawVisual(drawingContext, lineProps, baseParaClient.Rect.u, firstTextLineBaseline);
							if (base.PageFlowDirection != base.ThisFlowDirection)
							{
								drawingContext.Pop();
							}
						}
					}
					listMarkerLine.Dispose();
				}
				PtsHelper.UpdateParaListVisuals(base.PtsContext, this._visual.Children, fskupdInherited, array);
				return;
			}
			this._visual.Children.Clear();
		}
	}
}
