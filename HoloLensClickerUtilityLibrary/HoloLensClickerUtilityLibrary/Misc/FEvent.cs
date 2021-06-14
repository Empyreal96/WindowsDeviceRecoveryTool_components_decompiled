using System;

namespace ClickerUtilityLibrary.Misc
{
	// Token: 0x0200000B RID: 11
	public class FEvent : EventArgs
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00004969 File Offset: 0x00002B69
		public FEvent(EventType eventType)
		{
			this.EventType = eventType;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0000497B File Offset: 0x00002B7B
		public FEvent(EventType eventType, string parameter)
		{
			this.EventType = eventType;
			this.StringParameter = parameter;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600004F RID: 79 RVA: 0x00004995 File Offset: 0x00002B95
		// (set) Token: 0x06000050 RID: 80 RVA: 0x0000499D File Offset: 0x00002B9D
		public EventType EventType { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000051 RID: 81 RVA: 0x000049A6 File Offset: 0x00002BA6
		// (set) Token: 0x06000052 RID: 82 RVA: 0x000049AE File Offset: 0x00002BAE
		public string StringParameter { get; set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000053 RID: 83 RVA: 0x000049B7 File Offset: 0x00002BB7
		// (set) Token: 0x06000054 RID: 84 RVA: 0x000049BF File Offset: 0x00002BBF
		public object ObjectParameter { get; set; }
	}
}
