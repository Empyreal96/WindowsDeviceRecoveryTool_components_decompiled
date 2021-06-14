using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x020002A2 RID: 674
	internal sealed class JsonWriter : IJsonWriter
	{
		// Token: 0x060016A8 RID: 5800 RVA: 0x0005250A File Offset: 0x0005070A
		internal JsonWriter(TextWriter writer, bool indent, ODataFormat jsonFormat)
		{
			this.writer = new IndentedTextWriter(writer, indent);
			this.scopes = new Stack<JsonWriter.Scope>();
			this.mustWriteDecimalPointInDoubleValues = (jsonFormat == ODataFormat.Json);
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x00052538 File Offset: 0x00050738
		public void StartPaddingFunctionScope()
		{
			this.StartScope(JsonWriter.ScopeType.Padding);
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x00052544 File Offset: 0x00050744
		public void EndPaddingFunctionScope()
		{
			this.writer.WriteLine();
			this.writer.DecreaseIndentation();
			JsonWriter.Scope scope = this.scopes.Pop();
			this.writer.Write(scope.EndString);
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00052584 File Offset: 0x00050784
		public void StartObjectScope()
		{
			this.StartScope(JsonWriter.ScopeType.Object);
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x00052590 File Offset: 0x00050790
		public void EndObjectScope()
		{
			this.writer.WriteLine();
			this.writer.DecreaseIndentation();
			JsonWriter.Scope scope = this.scopes.Pop();
			this.writer.Write(scope.EndString);
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x000525D0 File Offset: 0x000507D0
		public void StartArrayScope()
		{
			this.StartScope(JsonWriter.ScopeType.Array);
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x000525DC File Offset: 0x000507DC
		public void EndArrayScope()
		{
			this.writer.WriteLine();
			this.writer.DecreaseIndentation();
			JsonWriter.Scope scope = this.scopes.Pop();
			this.writer.Write(scope.EndString);
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x0005261C File Offset: 0x0005081C
		public void WriteDataWrapper()
		{
			this.writer.Write("\"d\":");
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x0005262E File Offset: 0x0005082E
		public void WriteDataArrayName()
		{
			this.WriteName("results");
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x0005263C File Offset: 0x0005083C
		public void WriteName(string name)
		{
			JsonWriter.Scope scope = this.scopes.Peek();
			if (scope.ObjectCount != 0)
			{
				this.writer.Write(",");
			}
			scope.ObjectCount++;
			JsonValueUtils.WriteEscapedJsonString(this.writer, name);
			this.writer.Write(":");
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x00052697 File Offset: 0x00050897
		public void WritePaddingFunctionName(string functionName)
		{
			this.writer.Write(functionName);
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x000526A5 File Offset: 0x000508A5
		public void WriteValue(bool value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x000526B9 File Offset: 0x000508B9
		public void WriteValue(int value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x000526CD File Offset: 0x000508CD
		public void WriteValue(float value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x000526E1 File Offset: 0x000508E1
		public void WriteValue(short value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x000526F5 File Offset: 0x000508F5
		public void WriteValue(long value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x00052709 File Offset: 0x00050909
		public void WriteValue(double value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value, this.mustWriteDecimalPointInDoubleValues);
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x00052723 File Offset: 0x00050923
		public void WriteValue(Guid value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x00052737 File Offset: 0x00050937
		public void WriteValue(decimal value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0005274B File Offset: 0x0005094B
		public void WriteValue(DateTime value, ODataVersion odataVersion)
		{
			this.WriteValueSeparator();
			if (odataVersion < ODataVersion.V3)
			{
				JsonValueUtils.WriteValue(this.writer, value, ODataJsonDateTimeFormat.ODataDateTime);
				return;
			}
			JsonValueUtils.WriteValue(this.writer, value, ODataJsonDateTimeFormat.ISO8601DateTime);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x00052772 File Offset: 0x00050972
		public void WriteValue(DateTimeOffset value, ODataVersion odataVersion)
		{
			this.WriteValueSeparator();
			if (odataVersion < ODataVersion.V3)
			{
				JsonValueUtils.WriteValue(this.writer, value, ODataJsonDateTimeFormat.ODataDateTime);
				return;
			}
			JsonValueUtils.WriteValue(this.writer, value, ODataJsonDateTimeFormat.ISO8601DateTime);
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x00052799 File Offset: 0x00050999
		public void WriteValue(TimeSpan value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x000527AD File Offset: 0x000509AD
		public void WriteValue(byte value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x000527C1 File Offset: 0x000509C1
		public void WriteValue(sbyte value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x000527D5 File Offset: 0x000509D5
		public void WriteValue(string value)
		{
			this.WriteValueSeparator();
			JsonValueUtils.WriteValue(this.writer, value);
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x000527E9 File Offset: 0x000509E9
		public void Flush()
		{
			this.writer.Flush();
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x000527F8 File Offset: 0x000509F8
		public void WriteValueSeparator()
		{
			if (this.scopes.Count == 0)
			{
				return;
			}
			JsonWriter.Scope scope = this.scopes.Peek();
			if (scope.Type == JsonWriter.ScopeType.Array)
			{
				if (scope.ObjectCount != 0)
				{
					this.writer.Write(",");
				}
				scope.ObjectCount++;
			}
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x00052850 File Offset: 0x00050A50
		public void StartScope(JsonWriter.ScopeType type)
		{
			if (this.scopes.Count != 0 && this.scopes.Peek().Type != JsonWriter.ScopeType.Padding)
			{
				JsonWriter.Scope scope = this.scopes.Peek();
				if (scope.Type == JsonWriter.ScopeType.Array && scope.ObjectCount != 0)
				{
					this.writer.Write(",");
				}
				scope.ObjectCount++;
			}
			JsonWriter.Scope scope2 = new JsonWriter.Scope(type);
			this.scopes.Push(scope2);
			this.writer.Write(scope2.StartString);
			this.writer.IncreaseIndentation();
			this.writer.WriteLine();
		}

		// Token: 0x04000966 RID: 2406
		private readonly IndentedTextWriter writer;

		// Token: 0x04000967 RID: 2407
		private readonly Stack<JsonWriter.Scope> scopes;

		// Token: 0x04000968 RID: 2408
		private readonly bool mustWriteDecimalPointInDoubleValues;

		// Token: 0x020002A3 RID: 675
		internal enum ScopeType
		{
			// Token: 0x0400096A RID: 2410
			Array,
			// Token: 0x0400096B RID: 2411
			Object,
			// Token: 0x0400096C RID: 2412
			Padding
		}

		// Token: 0x020002A4 RID: 676
		private sealed class Scope
		{
			// Token: 0x060016C4 RID: 5828 RVA: 0x000528F4 File Offset: 0x00050AF4
			public Scope(JsonWriter.ScopeType type)
			{
				this.type = type;
				switch (type)
				{
				case JsonWriter.ScopeType.Array:
					this.StartString = "[";
					this.EndString = "]";
					return;
				case JsonWriter.ScopeType.Object:
					this.StartString = "{";
					this.EndString = "}";
					return;
				case JsonWriter.ScopeType.Padding:
					this.StartString = "(";
					this.EndString = ")";
					return;
				default:
					return;
				}
			}

			// Token: 0x1700048C RID: 1164
			// (get) Token: 0x060016C5 RID: 5829 RVA: 0x00052967 File Offset: 0x00050B67
			// (set) Token: 0x060016C6 RID: 5830 RVA: 0x0005296F File Offset: 0x00050B6F
			public string StartString { get; private set; }

			// Token: 0x1700048D RID: 1165
			// (get) Token: 0x060016C7 RID: 5831 RVA: 0x00052978 File Offset: 0x00050B78
			// (set) Token: 0x060016C8 RID: 5832 RVA: 0x00052980 File Offset: 0x00050B80
			public string EndString { get; private set; }

			// Token: 0x1700048E RID: 1166
			// (get) Token: 0x060016C9 RID: 5833 RVA: 0x00052989 File Offset: 0x00050B89
			// (set) Token: 0x060016CA RID: 5834 RVA: 0x00052991 File Offset: 0x00050B91
			public int ObjectCount { get; set; }

			// Token: 0x1700048F RID: 1167
			// (get) Token: 0x060016CB RID: 5835 RVA: 0x0005299A File Offset: 0x00050B9A
			public JsonWriter.ScopeType Type
			{
				get
				{
					return this.type;
				}
			}

			// Token: 0x0400096D RID: 2413
			private readonly JsonWriter.ScopeType type;
		}
	}
}
