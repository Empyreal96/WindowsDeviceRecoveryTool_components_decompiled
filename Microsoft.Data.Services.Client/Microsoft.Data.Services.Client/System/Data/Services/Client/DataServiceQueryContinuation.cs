using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace System.Data.Services.Client
{
	// Token: 0x0200005A RID: 90
	[DebuggerDisplay("{NextLinkUri}")]
	public abstract class DataServiceQueryContinuation
	{
		// Token: 0x06000304 RID: 772 RVA: 0x0000DE13 File Offset: 0x0000C013
		internal DataServiceQueryContinuation(Uri nextLinkUri, ProjectionPlan plan)
		{
			this.nextLinkUri = nextLinkUri;
			this.plan = plan;
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0000DE29 File Offset: 0x0000C029
		// (set) Token: 0x06000306 RID: 774 RVA: 0x0000DE31 File Offset: 0x0000C031
		public Uri NextLinkUri
		{
			get
			{
				return this.nextLinkUri;
			}
			internal set
			{
				this.nextLinkUri = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000307 RID: 775
		internal abstract Type ElementType { get; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000308 RID: 776 RVA: 0x0000DE3A File Offset: 0x0000C03A
		// (set) Token: 0x06000309 RID: 777 RVA: 0x0000DE42 File Offset: 0x0000C042
		internal ProjectionPlan Plan
		{
			get
			{
				return this.plan;
			}
			set
			{
				this.plan = value;
			}
		}

		// Token: 0x0600030A RID: 778 RVA: 0x0000DE4B File Offset: 0x0000C04B
		public override string ToString()
		{
			return this.NextLinkUri.ToString();
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000DE58 File Offset: 0x0000C058
		internal static DataServiceQueryContinuation Create(Uri nextLinkUri, ProjectionPlan plan)
		{
			if (nextLinkUri == null)
			{
				return null;
			}
			IEnumerable<ConstructorInfo> instanceConstructors = typeof(DataServiceQueryContinuation<>).MakeGenericType(new Type[]
			{
				plan.ProjectedType
			}).GetInstanceConstructors(false);
			object obj = Util.ConstructorInvoke(instanceConstructors.Single<ConstructorInfo>(), new object[]
			{
				nextLinkUri,
				plan
			});
			return (DataServiceQueryContinuation)obj;
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000DEBC File Offset: 0x0000C0BC
		internal QueryComponents CreateQueryComponents()
		{
			return new QueryComponents(this.NextLinkUri, Util.DataServiceVersionEmpty, this.Plan.LastSegmentType, null, null);
		}

		// Token: 0x04000278 RID: 632
		private Uri nextLinkUri;

		// Token: 0x04000279 RID: 633
		private ProjectionPlan plan;
	}
}
