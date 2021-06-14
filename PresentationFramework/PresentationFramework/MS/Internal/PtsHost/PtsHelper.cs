using System;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200063A RID: 1594
	internal static class PtsHelper
	{
		// Token: 0x0600692F RID: 26927 RVA: 0x001DC514 File Offset: 0x001DA714
		internal static void UpdateMirroringTransform(FlowDirection parentFD, FlowDirection childFD, ContainerVisual visualChild, double width)
		{
			if (parentFD != childFD)
			{
				MatrixTransform transform = new MatrixTransform(-1.0, 0.0, 0.0, 1.0, width, 0.0);
				visualChild.Transform = transform;
				visualChild.SetValue(FrameworkElement.FlowDirectionProperty, childFD);
				return;
			}
			visualChild.Transform = null;
			visualChild.ClearValue(FrameworkElement.FlowDirectionProperty);
		}

		// Token: 0x06006930 RID: 26928 RVA: 0x001DC584 File Offset: 0x001DA784
		internal static void ClipChildrenToRect(ContainerVisual visual, Rect rect)
		{
			VisualCollection children = visual.Children;
			for (int i = 0; i < children.Count; i++)
			{
				((ContainerVisual)children[i]).Clip = new RectangleGeometry(rect);
			}
		}

		// Token: 0x06006931 RID: 26929 RVA: 0x001DC5C0 File Offset: 0x001DA7C0
		internal static void UpdateFloatingElementVisuals(ContainerVisual visual, List<BaseParaClient> floatingElementList)
		{
			VisualCollection children = visual.Children;
			int num = 0;
			if (floatingElementList == null || floatingElementList.Count == 0)
			{
				children.Clear();
				return;
			}
			for (int i = 0; i < floatingElementList.Count; i++)
			{
				Visual visual2 = floatingElementList[i].Visual;
				while (num < children.Count && children[num] != visual2)
				{
					children.RemoveAt(num);
				}
				if (num == children.Count)
				{
					children.Add(visual2);
				}
				num++;
			}
			if (children.Count > floatingElementList.Count)
			{
				children.RemoveRange(floatingElementList.Count, children.Count - floatingElementList.Count);
			}
		}

		// Token: 0x06006932 RID: 26930 RVA: 0x001DC660 File Offset: 0x001DA860
		[SecurityCritical]
		internal static void ArrangeTrack(PtsContext ptsContext, ref PTS.FSTRACKDESCRIPTION trackDesc, uint fswdirTrack)
		{
			if (trackDesc.pfstrack != IntPtr.Zero)
			{
				PTS.FSTRACKDETAILS fstrackdetails;
				PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, trackDesc.pfstrack, out fstrackdetails));
				if (fstrackdetails.cParas != 0)
				{
					PTS.FSPARADESCRIPTION[] arrayParaDesc;
					PtsHelper.ParaListFromTrack(ptsContext, trackDesc.pfstrack, ref fstrackdetails, out arrayParaDesc);
					PtsHelper.ArrangeParaList(ptsContext, trackDesc.fsrc, arrayParaDesc, fswdirTrack);
				}
			}
		}

		// Token: 0x06006933 RID: 26931 RVA: 0x001DC6C0 File Offset: 0x001DA8C0
		[SecurityCritical]
		internal static void ArrangeParaList(PtsContext ptsContext, PTS.FSRECT rcTrackContent, PTS.FSPARADESCRIPTION[] arrayParaDesc, uint fswdirTrack)
		{
			int num = 0;
			for (int i = 0; i < arrayParaDesc.Length; i++)
			{
				BaseParaClient baseParaClient = ptsContext.HandleToObject(arrayParaDesc[i].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				if (i == 0)
				{
					uint num2 = PTS.FlowDirectionToFswdir(baseParaClient.PageFlowDirection);
					if (fswdirTrack != num2)
					{
						PTS.FSRECT pageRect = baseParaClient.Paragraph.StructuralCache.CurrentArrangeContext.PageContext.PageRect;
						PTS.Validate(PTS.FsTransformRectangle(fswdirTrack, ref pageRect, ref rcTrackContent, num2, out rcTrackContent));
					}
				}
				int dvrTopSpace = arrayParaDesc[i].dvrTopSpace;
				PTS.FSRECT rcPara = rcTrackContent;
				rcPara.v += num + dvrTopSpace;
				rcPara.dv = arrayParaDesc[i].dvrUsed - dvrTopSpace;
				baseParaClient.Arrange(arrayParaDesc[i].pfspara, rcPara, dvrTopSpace, fswdirTrack);
				num += arrayParaDesc[i].dvrUsed;
			}
		}

		// Token: 0x06006934 RID: 26932 RVA: 0x001DC7A0 File Offset: 0x001DA9A0
		[SecurityCritical]
		internal static void UpdateTrackVisuals(PtsContext ptsContext, VisualCollection visualCollection, PTS.FSKUPDATE fskupdInherited, ref PTS.FSTRACKDESCRIPTION trackDesc)
		{
			PTS.FSKUPDATE fskupdate = trackDesc.fsupdinf.fskupd;
			if (trackDesc.fsupdinf.fskupd == PTS.FSKUPDATE.fskupdInherited)
			{
				fskupdate = fskupdInherited;
			}
			if (fskupdate == PTS.FSKUPDATE.fskupdNoChange)
			{
				return;
			}
			ErrorHandler.Assert(fskupdate != PTS.FSKUPDATE.fskupdShifted, ErrorHandler.UpdateShiftedNotValid);
			bool flag = trackDesc.pfstrack == IntPtr.Zero;
			if (!flag)
			{
				PTS.FSTRACKDETAILS fstrackdetails;
				PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, trackDesc.pfstrack, out fstrackdetails));
				flag = (fstrackdetails.cParas == 0);
				if (!flag)
				{
					PTS.FSPARADESCRIPTION[] arrayParaDesc;
					PtsHelper.ParaListFromTrack(ptsContext, trackDesc.pfstrack, ref fstrackdetails, out arrayParaDesc);
					PtsHelper.UpdateParaListVisuals(ptsContext, visualCollection, fskupdate, arrayParaDesc);
				}
			}
			if (flag)
			{
				visualCollection.Clear();
			}
		}

		// Token: 0x06006935 RID: 26933 RVA: 0x001DC83C File Offset: 0x001DAA3C
		internal static void UpdateParaListVisuals(PtsContext ptsContext, VisualCollection visualCollection, PTS.FSKUPDATE fskupdInherited, PTS.FSPARADESCRIPTION[] arrayParaDesc)
		{
			for (int i = 0; i < arrayParaDesc.Length; i++)
			{
				BaseParaClient baseParaClient = ptsContext.HandleToObject(arrayParaDesc[i].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				PTS.FSKUPDATE fskupdate = arrayParaDesc[i].fsupdinf.fskupd;
				if (fskupdate == PTS.FSKUPDATE.fskupdInherited)
				{
					fskupdate = fskupdInherited;
				}
				if (fskupdate == PTS.FSKUPDATE.fskupdNew)
				{
					Visual visual = VisualTreeHelper.GetParent(baseParaClient.Visual) as Visual;
					if (visual != null)
					{
						ContainerVisual containerVisual = visual as ContainerVisual;
						Invariant.Assert(containerVisual != null, "parent should always derives from ContainerVisual");
						containerVisual.Children.Remove(baseParaClient.Visual);
					}
					visualCollection.Insert(i, baseParaClient.Visual);
					baseParaClient.ValidateVisual(fskupdate);
				}
				else
				{
					while (visualCollection[i] != baseParaClient.Visual)
					{
						visualCollection.RemoveAt(i);
						Invariant.Assert(i < visualCollection.Count);
					}
					if (fskupdate == PTS.FSKUPDATE.fskupdChangeInside || fskupdate == PTS.FSKUPDATE.fskupdShifted)
					{
						baseParaClient.ValidateVisual(fskupdate);
					}
				}
			}
			if (arrayParaDesc.Length < visualCollection.Count)
			{
				visualCollection.RemoveRange(arrayParaDesc.Length, visualCollection.Count - arrayParaDesc.Length);
			}
		}

		// Token: 0x06006936 RID: 26934 RVA: 0x001DC93C File Offset: 0x001DAB3C
		[SecurityCritical]
		internal static void UpdateViewportTrack(PtsContext ptsContext, ref PTS.FSTRACKDESCRIPTION trackDesc, ref PTS.FSRECT viewport)
		{
			if (trackDesc.pfstrack != IntPtr.Zero)
			{
				PTS.FSTRACKDETAILS fstrackdetails;
				PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, trackDesc.pfstrack, out fstrackdetails));
				if (fstrackdetails.cParas != 0)
				{
					PTS.FSPARADESCRIPTION[] arrayParaDesc;
					PtsHelper.ParaListFromTrack(ptsContext, trackDesc.pfstrack, ref fstrackdetails, out arrayParaDesc);
					PtsHelper.UpdateViewportParaList(ptsContext, arrayParaDesc, ref viewport);
				}
			}
		}

		// Token: 0x06006937 RID: 26935 RVA: 0x001DC994 File Offset: 0x001DAB94
		internal static void UpdateViewportParaList(PtsContext ptsContext, PTS.FSPARADESCRIPTION[] arrayParaDesc, ref PTS.FSRECT viewport)
		{
			for (int i = 0; i < arrayParaDesc.Length; i++)
			{
				BaseParaClient baseParaClient = ptsContext.HandleToObject(arrayParaDesc[i].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				baseParaClient.UpdateViewport(ref viewport);
			}
		}

		// Token: 0x06006938 RID: 26936 RVA: 0x001DC9D4 File Offset: 0x001DABD4
		[SecurityCritical]
		internal static IInputElement InputHitTestTrack(PtsContext ptsContext, PTS.FSPOINT pt, ref PTS.FSTRACKDESCRIPTION trackDesc)
		{
			if (trackDesc.pfstrack == IntPtr.Zero)
			{
				return null;
			}
			IInputElement result = null;
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, trackDesc.pfstrack, out fstrackdetails));
			if (fstrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromTrack(ptsContext, trackDesc.pfstrack, ref fstrackdetails, out arrayParaDesc);
				result = PtsHelper.InputHitTestParaList(ptsContext, pt, ref trackDesc.fsrc, arrayParaDesc);
			}
			return result;
		}

		// Token: 0x06006939 RID: 26937 RVA: 0x001DCA38 File Offset: 0x001DAC38
		internal static IInputElement InputHitTestParaList(PtsContext ptsContext, PTS.FSPOINT pt, ref PTS.FSRECT rcTrack, PTS.FSPARADESCRIPTION[] arrayParaDesc)
		{
			IInputElement inputElement = null;
			int num = 0;
			while (num < arrayParaDesc.Length && inputElement == null)
			{
				BaseParaClient baseParaClient = ptsContext.HandleToObject(arrayParaDesc[num].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				if (baseParaClient.Rect.Contains(pt))
				{
					inputElement = baseParaClient.InputHitTest(pt);
				}
				num++;
			}
			return inputElement;
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x001DCA90 File Offset: 0x001DAC90
		[SecurityCritical]
		internal static List<Rect> GetRectanglesInTrack(PtsContext ptsContext, ContentElement e, int start, int length, ref PTS.FSTRACKDESCRIPTION trackDesc)
		{
			List<Rect> result = new List<Rect>();
			if (trackDesc.pfstrack == IntPtr.Zero)
			{
				return result;
			}
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, trackDesc.pfstrack, out fstrackdetails));
			if (fstrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] arrayParaDesc;
				PtsHelper.ParaListFromTrack(ptsContext, trackDesc.pfstrack, ref fstrackdetails, out arrayParaDesc);
				result = PtsHelper.GetRectanglesInParaList(ptsContext, e, start, length, arrayParaDesc);
			}
			return result;
		}

		// Token: 0x0600693B RID: 26939 RVA: 0x001DCAF8 File Offset: 0x001DACF8
		internal static List<Rect> GetRectanglesInParaList(PtsContext ptsContext, ContentElement e, int start, int length, PTS.FSPARADESCRIPTION[] arrayParaDesc)
		{
			List<Rect> list = new List<Rect>();
			for (int i = 0; i < arrayParaDesc.Length; i++)
			{
				BaseParaClient baseParaClient = ptsContext.HandleToObject(arrayParaDesc[i].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				if (start < baseParaClient.Paragraph.ParagraphEndCharacterPosition)
				{
					list = baseParaClient.GetRectangles(e, start, length);
					Invariant.Assert(list != null);
					if (list.Count != 0)
					{
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x0600693C RID: 26940 RVA: 0x001DCB64 File Offset: 0x001DAD64
		internal static List<Rect> OffsetRectangleList(List<Rect> rectangleList, double xOffset, double yOffset)
		{
			List<Rect> list = new List<Rect>(rectangleList.Count);
			for (int i = 0; i < rectangleList.Count; i++)
			{
				Rect item = rectangleList[i];
				item.X += xOffset;
				item.Y += yOffset;
				list.Add(item);
			}
			return list;
		}

		// Token: 0x0600693D RID: 26941 RVA: 0x001DCBBC File Offset: 0x001DADBC
		[SecurityCritical]
		internal unsafe static void SectionListFromPage(PtsContext ptsContext, IntPtr page, ref PTS.FSPAGEDETAILS pageDetails, out PTS.FSSECTIONDESCRIPTION[] arraySectionDesc)
		{
			arraySectionDesc = new PTS.FSSECTIONDESCRIPTION[pageDetails.u.complex.cSections];
			int num;
			fixed (PTS.FSSECTIONDESCRIPTION* ptr = arraySectionDesc)
			{
				PTS.Validate(PTS.FsQueryPageSectionList(ptsContext.Context, page, pageDetails.u.complex.cSections, ptr, out num));
			}
			ErrorHandler.Assert(pageDetails.u.complex.cSections == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x001DCC40 File Offset: 0x001DAE40
		[SecurityCritical]
		internal unsafe static void TrackListFromSubpage(PtsContext ptsContext, IntPtr subpage, ref PTS.FSSUBPAGEDETAILS subpageDetails, out PTS.FSTRACKDESCRIPTION[] arrayTrackDesc)
		{
			arrayTrackDesc = new PTS.FSTRACKDESCRIPTION[subpageDetails.u.complex.cBasicColumns];
			int num;
			fixed (PTS.FSTRACKDESCRIPTION* ptr = arrayTrackDesc)
			{
				PTS.Validate(PTS.FsQuerySubpageBasicColumnList(ptsContext.Context, subpage, subpageDetails.u.complex.cBasicColumns, ptr, out num));
			}
			ErrorHandler.Assert(subpageDetails.u.complex.cBasicColumns == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x0600693F RID: 26943 RVA: 0x001DCCC4 File Offset: 0x001DAEC4
		[SecurityCritical]
		internal unsafe static void TrackListFromSection(PtsContext ptsContext, IntPtr section, ref PTS.FSSECTIONDETAILS sectionDetails, out PTS.FSTRACKDESCRIPTION[] arrayTrackDesc)
		{
			arrayTrackDesc = new PTS.FSTRACKDESCRIPTION[sectionDetails.u.withpagenotes.cBasicColumns];
			int num;
			fixed (PTS.FSTRACKDESCRIPTION* ptr = arrayTrackDesc)
			{
				PTS.Validate(PTS.FsQuerySectionBasicColumnList(ptsContext.Context, section, sectionDetails.u.withpagenotes.cBasicColumns, ptr, out num));
			}
			ErrorHandler.Assert(sectionDetails.u.withpagenotes.cBasicColumns == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x06006940 RID: 26944 RVA: 0x001DCD48 File Offset: 0x001DAF48
		[SecurityCritical]
		internal unsafe static void ParaListFromTrack(PtsContext ptsContext, IntPtr track, ref PTS.FSTRACKDETAILS trackDetails, out PTS.FSPARADESCRIPTION[] arrayParaDesc)
		{
			arrayParaDesc = new PTS.FSPARADESCRIPTION[trackDetails.cParas];
			int num;
			fixed (PTS.FSPARADESCRIPTION* ptr = arrayParaDesc)
			{
				PTS.Validate(PTS.FsQueryTrackParaList(ptsContext.Context, track, trackDetails.cParas, ptr, out num));
			}
			ErrorHandler.Assert(trackDetails.cParas == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x06006941 RID: 26945 RVA: 0x001DCDAC File Offset: 0x001DAFAC
		[SecurityCritical]
		internal unsafe static void ParaListFromSubtrack(PtsContext ptsContext, IntPtr subtrack, ref PTS.FSSUBTRACKDETAILS subtrackDetails, out PTS.FSPARADESCRIPTION[] arrayParaDesc)
		{
			arrayParaDesc = new PTS.FSPARADESCRIPTION[subtrackDetails.cParas];
			int num;
			fixed (PTS.FSPARADESCRIPTION* ptr = arrayParaDesc)
			{
				PTS.Validate(PTS.FsQuerySubtrackParaList(ptsContext.Context, subtrack, subtrackDetails.cParas, ptr, out num));
			}
			ErrorHandler.Assert(subtrackDetails.cParas == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x06006942 RID: 26946 RVA: 0x001DCE10 File Offset: 0x001DB010
		[SecurityCritical]
		internal unsafe static void LineListSimpleFromTextPara(PtsContext ptsContext, IntPtr para, ref PTS.FSTEXTDETAILSFULL textDetails, out PTS.FSLINEDESCRIPTIONSINGLE[] arrayLineDesc)
		{
			arrayLineDesc = new PTS.FSLINEDESCRIPTIONSINGLE[textDetails.cLines];
			int num;
			fixed (PTS.FSLINEDESCRIPTIONSINGLE* ptr = arrayLineDesc)
			{
				PTS.Validate(PTS.FsQueryLineListSingle(ptsContext.Context, para, textDetails.cLines, ptr, out num));
			}
			ErrorHandler.Assert(textDetails.cLines == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x06006943 RID: 26947 RVA: 0x001DCE74 File Offset: 0x001DB074
		[SecurityCritical]
		internal unsafe static void LineListCompositeFromTextPara(PtsContext ptsContext, IntPtr para, ref PTS.FSTEXTDETAILSFULL textDetails, out PTS.FSLINEDESCRIPTIONCOMPOSITE[] arrayLineDesc)
		{
			arrayLineDesc = new PTS.FSLINEDESCRIPTIONCOMPOSITE[textDetails.cLines];
			int num;
			fixed (PTS.FSLINEDESCRIPTIONCOMPOSITE* ptr = arrayLineDesc)
			{
				PTS.Validate(PTS.FsQueryLineListComposite(ptsContext.Context, para, textDetails.cLines, ptr, out num));
			}
			ErrorHandler.Assert(textDetails.cLines == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x06006944 RID: 26948 RVA: 0x001DCED8 File Offset: 0x001DB0D8
		[SecurityCritical]
		internal unsafe static void LineElementListFromCompositeLine(PtsContext ptsContext, ref PTS.FSLINEDESCRIPTIONCOMPOSITE lineDesc, out PTS.FSLINEELEMENT[] arrayLineElement)
		{
			arrayLineElement = new PTS.FSLINEELEMENT[lineDesc.cElements];
			int num;
			fixed (PTS.FSLINEELEMENT* ptr = arrayLineElement)
			{
				PTS.Validate(PTS.FsQueryLineCompositeElementList(ptsContext.Context, lineDesc.pline, lineDesc.cElements, ptr, out num));
			}
			ErrorHandler.Assert(lineDesc.cElements == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x06006945 RID: 26949 RVA: 0x001DCF40 File Offset: 0x001DB140
		[SecurityCritical]
		internal unsafe static void AttachedObjectListFromParagraph(PtsContext ptsContext, IntPtr para, int cAttachedObject, out PTS.FSATTACHEDOBJECTDESCRIPTION[] arrayAttachedObjectDesc)
		{
			arrayAttachedObjectDesc = new PTS.FSATTACHEDOBJECTDESCRIPTION[cAttachedObject];
			int num;
			fixed (PTS.FSATTACHEDOBJECTDESCRIPTION* ptr = arrayAttachedObjectDesc)
			{
				PTS.Validate(PTS.FsQueryAttachedObjectList(ptsContext.Context, para, cAttachedObject, ptr, out num));
			}
			ErrorHandler.Assert(cAttachedObject == num, ErrorHandler.PTSObjectsCountMismatch);
		}

		// Token: 0x06006946 RID: 26950 RVA: 0x001DCF94 File Offset: 0x001DB194
		[SecurityCritical]
		internal static TextContentRange TextContentRangeFromTrack(PtsContext ptsContext, IntPtr pfstrack)
		{
			PTS.FSTRACKDETAILS fstrackdetails;
			PTS.Validate(PTS.FsQueryTrackDetails(ptsContext.Context, pfstrack, out fstrackdetails));
			TextContentRange textContentRange = new TextContentRange();
			if (fstrackdetails.cParas != 0)
			{
				PTS.FSPARADESCRIPTION[] array;
				PtsHelper.ParaListFromTrack(ptsContext, pfstrack, ref fstrackdetails, out array);
				for (int i = 0; i < array.Length; i++)
				{
					BaseParaClient baseParaClient = ptsContext.HandleToObject(array[i].pfsparaclient) as BaseParaClient;
					PTS.ValidateHandle(baseParaClient);
					textContentRange.Merge(baseParaClient.GetTextContentRange());
				}
			}
			return textContentRange;
		}

		// Token: 0x06006947 RID: 26951 RVA: 0x001DD00C File Offset: 0x001DB20C
		internal static double CalculatePageMarginAdjustment(StructuralCache structuralCache, double pageMarginWidth)
		{
			double result = 0.0;
			DependencyObject element = structuralCache.Section.Element;
			if (element is FlowDocument)
			{
				ColumnPropertiesGroup columnPropertiesGroup = new ColumnPropertiesGroup(element);
				if (!columnPropertiesGroup.IsColumnWidthFlexible)
				{
					double lineHeightValue = DynamicPropertyReader.GetLineHeightValue(element);
					double pageFontSize = (double)structuralCache.PropertyOwner.GetValue(TextElement.FontSizeProperty);
					FontFamily pageFontFamily = (FontFamily)structuralCache.PropertyOwner.GetValue(TextElement.FontFamilyProperty);
					int cColumns = PtsHelper.CalculateColumnCount(columnPropertiesGroup, lineHeightValue, pageMarginWidth, pageFontSize, pageFontFamily, true);
					double num;
					double num2;
					double num3;
					PtsHelper.GetColumnMetrics(columnPropertiesGroup, pageMarginWidth, pageFontSize, pageFontFamily, true, cColumns, ref lineHeightValue, out num, out num2, out num3);
					result = num2;
				}
			}
			return result;
		}

		// Token: 0x06006948 RID: 26952 RVA: 0x001DD0A4 File Offset: 0x001DB2A4
		internal static int CalculateColumnCount(ColumnPropertiesGroup columnProperties, double lineHeight, double pageWidth, double pageFontSize, FontFamily pageFontFamily, bool enableColumns)
		{
			int val = 1;
			double columnRuleWidth = columnProperties.ColumnRuleWidth;
			if (enableColumns)
			{
				double num;
				if (columnProperties.ColumnGapAuto)
				{
					num = 1.0 * lineHeight;
				}
				else
				{
					num = columnProperties.ColumnGap;
				}
				if (!columnProperties.ColumnWidthAuto)
				{
					double columnWidth = columnProperties.ColumnWidth;
					val = (int)((pageWidth + num) / (columnWidth + num));
				}
				else
				{
					double num2 = 20.0 * pageFontSize;
					val = (int)((pageWidth + num) / (num2 + num));
				}
			}
			return Math.Max(1, Math.Min(999, val));
		}

		// Token: 0x06006949 RID: 26953 RVA: 0x001DD120 File Offset: 0x001DB320
		internal static void GetColumnMetrics(ColumnPropertiesGroup columnProperties, double pageWidth, double pageFontSize, FontFamily pageFontFamily, bool enableColumns, int cColumns, ref double lineHeight, out double columnWidth, out double freeSpace, out double gapSpace)
		{
			double columnRuleWidth = columnProperties.ColumnRuleWidth;
			if (!enableColumns)
			{
				Invariant.Assert(cColumns == 1);
				columnWidth = pageWidth;
				gapSpace = 0.0;
				lineHeight = 0.0;
				freeSpace = 0.0;
			}
			else
			{
				if (columnProperties.ColumnWidthAuto)
				{
					columnWidth = 20.0 * pageFontSize;
				}
				else
				{
					columnWidth = columnProperties.ColumnWidth;
				}
				if (columnProperties.ColumnGapAuto)
				{
					gapSpace = 1.0 * lineHeight;
				}
				else
				{
					gapSpace = columnProperties.ColumnGap;
				}
			}
			columnWidth = Math.Max(1.0, Math.Min(columnWidth, pageWidth));
			freeSpace = pageWidth - (double)cColumns * columnWidth - (double)(cColumns - 1) * gapSpace;
			freeSpace = Math.Max(0.0, freeSpace);
		}

		// Token: 0x0600694A RID: 26954 RVA: 0x001DD1F4 File Offset: 0x001DB3F4
		[SecurityCritical]
		internal unsafe static void GetColumnsInfo(ColumnPropertiesGroup columnProperties, double lineHeight, double pageWidth, double pageFontSize, FontFamily pageFontFamily, int cColumns, PTS.FSCOLUMNINFO* pfscolinfo, bool enableColumns)
		{
			double columnRuleWidth = columnProperties.ColumnRuleWidth;
			double num;
			double num2;
			double num3;
			PtsHelper.GetColumnMetrics(columnProperties, pageWidth, pageFontSize, pageFontFamily, enableColumns, cColumns, ref lineHeight, out num, out num2, out num3);
			if (!columnProperties.IsColumnWidthFlexible)
			{
				for (int i = 0; i < cColumns; i++)
				{
					pfscolinfo[i].durBefore = TextDpi.ToTextDpi((i == 0) ? 0.0 : num3);
					pfscolinfo[i].durWidth = TextDpi.ToTextDpi(num);
					pfscolinfo[i].durBefore = Math.Max(0, pfscolinfo[i].durBefore);
					pfscolinfo[i].durWidth = Math.Max(1, pfscolinfo[i].durWidth);
				}
				return;
			}
			for (int j = 0; j < cColumns; j++)
			{
				if (columnProperties.ColumnSpaceDistribution == ColumnSpaceDistribution.Right)
				{
					pfscolinfo[j].durWidth = TextDpi.ToTextDpi((j == cColumns - 1) ? (num + num2) : num);
				}
				else if (columnProperties.ColumnSpaceDistribution == ColumnSpaceDistribution.Left)
				{
					pfscolinfo[j].durWidth = TextDpi.ToTextDpi((j == 0) ? (num + num2) : num);
				}
				else
				{
					pfscolinfo[j].durWidth = TextDpi.ToTextDpi(num + num2 / (double)cColumns);
				}
				if (pfscolinfo[j].durWidth > TextDpi.ToTextDpi(pageWidth))
				{
					pfscolinfo[j].durWidth = TextDpi.ToTextDpi(pageWidth);
				}
				pfscolinfo[j].durBefore = TextDpi.ToTextDpi((j == 0) ? 0.0 : num3);
				pfscolinfo[j].durBefore = Math.Max(0, pfscolinfo[j].durBefore);
				pfscolinfo[j].durWidth = Math.Max(1, pfscolinfo[j].durWidth);
			}
		}
	}
}
