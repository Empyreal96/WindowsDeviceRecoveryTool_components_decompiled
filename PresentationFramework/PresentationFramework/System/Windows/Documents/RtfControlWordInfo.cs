using System;

namespace System.Windows.Documents
{
	// Token: 0x020003A7 RID: 935
	internal class RtfControlWordInfo
	{
		// Token: 0x06003296 RID: 12950 RVA: 0x000E3C0D File Offset: 0x000E1E0D
		internal RtfControlWordInfo(RtfControlWord controlWord, string controlName, uint flags)
		{
			this._controlWord = controlWord;
			this._controlName = controlName;
			this._flags = flags;
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x06003297 RID: 12951 RVA: 0x000E3C2A File Offset: 0x000E1E2A
		internal RtfControlWord Control
		{
			get
			{
				return this._controlWord;
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x06003298 RID: 12952 RVA: 0x000E3C32 File Offset: 0x000E1E32
		internal string ControlName
		{
			get
			{
				return this._controlName;
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x06003299 RID: 12953 RVA: 0x000E3C3A File Offset: 0x000E1E3A
		internal uint Flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x040023A1 RID: 9121
		private RtfControlWord _controlWord;

		// Token: 0x040023A2 RID: 9122
		private string _controlName;

		// Token: 0x040023A3 RID: 9123
		private uint _flags;
	}
}
