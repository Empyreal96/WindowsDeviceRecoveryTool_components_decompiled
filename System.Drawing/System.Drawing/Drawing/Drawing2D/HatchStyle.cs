using System;

namespace System.Drawing.Drawing2D
{
	/// <summary>Specifies the different patterns available for <see cref="T:System.Drawing.Drawing2D.HatchBrush" /> objects.</summary>
	// Token: 0x020000C4 RID: 196
	public enum HatchStyle
	{
		/// <summary>A pattern of horizontal lines.</summary>
		// Token: 0x04000993 RID: 2451
		Horizontal,
		/// <summary>A pattern of vertical lines.</summary>
		// Token: 0x04000994 RID: 2452
		Vertical,
		/// <summary>A pattern of lines on a diagonal from upper left to lower right.</summary>
		// Token: 0x04000995 RID: 2453
		ForwardDiagonal,
		/// <summary>A pattern of lines on a diagonal from upper right to lower left.</summary>
		// Token: 0x04000996 RID: 2454
		BackwardDiagonal,
		/// <summary>Specifies horizontal and vertical lines that cross.</summary>
		// Token: 0x04000997 RID: 2455
		Cross,
		/// <summary>A pattern of crisscross diagonal lines.</summary>
		// Token: 0x04000998 RID: 2456
		DiagonalCross,
		/// <summary>Specifies a 5-percent hatch. The ratio of foreground color to background color is 5:95.</summary>
		// Token: 0x04000999 RID: 2457
		Percent05,
		/// <summary>Specifies a 10-percent hatch. The ratio of foreground color to background color is 10:90.</summary>
		// Token: 0x0400099A RID: 2458
		Percent10,
		/// <summary>Specifies a 20-percent hatch. The ratio of foreground color to background color is 20:80.</summary>
		// Token: 0x0400099B RID: 2459
		Percent20,
		/// <summary>Specifies a 25-percent hatch. The ratio of foreground color to background color is 25:75.</summary>
		// Token: 0x0400099C RID: 2460
		Percent25,
		/// <summary>Specifies a 30-percent hatch. The ratio of foreground color to background color is 30:70.</summary>
		// Token: 0x0400099D RID: 2461
		Percent30,
		/// <summary>Specifies a 40-percent hatch. The ratio of foreground color to background color is 40:60.</summary>
		// Token: 0x0400099E RID: 2462
		Percent40,
		/// <summary>Specifies a 50-percent hatch. The ratio of foreground color to background color is 50:50.</summary>
		// Token: 0x0400099F RID: 2463
		Percent50,
		/// <summary>Specifies a 60-percent hatch. The ratio of foreground color to background color is 60:40.</summary>
		// Token: 0x040009A0 RID: 2464
		Percent60,
		/// <summary>Specifies a 70-percent hatch. The ratio of foreground color to background color is 70:30.</summary>
		// Token: 0x040009A1 RID: 2465
		Percent70,
		/// <summary>Specifies a 75-percent hatch. The ratio of foreground color to background color is 75:25.</summary>
		// Token: 0x040009A2 RID: 2466
		Percent75,
		/// <summary>Specifies a 80-percent hatch. The ratio of foreground color to background color is 80:100.</summary>
		// Token: 0x040009A3 RID: 2467
		Percent80,
		/// <summary>Specifies a 90-percent hatch. The ratio of foreground color to background color is 90:10.</summary>
		// Token: 0x040009A4 RID: 2468
		Percent90,
		/// <summary>Specifies diagonal lines that slant to the right from top points to bottom points and are spaced 50 percent closer together than <see cref="F:System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal" />, but are not antialiased.</summary>
		// Token: 0x040009A5 RID: 2469
		LightDownwardDiagonal,
		/// <summary>Specifies diagonal lines that slant to the left from top points to bottom points and are spaced 50 percent closer together than <see cref="F:System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal" />, but they are not antialiased.</summary>
		// Token: 0x040009A6 RID: 2470
		LightUpwardDiagonal,
		/// <summary>Specifies diagonal lines that slant to the right from top points to bottom points, are spaced 50 percent closer together than, and are twice the width of <see cref="F:System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal" />. This hatch pattern is not antialiased.</summary>
		// Token: 0x040009A7 RID: 2471
		DarkDownwardDiagonal,
		/// <summary>Specifies diagonal lines that slant to the left from top points to bottom points, are spaced 50 percent closer together than <see cref="F:System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal" />, and are twice its width, but the lines are not antialiased.</summary>
		// Token: 0x040009A8 RID: 2472
		DarkUpwardDiagonal,
		/// <summary>Specifies diagonal lines that slant to the right from top points to bottom points, have the same spacing as hatch style <see cref="F:System.Drawing.Drawing2D.HatchStyle.ForwardDiagonal" />, and are triple its width, but are not antialiased.</summary>
		// Token: 0x040009A9 RID: 2473
		WideDownwardDiagonal,
		/// <summary>Specifies diagonal lines that slant to the left from top points to bottom points, have the same spacing as hatch style <see cref="F:System.Drawing.Drawing2D.HatchStyle.BackwardDiagonal" />, and are triple its width, but are not antialiased.</summary>
		// Token: 0x040009AA RID: 2474
		WideUpwardDiagonal,
		/// <summary>Specifies vertical lines that are spaced 50 percent closer together than <see cref="F:System.Drawing.Drawing2D.HatchStyle.Vertical" />.</summary>
		// Token: 0x040009AB RID: 2475
		LightVertical,
		/// <summary>Specifies horizontal lines that are spaced 50 percent closer together than <see cref="F:System.Drawing.Drawing2D.HatchStyle.Horizontal" />.</summary>
		// Token: 0x040009AC RID: 2476
		LightHorizontal,
		/// <summary>Specifies vertical lines that are spaced 75 percent closer together than hatch style <see cref="F:System.Drawing.Drawing2D.HatchStyle.Vertical" /> (or 25 percent closer together than <see cref="F:System.Drawing.Drawing2D.HatchStyle.LightVertical" />).</summary>
		// Token: 0x040009AD RID: 2477
		NarrowVertical,
		/// <summary>Specifies horizontal lines that are spaced 75 percent closer together than hatch style <see cref="F:System.Drawing.Drawing2D.HatchStyle.Horizontal" /> (or 25 percent closer together than <see cref="F:System.Drawing.Drawing2D.HatchStyle.LightHorizontal" />).</summary>
		// Token: 0x040009AE RID: 2478
		NarrowHorizontal,
		/// <summary>Specifies vertical lines that are spaced 50 percent closer together than <see cref="F:System.Drawing.Drawing2D.HatchStyle.Vertical" /> and are twice its width.</summary>
		// Token: 0x040009AF RID: 2479
		DarkVertical,
		/// <summary>Specifies horizontal lines that are spaced 50 percent closer together than <see cref="F:System.Drawing.Drawing2D.HatchStyle.Horizontal" /> and are twice the width of <see cref="F:System.Drawing.Drawing2D.HatchStyle.Horizontal" />.</summary>
		// Token: 0x040009B0 RID: 2480
		DarkHorizontal,
		/// <summary>Specifies dashed diagonal lines, that slant to the right from top points to bottom points.</summary>
		// Token: 0x040009B1 RID: 2481
		DashedDownwardDiagonal,
		/// <summary>Specifies dashed diagonal lines, that slant to the left from top points to bottom points.</summary>
		// Token: 0x040009B2 RID: 2482
		DashedUpwardDiagonal,
		/// <summary>Specifies dashed horizontal lines.</summary>
		// Token: 0x040009B3 RID: 2483
		DashedHorizontal,
		/// <summary>Specifies dashed vertical lines.</summary>
		// Token: 0x040009B4 RID: 2484
		DashedVertical,
		/// <summary>Specifies a hatch that has the appearance of confetti.</summary>
		// Token: 0x040009B5 RID: 2485
		SmallConfetti,
		/// <summary>Specifies a hatch that has the appearance of confetti, and is composed of larger pieces than <see cref="F:System.Drawing.Drawing2D.HatchStyle.SmallConfetti" />.</summary>
		// Token: 0x040009B6 RID: 2486
		LargeConfetti,
		/// <summary>Specifies horizontal lines that are composed of zigzags.</summary>
		// Token: 0x040009B7 RID: 2487
		ZigZag,
		/// <summary>Specifies horizontal lines that are composed of tildes.</summary>
		// Token: 0x040009B8 RID: 2488
		Wave,
		/// <summary>Specifies a hatch that has the appearance of layered bricks that slant to the left from top points to bottom points.</summary>
		// Token: 0x040009B9 RID: 2489
		DiagonalBrick,
		/// <summary>Specifies a hatch that has the appearance of horizontally layered bricks.</summary>
		// Token: 0x040009BA RID: 2490
		HorizontalBrick,
		/// <summary>Specifies a hatch that has the appearance of a woven material.</summary>
		// Token: 0x040009BB RID: 2491
		Weave,
		/// <summary>Specifies a hatch that has the appearance of a plaid material.</summary>
		// Token: 0x040009BC RID: 2492
		Plaid,
		/// <summary>Specifies a hatch that has the appearance of divots.</summary>
		// Token: 0x040009BD RID: 2493
		Divot,
		/// <summary>Specifies horizontal and vertical lines, each of which is composed of dots, that cross.</summary>
		// Token: 0x040009BE RID: 2494
		DottedGrid,
		/// <summary>Specifies forward diagonal and backward diagonal lines, each of which is composed of dots, that cross.</summary>
		// Token: 0x040009BF RID: 2495
		DottedDiamond,
		/// <summary>Specifies a hatch that has the appearance of diagonally layered shingles that slant to the right from top points to bottom points.</summary>
		// Token: 0x040009C0 RID: 2496
		Shingle,
		/// <summary>Specifies a hatch that has the appearance of a trellis.</summary>
		// Token: 0x040009C1 RID: 2497
		Trellis,
		/// <summary>Specifies a hatch that has the appearance of spheres laid adjacent to one another.</summary>
		// Token: 0x040009C2 RID: 2498
		Sphere,
		/// <summary>Specifies horizontal and vertical lines that cross and are spaced 50 percent closer together than hatch style <see cref="F:System.Drawing.Drawing2D.HatchStyle.Cross" />.</summary>
		// Token: 0x040009C3 RID: 2499
		SmallGrid,
		/// <summary>Specifies a hatch that has the appearance of a checkerboard.</summary>
		// Token: 0x040009C4 RID: 2500
		SmallCheckerBoard,
		/// <summary>Specifies a hatch that has the appearance of a checkerboard with squares that are twice the size of <see cref="F:System.Drawing.Drawing2D.HatchStyle.SmallCheckerBoard" />.</summary>
		// Token: 0x040009C5 RID: 2501
		LargeCheckerBoard,
		/// <summary>Specifies forward diagonal and backward diagonal lines that cross but are not antialiased.</summary>
		// Token: 0x040009C6 RID: 2502
		OutlinedDiamond,
		/// <summary>Specifies a hatch that has the appearance of a checkerboard placed diagonally.</summary>
		// Token: 0x040009C7 RID: 2503
		SolidDiamond,
		/// <summary>Specifies the hatch style <see cref="F:System.Drawing.Drawing2D.HatchStyle.Cross" />.</summary>
		// Token: 0x040009C8 RID: 2504
		LargeGrid = 4,
		/// <summary>Specifies hatch style <see cref="F:System.Drawing.Drawing2D.HatchStyle.Horizontal" />.</summary>
		// Token: 0x040009C9 RID: 2505
		Min = 0,
		/// <summary>Specifies hatch style <see cref="F:System.Drawing.Drawing2D.HatchStyle.SolidDiamond" />.</summary>
		// Token: 0x040009CA RID: 2506
		Max = 4
	}
}
