using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Windows;
using MS.Internal.Text;

namespace MS.Internal.PtsHost.UnsafeNativeMethods
{
	// Token: 0x02000654 RID: 1620
	internal static class PTS
	{
		// Token: 0x06006B7E RID: 27518 RVA: 0x00002137 File Offset: 0x00000337
		internal static void IgnoreError(int fserr)
		{
		}

		// Token: 0x06006B7F RID: 27519 RVA: 0x001F0F5C File Offset: 0x001EF15C
		internal static void Validate(int fserr)
		{
			if (fserr != 0)
			{
				PTS.Error(fserr, null);
			}
		}

		// Token: 0x06006B80 RID: 27520 RVA: 0x001F0F68 File Offset: 0x001EF168
		internal static void Validate(int fserr, PtsContext ptsContext)
		{
			if (fserr != 0)
			{
				PTS.Error(fserr, ptsContext);
			}
		}

		// Token: 0x06006B81 RID: 27521 RVA: 0x001F0F74 File Offset: 0x001EF174
		private static void Error(int fserr, PtsContext ptsContext)
		{
			if (fserr <= -112)
			{
				if (fserr != -100002)
				{
					if (fserr != -112)
					{
						goto IL_7A;
					}
				}
				else
				{
					if (ptsContext != null)
					{
						PTS.SecondaryException ex = new PTS.SecondaryException(ptsContext.CallbackException);
						ptsContext.CallbackException = null;
						throw ex;
					}
					throw new Exception(SR.Get("PTSError", new object[]
					{
						fserr
					}));
				}
			}
			else if (fserr != -100)
			{
				if (fserr == -2)
				{
					throw new OutOfMemoryException();
				}
				goto IL_7A;
			}
			throw new PTS.PtsException(SR.Get("FormatRestrictionsExceeded", new object[]
			{
				fserr
			}));
			IL_7A:
			throw new PTS.PtsException(SR.Get("PTSError", new object[]
			{
				fserr
			}));
		}

		// Token: 0x06006B82 RID: 27522 RVA: 0x001F1019 File Offset: 0x001EF219
		internal static void ValidateAndTrace(int fserr, PtsContext ptsContext)
		{
			if (fserr != 0)
			{
				PTS.ErrorTrace(fserr, ptsContext);
			}
		}

		// Token: 0x06006B83 RID: 27523 RVA: 0x001F1028 File Offset: 0x001EF228
		private static void ErrorTrace(int fserr, PtsContext ptsContext)
		{
			if (fserr == -2)
			{
				throw new OutOfMemoryException();
			}
			if (ptsContext == null)
			{
				throw new Exception(SR.Get("PTSError", new object[]
				{
					fserr
				}));
			}
			Exception innermostException = PTS.GetInnermostException(ptsContext);
			if (innermostException != null && !(innermostException is PTS.SecondaryException) && !(innermostException is PTS.PtsException))
			{
				PTS.SecondaryException ex = new PTS.SecondaryException(innermostException);
				ptsContext.CallbackException = null;
				throw ex;
			}
			string p = (innermostException == null) ? string.Empty : innermostException.Message;
			if (TracePageFormatting.IsEnabled)
			{
				TracePageFormatting.Trace(TraceEventType.Start, TracePageFormatting.PageFormattingError, ptsContext, p);
				TracePageFormatting.Trace(TraceEventType.Stop, TracePageFormatting.PageFormattingError, ptsContext, p);
				return;
			}
		}

		// Token: 0x06006B84 RID: 27524 RVA: 0x001F10C8 File Offset: 0x001EF2C8
		private static Exception GetInnermostException(PtsContext ptsContext)
		{
			Invariant.Assert(ptsContext != null);
			Exception ex = ptsContext.CallbackException;
			Exception ex2 = ex;
			while (ex2 != null)
			{
				ex2 = ex.InnerException;
				if (ex2 != null)
				{
					ex = ex2;
				}
				if (!(ex is PTS.PtsException) && !(ex is PTS.SecondaryException))
				{
					break;
				}
			}
			return ex;
		}

		// Token: 0x06006B85 RID: 27525 RVA: 0x001F1109 File Offset: 0x001EF309
		internal static void ValidateHandle(object handle)
		{
			if (handle == null)
			{
				PTS.InvalidHandle();
			}
		}

		// Token: 0x06006B86 RID: 27526 RVA: 0x001F1113 File Offset: 0x001EF313
		private static void InvalidHandle()
		{
			throw new Exception(SR.Get("PTSInvalidHandle"));
		}

