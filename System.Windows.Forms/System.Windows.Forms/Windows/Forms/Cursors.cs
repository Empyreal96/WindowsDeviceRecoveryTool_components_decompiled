using System;

namespace System.Windows.Forms
{
	/// <summary>Provides a collection of <see cref="T:System.Windows.Forms.Cursor" /> objects for use by a Windows Forms application.</summary>
	// Token: 0x02000168 RID: 360
	public sealed class Cursors
	{
		// Token: 0x060010B8 RID: 4280 RVA: 0x000027DB File Offset: 0x000009DB
		private Cursors()
		{
		}

		// Token: 0x060010B9 RID: 4281 RVA: 0x0003B5E6 File Offset: 0x000397E6
		internal static Cursor KnownCursorFromHCursor(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
			return new Cursor(handle);
		}

		/// <summary>Gets the cursor that appears when an application starts.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears when an application starts.</returns>
		// Token: 0x1700040B RID: 1035
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x0003B5FD File Offset: 0x000397FD
		public static Cursor AppStarting
		{
			get
			{
				if (Cursors.appStarting == null)
				{
					Cursors.appStarting = new Cursor(32650, 0);
				}
				return Cursors.appStarting;
			}
		}

		/// <summary>Gets the arrow cursor.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the arrow cursor.</returns>
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x060010BB RID: 4283 RVA: 0x0003B621 File Offset: 0x00039821
		public static Cursor Arrow
		{
			get
			{
				if (Cursors.arrow == null)
				{
					Cursors.arrow = new Cursor(32512, 0);
				}
				return Cursors.arrow;
			}
		}

		/// <summary>Gets the crosshair cursor.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the crosshair cursor.</returns>
		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x0003B645 File Offset: 0x00039845
		public static Cursor Cross
		{
			get
			{
				if (Cursors.cross == null)
				{
					Cursors.cross = new Cursor(32515, 0);
				}
				return Cursors.cross;
			}
		}

		/// <summary>Gets the default cursor, which is usually an arrow cursor.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the default cursor.</returns>
		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x060010BD RID: 4285 RVA: 0x0003B669 File Offset: 0x00039869
		public static Cursor Default
		{
			get
			{
				if (Cursors.defaultCursor == null)
				{
					Cursors.defaultCursor = new Cursor(32512, 0);
				}
				return Cursors.defaultCursor;
			}
		}

		/// <summary>Gets the I-beam cursor, which is used to show where the text cursor appears when the mouse is clicked.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the I-beam cursor.</returns>
		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x0003B68D File Offset: 0x0003988D
		public static Cursor IBeam
		{
			get
			{
				if (Cursors.iBeam == null)
				{
					Cursors.iBeam = new Cursor(32513, 0);
				}
				return Cursors.iBeam;
			}
		}

		/// <summary>Gets the cursor that indicates that a particular region is invalid for the current operation.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that indicates that a particular region is invalid for the current operation.</returns>
		// Token: 0x17000410 RID: 1040
		// (get) Token: 0x060010BF RID: 4287 RVA: 0x0003B6B1 File Offset: 0x000398B1
		public static Cursor No
		{
			get
			{
				if (Cursors.no == null)
				{
					Cursors.no = new Cursor(32648, 0);
				}
				return Cursors.no;
			}
		}

		/// <summary>Gets the four-headed sizing cursor, which consists of four joined arrows that point north, south, east, and west.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the four-headed sizing cursor.</returns>
		// Token: 0x17000411 RID: 1041
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x0003B6D5 File Offset: 0x000398D5
		public static Cursor SizeAll
		{
			get
			{
				if (Cursors.sizeAll == null)
				{
					Cursors.sizeAll = new Cursor(32646, 0);
				}
				return Cursors.sizeAll;
			}
		}

		/// <summary>Gets the two-headed diagonal (northeast/southwest) sizing cursor.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents two-headed diagonal (northeast/southwest) sizing cursor.</returns>
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x060010C1 RID: 4289 RVA: 0x0003B6F9 File Offset: 0x000398F9
		public static Cursor SizeNESW
		{
			get
			{
				if (Cursors.sizeNESW == null)
				{
					Cursors.sizeNESW = new Cursor(32643, 0);
				}
				return Cursors.sizeNESW;
			}
		}

		/// <summary>Gets the two-headed vertical (north/south) sizing cursor.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the two-headed vertical (north/south) sizing cursor.</returns>
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x0003B71D File Offset: 0x0003991D
		public static Cursor SizeNS
		{
			get
			{
				if (Cursors.sizeNS == null)
				{
					Cursors.sizeNS = new Cursor(32645, 0);
				}
				return Cursors.sizeNS;
			}
		}

		/// <summary>Gets the two-headed diagonal (northwest/southeast) sizing cursor.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the two-headed diagonal (northwest/southeast) sizing cursor.</returns>
		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x0003B741 File Offset: 0x00039941
		public static Cursor SizeNWSE
		{
			get
			{
				if (Cursors.sizeNWSE == null)
				{
					Cursors.sizeNWSE = new Cursor(32642, 0);
				}
				return Cursors.sizeNWSE;
			}
		}

