using System;
using System.Security;
using System.Windows.Documents;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsHost
{
	// Token: 0x02000621 RID: 1569
	internal abstract class FloaterBaseParagraph : BaseParagraph
	{
		// Token: 0x060067FC RID: 26620 RVA: 0x001CF9E3 File Offset: 0x001CDBE3
		protected FloaterBaseParagraph(TextElement element, StructuralCache structuralCache) : base(element, structuralCache)
		{
		}

		// Token: 0x060067FD RID: 26621 RVA: 0x001D3B46 File Offset: 0x001D1D46
		public override void Dispose()
		{
			base.Dispose();
			GC.SuppressFinalize(this);
		}

		// Token: 0x060067FE RID: 26622 RVA: 0x001D3B54 File Offset: 0x001D1D54
		internal override void UpdGetParaChange(out PTS.FSKCHANGE fskch, out int fNoFurtherChanges)
		{
			fskch = PTS.FSKCHANGE.fskchNew;
			fNoFurtherChanges = PTS.FromBoolean(this._stopAsking);
		}

		// Token: 0x060067FF RID: 26623 RVA: 0x001D3B66 File Offset: 0x001D1D66
		internal override void GetParaProperties(ref PTS.FSPAP fspap)
		{
			base.GetParaProperties(ref fspap, false);
			fspap.idobj = PtsHost.FloaterParagraphId;
		}

		// Token: 0x06006800 RID: 26624
		internal abstract override void CreateParaclient(out IntPtr paraClientHandle);

		// Token: 0x06006801 RID: 26625
		internal abstract override void CollapseMargin(BaseParaClient paraClient, MarginCollapsingState mcs, uint fswdir, bool suppressTopSpace, out int dvr);

		// Token: 0x06006802 RID: 26626
		internal abstract void GetFloaterProperties(uint fswdirTrack, out PTS.FSFLOATERPROPS fsfloaterprops);

		// Token: 0x06006803 RID: 26627 RVA: 0x001D3B7C File Offset: 0x001D1D7C
		[SecurityCritical]
		internal unsafe virtual void GetFloaterPolygons(FloaterBaseParaClient paraClient, uint fswdirTrack, int ncVertices, int nfspt, int* rgcVertices, out int ccVertices, PTS.FSPOINT* rgfspt, out int cfspt, out int fWrapThrough)
		{
			ccVertices = (cfspt = (fWrapThrough = 0));
		}

		// Token: 0x06006804 RID: 26628
		internal abstract void FormatFloaterContentFinite(FloaterBaseParaClient paraClient, IntPtr pbrkrecIn, int fBRFromPreviousPage, IntPtr footnoteRejector, int fEmptyOk, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, PTS.FSKSUPPRESSHARDBREAKBEFOREFIRSTPARA fsksuppresshardbreakbeforefirstparaIn, out PTS.FSFMTR fsfmtr, out IntPtr pfsFloatContent, out IntPtr pbrkrecOut, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x06006805 RID: 26629
		internal abstract void FormatFloaterContentBottomless(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, out PTS.FSFMTRBL fsfmtrbl, out IntPtr pfsFloatContent, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x06006806 RID: 26630
		internal abstract void UpdateBottomlessFloaterContent(FloaterBaseParaClient paraClient, int fSuppressTopSpace, uint fswdir, int fAtMaxWidth, int durAvailable, int dvrAvailable, IntPtr pfsFloatContent, out PTS.FSFMTRBL fsfmtrbl, out int durFloaterWidth, out int dvrFloaterHeight, out PTS.FSBBOX fsbbox, out int cPolygons, out int cVertices);

		// Token: 0x06006807 RID: 26631
		internal abstract void GetMCSClientAfterFloater(uint fswdirTrack, MarginCollapsingState mcs, out IntPtr pmcsclientOut);

		// Token: 0x06006808 RID: 26632 RVA: 0x001D3B99 File Offset: 0x001D1D99
		internal virtual void GetDvrUsedForFloater(uint fswdirTrack, MarginCollapsingState mcs, int dvrDisplaced, out int dvrUsed)
		{
			dvrUsed = 0;
		}
	}
}
