using System;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.OData;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x02000044 RID: 68
	public class TableStorageModel : EdmModel, IEdmModel, IEdmElement
	{
		// Token: 0x06000C65 RID: 3173 RVA: 0x0002C7AF File Offset: 0x0002A9AF
		internal TableStorageModel() : this("account")
		{
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0002C7BC File Offset: 0x0002A9BC
		public TableStorageModel(string accountName)
		{
			this.accountName = accountName;
			this.tableType = new EdmEntityType(this.accountName, "Tables");
			this.tableType.AddKeys(new IEdmStructuralProperty[]
			{
				this.tableType.AddStructuralProperty("TableName", EdmPrimitiveTypeKind.String)
			});
			base.AddElement(this.tableType);
			TableStorageEntityContainer tableStorageEntityContainer = new TableStorageEntityContainer(this);
			base.AddElement(tableStorageEntityContainer);
			this.SetIsDefaultEntityContainer(tableStorageEntityContainer, true);
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0002C838 File Offset: 0x0002AA38
		IEdmSchemaType IEdmModel.FindDeclaredType(string qualifiedName)
		{
			CommonUtility.AssertNotNullOrEmpty("qualifiedName", qualifiedName);
			if (qualifiedName.StartsWith("Edm.", StringComparison.Ordinal))
			{
				return null;
			}
			IEdmSchemaType edmSchemaType = base.FindDeclaredType(qualifiedName);
			if (edmSchemaType == null)
			{
				string name;
				string namespaceName;
				TableStorageModel.SplitFullTypeName(qualifiedName, out name, out namespaceName);
				edmSchemaType = TableStorageModel.CreateEntityType(namespaceName, name);
				base.AddElement(edmSchemaType);
			}
			return edmSchemaType;
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0002C888 File Offset: 0x0002AA88
		internal static EdmEntityType CreateEntityType(string namespaceName, string name)
		{
			EdmEntityType edmEntityType = new EdmEntityType(namespaceName, name, null, false, true);
			edmEntityType.AddKeys(new IEdmStructuralProperty[]
			{
				edmEntityType.AddStructuralProperty("RowKey", EdmPrimitiveTypeKind.String),
				edmEntityType.AddStructuralProperty("PartitionKey", EdmPrimitiveTypeKind.String)
			});
			edmEntityType.AddStructuralProperty("Timestamp", EdmCoreModel.Instance.GetDateTime(false), null, EdmConcurrencyMode.Fixed);
			return edmEntityType;
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0002C8E8 File Offset: 0x0002AAE8
		internal bool IsKnownType(string qualifiedName)
		{
			return base.FindDeclaredType(qualifiedName) != null;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0002C8F8 File Offset: 0x0002AAF8
		private static void SplitFullTypeName(string qualifiedName, out string name, out string namespaceName)
		{
			int num = qualifiedName.LastIndexOf('.');
			name = qualifiedName.Substring(num + 1);
			namespaceName = qualifiedName.Substring(0, num);
		}

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x0002C923 File Offset: 0x0002AB23
		internal string AccountName
		{
			get
			{
				return this.accountName;
			}
		}

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x0002C92B File Offset: 0x0002AB2B
		internal EdmEntityType TableType
		{
			get
			{
				return this.tableType;
			}
		}

		// Token: 0x0400017C RID: 380
		private string accountName;

		// Token: 0x0400017D RID: 381
		private readonly EdmEntityType tableType;
	}
}
