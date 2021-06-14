using System;

namespace Microsoft.Data.Spatial
{
	// Token: 0x0200007D RID: 125
	internal class LexerToken
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000086D2 File Offset: 0x000068D2
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x000086DA File Offset: 0x000068DA
		public string Text { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x000086E3 File Offset: 0x000068E3
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x000086EB File Offset: 0x000068EB
		public int Type { get; set; }

		// Token: 0x060002FA RID: 762 RVA: 0x000086F4 File Offset: 0x000068F4
		public bool MatchToken(int targetType, string targetText, StringComparison comparison)
		{
			return this.Type == targetType && (string.IsNullOrEmpty(targetText) || this.Text.Equals(targetText, comparison));
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00008718 File Offset: 0x00006918
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Type:[",
				this.Type,
				"] Text:[",
				this.Text,
				"]"
			});
		}
	}
}
