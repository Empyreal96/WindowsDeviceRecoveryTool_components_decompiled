using System;

namespace System.Drawing
{
	/// <summary>Specifies the known system colors.</summary>
	// Token: 0x02000024 RID: 36
	public enum KnownColor
	{
		/// <summary>The system-defined color of the active window's border.</summary>
		// Token: 0x040001A2 RID: 418
		ActiveBorder = 1,
		/// <summary>The system-defined color of the background of the active window's title bar.</summary>
		// Token: 0x040001A3 RID: 419
		ActiveCaption,
		/// <summary>The system-defined color of the text in the active window's title bar.</summary>
		// Token: 0x040001A4 RID: 420
		ActiveCaptionText,
		/// <summary>The system-defined color of the application workspace. The application workspace is the area in a multiple-document view that is not being occupied by documents.</summary>
		// Token: 0x040001A5 RID: 421
		AppWorkspace,
		/// <summary>The system-defined face color of a 3-D element.</summary>
		// Token: 0x040001A6 RID: 422
		Control,
		/// <summary>The system-defined shadow color of a 3-D element. The shadow color is applied to parts of a 3-D element that face away from the light source.</summary>
		// Token: 0x040001A7 RID: 423
		ControlDark,
		/// <summary>The system-defined color that is the dark shadow color of a 3-D element. The dark shadow color is applied to the parts of a 3-D element that are the darkest color.</summary>
		// Token: 0x040001A8 RID: 424
		ControlDarkDark,
		/// <summary>The system-defined color that is the light color of a 3-D element. The light color is applied to parts of a 3-D element that face the light source.</summary>
		// Token: 0x040001A9 RID: 425
		ControlLight,
		/// <summary>The system-defined highlight color of a 3-D element. The highlight color is applied to the parts of a 3-D element that are the lightest color.</summary>
		// Token: 0x040001AA RID: 426
		ControlLightLight,
		/// <summary>The system-defined color of text in a 3-D element.</summary>
		// Token: 0x040001AB RID: 427
		ControlText,
		/// <summary>The system-defined color of the desktop.</summary>
		// Token: 0x040001AC RID: 428
		Desktop,
		/// <summary>The system-defined color of dimmed text. Items in a list that are disabled are displayed in dimmed text.</summary>
		// Token: 0x040001AD RID: 429
		GrayText,
		/// <summary>The system-defined color of the background of selected items. This includes selected menu items as well as selected text. </summary>
		// Token: 0x040001AE RID: 430
		Highlight,
		/// <summary>The system-defined color of the text of selected items.</summary>
		// Token: 0x040001AF RID: 431
		HighlightText,
		/// <summary>The system-defined color used to designate a hot-tracked item. Single-clicking a hot-tracked item executes the item.</summary>
		// Token: 0x040001B0 RID: 432
		HotTrack,
		/// <summary>The system-defined color of an inactive window's border.</summary>
		// Token: 0x040001B1 RID: 433
		InactiveBorder,
		/// <summary>The system-defined color of the background of an inactive window's title bar.</summary>
		// Token: 0x040001B2 RID: 434
		InactiveCaption,
		/// <summary>The system-defined color of the text in an inactive window's title bar.</summary>
		// Token: 0x040001B3 RID: 435
		InactiveCaptionText,
		/// <summary>The system-defined color of the background of a ToolTip.</summary>
		// Token: 0x040001B4 RID: 436
		Info,
		/// <summary>The system-defined color of the text of a ToolTip.</summary>
		// Token: 0x040001B5 RID: 437
		InfoText,
		/// <summary>The system-defined color of a menu's background.</summary>
		// Token: 0x040001B6 RID: 438
		Menu,
		/// <summary>The system-defined color of a menu's text.</summary>
		// Token: 0x040001B7 RID: 439
		MenuText,
		/// <summary>The system-defined color of the background of a scroll bar.</summary>
		// Token: 0x040001B8 RID: 440
		ScrollBar,
		/// <summary>The system-defined color of the background in the client area of a window.</summary>
		// Token: 0x040001B9 RID: 441
		Window,
		/// <summary>The system-defined color of a window frame.</summary>
		// Token: 0x040001BA RID: 442
		WindowFrame,
		/// <summary>The system-defined color of the text in the client area of a window.</summary>
		// Token: 0x040001BB RID: 443
		WindowText,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001BC RID: 444
		Transparent,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001BD RID: 445
		AliceBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001BE RID: 446
		AntiqueWhite,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001BF RID: 447
		Aqua,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001C0 RID: 448
		Aquamarine,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001C1 RID: 449
		Azure,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001C2 RID: 450
		Beige,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001C3 RID: 451
		Bisque,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001C4 RID: 452
		Black,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001C5 RID: 453
		BlanchedAlmond,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001C6 RID: 454
		Blue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001C7 RID: 455
		BlueViolet,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001C8 RID: 456
		Brown,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001C9 RID: 457
		BurlyWood,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001CA RID: 458
		CadetBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001CB RID: 459
		Chartreuse,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001CC RID: 460
		Chocolate,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001CD RID: 461
		Coral,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001CE RID: 462
		CornflowerBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001CF RID: 463
		Cornsilk,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001D0 RID: 464
		Crimson,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001D1 RID: 465
		Cyan,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001D2 RID: 466
		DarkBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001D3 RID: 467
		DarkCyan,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001D4 RID: 468
		DarkGoldenrod,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001D5 RID: 469
		DarkGray,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001D6 RID: 470
		DarkGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001D7 RID: 471
		DarkKhaki,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001D8 RID: 472
		DarkMagenta,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001D9 RID: 473
		DarkOliveGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001DA RID: 474
		DarkOrange,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001DB RID: 475
		DarkOrchid,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001DC RID: 476
		DarkRed,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001DD RID: 477
		DarkSalmon,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001DE RID: 478
		DarkSeaGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001DF RID: 479
		DarkSlateBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001E0 RID: 480
		DarkSlateGray,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001E1 RID: 481
		DarkTurquoise,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001E2 RID: 482
		DarkViolet,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001E3 RID: 483
		DeepPink,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001E4 RID: 484
		DeepSkyBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001E5 RID: 485
		DimGray,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001E6 RID: 486
		DodgerBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001E7 RID: 487
		Firebrick,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001E8 RID: 488
		FloralWhite,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001E9 RID: 489
		ForestGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001EA RID: 490
		Fuchsia,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001EB RID: 491
		Gainsboro,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001EC RID: 492
		GhostWhite,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001ED RID: 493
		Gold,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001EE RID: 494
		Goldenrod,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001EF RID: 495
		Gray,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001F0 RID: 496
		Green,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001F1 RID: 497
		GreenYellow,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001F2 RID: 498
		Honeydew,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001F3 RID: 499
		HotPink,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001F4 RID: 500
		IndianRed,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001F5 RID: 501
		Indigo,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001F6 RID: 502
		Ivory,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001F7 RID: 503
		Khaki,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001F8 RID: 504
		Lavender,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001F9 RID: 505
		LavenderBlush,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001FA RID: 506
		LawnGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001FB RID: 507
		LemonChiffon,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001FC RID: 508
		LightBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001FD RID: 509
		LightCoral,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001FE RID: 510
		LightCyan,
		/// <summary>A system-defined color.</summary>
		// Token: 0x040001FF RID: 511
		LightGoldenrodYellow,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000200 RID: 512
		LightGray,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000201 RID: 513
		LightGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000202 RID: 514
		LightPink,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000203 RID: 515
		LightSalmon,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000204 RID: 516
		LightSeaGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000205 RID: 517
		LightSkyBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000206 RID: 518
		LightSlateGray,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000207 RID: 519
		LightSteelBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000208 RID: 520
		LightYellow,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000209 RID: 521
		Lime,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400020A RID: 522
		LimeGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400020B RID: 523
		Linen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400020C RID: 524
		Magenta,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400020D RID: 525
		Maroon,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400020E RID: 526
		MediumAquamarine,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400020F RID: 527
		MediumBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000210 RID: 528
		MediumOrchid,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000211 RID: 529
		MediumPurple,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000212 RID: 530
		MediumSeaGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000213 RID: 531
		MediumSlateBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000214 RID: 532
		MediumSpringGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000215 RID: 533
		MediumTurquoise,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000216 RID: 534
		MediumVioletRed,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000217 RID: 535
		MidnightBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000218 RID: 536
		MintCream,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000219 RID: 537
		MistyRose,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400021A RID: 538
		Moccasin,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400021B RID: 539
		NavajoWhite,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400021C RID: 540
		Navy,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400021D RID: 541
		OldLace,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400021E RID: 542
		Olive,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400021F RID: 543
		OliveDrab,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000220 RID: 544
		Orange,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000221 RID: 545
		OrangeRed,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000222 RID: 546
		Orchid,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000223 RID: 547
		PaleGoldenrod,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000224 RID: 548
		PaleGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000225 RID: 549
		PaleTurquoise,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000226 RID: 550
		PaleVioletRed,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000227 RID: 551
		PapayaWhip,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000228 RID: 552
		PeachPuff,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000229 RID: 553
		Peru,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400022A RID: 554
		Pink,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400022B RID: 555
		Plum,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400022C RID: 556
		PowderBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400022D RID: 557
		Purple,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400022E RID: 558
		Red,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400022F RID: 559
		RosyBrown,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000230 RID: 560
		RoyalBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000231 RID: 561
		SaddleBrown,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000232 RID: 562
		Salmon,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000233 RID: 563
		SandyBrown,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000234 RID: 564
		SeaGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000235 RID: 565
		SeaShell,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000236 RID: 566
		Sienna,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000237 RID: 567
		Silver,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000238 RID: 568
		SkyBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000239 RID: 569
		SlateBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400023A RID: 570
		SlateGray,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400023B RID: 571
		Snow,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400023C RID: 572
		SpringGreen,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400023D RID: 573
		SteelBlue,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400023E RID: 574
		Tan,
		/// <summary>A system-defined color.</summary>
		// Token: 0x0400023F RID: 575
		Teal,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000240 RID: 576
		Thistle,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000241 RID: 577
		Tomato,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000242 RID: 578
		Turquoise,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000243 RID: 579
		Violet,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000244 RID: 580
		Wheat,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000245 RID: 581
		White,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000246 RID: 582
		WhiteSmoke,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000247 RID: 583
		Yellow,
		/// <summary>A system-defined color.</summary>
		// Token: 0x04000248 RID: 584
		YellowGreen,
		/// <summary>The system-defined face color of a 3-D element.</summary>
		// Token: 0x04000249 RID: 585
		ButtonFace,
		/// <summary>The system-defined color that is the highlight color of a 3-D element. This color is applied to parts of a 3-D element that face the light source.</summary>
		// Token: 0x0400024A RID: 586
		ButtonHighlight,
		/// <summary>The system-defined color that is the shadow color of a 3-D element. This color is applied to parts of a 3-D element that face away from the light source.</summary>
		// Token: 0x0400024B RID: 587
		ButtonShadow,
		/// <summary>The system-defined color of the lightest color in the color gradient of an active window's title bar.</summary>
		// Token: 0x0400024C RID: 588
		GradientActiveCaption,
		/// <summary>The system-defined color of the lightest color in the color gradient of an inactive window's title bar. </summary>
		// Token: 0x0400024D RID: 589
		GradientInactiveCaption,
		/// <summary>The system-defined color of the background of a menu bar.</summary>
		// Token: 0x0400024E RID: 590
		MenuBar,
		/// <summary>The system-defined color used to highlight menu items when the menu appears as a flat menu.</summary>
		// Token: 0x0400024F RID: 591
		MenuHighlight
	}
}
