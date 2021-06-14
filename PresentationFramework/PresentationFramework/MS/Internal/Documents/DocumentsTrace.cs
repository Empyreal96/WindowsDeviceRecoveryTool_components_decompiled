using System;
using System.Diagnostics;
using System.Security;

namespace MS.Internal.Documents
{
	// Token: 0x020006C1 RID: 1729
	internal sealed class DocumentsTrace
	{
		// Token: 0x06006FC6 RID: 28614 RVA: 0x0000326D File Offset: 0x0000146D
		public DocumentsTrace(string switchName)
		{
		}

		// Token: 0x06006FC7 RID: 28615 RVA: 0x002021D5 File Offset: 0x002003D5
		[SecurityCritical]
		[SecurityTreatAsSafe]
		public DocumentsTrace(string switchName, bool initialState) : this(switchName)
		{
		}

		// Token: 0x06006FC8 RID: 28616 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		public void Trace(string message)
		{
		}

		// Token: 0x06006FC9 RID: 28617 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		public void TraceCallers(int Depth)
		{
		}

		// Token: 0x06006FCA RID: 28618 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		public void Indent()
		{
		}

		// Token: 0x06006FCB RID: 28619 RVA: 0x00002137 File Offset: 0x00000337
		[Conditional("DEBUG")]
		public void Unindent()
		{
		}

		// Token: 0x06006FCC RID: 28620 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		[SecurityTreatAsSafe]
		[Conditional("DEBUG")]
		public void Enable()
		{
		}

		// Token: 0x06006FCD RID: 28621 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		[SecurityTreatAsSafe]
		[Conditional("DEBUG")]
		public void Disable()
		{
		}

		// Token: 0x17001A8B RID: 6795
		// (get) Token: 0x06006FCE RID: 28622 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool IsEnabled
		{
			get
			{
				return false;
			}
		}

		// Token: 0x02000B36 RID: 2870
		internal static class FixedFormat
		{
			// Token: 0x04004A9C RID: 19100
			public static DocumentsTrace FixedDocument;

			// Token: 0x04004A9D RID: 19101
			public static DocumentsTrace PageContent;

			// Token: 0x04004A9E RID: 19102
			public static DocumentsTrace IDF;
		}

		// Token: 0x02000B37 RID: 2871
		internal static class FixedTextOM
		{
			// Token: 0x04004A9F RID: 19103
			public static DocumentsTrace TextView;

			// Token: 0x04004AA0 RID: 19104
			public static DocumentsTrace TextContainer;

			// Token: 0x04004AA1 RID: 19105
			public static DocumentsTrace Map;

			// Token: 0x04004AA2 RID: 19106
			public static DocumentsTrace Highlight;

			// Token: 0x04004AA3 RID: 19107
			public static DocumentsTrace Builder;

			// Token: 0x04004AA4 RID: 19108
			public static DocumentsTrace FlowPosition;
		}

		// Token: 0x02000B38 RID: 2872
		internal static class FixedDocumentSequence
		{
			// Token: 0x04004AA5 RID: 19109
			public static DocumentsTrace Content;

			// Token: 0x04004AA6 RID: 19110
			public static DocumentsTrace IDF;

			// Token: 0x04004AA7 RID: 19111
			public static DocumentsTrace TextOM;

			// Token: 0x04004AA8 RID: 19112
			public static DocumentsTrace Highlights;
		}
	}
}
