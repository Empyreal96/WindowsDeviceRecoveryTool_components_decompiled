using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;
using MS.Internal.PtsHost.UnsafeNativeMethods;
using MS.Internal.Text;
using MS.Internal.TextFormatting;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000638 RID: 1592
	internal sealed class PtsCache
	{
		// Token: 0x06006903 RID: 26883 RVA: 0x001DA378 File Offset: 0x001D8578
		internal static PtsHost AcquireContext(PtsContext ptsContext, TextFormattingMode textFormattingMode)
		{
			PtsCache ptsCache = ptsContext.Dispatcher.PtsCache as PtsCache;
			if (ptsCache == null)
			{
				ptsCache = new PtsCache(ptsContext.Dispatcher);
				ptsContext.Dispatcher.PtsCache = ptsCache;
			}
			return ptsCache.AcquireContextCore(ptsContext, textFormattingMode);
		}

		// Token: 0x06006904 RID: 26884 RVA: 0x001DA3BC File Offset: 0x001D85BC
		internal static void ReleaseContext(PtsContext ptsContext)
		{
			PtsCache ptsCache = ptsContext.Dispatcher.PtsCache as PtsCache;
			Invariant.Assert(ptsCache != null, "Cannot retrieve PtsCache from PtsContext object.");
			ptsCache.ReleaseContextCore(ptsContext);
		}

		// Token: 0x06006905 RID: 26885 RVA: 0x001DA3F0 File Offset: 0x001D85F0
		[SecurityCritical]
		internal static void GetFloaterHandlerInfo(PtsHost ptsHost, IntPtr pobjectinfo)
		{
			PtsCache ptsCache = Dispatcher.CurrentDispatcher.PtsCache as PtsCache;
			Invariant.Assert(ptsCache != null, "Cannot retrieve PtsCache from the current Dispatcher.");
			ptsCache.GetFloaterHandlerInfoCore(ptsHost, pobjectinfo);
		}

		// Token: 0x06006906 RID: 26886 RVA: 0x001DA424 File Offset: 0x001D8624
		[SecurityCritical]
		internal static void GetTableObjHandlerInfo(PtsHost ptsHost, IntPtr pobjectinfo)
		{
			PtsCache ptsCache = Dispatcher.CurrentDispatcher.PtsCache as PtsCache;
			Invariant.Assert(ptsCache != null, "Cannot retrieve PtsCache from the current Dispatcher.");
			ptsCache.GetTableObjHandlerInfoCore(ptsHost, pobjectinfo);
		}

		// Token: 0x06006907 RID: 26887 RVA: 0x001DA458 File Offset: 0x001D8658
		internal static bool IsDisposed()
		{
			bool result = true;
			Dispatcher currentDispatcher = Dispatcher.CurrentDispatcher;
			if (currentDispatcher != null)
			{
				PtsCache ptsCache = Dispatcher.CurrentDispatcher.PtsCache as PtsCache;
				if (ptsCache != null)
				{
					result = (ptsCache._disposed == 1);
				}
			}
			return result;
		}

		// Token: 0x06006908 RID: 26888 RVA: 0x001DA490 File Offset: 0x001D8690
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private PtsCache(Dispatcher dispatcher)
		{
			this._contextPool = new List<PtsCache.ContextDesc>(1);
			PtsCache.PtsCacheShutDownListener ptsCacheShutDownListener = new PtsCache.PtsCacheShutDownListener(this);
		}

		// Token: 0x06006909 RID: 26889 RVA: 0x001DA4C4 File Offset: 0x001D86C4
		~PtsCache()
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				this.DestroyPTSContexts();
			}
		}

		// Token: 0x0600690A RID: 26890 RVA: 0x001DA500 File Offset: 0x001D8700
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private PtsHost AcquireContextCore(PtsContext ptsContext, TextFormattingMode textFormattingMode)
		{
			int num = 0;
			while (num < this._contextPool.Count && (this._contextPool[num].InUse || this._contextPool[num].IsOptimalParagraphEnabled != ptsContext.IsOptimalParagraphEnabled))
			{
				num++;
			}
			if (num == this._contextPool.Count)
			{
				this._contextPool.Add(new PtsCache.ContextDesc());
				this._contextPool[num].IsOptimalParagraphEnabled = ptsContext.IsOptimalParagraphEnabled;
				this._contextPool[num].PtsHost = new PtsHost();
				this._contextPool[num].PtsHost.Context = this.CreatePTSContext(num, textFormattingMode);
			}
			if (this._contextPool[num].IsOptimalParagraphEnabled)
			{
				ptsContext.TextFormatter = this._contextPool[num].TextFormatter;
			}
			this._contextPool[num].InUse = true;
			this._contextPool[num].Owner = new WeakReference(ptsContext);
			return this._contextPool[num].PtsHost;
		}

		// Token: 0x0600690B RID: 26891 RVA: 0x001DA61C File Offset: 0x001D881C
		private void ReleaseContextCore(PtsContext ptsContext)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._disposed == 0)
				{
					if (this._releaseQueue == null)
					{
						this._releaseQueue = new List<PtsContext>();
						ptsContext.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(this.OnPtsContextReleased), null);
					}
					this._releaseQueue.Add(ptsContext);
				}
			}
		}

		// Token: 0x0600690C RID: 26892 RVA: 0x001DA698 File Offset: 0x001D8898
		[SecurityCritical]
		private void GetFloaterHandlerInfoCore(PtsHost ptsHost, IntPtr pobjectinfo)
		{
			int num = 0;
			while (num < this._contextPool.Count && this._contextPool[num].PtsHost != ptsHost)
			{
				num++;
			}
			Invariant.Assert(num < this._contextPool.Count, "Cannot find matching PtsHost in the Context pool.");
			PTS.Validate(PTS.GetFloaterHandlerInfo(ref this._contextPool[num].FloaterInit, pobjectinfo));
		}

		// Token: 0x0600690D RID: 26893 RVA: 0x001DA704 File Offset: 0x001D8904
		[SecurityCritical]
		private void GetTableObjHandlerInfoCore(PtsHost ptsHost, IntPtr pobjectinfo)
		{
			int num = 0;
			while (num < this._contextPool.Count && this._contextPool[num].PtsHost != ptsHost)
			{
				num++;
			}
			Invariant.Assert(num < this._contextPool.Count, "Cannot find matching PtsHost in the context pool.");
			PTS.Validate(PTS.GetTableObjHandlerInfo(ref this._contextPool[num].TableobjInit, pobjectinfo));
		}

		// Token: 0x0600690E RID: 26894 RVA: 0x001DA76F File Offset: 0x001D896F
		private void Shutdown()
		{
			GC.WaitForPendingFinalizers();
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) == 0)
			{
				this.OnPtsContextReleased(false);
				this.DestroyPTSContexts();
			}
		}

		// Token: 0x0600690F RID: 26895 RVA: 0x001DA794 File Offset: 0x001D8994
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void DestroyPTSContexts()
		{
			int i = 0;
			while (i < this._contextPool.Count)
			{
				PtsContext ptsContext = this._contextPool[i].Owner.Target as PtsContext;
				if (ptsContext != null)
				{
					Invariant.Assert(this._contextPool[i].PtsHost.Context == ptsContext.Context, "PTS Context mismatch.");
					this._contextPool[i].Owner = new WeakReference(null);
					this._contextPool[i].InUse = false;
					Invariant.Assert(!ptsContext.Disposed, "PtsContext has been already disposed.");
					ptsContext.Dispose();
				}
				if (!this._contextPool[i].InUse)
				{
					Invariant.Assert(this._contextPool[i].PtsHost.Context != IntPtr.Zero, "PTS Context handle is not valid.");
					PTS.IgnoreError(PTS.DestroyDocContext(this._contextPool[i].PtsHost.Context));
					Invariant.Assert(this._contextPool[i].InstalledObjects != IntPtr.Zero, "Installed Objects handle is not valid.");
					PTS.IgnoreError(PTS.DestroyInstalledObjectsInfo(this._contextPool[i].InstalledObjects));
					if (this._contextPool[i].TextPenaltyModule != null)
					{
						this._contextPool[i].TextPenaltyModule.Dispose();
					}
					this._contextPool.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06006910 RID: 26896 RVA: 0x001DA920 File Offset: 0x001D8B20
		private object OnPtsContextReleased(object args)
		{
			this.OnPtsContextReleased(true);
			return null;
		}

		// Token: 0x06006911 RID: 26897 RVA: 0x001DA92C File Offset: 0x001D8B2C
		[SecurityCritical]
		[SecurityTreatAsSafe]
		private void OnPtsContextReleased(bool cleanContextPool)
		{
			object @lock = this._lock;
			lock (@lock)
			{
				if (this._releaseQueue != null)
				{
					foreach (PtsContext ptsContext in this._releaseQueue)
					{
						int i;
						for (i = 0; i < this._contextPool.Count; i++)
						{
							if (this._contextPool[i].PtsHost.Context == ptsContext.Context)
							{
								this._contextPool[i].Owner = new WeakReference(null);
								this._contextPool[i].InUse = false;
								break;
							}
						}
						Invariant.Assert(i < this._contextPool.Count, "PtsContext not found in the context pool.");
						Invariant.Assert(!ptsContext.Disposed, "PtsContext has been already disposed.");
						ptsContext.Dispose();
					}
					this._releaseQueue = null;
				}
			}
			if (cleanContextPool && this._contextPool.Count > 4)
			{
				int i = 4;
				while (i < this._contextPool.Count)
				{
					if (!this._contextPool[i].InUse)
					{
						Invariant.Assert(this._contextPool[i].PtsHost.Context != IntPtr.Zero, "PTS Context handle is not valid.");
						PTS.Validate(PTS.DestroyDocContext(this._contextPool[i].PtsHost.Context));
						Invariant.Assert(this._contextPool[i].InstalledObjects != IntPtr.Zero, "Installed Objects handle is not valid.");
						PTS.Validate(PTS.DestroyInstalledObjectsInfo(this._contextPool[i].InstalledObjects));
						if (this._contextPool[i].TextPenaltyModule != null)
						{
							this._contextPool[i].TextPenaltyModule.Dispose();
						}
						this._contextPool.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06006912 RID: 26898 RVA: 0x001DAB5C File Offset: 0x001D8D5C
		[SecurityCritical]
		private IntPtr CreatePTSContext(int index, TextFormattingMode textFormattingMode)
		{
			PtsHost ptsHost = this._contextPool[index].PtsHost;
			Invariant.Assert(ptsHost != null);
			IntPtr installedObjects;
			int installedObjectsCount;
			this.InitInstalledObjectsInfo(ptsHost, ref this._contextPool[index].SubtrackParaInfo, ref this._contextPool[index].SubpageParaInfo, out installedObjects, out installedObjectsCount);
			this._contextPool[index].InstalledObjects = installedObjects;
			this.InitGenericInfo(ptsHost, (IntPtr)(index + 1), installedObjects, installedObjectsCount, ref this._contextPool[index].ContextInfo);
			this.InitFloaterObjInfo(ptsHost, ref this._contextPool[index].FloaterInit);
			this.InitTableObjInfo(ptsHost, ref this._contextPool[index].TableobjInit);
			if (this._contextPool[index].IsOptimalParagraphEnabled)
			{
				TextFormatterContext textFormatterContext = new TextFormatterContext();
				TextPenaltyModule textPenaltyModule = textFormatterContext.GetTextPenaltyModule();
				IntPtr ptsPenaltyModule = textPenaltyModule.DangerousGetHandle();
				this._contextPool[index].TextPenaltyModule = textPenaltyModule;
				this._contextPool[index].ContextInfo.ptsPenaltyModule = ptsPenaltyModule;
				this._contextPool[index].TextFormatter = TextFormatter.CreateFromContext(textFormatterContext, textFormattingMode);
				GC.SuppressFinalize(this._contextPool[index].TextPenaltyModule);
			}
			IntPtr result;
			PTS.Validate(PTS.CreateDocContext(ref this._contextPool[index].ContextInfo, out result));
			return result;
		}

		// Token: 0x06006913 RID: 26899 RVA: 0x001DACB8 File Offset: 0x001D8EB8
		[SecurityCritical]
		private void InitGenericInfo(PtsHost ptsHost, IntPtr clientData, IntPtr installedObjects, int installedObjectsCount, ref PTS.FSCONTEXTINFO contextInfo)
		{
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			Invariant.Assert(true);
			contextInfo.version = 0U;
			contextInfo.fsffi = 320U;
			contextInfo.drMinColumnBalancingStep = TextDpi.ToTextDpi(10.0);
			contextInfo.cInstalledObjects = installedObjectsCount;
			contextInfo.pInstalledObjects = installedObjects;
			contextInfo.pfsclient = clientData;
			contextInfo.pfnAssertFailed = new PTS.AssertFailed(ptsHost.AssertFailed);
			contextInfo.fscbk.cbkfig.pfnGetFigureProperties = new PTS.GetFigureProperties(ptsHost.GetFigureProperties);
			contextInfo.fscbk.cbkfig.pfnGetFigurePolygons = new PTS.GetFigurePolygons(ptsHost.GetFigurePolygons);
			contextInfo.fscbk.cbkfig.pfnCalcFigurePosition = new PTS.CalcFigurePosition(ptsHost.CalcFigurePosition);
			contextInfo.fscbk.cbkgen.pfnFSkipPage = new PTS.FSkipPage(ptsHost.FSkipPage);
			contextInfo.fscbk.cbkgen.pfnGetPageDimensions = new PTS.GetPageDimensions(ptsHost.GetPageDimensions);
			contextInfo.fscbk.cbkgen.pfnGetNextSection = new PTS.GetNextSection(ptsHost.GetNextSection);
			contextInfo.fscbk.cbkgen.pfnGetSectionProperties = new PTS.GetSectionProperties(ptsHost.GetSectionProperties);
			contextInfo.fscbk.cbkgen.pfnGetJustificationProperties = new PTS.GetJustificationProperties(ptsHost.GetJustificationProperties);
			contextInfo.fscbk.cbkgen.pfnGetMainTextSegment = new PTS.GetMainTextSegment(ptsHost.GetMainTextSegment);
			contextInfo.fscbk.cbkgen.pfnGetHeaderSegment = new PTS.GetHeaderSegment(ptsHost.GetHeaderSegment);
			contextInfo.fscbk.cbkgen.pfnGetFooterSegment = new PTS.GetFooterSegment(ptsHost.GetFooterSegment);
			contextInfo.fscbk.cbkgen.pfnUpdGetSegmentChange = new PTS.UpdGetSegmentChange(ptsHost.UpdGetSegmentChange);
			contextInfo.fscbk.cbkgen.pfnGetSectionColumnInfo = new PTS.GetSectionColumnInfo(ptsHost.GetSectionColumnInfo);
			contextInfo.fscbk.cbkgen.pfnGetSegmentDefinedColumnSpanAreaInfo = new PTS.GetSegmentDefinedColumnSpanAreaInfo(ptsHost.GetSegmentDefinedColumnSpanAreaInfo);
			contextInfo.fscbk.cbkgen.pfnGetHeightDefinedColumnSpanAreaInfo = new PTS.GetHeightDefinedColumnSpanAreaInfo(ptsHost.GetHeightDefinedColumnSpanAreaInfo);
			contextInfo.fscbk.cbkgen.pfnGetFirstPara = new PTS.GetFirstPara(ptsHost.GetFirstPara);
			contextInfo.fscbk.cbkgen.pfnGetNextPara = new PTS.GetNextPara(ptsHost.GetNextPara);
			contextInfo.fscbk.cbkgen.pfnUpdGetFirstChangeInSegment = new PTS.UpdGetFirstChangeInSegment(ptsHost.UpdGetFirstChangeInSegment);
			contextInfo.fscbk.cbkgen.pfnUpdGetParaChange = new PTS.UpdGetParaChange(ptsHost.UpdGetParaChange);
			contextInfo.fscbk.cbkgen.pfnGetParaProperties = new PTS.GetParaProperties(ptsHost.GetParaProperties);
			contextInfo.fscbk.cbkgen.pfnCreateParaclient = new PTS.CreateParaclient(ptsHost.CreateParaclient);
			contextInfo.fscbk.cbkgen.pfnTransferDisplayInfo = new PTS.TransferDisplayInfo(ptsHost.TransferDisplayInfo);
			contextInfo.fscbk.cbkgen.pfnDestroyParaclient = new PTS.DestroyParaclient(ptsHost.DestroyParaclient);
			contextInfo.fscbk.cbkgen.pfnFInterruptFormattingAfterPara = new PTS.FInterruptFormattingAfterPara(ptsHost.FInterruptFormattingAfterPara);
			contextInfo.fscbk.cbkgen.pfnGetEndnoteSeparators = new PTS.GetEndnoteSeparators(ptsHost.GetEndnoteSeparators);
			contextInfo.fscbk.cbkgen.pfnGetEndnoteSegment = new PTS.GetEndnoteSegment(ptsHost.GetEndnoteSegment);
			contextInfo.fscbk.cbkgen.pfnGetNumberEndnoteColumns = new PTS.GetNumberEndnoteColumns(ptsHost.GetNumberEndnoteColumns);
			contextInfo.fscbk.cbkgen.pfnGetEndnoteColumnInfo = new PTS.GetEndnoteColumnInfo(ptsHost.GetEndnoteColumnInfo);
			contextInfo.fscbk.cbkgen.pfnGetFootnoteSeparators = new PTS.GetFootnoteSeparators(ptsHost.GetFootnoteSeparators);
			contextInfo.fscbk.cbkgen.pfnFFootnoteBeneathText = new PTS.FFootnoteBeneathText(ptsHost.FFootnoteBeneathText);
			contextInfo.fscbk.cbkgen.pfnGetNumberFootnoteColumns = new PTS.GetNumberFootnoteColumns(ptsHost.GetNumberFootnoteColumns);
			contextInfo.fscbk.cbkgen.pfnGetFootnoteColumnInfo = new PTS.GetFootnoteColumnInfo(ptsHost.GetFootnoteColumnInfo);
			contextInfo.fscbk.cbkgen.pfnGetFootnoteSegment = new PTS.GetFootnoteSegment(ptsHost.GetFootnoteSegment);
			contextInfo.fscbk.cbkgen.pfnGetFootnotePresentationAndRejectionOrder = new PTS.GetFootnotePresentationAndRejectionOrder(ptsHost.GetFootnotePresentationAndRejectionOrder);
			contextInfo.fscbk.cbkgen.pfnFAllowFootnoteSeparation = new PTS.FAllowFootnoteSeparation(ptsHost.FAllowFootnoteSeparation);
			contextInfo.fscbk.cbkobj.pfnDuplicateMcsclient = new PTS.DuplicateMcsclient(ptsHost.DuplicateMcsclient);
			contextInfo.fscbk.cbkobj.pfnDestroyMcsclient = new PTS.DestroyMcsclient(ptsHost.DestroyMcsclient);
			contextInfo.fscbk.cbkobj.pfnFEqualMcsclient = new PTS.FEqualMcsclient(ptsHost.FEqualMcsclient);
			contextInfo.fscbk.cbkobj.pfnConvertMcsclient = new PTS.ConvertMcsclient(ptsHost.ConvertMcsclient);
			contextInfo.fscbk.cbkobj.pfnGetObjectHandlerInfo = new PTS.GetObjectHandlerInfo(ptsHost.GetObjectHandlerInfo);
			contextInfo.fscbk.cbktxt.pfnCreateParaBreakingSession = new PTS.CreateParaBreakingSession(ptsHost.CreateParaBreakingSession);
			contextInfo.fscbk.cbktxt.pfnDestroyParaBreakingSession = new PTS.DestroyParaBreakingSession(ptsHost.DestroyParaBreakingSession);
			contextInfo.fscbk.cbktxt.pfnGetTextProperties = new PTS.GetTextProperties(ptsHost.GetTextProperties);
			contextInfo.fscbk.cbktxt.pfnGetNumberFootnotes = new PTS.GetNumberFootnotes(ptsHost.GetNumberFootnotes);
			contextInfo.fscbk.cbktxt.pfnGetFootnotes = new PTS.GetFootnotes(ptsHost.GetFootnotes);
			contextInfo.fscbk.cbktxt.pfnFormatDropCap = new PTS.FormatDropCap(ptsHost.FormatDropCap);
			contextInfo.fscbk.cbktxt.pfnGetDropCapPolygons = new PTS.GetDropCapPolygons(ptsHost.GetDropCapPolygons);
			contextInfo.fscbk.cbktxt.pfnDestroyDropCap = new PTS.DestroyDropCap(ptsHost.DestroyDropCap);
			contextInfo.fscbk.cbktxt.pfnFormatBottomText = new PTS.FormatBottomText(ptsHost.FormatBottomText);
			contextInfo.fscbk.cbktxt.pfnFormatLine = new PTS.FormatLine(ptsHost.FormatLine);
			contextInfo.fscbk.cbktxt.pfnFormatLineForced = new PTS.FormatLineForced(ptsHost.FormatLineForced);
			contextInfo.fscbk.cbktxt.pfnFormatLineVariants = new PTS.FormatLineVariants(ptsHost.FormatLineVariants);
			contextInfo.fscbk.cbktxt.pfnReconstructLineVariant = new PTS.ReconstructLineVariant(ptsHost.ReconstructLineVariant);
			contextInfo.fscbk.cbktxt.pfnDestroyLine = new PTS.DestroyLine(ptsHost.DestroyLine);
			contextInfo.fscbk.cbktxt.pfnDuplicateLineBreakRecord = new PTS.DuplicateLineBreakRecord(ptsHost.DuplicateLineBreakRecord);
			contextInfo.fscbk.cbktxt.pfnDestroyLineBreakRecord = new PTS.DestroyLineBreakRecord(ptsHost.DestroyLineBreakRecord);
			contextInfo.fscbk.cbktxt.pfnSnapGridVertical = new PTS.SnapGridVertical(ptsHost.SnapGridVertical);
			contextInfo.fscbk.cbktxt.pfnGetDvrSuppressibleBottomSpace = new PTS.GetDvrSuppressibleBottomSpace(ptsHost.GetDvrSuppressibleBottomSpace);
			contextInfo.fscbk.cbktxt.pfnGetDvrAdvance = new PTS.GetDvrAdvance(ptsHost.GetDvrAdvance);
			contextInfo.fscbk.cbktxt.pfnUpdGetChangeInText = new PTS.UpdGetChangeInText(ptsHost.UpdGetChangeInText);
			contextInfo.fscbk.cbktxt.pfnUpdGetDropCapChange = new PTS.UpdGetDropCapChange(ptsHost.UpdGetDropCapChange);
			contextInfo.fscbk.cbktxt.pfnFInterruptFormattingText = new PTS.FInterruptFormattingText(ptsHost.FInterruptFormattingText);
			contextInfo.fscbk.cbktxt.pfnGetTextParaCache = new PTS.GetTextParaCache(ptsHost.GetTextParaCache);
			contextInfo.fscbk.cbktxt.pfnSetTextParaCache = new PTS.SetTextParaCache(ptsHost.SetTextParaCache);
			contextInfo.fscbk.cbktxt.pfnGetOptimalLineDcpCache = new PTS.GetOptimalLineDcpCache(ptsHost.GetOptimalLineDcpCache);
			contextInfo.fscbk.cbktxt.pfnGetNumberAttachedObjectsBeforeTextLine = new PTS.GetNumberAttachedObjectsBeforeTextLine(ptsHost.GetNumberAttachedObjectsBeforeTextLine);
			contextInfo.fscbk.cbktxt.pfnGetAttachedObjectsBeforeTextLine = new PTS.GetAttachedObjectsBeforeTextLine(ptsHost.GetAttachedObjectsBeforeTextLine);
			contextInfo.fscbk.cbktxt.pfnGetNumberAttachedObjectsInTextLine = new PTS.GetNumberAttachedObjectsInTextLine(ptsHost.GetNumberAttachedObjectsInTextLine);
			contextInfo.fscbk.cbktxt.pfnGetAttachedObjectsInTextLine = new PTS.GetAttachedObjectsInTextLine(ptsHost.GetAttachedObjectsInTextLine);
			contextInfo.fscbk.cbktxt.pfnUpdGetAttachedObjectChange = new PTS.UpdGetAttachedObjectChange(ptsHost.UpdGetAttachedObjectChange);
			contextInfo.fscbk.cbktxt.pfnGetDurFigureAnchor = new PTS.GetDurFigureAnchor(ptsHost.GetDurFigureAnchor);
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x001DB588 File Offset: 0x001D9788
		[SecurityCritical]
		private void InitInstalledObjectsInfo(PtsHost ptsHost, ref PTS.FSIMETHODS subtrackParaInfo, ref PTS.FSIMETHODS subpageParaInfo, out IntPtr installedObjects, out int installedObjectsCount)
		{
			subtrackParaInfo.pfnCreateContext = new PTS.ObjCreateContext(ptsHost.SubtrackCreateContext);
			subtrackParaInfo.pfnDestroyContext = new PTS.ObjDestroyContext(ptsHost.SubtrackDestroyContext);
			subtrackParaInfo.pfnFormatParaFinite = new PTS.ObjFormatParaFinite(ptsHost.SubtrackFormatParaFinite);
			subtrackParaInfo.pfnFormatParaBottomless = new PTS.ObjFormatParaBottomless(ptsHost.SubtrackFormatParaBottomless);
			subtrackParaInfo.pfnUpdateBottomlessPara = new PTS.ObjUpdateBottomlessPara(ptsHost.SubtrackUpdateBottomlessPara);
			subtrackParaInfo.pfnSynchronizeBottomlessPara = new PTS.ObjSynchronizeBottomlessPara(ptsHost.SubtrackSynchronizeBottomlessPara);
			subtrackParaInfo.pfnComparePara = new PTS.ObjComparePara(ptsHost.SubtrackComparePara);
			subtrackParaInfo.pfnClearUpdateInfoInPara = new PTS.ObjClearUpdateInfoInPara(ptsHost.SubtrackClearUpdateInfoInPara);
			subtrackParaInfo.pfnDestroyPara = new PTS.ObjDestroyPara(ptsHost.SubtrackDestroyPara);
			subtrackParaInfo.pfnDuplicateBreakRecord = new PTS.ObjDuplicateBreakRecord(ptsHost.SubtrackDuplicateBreakRecord);
			subtrackParaInfo.pfnDestroyBreakRecord = new PTS.ObjDestroyBreakRecord(ptsHost.SubtrackDestroyBreakRecord);
			subtrackParaInfo.pfnGetColumnBalancingInfo = new PTS.ObjGetColumnBalancingInfo(ptsHost.SubtrackGetColumnBalancingInfo);
			subtrackParaInfo.pfnGetNumberFootnotes = new PTS.ObjGetNumberFootnotes(ptsHost.SubtrackGetNumberFootnotes);
			subtrackParaInfo.pfnGetFootnoteInfo = new PTS.ObjGetFootnoteInfo(ptsHost.SubtrackGetFootnoteInfo);
			subtrackParaInfo.pfnGetFootnoteInfoWord = IntPtr.Zero;
			subtrackParaInfo.pfnShiftVertical = new PTS.ObjShiftVertical(ptsHost.SubtrackShiftVertical);
			subtrackParaInfo.pfnTransferDisplayInfoPara = new PTS.ObjTransferDisplayInfoPara(ptsHost.SubtrackTransferDisplayInfoPara);
			subpageParaInfo.pfnCreateContext = new PTS.ObjCreateContext(ptsHost.SubpageCreateContext);
			subpageParaInfo.pfnDestroyContext = new PTS.ObjDestroyContext(ptsHost.SubpageDestroyContext);
			subpageParaInfo.pfnFormatParaFinite = new PTS.ObjFormatParaFinite(ptsHost.SubpageFormatParaFinite);
			subpageParaInfo.pfnFormatParaBottomless = new PTS.ObjFormatParaBottomless(ptsHost.SubpageFormatParaBottomless);
			subpageParaInfo.pfnUpdateBottomlessPara = new PTS.ObjUpdateBottomlessPara(ptsHost.SubpageUpdateBottomlessPara);
			subpageParaInfo.pfnSynchronizeBottomlessPara = new PTS.ObjSynchronizeBottomlessPara(ptsHost.SubpageSynchronizeBottomlessPara);
			subpageParaInfo.pfnComparePara = new PTS.ObjComparePara(ptsHost.SubpageComparePara);
			subpageParaInfo.pfnClearUpdateInfoInPara = new PTS.ObjClearUpdateInfoInPara(ptsHost.SubpageClearUpdateInfoInPara);
			subpageParaInfo.pfnDestroyPara = new PTS.ObjDestroyPara(ptsHost.SubpageDestroyPara);
			subpageParaInfo.pfnDuplicateBreakRecord = new PTS.ObjDuplicateBreakRecord(ptsHost.SubpageDuplicateBreakRecord);
			subpageParaInfo.pfnDestroyBreakRecord = new PTS.ObjDestroyBreakRecord(ptsHost.SubpageDestroyBreakRecord);
			subpageParaInfo.pfnGetColumnBalancingInfo = new PTS.ObjGetColumnBalancingInfo(ptsHost.SubpageGetColumnBalancingInfo);
			subpageParaInfo.pfnGetNumberFootnotes = new PTS.ObjGetNumberFootnotes(ptsHost.SubpageGetNumberFootnotes);
			subpageParaInfo.pfnGetFootnoteInfo = new PTS.ObjGetFootnoteInfo(ptsHost.SubpageGetFootnoteInfo);
			subpageParaInfo.pfnShiftVertical = new PTS.ObjShiftVertical(ptsHost.SubpageShiftVertical);
			subpageParaInfo.pfnTransferDisplayInfoPara = new PTS.ObjTransferDisplayInfoPara(ptsHost.SubpageTransferDisplayInfoPara);
			PTS.Validate(PTS.CreateInstalledObjectsInfo(ref subtrackParaInfo, ref subpageParaInfo, out installedObjects, out installedObjectsCount));
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x001DB7F0 File Offset: 0x001D99F0
		[SecurityCritical]
		private void InitFloaterObjInfo(PtsHost ptsHost, ref PTS.FSFLOATERINIT floaterInit)
		{
			floaterInit.fsfloatercbk.pfnGetFloaterProperties = new PTS.GetFloaterProperties(ptsHost.GetFloaterProperties);
			floaterInit.fsfloatercbk.pfnFormatFloaterContentFinite = new PTS.FormatFloaterContentFinite(ptsHost.FormatFloaterContentFinite);
			floaterInit.fsfloatercbk.pfnFormatFloaterContentBottomless = new PTS.FormatFloaterContentBottomless(ptsHost.FormatFloaterContentBottomless);
			floaterInit.fsfloatercbk.pfnUpdateBottomlessFloaterContent = new PTS.UpdateBottomlessFloaterContent(ptsHost.UpdateBottomlessFloaterContent);
			floaterInit.fsfloatercbk.pfnGetFloaterPolygons = new PTS.GetFloaterPolygons(ptsHost.GetFloaterPolygons);
			floaterInit.fsfloatercbk.pfnClearUpdateInfoInFloaterContent = new PTS.ClearUpdateInfoInFloaterContent(ptsHost.ClearUpdateInfoInFloaterContent);
			floaterInit.fsfloatercbk.pfnCompareFloaterContents = new PTS.CompareFloaterContents(ptsHost.CompareFloaterContents);
			floaterInit.fsfloatercbk.pfnDestroyFloaterContent = new PTS.DestroyFloaterContent(ptsHost.DestroyFloaterContent);
			floaterInit.fsfloatercbk.pfnDuplicateFloaterContentBreakRecord = new PTS.DuplicateFloaterContentBreakRecord(ptsHost.DuplicateFloaterContentBreakRecord);
			floaterInit.fsfloatercbk.pfnDestroyFloaterContentBreakRecord = new PTS.DestroyFloaterContentBreakRecord(ptsHost.DestroyFloaterContentBreakRecord);
			floaterInit.fsfloatercbk.pfnGetFloaterContentColumnBalancingInfo = new PTS.GetFloaterContentColumnBalancingInfo(ptsHost.GetFloaterContentColumnBalancingInfo);
			floaterInit.fsfloatercbk.pfnGetFloaterContentNumberFootnotes = new PTS.GetFloaterContentNumberFootnotes(ptsHost.GetFloaterContentNumberFootnotes);
			floaterInit.fsfloatercbk.pfnGetFloaterContentFootnoteInfo = new PTS.GetFloaterContentFootnoteInfo(ptsHost.GetFloaterContentFootnoteInfo);
			floaterInit.fsfloatercbk.pfnTransferDisplayInfoInFloaterContent = new PTS.TransferDisplayInfoInFloaterContent(ptsHost.TransferDisplayInfoInFloaterContent);
			floaterInit.fsfloatercbk.pfnGetMCSClientAfterFloater = new PTS.GetMCSClientAfterFloater(ptsHost.GetMCSClientAfterFloater);
			floaterInit.fsfloatercbk.pfnGetDvrUsedForFloater = new PTS.GetDvrUsedForFloater(ptsHost.GetDvrUsedForFloater);
		}

		// Token: 0x06006916 RID: 26902 RVA: 0x001DB970 File Offset: 0x001D9B70
		[SecurityCritical]
		private void InitTableObjInfo(PtsHost ptsHost, ref PTS.FSTABLEOBJINIT tableobjInit)
		{
			tableobjInit.tableobjcbk.pfnGetTableProperties = new PTS.GetTableProperties(ptsHost.GetTableProperties);
			tableobjInit.tableobjcbk.pfnAutofitTable = new PTS.AutofitTable(ptsHost.AutofitTable);
			tableobjInit.tableobjcbk.pfnUpdAutofitTable = new PTS.UpdAutofitTable(ptsHost.UpdAutofitTable);
			tableobjInit.tableobjcbk.pfnGetMCSClientAfterTable = new PTS.GetMCSClientAfterTable(ptsHost.GetMCSClientAfterTable);
			tableobjInit.tableobjcbk.pfnGetDvrUsedForFloatTable = IntPtr.Zero;
			tableobjInit.tablecbkfetch.pfnGetFirstHeaderRow = new PTS.GetFirstHeaderRow(ptsHost.GetFirstHeaderRow);
			tableobjInit.tablecbkfetch.pfnGetNextHeaderRow = new PTS.GetNextHeaderRow(ptsHost.GetNextHeaderRow);
			tableobjInit.tablecbkfetch.pfnGetFirstFooterRow = new PTS.GetFirstFooterRow(ptsHost.GetFirstFooterRow);
			tableobjInit.tablecbkfetch.pfnGetNextFooterRow = new PTS.GetNextFooterRow(ptsHost.GetNextFooterRow);
			tableobjInit.tablecbkfetch.pfnGetFirstRow = new PTS.GetFirstRow(ptsHost.GetFirstRow);
			tableobjInit.tablecbkfetch.pfnGetNextRow = new PTS.GetNextRow(ptsHost.GetNextRow);
			tableobjInit.tablecbkfetch.pfnUpdFChangeInHeaderFooter = new PTS.UpdFChangeInHeaderFooter(ptsHost.UpdFChangeInHeaderFooter);
			tableobjInit.tablecbkfetch.pfnUpdGetFirstChangeInTable = new PTS.UpdGetFirstChangeInTable(ptsHost.UpdGetFirstChangeInTable);
			tableobjInit.tablecbkfetch.pfnUpdGetRowChange = new PTS.UpdGetRowChange(ptsHost.UpdGetRowChange);
			tableobjInit.tablecbkfetch.pfnUpdGetCellChange = new PTS.UpdGetCellChange(ptsHost.UpdGetCellChange);
			tableobjInit.tablecbkfetch.pfnGetDistributionKind = new PTS.GetDistributionKind(ptsHost.GetDistributionKind);
			tableobjInit.tablecbkfetch.pfnGetRowProperties = new PTS.GetRowProperties(ptsHost.GetRowProperties);
			tableobjInit.tablecbkfetch.pfnGetCells = new PTS.GetCells(ptsHost.GetCells);
			tableobjInit.tablecbkfetch.pfnFInterruptFormattingTable = new PTS.FInterruptFormattingTable(ptsHost.FInterruptFormattingTable);
			tableobjInit.tablecbkfetch.pfnCalcHorizontalBBoxOfRow = new PTS.CalcHorizontalBBoxOfRow(ptsHost.CalcHorizontalBBoxOfRow);
			tableobjInit.tablecbkcell.pfnFormatCellFinite = new PTS.FormatCellFinite(ptsHost.FormatCellFinite);
			tableobjInit.tablecbkcell.pfnFormatCellBottomless = new PTS.FormatCellBottomless(ptsHost.FormatCellBottomless);
			tableobjInit.tablecbkcell.pfnUpdateBottomlessCell = new PTS.UpdateBottomlessCell(ptsHost.UpdateBottomlessCell);
			tableobjInit.tablecbkcell.pfnCompareCells = new PTS.CompareCells(ptsHost.CompareCells);
			tableobjInit.tablecbkcell.pfnClearUpdateInfoInCell = new PTS.ClearUpdateInfoInCell(ptsHost.ClearUpdateInfoInCell);
			tableobjInit.tablecbkcell.pfnSetCellHeight = new PTS.SetCellHeight(ptsHost.SetCellHeight);
			tableobjInit.tablecbkcell.pfnDestroyCell = new PTS.DestroyCell(ptsHost.DestroyCell);
			tableobjInit.tablecbkcell.pfnDuplicateCellBreakRecord = new PTS.DuplicateCellBreakRecord(ptsHost.DuplicateCellBreakRecord);
			tableobjInit.tablecbkcell.pfnDestroyCellBreakRecord = new PTS.DestroyCellBreakRecord(ptsHost.DestroyCellBreakRecord);
			tableobjInit.tablecbkcell.pfnGetCellNumberFootnotes = new PTS.GetCellNumberFootnotes(ptsHost.GetCellNumberFootnotes);
			tableobjInit.tablecbkcell.pfnGetCellFootnoteInfo = IntPtr.Zero;
			tableobjInit.tablecbkcell.pfnGetCellFootnoteInfoWord = IntPtr.Zero;
			tableobjInit.tablecbkcell.pfnGetCellMinColumnBalancingStep = new PTS.GetCellMinColumnBalancingStep(ptsHost.GetCellMinColumnBalancingStep);
			tableobjInit.tablecbkcell.pfnTransferDisplayInfoCell = new PTS.TransferDisplayInfoCell(ptsHost.TransferDisplayInfoCell);
		}

		// Token: 0x04003409 RID: 13321
		[SecurityCritical]
		private List<PtsCache.ContextDesc> _contextPool;

		// Token: 0x0400340A RID: 13322
		private List<PtsContext> _releaseQueue;

		// Token: 0x0400340B RID: 13323
		private object _lock = new object();

		// Token: 0x0400340C RID: 13324
		private int _disposed;

		// Token: 0x02000A20 RID: 2592
		private class ContextDesc
		{
			// Token: 0x04004704 RID: 18180
			internal PtsHost PtsHost;

			// Token: 0x04004705 RID: 18181
			internal PTS.FSCONTEXTINFO ContextInfo;

			// Token: 0x04004706 RID: 18182
			internal PTS.FSIMETHODS SubtrackParaInfo;

			// Token: 0x04004707 RID: 18183
			internal PTS.FSIMETHODS SubpageParaInfo;

			// Token: 0x04004708 RID: 18184
			internal PTS.FSFLOATERINIT FloaterInit;

			// Token: 0x04004709 RID: 18185
			internal PTS.FSTABLEOBJINIT TableobjInit;

			// Token: 0x0400470A RID: 18186
			internal IntPtr InstalledObjects;

			// Token: 0x0400470B RID: 18187
			internal TextFormatter TextFormatter;

			// Token: 0x0400470C RID: 18188
			internal TextPenaltyModule TextPenaltyModule;

			// Token: 0x0400470D RID: 18189
			internal bool IsOptimalParagraphEnabled;

			// Token: 0x0400470E RID: 18190
			internal WeakReference Owner;

			// Token: 0x0400470F RID: 18191
			internal bool InUse;
		}

		// Token: 0x02000A21 RID: 2593
		private sealed class PtsCacheShutDownListener : ShutDownListener
		{
			// Token: 0x06008AB8 RID: 35512 RVA: 0x002579C2 File Offset: 0x00255BC2
			[SecurityCritical]
			[SecurityTreatAsSafe]
			public PtsCacheShutDownListener(PtsCache target) : base(target)
			{
			}

			// Token: 0x06008AB9 RID: 35513 RVA: 0x002579CC File Offset: 0x00255BCC
			internal override void OnShutDown(object target, object sender, EventArgs e)
			{
				PtsCache ptsCache = (PtsCache)target;
				ptsCache.Shutdown();
			}
		}
	}
}
