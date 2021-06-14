using System;
using System.Text;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020003C4 RID: 964
	internal class FontTableEntry
	{
		// Token: 0x060033DF RID: 13279 RVA: 0x000E711C File Offset: 0x000E531C
		internal FontTableEntry()
		{
			this._index = -1;
			this._codePage = -1;
			this._charSet = 0;
			this._bNameSealed = false;
			this._bPending = true;
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x060033E0 RID: 13280 RVA: 0x000E7147 File Offset: 0x000E5347
		// (set) Token: 0x060033E1 RID: 13281 RVA: 0x000E714F File Offset: 0x000E534F
		internal int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		// Token: 0x17000D4E RID: 3406
		// (get) Token: 0x060033E2 RID: 13282 RVA: 0x000E7158 File Offset: 0x000E5358
		// (set) Token: 0x060033E3 RID: 13283 RVA: 0x000E7160 File Offset: 0x000E5360
		internal string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		// Token: 0x17000D4F RID: 3407
		// (get) Token: 0x060033E4 RID: 13284 RVA: 0x000E7169 File Offset: 0x000E5369
		// (set) Token: 0x060033E5 RID: 13285 RVA: 0x000E7171 File Offset: 0x000E5371
		internal bool IsNameSealed
		{
			get
			{
				return this._bNameSealed;
			}
			set
			{
				this._bNameSealed = value;
			}
		}

		// Token: 0x17000D50 RID: 3408
		// (get) Token: 0x060033E6 RID: 13286 RVA: 0x000E717A File Offset: 0x000E537A
		// (set) Token: 0x060033E7 RID: 13287 RVA: 0x000E7182 File Offset: 0x000E5382
		internal bool IsPending
		{
			get
			{
				return this._bPending;
			}
			set
			{
				this._bPending = value;
			}
		}

		// Token: 0x17000D51 RID: 3409
		// (get) Token: 0x060033E8 RID: 13288 RVA: 0x000E718B File Offset: 0x000E538B
		// (set) Token: 0x060033E9 RID: 13289 RVA: 0x000E7193 File Offset: 0x000E5393
		internal int CodePage
		{
			get
			{
				return this._codePage;
			}
			set
			{
				this._codePage = value;
			}
		}

		// Token: 0x17000D52 RID: 3410
		// (set) Token: 0x060033EA RID: 13290 RVA: 0x000E719C File Offset: 0x000E539C
		internal int CodePageFromCharSet
		{
			set
			{
				int num = FontTableEntry.CharSetToCodePage(value);
				if (num != 0)
				{
					this.CodePage = num;
				}
			}
		}

		// Token: 0x17000D53 RID: 3411
		// (get) Token: 0x060033EB RID: 13291 RVA: 0x000E71BA File Offset: 0x000E53BA
		// (set) Token: 0x060033EC RID: 13292 RVA: 0x000E71C2 File Offset: 0x000E53C2
		internal int CharSet
		{
			get
			{
				return this._charSet;
			}
			set
			{
				this._charSet = value;
			}
		}

		// Token: 0x060033ED RID: 13293 RVA: 0x000E71CC File Offset: 0x000E53CC
		internal static int CharSetToCodePage(int cs)
		{
			if (cs <= 163)
			{
				if (cs > 77)
				{
					if (cs != 78)
					{
						switch (cs)
						{
						case 128:
							break;
						case 129:
							return 949;
						case 130:
							return 1361;
						case 131:
						case 132:
						case 133:
						case 135:
							return 0;
						case 134:
							return 936;
						case 136:
							return 950;
						default:
							switch (cs)
							{
							case 161:
								return 1253;
							case 162:
								return 1254;
							case 163:
								return 1258;
							default:
								return 0;
							}
							break;
						}
					}
					return 932;
				}
				switch (cs)
				{
				case 0:
					return 1252;
				case 1:
					return -1;
				case 2:
					return 1252;
				case 3:
					return -1;
				default:
					if (cs == 77)
					{
						return 10000;
					}
					break;
				}
			}
			else if (cs <= 222)
			{
				switch (cs)
				{
				case 177:
					return 1255;
				case 178:
					return 1256;
				case 179:
					return 1256;
				case 180:
					return 1256;
				case 181:
					return 1255;
				case 182:
				case 183:
				case 184:
				case 185:
					break;
				case 186:
					return 1257;
				default:
					if (cs == 204)
					{
						return 1251;
					}
					if (cs == 222)
					{
						return 874;
					}
					break;
				}
			}
			else
			{
				if (cs == 238)
				{
					return 1250;
				}
				if (cs == 254)
				{
					return 437;
				}
				if (cs == 255)
				{
					return 850;
				}
			}
			return 0;
		}

		// Token: 0x060033EE RID: 13294 RVA: 0x000E7364 File Offset: 0x000E5564
		internal void ComputePreferredCodePage()
		{
			int[] array = new int[]
			{
				1252,
				932,
				949,
				1361,
				936,
				950,
				1253,
				1254,
				1258,
				1255,
				1256,
				1257,
				1251,
				874,
				1250,
				437,
				850
			};
			this.CodePage = 1252;
			this.CharSet = 0;
			if (this.Name != null && this.Name.Length > 0)
			{
				byte[] bytes = new byte[this.Name.Length * 6];
				char[] array2 = new char[this.Name.Length * 6];
				for (int i = 0; i < array.Length; i++)
				{
					Encoding encoding = Encoding.GetEncoding(array[i]);
					int bytes2 = encoding.GetBytes(this.Name, 0, this.Name.Length, bytes, 0);
					int chars = encoding.GetChars(bytes, 0, bytes2, array2, 0);
					if (chars == this.Name.Length)
					{
						int num = 0;
						while (num < chars && array2[num] == this.Name[num])
						{
							num++;
						}
						if (num == chars)
						{
							this.CodePage = array[i];
							this.CharSet = FontTableEntry.CodePageToCharSet(this.CodePage);
							break;
						}
					}
				}
				if (FontTableEntry.IsSymbolFont(this.Name))
				{
					this.CharSet = 2;
				}
			}
		}

		// Token: 0x060033EF RID: 13295 RVA: 0x000E7490 File Offset: 0x000E5690
		private static int CodePageToCharSet(int cp)
		{
			if (cp <= 936)
			{
				if (cp <= 850)
				{
					if (cp == 437)
					{
						return 254;
					}
					if (cp == 850)
					{
						return 255;
					}
				}
				else
				{
					if (cp == 874)
					{
						return 222;
					}
					if (cp == 932)
					{
						return 128;
					}
					if (cp == 936)
					{
						return 134;
					}
				}
			}
			else if (cp <= 950)
			{
				if (cp == 949)
				{
					return 129;
				}
				if (cp == 950)
				{
					return 136;
				}
			}
			else
			{
				switch (cp)
				{
				case 1250:
					return 238;
				case 1251:
					return 204;
				case 1252:
					return 0;
				case 1253:
					return 161;
				case 1254:
					return 162;
				case 1255:
					return 177;
				case 1256:
					return 178;
				case 1257:
					return 186;
				case 1258:
					return 163;
				default:
					if (cp == 1361)
					{
						return 130;
					}
					if (cp == 10000)
					{
						return 77;
					}
					break;
				}
			}
			return 0;
		}

		// Token: 0x060033F0 RID: 13296 RVA: 0x000E75B0 File Offset: 0x000E57B0
		private static bool IsSymbolFont(string typefaceName)
		{
			bool result = false;
			Typeface typeface = new Typeface(typefaceName);
			if (typeface != null)
			{
				GlyphTypeface glyphTypeface = typeface.TryGetGlyphTypeface();
				if (glyphTypeface != null && glyphTypeface.Symbol)
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x040024B1 RID: 9393
		private string _name;

		// Token: 0x040024B2 RID: 9394
		private int _index;

		// Token: 0x040024B3 RID: 9395
		private int _codePage;

		// Token: 0x040024B4 RID: 9396
		private int _charSet;

		// Token: 0x040024B5 RID: 9397
		private bool _bNameSealed;

		// Token: 0x040024B6 RID: 9398
		private bool _bPending;
	}
}
