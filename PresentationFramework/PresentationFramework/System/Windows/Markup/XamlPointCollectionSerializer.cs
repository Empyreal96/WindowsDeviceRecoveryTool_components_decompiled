using System;
using System.IO;
using System.Windows.Media;
using MS.Internal.Media;

namespace System.Windows.Markup
{
	// Token: 0x02000236 RID: 566
	internal class XamlPointCollectionSerializer : XamlSerializer
	{
		// Token: 0x0600227D RID: 8829 RVA: 0x000AB71D File Offset: 0x000A991D
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return XamlSerializationHelper.SerializePoint(writer, stringValue);
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x000AB726 File Offset: 0x000A9926
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return PointCollection.DeserializeFrom(reader);
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x000AB72E File Offset: 0x000A992E
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return PointCollection.DeserializeFrom(reader);
		}
	}
}
