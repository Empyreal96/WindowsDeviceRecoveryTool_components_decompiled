using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x020000C8 RID: 200
	public class MemoryTraceWriter : ITraceWriter
	{
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x00027E5A File Offset: 0x0002605A
		// (set) Token: 0x06000A06 RID: 2566 RVA: 0x00027E62 File Offset: 0x00026062
		public TraceLevel LevelFilter { get; set; }

		// Token: 0x06000A07 RID: 2567 RVA: 0x00027E6B File Offset: 0x0002606B
		public MemoryTraceWriter()
		{
			this.LevelFilter = TraceLevel.Verbose;
			this._traceMessages = new Queue<string>();
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00027E88 File Offset: 0x00026088
		public void Trace(TraceLevel level, string message, Exception ex)
		{
			string item = string.Concat(new string[]
			{
				DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", CultureInfo.InvariantCulture),
				" ",
				level.ToString("g"),
				" ",
				message
			});
			if (this._traceMessages.Count >= 1000)
			{
				this._traceMessages.Dequeue();
			}
			this._traceMessages.Enqueue(item);
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00027F0E File Offset: 0x0002610E
		public IEnumerable<string> GetTraceMessages()
		{
			return this._traceMessages;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00027F18 File Offset: 0x00026118
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in this._traceMessages)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(value);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400036C RID: 876
		private readonly Queue<string> _traceMessages;
	}
}
