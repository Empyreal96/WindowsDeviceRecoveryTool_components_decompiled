using System;
using System.IO;
using System.Windows.Media.Media3D;
using MS.Internal.Media;

namespace System.Windows.Markup
{
	// Token: 0x02000234 RID: 564
	internal class XamlPoint3DCollectionSerializer : XamlSerializer
	{
		// Token: 0x06002275 RID: 8821 RVA: 0x000AB6EB File Offset: 0x000A98EB
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return XamlSerializationHelper.SerializePoint3D(writer, stringValue);
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x000AB6F4 File Offset: 0x000A98F4
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Point3DCollection.DeserializeFrom(reader);
		}

		// Token: 0x06002277 RID: 8823 RVA: 0x000AB6FC File Offset: 0x000A98FC
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Point3DCollection.DeserializeFrom(reader);
		}
	}
}
