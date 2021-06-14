using System;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001DA RID: 474
	public class EdmStructuralProperty : EdmProperty, IEdmStructuralProperty, IEdmProperty, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000B47 RID: 2887 RVA: 0x00020CB8 File Offset: 0x0001EEB8
		public EdmStructuralProperty(IEdmStructuredType declaringType, string name, IEdmTypeReference type) : this(declaringType, name, type, null, EdmConcurrencyMode.None)
		{
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00020CC5 File Offset: 0x0001EEC5
		public EdmStructuralProperty(IEdmStructuredType declaringType, string name, IEdmTypeReference type, string defaultValueString, EdmConcurrencyMode concurrencyMode) : base(declaringType, name, type)
		{
			this.defaultValueString = defaultValueString;
			this.concurrencyMode = concurrencyMode;
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00020CE0 File Offset: 0x0001EEE0
		public string DefaultValueString
		{
			get
			{
				return this.defaultValueString;
			}
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00020CE8 File Offset: 0x0001EEE8
		public EdmConcurrencyMode ConcurrencyMode
		{
			get
			{
				return this.concurrencyMode;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00020CF0 File Offset: 0x0001EEF0
		public override EdmPropertyKind PropertyKind
		{
			get
			{
				return EdmPropertyKind.Structural;
			}
		}

		// Token: 0x0400054A RID: 1354
		private readonly string defaultValueString;

		// Token: 0x0400054B RID: 1355
		private readonly EdmConcurrencyMode concurrencyMode;
	}
}
