using System;
using System.Collections.Generic;
using ClickerUtilityLibrary.Misc;

namespace ClickerUtilityLibrary.DataModel
{
	// Token: 0x02000011 RID: 17
	public class FCommand
	{
		// Token: 0x0600005C RID: 92 RVA: 0x00004B66 File Offset: 0x00002D66
		public FCommand(int commandCode, string name, List<DataElement> args, List<DataElement> responseArgs)
		{
			this.CommandCode = commandCode;
			this.Name = name;
			this.Args = args;
			this.ResponseArgs = responseArgs;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600005D RID: 93 RVA: 0x00004B91 File Offset: 0x00002D91
		// (set) Token: 0x0600005E RID: 94 RVA: 0x00004B99 File Offset: 0x00002D99
		public List<DataElement> ResponseArgs { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00004BA2 File Offset: 0x00002DA2
		// (set) Token: 0x06000060 RID: 96 RVA: 0x00004BAA File Offset: 0x00002DAA
		public List<DataElement> Args { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000061 RID: 97 RVA: 0x00004BB3 File Offset: 0x00002DB3
		// (set) Token: 0x06000062 RID: 98 RVA: 0x00004BBB File Offset: 0x00002DBB
		public string Name { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000063 RID: 99 RVA: 0x00004BC4 File Offset: 0x00002DC4
		// (set) Token: 0x06000064 RID: 100 RVA: 0x00004BCC File Offset: 0x00002DCC
		public int CommandCode { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000065 RID: 101 RVA: 0x00004BD5 File Offset: 0x00002DD5
		// (set) Token: 0x06000066 RID: 102 RVA: 0x00004BDD File Offset: 0x00002DDD
		public FStatus Status { get; set; }
	}
}
