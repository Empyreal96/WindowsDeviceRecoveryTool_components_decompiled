using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020005F7 RID: 1527
	internal static class TracePageFormatting
	{
		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x0600657F RID: 25983 RVA: 0x001C7660 File Offset: 0x001C5860
		public static AvTraceDetails FormatPage
		{
			get
			{
				if (TracePageFormatting._FormatPage == null)
				{
					TracePageFormatting._FormatPage = new AvTraceDetails(1, new string[]
					{
						"Formatting page",
						"PageContext",
						"PtsContext"
					});
				}
				return TracePageFormatting._FormatPage;
			}
		}

		// Token: 0x1700184F RID: 6223
		// (get) Token: 0x06006580 RID: 25984 RVA: 0x001C7697 File Offset: 0x001C5897
		public static AvTraceDetails PageFormattingError
		{
			get
			{
				if (TracePageFormatting._PageFormattingError == null)
				{
					TracePageFormatting._PageFormattingError = new AvTraceDetails(2, new string[]
					{
						"Error. Page formatting engine could not complete the formatting operation.",
						"PtsContext",
						"Message"
					});
				}
				return TracePageFormatting._PageFormattingError;
			}
		}

		// Token: 0x06006581 RID: 25985 RVA: 0x001C76CE File Offset: 0x001C58CE
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06006582 RID: 25986 RVA: 0x001C76EE File Offset: 0x001C58EE
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06006583 RID: 25987 RVA: 0x001C7714 File Offset: 0x001C5914
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06006584 RID: 25988 RVA: 0x001C7748 File Offset: 0x001C5948
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06006585 RID: 25989 RVA: 0x001C7780 File Offset: 0x001C5980
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TracePageFormatting._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06006586 RID: 25990 RVA: 0x001C77BD File Offset: 0x001C59BD
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06006587 RID: 25991 RVA: 0x001C77DC File Offset: 0x001C59DC
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06006588 RID: 25992 RVA: 0x001C7800 File Offset: 0x001C5A00
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06006589 RID: 25993 RVA: 0x001C7828 File Offset: 0x001C5A28
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x0600658A RID: 25994 RVA: 0x001C7854 File Offset: 0x001C5A54
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TracePageFormatting._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x17001850 RID: 6224
		// (get) Token: 0x0600658B RID: 25995 RVA: 0x001C7884 File Offset: 0x001C5A84
		public static bool IsEnabled
		{
			get
			{
				return TracePageFormatting._avTrace != null && TracePageFormatting._avTrace.IsEnabled;
			}
		}

		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x0600658C RID: 25996 RVA: 0x001C7899 File Offset: 0x001C5A99
		public static bool IsEnabledOverride
		{
			get
			{
				return TracePageFormatting._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x0600658D RID: 25997 RVA: 0x001C78A5 File Offset: 0x001C5AA5
		public static void Refresh()
		{
			TracePageFormatting._avTrace.Refresh();
		}

		// Token: 0x040032CA RID: 13002
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.DocumentsSource, delegate()
		{
			PresentationTraceSources._DocumentsSource = null;
		});

		// Token: 0x040032CB RID: 13003
		private static AvTraceDetails _FormatPage;

		// Token: 0x040032CC RID: 13004
		private static AvTraceDetails _PageFormattingError;
	}
}
