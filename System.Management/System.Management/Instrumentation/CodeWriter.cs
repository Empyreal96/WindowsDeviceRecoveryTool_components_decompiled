using System;
using System.Collections;
using System.Globalization;
using System.IO;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B1 RID: 177
	internal class CodeWriter
	{
		// Token: 0x060004A0 RID: 1184 RVA: 0x00022553 File Offset: 0x00020753
		public static explicit operator string(CodeWriter writer)
		{
			return writer.ToString();
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0002255C File Offset: 0x0002075C
		public override string ToString()
		{
			StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
			this.WriteCode(stringWriter);
			string result = stringWriter.ToString();
			stringWriter.Close();
			return result;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0002258C File Offset: 0x0002078C
		private void WriteCode(TextWriter writer)
		{
			string value = new string(' ', this.depth * 4);
			foreach (object obj in this.children)
			{
				if (obj == null)
				{
					writer.WriteLine();
				}
				else if (obj is string)
				{
					writer.Write(value);
					writer.WriteLine(obj);
				}
				else
				{
					((CodeWriter)obj).WriteCode(writer);
				}
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00022618 File Offset: 0x00020818
		public CodeWriter AddChild(string name)
		{
			this.Line(name);
			this.Line("{");
			CodeWriter codeWriter = new CodeWriter();
			codeWriter.depth = this.depth + 1;
			this.children.Add(codeWriter);
			this.Line("}");
			return codeWriter;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00022664 File Offset: 0x00020864
		public CodeWriter AddChild(params string[] parts)
		{
			return this.AddChild(string.Concat(parts));
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00022674 File Offset: 0x00020874
		public CodeWriter AddChildNoIndent(string name)
		{
			this.Line(name);
			CodeWriter codeWriter = new CodeWriter();
			codeWriter.depth = this.depth + 1;
			this.children.Add(codeWriter);
			return codeWriter;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x000226AA File Offset: 0x000208AA
		public CodeWriter AddChild(CodeWriter snippet)
		{
			snippet.depth = this.depth;
			this.children.Add(snippet);
			return snippet;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x000226C6 File Offset: 0x000208C6
		public void Line(string line)
		{
			this.children.Add(line);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x000226D5 File Offset: 0x000208D5
		public void Line(params string[] parts)
		{
			this.Line(string.Concat(parts));
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x000226E3 File Offset: 0x000208E3
		public void Line()
		{
			this.children.Add(null);
		}

		// Token: 0x040004EA RID: 1258
		private int depth;

		// Token: 0x040004EB RID: 1259
		private ArrayList children = new ArrayList();
	}
}