		// Token: 0x06006B87 RID: 27527 RVA: 0x001F1124 File Offset: 0x001EF324
		internal static int FromBoolean(bool condition)
		{
			if (!condition)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06006B88 RID: 27528 RVA: 0x001F112C File Offset: 0x001EF32C
		internal static bool ToBoolean(int flag)
		{
			return flag != 0;
		}

		// Token: 0x06006B89 RID: 27529 RVA: 0x0001B7E3 File Offset: 0x000199E3
		internal static PTS.FSKWRAP WrapDirectionToFskwrap(WrapDirection wrapDirection)
		{
			return (PTS.FSKWRAP)wrapDirection;
		}

		// Token: 0x06006B8A RID: 27530 RVA: 0x001F1134 File Offset: 0x001EF334
		internal static PTS.FSKCLEAR WrapDirectionToFskclear(WrapDirection wrapDirection)
		{
			PTS.FSKCLEAR result;
			switch (wrapDirection)
			{
			case WrapDirection.None:
				result = PTS.FSKCLEAR.fskclearNone;
				break;
			case WrapDirection.Left:
				result = PTS.FSKCLEAR.fskclearLeft;
				break;
			case WrapDirection.Right:
				result = PTS.FSKCLEAR.fskclearRight;
				break;
			case WrapDirection.Both:
				result = PTS.FSKCLEAR.fskclearBoth;
				break;
			default:
				result = PTS.FSKCLEAR.fskclearNone;
				break;
			}
			return result;
		}

		// Token: 0x06006B8B RID: 27531 RVA: 0x001F116C File Offset: 0x001EF36C
		internal static FlowDirection FswdirToFlowDirection(uint fswdir)
		{
			FlowDirection result;
			if (fswdir == 4U)
			{
				result = FlowDirection.RightToLeft;
			}
			else
			{
				result = FlowDirection.LeftToRight;
			}
			return result;
		}

		// Token: 0x06006B8C RID: 27532 RVA: 0x001F1184 File Offset: 0x001EF384
		internal static uint FlowDirectionToFswdir(FlowDirection fd)
		{
			uint result;
			if (fd == FlowDirection.RightToLeft)
			{
				result = 4U;
			}
			else
			{
				result = 0U;
			}
			return result;
		}

		// Token: 0x06006B8D RID: 27533
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int GetFloaterHandlerInfo([In] ref PTS.FSFLOATERINIT pfsfloaterinit, IntPtr pFloaterObjectInfo);

		// Token: 0x06006B8E RID: 27534
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int GetTableObjHandlerInfo([In] ref PTS.FSTABLEOBJINIT pfstableobjinit, IntPtr pTableObjectInfo);

		// Token: 0x06006B8F RID: 27535
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int CreateInstalledObjectsInfo([In] ref PTS.FSIMETHODS fssubtrackparamethods, ref PTS.FSIMETHODS fssubpageparamethods, out IntPtr pInstalledObjects, out int cInstalledObjects);

		// Token: 0x06006B90 RID: 27536
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int DestroyInstalledObjectsInfo(IntPtr pInstalledObjects);

		// Token: 0x06006B91 RID: 27537
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int CreateDocContext([In] ref PTS.FSCONTEXTINFO fscontextinfo, out IntPtr pfscontext);

		// Token: 0x06006B92 RID: 27538
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int DestroyDocContext(IntPtr pfscontext);

		// Token: 0x06006B93 RID: 27539
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsCreatePageFinite(IntPtr pfscontext, IntPtr pfsBRPageStart, IntPtr fsnmSectStart, out PTS.FSFMTR pfsfmtrOut, out IntPtr ppfsPageOut, out IntPtr ppfsBRPageOut);

		// Token: 0x06006B94 RID: 27540
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsUpdateFinitePage(IntPtr pfscontext, IntPtr pfspage, IntPtr pfsBRPageStart, IntPtr fsnmSectStart, out PTS.FSFMTR pfsfmtrOut, out IntPtr ppfsBRPageOut);

		// Token: 0x06006B95 RID: 27541
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsCreatePageBottomless(IntPtr pfscontext, IntPtr fsnmsect, out PTS.FSFMTRBL pfsfmtrbl, out IntPtr ppfspage);

		// Token: 0x06006B96 RID: 27542
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsUpdateBottomlessPage(IntPtr pfscontext, IntPtr pfspage, IntPtr fsnmsect, out PTS.FSFMTRBL pfsfmtrbl);

		// Token: 0x06006B97 RID: 27543
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsClearUpdateInfoInPage(IntPtr pfscontext, IntPtr pfspage);

		// Token: 0x06006B98 RID: 27544
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsDestroyPage(IntPtr pfscontext, IntPtr pfspage);

		// Token: 0x06006B99 RID: 27545
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsDestroyPageBreakRecord(IntPtr pfscontext, IntPtr pfsbreakrec);

		// Token: 0x06006B9A RID: 27546
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsCreateSubpageFinite(IntPtr pfsContext, IntPtr pBRSubPageStart, int fFromPreviousPage, IntPtr nSeg, IntPtr pFtnRej, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int lWidth, int lHeight, ref PTS.FSRECT rcMargin, int cColumns, PTS.FSCOLUMNINFO* rgColumnInfo, int fApplyColumnBalancing, int cSegmentAreas, IntPtr* rgnSegmentForArea, int* rgSpanForSegmentArea, int cHeightAreas, int* rgHeightForArea, int* rgSpanForHeightArea, int fAllowOverhangBottom, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pSubPage, out IntPtr pBRSubPageOut, out int dvrUsed, out PTS.FSBBOX fsBBox, out IntPtr pfsMcsClient, out int topSpace);

		// Token: 0x06006B9B RID: 27547
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsCreateSubpageBottomless(IntPtr pfsContext, IntPtr nSeg, int fSuppressTopSpace, uint fswdir, int lWidth, int urMargin, int durMargin, int vrMargin, int cColumns, PTS.FSCOLUMNINFO* rgColumnInfo, int cSegmentAreas, IntPtr* rgnSegmentForArea, int* rgSpanForSegmentArea, int cHeightAreas, int* rgHeightForArea, int* rgSpanForHeightArea, int fINterrruptible, out PTS.FSFMTRBL pfsfmtr, out IntPtr ppSubPage, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr pfsMcsClient, out int pTopSpace, out int fPageBecomesUninterruptible);

		// Token: 0x06006B9C RID: 27548
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsUpdateBottomlessSubpage(IntPtr pfsContext, IntPtr pfsSubpage, IntPtr nmSeg, int fSuppressTopSpace, uint fswdir, int lWidth, int urMargin, int durMargin, int vrMargin, int cColumns, PTS.FSCOLUMNINFO* rgColumnInfo, int cSegmentAreas, IntPtr* rgnSegmentForArea, int* rgSpanForSegmentArea, int cHeightAreas, int* rgHeightForArea, int* rgSpanForHeightArea, int fINterrruptible, out PTS.FSFMTRBL pfsfmtr, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr pfsMcsClient, out int pTopSpace, out int fPageBecomesUninterruptible);

		// Token: 0x06006B9D RID: 27549
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsCompareSubpages(IntPtr pfsContext, IntPtr pfsSubpageOld, IntPtr pfsSubpageNew, out PTS.FSCOMPRESULT fsCompResult);

		// Token: 0x06006B9E RID: 27550
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsClearUpdateInfoInSubpage(IntPtr pfscontext, IntPtr pSubpage);

		// Token: 0x06006B9F RID: 27551
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsDestroySubpage(IntPtr pfsContext, IntPtr pSubpage);

		// Token: 0x06006BA0 RID: 27552
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsDuplicateSubpageBreakRecord(IntPtr pfsContext, IntPtr pBreakRecSubPageIn, out IntPtr ppBreakRecSubPageOut);

		// Token: 0x06006BA1 RID: 27553
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsDestroySubpageBreakRecord(IntPtr pfscontext, IntPtr pfsbreakrec);

		// Token: 0x06006BA2 RID: 27554
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsGetSubpageColumnBalancingInfo(IntPtr pfsContext, IntPtr pSubpage, out uint fswdir, out int lLineNumber, out int lLineHeights, out int lMinimumLineHeight);

		// Token: 0x06006BA3 RID: 27555
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsGetNumberSubpageFootnotes(IntPtr pfsContext, IntPtr pSubpage, out int cFootnotes);

		// Token: 0x06006BA4 RID: 27556
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsGetSubpageFootnoteInfo(IntPtr pfsContext, IntPtr pSubpage, int cArraySize, int indexStart, out uint fswdir, PTS.FSFTNINFO* rgFootnoteInfo, out int indexLim);

		// Token: 0x06006BA5 RID: 27557
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsTransferDisplayInfoSubpage(IntPtr pfsContext, IntPtr pSubpageOld, IntPtr pfsSubpageNew);

		// Token: 0x06006BA6 RID: 27558
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsFormatSubtrackFinite(IntPtr pfsContext, IntPtr pfsBRSubtackIn, int fFromPreviousPage, IntPtr fsnmSegment, int iArea, IntPtr pfsFtnRej, IntPtr pfsGeom, int fEmptyOk, int fSuppressTopSpace, uint fswdir, [In] ref PTS.FSRECT fsRectToFill, IntPtr pfsMcsClientIn, PTS.FSKCLEAR fsKClearIn, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstpara, out PTS.FSFMTR pfsfmtr, out IntPtr ppfsSubtrack, out IntPtr pfsBRSubtrackOut, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr ppfsMcsClientOut, out PTS.FSKCLEAR pfsKClearOut, out int pTopSpace);

		// Token: 0x06006BA7 RID: 27559
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsFormatSubtrackBottomless(IntPtr pfsContext, IntPtr fsnmSegment, int iArea, IntPtr pfsGeom, int fSuppressTopSpace, uint fswdir, int ur, int dur, int vr, IntPtr pfsMcsClientIn, PTS.FSKCLEAR fsKClearIn, int fCanBeInterruptedIn, out PTS.FSFMTRBL pfsfmtrbl, out IntPtr ppfsSubtrack, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr ppfsMcsClientOut, out PTS.FSKCLEAR pfsKClearOut, out int pTopSpace, out int pfCanBeInterruptedOut);

		// Token: 0x06006BA8 RID: 27560
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsUpdateBottomlessSubtrack(IntPtr pfsContext, IntPtr pfsSubtrack, IntPtr fsnmSegment, int iArea, IntPtr pfsGeom, int fSuppressTopSpace, uint fswdir, int ur, int dur, int vr, IntPtr pfsMcsClientIn, PTS.FSKCLEAR fsKClearIn, int fCanBeInterruptedIn, out PTS.FSFMTRBL pfsfmtrbl, out int pdvrUsed, out PTS.FSBBOX pfsBBox, out IntPtr ppfsMcsClientOut, out PTS.FSKCLEAR pfsKClearOut, out int pTopSpace, out int pfCanBeInterruptedOut);

		// Token: 0x06006BA9 RID: 27561
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsSynchronizeBottomlessSubtrack(IntPtr pfsContext, IntPtr pfsSubtrack, IntPtr pfsGeom, uint fswdir, int vrShift);

		// Token: 0x06006BAA RID: 27562
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsCompareSubtrack(IntPtr pfsContext, IntPtr pfsSubtrackOld, IntPtr pfsSubtrackNew, uint fswdir, out PTS.FSCOMPRESULT fsCompResult, out int dvrShifted);

		// Token: 0x06006BAB RID: 27563
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsClearUpdateInfoInSubtrack(IntPtr pfsContext, IntPtr pfsSubtrack);

		// Token: 0x06006BAC RID: 27564
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsDestroySubtrack(IntPtr pfsContext, IntPtr pfsSubtrack);

		// Token: 0x06006BAD RID: 27565
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsDuplicateSubtrackBreakRecord(IntPtr pfsContext, IntPtr pfsBRSubtrackIn, out IntPtr ppfsBRSubtrackOut);

		// Token: 0x06006BAE RID: 27566
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsDestroySubtrackBreakRecord(IntPtr pfscontext, IntPtr pfsbreakrec);

		// Token: 0x06006BAF RID: 27567
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsGetSubtrackColumnBalancingInfo(IntPtr pfscontext, IntPtr pfsSubtrack, uint fswdir, out int lLineNumber, out int lLineHeights, out int lMinimumLineHeight);

		// Token: 0x06006BB0 RID: 27568
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsGetNumberSubtrackFootnotes(IntPtr pfscontext, IntPtr pfsSubtrack, out int cFootnotes);

		// Token: 0x06006BB1 RID: 27569
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsTransferDisplayInfoSubtrack(IntPtr pfscontext, IntPtr pfsSubtrackOld, IntPtr pfsSubtrackNew);

		// Token: 0x06006BB2 RID: 27570
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQueryFloaterDetails(IntPtr pfsContext, IntPtr pfsfloater, out PTS.FSFLOATERDETAILS fsfloaterdetails);

		// Token: 0x06006BB3 RID: 27571
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQueryPageDetails(IntPtr pfsContext, IntPtr pPage, out PTS.FSPAGEDETAILS pPageDetails);

		// Token: 0x06006BB4 RID: 27572
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQueryPageSectionList(IntPtr pfsContext, IntPtr pPage, int cArraySize, PTS.FSSECTIONDESCRIPTION* rgSectionDescription, out int cActualSize);

		// Token: 0x06006BB5 RID: 27573
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQuerySectionDetails(IntPtr pfsContext, IntPtr pSection, out PTS.FSSECTIONDETAILS pSectionDetails);

		// Token: 0x06006BB6 RID: 27574
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQuerySectionBasicColumnList(IntPtr pfsContext, IntPtr pSection, int cArraySize, PTS.FSTRACKDESCRIPTION* rgColumnDescription, out int cActualSize);

		// Token: 0x06006BB7 RID: 27575
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQueryTrackDetails(IntPtr pfsContext, IntPtr pTrack, out PTS.FSTRACKDETAILS pTrackDetails);

		// Token: 0x06006BB8 RID: 27576
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQueryTrackParaList(IntPtr pfsContext, IntPtr pTrack, int cParas, PTS.FSPARADESCRIPTION* rgParaDesc, out int cParaDesc);

		// Token: 0x06006BB9 RID: 27577
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQuerySubpageDetails(IntPtr pfsContext, IntPtr pSubPage, out PTS.FSSUBPAGEDETAILS pSubPageDetails);

		// Token: 0x06006BBA RID: 27578
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQuerySubpageBasicColumnList(IntPtr pfsContext, IntPtr pSubPage, int cArraySize, PTS.FSTRACKDESCRIPTION* rgColumnDescription, out int cActualSize);

		// Token: 0x06006BBB RID: 27579
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQuerySubtrackDetails(IntPtr pfsContext, IntPtr pSubTrack, out PTS.FSSUBTRACKDETAILS pSubTrackDetails);

		// Token: 0x06006BBC RID: 27580
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQuerySubtrackParaList(IntPtr pfsContext, IntPtr pSubTrack, int cParas, PTS.FSPARADESCRIPTION* rgParaDesc, out int cParaDesc);

		// Token: 0x06006BBD RID: 27581
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQueryTextDetails(IntPtr pfsContext, IntPtr pPara, out PTS.FSTEXTDETAILS pTextDetails);

		// Token: 0x06006BBE RID: 27582
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQueryLineListSingle(IntPtr pfsContext, IntPtr pPara, int cLines, PTS.FSLINEDESCRIPTIONSINGLE* rgLineDesc, out int cLineDesc);

		// Token: 0x06006BBF RID: 27583
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQueryLineListComposite(IntPtr pfsContext, IntPtr pPara, int cElements, PTS.FSLINEDESCRIPTIONCOMPOSITE* rgLineDescription, out int cLineElements);

		// Token: 0x06006BC0 RID: 27584
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQueryLineCompositeElementList(IntPtr pfsContext, IntPtr pLine, int cElements, PTS.FSLINEELEMENT* rgLineElement, out int cLineElements);

		// Token: 0x06006BC1 RID: 27585
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQueryAttachedObjectList(IntPtr pfsContext, IntPtr pPara, int cAttachedObject, PTS.FSATTACHEDOBJECTDESCRIPTION* rgAttachedObjects, out int cAttachedObjectDesc);

		// Token: 0x06006BC2 RID: 27586
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQueryFigureObjectDetails(IntPtr pfsContext, IntPtr pPara, out PTS.FSFIGUREDETAILS fsFigureDetails);

		// Token: 0x06006BC3 RID: 27587
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQueryTableObjDetails(IntPtr pfscontext, IntPtr pfstableobj, out PTS.FSTABLEOBJDETAILS pfstableobjdetails);

		// Token: 0x06006BC4 RID: 27588
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQueryTableObjTableProperDetails(IntPtr pfscontext, IntPtr pfstableProper, out PTS.FSTABLEDETAILS pfstabledetailsProper);

		// Token: 0x06006BC5 RID: 27589
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQueryTableObjRowList(IntPtr pfscontext, IntPtr pfstableProper, int cRows, PTS.FSTABLEROWDESCRIPTION* rgtablerowdescr, out int pcRowsActual);

		// Token: 0x06006BC6 RID: 27590
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsQueryTableObjRowDetails(IntPtr pfscontext, IntPtr pfstablerow, out PTS.FSTABLEROWDETAILS ptableorowdetails);

		// Token: 0x06006BC7 RID: 27591
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal unsafe static extern int FsQueryTableObjCellList(IntPtr pfscontext, IntPtr pfstablerow, int cCells, PTS.FSKUPDATE* rgfskupd, IntPtr* rgpfscell, PTS.FSTABLEKCELLMERGE* rgkcellmerge, out int pcCellsActual);

		// Token: 0x06006BC8 RID: 27592
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsTransformRectangle(uint fswdirIn, ref PTS.FSRECT rectPage, ref PTS.FSRECT rectTransform, uint fswdirOut, out PTS.FSRECT rectOut);

		// Token: 0x06006BC9 RID: 27593
		[SecurityCritical]
		[SuppressUnmanagedCodeSecurity]
		[DllImport("PresentationNative_v0400.dll")]
		internal static extern int FsTransformBbox(uint fswdirIn, ref PTS.FSRECT rectPage, ref PTS.FSBBOX bboxTransform, uint fswdirOut, out PTS.FSBBOX bboxOut);

		// Token: 0x04003472 RID: 13426
		internal const int True = 1;

		// Token: 0x04003473 RID: 13427
		internal const int False = 0;

		// Token: 0x04003474 RID: 13428
		internal const int dvBottomUndefined = 2147483647;

		// Token: 0x04003475 RID: 13429
		internal const int MaxFontSize = 160000;

		// Token: 0x04003476 RID: 13430
		internal const int MaxPageSize = 3500000;

		// Token: 0x04003477 RID: 13431
		internal const int fsffiWordFlowTextFinite = 1;

		// Token: 0x04003478 RID: 13432
		internal const int fsffiWordClashFootnotesWithText = 2;

		// Token: 0x04003479 RID: 13433
		internal const int fsffiWordNewSectionAboveFootnotes = 4;

		// Token: 0x0400347A RID: 13434
		internal const int fsffiWordStopAfterFirstCollision = 8;

		// Token: 0x0400347B RID: 13435
		internal const int fsffiUseTextParaCache = 16;

		// Token: 0x0400347C RID: 13436
		internal const int fsffiKeepClientLines = 32;

		// Token: 0x0400347D RID: 13437
		internal const int fsffiUseTextQuickLoop = 64;

		// Token: 0x0400347E RID: 13438
		internal const int fsffiAvalonDisableOptimalInChains = 256;

		// Token: 0x0400347F RID: 13439
		internal const int fsffiWordAdjustTrackWidthsForFigureInWebView = 512;

		// Token: 0x04003480 RID: 13440
		internal const int fsidobjText = -1;

		// Token: 0x04003481 RID: 13441
		internal const int fsidobjFigure = -2;

		// Token: 0x04003482 RID: 13442
		internal const int fswdirDefault = 0;

		// Token: 0x04003483 RID: 13443
		internal const int fswdirES = 0;

		// Token: 0x04003484 RID: 13444
		internal const int fswdirEN = 1;

		// Token: 0x04003485 RID: 13445
		internal const int fswdirSE = 2;

		// Token: 0x04003486 RID: 13446
		internal const int fswdirSW = 3;

		// Token: 0x04003487 RID: 13447
		internal const int fswdirWS = 4;

		// Token: 0x04003488 RID: 13448
		internal const int fswdirWN = 5;

		// Token: 0x04003489 RID: 13449
		internal const int fswdirNE = 6;

		// Token: 0x0400348A RID: 13450
		internal const int fswdirNW = 7;

		// Token: 0x0400348B RID: 13451
		internal const int fUDirection = 4;

		// Token: 0x0400348C RID: 13452
		internal const int fVDirection = 1;

		// Token: 0x0400348D RID: 13453
		internal const int fUVertical = 2;

		// Token: 0x0400348E RID: 13454
		internal const int fserrNone = 0;

		// Token: 0x0400348F RID: 13455
		internal const int fserrOutOfMemory = -2;

		// Token: 0x04003490 RID: 13456
		internal const int fserrNotImplemented = -10000;

		// Token: 0x04003491 RID: 13457
		internal const int fserrCallbackException = -100002;

		// Token: 0x04003492 RID: 13458
		internal const int tserrNone = 0;

		// Token: 0x04003493 RID: 13459
		internal const int tserrInvalidParameter = -1;

		// Token: 0x04003494 RID: 13460
		internal const int tserrOutOfMemory = -2;

		// Token: 0x04003495 RID: 13461
		internal const int tserrNullOutputParameter = -3;

		// Token: 0x04003496 RID: 13462
		internal const int tserrInvalidLsContext = -4;

		// Token: 0x04003497 RID: 13463
		internal const int tserrInvalidLine = -5;

		// Token: 0x04003498 RID: 13464
		internal const int tserrInvalidDnode = -6;

		// Token: 0x04003499 RID: 13465
		internal const int tserrInvalidDeviceResolution = -7;

		// Token: 0x0400349A RID: 13466
		internal const int tserrInvalidRun = -8;

		// Token: 0x0400349B RID: 13467
		internal const int tserrMismatchLineContext = -9;

		// Token: 0x0400349C RID: 13468
		internal const int tserrContextInUse = -10;

		// Token: 0x0400349D RID: 13469
		internal const int tserrDuplicateSpecialCharacter = -11;

		// Token: 0x0400349E RID: 13470
		internal const int tserrInvalidAutonumRun = -12;

		// Token: 0x0400349F RID: 13471
		internal const int tserrFormattingFunctionDisabled = -13;

		// Token: 0x040034A0 RID: 13472
		internal const int tserrUnfinishedDnode = -14;

		// Token: 0x040034A1 RID: 13473
		internal const int tserrInvalidDnodeType = -15;

		// Token: 0x040034A2 RID: 13474
		internal const int tserrInvalidPenDnode = -16;

		// Token: 0x040034A3 RID: 13475
		internal const int tserrInvalidNonPenDnode = -17;

		// Token: 0x040034A4 RID: 13476
		internal const int tserrInvalidBaselinePenDnode = -18;

		// Token: 0x040034A5 RID: 13477
		internal const int tserrInvalidFormatterResult = -19;

		// Token: 0x040034A6 RID: 13478
		internal const int tserrInvalidObjectIdFetched = -20;

		// Token: 0x040034A7 RID: 13479
		internal const int tserrInvalidDcpFetched = -21;

		// Token: 0x040034A8 RID: 13480
		internal const int tserrInvalidCpContentFetched = -22;

		// Token: 0x040034A9 RID: 13481
		internal const int tserrInvalidBookmarkType = -23;

		// Token: 0x040034AA RID: 13482
		internal const int tserrSetDocDisabled = -24;

		// Token: 0x040034AB RID: 13483
		internal const int tserrFiniFunctionDisabled = -25;

		// Token: 0x040034AC RID: 13484
		internal const int tserrCurrentDnodeIsNotTab = -26;

		// Token: 0x040034AD RID: 13485
		internal const int tserrPendingTabIsNotResolved = -27;

		// Token: 0x040034AE RID: 13486
		internal const int tserrWrongFiniFunction = -28;

		// Token: 0x040034AF RID: 13487
		internal const int tserrInvalidBreakingClass = -29;

		// Token: 0x040034B0 RID: 13488
		internal const int tserrBreakingTableNotSet = -30;

		// Token: 0x040034B1 RID: 13489
		internal const int tserrInvalidModWidthClass = -31;

		// Token: 0x040034B2 RID: 13490
		internal const int tserrModWidthPairsNotSet = -32;

		// Token: 0x040034B3 RID: 13491
		internal const int tserrWrongTruncationPoint = -33;

		// Token: 0x040034B4 RID: 13492
		internal const int tserrWrongBreak = -34;

		// Token: 0x040034B5 RID: 13493
		internal const int tserrDupInvalid = -35;

		// Token: 0x040034B6 RID: 13494
		internal const int tserrRubyInvalidVersion = -36;

		// Token: 0x040034B7 RID: 13495
		internal const int tserrTatenakayokoInvalidVersion = -37;

		// Token: 0x040034B8 RID: 13496
		internal const int tserrWarichuInvalidVersion = -38;

		// Token: 0x040034B9 RID: 13497
		internal const int tserrWarichuInvalidData = -39;

		// Token: 0x040034BA RID: 13498
		internal const int tserrCreateSublineDisabled = -40;

		// Token: 0x040034BB RID: 13499
		internal const int tserrCurrentSublineDoesNotExist = -41;

		// Token: 0x040034BC RID: 13500
		internal const int tserrCpOutsideSubline = -42;

		// Token: 0x040034BD RID: 13501
		internal const int tserrHihInvalidVersion = -43;

		// Token: 0x040034BE RID: 13502
		internal const int tserrInsufficientQueryDepth = -44;

		// Token: 0x040034BF RID: 13503
		internal const int tserrInvalidBreakRecord = -45;

		// Token: 0x040034C0 RID: 13504
		internal const int tserrInvalidPap = -46;

		// Token: 0x040034C1 RID: 13505
		internal const int tserrContradictoryQueryInput = -47;

		// Token: 0x040034C2 RID: 13506
		internal const int tserrLineIsNotActive = -48;

		// Token: 0x040034C3 RID: 13507
		internal const int tserrTooLongParagraph = -49;

		// Token: 0x040034C4 RID: 13508
		internal const int tserrTooManyCharsToGlyph = -50;

		// Token: 0x040034C5 RID: 13509
		internal const int tserrWrongHyphenationPosition = -51;

		// Token: 0x040034C6 RID: 13510
		internal const int tserrTooManyPriorities = -52;

		// Token: 0x040034C7 RID: 13511
		internal const int tserrWrongGivenCp = -53;

		// Token: 0x040034C8 RID: 13512
		internal const int tserrWrongCpFirstForGetBreaks = -54;

		// Token: 0x040034C9 RID: 13513
		internal const int tserrWrongJustTypeForGetBreaks = -55;

		// Token: 0x040034CA RID: 13514
		internal const int tserrWrongJustTypeForCreateLineGivenCp = -56;

		// Token: 0x040034CB RID: 13515
		internal const int tserrTooLongGlyphContext = -57;

		// Token: 0x040034CC RID: 13516
		internal const int tserrInvalidCharToGlyphMapping = -58;

		// Token: 0x040034CD RID: 13517
		internal const int tserrInvalidMathUsage = -59;

		// Token: 0x040034CE RID: 13518
		internal const int tserrInconsistentChp = -60;

		// Token: 0x040034CF RID: 13519
		internal const int tserrStoppedInSubline = -61;

		// Token: 0x040034D0 RID: 13520
		internal const int tserrPenPositionCouldNotBeUsed = -62;

		// Token: 0x040034D1 RID: 13521
		internal const int tserrDebugFlagsInShip = -63;

		// Token: 0x040034D2 RID: 13522
		internal const int tserrInvalidOrderTabs = -64;

		// Token: 0x040034D3 RID: 13523
		internal const int tserrSystemRestrictionsExceeded = -100;

		// Token: 0x040034D4 RID: 13524
		internal const int tserrInvalidPtsContext = -103;

		// Token: 0x040034D5 RID: 13525
		internal const int tserrInvalidClientOutput = -104;

		// Token: 0x040034D6 RID: 13526
		internal const int tserrInvalidObjectOutput = -105;

		// Token: 0x040034D7 RID: 13527
		internal const int tserrInvalidGeometry = -106;

		// Token: 0x040034D8 RID: 13528
		internal const int tserrInvalidFootnoteRejector = -107;

		// Token: 0x040034D9 RID: 13529
		internal const int tserrInvalidFootnoteInfo = -108;

		// Token: 0x040034DA RID: 13530
		internal const int tserrOutputArrayTooSmall = -110;

		// Token: 0x040034DB RID: 13531
		internal const int tserrWordNotSupportedInBottomless = -111;

		// Token: 0x040034DC RID: 13532
		internal const int tserrPageTooLong = -112;

		// Token: 0x040034DD RID: 13533
		internal const int tserrInvalidQuery = -113;

		// Token: 0x040034DE RID: 13534
		internal const int tserrWrongWritingDirection = -114;

		// Token: 0x040034DF RID: 13535
		internal const int tserrPageNotClearedForUpdate = -115;

		// Token: 0x040034E0 RID: 13536
		internal const int tserrInternalError = -1000;

		// Token: 0x040034E1 RID: 13537
		internal const int tserrNotImplemented = -10000;

		// Token: 0x040034E2 RID: 13538
		internal const int tserrClientAbort = -100000;

		// Token: 0x040034E3 RID: 13539
		internal const int tserrPageSizeMismatch = -100001;

		// Token: 0x040034E4 RID: 13540
		internal const int tserrCallbackException = -100002;

		// Token: 0x040034E5 RID: 13541
		internal const int fsfdbgCheckVariantsConsistency = 1;

		// Token: 0x02000A29 RID: 2601
		[Serializable]
		private class SecondaryException : Exception
		{
			// Token: 0x06008AD6 RID: 35542 RVA: 0x00257D76 File Offset: 0x00255F76
			internal SecondaryException(Exception e) : base(null, e)
			{
			}

			// Token: 0x06008AD7 RID: 35543 RVA: 0x001767FC File Offset: 0x001749FC
			protected SecondaryException(SerializationInfo info, StreamingContext context) : base(info, context)
			{
			}

			// Token: 0x17001F67 RID: 8039
			// (get) Token: 0x06008AD8 RID: 35544 RVA: 0x00257D80 File Offset: 0x00255F80
			// (set) Token: 0x06008AD9 RID: 35545 RVA: 0x00257D8D File Offset: 0x00255F8D
			public override string HelpLink
			{
				get
				{
					return base.InnerException.HelpLink;
				}
				set
				{
					base.InnerException.HelpLink = value;
				}
			}

			// Token: 0x17001F68 RID: 8040
			// (get) Token: 0x06008ADA RID: 35546 RVA: 0x00257D9B File Offset: 0x00255F9B
			public override string Message
			{
				get
				{
					return base.InnerException.Message;
				}
			}

			// Token: 0x17001F69 RID: 8041
			// (get) Token: 0x06008ADB RID: 35547 RVA: 0x00257DA8 File Offset: 0x00255FA8
			// (set) Token: 0x06008ADC RID: 35548 RVA: 0x00257DB5 File Offset: 0x00255FB5
			public override string Source
			{
				get
				{
					return base.InnerException.Source;
				}
				set
				{
					base.InnerException.Source = value;
				}
			}

			// Token: 0x17001F6A RID: 8042
			// (get) Token: 0x06008ADD RID: 35549 RVA: 0x00257DC3 File Offset: 0x00255FC3
			public override string StackTrace
			{
				get
				{
					return base.InnerException.StackTrace;
				}
			}
		}

		// Token: 0x02000A2A RID: 2602
		private class PtsException : Exception
		{
			// Token: 0x06008ADE RID: 35550 RVA: 0x00127A13 File Offset: 0x00125C13
			internal PtsException(string s) : base(s)
			{
			}
		}

		// Token: 0x02000A2B RID: 2603
		internal struct Restrictions
		{
			// Token: 0x0400471D RID: 18205
			internal const int tsduRestriction = 1073741823;

			// Token: 0x0400471E RID: 18206
			internal const int tsdvRestriction = 1073741823;

			// Token: 0x0400471F RID: 18207
			internal const int tscColumnRestriction = 1000;

			// Token: 0x04004720 RID: 18208
			internal const int tscSegmentAreaRestriction = 1000;

			// Token: 0x04004721 RID: 18209
			internal const int tscHeightAreaRestriction = 1000;

			// Token: 0x04004722 RID: 18210
			internal const int tscTableColumnsRestriction = 1000;

			// Token: 0x04004723 RID: 18211
			internal const int tscFootnotesRestriction = 1000;

			// Token: 0x04004724 RID: 18212
			internal const int tscAttachedObjectsRestriction = 100000;

			// Token: 0x04004725 RID: 18213
			internal const int tscLineInParaRestriction = 1000000;

			// Token: 0x04004726 RID: 18214
			internal const int tscVerticesRestriction = 10000;

			// Token: 0x04004727 RID: 18215
			internal const int tscPolygonsRestriction = 200;

			// Token: 0x04004728 RID: 18216
			internal const int tscSeparatorsRestriction = 1000;

			// Token: 0x04004729 RID: 18217
			internal const int tscMatrixColumnsRestriction = 1000;

			// Token: 0x0400472A RID: 18218
			internal const int tscMatrixRowsRestriction = 10000;

			// Token: 0x0400472B RID: 18219
			internal const int tscEquationsRestriction = 10000;

			// Token: 0x0400472C RID: 18220
			internal const int tsduFontParameterRestriction = 50000000;

			// Token: 0x0400472D RID: 18221
			internal const int tsdvFontParameterRestriction = 50000000;

			// Token: 0x0400472E RID: 18222
			internal const int tscBreakingClassesRestriction = 200;

			// Token: 0x0400472F RID: 18223
			internal const int tscBreakingUnitsRestriction = 200;

			// Token: 0x04004730 RID: 18224
			internal const int tscModWidthClassesRestriction = 200;

			// Token: 0x04004731 RID: 18225
			internal const int tscPairActionsRestriction = 200;

			// Token: 0x04004732 RID: 18226
			internal const int tscPriorityActionsRestriction = 200;

			// Token: 0x04004733 RID: 18227
			internal const int tscExpansionUnitsRestriction = 200;

			// Token: 0x04004734 RID: 18228
			internal const int tscCharacterRestriction = 268435455;

			// Token: 0x04004735 RID: 18229
			internal const int tscInstalledHandlersRestriction = 200;

			// Token: 0x04004736 RID: 18230
			internal const int tscJustPriorityLimRestriction = 20;
		}

		// Token: 0x02000A2C RID: 2604
		internal struct FSCBK
		{
			// Token: 0x04004737 RID: 18231
			internal PTS.FSCBKGEN cbkgen;

			// Token: 0x04004738 RID: 18232
			internal PTS.FSCBKTXT cbktxt;

			// Token: 0x04004739 RID: 18233
			internal PTS.FSCBKOBJ cbkobj;

			// Token: 0x0400473A RID: 18234
			internal PTS.FSCBKFIG cbkfig;

			// Token: 0x0400473B RID: 18235
			internal PTS.FSCBKWRD cbkwrd;
		}

		// Token: 0x02000A2D RID: 2605
		internal struct FSCBKFIG
		{
			// Token: 0x0400473C RID: 18236
			[SecurityCritical]
			internal PTS.GetFigureProperties pfnGetFigureProperties;

			// Token: 0x0400473D RID: 18237
			[SecurityCritical]
			internal PTS.GetFigurePolygons pfnGetFigurePolygons;

			// Token: 0x0400473E RID: 18238
			[SecurityCritical]
			internal PTS.CalcFigurePosition pfnCalcFigurePosition;
		}

		// Token: 0x02000A2E RID: 2606
		internal enum FSKREF
		{
			// Token: 0x04004740 RID: 18240
			fskrefPage,
			// Token: 0x04004741 RID: 18241
			fskrefMargin,
			// Token: 0x04004742 RID: 18242
			fskrefParagraph,
			// Token: 0x04004743 RID: 18243
			fskrefChar,
			// Token: 0x04004744 RID: 18244
			fskrefOutOfMinMargin,
			// Token: 0x04004745 RID: 18245
			fskrefOutOfMaxMargin
		}

		// Token: 0x02000A2F RID: 2607
		internal enum FSKALIGNFIG
		{
			// Token: 0x04004747 RID: 18247
			fskalfMin,
			// Token: 0x04004748 RID: 18248
			fskalfCenter,
			// Token: 0x04004749 RID: 18249
			fskalfMax
		}

		// Token: 0x02000A30 RID: 2608
		internal struct FSFIGUREPROPS
		{
			// Token: 0x0400474A RID: 18250
			internal PTS.FSKREF fskrefU;

			// Token: 0x0400474B RID: 18251
			internal PTS.FSKREF fskrefV;

			// Token: 0x0400474C RID: 18252
			internal PTS.FSKALIGNFIG fskalfU;

			// Token: 0x0400474D RID: 18253
			internal PTS.FSKALIGNFIG fskalfV;

			// Token: 0x0400474E RID: 18254
			internal PTS.FSKWRAP fskwrap;

			// Token: 0x0400474F RID: 18255
			internal int fNonTextPlane;

			// Token: 0x04004750 RID: 18256
			internal int fAllowOverlap;

			// Token: 0x04004751 RID: 18257
			internal int fDelayable;
		}

		// Token: 0x02000A31 RID: 2609
		internal struct FSCBKGEN
		{
			// Token: 0x04004752 RID: 18258
			[SecurityCritical]
			internal PTS.FSkipPage pfnFSkipPage;

			// Token: 0x04004753 RID: 18259
			[SecurityCritical]
			internal PTS.GetPageDimensions pfnGetPageDimensions;

			// Token: 0x04004754 RID: 18260
			[SecurityCritical]
			internal PTS.GetNextSection pfnGetNextSection;

			// Token: 0x04004755 RID: 18261
			[SecurityCritical]
			internal PTS.GetSectionProperties pfnGetSectionProperties;

			// Token: 0x04004756 RID: 18262
			[SecurityCritical]
			internal PTS.GetJustificationProperties pfnGetJustificationProperties;

			// Token: 0x04004757 RID: 18263
			[SecurityCritical]
			internal PTS.GetMainTextSegment pfnGetMainTextSegment;

			// Token: 0x04004758 RID: 18264
			[SecurityCritical]
			internal PTS.GetHeaderSegment pfnGetHeaderSegment;

			// Token: 0x04004759 RID: 18265
			[SecurityCritical]
			internal PTS.GetFooterSegment pfnGetFooterSegment;

			// Token: 0x0400475A RID: 18266
			[SecurityCritical]
			internal PTS.UpdGetSegmentChange pfnUpdGetSegmentChange;

			// Token: 0x0400475B RID: 18267
			[SecurityCritical]
			internal PTS.GetSectionColumnInfo pfnGetSectionColumnInfo;

			// Token: 0x0400475C RID: 18268
			[SecurityCritical]
			internal PTS.GetSegmentDefinedColumnSpanAreaInfo pfnGetSegmentDefinedColumnSpanAreaInfo;

			// Token: 0x0400475D RID: 18269
			[SecurityCritical]
			internal PTS.GetHeightDefinedColumnSpanAreaInfo pfnGetHeightDefinedColumnSpanAreaInfo;

			// Token: 0x0400475E RID: 18270
			[SecurityCritical]
			internal PTS.GetFirstPara pfnGetFirstPara;

			// Token: 0x0400475F RID: 18271
			[SecurityCritical]
			internal PTS.GetNextPara pfnGetNextPara;

			// Token: 0x04004760 RID: 18272
			[SecurityCritical]
			internal PTS.UpdGetFirstChangeInSegment pfnUpdGetFirstChangeInSegment;

			// Token: 0x04004761 RID: 18273
			[SecurityCritical]
			internal PTS.UpdGetParaChange pfnUpdGetParaChange;

			// Token: 0x04004762 RID: 18274
			[SecurityCritical]
			internal PTS.GetParaProperties pfnGetParaProperties;

			// Token: 0x04004763 RID: 18275
			[SecurityCritical]
			internal PTS.CreateParaclient pfnCreateParaclient;

			// Token: 0x04004764 RID: 18276
			[SecurityCritical]
			internal PTS.TransferDisplayInfo pfnTransferDisplayInfo;

			// Token: 0x04004765 RID: 18277
			[SecurityCritical]
			internal PTS.DestroyParaclient pfnDestroyParaclient;

			// Token: 0x04004766 RID: 18278
			[SecurityCritical]
			internal PTS.FInterruptFormattingAfterPara pfnFInterruptFormattingAfterPara;

			// Token: 0x04004767 RID: 18279
			[SecurityCritical]
			internal PTS.GetEndnoteSeparators pfnGetEndnoteSeparators;

			// Token: 0x04004768 RID: 18280
			[SecurityCritical]
			internal PTS.GetEndnoteSegment pfnGetEndnoteSegment;

			// Token: 0x04004769 RID: 18281
			[SecurityCritical]
			internal PTS.GetNumberEndnoteColumns pfnGetNumberEndnoteColumns;

			// Token: 0x0400476A RID: 18282
			[SecurityCritical]
			internal PTS.GetEndnoteColumnInfo pfnGetEndnoteColumnInfo;

			// Token: 0x0400476B RID: 18283
			[SecurityCritical]
			internal PTS.GetFootnoteSeparators pfnGetFootnoteSeparators;

			// Token: 0x0400476C RID: 18284
			[SecurityCritical]
			internal PTS.FFootnoteBeneathText pfnFFootnoteBeneathText;

			// Token: 0x0400476D RID: 18285
			[SecurityCritical]
			internal PTS.GetNumberFootnoteColumns pfnGetNumberFootnoteColumns;

			// Token: 0x0400476E RID: 18286
			[SecurityCritical]
			internal PTS.GetFootnoteColumnInfo pfnGetFootnoteColumnInfo;

			// Token: 0x0400476F RID: 18287
			[SecurityCritical]
			internal PTS.GetFootnoteSegment pfnGetFootnoteSegment;

			// Token: 0x04004770 RID: 18288
			[SecurityCritical]
			internal PTS.GetFootnotePresentationAndRejectionOrder pfnGetFootnotePresentationAndRejectionOrder;

			// Token: 0x04004771 RID: 18289
			[SecurityCritical]
			internal PTS.FAllowFootnoteSeparation pfnFAllowFootnoteSeparation;
		}

		// Token: 0x02000A32 RID: 2610
		internal struct FSPAP
		{
			// Token: 0x04004772 RID: 18290
			internal int idobj;

			// Token: 0x04004773 RID: 18291
			internal int fKeepWithNext;

			// Token: 0x04004774 RID: 18292
			internal int fBreakPageBefore;

			// Token: 0x04004775 RID: 18293
			internal int fBreakColumnBefore;
		}

		// Token: 0x02000A33 RID: 2611
		internal struct FSCBKOBJ
		{
			// Token: 0x04004776 RID: 18294
			[SecurityCritical]
			internal IntPtr pfnNewPtr;

			// Token: 0x04004777 RID: 18295
			[SecurityCritical]
			internal IntPtr pfnDisposePtr;

			// Token: 0x04004778 RID: 18296
			[SecurityCritical]
			internal IntPtr pfnReallocPtr;

			// Token: 0x04004779 RID: 18297
			[SecurityCritical]
			internal PTS.DuplicateMcsclient pfnDuplicateMcsclient;

			// Token: 0x0400477A RID: 18298
			[SecurityCritical]
			internal PTS.DestroyMcsclient pfnDestroyMcsclient;

			// Token: 0x0400477B RID: 18299
			[SecurityCritical]
			internal PTS.FEqualMcsclient pfnFEqualMcsclient;

			// Token: 0x0400477C RID: 18300
			[SecurityCritical]
			internal PTS.ConvertMcsclient pfnConvertMcsclient;

			// Token: 0x0400477D RID: 18301
			[SecurityCritical]
			internal PTS.GetObjectHandlerInfo pfnGetObjectHandlerInfo;
		}

		// Token: 0x02000A34 RID: 2612
		internal struct FSCBKTXT
		{
			// Token: 0x0400477E RID: 18302
			[SecurityCritical]
			internal PTS.CreateParaBreakingSession pfnCreateParaBreakingSession;

			// Token: 0x0400477F RID: 18303
			[SecurityCritical]
			internal PTS.DestroyParaBreakingSession pfnDestroyParaBreakingSession;

			// Token: 0x04004780 RID: 18304
			[SecurityCritical]
			internal PTS.GetTextProperties pfnGetTextProperties;

			// Token: 0x04004781 RID: 18305
			[SecurityCritical]
			internal PTS.GetNumberFootnotes pfnGetNumberFootnotes;

			// Token: 0x04004782 RID: 18306
			[SecurityCritical]
			internal PTS.GetFootnotes pfnGetFootnotes;

			// Token: 0x04004783 RID: 18307
			[SecurityCritical]
			internal PTS.FormatDropCap pfnFormatDropCap;

			// Token: 0x04004784 RID: 18308
			[SecurityCritical]
			internal PTS.GetDropCapPolygons pfnGetDropCapPolygons;

			// Token: 0x04004785 RID: 18309
			[SecurityCritical]
			internal PTS.DestroyDropCap pfnDestroyDropCap;

			// Token: 0x04004786 RID: 18310
			[SecurityCritical]
			internal PTS.FormatBottomText pfnFormatBottomText;

			// Token: 0x04004787 RID: 18311
			[SecurityCritical]
			internal PTS.FormatLine pfnFormatLine;

			// Token: 0x04004788 RID: 18312
			[SecurityCritical]
			internal PTS.FormatLineForced pfnFormatLineForced;

			// Token: 0x04004789 RID: 18313
			[SecurityCritical]
			internal PTS.FormatLineVariants pfnFormatLineVariants;

			// Token: 0x0400478A RID: 18314
			[SecurityCritical]
			internal PTS.ReconstructLineVariant pfnReconstructLineVariant;

			// Token: 0x0400478B RID: 18315
			[SecurityCritical]
			internal PTS.DestroyLine pfnDestroyLine;

			// Token: 0x0400478C RID: 18316
			[SecurityCritical]
			internal PTS.DuplicateLineBreakRecord pfnDuplicateLineBreakRecord;

			// Token: 0x0400478D RID: 18317
			[SecurityCritical]
			internal PTS.DestroyLineBreakRecord pfnDestroyLineBreakRecord;

			// Token: 0x0400478E RID: 18318
			[SecurityCritical]
			internal PTS.SnapGridVertical pfnSnapGridVertical;

			// Token: 0x0400478F RID: 18319
			[SecurityCritical]
			internal PTS.GetDvrSuppressibleBottomSpace pfnGetDvrSuppressibleBottomSpace;

			// Token: 0x04004790 RID: 18320
			[SecurityCritical]
			internal PTS.GetDvrAdvance pfnGetDvrAdvance;

			// Token: 0x04004791 RID: 18321
			[SecurityCritical]
			internal PTS.UpdGetChangeInText pfnUpdGetChangeInText;

			// Token: 0x04004792 RID: 18322
			[SecurityCritical]
			internal PTS.UpdGetDropCapChange pfnUpdGetDropCapChange;

			// Token: 0x04004793 RID: 18323
			[SecurityCritical]
			internal PTS.FInterruptFormattingText pfnFInterruptFormattingText;

			// Token: 0x04004794 RID: 18324
			[SecurityCritical]
			internal PTS.GetTextParaCache pfnGetTextParaCache;

			// Token: 0x04004795 RID: 18325
			[SecurityCritical]
			internal PTS.SetTextParaCache pfnSetTextParaCache;

			// Token: 0x04004796 RID: 18326
			[SecurityCritical]
			internal PTS.GetOptimalLineDcpCache pfnGetOptimalLineDcpCache;

			// Token: 0x04004797 RID: 18327
			[SecurityCritical]
			internal PTS.GetNumberAttachedObjectsBeforeTextLine pfnGetNumberAttachedObjectsBeforeTextLine;

			// Token: 0x04004798 RID: 18328
			[SecurityCritical]
			internal PTS.GetAttachedObjectsBeforeTextLine pfnGetAttachedObjectsBeforeTextLine;

			// Token: 0x04004799 RID: 18329
			[SecurityCritical]
			internal PTS.GetNumberAttachedObjectsInTextLine pfnGetNumberAttachedObjectsInTextLine;

			// Token: 0x0400479A RID: 18330
			[SecurityCritical]
			internal PTS.GetAttachedObjectsInTextLine pfnGetAttachedObjectsInTextLine;

			// Token: 0x0400479B RID: 18331
			[SecurityCritical]
			internal PTS.UpdGetAttachedObjectChange pfnUpdGetAttachedObjectChange;

			// Token: 0x0400479C RID: 18332
			[SecurityCritical]
			internal PTS.GetDurFigureAnchor pfnGetDurFigureAnchor;
		}

		// Token: 0x02000A35 RID: 2613
		internal struct FSLINEVARIANT
		{
			// Token: 0x0400479D RID: 18333
			internal IntPtr pfsbreakreclineclient;

			// Token: 0x0400479E RID: 18334
			internal IntPtr pfslineclient;

			// Token: 0x0400479F RID: 18335
			internal int dcpLine;

			// Token: 0x040047A0 RID: 18336
			internal int fForceBroken;

			// Token: 0x040047A1 RID: 18337
			internal PTS.FSFLRES fslres;

			// Token: 0x040047A2 RID: 18338
			internal int dvrAscent;

			// Token: 0x040047A3 RID: 18339
			internal int dvrDescent;

			// Token: 0x040047A4 RID: 18340
			internal int fReformatNeighborsAsLastLine;

			// Token: 0x040047A5 RID: 18341
			internal IntPtr ptsLinePenaltyInfo;
		}

		// Token: 0x02000A36 RID: 2614
		internal struct FSTXTPROPS
		{
			// Token: 0x040047A6 RID: 18342
			internal uint fswdir;

			// Token: 0x040047A7 RID: 18343
			internal int dcpStartContent;

			// Token: 0x040047A8 RID: 18344
			internal int fKeepTogether;

			// Token: 0x040047A9 RID: 18345
			internal int fDropCap;

			// Token: 0x040047AA RID: 18346
			internal int cMinLinesAfterBreak;

			// Token: 0x040047AB RID: 18347
			internal int cMinLinesBeforeBreak;

			// Token: 0x040047AC RID: 18348
			internal int fVerticalGrid;

			// Token: 0x040047AD RID: 18349
			internal int fOptimizeParagraph;

			// Token: 0x040047AE RID: 18350
			internal int fAvoidHyphenationAtTrackBottom;

			// Token: 0x040047AF RID: 18351
			internal int fAvoidHyphenationOnLastChainElement;

			// Token: 0x040047B0 RID: 18352
			internal int cMaxConsecutiveHyphens;
		}

		// Token: 0x02000A37 RID: 2615
		internal enum FSKFMTLINE
		{
			// Token: 0x040047B2 RID: 18354
			fskfmtlineNormal,
			// Token: 0x040047B3 RID: 18355
			fskfmtlineOptimal,
			// Token: 0x040047B4 RID: 18356
			fskfmtlineForced,
			// Token: 0x040047B5 RID: 18357
			fskfmtlineWord
		}

		// Token: 0x02000A38 RID: 2616
		internal struct FSFMTLINEIN
		{
			// Token: 0x040047B6 RID: 18358
			internal PTS.FSKFMTLINE fskfmtline;

			// Token: 0x040047B7 RID: 18359
			internal IntPtr nmp;

			// Token: 0x040047B8 RID: 18360
			private int iArea;

			// Token: 0x040047B9 RID: 18361
			private int dcpStartLine;

			// Token: 0x040047BA RID: 18362
			private IntPtr pbrLineIn;

			// Token: 0x040047BB RID: 18363
			private int urStartLine;

			// Token: 0x040047BC RID: 18364
			private int durLine;

			// Token: 0x040047BD RID: 18365
			private int urStartTrack;

			// Token: 0x040047BE RID: 18366
			private int durTrack;

			// Token: 0x040047BF RID: 18367
			private int urPageLeftMargin;

			// Token: 0x040047C0 RID: 18368
			private int fAllowHyphenation;

			// Token: 0x040047C1 RID: 18369
			private int fClearOnleft;

			// Token: 0x040047C2 RID: 18370
			private int fClearOnRight;

			// Token: 0x040047C3 RID: 18371
			private int fTreatAsFirstInPara;

			// Token: 0x040047C4 RID: 18372
			private int fTreatAsLastInPara;

			// Token: 0x040047C5 RID: 18373
			private int fSuppressTopSpace;

			// Token: 0x040047C6 RID: 18374
			private int dcpLine;

			// Token: 0x040047C7 RID: 18375
			private int dvrAvailable;

			// Token: 0x040047C8 RID: 18376
			private int fChain;

			// Token: 0x040047C9 RID: 18377
			private int vrStartLine;

			// Token: 0x040047CA RID: 18378
			private int urStartLr;

			// Token: 0x040047CB RID: 18379
			private int durLr;

			// Token: 0x040047CC RID: 18380
			private int fHitByPolygon;

			// Token: 0x040047CD RID: 18381
			private int fClearLeftLrWord;

			// Token: 0x040047CE RID: 18382
			private int fClearRightLrWord;
		}

		// Token: 0x02000A39 RID: 2617
		internal struct FSCBKWRD
		{
			// Token: 0x040047CF RID: 18383
			[SecurityCritical]
			internal IntPtr pfnGetSectionHorizMargins;

			// Token: 0x040047D0 RID: 18384
			[SecurityCritical]
			internal IntPtr pfnFPerformColumnBalancing;

			// Token: 0x040047D1 RID: 18385
			[SecurityCritical]
			internal IntPtr pfnCalculateColumnBalancingApproximateHeight;

			// Token: 0x040047D2 RID: 18386
			[SecurityCritical]
			internal IntPtr pfnCalculateColumnBalancingStep;

			// Token: 0x040047D3 RID: 18387
			[SecurityCritical]
			internal IntPtr pfnGetColumnSectionBreak;

			// Token: 0x040047D4 RID: 18388
			[SecurityCritical]
			internal IntPtr pfnFSuppressKeepWithNextAtTopOfPage;

			// Token: 0x040047D5 RID: 18389
			[SecurityCritical]
			internal IntPtr pfnFSuppressKeepTogetherAtTopOfPage;

			// Token: 0x040047D6 RID: 18390
			[SecurityCritical]
			internal IntPtr pfnFAllowSpaceAfterOverhang;

			// Token: 0x040047D7 RID: 18391
			[SecurityCritical]
			internal IntPtr pfnFormatLineWord;

			// Token: 0x040047D8 RID: 18392
			[SecurityCritical]
			internal IntPtr pfnGetSuppressedTopSpace;

			// Token: 0x040047D9 RID: 18393
			[SecurityCritical]
			internal IntPtr pfnChangeSplatLineHeight;

			// Token: 0x040047DA RID: 18394
			[SecurityCritical]
			internal IntPtr pfnGetDvrAdvanceWord;

			// Token: 0x040047DB RID: 18395
			[SecurityCritical]
			internal IntPtr pfnGetMinDvrAdvance;

			// Token: 0x040047DC RID: 18396
			[SecurityCritical]
			internal IntPtr pfnGetDurTooNarrowForFigure;

			// Token: 0x040047DD RID: 18397
			[SecurityCritical]
			internal IntPtr pfnResolveOverlap;

			// Token: 0x040047DE RID: 18398
			[SecurityCritical]
			internal IntPtr pfnGetOffsetForFlowAroundAndBBox;

			// Token: 0x040047DF RID: 18399
			[SecurityCritical]
			internal IntPtr pfnGetClientGeometryHandle;

			// Token: 0x040047E0 RID: 18400
			[SecurityCritical]
			internal IntPtr pfnDuplicateClientGeometryHandle;

			// Token: 0x040047E1 RID: 18401
			[SecurityCritical]
			internal IntPtr pfnDestroyClientGeometryHandle;

			// Token: 0x040047E2 RID: 18402
			[SecurityCritical]
			internal IntPtr pfnObstacleAddNotification;

			// Token: 0x040047E3 RID: 18403
			[SecurityCritical]
			internal IntPtr pfnGetFigureObstaclesForRestart;

			// Token: 0x040047E4 RID: 18404
			[SecurityCritical]
			internal IntPtr pfnRepositionFigure;

			// Token: 0x040047E5 RID: 18405
			[SecurityCritical]
			internal IntPtr pfnFStopBeforeLr;

			// Token: 0x040047E6 RID: 18406
			[SecurityCritical]
			internal IntPtr pfnFStopBeforeLine;

			// Token: 0x040047E7 RID: 18407
			[SecurityCritical]
			internal IntPtr pfnFIgnoreCollision;

			// Token: 0x040047E8 RID: 18408
			[SecurityCritical]
			internal IntPtr pfnGetNumberOfLinesForColumnBalancing;

			// Token: 0x040047E9 RID: 18409
			[SecurityCritical]
			internal IntPtr pfnFCancelPageBreakBefore;

			// Token: 0x040047EA RID: 18410
			[SecurityCritical]
			internal IntPtr pfnChangeVrTopLineForFigure;

			// Token: 0x040047EB RID: 18411
			[SecurityCritical]
			internal IntPtr pfnFApplyWidowOrphanControlInFootnoteResolution;
		}

		// Token: 0x02000A3A RID: 2618
		internal struct FSCOLUMNINFO
		{
			// Token: 0x040047EC RID: 18412
			internal int durBefore;

			// Token: 0x040047ED RID: 18413
			internal int durWidth;
		}

		// Token: 0x02000A3B RID: 2619
		internal enum FSCOMPRESULT
		{
			// Token: 0x040047EF RID: 18415
			fscmprNoChange,
			// Token: 0x040047F0 RID: 18416
			fscmprChangeInside,
			// Token: 0x040047F1 RID: 18417
			fscmprShifted
		}

		// Token: 0x02000A3C RID: 2620
		internal struct FSCONTEXTINFO
		{
			// Token: 0x040047F2 RID: 18418
			[SecurityCritical]
			internal uint version;

			// Token: 0x040047F3 RID: 18419
			[SecurityCritical]
			internal uint fsffi;

			// Token: 0x040047F4 RID: 18420
			[SecurityCritical]
			internal int drMinColumnBalancingStep;

			// Token: 0x040047F5 RID: 18421
			[SecurityCritical]
			internal int cInstalledObjects;

			// Token: 0x040047F6 RID: 18422
			[SecurityCritical]
			internal IntPtr pInstalledObjects;

			// Token: 0x040047F7 RID: 18423
			[SecurityCritical]
			internal IntPtr pfsclient;

			// Token: 0x040047F8 RID: 18424
			[SecurityCritical]
			internal IntPtr ptsPenaltyModule;

			// Token: 0x040047F9 RID: 18425
			[SecurityCritical]
			internal PTS.FSCBK fscbk;

			// Token: 0x040047FA RID: 18426
			[SecurityCritical]
			internal PTS.AssertFailed pfnAssertFailed;
		}

		// Token: 0x02000A3D RID: 2621
		internal struct FSRECT
		{
			// Token: 0x06008ADF RID: 35551 RVA: 0x00257DD0 File Offset: 0x00255FD0
			internal FSRECT(int inU, int inV, int inDU, int inDV)
			{
				this.u = inU;
				this.v = inV;
				this.du = inDU;
				this.dv = inDV;
			}

			// Token: 0x06008AE0 RID: 35552 RVA: 0x00257DEF File Offset: 0x00255FEF
			internal FSRECT(PTS.FSRECT rect)
			{
				this.u = rect.u;
				this.v = rect.v;
				this.du = rect.du;
				this.dv = rect.dv;
			}

			// Token: 0x06008AE1 RID: 35553 RVA: 0x00257E24 File Offset: 0x00256024
			internal FSRECT(Rect rect)
			{
				if (!rect.IsEmpty)
				{
					this.u = TextDpi.ToTextDpi(rect.Left);
					this.v = TextDpi.ToTextDpi(rect.Top);
					this.du = TextDpi.ToTextDpi(rect.Width);
					this.dv = TextDpi.ToTextDpi(rect.Height);
					return;
				}
				this.u = (this.v = (this.du = (this.dv = 0)));
			}

			// Token: 0x06008AE2 RID: 35554 RVA: 0x00257EA5 File Offset: 0x002560A5
			public static bool operator ==(PTS.FSRECT rect1, PTS.FSRECT rect2)
			{
				return rect1.u == rect2.u && rect1.v == rect2.v && rect1.du == rect2.du && rect1.dv == rect2.dv;
			}

			// Token: 0x06008AE3 RID: 35555 RVA: 0x00257EE1 File Offset: 0x002560E1
			public static bool operator !=(PTS.FSRECT rect1, PTS.FSRECT rect2)
			{
				return !(rect1 == rect2);
			}

			// Token: 0x06008AE4 RID: 35556 RVA: 0x00257EED File Offset: 0x002560ED
			public override bool Equals(object o)
			{
				return o is PTS.FSRECT && (PTS.FSRECT)o == this;
			}

			// Token: 0x06008AE5 RID: 35557 RVA: 0x00257F0A File Offset: 0x0025610A
			public override int GetHashCode()
			{
				return this.u.GetHashCode() ^ this.v.GetHashCode() ^ this.du.GetHashCode() ^ this.dv.GetHashCode();
			}

			// Token: 0x06008AE6 RID: 35558 RVA: 0x001CC818 File Offset: 0x001CAA18
			internal Rect FromTextDpi()
			{
				return new Rect(TextDpi.FromTextDpi(this.u), TextDpi.FromTextDpi(this.v), TextDpi.FromTextDpi(this.du), TextDpi.FromTextDpi(this.dv));
			}

			// Token: 0x06008AE7 RID: 35559 RVA: 0x00257F3C File Offset: 0x0025613C
			internal bool Contains(PTS.FSPOINT point)
			{
				return point.u >= this.u && point.u <= this.u + this.du && point.v >= this.v && point.v <= this.v + this.dv;
			}

			// Token: 0x040047FB RID: 18427
			internal int u;

			// Token: 0x040047FC RID: 18428
			internal int v;

			// Token: 0x040047FD RID: 18429
			internal int du;

			// Token: 0x040047FE RID: 18430
			internal int dv;
		}

		// Token: 0x02000A3E RID: 2622
		internal struct FSPOINT
		{
			// Token: 0x06008AE8 RID: 35560 RVA: 0x00257F94 File Offset: 0x00256194
			internal FSPOINT(int inU, int inV)
			{
				this.u = inU;
				this.v = inV;
			}

			// Token: 0x06008AE9 RID: 35561 RVA: 0x00257FA4 File Offset: 0x002561A4
			public static bool operator ==(PTS.FSPOINT point1, PTS.FSPOINT point2)
			{
				return point1.u == point2.u && point1.v == point2.v;
			}

			// Token: 0x06008AEA RID: 35562 RVA: 0x00257FC4 File Offset: 0x002561C4
			public static bool operator !=(PTS.FSPOINT point1, PTS.FSPOINT point2)
			{
				return !(point1 == point2);
			}

			// Token: 0x06008AEB RID: 35563 RVA: 0x00257FD0 File Offset: 0x002561D0
			public override bool Equals(object o)
			{
				return o is PTS.FSPOINT && (PTS.FSPOINT)o == this;
			}

			// Token: 0x06008AEC RID: 35564 RVA: 0x00257FED File Offset: 0x002561ED
			public override int GetHashCode()
			{
				return this.u.GetHashCode() ^ this.v.GetHashCode();
			}

			// Token: 0x040047FF RID: 18431
			internal int u;

			// Token: 0x04004800 RID: 18432
			internal int v;
		}

		// Token: 0x02000A3F RID: 2623
		internal struct FSVECTOR
		{
			// Token: 0x06008AED RID: 35565 RVA: 0x00258006 File Offset: 0x00256206
			internal FSVECTOR(int inDU, int inDV)
			{
				this.du = inDU;
				this.dv = inDV;
			}

			// Token: 0x06008AEE RID: 35566 RVA: 0x00258016 File Offset: 0x00256216
			public static bool operator ==(PTS.FSVECTOR vector1, PTS.FSVECTOR vector2)
			{
				return vector1.du == vector2.du && vector1.dv == vector2.dv;
			}

			// Token: 0x06008AEF RID: 35567 RVA: 0x00258036 File Offset: 0x00256236
			public static bool operator !=(PTS.FSVECTOR vector1, PTS.FSVECTOR vector2)
			{
				return !(vector1 == vector2);
			}

			// Token: 0x06008AF0 RID: 35568 RVA: 0x00258042 File Offset: 0x00256242
			public override bool Equals(object o)
			{
				return o is PTS.FSVECTOR && (PTS.FSVECTOR)o == this;
			}

			// Token: 0x06008AF1 RID: 35569 RVA: 0x0025805F File Offset: 0x0025625F
			public override int GetHashCode()
			{
				return this.du.GetHashCode() ^ this.dv.GetHashCode();
			}

			// Token: 0x06008AF2 RID: 35570 RVA: 0x00258078 File Offset: 0x00256278
			internal Vector FromTextDpi()
			{
				return new Vector(TextDpi.FromTextDpi(this.du), TextDpi.FromTextDpi(this.dv));
			}

			// Token: 0x04004801 RID: 18433
			internal int du;

			// Token: 0x04004802 RID: 18434
			internal int dv;
		}

		// Token: 0x02000A40 RID: 2624
		internal struct FSBBOX
		{
			// Token: 0x04004803 RID: 18435
			internal int fDefined;

			// Token: 0x04004804 RID: 18436
			internal PTS.FSRECT fsrc;
		}

		// Token: 0x02000A41 RID: 2625
		internal struct FSFIGOBSTINFO
		{
			// Token: 0x04004805 RID: 18437
			internal IntPtr nmpFigure;

			// Token: 0x04004806 RID: 18438
			internal PTS.FSFLOWAROUND flow;

			// Token: 0x04004807 RID: 18439
			internal PTS.FSPOLYGONINFO polyginfo;

			// Token: 0x04004808 RID: 18440
			internal PTS.FSOVERLAP overlap;

			// Token: 0x04004809 RID: 18441
			internal PTS.FSBBOX fsbbox;

			// Token: 0x0400480A RID: 18442
			internal PTS.FSPOINT fsptPosPreliminary;

			// Token: 0x0400480B RID: 18443
			internal int fNonTextPlane;

			// Token: 0x0400480C RID: 18444
			internal int fUTextRelative;

			// Token: 0x0400480D RID: 18445
			internal int fVTextRelative;
		}

		// Token: 0x02000A42 RID: 2626
		internal struct FSFIGOBSTRESTARTINFO
		{
			// Token: 0x0400480E RID: 18446
			internal IntPtr nmpFigure;

			// Token: 0x0400480F RID: 18447
			internal int fReached;

			// Token: 0x04004810 RID: 18448
			internal int fNonTextPlane;
		}

		// Token: 0x02000A43 RID: 2627
		internal struct FSFLOATERCBK
		{
			// Token: 0x04004811 RID: 18449
			[SecurityCritical]
			internal PTS.GetFloaterProperties pfnGetFloaterProperties;

			// Token: 0x04004812 RID: 18450
			[SecurityCritical]
			internal PTS.FormatFloaterContentFinite pfnFormatFloaterContentFinite;

			// Token: 0x04004813 RID: 18451
			[SecurityCritical]
			internal PTS.FormatFloaterContentBottomless pfnFormatFloaterContentBottomless;

			// Token: 0x04004814 RID: 18452
			[SecurityCritical]
			internal PTS.UpdateBottomlessFloaterContent pfnUpdateBottomlessFloaterContent;

			// Token: 0x04004815 RID: 18453
			[SecurityCritical]
			internal PTS.GetFloaterPolygons pfnGetFloaterPolygons;

			// Token: 0x04004816 RID: 18454
			[SecurityCritical]
			internal PTS.ClearUpdateInfoInFloaterContent pfnClearUpdateInfoInFloaterContent;

			// Token: 0x04004817 RID: 18455
			[SecurityCritical]
			internal PTS.CompareFloaterContents pfnCompareFloaterContents;

			// Token: 0x04004818 RID: 18456
			[SecurityCritical]
			internal PTS.DestroyFloaterContent pfnDestroyFloaterContent;

			// Token: 0x04004819 RID: 18457
			[SecurityCritical]
			internal PTS.DuplicateFloaterContentBreakRecord pfnDuplicateFloaterContentBreakRecord;

			// Token: 0x0400481A RID: 18458
			[SecurityCritical]
			internal PTS.DestroyFloaterContentBreakRecord pfnDestroyFloaterContentBreakRecord;

			// Token: 0x0400481B RID: 18459
			[SecurityCritical]
			internal PTS.GetFloaterContentColumnBalancingInfo pfnGetFloaterContentColumnBalancingInfo;

			// Token: 0x0400481C RID: 18460
			[SecurityCritical]
			internal PTS.GetFloaterContentNumberFootnotes pfnGetFloaterContentNumberFootnotes;

			// Token: 0x0400481D RID: 18461
			[SecurityCritical]
			internal PTS.GetFloaterContentFootnoteInfo pfnGetFloaterContentFootnoteInfo;

			// Token: 0x0400481E RID: 18462
			[SecurityCritical]
			internal PTS.TransferDisplayInfoInFloaterContent pfnTransferDisplayInfoInFloaterContent;

			// Token: 0x0400481F RID: 18463
			[SecurityCritical]
			internal PTS.GetMCSClientAfterFloater pfnGetMCSClientAfterFloater;

			// Token: 0x04004820 RID: 18464
			[SecurityCritical]
			internal PTS.GetDvrUsedForFloater pfnGetDvrUsedForFloater;
		}

		// Token: 0x02000A44 RID: 2628
		internal enum FSKFLOATALIGNMENT
		{
			// Token: 0x04004822 RID: 18466
			fskfloatalignMin,
			// Token: 0x04004823 RID: 18467
			fskfloatalignCenter,
			// Token: 0x04004824 RID: 18468
			fskfloatalignMax
		}

		// Token: 0x02000A45 RID: 2629
		internal struct FSFLOATERPROPS
		{
			// Token: 0x04004825 RID: 18469
			internal PTS.FSKCLEAR fskclear;

			// Token: 0x04004826 RID: 18470
			internal PTS.FSKFLOATALIGNMENT fskfloatalignment;

			// Token: 0x04004827 RID: 18471
			internal int fFloat;

			// Token: 0x04004828 RID: 18472
			internal PTS.FSKWRAP fskwr;

			// Token: 0x04004829 RID: 18473
			internal int fDelayNoProgress;

			// Token: 0x0400482A RID: 18474
			internal int durDistTextLeft;

			// Token: 0x0400482B RID: 18475
			internal int durDistTextRight;

			// Token: 0x0400482C RID: 18476
			internal int dvrDistTextTop;

			// Token: 0x0400482D RID: 18477
			internal int dvrDistTextBottom;
		}

		// Token: 0x02000A46 RID: 2630
		internal struct FSFLOATERINIT
		{
			// Token: 0x0400482E RID: 18478
			internal PTS.FSFLOATERCBK fsfloatercbk;
		}

		// Token: 0x02000A47 RID: 2631
		internal struct FSFLOATERDETAILS
		{
			// Token: 0x0400482F RID: 18479
			internal PTS.FSKUPDATE fskupdContent;

			// Token: 0x04004830 RID: 18480
			internal IntPtr fsnmFloater;

			// Token: 0x04004831 RID: 18481
			internal PTS.FSRECT fsrcFloater;

			// Token: 0x04004832 RID: 18482
			internal IntPtr pfsFloaterContent;
		}

		// Token: 0x02000A48 RID: 2632
		internal enum FSFLRES
		{
			// Token: 0x04004834 RID: 18484
			fsflrOutOfSpace,
			// Token: 0x04004835 RID: 18485
			fsflrOutOfSpaceHyphenated,
			// Token: 0x04004836 RID: 18486
			fsflrEndOfParagraph,
			// Token: 0x04004837 RID: 18487
			fsflrEndOfParagraphClearLeft,
			// Token: 0x04004838 RID: 18488
			fsflrEndOfParagraphClearRight,
			// Token: 0x04004839 RID: 18489
			fsflrEndOfParagraphClearBoth,
			// Token: 0x0400483A RID: 18490
			fsflrPageBreak,
			// Token: 0x0400483B RID: 18491
			fsflrColumnBreak,
			// Token: 0x0400483C RID: 18492
			fsflrSoftBreak,
			// Token: 0x0400483D RID: 18493
			fsflrSoftBreakClearLeft,
			// Token: 0x0400483E RID: 18494
			fsflrSoftBreakClearRight,
			// Token: 0x0400483F RID: 18495
			fsflrSoftBreakClearBoth,
			// Token: 0x04004840 RID: 18496
			fsflrNoProgressClear
		}

		// Token: 0x02000A49 RID: 2633
		internal struct FSFLTOBSTINFO
		{
			// Token: 0x04004841 RID: 18497
			internal PTS.FSFLOWAROUND flow;

			// Token: 0x04004842 RID: 18498
			internal PTS.FSPOLYGONINFO polyginfo;

			// Token: 0x04004843 RID: 18499
			internal int fSuppressAutoClear;
		}

		// Token: 0x02000A4A RID: 2634
		internal enum FSFMTRKSTOP
		{
			// Token: 0x04004845 RID: 18501
			fmtrGoalReached,
			// Token: 0x04004846 RID: 18502
			fmtrBrokenOutOfSpace,
			// Token: 0x04004847 RID: 18503
			fmtrBrokenPageBreak,
			// Token: 0x04004848 RID: 18504
			fmtrBrokenColumnBreak,
			// Token: 0x04004849 RID: 18505
			fmtrBrokenPageBreakBeforePara,
			// Token: 0x0400484A RID: 18506
			fmtrBrokenColumnBreakBeforePara,
			// Token: 0x0400484B RID: 18507
			fmtrBrokenPageBreakBeforeSection,
			// Token: 0x0400484C RID: 18508
			fmtrBrokenDelayable,
			// Token: 0x0400484D RID: 18509
			fmtrNoProgressOutOfSpace,
			// Token: 0x0400484E RID: 18510
			fmtrNoProgressPageBreak,
			// Token: 0x0400484F RID: 18511
			fmtrNoProgressPageBreakBeforePara,
			// Token: 0x04004850 RID: 18512
			fmtrNoProgressColumnBreakBeforePara,
			// Token: 0x04004851 RID: 18513
			fmtrNoProgressPageBreakBeforeSection,
			// Token: 0x04004852 RID: 18514
			fmtrNoProgressPageSkipped,
			// Token: 0x04004853 RID: 18515
			fmtrNoProgressDelayable,
			// Token: 0x04004854 RID: 18516
			fmtrCollision
		}

		// Token: 0x02000A4B RID: 2635
		internal struct FSFMTR
		{
			// Token: 0x04004855 RID: 18517
			internal PTS.FSFMTRKSTOP kstop;

			// Token: 0x04004856 RID: 18518
			internal int fContainsItemThatStoppedBeforeFootnote;

			// Token: 0x04004857 RID: 18519
			internal int fForcedProgress;
		}

		// Token: 0x02000A4C RID: 2636
		internal enum FSFMTRBL
		{
			// Token: 0x04004859 RID: 18521
			fmtrblGoalReached,
			// Token: 0x0400485A RID: 18522
			fmtrblCollision,
			// Token: 0x0400485B RID: 18523
			fmtrblInterrupted
		}

		// Token: 0x02000A4D RID: 2637
		internal struct FSFTNINFO
		{
			// Token: 0x0400485C RID: 18524
			internal IntPtr nmftn;

			// Token: 0x0400485D RID: 18525
			internal int vrAccept;

			// Token: 0x0400485E RID: 18526
			internal int vrReject;
		}

		// Token: 0x02000A4E RID: 2638
		internal struct FSINTERVAL
		{
			// Token: 0x0400485F RID: 18527
			internal int ur;

			// Token: 0x04004860 RID: 18528
			internal int dur;
		}

		// Token: 0x02000A4F RID: 2639
		internal struct FSFILLINFO
		{
			// Token: 0x04004861 RID: 18529
			internal PTS.FSRECT fsrc;

			// Token: 0x04004862 RID: 18530
			internal int fLastInPara;
		}

		// Token: 0x02000A50 RID: 2640
		internal struct FSEMPTYSPACE
		{
			// Token: 0x04004863 RID: 18531
			internal int ur;

			// Token: 0x04004864 RID: 18532
			internal int dur;

			// Token: 0x04004865 RID: 18533
			internal int fPolygonInside;
		}

		// Token: 0x02000A51 RID: 2641
		internal enum FSHYPHENQUALITY
		{
			// Token: 0x04004867 RID: 18535
			fshqExcellent,
			// Token: 0x04004868 RID: 18536
			fshqGood,
			// Token: 0x04004869 RID: 18537
			fshqFair,
			// Token: 0x0400486A RID: 18538
			fshqPoor,
			// Token: 0x0400486B RID: 18539
			fshqBad
		}

		// Token: 0x02000A52 RID: 2642
		internal struct FSIMETHODS
		{
			// Token: 0x0400486C RID: 18540
			[SecurityCritical]
			internal PTS.ObjCreateContext pfnCreateContext;

			// Token: 0x0400486D RID: 18541
			[SecurityCritical]
			internal PTS.ObjDestroyContext pfnDestroyContext;

			// Token: 0x0400486E RID: 18542
			[SecurityCritical]
			internal PTS.ObjFormatParaFinite pfnFormatParaFinite;

			// Token: 0x0400486F RID: 18543
			[SecurityCritical]
			internal PTS.ObjFormatParaBottomless pfnFormatParaBottomless;

			// Token: 0x04004870 RID: 18544
			[SecurityCritical]
			internal PTS.ObjUpdateBottomlessPara pfnUpdateBottomlessPara;

			// Token: 0x04004871 RID: 18545
			[SecurityCritical]
			internal PTS.ObjSynchronizeBottomlessPara pfnSynchronizeBottomlessPara;

			// Token: 0x04004872 RID: 18546
			[SecurityCritical]
			internal PTS.ObjComparePara pfnComparePara;

			// Token: 0x04004873 RID: 18547
			[SecurityCritical]
			internal PTS.ObjClearUpdateInfoInPara pfnClearUpdateInfoInPara;

			// Token: 0x04004874 RID: 18548
			[SecurityCritical]
			internal PTS.ObjDestroyPara pfnDestroyPara;

			// Token: 0x04004875 RID: 18549
			[SecurityCritical]
			internal PTS.ObjDuplicateBreakRecord pfnDuplicateBreakRecord;

			// Token: 0x04004876 RID: 18550
			[SecurityCritical]
			internal PTS.ObjDestroyBreakRecord pfnDestroyBreakRecord;

			// Token: 0x04004877 RID: 18551
			[SecurityCritical]
			internal PTS.ObjGetColumnBalancingInfo pfnGetColumnBalancingInfo;

			// Token: 0x04004878 RID: 18552
			[SecurityCritical]
			internal PTS.ObjGetNumberFootnotes pfnGetNumberFootnotes;

			// Token: 0x04004879 RID: 18553
			[SecurityCritical]
			internal PTS.ObjGetFootnoteInfo pfnGetFootnoteInfo;

			// Token: 0x0400487A RID: 18554
			[SecurityCritical]
			internal IntPtr pfnGetFootnoteInfoWord;

			// Token: 0x0400487B RID: 18555
			[SecurityCritical]
			internal PTS.ObjShiftVertical pfnShiftVertical;

			// Token: 0x0400487C RID: 18556
			[SecurityCritical]
			internal PTS.ObjTransferDisplayInfoPara pfnTransferDisplayInfoPara;
		}

		// Token: 0x02000A53 RID: 2643
		internal enum FSKALIGNPAGE
		{
			// Token: 0x0400487E RID: 18558
			fskalpgTop,
			// Token: 0x0400487F RID: 18559
			fskalpgCenter,
			// Token: 0x04004880 RID: 18560
			fskalpgBottom
		}

		// Token: 0x02000A54 RID: 2644
		internal enum FSKCHANGE
		{
			// Token: 0x04004882 RID: 18562
			fskchNone,
			// Token: 0x04004883 RID: 18563
			fskchNew,
			// Token: 0x04004884 RID: 18564
			fskchInside
		}

		// Token: 0x02000A55 RID: 2645
		internal enum FSKCLEAR
		{
			// Token: 0x04004886 RID: 18566
			fskclearNone,
			// Token: 0x04004887 RID: 18567
			fskclearLeft,
			// Token: 0x04004888 RID: 18568
			fskclearRight,
			// Token: 0x04004889 RID: 18569
			fskclearBoth
		}

		// Token: 0x02000A56 RID: 2646
		internal enum FSKWRAP
		{
			// Token: 0x0400488B RID: 18571
			fskwrNone,
			// Token: 0x0400488C RID: 18572
			fskwrLeft,
			// Token: 0x0400488D RID: 18573
			fskwrRight,
			// Token: 0x0400488E RID: 18574
			fskwrBoth,
			// Token: 0x0400488F RID: 18575
			fskwrLargest
		}

		// Token: 0x02000A57 RID: 2647
		internal enum FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA
		{
			// Token: 0x04004891 RID: 18577
			fsksuppresshardbreakbeforefirstparaNone,
			// Token: 0x04004892 RID: 18578
			fsksuppresshardbreakbeforefirstparaColumn,
			// Token: 0x04004893 RID: 18579
			fsksuppresshardbreakbeforefirstparaPageAndColumn
		}

		// Token: 0x02000A58 RID: 2648
		internal struct FSFLOWAROUND
		{
			// Token: 0x04004894 RID: 18580
			internal PTS.FSRECT fsrcBounding;

			// Token: 0x04004895 RID: 18581
			internal PTS.FSKWRAP fskwr;

			// Token: 0x04004896 RID: 18582
			internal int durTooNarrow;

			// Token: 0x04004897 RID: 18583
			internal int durDistTextLeft;

			// Token: 0x04004898 RID: 18584
			internal int durDistTextRight;

			// Token: 0x04004899 RID: 18585
			internal int dvrDistTextTop;

			// Token: 0x0400489A RID: 18586
			internal int dvrDistTextBottom;
		}

		// Token: 0x02000A59 RID: 2649
		internal struct FSPOLYGONINFO
		{
			// Token: 0x0400489B RID: 18587
			internal int cPolygons;

			// Token: 0x0400489C RID: 18588
			[SecurityCritical]
			internal unsafe int* rgcVertices;

			// Token: 0x0400489D RID: 18589
			internal int cfspt;

			// Token: 0x0400489E RID: 18590
			[SecurityCritical]
			internal unsafe PTS.FSPOINT* rgfspt;

			// Token: 0x0400489F RID: 18591
			internal int fWrapThrough;
		}

		// Token: 0x02000A5A RID: 2650
		internal struct FSOVERLAP
		{
			// Token: 0x040048A0 RID: 18592
			internal PTS.FSRECT fsrc;

			// Token: 0x040048A1 RID: 18593
			internal int fAllowOverlap;
		}

		// Token: 0x02000A5B RID: 2651
		internal struct FSFIGUREDETAILS
		{
			// Token: 0x040048A2 RID: 18594
			internal PTS.FSRECT fsrcFlowAround;

			// Token: 0x040048A3 RID: 18595
			internal PTS.FSBBOX fsbbox;

			// Token: 0x040048A4 RID: 18596
			internal PTS.FSPOINT fsptPosPreliminary;

			// Token: 0x040048A5 RID: 18597
			internal int fDelayed;
		}

		// Token: 0x02000A5C RID: 2652
		internal struct FSLINEELEMENT
		{
			// Token: 0x040048A6 RID: 18598
			internal IntPtr pfslineclient;

			// Token: 0x040048A7 RID: 18599
			internal int dcpFirst;

			// Token: 0x040048A8 RID: 18600
			internal IntPtr pfsbreakreclineclient;

			// Token: 0x040048A9 RID: 18601
			internal int dcpLim;

			// Token: 0x040048AA RID: 18602
			internal int urStart;

			// Token: 0x040048AB RID: 18603
			internal int dur;

			// Token: 0x040048AC RID: 18604
			internal int fAllowHyphenation;

			// Token: 0x040048AD RID: 18605
			internal int urBBox;

			// Token: 0x040048AE RID: 18606
			internal int durBBox;

			// Token: 0x040048AF RID: 18607
			internal int urLrWord;

			// Token: 0x040048B0 RID: 18608
			internal int durLrWord;

			// Token: 0x040048B1 RID: 18609
			internal int dvrAscent;

			// Token: 0x040048B2 RID: 18610
			internal int dvrDescent;

			// Token: 0x040048B3 RID: 18611
			internal int fClearOnLeft;

			// Token: 0x040048B4 RID: 18612
			internal int fClearOnRight;

			// Token: 0x040048B5 RID: 18613
			internal int fHitByPolygon;

			// Token: 0x040048B6 RID: 18614
			internal int fForceBroken;

			// Token: 0x040048B7 RID: 18615
			internal int fClearLeftLrWord;

			// Token: 0x040048B8 RID: 18616
			internal int fClearRightLrWord;
		}

		// Token: 0x02000A5D RID: 2653
		internal struct FSLINEDESCRIPTIONCOMPOSITE
		{
			// Token: 0x040048B9 RID: 18617
			internal IntPtr pline;

			// Token: 0x040048BA RID: 18618
			internal int cElements;

			// Token: 0x040048BB RID: 18619
			internal int vrStart;

			// Token: 0x040048BC RID: 18620
			internal int dvrAscent;

			// Token: 0x040048BD RID: 18621
			internal int dvrDescent;

			// Token: 0x040048BE RID: 18622
			internal int fTreatedAsFirst;

			// Token: 0x040048BF RID: 18623
			internal int fTreatedAsLast;

			// Token: 0x040048C0 RID: 18624
			internal int dvrAvailableForcedLine;

			// Token: 0x040048C1 RID: 18625
			internal int fUsedWordFormatLineInChain;

			// Token: 0x040048C2 RID: 18626
			internal int fFirstLineInWordLr;
		}

		// Token: 0x02000A5E RID: 2654
		internal struct FSLINEDESCRIPTIONSINGLE
		{
			// Token: 0x040048C3 RID: 18627
			internal IntPtr pfslineclient;

			// Token: 0x040048C4 RID: 18628
			internal IntPtr pfsbreakreclineclient;

			// Token: 0x040048C5 RID: 18629
			internal int dcpFirst;

			// Token: 0x040048C6 RID: 18630
			internal int dcpLim;

			// Token: 0x040048C7 RID: 18631
			internal int urStart;

			// Token: 0x040048C8 RID: 18632
			internal int dur;

			// Token: 0x040048C9 RID: 18633
			internal int fAllowHyphenation;

			// Token: 0x040048CA RID: 18634
			internal int urBBox;

			// Token: 0x040048CB RID: 18635
			internal int durBBox;

			// Token: 0x040048CC RID: 18636
			internal int vrStart;

			// Token: 0x040048CD RID: 18637
			internal int dvrAscent;

			// Token: 0x040048CE RID: 18638
			internal int dvrDescent;

			// Token: 0x040048CF RID: 18639
			internal int fClearOnLeft;

			// Token: 0x040048D0 RID: 18640
			internal int fClearOnRight;

			// Token: 0x040048D1 RID: 18641
			internal int fTreatedAsFirst;

			// Token: 0x040048D2 RID: 18642
			internal int fForceBroken;
		}

		// Token: 0x02000A5F RID: 2655
		internal struct FSATTACHEDOBJECTDESCRIPTION
		{
			// Token: 0x040048D3 RID: 18643
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x040048D4 RID: 18644
			internal IntPtr pfspara;

			// Token: 0x040048D5 RID: 18645
			internal IntPtr pfsparaclient;

			// Token: 0x040048D6 RID: 18646
			internal IntPtr nmp;

			// Token: 0x040048D7 RID: 18647
			internal int idobj;

			// Token: 0x040048D8 RID: 18648
			internal int vrStart;

			// Token: 0x040048D9 RID: 18649
			internal int dvrUsed;

			// Token: 0x040048DA RID: 18650
			internal PTS.FSBBOX fsbbox;

			// Token: 0x040048DB RID: 18651
			internal int dvrTopSpace;
		}

		// Token: 0x02000A60 RID: 2656
		internal struct FSDROPCAPDETAILS
		{
			// Token: 0x040048DC RID: 18652
			internal PTS.FSRECT fsrcDropCap;

			// Token: 0x040048DD RID: 18653
			internal int fSuppressDropCapTopSpacing;

			// Token: 0x040048DE RID: 18654
			internal IntPtr pdcclient;
		}

		// Token: 0x02000A61 RID: 2657
		internal enum FSKTEXTLINES
		{
			// Token: 0x040048E0 RID: 18656
			fsklinesNormal,
			// Token: 0x040048E1 RID: 18657
			fsklinesOptimal,
			// Token: 0x040048E2 RID: 18658
			fsklinesForced,
			// Token: 0x040048E3 RID: 18659
			fsklinesWord
		}

		// Token: 0x02000A62 RID: 2658
		internal struct FSTEXTDETAILSFULL
		{
			// Token: 0x040048E4 RID: 18660
			internal uint fswdir;

			// Token: 0x040048E5 RID: 18661
			internal PTS.FSKTEXTLINES fsklines;

			// Token: 0x040048E6 RID: 18662
			internal int fLinesComposite;

			// Token: 0x040048E7 RID: 18663
			internal int cLines;

			// Token: 0x040048E8 RID: 18664
			internal int cAttachedObjects;

			// Token: 0x040048E9 RID: 18665
			internal int dcpFirst;

			// Token: 0x040048EA RID: 18666
			internal int dcpLim;

			// Token: 0x040048EB RID: 18667
			internal int fDropCapPresent;

			// Token: 0x040048EC RID: 18668
			internal PTS.FSUPDATEINFO fsupdinfDropCap;

			// Token: 0x040048ED RID: 18669
			internal PTS.FSDROPCAPDETAILS dcdetails;

			// Token: 0x040048EE RID: 18670
			internal int fSuppressTopLineSpacing;

			// Token: 0x040048EF RID: 18671
			internal int fUpdateInfoForLinesPresent;

			// Token: 0x040048F0 RID: 18672
			internal int cLinesBeforeChange;

			// Token: 0x040048F1 RID: 18673
			internal int dvrShiftBeforeChange;

			// Token: 0x040048F2 RID: 18674
			internal int cLinesChanged;

			// Token: 0x040048F3 RID: 18675
			internal int dcLinesChanged;

			// Token: 0x040048F4 RID: 18676
			internal int dvrShiftAfterChange;

			// Token: 0x040048F5 RID: 18677
			internal int ddcpAfterChange;
		}

		// Token: 0x02000A63 RID: 2659
		internal struct FSTEXTDETAILSCACHED
		{
			// Token: 0x040048F6 RID: 18678
			internal uint fswdir;

			// Token: 0x040048F7 RID: 18679
			internal PTS.FSKTEXTLINES fsklines;

			// Token: 0x040048F8 RID: 18680
			internal PTS.FSRECT fsrcPara;

			// Token: 0x040048F9 RID: 18681
			internal int fSuppressTopLineSpacing;

			// Token: 0x040048FA RID: 18682
			internal int dcpFirst;

			// Token: 0x040048FB RID: 18683
			internal int dcpLim;

			// Token: 0x040048FC RID: 18684
			internal int cLines;

			// Token: 0x040048FD RID: 18685
			internal int fClearOnLeft;

			// Token: 0x040048FE RID: 18686
			internal int fClearOnRight;

			// Token: 0x040048FF RID: 18687
			internal int fOptimalLineDcpsCached;
		}

		// Token: 0x02000A64 RID: 2660
		internal enum FSKTEXTDETAILS
		{
			// Token: 0x04004901 RID: 18689
			fsktdCached,
			// Token: 0x04004902 RID: 18690
			fsktdFull
		}

		// Token: 0x02000A65 RID: 2661
		internal struct FSTEXTDETAILS
		{
			// Token: 0x04004903 RID: 18691
			internal PTS.FSKTEXTDETAILS fsktd;

			// Token: 0x04004904 RID: 18692
			internal PTS.FSTEXTDETAILS.nested_u u;

			// Token: 0x02000BB8 RID: 3000
			[StructLayout(LayoutKind.Explicit)]
			internal struct nested_u
			{
				// Token: 0x04004EFA RID: 20218
				[FieldOffset(0)]
				internal PTS.FSTEXTDETAILSFULL full;

				// Token: 0x04004EFB RID: 20219
				[FieldOffset(0)]
				internal PTS.FSTEXTDETAILSCACHED cached;
			}
		}

		// Token: 0x02000A66 RID: 2662
		internal struct FSPARADESCRIPTION
		{
			// Token: 0x04004905 RID: 18693
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04004906 RID: 18694
			internal IntPtr pfspara;

			// Token: 0x04004907 RID: 18695
			internal IntPtr pfsparaclient;

			// Token: 0x04004908 RID: 18696
			internal IntPtr nmp;

			// Token: 0x04004909 RID: 18697
			internal int idobj;

			// Token: 0x0400490A RID: 18698
			internal int dvrUsed;

			// Token: 0x0400490B RID: 18699
			internal PTS.FSBBOX fsbbox;

			// Token: 0x0400490C RID: 18700
			internal int dvrTopSpace;
		}

		// Token: 0x02000A67 RID: 2663
		internal struct FSTRACKDETAILS
		{
			// Token: 0x0400490D RID: 18701
			internal int cParas;
		}

		// Token: 0x02000A68 RID: 2664
		internal struct FSTRACKDESCRIPTION
		{
			// Token: 0x0400490E RID: 18702
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x0400490F RID: 18703
			internal IntPtr nms;

			// Token: 0x04004910 RID: 18704
			internal PTS.FSRECT fsrc;

			// Token: 0x04004911 RID: 18705
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04004912 RID: 18706
			internal int fTrackRelativeToRect;

			// Token: 0x04004913 RID: 18707
			internal IntPtr pfstrack;
		}

		// Token: 0x02000A69 RID: 2665
		internal struct FSSUBTRACKDETAILS
		{
			// Token: 0x04004914 RID: 18708
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04004915 RID: 18709
			internal IntPtr nms;

			// Token: 0x04004916 RID: 18710
			internal PTS.FSRECT fsrc;

			// Token: 0x04004917 RID: 18711
			internal int cParas;
		}

		// Token: 0x02000A6A RID: 2666
		internal struct FSSUBPAGEDETAILSCOMPLEX
		{
			// Token: 0x04004918 RID: 18712
			internal IntPtr nms;

			// Token: 0x04004919 RID: 18713
			internal uint fswdir;

			// Token: 0x0400491A RID: 18714
			internal PTS.FSRECT fsrc;

			// Token: 0x0400491B RID: 18715
			internal PTS.FSBBOX fsbbox;

			// Token: 0x0400491C RID: 18716
			internal int cBasicColumns;

			// Token: 0x0400491D RID: 18717
			internal int cSegmentDefinedColumnSpanAreas;

			// Token: 0x0400491E RID: 18718
			internal int cHeightDefinedColumnSpanAreas;
		}

		// Token: 0x02000A6B RID: 2667
		internal struct FSSUBPAGEDETAILSSIMPLE
		{
			// Token: 0x0400491F RID: 18719
			internal uint fswdir;

			// Token: 0x04004920 RID: 18720
			internal PTS.FSTRACKDESCRIPTION trackdescr;
		}

		// Token: 0x02000A6C RID: 2668
		internal struct FSSUBPAGEDETAILS
		{
			// Token: 0x04004921 RID: 18721
			internal int fSimple;

			// Token: 0x04004922 RID: 18722
			internal PTS.FSSUBPAGEDETAILS.nested_u u;

			// Token: 0x02000BB9 RID: 3001
			[StructLayout(LayoutKind.Explicit)]
			internal struct nested_u
			{
				// Token: 0x04004EFC RID: 20220
				[FieldOffset(0)]
				internal PTS.FSSUBPAGEDETAILSSIMPLE simple;

				// Token: 0x04004EFD RID: 20221
				[FieldOffset(0)]
				internal PTS.FSSUBPAGEDETAILSCOMPLEX complex;
			}
		}

		// Token: 0x02000A6D RID: 2669
		internal struct FSCOMPOSITECOLUMNDETAILS
		{
			// Token: 0x04004923 RID: 18723
			internal PTS.FSTRACKDESCRIPTION trackdescrMainText;

			// Token: 0x04004924 RID: 18724
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteSeparator;

			// Token: 0x04004925 RID: 18725
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteContinuationSeparator;

			// Token: 0x04004926 RID: 18726
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteContinuationNotice;

			// Token: 0x04004927 RID: 18727
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteSeparator;

			// Token: 0x04004928 RID: 18728
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteContinuationSeparator;

			// Token: 0x04004929 RID: 18729
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteContinuationNotice;

			// Token: 0x0400492A RID: 18730
			internal int cFootnotes;

			// Token: 0x0400492B RID: 18731
			internal PTS.FSRECT fsrcFootnotes;

			// Token: 0x0400492C RID: 18732
			internal PTS.FSBBOX fsbboxFootnotes;

			// Token: 0x0400492D RID: 18733
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnote;
		}

		// Token: 0x02000A6E RID: 2670
		internal struct FSENDNOTECOLUMNDETAILS
		{
			// Token: 0x0400492E RID: 18734
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteContinuationSeparator;

			// Token: 0x0400492F RID: 18735
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteContinuationNotice;

			// Token: 0x04004930 RID: 18736
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnote;
		}

		// Token: 0x02000A6F RID: 2671
		internal struct FSCOMPOSITECOLUMNDESCRIPTION
		{
			// Token: 0x04004931 RID: 18737
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04004932 RID: 18738
			internal PTS.FSRECT fsrc;

			// Token: 0x04004933 RID: 18739
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04004934 RID: 18740
			internal IntPtr pfscompcol;
		}

		// Token: 0x02000A70 RID: 2672
		internal struct FSENDNOTECOLUMNDESCRIPTION
		{
			// Token: 0x04004935 RID: 18741
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04004936 RID: 18742
			internal PTS.FSRECT fsrc;

			// Token: 0x04004937 RID: 18743
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04004938 RID: 18744
			internal IntPtr pfsendnotecol;
		}

		// Token: 0x02000A71 RID: 2673
		internal struct FSSECTIONDETAILSWITHPAGENOTES
		{
			// Token: 0x04004939 RID: 18745
			internal uint fswdir;

			// Token: 0x0400493A RID: 18746
			internal int fColumnBalancingApplied;

			// Token: 0x0400493B RID: 18747
			internal PTS.FSRECT fsrcSectionBody;

			// Token: 0x0400493C RID: 18748
			internal PTS.FSBBOX fsbboxSectionBody;

			// Token: 0x0400493D RID: 18749
			internal int cBasicColumns;

			// Token: 0x0400493E RID: 18750
			internal int cSegmentDefinedColumnSpanAreas;

			// Token: 0x0400493F RID: 18751
			internal int cHeightDefinedColumnSpanAreas;

			// Token: 0x04004940 RID: 18752
			internal PTS.FSRECT fsrcEndnote;

			// Token: 0x04004941 RID: 18753
			internal PTS.FSBBOX fsbboxEndnote;

			// Token: 0x04004942 RID: 18754
			internal int cEndnoteColumns;

			// Token: 0x04004943 RID: 18755
			internal PTS.FSTRACKDESCRIPTION trackdescrEndnoteSeparator;
		}

		// Token: 0x02000A72 RID: 2674
		internal struct FSSECTIONDETAILSWITHCOLNOTES
		{
			// Token: 0x04004944 RID: 18756
			internal uint fswdir;

			// Token: 0x04004945 RID: 18757
			internal int fColumnBalancingApplied;

			// Token: 0x04004946 RID: 18758
			internal int cCompositeColumns;
		}

		// Token: 0x02000A73 RID: 2675
		internal struct FSSECTIONDETAILS
		{
			// Token: 0x04004947 RID: 18759
			internal int fFootnotesAsPagenotes;

			// Token: 0x04004948 RID: 18760
			internal PTS.FSSECTIONDETAILS.nested_u u;

			// Token: 0x02000BBA RID: 3002
			[StructLayout(LayoutKind.Explicit)]
			internal struct nested_u
			{
				// Token: 0x04004EFE RID: 20222
				[FieldOffset(0)]
				internal PTS.FSSECTIONDETAILSWITHPAGENOTES withpagenotes;

				// Token: 0x04004EFF RID: 20223
				[FieldOffset(0)]
				internal PTS.FSSECTIONDETAILSWITHCOLNOTES withcolumnnotes;
			}
		}

		// Token: 0x02000A74 RID: 2676
		internal struct FSSECTIONDESCRIPTION
		{
			// Token: 0x04004949 RID: 18761
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x0400494A RID: 18762
			internal IntPtr nms;

			// Token: 0x0400494B RID: 18763
			internal PTS.FSRECT fsrc;

			// Token: 0x0400494C RID: 18764
			internal PTS.FSBBOX fsbbox;

			// Token: 0x0400494D RID: 18765
			internal int fOtherSectionInside;

			// Token: 0x0400494E RID: 18766
			internal int dvrUsedTop;

			// Token: 0x0400494F RID: 18767
			internal int dvrUsedBottom;

			// Token: 0x04004950 RID: 18768
			internal IntPtr pfssection;
		}

		// Token: 0x02000A75 RID: 2677
		internal struct FSFOOTNOTECOLUMNDETAILS
		{
			// Token: 0x04004951 RID: 18769
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteContinuationSeparator;

			// Token: 0x04004952 RID: 18770
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteContinuationNotice;

			// Token: 0x04004953 RID: 18771
			internal int cTracks;
		}

		// Token: 0x02000A76 RID: 2678
		internal struct FSFOOTNOTECOLUMNDESCRIPTION
		{
			// Token: 0x04004954 RID: 18772
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x04004955 RID: 18773
			internal PTS.FSRECT fsrc;

			// Token: 0x04004956 RID: 18774
			internal PTS.FSBBOX fsbbox;

			// Token: 0x04004957 RID: 18775
			internal IntPtr pfsfootnotecol;
		}

		// Token: 0x02000A77 RID: 2679
		internal struct FSPAGEDETAILSCOMPLEX
		{
			// Token: 0x04004958 RID: 18776
			internal int fTopBottomHeaderFooter;

			// Token: 0x04004959 RID: 18777
			internal uint fswdirHeader;

			// Token: 0x0400495A RID: 18778
			internal PTS.FSTRACKDESCRIPTION trackdescrHeader;

			// Token: 0x0400495B RID: 18779
			internal uint fswdirFooter;

			// Token: 0x0400495C RID: 18780
			internal PTS.FSTRACKDESCRIPTION trackdescrFooter;

			// Token: 0x0400495D RID: 18781
			internal int fJustified;

			// Token: 0x0400495E RID: 18782
			internal PTS.FSKALIGNPAGE fskalpg;

			// Token: 0x0400495F RID: 18783
			internal uint fswdirPageProper;

			// Token: 0x04004960 RID: 18784
			internal PTS.FSUPDATEINFO fsupdinfPageBody;

			// Token: 0x04004961 RID: 18785
			internal PTS.FSRECT fsrcPageBody;

			// Token: 0x04004962 RID: 18786
			internal PTS.FSRECT fsrcPageMarginActual;

			// Token: 0x04004963 RID: 18787
			internal PTS.FSBBOX fsbboxPageBody;

			// Token: 0x04004964 RID: 18788
			internal int cSections;

			// Token: 0x04004965 RID: 18789
			internal PTS.FSRECT fsrcFootnote;

			// Token: 0x04004966 RID: 18790
			internal PTS.FSBBOX fsbboxFootnote;

			// Token: 0x04004967 RID: 18791
			internal int cFootnoteColumns;

			// Token: 0x04004968 RID: 18792
			internal PTS.FSTRACKDESCRIPTION trackdescrFootnoteSeparator;
		}

		// Token: 0x02000A78 RID: 2680
		internal struct FSPAGEDETAILSSIMPLE
		{
			// Token: 0x04004969 RID: 18793
			internal PTS.FSTRACKDESCRIPTION trackdescr;
		}

		// Token: 0x02000A79 RID: 2681
		internal struct FSPAGEDETAILS
		{
			// Token: 0x0400496A RID: 18794
			internal PTS.FSKUPDATE fskupd;

			// Token: 0x0400496B RID: 18795
			internal int fSimple;

			// Token: 0x0400496C RID: 18796
			internal PTS.FSPAGEDETAILS.nested_u u;

			// Token: 0x02000BBB RID: 3003
			[StructLayout(LayoutKind.Explicit)]
			internal struct nested_u
			{
				// Token: 0x04004F00 RID: 20224
				[FieldOffset(0)]
				internal PTS.FSPAGEDETAILSSIMPLE simple;

				// Token: 0x04004F01 RID: 20225
				[FieldOffset(0)]
				internal PTS.FSPAGEDETAILSCOMPLEX complex;
			}
		}

		// Token: 0x02000A7A RID: 2682
		internal struct FSTABLECBKFETCH
		{
			// Token: 0x0400496D RID: 18797
			[SecurityCritical]
			internal PTS.GetFirstHeaderRow pfnGetFirstHeaderRow;

			// Token: 0x0400496E RID: 18798
			[SecurityCritical]
			internal PTS.GetNextHeaderRow pfnGetNextHeaderRow;

			// Token: 0x0400496F RID: 18799
			[SecurityCritical]
			internal PTS.GetFirstFooterRow pfnGetFirstFooterRow;

			// Token: 0x04004970 RID: 18800
			[SecurityCritical]
			internal PTS.GetNextFooterRow pfnGetNextFooterRow;

			// Token: 0x04004971 RID: 18801
			[SecurityCritical]
			internal PTS.GetFirstRow pfnGetFirstRow;

			// Token: 0x04004972 RID: 18802
			[SecurityCritical]
			internal PTS.GetNextRow pfnGetNextRow;

			// Token: 0x04004973 RID: 18803
			[SecurityCritical]
			internal PTS.UpdFChangeInHeaderFooter pfnUpdFChangeInHeaderFooter;

			// Token: 0x04004974 RID: 18804
			[SecurityCritical]
			internal PTS.UpdGetFirstChangeInTable pfnUpdGetFirstChangeInTable;

			// Token: 0x04004975 RID: 18805
			[SecurityCritical]
			internal PTS.UpdGetRowChange pfnUpdGetRowChange;

			// Token: 0x04004976 RID: 18806
			[SecurityCritical]
			internal PTS.UpdGetCellChange pfnUpdGetCellChange;

			// Token: 0x04004977 RID: 18807
			[SecurityCritical]
			internal PTS.GetDistributionKind pfnGetDistributionKind;

			// Token: 0x04004978 RID: 18808
			[SecurityCritical]
			internal PTS.GetRowProperties pfnGetRowProperties;

			// Token: 0x04004979 RID: 18809
			[SecurityCritical]
			internal PTS.GetCells pfnGetCells;

			// Token: 0x0400497A RID: 18810
			[SecurityCritical]
			internal PTS.FInterruptFormattingTable pfnFInterruptFormattingTable;

			// Token: 0x0400497B RID: 18811
			[SecurityCritical]
			internal PTS.CalcHorizontalBBoxOfRow pfnCalcHorizontalBBoxOfRow;
		}

		// Token: 0x02000A7B RID: 2683
		internal struct FSTABLECBKCELL
		{
			// Token: 0x0400497C RID: 18812
			[SecurityCritical]
			internal PTS.FormatCellFinite pfnFormatCellFinite;

			// Token: 0x0400497D RID: 18813
			[SecurityCritical]
			internal PTS.FormatCellBottomless pfnFormatCellBottomless;

			// Token: 0x0400497E RID: 18814
			[SecurityCritical]
			internal PTS.UpdateBottomlessCell pfnUpdateBottomlessCell;

			// Token: 0x0400497F RID: 18815
			[SecurityCritical]
			internal PTS.CompareCells pfnCompareCells;

			// Token: 0x04004980 RID: 18816
			[SecurityCritical]
			internal PTS.ClearUpdateInfoInCell pfnClearUpdateInfoInCell;

			// Token: 0x04004981 RID: 18817
			[SecurityCritical]
			internal PTS.SetCellHeight pfnSetCellHeight;

			// Token: 0x04004982 RID: 18818
			[SecurityCritical]
			internal PTS.DestroyCell pfnDestroyCell;

			// Token: 0x04004983 RID: 18819
			[SecurityCritical]
			internal PTS.DuplicateCellBreakRecord pfnDuplicateCellBreakRecord;

			// Token: 0x04004984 RID: 18820
			[SecurityCritical]
			internal PTS.DestroyCellBreakRecord pfnDestroyCellBreakRecord;

			// Token: 0x04004985 RID: 18821
			[SecurityCritical]
			internal PTS.GetCellNumberFootnotes pfnGetCellNumberFootnotes;

			// Token: 0x04004986 RID: 18822
			[SecurityCritical]
			internal IntPtr pfnGetCellFootnoteInfo;

			// Token: 0x04004987 RID: 18823
			[SecurityCritical]
			internal IntPtr pfnGetCellFootnoteInfoWord;

			// Token: 0x04004988 RID: 18824
			[SecurityCritical]
			internal PTS.GetCellMinColumnBalancingStep pfnGetCellMinColumnBalancingStep;

			// Token: 0x04004989 RID: 18825
			[SecurityCritical]
			internal PTS.TransferDisplayInfoCell pfnTransferDisplayInfoCell;
		}

		// Token: 0x02000A7C RID: 2684
		internal enum FSKTABLEHEIGHTDISTRIBUTION
		{
			// Token: 0x0400498B RID: 18827
			fskdistributeUnchanged,
			// Token: 0x0400498C RID: 18828
			fskdistributeEqually,
			// Token: 0x0400498D RID: 18829
			fskdistributeProportionally
		}

		// Token: 0x02000A7D RID: 2685
		internal enum FSKROWHEIGHTRESTRICTION
		{
			// Token: 0x0400498F RID: 18831
			fskrowheightNatural,
			// Token: 0x04004990 RID: 18832
			fskrowheightAtLeast,
			// Token: 0x04004991 RID: 18833
			fskrowheightAtMostNoBreak,
			// Token: 0x04004992 RID: 18834
			fskrowheightExactNoBreak
		}

		// Token: 0x02000A7E RID: 2686
		internal enum FSKROWBREAKRESTRICTION
		{
			// Token: 0x04004994 RID: 18836
			fskrowbreakAnywhere,
			// Token: 0x04004995 RID: 18837
			fskrowbreakNoBreakInside,
			// Token: 0x04004996 RID: 18838
			fskrowbreakNoBreakInsideAfter
		}

		// Token: 0x02000A7F RID: 2687
		internal struct FSTABLEROWPROPS
		{
			// Token: 0x04004997 RID: 18839
			internal PTS.FSKROWBREAKRESTRICTION fskrowbreak;

			// Token: 0x04004998 RID: 18840
			internal PTS.FSKROWHEIGHTRESTRICTION fskrowheight;

			// Token: 0x04004999 RID: 18841
			internal int dvrRowHeightRestriction;

			// Token: 0x0400499A RID: 18842
			internal int fBBoxOverflowsBottom;

			// Token: 0x0400499B RID: 18843
			internal int dvrAboveRow;

			// Token: 0x0400499C RID: 18844
			internal int dvrBelowRow;

			// Token: 0x0400499D RID: 18845
			internal int dvrAboveTopRow;

			// Token: 0x0400499E RID: 18846
			internal int dvrBelowBottomRow;

			// Token: 0x0400499F RID: 18847
			internal int dvrAboveRowBreak;

			// Token: 0x040049A0 RID: 18848
			internal int dvrBelowRowBreak;

			// Token: 0x040049A1 RID: 18849
			internal int cCells;
		}

		// Token: 0x02000A80 RID: 2688
		internal enum FSTABLEKCELLMERGE
		{
			// Token: 0x040049A3 RID: 18851
			fskcellmergeNo,
			// Token: 0x040049A4 RID: 18852
			fskcellmergeFirst,
			// Token: 0x040049A5 RID: 18853
			fskcellmergeMiddle,
			// Token: 0x040049A6 RID: 18854
			fskcellmergeLast
		}

		// Token: 0x02000A81 RID: 2689
		internal enum FSKTABLEOBJALIGNMENT
		{
			// Token: 0x040049A8 RID: 18856
			fsktableobjAlignLeft,
			// Token: 0x040049A9 RID: 18857
			fsktableobjAlignRight,
			// Token: 0x040049AA RID: 18858
			fsktableobjAlignCenter
		}

		// Token: 0x02000A82 RID: 2690
		internal struct FSTABLEOBJPROPS
		{
			// Token: 0x040049AB RID: 18859
			internal PTS.FSKCLEAR fskclear;

			// Token: 0x040049AC RID: 18860
			internal PTS.FSKTABLEOBJALIGNMENT ktablealignment;

			// Token: 0x040049AD RID: 18861
			internal int fFloat;

			// Token: 0x040049AE RID: 18862
			internal PTS.FSKWRAP fskwr;

			// Token: 0x040049AF RID: 18863
			internal int fDelayNoProgress;

			// Token: 0x040049B0 RID: 18864
			internal int dvrCaptionTop;

			// Token: 0x040049B1 RID: 18865
			internal int dvrCaptionBottom;

			// Token: 0x040049B2 RID: 18866
			internal int durCaptionLeft;

			// Token: 0x040049B3 RID: 18867
			internal int durCaptionRight;

			// Token: 0x040049B4 RID: 18868
			internal uint fswdirTable;
		}

		// Token: 0x02000A83 RID: 2691
		internal struct FSTABLEOBJCBK
		{
			// Token: 0x040049B5 RID: 18869
			[SecurityCritical]
			internal PTS.GetTableProperties pfnGetTableProperties;

			// Token: 0x040049B6 RID: 18870
			[SecurityCritical]
			internal PTS.AutofitTable pfnAutofitTable;

			// Token: 0x040049B7 RID: 18871
			[SecurityCritical]
			internal PTS.UpdAutofitTable pfnUpdAutofitTable;

			// Token: 0x040049B8 RID: 18872
			[SecurityCritical]
			internal PTS.GetMCSClientAfterTable pfnGetMCSClientAfterTable;

			// Token: 0x040049B9 RID: 18873
			[SecurityCritical]
			internal IntPtr pfnGetDvrUsedForFloatTable;
		}

		// Token: 0x02000A84 RID: 2692
		internal struct FSTABLECBKFETCHWORD
		{
			// Token: 0x040049BA RID: 18874
			[SecurityCritical]
			internal IntPtr pfnGetTablePropertiesWord;

			// Token: 0x040049BB RID: 18875
			[SecurityCritical]
			internal IntPtr pfnGetRowPropertiesWord;

			// Token: 0x040049BC RID: 18876
			[SecurityCritical]
			internal IntPtr pfnGetRowWidthWord;

			// Token: 0x040049BD RID: 18877
			[SecurityCritical]
			internal IntPtr pfnGetNumberFiguresForTableRow;

			// Token: 0x040049BE RID: 18878
			[SecurityCritical]
			internal IntPtr pfnGetFiguresForTableRow;

			// Token: 0x040049BF RID: 18879
			[SecurityCritical]
			internal IntPtr pfnFStopBeforeTableRowLr;

			// Token: 0x040049C0 RID: 18880
			[SecurityCritical]
			internal IntPtr pfnFIgnoreCollisionForTableRow;

			// Token: 0x040049C1 RID: 18881
			[SecurityCritical]
			internal IntPtr pfnChangeRowHeightRestriction;

			// Token: 0x040049C2 RID: 18882
			[SecurityCritical]
			internal IntPtr pfnNotifyRowPosition;

			// Token: 0x040049C3 RID: 18883
			[SecurityCritical]
			internal IntPtr pfnNotifyRowBorderAbove;

			// Token: 0x040049C4 RID: 18884
			[SecurityCritical]
			internal IntPtr pfnNotifyTableBreakRec;
		}

		// Token: 0x02000A85 RID: 2693
		internal struct FSTABLEOBJINIT
		{
			// Token: 0x040049C5 RID: 18885
			internal PTS.FSTABLEOBJCBK tableobjcbk;

			// Token: 0x040049C6 RID: 18886
			internal PTS.FSTABLECBKFETCH tablecbkfetch;

			// Token: 0x040049C7 RID: 18887
			internal PTS.FSTABLECBKCELL tablecbkcell;

			// Token: 0x040049C8 RID: 18888
			internal PTS.FSTABLECBKFETCHWORD tablecbkfetchword;
		}

		// Token: 0x02000A86 RID: 2694
		internal struct FSTABLEOBJDETAILS
		{
			// Token: 0x040049C9 RID: 18889
			internal IntPtr fsnmTable;

			// Token: 0x040049CA RID: 18890
			internal PTS.FSRECT fsrcTableObj;

			// Token: 0x040049CB RID: 18891
			internal int dvrTopCaption;

			// Token: 0x040049CC RID: 18892
			internal int dvrBottomCaption;

			// Token: 0x040049CD RID: 18893
			internal int durLeftCaption;

			// Token: 0x040049CE RID: 18894
			internal int durRightCaption;

			// Token: 0x040049CF RID: 18895
			internal uint fswdirTable;

			// Token: 0x040049D0 RID: 18896
			internal PTS.FSKUPDATE fskupdTableProper;

			// Token: 0x040049D1 RID: 18897
			internal IntPtr pfstableProper;
		}

		// Token: 0x02000A87 RID: 2695
		internal struct FSTABLEDETAILS
		{
			// Token: 0x040049D2 RID: 18898
			internal int dvrTable;

			// Token: 0x040049D3 RID: 18899
			internal int cRows;
		}

		// Token: 0x02000A88 RID: 2696
		internal struct FSTABLEROWDESCRIPTION
		{
			// Token: 0x040049D4 RID: 18900
			internal PTS.FSUPDATEINFO fsupdinf;

			// Token: 0x040049D5 RID: 18901
			internal IntPtr fsnmRow;

			// Token: 0x040049D6 RID: 18902
			internal IntPtr pfstablerow;

			// Token: 0x040049D7 RID: 18903
			internal int fRowInSeparateRect;

			// Token: 0x040049D8 RID: 18904
			internal PTS.FSTABLEROWDESCRIPTION.nested_u u;

			// Token: 0x02000BBC RID: 3004
			[StructLayout(LayoutKind.Explicit)]
			internal struct nested_u
			{
				// Token: 0x04004F02 RID: 20226
				[FieldOffset(0)]
				internal PTS.FSRECT fsrcRow;

				// Token: 0x04004F03 RID: 20227
				[FieldOffset(0)]
				internal int dvrRow;
			}
		}

		// Token: 0x02000A89 RID: 2697
		internal enum FSKTABLEROWBOUNDARY
		{
			// Token: 0x040049DA RID: 18906
			fsktablerowboundaryOuter,
			// Token: 0x040049DB RID: 18907
			fsktablerowboundaryInner,
			// Token: 0x040049DC RID: 18908
			fsktablerowboundaryBreak
		}

		// Token: 0x02000A8A RID: 2698
		internal struct FSTABLEROWDETAILS
		{
			// Token: 0x040049DD RID: 18909
			internal PTS.FSKTABLEROWBOUNDARY fskboundaryAbove;

			// Token: 0x040049DE RID: 18910
			internal int dvrAbove;

			// Token: 0x040049DF RID: 18911
			internal PTS.FSKTABLEROWBOUNDARY fskboundaryBelow;

			// Token: 0x040049E0 RID: 18912
			internal int dvrBelow;

			// Token: 0x040049E1 RID: 18913
			internal int cCells;

			// Token: 0x040049E2 RID: 18914
			internal int fForcedRow;
		}

		// Token: 0x02000A8B RID: 2699
		internal struct FSTABLESRVCONTEXT
		{
			// Token: 0x040049E3 RID: 18915
			internal IntPtr pfscontext;

			// Token: 0x040049E4 RID: 18916
			internal IntPtr pfsclient;

			// Token: 0x040049E5 RID: 18917
			internal PTS.FSCBKOBJ cbkobj;

			// Token: 0x040049E6 RID: 18918
			internal PTS.FSTABLECBKFETCH tablecbkfetch;

			// Token: 0x040049E7 RID: 18919
			internal PTS.FSTABLECBKCELL tablecbkcell;

			// Token: 0x040049E8 RID: 18920
			internal uint fsffi;
		}

		// Token: 0x02000A8C RID: 2700
		internal enum FSKUPDATE
		{
			// Token: 0x040049EA RID: 18922
			fskupdInherited,
			// Token: 0x040049EB RID: 18923
			fskupdNoChange,
			// Token: 0x040049EC RID: 18924
			fskupdNew,
			// Token: 0x040049ED RID: 18925
			fskupdChangeInside,
			// Token: 0x040049EE RID: 18926
			fskupdShifted
		}

		// Token: 0x02000A8D RID: 2701
		internal struct FSUPDATEINFO
		{
			// Token: 0x040049EF RID: 18927
			public PTS.FSKUPDATE fskupd;

			// Token: 0x040049F0 RID: 18928
			public int dvrShifted;
		}

		// Token: 0x02000A8E RID: 2702
		// (Invoke) Token: 0x06008AF4 RID: 35572
		[SecurityCritical]
		internal delegate void AssertFailed(string arg1, string arg2, int arg3, uint arg4);

		// Token: 0x02000A8F RID: 2703
		// (Invoke) Token: 0x06008AF8 RID: 35576
		[SecurityCritical]
		internal delegate int GetFigureProperties(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, int fInTextLine, uint fswdir, int fBottomUndefined, out int dur, out int dvr, out PTS.FSFIGUREPROPS fsfigprops, out int cPolygons, out int cVertices, out int durDistTextLeft, out int durDistTextRight, out int dvrDistTextTop, out int dvrDistTextBottom);

		// Token: 0x02000A90 RID: 2704
		// (Invoke) Token: 0x06008AFC RID: 35580
		[SecurityCritical]
		internal unsafe delegate int GetFigurePolygons(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, uint fswdir, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough);

		// Token: 0x02000A91 RID: 2705
		// (Invoke) Token: 0x06008B00 RID: 35584
		[SecurityCritical]
		internal delegate int CalcFigurePosition(IntPtr pfsclient, IntPtr pfsparaclientFigure, IntPtr nmpFigure, uint fswdir, ref PTS.FSRECT fsrcPage, ref PTS.FSRECT fsrcMargin, ref PTS.FSRECT fsrcTrack, ref PTS.FSRECT fsrcFigurePreliminary, int fMustPosition, int fInTextLine, out int fPushToNextTrack, out PTS.FSRECT fsrcFlow, out PTS.FSRECT fsrcOverlap, out PTS.FSBBOX fsbbox, out PTS.FSRECT fsrcSearch);

		// Token: 0x02000A92 RID: 2706
		// (Invoke) Token: 0x06008B04 RID: 35588
		[SecurityCritical]
		internal delegate int FSkipPage(IntPtr pfsclient, IntPtr nms, out int fSkip);

		// Token: 0x02000A93 RID: 2707
		// (Invoke) Token: 0x06008B08 RID: 35592
		[SecurityCritical]
		internal delegate int GetPageDimensions(IntPtr pfsclient, IntPtr nms, out uint fswdir, out int fHeaderFooterAtTopBottom, out int durPage, out int dvrPage, ref PTS.FSRECT fsrcMargin);

		// Token: 0x02000A94 RID: 2708
		// (Invoke) Token: 0x06008B0C RID: 35596
		[SecurityCritical]
		internal delegate int GetNextSection(IntPtr pfsclient, IntPtr nmsCur, out int fSuccess, out IntPtr nmsNext);

		// Token: 0x02000A95 RID: 2709
		// (Invoke) Token: 0x06008B10 RID: 35600
		[SecurityCritical]
		internal delegate int GetSectionProperties(IntPtr pfsclient, IntPtr nms, out int fNewPage, out uint fswdir, out int fApplyColumnBalancing, out int ccol, out int cSegmentDefinedColumnSpanAreas, out int cHeightDefinedColumnSpanAreas);

		// Token: 0x02000A96 RID: 2710
		// (Invoke) Token: 0x06008B14 RID: 35604
		[SecurityCritical]
		internal unsafe delegate int GetJustificationProperties(IntPtr pfsclient, IntPtr* rgnms, int cnms, int fLastSectionNotBroken, out int fJustify, out PTS.FSKALIGNPAGE fskal, out int fCancelAtLastColumn);

		// Token: 0x02000A97 RID: 2711
		// (Invoke) Token: 0x06008B18 RID: 35608
		[SecurityCritical]
		internal delegate int GetMainTextSegment(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmSegment);

		// Token: 0x02000A98 RID: 2712
		// (Invoke) Token: 0x06008B1C RID: 35612
		[SecurityCritical]
		internal delegate int GetHeaderSegment(IntPtr pfsclient, IntPtr nms, IntPtr pfsbrpagePrelim, uint fswdir, out int fHeaderPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirHeader, out IntPtr nmsHeader);

		// Token: 0x02000A99 RID: 2713
		// (Invoke) Token: 0x06008B20 RID: 35616
		[SecurityCritical]
		internal delegate int GetFooterSegment(IntPtr pfsclient, IntPtr nms, IntPtr pfsbrpagePrelim, uint fswdir, out int fFooterPresent, out int fHardMargin, out int dvrMaxHeight, out int dvrFromEdge, out uint fswdirFooter, out IntPtr nmsFooter);

		// Token: 0x02000A9A RID: 2714
		// (Invoke) Token: 0x06008B24 RID: 35620
		[SecurityCritical]
		internal delegate int UpdGetSegmentChange(IntPtr pfsclient, IntPtr nms, out PTS.FSKCHANGE fskch);

		// Token: 0x02000A9B RID: 2715
		// (Invoke) Token: 0x06008B28 RID: 35624
		[SecurityCritical]
		internal unsafe delegate int GetSectionColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncol, PTS.FSCOLUMNINFO* fscolinfo, out int ccol);

