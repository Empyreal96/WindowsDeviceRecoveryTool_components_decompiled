using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.Text;

namespace System.Drawing
{
	/// <summary>Represents an ARGB (alpha, red, green, blue) color.</summary>
	// Token: 0x02000017 RID: 23
	[TypeConverter(typeof(ColorConverter))]
	[DebuggerDisplay("{NameAndARGBValue}")]
	[Editor("System.Drawing.Design.ColorEditor, System.Drawing.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[Serializable]
	public struct Color
	{
		/// <summary>Gets a system-defined color.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00006E7E File Offset: 0x0000507E
		public static Color Transparent
		{
			get
			{
				return new Color(KnownColor.Transparent);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF0F8FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00006E87 File Offset: 0x00005087
		public static Color AliceBlue
		{
			get
			{
				return new Color(KnownColor.AliceBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFAEBD7.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00006E90 File Offset: 0x00005090
		public static Color AntiqueWhite
		{
			get
			{
				return new Color(KnownColor.AntiqueWhite);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00FFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00006E99 File Offset: 0x00005099
		public static Color Aqua
		{
			get
			{
				return new Color(KnownColor.Aqua);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF7FFFD4.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600011A RID: 282 RVA: 0x00006EA2 File Offset: 0x000050A2
		public static Color Aquamarine
		{
			get
			{
				return new Color(KnownColor.Aquamarine);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF0FFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00006EAB File Offset: 0x000050AB
		public static Color Azure
		{
			get
			{
				return new Color(KnownColor.Azure);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF5F5DC.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600011C RID: 284 RVA: 0x00006EB4 File Offset: 0x000050B4
		public static Color Beige
		{
			get
			{
				return new Color(KnownColor.Beige);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFE4C4.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00006EBD File Offset: 0x000050BD
		public static Color Bisque
		{
			get
			{
				return new Color(KnownColor.Bisque);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF000000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x0600011E RID: 286 RVA: 0x00006EC6 File Offset: 0x000050C6
		public static Color Black
		{
			get
			{
				return new Color(KnownColor.Black);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFEBCD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00006ECF File Offset: 0x000050CF
		public static Color BlanchedAlmond
		{
			get
			{
				return new Color(KnownColor.BlanchedAlmond);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF0000FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000120 RID: 288 RVA: 0x00006ED8 File Offset: 0x000050D8
		public static Color Blue
		{
			get
			{
				return new Color(KnownColor.Blue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF8A2BE2.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000121 RID: 289 RVA: 0x00006EE1 File Offset: 0x000050E1
		public static Color BlueViolet
		{
			get
			{
				return new Color(KnownColor.BlueViolet);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFA52A2A.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00006EEA File Offset: 0x000050EA
		public static Color Brown
		{
			get
			{
				return new Color(KnownColor.Brown);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDEB887.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00006EF3 File Offset: 0x000050F3
		public static Color BurlyWood
		{
			get
			{
				return new Color(KnownColor.BurlyWood);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF5F9EA0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00006EFC File Offset: 0x000050FC
		public static Color CadetBlue
		{
			get
			{
				return new Color(KnownColor.CadetBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF7FFF00.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00006F05 File Offset: 0x00005105
		public static Color Chartreuse
		{
			get
			{
				return new Color(KnownColor.Chartreuse);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFD2691E.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00006F0E File Offset: 0x0000510E
		public static Color Chocolate
		{
			get
			{
				return new Color(KnownColor.Chocolate);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF7F50.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00006F17 File Offset: 0x00005117
		public static Color Coral
		{
			get
			{
				return new Color(KnownColor.Coral);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF6495ED.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00006F20 File Offset: 0x00005120
		public static Color CornflowerBlue
		{
			get
			{
				return new Color(KnownColor.CornflowerBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFF8DC.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000129 RID: 297 RVA: 0x00006F29 File Offset: 0x00005129
		public static Color Cornsilk
		{
			get
			{
				return new Color(KnownColor.Cornsilk);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDC143C.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600012A RID: 298 RVA: 0x00006F32 File Offset: 0x00005132
		public static Color Crimson
		{
			get
			{
				return new Color(KnownColor.Crimson);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00FFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600012B RID: 299 RVA: 0x00006F3B File Offset: 0x0000513B
		public static Color Cyan
		{
			get
			{
				return new Color(KnownColor.Cyan);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00008B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600012C RID: 300 RVA: 0x00006F44 File Offset: 0x00005144
		public static Color DarkBlue
		{
			get
			{
				return new Color(KnownColor.DarkBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF008B8B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x0600012D RID: 301 RVA: 0x00006F4D File Offset: 0x0000514D
		public static Color DarkCyan
		{
			get
			{
				return new Color(KnownColor.DarkCyan);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFB8860B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x0600012E RID: 302 RVA: 0x00006F56 File Offset: 0x00005156
		public static Color DarkGoldenrod
		{
			get
			{
				return new Color(KnownColor.DarkGoldenrod);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFA9A9A9.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600012F RID: 303 RVA: 0x00006F5F File Offset: 0x0000515F
		public static Color DarkGray
		{
			get
			{
				return new Color(KnownColor.DarkGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF006400.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000130 RID: 304 RVA: 0x00006F68 File Offset: 0x00005168
		public static Color DarkGreen
		{
			get
			{
				return new Color(KnownColor.DarkGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFBDB76B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00006F71 File Offset: 0x00005171
		public static Color DarkKhaki
		{
			get
			{
				return new Color(KnownColor.DarkKhaki);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF8B008B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000132 RID: 306 RVA: 0x00006F7A File Offset: 0x0000517A
		public static Color DarkMagenta
		{
			get
			{
				return new Color(KnownColor.DarkMagenta);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF556B2F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000133 RID: 307 RVA: 0x00006F83 File Offset: 0x00005183
		public static Color DarkOliveGreen
		{
			get
			{
				return new Color(KnownColor.DarkOliveGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF8C00.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000134 RID: 308 RVA: 0x00006F8C File Offset: 0x0000518C
		public static Color DarkOrange
		{
			get
			{
				return new Color(KnownColor.DarkOrange);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF9932CC.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000BA RID: 186
		// (get) Token: 0x06000135 RID: 309 RVA: 0x00006F95 File Offset: 0x00005195
		public static Color DarkOrchid
		{
			get
			{
				return new Color(KnownColor.DarkOrchid);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF8B0000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000BB RID: 187
		// (get) Token: 0x06000136 RID: 310 RVA: 0x00006F9E File Offset: 0x0000519E
		public static Color DarkRed
		{
			get
			{
				return new Color(KnownColor.DarkRed);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFE9967A.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000137 RID: 311 RVA: 0x00006FA7 File Offset: 0x000051A7
		public static Color DarkSalmon
		{
			get
			{
				return new Color(KnownColor.DarkSalmon);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF8FBC8F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000138 RID: 312 RVA: 0x00006FB0 File Offset: 0x000051B0
		public static Color DarkSeaGreen
		{
			get
			{
				return new Color(KnownColor.DarkSeaGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF483D8B.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000139 RID: 313 RVA: 0x00006FB9 File Offset: 0x000051B9
		public static Color DarkSlateBlue
		{
			get
			{
				return new Color(KnownColor.DarkSlateBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF2F4F4F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000BF RID: 191
		// (get) Token: 0x0600013A RID: 314 RVA: 0x00006FC2 File Offset: 0x000051C2
		public static Color DarkSlateGray
		{
			get
			{
				return new Color(KnownColor.DarkSlateGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00CED1.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600013B RID: 315 RVA: 0x00006FCB File Offset: 0x000051CB
		public static Color DarkTurquoise
		{
			get
			{
				return new Color(KnownColor.DarkTurquoise);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF9400D3.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00006FD4 File Offset: 0x000051D4
		public static Color DarkViolet
		{
			get
			{
				return new Color(KnownColor.DarkViolet);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF1493.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600013D RID: 317 RVA: 0x00006FDD File Offset: 0x000051DD
		public static Color DeepPink
		{
			get
			{
				return new Color(KnownColor.DeepPink);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00BFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00006FE6 File Offset: 0x000051E6
		public static Color DeepSkyBlue
		{
			get
			{
				return new Color(KnownColor.DeepSkyBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF696969.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x0600013F RID: 319 RVA: 0x00006FEF File Offset: 0x000051EF
		public static Color DimGray
		{
			get
			{
				return new Color(KnownColor.DimGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF1E90FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00006FF8 File Offset: 0x000051F8
		public static Color DodgerBlue
		{
			get
			{
				return new Color(KnownColor.DodgerBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFB22222.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000141 RID: 321 RVA: 0x00007001 File Offset: 0x00005201
		public static Color Firebrick
		{
			get
			{
				return new Color(KnownColor.Firebrick);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFAF0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000142 RID: 322 RVA: 0x0000700A File Offset: 0x0000520A
		public static Color FloralWhite
		{
			get
			{
				return new Color(KnownColor.FloralWhite);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF228B22.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000143 RID: 323 RVA: 0x00007013 File Offset: 0x00005213
		public static Color ForestGreen
		{
			get
			{
				return new Color(KnownColor.ForestGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF00FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x06000144 RID: 324 RVA: 0x0000701C File Offset: 0x0000521C
		public static Color Fuchsia
		{
			get
			{
				return new Color(KnownColor.Fuchsia);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDCDCDC.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000145 RID: 325 RVA: 0x00007025 File Offset: 0x00005225
		public static Color Gainsboro
		{
			get
			{
				return new Color(KnownColor.Gainsboro);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF8F8FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000146 RID: 326 RVA: 0x0000702E File Offset: 0x0000522E
		public static Color GhostWhite
		{
			get
			{
				return new Color(KnownColor.GhostWhite);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFD700.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000147 RID: 327 RVA: 0x00007037 File Offset: 0x00005237
		public static Color Gold
		{
			get
			{
				return new Color(KnownColor.Gold);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDAA520.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00007040 File Offset: 0x00005240
		public static Color Goldenrod
		{
			get
			{
				return new Color(KnownColor.Goldenrod);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF808080.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> strcture representing a system-defined color.</returns>
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000149 RID: 329 RVA: 0x00007049 File Offset: 0x00005249
		public static Color Gray
		{
			get
			{
				return new Color(KnownColor.Gray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF008000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00007052 File Offset: 0x00005252
		public static Color Green
		{
			get
			{
				return new Color(KnownColor.Green);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFADFF2F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000705B File Offset: 0x0000525B
		public static Color GreenYellow
		{
			get
			{
				return new Color(KnownColor.GreenYellow);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF0FFF0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00007064 File Offset: 0x00005264
		public static Color Honeydew
		{
			get
			{
				return new Color(KnownColor.Honeydew);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF69B4.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000706D File Offset: 0x0000526D
		public static Color HotPink
		{
			get
			{
				return new Color(KnownColor.HotPink);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFCD5C5C.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00007076 File Offset: 0x00005276
		public static Color IndianRed
		{
			get
			{
				return new Color(KnownColor.IndianRed);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF4B0082.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600014F RID: 335 RVA: 0x0000707F File Offset: 0x0000527F
		public static Color Indigo
		{
			get
			{
				return new Color(KnownColor.Indigo);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFFF0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00007088 File Offset: 0x00005288
		public static Color Ivory
		{
			get
			{
				return new Color(KnownColor.Ivory);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF0E68C.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00007091 File Offset: 0x00005291
		public static Color Khaki
		{
			get
			{
				return new Color(KnownColor.Khaki);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFE6E6FA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000709A File Offset: 0x0000529A
		public static Color Lavender
		{
			get
			{
				return new Color(KnownColor.Lavender);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFF0F5.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000070A3 File Offset: 0x000052A3
		public static Color LavenderBlush
		{
			get
			{
				return new Color(KnownColor.LavenderBlush);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF7CFC00.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000154 RID: 340 RVA: 0x000070AC File Offset: 0x000052AC
		public static Color LawnGreen
		{
			get
			{
				return new Color(KnownColor.LawnGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFACD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000155 RID: 341 RVA: 0x000070B5 File Offset: 0x000052B5
		public static Color LemonChiffon
		{
			get
			{
				return new Color(KnownColor.LemonChiffon);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFADD8E6.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000156 RID: 342 RVA: 0x000070BE File Offset: 0x000052BE
		public static Color LightBlue
		{
			get
			{
				return new Color(KnownColor.LightBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF08080.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000157 RID: 343 RVA: 0x000070C7 File Offset: 0x000052C7
		public static Color LightCoral
		{
			get
			{
				return new Color(KnownColor.LightCoral);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFE0FFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000158 RID: 344 RVA: 0x000070D0 File Offset: 0x000052D0
		public static Color LightCyan
		{
			get
			{
				return new Color(KnownColor.LightCyan);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFAFAD2.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x06000159 RID: 345 RVA: 0x000070D9 File Offset: 0x000052D9
		public static Color LightGoldenrodYellow
		{
			get
			{
				return new Color(KnownColor.LightGoldenrodYellow);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF90EE90.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600015A RID: 346 RVA: 0x000070E2 File Offset: 0x000052E2
		public static Color LightGreen
		{
			get
			{
				return new Color(KnownColor.LightGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFD3D3D3.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600015B RID: 347 RVA: 0x000070EB File Offset: 0x000052EB
		public static Color LightGray
		{
			get
			{
				return new Color(KnownColor.LightGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFB6C1.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000070F4 File Offset: 0x000052F4
		public static Color LightPink
		{
			get
			{
				return new Color(KnownColor.LightPink);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFA07A.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600015D RID: 349 RVA: 0x000070FD File Offset: 0x000052FD
		public static Color LightSalmon
		{
			get
			{
				return new Color(KnownColor.LightSalmon);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF20B2AA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00007106 File Offset: 0x00005306
		public static Color LightSeaGreen
		{
			get
			{
				return new Color(KnownColor.LightSeaGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF87CEFA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600015F RID: 351 RVA: 0x0000710F File Offset: 0x0000530F
		public static Color LightSkyBlue
		{
			get
			{
				return new Color(KnownColor.LightSkyBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF778899.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000160 RID: 352 RVA: 0x00007118 File Offset: 0x00005318
		public static Color LightSlateGray
		{
			get
			{
				return new Color(KnownColor.LightSlateGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFB0C4DE.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000161 RID: 353 RVA: 0x00007121 File Offset: 0x00005321
		public static Color LightSteelBlue
		{
			get
			{
				return new Color(KnownColor.LightSteelBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFFE0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000712A File Offset: 0x0000532A
		public static Color LightYellow
		{
			get
			{
				return new Color(KnownColor.LightYellow);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00FF00.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000163 RID: 355 RVA: 0x00007133 File Offset: 0x00005333
		public static Color Lime
		{
			get
			{
				return new Color(KnownColor.Lime);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF32CD32.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000713C File Offset: 0x0000533C
		public static Color LimeGreen
		{
			get
			{
				return new Color(KnownColor.LimeGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFAF0E6.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000EA RID: 234
		// (get) Token: 0x06000165 RID: 357 RVA: 0x00007145 File Offset: 0x00005345
		public static Color Linen
		{
			get
			{
				return new Color(KnownColor.Linen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF00FF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000714E File Offset: 0x0000534E
		public static Color Magenta
		{
			get
			{
				return new Color(KnownColor.Magenta);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF800000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000167 RID: 359 RVA: 0x00007157 File Offset: 0x00005357
		public static Color Maroon
		{
			get
			{
				return new Color(KnownColor.Maroon);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF66CDAA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00007160 File Offset: 0x00005360
		public static Color MediumAquamarine
		{
			get
			{
				return new Color(KnownColor.MediumAquamarine);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF0000CD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000169 RID: 361 RVA: 0x00007169 File Offset: 0x00005369
		public static Color MediumBlue
		{
			get
			{
				return new Color(KnownColor.MediumBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFBA55D3.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00007172 File Offset: 0x00005372
		public static Color MediumOrchid
		{
			get
			{
				return new Color(KnownColor.MediumOrchid);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF9370DB.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x0600016B RID: 363 RVA: 0x0000717B File Offset: 0x0000537B
		public static Color MediumPurple
		{
			get
			{
				return new Color(KnownColor.MediumPurple);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF3CB371.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00007184 File Offset: 0x00005384
		public static Color MediumSeaGreen
		{
			get
			{
				return new Color(KnownColor.MediumSeaGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF7B68EE.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000718D File Offset: 0x0000538D
		public static Color MediumSlateBlue
		{
			get
			{
				return new Color(KnownColor.MediumSlateBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00FA9A.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00007196 File Offset: 0x00005396
		public static Color MediumSpringGreen
		{
			get
			{
				return new Color(KnownColor.MediumSpringGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF48D1CC.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000719F File Offset: 0x0000539F
		public static Color MediumTurquoise
		{
			get
			{
				return new Color(KnownColor.MediumTurquoise);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFC71585.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000170 RID: 368 RVA: 0x000071A8 File Offset: 0x000053A8
		public static Color MediumVioletRed
		{
			get
			{
				return new Color(KnownColor.MediumVioletRed);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF191970.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000171 RID: 369 RVA: 0x000071B1 File Offset: 0x000053B1
		public static Color MidnightBlue
		{
			get
			{
				return new Color(KnownColor.MidnightBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF5FFFA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000172 RID: 370 RVA: 0x000071BA File Offset: 0x000053BA
		public static Color MintCream
		{
			get
			{
				return new Color(KnownColor.MintCream);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFE4E1.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000173 RID: 371 RVA: 0x000071C3 File Offset: 0x000053C3
		public static Color MistyRose
		{
			get
			{
				return new Color(KnownColor.MistyRose);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFE4B5.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000071CC File Offset: 0x000053CC
		public static Color Moccasin
		{
			get
			{
				return new Color(KnownColor.Moccasin);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFDEAD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000175 RID: 373 RVA: 0x000071D5 File Offset: 0x000053D5
		public static Color NavajoWhite
		{
			get
			{
				return new Color(KnownColor.NavajoWhite);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF000080.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000176 RID: 374 RVA: 0x000071DE File Offset: 0x000053DE
		public static Color Navy
		{
			get
			{
				return new Color(KnownColor.Navy);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFDF5E6.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000177 RID: 375 RVA: 0x000071E7 File Offset: 0x000053E7
		public static Color OldLace
		{
			get
			{
				return new Color(KnownColor.OldLace);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF808000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000178 RID: 376 RVA: 0x000071F0 File Offset: 0x000053F0
		public static Color Olive
		{
			get
			{
				return new Color(KnownColor.Olive);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF6B8E23.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000179 RID: 377 RVA: 0x000071F9 File Offset: 0x000053F9
		public static Color OliveDrab
		{
			get
			{
				return new Color(KnownColor.OliveDrab);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFA500.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00007202 File Offset: 0x00005402
		public static Color Orange
		{
			get
			{
				return new Color(KnownColor.Orange);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF4500.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600017B RID: 379 RVA: 0x0000720B File Offset: 0x0000540B
		public static Color OrangeRed
		{
			get
			{
				return new Color(KnownColor.OrangeRed);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDA70D6.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000101 RID: 257
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00007217 File Offset: 0x00005417
		public static Color Orchid
		{
			get
			{
				return new Color(KnownColor.Orchid);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFEEE8AA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x0600017D RID: 381 RVA: 0x00007223 File Offset: 0x00005423
		public static Color PaleGoldenrod
		{
			get
			{
				return new Color(KnownColor.PaleGoldenrod);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF98FB98.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000722F File Offset: 0x0000542F
		public static Color PaleGreen
		{
			get
			{
				return new Color(KnownColor.PaleGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFAFEEEE.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000104 RID: 260
		// (get) Token: 0x0600017F RID: 383 RVA: 0x0000723B File Offset: 0x0000543B
		public static Color PaleTurquoise
		{
			get
			{
				return new Color(KnownColor.PaleTurquoise);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDB7093.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000180 RID: 384 RVA: 0x00007247 File Offset: 0x00005447
		public static Color PaleVioletRed
		{
			get
			{
				return new Color(KnownColor.PaleVioletRed);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFEFD5.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000181 RID: 385 RVA: 0x00007253 File Offset: 0x00005453
		public static Color PapayaWhip
		{
			get
			{
				return new Color(KnownColor.PapayaWhip);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFDAB9.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000725F File Offset: 0x0000545F
		public static Color PeachPuff
		{
			get
			{
				return new Color(KnownColor.PeachPuff);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFCD853F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000726B File Offset: 0x0000546B
		public static Color Peru
		{
			get
			{
				return new Color(KnownColor.Peru);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFC0CB.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00007277 File Offset: 0x00005477
		public static Color Pink
		{
			get
			{
				return new Color(KnownColor.Pink);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFDDA0DD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000185 RID: 389 RVA: 0x00007283 File Offset: 0x00005483
		public static Color Plum
		{
			get
			{
				return new Color(KnownColor.Plum);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFB0E0E6.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000186 RID: 390 RVA: 0x0000728F File Offset: 0x0000548F
		public static Color PowderBlue
		{
			get
			{
				return new Color(KnownColor.PowderBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF800080.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000729B File Offset: 0x0000549B
		public static Color Purple
		{
			get
			{
				return new Color(KnownColor.Purple);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF0000.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000188 RID: 392 RVA: 0x000072A7 File Offset: 0x000054A7
		public static Color Red
		{
			get
			{
				return new Color(KnownColor.Red);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFBC8F8F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000189 RID: 393 RVA: 0x000072B3 File Offset: 0x000054B3
		public static Color RosyBrown
		{
			get
			{
				return new Color(KnownColor.RosyBrown);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF4169E1.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x0600018A RID: 394 RVA: 0x000072BF File Offset: 0x000054BF
		public static Color RoyalBlue
		{
			get
			{
				return new Color(KnownColor.RoyalBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF8B4513.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600018B RID: 395 RVA: 0x000072CB File Offset: 0x000054CB
		public static Color SaddleBrown
		{
			get
			{
				return new Color(KnownColor.SaddleBrown);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFA8072.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600018C RID: 396 RVA: 0x000072D7 File Offset: 0x000054D7
		public static Color Salmon
		{
			get
			{
				return new Color(KnownColor.Salmon);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF4A460.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600018D RID: 397 RVA: 0x000072E3 File Offset: 0x000054E3
		public static Color SandyBrown
		{
			get
			{
				return new Color(KnownColor.SandyBrown);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF2E8B57.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000072EF File Offset: 0x000054EF
		public static Color SeaGreen
		{
			get
			{
				return new Color(KnownColor.SeaGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFF5EE.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600018F RID: 399 RVA: 0x000072FB File Offset: 0x000054FB
		public static Color SeaShell
		{
			get
			{
				return new Color(KnownColor.SeaShell);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFA0522D.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000115 RID: 277
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00007307 File Offset: 0x00005507
		public static Color Sienna
		{
			get
			{
				return new Color(KnownColor.Sienna);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFC0C0C0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00007313 File Offset: 0x00005513
		public static Color Silver
		{
			get
			{
				return new Color(KnownColor.Silver);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF87CEEB.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000192 RID: 402 RVA: 0x0000731F File Offset: 0x0000551F
		public static Color SkyBlue
		{
			get
			{
				return new Color(KnownColor.SkyBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF6A5ACD.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000193 RID: 403 RVA: 0x0000732B File Offset: 0x0000552B
		public static Color SlateBlue
		{
			get
			{
				return new Color(KnownColor.SlateBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF708090.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00007337 File Offset: 0x00005537
		public static Color SlateGray
		{
			get
			{
				return new Color(KnownColor.SlateGray);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFAFA.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00007343 File Offset: 0x00005543
		public static Color Snow
		{
			get
			{
				return new Color(KnownColor.Snow);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF00FF7F.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000734F File Offset: 0x0000554F
		public static Color SpringGreen
		{
			get
			{
				return new Color(KnownColor.SpringGreen);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF4682B4.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000197 RID: 407 RVA: 0x0000735B File Offset: 0x0000555B
		public static Color SteelBlue
		{
			get
			{
				return new Color(KnownColor.SteelBlue);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFD2B48C.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00007367 File Offset: 0x00005567
		public static Color Tan
		{
			get
			{
				return new Color(KnownColor.Tan);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF008080.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700011E RID: 286
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00007373 File Offset: 0x00005573
		public static Color Teal
		{
			get
			{
				return new Color(KnownColor.Teal);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFD8BFD8.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000737F File Offset: 0x0000557F
		public static Color Thistle
		{
			get
			{
				return new Color(KnownColor.Thistle);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFF6347.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600019B RID: 411 RVA: 0x0000738B File Offset: 0x0000558B
		public static Color Tomato
		{
			get
			{
				return new Color(KnownColor.Tomato);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF40E0D0.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00007397 File Offset: 0x00005597
		public static Color Turquoise
		{
			get
			{
				return new Color(KnownColor.Turquoise);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFEE82EE.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000073A3 File Offset: 0x000055A3
		public static Color Violet
		{
			get
			{
				return new Color(KnownColor.Violet);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF5DEB3.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600019E RID: 414 RVA: 0x000073AF File Offset: 0x000055AF
		public static Color Wheat
		{
			get
			{
				return new Color(KnownColor.Wheat);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFFFF.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600019F RID: 415 RVA: 0x000073BB File Offset: 0x000055BB
		public static Color White
		{
			get
			{
				return new Color(KnownColor.White);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFF5F5F5.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000125 RID: 293
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000073C7 File Offset: 0x000055C7
		public static Color WhiteSmoke
		{
			get
			{
				return new Color(KnownColor.WhiteSmoke);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FFFFFF00.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000073D3 File Offset: 0x000055D3
		public static Color Yellow
		{
			get
			{
				return new Color(KnownColor.Yellow);
			}
		}

		/// <summary>Gets a system-defined color that has an ARGB value of #FF9ACD32.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> representing a system-defined color.</returns>
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x000073DF File Offset: 0x000055DF
		public static Color YellowGreen
		{
			get
			{
				return new Color(KnownColor.YellowGreen);
			}
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x000073EB File Offset: 0x000055EB
		internal Color(KnownColor knownColor)
		{
			this.value = 0L;
			this.state = Color.StateKnownColorValid;
			this.name = null;
			this.knownColor = (short)knownColor;
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000740F File Offset: 0x0000560F
		private Color(long value, short state, string name, KnownColor knownColor)
		{
			this.value = value;
			this.state = state;
			this.name = name;
			this.knownColor = (short)knownColor;
		}

		/// <summary>Gets the red component value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The red component value of this <see cref="T:System.Drawing.Color" />.</returns>
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0000742F File Offset: 0x0000562F
		public byte R
		{
			get
			{
				return (byte)(this.Value >> 16 & 255L);
			}
		}

		/// <summary>Gets the green component value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The green component value of this <see cref="T:System.Drawing.Color" />.</returns>
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007442 File Offset: 0x00005642
		public byte G
		{
			get
			{
				return (byte)(this.Value >> 8 & 255L);
			}
		}

		/// <summary>Gets the blue component value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The blue component value of this <see cref="T:System.Drawing.Color" />.</returns>
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x00007454 File Offset: 0x00005654
		public byte B
		{
			get
			{
				return (byte)(this.Value & 255L);
			}
		}

		/// <summary>Gets the alpha component value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The alpha component value of this <see cref="T:System.Drawing.Color" />.</returns>
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007464 File Offset: 0x00005664
		public byte A
		{
			get
			{
				return (byte)(this.Value >> 24 & 255L);
			}
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Color" /> structure is a predefined color. Predefined colors are represented by the elements of the <see cref="T:System.Drawing.KnownColor" /> enumeration.</summary>
		/// <returns>
		///     <see langword="true" /> if this <see cref="T:System.Drawing.Color" /> was created from a predefined color by using either the <see cref="M:System.Drawing.Color.FromName(System.String)" /> method or the <see cref="M:System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor)" /> method; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x00007477 File Offset: 0x00005677
		public bool IsKnownColor
		{
			get
			{
				return (this.state & Color.StateKnownColorValid) != 0;
			}
		}

		/// <summary>Specifies whether this <see cref="T:System.Drawing.Color" /> structure is uninitialized.</summary>
		/// <returns>This property returns <see langword="true" /> if this color is uninitialized; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007488 File Offset: 0x00005688
		public bool IsEmpty
		{
			get
			{
				return this.state == 0;
			}
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Color" /> structure is a named color or a member of the <see cref="T:System.Drawing.KnownColor" /> enumeration.</summary>
		/// <returns>
		///     <see langword="true" /> if this <see cref="T:System.Drawing.Color" /> was created by using either the <see cref="M:System.Drawing.Color.FromName(System.String)" /> method or the <see cref="M:System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor)" /> method; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060001AB RID: 427 RVA: 0x00007493 File Offset: 0x00005693
		public bool IsNamedColor
		{
			get
			{
				return (this.state & Color.StateNameValid) != 0 || this.IsKnownColor;
			}
		}

		/// <summary>Gets a value indicating whether this <see cref="T:System.Drawing.Color" /> structure is a system color. A system color is a color that is used in a Windows display element. System colors are represented by elements of the <see cref="T:System.Drawing.KnownColor" /> enumeration.</summary>
		/// <returns>
		///     <see langword="true" /> if this <see cref="T:System.Drawing.Color" /> was created from a system color by using either the <see cref="M:System.Drawing.Color.FromName(System.String)" /> method or the <see cref="M:System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor)" /> method; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060001AC RID: 428 RVA: 0x000074AB File Offset: 0x000056AB
		public bool IsSystemColor
		{
			get
			{
				return this.IsKnownColor && (this.knownColor <= 26 || this.knownColor > 167);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000074D0 File Offset: 0x000056D0
		private string NameAndARGBValue
		{
			get
			{
				return string.Format(CultureInfo.CurrentCulture, "{{Name={0}, ARGB=({1}, {2}, {3}, {4})}}", new object[]
				{
					this.Name,
					this.A,
					this.R,
					this.G,
					this.B
				});
			}
		}

		/// <summary>Gets the name of this <see cref="T:System.Drawing.Color" />.</summary>
		/// <returns>The name of this <see cref="T:System.Drawing.Color" />.</returns>
		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00007534 File Offset: 0x00005734
		public string Name
		{
			get
			{
				if ((this.state & Color.StateNameValid) != 0)
				{
					return this.name;
				}
				if (!this.IsKnownColor)
				{
					return Convert.ToString(this.value, 16);
				}
				string text = KnownColorTable.KnownColorToName((KnownColor)this.knownColor);
				if (text != null)
				{
					return text;
				}
				return ((KnownColor)this.knownColor).ToString();
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00007591 File Offset: 0x00005791
		private long Value
		{
			get
			{
				if ((this.state & Color.StateValueMask) != 0)
				{
					return this.value;
				}
				if (this.IsKnownColor)
				{
					return (long)KnownColorTable.KnownColorToArgb((KnownColor)this.knownColor);
				}
				return Color.NotDefinedValue;
			}
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x000075C4 File Offset: 0x000057C4
		private static void CheckByte(int value, string name)
		{
			if (value < 0 || value > 255)
			{
				throw new ArgumentException(SR.GetString("InvalidEx2BoundArgument", new object[]
				{
					name,
					value,
					0,
					255
				}));
			}
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00007616 File Offset: 0x00005816
		private static long MakeArgb(byte alpha, byte red, byte green, byte blue)
		{
			return (long)((ulong)((int)red << 16 | (int)green << 8 | (int)blue | (int)alpha << 24) & (ulong)-1);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from a 32-bit ARGB value.</summary>
		/// <param name="argb">A value specifying the 32-bit ARGB value. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> structure that this method creates.</returns>
		// Token: 0x060001B2 RID: 434 RVA: 0x0000762B File Offset: 0x0000582B
		public static Color FromArgb(int argb)
		{
			return new Color((long)argb & (long)((ulong)-1), Color.StateARGBValueValid, null, (KnownColor)0);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from the four ARGB component (alpha, red, green, and blue) values. Although this method allows a 32-bit value to be passed for each component, the value of each component is limited to 8 bits.</summary>
		/// <param name="alpha">The alpha component. Valid values are 0 through 255. </param>
		/// <param name="red">The red component. Valid values are 0 through 255. </param>
		/// <param name="green">The green component. Valid values are 0 through 255. </param>
		/// <param name="blue">The blue component. Valid values are 0 through 255. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="alpha" />, <paramref name="red" />, <paramref name="green" />, or <paramref name="blue" /> is less than 0 or greater than 255.</exception>
		// Token: 0x060001B3 RID: 435 RVA: 0x00007640 File Offset: 0x00005840
		public static Color FromArgb(int alpha, int red, int green, int blue)
		{
			Color.CheckByte(alpha, "alpha");
			Color.CheckByte(red, "red");
			Color.CheckByte(green, "green");
			Color.CheckByte(blue, "blue");
			return new Color(Color.MakeArgb((byte)alpha, (byte)red, (byte)green, (byte)blue), Color.StateARGBValueValid, null, (KnownColor)0);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from the specified <see cref="T:System.Drawing.Color" /> structure, but with the new specified alpha value. Although this method allows a 32-bit value to be passed for the alpha value, the value is limited to 8 bits.</summary>
		/// <param name="alpha">The alpha value for the new <see cref="T:System.Drawing.Color" />. Valid values are 0 through 255. </param>
		/// <param name="baseColor">The <see cref="T:System.Drawing.Color" /> from which to create the new <see cref="T:System.Drawing.Color" />. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="alpha" /> is less than 0 or greater than 255.</exception>
		// Token: 0x060001B4 RID: 436 RVA: 0x00007692 File Offset: 0x00005892
		public static Color FromArgb(int alpha, Color baseColor)
		{
			Color.CheckByte(alpha, "alpha");
			return new Color(Color.MakeArgb((byte)alpha, baseColor.R, baseColor.G, baseColor.B), Color.StateARGBValueValid, null, (KnownColor)0);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from the specified 8-bit color values (red, green, and blue). The alpha value is implicitly 255 (fully opaque). Although this method allows a 32-bit value to be passed for each color component, the value of each component is limited to 8 bits.</summary>
		/// <param name="red">The red component value for the new <see cref="T:System.Drawing.Color" />. Valid values are 0 through 255. </param>
		/// <param name="green">The green component value for the new <see cref="T:System.Drawing.Color" />. Valid values are 0 through 255. </param>
		/// <param name="blue">The blue component value for the new <see cref="T:System.Drawing.Color" />. Valid values are 0 through 255. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that this method creates.</returns>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="red" />, <paramref name="green" />, or <paramref name="blue" /> is less than 0 or greater than 255.</exception>
		// Token: 0x060001B5 RID: 437 RVA: 0x000076C7 File Offset: 0x000058C7
		public static Color FromArgb(int red, int green, int blue)
		{
			return Color.FromArgb(255, red, green, blue);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from the specified predefined color.</summary>
		/// <param name="color">An element of the <see cref="T:System.Drawing.KnownColor" /> enumeration. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that this method creates.</returns>
		// Token: 0x060001B6 RID: 438 RVA: 0x000076D6 File Offset: 0x000058D6
		public static Color FromKnownColor(KnownColor color)
		{
			if (!ClientUtils.IsEnumValid(color, (int)color, 1, 174))
			{
				return Color.FromName(color.ToString());
			}
			return new Color(color);
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Color" /> structure from the specified name of a predefined color.</summary>
		/// <param name="name">A string that is the name of a predefined color. Valid names are the same as the names of the elements of the <see cref="T:System.Drawing.KnownColor" /> enumeration. </param>
		/// <returns>The <see cref="T:System.Drawing.Color" /> that this method creates.</returns>
		// Token: 0x060001B7 RID: 439 RVA: 0x00007708 File Offset: 0x00005908
		public static Color FromName(string name)
		{
			object namedColor = ColorConverter.GetNamedColor(name);
			if (namedColor != null)
			{
				return (Color)namedColor;
			}
			return new Color(Color.NotDefinedValue, Color.StateNameValid, name, (KnownColor)0);
		}

		/// <summary>Gets the hue-saturation-brightness (HSB) brightness value for this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The brightness of this <see cref="T:System.Drawing.Color" />. The brightness ranges from 0.0 through 1.0, where 0.0 represents black and 1.0 represents white.</returns>
		// Token: 0x060001B8 RID: 440 RVA: 0x00007738 File Offset: 0x00005938
		public float GetBrightness()
		{
			float num = (float)this.R / 255f;
			float num2 = (float)this.G / 255f;
			float num3 = (float)this.B / 255f;
			float num4 = num;
			float num5 = num;
			if (num2 > num4)
			{
				num4 = num2;
			}
			if (num3 > num4)
			{
				num4 = num3;
			}
			if (num2 < num5)
			{
				num5 = num2;
			}
			if (num3 < num5)
			{
				num5 = num3;
			}
			return (num4 + num5) / 2f;
		}

		/// <summary>Gets the hue-saturation-brightness (HSB) hue value, in degrees, for this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The hue, in degrees, of this <see cref="T:System.Drawing.Color" />. The hue is measured in degrees, ranging from 0.0 through 360.0, in HSB color space.</returns>
		// Token: 0x060001B9 RID: 441 RVA: 0x0000779C File Offset: 0x0000599C
		public float GetHue()
		{
			if (this.R == this.G && this.G == this.B)
			{
				return 0f;
			}
			float num = (float)this.R / 255f;
			float num2 = (float)this.G / 255f;
			float num3 = (float)this.B / 255f;
			float num4 = 0f;
			float num5 = num;
			float num6 = num;
			if (num2 > num5)
			{
				num5 = num2;
			}
			if (num3 > num5)
			{
				num5 = num3;
			}
			if (num2 < num6)
			{
				num6 = num2;
			}
			if (num3 < num6)
			{
				num6 = num3;
			}
			float num7 = num5 - num6;
			if (num == num5)
			{
				num4 = (num2 - num3) / num7;
			}
			else if (num2 == num5)
			{
				num4 = 2f + (num3 - num) / num7;
			}
			else if (num3 == num5)
			{
				num4 = 4f + (num - num2) / num7;
			}
			num4 *= 60f;
			if (num4 < 0f)
			{
				num4 += 360f;
			}
			return num4;
		}

		/// <summary>Gets the hue-saturation-brightness (HSB) saturation value for this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The saturation of this <see cref="T:System.Drawing.Color" />. The saturation ranges from 0.0 through 1.0, where 0.0 is grayscale and 1.0 is the most saturated.</returns>
		// Token: 0x060001BA RID: 442 RVA: 0x00007878 File Offset: 0x00005A78
		public float GetSaturation()
		{
			float num = (float)this.R / 255f;
			float num2 = (float)this.G / 255f;
			float num3 = (float)this.B / 255f;
			float result = 0f;
			float num4 = num;
			float num5 = num;
			if (num2 > num4)
			{
				num4 = num2;
			}
			if (num3 > num4)
			{
				num4 = num3;
			}
			if (num2 < num5)
			{
				num5 = num2;
			}
			if (num3 < num5)
			{
				num5 = num3;
			}
			if (num4 != num5)
			{
				float num6 = (num4 + num5) / 2f;
				if ((double)num6 <= 0.5)
				{
					result = (num4 - num5) / (num4 + num5);
				}
				else
				{
					result = (num4 - num5) / (2f - num4 - num5);
				}
			}
			return result;
		}

		/// <summary>Gets the 32-bit ARGB value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>The 32-bit ARGB value of this <see cref="T:System.Drawing.Color" />.</returns>
		// Token: 0x060001BB RID: 443 RVA: 0x00007916 File Offset: 0x00005B16
		public int ToArgb()
		{
			return (int)this.Value;
		}

		/// <summary>Gets the <see cref="T:System.Drawing.KnownColor" /> value of this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>An element of the <see cref="T:System.Drawing.KnownColor" /> enumeration, if the <see cref="T:System.Drawing.Color" /> is created from a predefined color by using either the <see cref="M:System.Drawing.Color.FromName(System.String)" /> method or the <see cref="M:System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor)" /> method; otherwise, 0.</returns>
		// Token: 0x060001BC RID: 444 RVA: 0x0000791F File Offset: 0x00005B1F
		public KnownColor ToKnownColor()
		{
			return (KnownColor)this.knownColor;
		}

		/// <summary>Converts this <see cref="T:System.Drawing.Color" /> structure to a human-readable string.</summary>
		/// <returns>A string that is the name of this <see cref="T:System.Drawing.Color" />, if the <see cref="T:System.Drawing.Color" /> is created from a predefined color by using either the <see cref="M:System.Drawing.Color.FromName(System.String)" /> method or the <see cref="M:System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor)" /> method; otherwise, a string that consists of the ARGB component names and their values.</returns>
		// Token: 0x060001BD RID: 445 RVA: 0x00007928 File Offset: 0x00005B28
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			stringBuilder.Append(base.GetType().Name);
			stringBuilder.Append(" [");
			if ((this.state & Color.StateNameValid) != 0)
			{
				stringBuilder.Append(this.Name);
			}
			else if ((this.state & Color.StateKnownColorValid) != 0)
			{
				stringBuilder.Append(this.Name);
			}
			else if ((this.state & Color.StateValueMask) != 0)
			{
				stringBuilder.Append("A=");
				stringBuilder.Append(this.A);
				stringBuilder.Append(", R=");
				stringBuilder.Append(this.R);
				stringBuilder.Append(", G=");
				stringBuilder.Append(this.G);
				stringBuilder.Append(", B=");
				stringBuilder.Append(this.B);
			}
			else
			{
				stringBuilder.Append("Empty");
			}
			stringBuilder.Append("]");
			return stringBuilder.ToString();
		}

		/// <summary>Tests whether two specified <see cref="T:System.Drawing.Color" /> structures are equivalent.</summary>
		/// <param name="left">The <see cref="T:System.Drawing.Color" /> that is to the left of the equality operator. </param>
		/// <param name="right">The <see cref="T:System.Drawing.Color" /> that is to the right of the equality operator. </param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Drawing.Color" /> structures are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x060001BE RID: 446 RVA: 0x00007A38 File Offset: 0x00005C38
		public static bool operator ==(Color left, Color right)
		{
			return left.value == right.value && left.state == right.state && left.knownColor == right.knownColor && (left.name == right.name || (left.name != null && right.name != null && left.name.Equals(right.name)));
		}

		/// <summary>Tests whether two specified <see cref="T:System.Drawing.Color" /> structures are different.</summary>
		/// <param name="left">The <see cref="T:System.Drawing.Color" /> that is to the left of the inequality operator. </param>
		/// <param name="right">The <see cref="T:System.Drawing.Color" /> that is to the right of the inequality operator. </param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Drawing.Color" /> structures are different; otherwise, <see langword="false" />.</returns>
		// Token: 0x060001BF RID: 447 RVA: 0x00007AA9 File Offset: 0x00005CA9
		public static bool operator !=(Color left, Color right)
		{
			return !(left == right);
		}

		/// <summary>Tests whether the specified object is a <see cref="T:System.Drawing.Color" /> structure and is equivalent to this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <param name="obj">The object to test. </param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="obj" /> is a <see cref="T:System.Drawing.Color" /> structure equivalent to this <see cref="T:System.Drawing.Color" /> structure; otherwise, <see langword="false" />.</returns>
		// Token: 0x060001C0 RID: 448 RVA: 0x00007AB8 File Offset: 0x00005CB8
		public override bool Equals(object obj)
		{
			if (obj is Color)
			{
				Color color = (Color)obj;
				if (this.value == color.value && this.state == color.state && this.knownColor == color.knownColor)
				{
					return this.name == color.name || (this.name != null && color.name != null && this.name.Equals(this.name));
				}
			}
			return false;
		}

		/// <summary>Returns a hash code for this <see cref="T:System.Drawing.Color" /> structure.</summary>
		/// <returns>An integer value that specifies the hash code for this <see cref="T:System.Drawing.Color" />.</returns>
		// Token: 0x060001C1 RID: 449 RVA: 0x00007B38 File Offset: 0x00005D38
		public override int GetHashCode()
		{
			return this.value.GetHashCode() ^ this.state.GetHashCode() ^ this.knownColor.GetHashCode();
		}

		/// <summary>Represents a color that is <see langword="null" />.</summary>
		// Token: 0x04000144 RID: 324
		public static readonly Color Empty = default(Color);

		// Token: 0x04000145 RID: 325
		private static short StateKnownColorValid = 1;

		// Token: 0x04000146 RID: 326
		private static short StateARGBValueValid = 2;

		// Token: 0x04000147 RID: 327
		private static short StateValueMask = Color.StateARGBValueValid;

		// Token: 0x04000148 RID: 328
		private static short StateNameValid = 8;

		// Token: 0x04000149 RID: 329
		private static long NotDefinedValue = 0L;

		// Token: 0x0400014A RID: 330
		private const int ARGBAlphaShift = 24;

		// Token: 0x0400014B RID: 331
		private const int ARGBRedShift = 16;

		// Token: 0x0400014C RID: 332
		private const int ARGBGreenShift = 8;

		// Token: 0x0400014D RID: 333
		private const int ARGBBlueShift = 0;

		// Token: 0x0400014E RID: 334
		private readonly string name;

		// Token: 0x0400014F RID: 335
		private readonly long value;

		// Token: 0x04000150 RID: 336
		private readonly short knownColor;

		// Token: 0x04000151 RID: 337
		private readonly short state;
	}
}