		/// <summary>Gets the two-headed horizontal (west/east) sizing cursor.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the two-headed horizontal (west/east) sizing cursor.</returns>
		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x0003B765 File Offset: 0x00039965
		public static Cursor SizeWE
		{
			get
			{
				if (Cursors.sizeWE == null)
				{
					Cursors.sizeWE = new Cursor(32644, 0);
				}
				return Cursors.sizeWE;
			}
		}

		/// <summary>Gets the up arrow cursor, typically used to identify an insertion point.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the up arrow cursor.</returns>
		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x0003B789 File Offset: 0x00039989
		public static Cursor UpArrow
		{
			get
			{
				if (Cursors.upArrow == null)
				{
					Cursors.upArrow = new Cursor(32516, 0);
				}
				return Cursors.upArrow;
			}
		}

		/// <summary>Gets the wait cursor, typically an hourglass shape.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the wait cursor.</returns>
		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x0003B7AD File Offset: 0x000399AD
		public static Cursor WaitCursor
		{
			get
			{
				if (Cursors.wait == null)
				{
					Cursors.wait = new Cursor(32514, 0);
				}
				return Cursors.wait;
			}
		}

		/// <summary>Gets the Help cursor, which is a combination of an arrow and a question mark.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the Help cursor.</returns>
		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x060010C7 RID: 4295 RVA: 0x0003B7D1 File Offset: 0x000399D1
		public static Cursor Help
		{
			get
			{
				if (Cursors.help == null)
				{
					Cursors.help = new Cursor(32651, 0);
				}
				return Cursors.help;
			}
		}

		/// <summary>Gets the cursor that appears when the mouse is positioned over a horizontal splitter bar.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears when the mouse is positioned over a horizontal splitter bar.</returns>
		// Token: 0x17000419 RID: 1049
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x0003B7F5 File Offset: 0x000399F5
		public static Cursor HSplit
		{
			get
			{
				if (Cursors.hSplit == null)
				{
					Cursors.hSplit = new Cursor("hsplit.cur", 0);
				}
				return Cursors.hSplit;
			}
		}

