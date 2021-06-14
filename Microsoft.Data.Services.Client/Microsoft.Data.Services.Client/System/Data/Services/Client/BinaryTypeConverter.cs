using System;
using System.Reflection;

namespace System.Data.Services.Client
{
	// Token: 0x02000092 RID: 146
	internal sealed class BinaryTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00014CAB File Offset: 0x00012EAB
		// (set) Token: 0x06000536 RID: 1334 RVA: 0x00014CB2 File Offset: 0x00012EB2
		internal static Type BinaryType { get; set; }

		// Token: 0x06000537 RID: 1335 RVA: 0x00014CBC File Offset: 0x00012EBC
		internal override object Parse(string text)
		{
			return Activator.CreateInstance(BinaryTypeConverter.BinaryType, new object[]
			{
				Convert.FromBase64String(text)
			});
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00014CE4 File Offset: 0x00012EE4
		internal override string ToString(object instance)
		{
			return instance.ToString();
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00014CEC File Offset: 0x00012EEC
		internal byte[] ToArray(object instance)
		{
			if (this.convertToByteArrayMethodInfo == null)
			{
				this.convertToByteArrayMethodInfo = instance.GetType().GetMethod("ToArray", BindingFlags.Instance | BindingFlags.Public);
			}
			return (byte[])this.convertToByteArrayMethodInfo.Invoke(instance, null);
		}

		// Token: 0x04000304 RID: 772
		private MethodInfo convertToByteArrayMethodInfo;
	}
}
