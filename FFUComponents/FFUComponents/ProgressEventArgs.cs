using System;

namespace FFUComponents
{
	// Token: 0x0200004A RID: 74
	public class ProgressEventArgs : EventArgs
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000101 RID: 257 RVA: 0x00004792 File Offset: 0x00002992
		// (set) Token: 0x06000102 RID: 258 RVA: 0x0000479A File Offset: 0x0000299A
		public IFFUDevice Device { get; private set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000103 RID: 259 RVA: 0x000047A3 File Offset: 0x000029A3
		// (set) Token: 0x06000104 RID: 260 RVA: 0x000047AB File Offset: 0x000029AB
		public long Position { get; private set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000105 RID: 261 RVA: 0x000047B4 File Offset: 0x000029B4
		// (set) Token: 0x06000106 RID: 262 RVA: 0x000047BC File Offset: 0x000029BC
		public long Length { get; private set; }

		// Token: 0x06000107 RID: 263 RVA: 0x000047C5 File Offset: 0x000029C5
		public ProgressEventArgs(IFFUDevice device, long pos, long len)
		{
			this.Device = device;
			this.Position = pos;
			this.Length = len;
		}
	}
}
