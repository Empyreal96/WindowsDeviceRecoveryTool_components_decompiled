using System;
using System.IO;
using System.Spatial;

namespace Microsoft.Data.OData.Query
{
	// Token: 0x020000B0 RID: 176
	internal static class LiteralUtils
	{
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x0000D468 File Offset: 0x0000B668
		private static WellKnownTextSqlFormatter Formatter
		{
			get
			{
				return SpatialImplementation.CurrentImplementation.CreateWellKnownTextSqlFormatter(false);
			}
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x0000D478 File Offset: 0x0000B678
		internal static Geography ParseGeography(string text)
		{
			Geography result;
			using (StringReader stringReader = new StringReader(text))
			{
				result = LiteralUtils.Formatter.Read<Geography>(stringReader);
			}
			return result;
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x0000D4B8 File Offset: 0x0000B6B8
		internal static Geometry ParseGeometry(string text)
		{
			Geometry result;
			using (StringReader stringReader = new StringReader(text))
			{
				result = LiteralUtils.Formatter.Read<Geometry>(stringReader);
			}
			return result;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0000D4F8 File Offset: 0x0000B6F8
		internal static string ToWellKnownText(Geography instance)
		{
			return LiteralUtils.Formatter.Write(instance);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0000D505 File Offset: 0x0000B705
		internal static string ToWellKnownText(Geometry instance)
		{
			return LiteralUtils.Formatter.Write(instance);
		}
	}
}
