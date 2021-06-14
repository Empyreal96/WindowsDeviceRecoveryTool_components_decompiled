using System;
using System.Diagnostics;

namespace MS.Internal
{
	// Token: 0x020005E0 RID: 1504
	internal static class TraceHwndHost
	{
		// Token: 0x06006452 RID: 25682 RVA: 0x001C2959 File Offset: 0x001C0B59
		static TraceHwndHost()
		{
			TraceHwndHost._avTrace.EnabledByDebugger = true;
		}

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x06006453 RID: 25683 RVA: 0x001C2990 File Offset: 0x001C0B90
		public static AvTraceDetails HwndHostIn3D
		{
			get
			{
				if (TraceHwndHost._HwndHostIn3D == null)
				{
					TraceHwndHost._HwndHostIn3D = new AvTraceDetails(1, new string[]
					{
						"An HwndHost may not be embedded in a 3D scene."
					});
				}
				return TraceHwndHost._HwndHostIn3D;
			}
		}

		// Token: 0x06006454 RID: 25684 RVA: 0x001C29B7 File Offset: 0x001C0BB7
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x06006455 RID: 25685 RVA: 0x001C29D7 File Offset: 0x001C0BD7
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x06006456 RID: 25686 RVA: 0x001C29FC File Offset: 0x001C0BFC
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x06006457 RID: 25687 RVA: 0x001C2A30 File Offset: 0x001C0C30
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x06006458 RID: 25688 RVA: 0x001C2A68 File Offset: 0x001C0C68
		public static void Trace(TraceEventType type, AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceHwndHost._avTrace.Trace(type, traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x06006459 RID: 25689 RVA: 0x001C2AA5 File Offset: 0x001C0CA5
		public static void TraceActivityItem(AvTraceDetails traceDetails, params object[] parameters)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, parameters);
		}

		// Token: 0x0600645A RID: 25690 RVA: 0x001C2AC4 File Offset: 0x001C0CC4
		public static void TraceActivityItem(AvTraceDetails traceDetails)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[0]);
		}

		// Token: 0x0600645B RID: 25691 RVA: 0x001C2AE8 File Offset: 0x001C0CE8
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1
			});
		}

		// Token: 0x0600645C RID: 25692 RVA: 0x001C2B10 File Offset: 0x001C0D10
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2
			});
		}

		// Token: 0x0600645D RID: 25693 RVA: 0x001C2B3C File Offset: 0x001C0D3C
		public static void TraceActivityItem(AvTraceDetails traceDetails, object p1, object p2, object p3)
		{
			TraceHwndHost._avTrace.TraceStartStop(traceDetails.Id, traceDetails.Message, traceDetails.Labels, new object[]
			{
				p1,
				p2,
				p3
			});
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x0600645E RID: 25694 RVA: 0x001C2B6C File Offset: 0x001C0D6C
		public static bool IsEnabled
		{
			get
			{
				return TraceHwndHost._avTrace != null && TraceHwndHost._avTrace.IsEnabled;
			}
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x0600645F RID: 25695 RVA: 0x001C2B81 File Offset: 0x001C0D81
		public static bool IsEnabledOverride
		{
			get
			{
				return TraceHwndHost._avTrace.IsEnabledOverride;
			}
		}

		// Token: 0x06006460 RID: 25696 RVA: 0x001C2B8D File Offset: 0x001C0D8D
		public static void Refresh()
		{
			TraceHwndHost._avTrace.Refresh();
		}

		// Token: 0x0400329C RID: 12956
		private static AvTrace _avTrace = new AvTrace(() => PresentationTraceSources.HwndHostSource, delegate()
		{
			PresentationTraceSources._HwndHostSource = null;
		});

		// Token: 0x0400329D RID: 12957
		private static AvTraceDetails _HwndHostIn3D;
	}
}
