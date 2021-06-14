using System;

namespace System.Data.Services.Client
{
	// Token: 0x0200009F RID: 159
	internal sealed class ClrTypeConverter : PrimitiveTypeConverter
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x00014EAD File Offset: 0x000130AD
		internal override object Parse(string text)
		{
			return PlatformHelper.GetTypeOrThrow(text);
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x00014EB5 File Offset: 0x000130B5
		internal override string ToString(object instance)
		{
			return ((Type)instance).AssemblyQualifiedName;
		}
	}
}
