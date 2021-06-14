using System;

namespace System.Data.Services.Client
{
	// Token: 0x020000BE RID: 190
	internal enum SequenceMethod
	{
		// Token: 0x04000348 RID: 840
		Where,
		// Token: 0x04000349 RID: 841
		WhereOrdinal,
		// Token: 0x0400034A RID: 842
		OfType,
		// Token: 0x0400034B RID: 843
		Cast,
		// Token: 0x0400034C RID: 844
		Select,
		// Token: 0x0400034D RID: 845
		SelectOrdinal,
		// Token: 0x0400034E RID: 846
		SelectMany,
		// Token: 0x0400034F RID: 847
		SelectManyOrdinal,
		// Token: 0x04000350 RID: 848
		SelectManyResultSelector,
		// Token: 0x04000351 RID: 849
		SelectManyOrdinalResultSelector,
		// Token: 0x04000352 RID: 850
		Join,
		// Token: 0x04000353 RID: 851
		JoinComparer,
		// Token: 0x04000354 RID: 852
		GroupJoin,
		// Token: 0x04000355 RID: 853
		GroupJoinComparer,
		// Token: 0x04000356 RID: 854
		OrderBy,
		// Token: 0x04000357 RID: 855
		OrderByComparer,
		// Token: 0x04000358 RID: 856
		OrderByDescending,
		// Token: 0x04000359 RID: 857
		OrderByDescendingComparer,
		// Token: 0x0400035A RID: 858
		ThenBy,
		// Token: 0x0400035B RID: 859
		ThenByComparer,
		// Token: 0x0400035C RID: 860
		ThenByDescending,
		// Token: 0x0400035D RID: 861
		ThenByDescendingComparer,
		// Token: 0x0400035E RID: 862
		Take,
		// Token: 0x0400035F RID: 863
		TakeWhile,
		// Token: 0x04000360 RID: 864
		TakeWhileOrdinal,
		// Token: 0x04000361 RID: 865
		Skip,
		// Token: 0x04000362 RID: 866
		SkipWhile,
		// Token: 0x04000363 RID: 867
		SkipWhileOrdinal,
		// Token: 0x04000364 RID: 868
		GroupBy,
		// Token: 0x04000365 RID: 869
		GroupByComparer,
		// Token: 0x04000366 RID: 870
		GroupByElementSelector,
		// Token: 0x04000367 RID: 871
		GroupByElementSelectorComparer,
		// Token: 0x04000368 RID: 872
		GroupByResultSelector,
		// Token: 0x04000369 RID: 873
		GroupByResultSelectorComparer,
		// Token: 0x0400036A RID: 874
		GroupByElementSelectorResultSelector,
		// Token: 0x0400036B RID: 875
		GroupByElementSelectorResultSelectorComparer,
		// Token: 0x0400036C RID: 876
		Distinct,
		// Token: 0x0400036D RID: 877
		DistinctComparer,
		// Token: 0x0400036E RID: 878
		Concat,
		// Token: 0x0400036F RID: 879
		Union,
		// Token: 0x04000370 RID: 880
		UnionComparer,
		// Token: 0x04000371 RID: 881
		Intersect,
		// Token: 0x04000372 RID: 882
		IntersectComparer,
		// Token: 0x04000373 RID: 883
		Except,
		// Token: 0x04000374 RID: 884
		ExceptComparer,
		// Token: 0x04000375 RID: 885
		First,
		// Token: 0x04000376 RID: 886
		FirstPredicate,
		// Token: 0x04000377 RID: 887
		FirstOrDefault,
		// Token: 0x04000378 RID: 888
		FirstOrDefaultPredicate,
		// Token: 0x04000379 RID: 889
		Last,
		// Token: 0x0400037A RID: 890
		LastPredicate,
		// Token: 0x0400037B RID: 891
		LastOrDefault,
		// Token: 0x0400037C RID: 892
		LastOrDefaultPredicate,
		// Token: 0x0400037D RID: 893
		Single,
		// Token: 0x0400037E RID: 894
		SinglePredicate,
		// Token: 0x0400037F RID: 895
		SingleOrDefault,
		// Token: 0x04000380 RID: 896
		SingleOrDefaultPredicate,
		// Token: 0x04000381 RID: 897
		ElementAt,
		// Token: 0x04000382 RID: 898
		ElementAtOrDefault,
		// Token: 0x04000383 RID: 899
		DefaultIfEmpty,
		// Token: 0x04000384 RID: 900
		DefaultIfEmptyValue,
		// Token: 0x04000385 RID: 901
		Contains,
		// Token: 0x04000386 RID: 902
		ContainsComparer,
		// Token: 0x04000387 RID: 903
		Reverse,
		// Token: 0x04000388 RID: 904
		Empty,
		// Token: 0x04000389 RID: 905
		SequenceEqual,
		// Token: 0x0400038A RID: 906
		SequenceEqualComparer,
		// Token: 0x0400038B RID: 907
		Any,
		// Token: 0x0400038C RID: 908
		AnyPredicate,
		// Token: 0x0400038D RID: 909
		All,
		// Token: 0x0400038E RID: 910
		Count,
		// Token: 0x0400038F RID: 911
		CountPredicate,
		// Token: 0x04000390 RID: 912
		LongCount,
		// Token: 0x04000391 RID: 913
		LongCountPredicate,
		// Token: 0x04000392 RID: 914
		Min,
		// Token: 0x04000393 RID: 915
		MinSelector,
		// Token: 0x04000394 RID: 916
		Max,
		// Token: 0x04000395 RID: 917
		MaxSelector,
		// Token: 0x04000396 RID: 918
		MinInt,
		// Token: 0x04000397 RID: 919
		MinNullableInt,
		// Token: 0x04000398 RID: 920
		MinLong,
		// Token: 0x04000399 RID: 921
		MinNullableLong,
		// Token: 0x0400039A RID: 922
		MinDouble,
		// Token: 0x0400039B RID: 923
		MinNullableDouble,
		// Token: 0x0400039C RID: 924
		MinDecimal,
		// Token: 0x0400039D RID: 925
		MinNullableDecimal,
		// Token: 0x0400039E RID: 926
		MinSingle,
		// Token: 0x0400039F RID: 927
		MinNullableSingle,
		// Token: 0x040003A0 RID: 928
		MinIntSelector,
		// Token: 0x040003A1 RID: 929
		MinNullableIntSelector,
		// Token: 0x040003A2 RID: 930
		MinLongSelector,
		// Token: 0x040003A3 RID: 931
		MinNullableLongSelector,
		// Token: 0x040003A4 RID: 932
		MinDoubleSelector,
		// Token: 0x040003A5 RID: 933
		MinNullableDoubleSelector,
		// Token: 0x040003A6 RID: 934
		MinDecimalSelector,
		// Token: 0x040003A7 RID: 935
		MinNullableDecimalSelector,
		// Token: 0x040003A8 RID: 936
		MinSingleSelector,
		// Token: 0x040003A9 RID: 937
		MinNullableSingleSelector,
		// Token: 0x040003AA RID: 938
		MaxInt,
		// Token: 0x040003AB RID: 939
		MaxNullableInt,
		// Token: 0x040003AC RID: 940
		MaxLong,
		// Token: 0x040003AD RID: 941
		MaxNullableLong,
		// Token: 0x040003AE RID: 942
		MaxDouble,
		// Token: 0x040003AF RID: 943
		MaxNullableDouble,
		// Token: 0x040003B0 RID: 944
		MaxDecimal,
		// Token: 0x040003B1 RID: 945
		MaxNullableDecimal,
		// Token: 0x040003B2 RID: 946
		MaxSingle,
		// Token: 0x040003B3 RID: 947
		MaxNullableSingle,
		// Token: 0x040003B4 RID: 948
		MaxIntSelector,
		// Token: 0x040003B5 RID: 949
		MaxNullableIntSelector,
		// Token: 0x040003B6 RID: 950
		MaxLongSelector,
		// Token: 0x040003B7 RID: 951
		MaxNullableLongSelector,
		// Token: 0x040003B8 RID: 952
		MaxDoubleSelector,
		// Token: 0x040003B9 RID: 953
		MaxNullableDoubleSelector,
		// Token: 0x040003BA RID: 954
		MaxDecimalSelector,
		// Token: 0x040003BB RID: 955
		MaxNullableDecimalSelector,
		// Token: 0x040003BC RID: 956
		MaxSingleSelector,
		// Token: 0x040003BD RID: 957
		MaxNullableSingleSelector,
		// Token: 0x040003BE RID: 958
		SumInt,
		// Token: 0x040003BF RID: 959
		SumNullableInt,
		// Token: 0x040003C0 RID: 960
		SumLong,
		// Token: 0x040003C1 RID: 961
		SumNullableLong,
		// Token: 0x040003C2 RID: 962
		SumDouble,
		// Token: 0x040003C3 RID: 963
		SumNullableDouble,
		// Token: 0x040003C4 RID: 964
		SumDecimal,
		// Token: 0x040003C5 RID: 965
		SumNullableDecimal,
		// Token: 0x040003C6 RID: 966
		SumSingle,
		// Token: 0x040003C7 RID: 967
		SumNullableSingle,
		// Token: 0x040003C8 RID: 968
		SumIntSelector,
		// Token: 0x040003C9 RID: 969
		SumNullableIntSelector,
		// Token: 0x040003CA RID: 970
		SumLongSelector,
		// Token: 0x040003CB RID: 971
		SumNullableLongSelector,
		// Token: 0x040003CC RID: 972
		SumDoubleSelector,
		// Token: 0x040003CD RID: 973
		SumNullableDoubleSelector,
		// Token: 0x040003CE RID: 974
		SumDecimalSelector,
		// Token: 0x040003CF RID: 975
		SumNullableDecimalSelector,
		// Token: 0x040003D0 RID: 976
		SumSingleSelector,
		// Token: 0x040003D1 RID: 977
		SumNullableSingleSelector,
		// Token: 0x040003D2 RID: 978
		AverageInt,
		// Token: 0x040003D3 RID: 979
		AverageNullableInt,
		// Token: 0x040003D4 RID: 980
		AverageLong,
		// Token: 0x040003D5 RID: 981
		AverageNullableLong,
		// Token: 0x040003D6 RID: 982
		AverageDouble,
		// Token: 0x040003D7 RID: 983
		AverageNullableDouble,
		// Token: 0x040003D8 RID: 984
		AverageDecimal,
		// Token: 0x040003D9 RID: 985
		AverageNullableDecimal,
		// Token: 0x040003DA RID: 986
		AverageSingle,
		// Token: 0x040003DB RID: 987
		AverageNullableSingle,
		// Token: 0x040003DC RID: 988
		AverageIntSelector,
		// Token: 0x040003DD RID: 989
		AverageNullableIntSelector,
		// Token: 0x040003DE RID: 990
		AverageLongSelector,
		// Token: 0x040003DF RID: 991
		AverageNullableLongSelector,
		// Token: 0x040003E0 RID: 992
		AverageDoubleSelector,
		// Token: 0x040003E1 RID: 993
		AverageNullableDoubleSelector,
		// Token: 0x040003E2 RID: 994
		AverageDecimalSelector,
		// Token: 0x040003E3 RID: 995
		AverageNullableDecimalSelector,
		// Token: 0x040003E4 RID: 996
		AverageSingleSelector,
		// Token: 0x040003E5 RID: 997
		AverageNullableSingleSelector,
		// Token: 0x040003E6 RID: 998
		Aggregate,
		// Token: 0x040003E7 RID: 999
		AggregateSeed,
		// Token: 0x040003E8 RID: 1000
		AggregateSeedSelector,
		// Token: 0x040003E9 RID: 1001
		AsQueryable,
		// Token: 0x040003EA RID: 1002
		AsQueryableGeneric,
		// Token: 0x040003EB RID: 1003
		AsEnumerable,
		// Token: 0x040003EC RID: 1004
		ToList,
		// Token: 0x040003ED RID: 1005
		NotSupported
	}
}
