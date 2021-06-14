using System;

namespace System.Drawing
{
	/// <summary>Each property of the <see cref="T:System.Drawing.SystemColors" /> class is a <see cref="T:System.Drawing.Color" /> structure that is the color of a Windows display element.</summary>
	// Token: 0x02000034 RID: 52
	public sealed class SystemColors
	{
		// Token: 0x06000528 RID: 1320 RVA: 0x00003800 File Offset: 0x00001A00
		private SystemColors()
		{
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the active window's border.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the active window's border.</returns>
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000529 RID: 1321 RVA: 0x00017696 File Offset: 0x00015896
		public static Color ActiveBorder
		{
			get
			{
				return new Color(KnownColor.ActiveBorder);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the background of the active window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the active window's title bar.</returns>
		// Token: 0x17000238 RID: 568
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x0001769E File Offset: 0x0001589E
		public static Color ActiveCaption
		{
			get
			{
				return new Color(KnownColor.ActiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the text in the active window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the text in the active window's title bar.</returns>
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x000176A6 File Offset: 0x000158A6
		public static Color ActiveCaptionText
		{
			get
			{
				return new Color(KnownColor.ActiveCaptionText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the application workspace. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the application workspace.</returns>
		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600052C RID: 1324 RVA: 0x000176AE File Offset: 0x000158AE
		public static Color AppWorkspace
		{
			get
			{
				return new Color(KnownColor.AppWorkspace);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the face color of a 3-D element.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the face color of a 3-D element.</returns>
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600052D RID: 1325 RVA: 0x000176B6 File Offset: 0x000158B6
		public static Color ButtonFace
		{
			get
			{
				return new Color(KnownColor.ButtonFace);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the highlight color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the highlight color of a 3-D element.</returns>
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600052E RID: 1326 RVA: 0x000176C2 File Offset: 0x000158C2
		public static Color ButtonHighlight
		{
			get
			{
				return new Color(KnownColor.ButtonHighlight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the shadow color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the shadow color of a 3-D element.</returns>
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600052F RID: 1327 RVA: 0x000176CE File Offset: 0x000158CE
		public static Color ButtonShadow
		{
			get
			{
				return new Color(KnownColor.ButtonShadow);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the face color of a 3-D element.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the face color of a 3-D element.</returns>
		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x000176DA File Offset: 0x000158DA
		public static Color Control
		{
			get
			{
				return new Color(KnownColor.Control);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the shadow color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the shadow color of a 3-D element.</returns>
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000531 RID: 1329 RVA: 0x000176E2 File Offset: 0x000158E2
		public static Color ControlDark
		{
			get
			{
				return new Color(KnownColor.ControlDark);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the dark shadow color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the dark shadow color of a 3-D element.</returns>
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000532 RID: 1330 RVA: 0x000176EA File Offset: 0x000158EA
		public static Color ControlDarkDark
		{
			get
			{
				return new Color(KnownColor.ControlDarkDark);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the light color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the light color of a 3-D element.</returns>
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000533 RID: 1331 RVA: 0x000176F2 File Offset: 0x000158F2
		public static Color ControlLight
		{
			get
			{
				return new Color(KnownColor.ControlLight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the highlight color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the highlight color of a 3-D element.</returns>
		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000534 RID: 1332 RVA: 0x000176FA File Offset: 0x000158FA
		public static Color ControlLightLight
		{
			get
			{
				return new Color(KnownColor.ControlLightLight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of text in a 3-D element.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of text in a 3-D element.</returns>
		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000535 RID: 1333 RVA: 0x00017703 File Offset: 0x00015903
		public static Color ControlText
		{
			get
			{
				return new Color(KnownColor.ControlText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the desktop.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the desktop.</returns>
		// Token: 0x17000244 RID: 580
		// (get) Token: 0x06000536 RID: 1334 RVA: 0x0001770C File Offset: 0x0001590C
		public static Color Desktop
		{
			get
			{
				return new Color(KnownColor.Desktop);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the lightest color in the color gradient of an active window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the lightest color in the color gradient of an active window's title bar.</returns>
		// Token: 0x17000245 RID: 581
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00017715 File Offset: 0x00015915
		public static Color GradientActiveCaption
		{
			get
			{
				return new Color(KnownColor.GradientActiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the lightest color in the color gradient of an inactive window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the lightest color in the color gradient of an inactive window's title bar.</returns>
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x00017721 File Offset: 0x00015921
		public static Color GradientInactiveCaption
		{
			get
			{
				return new Color(KnownColor.GradientInactiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of dimmed text. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of dimmed text.</returns>
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000539 RID: 1337 RVA: 0x0001772D File Offset: 0x0001592D
		public static Color GrayText
		{
			get
			{
				return new Color(KnownColor.GrayText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the background of selected items.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the background of selected items.</returns>
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x00017736 File Offset: 0x00015936
		public static Color Highlight
		{
			get
			{
				return new Color(KnownColor.Highlight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the text of selected items.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the text of selected items.</returns>
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x0600053B RID: 1339 RVA: 0x0001773F File Offset: 0x0001593F
		public static Color HighlightText
		{
			get
			{
				return new Color(KnownColor.HighlightText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color used to designate a hot-tracked item. </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color used to designate a hot-tracked item.</returns>
		// Token: 0x1700024A RID: 586
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x00017748 File Offset: 0x00015948
		public static Color HotTrack
		{
			get
			{
				return new Color(KnownColor.HotTrack);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of an inactive window's border.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of an inactive window's border.</returns>
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600053D RID: 1341 RVA: 0x00017751 File Offset: 0x00015951
		public static Color InactiveBorder
		{
			get
			{
				return new Color(KnownColor.InactiveBorder);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the background of an inactive window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the background of an inactive window's title bar.</returns>
		// Token: 0x1700024C RID: 588
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001775A File Offset: 0x0001595A
		public static Color InactiveCaption
		{
			get
			{
				return new Color(KnownColor.InactiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the text in an inactive window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the text in an inactive window's title bar.</returns>
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x0600053F RID: 1343 RVA: 0x00017763 File Offset: 0x00015963
		public static Color InactiveCaptionText
		{
			get
			{
				return new Color(KnownColor.InactiveCaptionText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the background of a ToolTip.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the background of a ToolTip.</returns>
		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001776C File Offset: 0x0001596C
		public static Color Info
		{
			get
			{
				return new Color(KnownColor.Info);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the text of a ToolTip.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the text of a ToolTip.</returns>
		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06000541 RID: 1345 RVA: 0x00017775 File Offset: 0x00015975
		public static Color InfoText
		{
			get
			{
				return new Color(KnownColor.InfoText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of a menu's background.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of a menu's background.</returns>
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001777E File Offset: 0x0001597E
		public static Color Menu
		{
			get
			{
				return new Color(KnownColor.Menu);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the background of a menu bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the background of a menu bar.</returns>
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06000543 RID: 1347 RVA: 0x00017787 File Offset: 0x00015987
		public static Color MenuBar
		{
			get
			{
				return new Color(KnownColor.MenuBar);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color used to highlight menu items when the menu appears as a flat menu.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color used to highlight menu items when the menu appears as a flat menu.</returns>
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x00017793 File Offset: 0x00015993
		public static Color MenuHighlight
		{
			get
			{
				return new Color(KnownColor.MenuHighlight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of a menu's text.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of a menu's text.</returns>
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06000545 RID: 1349 RVA: 0x0001779F File Offset: 0x0001599F
		public static Color MenuText
		{
			get
			{
				return new Color(KnownColor.MenuText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the background of a scroll bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the background of a scroll bar.</returns>
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x000177A8 File Offset: 0x000159A8
		public static Color ScrollBar
		{
			get
			{
				return new Color(KnownColor.ScrollBar);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the background in the client area of a window.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the background in the client area of a window.</returns>
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x000177B1 File Offset: 0x000159B1
		public static Color Window
		{
			get
			{
				return new Color(KnownColor.Window);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of a window frame.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of a window frame.</returns>
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x000177BA File Offset: 0x000159BA
		public static Color WindowFrame
		{
			get
			{
				return new Color(KnownColor.WindowFrame);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Color" /> structure that is the color of the text in the client area of a window.</summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> that is the color of the text in the client area of a window.</returns>
		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x000177C3 File Offset: 0x000159C3
		public static Color WindowText
		{
			get
			{
				return new Color(KnownColor.WindowText);
			}
		}
	}
}
