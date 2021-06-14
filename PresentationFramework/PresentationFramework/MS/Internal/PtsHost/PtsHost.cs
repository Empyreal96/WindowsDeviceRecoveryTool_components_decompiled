using System;
using System.Collections.Generic;
using System.Security;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;

namespace MS.Internal.PtsHost
{
	// Token: 0x0200063B RID: 1595
	internal sealed class PtsHost
	{
		// Token: 0x0600694B RID: 26955 RVA: 0x001DD411 File Offset: 0x001DB611
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal PtsHost()
		{
			this._context = new SecurityCriticalDataForSet<IntPtr>(IntPtr.Zero);
		}

		// Token: 0x0600694C RID: 26956 RVA: 0x001DD429 File Offset: 0x001DB629
		internal void EnterContext(PtsContext ptsContext)
		{
			Invariant.Assert(this._ptsContext == null);
			this._ptsContext = ptsContext;
		}

		// Token: 0x0600694D RID: 26957 RVA: 0x001DD440 File Offset: 0x001DB640
		internal void LeaveContext(PtsContext ptsContext)
		{
			Invariant.Assert(this._ptsContext == ptsContext);
			this._ptsContext = null;
		}

		// Token: 0x1700196D RID: 6509
		// (get) Token: 0x0600694E RID: 26958 RVA: 0x001DD457 File Offset: 0x001DB657
		private PtsContext PtsContext
		{
			get
			{
				Invariant.Assert(this._ptsContext != null);
				return this._ptsContext;
			}
		}

		// Token: 0x1700196E RID: 6510
		// (get) Token: 0x0600694F RID: 26959 RVA: 0x001DD46D File Offset: 0x001DB66D
		// (set) Token: 0x06006950 RID: 26960 RVA: 0x001DD494 File Offset: 0x001DB694
		internal IntPtr Context
		{
			get
			{
				Invariant.Assert(this._context.Value != IntPtr.Zero);
				return this._context.Value;
			}
			[SecurityCritical]
			set
			{
				Invariant.Assert(this._context.Value == IntPtr.Zero);
				this._context.Value = value;
			}
		}

		// Token: 0x1700196F RID: 6511
		// (get) Token: 0x06006951 RID: 26961 RVA: 0x001DD4BC File Offset: 0x001DB6BC
		internal static int ContainerParagraphId
		{
			get
			{
				return PtsHost._customParaId;
			}
		}

		// Token: 0x17001970 RID: 6512
		// (get) Token: 0x06006952 RID: 26962 RVA: 0x001DD4C3 File Offset: 0x001DB6C3
		internal static int SubpageParagraphId
		{
			get
			{
				return PtsHost.ContainerParagraphId + 1;
			}
		}

		// Token: 0x17001971 RID: 6513
		// (get) Token: 0x06006953 RID: 26963 RVA: 0x001DD4CC File Offset: 0x001DB6CC
		internal static int FloaterParagraphId
		{
			get
			{
				return PtsHost.SubpageParagraphId + 1;
			}
		}

		// Token: 0x17001972 RID: 6514
		// (get) Token: 0x06006954 RID: 26964 RVA: 0x001DD4D5 File Offset: 0x001DB6D5
		internal static int TableParagraphId
		{
			get
			{
				return PtsHost.FloaterParagraphId + 1;
			}
		}

		// Token: 0x06006955 RID: 26965 RVA: 0x001DD4DE File Offset: 0x001DB6DE
		internal void AssertFailed(string arg1, string arg2, int arg3, uint arg4)
		{
			if (!PtsCache.IsDisposed())
			{
				ErrorHandler.Assert(false, ErrorHandler.PTSAssert, new object[]
				{
					arg1,
					arg2,
					arg3,
					arg4
				});
			}
		}

