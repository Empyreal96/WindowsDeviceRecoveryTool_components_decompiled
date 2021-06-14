using System;

namespace System.Drawing
{
	/// <summary>Brushes for all the standard colors. This class cannot be inherited.</summary>
	// Token: 0x02000013 RID: 19
	public sealed class Brushes
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00003800 File Offset: 0x00001A00
		private Brushes()
		{
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000064 RID: 100 RVA: 0x00003808 File Offset: 0x00001A08
		public static Brush Transparent
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.TransparentKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Transparent);
					SafeNativeMethods.Gdip.ThreadData[Brushes.TransparentKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000065 RID: 101 RVA: 0x0000384C File Offset: 0x00001A4C
		public static Brush AliceBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.AliceBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.AliceBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.AliceBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000066 RID: 102 RVA: 0x00003890 File Offset: 0x00001A90
		public static Brush AntiqueWhite
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.AntiqueWhiteKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.AntiqueWhite);
					SafeNativeMethods.Gdip.ThreadData[Brushes.AntiqueWhiteKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000067 RID: 103 RVA: 0x000038D4 File Offset: 0x00001AD4
		public static Brush Aqua
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.AquaKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Aqua);
					SafeNativeMethods.Gdip.ThreadData[Brushes.AquaKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00003918 File Offset: 0x00001B18
		public static Brush Aquamarine
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.AquamarineKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Aquamarine);
					SafeNativeMethods.Gdip.ThreadData[Brushes.AquamarineKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000069 RID: 105 RVA: 0x0000395C File Offset: 0x00001B5C
		public static Brush Azure
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.AzureKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Azure);
					SafeNativeMethods.Gdip.ThreadData[Brushes.AzureKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000039A0 File Offset: 0x00001BA0
		public static Brush Beige
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BeigeKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Beige);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BeigeKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600006B RID: 107 RVA: 0x000039E4 File Offset: 0x00001BE4
		public static Brush Bisque
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BisqueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Bisque);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BisqueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00003A28 File Offset: 0x00001C28
		public static Brush Black
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BlackKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Black);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BlackKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600006D RID: 109 RVA: 0x00003A6C File Offset: 0x00001C6C
		public static Brush BlanchedAlmond
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BlanchedAlmondKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.BlanchedAlmond);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BlanchedAlmondKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00003AB0 File Offset: 0x00001CB0
		public static Brush Blue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Blue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00003AF4 File Offset: 0x00001CF4
		public static Brush BlueViolet
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BlueVioletKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.BlueViolet);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BlueVioletKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003B38 File Offset: 0x00001D38
		public static Brush Brown
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BrownKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Brown);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BrownKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000071 RID: 113 RVA: 0x00003B7C File Offset: 0x00001D7C
		public static Brush BurlyWood
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.BurlyWoodKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.BurlyWood);
					SafeNativeMethods.Gdip.ThreadData[Brushes.BurlyWoodKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003BC0 File Offset: 0x00001DC0
		public static Brush CadetBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.CadetBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.CadetBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.CadetBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000073 RID: 115 RVA: 0x00003C04 File Offset: 0x00001E04
		public static Brush Chartreuse
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.ChartreuseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Chartreuse);
					SafeNativeMethods.Gdip.ThreadData[Brushes.ChartreuseKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003C48 File Offset: 0x00001E48
		public static Brush Chocolate
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.ChocolateKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Chocolate);
					SafeNativeMethods.Gdip.ThreadData[Brushes.ChocolateKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00003C8C File Offset: 0x00001E8C
		public static Brush Coral
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.ChoralKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Coral);
					SafeNativeMethods.Gdip.ThreadData[Brushes.ChoralKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003CD0 File Offset: 0x00001ED0
		public static Brush CornflowerBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.CornflowerBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.CornflowerBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.CornflowerBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003D14 File Offset: 0x00001F14
		public static Brush Cornsilk
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.CornsilkKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Cornsilk);
					SafeNativeMethods.Gdip.ThreadData[Brushes.CornsilkKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000078 RID: 120 RVA: 0x00003D58 File Offset: 0x00001F58
		public static Brush Crimson
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.CrimsonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Crimson);
					SafeNativeMethods.Gdip.ThreadData[Brushes.CrimsonKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003D9C File Offset: 0x00001F9C
		public static Brush Cyan
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.CyanKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Cyan);
					SafeNativeMethods.Gdip.ThreadData[Brushes.CyanKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00003DE0 File Offset: 0x00001FE0
		public static Brush DarkBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600007B RID: 123 RVA: 0x00003E24 File Offset: 0x00002024
		public static Brush DarkCyan
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkCyanKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkCyan);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkCyanKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003E68 File Offset: 0x00002068
		public static Brush DarkGoldenrod
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGoldenrodKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkGoldenrod);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGoldenrodKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003EAC File Offset: 0x000020AC
		public static Brush DarkGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGrayKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600007E RID: 126 RVA: 0x00003EF0 File Offset: 0x000020F0
		public static Brush DarkGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007F RID: 127 RVA: 0x00003F34 File Offset: 0x00002134
		public static Brush DarkKhaki
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkKhakiKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkKhaki);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkKhakiKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00003F78 File Offset: 0x00002178
		public static Brush DarkMagenta
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkMagentaKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkMagenta);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkMagentaKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00003FBC File Offset: 0x000021BC
		public static Brush DarkOliveGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOliveGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkOliveGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOliveGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000082 RID: 130 RVA: 0x00004000 File Offset: 0x00002200
		public static Brush DarkOrange
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOrangeKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkOrange);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOrangeKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000083 RID: 131 RVA: 0x00004044 File Offset: 0x00002244
		public static Brush DarkOrchid
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOrchidKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkOrchid);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkOrchidKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00004088 File Offset: 0x00002288
		public static Brush DarkRed
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkRedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkRed);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkRedKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000040CC File Offset: 0x000022CC
		public static Brush DarkSalmon
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSalmonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkSalmon);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSalmonKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00004110 File Offset: 0x00002310
		public static Brush DarkSeaGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSeaGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkSeaGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSeaGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000087 RID: 135 RVA: 0x00004154 File Offset: 0x00002354
		public static Brush DarkSlateBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSlateBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkSlateBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSlateBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000088 RID: 136 RVA: 0x00004198 File Offset: 0x00002398
		public static Brush DarkSlateGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSlateGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkSlateGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkSlateGrayKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000041DC File Offset: 0x000023DC
		public static Brush DarkTurquoise
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkTurquoiseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkTurquoise);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkTurquoiseKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600008A RID: 138 RVA: 0x00004220 File Offset: 0x00002420
		public static Brush DarkViolet
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DarkVioletKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DarkViolet);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DarkVioletKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00004264 File Offset: 0x00002464
		public static Brush DeepPink
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DeepPinkKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DeepPink);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DeepPinkKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000042A8 File Offset: 0x000024A8
		public static Brush DeepSkyBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DeepSkyBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DeepSkyBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DeepSkyBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000042EC File Offset: 0x000024EC
		public static Brush DimGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DimGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DimGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DimGrayKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000033 RID: 51
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004330 File Offset: 0x00002530
		public static Brush DodgerBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.DodgerBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.DodgerBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.DodgerBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00004374 File Offset: 0x00002574
		public static Brush Firebrick
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.FirebrickKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Firebrick);
					SafeNativeMethods.Gdip.ThreadData[Brushes.FirebrickKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000090 RID: 144 RVA: 0x000043B8 File Offset: 0x000025B8
		public static Brush FloralWhite
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.FloralWhiteKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.FloralWhite);
					SafeNativeMethods.Gdip.ThreadData[Brushes.FloralWhiteKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000091 RID: 145 RVA: 0x000043FC File Offset: 0x000025FC
		public static Brush ForestGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.ForestGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.ForestGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.ForestGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000092 RID: 146 RVA: 0x00004440 File Offset: 0x00002640
		public static Brush Fuchsia
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.FuchiaKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Fuchsia);
					SafeNativeMethods.Gdip.ThreadData[Brushes.FuchiaKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00004484 File Offset: 0x00002684
		public static Brush Gainsboro
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GainsboroKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Gainsboro);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GainsboroKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000094 RID: 148 RVA: 0x000044C8 File Offset: 0x000026C8
		public static Brush GhostWhite
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GhostWhiteKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.GhostWhite);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GhostWhiteKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000095 RID: 149 RVA: 0x0000450C File Offset: 0x0000270C
		public static Brush Gold
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GoldKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Gold);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GoldKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00004550 File Offset: 0x00002750
		public static Brush Goldenrod
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GoldenrodKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Goldenrod);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GoldenrodKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00004594 File Offset: 0x00002794
		public static Brush Gray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Gray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GrayKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000098 RID: 152 RVA: 0x000045D8 File Offset: 0x000027D8
		public static Brush Green
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Green);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000461C File Offset: 0x0000281C
		public static Brush GreenYellow
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.GreenYellowKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.GreenYellow);
					SafeNativeMethods.Gdip.ThreadData[Brushes.GreenYellowKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600009A RID: 154 RVA: 0x00004660 File Offset: 0x00002860
		public static Brush Honeydew
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.HoneydewKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Honeydew);
					SafeNativeMethods.Gdip.ThreadData[Brushes.HoneydewKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000046A4 File Offset: 0x000028A4
		public static Brush HotPink
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.HotPinkKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.HotPink);
					SafeNativeMethods.Gdip.ThreadData[Brushes.HotPinkKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600009C RID: 156 RVA: 0x000046E8 File Offset: 0x000028E8
		public static Brush IndianRed
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.IndianRedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.IndianRed);
					SafeNativeMethods.Gdip.ThreadData[Brushes.IndianRedKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600009D RID: 157 RVA: 0x0000472C File Offset: 0x0000292C
		public static Brush Indigo
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.IndigoKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Indigo);
					SafeNativeMethods.Gdip.ThreadData[Brushes.IndigoKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00004770 File Offset: 0x00002970
		public static Brush Ivory
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.IvoryKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Ivory);
					SafeNativeMethods.Gdip.ThreadData[Brushes.IvoryKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600009F RID: 159 RVA: 0x000047B4 File Offset: 0x000029B4
		public static Brush Khaki
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.KhakiKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Khaki);
					SafeNativeMethods.Gdip.ThreadData[Brushes.KhakiKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x000047F8 File Offset: 0x000029F8
		public static Brush Lavender
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LavenderKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Lavender);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LavenderKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000483C File Offset: 0x00002A3C
		public static Brush LavenderBlush
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LavenderBlushKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LavenderBlush);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LavenderBlushKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004880 File Offset: 0x00002A80
		public static Brush LawnGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LawnGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LawnGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LawnGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x000048C4 File Offset: 0x00002AC4
		public static Brush LemonChiffon
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LemonChiffonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LemonChiffon);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LemonChiffonKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004908 File Offset: 0x00002B08
		public static Brush LightBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x0000494C File Offset: 0x00002B4C
		public static Brush LightCoral
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightCoralKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightCoral);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightCoralKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004990 File Offset: 0x00002B90
		public static Brush LightCyan
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightCyanKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightCyan);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightCyanKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000049D4 File Offset: 0x00002BD4
		public static Brush LightGoldenrodYellow
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightGoldenrodYellowKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightGoldenrodYellow);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightGoldenrodYellowKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004A18 File Offset: 0x00002C18
		public static Brush LightGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x00004A5C File Offset: 0x00002C5C
		public static Brush LightGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightGrayKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00004AA0 File Offset: 0x00002CA0
		public static Brush LightPink
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightPinkKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightPink);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightPinkKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00004AE4 File Offset: 0x00002CE4
		public static Brush LightSalmon
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightSalmonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightSalmon);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightSalmonKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00004B28 File Offset: 0x00002D28
		public static Brush LightSeaGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightSeaGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightSeaGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightSeaGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00004B6C File Offset: 0x00002D6C
		public static Brush LightSkyBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightSkyBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightSkyBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightSkyBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000AE RID: 174 RVA: 0x00004BB0 File Offset: 0x00002DB0
		public static Brush LightSlateGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightSlateGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightSlateGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightSlateGrayKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00004BF4 File Offset: 0x00002DF4
		public static Brush LightSteelBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightSteelBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightSteelBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightSteelBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060000B0 RID: 176 RVA: 0x00004C38 File Offset: 0x00002E38
		public static Brush LightYellow
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LightYellowKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LightYellow);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LightYellowKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004C7C File Offset: 0x00002E7C
		public static Brush Lime
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LimeKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Lime);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LimeKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060000B2 RID: 178 RVA: 0x00004CC0 File Offset: 0x00002EC0
		public static Brush LimeGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LimeGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.LimeGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LimeGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060000B3 RID: 179 RVA: 0x00004D04 File Offset: 0x00002F04
		public static Brush Linen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.LinenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Linen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.LinenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00004D48 File Offset: 0x00002F48
		public static Brush Magenta
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MagentaKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Magenta);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MagentaKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060000B5 RID: 181 RVA: 0x00004D8C File Offset: 0x00002F8C
		public static Brush Maroon
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MaroonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Maroon);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MaroonKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x00004DD0 File Offset: 0x00002FD0
		public static Brush MediumAquamarine
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumAquamarineKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumAquamarine);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumAquamarineKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00004E14 File Offset: 0x00003014
		public static Brush MediumBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x00004E58 File Offset: 0x00003058
		public static Brush MediumOrchid
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumOrchidKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumOrchid);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumOrchidKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060000B9 RID: 185 RVA: 0x00004E9C File Offset: 0x0000309C
		public static Brush MediumPurple
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumPurpleKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumPurple);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumPurpleKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060000BA RID: 186 RVA: 0x00004EE0 File Offset: 0x000030E0
		public static Brush MediumSeaGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSeaGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumSeaGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSeaGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060000BB RID: 187 RVA: 0x00004F24 File Offset: 0x00003124
		public static Brush MediumSlateBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSlateBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumSlateBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSlateBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060000BC RID: 188 RVA: 0x00004F68 File Offset: 0x00003168
		public static Brush MediumSpringGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSpringGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumSpringGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumSpringGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00004FAC File Offset: 0x000031AC
		public static Brush MediumTurquoise
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumTurquoiseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumTurquoise);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumTurquoiseKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060000BE RID: 190 RVA: 0x00004FF0 File Offset: 0x000031F0
		public static Brush MediumVioletRed
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MediumVioletRedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MediumVioletRed);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MediumVioletRedKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00005034 File Offset: 0x00003234
		public static Brush MidnightBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MidnightBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MidnightBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MidnightBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x00005078 File Offset: 0x00003278
		public static Brush MintCream
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MintCreamKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MintCream);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MintCreamKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x000050BC File Offset: 0x000032BC
		public static Brush MistyRose
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MistyRoseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.MistyRose);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MistyRoseKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x00005100 File Offset: 0x00003300
		public static Brush Moccasin
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.MoccasinKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Moccasin);
					SafeNativeMethods.Gdip.ThreadData[Brushes.MoccasinKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060000C3 RID: 195 RVA: 0x00005144 File Offset: 0x00003344
		public static Brush NavajoWhite
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.NavajoWhiteKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.NavajoWhite);
					SafeNativeMethods.Gdip.ThreadData[Brushes.NavajoWhiteKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060000C4 RID: 196 RVA: 0x00005188 File Offset: 0x00003388
		public static Brush Navy
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.NavyKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Navy);
					SafeNativeMethods.Gdip.ThreadData[Brushes.NavyKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x000051CC File Offset: 0x000033CC
		public static Brush OldLace
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OldLaceKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.OldLace);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OldLaceKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060000C6 RID: 198 RVA: 0x00005210 File Offset: 0x00003410
		public static Brush Olive
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OliveKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Olive);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OliveKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00005254 File Offset: 0x00003454
		public static Brush OliveDrab
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OliveDrabKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.OliveDrab);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OliveDrabKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00005298 File Offset: 0x00003498
		public static Brush Orange
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OrangeKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Orange);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OrangeKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060000C9 RID: 201 RVA: 0x000052DC File Offset: 0x000034DC
		public static Brush OrangeRed
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OrangeRedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.OrangeRed);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OrangeRedKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00005320 File Offset: 0x00003520
		public static Brush Orchid
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.OrchidKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Orchid);
					SafeNativeMethods.Gdip.ThreadData[Brushes.OrchidKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00005364 File Offset: 0x00003564
		public static Brush PaleGoldenrod
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PaleGoldenrodKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PaleGoldenrod);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PaleGoldenrodKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060000CC RID: 204 RVA: 0x000053A8 File Offset: 0x000035A8
		public static Brush PaleGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PaleGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PaleGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PaleGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060000CD RID: 205 RVA: 0x000053EC File Offset: 0x000035EC
		public static Brush PaleTurquoise
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PaleTurquoiseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PaleTurquoise);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PaleTurquoiseKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00005430 File Offset: 0x00003630
		public static Brush PaleVioletRed
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PaleVioletRedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PaleVioletRed);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PaleVioletRedKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00005474 File Offset: 0x00003674
		public static Brush PapayaWhip
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PapayaWhipKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PapayaWhip);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PapayaWhipKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x000054B8 File Offset: 0x000036B8
		public static Brush PeachPuff
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PeachPuffKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PeachPuff);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PeachPuffKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x000054FC File Offset: 0x000036FC
		public static Brush Peru
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PeruKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Peru);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PeruKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00005540 File Offset: 0x00003740
		public static Brush Pink
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PinkKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Pink);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PinkKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00005584 File Offset: 0x00003784
		public static Brush Plum
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PlumKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Plum);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PlumKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x000055C8 File Offset: 0x000037C8
		public static Brush PowderBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PowderBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.PowderBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PowderBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x0000560C File Offset: 0x0000380C
		public static Brush Purple
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.PurpleKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Purple);
					SafeNativeMethods.Gdip.ThreadData[Brushes.PurpleKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00005650 File Offset: 0x00003850
		public static Brush Red
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.RedKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Red);
					SafeNativeMethods.Gdip.ThreadData[Brushes.RedKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00005694 File Offset: 0x00003894
		public static Brush RosyBrown
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.RosyBrownKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.RosyBrown);
					SafeNativeMethods.Gdip.ThreadData[Brushes.RosyBrownKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x000056D8 File Offset: 0x000038D8
		public static Brush RoyalBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.RoyalBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.RoyalBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.RoyalBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x0000571C File Offset: 0x0000391C
		public static Brush SaddleBrown
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SaddleBrownKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SaddleBrown);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SaddleBrownKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00005760 File Offset: 0x00003960
		public static Brush Salmon
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SalmonKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Salmon);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SalmonKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060000DB RID: 219 RVA: 0x000057A4 File Offset: 0x000039A4
		public static Brush SandyBrown
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SandyBrownKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SandyBrown);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SandyBrownKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060000DC RID: 220 RVA: 0x000057E8 File Offset: 0x000039E8
		public static Brush SeaGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SeaGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SeaGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SeaGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060000DD RID: 221 RVA: 0x0000582C File Offset: 0x00003A2C
		public static Brush SeaShell
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SeaShellKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SeaShell);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SeaShellKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060000DE RID: 222 RVA: 0x00005870 File Offset: 0x00003A70
		public static Brush Sienna
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SiennaKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Sienna);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SiennaKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060000DF RID: 223 RVA: 0x000058B4 File Offset: 0x00003AB4
		public static Brush Silver
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SilverKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Silver);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SilverKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060000E0 RID: 224 RVA: 0x000058F8 File Offset: 0x00003AF8
		public static Brush SkyBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SkyBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SkyBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SkyBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000593C File Offset: 0x00003B3C
		public static Brush SlateBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SlateBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SlateBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SlateBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060000E2 RID: 226 RVA: 0x00005980 File Offset: 0x00003B80
		public static Brush SlateGray
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SlateGrayKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SlateGray);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SlateGrayKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x000059C4 File Offset: 0x00003BC4
		public static Brush Snow
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SnowKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Snow);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SnowKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060000E4 RID: 228 RVA: 0x00005A08 File Offset: 0x00003C08
		public static Brush SpringGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SpringGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SpringGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SpringGreenKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00005A4C File Offset: 0x00003C4C
		public static Brush SteelBlue
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.SteelBlueKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.SteelBlue);
					SafeNativeMethods.Gdip.ThreadData[Brushes.SteelBlueKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00005A90 File Offset: 0x00003C90
		public static Brush Tan
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.TanKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Tan);
					SafeNativeMethods.Gdip.ThreadData[Brushes.TanKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005AD4 File Offset: 0x00003CD4
		public static Brush Teal
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.TealKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Teal);
					SafeNativeMethods.Gdip.ThreadData[Brushes.TealKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005B18 File Offset: 0x00003D18
		public static Brush Thistle
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.ThistleKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Thistle);
					SafeNativeMethods.Gdip.ThreadData[Brushes.ThistleKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005B5C File Offset: 0x00003D5C
		public static Brush Tomato
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.TomatoKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Tomato);
					SafeNativeMethods.Gdip.ThreadData[Brushes.TomatoKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00005BA0 File Offset: 0x00003DA0
		public static Brush Turquoise
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.TurquoiseKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Turquoise);
					SafeNativeMethods.Gdip.ThreadData[Brushes.TurquoiseKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005BE4 File Offset: 0x00003DE4
		public static Brush Violet
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.VioletKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Violet);
					SafeNativeMethods.Gdip.ThreadData[Brushes.VioletKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005C28 File Offset: 0x00003E28
		public static Brush Wheat
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.WheatKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Wheat);
					SafeNativeMethods.Gdip.ThreadData[Brushes.WheatKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00005C6C File Offset: 0x00003E6C
		public static Brush White
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.WhiteKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.White);
					SafeNativeMethods.Gdip.ThreadData[Brushes.WhiteKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00005CB0 File Offset: 0x00003EB0
		public static Brush WhiteSmoke
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.WhiteSmokeKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.WhiteSmoke);
					SafeNativeMethods.Gdip.ThreadData[Brushes.WhiteSmokeKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00005CF4 File Offset: 0x00003EF4
		public static Brush Yellow
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.YellowKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.Yellow);
					SafeNativeMethods.Gdip.ThreadData[Brushes.YellowKey] = brush;
				}
				return brush;
			}
		}

		/// <summary>Gets a system-defined <see cref="T:System.Drawing.Brush" /> object.</summary>
		/// <returns>A <see cref="T:System.Drawing.Brush" /> object set to a system-defined color.</returns>
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005D38 File Offset: 0x00003F38
		public static Brush YellowGreen
		{
			get
			{
				Brush brush = (Brush)SafeNativeMethods.Gdip.ThreadData[Brushes.YellowGreenKey];
				if (brush == null)
				{
					brush = new SolidBrush(Color.YellowGreen);
					SafeNativeMethods.Gdip.ThreadData[Brushes.YellowGreenKey] = brush;
				}
				return brush;
			}
		}

		// Token: 0x0400009F RID: 159
		private static readonly object TransparentKey = new object();

		// Token: 0x040000A0 RID: 160
		private static readonly object AliceBlueKey = new object();

		// Token: 0x040000A1 RID: 161
		private static readonly object AntiqueWhiteKey = new object();

		// Token: 0x040000A2 RID: 162
		private static readonly object AquaKey = new object();

		// Token: 0x040000A3 RID: 163
		private static readonly object AquamarineKey = new object();

		// Token: 0x040000A4 RID: 164
		private static readonly object AzureKey = new object();

		// Token: 0x040000A5 RID: 165
		private static readonly object BeigeKey = new object();

		// Token: 0x040000A6 RID: 166
		private static readonly object BisqueKey = new object();

		// Token: 0x040000A7 RID: 167
		private static readonly object BlackKey = new object();

		// Token: 0x040000A8 RID: 168
		private static readonly object BlanchedAlmondKey = new object();

		// Token: 0x040000A9 RID: 169
		private static readonly object BlueKey = new object();

		// Token: 0x040000AA RID: 170
		private static readonly object BlueVioletKey = new object();

		// Token: 0x040000AB RID: 171
		private static readonly object BrownKey = new object();

		// Token: 0x040000AC RID: 172
		private static readonly object BurlyWoodKey = new object();

		// Token: 0x040000AD RID: 173
		private static readonly object CadetBlueKey = new object();

		// Token: 0x040000AE RID: 174
		private static readonly object ChartreuseKey = new object();

		// Token: 0x040000AF RID: 175
		private static readonly object ChocolateKey = new object();

		// Token: 0x040000B0 RID: 176
		private static readonly object ChoralKey = new object();

		// Token: 0x040000B1 RID: 177
		private static readonly object CornflowerBlueKey = new object();

		// Token: 0x040000B2 RID: 178
		private static readonly object CornsilkKey = new object();

		// Token: 0x040000B3 RID: 179
		private static readonly object CrimsonKey = new object();

		// Token: 0x040000B4 RID: 180
		private static readonly object CyanKey = new object();

		// Token: 0x040000B5 RID: 181
		private static readonly object DarkBlueKey = new object();

		// Token: 0x040000B6 RID: 182
		private static readonly object DarkCyanKey = new object();

		// Token: 0x040000B7 RID: 183
		private static readonly object DarkGoldenrodKey = new object();

		// Token: 0x040000B8 RID: 184
		private static readonly object DarkGrayKey = new object();

		// Token: 0x040000B9 RID: 185
		private static readonly object DarkGreenKey = new object();

		// Token: 0x040000BA RID: 186
		private static readonly object DarkKhakiKey = new object();

		// Token: 0x040000BB RID: 187
		private static readonly object DarkMagentaKey = new object();

		// Token: 0x040000BC RID: 188
		private static readonly object DarkOliveGreenKey = new object();

		// Token: 0x040000BD RID: 189
		private static readonly object DarkOrangeKey = new object();

		// Token: 0x040000BE RID: 190
		private static readonly object DarkOrchidKey = new object();

		// Token: 0x040000BF RID: 191
		private static readonly object DarkRedKey = new object();

		// Token: 0x040000C0 RID: 192
		private static readonly object DarkSalmonKey = new object();

		// Token: 0x040000C1 RID: 193
		private static readonly object DarkSeaGreenKey = new object();

		// Token: 0x040000C2 RID: 194
		private static readonly object DarkSlateBlueKey = new object();

		// Token: 0x040000C3 RID: 195
		private static readonly object DarkSlateGrayKey = new object();

		// Token: 0x040000C4 RID: 196
		private static readonly object DarkTurquoiseKey = new object();

		// Token: 0x040000C5 RID: 197
		private static readonly object DarkVioletKey = new object();

		// Token: 0x040000C6 RID: 198
		private static readonly object DeepPinkKey = new object();

		// Token: 0x040000C7 RID: 199
		private static readonly object DeepSkyBlueKey = new object();

		// Token: 0x040000C8 RID: 200
		private static readonly object DimGrayKey = new object();

		// Token: 0x040000C9 RID: 201
		private static readonly object DodgerBlueKey = new object();

		// Token: 0x040000CA RID: 202
		private static readonly object FirebrickKey = new object();

		// Token: 0x040000CB RID: 203
		private static readonly object FloralWhiteKey = new object();

		// Token: 0x040000CC RID: 204
		private static readonly object ForestGreenKey = new object();

		// Token: 0x040000CD RID: 205
		private static readonly object FuchiaKey = new object();

		// Token: 0x040000CE RID: 206
		private static readonly object GainsboroKey = new object();

		// Token: 0x040000CF RID: 207
		private static readonly object GhostWhiteKey = new object();

		// Token: 0x040000D0 RID: 208
		private static readonly object GoldKey = new object();

		// Token: 0x040000D1 RID: 209
		private static readonly object GoldenrodKey = new object();

		// Token: 0x040000D2 RID: 210
		private static readonly object GrayKey = new object();

		// Token: 0x040000D3 RID: 211
		private static readonly object GreenKey = new object();

		// Token: 0x040000D4 RID: 212
		private static readonly object GreenYellowKey = new object();

		// Token: 0x040000D5 RID: 213
		private static readonly object HoneydewKey = new object();

		// Token: 0x040000D6 RID: 214
		private static readonly object HotPinkKey = new object();

		// Token: 0x040000D7 RID: 215
		private static readonly object IndianRedKey = new object();

		// Token: 0x040000D8 RID: 216
		private static readonly object IndigoKey = new object();

		// Token: 0x040000D9 RID: 217
		private static readonly object IvoryKey = new object();

		// Token: 0x040000DA RID: 218
		private static readonly object KhakiKey = new object();

		// Token: 0x040000DB RID: 219
		private static readonly object LavenderKey = new object();

		// Token: 0x040000DC RID: 220
		private static readonly object LavenderBlushKey = new object();

		// Token: 0x040000DD RID: 221
		private static readonly object LawnGreenKey = new object();

		// Token: 0x040000DE RID: 222
		private static readonly object LemonChiffonKey = new object();

		// Token: 0x040000DF RID: 223
		private static readonly object LightBlueKey = new object();

		// Token: 0x040000E0 RID: 224
		private static readonly object LightCoralKey = new object();

		// Token: 0x040000E1 RID: 225
		private static readonly object LightCyanKey = new object();

		// Token: 0x040000E2 RID: 226
		private static readonly object LightGoldenrodYellowKey = new object();

		// Token: 0x040000E3 RID: 227
		private static readonly object LightGreenKey = new object();

		// Token: 0x040000E4 RID: 228
		private static readonly object LightGrayKey = new object();

		// Token: 0x040000E5 RID: 229
		private static readonly object LightPinkKey = new object();

		// Token: 0x040000E6 RID: 230
		private static readonly object LightSalmonKey = new object();

		// Token: 0x040000E7 RID: 231
		private static readonly object LightSeaGreenKey = new object();

		// Token: 0x040000E8 RID: 232
		private static readonly object LightSkyBlueKey = new object();

		// Token: 0x040000E9 RID: 233
		private static readonly object LightSlateGrayKey = new object();

		// Token: 0x040000EA RID: 234
		private static readonly object LightSteelBlueKey = new object();

		// Token: 0x040000EB RID: 235
		private static readonly object LightYellowKey = new object();

		// Token: 0x040000EC RID: 236
		private static readonly object LimeKey = new object();

		// Token: 0x040000ED RID: 237
		private static readonly object LimeGreenKey = new object();

		// Token: 0x040000EE RID: 238
		private static readonly object LinenKey = new object();

		// Token: 0x040000EF RID: 239
		private static readonly object MagentaKey = new object();

		// Token: 0x040000F0 RID: 240
		private static readonly object MaroonKey = new object();

		// Token: 0x040000F1 RID: 241
		private static readonly object MediumAquamarineKey = new object();

		// Token: 0x040000F2 RID: 242
		private static readonly object MediumBlueKey = new object();

		// Token: 0x040000F3 RID: 243
		private static readonly object MediumOrchidKey = new object();

		// Token: 0x040000F4 RID: 244
		private static readonly object MediumPurpleKey = new object();

		// Token: 0x040000F5 RID: 245
		private static readonly object MediumSeaGreenKey = new object();

		// Token: 0x040000F6 RID: 246
		private static readonly object MediumSlateBlueKey = new object();

		// Token: 0x040000F7 RID: 247
		private static readonly object MediumSpringGreenKey = new object();

		// Token: 0x040000F8 RID: 248
		private static readonly object MediumTurquoiseKey = new object();

		// Token: 0x040000F9 RID: 249
		private static readonly object MediumVioletRedKey = new object();

		// Token: 0x040000FA RID: 250
		private static readonly object MidnightBlueKey = new object();

		// Token: 0x040000FB RID: 251
		private static readonly object MintCreamKey = new object();

		// Token: 0x040000FC RID: 252
		private static readonly object MistyRoseKey = new object();

		// Token: 0x040000FD RID: 253
		private static readonly object MoccasinKey = new object();

		// Token: 0x040000FE RID: 254
		private static readonly object NavajoWhiteKey = new object();

		// Token: 0x040000FF RID: 255
		private static readonly object NavyKey = new object();

		// Token: 0x04000100 RID: 256
		private static readonly object OldLaceKey = new object();

		// Token: 0x04000101 RID: 257
		private static readonly object OliveKey = new object();

		// Token: 0x04000102 RID: 258
		private static readonly object OliveDrabKey = new object();

		// Token: 0x04000103 RID: 259
		private static readonly object OrangeKey = new object();

		// Token: 0x04000104 RID: 260
		private static readonly object OrangeRedKey = new object();

		// Token: 0x04000105 RID: 261
		private static readonly object OrchidKey = new object();

		// Token: 0x04000106 RID: 262
		private static readonly object PaleGoldenrodKey = new object();

		// Token: 0x04000107 RID: 263
		private static readonly object PaleGreenKey = new object();

		// Token: 0x04000108 RID: 264
		private static readonly object PaleTurquoiseKey = new object();

		// Token: 0x04000109 RID: 265
		private static readonly object PaleVioletRedKey = new object();

		// Token: 0x0400010A RID: 266
		private static readonly object PapayaWhipKey = new object();

		// Token: 0x0400010B RID: 267
		private static readonly object PeachPuffKey = new object();

		// Token: 0x0400010C RID: 268
		private static readonly object PeruKey = new object();

		// Token: 0x0400010D RID: 269
		private static readonly object PinkKey = new object();

		// Token: 0x0400010E RID: 270
		private static readonly object PlumKey = new object();

		// Token: 0x0400010F RID: 271
		private static readonly object PowderBlueKey = new object();

		// Token: 0x04000110 RID: 272
		private static readonly object PurpleKey = new object();

		// Token: 0x04000111 RID: 273
		private static readonly object RedKey = new object();

		// Token: 0x04000112 RID: 274
		private static readonly object RosyBrownKey = new object();

		// Token: 0x04000113 RID: 275
		private static readonly object RoyalBlueKey = new object();

		// Token: 0x04000114 RID: 276
		private static readonly object SaddleBrownKey = new object();

		// Token: 0x04000115 RID: 277
		private static readonly object SalmonKey = new object();

		// Token: 0x04000116 RID: 278
		private static readonly object SandyBrownKey = new object();

		// Token: 0x04000117 RID: 279
		private static readonly object SeaGreenKey = new object();

		// Token: 0x04000118 RID: 280
		private static readonly object SeaShellKey = new object();

		// Token: 0x04000119 RID: 281
		private static readonly object SiennaKey = new object();

		// Token: 0x0400011A RID: 282
		private static readonly object SilverKey = new object();

		// Token: 0x0400011B RID: 283
		private static readonly object SkyBlueKey = new object();

		// Token: 0x0400011C RID: 284
		private static readonly object SlateBlueKey = new object();

		// Token: 0x0400011D RID: 285
		private static readonly object SlateGrayKey = new object();

		// Token: 0x0400011E RID: 286
		private static readonly object SnowKey = new object();

		// Token: 0x0400011F RID: 287
		private static readonly object SpringGreenKey = new object();

		// Token: 0x04000120 RID: 288
		private static readonly object SteelBlueKey = new object();

		// Token: 0x04000121 RID: 289
		private static readonly object TanKey = new object();

		// Token: 0x04000122 RID: 290
		private static readonly object TealKey = new object();

		// Token: 0x04000123 RID: 291
		private static readonly object ThistleKey = new object();

		// Token: 0x04000124 RID: 292
		private static readonly object TomatoKey = new object();

		// Token: 0x04000125 RID: 293
		private static readonly object TurquoiseKey = new object();

		// Token: 0x04000126 RID: 294
		private static readonly object VioletKey = new object();

		// Token: 0x04000127 RID: 295
		private static readonly object WheatKey = new object();

		// Token: 0x04000128 RID: 296
		private static readonly object WhiteKey = new object();

		// Token: 0x04000129 RID: 297
		private static readonly object WhiteSmokeKey = new object();

		// Token: 0x0400012A RID: 298
		private static readonly object YellowKey = new object();

		// Token: 0x0400012B RID: 299
		private static readonly object YellowGreenKey = new object();
	}
}
