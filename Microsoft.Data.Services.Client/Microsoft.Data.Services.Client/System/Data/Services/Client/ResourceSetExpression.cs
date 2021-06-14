using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Data.Services.Client
{
	// Token: 0x020000D7 RID: 215
	[DebuggerDisplay("ResourceSetExpression {Source}.{MemberExpression}")]
	internal class ResourceSetExpression : ResourceExpression
	{
		// Token: 0x060006DA RID: 1754 RVA: 0x0001C6CC File Offset: 0x0001A8CC
		internal ResourceSetExpression(Type type, Expression source, Expression memberExpression, Type resourceType, List<string> expandPaths, CountOption countOption, Dictionary<ConstantExpression, ConstantExpression> customQueryOptions, ProjectionQueryOptionExpression projection, Type resourceTypeAs, Version uriVersion) : base(source, type, expandPaths, countOption, customQueryOptions, projection, resourceTypeAs, uriVersion)
		{
			this.member = memberExpression;
			this.resourceType = resourceType;
			this.sequenceQueryOptions = new List<QueryOptionExpression>();
			this.keyPredicateConjuncts = new List<Expression>();
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060006DB RID: 1755 RVA: 0x0001C712 File Offset: 0x0001A912
		public override ExpressionType NodeType
		{
			get
			{
				if (this.source == null)
				{
					return (ExpressionType)10000;
				}
				return (ExpressionType)10001;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0001C727 File Offset: 0x0001A927
		internal Expression MemberExpression
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x0001C72F File Offset: 0x0001A92F
		internal override Type ResourceType
		{
			get
			{
				return this.resourceType;
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0001C737 File Offset: 0x0001A937
		internal bool HasTransparentScope
		{
			get
			{
				return this.transparentScope != null;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x0001C745 File Offset: 0x0001A945
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x0001C74D File Offset: 0x0001A94D
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

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060006E1 RID: 1761 RVA: 0x0001C756 File Offset: 0x0001A956
		internal bool HasKeyPredicate
		{
			get
			{
				return this.keyPredicateConjuncts.Count > 0;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0001C766 File Offset: 0x0001A966
		internal ReadOnlyCollection<Expression> KeyPredicateConjuncts
		{
			get
			{
				return new ReadOnlyCollection<Expression>(this.keyPredicateConjuncts);
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x0001C773 File Offset: 0x0001A973
		internal override bool IsSingleton
		{
			get
			{
				return this.keyPredicateConjuncts.Count > 0;
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0001C784 File Offset: 0x0001A984
		internal override bool HasQueryOptions
		{
			get
			{
				return this.sequenceQueryOptions.Count > 0 || this.ExpandPaths.Count > 0 || this.CountOption == CountOption.InlineAll || this.CustomQueryOptions.Count > 0 || base.Projection != null;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060006E5 RID: 1765 RVA: 0x0001C7D2 File Offset: 0x0001A9D2
		// (set) Token: 0x060006E6 RID: 1766 RVA: 0x0001C7DA File Offset: 0x0001A9DA
		internal bool ContainsNonKeyPredicate { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0001C7E3 File Offset: 0x0001A9E3
		internal FilterQueryOptionExpression Filter
		{
			get
			{
				return this.sequenceQueryOptions.OfType<FilterQueryOptionExpression>().SingleOrDefault<FilterQueryOptionExpression>();
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0001C7F5 File Offset: 0x0001A9F5
		internal OrderByQueryOptionExpression OrderBy
		{
			get
			{
				return this.sequenceQueryOptions.OfType<OrderByQueryOptionExpression>().SingleOrDefault<OrderByQueryOptionExpression>();
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0001C807 File Offset: 0x0001AA07
		internal SkipQueryOptionExpression Skip
		{
			get
			{
				return this.sequenceQueryOptions.OfType<SkipQueryOptionExpression>().SingleOrDefault<SkipQueryOptionExpression>();
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0001C819 File Offset: 0x0001AA19
		internal TakeQueryOptionExpression Take
		{
			get
			{
				return this.sequenceQueryOptions.OfType<TakeQueryOptionExpression>().SingleOrDefault<TakeQueryOptionExpression>();
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0001C82B File Offset: 0x0001AA2B
		internal IEnumerable<QueryOptionExpression> SequenceQueryOptions
		{
			get
			{
				return this.sequenceQueryOptions.ToList<QueryOptionExpression>();
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0001C838 File Offset: 0x0001AA38
		internal bool HasSequenceQueryOptions
		{
			get
			{
				return this.sequenceQueryOptions.Count > 0;
			}
		}

		// Token: 0x060006ED RID: 1773 RVA: 0x0001C848 File Offset: 0x0001AA48
		internal override ResourceExpression CreateCloneWithNewType(Type type)
		{
			return this.CreateCloneWithNewTypes(type, TypeSystem.GetElementType(type));
		}

		// Token: 0x060006EE RID: 1774 RVA: 0x0001C858 File Offset: 0x0001AA58
		internal ResourceSetExpression CreateCloneForTransparentScope(Type type)
		{
			Type elementType = TypeSystem.GetElementType(type);
			Type newType = typeof(IOrderedQueryable<>).MakeGenericType(new Type[]
			{
				elementType
			});
			return this.CreateCloneWithNewTypes(newType, this.ResourceType);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001C895 File Offset: 0x0001AA95
		internal void ConvertKeyToFilterExpression()
		{
			if (this.keyPredicateConjuncts.Count > 0)
			{
				this.AddFilter(this.keyPredicateConjuncts);
			}
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001C8B4 File Offset: 0x0001AAB4
		internal void AddFilter(IEnumerable<Expression> predicateConjuncts)
		{
			if (this.Skip != null)
			{
				throw new NotSupportedException(Strings.ALinq_QueryOptionOutOfOrder("filter", "skip"));
			}
			if (this.Take != null)
			{
				throw new NotSupportedException(Strings.ALinq_QueryOptionOutOfOrder("filter", "top"));
			}
			if (base.Projection != null)
			{
				throw new NotSupportedException(Strings.ALinq_QueryOptionOutOfOrder("filter", "select"));
			}
			if (this.Filter == null)
			{
				this.AddSequenceQueryOption(new FilterQueryOptionExpression(this.Type));
			}
			this.Filter.AddPredicateConjuncts(predicateConjuncts);
			this.keyPredicateConjuncts.Clear();
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001C968 File Offset: 0x0001AB68
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

		// Token: 0x060006F2 RID: 1778 RVA: 0x0001C9D2 File Offset: 0x0001ABD2
		internal void RemoveFilterExpression()
		{
			if (this.Filter != null)
			{
				this.sequenceQueryOptions.Remove(this.Filter);
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0001C9F0 File Offset: 0x0001ABF0
		internal void OverrideInputReference(ResourceSetExpression newInput)
		{
			InputReferenceExpression inputRef = newInput.inputRef;
			if (inputRef != null)
			{
				this.inputRef = inputRef;
				inputRef.OverrideTarget(this);
			}
		}

		// Token: 0x060006F4 RID: 1780 RVA: 0x0001CA18 File Offset: 0x0001AC18
		internal void SetKeyPredicate(IEnumerable<Expression> keyValues)
		{
			this.keyPredicateConjuncts.Clear();
			foreach (Expression item in keyValues)
			{
				this.keyPredicateConjuncts.Add(item);
			}
		}

		// Token: 0x060006F5 RID: 1781 RVA: 0x0001CA70 File Offset: 0x0001AC70
		internal Dictionary<PropertyInfo, ConstantExpression> GetKeyProperties()
		{
			Dictionary<PropertyInfo, ConstantExpression> dictionary = new Dictionary<PropertyInfo, ConstantExpression>(EqualityComparer<PropertyInfo>.Default);
			if (this.keyPredicateConjuncts.Count > 0)
			{
				foreach (Expression e in this.keyPredicateConjuncts)
				{
					PropertyInfo key;
					ConstantExpression value;
					if (ResourceBinder.PatternRules.MatchKeyComparison(e, out key, out value))
					{
						dictionary.Add(key, value);
					}
				}
			}
			return dictionary;
		}

		// Token: 0x060006F6 RID: 1782 RVA: 0x0001CB00 File Offset: 0x0001AD00
		private ResourceSetExpression CreateCloneWithNewTypes(Type newType, Type newResourceType)
		{
			ResourceSetExpression resourceSetExpression = new ResourceSetExpression(newType, this.source, this.MemberExpression, newResourceType, this.ExpandPaths.ToList<string>(), this.CountOption, this.CustomQueryOptions.ToDictionary((KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Key, (KeyValuePair<ConstantExpression, ConstantExpression> kvp) => kvp.Value), base.Projection, base.ResourceTypeAs, base.UriVersion);
			if (this.keyPredicateConjuncts != null && this.keyPredicateConjuncts.Count > 0)
			{
				resourceSetExpression.SetKeyPredicate(this.keyPredicateConjuncts);
			}
			resourceSetExpression.keyFilter = this.keyFilter;
			resourceSetExpression.sequenceQueryOptions = this.sequenceQueryOptions;
			resourceSetExpression.transparentScope = this.transparentScope;
			return resourceSetExpression;
		}

		// Token: 0x04000436 RID: 1078
		private readonly Type resourceType;

		// Token: 0x04000437 RID: 1079
		private readonly Expression member;

		// Token: 0x04000438 RID: 1080
		private Dictionary<PropertyInfo, ConstantExpression> keyFilter;

		// Token: 0x04000439 RID: 1081
		private List<QueryOptionExpression> sequenceQueryOptions;

		// Token: 0x0400043A RID: 1082
		private ResourceSetExpression.TransparentAccessors transparentScope;

		// Token: 0x0400043B RID: 1083
		private readonly List<Expression> keyPredicateConjuncts;

		// Token: 0x020000D8 RID: 216
		[DebuggerDisplay("{ToString()}")]
		internal class TransparentAccessors
		{
			// Token: 0x060006F9 RID: 1785 RVA: 0x0001CBD0 File Offset: 0x0001ADD0
			internal TransparentAccessors(string acc, Dictionary<string, Expression> sourceAccesors)
			{
				this.Accessor = acc;
				this.SourceAccessors = sourceAccesors;
			}

			// Token: 0x060006FA RID: 1786 RVA: 0x0001CBE8 File Offset: 0x0001ADE8
			public override string ToString()
			{
				string str = "SourceAccessors=[" + string.Join(",", this.SourceAccessors.Keys.ToArray<string>());
				return str + "] ->* Accessor=" + this.Accessor;
			}

			// Token: 0x0400043F RID: 1087
			internal readonly string Accessor;

			// Token: 0x04000440 RID: 1088
			internal readonly Dictionary<string, Expression> SourceAccessors;
		}
	}
}
