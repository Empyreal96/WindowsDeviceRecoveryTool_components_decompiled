using System;
using ComponentAce.Compression.Archiver;

namespace ComponentAce.Compression.ZipForge
{
	// Token: 0x020000A9 RID: 169
	internal class ZipUtil : CompressionUtils
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x0002CD44 File Offset: 0x0002BD44
		public static long InternalGetBlockSize(byte CompressionMode)
		{
			if (CompressionMode == 0)
			{
				return 1048576L;
			}
			if (CompressionMode <= 3)
			{
				return 524288L;
			}
			if (CompressionMode <= 6)
			{
				return 1048576L;
			}
			if (CompressionMode <= 9)
			{
				return 1572864L;
			}
			return 1048576L;
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x0002CD78 File Offset: 0x0002BD78
		public static CompressionLevel InternalGetCompressionLevel(byte CompressionMode)
		{
			if (CompressionMode <= 0)
			{
				return CompressionLevel.None;
			}
			if (CompressionMode <= 3)
			{
				return CompressionLevel.Fastest;
			}
			if (CompressionMode <= 6)
			{
				return CompressionLevel.Normal;
			}
			if (CompressionMode <= 9)
			{
				return CompressionLevel.Max;
			}
			return CompressionLevel.None;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0002CD94 File Offset: 0x0002BD94
		public static byte InternalGetCompressionMode(CompressionLevel compressionLevel)
		{
			switch (compressionLevel)
			{
			case CompressionLevel.Fastest:
				return 1;
			case CompressionLevel.Normal:
				return 6;
			case CompressionLevel.Max:
				return 9;
			default:
				return 0;
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0002CDC4 File Offset: 0x0002BDC4
		public static int GetZlibStreamHeader(int CompMode)
		{
			switch (CompMode)
			{
			case 0:
			case 7:
			case 8:
			case 9:
				return 55928;
			case 1:
			case 2:
				return 376;
			case 3:
			case 4:
				return 24184;
			case 5:
			case 6:
				return 40056;
			default:
				return 0;
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x0002CE1C File Offset: 0x0002BE1C
		public static uint CRC32(uint CRC, byte[] Data, uint DataSize)
		{
			uint num = CRC;
			int num2 = 0;
			while ((long)num2 != (long)((ulong)DataSize))
			{
				num = (num >> 8 ^ ZipUtil.Crc32Table[(int)((byte)num ^ Data[num2++])]);
			}
			return num;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x0002CE4B File Offset: 0x0002BE4B
		public static void UpdateCRC32(byte[] buf, uint Count, ref uint crc_32)
		{
			crc_32 = ZipUtil.CRC32(crc_32, buf, Count);
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x0002CE58 File Offset: 0x0002BE58
		public static uint UpdCRC(byte Octet, uint Crc32)
		{
			return CompressionConst.Crc32Table[(int)((byte)(Crc32 ^ (uint)Octet))] ^ (Crc32 >> 8 & 16777215U);
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0002CE6E File Offset: 0x0002BE6E
		public static ushort LOWORD(uint par)
		{
			return (ushort)(par & 65535U);
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0002CE78 File Offset: 0x0002BE78
		public static ushort HIWORD(uint par)
		{
			return (ushort)(par >> 16);
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002CE7F File Offset: 0x0002BE7F
		public static byte LOWBYTE(ushort par)
		{
			return (byte)(par & 255);
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0002CE89 File Offset: 0x0002BE89
		public static byte HIBYTE(ushort par)
		{
			return (byte)(par >> 8);
		}

		// Token: 0x04000416 RID: 1046
		public const int MAX_PATH = 256;

		// Token: 0x04000417 RID: 1047
		public const int DefaultMaxBlockSize = 1048576;

		// Token: 0x04000418 RID: 1048
		public const int BlockSizeForFastest = 524288;

		// Token: 0x04000419 RID: 1049
		public const int BlockSizeForNormal = 1048576;

		// Token: 0x0400041A RID: 1050
		public const int BlockSizeForMax = 1572864;

		// Token: 0x0400041B RID: 1051
		public const int BlockHeaderSize = 4;

		// Token: 0x0400041C RID: 1052
		public const int FileHeaderSize = 50;

		// Token: 0x0400041D RID: 1053
		public const int CustomHeaderSizeOffset = 23;

		// Token: 0x0400041E RID: 1054
		public const int ZFNormalFileAttr = 32;

		// Token: 0x0400041F RID: 1055
		public const int ZFDirectoryAttr = 16;

		// Token: 0x04000420 RID: 1056
		public const int MinVolumeSize = 65536;

		// Token: 0x04000421 RID: 1057
		public const int HeaderZip64ExtraFieldSize = 20;

		// Token: 0x04000422 RID: 1058
		internal const int ZipFileHeaderSignature = 67324752;

		// Token: 0x04000423 RID: 1059
		internal const int ZipCentralDirSignature = 33639248;

		// Token: 0x04000424 RID: 1060
		internal const int ZipCentralDirEndSignature = 101010256;

		// Token: 0x04000425 RID: 1061
		internal const ushort ZipVersion = 20;

		// Token: 0x04000426 RID: 1062
		internal const ushort FXCVersion = 16660;

		// Token: 0x04000427 RID: 1063
		internal const int Zip64Version = 45;

		// Token: 0x04000428 RID: 1064
		internal const int FXCZF64Version = 16685;

		// Token: 0x04000429 RID: 1065
		internal const int ZipGenPurposeFlag = 0;

		// Token: 0x0400042A RID: 1066
		internal const ushort LanguageEncodingFlagBitNumber = 11;

		// Token: 0x0400042B RID: 1067
		internal const int DataDescriptorBitNumber = 3;

		// Token: 0x0400042C RID: 1068
		public const int caNone = 65535;

		// Token: 0x0400042D RID: 1069
		public static readonly byte[] PPM_MO = new byte[]
		{
			2,
			3,
			4,
			5,
			7,
			8,
			10,
			13,
			16
		};

		// Token: 0x0400042E RID: 1070
		public static readonly byte[] PPM_SA = new byte[]
		{
			2,
			3,
			7,
			16,
			22,
			25,
			40,
			100,
			100
		};

		// Token: 0x0400042F RID: 1071
		internal static readonly int ZipCentralDirEndSize = ZipCentralDirEnd.SizeOf();

		// Token: 0x04000430 RID: 1072
		internal static readonly int Zip64CentralDirEndSize = Zip64CentralDirEnd.SizeOf();

		// Token: 0x04000431 RID: 1073
		internal static readonly int Zip64CentralDirEndLocatorSize = Zip64CentralDirEndLocator.SizeOf();

		// Token: 0x04000432 RID: 1074
		internal static readonly uint Zip64CentralDirEndSignature = 101075792U;

		// Token: 0x04000433 RID: 1075
		internal static readonly uint Zip64CentralDirEndLocatorSignature = 117853008U;

		// Token: 0x04000434 RID: 1076
		internal static readonly uint[] Crc32Table = new uint[]
		{
			0U,
			1996959894U,
			3993919788U,
			2567524794U,
			124634137U,
			1886057615U,
			3915621685U,
			2657392035U,
			249268274U,
			2044508324U,
			3772115230U,
			2547177864U,
			162941995U,
			2125561021U,
			3887607047U,
			2428444049U,
			498536548U,
			1789927666U,
			4089016648U,
			2227061214U,
			450548861U,
			1843258603U,
			4107580753U,
			2211677639U,
			325883990U,
			1684777152U,
			4251122042U,
			2321926636U,
			335633487U,
			1661365465U,
			4195302755U,
			2366115317U,
			997073096U,
			1281953886U,
			3579855332U,
			2724688242U,
			1006888145U,
			1258607687U,
			3524101629U,
			2768942443U,
			901097722U,
			1119000684U,
			3686517206U,
			2898065728U,
			853044451U,
			1172266101U,
			3705015759U,
			2882616665U,
			651767980U,
			1373503546U,
			3369554304U,
			3218104598U,
			565507253U,
			1454621731U,
			3485111705U,
			3099436303U,
			671266974U,
			1594198024U,
			3322730930U,
			2970347812U,
			795835527U,
			1483230225U,
			3244367275U,
			3060149565U,
			1994146192U,
			31158534U,
			2563907772U,
			4023717930U,
			1907459465U,
			112637215U,
			2680153253U,
			3904427059U,
			2013776290U,
			251722036U,
			2517215374U,
			3775830040U,
			2137656763U,
			141376813U,
			2439277719U,
			3865271297U,
			1802195444U,
			476864866U,
			2238001368U,
			4066508878U,
			1812370925U,
			453092731U,
			2181625025U,
			4111451223U,
			1706088902U,
			314042704U,
			2344532202U,
			4240017532U,
			1658658271U,
			366619977U,
			2362670323U,
			4224994405U,
			1303535960U,
			984961486U,
			2747007092U,
			3569037538U,
			1256170817U,
			1037604311U,
			2765210733U,
			3554079995U,
			1131014506U,
			879679996U,
			2909243462U,
			3663771856U,
			1141124467U,
			855842277U,
			2852801631U,
			3708648649U,
			1342533948U,
			654459306U,
			3188396048U,
			3373015174U,
			1466479909U,
			544179635U,
			3110523913U,
			3462522015U,
			1591671054U,
			702138776U,
			2966460450U,
			3352799412U,
			1504918807U,
			783551873U,
			3082640443U,
			3233442989U,
			3988292384U,
			2596254646U,
			62317068U,
			1957810842U,
			3939845945U,
			2647816111U,
			81470997U,
			1943803523U,
			3814918930U,
			2489596804U,
			225274430U,
			2053790376U,
			3826175755U,
			2466906013U,
			167816743U,
			2097651377U,
			4027552580U,
			2265490386U,
			503444072U,
			1762050814U,
			4150417245U,
			2154129355U,
			426522225U,
			1852507879U,
			4275313526U,
			2312317920U,
			282753626U,
			1742555852U,
			4189708143U,
			2394877945U,
			397917763U,
			1622183637U,
			3604390888U,
			2714866558U,
			953729732U,
			1340076626U,
			3518719985U,
			2797360999U,
			1068828381U,
			1219638859U,
			3624741850U,
			2936675148U,
			906185462U,
			1090812512U,
			3747672003U,
			2825379669U,
			829329135U,
			1181335161U,
			3412177804U,
			3160834842U,
			628085408U,
			1382605366U,
			3423369109U,
			3138078467U,
			570562233U,
			1426400815U,
			3317316542U,
			2998733608U,
			733239954U,
			1555261956U,
			3268935591U,
			3050360625U,
			752459403U,
			1541320221U,
			2607071920U,
			3965973030U,
			1969922972U,
			40735498U,
			2617837225U,
			3943577151U,
			1913087877U,
			83908371U,
			2512341634U,
			3803740692U,
			2075208622U,
			213261112U,
			2463272603U,
			3855990285U,
			2094854071U,
			198958881U,
			2262029012U,
			4057260610U,
			1759359992U,
			534414190U,
			2176718541U,
			4139329115U,
			1873836001U,
			414664567U,
			2282248934U,
			4279200368U,
			1711684554U,
			285281116U,
			2405801727U,
			4167216745U,
			1634467795U,
			376229701U,
			2685067896U,
			3608007406U,
			1308918612U,
			956543938U,
			2808555105U,
			3495958263U,
			1231636301U,
			1047427035U,
			2932959818U,
			3654703836U,
			1088359270U,
			936918000U,
			2847714899U,
			3736837829U,
			1202900863U,
			817233897U,
			3183342108U,
			3401237130U,
			1404277552U,
			615818150U,
			3134207493U,
			3453421203U,
			1423857449U,
			601450431U,
			3009837614U,
			3294710456U,
			1567103746U,
			711928724U,
			3020668471U,
			3272380065U,
			1510334235U,
			755167117U,
			1953656656U,
			1936617321U,
			1886339872U,
			1734963833U,
			673215592U,
			824191331U,
			540621113U,
			1210087778U,
			1852139361U,
			1684361760U,
			1851878756U,
			1874568046U
		};
	}
}
