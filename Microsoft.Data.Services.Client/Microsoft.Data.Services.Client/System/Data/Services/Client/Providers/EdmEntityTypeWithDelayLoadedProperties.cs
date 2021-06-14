using System;
using System.Collections.Generic;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace System.Data.Services.Client.Providers
{
	// Token: 0x02000011 RID: 17
	internal class EdmEntityTypeWithDelayLoadedProperties : EdmEntityType, IEdmEntityType, IEdmStructuredType, IEdmSchemaType, IEdmType, IEdmTerm, IEdmSchemaElement, IEdmNamedElement, IEdmVocabularyAnnotatable, IEdmElement
	{
		// Token: 0x06000063 RID: 99 RVA: 0x0000376C File Offset: 0x0000196C
		internal EdmEntityTypeWithDelayLoadedProperties(string namespaceName, string name, IEdmEntityType baseType, bool isAbstract, bool isOpen, Action<EdmEntityTypeWithDelayLoadedProperties> propertyLoadAction) : base(namespaceName, name, baseType, isAbstract, isOpen)
		{
			this.propertyLoadAction = propertyLoadAction;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000064 RID: 100 RVA: 0x0000378E File Offset: 0x0000198E
		public override IEnumerable<IEdmStructuralProperty> DeclaredKey
		{
			get
			{
				this.EnsurePropertyLoaded();
				return base.DeclaredKey;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000379C File Offset: 0x0000199C
		public override IEnumerable<IEdmProperty> DeclaredProperties
		{
			get
			{
				this.EnsurePropertyLoaded();
				return base.DeclaredProperties;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000037AC File Offset: 0x000019AC
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

		// Token: 0x04000016 RID: 22
		private readonly object lockObject = new object();

		// Token: 0x04000017 RID: 23
		private Action<EdmEntityTypeWithDelayLoadedProperties> propertyLoadAction;
	}
}
