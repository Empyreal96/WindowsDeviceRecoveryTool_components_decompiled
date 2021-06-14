using System;

namespace System.Windows.Markup
{
	// Token: 0x020001C6 RID: 454
	internal class DefAttributeData
	{
		// Token: 0x06001D0F RID: 7439 RVA: 0x00087A94 File Offset: 0x00085C94
		internal DefAttributeData(string targetAssemblyName, string targetFullName, Type targetType, string args, Type declaringType, string targetNamespaceUri, int lineNumber, int linePosition, int depth, bool isSimple)
		{
			this.TargetType = targetType;
			this.DeclaringType = declaringType;
			this.TargetFullName = targetFullName;
			this.TargetAssemblyName = targetAssemblyName;
			this.Args = args;
			this.TargetNamespaceUri = targetNamespaceUri;
			this.LineNumber = lineNumber;
			this.LinePosition = linePosition;
			this.Depth = depth;
			this.IsSimple = isSimple;
		}

		// Token: 0x170006D5 RID: 1749
		// (get) Token: 0x06001D10 RID: 7440 RVA: 0x00087AF4 File Offset: 0x00085CF4
		internal bool IsUnknownExtension
		{
			get
			{
				return this.TargetType == typeof(MarkupExtensionParser.UnknownMarkupExtension);
			}
		}

		// Token: 0x0400140C RID: 5132
		internal Type TargetType;

		// Token: 0x0400140D RID: 5133
		internal Type DeclaringType;

		// Token: 0x0400140E RID: 5134
		internal string TargetFullName;

		// Token: 0x0400140F RID: 5135
		internal string TargetAssemblyName;

		// Token: 0x04001410 RID: 5136
		internal string Args;

		// Token: 0x04001411 RID: 5137
		internal string TargetNamespaceUri;

		// Token: 0x04001412 RID: 5138
		internal int LineNumber;

		// Token: 0x04001413 RID: 5139
		internal int LinePosition;

		// Token: 0x04001414 RID: 5140
		internal int Depth;

		// Token: 0x04001415 RID: 5141
		internal bool IsSimple;
	}
}
