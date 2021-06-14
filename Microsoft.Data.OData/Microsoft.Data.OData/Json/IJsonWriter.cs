using System;

namespace Microsoft.Data.OData.Json
{
	// Token: 0x02000141 RID: 321
	internal interface IJsonWriter
	{
		// Token: 0x06000892 RID: 2194
		void StartPaddingFunctionScope();

		// Token: 0x06000893 RID: 2195
		void EndPaddingFunctionScope();

		// Token: 0x06000894 RID: 2196
		void StartObjectScope();

		// Token: 0x06000895 RID: 2197
		void EndObjectScope();

		// Token: 0x06000896 RID: 2198
		void StartArrayScope();

		// Token: 0x06000897 RID: 2199
		void EndArrayScope();

		// Token: 0x06000898 RID: 2200
		void WriteDataWrapper();

		// Token: 0x06000899 RID: 2201
		void WriteDataArrayName();

		// Token: 0x0600089A RID: 2202
		void WriteName(string name);

		// Token: 0x0600089B RID: 2203
		void WritePaddingFunctionName(string functionName);

		// Token: 0x0600089C RID: 2204
		void WriteValue(bool value);

		// Token: 0x0600089D RID: 2205
		void WriteValue(int value);

		// Token: 0x0600089E RID: 2206
		void WriteValue(float value);

		// Token: 0x0600089F RID: 2207
		void WriteValue(short value);

		// Token: 0x060008A0 RID: 2208
		void WriteValue(long value);

		// Token: 0x060008A1 RID: 2209
		void WriteValue(double value);

		// Token: 0x060008A2 RID: 2210
		void WriteValue(Guid value);

		// Token: 0x060008A3 RID: 2211
		void WriteValue(decimal value);

		// Token: 0x060008A4 RID: 2212
		void WriteValue(DateTime value, ODataVersion odataVersion);

		// Token: 0x060008A5 RID: 2213
		void WriteValue(DateTimeOffset value, ODataVersion odataVersion);

		// Token: 0x060008A6 RID: 2214
		void WriteValue(TimeSpan value);

		// Token: 0x060008A7 RID: 2215
		void WriteValue(byte value);

		// Token: 0x060008A8 RID: 2216
		void WriteValue(sbyte value);

		// Token: 0x060008A9 RID: 2217
		void WriteValue(string value);

		// Token: 0x060008AA RID: 2218
		void Flush();

		// Token: 0x060008AB RID: 2219
		void WriteValueSeparator();

		// Token: 0x060008AC RID: 2220
		void StartScope(JsonWriter.ScopeType type);
	}
}
