using System;

namespace MS.Internal.AppModel
{
	// Token: 0x020007C0 RID: 1984
	[Serializable]
	internal struct SubStream
	{
		// Token: 0x06007B5D RID: 31581 RVA: 0x0022B246 File Offset: 0x00229446
		internal SubStream(string propertyName, byte[] dataBytes)
		{
			this._propertyName = propertyName;
			this._data = dataBytes;
		}

		// Token: 0x04003A15 RID: 14869
		internal string _propertyName;

		// Token: 0x04003A16 RID: 14870
		internal byte[] _data;
	}
}
