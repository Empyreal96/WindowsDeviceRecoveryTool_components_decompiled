using System;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Library.Internal
{
	// Token: 0x020000E4 RID: 228
	internal class AmbiguousValueTermBinding : AmbiguousBinding<IEdmValueTerm>, IEdmValueTerm, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x0000C1BF File Offset: 0x0000A3BF
		public AmbiguousValueTermBinding(IEdmValueTerm first, IEdmValueTerm second) : base(first, second)
		{
			this.first = first;
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x0000C1DB File Offset: 0x0000A3DB
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.ValueTerm;
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0000C1DE File Offset: 0x0000A3DE
		public string Namespace
		{
			get
			{
				return this.first.Namespace ?? string.Empty;
			}
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0000C1F4 File Offset: 0x0000A3F4
		public IEdmTypeReference Type
		{
			get
			{
				return this.type.GetValue(this, AmbiguousValueTermBinding.ComputeTypeFunc, null);
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0000C208 File Offset: 0x0000A408
		public EdmTermKind TermKind
		{
			get
			{
				return EdmTermKind.Value;
			}
		}

		// Token: 0x0600049A RID: 1178 RVA: 0x0000C20B File Offset: 0x0000A40B
		private IEdmTypeReference ComputeType()
		{
			return new BadTypeReference(new BadType(base.Errors), true);
		}

		// Token: 0x040001B6 RID: 438
		private readonly IEdmValueTerm first;

		// Token: 0x040001B7 RID: 439
		private readonly Cache<AmbiguousValueTermBinding, IEdmTypeReference> type = new Cache<AmbiguousValueTermBinding, IEdmTypeReference>();

		// Token: 0x040001B8 RID: 440
		private static readonly Func<AmbiguousValueTermBinding, IEdmTypeReference> ComputeTypeFunc = (AmbiguousValueTermBinding me) => me.ComputeType();
	}
}