		// Token: 0x02000A9C RID: 2716
		// (Invoke) Token: 0x06008B2C RID: 35628
		[SecurityCritical]
		internal unsafe delegate int GetSegmentDefinedColumnSpanAreaInfo(IntPtr pfsclient, IntPtr nms, int cAreas, IntPtr* rgnmSeg, int* rgcColumns, out int cAreasActual);

		// Token: 0x02000A9D RID: 2717
		// (Invoke) Token: 0x06008B30 RID: 35632
		[SecurityCritical]
		internal unsafe delegate int GetHeightDefinedColumnSpanAreaInfo(IntPtr pfsclient, IntPtr nms, int cAreas, int* rgdvrAreaHeight, int* rgcColumns, out int cAreasActual);

		// Token: 0x02000A9E RID: 2718
		// (Invoke) Token: 0x06008B34 RID: 35636
		[SecurityCritical]
		internal delegate int GetFirstPara(IntPtr pfsclient, IntPtr nms, out int fSuccessful, out IntPtr nmp);

		// Token: 0x02000A9F RID: 2719
		// (Invoke) Token: 0x06008B38 RID: 35640
		[SecurityCritical]
		internal delegate int GetNextPara(IntPtr pfsclient, IntPtr nms, IntPtr nmpCur, out int fFound, out IntPtr nmpNext);

