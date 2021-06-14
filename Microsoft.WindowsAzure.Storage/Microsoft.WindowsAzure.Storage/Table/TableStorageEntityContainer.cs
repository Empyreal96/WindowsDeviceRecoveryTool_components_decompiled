using System;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000043 RID: 67
	internal class TableStorageEntityContainer : EdmEntityContainer
	{
		// Token: 0x06000C62 RID: 3170 RVA: 0x0002C6F8 File Offset: 0x0002A8F8
		public TableStorageEntityContainer(TableStorageModel model) : base("AzureTableStorage", "DefaultContainer")
		{
			this.model = model;
			this.tablesSet = this.AddEntitySet("Tables", this.model.TableType);
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0002C730 File Offset: 0x0002A930
		public override IEdmEntitySet FindEntitySet(string setName)
		{
			if (this.model.IsKnownType(setName))
			{
				return null;
			}
			if (setName == "Tables")
			{
				return this.tablesSet;
			}
			IEdmEntitySet edmEntitySet = base.FindEntitySet(setName);
			if (edmEntitySet == null)
			{
				string qualifiedName = this.InferServerTypeNameFromTableName(setName);
				edmEntitySet = new EdmEntitySet(this, setName, (IEdmEntityType)((IEdmModel)this.model).FindDeclaredType(qualifiedName));
				base.AddElement(edmEntitySet);
			}
			return edmEntitySet;
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0002C795 File Offset: 0x0002A995
		private string InferServerTypeNameFromTableName(string setName)
		{
			return this.model.AccountName + '.' + setName;
		}

		// Token: 0x0400017A RID: 378
		private readonly TableStorageModel model;

		// Token: 0x0400017B RID: 379
		private readonly IEdmEntitySet tablesSet;
	}
}
