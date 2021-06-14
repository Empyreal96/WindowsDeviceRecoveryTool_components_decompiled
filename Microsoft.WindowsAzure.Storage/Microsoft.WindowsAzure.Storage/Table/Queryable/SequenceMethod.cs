using System;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000127 RID: 295
	internal enum SequenceMethod
	{
		// Token: 0x040005D5 RID: 1493
		Where,
		// Token: 0x040005D6 RID: 1494
		WhereOrdinal,
		// Token: 0x040005D7 RID: 1495
		OfType,
		// Token: 0x040005D8 RID: 1496
		Cast,
		// Token: 0x040005D9 RID: 1497
		Select,
		// Token: 0x040005DA RID: 1498
		SelectOrdinal,
		// Token: 0x040005DB RID: 1499
		SelectMany,
		// Token: 0x040005DC RID: 1500
		SelectManyOrdinal,
		// Token: 0x040005DD RID: 1501
		SelectManyResultSelector,
		// Token: 0x040005DE RID: 1502
		SelectManyOrdinalResultSelector,
		// Token: 0x040005DF RID: 1503
		Join,
		// Token: 0x040005E0 RID: 1504
		JoinComparer,
		// Token: 0x040005E1 RID: 1505
		GroupJoin,
		// Token: 0x040005E2 RID: 1506
		GroupJoinComparer,
		// Token: 0x040005E3 RID: 1507
		OrderBy,
		// Token: 0x040005E4 RID: 1508
		OrderByComparer,
		// Token: 0x040005E5 RID: 1509
		OrderByDescending,
		// Token: 0x040005E6 RID: 1510
		OrderByDescendingComparer,
		// Token: 0x040005E7 RID: 1511
		ThenBy,
		// Token: 0x040005E8 RID: 1512
		ThenByComparer,
		// Token: 0x040005E9 RID: 1513
		ThenByDescending,
		// Token: 0x040005EA RID: 1514
		ThenByDescendingComparer,
		// Token: 0x040005EB RID: 1515
		Take,
		// Token: 0x040005EC RID: 1516
		TakeWhile,
		// Token: 0x040005ED RID: 1517
		TakeWhileOrdinal,
		// Token: 0x040005EE RID: 1518
		Skip,
		// Token: 0x040005EF RID: 1519
		SkipWhile,
		// Token: 0x040005F0 RID: 1520
		SkipWhileOrdinal,
		// Token: 0x040005F1 RID: 1521
		GroupBy,
		// Token: 0x040005F2 RID: 1522
		GroupByComparer,
		// Token: 0x040005F3 RID: 1523
		GroupByElementSelector,
		// Token: 0x040005F4 RID: 1524
		GroupByElementSelectorComparer,
		// Token: 0x040005F5 RID: 1525
		GroupByResultSelector,
		// Token: 0x040005F6 RID: 1526
		GroupByResultSelectorComparer,
		// Token: 0x040005F7 RID: 1527
		GroupByElementSelectorResultSelector,
		// Token: 0x040005F8 RID: 1528
		GroupByElementSelectorResultSelectorComparer,
		// Token: 0x040005F9 RID: 1529
		Distinct,
		// Token: 0x040005FA RID: 1530
		DistinctComparer,
		// Token: 0x040005FB RID: 1531
		Concat,
		// Token: 0x040005FC RID: 1532
		Union,
		// Token: 0x040005FD RID: 1533
		UnionComparer,
		// Token: 0x040005FE RID: 1534
		Intersect,
		// Token: 0x040005FF RID: 1535
		IntersectComparer,
		// Token: 0x04000600 RID: 1536
		Except,
		// Token: 0x04000601 RID: 1537
		ExceptComparer,
		// Token: 0x04000602 RID: 1538
		First,
		// Token: 0x04000603 RID: 1539
		FirstPredicate,
		// Token: 0x04000604 RID: 1540
		FirstOrDefault,
		// Token: 0x04000605 RID: 1541
		FirstOrDefaultPredicate,
		// Token: 0x04000606 RID: 1542
		Last,
		// Token: 0x04000607 RID: 1543
		LastPredicate,
		// Token: 0x04000608 RID: 1544
		LastOrDefault,
		// Token: 0x04000609 RID: 1545
		LastOrDefaultPredicate,
		// Token: 0x0400060A RID: 1546
		Single,
		// Token: 0x0400060B RID: 1547
		SinglePredicate,
		// Token: 0x0400060C RID: 1548
		SingleOrDefault,
		// Token: 0x0400060D RID: 1549
		SingleOrDefaultPredicate,
		// Token: 0x0400060E RID: 1550
		ElementAt,
		// Token: 0x0400060F RID: 1551
		ElementAtOrDefault,
		// Token: 0x04000610 RID: 1552
		DefaultIfEmpty,
		// Token: 0x04000611 RID: 1553
		DefaultIfEmptyValue,
		// Token: 0x04000612 RID: 1554
		Contains,
		// Token: 0x04000613 RID: 1555
		ContainsComparer,
		// Token: 0x04000614 RID: 1556
		Reverse,
		// Token: 0x04000615 RID: 1557
		Empty,
		// Token: 0x04000616 RID: 1558
		SequenceEqual,
		// Token: 0x04000617 RID: 1559
		SequenceEqualComparer,
		// Token: 0x04000618 RID: 1560
		Any,
		// Token: 0x04000619 RID: 1561
		AnyPredicate,
		// Token: 0x0400061A RID: 1562
		All,
		// Token: 0x0400061B RID: 1563
		Count,
		// Token: 0x0400061C RID: 1564
		CountPredicate,
		// Token: 0x0400061D RID: 1565
		LongCount,
		// Token: 0x0400061E RID: 1566
		LongCountPredicate,
		// Token: 0x0400061F RID: 1567
		Min,
		// Token: 0x04000620 RID: 1568
		MinSelector,
		// Token: 0x04000621 RID: 1569
		Max,
		// Token: 0x04000622 RID: 1570
		MaxSelector,
		// Token: 0x04000623 RID: 1571
		MinInt,
		// Token: 0x04000624 RID: 1572
		MinNullableInt,
		// Token: 0x04000625 RID: 1573
		MinLong,
		// Token: 0x04000626 RID: 1574
		MinNullableLong,
		// Token: 0x04000627 RID: 1575
		MinDouble,
		// Token: 0x04000628 RID: 1576
		MinNullableDouble,
		// Token: 0x04000629 RID: 1577
		MinDecimal,
		// Token: 0x0400062A RID: 1578
		MinNullableDecimal,
		// Token: 0x0400062B RID: 1579
		MinSingle,
		// Token: 0x0400062C RID: 1580
		MinNullableSingle,
		// Token: 0x0400062D RID: 1581
		MinIntSelector,
		// Token: 0x0400062E RID: 1582
		MinNullableIntSelector,
		// Token: 0x0400062F RID: 1583
		MinLongSelector,
		// Token: 0x04000630 RID: 1584
		MinNullableLongSelector,
		// Token: 0x04000631 RID: 1585
		MinDoubleSelector,
		// Token: 0x04000632 RID: 1586
		MinNullableDoubleSelector,
		// Token: 0x04000633 RID: 1587
		MinDecimalSelector,
		// Token: 0x04000634 RID: 1588
		MinNullableDecimalSelector,
		// Token: 0x04000635 RID: 1589
		MinSingleSelector,
		// Token: 0x04000636 RID: 1590
		MinNullableSingleSelector,
		// Token: 0x04000637 RID: 1591
		MaxInt,
		// Token: 0x04000638 RID: 1592
		MaxNullableInt,
		// Token: 0x04000639 RID: 1593
		MaxLong,
		// Token: 0x0400063A RID: 1594
		MaxNullableLong,
		// Token: 0x0400063B RID: 1595
		MaxDouble,
		// Token: 0x0400063C RID: 1596
		MaxNullableDouble,
		// Token: 0x0400063D RID: 1597
		MaxDecimal,
		// Token: 0x0400063E RID: 1598
		MaxNullableDecimal,
		// Token: 0x0400063F RID: 1599
		MaxSingle,
		// Token: 0x04000640 RID: 1600
		MaxNullableSingle,
		// Token: 0x04000641 RID: 1601
		MaxIntSelector,
		// Token: 0x04000642 RID: 1602
		MaxNullableIntSelector,
		// Token: 0x04000643 RID: 1603
		MaxLongSelector,
		// Token: 0x04000644 RID: 1604
		MaxNullableLongSelector,
		// Token: 0x04000645 RID: 1605
		MaxDoubleSelector,
		// Token: 0x04000646 RID: 1606
		MaxNullableDoubleSelector,
		// Token: 0x04000647 RID: 1607
		MaxDecimalSelector,
		// Token: 0x04000648 RID: 1608
		MaxNullableDecimalSelector,
		// Token: 0x04000649 RID: 1609
		MaxSingleSelector,
		// Token: 0x0400064A RID: 1610
		MaxNullableSingleSelector,
		// Token: 0x0400064B RID: 1611
		SumInt,
		// Token: 0x0400064C RID: 1612
		SumNullableInt,
		// Token: 0x0400064D RID: 1613
		SumLong,
		// Token: 0x0400064E RID: 1614
		SumNullableLong,
		// Token: 0x0400064F RID: 1615
		SumDouble,
		// Token: 0x04000650 RID: 1616
		SumNullableDouble,
		// Token: 0x04000651 RID: 1617
		SumDecimal,
		// Token: 0x04000652 RID: 1618
		SumNullableDecimal,
		// Token: 0x04000653 RID: 1619
		SumSingle,
		// Token: 0x04000654 RID: 1620
		SumNullableSingle,
		// Token: 0x04000655 RID: 1621
		SumIntSelector,
		// Token: 0x04000656 RID: 1622
		SumNullableIntSelector,
		// Token: 0x04000657 RID: 1623
		SumLongSelector,
		// Token: 0x04000658 RID: 1624
		SumNullableLongSelector,
		// Token: 0x04000659 RID: 1625
		SumDoubleSelector,
		// Token: 0x0400065A RID: 1626
		SumNullableDoubleSelector,
		// Token: 0x0400065B RID: 1627
		SumDecimalSelector,
		// Token: 0x0400065C RID: 1628
		SumNullableDecimalSelector,
		// Token: 0x0400065D RID: 1629
		SumSingleSelector,
		// Token: 0x0400065E RID: 1630
		SumNullableSingleSelector,
		// Token: 0x0400065F RID: 1631
		AverageInt,
		// Token: 0x04000660 RID: 1632
		AverageNullableInt,
		// Token: 0x04000661 RID: 1633
		AverageLong,
		// Token: 0x04000662 RID: 1634
		AverageNullableLong,
		// Token: 0x04000663 RID: 1635
		AverageDouble,
		// Token: 0x04000664 RID: 1636
		AverageNullableDouble,
		// Token: 0x04000665 RID: 1637
		AverageDecimal,
		// Token: 0x04000666 RID: 1638
		AverageNullableDecimal,
		// Token: 0x04000667 RID: 1639
		AverageSingle,
		// Token: 0x04000668 RID: 1640
		AverageNullableSingle,
		// Token: 0x04000669 RID: 1641
		AverageIntSelector,
		// Token: 0x0400066A RID: 1642
		AverageNullableIntSelector,
		// Token: 0x0400066B RID: 1643
		AverageLongSelector,
		// Token: 0x0400066C RID: 1644
		AverageNullableLongSelector,
		// Token: 0x0400066D RID: 1645
		AverageDoubleSelector,
		// Token: 0x0400066E RID: 1646
		AverageNullableDoubleSelector,
		// Token: 0x0400066F RID: 1647
		AverageDecimalSelector,
		// Token: 0x04000670 RID: 1648
		AverageNullableDecimalSelector,
		// Token: 0x04000671 RID: 1649
		AverageSingleSelector,
		// Token: 0x04000672 RID: 1650
		AverageNullableSingleSelector,
		// Token: 0x04000673 RID: 1651
		Aggregate,
		// Token: 0x04000674 RID: 1652
		AggregateSeed,
		// Token: 0x04000675 RID: 1653
		AggregateSeedSelector,
		// Token: 0x04000676 RID: 1654
		AsQueryable,
		// Token: 0x04000677 RID: 1655
		AsQueryableGeneric,
		// Token: 0x04000678 RID: 1656
		AsEnumerable,
		// Token: 0x04000679 RID: 1657
		ToList,
		// Token: 0x0400067A RID: 1658
		NotSupported,
		// Token: 0x0400067B RID: 1659
		WithOptions
	}
}
