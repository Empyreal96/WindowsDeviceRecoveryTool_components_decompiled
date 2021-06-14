using System;
using System.Collections.Generic;

namespace ClickerUtilityLibrary.DataModel
{
	// Token: 0x02000018 RID: 24
	public class DataElementDictionary
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600009E RID: 158 RVA: 0x000059CC File Offset: 0x00003BCC
		public static DataElementDictionary Instance
		{
			get
			{
				bool flag = DataElementDictionary.instance == null;
				if (flag)
				{
					object obj = DataElementDictionary.sHandler;
					lock (obj)
					{
						DataElementDictionary.instance = new DataElementDictionary();
					}
				}
				return DataElementDictionary.instance;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00005A34 File Offset: 0x00003C34
		private DataElementDictionary()
		{
			this.mDataTable = new Dictionary<DataElementType, DataElement>();
			this.Initialize();
		}

		// Token: 0x17000026 RID: 38
		public DataElement this[DataElementType dataElementType]
		{
			get
			{
				return this.mDataTable[dataElementType];
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005A70 File Offset: 0x00003C70
		private void Initialize()
		{
			foreach (object obj in Enum.GetValues(typeof(DataElementType)))
			{
				DataElementType dataElementType = (DataElementType)obj;
				this.mDataTable.Add(dataElementType, new DataElement(dataElementType, 4, DataType.DATA_TYPE_UINT32, dataElementType.ToString()));
			}
			this.mDataTable[DataElementType.DI_FW_BINARY].DataType = DataType.DATA_TYPE_BINARYSTREAM;
		}

		// Token: 0x040000B7 RID: 183
		private static volatile DataElementDictionary instance;

		// Token: 0x040000B8 RID: 184
		private static readonly object sHandler = new object();

		// Token: 0x040000B9 RID: 185
		private readonly Dictionary<DataElementType, DataElement> mDataTable;
	}
}
