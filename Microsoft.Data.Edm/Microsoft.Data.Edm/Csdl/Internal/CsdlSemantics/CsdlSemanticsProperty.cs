using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x020001AA RID: 426
	internal class CsdlSemanticsProperty : CsdlSemanticsElement, IEdmStructuralProperty, IEdmProperty, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000930 RID: 2352 RVA: 0x000188F3 File Offset: 0x00016AF3
		public CsdlSemanticsProperty(CsdlSemanticsStructuredTypeDefinition declaringType, CsdlProperty property) : base(property)
		{
			this.property = property;
			this.declaringType = declaringType;
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000931 RID: 2353 RVA: 0x00018915 File Offset: 0x00016B15
		public string Name
		{
			get
			{
				return this.property.Name;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x00018922 File Offset: 0x00016B22
		public IEdmStructuredType DeclaringType
		{
			get
			{
				return this.declaringType;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06000933 RID: 2355 RVA: 0x0001892A File Offset: 0x00016B2A
		public IEdmTypeReference Type
		{
			get
			{
				return this.typeCache.GetValue(this, CsdlSemanticsProperty.ComputeTypeFunc, null);
			}
		}

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x0001893E File Offset: 0x00016B3E
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.declaringType.Model;
			}
		}

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x0001894B File Offset: 0x00016B4B
		public string DefaultValueString
		{
			get
			{
				return this.property.DefaultValue;
			}
		}

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x00018958 File Offset: 0x00016B58
		public EdmConcurrencyMode ConcurrencyMode
		{
			get
			{
				if (!this.property.IsFixedConcurrency)
				{
					return EdmConcurrencyMode.None;
				}
				return EdmConcurrencyMode.Fixed;
			}
		}

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x0001896A File Offset: 0x00016B6A
		public EdmPropertyKind PropertyKind
		{
			get
			{
				return EdmPropertyKind.Structural;
			}
		}

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0001896D File Offset: 0x00016B6D
		public override CsdlElement Element
		{
			get
			{
				return this.property;
			}
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00018975 File Offset: 0x00016B75
		protected override IEnumerable<IEdmVocabularyAnnotation> ComputeInlineVocabularyAnnotations()
		{
			return this.Model.WrapInlineVocabularyAnnotations(this, this.declaringType.Context);
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0001898E File Offset: 0x00016B8E
		private IEdmTypeReference ComputeType()
		{
			return CsdlSemanticsModel.WrapTypeReference(this.declaringType.Context, this.property.Type);
		}

		// Token: 0x04000478 RID: 1144
		protected CsdlProperty property;

		// Token: 0x04000479 RID: 1145
		private readonly CsdlSemanticsStructuredTypeDefinition declaringType;

		// Token: 0x0400047A RID: 1146
		private readonly Cache<CsdlSemanticsProperty, IEdmTypeReference> typeCache = new Cache<CsdlSemanticsProperty, IEdmTypeReference>();

		// Token: 0x0400047B RID: 1147
		private static readonly Func<CsdlSemanticsProperty, IEdmTypeReference> ComputeTypeFunc = (CsdlSemanticsProperty me) => me.ComputeType();
	}
}
