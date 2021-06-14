using System;

namespace System.Data.Services.Client
{
	// Token: 0x020000AF RID: 175
	internal class TextPrimitiveParserToken : PrimitiveParserToken
	{
		// Token: 0x060005A2 RID: 1442 RVA: 0x0001593A File Offset: 0x00013B3A
		internal TextPrimitiveParserToken(string text)
		{
			this.Text = text;
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x00015949 File Offset: 0x00013B49
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x00015951 File Offset: 0x00013B51
		internal string Text { get; private set; }

		// Token: 0x060005A5 RID: 1445 RVA: 0x0001595A File Offset: 0x00013B5A
		internal override object Materialize(Type clrType)
		{
			return ClientConvert.ChangeType(this.Text, clrType);
		}
	}
}
