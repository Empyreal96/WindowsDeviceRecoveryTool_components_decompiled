using System;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Library.Values;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000060 RID: 96
	internal class CsdlSemanticsDirectValueAnnotation : CsdlSemanticsElement, IEdmDirectValueAnnotation, IEdmNamedElement, IEdmElement
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00004D9D File Offset: 0x00002F9D
		public CsdlSemanticsDirectValueAnnotation(CsdlDirectValueAnnotation annotation, CsdlSemanticsModel model) : base(annotation)
		{
			this.annotation = annotation;
			this.model = model;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00004DBF File Offset: 0x00002FBF
		public override CsdlElement Element
		{
			get
			{
				return this.annotation;
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00004DC7 File Offset: 0x00002FC7
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00004DCF File Offset: 0x00002FCF
		public string NamespaceUri
		{
			get
			{
				return this.annotation.NamespaceName;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00004DDC File Offset: 0x00002FDC
		public string Name
		{
			get
			{
				return this.annotation.Name;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00004DE9 File Offset: 0x00002FE9
		public object Value
		{
			get
			{
				return this.valueCache.GetValue(this, CsdlSemanticsDirectValueAnnotation.ComputeValueFunc, null);
			}
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00004E00 File Offset: 0x00003000
		private IEdmValue ComputeValue()
		{
			IEdmStringValue edmStringValue = new EdmStringConstant(new EdmStringTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.String), false), this.annotation.Value);
			edmStringValue.SetIsSerializedAsElement(this.model, !this.annotation.IsAttribute);
			return edmStringValue;
		}

		// Token: 0x040000A5 RID: 165
		private readonly CsdlDirectValueAnnotation annotation;

		// Token: 0x040000A6 RID: 166
		private readonly CsdlSemanticsModel model;

		// Token: 0x040000A7 RID: 167
		private readonly Cache<CsdlSemanticsDirectValueAnnotation, IEdmValue> valueCache = new Cache<CsdlSemanticsDirectValueAnnotation, IEdmValue>();

		// Token: 0x040000A8 RID: 168
		private static readonly Func<CsdlSemanticsDirectValueAnnotation, IEdmValue> ComputeValueFunc = (CsdlSemanticsDirectValueAnnotation me) => me.ComputeValue();
	}
}
