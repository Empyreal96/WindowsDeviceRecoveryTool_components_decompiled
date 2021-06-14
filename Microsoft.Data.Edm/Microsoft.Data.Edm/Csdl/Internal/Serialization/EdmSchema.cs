using System;
using System.Collections.Generic;
using Microsoft.Data.Edm.Annotations;

namespace Microsoft.Data.Edm.Csdl.Internal.Serialization
{
	// Token: 0x020001B2 RID: 434
	internal class EdmSchema
	{
		// Token: 0x06000A27 RID: 2599 RVA: 0x0001D244 File Offset: 0x0001B444
		public EdmSchema(string namespaceString)
		{
			this.schemaNamespace = namespaceString;
			this.schemaElements = new List<IEdmSchemaElement>();
			this.entityContainers = new List<IEdmEntityContainer>();
			this.associationNavigationProperties = new List<IEdmNavigationProperty>();
			this.annotations = new Dictionary<string, List<IEdmVocabularyAnnotation>>();
			this.usedNamespaces = new List<string>();
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06000A28 RID: 2600 RVA: 0x0001D295 File Offset: 0x0001B495
		public string Namespace
		{
			get
			{
				return this.schemaNamespace;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06000A29 RID: 2601 RVA: 0x0001D29D File Offset: 0x0001B49D
		public List<IEdmSchemaElement> SchemaElements
		{
			get
			{
				return this.schemaElements;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0001D2A5 File Offset: 0x0001B4A5
		public List<IEdmEntityContainer> EntityContainers
		{
			get
			{
				return this.entityContainers;
			}
		}

		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x06000A2B RID: 2603 RVA: 0x0001D2AD File Offset: 0x0001B4AD
		public List<IEdmNavigationProperty> AssociationNavigationProperties
		{
			get
			{
				return this.associationNavigationProperties;
			}
		}

		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x06000A2C RID: 2604 RVA: 0x0001D2B5 File Offset: 0x0001B4B5
		public IEnumerable<string> NamespaceUsings
		{
			get
			{
				return this.usedNamespaces;
			}
		}

		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000A2D RID: 2605 RVA: 0x0001D2BD File Offset: 0x0001B4BD
		public IEnumerable<KeyValuePair<string, List<IEdmVocabularyAnnotation>>> OutOfLineAnnotations
		{
			get
			{
				return this.annotations;
			}
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0001D2C5 File Offset: 0x0001B4C5
		public void AddSchemaElement(IEdmSchemaElement element)
		{
			this.schemaElements.Add(element);
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0001D2D3 File Offset: 0x0001B4D3
		public void AddEntityContainer(IEdmEntityContainer container)
		{
			this.entityContainers.Add(container);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0001D2E1 File Offset: 0x0001B4E1
		public void AddNamespaceUsing(string usedNamespace)
		{
			if (usedNamespace != "Edm" && !this.usedNamespaces.Contains(usedNamespace))
			{
				this.usedNamespaces.Add(usedNamespace);
			}
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0001D30C File Offset: 0x0001B50C
		public void AddVocabularyAnnotation(IEdmVocabularyAnnotation annotation)
		{
			List<IEdmVocabularyAnnotation> list;
			if (!this.annotations.TryGetValue(annotation.TargetString(), out list))
			{
				list = new List<IEdmVocabularyAnnotation>();
				this.annotations[annotation.TargetString()] = list;
			}
			list.Add(annotation);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0001D34D File Offset: 0x0001B54D
		internal void AddAssociatedNavigationProperty(IEdmNavigationProperty property)
		{
			this.associationNavigationProperties.Add(property);
		}

		// Token: 0x040004AF RID: 1199
		private readonly string schemaNamespace;

		// Token: 0x040004B0 RID: 1200
		private readonly List<IEdmSchemaElement> schemaElements;

		// Token: 0x040004B1 RID: 1201
		private readonly List<IEdmNavigationProperty> associationNavigationProperties;

		// Token: 0x040004B2 RID: 1202
		private readonly List<IEdmEntityContainer> entityContainers;

		// Token: 0x040004B3 RID: 1203
		private readonly Dictionary<string, List<IEdmVocabularyAnnotation>> annotations;

		// Token: 0x040004B4 RID: 1204
		private readonly List<string> usedNamespaces;
	}
}
