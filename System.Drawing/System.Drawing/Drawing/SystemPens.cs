using System;

namespace System.Drawing
{
	/// <summary>Each property of the <see cref="T:System.Drawing.SystemPens" /> class is a <see cref="T:System.Drawing.Pen" /> that is the color of a Windows display element and that has a width of 1 pixel.</summary>
	// Token: 0x02000037 RID: 55
	public sealed class SystemPens
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x00003800 File Offset: 0x00001A00
		private SystemPens()
		{
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the active window's border.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the active window's border.</returns>
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000563 RID: 1379 RVA: 0x00018070 File Offset: 0x00016270
		public static Pen ActiveBorder
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ActiveBorder);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the background of the active window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the background of the active window's title bar.</returns>
		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000564 RID: 1380 RVA: 0x0001807C File Offset: 0x0001627C
		public static Pen ActiveCaption
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ActiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the text in the active window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the text in the active window's title bar.</returns>
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x00018088 File Offset: 0x00016288
		public static Pen ActiveCaptionText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ActiveCaptionText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the application workspace.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the application workspace.</returns>
		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x00018094 File Offset: 0x00016294
		public static Pen AppWorkspace
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.AppWorkspace);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the face color of a 3-D element.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the face color of a 3-D element.</returns>
		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x000180A0 File Offset: 0x000162A0
		public static Pen ButtonFace
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ButtonFace);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the highlight color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the highlight color of a 3-D element.</returns>
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x000180AC File Offset: 0x000162AC
		public static Pen ButtonHighlight
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ButtonHighlight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the shadow color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the shadow color of a 3-D element.</returns>
		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x000180B8 File Offset: 0x000162B8
		public static Pen ButtonShadow
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ButtonShadow);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the face color of a 3-D element.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the face color of a 3-D element.</returns>
		// Token: 0x17000271 RID: 625
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x000180C4 File Offset: 0x000162C4
		public static Pen Control
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Control);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of text in a 3-D element.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of text in a 3-D element.</returns>
		// Token: 0x17000272 RID: 626
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x000180D0 File Offset: 0x000162D0
		public static Pen ControlText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ControlText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the shadow color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the shadow color of a 3-D element.</returns>
		// Token: 0x17000273 RID: 627
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x000180DC File Offset: 0x000162DC
		public static Pen ControlDark
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ControlDark);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the dark shadow color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the dark shadow color of a 3-D element.</returns>
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x000180E8 File Offset: 0x000162E8
		public static Pen ControlDarkDark
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ControlDarkDark);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the light color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the light color of a 3-D element.</returns>
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x0600056E RID: 1390 RVA: 0x000180F4 File Offset: 0x000162F4
		public static Pen ControlLight
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ControlLight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the highlight color of a 3-D element. </summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the highlight color of a 3-D element.</returns>
		// Token: 0x17000276 RID: 630
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x00018100 File Offset: 0x00016300
		public static Pen ControlLightLight
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ControlLightLight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the Windows desktop.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the Windows desktop.</returns>
		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000570 RID: 1392 RVA: 0x0001810C File Offset: 0x0001630C
		public static Pen Desktop
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Desktop);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the lightest color in the color gradient of an active window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the lightest color in the color gradient of an active window's title bar.</returns>
		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000571 RID: 1393 RVA: 0x00018118 File Offset: 0x00016318
		public static Pen GradientActiveCaption
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.GradientActiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the lightest color in the color gradient of an inactive window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the lightest color in the color gradient of an inactive window's title bar.</returns>
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000572 RID: 1394 RVA: 0x00018124 File Offset: 0x00016324
		public static Pen GradientInactiveCaption
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.GradientInactiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of dimmed text. </summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of dimmed text.</returns>
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000573 RID: 1395 RVA: 0x00018130 File Offset: 0x00016330
		public static Pen GrayText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.GrayText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the background of selected items. </summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the background of selected items.</returns>
		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0001813C File Offset: 0x0001633C
		public static Pen Highlight
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Highlight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the text of selected items. </summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the text of selected items.</returns>
		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x00018148 File Offset: 0x00016348
		public static Pen HighlightText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.HighlightText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color used to designate a hot-tracked item.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color used to designate a hot-tracked item.</returns>
		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000576 RID: 1398 RVA: 0x00018154 File Offset: 0x00016354
		public static Pen HotTrack
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.HotTrack);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> is the color of the border of an inactive window.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the border of an inactive window.</returns>
		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x00018160 File Offset: 0x00016360
		public static Pen InactiveBorder
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.InactiveBorder);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the title bar caption of an inactive window.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the title bar caption of an inactive window.</returns>
		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0001816C File Offset: 0x0001636C
		public static Pen InactiveCaption
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.InactiveCaption);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the text in an inactive window's title bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the text in an inactive window's title bar.</returns>
		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x00018178 File Offset: 0x00016378
		public static Pen InactiveCaptionText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.InactiveCaptionText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the background of a ToolTip.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the background of a ToolTip.</returns>
		// Token: 0x17000281 RID: 641
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x00018184 File Offset: 0x00016384
		public static Pen Info
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Info);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the text of a ToolTip.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the text of a ToolTip.</returns>
		// Token: 0x17000282 RID: 642
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x00018190 File Offset: 0x00016390
		public static Pen InfoText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.InfoText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of a menu's background.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of a menu's background.</returns>
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0001819C File Offset: 0x0001639C
		public static Pen Menu
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Menu);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the background of a menu bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the background of a menu bar.</returns>
		// Token: 0x17000284 RID: 644
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x000181A8 File Offset: 0x000163A8
		public static Pen MenuBar
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.MenuBar);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color used to highlight menu items when the menu appears as a flat menu.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color used to highlight menu items when the menu appears as a flat menu.</returns>
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x000181B4 File Offset: 0x000163B4
		public static Pen MenuHighlight
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.MenuHighlight);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of a menu's text.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of a menu's text.</returns>
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x0600057F RID: 1407 RVA: 0x000181C0 File Offset: 0x000163C0
		public static Pen MenuText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.MenuText);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the background of a scroll bar.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the background of a scroll bar.</returns>
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x000181CC File Offset: 0x000163CC
		public static Pen ScrollBar
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.ScrollBar);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the background in the client area of a window.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the background in the client area of a window.</returns>
		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000581 RID: 1409 RVA: 0x000181D8 File Offset: 0x000163D8
		public static Pen Window
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.Window);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of a window frame.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of a window frame.</returns>
		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x000181E4 File Offset: 0x000163E4
		public static Pen WindowFrame
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.WindowFrame);
			}
		}

		/// <summary>Gets a <see cref="T:System.Drawing.Pen" /> that is the color of the text in the client area of a window.</summary>
		/// <returns>A <see cref="T:System.Drawing.Pen" /> that is the color of the text in the client area of a window.</returns>
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000583 RID: 1411 RVA: 0x000181F0 File Offset: 0x000163F0
		public static Pen WindowText
		{
			get
			{
				return SystemPens.FromSystemColor(SystemColors.WindowText);
			}
		}

		/// <summary>Creates a <see cref="T:System.Drawing.Pen" /> from the specified <see cref="T:System.Drawing.Color" />.</summary>
		/// <param name="c">The <see cref="T:System.Drawing.Color" /> for the new <see cref="T:System.Drawing.Pen" />. </param>
		/// <returns>The <see cref="T:System.Drawing.Pen" /> this method creates.</returns>
		// Token: 0x06000584 RID: 1412 RVA: 0x000181FC File Offset: 0x000163FC
		public static Pen FromSystemColor(Color c)
		{
			if (!c.IsSystemColor)
			{
				throw new ArgumentException(SR.GetString("ColorNotSystemColor", new object[]
				{
					c.ToString()
				}));
			}
			Pen[] array = (Pen[])SafeNativeMethods.Gdip.ThreadData[SystemPens.SystemPensKey];
			if (array == null)
			{
				array = new Pen[33];
				SafeNativeMethods.Gdip.ThreadData[SystemPens.SystemPensKey] = array;
			}
			int num = (int)c.ToKnownColor();
			if (num > 167)
			{
				num -= 141;
			}
			num--;
			if (array[num] == null)
			{
				array[num] = new Pen(c, true);
			}
			return array[num];
		}

		// Token: 0x04000326 RID: 806
		private static readonly object SystemPensKey = new object();
	}
}
