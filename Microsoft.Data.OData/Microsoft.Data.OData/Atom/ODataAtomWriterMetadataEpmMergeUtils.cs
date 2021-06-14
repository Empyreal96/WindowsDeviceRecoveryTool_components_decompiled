using System;
using System.Collections.Generic;

namespace Microsoft.Data.OData.Atom
{
	// Token: 0x0200026D RID: 621
	internal static class ODataAtomWriterMetadataEpmMergeUtils
	{
		// Token: 0x0600146E RID: 5230 RVA: 0x0004BFF4 File Offset: 0x0004A1F4
		internal static AtomEntryMetadata MergeCustomAndEpmEntryMetadata(AtomEntryMetadata customEntryMetadata, AtomEntryMetadata epmEntryMetadata, ODataWriterBehavior writerBehavior)
		{
			if (customEntryMetadata != null && writerBehavior.FormatBehaviorKind == ODataBehaviorKind.WcfDataServicesClient)
			{
				if (customEntryMetadata.Updated != null)
				{
					customEntryMetadata.UpdatedString = ODataAtomConvert.ToAtomString(customEntryMetadata.Updated.Value);
				}
				if (customEntryMetadata.Published != null)
				{
					customEntryMetadata.PublishedString = ODataAtomConvert.ToAtomString(customEntryMetadata.Published.Value);
				}
			}
			AtomEntryMetadata result;
			if (ODataAtomWriterMetadataEpmMergeUtils.TryMergeIfNull<AtomEntryMetadata>(customEntryMetadata, epmEntryMetadata, out result))
			{
				return result;
			}
			epmEntryMetadata.Title = ODataAtomWriterMetadataEpmMergeUtils.MergeAtomTextValue(customEntryMetadata.Title, epmEntryMetadata.Title, "Title");
			epmEntryMetadata.Summary = ODataAtomWriterMetadataEpmMergeUtils.MergeAtomTextValue(customEntryMetadata.Summary, epmEntryMetadata.Summary, "Summary");
			epmEntryMetadata.Rights = ODataAtomWriterMetadataEpmMergeUtils.MergeAtomTextValue(customEntryMetadata.Rights, epmEntryMetadata.Rights, "Rights");
			if (writerBehavior.FormatBehaviorKind == ODataBehaviorKind.WcfDataServicesClient)
			{
				epmEntryMetadata.PublishedString = ODataAtomWriterMetadataEpmMergeUtils.MergeTextValue(customEntryMetadata.PublishedString, epmEntryMetadata.PublishedString, "PublishedString");
				epmEntryMetadata.UpdatedString = ODataAtomWriterMetadataEpmMergeUtils.MergeTextValue(customEntryMetadata.UpdatedString, epmEntryMetadata.UpdatedString, "UpdatedString");
			}
			else
			{
				epmEntryMetadata.Published = ODataAtomWriterMetadataEpmMergeUtils.MergeDateTimeValue(customEntryMetadata.Published, epmEntryMetadata.Published, "Published");
				epmEntryMetadata.Updated = ODataAtomWriterMetadataEpmMergeUtils.MergeDateTimeValue(customEntryMetadata.Updated, epmEntryMetadata.Updated, "Updated");
			}
			epmEntryMetadata.Authors = ODataAtomWriterMetadataEpmMergeUtils.MergeSyndicationMapping<AtomPersonMetadata>(customEntryMetadata.Authors, epmEntryMetadata.Authors);
			epmEntryMetadata.Contributors = ODataAtomWriterMetadataEpmMergeUtils.MergeSyndicationMapping<AtomPersonMetadata>(customEntryMetadata.Contributors, epmEntryMetadata.Contributors);
			epmEntryMetadata.Categories = ODataAtomWriterMetadataEpmMergeUtils.MergeSyndicationMapping<AtomCategoryMetadata>(customEntryMetadata.Categories, epmEntryMetadata.Categories);
			epmEntryMetadata.Links = ODataAtomWriterMetadataEpmMergeUtils.MergeSyndicationMapping<AtomLinkMetadata>(customEntryMetadata.Links, epmEntryMetadata.Links);
			epmEntryMetadata.Source = customEntryMetadata.Source;
			return epmEntryMetadata;
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0004C1A8 File Offset: 0x0004A3A8
		private static IEnumerable<T> MergeSyndicationMapping<T>(IEnumerable<T> customValues, IEnumerable<T> epmValues)
		{
			IEnumerable<T> result;
			if (ODataAtomWriterMetadataEpmMergeUtils.TryMergeIfNull<IEnumerable<T>>(customValues, epmValues, out result))
			{
				return result;
			}
			List<T> list = (List<T>)epmValues;
			foreach (T item in customValues)
			{
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0004C208 File Offset: 0x0004A408
		private static AtomTextConstruct MergeAtomTextValue(AtomTextConstruct customValue, AtomTextConstruct epmValue, string propertyName)
		{
			AtomTextConstruct result;
			if (ODataAtomWriterMetadataEpmMergeUtils.TryMergeIfNull<AtomTextConstruct>(customValue, epmValue, out result))
			{
				return result;
			}
			if (customValue.Kind != epmValue.Kind)
			{
				throw new ODataException(Strings.ODataAtomMetadataEpmMerge_TextKindConflict(propertyName, customValue.Kind.ToString(), epmValue.Kind.ToString()));
			}
			if (string.CompareOrdinal(customValue.Text, epmValue.Text) != 0)
			{
				throw new ODataException(Strings.ODataAtomMetadataEpmMerge_TextValueConflict(propertyName, customValue.Text, epmValue.Text));
			}
			return epmValue;
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0004C288 File Offset: 0x0004A488
		private static string MergeTextValue(string customValue, string epmValue, string propertyName)
		{
			string result;
			if (ODataAtomWriterMetadataEpmMergeUtils.TryMergeIfNull<string>(customValue, epmValue, out result))
			{
				return result;
			}
			if (string.CompareOrdinal(customValue, epmValue) != 0)
			{
				throw new ODataException(Strings.ODataAtomMetadataEpmMerge_TextValueConflict(propertyName, customValue, epmValue));
			}
			return epmValue;
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0004C2BC File Offset: 0x0004A4BC
		private static DateTimeOffset? MergeDateTimeValue(DateTimeOffset? customValue, DateTimeOffset? epmValue, string propertyName)
		{
			DateTimeOffset? result;
			if (ODataAtomWriterMetadataEpmMergeUtils.TryMergeIfNull<DateTimeOffset>(customValue, epmValue, out result))
			{
				return result;
			}
			if (customValue != epmValue)
			{
				throw new ODataException(Strings.ODataAtomMetadataEpmMerge_TextValueConflict(propertyName, customValue.ToString(), epmValue.ToString()));
			}
			return epmValue;
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0004C335 File Offset: 0x0004A535
		private static bool TryMergeIfNull<T>(T customValue, T epmValue, out T result) where T : class
		{
			if (customValue == null)
			{
				result = epmValue;
				return true;
			}
			if (epmValue == null)
			{
				result = customValue;
				return true;
			}
			result = default(T);
			return false;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0004C361 File Offset: 0x0004A561
		private static bool TryMergeIfNull<T>(T? customValue, T? epmValue, out T? result) where T : struct
		{
			if (customValue == null)
			{
				result = epmValue;
				return true;
			}
			if (epmValue == null)
			{
				result = customValue;
				return true;
			}
			result = null;
			return false;
		}
	}
}
