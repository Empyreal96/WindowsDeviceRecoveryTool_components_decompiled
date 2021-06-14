using System;

namespace System.Drawing.Printing
{
	/// <summary>Specifies the standard paper sizes.</summary>
	// Token: 0x02000059 RID: 89
	[Serializable]
	public enum PaperKind
	{
		/// <summary>The paper size is defined by the user.</summary>
		// Token: 0x04000628 RID: 1576
		Custom,
		/// <summary>Letter paper (8.5 in. by 11 in.).</summary>
		// Token: 0x04000629 RID: 1577
		Letter,
		/// <summary>Legal paper (8.5 in. by 14 in.).</summary>
		// Token: 0x0400062A RID: 1578
		Legal = 5,
		/// <summary>A4 paper (210 mm by 297 mm).</summary>
		// Token: 0x0400062B RID: 1579
		A4 = 9,
		/// <summary>C paper (17 in. by 22 in.).</summary>
		// Token: 0x0400062C RID: 1580
		CSheet = 24,
		/// <summary>D paper (22 in. by 34 in.).</summary>
		// Token: 0x0400062D RID: 1581
		DSheet,
		/// <summary>E paper (34 in. by 44 in.).</summary>
		// Token: 0x0400062E RID: 1582
		ESheet,
		/// <summary>Letter small paper (8.5 in. by 11 in.).</summary>
		// Token: 0x0400062F RID: 1583
		LetterSmall = 2,
		/// <summary>Tabloid paper (11 in. by 17 in.).</summary>
		// Token: 0x04000630 RID: 1584
		Tabloid,
		/// <summary>Ledger paper (17 in. by 11 in.).</summary>
		// Token: 0x04000631 RID: 1585
		Ledger,
		/// <summary>Statement paper (5.5 in. by 8.5 in.).</summary>
		// Token: 0x04000632 RID: 1586
		Statement = 6,
		/// <summary>Executive paper (7.25 in. by 10.5 in.).</summary>
		// Token: 0x04000633 RID: 1587
		Executive,
		/// <summary>A3 paper (297 mm by 420 mm).</summary>
		// Token: 0x04000634 RID: 1588
		A3,
		/// <summary>A4 small paper (210 mm by 297 mm).</summary>
		// Token: 0x04000635 RID: 1589
		A4Small = 10,
		/// <summary>A5 paper (148 mm by 210 mm).</summary>
		// Token: 0x04000636 RID: 1590
		A5,
		/// <summary>B4 paper (250 mm by 353 mm).</summary>
		// Token: 0x04000637 RID: 1591
		B4,
		/// <summary>B5 paper (176 mm by 250 mm).</summary>
		// Token: 0x04000638 RID: 1592
		B5,
		/// <summary>Folio paper (8.5 in. by 13 in.).</summary>
		// Token: 0x04000639 RID: 1593
		Folio,
		/// <summary>Quarto paper (215 mm by 275 mm).</summary>
		// Token: 0x0400063A RID: 1594
		Quarto,
		/// <summary>Standard paper (10 in. by 14 in.).</summary>
		// Token: 0x0400063B RID: 1595
		Standard10x14,
		/// <summary>Standard paper (11 in. by 17 in.).</summary>
		// Token: 0x0400063C RID: 1596
		Standard11x17,
		/// <summary>Note paper (8.5 in. by 11 in.).</summary>
		// Token: 0x0400063D RID: 1597
		Note,
		/// <summary>#9 envelope (3.875 in. by 8.875 in.).</summary>
		// Token: 0x0400063E RID: 1598
		Number9Envelope,
		/// <summary>#10 envelope (4.125 in. by 9.5 in.).</summary>
		// Token: 0x0400063F RID: 1599
		Number10Envelope,
		/// <summary>#11 envelope (4.5 in. by 10.375 in.).</summary>
		// Token: 0x04000640 RID: 1600
		Number11Envelope,
		/// <summary>#12 envelope (4.75 in. by 11 in.).</summary>
		// Token: 0x04000641 RID: 1601
		Number12Envelope,
		/// <summary>#14 envelope (5 in. by 11.5 in.).</summary>
		// Token: 0x04000642 RID: 1602
		Number14Envelope,
		/// <summary>DL envelope (110 mm by 220 mm).</summary>
		// Token: 0x04000643 RID: 1603
		DLEnvelope = 27,
		/// <summary>C5 envelope (162 mm by 229 mm).</summary>
		// Token: 0x04000644 RID: 1604
		C5Envelope,
		/// <summary>C3 envelope (324 mm by 458 mm).</summary>
		// Token: 0x04000645 RID: 1605
		C3Envelope,
		/// <summary>C4 envelope (229 mm by 324 mm).</summary>
		// Token: 0x04000646 RID: 1606
		C4Envelope,
		/// <summary>C6 envelope (114 mm by 162 mm).</summary>
		// Token: 0x04000647 RID: 1607
		C6Envelope,
		/// <summary>C65 envelope (114 mm by 229 mm).</summary>
		// Token: 0x04000648 RID: 1608
		C65Envelope,
		/// <summary>B4 envelope (250 mm by 353 mm).</summary>
		// Token: 0x04000649 RID: 1609
		B4Envelope,
		/// <summary>B5 envelope (176 mm by 250 mm).</summary>
		// Token: 0x0400064A RID: 1610
		B5Envelope,
		/// <summary>B6 envelope (176 mm by 125 mm).</summary>
		// Token: 0x0400064B RID: 1611
		B6Envelope,
		/// <summary>Italy envelope (110 mm by 230 mm).</summary>
		// Token: 0x0400064C RID: 1612
		ItalyEnvelope,
		/// <summary>Monarch envelope (3.875 in. by 7.5 in.).</summary>
		// Token: 0x0400064D RID: 1613
		MonarchEnvelope,
		/// <summary>6 3/4 envelope (3.625 in. by 6.5 in.).</summary>
		// Token: 0x0400064E RID: 1614
		PersonalEnvelope,
		/// <summary>US standard fanfold (14.875 in. by 11 in.).</summary>
		// Token: 0x0400064F RID: 1615
		USStandardFanfold,
		/// <summary>German standard fanfold (8.5 in. by 12 in.).</summary>
		// Token: 0x04000650 RID: 1616
		GermanStandardFanfold,
		/// <summary>German legal fanfold (8.5 in. by 13 in.).</summary>
		// Token: 0x04000651 RID: 1617
		GermanLegalFanfold,
		/// <summary>ISO B4 (250 mm by 353 mm).</summary>
		// Token: 0x04000652 RID: 1618
		IsoB4,
		/// <summary>Japanese postcard (100 mm by 148 mm).</summary>
		// Token: 0x04000653 RID: 1619
		JapanesePostcard,
		/// <summary>Standard paper (9 in. by 11 in.).</summary>
		// Token: 0x04000654 RID: 1620
		Standard9x11,
		/// <summary>Standard paper (10 in. by 11 in.).</summary>
		// Token: 0x04000655 RID: 1621
		Standard10x11,
		/// <summary>Standard paper (15 in. by 11 in.).</summary>
		// Token: 0x04000656 RID: 1622
		Standard15x11,
		/// <summary>Invitation envelope (220 mm by 220 mm).</summary>
		// Token: 0x04000657 RID: 1623
		InviteEnvelope,
		/// <summary>Letter extra paper (9.275 in. by 12 in.). This value is specific to the PostScript driver and is used only by Linotronic printers in order to conserve paper.</summary>
		// Token: 0x04000658 RID: 1624
		LetterExtra = 50,
		/// <summary>Legal extra paper (9.275 in. by 15 in.). This value is specific to the PostScript driver and is used only by Linotronic printers in order to conserve paper.</summary>
		// Token: 0x04000659 RID: 1625
		LegalExtra,
		/// <summary>Tabloid extra paper (11.69 in. by 18 in.). This value is specific to the PostScript driver and is used only by Linotronic printers in order to conserve paper.</summary>
		// Token: 0x0400065A RID: 1626
		TabloidExtra,
		/// <summary>A4 extra paper (236 mm by 322 mm). This value is specific to the PostScript driver and is used only by Linotronic printers to help save paper.</summary>
		// Token: 0x0400065B RID: 1627
		A4Extra,
		/// <summary>Letter transverse paper (8.275 in. by 11 in.).</summary>
		// Token: 0x0400065C RID: 1628
		LetterTransverse,
		/// <summary>A4 transverse paper (210 mm by 297 mm).</summary>
		// Token: 0x0400065D RID: 1629
		A4Transverse,
		/// <summary>Letter extra transverse paper (9.275 in. by 12 in.).</summary>
		// Token: 0x0400065E RID: 1630
		LetterExtraTransverse,
		/// <summary>SuperA/SuperA/A4 paper (227 mm by 356 mm).</summary>
		// Token: 0x0400065F RID: 1631
		APlus,
		/// <summary>SuperB/SuperB/A3 paper (305 mm by 487 mm).</summary>
		// Token: 0x04000660 RID: 1632
		BPlus,
		/// <summary>Letter plus paper (8.5 in. by 12.69 in.).</summary>
		// Token: 0x04000661 RID: 1633
		LetterPlus,
		/// <summary>A4 plus paper (210 mm by 330 mm).</summary>
		// Token: 0x04000662 RID: 1634
		A4Plus,
		/// <summary>A5 transverse paper (148 mm by 210 mm).</summary>
		// Token: 0x04000663 RID: 1635
		A5Transverse,
		/// <summary>JIS B5 transverse paper (182 mm by 257 mm).</summary>
		// Token: 0x04000664 RID: 1636
		B5Transverse,
		/// <summary>A3 extra paper (322 mm by 445 mm).</summary>
		// Token: 0x04000665 RID: 1637
		A3Extra,
		/// <summary>A5 extra paper (174 mm by 235 mm).</summary>
		// Token: 0x04000666 RID: 1638
		A5Extra,
		/// <summary>ISO B5 extra paper (201 mm by 276 mm).</summary>
		// Token: 0x04000667 RID: 1639
		B5Extra,
		/// <summary>A2 paper (420 mm by 594 mm).</summary>
		// Token: 0x04000668 RID: 1640
		A2,
		/// <summary>A3 transverse paper (297 mm by 420 mm).</summary>
		// Token: 0x04000669 RID: 1641
		A3Transverse,
		/// <summary>A3 extra transverse paper (322 mm by 445 mm).</summary>
		// Token: 0x0400066A RID: 1642
		A3ExtraTransverse,
		/// <summary>Japanese double postcard (200 mm by 148 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400066B RID: 1643
		JapaneseDoublePostcard,
		/// <summary>A6 paper (105 mm by 148 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400066C RID: 1644
		A6,
		/// <summary>Japanese Kaku #2 envelope. Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400066D RID: 1645
		JapaneseEnvelopeKakuNumber2,
		/// <summary>Japanese Kaku #3 envelope. Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400066E RID: 1646
		JapaneseEnvelopeKakuNumber3,
		/// <summary>Japanese Chou #3 envelope. Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400066F RID: 1647
		JapaneseEnvelopeChouNumber3,
		/// <summary>Japanese Chou #4 envelope. Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000670 RID: 1648
		JapaneseEnvelopeChouNumber4,
		/// <summary>Letter rotated paper (11 in. by 8.5 in.).</summary>
		// Token: 0x04000671 RID: 1649
		LetterRotated,
		/// <summary>A3 rotated paper (420 mm by 297 mm).</summary>
		// Token: 0x04000672 RID: 1650
		A3Rotated,
		/// <summary>A4 rotated paper (297 mm by 210 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000673 RID: 1651
		A4Rotated,
		/// <summary>A5 rotated paper (210 mm by 148 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000674 RID: 1652
		A5Rotated,
		/// <summary>JIS B4 rotated paper (364 mm by 257 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000675 RID: 1653
		B4JisRotated,
		/// <summary>JIS B5 rotated paper (257 mm by 182 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000676 RID: 1654
		B5JisRotated,
		/// <summary>Japanese rotated postcard (148 mm by 100 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000677 RID: 1655
		JapanesePostcardRotated,
		/// <summary>Japanese rotated double postcard (148 mm by 200 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000678 RID: 1656
		JapaneseDoublePostcardRotated,
		/// <summary>A6 rotated paper (148 mm by 105 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000679 RID: 1657
		A6Rotated,
		/// <summary>Japanese rotated Kaku #2 envelope. Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400067A RID: 1658
		JapaneseEnvelopeKakuNumber2Rotated,
		/// <summary>Japanese rotated Kaku #3 envelope. Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400067B RID: 1659
		JapaneseEnvelopeKakuNumber3Rotated,
		/// <summary>Japanese rotated Chou #3 envelope. Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400067C RID: 1660
		JapaneseEnvelopeChouNumber3Rotated,
		/// <summary>Japanese rotated Chou #4 envelope. Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400067D RID: 1661
		JapaneseEnvelopeChouNumber4Rotated,
		/// <summary>JIS B6 paper (128 mm by 182 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400067E RID: 1662
		B6Jis,
		/// <summary>JIS B6 rotated paper (182 mm by 128 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400067F RID: 1663
		B6JisRotated,
		/// <summary>Standard paper (12 in. by 11 in.). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000680 RID: 1664
		Standard12x11,
		/// <summary>Japanese You #4 envelope. Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000681 RID: 1665
		JapaneseEnvelopeYouNumber4,
		/// <summary>Japanese You #4 rotated envelope. Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000682 RID: 1666
		JapaneseEnvelopeYouNumber4Rotated,
		/// <summary> 16K paper (146 mm by 215 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000683 RID: 1667
		Prc16K,
		/// <summary> 32K paper (97 mm by 151 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000684 RID: 1668
		Prc32K,
		/// <summary> 32K big paper (97 mm by 151 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000685 RID: 1669
		Prc32KBig,
		/// <summary> #1 envelope (102 mm by 165 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000686 RID: 1670
		PrcEnvelopeNumber1,
		/// <summary> #2 envelope (102 mm by 176 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000687 RID: 1671
		PrcEnvelopeNumber2,
		/// <summary> #3 envelope (125 mm by 176 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000688 RID: 1672
		PrcEnvelopeNumber3,
		/// <summary> #4 envelope (110 mm by 208 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000689 RID: 1673
		PrcEnvelopeNumber4,
		/// <summary> #5 envelope (110 mm by 220 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400068A RID: 1674
		PrcEnvelopeNumber5,
		/// <summary> #6 envelope (120 mm by 230 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400068B RID: 1675
		PrcEnvelopeNumber6,
		/// <summary> #7 envelope (160 mm by 230 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400068C RID: 1676
		PrcEnvelopeNumber7,
		/// <summary> #8 envelope (120 mm by 309 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400068D RID: 1677
		PrcEnvelopeNumber8,
		/// <summary> #9 envelope (229 mm by 324 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400068E RID: 1678
		PrcEnvelopeNumber9,
		/// <summary> #10 envelope (324 mm by 458 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400068F RID: 1679
		PrcEnvelopeNumber10,
		/// <summary> 16K rotated paper (146 mm by 215 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000690 RID: 1680
		Prc16KRotated,
		/// <summary> 32K rotated paper (97 mm by 151 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000691 RID: 1681
		Prc32KRotated,
		/// <summary> 32K big rotated paper (97 mm by 151 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000692 RID: 1682
		Prc32KBigRotated,
		/// <summary> #1 rotated envelope (165 mm by 102 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000693 RID: 1683
		PrcEnvelopeNumber1Rotated,
		/// <summary> #2 rotated envelope (176 mm by 102 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000694 RID: 1684
		PrcEnvelopeNumber2Rotated,
		/// <summary> #3 rotated envelope (176 mm by 125 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000695 RID: 1685
		PrcEnvelopeNumber3Rotated,
		/// <summary> #4 rotated envelope (208 mm by 110 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000696 RID: 1686
		PrcEnvelopeNumber4Rotated,
		/// <summary> Envelope #5 rotated envelope (220 mm by 110 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000697 RID: 1687
		PrcEnvelopeNumber5Rotated,
		/// <summary> #6 rotated envelope (230 mm by 120 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000698 RID: 1688
		PrcEnvelopeNumber6Rotated,
		/// <summary> #7 rotated envelope (230 mm by 160 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x04000699 RID: 1689
		PrcEnvelopeNumber7Rotated,
		/// <summary> #8 rotated envelope (309 mm by 120 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400069A RID: 1690
		PrcEnvelopeNumber8Rotated,
		/// <summary> #9 rotated envelope (324 mm by 229 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400069B RID: 1691
		PrcEnvelopeNumber9Rotated,
		/// <summary> #10 rotated envelope (458 mm by 324 mm). Requires Windows 98, Windows NT 4.0, or later.</summary>
		// Token: 0x0400069C RID: 1692
		PrcEnvelopeNumber10Rotated
	}
}
