using System;
using System.IO;
using System.Windows.Media.Media3D;
using MS.Internal.Media;

namespace System.Windows.Markup
{
	// Token: 0x02000235 RID: 565
	internal class XamlVector3DCollectionSerializer : XamlSerializer
	{
		// Token: 0x06002278 RID: 8824 RVA: 0x000AB6CD File Offset: 0x000A98CD
		internal XamlVector3DCollectionSerializer()
		{
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000AB704 File Offset: 0x000A9904
		public override bool ConvertStringToCustomBinary(BinaryWriter writer, string stringValue)
		{
			return XamlSerializationHelper.SerializeVector3D(writer, stringValue);
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x000AB70D File Offset: 0x000A990D
		public override object ConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Vector3DCollection.DeserializeFrom(reader);
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x000AB715 File Offset: 0x000A9915
		public static object StaticConvertCustomBinaryToObject(BinaryReader reader)
		{
			return Vector3DCollection.DeserializeFrom(reader);
		}
	}
}
