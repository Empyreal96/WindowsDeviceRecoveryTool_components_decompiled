using System;

namespace MS.Internal.Controls.StickyNote
{
	// Token: 0x02000766 RID: 1894
	[Flags]
	internal enum XmlToken
	{
		// Token: 0x040038F4 RID: 14580
		MetaData = 1,
		// Token: 0x040038F5 RID: 14581
		Left = 4,
		// Token: 0x040038F6 RID: 14582
		Top = 8,
		// Token: 0x040038F7 RID: 14583
		XOffset = 16,
		// Token: 0x040038F8 RID: 14584
		YOffset = 32,
		// Token: 0x040038F9 RID: 14585
		Width = 128,
		// Token: 0x040038FA RID: 14586
		Height = 256,
		// Token: 0x040038FB RID: 14587
		IsExpanded = 512,
		// Token: 0x040038FC RID: 14588
		Author = 1024,
		// Token: 0x040038FD RID: 14589
		Text = 8192,
		// Token: 0x040038FE RID: 14590
		Ink = 32768,
		// Token: 0x040038FF RID: 14591
		ZOrder = 131072
	}
}
