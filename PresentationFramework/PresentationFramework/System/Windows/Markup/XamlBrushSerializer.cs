using System;
using System.IO;
using System.Windows.Media;

namespace System.Windows.Markup
{
	// Token: 0x02000233 RID: 563
	internal class XamlBrushSerializer : XamlSerializer
	{
		// Token: 0x06002272 RID: 8818 RVA: 0x000AB6D5 File Offset: 0x000A98D5
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return SolidColorBrush.SerializeOn(writer, stringValue.Trim());
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x000AB6E3 File Offset: 0x000A98E3
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return SolidColorBrush.DeserializeFrom(reader);
		}
	}
}
