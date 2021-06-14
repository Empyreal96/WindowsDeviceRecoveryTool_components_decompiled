using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Annotations;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Csdl.Internal.CsdlSemantics
{
	// Token: 0x02000167 RID: 359
	internal abstract class CsdlSemanticsStructuredTypeDefinition : CsdlSemanticsTypeDefinition, IEdmStructuredType, IEdmType, IEdmElement
	{
		// Token: 0x06000782 RID: 1922 RVA: 0x00014891 File Offset: 0x00012A91
		protected CsdlSemanticsStructuredTypeDefinition(CsdlSemanticsSchema context, CsdlStructuredType type) : base(type)
		{
			this.context = context;
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x000148B7 File Offset: 0x00012AB7
		public virtual bool IsAbstract
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x000148BA File Offset: 0x00012ABA
		public virtual bool IsOpen
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000785 RID: 1925
		public abstract IEdmStructuredType BaseType { get; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x000148BD File Offset: 0x00012ABD
		public override CsdlElement Element
		{
			get
			{
				return this.MyStructured;
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x000148C5 File Offset: 0x00012AC5
		public override CsdlSemanticsModel Model
		{
			get
			{
				return this.context.Model;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x000148D2 File Offset: 0x00012AD2
		public string Namespace
		{
			get
			{
				return this.context.Namespace;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000789 RID: 1929 RVA: 0x000148DF File Offset: 0x00012ADF
		public CsdlSemanticsSchema Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x0600078A RID: 1930 RVA: 0x000148E7 File Offset: 0x00012AE7
		public IEnumerable<IEdmProperty> DeclaredProperties
		{
			get
			{
				return this.declaredPropertiesCache.GetValue(this, CsdlSemanticsStructuredTypeDefinition.ComputeDeclaredPropertiesFunc, null);
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600078B RID: 1931
		protected abstract CsdlStructuredType MyStructured { get; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x0600078C RID: 1932 RVA: 0x000148FB File Offset: 0x00012AFB
		private IDictionary<string, IEdmProperty> PropertiesDictionary
		{
			get
			{
				return this.propertiesDictionaryCache.GetValue(this, CsdlSemanticsStructuredTypeDefinition.ComputePropertiesDictionaryFunc, null);
			}
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00014910 File Offset: 0x00012B10
		public IEdmProperty FindProperty(string name)
		{
			IEdmProperty result;
			this.PropertiesDictionary.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00014930 File Offset: 0x00012B30
		protected virtual List<IEdmProperty> ComputeDeclaredProperties()
		{
			List<IEdmProperty> list = new List<IEdmProperty>();
			foreach (CsdlProperty property in this.MyStructured.Properties)
			{
				list.Add(new CsdlSemanticsProperty(this, property));
			}
			return list;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00014990 File Offset: 0x00012B90
		protected string GetCyclicBaseTypeName(string baseTypeName)
		{
			IEdmSchemaType edmSchemaType = this.context.FindType(baseTypeName);
			if (edmSchemaType == null)
			{
				return baseTypeName;
			}
			return edmSchemaType.FullName();
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x000149B5 File Offset: 0x00012BB5
		protected override IEnumerable<IEdmVocabularyAnnotation> ComputeInlineVocabularyAnnotations()
		{
			return this.Model.WrapInlineVocabularyAnnotations(this, this.context);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x000149CC File Offset: 0x00012BCC
		private IDictionary<string, IEdmProperty> ComputePropertiesDictionary()
		{
			Dictionary<string, IEdmProperty> dictionary = new Dictionary<string, IEdmProperty>();
			foreach (IEdmProperty edmProperty in this.Properties())
			{
				RegistrationHelper.RegisterProperty(edmProperty, edmProperty.Name, dictionary);
			}
			return dictionary;
		}

		// Token: 0x040003C7 RID: 967
		private readonly CsdlSemanticsSchema context;

		// Token: 0x040003C8 RID: 968
		private readonly Cache<CsdlSemanticsStructuredTypeDefinition, List<IEdmProperty>> declaredPropertiesCache = new Cache<CsdlSemanticsStructuredTypeDefinition, List<IEdmProperty>>();

		// Token: 0x040003C9 RID: 969
		private static readonly Func<CsdlSemanticsStructuredTypeDefinition, List<IEdmProperty>> ComputeDeclaredPropertiesFunc = (CsdlSemanticsStructuredTypeDefinition me) => me.ComputeDeclaredProperties();

		// Token: 0x040003CA RID: 970
		private readonly Cache<CsdlSemanticsStructuredTypeDefinition, IDictionary<string, IEdmProperty>> propertiesDictionaryCache = new Cache<CsdlSemanticsStructuredTypeDefinition, IDictionary<string, IEdmProperty>>();

		// Token: 0x040003CB RID: 971
		private static readonly Func<CsdlSemanticsStructuredTypeDefinition, IDictionary<string, IEdmProperty>> ComputePropertiesDictionaryFunc = (CsdlSemanticsStructuredTypeDefinition me) => me.ComputePropertiesDictionary();
	}
}
