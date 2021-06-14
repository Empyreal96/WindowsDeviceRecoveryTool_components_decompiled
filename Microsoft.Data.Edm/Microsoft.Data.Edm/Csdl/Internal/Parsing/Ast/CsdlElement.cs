using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast
{
	// Token: 0x02000008 RID: 8
	internal abstract class CsdlElement
	{
		// Token: 0x0600002A RID: 42 RVA: 0x00002859 File Offset: 0x00000A59
		public CsdlElement(CsdlLocation location)
		{
			this.location = location;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002868 File Offset: 0x00000A68
		public virtual bool HasDirectValueAnnotations
		{
			get
			{
				return this.HasAnnotations<CsdlDirectValueAnnotation>();
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002870 File Offset: 0x00000A70
		public bool HasVocabularyAnnotations
		{
			get
			{
				return this.HasAnnotations<CsdlVocabularyAnnotationBase>();
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002878 File Offset: 0x00000A78
		public IEnumerable<CsdlDirectValueAnnotation> ImmediateValueAnnotations
		{
			get
			{
				return this.GetAnnotations<CsdlDirectValueAnnotation>();
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002880 File Offset: 0x00000A80
		public IEnumerable<CsdlVocabularyAnnotationBase> VocabularyAnnotations
		{
			get
			{
				return this.GetAnnotations<CsdlVocabularyAnnotationBase>();
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002888 File Offset: 0x00000A88
		public EdmLocation Location
		{
			get
			{
				return this.location;
			}
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002890 File Offset: 0x00000A90
		public void AddAnnotation(CsdlDirectValueAnnotation annotation)
		{
			this.AddUntypedAnnotation(annotation);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002899 File Offset: 0x00000A99
		public void AddAnnotation(CsdlVocabularyAnnotationBase annotation)
		{
			this.AddUntypedAnnotation(annotation);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000028A2 File Offset: 0x00000AA2
		private IEnumerable<T> GetAnnotations<T>() where T : class
		{
			if (this.annotations == null)
			{
				return Enumerable.Empty<T>();
			}
			return this.annotations.OfType<T>();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x000028BD File Offset: 0x00000ABD
		private void AddUntypedAnnotation(object annotation)
		{
			if (this.annotations == null)
			{
				this.annotations = new List<object>();
			}
			this.annotations.Add(annotation);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000028E0 File Offset: 0x00000AE0
		private bool HasAnnotations<T>()
		{
			if (this.annotations == null)
			{
				return false;
			}
			foreach (object obj in this.annotations)
			{
				if (obj is T)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400000B RID: 11
		protected List<object> annotations;

		// Token: 0x0400000C RID: 12
		protected EdmLocation location;
	}
}