		/// <summary>Gets the cursor that appears when the mouse is positioned over a vertical splitter bar.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears when the mouse is positioned over a vertical splitter bar.</returns>
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x060010C9 RID: 4297 RVA: 0x0003B819 File Offset: 0x00039A19
		public static Cursor VSplit
		{
			get
			{
				if (Cursors.vSplit == null)
				{
					Cursors.vSplit = new Cursor("vsplit.cur", 0);
				}
				return Cursors.vSplit;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is not moving, but the window can be scrolled in both a horizontal and vertical direction.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is not moving.</returns>
		// Token: 0x1700041B RID: 1051
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x0003B83D File Offset: 0x00039A3D
		public static Cursor NoMove2D
		{
			get
			{
				if (Cursors.noMove2D == null)
				{
					Cursors.noMove2D = new Cursor("nomove2d.cur", 0);
				}
				return Cursors.noMove2D;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is not moving, but the window can be scrolled in a horizontal direction.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is not moving.</returns>
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060010CB RID: 4299 RVA: 0x0003B861 File Offset: 0x00039A61
		public static Cursor NoMoveHoriz
		{
			get
			{
				if (Cursors.noMoveHoriz == null)
				{
					Cursors.noMoveHoriz = new Cursor("nomoveh.cur", 0);
				}
				return Cursors.noMoveHoriz;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is not moving, but the window can be scrolled in a vertical direction.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is not moving.</returns>
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x0003B885 File Offset: 0x00039A85
		public static Cursor NoMoveVert
		{
			get
			{
				if (Cursors.noMoveVert == null)
				{
					Cursors.noMoveVert = new Cursor("nomovev.cur", 0);
				}
				return Cursors.noMoveVert;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally to the right.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally to the right.</returns>
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060010CD RID: 4301 RVA: 0x0003B8A9 File Offset: 0x00039AA9
		public static Cursor PanEast
		{
			get
			{
				if (Cursors.panEast == null)
				{
					Cursors.panEast = new Cursor("east.cur", 0);
				}
				return Cursors.panEast;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally and vertically upward and to the right.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally and vertically upward and to the right.</returns>
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x0003B8CD File Offset: 0x00039ACD
		public static Cursor PanNE
		{
			get
			{
				if (Cursors.panNE == null)
				{
					Cursors.panNE = new Cursor("ne.cur", 0);
				}
				return Cursors.panNE;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is moving and the window is scrolling vertically in an upward direction.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is moving and the window is scrolling vertically in an upward direction.</returns>
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060010CF RID: 4303 RVA: 0x0003B8F1 File Offset: 0x00039AF1
		public static Cursor PanNorth
		{
			get
			{
				if (Cursors.panNorth == null)
				{
					Cursors.panNorth = new Cursor("north.cur", 0);
				}
				return Cursors.panNorth;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally and vertically upward and to the left.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally and vertically upward and to the left.</returns>
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x0003B915 File Offset: 0x00039B15
		public static Cursor PanNW
		{
			get
			{
				if (Cursors.panNW == null)
				{
					Cursors.panNW = new Cursor("nw.cur", 0);
				}
				return Cursors.panNW;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally and vertically downward and to the right.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally and vertically downward and to the right.</returns>
		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060010D1 RID: 4305 RVA: 0x0003B939 File Offset: 0x00039B39
		public static Cursor PanSE
		{
			get
			{
				if (Cursors.panSE == null)
				{
					Cursors.panSE = new Cursor("se.cur", 0);
				}
				return Cursors.panSE;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is moving and the window is scrolling vertically in a downward direction.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is moving and the window is scrolling vertically in a downward direction.</returns>
		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x0003B95D File Offset: 0x00039B5D
		public static Cursor PanSouth
		{
			get
			{
				if (Cursors.panSouth == null)
				{
					Cursors.panSouth = new Cursor("south.cur", 0);
				}
				return Cursors.panSouth;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally and vertically downward and to the left.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally and vertically downward and to the left.</returns>
		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x060010D3 RID: 4307 RVA: 0x0003B981 File Offset: 0x00039B81
		public static Cursor PanSW
		{
			get
			{
				if (Cursors.panSW == null)
				{
					Cursors.panSW = new Cursor("sw.cur", 0);
				}
				return Cursors.panSW;
			}
		}

		/// <summary>Gets the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally to the left.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the cursor that appears during wheel operations when the mouse is moving and the window is scrolling horizontally to the left.</returns>
		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x060010D4 RID: 4308 RVA: 0x0003B9A5 File Offset: 0x00039BA5
		public static Cursor PanWest
		{
			get
			{
				if (Cursors.panWest == null)
				{
					Cursors.panWest = new Cursor("west.cur", 0);
				}
				return Cursors.panWest;
			}
		}

		/// <summary>Gets the hand cursor, typically used when hovering over a Web link.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.Cursor" /> that represents the hand cursor.</returns>
		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x060010D5 RID: 4309 RVA: 0x0003B9C9 File Offset: 0x00039BC9
		public static Cursor Hand
		{
			get
			{
				if (Cursors.hand == null)
				{
					Cursors.hand = new Cursor("hand.cur", 0);
				}
				return Cursors.hand;
			}
		}

		// Token: 0x040008A0 RID: 2208
		private static Cursor appStarting;

		// Token: 0x040008A1 RID: 2209
		private static Cursor arrow;

		// Token: 0x040008A2 RID: 2210
		private static Cursor cross;

		// Token: 0x040008A3 RID: 2211
		private static Cursor defaultCursor;

		// Token: 0x040008A4 RID: 2212
		private static Cursor iBeam;

		// Token: 0x040008A5 RID: 2213
		private static Cursor no;

		// Token: 0x040008A6 RID: 2214
		private static Cursor sizeAll;

		// Token: 0x040008A7 RID: 2215
		private static Cursor sizeNESW;

		// Token: 0x040008A8 RID: 2216
		private static Cursor sizeNS;

		// Token: 0x040008A9 RID: 2217
		private static Cursor sizeNWSE;

		// Token: 0x040008AA RID: 2218
		private static Cursor sizeWE;

		// Token: 0x040008AB RID: 2219
		private static Cursor upArrow;

		// Token: 0x040008AC RID: 2220
		private static Cursor wait;

		// Token: 0x040008AD RID: 2221
		private static Cursor help;

		// Token: 0x040008AE RID: 2222
		private static Cursor hSplit;

		// Token: 0x040008AF RID: 2223
		private static Cursor vSplit;

		// Token: 0x040008B0 RID: 2224
		private static Cursor noMove2D;

		// Token: 0x040008B1 RID: 2225
		private static Cursor noMoveHoriz;

		// Token: 0x040008B2 RID: 2226
		private static Cursor noMoveVert;

		// Token: 0x040008B3 RID: 2227
		private static Cursor panEast;

		// Token: 0x040008B4 RID: 2228
		private static Cursor panNE;

		// Token: 0x040008B5 RID: 2229
		private static Cursor panNorth;

		// Token: 0x040008B6 RID: 2230
		private static Cursor panNW;

		// Token: 0x040008B7 RID: 2231
		private static Cursor panSE;

		// Token: 0x040008B8 RID: 2232
		private static Cursor panSouth;

		// Token: 0x040008B9 RID: 2233
		private static Cursor panSW;

		// Token: 0x040008BA RID: 2234
		private static Cursor panWest;

		// Token: 0x040008BB RID: 2235
		private static Cursor hand;
	}
}
