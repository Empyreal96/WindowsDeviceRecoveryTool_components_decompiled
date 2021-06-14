using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Internal;
using Microsoft.Data.Edm.Library.Annotations;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x02000174 RID: 372
	public abstract class EdmModelBase : EdmElement, IEdmModel, IEdmElement
	{
		// Token: 0x06000815 RID: 2069 RVA: 0x00015E2C File Offset: 0x0001402C
		protected EdmModelBase(IEnumerable<IEdmModel> referencedModels, EdmDirectValueAnnotationsManager annotationsManager)
		{
			EdmUtil.CheckArgumentNull<IEnumerable<IEdmModel>>(referencedModels, "referencedModels");
			EdmUtil.CheckArgumentNull<EdmDirectValueAnnotationsManager>(annotationsManager, "annotationsManager");
			this.referencedModels = new List<IEdmModel>(referencedModels);
			this.referencedModels.Add(EdmCoreModel.Instance);
			this.annotationsManager = annotationsManager;
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06000816 RID: 2070
		public abstract IEnumerable<IEdmSchemaElement> SchemaElements { get; }

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x00015EA6 File Offset: 0x000140A6
		public virtual IEnumerable<IEdmVocabularyAnnotation> VocabularyAnnotations
		{
			get
			{
				return Enumerable.Empty<IEdmVocabularyAnnotation>();
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000818 RID: 2072 RVA: 0x00015EAD File Offset: 0x000140AD
		public IEnumerable<IEdmModel> ReferencedModels
		{
			get
			{
				return this.referencedModels;
			}
		}

		// Token: 0x1700034F RID: 847
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x00015EB5 File Offset: 0x000140B5
		public IEdmDirectValueAnnotationsManager DirectValueAnnotationsManager
		{
			get
			{
				return this.annotationsManager;
			}
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00015EC0 File Offset: 0x000140C0
		public IEdmEntityContainer FindDeclaredEntityContainer(string name)
		{
			IEdmEntityContainer result;
			if (!this.containersDictionary.TryGetValue(name, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00015EE0 File Offset: 0x000140E0
		public IEdmSchemaType FindDeclaredType(string qualifiedName)
		{
			IEdmSchemaType result;
			this.schemaTypeDictionary.TryGetValue(qualifiedName, out result);
			return result;
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00015F00 File Offset: 0x00014100
		public IEdmValueTerm FindDeclaredValueTerm(string qualifiedName)
		{
			IEdmValueTerm result;
			this.valueTermDictionary.TryGetValue(qualifiedName, out result);
			return result;
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00015F20 File Offset: 0x00014120
		public IEnumerable<IEdmFunction> FindDeclaredFunctions(string qualifiedName)
		{
			object obj;
			if (!this.functionDictionary.TryGetValue(qualifiedName, out obj))
			{
				return Enumerable.Empty<IEdmFunction>();
			}
			List<IEdmFunction> list = obj as List<IEdmFunction>;
			if (list != null)
			{
				return list;
			}
			return new IEdmFunction[]
			{
				(IEdmFunction)obj
			};
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00015F60 File Offset: 0x00014160
		public virtual IEnumerable<IEdmVocabularyAnnotation> FindDeclaredVocabularyAnnotations(IEdmVocabularyAnnotatable element)
		{
			return Enumerable.Empty<IEdmVocabularyAnnotation>();
		}

		// Token: 0x0600081F RID: 2079
		public abstract IEnumerable<IEdmStructuredType> FindDirectlyDerivedTypes(IEdmStructuredType baseType);

		// Token: 0x06000820 RID: 2080 RVA: 0x00015F67 File Offset: 0x00014167
		protected void RegisterElement(IEdmSchemaElement element)
		{
			EdmUtil.CheckArgumentNull<IEdmSchemaElement>(element, "element");
			RegistrationHelper.RegisterSchemaElement(element, this.schemaTypeDictionary, this.valueTermDictionary, this.functionDictionary, this.containersDictionary);
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x00015F93 File Offset: 0x00014193
		protected void AddReferencedModel(IEdmModel model)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			this.referencedModels.Add(model);
		}

		// Token: 0x0400040B RID: 1035
		private readonly EdmDirectValueAnnotationsManager annotationsManager;

		// Token: 0x0400040C RID: 1036
		private readonly Dictionary<string, IEdmEntityContainer> containersDictionary = new Dictionary<string, IEdmEntityContainer>();

		// Token: 0x0400040D RID: 1037
		private readonly Dictionary<string, IEdmSchemaType> schemaTypeDictionary = new Dictionary<string, IEdmSchemaType>();

		// Token: 0x0400040E RID: 1038
		private readonly Dictionary<string, IEdmValueTerm> valueTermDictionary = new Dictionary<string, IEdmValueTerm>();

		// Token: 0x0400040F RID: 1039
		private readonly Dictionary<string, object> functionDictionary = new Dictionary<string, object>();

		// Token: 0x04000410 RID: 1040
		private readonly List<IEdmModel> referencedModels;
	}
}
