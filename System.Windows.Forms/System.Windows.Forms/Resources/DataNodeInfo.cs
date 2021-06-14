using System;
using System.Drawing;

namespace System.Resources
{
	// Token: 0x020000EA RID: 234
	internal class DataNodeInfo
	{
		// Token: 0x0600034C RID: 844 RVA: 0x00009D8C File Offset: 0x00007F8C
		internal DataNodeInfo Clone()
		{
			return new DataNodeInfo
			{
				Name = this.Name,
				Comment = this.Comment,
				TypeName = this.TypeName,
				MimeType = this.MimeType,
				ValueData = this.ValueData,
				ReaderPosition = new Point(this.ReaderPosition.X, this.ReaderPosition.Y)
			};
		}

		// Token: 0x040003B5 RID: 949
		internal string Name;

		// Token: 0x040003B6 RID: 950
		internal string Comment;

		// Token: 0x040003B7 RID: 951
		internal string TypeName;

		// Token: 0x040003B8 RID: 952
		internal string MimeType;

		// Token: 0x040003B9 RID: 953
		internal string ValueData;

		// Token: 0x040003BA RID: 954
		internal Point ReaderPosition;
	}
}
