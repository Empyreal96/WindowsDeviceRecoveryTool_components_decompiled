using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200064E RID: 1614
	internal sealed class TextParagraph : BaseParagraph
	{
		// Token: 0x06006B23 RID: 27427 RVA: 0x001EEDB9 File Offset: 0x001ECFB9
		internal TextParagraph(DependencyObject element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x06006B24 RID: 27428 RVA: 0x001EEDD0 File Offset: 0x001ECFD0
		public override void Dispose()
		{
			if (this._attachedObjects != null)
			{
				foreach (AttachedObject attachedObject in this._attachedObjects)
				{
					attachedObject.Dispose();
				}
				this._attachedObjects = null;
			}
			if (this._inlineObjects != null)
			{
				foreach (InlineObject inlineObject in this._inlineObjects)
				{
					inlineObject.Dispose();
				}
				this._inlineObjects = null;
			}
			base.Dispose();
		}

		// Token: 0x06006B25 RID: 27429 RVA: 0x001EEE88 File Offset: 0x001ED088
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			fspap.fKeepWithNext = 0;
			fspap.fBreakPageBefore = 0;
			fspap.fBreakColumnBefore = 0;
			base.GetParaProperties(ref fspap, true);
			fspap.idobj = -1;
		}

		// Token: 0x06006B26 RID: 27430 RVA: 0x001EEEB0 File Offset: 0x001ED0B0
		internal override void CreateParaclient(out IntPtr paraClientHandle)
		{
			TextParaClient textParaClient = new TextParaClient(this);
			paraClientHandle = textParaClient.Handle;
		}

		// Token: 0x06006B27 RID: 27431 RVA: 0x001EEECC File Offset: 0x001ED0CC
		internal void GetTextProperties(int iArea, ref PTS.FSTXTPROPS fstxtprops)
		{
			fstxtprops.fswdir = PTS.FlowDirectionToFswdir((FlowDirection)base.Element.GetValue(FrameworkElement.FlowDirectionProperty));
			fstxtprops.dcpStartContent = 0;
			fstxtprops.fKeepTogether = PTS.FromBoolean(DynamicPropertyReader.GetKeepTogether(base.Element));
			fstxtprops.cMinLinesAfterBreak = DynamicPropertyReader.GetMinOrphanLines(base.Element);
			fstxtprops.cMinLinesBeforeBreak = DynamicPropertyReader.GetMinWidowLines(base.Element);
			fstxtprops.fDropCap = 0;
			fstxtprops.fVerticalGrid = 0;
			fstxtprops.fOptimizeParagraph = PTS.FromBoolean(this.IsOptimalParagraph);
			fstxtprops.fAvoidHyphenationAtTrackBottom = 0;
			fstxtprops.fAvoidHyphenationOnLastChainElement = 0;
			fstxtprops.cMaxConsecutiveHyphens = int.MaxValue;
		}

		// Token: 0x06006B28 RID: 27432 RVA: 0x001EEF70 File Offset: 0x001ED170
		internal void CreateOptimalBreakSession(TextParaClient textParaClient, int dcpStart, int durTrack, LineBreakRecord lineBreakRecord, out OptimalBreakSession optimalBreakSession, out bool isParagraphJustified)
		{
			this._textRunCache = new TextRunCache();
			TextFormatter textFormatter = base.StructuralCache.TextFormatterHost.TextFormatter;
			TextLineBreak previousLineBreak = (lineBreakRecord != null) ? lineBreakRecord.TextLineBreak : null;
			OptimalTextSource optimalTextSource = new OptimalTextSource(base.StructuralCache.TextFormatterHost, base.ParagraphStartCharacterPosition, durTrack, textParaClient, this._textRunCache);
			base.StructuralCache.TextFormatterHost.Context = optimalTextSource;
			TextParagraphCache textParagraphCache = textFormatter.CreateParagraphCache(base.StructuralCache.TextFormatterHost, dcpStart, TextDpi.FromTextDpi(durTrack), this.GetLineProperties(true, dcpStart), previousLineBreak, this._textRunCache);
			base.StructuralCache.TextFormatterHost.Context = null;
			optimalBreakSession = new OptimalBreakSession(this, textParaClient, textParagraphCache, optimalTextSource);
			isParagraphJustified = ((TextAlignment)base.Element.GetValue(Block.TextAlignmentProperty) == TextAlignment.Justify);
		}

		// Token: 0x06006B29 RID: 27433 RVA: 0x001EF038 File Offset: 0x001ED238
		internal void GetNumberFootnotes(int fsdcpStart, int fsdcpLim, out int nFootnote)
		{
			nFootnote = 0;
		}

		// Token: 0x06006B2A RID: 27434 RVA: 0x001EF03D File Offset: 0x001ED23D
		internal void FormatBottomText(int iArea, uint fswdir, Line lastLine, int dvrLine, out IntPtr mcsClient)
		{
			Invariant.Assert(iArea == 0);
			mcsClient = IntPtr.Zero;
		}

		// Token: 0x06006B2B RID: 27435 RVA: 0x001EF050 File Offset: 0x001ED250
		internal bool InterruptFormatting(int dcpCur, int vrCur)
		{
			BackgroundFormatInfo backgroundFormatInfo = base.StructuralCache.BackgroundFormatInfo;
			if (!BackgroundFormatInfo.IsBackgroundFormatEnabled)
			{
				return false;
			}
			if (base.StructuralCache.CurrentFormatContext.FinitePage)
			{
				return false;
			}
			if (vrCur < TextDpi.ToTextDpi(double.IsPositiveInfinity(backgroundFormatInfo.ViewportHeight) ? 500.0 : backgroundFormatInfo.ViewportHeight))
			{
				return false;
			}
			if (backgroundFormatInfo.BackgroundFormatStopTime > DateTime.UtcNow)
			{
				return false;
			}
			if (!backgroundFormatInfo.DoesFinalDTRCoverRestOfText)
			{
				return false;
			}
			if (dcpCur + base.ParagraphStartCharacterPosition <= backgroundFormatInfo.LastCPUninterruptible)
			{
				return false;
			}
			base.StructuralCache.BackgroundFormatInfo.CPInterrupted = dcpCur + base.ParagraphStartCharacterPosition;
			return true;
		}

		// Token: 0x06006B2C RID: 27436 RVA: 0x001EF0F8 File Offset: 0x001ED2F8
		internal IList<TextBreakpoint> FormatLineVariants(TextParaClient textParaClient, TextParagraphCache textParagraphCache, OptimalTextSource optimalTextSource, int dcp, TextLineBreak textLineBreak, uint fswdir, int urStartLine, int durLine, bool allowHyphenation, bool clearOnLeft, bool clearOnRight, bool treatAsFirstInPara, bool treatAsLastInPara, bool suppressTopSpace, IntPtr lineVariantRestriction, out int iLineBestVariant)
		{
			base.StructuralCache.TextFormatterHost.Context = optimalTextSource;
			IList<TextBreakpoint> result = textParagraphCache.FormatBreakpoints(dcp, textLineBreak, lineVariantRestriction, TextDpi.FromTextDpi(durLine), out iLineBestVariant);
			base.StructuralCache.TextFormatterHost.Context = null;
			return result;
		}

		// Token: 0x06006B2D RID: 27437 RVA: 0x001EF140 File Offset: 0x001ED340
		internal void ReconstructLineVariant(TextParaClient paraClient, int iArea, int dcp, IntPtr pbrlineIn, int dcpLineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, bool fAllowHyphenation, bool fClearOnLeft, bool fClearOnRight, bool fTreatAsFirstInPara, bool fTreatAsLastInPara, bool fSuppressTopSpace, out IntPtr lineHandle, out int dcpLine, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine)
		{
			Invariant.Assert(iArea == 0);
			base.StructuralCache.CurrentFormatContext.OnFormatLine();
			Line line = new Line(base.StructuralCache.TextFormatterHost, paraClient, base.ParagraphStartCharacterPosition);
			this.FormatLineCore(line, pbrlineIn, new Line.FormattingContext(true, fClearOnLeft, fClearOnRight, this._textRunCache)
			{
				LineFormatLengthTarget = dcpLineIn
			}, dcp, durLine, durTrack, fTreatAsFirstInPara, dcp);
			lineHandle = line.Handle;
			dcpLine = line.SafeLength;
			TextLineBreak textLineBreak = line.GetTextLineBreak();
			if (textLineBreak != null)
			{
				LineBreakRecord lineBreakRecord = new LineBreakRecord(base.PtsContext, textLineBreak);
				ppbrlineOut = lineBreakRecord.Handle;
			}
			else
			{
				ppbrlineOut = IntPtr.Zero;
			}
			fForcedBroken = PTS.FromBoolean(line.IsTruncated);
			fsflres = line.FormattingResult;
			dvrAscent = line.Baseline;
			dvrDescent = line.Height - line.Baseline;
			urBBox = urStartLine + line.Start;
			durBBox = line.Width;
			dcpDepend = line.DependantLength;
			fReformatNeighborsAsLastLine = 0;
			this.CalcLineAscentDescent(dcp, ref dvrAscent, ref dvrDescent);
			int num = base.ParagraphStartCharacterPosition + dcp + line.ActualLength + dcpDepend;
			int symbolCount = base.StructuralCache.TextContainer.SymbolCount;
			if (num > symbolCount)
			{
				num = symbolCount;
			}
			base.StructuralCache.CurrentFormatContext.DependentMax = base.StructuralCache.TextContainer.CreatePointerAtOffset(num, LogicalDirection.Backward);
		}

		// Token: 0x06006B2E RID: 27438 RVA: 0x001EF294 File Offset: 0x001ED494
		internal void FormatLine(TextParaClient paraClient, int iArea, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, bool fAllowHyphenation, bool fClearOnLeft, bool fClearOnRight, bool fTreatAsFirstInPara, bool fTreatAsLastInPara, bool fSuppressTopSpace, out IntPtr lineHandle, out int dcpLine, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine)
		{
			Invariant.Assert(iArea == 0);
			base.StructuralCache.CurrentFormatContext.OnFormatLine();
			Line line = new Line(base.StructuralCache.TextFormatterHost, paraClient, base.ParagraphStartCharacterPosition);
			Line.FormattingContext ctx = new Line.FormattingContext(true, fClearOnLeft, fClearOnRight, this._textRunCache);
			this.FormatLineCore(line, pbrlineIn, ctx, dcp, durLine, durTrack, fTreatAsFirstInPara, dcp);
			lineHandle = line.Handle;
			dcpLine = line.SafeLength;
			TextLineBreak textLineBreak = line.GetTextLineBreak();
			if (textLineBreak != null)
			{
				LineBreakRecord lineBreakRecord = new LineBreakRecord(base.PtsContext, textLineBreak);
				ppbrlineOut = lineBreakRecord.Handle;
			}
			else
			{
				ppbrlineOut = IntPtr.Zero;
			}
			fForcedBroken = PTS.FromBoolean(line.IsTruncated);
			fsflres = line.FormattingResult;
			dvrAscent = line.Baseline;
			dvrDescent = line.Height - line.Baseline;
			urBBox = urStartLine + line.Start;
			durBBox = line.Width;
			dcpDepend = line.DependantLength;
			fReformatNeighborsAsLastLine = 0;
			this.CalcLineAscentDescent(dcp, ref dvrAscent, ref dvrDescent);
			int num = base.ParagraphStartCharacterPosition + dcp + line.ActualLength + dcpDepend;
			int symbolCount = base.StructuralCache.TextContainer.SymbolCount;
			if (num > symbolCount)
			{
				num = symbolCount;
			}
			base.StructuralCache.CurrentFormatContext.DependentMax = base.StructuralCache.TextContainer.CreatePointerAtOffset(num, LogicalDirection.Backward);
		}

		// Token: 0x06006B2F RID: 27439 RVA: 0x001EF3E0 File Offset: 0x001ED5E0
		internal void UpdGetChangeInText(out int dcpStart, out int ddcpOld, out int ddcpNew)
		{
			DtrList dtrList = base.StructuralCache.DtrsFromRange(base.ParagraphStartCharacterPosition, base.LastFormatCch);
			if (dtrList != null)
			{
				dcpStart = dtrList[0].StartIndex - base.ParagraphStartCharacterPosition;
				ddcpNew = dtrList[0].PositionsAdded;
				ddcpOld = dtrList[0].PositionsRemoved;
				if (dtrList.Length > 1)
				{
					for (int i = 1; i < dtrList.Length; i++)
					{
						int num = dtrList[i].StartIndex - dtrList[i - 1].StartIndex;
						ddcpNew += num + dtrList[i].PositionsAdded;
						ddcpOld += num + dtrList[i].PositionsRemoved;
					}
				}
				if (!base.StructuralCache.CurrentFormatContext.FinitePage)
				{
					this.UpdateEmbeddedObjectsCache<AttachedObject>(ref this._attachedObjects, dcpStart, ddcpOld, ddcpNew - ddcpOld);
					this.UpdateEmbeddedObjectsCache<InlineObject>(ref this._inlineObjects, dcpStart, ddcpOld, ddcpNew - ddcpOld);
				}
				Invariant.Assert(dcpStart >= 0 && base.Cch >= dcpStart && base.LastFormatCch >= dcpStart);
				ddcpOld = Math.Min(ddcpOld, base.LastFormatCch - dcpStart + 1);
				ddcpNew = Math.Min(ddcpNew, base.Cch - dcpStart + 1);
				return;
			}
			dcpStart = (ddcpOld = (ddcpNew = 0));
		}

		// Token: 0x06006B30 RID: 27440 RVA: 0x001EF547 File Offset: 0x001ED747
		internal void GetDvrAdvance(int dcp, uint fswdir, out int dvr)
		{
			this.EnsureLineProperties();
			dvr = TextDpi.ToTextDpi(this._lineProperties.CalcLineAdvanceForTextParagraph(this, dcp, this._lineProperties.DefaultTextRunProperties.FontRenderingEmSize));
		}

		// Token: 0x06006B31 RID: 27441 RVA: 0x001EF574 File Offset: 0x001ED774
		internal int GetLastDcpAttachedObjectBeforeLine(int dcpFirst)
		{
			ITextPointer textPointerFromCP = TextContainerHelper.GetTextPointerFromCP(base.StructuralCache.TextContainer, base.ParagraphStartCharacterPosition + dcpFirst, LogicalDirection.Forward);
			ITextPointer contentStart = TextContainerHelper.GetContentStart(base.StructuralCache.TextContainer, base.Element);
			while (textPointerFromCP.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
			{
				TextElement adjacentElementFromOuterPosition = ((TextPointer)textPointerFromCP).GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
				if (!(adjacentElementFromOuterPosition is Figure) && !(adjacentElementFromOuterPosition is Floater))
				{
					break;
				}
				textPointerFromCP.MoveByOffset(adjacentElementFromOuterPosition.SymbolCount);
			}
			return contentStart.GetOffsetToPosition(textPointerFromCP);
		}

		// Token: 0x06006B32 RID: 27442 RVA: 0x001EF5F0 File Offset: 0x001ED7F0
		private List<TextElement> GetAttachedObjectElements(int dcpFirst, int dcpLast)
		{
			List<TextElement> list = new List<TextElement>();
			ITextPointer contentStart = TextContainerHelper.GetContentStart(base.StructuralCache.TextContainer, base.Element);
			ITextPointer textPointerFromCP = TextContainerHelper.GetTextPointerFromCP(base.StructuralCache.TextContainer, base.ParagraphStartCharacterPosition + dcpFirst, LogicalDirection.Forward);
			if (dcpLast > base.Cch)
			{
				dcpLast = base.Cch;
			}
			while (contentStart.GetOffsetToPosition(textPointerFromCP) < dcpLast)
			{
				if (textPointerFromCP.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
				{
					TextElement adjacentElementFromOuterPosition = ((TextPointer)textPointerFromCP).GetAdjacentElementFromOuterPosition(LogicalDirection.Forward);
					if (adjacentElementFromOuterPosition is Figure || adjacentElementFromOuterPosition is Floater)
					{
						list.Add(adjacentElementFromOuterPosition);
						textPointerFromCP.MoveByOffset(adjacentElementFromOuterPosition.SymbolCount);
					}
					else
					{
						textPointerFromCP.MoveToNextContextPosition(LogicalDirection.Forward);
					}
				}
				else
				{
					textPointerFromCP.MoveToNextContextPosition(LogicalDirection.Forward);
				}
			}
			return list;
		}

		// Token: 0x06006B33 RID: 27443 RVA: 0x001EF6A4 File Offset: 0x001ED8A4
		internal int GetAttachedObjectCount(int dcpFirst, int dcpLast)
		{
			List<TextElement> attachedObjectElements = this.GetAttachedObjectElements(dcpFirst, dcpLast);
			if (attachedObjectElements.Count == 0)
			{
				this.SubmitAttachedObjects(dcpFirst, dcpLast, null);
			}
			return attachedObjectElements.Count;
		}

		// Token: 0x06006B34 RID: 27444 RVA: 0x001EF6D4 File Offset: 0x001ED8D4
		internal List<AttachedObject> GetAttachedObjects(int dcpFirst, int dcpLast)
		{
			ITextPointer contentStart = TextContainerHelper.GetContentStart(base.StructuralCache.TextContainer, base.Element);
			List<AttachedObject> list = new List<AttachedObject>();
			List<TextElement> attachedObjectElements = this.GetAttachedObjectElements(dcpFirst, dcpLast);
			for (int i = 0; i < attachedObjectElements.Count; i++)
			{
				TextElement textElement = attachedObjectElements[i];
				if (textElement is Figure && base.StructuralCache.CurrentFormatContext.FinitePage)
				{
					FigureParagraph figureParagraph = new FigureParagraph(textElement, base.StructuralCache);
					if (base.StructuralCache.CurrentFormatContext.IncrementalUpdate)
					{
						figureParagraph.SetUpdateInfo(PTS.FSKCHANGE.fskchNew, false);
					}
					FigureObject item = new FigureObject(contentStart.GetOffsetToPosition(textElement.ElementStart), figureParagraph);
					list.Add(item);
				}
				else
				{
					FloaterParagraph floaterParagraph = new FloaterParagraph(textElement, base.StructuralCache);
					if (base.StructuralCache.CurrentFormatContext.IncrementalUpdate)
					{
						floaterParagraph.SetUpdateInfo(PTS.FSKCHANGE.fskchNew, false);
					}
					FloaterObject item2 = new FloaterObject(contentStart.GetOffsetToPosition(textElement.ElementStart), floaterParagraph);
					list.Add(item2);
				}
			}
			if (list.Count != 0)
			{
				this.SubmitAttachedObjects(dcpFirst, dcpLast, list);
			}
			return list;
		}

		// Token: 0x06006B35 RID: 27445 RVA: 0x001EF7E6 File Offset: 0x001ED9E6
		internal void SubmitInlineObjects(int dcpStart, int dcpLim, List<InlineObject> inlineObjects)
		{
			this.SubmitEmbeddedObjects<InlineObject>(ref this._inlineObjects, dcpStart, dcpLim, inlineObjects);
		}

		// Token: 0x06006B36 RID: 27446 RVA: 0x001EF7F7 File Offset: 0x001ED9F7
		internal void SubmitAttachedObjects(int dcpStart, int dcpLim, List<AttachedObject> attachedObjects)
		{
			this.SubmitEmbeddedObjects<AttachedObject>(ref this._attachedObjects, dcpStart, dcpLim, attachedObjects);
		}

		// Token: 0x06006B37 RID: 27447 RVA: 0x001EF808 File Offset: 0x001EDA08
		internal List<InlineObject> InlineObjectsFromRange(int dcpStart, int dcpLast)
		{
			List<InlineObject> list = null;
			if (this._inlineObjects != null)
			{
				list = new List<InlineObject>(this._inlineObjects.Count);
				for (int i = 0; i < this._inlineObjects.Count; i++)
				{
					InlineObject inlineObject = this._inlineObjects[i];
					if (inlineObject.Dcp >= dcpStart && inlineObject.Dcp < dcpLast)
					{
						list.Add(inlineObject);
					}
					else if (inlineObject.Dcp >= dcpLast)
					{
						break;
					}
				}
			}
			if (list == null || list.Count == 0)
			{
				return null;
			}
			return list;
		}

		// Token: 0x06006B38 RID: 27448 RVA: 0x001EF888 File Offset: 0x001EDA88
		internal void CalcLineAscentDescent(int dcp, ref int dvrAscent, ref int dvrDescent)
		{
			this.EnsureLineProperties();
			int num = dvrAscent + dvrDescent;
			int num2 = TextDpi.ToTextDpi(this._lineProperties.CalcLineAdvanceForTextParagraph(this, dcp, TextDpi.FromTextDpi(num)));
			if (num != num2)
			{
				double num3 = 1.0 * (double)num2 / (1.0 * (double)num);
				dvrAscent = (int)((double)dvrAscent * num3);
				dvrDescent = (int)((double)dvrDescent * num3);
			}
		}

		// Token: 0x06006B39 RID: 27449 RVA: 0x001EF8E8 File Offset: 0x001EDAE8
		internal override void SetUpdateInfo(PTS.FSKCHANGE fskch, bool stopAsking)
		{
			base.SetUpdateInfo(fskch, stopAsking);
			if (fskch == PTS.FSKCHANGE.fskchInside)
			{
				this._textRunCache = new TextRunCache();
				this._lineProperties = null;
			}
		}

		// Token: 0x06006B3A RID: 27450 RVA: 0x001EF908 File Offset: 0x001EDB08
		internal override void ClearUpdateInfo()
		{
			base.ClearUpdateInfo();
			if (this._attachedObjects != null)
			{
				for (int i = 0; i < this._attachedObjects.Count; i++)
				{
					this._attachedObjects[i].Para.ClearUpdateInfo();
				}
			}
		}

		// Token: 0x06006B3B RID: 27451 RVA: 0x001EF950 File Offset: 0x001EDB50
		internal override bool InvalidateStructure(int startPosition)
		{
			Invariant.Assert(base.ParagraphEndCharacterPosition >= startPosition);
			bool result = false;
			if (base.ParagraphStartCharacterPosition == startPosition)
			{
				result = true;
				AnchoredBlock anchoredBlock = null;
				if (this._attachedObjects != null && this._attachedObjects.Count > 0)
				{
					anchoredBlock = (AnchoredBlock)this._attachedObjects[0].Element;
				}
				if (anchoredBlock != null && startPosition == anchoredBlock.ElementStartOffset)
				{
					StaticTextPointer staticTextPointerFromCP = TextContainerHelper.GetStaticTextPointerFromCP(base.StructuralCache.TextContainer, startPosition);
					if (staticTextPointerFromCP.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.ElementStart)
					{
						result = (anchoredBlock != staticTextPointerFromCP.GetAdjacentElement(LogicalDirection.Forward));
					}
				}
			}
			this.InvalidateTextFormatCache();
			if (this._attachedObjects != null)
			{
				for (int i = 0; i < this._attachedObjects.Count; i++)
				{
					BaseParagraph para = this._attachedObjects[i].Para;
					if (para.ParagraphEndCharacterPosition >= startPosition)
					{
						para.InvalidateStructure(startPosition);
					}
				}
			}
			return result;
		}

		// Token: 0x06006B3C RID: 27452 RVA: 0x001EFA30 File Offset: 0x001EDC30
		internal override void InvalidateFormatCache()
		{
			this.InvalidateTextFormatCache();
			if (this._attachedObjects != null)
			{
				for (int i = 0; i < this._attachedObjects.Count; i++)
				{
					this._attachedObjects[i].Para.InvalidateFormatCache();
				}
			}
		}

		// Token: 0x06006B3D RID: 27453 RVA: 0x001EFA77 File Offset: 0x001EDC77
		internal void InvalidateTextFormatCache()
		{
			this._textRunCache = new TextRunCache();
			this._lineProperties = null;
		}

		// Token: 0x06006B3E RID: 27454 RVA: 0x001EFA8C File Offset: 0x001EDC8C
		internal void FormatLineCore(Line line, IntPtr pbrLineIn, Line.FormattingContext ctx, int dcp, int width, bool firstLine, int dcpLine)
		{
			this.FormatLineCore(line, pbrLineIn, ctx, dcp, width, -1, firstLine, dcpLine);
		}

		// Token: 0x06006B3F RID: 27455 RVA: 0x001EFAAC File Offset: 0x001EDCAC
		internal void FormatLineCore(Line line, IntPtr pbrLineIn, Line.FormattingContext ctx, int dcp, int width, int trackWidth, bool firstLine, int dcpLine)
		{
			TextDpi.EnsureValidLineWidth(ref width);
			this._currentLine = line;
			TextLineBreak textLineBreak = null;
			if (pbrLineIn != IntPtr.Zero)
			{
				LineBreakRecord lineBreakRecord = base.PtsContext.HandleToObject(pbrLineIn) as LineBreakRecord;
				PTS.ValidateHandle(lineBreakRecord);
				textLineBreak = lineBreakRecord.TextLineBreak;
			}
			try
			{
				line.Format(ctx, dcp, width, trackWidth, this.GetLineProperties(firstLine, dcpLine), textLineBreak);
			}
			finally
			{
				this._currentLine = null;
			}
		}

		// Token: 0x06006B40 RID: 27456 RVA: 0x001EFB28 File Offset: 0x001EDD28
		internal Size MeasureChild(InlineObjectRun inlineObject)
		{
			if (this._currentLine == null)
			{
				return ((OptimalTextSource)base.StructuralCache.TextFormatterHost.Context).MeasureChild(inlineObject);
			}
			return this._currentLine.MeasureChild(inlineObject);
		}

		// Token: 0x06006B41 RID: 27457 RVA: 0x001EFB5A File Offset: 0x001EDD5A
		internal bool HasFiguresFloatersOrInlineObjects()
		{
			return this.HasFiguresOrFloaters() || (this._inlineObjects != null && this._inlineObjects.Count > 0);
		}

		// Token: 0x06006B42 RID: 27458 RVA: 0x001EFB7D File Offset: 0x001EDD7D
		internal bool HasFiguresOrFloaters()
		{
			return this._attachedObjects != null && this._attachedObjects.Count > 0;
		}

		// Token: 0x06006B43 RID: 27459 RVA: 0x001EFB98 File Offset: 0x001EDD98
		internal void UpdateTextContentRangeFromAttachedObjects(TextContentRange textContentRange, int dcpFirst, int dcpLast, PTS.FSATTACHEDOBJECTDESCRIPTION[] arrayAttachedObjectDesc)
		{
			int num = dcpFirst;
			int num2 = 0;
			while (this._attachedObjects != null && num2 < this._attachedObjects.Count)
			{
				AttachedObject attachedObject = this._attachedObjects[num2];
				int paragraphStartCharacterPosition = attachedObject.Para.ParagraphStartCharacterPosition;
				int cch = attachedObject.Para.Cch;
				if (paragraphStartCharacterPosition >= num && paragraphStartCharacterPosition < dcpLast)
				{
					textContentRange.Merge(new TextContentRange(num, paragraphStartCharacterPosition, base.StructuralCache.TextContainer));
					num = paragraphStartCharacterPosition + cch;
				}
				if (dcpLast < num)
				{
					break;
				}
				num2++;
			}
			if (num < dcpLast)
			{
				textContentRange.Merge(new TextContentRange(num, dcpLast, base.StructuralCache.TextContainer));
			}
			int num3 = 0;
			while (arrayAttachedObjectDesc != null && num3 < arrayAttachedObjectDesc.Length)
			{
				PTS.FSATTACHEDOBJECTDESCRIPTION fsattachedobjectdescription = arrayAttachedObjectDesc[num3];
				BaseParaClient baseParaClient = base.PtsContext.HandleToObject(arrayAttachedObjectDesc[num3].pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				textContentRange.Merge(baseParaClient.GetTextContentRange());
				num3++;
			}
		}

		// Token: 0x06006B44 RID: 27460 RVA: 0x001EFC86 File Offset: 0x001EDE86
		internal void OnUIElementDesiredSizeChanged(object sender, DesiredSizeChangedEventArgs e)
		{
			base.StructuralCache.FormattingOwner.OnChildDesiredSizeChanged(e.Child);
		}

		// Token: 0x170019B2 RID: 6578
		// (get) Token: 0x06006B45 RID: 27461 RVA: 0x001EFC9E File Offset: 0x001EDE9E
		internal TextRunCache TextRunCache
		{
			get
			{
				return this._textRunCache;
			}
		}

		// Token: 0x170019B3 RID: 6579
		// (get) Token: 0x06006B46 RID: 27462 RVA: 0x001EFCA6 File Offset: 0x001EDEA6
		internal LineProperties Properties
		{
			get
			{
				this.EnsureLineProperties();
				return this._lineProperties;
			}
		}

		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x06006B47 RID: 27463 RVA: 0x001EFCB4 File Offset: 0x001EDEB4
		internal bool IsOptimalParagraph
		{
			get
			{
				return base.StructuralCache.IsOptimalParagraphEnabled && this.GetLineProperties(false, 0).TextWrapping != TextWrapping.NoWrap;
			}
		}

		// Token: 0x06006B48 RID: 27464 RVA: 0x001EFCD8 File Offset: 0x001EDED8
		private void EnsureLineProperties()
		{
			if (this._lineProperties == null || (this._lineProperties != null && this._lineProperties.DefaultTextRunProperties.PixelsPerDip != base.StructuralCache.TextFormatterHost.PixelsPerDip))
			{
				TextProperties defaultTextProperties = new TextProperties(base.Element, StaticTextPointer.Null, false, false, base.StructuralCache.TextFormatterHost.PixelsPerDip);
				this._lineProperties = new LineProperties(base.Element, base.StructuralCache.FormattingOwner, defaultTextProperties, null);
				bool flag = (bool)base.Element.GetValue(Block.IsHyphenationEnabledProperty);
				if (flag)
				{
					this._lineProperties.Hyphenator = base.StructuralCache.Hyphenator;
				}
			}
		}

		// Token: 0x06006B49 RID: 27465 RVA: 0x001EFD8C File Offset: 0x001EDF8C
		private void SubmitEmbeddedObjects<T>(ref List<T> objectsCached, int dcpStart, int dcpLim, List<T> objectsNew) where T : EmbeddedObject
		{
			ErrorHandler.Assert(objectsNew == null || (objectsNew[0].Dcp >= dcpStart && objectsNew[objectsNew.Count - 1].Dcp <= dcpLim), ErrorHandler.SubmitInvalidList);
			if (objectsCached == null)
			{
				if (objectsNew == null)
				{
					return;
				}
				objectsCached = new List<T>(objectsNew.Count);
			}
			int num = objectsCached.Count;
			while (num > 0 && objectsCached[num - 1].Dcp >= dcpLim)
			{
				num--;
			}
			int i = num;
			while (i > 0 && objectsCached[i - 1].Dcp >= dcpStart)
			{
				i--;
			}
			if (objectsNew == null)
			{
				for (int j = i; j < num; j++)
				{
					objectsCached[j].Dispose();
				}
				objectsCached.RemoveRange(i, num - i);
				return;
			}
			if (num == i)
			{
				objectsCached.InsertRange(i, objectsNew);
				return;
			}
			int num2 = 0;
			while (i < num)
			{
				T t = objectsCached[i];
				int k;
				for (k = num2; k < objectsNew.Count; k++)
				{
					T t2 = objectsNew[k];
					if (t.Element == t2.Element)
					{
						if (k > num2)
						{
							objectsCached.InsertRange(i, objectsNew.GetRange(num2, k - num2));
							num += k - num2;
							i += k - num2;
						}
						t.Update(t2);
						objectsNew[k] = t;
						num2 = k + 1;
						i++;
						t2.Dispose();
						break;
					}
				}
				if (k >= objectsNew.Count)
				{
					objectsCached[i].Dispose();
					objectsCached.RemoveAt(i);
					num--;
				}
			}
			if (num2 < objectsNew.Count)
			{
				objectsCached.InsertRange(num, objectsNew.GetRange(num2, objectsNew.Count - num2));
			}
		}

		// Token: 0x06006B4A RID: 27466 RVA: 0x001EFF84 File Offset: 0x001EE184
		private void UpdateEmbeddedObjectsCache<T>(ref List<T> objectsCached, int dcpStart, int cchDeleted, int cchDiff) where T : EmbeddedObject
		{
			if (objectsCached != null)
			{
				int num = 0;
				while (num < objectsCached.Count && objectsCached[num].Dcp < dcpStart)
				{
					num++;
				}
				int i = num;
				while (i < objectsCached.Count && objectsCached[i].Dcp < dcpStart + cchDeleted)
				{
					i++;
				}
				if (num != i)
				{
					for (int j = num; j < i; j++)
					{
						objectsCached[j].Dispose();
					}
					objectsCached.RemoveRange(num, i - num);
				}
				while (i < objectsCached.Count)
				{
					objectsCached[i].Dcp += cchDiff;
					i++;
				}
				if (objectsCached.Count == 0)
				{
					objectsCached = null;
				}
			}
		}

		// Token: 0x06006B4B RID: 27467 RVA: 0x001F004C File Offset: 0x001EE24C
		private TextParagraphProperties GetLineProperties(bool firstLine, int dcpLine)
		{
			this.EnsureLineProperties();
			if (firstLine && this._lineProperties.HasFirstLineProperties)
			{
				if (dcpLine != 0)
				{
					firstLine = false;
				}
				else
				{
					int cpfromElement = TextContainerHelper.GetCPFromElement(base.StructuralCache.TextContainer, base.Element, ElementEdge.AfterStart);
					if (cpfromElement < base.ParagraphStartCharacterPosition)
					{
						firstLine = false;
					}
				}
				if (firstLine)
				{
					return this._lineProperties.FirstLineProps;
				}
			}
			return this._lineProperties;
		}

		// Token: 0x0400345B RID: 13403
		private List<AttachedObject> _attachedObjects;

		// Token: 0x0400345C RID: 13404
		private List<InlineObject> _inlineObjects;

		// Token: 0x0400345D RID: 13405
		private LineProperties _lineProperties;

		// Token: 0x0400345E RID: 13406
		private TextRunCache _textRunCache = new TextRunCache();

		// Token: 0x0400345F RID: 13407
		private Line _currentLine;
	}
}
