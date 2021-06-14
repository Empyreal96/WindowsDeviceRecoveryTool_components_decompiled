using System;
using System.Xaml;

namespace System.Windows.Baml2006
{
	// Token: 0x0200015C RID: 348
	internal class StaticResource
	{
		// Token: 0x06000F99 RID: 3993 RVA: 0x0003C9BE File Offset: 0x0003ABBE
		public StaticResource(XamlType type, XamlSchemaContext schemaContext)
		{
			this.ResourceNodeList = new XamlNodeList(schemaContext, 8);
			this.ResourceNodeList.Writer.WriteStartObject(type);
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06000F9A RID: 3994 RVA: 0x0003C9E4 File Offset: 0x0003ABE4
		// (set) Token: 0x06000F9B RID: 3995 RVA: 0x0003C9EC File Offset: 0x0003ABEC
		public XamlNodeList ResourceNodeList { get; private set; }
	}
}
