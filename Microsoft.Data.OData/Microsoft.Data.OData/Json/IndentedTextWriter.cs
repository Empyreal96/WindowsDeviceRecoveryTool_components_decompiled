using System;
using System.IO;
using System.Text;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x020002A0 RID: 672
	internal sealed class IndentedTextWriter : TextWriter
	{
		// Token: 0x0600169C RID: 5788 RVA: 0x00052401 File Offset: 0x00050601
		public IndentedTextWriter(TextWriter writer, bool enableIndentation) : base(writer.FormatProvider)
		{
			this.writer = writer;
			this.enableIndentation = enableIndentation;
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x0600169D RID: 5789 RVA: 0x0005241D File Offset: 0x0005061D
		public override Encoding Encoding
		{
			get
			{
				return this.writer.Encoding;
			}
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x0600169E RID: 5790 RVA: 0x0005242A File Offset: 0x0005062A
		public override string NewLine
		{
			get
			{
				return this.writer.NewLine;
			}
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00052437 File Offset: 0x00050637
		public void IncreaseIndentation()
		{
			this.indentLevel++;
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x00052447 File Offset: 0x00050647
		public void DecreaseIndentation()
		{
			if (this.indentLevel < 1)
			{
				this.indentLevel = 0;
				return;
			}
			this.indentLevel--;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00052468 File Offset: 0x00050668
		public override void Close()
		{
			IndentedTextWriter.InternalCloseOrDispose();
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x0005246F File Offset: 0x0005066F
		public override void Flush()
		{
			this.writer.Flush();
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x0005247C File Offset: 0x0005067C
		public override void Write(string s)
		{
			this.WriteIndentation();
			this.writer.Write(s);
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00052490 File Offset: 0x00050690
		public override void Write(char value)
		{
			this.WriteIndentation();
			this.writer.Write(value);
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x000524A4 File Offset: 0x000506A4
		public override void WriteLine()
		{
			if (this.enableIndentation)
			{
				base.WriteLine();
			}
			this.indentationPending = true;
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x000524BB File Offset: 0x000506BB
		private static void InternalCloseOrDispose()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x000524C4 File Offset: 0x000506C4
		private void WriteIndentation()
		{
			if (!this.enableIndentation || !this.indentationPending)
			{
				return;
			}
			for (int i = 0; i < this.indentLevel; i++)
			{
				this.writer.Write("  ");
			}
			this.indentationPending = false;
		}

		// Token: 0x0400092D RID: 2349
		private const string IndentationString = "  ";

		// Token: 0x0400092E RID: 2350
		private readonly TextWriter writer;

		// Token: 0x0400092F RID: 2351
		private readonly bool enableIndentation;

		// Token: 0x04000930 RID: 2352
		private int indentLevel;

		// Token: 0x04000931 RID: 2353
		private bool indentationPending;
	}
}
