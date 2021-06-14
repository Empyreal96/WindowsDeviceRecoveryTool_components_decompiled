using System;

namespace System.Windows.Documents
{
	// Token: 0x02000405 RID: 1029
	[Flags]
	internal enum FindFlags
	{
		// Token: 0x040025BA RID: 9658
		None = 0,
		// Token: 0x040025BB RID: 9659
		MatchCase = 1,
		// Token: 0x040025BC RID: 9660
		FindInReverse = 2,
		// Token: 0x040025BD RID: 9661
		FindWholeWordsOnly = 4,
		// Token: 0x040025BE RID: 9662
		MatchDiacritics = 8,
		// Token: 0x040025BF RID: 9663
		MatchKashida = 16,
		// Token: 0x040025C0 RID: 9664
		MatchAlefHamza = 32
	}
}
