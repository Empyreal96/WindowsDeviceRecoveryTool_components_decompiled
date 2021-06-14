using System;

namespace System.Drawing.Imaging
{
	/// <summary>An <see cref="T:System.Drawing.Imaging.Encoder" /> object encapsulates a globally unique identifier (GUID) that identifies the category of an image encoder parameter.</summary>
	// Token: 0x02000098 RID: 152
	public sealed class Encoder
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Imaging.Encoder" /> class from the specified globally unique identifier (GUID). The GUID specifies an image encoder parameter category.</summary>
		/// <param name="guid">A globally unique identifier that identifies an image encoder parameter category. </param>
		// Token: 0x06000927 RID: 2343 RVA: 0x00022C2D File Offset: 0x00020E2D
		public Encoder(Guid guid)
		{
			this.guid = guid;
		}

		/// <summary>Gets a globally unique identifier (GUID) that identifies an image encoder parameter category.</summary>
		/// <returns>The GUID that identifies an image encoder parameter category.</returns>
		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06000928 RID: 2344 RVA: 0x00022C3C File Offset: 0x00020E3C
		public Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		/// <summary>An <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the compression parameter category.</summary>
		// Token: 0x04000873 RID: 2163
		public static readonly Encoder Compression = new Encoder(new Guid(-526552163, -13100, 17646, new byte[]
		{
			142,
			186,
			63,
			191,
			139,
			228,
			252,
			88
		}));

		/// <summary>An <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the color depth parameter category.</summary>
		// Token: 0x04000874 RID: 2164
		public static readonly Encoder ColorDepth = new Encoder(new Guid(1711829077, -21146, 19580, new byte[]
		{
			154,
			24,
			56,
			162,
			49,
			11,
			131,
			55
		}));

		/// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the scan method parameter category.</summary>
		// Token: 0x04000875 RID: 2165
		public static readonly Encoder ScanMethod = new Encoder(new Guid(978200161, 12553, 20054, new byte[]
		{
			133,
			54,
			66,
			193,
			86,
			231,
			220,
			250
		}));

		/// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the version parameter category.</summary>
		// Token: 0x04000876 RID: 2166
		public static readonly Encoder Version = new Encoder(new Guid(617712758, -32438, 16804, new byte[]
		{
			191,
			83,
			28,
			33,
			156,
			204,
			247,
			151
		}));

		/// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the render method parameter category.</summary>
		// Token: 0x04000877 RID: 2167
		public static readonly Encoder RenderMethod = new Encoder(new Guid(1833092410, 8858, 18469, new byte[]
		{
			139,
			183,
			92,
			153,
			226,
			185,
			168,
			184
		}));

		/// <summary>Gets an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the quality parameter category.</summary>
		// Token: 0x04000878 RID: 2168
		public static readonly Encoder Quality = new Encoder(new Guid(492561589, -1462, 17709, new byte[]
		{
			156,
			221,
			93,
			179,
			81,
			5,
			231,
			235
		}));

		/// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the transformation parameter category.</summary>
		// Token: 0x04000879 RID: 2169
		public static readonly Encoder Transformation = new Encoder(new Guid(-1928416559, -23154, 20136, new byte[]
		{
			170,
			20,
			16,
			128,
			116,
			183,
			182,
			249
		}));

		/// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the luminance table parameter category.</summary>
		// Token: 0x0400087A RID: 2170
		public static readonly Encoder LuminanceTable = new Encoder(new Guid(-307020850, 614, 19063, new byte[]
		{
			185,
			4,
			39,
			33,
			96,
			153,
			231,
			23
		}));

		/// <summary>An <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the chrominance table parameter category.</summary>
		// Token: 0x0400087B RID: 2171
		public static readonly Encoder ChrominanceTable = new Encoder(new Guid(-219916836, 2483, 17174, new byte[]
		{
			130,
			96,
			103,
			106,
			218,
			50,
			72,
			28
		}));

		/// <summary>Represents an <see cref="T:System.Drawing.Imaging.Encoder" /> object that is initialized with the globally unique identifier for the save flag parameter category.</summary>
		// Token: 0x0400087C RID: 2172
		public static readonly Encoder SaveFlag = new Encoder(new Guid(690120444, -21440, 18367, new byte[]
		{
			140,
			252,
			168,
			91,
			137,
			166,
			85,
			222
		}));

		// Token: 0x0400087D RID: 2173
		private Guid guid;
	}
}
