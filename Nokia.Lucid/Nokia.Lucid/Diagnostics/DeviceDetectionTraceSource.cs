using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;
using Nokia.Lucid.DeviceDetection;

namespace Nokia.Lucid.Diagnostics
{
	// Token: 0x0200001F RID: 31
	internal sealed class DeviceDetectionTraceSource : TraceSource
	{
		// Token: 0x060000EC RID: 236 RVA: 0x00009DA2 File Offset: 0x00007FA2
		private DeviceDetectionTraceSource() : base("Nokia.Lucid.DeviceDetection")
		{
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00009DB0 File Offset: 0x00007FB0
		public void FilterExpressionCompilation_Start(Expression filter)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Compiling filter expression.\r\nExpression:\r\n{0}", new object[]
			{
				filter
			});
			this.TraceEvent(TraceEventType.Start, DeviceDetectionTraceEventId.FilterExpressionEvaluation, messageText);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00009DE8 File Offset: 0x00007FE8
		public void FilterExpressionCompilation_Stop(Expression filter)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Compiled filter expression.\r\nExpression:\r\n{0}", new object[]
			{
				filter
			});
			this.TraceEvent(TraceEventType.Stop, DeviceDetectionTraceEventId.FilterExpressionEvaluation, messageText);
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00009E20 File Offset: 0x00008020
		public void FilterExpressionCompilation_Error(Expression filter, Exception exception)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Error while compiling filter expression.\r\nExpression:\r\n{0}\r\nException:\r\n{1}", new object[]
			{
				filter,
				exception
			});
			this.TraceEvent(TraceEventType.Error, DeviceDetectionTraceEventId.FilterExpressionEvaluation, messageText);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00009E58 File Offset: 0x00008058
		public void FilterExpressionEvaluation_Start(string devicePath)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Evaluating filter expression for device path.\r\nPath: {0}", new object[]
			{
				devicePath
			});
			this.TraceEvent(TraceEventType.Start, DeviceDetectionTraceEventId.FilterExpressionEvaluation, messageText);
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00009E90 File Offset: 0x00008090
		public void FilterExpressionEvaluation_Stop(string devicePath, bool matched)
		{
			string messageText;
			if (matched)
			{
				messageText = string.Format(CultureInfo.InvariantCulture, "Device path matches filter expression.\r\nPath: {0}", new object[]
				{
					devicePath
				});
			}
			else
			{
				messageText = string.Format(CultureInfo.InvariantCulture, "Device path does not match filter expression, rejecting device notification.\r\nPath: {0}", new object[]
				{
					devicePath
				});
			}
			this.TraceEvent(TraceEventType.Stop, DeviceDetectionTraceEventId.FilterExpressionEvaluation, messageText);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00009EE8 File Offset: 0x000080E8
		public void FilterExpressionEvaluation_Error(string devicePath, Exception exception)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Error while evaluating filter expression for device path.\r\nPath: {0}\r\nException:\r\n{1}", new object[]
			{
				devicePath,
				exception
			});
			this.TraceEvent(TraceEventType.Error, DeviceDetectionTraceEventId.FilterExpressionEvaluation, messageText);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00009F20 File Offset: 0x00008120
		public void DeviceChangeEvent_Start(DeviceChangeAction action, string devicePath, DeviceType deviceType)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Raising device change event.\r\nAction: {0}\r\nPath: {1}\r\nType: {2}", new object[]
			{
				action,
				devicePath,
				deviceType
			});
			this.TraceEvent(TraceEventType.Start, DeviceDetectionTraceEventId.DeviceChangeEvent, messageText);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00009F68 File Offset: 0x00008168
		public void DeviceChangeEvent_Stop(DeviceChangeAction action, string devicePath, DeviceType deviceType)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Raised device change event.\r\nAction: {0}\r\nPath: {1}\r\nType: {2}", new object[]
			{
				action,
				devicePath,
				deviceType
			});
			this.TraceEvent(TraceEventType.Stop, DeviceDetectionTraceEventId.DeviceChangeEvent, messageText);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00009FB0 File Offset: 0x000081B0
		public void DeviceChangeEvent_Error(DeviceChangeAction action, string devicePath, DeviceType deviceType, Exception exception)
		{
			string messageText = string.Format(CultureInfo.InvariantCulture, "Error while raising device change event.\r\nAction: {0}\r\nPath: {1}\r\nType: {2}\r\nException:\r\n{3}", new object[]
			{
				action,
				devicePath,
				deviceType,
				exception
			});
			this.TraceEvent(TraceEventType.Error, DeviceDetectionTraceEventId.DeviceChangeEvent, messageText);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00009FFC File Offset: 0x000081FC
		public void InvalidDeviceMapping(Guid classGuid, DeviceType deviceType)
		{
			string str;
			string text = KnownNames.TryGetInterfaceClassName(classGuid, out str) ? (" (" + str + ")") : string.Empty;
			string messageText = string.Format(CultureInfo.InvariantCulture, "Invalid device interface class mapping.\r\nClass: {0}{1}\r\nType: {2}", new object[]
			{
				classGuid,
				text,
				deviceType
			});
			this.TraceEvent(TraceEventType.Warning, DeviceDetectionTraceEventId.InvalidDeviceMapping, messageText);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000A062 File Offset: 0x00008262
		private void TraceEvent(TraceEventType eventType, DeviceDetectionTraceEventId id, string messageText)
		{
			base.TraceEvent(eventType, (int)id, messageText);
		}

		// Token: 0x04000084 RID: 132
		private const string TraceSourceName = "Nokia.Lucid.DeviceDetection";

		// Token: 0x04000085 RID: 133
		public static readonly DeviceDetectionTraceSource Instance = new DeviceDetectionTraceSource();
	}
}