		// Token: 0x02000AA0 RID: 2720
		// (Invoke) Token: 0x06008B3C RID: 35644
		[SecurityCritical]
		internal delegate int UpdGetFirstChangeInSegment(IntPtr pfsclient, IntPtr nms, out int fFound, out int fChangeFirst, out IntPtr nmpBeforeChange);

		// Token: 0x02000AA1 RID: 2721
		// (Invoke) Token: 0x06008B40 RID: 35648
		[SecurityCritical]
		internal delegate int UpdGetParaChange(IntPtr pfsclient, IntPtr nmp, out PTS.FSKCHANGE fskch, out int fNoFurtherChanges);

		// Token: 0x02000AA2 RID: 2722
		// (Invoke) Token: 0x06008B44 RID: 35652
		[SecurityCritical]
		internal delegate int GetParaProperties(IntPtr pfsclient, IntPtr nmp, ref PTS.FSPAP fspap);

		// Token: 0x02000AA3 RID: 2723
		// (Invoke) Token: 0x06008B48 RID: 35656
		[SecurityCritical]
		internal delegate int CreateParaclient(IntPtr pfsclient, IntPtr nmp, out IntPtr pfsparaclient);

		// Token: 0x02000AA4 RID: 2724
		// (Invoke) Token: 0x06008B4C RID: 35660
		[SecurityCritical]
		internal delegate int TransferDisplayInfo(IntPtr pfsclient, IntPtr pfsparaclientOld, IntPtr pfsparaclientNew);

