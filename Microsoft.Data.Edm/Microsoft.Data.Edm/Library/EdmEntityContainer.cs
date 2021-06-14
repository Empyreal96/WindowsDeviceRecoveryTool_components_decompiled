using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Internal;

namespace Microsoft.Data.Edm.Library
{
	// Token: 0x020001CD RID: 461
	public class EdmEntityContainer : EdmElement, IEdmEntityContainer, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000AE1 RID: 2785 RVA: 0x0001FF28 File Offset: 0x0001E128
		public EdmEntityContainer(string namespaceName, string name)
		{
			EdmUtil.CheckArgumentNull<string>(namespaceName, "namespaceName");
			EdmUtil.CheckArgumentNull<string>(name, "name");
			this.namespaceName = namespaceName;
			this.name = name;
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06000AE2 RID: 2786 RVA: 0x0001FF82 File Offset: 0x0001E182
		public IEnumerable<IEdmEntityContainerElement> Elements
		{
			get
			{
				return this.containerElements;
			}
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06000AE3 RID: 2787 RVA: 0x0001FF8A File Offset: 0x0001E18A
		public string Namespace
		{
			get
			{
				return this.namespaceName;
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x0001FF92 File Offset: 0x0001E192
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0001FF9A File Offset: 0x0001E19A
		public EdmSchemaElementKind SchemaElementKind
		{
			get
			{
				return EdmSchemaElementKind.EntityContainer;
			}
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x0001FFA0 File Offset: 0x0001E1A0
		public void AddElement(IEdmEntityContainerElement element)
		{
			EdmUtil.CheckArgumentNull<IEdmEntityContainerElement>(element, "element");
			this.containerElements.Add(element);
			switch (element.ContainerElementKind)
			{
			case EdmContainerElementKind.None:
				throw new InvalidOperationException(Strings.EdmEntityContainer_CannotUseElementWithTypeNone);
			case EdmContainerElementKind.EntitySet:
				RegistrationHelper.AddElement<IEdmEntitySet>((IEdmEntitySet)element, element.Name, this.entitySetDictionary, new Func<IEdmEntitySet, IEdmEntitySet, IEdmEntitySet>(RegistrationHelper.CreateAmbiguousEntitySetBinding));
				return;
			case EdmContainerElementKind.FunctionImport:
				RegistrationHelper.AddFunction<IEdmFunctionImport>((IEdmFunctionImport)element, element.Name, this.functionImportDictionary);
				return;
			default:
				throw new InvalidOperationException(Strings.UnknownEnumVal_ContainerElementKind(element.ContainerElementKind));
			}
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x0002003C File Offset: 0x0001E23C
		public virtual EdmEntitySet AddEntitySet(string name, IEdmEntityType elementType)
		{
			EdmEntitySet edmEntitySet = new EdmEntitySet(this, name, elementType);
			this.AddElement(edmEntitySet);
			return edmEntitySet;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x0002005C File Offset: 0x0001E25C
		public virtual EdmFunctionImport AddFunctionImport(string name, IEdmTypeReference returnType)
		{
			EdmFunctionImport edmFunctionImport = new EdmFunctionImport(this, name, returnType);
			this.AddElement(edmFunctionImport);
			return edmFunctionImport;
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x0002007C File Offset: 0x0001E27C
		public virtual EdmFunctionImport AddFunctionImport(string name, IEdmTypeReference returnType, IEdmExpression entitySet)
		{
			EdmFunctionImport edmFunctionImport = new EdmFunctionImport(this, name, returnType, entitySet);
			this.AddElement(edmFunctionImport);
			return edmFunctionImport;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x0002009C File Offset: 0x0001E29C
		public virtual EdmFunctionImport AddFunctionImport(string name, IEdmTypeReference returnType, IEdmExpression entitySet, bool sideEffecting, bool composable, bool bindable)
		{
			EdmFunctionImport edmFunctionImport = new EdmFunctionImport(this, name, returnType, entitySet, sideEffecting, composable, bindable);
			this.AddElement(edmFunctionImport);
			return edmFunctionImport;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x000200C4 File Offset: 0x0001E2C4
		public virtual IEdmEntitySet FindEntitySet(string setName)
		{
			IEdmEntitySet result;
			if (!this.entitySetDictionary.TryGetValue(setName, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x000200E4 File Offset: 0x0001E2E4
		public IEnumerable<IEdmFunctionImport> FindFunctionImports(string functionName)
		{
			object obj;
			if (!this.functionImportDictionary.TryGetValue(functionName, out obj))
			{
				return Enumerable.Empty<IEdmFunctionImport>();
			}
			List<IEdmFunctionImport> list = obj as List<IEdmFunctionImport>;
			if (list != null)
			{
				return list;
			}
			return new IEdmFunctionImport[]
			{
				(IEdmFunctionImport)obj
			};
		}

		// Token: 0x04000521 RID: 1313
		private readonly string namespaceName;

		// Token: 0x04000522 RID: 1314
		private readonly string name;

		// Token: 0x04000523 RID: 1315
		private readonly List<IEdmEntityContainerElement> containerElements = new List<IEdmEntityContainerElement>();

		// Token: 0x04000524 RID: 1316
		private readonly Dictionary<string, IEdmEntitySet> entitySetDictionary = new Dictionary<string, IEdmEntitySet>();

		// Token: 0x04000525 RID: 1317
		private readonly Dictionary<string, object> functionImportDictionary = new Dictionary<string, object>();
	}
}