		// Token: 0x06006956 RID: 26966 RVA: 0x001DD514 File Offset: 0x001DB714
		internal int GetFigureProperties(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, int fInTextLine, uint fswdir, int fBottomUndefined, out int dur, out int dvr, out PTS.FSFIGUREPROPS fsfigprops, out int cPolygons, out int cVertices, out int durDistTextLeft, out int durDistTextRight, out int dvrDistTextTop, out int dvrDistTextBottom)
		{
			int result = 0;
			try
			{
				FigureParagraph figureParagraph = this.PtsContext.HandleToObject(nmpFigure) as FigureParagraph;
				PTS.ValidateHandle(figureParagraph);
				FigureParaClient figureParaClient = this.PtsContext.HandleToObject(pfsparaclientFigure) as FigureParaClient;
				PTS.ValidateHandle(figureParaClient);
				figureParagraph.GetFigureProperties(figureParaClient, fInTextLine, fswdir, fBottomUndefined, out dur, out dvr, out fsfigprops, out cPolygons, out cVertices, out durDistTextLeft, out durDistTextRight, out dvrDistTextTop, out dvrDistTextBottom);
			}
			catch (Exception callbackException)
			{
				dur = (dvr = (cPolygons = (cVertices = 0)));
				fsfigprops = default(PTS.FSFIGUREPROPS);
				durDistTextLeft = (durDistTextRight = (dvrDistTextTop = (dvrDistTextBottom = 0)));
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dur = (dvr = (cPolygons = (cVertices = 0)));
				fsfigprops = default(PTS.FSFIGUREPROPS);
				durDistTextLeft = (durDistTextRight = (dvrDistTextTop = (dvrDistTextBottom = 0)));
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006957 RID: 26967 RVA: 0x001DD648 File Offset: 0x001DB848
		[SecurityCritical]
		internal unsafe int GetFigurePolygons(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, uint fswdir, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough)
		{
			int result = 0;
			try
			{
				FigureParagraph figureParagraph = this.PtsContext.HandleToObject(nmpFigure) as FigureParagraph;
				PTS.ValidateHandle(figureParagraph);
				FigureParaClient figureParaClient = this.PtsContext.HandleToObject(pfsparaclientFigure) as FigureParaClient;
				PTS.ValidateHandle(figureParaClient);
				figureParagraph.GetFigurePolygons(figureParaClient, fswdir, ncVertices, nfspt, rgcVertices, out ccVertices, rgfspt, out cfspt, out fWrapThrough);
			}
			catch (Exception callbackException)
			{
				ccVertices = (cfspt = (fWrapThrough = 0));
				rgfspt = null;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				ccVertices = (cfspt = (fWrapThrough = 0));
				rgfspt = null;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006958 RID: 26968 RVA: 0x001DD720 File Offset: 0x001DB920
		internal int CalcFigurePosition(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, uint fswdir, ref PTS.FSRECT fsrcPage, ref PTS.FSRECT fsrcMargin, ref PTS.FSRECT fsrcTrack, ref PTS.FSRECT fsrcFigurePreliminary, int fMustPosition, int fInTextLine, out int fPushToNextTrack, out PTS.FSRECT fsrcFlow, out PTS.FSRECT fsrcOverlap, out PTS.FSBBOX fsbbox, out PTS.FSRECT fsrcSearch)
		{
			int result = 0;
			try
			{
				FigureParagraph figureParagraph = this.PtsContext.HandleToObject(nmpFigure) as FigureParagraph;
				PTS.ValidateHandle(figureParagraph);
				FigureParaClient figureParaClient = this.PtsContext.HandleToObject(pfsparaclientFigure) as FigureParaClient;
				PTS.ValidateHandle(figureParaClient);
				figureParagraph.CalcFigurePosition(figureParaClient, fswdir, ref fsrcPage, ref fsrcMargin, ref fsrcTrack, ref fsrcFigurePreliminary, fMustPosition, fInTextLine, out fPushToNextTrack, out fsrcFlow, out fsrcOverlap, out fsbbox, out fsrcSearch);
			}
			catch (Exception callbackException)
			{
				fPushToNextTrack = 0;
				fsrcFlow = (fsrcOverlap = (fsrcSearch = default(PTS.FSRECT)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fPushToNextTrack = 0;
				fsrcFlow = (fsrcOverlap = (fsrcSearch = default(PTS.FSRECT)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006959 RID: 26969 RVA: 0x001DD840 File Offset: 0x001DBA40
		internal int FSkipPage(IntPtr pfsclient, IntPtr nms, out int fSkip)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.FSkipPage(out fSkip);
			}
			catch (Exception callbackException)
			{
				fSkip = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fSkip = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600695A RID: 26970 RVA: 0x001DD8C8 File Offset: 0x001DBAC8
		internal int GetPageDimensions(IntPtr pfsclient, IntPtr nms, out uint fswdir, out int fHeaderFooterAtTopBottom, out int durPage, out int dvrPage, ref PTS.FSRECT fsrcMargin)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.GetPageDimensions(out fswdir, out fHeaderFooterAtTopBottom, out durPage, out dvrPage, ref fsrcMargin);
			}
			catch (Exception callbackException)
			{
				fswdir = 0U;
				fHeaderFooterAtTopBottom = (durPage = (dvrPage = 0));
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fswdir = 0U;
				fHeaderFooterAtTopBottom = (durPage = (dvrPage = 0));
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600695B RID: 26971 RVA: 0x001DD978 File Offset: 0x001DBB78
		internal int GetNextSection(IntPtr pfsclient, IntPtr nmsCur, out int fSuccess, out IntPtr nmsNext)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nmsCur) as Section;
				PTS.ValidateHandle(section);
				section.GetNextSection(out fSuccess, out nmsNext);
			}
			catch (Exception callbackException)
			{
				fSuccess = 0;
				nmsNext = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fSuccess = 0;
				nmsNext = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600695C RID: 26972 RVA: 0x001DDA10 File Offset: 0x001DBC10
		internal int GetSectionProperties(IntPtr pfsclient, IntPtr nms, out int fNewPage, out uint fswdir, out int fApplyColumnBalancing, out int ccol, out int cSegmentDefinedColumnSpanAreas, out int cHeightDefinedColumnSpanAreas)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.GetSectionProperties(out fNewPage, out fswdir, out fApplyColumnBalancing, out ccol, out cSegmentDefinedColumnSpanAreas, out cHeightDefinedColumnSpanAreas);
			}
			catch (Exception callbackException)
			{
				fNewPage = (fApplyColumnBalancing = (ccol = 0));
				fswdir = 0U;
				cSegmentDefinedColumnSpanAreas = (cHeightDefinedColumnSpanAreas = 0);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fNewPage = (fApplyColumnBalancing = (ccol = 0));
				fswdir = 0U;
				cSegmentDefinedColumnSpanAreas = (cHeightDefinedColumnSpanAreas = 0);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600695D RID: 26973 RVA: 0x001DDAD4 File Offset: 0x001DBCD4
		[SecurityCritical]
		internal unsafe int GetJustificationProperties(IntPtr pfsclient, IntPtr* rgnms, int cnms, int fLastSectionNotBroken, out int fJustify, out PTS.FSKALIGNPAGE fskal, out int fCancelAtLastColumn)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(*rgnms) as Section;
				PTS.ValidateHandle(section);
				section.GetJustificationProperties(rgnms, cnms, fLastSectionNotBroken, out fJustify, out fskal, out fCancelAtLastColumn);
			}
			catch (Exception callbackException)
			{
				fJustify = (fCancelAtLastColumn = 0);
				fskal = PTS.FSKALIGNPAGE.fskalpgTop;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fJustify = (fCancelAtLastColumn = 0);
				fskal = PTS.FSKALIGNPAGE.fskalpgTop;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600695E RID: 26974 RVA: 0x001DDB7C File Offset: 0x001DBD7C
		internal int GetMainTextSegment(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmSegment)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nmsSection) as Section;
				PTS.ValidateHandle(section);
				section.GetMainTextSegment(out nmSegment);
			}
			catch (Exception callbackException)
			{
				nmSegment = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				nmSegment = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600695F RID: 26975 RVA: 0x001DDC0C File Offset: 0x001DBE0C
		internal int GetHeaderSegment(IntPtr pfsclient, IntPtr nms, IntPtr pfsbrpagePrelim, uint fswdir, out int fHeaderPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirHeader, out IntPtr nmsHeader)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.GetHeaderSegment(pfsbrpagePrelim, fswdir, out fHeaderPresent, out fHardMargin, out dvrMaxHeight, out dvrFromEdge, out fswdirHeader, out nmsHeader);
			}
			catch (Exception callbackException)
			{
				fHeaderPresent = (fHardMargin = (dvrMaxHeight = (dvrFromEdge = 0)));
				fswdirHeader = 0U;
				nmsHeader = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fHeaderPresent = (fHardMargin = (dvrMaxHeight = (dvrFromEdge = 0)));
				fswdirHeader = 0U;
				nmsHeader = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006960 RID: 26976 RVA: 0x001DDCE0 File Offset: 0x001DBEE0
		internal int GetFooterSegment(IntPtr pfsclient, IntPtr nms, IntPtr pfsbrpagePrelim, uint fswdir, out int fFooterPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirFooter, out IntPtr nmsFooter)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.GetFooterSegment(pfsbrpagePrelim, fswdir, out fFooterPresent, out fHardMargin, out dvrMaxHeight, out dvrFromEdge, out fswdirFooter, out nmsFooter);
			}
			catch (Exception callbackException)
			{
				fFooterPresent = (fHardMargin = (dvrMaxHeight = (dvrFromEdge = 0)));
				fswdirFooter = 0U;
				nmsFooter = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFooterPresent = (fHardMargin = (dvrMaxHeight = (dvrFromEdge = 0)));
				fswdirFooter = 0U;
				nmsFooter = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006961 RID: 26977 RVA: 0x001DDDB4 File Offset: 0x001DBFB4
		internal int UpdGetSegmentChange(IntPtr pfsclient, IntPtr nms, out PTS.FSKCHANGE fskch)
		{
			int result = 0;
			try
			{
				ContainerParagraph containerParagraph = this.PtsContext.HandleToObject(nms) as ContainerParagraph;
				PTS.ValidateHandle(containerParagraph);
				containerParagraph.UpdGetSegmentChange(out fskch);
			}
			catch (Exception callbackException)
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x001DDE3C File Offset: 0x001DC03C
		[SecurityCritical]
		internal unsafe int GetSectionColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncol, PTS.FSCOLUMNINFO* fscolinfo, out int ccol)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nms) as Section;
				PTS.ValidateHandle(section);
				section.GetSectionColumnInfo(fswdir, ncol, fscolinfo, out ccol);
			}
			catch (Exception callbackException)
			{
				ccol = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				ccol = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006963 RID: 26979 RVA: 0x001DDECC File Offset: 0x001DC0CC
		[SecurityCritical]
		internal unsafe int GetSegmentDefinedColumnSpanAreaInfo(IntPtr pfsclient, IntPtr nms, int cAreas, IntPtr* rgnmSeg, int* rgcColumns, out int cAreasActual)
		{
			cAreasActual = 0;
			return -10000;
		}

		// Token: 0x06006964 RID: 26980 RVA: 0x001DDECC File Offset: 0x001DC0CC
		[SecurityCritical]
		internal unsafe int GetHeightDefinedColumnSpanAreaInfo(IntPtr pfsclient, IntPtr nms, int cAreas, int* rgdvrAreaHeight, int* rgcColumns, out int cAreasActual)
		{
			cAreasActual = 0;
			return -10000;
		}

		// Token: 0x06006965 RID: 26981 RVA: 0x001DDED8 File Offset: 0x001DC0D8
		internal int GetFirstPara(IntPtr pfsclient, IntPtr nms, out int fSuccessful, out IntPtr nmp)
		{
			int result = 0;
			try
			{
				ISegment segment = this.PtsContext.HandleToObject(nms) as ISegment;
				PTS.ValidateHandle(segment);
				segment.GetFirstPara(out fSuccessful, out nmp);
			}
			catch (Exception callbackException)
			{
				fSuccessful = 0;
				nmp = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fSuccessful = 0;
				nmp = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006966 RID: 26982 RVA: 0x001DDF70 File Offset: 0x001DC170
		internal int GetNextPara(IntPtr pfsclient, IntPtr nms, IntPtr nmpCur, out int fFound, out IntPtr nmpNext)
		{
			int result = 0;
			try
			{
				ISegment segment = this.PtsContext.HandleToObject(nms) as ISegment;
				PTS.ValidateHandle(segment);
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmpCur) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				segment.GetNextPara(baseParagraph, out fFound, out nmpNext);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				nmpNext = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				nmpNext = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006967 RID: 26983 RVA: 0x001DE024 File Offset: 0x001DC224
		internal int UpdGetFirstChangeInSegment(IntPtr pfsclient, IntPtr nms, out int fFound, out int fChangeFirst, out IntPtr nmpBeforeChange)
		{
			int result = 0;
			try
			{
				ISegment segment = this.PtsContext.HandleToObject(nms) as ISegment;
				PTS.ValidateHandle(segment);
				segment.UpdGetFirstChangeInSegment(out fFound, out fChangeFirst, out nmpBeforeChange);
			}
			catch (Exception callbackException)
			{
				fFound = (fChangeFirst = 0);
				nmpBeforeChange = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = (fChangeFirst = 0);
				nmpBeforeChange = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006968 RID: 26984 RVA: 0x001DE0CC File Offset: 0x001DC2CC
		internal int UpdGetParaChange(IntPtr pfsclient, IntPtr nmp, out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			int result = 0;
			try
			{
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmp) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				baseParagraph.UpdGetParaChange(out fskch, out fNoFurtherChanges);
			}
			catch (Exception callbackException)
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				fNoFurtherChanges = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				fNoFurtherChanges = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006969 RID: 26985 RVA: 0x001DE15C File Offset: 0x001DC35C
		internal int GetParaProperties(IntPtr pfsclient, IntPtr nmp, ref PTS.FSPAP fspap)
		{
			int result = 0;
			try
			{
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmp) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				baseParagraph.GetParaProperties(ref fspap);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600696A RID: 26986 RVA: 0x001DE1DC File Offset: 0x001DC3DC
		internal int CreateParaclient(IntPtr pfsclient, IntPtr nmp, out IntPtr pfsparaclient)
		{
			int result = 0;
			try
			{
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmp) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				baseParagraph.CreateParaclient(out pfsparaclient);
			}
			catch (Exception callbackException)
			{
				pfsparaclient = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pfsparaclient = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600696B RID: 26987 RVA: 0x001DE26C File Offset: 0x001DC46C
		internal int TransferDisplayInfo(IntPtr pfsclient, IntPtr pfsparaclientOld, IntPtr pfsparaclientNew)
		{
			int result = 0;
			try
			{
				BaseParaClient baseParaClient = this.PtsContext.HandleToObject(pfsparaclientOld) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				BaseParaClient baseParaClient2 = this.PtsContext.HandleToObject(pfsparaclientNew) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient2);
				baseParaClient2.TransferDisplayInfo(baseParaClient);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600696C RID: 26988 RVA: 0x001DE304 File Offset: 0x001DC504
		internal int DestroyParaclient(IntPtr pfsclient, IntPtr pfsparaclient)
		{
			int result = 0;
			try
			{
				BaseParaClient baseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				baseParaClient.Dispose();
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600696D RID: 26989 RVA: 0x001DE384 File Offset: 0x001DC584
		internal int FInterruptFormattingAfterPara(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int vr, out int fInterruptFormatting)
		{
			fInterruptFormatting = 0;
			return 0;
		}

		// Token: 0x0600696E RID: 26990 RVA: 0x001DE38C File Offset: 0x001DC58C
		internal int GetEndnoteSeparators(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmsEndnoteSeparator, out IntPtr nmsEndnoteContSeparator, out IntPtr nmsEndnoteContNotice)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nmsSection) as Section;
				PTS.ValidateHandle(section);
				section.GetEndnoteSeparators(out nmsEndnoteSeparator, out nmsEndnoteContSeparator, out nmsEndnoteContNotice);
			}
			catch (Exception callbackException)
			{
				nmsEndnoteSeparator = (nmsEndnoteContSeparator = (nmsEndnoteContNotice = IntPtr.Zero));
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				nmsEndnoteSeparator = (nmsEndnoteContSeparator = (nmsEndnoteContNotice = IntPtr.Zero));
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600696F RID: 26991 RVA: 0x001DE438 File Offset: 0x001DC638
		internal int GetEndnoteSegment(IntPtr pfsclient, IntPtr nmsSection, out int fEndnotesPresent, out IntPtr nmsEndnotes)
		{
			int result = 0;
			try
			{
				Section section = this.PtsContext.HandleToObject(nmsSection) as Section;
				PTS.ValidateHandle(section);
				section.GetEndnoteSegment(out fEndnotesPresent, out nmsEndnotes);
			}
			catch (Exception callbackException)
			{
				fEndnotesPresent = 0;
				nmsEndnotes = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fEndnotesPresent = 0;
				nmsEndnotes = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006970 RID: 26992 RVA: 0x001DE4D0 File Offset: 0x001DC6D0
		internal int GetNumberEndnoteColumns(IntPtr pfsclient, IntPtr nms, out int ccolEndnote)
		{
			ccolEndnote = 0;
			return -10000;
		}

		// Token: 0x06006971 RID: 26993 RVA: 0x001DDECC File Offset: 0x001DC0CC
		[SecurityCritical]
		internal unsafe int GetEndnoteColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncolEndnote, PTS.FSCOLUMNINFO* fscolinfoEndnote, out int ccolEndnote)
		{
			ccolEndnote = 0;
			return -10000;
		}

		// Token: 0x06006972 RID: 26994 RVA: 0x001DE4DC File Offset: 0x001DC6DC
		internal int GetFootnoteSeparators(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmsFtnSeparator, out IntPtr nmsFtnContSeparator, out IntPtr nmsFtnContNotice)
		{
			nmsFtnSeparator = (nmsFtnContSeparator = (nmsFtnContNotice = IntPtr.Zero));
			return -10000;
		}

		// Token: 0x06006973 RID: 26995 RVA: 0x001DE4D0 File Offset: 0x001DC6D0
		internal int FFootnoteBeneathText(IntPtr pfsclient, IntPtr nms, out int fFootnoteBeneathText)
		{
			fFootnoteBeneathText = 0;
			return -10000;
		}

		// Token: 0x06006974 RID: 26996 RVA: 0x001DE4D0 File Offset: 0x001DC6D0
		internal int GetNumberFootnoteColumns(IntPtr pfsclient, IntPtr nms, out int ccolFootnote)
		{
			ccolFootnote = 0;
			return -10000;
		}

		// Token: 0x06006975 RID: 26997 RVA: 0x001DDECC File Offset: 0x001DC0CC
		[SecurityCritical]
		internal unsafe int GetFootnoteColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncolFootnote, PTS.FSCOLUMNINFO* fscolinfoFootnote, out int ccolFootnote)
		{
			ccolFootnote = 0;
			return -10000;
		}

		// Token: 0x06006976 RID: 26998 RVA: 0x001DE501 File Offset: 0x001DC701
		internal int GetFootnoteSegment(IntPtr pfsclient, IntPtr nmftn, out IntPtr nmsFootnote)
		{
			nmsFootnote = IntPtr.Zero;
			return -10000;
		}

		// Token: 0x06006977 RID: 26999 RVA: 0x001DE510 File Offset: 0x001DC710
		[SecurityCritical]
		internal unsafe int GetFootnotePresentationAndRejectionOrder(IntPtr pfsclient, int cFootnotes, IntPtr* rgProposedPresentationOrder, IntPtr* rgProposedRejectionOrder, out int fProposedPresentationOrderAccepted, IntPtr* rgFinalPresentationOrder, out int fProposedRejectionOrderAccepted, IntPtr* rgFinalRejectionOrder)
		{
			fProposedPresentationOrderAccepted = (fProposedRejectionOrderAccepted = 0);
			return -10000;
		}

		// Token: 0x06006978 RID: 27000 RVA: 0x001DE4D0 File Offset: 0x001DC6D0
		internal int FAllowFootnoteSeparation(IntPtr pfsclient, IntPtr nmftn, out int fAllow)
		{
			fAllow = 0;
			return -10000;
		}

		// Token: 0x06006979 RID: 27001 RVA: 0x001DE52C File Offset: 0x001DC72C
		internal int DuplicateMcsclient(IntPtr pfsclient, IntPtr pmcsclientIn, out IntPtr pmcsclientNew)
		{
			int result = 0;
			try
			{
				MarginCollapsingState marginCollapsingState = this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState);
				pmcsclientNew = marginCollapsingState.Clone().Handle;
			}
			catch (Exception callbackException)
			{
				pmcsclientNew = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pmcsclientNew = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600697A RID: 27002 RVA: 0x001DE5C0 File Offset: 0x001DC7C0
		internal int DestroyMcsclient(IntPtr pfsclient, IntPtr pmcsclient)
		{
			int result = 0;
			try
			{
				MarginCollapsingState marginCollapsingState = this.PtsContext.HandleToObject(pmcsclient) as MarginCollapsingState;
				PTS.ValidateHandle(marginCollapsingState);
				marginCollapsingState.Dispose();
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600697B RID: 27003 RVA: 0x001DE640 File Offset: 0x001DC840
		internal int FEqualMcsclient(IntPtr pfsclient, IntPtr pmcsclient1, IntPtr pmcsclient2, out int fEqual)
		{
			int result = 0;
			if (pmcsclient1 == IntPtr.Zero || pmcsclient2 == IntPtr.Zero)
			{
				fEqual = PTS.FromBoolean(pmcsclient1 == pmcsclient2);
			}
			else
			{
				try
				{
					MarginCollapsingState marginCollapsingState = this.PtsContext.HandleToObject(pmcsclient1) as MarginCollapsingState;
					PTS.ValidateHandle(marginCollapsingState);
					MarginCollapsingState marginCollapsingState2 = this.PtsContext.HandleToObject(pmcsclient2) as MarginCollapsingState;
					PTS.ValidateHandle(marginCollapsingState2);
					fEqual = PTS.FromBoolean(marginCollapsingState.IsEqual(marginCollapsingState2));
				}
				catch (Exception callbackException)
				{
					fEqual = 0;
					this.PtsContext.CallbackException = callbackException;
					result = -100002;
				}
				catch
				{
					fEqual = 0;
					this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
					result = -100002;
				}
			}
			return result;
		}

		// Token: 0x0600697C RID: 27004 RVA: 0x001DE714 File Offset: 0x001DC914
		internal int ConvertMcsclient(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, uint fswdir, IntPtr pmcsclient, int fSuppressTopSpace, out int dvr)
		{
			int result = 0;
			try
			{
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmp) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				BaseParaClient baseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as BaseParaClient;
				PTS.ValidateHandle(baseParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclient != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclient) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				baseParagraph.CollapseMargin(baseParaClient, marginCollapsingState, fswdir, PTS.ToBoolean(fSuppressTopSpace), out dvr);
			}
			catch (Exception callbackException)
			{
				dvr = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dvr = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600697D RID: 27005 RVA: 0x001DE7EC File Offset: 0x001DC9EC
		[SecurityCritical]
		internal int GetObjectHandlerInfo(IntPtr pfsclient, int idobj, IntPtr pObjectInfo)
		{
			int result = 0;
			try
			{
				if (idobj == PtsHost.FloaterParagraphId)
				{
					PtsCache.GetFloaterHandlerInfo(this, pObjectInfo);
				}
				else if (idobj == PtsHost.TableParagraphId)
				{
					PtsCache.GetTableObjHandlerInfo(this, pObjectInfo);
				}
				else
				{
					pObjectInfo = IntPtr.Zero;
				}
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600697E RID: 27006 RVA: 0x001DE878 File Offset: 0x001DCA78
		internal int CreateParaBreakingSession(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int fsdcpStart, IntPtr pfsbreakreclineclient, uint fswdir, int urStartTrack, int durTrack, int urPageLeftMargin, out IntPtr ppfsparabreakingsession, out int fParagraphJustified)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				TextParaClient textParaClient = this.PtsContext.HandleToObject(pfsparaclient) as TextParaClient;
				PTS.ValidateHandle(textParaClient);
				LineBreakRecord lineBreakRecord = null;
				if (pfsbreakreclineclient != IntPtr.Zero)
				{
					lineBreakRecord = (this.PtsContext.HandleToObject(pfsbreakreclineclient) as LineBreakRecord);
					PTS.ValidateHandle(lineBreakRecord);
				}
				OptimalBreakSession optimalBreakSession;
				bool condition;
				textParagraph.CreateOptimalBreakSession(textParaClient, fsdcpStart, durTrack, lineBreakRecord, out optimalBreakSession, out condition);
				fParagraphJustified = PTS.FromBoolean(condition);
				ppfsparabreakingsession = optimalBreakSession.Handle;
			}
			catch (Exception callbackException)
			{
				ppfsparabreakingsession = IntPtr.Zero;
				fParagraphJustified = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				ppfsparabreakingsession = IntPtr.Zero;
				fParagraphJustified = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600697F RID: 27007 RVA: 0x001DE970 File Offset: 0x001DCB70
		internal int DestroyParaBreakingSession(IntPtr pfsclient, IntPtr pfsparabreakingsession)
		{
			int result = 0;
			OptimalBreakSession optimalBreakSession = this.PtsContext.HandleToObject(pfsparabreakingsession) as OptimalBreakSession;
			PTS.ValidateHandle(optimalBreakSession);
			optimalBreakSession.Dispose();
			return result;
		}

		// Token: 0x06006980 RID: 27008 RVA: 0x001DE9A0 File Offset: 0x001DCBA0
		internal int GetTextProperties(IntPtr pfsclient, IntPtr nmp, int iArea, ref PTS.FSTXTPROPS fstxtprops)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				textParagraph.GetTextProperties(iArea, ref fstxtprops);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006981 RID: 27009 RVA: 0x001DEA24 File Offset: 0x001DCC24
		internal int GetNumberFootnotes(IntPtr pfsclient, IntPtr nmp, int fsdcpStart, int fsdcpLim, out int nFootnote)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				textParagraph.GetNumberFootnotes(fsdcpStart, fsdcpLim, out nFootnote);
			}
			catch (Exception callbackException)
			{
				nFootnote = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				nFootnote = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006982 RID: 27010 RVA: 0x001DEAB0 File Offset: 0x001DCCB0
		[SecurityCritical]
		internal unsafe int GetFootnotes(IntPtr pfsclient, IntPtr nmp, int fsdcpStart, int fsdcpLim, int nFootnotes, IntPtr* rgnmftn, int* rgdcp, out int cFootnotes)
		{
			cFootnotes = 0;
			return -10000;
		}

		// Token: 0x06006983 RID: 27011 RVA: 0x001DEABC File Offset: 0x001DCCBC
		internal int FormatDropCap(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int fSuppressTopSpace, out IntPtr pfsdropc, out int fInMargin, out int dur, out int dvr, out int cPolygons, out int cVertices, out int durText)
		{
			pfsdropc = IntPtr.Zero;
			fInMargin = (dur = (dvr = (cPolygons = (cVertices = (durText = 0)))));
			return -10000;
		}

		// Token: 0x06006984 RID: 27012 RVA: 0x001DEAF8 File Offset: 0x001DCCF8
		[SecurityCritical]
		internal unsafe int GetDropCapPolygons(IntPtr pfsclient, IntPtr pfsdropc, IntPtr nmp, uint fswdir, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough)
		{
			ccVertices = (cfspt = (fWrapThrough = 0));
			return -10000;
		}

		// Token: 0x06006985 RID: 27013 RVA: 0x001DEB1A File Offset: 0x001DCD1A
		internal int DestroyDropCap(IntPtr pfsclient, IntPtr pfsdropc)
		{
			return -10000;
		}

		// Token: 0x06006986 RID: 27014 RVA: 0x001DEB24 File Offset: 0x001DCD24
		internal int FormatBottomText(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, IntPtr pfslineLast, int dvrLine, out IntPtr pmcsclientOut)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				Line line = this.PtsContext.HandleToObject(pfslineLast) as Line;
				if (line != null)
				{
					PTS.ValidateHandle(line);
					textParagraph.FormatBottomText(iArea, fswdir, line, dvrLine, out pmcsclientOut);
				}
				else
				{
					Invariant.Assert(this.PtsContext.HandleToObject(pfslineLast) is LineBreakpoint);
					pmcsclientOut = IntPtr.Zero;
				}
			}
			catch (Exception callbackException)
			{
				pmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006987 RID: 27015 RVA: 0x001DEBFC File Offset: 0x001DCDFC
		internal int FormatLine(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, out IntPtr pfsline, out int dcpLine, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				TextParaClient textParaClient = this.PtsContext.HandleToObject(pfsparaclient) as TextParaClient;
				PTS.ValidateHandle(textParaClient);
				textParagraph.FormatLine(textParaClient, iArea, dcp, pbrlineIn, fswdir, urStartLine, durLine, urStartTrack, durTrack, urPageLeftMargin, PTS.ToBoolean(fAllowHyphenation), PTS.ToBoolean(fClearOnLeft), PTS.ToBoolean(fClearOnRight), PTS.ToBoolean(fTreatAsFirstInPara), PTS.ToBoolean(fTreatAsLastInPara), PTS.ToBoolean(fSuppressTopSpace), out pfsline, out dcpLine, out ppbrlineOut, out fForcedBroken, out fsflres, out dvrAscent, out dvrDescent, out urBBox, out durBBox, out dcpDepend, out fReformatNeighborsAsLastLine);
			}
			catch (Exception callbackException)
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (fForcedBroken = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = (fReformatNeighborsAsLastLine = 0)))))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (fForcedBroken = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = (fReformatNeighborsAsLastLine = 0)))))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006988 RID: 27016 RVA: 0x001DED8C File Offset: 0x001DCF8C
		internal int FormatLineForced(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, int dvrAvailable, out IntPtr pfsline, out int dcpLine, out IntPtr ppbrlineOut, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				TextParaClient textParaClient = this.PtsContext.HandleToObject(pfsparaclient) as TextParaClient;
				PTS.ValidateHandle(textParaClient);
				int num;
				int num2;
				textParagraph.FormatLine(textParaClient, iArea, dcp, pbrlineIn, fswdir, urStartLine, durLine, urStartTrack, durTrack, urPageLeftMargin, true, PTS.ToBoolean(fClearOnLeft), PTS.ToBoolean(fClearOnRight), PTS.ToBoolean(fTreatAsFirstInPara), PTS.ToBoolean(fTreatAsLastInPara), PTS.ToBoolean(fSuppressTopSpace), out pfsline, out dcpLine, out ppbrlineOut, out num, out fsflres, out dvrAscent, out dvrDescent, out urBBox, out durBBox, out dcpDepend, out num2);
			}
			catch (Exception callbackException)
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = 0)))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = 0)))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006989 RID: 27017 RVA: 0x001DEEF8 File Offset: 0x001DD0F8
		[SecurityCritical]
		internal unsafe int FormatLineVariants(IntPtr pfsclient, IntPtr pfsparabreakingsession, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, IntPtr lineVariantRestriction, int nLineVariantsAlloc, PTS.FSLINEVARIANT* rgfslinevariant, out int nLineVariantsActual, out int iLineVariantBest)
		{
			int result = 0;
			try
			{
				OptimalBreakSession optimalBreakSession = this.PtsContext.HandleToObject(pfsparabreakingsession) as OptimalBreakSession;
				PTS.ValidateHandle(optimalBreakSession);
				TextLineBreak textLineBreak = null;
				if (pbrlineIn != IntPtr.Zero)
				{
					LineBreakRecord lineBreakRecord = this.PtsContext.HandleToObject(pbrlineIn) as LineBreakRecord;
					PTS.ValidateHandle(lineBreakRecord);
					textLineBreak = lineBreakRecord.TextLineBreak;
				}
				IList<TextBreakpoint> list = optimalBreakSession.TextParagraph.FormatLineVariants(optimalBreakSession.TextParaClient, optimalBreakSession.TextParagraphCache, optimalBreakSession.OptimalTextSource, dcp, textLineBreak, fswdir, urStartLine, durLine, PTS.ToBoolean(fAllowHyphenation), PTS.ToBoolean(fClearOnLeft), PTS.ToBoolean(fClearOnRight), PTS.ToBoolean(fTreatAsFirstInPara), PTS.ToBoolean(fTreatAsLastInPara), PTS.ToBoolean(fSuppressTopSpace), lineVariantRestriction, out iLineVariantBest);
				for (int i = 0; i < Math.Min(list.Count, nLineVariantsAlloc); i++)
				{
					TextBreakpoint textBreakpoint = list[i];
					LineBreakpoint lineBreakpoint = new LineBreakpoint(optimalBreakSession, textBreakpoint);
					TextLineBreak textLineBreak2 = textBreakpoint.GetTextLineBreak();
					if (textLineBreak2 != null)
					{
						LineBreakRecord lineBreakRecord2 = new LineBreakRecord(optimalBreakSession.PtsContext, textLineBreak2);
						rgfslinevariant[i].pfsbreakreclineclient = lineBreakRecord2.Handle;
					}
					else
					{
						rgfslinevariant[i].pfsbreakreclineclient = IntPtr.Zero;
					}
					int dvrAscent = TextDpi.ToTextDpi(textBreakpoint.Baseline);
					int dvrDescent = TextDpi.ToTextDpi(textBreakpoint.Height - textBreakpoint.Baseline);
					optimalBreakSession.TextParagraph.CalcLineAscentDescent(dcp, ref dvrAscent, ref dvrDescent);
					rgfslinevariant[i].pfslineclient = lineBreakpoint.Handle;
					rgfslinevariant[i].dcpLine = textBreakpoint.Length;
					rgfslinevariant[i].fForceBroken = PTS.FromBoolean(textBreakpoint.IsTruncated);
					rgfslinevariant[i].fslres = optimalBreakSession.OptimalTextSource.GetFormatResultForBreakpoint(dcp, textBreakpoint);
					rgfslinevariant[i].dvrAscent = dvrAscent;
					rgfslinevariant[i].dvrDescent = dvrDescent;
					rgfslinevariant[i].fReformatNeighborsAsLastLine = 0;
					rgfslinevariant[i].ptsLinePenaltyInfo = textBreakpoint.GetTextPenaltyResource().Value;
				}
				nLineVariantsActual = list.Count;
			}
			catch (Exception callbackException)
			{
				nLineVariantsActual = 0;
				iLineVariantBest = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				nLineVariantsActual = 0;
				iLineVariantBest = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600698A RID: 27018 RVA: 0x001DF1B0 File Offset: 0x001DD3B0
		internal int ReconstructLineVariant(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcpStart, IntPtr pbrlineIn, int dcpLine, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, out IntPtr pfsline, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				TextParaClient textParaClient = this.PtsContext.HandleToObject(pfsparaclient) as TextParaClient;
				PTS.ValidateHandle(textParaClient);
				textParagraph.ReconstructLineVariant(textParaClient, iArea, dcpStart, pbrlineIn, dcpLine, fswdir, urStartLine, durLine, urStartTrack, durTrack, urPageLeftMargin, PTS.ToBoolean(fAllowHyphenation), PTS.ToBoolean(fClearOnLeft), PTS.ToBoolean(fClearOnRight), PTS.ToBoolean(fTreatAsFirstInPara), PTS.ToBoolean(fTreatAsLastInPara), PTS.ToBoolean(fSuppressTopSpace), out pfsline, out dcpLine, out ppbrlineOut, out fForcedBroken, out fsflres, out dvrAscent, out dvrDescent, out urBBox, out durBBox, out dcpDepend, out fReformatNeighborsAsLastLine);
			}
			catch (Exception callbackException)
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (fForcedBroken = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = (fReformatNeighborsAsLastLine = 0)))))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pfsline = (ppbrlineOut = IntPtr.Zero);
				dcpLine = (fForcedBroken = (dvrAscent = (dvrDescent = (urBBox = (durBBox = (dcpDepend = (fReformatNeighborsAsLastLine = 0)))))));
				fsflres = PTS.FSFLRES.fsflrOutOfSpace;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600698B RID: 27019 RVA: 0x001DF340 File Offset: 0x001DD540
		internal int DestroyLine(IntPtr pfsclient, IntPtr pfsline)
		{
			UnmanagedHandle unmanagedHandle = (UnmanagedHandle)this.PtsContext.HandleToObject(pfsline);
			unmanagedHandle.Dispose();
			return 0;
		}

		// Token: 0x0600698C RID: 27020 RVA: 0x001DF368 File Offset: 0x001DD568
		internal int DuplicateLineBreakRecord(IntPtr pfsclient, IntPtr pbrlineIn, out IntPtr pbrlineDup)
		{
			int result = 0;
			try
			{
				LineBreakRecord lineBreakRecord = this.PtsContext.HandleToObject(pbrlineIn) as LineBreakRecord;
				PTS.ValidateHandle(lineBreakRecord);
				pbrlineDup = lineBreakRecord.Clone().Handle;
			}
			catch (Exception callbackException)
			{
				pbrlineDup = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pbrlineDup = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600698D RID: 27021 RVA: 0x001DF3FC File Offset: 0x001DD5FC
		internal int DestroyLineBreakRecord(IntPtr pfsclient, IntPtr pbrlineIn)
		{
			int result = 0;
			try
			{
				LineBreakRecord lineBreakRecord = this.PtsContext.HandleToObject(pbrlineIn) as LineBreakRecord;
				PTS.ValidateHandle(lineBreakRecord);
				lineBreakRecord.Dispose();
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600698E RID: 27022 RVA: 0x001DF47C File Offset: 0x001DD67C
		internal int SnapGridVertical(IntPtr pfsclient, uint fswdir, int vrMargin, int vrCurrent, out int vrNew)
		{
			vrNew = 0;
			return -10000;
		}

		// Token: 0x0600698F RID: 27023 RVA: 0x001DF488 File Offset: 0x001DD688
		internal int GetDvrSuppressibleBottomSpace(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsline, uint fswdir, out int dvrSuppressible)
		{
			int result = 0;
			try
			{
				Line line = this.PtsContext.HandleToObject(pfsline) as Line;
				if (line != null)
				{
					PTS.ValidateHandle(line);
					line.GetDvrSuppressibleBottomSpace(out dvrSuppressible);
				}
				else
				{
					dvrSuppressible = 0;
				}
			}
			catch (Exception callbackException)
			{
				dvrSuppressible = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dvrSuppressible = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006990 RID: 27024 RVA: 0x001DF51C File Offset: 0x001DD71C
		internal int GetDvrAdvance(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int dcp, uint fswdir, out int dvr)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				textParagraph.GetDvrAdvance(dcp, fswdir, out dvr);
			}
			catch (Exception callbackException)
			{
				dvr = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dvr = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006991 RID: 27025 RVA: 0x001DF5AC File Offset: 0x001DD7AC
		internal int UpdGetChangeInText(IntPtr pfsclient, IntPtr nmp, out int dcpStart, out int ddcpOld, out int ddcpNew)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				textParagraph.UpdGetChangeInText(out dcpStart, out ddcpOld, out ddcpNew);
			}
			catch (Exception callbackException)
			{
				dcpStart = (ddcpOld = (ddcpNew = 0));
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dcpStart = (ddcpOld = (ddcpNew = 0));
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006992 RID: 27026 RVA: 0x001DE4D0 File Offset: 0x001DC6D0
		internal int UpdGetDropCapChange(IntPtr pfsclient, IntPtr nmp, out int fChanged)
		{
			fChanged = 0;
			return -10000;
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x001DF650 File Offset: 0x001DD850
		internal int FInterruptFormattingText(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int dcp, int vr, out int fInterruptFormatting)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				fInterruptFormatting = PTS.FromBoolean(textParagraph.InterruptFormatting(dcp, vr));
			}
			catch (Exception callbackException)
			{
				fInterruptFormatting = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fInterruptFormatting = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006994 RID: 27028 RVA: 0x001DF6E4 File Offset: 0x001DD8E4
		internal int GetTextParaCache(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fSuppressTopSpace, out int fFound, out int dcpPara, out int urBBox, out int durBBox, out int dvrPara, out PTS.FSKCLEAR fskclear, out IntPtr pmcsclientAfterPara, out int cLines, out int fOptimalLines, out int fOptimalLineDcpsCached, out int dvrMinLineHeight)
		{
			fFound = 0;
			dcpPara = (urBBox = (durBBox = (dvrPara = (cLines = (dvrMinLineHeight = (fOptimalLines = (fOptimalLineDcpsCached = 0)))))));
			pmcsclientAfterPara = IntPtr.Zero;
			fskclear = PTS.FSKCLEAR.fskclearNone;
			return 0;
		}

		// Token: 0x06006995 RID: 27029 RVA: 0x0000B02A File Offset: 0x0000922A
		[SecurityCritical]
		internal unsafe int SetTextParaCache(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fSuppressTopSpace, int dcpPara, int urBBox, int durBBox, int dvrPara, PTS.FSKCLEAR fskclear, IntPtr pmcsclientAfterPara, int cLines, int fOptimalLines, int* rgdcpOptimalLines, int dvrMinLineHeight)
		{
			return 0;
		}

		// Token: 0x06006996 RID: 27030 RVA: 0x001DEB1A File Offset: 0x001DCD1A
		[SecurityCritical]
		internal unsafe int GetOptimalLineDcpCache(IntPtr pfsclient, int cLines, int* rgdcp)
		{
			return -10000;
		}

		// Token: 0x06006997 RID: 27031 RVA: 0x001DF730 File Offset: 0x001DD930
		internal int GetNumberAttachedObjectsBeforeTextLine(IntPtr pfsclient, IntPtr nmp, int dcpFirst, out int cAttachedObjects)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				int lastDcpAttachedObjectBeforeLine = textParagraph.GetLastDcpAttachedObjectBeforeLine(dcpFirst);
				cAttachedObjects = textParagraph.GetAttachedObjectCount(dcpFirst, lastDcpAttachedObjectBeforeLine);
				return 0;
			}
			catch (Exception callbackException)
			{
				cAttachedObjects = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				cAttachedObjects = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006998 RID: 27032 RVA: 0x001DF7CC File Offset: 0x001DD9CC
		[SecurityCritical]
		internal unsafe int GetAttachedObjectsBeforeTextLine(IntPtr pfsclient, IntPtr nmp, int dcpFirst, int nAttachedObjects, IntPtr* rgnmpAttachedObject, int* rgidobj, int* rgdcpAnchor, out int cObjects, out int fEndOfParagraph)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				int lastDcpAttachedObjectBeforeLine = textParagraph.GetLastDcpAttachedObjectBeforeLine(dcpFirst);
				List<AttachedObject> attachedObjects = textParagraph.GetAttachedObjects(dcpFirst, lastDcpAttachedObjectBeforeLine);
				for (int i = 0; i < attachedObjects.Count; i++)
				{
					if (attachedObjects[i] is FigureObject)
					{
						FigureObject figureObject = (FigureObject)attachedObjects[i];
						rgnmpAttachedObject[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = figureObject.Para.Handle;
						rgdcpAnchor[i] = figureObject.Dcp;
						rgidobj[i] = -2;
					}
					else
					{
						FloaterObject floaterObject = (FloaterObject)attachedObjects[i];
						rgnmpAttachedObject[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = floaterObject.Para.Handle;
						rgdcpAnchor[i] = floaterObject.Dcp;
						rgidobj[i] = PtsHost.FloaterParagraphId;
					}
				}
				cObjects = attachedObjects.Count;
				fEndOfParagraph = 0;
			}
			catch (Exception callbackException)
			{
				cObjects = 0;
				fEndOfParagraph = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				cObjects = 0;
				fEndOfParagraph = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x06006999 RID: 27033 RVA: 0x001DF92C File Offset: 0x001DDB2C
		internal int GetNumberAttachedObjectsInTextLine(IntPtr pfsclient, IntPtr pfsline, IntPtr nmp, int dcpFirst, int dcpLim, int fFoundAttachedObjectsBeforeLine, int dcpMaxAnchorAttachedObjectBeforeLine, out int cAttachedObjects)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				LineBase lineBase = this.PtsContext.HandleToObject(pfsline) as LineBase;
				if (lineBase == null)
				{
					LineBreakpoint lineBreakpoint = this.PtsContext.HandleToObject(pfsline) as LineBreakpoint;
					PTS.ValidateHandle(lineBreakpoint);
					lineBase = lineBreakpoint.OptimalBreakSession.OptimalTextSource;
				}
				if (lineBase.HasFigures || lineBase.HasFloaters)
				{
					int lastDcpAttachedObjectBeforeLine = textParagraph.GetLastDcpAttachedObjectBeforeLine(dcpFirst);
					cAttachedObjects = textParagraph.GetAttachedObjectCount(lastDcpAttachedObjectBeforeLine, dcpLim);
				}
				else
				{
					cAttachedObjects = 0;
				}
				return 0;
			}
			catch (Exception callbackException)
			{
				cAttachedObjects = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				cAttachedObjects = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600699A RID: 27034 RVA: 0x001DFA1C File Offset: 0x001DDC1C
		[SecurityCritical]
		internal unsafe int GetAttachedObjectsInTextLine(IntPtr pfsclient, IntPtr pfsline, IntPtr nmp, int dcpFirst, int dcpLim, int fFoundAttachedObjectsBeforeLine, int dcpMaxAnchorAttachedObjectBeforeLine, int nAttachedObjects, IntPtr* rgnmpAttachedObject, int* rgidobj, int* rgdcpAnchor, out int cObjects)
		{
			int result = 0;
			try
			{
				TextParagraph textParagraph = this.PtsContext.HandleToObject(nmp) as TextParagraph;
				PTS.ValidateHandle(textParagraph);
				int lastDcpAttachedObjectBeforeLine = textParagraph.GetLastDcpAttachedObjectBeforeLine(dcpFirst);
				List<AttachedObject> attachedObjects = textParagraph.GetAttachedObjects(lastDcpAttachedObjectBeforeLine, dcpLim);
				for (int i = 0; i < attachedObjects.Count; i++)
				{
					if (attachedObjects[i] is FigureObject)
					{
						FigureObject figureObject = (FigureObject)attachedObjects[i];
						rgnmpAttachedObject[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = figureObject.Para.Handle;
						rgdcpAnchor[i] = figureObject.Dcp;
						rgidobj[i] = -2;
					}
					else
					{
						FloaterObject floaterObject = (FloaterObject)attachedObjects[i];
						rgnmpAttachedObject[(IntPtr)i * (IntPtr)sizeof(IntPtr) / (IntPtr)sizeof(IntPtr)] = floaterObject.Para.Handle;
						rgdcpAnchor[i] = floaterObject.Dcp;
						rgidobj[i] = PtsHost.FloaterParagraphId;
					}
				}
				cObjects = attachedObjects.Count;
			}
			catch (Exception callbackException)
			{
				cObjects = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				cObjects = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600699B RID: 27035 RVA: 0x001DFB74 File Offset: 0x001DDD74
		internal int UpdGetAttachedObjectChange(IntPtr pfsclient, IntPtr nmp, IntPtr nmpObject, out PTS.FSKCHANGE fskchObject)
		{
			int result = 0;
			try
			{
				BaseParagraph baseParagraph = this.PtsContext.HandleToObject(nmpObject) as BaseParagraph;
				PTS.ValidateHandle(baseParagraph);
				int num;
				baseParagraph.UpdGetParaChange(out fskchObject, out num);
			}
			catch (Exception callbackException)
			{
				fskchObject = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fskchObject = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600699C RID: 27036 RVA: 0x001DFC00 File Offset: 0x001DDE00
		internal int GetDurFigureAnchor(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsparaclientFigure, IntPtr pfsline, IntPtr nmpFigure, uint fswdir, IntPtr pfsfmtlinein, out int dur)
		{
			int result = 0;
			try
			{
				Line line = this.PtsContext.HandleToObject(pfsline) as Line;
				if (line != null)
				{
					PTS.ValidateHandle(line);
					FigureParagraph figureParagraph = this.PtsContext.HandleToObject(nmpFigure) as FigureParagraph;
					PTS.ValidateHandle(figureParagraph);
					line.GetDurFigureAnchor(figureParagraph, fswdir, out dur);
				}
				else
				{
					Invariant.Assert(false);
					dur = 0;
				}
			}
			catch (Exception callbackException)
			{
				dur = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dur = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600699D RID: 27037 RVA: 0x001DFCB8 File Offset: 0x001DDEB8
		internal int GetFloaterProperties(IntPtr pfsclient, IntPtr nmFloater, uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				floaterBaseParagraph.GetFloaterProperties(fswdirTrack, out fsfloaterprops);
			}
			catch (Exception callbackException)
			{
				fsfloaterprops = default(PTS.FSFLOATERPROPS);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfloaterprops = default(PTS.FSFLOATERPROPS);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600699E RID: 27038 RVA: 0x001DFD4C File Offset: 0x001DDF4C
		[SecurityCritical]
		internal int FormatFloaterContentFinite(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsbrkFloaterContentIn, int fBreakRecordFromPreviousPage, IntPtr nmFloater, IntPtr pftnrej, int fEmptyOk, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsFloatContent, out IntPtr pbrkrecpara, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as FloaterBaseParaClient;
				PTS.ValidateHandle(floaterBaseParaClient);
				floaterBaseParagraph.FormatFloaterContentFinite(floaterBaseParaClient, pfsbrkFloaterContentIn, fBreakRecordFromPreviousPage, pftnrej, fEmptyOk, fSuppressTopSpace, fswdirTrack, fAtMaxWidth, durAvailable, dvrAvailable, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfsFloatContent, out pbrkrecpara, out durFloaterWidth, out dvrFloaterHeight, out fsbbox, out cPolygons, out cVertices);
			}
			catch (Exception callbackException)
			{
				fsfmtr = default(PTS.FSFMTR);
				pfsFloatContent = (pbrkrecpara = IntPtr.Zero);
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtr = default(PTS.FSFMTR);
				pfsFloatContent = (pbrkrecpara = IntPtr.Zero);
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x0600699F RID: 27039 RVA: 0x001DFE84 File Offset: 0x001DE084
		internal int FormatFloaterContentBottomless(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsFloatContent, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as FloaterBaseParaClient;
				PTS.ValidateHandle(floaterBaseParaClient);
				floaterBaseParagraph.FormatFloaterContentBottomless(floaterBaseParaClient, fSuppressTopSpace, fswdirTrack, fAtMaxWidth, durAvailable, dvrAvailable, out fsfmtrbl, out pfsFloatContent, out durFloaterWidth, out dvrFloaterHeight, out fsbbox, out cPolygons, out cVertices);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfsFloatContent = IntPtr.Zero;
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfsFloatContent = IntPtr.Zero;
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069A0 RID: 27040 RVA: 0x001DFF98 File Offset: 0x001DE198
		internal int UpdateBottomlessFloaterContent(IntPtr pfsFloaterContent, IntPtr pfsparaclient, IntPtr nmFloater, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as FloaterBaseParaClient;
				PTS.ValidateHandle(floaterBaseParaClient);
				floaterBaseParagraph.UpdateBottomlessFloaterContent(floaterBaseParaClient, fSuppressTopSpace, fswdirTrack, fAtMaxWidth, durAvailable, dvrAvailable, pfsFloaterContent, out fsfmtrbl, out durFloaterWidth, out dvrFloaterHeight, out fsbbox, out cPolygons, out cVertices);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				durFloaterWidth = (dvrFloaterHeight = (cPolygons = (cVertices = 0)));
				fsbbox = default(PTS.FSBBOX);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069A1 RID: 27041 RVA: 0x001E009C File Offset: 0x001DE29C
		[SecurityCritical]
		internal unsafe int GetFloaterPolygons(IntPtr pfsparaclient, IntPtr pfsFloaterContent, IntPtr nmFloater, uint fswdirTrack, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsparaclient) as FloaterBaseParaClient;
				PTS.ValidateHandle(floaterBaseParaClient);
				floaterBaseParagraph.GetFloaterPolygons(floaterBaseParaClient, fswdirTrack, ncVertices, nfspt, rgcVertices, out ccVertices, rgfspt, out cfspt, out fWrapThrough);
			}
			catch (Exception callbackException)
			{
				ccVertices = (cfspt = (fWrapThrough = 0));
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				ccVertices = (cfspt = (fWrapThrough = 0));
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069A2 RID: 27042 RVA: 0x001E016C File Offset: 0x001DE36C
		[SecurityCritical]
		internal int ClearUpdateInfoInFloaterContent(IntPtr pfsFloaterContent)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContent))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsFloaterContent) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient)
				{
					return 0;
				}
			}
			return PTS.FsClearUpdateInfoInSubpage(this.Context, pfsFloaterContent);
		}

		// Token: 0x060069A3 RID: 27043 RVA: 0x001E01B0 File Offset: 0x001DE3B0
		[SecurityCritical]
		internal int CompareFloaterContents(IntPtr pfsFloaterContentOld, IntPtr pfsFloaterContentNew, out PTS.FSCOMPRESULT fscmpr)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContentOld) && this.PtsContext.IsValidHandle(pfsFloaterContentNew))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsFloaterContentOld) as FloaterBaseParaClient;
				FloaterBaseParaClient floaterBaseParaClient2 = this.PtsContext.HandleToObject(pfsFloaterContentNew) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient && !(floaterBaseParaClient2 is UIElementParaClient))
				{
					fscmpr = PTS.FSCOMPRESULT.fscmprChangeInside;
					return 0;
				}
				if (floaterBaseParaClient2 is UIElementParaClient && !(floaterBaseParaClient is UIElementParaClient))
				{
					fscmpr = PTS.FSCOMPRESULT.fscmprChangeInside;
					return 0;
				}
				if (floaterBaseParaClient is UIElementParaClient && floaterBaseParaClient2 is UIElementParaClient)
				{
					if (pfsFloaterContentOld.Equals(pfsFloaterContentNew))
					{
						fscmpr = PTS.FSCOMPRESULT.fscmprNoChange;
						return 0;
					}
					fscmpr = PTS.FSCOMPRESULT.fscmprChangeInside;
					return 0;
				}
			}
			return PTS.FsCompareSubpages(this.Context, pfsFloaterContentOld, pfsFloaterContentNew, out fscmpr);
		}

		// Token: 0x060069A4 RID: 27044 RVA: 0x001E0264 File Offset: 0x001DE464
		[SecurityCritical]
		internal int DestroyFloaterContent(IntPtr pfsFloaterContent)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContent))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsFloaterContent) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient)
				{
					return 0;
				}
			}
			return PTS.FsDestroySubpage(this.Context, pfsFloaterContent);
		}

		// Token: 0x060069A5 RID: 27045 RVA: 0x001E02A8 File Offset: 0x001DE4A8
		[SecurityCritical]
		internal int DuplicateFloaterContentBreakRecord(IntPtr pfsclient, IntPtr pfsbrkFloaterContent, out IntPtr pfsbrkFloaterContentDup)
		{
			if (this.PtsContext.IsValidHandle(pfsbrkFloaterContent))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsbrkFloaterContent) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient)
				{
					Invariant.Assert(false, "Embedded UIElement should not have break record");
				}
			}
			return PTS.FsDuplicateSubpageBreakRecord(this.Context, pfsbrkFloaterContent, out pfsbrkFloaterContentDup);
		}

		// Token: 0x060069A6 RID: 27046 RVA: 0x001E02F8 File Offset: 0x001DE4F8
		[SecurityCritical]
		internal int DestroyFloaterContentBreakRecord(IntPtr pfsclient, IntPtr pfsbrkFloaterContent)
		{
			if (this.PtsContext.IsValidHandle(pfsbrkFloaterContent))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsbrkFloaterContent) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient)
				{
					Invariant.Assert(false, "Embedded UIElement should not have break record");
				}
			}
			return PTS.FsDestroySubpageBreakRecord(this.Context, pfsbrkFloaterContent);
		}

		// Token: 0x060069A7 RID: 27047 RVA: 0x001E0344 File Offset: 0x001DE544
		[SecurityCritical]
		internal int GetFloaterContentColumnBalancingInfo(IntPtr pfsFloaterContent, uint fswdir, out int nlines, out int dvrSumHeight, out int dvrMinHeight)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContent))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsFloaterContent) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient)
				{
					if (((BlockUIContainer)floaterBaseParaClient.Paragraph.Element).Child != null)
					{
						nlines = 1;
						UIElement child = ((BlockUIContainer)floaterBaseParaClient.Paragraph.Element).Child;
						dvrSumHeight = TextDpi.ToTextDpi(child.DesiredSize.Height);
						dvrMinHeight = TextDpi.ToTextDpi(child.DesiredSize.Height);
					}
					else
					{
						nlines = 0;
						dvrSumHeight = (dvrMinHeight = 0);
					}
					return 0;
				}
			}
			uint num;
			return PTS.FsGetSubpageColumnBalancingInfo(this.Context, pfsFloaterContent, out num, out nlines, out dvrSumHeight, out dvrMinHeight);
		}

