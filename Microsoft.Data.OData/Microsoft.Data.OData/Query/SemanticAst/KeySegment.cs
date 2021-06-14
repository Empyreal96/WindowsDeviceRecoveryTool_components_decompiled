using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Query.SemanticAst
{
	// Token: 0x0200006D RID: 109
	public sealed class KeySegment : ODataPathSegment
	{
		// Token: 0x0600029A RID: 666 RVA: 0x0000A3E1 File Offset: 0x000085E1
		public KeySegment(IEnumerable<KeyValuePair<string, object>> keys, IEdmEntityType edmType, IEdmEntitySet entitySet)
		{
			this.keys = new ReadOnlyCollection<KeyValuePair<string, object>>(keys.ToList<KeyValuePair<string, object>>());
			this.edmType = edmType;
			this.entitySet = entitySet;
			if (entitySet != null)
			{
				UriParserErrorHelper.ThrowIfTypesUnrelated(edmType, entitySet.ElementType, "KeySegments");
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000A41C File Offset: 0x0000861C
		public IEnumerable<KeyValuePair<string, object>> Keys
		{
			get
			{
				return this.keys.AsEnumerable<KeyValuePair<string, object>>();
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000A429 File Offset: 0x00008629
		public override IEdmType EdmType
		{
			get
			{
				return this.edmType;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600029D RID: 669 RVA: 0x0000A431 File Offset: 0x00008631
		public IEdmEntitySet EntitySet
		{
			get
			{
				return this.entitySet;
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000A439 File Offset: 0x00008639
		public override T Translate<T>(PathSegmentTranslator<T> translator)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentTranslator<T>>(translator, "translator");
			return translator.Translate(this);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000A44D File Offset: 0x0000864D
		public override void Handle(PathSegmentHandler handler)
		{
			ExceptionUtils.CheckArgumentNotNull<PathSegmentHandler>(handler, "translator");
			handler.Handle(this);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000A464 File Offset: 0x00008664
		internal override bool Equals(ODataPathSegment other)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataPathSegment>(other, "other");
			KeySegment keySegment = other as KeySegment;
			return keySegment != null && keySegment.Keys.SequenceEqual(this.Keys) && keySegment.EdmType == this.edmType && keySegment.EntitySet == this.entitySet;
		}

		// Token: 0x040000B5 RID: 181
		private readonly ReadOnlyCollection<KeyValuePair<string, object>> keys;

		// Token: 0x040000B6 RID: 182
		private readonly IEdmEntityType edmType;

		// Token: 0x040000B7 RID: 183
		private readonly IEdmEntitySet entitySet;
	}
}
