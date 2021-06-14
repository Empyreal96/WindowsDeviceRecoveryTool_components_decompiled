using System;
using System.Collections.Generic;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x0200006D RID: 109
	internal sealed class ODataEntriesEntityMaterializer : ODataEntityMaterializer
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x0000FE89 File Offset: 0x0000E089
		public ODataEntriesEntityMaterializer(IEnumerable<ODataEntry> entries, IODataMaterializerContext materializerContext, EntityTrackingAdapter entityTrackingAdapter, QueryComponents queryComponents, Type expectedType, ProjectionPlan materializeEntryPlan, ODataFormat format) : base(materializerContext, entityTrackingAdapter, queryComponents, expectedType, materializeEntryPlan)
		{
			this.format = format;
			this.feedEntries = entries.GetEnumerator();
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0000FEAD File Offset: 0x0000E0AD
		internal override ODataFeed CurrentFeed
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0000FEB0 File Offset: 0x0000E0B0
		internal override ODataEntry CurrentEntry
		{
			get
			{
				base.VerifyNotDisposed();
				return this.feedEntries.Current;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0000FEC3 File Offset: 0x0000E0C3
		internal override long CountValue
		{
			get
			{
				throw new InvalidOperationException(Strings.MaterializeFromAtom_CountNotPresent);
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x0000FECF File Offset: 0x0000E0CF
		internal override bool IsCountable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x0000FED2 File Offset: 0x0000E0D2
		internal override bool IsEndOfStream
		{
			get
			{
				return this.isFinished;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x0000FEDA File Offset: 0x0000E0DA
		protected override bool IsDisposed
		{
			get
			{
				return this.feedEntries == null;
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x0000FEE5 File Offset: 0x0000E0E5
		protected override ODataFormat Format
		{
			get
			{
				return this.format;
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x0000FEED File Offset: 0x0000E0ED
		protected override bool ReadNextFeedOrEntry()
		{
			if (!this.isFinished && !this.feedEntries.MoveNext())
			{
				this.isFinished = true;
			}
			return !this.isFinished;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x0000FF14 File Offset: 0x0000E114
		protected override void OnDispose()
		{
			if (this.feedEntries != null)
			{
				this.feedEntries.Dispose();
				this.feedEntries = null;
			}
		}

		// Token: 0x040002B0 RID: 688
		private readonly ODataFormat format;

		// Token: 0x040002B1 RID: 689
		private IEnumerator<ODataEntry> feedEntries;

		// Token: 0x040002B2 RID: 690
		private bool isFinished;
	}
}
