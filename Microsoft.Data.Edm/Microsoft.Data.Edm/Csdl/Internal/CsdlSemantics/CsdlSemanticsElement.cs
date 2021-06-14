using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x0200003A RID: 58
	internal abstract class CsdlSemanticsElement : IEdmElement, IEdmLocatable
	{
		// Token: 0x060000BC RID: 188 RVA: 0x00002F17 File Offset: 0x00001117
		protected CsdlSemanticsElement(CsdlElement element)
		{
			if (element != null)
			{
				if (element.HasDirectValueAnnotations)
				{
					this.directValueAnnotationsCache = new Cache<CsdlSemanticsElement, IEnumerable<IEdmDirectValueAnnotation>>();
				}
				if (element.HasVocabularyAnnotations)
				{
					this.inlineVocabularyAnnotationsCache = new Cache<CsdlSemanticsElement, IEnumerable<IEdmVocabularyAnnotation>>();
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000BD RID: 189
		public abstract CsdlSemanticsModel Model { get; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000BE RID: 190
		public abstract CsdlElement Element { get; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00002F48 File Offset: 0x00001148
		public IEnumerable<IEdmVocabularyAnnotation> InlineVocabularyAnnotations
		{
			get
			{
				if (this.inlineVocabularyAnnotationsCache == null)
				{
					return CsdlSemanticsElement.emptyVocabularyAnnotations;
				}
				return this.inlineVocabularyAnnotationsCache.GetValue(this, CsdlSemanticsElement.ComputeInlineVocabularyAnnotationsFunc, null);
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00002F6A File Offset: 0x0000116A
		public EdmLocation Location
		{
			get
			{
				if (this.Element == null || this.Element.Location == null)
				{
					return new ObjectLocation(this);
				}
				return this.Element.Location;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00002F93 File Offset: 0x00001193
		public IEnumerable<IEdmDirectValueAnnotation> DirectValueAnnotations
		{
			get
			{
				if (this.directValueAnnotationsCache == null)
				{
					return null;
				}
				return this.directValueAnnotationsCache.GetValue(this, CsdlSemanticsElement.ComputeDirectValueAnnotationsFunc, null);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00002FB1 File Offset: 0x000011B1
		protected static List<T> AllocateAndAdd<T>(List<T> list, T item)
		{
			if (list == null)
			{
				list = new List<T>();
			}
			list.Add(item);
			return list;
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00002FC5 File Offset: 0x000011C5
		protected static List<T> AllocateAndAdd<T>(List<T> list, IEnumerable<T> items)
		{
			if (list == null)
			{
				list = new List<T>();
			}
			list.AddRange(items);
			return list;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00002FD9 File Offset: 0x000011D9
		protected virtual IEnumerable<IEdmVocabularyAnnotation> ComputeInlineVocabularyAnnotations()
		{
			return this.Model.WrapInlineVocabularyAnnotations(this, null);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00002FE8 File Offset: 0x000011E8
		protected IEnumerable<IEdmDirectValueAnnotation> ComputeDirectValueAnnotations()
		{
			if (this.Element == null)
			{
				return null;
			}
			List<CsdlDirectValueAnnotation> list = this.Element.ImmediateValueAnnotations.ToList<CsdlDirectValueAnnotation>();
			CsdlElementWithDocumentation csdlElementWithDocumentation = this.Element as CsdlElementWithDocumentation;
			CsdlDocumentation csdlDocumentation = (csdlElementWithDocumentation != null) ? csdlElementWithDocumentation.Documentation : null;
			if (csdlDocumentation != null || list.FirstOrDefault<CsdlDirectValueAnnotation>() != null)
			{
				List<IEdmDirectValueAnnotation> list2 = new List<IEdmDirectValueAnnotation>();
				foreach (CsdlDirectValueAnnotation annotation in list)
				{
					list2.Add(new CsdlSemanticsDirectValueAnnotation(annotation, this.Model));
				}
				if (csdlDocumentation != null)
				{
					list2.Add(new CsdlSemanticsDocumentation(csdlDocumentation, this.Model));
				}
				return list2;
			}
			return null;
		}

		// Token: 0x0400003D RID: 61
		private readonly Cache<CsdlSemanticsElement, IEnumerable<IEdmVocabularyAnnotation>> inlineVocabularyAnnotationsCache;

		// Token: 0x0400003E RID: 62
		private static readonly Func<CsdlSemanticsElement, IEnumerable<IEdmVocabularyAnnotation>> ComputeInlineVocabularyAnnotationsFunc = (CsdlSemanticsElement me) => me.ComputeInlineVocabularyAnnotations();

		// Token: 0x0400003F RID: 63
		private readonly Cache<CsdlSemanticsElement, IEnumerable<IEdmDirectValueAnnotation>> directValueAnnotationsCache;

		// Token: 0x04000040 RID: 64
		private static readonly Func<CsdlSemanticsElement, IEnumerable<IEdmDirectValueAnnotation>> ComputeDirectValueAnnotationsFunc = (CsdlSemanticsElement me) => me.ComputeDirectValueAnnotations();

		// Token: 0x04000041 RID: 65
		private static readonly IEnumerable<IEdmVocabularyAnnotation> emptyVocabularyAnnotations = Enumerable.Empty<IEdmVocabularyAnnotation>();
	}
}
