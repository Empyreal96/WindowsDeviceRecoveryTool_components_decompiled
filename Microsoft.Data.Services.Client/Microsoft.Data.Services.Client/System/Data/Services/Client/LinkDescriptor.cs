using System;
using System.Collections.Generic;
using System.Data.Services.Client.Metadata;
using System.Diagnostics;

namespace System.Data.Services.Client
{
	// Token: 0x02000101 RID: 257
	[DebuggerDisplay("State = {state}")]
	public sealed class LinkDescriptor : Descriptor
	{
		// Token: 0x06000855 RID: 2133 RVA: 0x00023183 File Offset: 0x00021383
		internal LinkDescriptor(object source, string sourceProperty, object target, ClientEdmModel model) : this(source, sourceProperty, target, EntityStates.Unchanged)
		{
			this.IsSourcePropertyCollection = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(source.GetType())).GetProperty(sourceProperty, false).IsEntityCollection;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x000231B5 File Offset: 0x000213B5
		internal LinkDescriptor(object source, string sourceProperty, object target, EntityStates state) : base(state)
		{
			this.source = source;
			this.sourceProperty = sourceProperty;
			this.target = target;
		}

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x000231D4 File Offset: 0x000213D4
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x000231DC File Offset: 0x000213DC
		public object Target
		{
			get
			{
				return this.target;
			}
			internal set
			{
				this.target = value;
			}
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x000231E5 File Offset: 0x000213E5
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x000231ED File Offset: 0x000213ED
		public object Source
		{
			get
			{
				return this.source;
			}
			internal set
			{
				this.source = value;
			}
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600085B RID: 2139 RVA: 0x000231F6 File Offset: 0x000213F6
		// (set) Token: 0x0600085C RID: 2140 RVA: 0x000231FE File Offset: 0x000213FE
		public string SourceProperty
		{
			get
			{
				return this.sourceProperty;
			}
			internal set
			{
				this.sourceProperty = value;
			}
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00023207 File Offset: 0x00021407
		internal override DescriptorKind DescriptorKind
		{
			get
			{
				return DescriptorKind.Link;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x0002320A File Offset: 0x0002140A
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x00023212 File Offset: 0x00021412
		internal bool IsSourcePropertyCollection { get; set; }

		// Token: 0x06000860 RID: 2144 RVA: 0x0002321B File Offset: 0x0002141B
		internal override void ClearChanges()
		{
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0002321D File Offset: 0x0002141D
		internal bool IsEquivalent(object src, string srcPropName, object targ)
		{
			return this.source == src && this.target == targ && this.sourceProperty == srcPropName;
		}

		// Token: 0x040004F3 RID: 1267
		internal static readonly IEqualityComparer<LinkDescriptor> EquivalenceComparer = new LinkDescriptor.Equivalent();

		// Token: 0x040004F4 RID: 1268
		private object source;

		// Token: 0x040004F5 RID: 1269
		private string sourceProperty;

		// Token: 0x040004F6 RID: 1270
		private object target;

		// Token: 0x02000102 RID: 258
		private sealed class Equivalent : IEqualityComparer<LinkDescriptor>
		{
			// Token: 0x06000863 RID: 2147 RVA: 0x0002324B File Offset: 0x0002144B
			public bool Equals(LinkDescriptor x, LinkDescriptor y)
			{
				return x != null && y != null && x.IsEquivalent(y.source, y.sourceProperty, y.target);
			}

			// Token: 0x06000864 RID: 2148 RVA: 0x0002326D File Offset: 0x0002146D
			public int GetHashCode(LinkDescriptor obj)
			{
				if (obj == null)
				{
					return 0;
				}
				return obj.Source.GetHashCode() ^ ((obj.Target != null) ? obj.Target.GetHashCode() : 0) ^ obj.SourceProperty.GetHashCode();
			}
		}
	}
}
