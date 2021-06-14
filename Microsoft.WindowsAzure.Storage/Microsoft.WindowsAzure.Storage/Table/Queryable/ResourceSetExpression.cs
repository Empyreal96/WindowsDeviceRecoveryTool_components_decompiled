using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000132 RID: 306
	[DebuggerDisplay("ResourceSetExpression {Source}.{MemberExpression}")]
	internal class ResourceSetExpression : ResourceExpression
	{
		// Token: 0x0600140C RID: 5132 RVA: 0x0004CAE7 File Offset: 0x0004ACE7
		internal ResourceSetExpression(Type type, Expression source, Expression memberExpression, Type resourceType, List<string> expandPaths, CountOption countOption, Dictionary<ConstantExpression, ConstantExpression> customQueryOptions, ProjectionQueryOptionExpression projection) : base(source, (source != null) ? ((ExpressionType)10001) : ((ExpressionType)10000), type, expandPaths, countOption, customQueryOptions, projection)
		{
			this.member = memberExpression;
			this.resourceType = resourceType;
			this.sequenceQueryOptions = new List<QueryOptionExpression>();
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600140D RID: 5133 RVA: 0x0004CB22 File Offset: 0x0004AD22
		internal Expression MemberExpression
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x0600140E RID: 5134 RVA: 0x0004CB2A File Offset: 0x0004AD2A
		internal override Type ResourceType
		{
			get
			{
				return this.resourceType;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x0004CB32 File Offset: 0x0004AD32
		internal bool HasTransparentScope
		{
			get
			{
				return this.transparentScope != null;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x0004CB40 File Offset: 0x0004AD40
		// (set) Token: 0x06001411 RID: 5137 RVA: 0x0004CB48 File Offset: 0x0004AD48
		internal ResourceSetExpression.TransparentAccessors TransparentScope
		{
			get
			{
				return this.transparentScope;
			}
			set
			{
				this.transparentScope = value;
			}
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x0004CB51 File Offset: 0x0004AD51
		internal bool HasKeyPredicate
		{
			get
			{
				return this.keyFilter != null;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x0004CB5F File Offset: 0x0004AD5F
		// (set) Token: 0x06001414 RID: 5140 RVA: 0x0004CB67 File Offset: 0x0004AD67
		internal Dictionary<PropertyInfo, ConstantExpression> KeyPredicate
		{
			get
			{
				return this.keyFilter;
			}
			set
			{
				this.keyFilter = value;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x0004CB70 File Offset: 0x0004AD70
		internal override bool IsSingleton
		{
			get
			{
				return this.HasKeyPredicate;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x0004CB78 File Offset: 0x0004AD78
		internal override bool HasQueryOptions
		{
			get
			{
				return this.sequenceQueryOptions.Count > 0 || this.ExpandPaths.Count > 0 || this.CountOption == CountOption.InlineAll || this.CustomQueryOptions.Count > 0 || base.Projection != null;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x0004CBC6 File Offset: 0x0004ADC6
		internal FilterQueryOptionExpression Filter
		{
			get
			{
				return this.sequenceQueryOptions.OfType<FilterQueryOptionExpression>().SingleOrDefault<FilterQueryOptionExpression>();
			}
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001418 RID: 5144 RVA: 0x0004CBD8 File Offset: 0x0004ADD8
		internal RequestOptionsQueryOptionExpression RequestOptions
		{
			get
			{
				return this.sequenceQueryOptions.OfType<RequestOptionsQueryOptionExpression>().SingleOrDefault<RequestOptionsQueryOptionExpression>();
			}
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06001419 RID: 5145 RVA: 0x0004CBEA File Offset: 0x0004ADEA
		internal TakeQueryOptionExpression Take
		{
			get
			{
				return this.sequenceQueryOptions.OfType<TakeQueryOptionExpression>().SingleOrDefault<TakeQueryOptionExpression>();
			}
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x0600141A RID: 5146 RVA: 0x0004CBFC File Offset: 0x0004ADFC
		internal IEnumerable<QueryOptionExpression> SequenceQueryOptions
		{
			get
			{
				return this.sequenceQueryOptions.ToList<QueryOptionExpression>();
			}
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x0600141B RID: 5147 RVA: 0x0004CC09 File Offset: 0x0004AE09
		internal bool HasSequenceQueryOptions
		{
			get
			{
				return this.sequenceQueryOptions.Count > 0;
			}
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0004CC2C File Offset: 0x0004AE2C
		internal override ResourceExpression CreateCloneWithNewType(Type type)
		{
			return new ResourceSetExpression(type, base.Source, this.MemberExpression, TypeSystem.GetElementType(type), this.ExpandPaths.ToList<string>(), this.CountOption, this.CustomQueryOptions.ToDictionary((KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Key, (KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Value), base.Projection)
			{
				keyFilter = this.keyFilter,
				sequenceQueryOptions = this.sequenceQueryOptions,
				transparentScope = this.transparentScope
			};
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0004CCF4 File Offset: 0x0004AEF4
		internal void AddSequenceQueryOption(QueryOptionExpression qoe)
		{
			QueryOptionExpression queryOptionExpression = (from o in this.sequenceQueryOptions
			where o.GetType() == qoe.GetType()
			select o).FirstOrDefault<QueryOptionExpression>();
			if (queryOptionExpression != null)
			{
				qoe = qoe.ComposeMultipleSpecification(queryOptionExpression);
				this.sequenceQueryOptions.Remove(queryOptionExpression);
			}
			this.sequenceQueryOptions.Add(qoe);
		}

		// Token: 0x0600141E RID: 5150 RVA: 0x0004CD60 File Offset: 0x0004AF60
		internal void OverrideInputReference(ResourceSetExpression newInput)
		{
			InputReferenceExpression inputRef = newInput.inputRef;
			if (inputRef != null)
			{
				this.inputRef = inputRef;
				inputRef.OverrideTarget(this);
			}
		}

		// Token: 0x040006A0 RID: 1696
		private readonly Type resourceType;

		// Token: 0x040006A1 RID: 1697
		private readonly Expression member;

		// Token: 0x040006A2 RID: 1698
		private Dictionary<PropertyInfo, ConstantExpression> keyFilter;

		// Token: 0x040006A3 RID: 1699
		private List<QueryOptionExpression> sequenceQueryOptions;

		// Token: 0x040006A4 RID: 1700
		private ResourceSetExpression.TransparentAccessors transparentScope;

		// Token: 0x02000133 RID: 307
		[DebuggerDisplay("{ToString()}")]
		internal class TransparentAccessors
		{
			// Token: 0x06001421 RID: 5153 RVA: 0x0004CD85 File Offset: 0x0004AF85
			internal TransparentAccessors(string acc, Dictionary<string, Expression> sourceAccesors)
			{
				this.Accessor = acc;
				this.SourceAccessors = sourceAccesors;
			}

			// Token: 0x06001422 RID: 5154 RVA: 0x0004CD9C File Offset: 0x0004AF9C
			public override string ToString()
			{
				string str = "SourceAccessors=[" + string.Join(",", this.SourceAccessors.Keys.ToArray<string>());
				return str + "] ->* Accessor=" + this.Accessor;
			}

			// Token: 0x040006A7 RID: 1703
			internal readonly string Accessor;

			// Token: 0x040006A8 RID: 1704
			internal readonly Dictionary<string, Expression> SourceAccessors;
		}
	}
}
