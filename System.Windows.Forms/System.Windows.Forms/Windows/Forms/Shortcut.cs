using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	/// <summary>Specifies shortcut keys that can be used by menu items.</summary>
	// Token: 0x02000358 RID: 856
	[ComVisible(true)]
	public enum Shortcut
	{
		/// <summary>No shortcut key is associated with the menu item.</summary>
		// Token: 0x040020C8 RID: 8392
		None,
		/// <summary>The shortcut keys CTRL+A.</summary>
		// Token: 0x040020C9 RID: 8393
		CtrlA = 131137,
		/// <summary>The shortcut keys CTRL+B.</summary>
		// Token: 0x040020CA RID: 8394
		CtrlB,
		/// <summary>The shortcut keys CTRL+C.</summary>
		// Token: 0x040020CB RID: 8395
		CtrlC,
		/// <summary>The shortcut keys CTRL+D.</summary>
		// Token: 0x040020CC RID: 8396
		CtrlD,
		/// <summary>The shortcut keys CTRL+E.</summary>
		// Token: 0x040020CD RID: 8397
		CtrlE,
		/// <summary>The shortcut keys CTRL+F.</summary>
		// Token: 0x040020CE RID: 8398
		CtrlF,
		/// <summary>The shortcut keys CTRL+G.</summary>
		// Token: 0x040020CF RID: 8399
		CtrlG,
		/// <summary>The shortcut keys CTRL+H.</summary>
		// Token: 0x040020D0 RID: 8400
		CtrlH,
		/// <summary>The shortcut keys CTRL+I.</summary>
		// Token: 0x040020D1 RID: 8401
		CtrlI,
		/// <summary>The shortcut keys CTRL+J.</summary>
		// Token: 0x040020D2 RID: 8402
		CtrlJ,
		/// <summary>The shortcut keys CTRL+K.</summary>
		// Token: 0x040020D3 RID: 8403
		CtrlK,
		/// <summary>The shortcut keys CTRL+L.</summary>
		// Token: 0x040020D4 RID: 8404
		CtrlL,
		/// <summary>The shortcut keys CTRL+M.</summary>
		// Token: 0x040020D5 RID: 8405
		CtrlM,
		/// <summary>The shortcut keys CTRL+N.</summary>
		// Token: 0x040020D6 RID: 8406
		CtrlN,
		/// <summary>The shortcut keys CTRL+O.</summary>
		// Token: 0x040020D7 RID: 8407
		CtrlO,
		/// <summary>The shortcut keys CTRL+P.</summary>
		// Token: 0x040020D8 RID: 8408
		CtrlP,
		/// <summary>The shortcut keys CTRL+Q.</summary>
		// Token: 0x040020D9 RID: 8409
		CtrlQ,
		/// <summary>The shortcut keys CTRL+R.</summary>
		// Token: 0x040020DA RID: 8410
		CtrlR,
		/// <summary>The shortcut keys CTRL+S.</summary>
		// Token: 0x040020DB RID: 8411
		CtrlS,
		/// <summary>The shortcut keys CTRL+T.</summary>
		// Token: 0x040020DC RID: 8412
		CtrlT,
		/// <summary>The shortcut keys CTRL+U.</summary>
		// Token: 0x040020DD RID: 8413
		CtrlU,
		/// <summary>The shortcut keys CTRL+V.</summary>
		// Token: 0x040020DE RID: 8414
		CtrlV,
		/// <summary>The shortcut keys CTRL+W.</summary>
		// Token: 0x040020DF RID: 8415
		CtrlW,
		/// <summary>The shortcut keys CTRL+X.</summary>
		// Token: 0x040020E0 RID: 8416
		CtrlX,
		/// <summary>The shortcut keys CTRL+Y.</summary>
		// Token: 0x040020E1 RID: 8417
		CtrlY,
		/// <summary>The shortcut keys CTRL+Z.</summary>
		// Token: 0x040020E2 RID: 8418
		CtrlZ,
		/// <summary>The shortcut keys CTRL+SHIFT+A.</summary>
		// Token: 0x040020E3 RID: 8419
		CtrlShiftA = 196673,
		/// <summary>The shortcut keys CTRL+SHIFT+B.</summary>
		// Token: 0x040020E4 RID: 8420
		CtrlShiftB,
		/// <summary>The shortcut keys CTRL+SHIFT+C.</summary>
		// Token: 0x040020E5 RID: 8421
		CtrlShiftC,
		/// <summary>The shortcut keys CTRL+SHIFT+D.</summary>
		// Token: 0x040020E6 RID: 8422
		CtrlShiftD,
		/// <summary>The shortcut keys CTRL+SHIFT+E.</summary>
		// Token: 0x040020E7 RID: 8423
		CtrlShiftE,
		/// <summary>The shortcut keys CTRL+SHIFT+F.</summary>
		// Token: 0x040020E8 RID: 8424
		CtrlShiftF,
		/// <summary>The shortcut keys CTRL+SHIFT+G.</summary>
		// Token: 0x040020E9 RID: 8425
		CtrlShiftG,
		/// <summary>The shortcut keys CTRL+SHIFT+H.</summary>
		// Token: 0x040020EA RID: 8426
		CtrlShiftH,
		/// <summary>The shortcut keys CTRL+SHIFT+I.</summary>
		// Token: 0x040020EB RID: 8427
		CtrlShiftI,
		/// <summary>The shortcut keys CTRL+SHIFT+J.</summary>
		// Token: 0x040020EC RID: 8428
		CtrlShiftJ,
		/// <summary>The shortcut keys CTRL+SHIFT+K.</summary>
		// Token: 0x040020ED RID: 8429
		CtrlShiftK,
		/// <summary>The shortcut keys CTRL+SHIFT+L.</summary>
		// Token: 0x040020EE RID: 8430
		CtrlShiftL,
		/// <summary>The shortcut keys CTRL+SHIFT+M.</summary>
		// Token: 0x040020EF RID: 8431
		CtrlShiftM,
		/// <summary>The shortcut keys CTRL+SHIFT+N.</summary>
		// Token: 0x040020F0 RID: 8432
		CtrlShiftN,
		/// <summary>The shortcut keys CTRL+SHIFT+O.</summary>
		// Token: 0x040020F1 RID: 8433
		CtrlShiftO,
		/// <summary>The shortcut keys CTRL+SHIFT+P.</summary>
		// Token: 0x040020F2 RID: 8434
		CtrlShiftP,
		/// <summary>The shortcut keys CTRL+SHIFT+Q.</summary>
		// Token: 0x040020F3 RID: 8435
		CtrlShiftQ,
		/// <summary>The shortcut keys CTRL+SHIFT+R.</summary>
		// Token: 0x040020F4 RID: 8436
		CtrlShiftR,
		/// <summary>The shortcut keys CTRL+SHIFT+S.</summary>
		// Token: 0x040020F5 RID: 8437
		CtrlShiftS,
		/// <summary>The shortcut keys CTRL+SHIFT+T.</summary>
		// Token: 0x040020F6 RID: 8438
		CtrlShiftT,
		/// <summary>The shortcut keys CTRL+SHIFT+U.</summary>
		// Token: 0x040020F7 RID: 8439
		CtrlShiftU,
		/// <summary>The shortcut keys CTRL+SHIFT+V.</summary>
		// Token: 0x040020F8 RID: 8440
		CtrlShiftV,
		/// <summary>The shortcut keys CTRL+SHIFT+W.</summary>
		// Token: 0x040020F9 RID: 8441
		CtrlShiftW,
		/// <summary>The shortcut keys CTRL+SHIFT+X.</summary>
		// Token: 0x040020FA RID: 8442
		CtrlShiftX,
		/// <summary>The shortcut keys CTRL+SHIFT+Y.</summary>
		// Token: 0x040020FB RID: 8443
		CtrlShiftY,
		/// <summary>The shortcut keys CTRL+SHIFT+Z.</summary>
		// Token: 0x040020FC RID: 8444
		CtrlShiftZ,
		/// <summary>The shortcut key F1.</summary>
		// Token: 0x040020FD RID: 8445
		F1 = 112,
		/// <summary>The shortcut key F2.</summary>
		// Token: 0x040020FE RID: 8446
		F2,
		/// <summary>The shortcut key F3.</summary>
		// Token: 0x040020FF RID: 8447
		F3,
		/// <summary>The shortcut key F4.</summary>
		// Token: 0x04002100 RID: 8448
		F4,
		/// <summary>The shortcut key F5.</summary>
		// Token: 0x04002101 RID: 8449
		F5,
		/// <summary>The shortcut key F6.</summary>
		// Token: 0x04002102 RID: 8450
		F6,
		/// <summary>The shortcut key F7.</summary>
		// Token: 0x04002103 RID: 8451
		F7,
		/// <summary>The shortcut key F8.</summary>
		// Token: 0x04002104 RID: 8452
		F8,
		/// <summary>The shortcut key F9.</summary>
		// Token: 0x04002105 RID: 8453
		F9,
		/// <summary>The shortcut key F10.</summary>
		// Token: 0x04002106 RID: 8454
		F10,
		/// <summary>The shortcut key F11.</summary>
		// Token: 0x04002107 RID: 8455
		F11,
		/// <summary>The shortcut key F12.</summary>
		// Token: 0x04002108 RID: 8456
		F12,
		/// <summary>The shortcut keys SHIFT+F1.</summary>
		// Token: 0x04002109 RID: 8457
		ShiftF1 = 65648,
		/// <summary>The shortcut keys SHIFT+F2.</summary>
		// Token: 0x0400210A RID: 8458
		ShiftF2,
		/// <summary>The shortcut keys SHIFT+F3.</summary>
		// Token: 0x0400210B RID: 8459
		ShiftF3,
		/// <summary>The shortcut keys SHIFT+F4.</summary>
		// Token: 0x0400210C RID: 8460
		ShiftF4,
		/// <summary>The shortcut keys SHIFT+F5.</summary>
		// Token: 0x0400210D RID: 8461
		ShiftF5,
		/// <summary>The shortcut keys SHIFT+F6.</summary>
		// Token: 0x0400210E RID: 8462
		ShiftF6,
		/// <summary>The shortcut keys SHIFT+F7.</summary>
		// Token: 0x0400210F RID: 8463
		ShiftF7,
		/// <summary>The shortcut keys SHIFT+F8.</summary>
		// Token: 0x04002110 RID: 8464
		ShiftF8,
		/// <summary>The shortcut keys SHIFT+F9.</summary>
		// Token: 0x04002111 RID: 8465
		ShiftF9,
		/// <summary>The shortcut keys SHIFT+F10.</summary>
		// Token: 0x04002112 RID: 8466
		ShiftF10,
		/// <summary>The shortcut keys SHIFT+F11.</summary>
		// Token: 0x04002113 RID: 8467
		ShiftF11,
		/// <summary>The shortcut keys SHIFT+F12.</summary>
		// Token: 0x04002114 RID: 8468
		ShiftF12,
		/// <summary>The shortcut keys CTRL+F1.</summary>
		// Token: 0x04002115 RID: 8469
		CtrlF1 = 131184,
		/// <summary>The shortcut keys CTRL+F2.</summary>
		// Token: 0x04002116 RID: 8470
		CtrlF2,
		/// <summary>The shortcut keys CTRL+F3.</summary>
		// Token: 0x04002117 RID: 8471
		CtrlF3,
		/// <summary>The shortcut keys CTRL+F4.</summary>
		// Token: 0x04002118 RID: 8472
		CtrlF4,
		/// <summary>The shortcut keys CTRL+F5.</summary>
		// Token: 0x04002119 RID: 8473
		CtrlF5,
		/// <summary>The shortcut keys CTRL+F6.</summary>
		// Token: 0x0400211A RID: 8474
		CtrlF6,
		/// <summary>The shortcut keys CTRL+F7.</summary>
		// Token: 0x0400211B RID: 8475
		CtrlF7,
		/// <summary>The shortcut keys CTRL+F8.</summary>
		// Token: 0x0400211C RID: 8476
		CtrlF8,
		/// <summary>The shortcut keys CTRL+F9.</summary>
		// Token: 0x0400211D RID: 8477
		CtrlF9,
		/// <summary>The shortcut keys CTRL+F10.</summary>
		// Token: 0x0400211E RID: 8478
		CtrlF10,
		/// <summary>The shortcut keys CTRL+F11.</summary>
		// Token: 0x0400211F RID: 8479
		CtrlF11,
		/// <summary>The shortcut keys CTRL+F12.</summary>
		// Token: 0x04002120 RID: 8480
		CtrlF12,
		/// <summary>The shortcut keys CTRL+SHIFT+F1.</summary>
		// Token: 0x04002121 RID: 8481
		CtrlShiftF1 = 196720,
		/// <summary>The shortcut keys CTRL+SHIFT+F2.</summary>
		// Token: 0x04002122 RID: 8482
		CtrlShiftF2,
		/// <summary>The shortcut keys CTRL+SHIFT+F3.</summary>
		// Token: 0x04002123 RID: 8483
		CtrlShiftF3,
		/// <summary>The shortcut keys CTRL+SHIFT+F4.</summary>
		// Token: 0x04002124 RID: 8484
		CtrlShiftF4,
		/// <summary>The shortcut keys CTRL+SHIFT+F5.</summary>
		// Token: 0x04002125 RID: 8485
		CtrlShiftF5,
		/// <summary>The shortcut keys CTRL+SHIFT+F6.</summary>
		// Token: 0x04002126 RID: 8486
		CtrlShiftF6,
		/// <summary>The shortcut keys CTRL+SHIFT+F7.</summary>
		// Token: 0x04002127 RID: 8487
		CtrlShiftF7,
		/// <summary>The shortcut keys CTRL+SHIFT+F8.</summary>
		// Token: 0x04002128 RID: 8488
		CtrlShiftF8,
		/// <summary>The shortcut keys CTRL+SHIFT+F9.</summary>
		// Token: 0x04002129 RID: 8489
		CtrlShiftF9,
		/// <summary>The shortcut keys CTRL+SHIFT+F10.</summary>
		// Token: 0x0400212A RID: 8490
		CtrlShiftF10,
		/// <summary>The shortcut keys CTRL+SHIFT+F11.</summary>
		// Token: 0x0400212B RID: 8491
		CtrlShiftF11,
		/// <summary>The shortcut keys CTRL+SHIFT+F12.</summary>
		// Token: 0x0400212C RID: 8492
		CtrlShiftF12,
		/// <summary>The shortcut key INSERT.</summary>
		// Token: 0x0400212D RID: 8493
		Ins = 45,
		/// <summary>The shortcut keys CTRL+INSERT.</summary>
		// Token: 0x0400212E RID: 8494
		CtrlIns = 131117,
		/// <summary>The shortcut keys SHIFT+INSERT.</summary>
		// Token: 0x0400212F RID: 8495
		ShiftIns = 65581,
		/// <summary>The shortcut key DELETE.</summary>
		// Token: 0x04002130 RID: 8496
		Del = 46,
		/// <summary>The shortcut keys CTRL+DELETE.</summary>
		// Token: 0x04002131 RID: 8497
		CtrlDel = 131118,
		/// <summary>The shortcut keys SHIFT+DELETE.</summary>
		// Token: 0x04002132 RID: 8498
		ShiftDel = 65582,
		/// <summary>The shortcut keys ALT+RIGHTARROW.</summary>
		// Token: 0x04002133 RID: 8499
		AltRightArrow = 262183,
		/// <summary>The shortcut keys ALT+LEFTARROW.</summary>
		// Token: 0x04002134 RID: 8500
		AltLeftArrow = 262181,
		/// <summary>The shortcut keys ALT+UPARROW.</summary>
		// Token: 0x04002135 RID: 8501
		AltUpArrow,
		/// <summary>The shortcut keys ALT+DOWNARROW.</summary>
		// Token: 0x04002136 RID: 8502
		AltDownArrow = 262184,
		/// <summary>The shortcut keys ALT+BACKSPACE.</summary>
		// Token: 0x04002137 RID: 8503
		AltBksp = 262152,
		/// <summary>The shortcut keys ALT+F1.</summary>
		// Token: 0x04002138 RID: 8504
		AltF1 = 262256,
		/// <summary>The shortcut keys ALT+F2.</summary>
		// Token: 0x04002139 RID: 8505
		AltF2,
		/// <summary>The shortcut keys ALT+F3.</summary>
		// Token: 0x0400213A RID: 8506
		AltF3,
		/// <summary>The shortcut keys ALT+F4.</summary>
		// Token: 0x0400213B RID: 8507
		AltF4,
		/// <summary>The shortcut keys ALT+F5.</summary>
		// Token: 0x0400213C RID: 8508
		AltF5,
		/// <summary>The shortcut keys ALT+F6.</summary>
		// Token: 0x0400213D RID: 8509
		AltF6,
		/// <summary>The shortcut keys ALT+F7.</summary>
		// Token: 0x0400213E RID: 8510
		AltF7,
		/// <summary>The shortcut keys ALT+F8.</summary>
		// Token: 0x0400213F RID: 8511
		AltF8,
		/// <summary>The shortcut keys ALT+F9.</summary>
		// Token: 0x04002140 RID: 8512
		AltF9,
		/// <summary>The shortcut keys ALT+F10.</summary>
		// Token: 0x04002141 RID: 8513
		AltF10,
		/// <summary>The shortcut keys ALT+F11.</summary>
		// Token: 0x04002142 RID: 8514
		AltF11,
		/// <summary>The shortcut keys ALT+F12.</summary>
		// Token: 0x04002143 RID: 8515
		AltF12,
		/// <summary>The shortcut keys ALT+0.</summary>
		// Token: 0x04002144 RID: 8516
		Alt0 = 262192,
		/// <summary>The shortcut keys ALT+1.</summary>
		// Token: 0x04002145 RID: 8517
		Alt1,
		/// <summary>The shortcut keys ALT+2.</summary>
		// Token: 0x04002146 RID: 8518
		Alt2,
		/// <summary>The shortcut keys ALT+3.</summary>
		// Token: 0x04002147 RID: 8519
		Alt3,
		/// <summary>The shortcut keys ALT+4.</summary>
		// Token: 0x04002148 RID: 8520
		Alt4,
		/// <summary>The shortcut keys ALT+5.</summary>
		// Token: 0x04002149 RID: 8521
		Alt5,
		/// <summary>The shortcut keys ALT+6.</summary>
		// Token: 0x0400214A RID: 8522
		Alt6,
		/// <summary>The shortcut keys ALT+7.</summary>
		// Token: 0x0400214B RID: 8523
		Alt7,
		/// <summary>The shortcut keys ALT+8.</summary>
		// Token: 0x0400214C RID: 8524
		Alt8,
		/// <summary>The shortcut keys ALT+9.</summary>
		// Token: 0x0400214D RID: 8525
		Alt9,
		/// <summary>The shortcut keys CTRL+0.</summary>
		// Token: 0x0400214E RID: 8526
		Ctrl0 = 131120,
		/// <summary>The shortcut keys CTRL+1.</summary>
		// Token: 0x0400214F RID: 8527
		Ctrl1,
		/// <summary>The shortcut keys CTRL+2.</summary>
		// Token: 0x04002150 RID: 8528
		Ctrl2,
		/// <summary>The shortcut keys CTRL+3.</summary>
		// Token: 0x04002151 RID: 8529
		Ctrl3,
		/// <summary>The shortcut keys CTRL+4.</summary>
		// Token: 0x04002152 RID: 8530
		Ctrl4,
		/// <summary>The shortcut keys CTRL+5.</summary>
		// Token: 0x04002153 RID: 8531
		Ctrl5,
		/// <summary>The shortcut keys CTRL+6.</summary>
		// Token: 0x04002154 RID: 8532
		Ctrl6,
		/// <summary>The shortcut keys CTRL+7.</summary>
		// Token: 0x04002155 RID: 8533
		Ctrl7,
		/// <summary>The shortcut keys CTRL+8.</summary>
		// Token: 0x04002156 RID: 8534
		Ctrl8,
		/// <summary>The shortcut keys CTRL+9.</summary>
		// Token: 0x04002157 RID: 8535
		Ctrl9,
		/// <summary>The shortcut keys CTRL+SHIFT+0.</summary>
		// Token: 0x04002158 RID: 8536
		CtrlShift0 = 196656,
		/// <summary>The shortcut keys CTRL+SHIFT+1.</summary>
		// Token: 0x04002159 RID: 8537
		CtrlShift1,
		/// <summary>The shortcut keys CTRL+SHIFT+2.</summary>
		// Token: 0x0400215A RID: 8538
		CtrlShift2,
		/// <summary>The shortcut keys CTRL+SHIFT+3.</summary>
		// Token: 0x0400215B RID: 8539
		CtrlShift3,
		/// <summary>The shortcut keys CTRL+SHIFT+4.</summary>
		// Token: 0x0400215C RID: 8540
		CtrlShift4,
		/// <summary>The shortcut keys CTRL+SHIFT+5.</summary>
		// Token: 0x0400215D RID: 8541
		CtrlShift5,
		/// <summary>The shortcut keys CTRL+SHIFT+6.</summary>
		// Token: 0x0400215E RID: 8542
		CtrlShift6,
		/// <summary>The shortcut keys CTRL+SHIFT+7.</summary>
		// Token: 0x0400215F RID: 8543
		CtrlShift7,
		/// <summary>The shortcut keys CTRL+SHIFT+8.</summary>
		// Token: 0x04002160 RID: 8544
		CtrlShift8,
		/// <summary>The shortcut keys CTRL+SHIFT+9.</summary>
		// Token: 0x04002161 RID: 8545
		CtrlShift9
	}
}
