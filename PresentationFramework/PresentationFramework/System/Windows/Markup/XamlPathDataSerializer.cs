using System;
using System.IO;
using MS.Internal;

namespace System.Windows.Markup
{
	// Token: 0x02000238 RID: 568
	internal class XamlPathDataSerializer : XamlSerializer
	{
		// Token: 0x06002286 RID: 8838 RVA: 0x000AB965 File Offset: 0x000A9B65
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			Parsers.PathMinilanguageToBinary(writer, stringValue);
			return true;
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000AB96F File Offset: 0x000A9B6F
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Parsers.DeserializeStreamGeometry(reader);
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000AB977 File Offset: 0x000A9B77
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Parsers.DeserializeStreamGeometry(reader);
		}
	}
}
