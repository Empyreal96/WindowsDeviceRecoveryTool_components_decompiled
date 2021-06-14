using System;

namespace System.Windows
{
	// Token: 0x020000CA RID: 202
	internal class ReadOnlyFrameworkPropertyMetadata : FrameworkPropertyMetadata
	{
		// Token: 0x060006CC RID: 1740 RVA: 0x0001585E File Offset: 0x00013A5E
		public ReadOnlyFrameworkPropertyMetadata(object defaultValue, GetReadOnlyValueCallback getValueCallback) : base(defaultValue)
		{
			this._getValueCallback = getValueCallback;
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006CD RID: 1741 RVA: 0x0001586E File Offset: 0x00013A6E
		internal override GetReadOnlyValueCallback GetReadOnlyValueCallback
		{
			get
			{
				return this._getValueCallback;
			}
		}

		// Token: 0x040006F5 RID: 1781
		private GetReadOnlyValueCallback _getValueCallback;
	}
}
