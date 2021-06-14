using System;
using System.Collections;
using System.Collections.Generic;

namespace ClickerUtilityLibrary.DataModel
{
	// Token: 0x02000012 RID: 18
	public class CommandDictionary : IEnumerable<FCommand>, IEnumerable
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000067 RID: 103 RVA: 0x00004BE8 File Offset: 0x00002DE8
		public static CommandDictionary Instance
		{
			get
			{
				bool flag = CommandDictionary.instance == null;
				if (flag)
				{
					object obj = CommandDictionary.sHandler;
					lock (obj)
					{
						CommandDictionary.instance = new CommandDictionary();
					}
				}
				return CommandDictionary.instance;
			}
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004C50 File Offset: 0x00002E50
		private CommandDictionary()
		{
			this.mCmdDictionary = new Dictionary<int, FCommand>();
			this.Initialize();
		}

		// Token: 0x17000010 RID: 16
		public FCommand this[int commandCode]
		{
			get
			{
				return this.mCmdDictionary[commandCode];
			}
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004C8C File Offset: 0x00002E8C
		private void Initialize()
		{
			List<DataElement> list = new List<DataElement>();
			List<DataElement> list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_BL_VER]);
			this.mCmdDictionary.Add(0, new FCommand(0, "CMD_GET_BL_INFO", list, list2));
			list = new List<DataElement>();
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_ADDRESS]);
			list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			this.mCmdDictionary.Add(1, new FCommand(1, "CMD_ERASE_FLASH", list, list2));
			list = new List<DataElement>();
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_CONFIG_SIZE]);
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_ADDRESS]);
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_VER]);
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_UPDATE_DATE]);
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_SIZE]);
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_CHECKSUM]);
			list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			this.mCmdDictionary.Add(2, new FCommand(2, "CMD_WRITE_FW_CONFIG", list, list2));
			list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_FW_CONFIG_SIZE]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_FW_ADDRESS]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_FW_VER]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_FW_UPDATE_DATE]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_FW_SIZE]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_FW_CHECKSUM]);
			this.mCmdDictionary.Add(3, new FCommand(3, "CMD_READ_FW_CONFIG", null, list2));
			list = new List<DataElement>();
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_TRANSFER_OFFSET]);
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_BINARY]);
			list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			this.mCmdDictionary.Add(4, new FCommand(4, "CMD_DOWNLOAD_FW", list, list2));
			list = new List<DataElement>();
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_TRANSFER_OFFSET]);
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_SIZE]);
			list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_FW_BINARY]);
			this.mCmdDictionary.Add(5, new FCommand(5, "CMD_READ_MEMORY", list, list2));
			list = new List<DataElement>();
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_ADDRESS]);
			list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			this.mCmdDictionary.Add(6, new FCommand(6, "CMD_RUN_APP", list, list2));
			list = new List<DataElement>();
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_TRANSFER_OFFSET]);
			list.Add(DataElementDictionary.Instance[DataElementType.DI_FW_SIZE]);
			list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_FW_BINARY]);
			this.mCmdDictionary.Add(7, new FCommand(7, "CMD_DUMP_FLASH", list, list2));
			list = new List<DataElement>();
			list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_FW_CHECKSUM]);
			this.mCmdDictionary.Add(8, new FCommand(8, "CMD_CALC_IMAGE_CHECKSUM", list, list2));
			list = new List<DataElement>();
			list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			this.mCmdDictionary.Add(9, new FCommand(9, "CMD_RESET", list, list2));
			list = new List<DataElement>();
			list2 = new List<DataElement>();
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_D_STATUS]);
			list2.Add(DataElementDictionary.Instance[DataElementType.DI_FW_HWID]);
			this.mCmdDictionary.Add(10, new FCommand(10, "CMD_GET_HWID", list, list2));
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000050CC File Offset: 0x000032CC
		public bool ContainsKey(int commandCode)
		{
			return this.mCmdDictionary.ContainsKey(commandCode);
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600006C RID: 108 RVA: 0x000050EC File Offset: 0x000032EC
		public int Count
		{
			get
			{
				return this.mCmdDictionary.Count;
			}
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000510C File Offset: 0x0000330C
		public IEnumerator<FCommand> GetEnumerator()
		{
			return this.mCmdDictionary.Values.GetEnumerator();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00005134 File Offset: 0x00003334
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04000083 RID: 131
		private static volatile CommandDictionary instance;

		// Token: 0x04000084 RID: 132
		private static object sHandler = new object();

		// Token: 0x04000085 RID: 133
		private Dictionary<int, FCommand> mCmdDictionary;
	}
}