		// Token: 0x060069A8 RID: 27048 RVA: 0x001E0400 File Offset: 0x001DE600
		[SecurityCritical]
		internal int GetFloaterContentNumberFootnotes(IntPtr pfsFloaterContent, out int cftn)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContent))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsFloaterContent) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient)
				{
					cftn = 0;
					return 0;
				}
			}
			return PTS.FsGetNumberSubpageFootnotes(this.Context, pfsFloaterContent, out cftn);
		}

		// Token: 0x060069A9 RID: 27049 RVA: 0x001E0447 File Offset: 0x001DE647
		internal int GetFloaterContentFootnoteInfo(IntPtr pfsFloaterContent, uint fswdir, int nftn, int iftnFirst, ref PTS.FSFTNINFO fsftninf, out int iftnLim)
		{
			iftnLim = 0;
			return 0;
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x001E0450 File Offset: 0x001DE650
		[SecurityCritical]
		internal int TransferDisplayInfoInFloaterContent(IntPtr pfsFloaterContentOld, IntPtr pfsFloaterContentNew)
		{
			if (this.PtsContext.IsValidHandle(pfsFloaterContentOld) && this.PtsContext.IsValidHandle(pfsFloaterContentNew))
			{
				FloaterBaseParaClient floaterBaseParaClient = this.PtsContext.HandleToObject(pfsFloaterContentOld) as FloaterBaseParaClient;
				FloaterBaseParaClient floaterBaseParaClient2 = this.PtsContext.HandleToObject(pfsFloaterContentNew) as FloaterBaseParaClient;
				if (floaterBaseParaClient is UIElementParaClient || floaterBaseParaClient2 is UIElementParaClient)
				{
					return 0;
				}
			}
			return PTS.FsTransferDisplayInfoSubpage(this.PtsContext.Context, pfsFloaterContentOld, pfsFloaterContentNew);
		}

		// Token: 0x060069AB RID: 27051 RVA: 0x001E04C4 File Offset: 0x001DE6C4
		internal int GetMCSClientAfterFloater(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, uint fswdirTrack, IntPtr pmcsclientIn, out IntPtr pmcsclientOut)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				floaterBaseParagraph.GetMCSClientAfterFloater(fswdirTrack, marginCollapsingState, out pmcsclientOut);
			}
			catch (Exception callbackException)
			{
				pmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069AC RID: 27052 RVA: 0x001E0584 File Offset: 0x001DE784
		internal int GetDvrUsedForFloater(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, uint fswdirTrack, IntPtr pmcsclientIn, int dvrDisplaced, out int dvrUsed)
		{
			int result = 0;
			try
			{
				FloaterBaseParagraph floaterBaseParagraph = this.PtsContext.HandleToObject(nmFloater) as FloaterBaseParagraph;
				PTS.ValidateHandle(floaterBaseParagraph);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				floaterBaseParagraph.GetDvrUsedForFloater(fswdirTrack, marginCollapsingState, dvrDisplaced, out dvrUsed);
			}
			catch (Exception callbackException)
			{
				dvrUsed = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dvrUsed = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069AD RID: 27053 RVA: 0x001E063C File Offset: 0x001DE83C
		internal int SubtrackCreateContext(IntPtr pfsclient, IntPtr pfsc, IntPtr pfscbkobj, uint ffi, int idobj, out IntPtr pfssobjc)
		{
			pfssobjc = (IntPtr)(idobj + PtsHost._objectContextOffset);
			return 0;
		}

		// Token: 0x060069AE RID: 27054 RVA: 0x0000B02A File Offset: 0x0000922A
		internal int SubtrackDestroyContext(IntPtr pfssobjc)
		{
			return 0;
		}

		// Token: 0x060069AF RID: 27055 RVA: 0x001E0650 File Offset: 0x001DE850
		[SecurityCritical]
		internal int SubtrackFormatParaFinite(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr pfsobjbrk, int fBreakRecordFromPreviousPage, IntPtr nmp, int iArea, IntPtr pftnrej, IntPtr pfsgeom, int fEmptyOk, int fSuppressTopSpace, uint fswdir, ref PTS.FSRECT fsrcToFill, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, int fBreakInside, out PTS.FSFMTR fsfmtr, out IntPtr pfspara, out IntPtr pbrkrecpara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fBreakInsidePossible)
		{
			int result = 0;
			fBreakInsidePossible = 0;
			try
			{
				ContainerParagraph containerParagraph = this.PtsContext.HandleToObject(nmp) as ContainerParagraph;
				PTS.ValidateHandle(containerParagraph);
				ContainerParaClient containerParaClient = this.PtsContext.HandleToObject(pfsparaclient) as ContainerParaClient;
				PTS.ValidateHandle(containerParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				containerParagraph.FormatParaFinite(containerParaClient, pfsobjbrk, fBreakRecordFromPreviousPage, iArea, pftnrej, pfsgeom, fEmptyOk, fSuppressTopSpace, fswdir, ref fsrcToFill, marginCollapsingState, fskclearIn, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfspara, out pbrkrecpara, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace);
			}
			catch (Exception callbackException)
			{
				fsfmtr = default(PTS.FSFMTR);
				pfspara = (pbrkrecpara = (pmcsclientOut = IntPtr.Zero));
				dvrUsed = (dvrTopSpace = 0);
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtr = default(PTS.FSFMTR);
				pfspara = (pbrkrecpara = (pmcsclientOut = IntPtr.Zero));
				dvrUsed = (dvrTopSpace = 0);
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069B0 RID: 27056 RVA: 0x001E07B4 File Offset: 0x001DE9B4
		[SecurityCritical]
		internal int SubtrackFormatParaBottomless(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfspara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			int result = 0;
			try
			{
				ContainerParagraph containerParagraph = this.PtsContext.HandleToObject(nmp) as ContainerParagraph;
				PTS.ValidateHandle(containerParagraph);
				ContainerParaClient containerParaClient = this.PtsContext.HandleToObject(pfsparaclient) as ContainerParaClient;
				PTS.ValidateHandle(containerParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				containerParagraph.FormatParaBottomless(containerParaClient, iArea, pfsgeom, fSuppressTopSpace, fswdir, urTrack, durTrack, vrTrack, marginCollapsingState, fskclearIn, fInterruptable, out fsfmtrbl, out pfspara, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace, out fPageBecomesUninterruptable);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069B1 RID: 27057 RVA: 0x001E0908 File Offset: 0x001DEB08
		[SecurityCritical]
		internal int SubtrackUpdateBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			int result = 0;
			try
			{
				ContainerParagraph containerParagraph = this.PtsContext.HandleToObject(nmp) as ContainerParagraph;
				PTS.ValidateHandle(containerParagraph);
				ContainerParaClient containerParaClient = this.PtsContext.HandleToObject(pfsparaclient) as ContainerParaClient;
				PTS.ValidateHandle(containerParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				containerParagraph.UpdateBottomlessPara(pfspara, containerParaClient, iArea, pfsgeom, fSuppressTopSpace, fswdir, urTrack, durTrack, vrTrack, marginCollapsingState, fskclearIn, fInterruptable, out fsfmtrbl, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace, out fPageBecomesUninterruptable);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069B2 RID: 27058 RVA: 0x001E0A58 File Offset: 0x001DEC58
		[SecurityCritical]
		internal int SubtrackSynchronizeBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsgeom, uint fswdir, int dvrShift)
		{
			int result = 0;
			try
			{
				ContainerParaClient handle = this.PtsContext.HandleToObject(pfsparaclient) as ContainerParaClient;
				PTS.ValidateHandle(handle);
				PTS.Validate(PTS.FsSynchronizeBottomlessSubtrack(this.Context, pfspara, pfsgeom, fswdir, dvrShift), this.PtsContext);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069B3 RID: 27059 RVA: 0x001E0AF0 File Offset: 0x001DECF0
		[SecurityCritical]
		internal int SubtrackComparePara(IntPtr pfsparaclientOld, IntPtr pfsparaOld, IntPtr pfsparaclientNew, IntPtr pfsparaNew, uint fswdir, out PTS.FSCOMPRESULT fscmpr, out int dvrShifted)
		{
			return PTS.FsCompareSubtrack(this.Context, pfsparaOld, pfsparaNew, fswdir, out fscmpr, out dvrShifted);
		}

		// Token: 0x060069B4 RID: 27060 RVA: 0x001E0B06 File Offset: 0x001DED06
		[SecurityCritical]
		internal int SubtrackClearUpdateInfoInPara(IntPtr pfspara)
		{
			return PTS.FsClearUpdateInfoInSubtrack(this.Context, pfspara);
		}

		// Token: 0x060069B5 RID: 27061 RVA: 0x001E0B14 File Offset: 0x001DED14
		[SecurityCritical]
		internal int SubtrackDestroyPara(IntPtr pfspara)
		{
			return PTS.FsDestroySubtrack(this.Context, pfspara);
		}

		// Token: 0x060069B6 RID: 27062 RVA: 0x001E0B22 File Offset: 0x001DED22
		[SecurityCritical]
		internal int SubtrackDuplicateBreakRecord(IntPtr pfssobjc, IntPtr pfsbrkrecparaOrig, out IntPtr pfsbrkrecparaDup)
		{
			return PTS.FsDuplicateSubtrackBreakRecord(this.Context, pfsbrkrecparaOrig, out pfsbrkrecparaDup);
		}

		// Token: 0x060069B7 RID: 27063 RVA: 0x001E0B31 File Offset: 0x001DED31
		[SecurityCritical]
		internal int SubtrackDestroyBreakRecord(IntPtr pfssobjc, IntPtr pfsobjbrk)
		{
			return PTS.FsDestroySubtrackBreakRecord(this.Context, pfsobjbrk);
		}

		// Token: 0x060069B8 RID: 27064 RVA: 0x001E0B3F File Offset: 0x001DED3F
		[SecurityCritical]
		internal int SubtrackGetColumnBalancingInfo(IntPtr pfspara, uint fswdir, out int nlines, out int dvrSumHeight, out int dvrMinHeight)
		{
			return PTS.FsGetSubtrackColumnBalancingInfo(this.Context, pfspara, fswdir, out nlines, out dvrSumHeight, out dvrMinHeight);
		}

		// Token: 0x060069B9 RID: 27065 RVA: 0x001E0B53 File Offset: 0x001DED53
		[SecurityCritical]
		internal int SubtrackGetNumberFootnotes(IntPtr pfspara, out int nftn)
		{
			return PTS.FsGetNumberSubtrackFootnotes(this.Context, pfspara, out nftn);
		}

		// Token: 0x060069BA RID: 27066 RVA: 0x001DDECC File Offset: 0x001DC0CC
		[SecurityCritical]
		internal unsafe int SubtrackGetFootnoteInfo(IntPtr pfspara, uint fswdir, int nftn, int iftnFirst, PTS.FSFTNINFO* pfsftninf, out int iftnLim)
		{
			iftnLim = 0;
			return -10000;
		}

		// Token: 0x060069BB RID: 27067 RVA: 0x001E0B62 File Offset: 0x001DED62
		internal int SubtrackShiftVertical(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsshift, uint fswdir, out PTS.FSBBOX pfsbbox)
		{
			pfsbbox = default(PTS.FSBBOX);
			return 0;
		}

		// Token: 0x060069BC RID: 27068 RVA: 0x001E0B6D File Offset: 0x001DED6D
		[SecurityCritical]
		internal int SubtrackTransferDisplayInfoPara(IntPtr pfsparaOld, IntPtr pfsparaNew)
		{
			return PTS.FsTransferDisplayInfoSubtrack(this.Context, pfsparaOld, pfsparaNew);
		}

		// Token: 0x060069BD RID: 27069 RVA: 0x001E0B7C File Offset: 0x001DED7C
		internal int SubpageCreateContext(IntPtr pfsclient, IntPtr pfsc, IntPtr pfscbkobj, uint ffi, int idobj, out IntPtr pfssobjc)
		{
			pfssobjc = (IntPtr)(idobj + PtsHost._objectContextOffset + 1);
			return 0;
		}

		// Token: 0x060069BE RID: 27070 RVA: 0x0000B02A File Offset: 0x0000922A
		internal int SubpageDestroyContext(IntPtr pfssobjc)
		{
			return 0;
		}

		// Token: 0x060069BF RID: 27071 RVA: 0x001E0B94 File Offset: 0x001DED94
		[SecurityCritical]
		internal int SubpageFormatParaFinite(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr pfsobjbrk, int fBreakRecordFromPreviousPage, IntPtr nmp, int iArea, IntPtr pftnrej, IntPtr pfsgeom, int fEmptyOk, int fSuppressTopSpace, uint fswdir, ref PTS.FSRECT fsrcToFill, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, int fBreakInside, out PTS.FSFMTR fsfmtr, out IntPtr pfspara, out IntPtr pbrkrecpara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fBreakInsidePossible)
		{
			int result = 0;
			fBreakInsidePossible = 0;
			try
			{
				SubpageParagraph subpageParagraph = this.PtsContext.HandleToObject(nmp) as SubpageParagraph;
				PTS.ValidateHandle(subpageParagraph);
				SubpageParaClient subpageParaClient = this.PtsContext.HandleToObject(pfsparaclient) as SubpageParaClient;
				PTS.ValidateHandle(subpageParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				subpageParagraph.FormatParaFinite(subpageParaClient, pfsobjbrk, fBreakRecordFromPreviousPage, pftnrej, fEmptyOk, fSuppressTopSpace, fswdir, ref fsrcToFill, marginCollapsingState, fskclearIn, fsksuppresshardbreakbeforefirstparaIn, out fsfmtr, out pfspara, out pbrkrecpara, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace);
			}
			catch (Exception callbackException)
			{
				fsfmtr = default(PTS.FSFMTR);
				pfspara = (pbrkrecpara = (pmcsclientOut = IntPtr.Zero));
				dvrUsed = (dvrTopSpace = 0);
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtr = default(PTS.FSFMTR);
				pfspara = (pbrkrecpara = (pmcsclientOut = IntPtr.Zero));
				dvrUsed = (dvrTopSpace = 0);
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069C0 RID: 27072 RVA: 0x001E0CF4 File Offset: 0x001DEEF4
		internal int SubpageFormatParaBottomless(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfspara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			int result = 0;
			try
			{
				SubpageParagraph subpageParagraph = this.PtsContext.HandleToObject(nmp) as SubpageParagraph;
				PTS.ValidateHandle(subpageParagraph);
				SubpageParaClient subpageParaClient = this.PtsContext.HandleToObject(pfsparaclient) as SubpageParaClient;
				PTS.ValidateHandle(subpageParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				subpageParagraph.FormatParaBottomless(subpageParaClient, fSuppressTopSpace, fswdir, urTrack, durTrack, vrTrack, marginCollapsingState, fskclearIn, fInterruptable, out fsfmtrbl, out pfspara, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace, out fPageBecomesUninterruptable);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069C1 RID: 27073 RVA: 0x001E0E44 File Offset: 0x001DF044
		[SecurityCritical]
		internal int SubpageUpdateBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable)
		{
			int result = 0;
			try
			{
				SubpageParagraph subpageParagraph = this.PtsContext.HandleToObject(nmp) as SubpageParagraph;
				PTS.ValidateHandle(subpageParagraph);
				SubpageParaClient subpageParaClient = this.PtsContext.HandleToObject(pfsparaclient) as SubpageParaClient;
				PTS.ValidateHandle(subpageParaClient);
				MarginCollapsingState marginCollapsingState = null;
				if (pmcsclientIn != IntPtr.Zero)
				{
					marginCollapsingState = (this.PtsContext.HandleToObject(pmcsclientIn) as MarginCollapsingState);
					PTS.ValidateHandle(marginCollapsingState);
				}
				subpageParagraph.UpdateBottomlessPara(pfspara, subpageParaClient, fSuppressTopSpace, fswdir, urTrack, durTrack, vrTrack, marginCollapsingState, fskclearIn, fInterruptable, out fsfmtrbl, out dvrUsed, out fsbbox, out pmcsclientOut, out fskclearOut, out dvrTopSpace, out fPageBecomesUninterruptable);
			}
			catch (Exception callbackException)
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fsfmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				pfspara = (pmcsclientOut = IntPtr.Zero);
				dvrUsed = (dvrTopSpace = (fPageBecomesUninterruptable = 0));
				fsbbox = default(PTS.FSBBOX);
				fskclearOut = PTS.FSKCLEAR.fskclearNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069C2 RID: 27074 RVA: 0x0000B02A File Offset: 0x0000922A
		internal int SubpageSynchronizeBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsgeom, uint fswdir, int dvrShift)
		{
			return 0;
		}

		// Token: 0x060069C3 RID: 27075 RVA: 0x001E0F90 File Offset: 0x001DF190
		[SecurityCritical]
		internal int SubpageComparePara(IntPtr pfsparaclientOld, IntPtr pfsparaOld, IntPtr pfsparaclientNew, IntPtr pfsparaNew, uint fswdir, out PTS.FSCOMPRESULT fscmpr, out int dvrShifted)
		{
			dvrShifted = 0;
			return PTS.FsCompareSubpages(this.Context, pfsparaOld, pfsparaNew, out fscmpr);
		}

		// Token: 0x060069C4 RID: 27076 RVA: 0x001E0FA6 File Offset: 0x001DF1A6
		[SecurityCritical]
		internal int SubpageClearUpdateInfoInPara(IntPtr pfspara)
		{
			return PTS.FsClearUpdateInfoInSubpage(this.Context, pfspara);
		}

		// Token: 0x060069C5 RID: 27077 RVA: 0x001E0FB4 File Offset: 0x001DF1B4
		[SecurityCritical]
		internal int SubpageDestroyPara(IntPtr pfspara)
		{
			return PTS.FsDestroySubpage(this.Context, pfspara);
		}

		// Token: 0x060069C6 RID: 27078 RVA: 0x001E0FC2 File Offset: 0x001DF1C2
		[SecurityCritical]
		internal int SubpageDuplicateBreakRecord(IntPtr pfssobjc, IntPtr pfsbrkrecparaOrig, out IntPtr pfsbrkrecparaDup)
		{
			return PTS.FsDuplicateSubpageBreakRecord(this.Context, pfsbrkrecparaOrig, out pfsbrkrecparaDup);
		}

		// Token: 0x060069C7 RID: 27079 RVA: 0x001E0FD1 File Offset: 0x001DF1D1
		[SecurityCritical]
		internal int SubpageDestroyBreakRecord(IntPtr pfssobjc, IntPtr pfsobjbrk)
		{
			return PTS.FsDestroySubpageBreakRecord(this.Context, pfsobjbrk);
		}

		// Token: 0x060069C8 RID: 27080 RVA: 0x001E0FDF File Offset: 0x001DF1DF
		[SecurityCritical]
		internal int SubpageGetColumnBalancingInfo(IntPtr pfspara, uint fswdir, out int nlines, out int dvrSumHeight, out int dvrMinHeight)
		{
			return PTS.FsGetSubpageColumnBalancingInfo(this.Context, pfspara, out fswdir, out nlines, out dvrSumHeight, out dvrMinHeight);
		}

		// Token: 0x060069C9 RID: 27081 RVA: 0x001E0FF4 File Offset: 0x001DF1F4
		[SecurityCritical]
		internal int SubpageGetNumberFootnotes(IntPtr pfspara, out int nftn)
		{
			return PTS.FsGetNumberSubpageFootnotes(this.Context, pfspara, out nftn);
		}

		// Token: 0x060069CA RID: 27082 RVA: 0x001E1003 File Offset: 0x001DF203
		[SecurityCritical]
		internal unsafe int SubpageGetFootnoteInfo(IntPtr pfspara, uint fswdir, int nftn, int iftnFirst, PTS.FSFTNINFO* pfsftninf, out int iftnLim)
		{
			return PTS.FsGetSubpageFootnoteInfo(this.Context, pfspara, nftn, iftnFirst, out fswdir, pfsftninf, out iftnLim);
		}

		// Token: 0x060069CB RID: 27083 RVA: 0x001E0B62 File Offset: 0x001DED62
		internal int SubpageShiftVertical(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsshift, uint fswdir, out PTS.FSBBOX pfsbbox)
		{
			pfsbbox = default(PTS.FSBBOX);
			return 0;
		}

		// Token: 0x060069CC RID: 27084 RVA: 0x001E101A File Offset: 0x001DF21A
		[SecurityCritical]
		internal int SubpageTransferDisplayInfoPara(IntPtr pfsparaOld, IntPtr pfsparaNew)
		{
			return PTS.FsTransferDisplayInfoSubpage(this.Context, pfsparaOld, pfsparaNew);
		}

		// Token: 0x060069CD RID: 27085 RVA: 0x001E102C File Offset: 0x001DF22C
		internal int GetTableProperties(IntPtr pfsclient, IntPtr nmTable, uint fswdirTrack, out PTS.FSTABLEOBJPROPS fstableobjprops)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetTableProperties(fswdirTrack, out fstableobjprops);
			}
			catch (Exception callbackException)
			{
				fstableobjprops = default(PTS.FSTABLEOBJPROPS);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fstableobjprops = default(PTS.FSTABLEOBJPROPS);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069CE RID: 27086 RVA: 0x001E10C0 File Offset: 0x001DF2C0
		internal int AutofitTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, int durAvailableSpace, out int durTableWidth)
		{
			int result = 0;
			try
			{
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				tableParaClient.AutofitTable(fswdirTrack, durAvailableSpace, out durTableWidth);
			}
			catch (Exception callbackException)
			{
				durTableWidth = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				durTableWidth = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069CF RID: 27087 RVA: 0x001E1150 File Offset: 0x001DF350
		internal int UpdAutofitTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, int durAvailableSpace, out int durTableWidth, out int fNoChangeInCellWidths)
		{
			int result = 0;
			try
			{
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				tableParaClient.UpdAutofitTable(fswdirTrack, durAvailableSpace, out durTableWidth, out fNoChangeInCellWidths);
			}
			catch (Exception callbackException)
			{
				durTableWidth = 0;
				fNoChangeInCellWidths = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				durTableWidth = 0;
				fNoChangeInCellWidths = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069D0 RID: 27088 RVA: 0x001E11E8 File Offset: 0x001DF3E8
		internal int GetMCSClientAfterTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, IntPtr pmcsclientIn, out IntPtr ppmcsclientOut)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetMCSClientAfterTable(fswdirTrack, pmcsclientIn, out ppmcsclientOut);
			}
			catch (Exception callbackException)
			{
				ppmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				ppmcsclientOut = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069D1 RID: 27089 RVA: 0x001E1280 File Offset: 0x001DF480
		internal int GetFirstHeaderRow(IntPtr pfsclient, IntPtr nmTable, int fRepeatedHeader, out int fFound, out IntPtr pnmFirstHeaderRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetFirstHeaderRow(fRepeatedHeader, out fFound, out pnmFirstHeaderRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmFirstHeaderRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmFirstHeaderRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069D2 RID: 27090 RVA: 0x001E131C File Offset: 0x001DF51C
		internal int GetNextHeaderRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmHeaderRow, int fRepeatedHeader, out int fFound, out IntPtr pnmNextHeaderRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetNextHeaderRow(fRepeatedHeader, nmHeaderRow, out fFound, out pnmNextHeaderRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmNextHeaderRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmNextHeaderRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069D3 RID: 27091 RVA: 0x001E13BC File Offset: 0x001DF5BC
		internal int GetFirstFooterRow(IntPtr pfsclient, IntPtr nmTable, int fRepeatedFooter, out int fFound, out IntPtr pnmFirstFooterRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetFirstFooterRow(fRepeatedFooter, out fFound, out pnmFirstFooterRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmFirstFooterRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmFirstFooterRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069D4 RID: 27092 RVA: 0x001E1458 File Offset: 0x001DF658
		internal int GetNextFooterRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmFooterRow, int fRepeatedFooter, out int fFound, out IntPtr pnmNextFooterRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetNextFooterRow(fRepeatedFooter, nmFooterRow, out fFound, out pnmNextFooterRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmNextFooterRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmNextFooterRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069D5 RID: 27093 RVA: 0x001E14F8 File Offset: 0x001DF6F8
		internal int GetFirstRow(IntPtr pfsclient, IntPtr nmTable, out int fFound, out IntPtr pnmFirstRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetFirstRow(out fFound, out pnmFirstRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmFirstRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmFirstRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069D6 RID: 27094 RVA: 0x001E1590 File Offset: 0x001DF790
		internal int GetNextRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmRow, out int fFound, out IntPtr pnmNextRow)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetNextRow(nmRow, out fFound, out pnmNextRow);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				pnmNextRow = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				pnmNextRow = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069D7 RID: 27095 RVA: 0x001E162C File Offset: 0x001DF82C
		internal int UpdFChangeInHeaderFooter(IntPtr pfsclient, IntPtr nmTable, out int fHeaderChanged, out int fFooterChanged, out int fRepeatedHeaderChanged, out int fRepeatedFooterChanged)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.UpdFChangeInHeaderFooter(out fHeaderChanged, out fFooterChanged, out fRepeatedHeaderChanged, out fRepeatedFooterChanged);
			}
			catch (Exception callbackException)
			{
				fHeaderChanged = 0;
				fFooterChanged = 0;
				fRepeatedHeaderChanged = 0;
				fRepeatedFooterChanged = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fHeaderChanged = 0;
				fFooterChanged = 0;
				fRepeatedHeaderChanged = 0;
				fRepeatedFooterChanged = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069D8 RID: 27096 RVA: 0x001E16D0 File Offset: 0x001DF8D0
		internal int UpdGetFirstChangeInTable(IntPtr pfsclient, IntPtr nmTable, out int fFound, out int fChangeFirst, out IntPtr pnmRowBeforeChange)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.UpdGetFirstChangeInTable(out fFound, out fChangeFirst, out pnmRowBeforeChange);
			}
			catch (Exception callbackException)
			{
				fFound = 0;
				fChangeFirst = 0;
				pnmRowBeforeChange = IntPtr.Zero;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fFound = 0;
				fChangeFirst = 0;
				pnmRowBeforeChange = IntPtr.Zero;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069D9 RID: 27097 RVA: 0x001E1774 File Offset: 0x001DF974
		internal int UpdGetRowChange(IntPtr pfsclient, IntPtr nmTable, IntPtr nmRow, out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			int result = 0;
			try
			{
				TableParagraph handle = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(handle);
				RowParagraph rowParagraph = this.PtsContext.HandleToObject(nmRow) as RowParagraph;
				PTS.ValidateHandle(rowParagraph);
				rowParagraph.UpdGetParaChange(out fskch, out fNoFurtherChanges);
			}
			catch (Exception callbackException)
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				fNoFurtherChanges = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fskch = PTS.FSKCHANGE.fskchNone;
				fNoFurtherChanges = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069DA RID: 27098 RVA: 0x001E1820 File Offset: 0x001DFA20
		internal int UpdGetCellChange(IntPtr pfsclient, IntPtr nmRow, IntPtr nmCell, out int fWidthChanged, out PTS.FSKCHANGE fskchCell)
		{
			int result = 0;
			try
			{
				CellParagraph cellParagraph = this.PtsContext.HandleToObject(nmCell) as CellParagraph;
				PTS.ValidateHandle(cellParagraph);
				cellParagraph.UpdGetCellChange(out fWidthChanged, out fskchCell);
			}
			catch (Exception callbackException)
			{
				fWidthChanged = 0;
				fskchCell = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fWidthChanged = 0;
				fskchCell = PTS.FSKCHANGE.fskchNone;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069DB RID: 27099 RVA: 0x001E18B4 File Offset: 0x001DFAB4
		internal int GetDistributionKind(IntPtr pfsclient, IntPtr nmTable, uint fswdirTable, out PTS.FSKTABLEHEIGHTDISTRIBUTION tabledistr)
		{
			int result = 0;
			try
			{
				TableParagraph tableParagraph = this.PtsContext.HandleToObject(nmTable) as TableParagraph;
				PTS.ValidateHandle(tableParagraph);
				tableParagraph.GetDistributionKind(fswdirTable, out tabledistr);
			}
			catch (Exception callbackException)
			{
				tabledistr = PTS.FSKTABLEHEIGHTDISTRIBUTION.fskdistributeUnchanged;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				tabledistr = PTS.FSKTABLEHEIGHTDISTRIBUTION.fskdistributeUnchanged;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069DC RID: 27100 RVA: 0x001E1940 File Offset: 0x001DFB40
		internal int GetRowProperties(IntPtr pfsclient, IntPtr nmRow, uint fswdirTable, out PTS.FSTABLEROWPROPS rowprops)
		{
			int result = 0;
			try
			{
				RowParagraph rowParagraph = this.PtsContext.HandleToObject(nmRow) as RowParagraph;
				PTS.ValidateHandle(rowParagraph);
				rowParagraph.GetRowProperties(fswdirTable, out rowprops);
			}
			catch (Exception callbackException)
			{
				rowprops = default(PTS.FSTABLEROWPROPS);
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				rowprops = default(PTS.FSTABLEROWPROPS);
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069DD RID: 27101 RVA: 0x001E19D4 File Offset: 0x001DFBD4
		[SecurityCritical]
		internal unsafe int GetCells(IntPtr pfsclient, IntPtr nmRow, int cCells, IntPtr* rgnmCell, PTS.FSTABLEKCELLMERGE* rgkcellmerge)
		{
			int result = 0;
			try
			{
				RowParagraph rowParagraph = this.PtsContext.HandleToObject(nmRow) as RowParagraph;
				PTS.ValidateHandle(rowParagraph);
				rowParagraph.GetCells(cCells, rgnmCell, rgkcellmerge);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069DE RID: 27102 RVA: 0x001E1A58 File Offset: 0x001DFC58
		internal int FInterruptFormattingTable(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmRow, int dvr, out int fInterrupt)
		{
			int result = 0;
			try
			{
				RowParagraph rowParagraph = this.PtsContext.HandleToObject(nmRow) as RowParagraph;
				PTS.ValidateHandle(rowParagraph);
				rowParagraph.FInterruptFormattingTable(dvr, out fInterrupt);
			}
			catch (Exception callbackException)
			{
				fInterrupt = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fInterrupt = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069DF RID: 27103 RVA: 0x001E1AE4 File Offset: 0x001DFCE4
		[SecurityCritical]
		internal unsafe int CalcHorizontalBBoxOfRow(IntPtr pfsclient, IntPtr nmRow, int cCells, IntPtr* rgnmCell, IntPtr* rgpfscell, out int urBBox, out int durBBox)
		{
			int result = 0;
			try
			{
				RowParagraph rowParagraph = this.PtsContext.HandleToObject(nmRow) as RowParagraph;
				PTS.ValidateHandle(rowParagraph);
				rowParagraph.CalcHorizontalBBoxOfRow(cCells, rgnmCell, rgpfscell, out urBBox, out durBBox);
			}
			catch (Exception callbackException)
			{
				urBBox = 0;
				durBBox = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				urBBox = 0;
				durBBox = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069E0 RID: 27104 RVA: 0x001E1B7C File Offset: 0x001DFD7C
		[SecurityCritical]
		internal int FormatCellFinite(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr pfsbrkcell, IntPtr nmCell, IntPtr pfsFtnRejector, int fEmptyOK, uint fswdirTable, int dvrExtraHeight, int dvrAvailable, out PTS.FSFMTR pfmtr, out IntPtr ppfscell, out IntPtr pfsbrkcellOut, out int dvrUsed)
		{
			int result = 0;
			try
			{
				CellParagraph cellParagraph = this.PtsContext.HandleToObject(nmCell) as CellParagraph;
				PTS.ValidateHandle(cellParagraph);
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				cellParagraph.FormatCellFinite(tableParaClient, pfsbrkcell, pfsFtnRejector, fEmptyOK, fswdirTable, dvrAvailable, out pfmtr, out ppfscell, out pfsbrkcellOut, out dvrUsed);
			}
			catch (Exception callbackException)
			{
				pfmtr = default(PTS.FSFMTR);
				ppfscell = IntPtr.Zero;
				pfsbrkcellOut = IntPtr.Zero;
				dvrUsed = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				pfmtr = default(PTS.FSFMTR);
				ppfscell = IntPtr.Zero;
				pfsbrkcellOut = IntPtr.Zero;
				dvrUsed = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069E1 RID: 27105 RVA: 0x001E1C60 File Offset: 0x001DFE60
		internal int FormatCellBottomless(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmCell, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out IntPtr ppfscell, out int dvrUsed)
		{
			int result = 0;
			try
			{
				CellParagraph cellParagraph = this.PtsContext.HandleToObject(nmCell) as CellParagraph;
				PTS.ValidateHandle(cellParagraph);
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				cellParagraph.FormatCellBottomless(tableParaClient, fswdirTable, out fmtrbl, out ppfscell, out dvrUsed);
			}
			catch (Exception callbackException)
			{
				fmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				ppfscell = IntPtr.Zero;
				dvrUsed = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				ppfscell = IntPtr.Zero;
				dvrUsed = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069E2 RID: 27106 RVA: 0x001E1D20 File Offset: 0x001DFF20
		internal int UpdateBottomlessCell(IntPtr pfscell, IntPtr pfsparaclientTable, IntPtr nmCell, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out int dvrUsed)
		{
			int result = 0;
			try
			{
				CellParagraph cellParagraph = this.PtsContext.HandleToObject(nmCell) as CellParagraph;
				PTS.ValidateHandle(cellParagraph);
				CellParaClient cellParaClient = this.PtsContext.HandleToObject(pfscell) as CellParaClient;
				PTS.ValidateHandle(cellParaClient);
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				cellParagraph.UpdateBottomlessCell(cellParaClient, tableParaClient, fswdirTable, out fmtrbl, out dvrUsed);
			}
			catch (Exception callbackException)
			{
				fmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				dvrUsed = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				fmtrbl = PTS.FSFMTRBL.fmtrblGoalReached;
				dvrUsed = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069E3 RID: 27107 RVA: 0x001E1DEC File Offset: 0x001DFFEC
		internal int CompareCells(IntPtr pfscellOld, IntPtr pfscellNew, out PTS.FSCOMPRESULT fscmpr)
		{
			fscmpr = PTS.FSCOMPRESULT.fscmprChangeInside;
			return 0;
		}

		// Token: 0x060069E4 RID: 27108 RVA: 0x0000B02A File Offset: 0x0000922A
		internal int ClearUpdateInfoInCell(IntPtr pfscell)
		{
			return 0;
		}

		// Token: 0x060069E5 RID: 27109 RVA: 0x001E1DF4 File Offset: 0x001DFFF4
		internal int SetCellHeight(IntPtr pfscell, IntPtr pfsparaclientTable, IntPtr pfsbrkcell, IntPtr nmCell, int fBrokenHere, uint fswdirTable, int dvrActual)
		{
			int result = 0;
			try
			{
				CellParagraph cellParagraph = this.PtsContext.HandleToObject(nmCell) as CellParagraph;
				PTS.ValidateHandle(cellParagraph);
				CellParaClient cellParaClient = this.PtsContext.HandleToObject(pfscell) as CellParaClient;
				PTS.ValidateHandle(cellParaClient);
				TableParaClient tableParaClient = this.PtsContext.HandleToObject(pfsparaclientTable) as TableParaClient;
				PTS.ValidateHandle(tableParaClient);
				cellParagraph.SetCellHeight(cellParaClient, tableParaClient, pfsbrkcell, fBrokenHere, fswdirTable, dvrActual);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069E6 RID: 27110 RVA: 0x001E0FC2 File Offset: 0x001DF1C2
		[SecurityCritical]
		internal int DuplicateCellBreakRecord(IntPtr pfsclient, IntPtr pfsbrkcell, out IntPtr ppfsbrkcellDup)
		{
			return PTS.FsDuplicateSubpageBreakRecord(this.Context, pfsbrkcell, out ppfsbrkcellDup);
		}

		// Token: 0x060069E7 RID: 27111 RVA: 0x001E0FD1 File Offset: 0x001DF1D1
		[SecurityCritical]
		internal int DestroyCellBreakRecord(IntPtr pfsclient, IntPtr pfsbrkcell)
		{
			return PTS.FsDestroySubpageBreakRecord(this.Context, pfsbrkcell);
		}

		// Token: 0x060069E8 RID: 27112 RVA: 0x001E1EB0 File Offset: 0x001E00B0
		internal int DestroyCell(IntPtr pfsCell)
		{
			int result = 0;
			try
			{
				CellParaClient cellParaClient = this.PtsContext.HandleToObject(pfsCell) as CellParaClient;
				if (cellParaClient != null)
				{
					cellParaClient.Dispose();
				}
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069E9 RID: 27113 RVA: 0x001E1F2C File Offset: 0x001E012C
		internal int GetCellNumberFootnotes(IntPtr pfsCell, out int cFtn)
		{
			int result = 0;
			try
			{
				CellParaClient handle = this.PtsContext.HandleToObject(pfsCell) as CellParaClient;
				PTS.ValidateHandle(handle);
				cFtn = 0;
			}
			catch (Exception callbackException)
			{
				cFtn = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				cFtn = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069EA RID: 27114 RVA: 0x001E1FB0 File Offset: 0x001E01B0
		internal int GetCellMinColumnBalancingStep(IntPtr pfscell, uint fswdir, out int dvrMinStep)
		{
			int result = 0;
			try
			{
				CellParaClient handle = this.PtsContext.HandleToObject(pfscell) as CellParaClient;
				PTS.ValidateHandle(handle);
				dvrMinStep = TextDpi.ToTextDpi(1.0);
			}
			catch (Exception callbackException)
			{
				dvrMinStep = 0;
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				dvrMinStep = 0;
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x060069EB RID: 27115 RVA: 0x001E2040 File Offset: 0x001E0240
		internal int TransferDisplayInfoCell(IntPtr pfscellOld, IntPtr pfscellNew)
		{
			int result = 0;
			try
			{
				CellParaClient cellParaClient = this.PtsContext.HandleToObject(pfscellOld) as CellParaClient;
				PTS.ValidateHandle(cellParaClient);
				CellParaClient cellParaClient2 = this.PtsContext.HandleToObject(pfscellNew) as CellParaClient;
				PTS.ValidateHandle(cellParaClient2);
				cellParaClient2.TransferDisplayInfo(cellParaClient);
			}
			catch (Exception callbackException)
			{
				this.PtsContext.CallbackException = callbackException;
				result = -100002;
			}
			catch
			{
				this.PtsContext.CallbackException = new Exception("Caught a non CLS Exception");
				result = -100002;
			}
			return result;
		}

		// Token: 0x04003417 RID: 13335
		private PtsContext _ptsContext;

		// Token: 0x04003418 RID: 13336
		private SecurityCriticalDataForSet<IntPtr> _context;

		// Token: 0x04003419 RID: 13337
		private static int _objectContextOffset = 10;

		// Token: 0x0400341A RID: 13338
		private static int _customParaId = 0;
	}
}
