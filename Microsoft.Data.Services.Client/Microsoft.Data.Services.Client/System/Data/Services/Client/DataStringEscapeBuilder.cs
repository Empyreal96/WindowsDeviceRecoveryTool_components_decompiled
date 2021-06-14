using System;
using System.Text;

namespace System.Data.Services.Client
{
	// Token: 0x0200001C RID: 28
	internal class DataStringEscapeBuilder
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00004294 File Offset: 0x00002494
		private DataStringEscapeBuilder(string dataString)
		{
			this.input = dataString;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x000042B0 File Offset: 0x000024B0
		internal static string EscapeDataString(string input)
		{
			DataStringEscapeBuilder dataStringEscapeBuilder = new DataStringEscapeBuilder(input);
			return dataStringEscapeBuilder.Build();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000042CC File Offset: 0x000024CC
		private string Build()
		{
			this.index = 0;
			while (this.index < this.input.Length)
			{
				char c = this.input[this.index];
				if (c == '\'' || c == '"')
				{
					this.ReadQuotedString(c);
				}
				else if ("+".IndexOf(c) >= 0)
				{
					this.output.Append(Uri.EscapeDataString(c.ToString()));
				}
				else
				{
					this.output.Append(c);
				}
				this.index++;
			}
			return this.output.ToString();
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00004368 File Offset: 0x00002568
		private void ReadQuotedString(char quoteStart)
		{
			if (this.quotedDataBuilder == null)
			{
				this.quotedDataBuilder = new StringBuilder();
			}
			this.output.Append(quoteStart);
			while (++this.index < this.input.Length)
			{
				if (this.input[this.index] == quoteStart)
				{
					this.output.Append(Uri.EscapeDataString(this.quotedDataBuilder.ToString()));
					this.output.Append(quoteStart);
					this.quotedDataBuilder.Clear();
					break;
				}
				this.quotedDataBuilder.Append(this.input[this.index]);
			}
			if (this.quotedDataBuilder.Length > 0)
			{
				this.output.Append(Uri.EscapeDataString(this.quotedDataBuilder.ToString()));
				this.quotedDataBuilder.Clear();
			}
		}

		// Token: 0x0400017E RID: 382
		private const string SensitiveCharacters = "+";

		// Token: 0x0400017F RID: 383
		private readonly string input;

		// Token: 0x04000180 RID: 384
		private readonly StringBuilder output = new StringBuilder();

		// Token: 0x04000181 RID: 385
		private int index;

		// Token: 0x04000182 RID: 386
		private StringBuilder quotedDataBuilder;
	}
}
