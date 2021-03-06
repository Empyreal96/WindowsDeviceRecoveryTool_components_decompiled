using System;

namespace Standard
{
	// Token: 0x0200001E RID: 30
	[Flags]
	internal enum STATE_SYSTEM
	{
		// Token: 0x04000232 RID: 562
		UNAVAILABLE = 1,
		// Token: 0x04000233 RID: 563
		SELECTED = 2,
		// Token: 0x04000234 RID: 564
		FOCUSED = 4,
		// Token: 0x04000235 RID: 565
		PRESSED = 8,
		// Token: 0x04000236 RID: 566
		CHECKED = 16,
		// Token: 0x04000237 RID: 567
		MIXED = 32,
		// Token: 0x04000238 RID: 568
		INDETERMINATE = 32,
		// Token: 0x04000239 RID: 569
		READONLY = 64,
		// Token: 0x0400023A RID: 570
		HOTTRACKED = 128,
		// Token: 0x0400023B RID: 571
		DEFAULT = 256,
		// Token: 0x0400023C RID: 572
		EXPANDED = 512,
		// Token: 0x0400023D RID: 573
		COLLAPSED = 1024,
		// Token: 0x0400023E RID: 574
		BUSY = 2048,
		// Token: 0x0400023F RID: 575
		FLOATING = 4096,
		// Token: 0x04000240 RID: 576
		MARQUEED = 8192,
		// Token: 0x04000241 RID: 577
		ANIMATED = 16384,
		// Token: 0x04000242 RID: 578
		INVISIBLE = 32768,
		// Token: 0x04000243 RID: 579
		OFFSCREEN = 65536,
		// Token: 0x04000244 RID: 580
		SIZEABLE = 131072,
		// Token: 0x04000245 RID: 581
		MOVEABLE = 262144,
		// Token: 0x04000246 RID: 582
		SELFVOICING = 524288,
		// Token: 0x04000247 RID: 583
		FOCUSABLE = 1048576,
		// Token: 0x04000248 RID: 584
		SELECTABLE = 2097152,
		// Token: 0x04000249 RID: 585
		LINKED = 4194304,
		// Token: 0x0400024A RID: 586
		TRAVERSED = 8388608,
		// Token: 0x0400024B RID: 587
		MULTISELECTABLE = 16777216,
		// Token: 0x0400024C RID: 588
		EXTSELECTABLE = 33554432,
		// Token: 0x0400024D RID: 589
		ALERT_LOW = 67108864,
		// Token: 0x0400024E RID: 590
		ALERT_MEDIUM = 134217728,
		// Token: 0x0400024F RID: 591
		ALERT_HIGH = 268435456,
		// Token: 0x04000250 RID: 592
		PROTECTED = 536870912,
		// Token: 0x04000251 RID: 593
		VALID = 1073741823
	}
}