		// Token: 0x02000AA5 RID: 2725
		// (Invoke) Token: 0x06008B50 RID: 35664
		[SecurityCritical]
		internal delegate int DestroyParaclient(IntPtr pfsclient, IntPtr pfsparaclient);

		// Token: 0x02000AA6 RID: 2726
		// (Invoke) Token: 0x06008B54 RID: 35668
		[SecurityCritical]
		internal delegate int FInterruptFormattingAfterPara(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int vr, out int fInterruptFormatting);

		// Token: 0x02000AA7 RID: 2727
		// (Invoke) Token: 0x06008B58 RID: 35672
		[SecurityCritical]
		internal delegate int GetEndnoteSeparators(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmsEndnoteSeparator, out IntPtr nmEndnoteContSeparator, out IntPtr nmsEndnoteContNotice);

		// Token: 0x02000AA8 RID: 2728
		// (Invoke) Token: 0x06008B5C RID: 35676
		[SecurityCritical]
		internal delegate int GetEndnoteSegment(IntPtr pfsclient, IntPtr nmsSection, out int fEndnotesPresent, out IntPtr nmsEndnotes);

		// Token: 0x02000AA9 RID: 2729
		// (Invoke) Token: 0x06008B60 RID: 35680
		[SecurityCritical]
		internal delegate int GetNumberEndnoteColumns(IntPtr pfsclient, IntPtr nms, out int ccolEndnote);

