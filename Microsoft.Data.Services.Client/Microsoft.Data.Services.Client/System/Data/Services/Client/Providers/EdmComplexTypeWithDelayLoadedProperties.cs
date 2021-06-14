using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace System.Data.Services.Client.Providers
{
	// Token: 0x02000010 RID: 16
	internal class EdmComplexTypeWithDelayLoadedProperties : EdmComplexType, IEdmComplexType, IEdmStructuredType, IEdmSchemaType, IEdmType, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000060 RID: 96 RVA: 0x000036E5 File Offset: 0x000018E5
		internal EdmComplexTypeWithDelayLoadedProperties(string namespaceName, string name, IEdmComplexType baseType, bool isAbstract, Action<EdmComplexTypeWithDelayLoadedProperties> propertyLoadAction) : base(namespaceName, name, baseType, isAbstract)
		{
			this.propertyLoadAction = propertyLoadAction;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00003705 File Offset: 0x00001905
		public override IEnumerable<IEdmProperty> DeclaredProperties
		{
			get
			{
				this.EnsurePropertyLoaded();
				return base.DeclaredProperties;
			}
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003714 File Offset: 0x00001914
		private void EnsurePropertyLoaded()
		{
			lock (this.lockObject)
			{
				if (this.propertyLoadAction != null)
				{
					this.propertyLoadAction(this);
					this.propertyLoadAction = null;
				}
			}
		}

		// Token: 0x04000014 RID: 20
		private readonly object lockObject = new object();

		// Token: 0x04000015 RID: 21
		private Action<EdmComplexTypeWithDelayLoadedProperties> propertyLoadAction;
	}
}
