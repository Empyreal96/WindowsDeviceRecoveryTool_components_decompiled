using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000126 RID: 294
	internal static class ReflectionUtil
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x060013AC RID: 5036 RVA: 0x00049C6C File Offset: 0x00047E6C
		// (set) Token: 0x060013AD RID: 5037 RVA: 0x00049C73 File Offset: 0x00047E73
		internal static MethodInfo DictionaryGetItemMethodInfo { get; set; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x060013AE RID: 5038 RVA: 0x00049C7B File Offset: 0x00047E7B
		// (set) Token: 0x060013AF RID: 5039 RVA: 0x00049C82 File Offset: 0x00047E82
		internal static MethodInfo TableQueryProviderReturnSingletonMethodInfo { get; set; }

		// Token: 0x17000312 RID: 786
		// (get) Token: 0x060013B0 RID: 5040 RVA: 0x00049C8A File Offset: 0x00047E8A
		// (set) Token: 0x060013B1 RID: 5041 RVA: 0x00049C91 File Offset: 0x00047E91
		internal static MethodInfo ProjectMethodInfo { get; set; }

		// Token: 0x060013B2 RID: 5042 RVA: 0x00049CAC File Offset: 0x00047EAC
		static ReflectionUtil()
		{
			Dictionary<string, SequenceMethod> dictionary = new Dictionary<string, SequenceMethod>(EqualityComparer<string>.Default);
			dictionary.Add("Sum(IQueryable`1<T0>, Expression`1<Func`2<T0, Double>>)->Double", SequenceMethod.SumDoubleSelector);
			dictionary.Add("Sum(IQueryable`1<T0>, Expression`1<Func`2<T0, Nullable`1<Double>>>)->Nullable`1<Double>", SequenceMethod.SumNullableDoubleSelector);
			dictionary.Add("Sum(IQueryable`1<T0>, Expression`1<Func`2<T0, Decimal>>)->Decimal", SequenceMethod.SumDecimalSelector);
			dictionary.Add("Sum(IQueryable`1<T0>, Expression`1<Func`2<T0, Nullable`1<Decimal>>>)->Nullable`1<Decimal>", SequenceMethod.SumNullableDecimalSelector);
			dictionary.Add("Average(IQueryable`1<T0>, Expression`1<Func`2<T0, Int32>>)->Double", SequenceMethod.AverageIntSelector);
			dictionary.Add("Average(IQueryable`1<T0>, Expression`1<Func`2<T0, Nullable`1<Int32>>>)->Nullable`1<Double>", SequenceMethod.AverageNullableIntSelector);
			dictionary.Add("Average(IQueryable`1<T0>, Expression`1<Func`2<T0, Single>>)->Single", SequenceMethod.AverageSingleSelector);
			dictionary.Add("Average(IQueryable`1<T0>, Expression`1<Func`2<T0, Nullable`1<Single>>>)->Nullable`1<Single>", SequenceMethod.AverageNullableSingleSelector);
			dictionary.Add("Average(IQueryable`1<T0>, Expression`1<Func`2<T0, Int64>>)->Double", SequenceMethod.AverageLongSelector);
			dictionary.Add("Average(IQueryable`1<T0>, Expression`1<Func`2<T0, Nullable`1<Int64>>>)->Nullable`1<Double>", SequenceMethod.AverageNullableLongSelector);
			dictionary.Add("Average(IQueryable`1<T0>, Expression`1<Func`2<T0, Double>>)->Double", SequenceMethod.AverageDoubleSelector);
			dictionary.Add("Average(IQueryable`1<T0>, Expression`1<Func`2<T0, Nullable`1<Double>>>)->Nullable`1<Double>", SequenceMethod.AverageNullableDoubleSelector);
			dictionary.Add("Average(IQueryable`1<T0>, Expression`1<Func`2<T0, Decimal>>)->Decimal", SequenceMethod.AverageDecimalSelector);
			dictionary.Add("Average(IQueryable`1<T0>, Expression`1<Func`2<T0, Nullable`1<Decimal>>>)->Nullable`1<Decimal>", SequenceMethod.AverageNullableDecimalSelector);
			dictionary.Add("Aggregate(IQueryable`1<T0>, Expression`1<Func`3<T0, T0, T0>>)->T0", SequenceMethod.Aggregate);
			dictionary.Add("Aggregate(IQueryable`1<T0>, T1, Expression`1<Func`3<T1, T0, T1>>)->T1", SequenceMethod.AggregateSeed);
			dictionary.Add("Aggregate(IQueryable`1<T0>, T1, Expression`1<Func`3<T1, T0, T1>>, Expression`1<Func`2<T1, T2>>)->T2", SequenceMethod.AggregateSeedSelector);
			dictionary.Add("AsQueryable(IEnumerable`1<T0>)->IQueryable`1<T0>", SequenceMethod.AsQueryableGeneric);
			dictionary.Add("Where(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->IQueryable`1<T0>", SequenceMethod.Where);
			dictionary.Add("Where(IQueryable`1<T0>, Expression`1<Func`3<T0, Int32, Boolean>>)->IQueryable`1<T0>", SequenceMethod.WhereOrdinal);
			dictionary.Add("OfType(IQueryable)->IQueryable`1<T0>", SequenceMethod.OfType);
			dictionary.Add("Cast(IQueryable)->IQueryable`1<T0>", SequenceMethod.Cast);
			dictionary.Add("Select(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>)->IQueryable`1<T1>", SequenceMethod.Select);
			dictionary.Add("Select(IQueryable`1<T0>, Expression`1<Func`3<T0, Int32, T1>>)->IQueryable`1<T1>", SequenceMethod.SelectOrdinal);
			dictionary.Add("SelectMany(IQueryable`1<T0>, Expression`1<Func`2<T0, IEnumerable`1<T1>>>)->IQueryable`1<T1>", SequenceMethod.SelectMany);
			dictionary.Add("SelectMany(IQueryable`1<T0>, Expression`1<Func`3<T0, Int32, IEnumerable`1<T1>>>)->IQueryable`1<T1>", SequenceMethod.SelectManyOrdinal);
			dictionary.Add("SelectMany(IQueryable`1<T0>, Expression`1<Func`3<T0, Int32, IEnumerable`1<T1>>>, Expression`1<Func`3<T0, T1, T2>>)->IQueryable`1<T2>", SequenceMethod.SelectManyOrdinalResultSelector);
			dictionary.Add("SelectMany(IQueryable`1<T0>, Expression`1<Func`2<T0, IEnumerable`1<T1>>>, Expression`1<Func`3<T0, T1, T2>>)->IQueryable`1<T2>", SequenceMethod.SelectManyResultSelector);
			dictionary.Add("Join(IQueryable`1<T0>, IEnumerable`1<T1>, Expression`1<Func`2<T0, T2>>, Expression`1<Func`2<T1, T2>>, Expression`1<Func`3<T0, T1, T3>>)->IQueryable`1<T3>", SequenceMethod.Join);
			dictionary.Add("Join(IQueryable`1<T0>, IEnumerable`1<T1>, Expression`1<Func`2<T0, T2>>, Expression`1<Func`2<T1, T2>>, Expression`1<Func`3<T0, T1, T3>>, IEqualityComparer`1<T2>)->IQueryable`1<T3>", SequenceMethod.JoinComparer);
			dictionary.Add("GroupJoin(IQueryable`1<T0>, IEnumerable`1<T1>, Expression`1<Func`2<T0, T2>>, Expression`1<Func`2<T1, T2>>, Expression`1<Func`3<T0, IEnumerable`1<T1>, T3>>)->IQueryable`1<T3>", SequenceMethod.GroupJoin);
			dictionary.Add("GroupJoin(IQueryable`1<T0>, IEnumerable`1<T1>, Expression`1<Func`2<T0, T2>>, Expression`1<Func`2<T1, T2>>, Expression`1<Func`3<T0, IEnumerable`1<T1>, T3>>, IEqualityComparer`1<T2>)->IQueryable`1<T3>", SequenceMethod.GroupJoinComparer);
			dictionary.Add("OrderBy(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>)->IOrderedQueryable`1<T0>", SequenceMethod.OrderBy);
			dictionary.Add("OrderBy(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, IComparer`1<T1>)->IOrderedQueryable`1<T0>", SequenceMethod.OrderByComparer);
			dictionary.Add("OrderByDescending(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>)->IOrderedQueryable`1<T0>", SequenceMethod.OrderByDescending);
			dictionary.Add("OrderByDescending(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, IComparer`1<T1>)->IOrderedQueryable`1<T0>", SequenceMethod.OrderByDescendingComparer);
			dictionary.Add("ThenBy(IOrderedQueryable`1<T0>, Expression`1<Func`2<T0, T1>>)->IOrderedQueryable`1<T0>", SequenceMethod.ThenBy);
			dictionary.Add("ThenBy(IOrderedQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, IComparer`1<T1>)->IOrderedQueryable`1<T0>", SequenceMethod.ThenByComparer);
			dictionary.Add("ThenByDescending(IOrderedQueryable`1<T0>, Expression`1<Func`2<T0, T1>>)->IOrderedQueryable`1<T0>", SequenceMethod.ThenByDescending);
			dictionary.Add("ThenByDescending(IOrderedQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, IComparer`1<T1>)->IOrderedQueryable`1<T0>", SequenceMethod.ThenByDescendingComparer);
			dictionary.Add("Take(IQueryable`1<T0>, Int32)->IQueryable`1<T0>", SequenceMethod.Take);
			dictionary.Add("TakeWhile(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->IQueryable`1<T0>", SequenceMethod.TakeWhile);
			dictionary.Add("TakeWhile(IQueryable`1<T0>, Expression`1<Func`3<T0, Int32, Boolean>>)->IQueryable`1<T0>", SequenceMethod.TakeWhileOrdinal);
			dictionary.Add("Skip(IQueryable`1<T0>, Int32)->IQueryable`1<T0>", SequenceMethod.Skip);
			dictionary.Add("SkipWhile(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->IQueryable`1<T0>", SequenceMethod.SkipWhile);
			dictionary.Add("SkipWhile(IQueryable`1<T0>, Expression`1<Func`3<T0, Int32, Boolean>>)->IQueryable`1<T0>", SequenceMethod.SkipWhileOrdinal);
			dictionary.Add("GroupBy(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>)->IQueryable`1<IGrouping`2<T1, T0>>", SequenceMethod.GroupBy);
			dictionary.Add("GroupBy(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, Expression`1<Func`2<T0, T2>>)->IQueryable`1<IGrouping`2<T1, T2>>", SequenceMethod.GroupByElementSelector);
			dictionary.Add("GroupBy(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, IEqualityComparer`1<T1>)->IQueryable`1<IGrouping`2<T1, T0>>", SequenceMethod.GroupByComparer);
			dictionary.Add("GroupBy(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, Expression`1<Func`2<T0, T2>>, IEqualityComparer`1<T1>)->IQueryable`1<IGrouping`2<T1, T2>>", SequenceMethod.GroupByElementSelectorComparer);
			dictionary.Add("GroupBy(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, Expression`1<Func`2<T0, T2>>, Expression`1<Func`3<T1, IEnumerable`1<T2>, T3>>)->IQueryable`1<T3>", SequenceMethod.GroupByElementSelectorResultSelector);
			dictionary.Add("GroupBy(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, Expression`1<Func`3<T1, IEnumerable`1<T0>, T2>>)->IQueryable`1<T2>", SequenceMethod.GroupByResultSelector);
			dictionary.Add("GroupBy(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, Expression`1<Func`3<T1, IEnumerable`1<T0>, T2>>, IEqualityComparer`1<T1>)->IQueryable`1<T2>", SequenceMethod.GroupByResultSelectorComparer);
			dictionary.Add("GroupBy(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>, Expression`1<Func`2<T0, T2>>, Expression`1<Func`3<T1, IEnumerable`1<T2>, T3>>, IEqualityComparer`1<T1>)->IQueryable`1<T3>", SequenceMethod.GroupByElementSelectorResultSelectorComparer);
			dictionary.Add("Distinct(IQueryable`1<T0>)->IQueryable`1<T0>", SequenceMethod.Distinct);
			dictionary.Add("Distinct(IQueryable`1<T0>, IEqualityComparer`1<T0>)->IQueryable`1<T0>", SequenceMethod.DistinctComparer);
			dictionary.Add("Concat(IQueryable`1<T0>, IEnumerable`1<T0>)->IQueryable`1<T0>", SequenceMethod.Concat);
			dictionary.Add("Union(IQueryable`1<T0>, IEnumerable`1<T0>)->IQueryable`1<T0>", SequenceMethod.Union);
			dictionary.Add("Union(IQueryable`1<T0>, IEnumerable`1<T0>, IEqualityComparer`1<T0>)->IQueryable`1<T0>", SequenceMethod.UnionComparer);
			dictionary.Add("Intersect(IQueryable`1<T0>, IEnumerable`1<T0>)->IQueryable`1<T0>", SequenceMethod.Intersect);
			dictionary.Add("Intersect(IQueryable`1<T0>, IEnumerable`1<T0>, IEqualityComparer`1<T0>)->IQueryable`1<T0>", SequenceMethod.IntersectComparer);
			dictionary.Add("Except(IQueryable`1<T0>, IEnumerable`1<T0>)->IQueryable`1<T0>", SequenceMethod.Except);
			dictionary.Add("Except(IQueryable`1<T0>, IEnumerable`1<T0>, IEqualityComparer`1<T0>)->IQueryable`1<T0>", SequenceMethod.ExceptComparer);
			dictionary.Add("First(IQueryable`1<T0>)->T0", SequenceMethod.First);
			dictionary.Add("First(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->T0", SequenceMethod.FirstPredicate);
			dictionary.Add("FirstOrDefault(IQueryable`1<T0>)->T0", SequenceMethod.FirstOrDefault);
			dictionary.Add("FirstOrDefault(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->T0", SequenceMethod.FirstOrDefaultPredicate);
			dictionary.Add("Last(IQueryable`1<T0>)->T0", SequenceMethod.Last);
			dictionary.Add("Last(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->T0", SequenceMethod.LastPredicate);
			dictionary.Add("LastOrDefault(IQueryable`1<T0>)->T0", SequenceMethod.LastOrDefault);
			dictionary.Add("LastOrDefault(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->T0", SequenceMethod.LastOrDefaultPredicate);
			dictionary.Add("Single(IQueryable`1<T0>)->T0", SequenceMethod.Single);
			dictionary.Add("Single(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->T0", SequenceMethod.SinglePredicate);
			dictionary.Add("SingleOrDefault(IQueryable`1<T0>)->T0", SequenceMethod.SingleOrDefault);
			dictionary.Add("SingleOrDefault(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->T0", SequenceMethod.SingleOrDefaultPredicate);
			dictionary.Add("ElementAt(IQueryable`1<T0>, Int32)->T0", SequenceMethod.ElementAt);
			dictionary.Add("ElementAtOrDefault(IQueryable`1<T0>, Int32)->T0", SequenceMethod.ElementAtOrDefault);
			dictionary.Add("DefaultIfEmpty(IQueryable`1<T0>)->IQueryable`1<T0>", SequenceMethod.DefaultIfEmpty);
			dictionary.Add("DefaultIfEmpty(IQueryable`1<T0>, T0)->IQueryable`1<T0>", SequenceMethod.DefaultIfEmptyValue);
			dictionary.Add("Contains(IQueryable`1<T0>, T0)->Boolean", SequenceMethod.Contains);
			dictionary.Add("Contains(IQueryable`1<T0>, T0, IEqualityComparer`1<T0>)->Boolean", SequenceMethod.ContainsComparer);
			dictionary.Add("Reverse(IQueryable`1<T0>)->IQueryable`1<T0>", SequenceMethod.Reverse);
			dictionary.Add("SequenceEqual(IQueryable`1<T0>, IEnumerable`1<T0>)->Boolean", SequenceMethod.SequenceEqual);
			dictionary.Add("SequenceEqual(IQueryable`1<T0>, IEnumerable`1<T0>, IEqualityComparer`1<T0>)->Boolean", SequenceMethod.SequenceEqualComparer);
			dictionary.Add("Any(IQueryable`1<T0>)->Boolean", SequenceMethod.Any);
			dictionary.Add("Any(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->Boolean", SequenceMethod.AnyPredicate);
			dictionary.Add("All(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->Boolean", SequenceMethod.All);
			dictionary.Add("Count(IQueryable`1<T0>)->Int32", SequenceMethod.Count);
			dictionary.Add("Count(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->Int32", SequenceMethod.CountPredicate);
			dictionary.Add("LongCount(IQueryable`1<T0>)->Int64", SequenceMethod.LongCount);
			dictionary.Add("LongCount(IQueryable`1<T0>, Expression`1<Func`2<T0, Boolean>>)->Int64", SequenceMethod.LongCountPredicate);
			dictionary.Add("Min(IQueryable`1<T0>)->T0", SequenceMethod.Min);
			dictionary.Add("Min(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>)->T1", SequenceMethod.MinSelector);
			dictionary.Add("Max(IQueryable`1<T0>)->T0", SequenceMethod.Max);
			dictionary.Add("Max(IQueryable`1<T0>, Expression`1<Func`2<T0, T1>>)->T1", SequenceMethod.MaxSelector);
			dictionary.Add("Sum(IQueryable`1<T0>, Expression`1<Func`2<T0, Int32>>)->Int32", SequenceMethod.SumIntSelector);
			dictionary.Add("Sum(IQueryable`1<T0>, Expression`1<Func`2<T0, Nullable`1<Int32>>>)->Nullable`1<Int32>", SequenceMethod.SumNullableIntSelector);
			dictionary.Add("Sum(IQueryable`1<T0>, Expression`1<Func`2<T0, Int64>>)->Int64", SequenceMethod.SumLongSelector);
			dictionary.Add("Sum(IQueryable`1<T0>, Expression`1<Func`2<T0, Nullable`1<Int64>>>)->Nullable`1<Int64>", SequenceMethod.SumNullableLongSelector);
			dictionary.Add("Sum(IQueryable`1<T0>, Expression`1<Func`2<T0, Single>>)->Single", SequenceMethod.SumSingleSelector);
			dictionary.Add("Sum(IQueryable`1<T0>, Expression`1<Func`2<T0, Nullable`1<Single>>>)->Nullable`1<Single>", SequenceMethod.SumNullableSingleSelector);
			dictionary.Add("AsQueryable(IEnumerable)->IQueryable", SequenceMethod.AsQueryable);
			dictionary.Add("Sum(IQueryable`1<Int32>)->Int32", SequenceMethod.SumInt);
			dictionary.Add("Sum(IQueryable`1<Nullable`1<Int32>>)->Nullable`1<Int32>", SequenceMethod.SumNullableInt);
			dictionary.Add("Sum(IQueryable`1<Int64>)->Int64", SequenceMethod.SumLong);
			dictionary.Add("Sum(IQueryable`1<Nullable`1<Int64>>)->Nullable`1<Int64>", SequenceMethod.SumNullableLong);
			dictionary.Add("Sum(IQueryable`1<Single>)->Single", SequenceMethod.SumSingle);
			dictionary.Add("Sum(IQueryable`1<Nullable`1<Single>>)->Nullable`1<Single>", SequenceMethod.SumNullableSingle);
			dictionary.Add("Sum(IQueryable`1<Double>)->Double", SequenceMethod.SumDouble);
			dictionary.Add("Sum(IQueryable`1<Nullable`1<Double>>)->Nullable`1<Double>", SequenceMethod.SumNullableDouble);
			dictionary.Add("Sum(IQueryable`1<Decimal>)->Decimal", SequenceMethod.SumDecimal);
			dictionary.Add("Sum(IQueryable`1<Nullable`1<Decimal>>)->Nullable`1<Decimal>", SequenceMethod.SumNullableDecimal);
			dictionary.Add("Average(IQueryable`1<Int32>)->Double", SequenceMethod.AverageInt);
			dictionary.Add("Average(IQueryable`1<Nullable`1<Int32>>)->Nullable`1<Double>", SequenceMethod.AverageNullableInt);
			dictionary.Add("Average(IQueryable`1<Int64>)->Double", SequenceMethod.AverageLong);
			dictionary.Add("Average(IQueryable`1<Nullable`1<Int64>>)->Nullable`1<Double>", SequenceMethod.AverageNullableLong);
			dictionary.Add("Average(IQueryable`1<Single>)->Single", SequenceMethod.AverageSingle);
			dictionary.Add("Average(IQueryable`1<Nullable`1<Single>>)->Nullable`1<Single>", SequenceMethod.AverageNullableSingle);
			dictionary.Add("Average(IQueryable`1<Double>)->Double", SequenceMethod.AverageDouble);
			dictionary.Add("Average(IQueryable`1<Nullable`1<Double>>)->Nullable`1<Double>", SequenceMethod.AverageNullableDouble);
			dictionary.Add("Average(IQueryable`1<Decimal>)->Decimal", SequenceMethod.AverageDecimal);
			dictionary.Add("Average(IQueryable`1<Nullable`1<Decimal>>)->Nullable`1<Decimal>", SequenceMethod.AverageNullableDecimal);
			dictionary.Add("First(IEnumerable`1<T0>)->T0", SequenceMethod.First);
			dictionary.Add("First(IEnumerable`1<T0>, Func`2<T0, Boolean>)->T0", SequenceMethod.FirstPredicate);
			dictionary.Add("FirstOrDefault(IEnumerable`1<T0>)->T0", SequenceMethod.FirstOrDefault);
			dictionary.Add("FirstOrDefault(IEnumerable`1<T0>, Func`2<T0, Boolean>)->T0", SequenceMethod.FirstOrDefaultPredicate);
			dictionary.Add("Last(IEnumerable`1<T0>)->T0", SequenceMethod.Last);
			dictionary.Add("Last(IEnumerable`1<T0>, Func`2<T0, Boolean>)->T0", SequenceMethod.LastPredicate);
			dictionary.Add("LastOrDefault(IEnumerable`1<T0>)->T0", SequenceMethod.LastOrDefault);
			dictionary.Add("LastOrDefault(IEnumerable`1<T0>, Func`2<T0, Boolean>)->T0", SequenceMethod.LastOrDefaultPredicate);
			dictionary.Add("Single(IEnumerable`1<T0>)->T0", SequenceMethod.Single);
			dictionary.Add("Single(IEnumerable`1<T0>, Func`2<T0, Boolean>)->T0", SequenceMethod.SinglePredicate);
			dictionary.Add("SingleOrDefault(IEnumerable`1<T0>)->T0", SequenceMethod.SingleOrDefault);
			dictionary.Add("SingleOrDefault(IEnumerable`1<T0>, Func`2<T0, Boolean>)->T0", SequenceMethod.SingleOrDefaultPredicate);
			dictionary.Add("ElementAt(IEnumerable`1<T0>, Int32)->T0", SequenceMethod.ElementAt);
			dictionary.Add("ElementAtOrDefault(IEnumerable`1<T0>, Int32)->T0", SequenceMethod.ElementAtOrDefault);
			dictionary.Add("Repeat(T0, Int32)->IEnumerable`1<T0>", SequenceMethod.NotSupported);
			dictionary.Add("Empty()->IEnumerable`1<T0>", SequenceMethod.Empty);
			dictionary.Add("Any(IEnumerable`1<T0>)->Boolean", SequenceMethod.Any);
			dictionary.Add("Any(IEnumerable`1<T0>, Func`2<T0, Boolean>)->Boolean", SequenceMethod.AnyPredicate);
			dictionary.Add("All(IEnumerable`1<T0>, Func`2<T0, Boolean>)->Boolean", SequenceMethod.All);
			dictionary.Add("Count(IEnumerable`1<T0>)->Int32", SequenceMethod.Count);
			dictionary.Add("Count(IEnumerable`1<T0>, Func`2<T0, Boolean>)->Int32", SequenceMethod.CountPredicate);
			dictionary.Add("LongCount(IEnumerable`1<T0>)->Int64", SequenceMethod.LongCount);
			dictionary.Add("LongCount(IEnumerable`1<T0>, Func`2<T0, Boolean>)->Int64", SequenceMethod.LongCountPredicate);
			dictionary.Add("Contains(IEnumerable`1<T0>, T0)->Boolean", SequenceMethod.Contains);
			dictionary.Add("Contains(IEnumerable`1<T0>, T0, IEqualityComparer`1<T0>)->Boolean", SequenceMethod.ContainsComparer);
			dictionary.Add("Aggregate(IEnumerable`1<T0>, Func`3<T0, T0, T0>)->T0", SequenceMethod.Aggregate);
			dictionary.Add("Aggregate(IEnumerable`1<T0>, T1, Func`3<T1, T0, T1>)->T1", SequenceMethod.AggregateSeed);
			dictionary.Add("Aggregate(IEnumerable`1<T0>, T1, Func`3<T1, T0, T1>, Func`2<T1, T2>)->T2", SequenceMethod.AggregateSeedSelector);
			dictionary.Add("Sum(IEnumerable`1<T0>, Func`2<T0, Int32>)->Int32", SequenceMethod.SumIntSelector);
			dictionary.Add("Sum(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Int32>>)->Nullable`1<Int32>", SequenceMethod.SumNullableIntSelector);
			dictionary.Add("Sum(IEnumerable`1<T0>, Func`2<T0, Int64>)->Int64", SequenceMethod.SumLongSelector);
			dictionary.Add("Sum(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Int64>>)->Nullable`1<Int64>", SequenceMethod.SumNullableLongSelector);
			dictionary.Add("Sum(IEnumerable`1<T0>, Func`2<T0, Single>)->Single", SequenceMethod.SumSingleSelector);
			dictionary.Add("Sum(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Single>>)->Nullable`1<Single>", SequenceMethod.SumNullableSingleSelector);
			dictionary.Add("Sum(IEnumerable`1<T0>, Func`2<T0, Double>)->Double", SequenceMethod.SumDoubleSelector);
			dictionary.Add("Sum(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Double>>)->Nullable`1<Double>", SequenceMethod.SumNullableDoubleSelector);
			dictionary.Add("Sum(IEnumerable`1<T0>, Func`2<T0, Decimal>)->Decimal", SequenceMethod.SumDecimalSelector);
			dictionary.Add("Sum(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Decimal>>)->Nullable`1<Decimal>", SequenceMethod.SumNullableDecimalSelector);
			dictionary.Add("Min(IEnumerable`1<T0>)->T0", SequenceMethod.Min);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, Int32>)->Int32", SequenceMethod.MinIntSelector);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Int32>>)->Nullable`1<Int32>", SequenceMethod.MinNullableIntSelector);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, Int64>)->Int64", SequenceMethod.MinLongSelector);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Int64>>)->Nullable`1<Int64>", SequenceMethod.MinNullableLongSelector);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, Single>)->Single", SequenceMethod.MinSingleSelector);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Single>>)->Nullable`1<Single>", SequenceMethod.MinNullableSingleSelector);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, Double>)->Double", SequenceMethod.MinDoubleSelector);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Double>>)->Nullable`1<Double>", SequenceMethod.MinNullableDoubleSelector);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, Decimal>)->Decimal", SequenceMethod.MinDecimalSelector);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Decimal>>)->Nullable`1<Decimal>", SequenceMethod.MinNullableDecimalSelector);
			dictionary.Add("Min(IEnumerable`1<T0>, Func`2<T0, T1>)->T1", SequenceMethod.MinSelector);
			dictionary.Add("Max(IEnumerable`1<T0>)->T0", SequenceMethod.Max);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, Int32>)->Int32", SequenceMethod.MaxIntSelector);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Int32>>)->Nullable`1<Int32>", SequenceMethod.MaxNullableIntSelector);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, Int64>)->Int64", SequenceMethod.MaxLongSelector);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Int64>>)->Nullable`1<Int64>", SequenceMethod.MaxNullableLongSelector);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, Single>)->Single", SequenceMethod.MaxSingleSelector);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Single>>)->Nullable`1<Single>", SequenceMethod.MaxNullableSingleSelector);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, Double>)->Double", SequenceMethod.MaxDoubleSelector);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Double>>)->Nullable`1<Double>", SequenceMethod.MaxNullableDoubleSelector);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, Decimal>)->Decimal", SequenceMethod.MaxDecimalSelector);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Decimal>>)->Nullable`1<Decimal>", SequenceMethod.MaxNullableDecimalSelector);
			dictionary.Add("Max(IEnumerable`1<T0>, Func`2<T0, T1>)->T1", SequenceMethod.MaxSelector);
			dictionary.Add("Average(IEnumerable`1<T0>, Func`2<T0, Int32>)->Double", SequenceMethod.AverageIntSelector);
			dictionary.Add("Average(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Int32>>)->Nullable`1<Double>", SequenceMethod.AverageNullableIntSelector);
			dictionary.Add("Average(IEnumerable`1<T0>, Func`2<T0, Int64>)->Double", SequenceMethod.AverageLongSelector);
			dictionary.Add("Average(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Int64>>)->Nullable`1<Double>", SequenceMethod.AverageNullableLongSelector);
			dictionary.Add("Average(IEnumerable`1<T0>, Func`2<T0, Single>)->Single", SequenceMethod.AverageSingleSelector);
			dictionary.Add("Average(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Single>>)->Nullable`1<Single>", SequenceMethod.AverageNullableSingleSelector);
			dictionary.Add("Average(IEnumerable`1<T0>, Func`2<T0, Double>)->Double", SequenceMethod.AverageDoubleSelector);
			dictionary.Add("Average(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Double>>)->Nullable`1<Double>", SequenceMethod.AverageNullableDoubleSelector);
			dictionary.Add("Average(IEnumerable`1<T0>, Func`2<T0, Decimal>)->Decimal", SequenceMethod.AverageDecimalSelector);
			dictionary.Add("Average(IEnumerable`1<T0>, Func`2<T0, Nullable`1<Decimal>>)->Nullable`1<Decimal>", SequenceMethod.AverageNullableDecimalSelector);
			dictionary.Add("Where(IEnumerable`1<T0>, Func`2<T0, Boolean>)->IEnumerable`1<T0>", SequenceMethod.Where);
			dictionary.Add("Where(IEnumerable`1<T0>, Func`3<T0, Int32, Boolean>)->IEnumerable`1<T0>", SequenceMethod.WhereOrdinal);
			dictionary.Add("Select(IEnumerable`1<T0>, Func`2<T0, T1>)->IEnumerable`1<T1>", SequenceMethod.Select);
			dictionary.Add("Select(IEnumerable`1<T0>, Func`3<T0, Int32, T1>)->IEnumerable`1<T1>", SequenceMethod.SelectOrdinal);
			dictionary.Add("SelectMany(IEnumerable`1<T0>, Func`2<T0, IEnumerable`1<T1>>)->IEnumerable`1<T1>", SequenceMethod.SelectMany);
			dictionary.Add("SelectMany(IEnumerable`1<T0>, Func`3<T0, Int32, IEnumerable`1<T1>>)->IEnumerable`1<T1>", SequenceMethod.SelectManyOrdinal);
			dictionary.Add("SelectMany(IEnumerable`1<T0>, Func`3<T0, Int32, IEnumerable`1<T1>>, Func`3<T0, T1, T2>)->IEnumerable`1<T2>", SequenceMethod.SelectManyOrdinalResultSelector);
			dictionary.Add("SelectMany(IEnumerable`1<T0>, Func`2<T0, IEnumerable`1<T1>>, Func`3<T0, T1, T2>)->IEnumerable`1<T2>", SequenceMethod.SelectManyResultSelector);
			dictionary.Add("Take(IEnumerable`1<T0>, Int32)->IEnumerable`1<T0>", SequenceMethod.Take);
			dictionary.Add("TakeWhile(IEnumerable`1<T0>, Func`2<T0, Boolean>)->IEnumerable`1<T0>", SequenceMethod.TakeWhile);
			dictionary.Add("TakeWhile(IEnumerable`1<T0>, Func`3<T0, Int32, Boolean>)->IEnumerable`1<T0>", SequenceMethod.TakeWhileOrdinal);
			dictionary.Add("Skip(IEnumerable`1<T0>, Int32)->IEnumerable`1<T0>", SequenceMethod.Skip);
			dictionary.Add("SkipWhile(IEnumerable`1<T0>, Func`2<T0, Boolean>)->IEnumerable`1<T0>", SequenceMethod.SkipWhile);
			dictionary.Add("SkipWhile(IEnumerable`1<T0>, Func`3<T0, Int32, Boolean>)->IEnumerable`1<T0>", SequenceMethod.SkipWhileOrdinal);
			dictionary.Add("Join(IEnumerable`1<T0>, IEnumerable`1<T1>, Func`2<T0, T2>, Func`2<T1, T2>, Func`3<T0, T1, T3>)->IEnumerable`1<T3>", SequenceMethod.Join);
			dictionary.Add("Join(IEnumerable`1<T0>, IEnumerable`1<T1>, Func`2<T0, T2>, Func`2<T1, T2>, Func`3<T0, T1, T3>, IEqualityComparer`1<T2>)->IEnumerable`1<T3>", SequenceMethod.JoinComparer);
			dictionary.Add("GroupJoin(IEnumerable`1<T0>, IEnumerable`1<T1>, Func`2<T0, T2>, Func`2<T1, T2>, Func`3<T0, IEnumerable`1<T1>, T3>)->IEnumerable`1<T3>", SequenceMethod.GroupJoin);
			dictionary.Add("GroupJoin(IEnumerable`1<T0>, IEnumerable`1<T1>, Func`2<T0, T2>, Func`2<T1, T2>, Func`3<T0, IEnumerable`1<T1>, T3>, IEqualityComparer`1<T2>)->IEnumerable`1<T3>", SequenceMethod.GroupJoinComparer);
			dictionary.Add("OrderBy(IEnumerable`1<T0>, Func`2<T0, T1>)->IOrderedEnumerable`1<T0>", SequenceMethod.OrderBy);
			dictionary.Add("OrderBy(IEnumerable`1<T0>, Func`2<T0, T1>, IComparer`1<T1>)->IOrderedEnumerable`1<T0>", SequenceMethod.OrderByComparer);
			dictionary.Add("OrderByDescending(IEnumerable`1<T0>, Func`2<T0, T1>)->IOrderedEnumerable`1<T0>", SequenceMethod.OrderByDescending);
			dictionary.Add("OrderByDescending(IEnumerable`1<T0>, Func`2<T0, T1>, IComparer`1<T1>)->IOrderedEnumerable`1<T0>", SequenceMethod.OrderByDescendingComparer);
			dictionary.Add("ThenBy(IOrderedEnumerable`1<T0>, Func`2<T0, T1>)->IOrderedEnumerable`1<T0>", SequenceMethod.ThenBy);
			dictionary.Add("ThenBy(IOrderedEnumerable`1<T0>, Func`2<T0, T1>, IComparer`1<T1>)->IOrderedEnumerable`1<T0>", SequenceMethod.ThenByComparer);
			dictionary.Add("ThenByDescending(IOrderedEnumerable`1<T0>, Func`2<T0, T1>)->IOrderedEnumerable`1<T0>", SequenceMethod.ThenByDescending);
			dictionary.Add("ThenByDescending(IOrderedEnumerable`1<T0>, Func`2<T0, T1>, IComparer`1<T1>)->IOrderedEnumerable`1<T0>", SequenceMethod.ThenByDescendingComparer);
			dictionary.Add("GroupBy(IEnumerable`1<T0>, Func`2<T0, T1>)->IEnumerable`1<IGrouping`2<T1, T0>>", SequenceMethod.GroupBy);
			dictionary.Add("GroupBy(IEnumerable`1<T0>, Func`2<T0, T1>, IEqualityComparer`1<T1>)->IEnumerable`1<IGrouping`2<T1, T0>>", SequenceMethod.GroupByComparer);
			dictionary.Add("GroupBy(IEnumerable`1<T0>, Func`2<T0, T1>, Func`2<T0, T2>)->IEnumerable`1<IGrouping`2<T1, T2>>", SequenceMethod.GroupByElementSelector);
			dictionary.Add("GroupBy(IEnumerable`1<T0>, Func`2<T0, T1>, Func`2<T0, T2>, IEqualityComparer`1<T1>)->IEnumerable`1<IGrouping`2<T1, T2>>", SequenceMethod.GroupByElementSelectorComparer);
			dictionary.Add("GroupBy(IEnumerable`1<T0>, Func`2<T0, T1>, Func`3<T1, IEnumerable`1<T0>, T2>)->IEnumerable`1<T2>", SequenceMethod.GroupByResultSelector);
			dictionary.Add("GroupBy(IEnumerable`1<T0>, Func`2<T0, T1>, Func`2<T0, T2>, Func`3<T1, IEnumerable`1<T2>, T3>)->IEnumerable`1<T3>", SequenceMethod.GroupByElementSelectorResultSelector);
			dictionary.Add("GroupBy(IEnumerable`1<T0>, Func`2<T0, T1>, Func`3<T1, IEnumerable`1<T0>, T2>, IEqualityComparer`1<T1>)->IEnumerable`1<T2>", SequenceMethod.GroupByResultSelectorComparer);
			dictionary.Add("GroupBy(IEnumerable`1<T0>, Func`2<T0, T1>, Func`2<T0, T2>, Func`3<T1, IEnumerable`1<T2>, T3>, IEqualityComparer`1<T1>)->IEnumerable`1<T3>", SequenceMethod.GroupByElementSelectorResultSelectorComparer);
			dictionary.Add("Concat(IEnumerable`1<T0>, IEnumerable`1<T0>)->IEnumerable`1<T0>", SequenceMethod.Concat);
			dictionary.Add("Distinct(IEnumerable`1<T0>)->IEnumerable`1<T0>", SequenceMethod.Distinct);
			dictionary.Add("Distinct(IEnumerable`1<T0>, IEqualityComparer`1<T0>)->IEnumerable`1<T0>", SequenceMethod.DistinctComparer);
			dictionary.Add("Union(IEnumerable`1<T0>, IEnumerable`1<T0>)->IEnumerable`1<T0>", SequenceMethod.Union);
			dictionary.Add("Union(IEnumerable`1<T0>, IEnumerable`1<T0>, IEqualityComparer`1<T0>)->IEnumerable`1<T0>", SequenceMethod.UnionComparer);
			dictionary.Add("Intersect(IEnumerable`1<T0>, IEnumerable`1<T0>)->IEnumerable`1<T0>", SequenceMethod.Intersect);
			dictionary.Add("Intersect(IEnumerable`1<T0>, IEnumerable`1<T0>, IEqualityComparer`1<T0>)->IEnumerable`1<T0>", SequenceMethod.IntersectComparer);
			dictionary.Add("Except(IEnumerable`1<T0>, IEnumerable`1<T0>)->IEnumerable`1<T0>", SequenceMethod.Except);
			dictionary.Add("Except(IEnumerable`1<T0>, IEnumerable`1<T0>, IEqualityComparer`1<T0>)->IEnumerable`1<T0>", SequenceMethod.ExceptComparer);
			dictionary.Add("Reverse(IEnumerable`1<T0>)->IEnumerable`1<T0>", SequenceMethod.Reverse);
			dictionary.Add("SequenceEqual(IEnumerable`1<T0>, IEnumerable`1<T0>)->Boolean", SequenceMethod.SequenceEqual);
			dictionary.Add("SequenceEqual(IEnumerable`1<T0>, IEnumerable`1<T0>, IEqualityComparer`1<T0>)->Boolean", SequenceMethod.SequenceEqualComparer);
			dictionary.Add("AsEnumerable(IEnumerable`1<T0>)->IEnumerable`1<T0>", SequenceMethod.AsEnumerable);
			dictionary.Add("ToArray(IEnumerable`1<T0>)->TSource[]", SequenceMethod.NotSupported);
			dictionary.Add("ToList(IEnumerable`1<T0>)->List`1<T0>", SequenceMethod.ToList);
			dictionary.Add("ToDictionary(IEnumerable`1<T0>, Func`2<T0, T1>)->Dictionary`2<T1, T0>", SequenceMethod.NotSupported);
			dictionary.Add("ToDictionary(IEnumerable`1<T0>, Func`2<T0, T1>, IEqualityComparer`1<T1>)->Dictionary`2<T1, T0>", SequenceMethod.NotSupported);
			dictionary.Add("ToDictionary(IEnumerable`1<T0>, Func`2<T0, T1>, Func`2<T0, T2>)->Dictionary`2<T1, T2>", SequenceMethod.NotSupported);
			dictionary.Add("ToDictionary(IEnumerable`1<T0>, Func`2<T0, T1>, Func`2<T0, T2>, IEqualityComparer`1<T1>)->Dictionary`2<T1, T2>", SequenceMethod.NotSupported);
			dictionary.Add("ToLookup(IEnumerable`1<T0>, Func`2<T0, T1>)->ILookup`2<T1, T0>", SequenceMethod.NotSupported);
			dictionary.Add("ToLookup(IEnumerable`1<T0>, Func`2<T0, T1>, IEqualityComparer`1<T1>)->ILookup`2<T1, T0>", SequenceMethod.NotSupported);
			dictionary.Add("ToLookup(IEnumerable`1<T0>, Func`2<T0, T1>, Func`2<T0, T2>)->ILookup`2<T1, T2>", SequenceMethod.NotSupported);
			dictionary.Add("ToLookup(IEnumerable`1<T0>, Func`2<T0, T1>, Func`2<T0, T2>, IEqualityComparer`1<T1>)->ILookup`2<T1, T2>", SequenceMethod.NotSupported);
			dictionary.Add("DefaultIfEmpty(IEnumerable`1<T0>)->IEnumerable`1<T0>", SequenceMethod.DefaultIfEmpty);
			dictionary.Add("DefaultIfEmpty(IEnumerable`1<T0>, T0)->IEnumerable`1<T0>", SequenceMethod.DefaultIfEmptyValue);
			dictionary.Add("OfType(IEnumerable)->IEnumerable`1<T0>", SequenceMethod.OfType);
			dictionary.Add("Cast(IEnumerable)->IEnumerable`1<T0>", SequenceMethod.Cast);
			dictionary.Add("Range(Int32, Int32)->IEnumerable`1<Int32>", SequenceMethod.NotSupported);
			dictionary.Add("Sum(IEnumerable`1<Int32>)->Int32", SequenceMethod.SumInt);
			dictionary.Add("Sum(IEnumerable`1<Nullable`1<Int32>>)->Nullable`1<Int32>", SequenceMethod.SumNullableInt);
			dictionary.Add("Sum(IEnumerable`1<Int64>)->Int64", SequenceMethod.SumLong);
			dictionary.Add("Sum(IEnumerable`1<Nullable`1<Int64>>)->Nullable`1<Int64>", SequenceMethod.SumNullableLong);
			dictionary.Add("Sum(IEnumerable`1<Single>)->Single", SequenceMethod.SumSingle);
			dictionary.Add("Sum(IEnumerable`1<Nullable`1<Single>>)->Nullable`1<Single>", SequenceMethod.SumNullableSingle);
			dictionary.Add("Sum(IEnumerable`1<Double>)->Double", SequenceMethod.SumDouble);
			dictionary.Add("Sum(IEnumerable`1<Nullable`1<Double>>)->Nullable`1<Double>", SequenceMethod.SumNullableDouble);
			dictionary.Add("Sum(IEnumerable`1<Decimal>)->Decimal", SequenceMethod.SumDecimal);
			dictionary.Add("Sum(IEnumerable`1<Nullable`1<Decimal>>)->Nullable`1<Decimal>", SequenceMethod.SumNullableDecimal);
			dictionary.Add("Min(IEnumerable`1<Int32>)->Int32", SequenceMethod.MinInt);
			dictionary.Add("Min(IEnumerable`1<Nullable`1<Int32>>)->Nullable`1<Int32>", SequenceMethod.MinNullableInt);
			dictionary.Add("Min(IEnumerable`1<Int64>)->Int64", SequenceMethod.MinLong);
			dictionary.Add("Min(IEnumerable`1<Nullable`1<Int64>>)->Nullable`1<Int64>", SequenceMethod.MinNullableLong);
			dictionary.Add("Min(IEnumerable`1<Single>)->Single", SequenceMethod.MinSingle);
			dictionary.Add("Min(IEnumerable`1<Nullable`1<Single>>)->Nullable`1<Single>", SequenceMethod.MinNullableSingle);
			dictionary.Add("Min(IEnumerable`1<Double>)->Double", SequenceMethod.MinDouble);
			dictionary.Add("Min(IEnumerable`1<Nullable`1<Double>>)->Nullable`1<Double>", SequenceMethod.MinNullableDouble);
			dictionary.Add("Min(IEnumerable`1<Decimal>)->Decimal", SequenceMethod.MinDecimal);
			dictionary.Add("Min(IEnumerable`1<Nullable`1<Decimal>>)->Nullable`1<Decimal>", SequenceMethod.MinNullableDecimal);
			dictionary.Add("Max(IEnumerable`1<Int32>)->Int32", SequenceMethod.MaxInt);
			dictionary.Add("Max(IEnumerable`1<Nullable`1<Int32>>)->Nullable`1<Int32>", SequenceMethod.MaxNullableInt);
			dictionary.Add("Max(IEnumerable`1<Int64>)->Int64", SequenceMethod.MaxLong);
			dictionary.Add("Max(IEnumerable`1<Nullable`1<Int64>>)->Nullable`1<Int64>", SequenceMethod.MaxNullableLong);
			dictionary.Add("Max(IEnumerable`1<Double>)->Double", SequenceMethod.MaxDouble);
			dictionary.Add("Max(IEnumerable`1<Nullable`1<Double>>)->Nullable`1<Double>", SequenceMethod.MaxNullableDouble);
			dictionary.Add("Max(IEnumerable`1<Single>)->Single", SequenceMethod.MaxSingle);
			dictionary.Add("Max(IEnumerable`1<Nullable`1<Single>>)->Nullable`1<Single>", SequenceMethod.MaxNullableSingle);
			dictionary.Add("Max(IEnumerable`1<Decimal>)->Decimal", SequenceMethod.MaxDecimal);
			dictionary.Add("Max(IEnumerable`1<Nullable`1<Decimal>>)->Nullable`1<Decimal>", SequenceMethod.MaxNullableDecimal);
			dictionary.Add("Average(IEnumerable`1<Int32>)->Double", SequenceMethod.AverageInt);
			dictionary.Add("Average(IEnumerable`1<Nullable`1<Int32>>)->Nullable`1<Double>", SequenceMethod.AverageNullableInt);
			dictionary.Add("Average(IEnumerable`1<Int64>)->Double", SequenceMethod.AverageLong);
			dictionary.Add("Average(IEnumerable`1<Nullable`1<Int64>>)->Nullable`1<Double>", SequenceMethod.AverageNullableLong);
			dictionary.Add("Average(IEnumerable`1<Single>)->Single", SequenceMethod.AverageSingle);
			dictionary.Add("Average(IEnumerable`1<Nullable`1<Single>>)->Nullable`1<Single>", SequenceMethod.AverageNullableSingle);
			dictionary.Add("Average(IEnumerable`1<Double>)->Double", SequenceMethod.AverageDouble);
			dictionary.Add("Average(IEnumerable`1<Nullable`1<Double>>)->Nullable`1<Double>", SequenceMethod.AverageNullableDouble);
			dictionary.Add("Average(IEnumerable`1<Decimal>)->Decimal", SequenceMethod.AverageDecimal);
			dictionary.Add("Average(IEnumerable`1<Nullable`1<Decimal>>)->Nullable`1<Decimal>", SequenceMethod.AverageNullableDecimal);
			ReflectionUtil.StaticMethodMap = new Dictionary<MethodInfo, SequenceMethod>(EqualityComparer<MethodInfo>.Default);
			ReflectionUtil.StaticInverseMap = new Dictionary<SequenceMethod, MethodInfo>(EqualityComparer<SequenceMethod>.Default);
			foreach (MethodInfo methodInfo in ReflectionUtil.GetAllLinqOperators())
			{
				string canonicalMethodDescription = ReflectionUtil.GetCanonicalMethodDescription(methodInfo);
				SequenceMethod sequenceMethod;
				if (dictionary.TryGetValue(canonicalMethodDescription, out sequenceMethod))
				{
					ReflectionUtil.StaticMethodMap.Add(methodInfo, sequenceMethod);
					ReflectionUtil.StaticInverseMap[sequenceMethod] = methodInfo;
				}
			}
			ReflectionUtil.DictionaryGetItemMethodInfo = typeof(IDictionary<string, EntityProperty>).GetMethod("get_Item");
			ReflectionUtil.TableQueryProviderReturnSingletonMethodInfo = typeof(TableQueryProvider).GetMethod("ReturnSingleton", BindingFlags.Instance | BindingFlags.NonPublic);
			ReflectionUtil.ProjectMethodInfo = typeof(TableQuery).GetMethods(BindingFlags.Static | BindingFlags.Public).First((MethodInfo m) => m.Name == "Project");
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x0004ADA0 File Offset: 0x00048FA0
		internal static bool TryIdentifySequenceMethod(MethodInfo method, out SequenceMethod sequenceMethod)
		{
			method = (method.IsGenericMethod ? method.GetGenericMethodDefinition() : method);
			return ReflectionUtil.StaticMethodMap.TryGetValue(method, out sequenceMethod);
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x0004ADC4 File Offset: 0x00048FC4
		internal static bool IsSequenceMethod(MethodInfo method, SequenceMethod sequenceMethod)
		{
			SequenceMethod sequenceMethod2;
			return ReflectionUtil.TryIdentifySequenceMethod(method, out sequenceMethod2) && sequenceMethod2 == sequenceMethod;
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x0004AE08 File Offset: 0x00049008
		internal static string GetCanonicalMethodDescription(MethodInfo method)
		{
			Dictionary<Type, int> genericArgumentOrdinals = null;
			if (method.IsGenericMethodDefinition)
			{
				genericArgumentOrdinals = (from t in method.GetGenericArguments()
				where t.IsGenericParameter
				select t).Select((Type t, int i) => new KeyValuePair<Type, int>(t, i)).ToDictionary((KeyValuePair<Type, int> r) => r.Key, (KeyValuePair<Type, int> r) => r.Value);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(method.Name).Append("(");
			bool flag = true;
			foreach (ParameterInfo parameterInfo in method.GetParameters())
			{
				if (flag)
				{
					flag = false;
				}
				else
				{
					stringBuilder.Append(", ");
				}
				ReflectionUtil.AppendCanonicalTypeDescription(parameterInfo.ParameterType, genericArgumentOrdinals, stringBuilder);
			}
			stringBuilder.Append(")");
			if (null != method.ReturnType)
			{
				stringBuilder.Append("->");
				ReflectionUtil.AppendCanonicalTypeDescription(method.ReturnType, genericArgumentOrdinals, stringBuilder);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0004AF48 File Offset: 0x00049148
		private static void AppendCanonicalTypeDescription(Type type, Dictionary<Type, int> genericArgumentOrdinals, StringBuilder description)
		{
			int num;
			if (genericArgumentOrdinals != null && genericArgumentOrdinals.TryGetValue(type, out num))
			{
				description.Append("T").Append(num.ToString(CultureInfo.InvariantCulture));
				return;
			}
			description.Append(type.Name);
			if (type.IsGenericType)
			{
				description.Append("<");
				bool flag = true;
				foreach (Type type2 in type.GetGenericArguments())
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						description.Append(", ");
					}
					ReflectionUtil.AppendCanonicalTypeDescription(type2, genericArgumentOrdinals, description);
				}
				description.Append(">");
			}
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0004AFE9 File Offset: 0x000491E9
		internal static IEnumerable<MethodInfo> GetAllLinqOperators()
		{
			return typeof(Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public).Concat(typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public));
		}

		// Token: 0x040005CA RID: 1482
		private static readonly Dictionary<MethodInfo, SequenceMethod> StaticMethodMap;

		// Token: 0x040005CB RID: 1483
		private static readonly Dictionary<SequenceMethod, MethodInfo> StaticInverseMap;
	}
}