		// Token: 0x02000AAA RID: 2730
		// (Invoke) Token: 0x06008B64 RID: 35684
		[SecurityCritical]
		internal unsafe delegate int GetEndnoteColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncolEndnote, PTS.FSCOLUMNINFO* fscolinfoEndnote, out int ccolEndnote);

		// Token: 0x02000AAB RID: 2731
		// (Invoke) Token: 0x06008B68 RID: 35688
		[SecurityCritical]
		internal delegate int GetFootnoteSeparators(IntPtr pfsclient, IntPtr nmsSection, out IntPtr nmsFtnSeparator, out IntPtr nmsFtnContSeparator, out IntPtr nmsFtnContNotice);

		// Token: 0x02000AAC RID: 2732
		// (Invoke) Token: 0x06008B6C RID: 35692
		[SecurityCritical]
		internal delegate int FFootnoteBeneathText(IntPtr pfsclient, IntPtr nms, out int fFootnoteBeneathText);

		// Token: 0x02000AAD RID: 2733
		// (Invoke) Token: 0x06008B70 RID: 35696
		[SecurityCritical]
		internal delegate int GetNumberFootnoteColumns(IntPtr pfsclient, IntPtr nms, out int ccolFootnote);

		// Token: 0x02000AAE RID: 2734
		// (Invoke) Token: 0x06008B74 RID: 35700
		[SecurityCritical]
		internal unsafe delegate int GetFootnoteColumnInfo(IntPtr pfsclient, IntPtr nms, uint fswdir, int ncolFootnote, PTS.FSCOLUMNINFO* fscolinfoFootnote, out int ccolFootnote);

