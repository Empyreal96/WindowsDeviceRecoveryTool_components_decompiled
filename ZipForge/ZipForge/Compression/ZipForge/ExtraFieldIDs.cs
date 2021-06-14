using System;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x02000095 RID: 149
	internal class ExtraFieldIDs
	{
		// Token: 0x060006CF RID: 1743 RVA: 0x0002AC20 File Offset: 0x00029C20
		public static bool IsExtraIdIsKnown(ushort extraFieldId)
		{
			return extraFieldId == 1 || extraFieldId == 7 || extraFieldId == 9 || extraFieldId == 10 || extraFieldId == 12 || extraFieldId == 13 || extraFieldId == 15 || extraFieldId == 23 || extraFieldId == 21838 || extraFieldId == 39169;
		}

		// Token: 0x040003A4 RID: 932
		public const ushort Zip64ExtraField = 1;

		// Token: 0x040003A5 RID: 933
		public const ushort AVInfoExtraField = 7;

		// Token: 0x040003A6 RID: 934
		public const ushort OS2ExtraField = 9;

		// Token: 0x040003A7 RID: 935
		public const ushort NTFSExtraField = 10;

		// Token: 0x040003A8 RID: 936
		public const ushort OpenVMSExtraField = 12;

		// Token: 0x040003A9 RID: 937
		public const ushort UNIXExtraField = 13;

		// Token: 0x040003AA RID: 938
		public const ushort PatchDescriptorExtraField = 15;

		// Token: 0x040003AB RID: 939
		public const ushort StrongEncryptionHeaderExtraField = 23;

		// Token: 0x040003AC RID: 940
		public const ushort UnicodeExtraField = 21838;

		// Token: 0x040003AD RID: 941
		public const ushort AESExtraField = 39169;
	}
}
