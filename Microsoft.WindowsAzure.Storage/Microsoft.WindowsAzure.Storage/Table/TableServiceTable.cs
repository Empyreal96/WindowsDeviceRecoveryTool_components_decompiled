using System;
using System.Data.Services.Common;
using Microsoft.WindowsAzure.Storage.Core.Util;

namespace Microsoft.WindowsAzure.Storage.Table
{
	// Token: 0x0200014F RID: 335
	[DataServiceKey("TableName")]
	internal class TableServiceTable
	{
		// Token: 0x060014FA RID: 5370 RVA: 0x0004FD02 File Offset: 0x0004DF02
		public TableServiceTable()
		{
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x0004FD0A File Offset: 0x0004DF0A
		public TableServiceTable(string name)
		{
			CommonUtility.CheckStringParameter(name, false, "name", 32768);
			this.TableName = name;
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x060014FC RID: 5372 RVA: 0x0004FD2A File Offset: 0x0004DF2A
		// (set) Token: 0x060014FD RID: 5373 RVA: 0x0004FD32 File Offset: 0x0004DF32
		public string TableName
		{
			get
			{
				return this.tableName;
			}
			set
			{
				CommonUtility.CheckStringParameter(value, false, "TableName", 32768);
				this.tableName = value;
			}
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x0004FD4C File Offset: 0x0004DF4C
		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			TableServiceTable tableServiceTable = obj as TableServiceTable;
			return tableServiceTable != null && this.TableName.Equals(tableServiceTable.TableName, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x0004FD7C File Offset: 0x0004DF7C
		public override int GetHashCode()
		{
			return this.TableName.GetHashCode();
		}

		// Token: 0x0400082D RID: 2093
		private string tableName;
	}
}