		// Token: 0x02000AAF RID: 2735
		// (Invoke) Token: 0x06008B78 RID: 35704
		[SecurityCritical]
		internal delegate int GetFootnoteSegment(IntPtr pfsclient, IntPtr nmftn, out IntPtr nmsFootnote);

		// Token: 0x02000AB0 RID: 2736
		// (Invoke) Token: 0x06008B7C RID: 35708
		[SecurityCritical]
		internal unsafe delegate int GetFootnotePresentationAndRejectionOrder(IntPtr pfsclient, int cFootnotes, IntPtr* rgProposedPresentationOrder, IntPtr* rgProposedRejectionOrder, out int fProposedPresentationOrderAccepted, IntPtr* rgFinalPresentationOrder, out int fProposedRejectionOrderAccepted, IntPtr* rgFinalRejectionOrder);

		// Token: 0x02000AB1 RID: 2737
		// (Invoke) Token: 0x06008B80 RID: 35712
		[SecurityCritical]
		internal delegate int FAllowFootnoteSeparation(IntPtr pfsclient, IntPtr nmftn, out int fAllow);

		// Token: 0x02000AB2 RID: 2738
		// (Invoke) Token: 0x06008B84 RID: 35716
		[SecurityCritical]
		internal delegate int DuplicateMcsclient(IntPtr pfsclient, IntPtr pmcsclientIn, out IntPtr pmcsclientNew);

		// Token: 0x02000AB3 RID: 2739
		// (Invoke) Token: 0x06008B88 RID: 35720
		[SecurityCritical]
		internal delegate int DestroyMcsclient(IntPtr pfsclient, IntPtr pmcsclient);

		// Token: 0x02000AB4 RID: 2740
		// (Invoke) Token: 0x06008B8C RID: 35724
		[SecurityCritical]
		internal delegate int FEqualMcsclient(IntPtr pfsclient, IntPtr pmcsclient1, IntPtr pmcsclient2, out int fEqual);

		// Token: 0x02000AB5 RID: 2741
		// (Invoke) Token: 0x06008B90 RID: 35728
		[SecurityCritical]
		internal delegate int ConvertMcsclient(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, uint fswdir, IntPtr pmcsclient, int fSuppressTopSpace, out int dvr);

		// Token: 0x02000AB6 RID: 2742
		// (Invoke) Token: 0x06008B94 RID: 35732
		[SecurityCritical]
		internal delegate int GetObjectHandlerInfo(IntPtr pfsclient, int idobj, IntPtr pobjectinfo);

		// Token: 0x02000AB7 RID: 2743
		// (Invoke) Token: 0x06008B98 RID: 35736
		[SecurityCritical]
		internal delegate int CreateParaBreakingSession(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int fsdcpStart, IntPtr pfsbreakreclineclient, uint fswdir, int urStartTrack, int durTrack, int urPageLeftMargin, out IntPtr ppfsparabreakingsession, out int fParagraphJustified);

		// Token: 0x02000AB8 RID: 2744
		// (Invoke) Token: 0x06008B9C RID: 35740
		[SecurityCritical]
		internal delegate int DestroyParaBreakingSession(IntPtr pfsclient, IntPtr pfsparabreakingsession);

		// Token: 0x02000AB9 RID: 2745
		// (Invoke) Token: 0x06008BA0 RID: 35744
		[SecurityCritical]
		internal delegate int GetTextProperties(IntPtr pfsclient, IntPtr nmp, int iArea, ref PTS.FSTXTPROPS fstxtprops);

		// Token: 0x02000ABA RID: 2746
		// (Invoke) Token: 0x06008BA4 RID: 35748
		[SecurityCritical]
		internal delegate int GetNumberFootnotes(IntPtr pfsclient, IntPtr nmp, int fsdcpStart, int fsdcpLim, out int nFootnote);

		// Token: 0x02000ABB RID: 2747
		// (Invoke) Token: 0x06008BA8 RID: 35752
		[SecurityCritical]
		internal unsafe delegate int GetFootnotes(IntPtr pfsclient, IntPtr nmp, int fsdcpStart, int fsdcpLim, int nFootnotes, IntPtr* rgnmftn, int* rgdcp, out int cFootnotes);

		// Token: 0x02000ABC RID: 2748
		// (Invoke) Token: 0x06008BAC RID: 35756
		[SecurityCritical]
		internal delegate int FormatDropCap(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int fSuppressTopSpace, out IntPtr pfsdropc, out int fInMargin, out int dur, out int dvr, out int cPolygons, out int cVertices, out int durText);

		// Token: 0x02000ABD RID: 2749
		// (Invoke) Token: 0x06008BB0 RID: 35760
		[SecurityCritical]
		internal unsafe delegate int GetDropCapPolygons(IntPtr pfsclient, IntPtr pfsdropc, IntPtr nmp, uint fswdir, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough);

		// Token: 0x02000ABE RID: 2750
		// (Invoke) Token: 0x06008BB4 RID: 35764
		[SecurityCritical]
		internal delegate int DestroyDropCap(IntPtr pfsclient, IntPtr pfsdropc);

		// Token: 0x02000ABF RID: 2751
		// (Invoke) Token: 0x06008BB8 RID: 35768
		[SecurityCritical]
		internal delegate int FormatBottomText(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, IntPtr pfslineLast, int dvrLine, out IntPtr pmcsclientOut);

		// Token: 0x02000AC0 RID: 2752
		// (Invoke) Token: 0x06008BBC RID: 35772
		[SecurityCritical]
		internal delegate int FormatLine(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, out IntPtr pfsline, out int dcpLine, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine);

		// Token: 0x02000AC1 RID: 2753
		// (Invoke) Token: 0x06008BC0 RID: 35776
		[SecurityCritical]
		internal delegate int FormatLineForced(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, int dvrAvailable, out IntPtr pfsline, out int dcpLine, out IntPtr ppbrlineOut, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend);

		// Token: 0x02000AC2 RID: 2754
		// (Invoke) Token: 0x06008BC4 RID: 35780
		[SecurityCritical]
		internal unsafe delegate int FormatLineVariants(IntPtr pfsclient, IntPtr pfsparabreakingsession, int dcp, IntPtr pbrlineIn, uint fswdir, int urStartLine, int durLine, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, IntPtr lineVariantRestriction, int nLineVariantsAlloc, PTS.FSLINEVARIANT* rgfslinevariant, out int nLineVariantsActual, out int iLineVariantBest);

		// Token: 0x02000AC3 RID: 2755
		// (Invoke) Token: 0x06008BC8 RID: 35784
		[SecurityCritical]
		internal delegate int ReconstructLineVariant(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, int dcpStart, IntPtr pbrlineIn, int dcpLine, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fAllowHyphenation, int fClearOnLeft, int fClearOnRight, int fTreatAsFirstInPara, int fTreatAsLastInPara, int fSuppressTopSpace, out IntPtr pfsline, out IntPtr ppbrlineOut, out int fForcedBroken, out PTS.FSFLRES fsflres, out int dvrAscent, out int dvrDescent, out int urBBox, out int durBBox, out int dcpDepend, out int fReformatNeighborsAsLastLine);

		// Token: 0x02000AC4 RID: 2756
		// (Invoke) Token: 0x06008BCC RID: 35788
		[SecurityCritical]
		internal delegate int DestroyLine(IntPtr pfsclient, IntPtr pfsline);

		// Token: 0x02000AC5 RID: 2757
		// (Invoke) Token: 0x06008BD0 RID: 35792
		[SecurityCritical]
		internal delegate int DuplicateLineBreakRecord(IntPtr pfsclient, IntPtr pbrlineIn, out IntPtr pbrlineDup);

		// Token: 0x02000AC6 RID: 2758
		// (Invoke) Token: 0x06008BD4 RID: 35796
		[SecurityCritical]
		internal delegate int DestroyLineBreakRecord(IntPtr pfsclient, IntPtr pbrlineIn);

		// Token: 0x02000AC7 RID: 2759
		// (Invoke) Token: 0x06008BD8 RID: 35800
		[SecurityCritical]
		internal delegate int SnapGridVertical(IntPtr pfsclient, uint fswdir, int vrMargin, int vrCurrent, out int vrNew);

		// Token: 0x02000AC8 RID: 2760
		// (Invoke) Token: 0x06008BDC RID: 35804
		[SecurityCritical]
		internal delegate int GetDvrSuppressibleBottomSpace(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsline, uint fswdir, out int dvrSuppressible);

		// Token: 0x02000AC9 RID: 2761
		// (Invoke) Token: 0x06008BE0 RID: 35808
		[SecurityCritical]
		internal delegate int GetDvrAdvance(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int dcp, uint fswdir, out int dvr);

		// Token: 0x02000ACA RID: 2762
		// (Invoke) Token: 0x06008BE4 RID: 35812
		[SecurityCritical]
		internal delegate int UpdGetChangeInText(IntPtr pfsclient, IntPtr nmp, out int dcpStart, out int ddcpOld, out int ddcpNew);

		// Token: 0x02000ACB RID: 2763
		// (Invoke) Token: 0x06008BE8 RID: 35816
		[SecurityCritical]
		internal delegate int UpdGetDropCapChange(IntPtr pfsclient, IntPtr nmp, out int fChanged);

		// Token: 0x02000ACC RID: 2764
		// (Invoke) Token: 0x06008BEC RID: 35820
		[SecurityCritical]
		internal delegate int FInterruptFormattingText(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int dcp, int vr, out int fInterruptFormatting);

		// Token: 0x02000ACD RID: 2765
		// (Invoke) Token: 0x06008BF0 RID: 35824
		[SecurityCritical]
		internal delegate int GetTextParaCache(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fSuppressTopSpace, out int fFound, out int dcpPara, out int urBBox, out int durBBox, out int dvrPara, out PTS.FSKCLEAR fskclear, out IntPtr pmcsclientAfterPara, out int cLines, out int fOptimalLines, out int fOptimalLineDcpsCached, out int dvrMinLineHeight);

		// Token: 0x02000ACE RID: 2766
		// (Invoke) Token: 0x06008BF4 RID: 35828
		[SecurityCritical]
		internal unsafe delegate int SetTextParaCache(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmp, int iArea, uint fswdir, int urStartLine, int durLine, int urStartTrack, int durTrack, int urPageLeftMargin, int fClearOnLeft, int fClearOnRight, int fSuppressTopSpace, int dcpPara, int urBBox, int durBBox, int dvrPara, PTS.FSKCLEAR fskclear, IntPtr pmcsclientAfterPara, int cLines, int fOptimalLines, int* rgdcpOptimalLines, int dvrMinLineHeight);

		// Token: 0x02000ACF RID: 2767
		// (Invoke) Token: 0x06008BF8 RID: 35832
		[SecurityCritical]
		internal unsafe delegate int GetOptimalLineDcpCache(IntPtr pfsclient, int cLines, int* rgdcp);

		// Token: 0x02000AD0 RID: 2768
		// (Invoke) Token: 0x06008BFC RID: 35836
		[SecurityCritical]
		internal delegate int GetNumberAttachedObjectsBeforeTextLine(IntPtr pfsclient, IntPtr nmp, int dcpFirst, out int cAttachedObjects);

		// Token: 0x02000AD1 RID: 2769
		// (Invoke) Token: 0x06008C00 RID: 35840
		[SecurityCritical]
		internal unsafe delegate int GetAttachedObjectsBeforeTextLine(IntPtr pfsclient, IntPtr nmp, int dcpFirst, int nAttachedObjects, IntPtr* rgnmpObjects, int* rgidobj, int* rgdcpAnchor, out int cObjects, out int fEndOfParagraph);

		// Token: 0x02000AD2 RID: 2770
		// (Invoke) Token: 0x06008C04 RID: 35844
		[SecurityCritical]
		internal delegate int GetNumberAttachedObjectsInTextLine(IntPtr pfsclient, IntPtr pfsline, IntPtr nmp, int dcpFirst, int dcpLim, int fFoundAttachedObjectsBeforeLine, int dcpMaxAnchorAttachedObjectBeforeLine, out int cAttachedObjects);

		// Token: 0x02000AD3 RID: 2771
		// (Invoke) Token: 0x06008C08 RID: 35848
		[SecurityCritical]
		internal unsafe delegate int GetAttachedObjectsInTextLine(IntPtr pfsclient, IntPtr pfsline, IntPtr nmp, int dcpFirst, int dcpLim, int fFoundAttachedObjectsBeforeLine, int dcpMaxAnchorAttachedObjectBeforeLine, int nAttachedObjects, IntPtr* rgnmpObjects, int* rgidobj, int* rgdcpAnchor, out int cObjects);

		// Token: 0x02000AD4 RID: 2772
		// (Invoke) Token: 0x06008C0C RID: 35852
		[SecurityCritical]
		internal delegate int UpdGetAttachedObjectChange(IntPtr pfsclient, IntPtr nmp, IntPtr nmpAttachedObject, out PTS.FSKCHANGE fskchObject);

		// Token: 0x02000AD5 RID: 2773
		// (Invoke) Token: 0x06008C10 RID: 35856
		[SecurityCritical]
		internal delegate int GetDurFigureAnchor(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsparaclientFigure, IntPtr pfsline, IntPtr nmpFigure, uint fswdir, IntPtr pfsFmtLineIn, out int dur);

		// Token: 0x02000AD6 RID: 2774
		// (Invoke) Token: 0x06008C14 RID: 35860
		[SecurityCritical]
		internal delegate int GetFloaterProperties(IntPtr pfsclient, IntPtr nmFloater, uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops);

		// Token: 0x02000AD7 RID: 2775
		// (Invoke) Token: 0x06008C18 RID: 35864
		[SecurityCritical]
		internal delegate int FormatFloaterContentFinite(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr pfsbrkFloaterContentIn, int fBreakRecordFromPreviousPage, IntPtr nmFloater, IntPtr pftnrej, int fEmptyOk, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsbrkFloatContentOut, out IntPtr pbrkrecpara, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x02000AD8 RID: 2776
		// (Invoke) Token: 0x06008C1C RID: 35868
		[SecurityCritical]
		internal delegate int FormatFloaterContentBottomless(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsbrkFloatContentOut, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x02000AD9 RID: 2777
		// (Invoke) Token: 0x06008C20 RID: 35872
		[SecurityCritical]
		internal delegate int UpdateBottomlessFloaterContent(IntPtr pfsFloaterContent, IntPtr pfsparaclient, IntPtr nmFloater, int fSuppressTopSpace, uint fswdirTrack, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x02000ADA RID: 2778
		// (Invoke) Token: 0x06008C24 RID: 35876
		[SecurityCritical]
		internal unsafe delegate int GetFloaterPolygons(IntPtr pfsparaclient, IntPtr pfsFloaterContent, IntPtr nmFloater, uint fswdirTrack, int ncVertices, int nfspt, int* rgcVertices, out int cVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough);

		// Token: 0x02000ADB RID: 2779
		// (Invoke) Token: 0x06008C28 RID: 35880
		[SecurityCritical]
		internal delegate int ClearUpdateInfoInFloaterContent(IntPtr pfsFloaterContent);

		// Token: 0x02000ADC RID: 2780
		// (Invoke) Token: 0x06008C2C RID: 35884
		[SecurityCritical]
		internal delegate int CompareFloaterContents(IntPtr pfsFloaterContentOld, IntPtr pfsFloaterContentNew, out PTS.FSCOMPRESULT fscmpr);

		// Token: 0x02000ADD RID: 2781
		// (Invoke) Token: 0x06008C30 RID: 35888
		[SecurityCritical]
		internal delegate int DestroyFloaterContent(IntPtr pfsFloaterContent);

		// Token: 0x02000ADE RID: 2782
		// (Invoke) Token: 0x06008C34 RID: 35892
		[SecurityCritical]
		internal delegate int DuplicateFloaterContentBreakRecord(IntPtr pfsclient, IntPtr pfsbrkFloaterContent, out IntPtr pfsbrkFloaterContentDup);

		// Token: 0x02000ADF RID: 2783
		// (Invoke) Token: 0x06008C38 RID: 35896
		[SecurityCritical]
		internal delegate int DestroyFloaterContentBreakRecord(IntPtr pfsclient, IntPtr pfsbrkFloaterContent);

		// Token: 0x02000AE0 RID: 2784
		// (Invoke) Token: 0x06008C3C RID: 35900
		[SecurityCritical]
		internal delegate int GetFloaterContentColumnBalancingInfo(IntPtr pfsFloaterContent, uint fswdir, out int nlines, out int dvrSumHeight, out int dvrMinHeight);

		// Token: 0x02000AE1 RID: 2785
		// (Invoke) Token: 0x06008C40 RID: 35904
		[SecurityCritical]
		internal delegate int GetFloaterContentNumberFootnotes(IntPtr pfsFloaterContent, out int cftn);

		// Token: 0x02000AE2 RID: 2786
		// (Invoke) Token: 0x06008C44 RID: 35908
		[SecurityCritical]
		internal delegate int GetFloaterContentFootnoteInfo(IntPtr pfsFloaterContent, uint fswdir, int nftn, int iftnFirst, ref PTS.FSFTNINFO fsftninf, out int iftnLim);

		// Token: 0x02000AE3 RID: 2787
		// (Invoke) Token: 0x06008C48 RID: 35912
		[SecurityCritical]
		internal delegate int TransferDisplayInfoInFloaterContent(IntPtr pfsFloaterContentOld, IntPtr pfsFloaterContentNew);

		// Token: 0x02000AE4 RID: 2788
		// (Invoke) Token: 0x06008C4C RID: 35916
		[SecurityCritical]
		internal delegate int GetMCSClientAfterFloater(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, uint fswdirTrack, IntPtr pmcsclientIn, out IntPtr pmcsclientOut);

		// Token: 0x02000AE5 RID: 2789
		// (Invoke) Token: 0x06008C50 RID: 35920
		[SecurityCritical]
		internal delegate int GetDvrUsedForFloater(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmFloater, uint fswdirTrack, IntPtr pmcsclientIn, int dvrDisplaced, out int dvrUsed);

		// Token: 0x02000AE6 RID: 2790
		// (Invoke) Token: 0x06008C54 RID: 35924
		[SecurityCritical]
		internal delegate int ObjCreateContext(IntPtr pfsclient, IntPtr pfsc, IntPtr pfscbkobj, uint ffi, int idobj, out IntPtr pfssobjc);

		// Token: 0x02000AE7 RID: 2791
		// (Invoke) Token: 0x06008C58 RID: 35928
		[SecurityCritical]
		internal delegate int ObjDestroyContext(IntPtr pfssobjc);

		// Token: 0x02000AE8 RID: 2792
		// (Invoke) Token: 0x06008C5C RID: 35932
		[SecurityCritical]
		internal delegate int ObjFormatParaFinite(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr pfsobjbrk, int fBreakRecordFromPreviousPage, IntPtr nmp, int iArea, IntPtr pftnrej, IntPtr pfsgeom, int fEmptyOk, int fSuppressTopSpace, uint fswdir, ref PTS.FSRECT fsrcToFill, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, int fBreakInside, out PTS.FSFMTR fsfmtr, out IntPtr pfspara, out IntPtr pbrkrecpara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fBreakInsidePossible);

		// Token: 0x02000AE9 RID: 2793
		// (Invoke) Token: 0x06008C60 RID: 35936
		[SecurityCritical]
		internal delegate int ObjFormatParaBottomless(IntPtr pfssobjc, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfspara, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable);

		// Token: 0x02000AEA RID: 2794
		// (Invoke) Token: 0x06008C64 RID: 35940
		[SecurityCritical]
		internal delegate int ObjUpdateBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr nmp, int iArea, IntPtr pfsgeom, int fSuppressTopSpace, uint fswdir, int urTrack, int durTrack, int vrTrack, IntPtr pmcsclientIn, PTS.FSKCLEAR fskclearIn, int fInterruptable, out PTS.FSFMTRBL fsfmtrbl, out int dvrUsed, out PTS.FSBBOX fsbbox, out IntPtr pmcsclientOut, out PTS.FSKCLEAR fskclearOut, out int dvrTopSpace, out int fPageBecomesUninterruptable);

		// Token: 0x02000AEB RID: 2795
		// (Invoke) Token: 0x06008C68 RID: 35944
		[SecurityCritical]
		internal delegate int ObjSynchronizeBottomlessPara(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsgeom, uint fswdir, int dvrShift);

		// Token: 0x02000AEC RID: 2796
		// (Invoke) Token: 0x06008C6C RID: 35948
		[SecurityCritical]
		internal delegate int ObjComparePara(IntPtr pfsparaclientOld, IntPtr pfsparaOld, IntPtr pfsparaclientNew, IntPtr pfsparaNew, uint fswdir, out PTS.FSCOMPRESULT fscmpr, out int dvrShifted);

		// Token: 0x02000AED RID: 2797
		// (Invoke) Token: 0x06008C70 RID: 35952
		[SecurityCritical]
		internal delegate int ObjClearUpdateInfoInPara(IntPtr pfspara);

		// Token: 0x02000AEE RID: 2798
		// (Invoke) Token: 0x06008C74 RID: 35956
		[SecurityCritical]
		internal delegate int ObjDestroyPara(IntPtr pfspara);

		// Token: 0x02000AEF RID: 2799
		// (Invoke) Token: 0x06008C78 RID: 35960
		[SecurityCritical]
		internal delegate int ObjDuplicateBreakRecord(IntPtr pfssobjc, IntPtr pfsbrkrecparaOrig, out IntPtr pfsbrkrecparaDup);

		// Token: 0x02000AF0 RID: 2800
		// (Invoke) Token: 0x06008C7C RID: 35964
		[SecurityCritical]
		internal delegate int ObjDestroyBreakRecord(IntPtr pfssobjc, IntPtr pfsobjbrk);

		// Token: 0x02000AF1 RID: 2801
		// (Invoke) Token: 0x06008C80 RID: 35968
		[SecurityCritical]
		internal delegate int ObjGetColumnBalancingInfo(IntPtr pfspara, uint fswdir, out int nlines, out int dvrSumHeight, out int dvrMinHeight);

		// Token: 0x02000AF2 RID: 2802
		// (Invoke) Token: 0x06008C84 RID: 35972
		[SecurityCritical]
		internal delegate int ObjGetNumberFootnotes(IntPtr pfspara, out int nftn);

		// Token: 0x02000AF3 RID: 2803
		// (Invoke) Token: 0x06008C88 RID: 35976
		[SecurityCritical]
		internal unsafe delegate int ObjGetFootnoteInfo(IntPtr pfspara, uint fswdir, int nftn, int iftnFirst, PTS.FSFTNINFO* pfsftninf, out int iftnLim);

		// Token: 0x02000AF4 RID: 2804
		// (Invoke) Token: 0x06008C8C RID: 35980
		[SecurityCritical]
		internal delegate int ObjShiftVertical(IntPtr pfspara, IntPtr pfsparaclient, IntPtr pfsshift, uint fswdir, out PTS.FSBBOX fsbbox);

		// Token: 0x02000AF5 RID: 2805
		// (Invoke) Token: 0x06008C90 RID: 35984
		[SecurityCritical]
		internal delegate int ObjTransferDisplayInfoPara(IntPtr pfsparaOld, IntPtr pfsparaNew);

		// Token: 0x02000AF6 RID: 2806
		// (Invoke) Token: 0x06008C94 RID: 35988
		[SecurityCritical]
		internal delegate int GetTableProperties(IntPtr pfsclient, IntPtr nmTable, uint fswdirTrack, out PTS.FSTABLEOBJPROPS fstableobjprops);

		// Token: 0x02000AF7 RID: 2807
		// (Invoke) Token: 0x06008C98 RID: 35992
		[SecurityCritical]
		internal delegate int AutofitTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, int durAvailableSpace, out int durTableWidth);

		// Token: 0x02000AF8 RID: 2808
		// (Invoke) Token: 0x06008C9C RID: 35996
		[SecurityCritical]
		internal delegate int UpdAutofitTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, int durAvailableSpace, out int durTableWidth, out int fNoChangeInCellWidths);

		// Token: 0x02000AF9 RID: 2809
		// (Invoke) Token: 0x06008CA0 RID: 36000
		[SecurityCritical]
		internal delegate int GetMCSClientAfterTable(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmTable, uint fswdirTrack, IntPtr pmcsclientIn, out IntPtr ppmcsclientOut);

		// Token: 0x02000AFA RID: 2810
		// (Invoke) Token: 0x06008CA4 RID: 36004
		[SecurityCritical]
		internal delegate int GetFirstHeaderRow(IntPtr pfsclient, IntPtr nmTable, int fRepeatedHeader, out int fFound, out IntPtr pnmFirstHeaderRow);

		// Token: 0x02000AFB RID: 2811
		// (Invoke) Token: 0x06008CA8 RID: 36008
		internal delegate int GetNextHeaderRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmHeaderRow, int fRepeatedHeader, out int fFound, out IntPtr pnmNextHeaderRow);

		// Token: 0x02000AFC RID: 2812
		// (Invoke) Token: 0x06008CAC RID: 36012
		[SecurityCritical]
		internal delegate int GetFirstFooterRow(IntPtr pfsclient, IntPtr nmTable, int fRepeatedFooter, out int fFound, out IntPtr pnmFirstFooterRow);

		// Token: 0x02000AFD RID: 2813
		// (Invoke) Token: 0x06008CB0 RID: 36016
		[SecurityCritical]
		internal delegate int GetNextFooterRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmFooterRow, int fRepeatedFooter, out int fFound, out IntPtr pnmNextFooterRow);

		// Token: 0x02000AFE RID: 2814
		// (Invoke) Token: 0x06008CB4 RID: 36020
		[SecurityCritical]
		internal delegate int GetFirstRow(IntPtr pfsclient, IntPtr nmTable, out int fFound, out IntPtr pnmFirstRow);

		// Token: 0x02000AFF RID: 2815
		// (Invoke) Token: 0x06008CB8 RID: 36024
		[SecurityCritical]
		internal delegate int GetNextRow(IntPtr pfsclient, IntPtr nmTable, IntPtr nmRow, out int fFound, out IntPtr pnmNextRow);

		// Token: 0x02000B00 RID: 2816
		// (Invoke) Token: 0x06008CBC RID: 36028
		[SecurityCritical]
		internal delegate int UpdFChangeInHeaderFooter(IntPtr pfsclient, IntPtr nmTable, out int fHeaderChanged, out int fFooterChanged, out int fRepeatedHeaderChanged, out int fRepeatedFooterChanged);

		// Token: 0x02000B01 RID: 2817
		// (Invoke) Token: 0x06008CC0 RID: 36032
		[SecurityCritical]
		internal delegate int UpdGetFirstChangeInTable(IntPtr pfsclient, IntPtr nmTable, out int fFound, out int fChangeFirst, out IntPtr pnmRowBeforeChange);

		// Token: 0x02000B02 RID: 2818
		// (Invoke) Token: 0x06008CC4 RID: 36036
		[SecurityCritical]
		internal delegate int UpdGetRowChange(IntPtr pfsclient, IntPtr nmTable, IntPtr nmRow, out PTS.FSKCHANGE fskch, out int fNoFurtherChanges);

		// Token: 0x02000B03 RID: 2819
		// (Invoke) Token: 0x06008CC8 RID: 36040
		[SecurityCritical]
		internal delegate int UpdGetCellChange(IntPtr pfsclient, IntPtr nmRow, IntPtr nmCell, out int fWidthChanged, out PTS.FSKCHANGE fskchCell);

		// Token: 0x02000B04 RID: 2820
		// (Invoke) Token: 0x06008CCC RID: 36044
		[SecurityCritical]
		internal delegate int GetDistributionKind(IntPtr pfsclient, IntPtr nmTable, uint fswdirTable, out PTS.FSKTABLEHEIGHTDISTRIBUTION tabledistr);

		// Token: 0x02000B05 RID: 2821
		// (Invoke) Token: 0x06008CD0 RID: 36048
		[SecurityCritical]
		internal delegate int GetRowProperties(IntPtr pfsclient, IntPtr nmRow, uint fswdirTable, out PTS.FSTABLEROWPROPS rowprops);

		// Token: 0x02000B06 RID: 2822
		// (Invoke) Token: 0x06008CD4 RID: 36052
		[SecurityCritical]
		internal unsafe delegate int GetCells(IntPtr pfsclient, IntPtr nmRow, int cCells, IntPtr* rgnmCell, PTS.FSTABLEKCELLMERGE* rgkcellmerge);

		// Token: 0x02000B07 RID: 2823
		// (Invoke) Token: 0x06008CD8 RID: 36056
		[SecurityCritical]
		internal delegate int FInterruptFormattingTable(IntPtr pfsclient, IntPtr pfsparaclient, IntPtr nmRow, int dvr, out int fInterrupt);

		// Token: 0x02000B08 RID: 2824
		// (Invoke) Token: 0x06008CDC RID: 36060
		[SecurityCritical]
		internal unsafe delegate int CalcHorizontalBBoxOfRow(IntPtr pfsclient, IntPtr nmRow, int cCells, IntPtr* rgnmCell, IntPtr* rgpfscell, out int urBBox, out int durBBox);

		// Token: 0x02000B09 RID: 2825
		// (Invoke) Token: 0x06008CE0 RID: 36064
		[SecurityCritical]
		internal delegate int FormatCellFinite(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr pfsbrkcell, IntPtr nmCell, IntPtr pfsFtnRejector, int fEmptyOK, uint fswdirTable, int dvrExtraHeight, int dvrAvailable, out PTS.FSFMTR pfmtr, out IntPtr ppfscell, out IntPtr pfsbrkcellOut, out int dvrUsed);

		// Token: 0x02000B0A RID: 2826
		// (Invoke) Token: 0x06008CE4 RID: 36068
		[SecurityCritical]
		internal delegate int FormatCellBottomless(IntPtr pfsclient, IntPtr pfsparaclientTable, IntPtr nmCell, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out IntPtr ppfscell, out int dvrUsed);

		// Token: 0x02000B0B RID: 2827
		// (Invoke) Token: 0x06008CE8 RID: 36072
		[SecurityCritical]
		internal delegate int UpdateBottomlessCell(IntPtr pfscell, IntPtr pfsparaclientTable, IntPtr nmCell, uint fswdirTable, out PTS.FSFMTRBL fmtrbl, out int dvrUsed);

		// Token: 0x02000B0C RID: 2828
		// (Invoke) Token: 0x06008CEC RID: 36076
		[SecurityCritical]
		internal delegate int CompareCells(IntPtr pfscellOld, IntPtr pfscellNew, out PTS.FSCOMPRESULT pfscmpr);

		// Token: 0x02000B0D RID: 2829
		// (Invoke) Token: 0x06008CF0 RID: 36080
		[SecurityCritical]
		internal delegate int ClearUpdateInfoInCell(IntPtr pfscell);

		// Token: 0x02000B0E RID: 2830
		// (Invoke) Token: 0x06008CF4 RID: 36084
		[SecurityCritical]
		internal delegate int SetCellHeight(IntPtr pfscell, IntPtr pfsparaclientTable, IntPtr pfsbrkcell, IntPtr nmCell, int fBrokenHere, uint fswdirTable, int dvrActual);

		// Token: 0x02000B0F RID: 2831
		// (Invoke) Token: 0x06008CF8 RID: 36088
		[SecurityCritical]
		internal delegate int DestroyCell(IntPtr pfsCell);

		// Token: 0x02000B10 RID: 2832
		// (Invoke) Token: 0x06008CFC RID: 36092
		[SecurityCritical]
		internal delegate int DuplicateCellBreakRecord(IntPtr pfsclient, IntPtr pfsbrkcell, out IntPtr ppfsbrkcellDup);

		// Token: 0x02000B11 RID: 2833
		// (Invoke) Token: 0x06008D00 RID: 36096
		[SecurityCritical]
		internal delegate int DestroyCellBreakRecord(IntPtr pfsclient, IntPtr pfsbrkcell);

		// Token: 0x02000B12 RID: 2834
		// (Invoke) Token: 0x06008D04 RID: 36100
		[SecurityCritical]
		internal delegate int GetCellNumberFootnotes(IntPtr pfscell, out int cFtn);

		// Token: 0x02000B13 RID: 2835
		// (Invoke) Token: 0x06008D08 RID: 36104
		[SecurityCritical]
		internal delegate int GetCellMinColumnBalancingStep(IntPtr pfscell, uint fswdir, out int pdvrMinStep);

		// Token: 0x02000B14 RID: 2836
		// (Invoke) Token: 0x06008D0C RID: 36108
		[SecurityCritical]
		internal delegate int TransferDisplayInfoCell(IntPtr pfscellOld, IntPtr pfscellNew);
	}
}
