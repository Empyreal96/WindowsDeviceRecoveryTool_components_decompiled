using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.OData.JsonLight;

namespace Microsoft.Data.OData
{
	// Token: 0x0200022C RID: 556
	internal sealed class DuplicatePropertyNamesChecker
	{
		// Token: 0x060011AB RID: 4523 RVA: 0x00041912 File Offset: 0x0003FB12
		public DuplicatePropertyNamesChecker(bool allowDuplicateProperties, bool isResponse)
		{
			this.allowDuplicateProperties = allowDuplicateProperties;
			this.isResponse = isResponse;
		}

		// Token: 0x060011AC RID: 4524 RVA: 0x00041928 File Offset: 0x0003FB28
		internal void CheckForDuplicatePropertyNames(ODataProperty property)
		{
			string name = property.Name;
			DuplicatePropertyNamesChecker.DuplicationKind duplicationKind = DuplicatePropertyNamesChecker.GetDuplicationKind(property);
			DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecord;
			if (!this.TryGetDuplicationRecord(name, out duplicationRecord))
			{
				this.propertyNameCache.Add(name, new DuplicatePropertyNamesChecker.DuplicationRecord(duplicationKind));
				return;
			}
			if (duplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.PropertyAnnotationSeen)
			{
				duplicationRecord.DuplicationKind = duplicationKind;
				return;
			}
			if (duplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.Prohibited || duplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.Prohibited || (duplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.NavigationProperty && duplicationRecord.AssociationLink != null) || !this.allowDuplicateProperties)
			{
				throw new ODataException(Strings.DuplicatePropertyNamesChecker_DuplicatePropertyNamesNotAllowed(name));
			}
		}

		// Token: 0x060011AD RID: 4525 RVA: 0x000419A4 File Offset: 0x0003FBA4
		internal void CheckForDuplicatePropertyNamesOnNavigationLinkStart(ODataNavigationLink navigationLink)
		{
			string name = navigationLink.Name;
			DuplicatePropertyNamesChecker.DuplicationRecord existingDuplicationRecord;
			if (this.propertyNameCache != null && this.propertyNameCache.TryGetValue(name, out existingDuplicationRecord))
			{
				this.CheckNavigationLinkDuplicateNameForExistingDuplicationRecord(name, existingDuplicationRecord);
			}
		}

		// Token: 0x060011AE RID: 4526 RVA: 0x000419D8 File Offset: 0x0003FBD8
		internal ODataAssociationLink CheckForDuplicatePropertyNames(ODataNavigationLink navigationLink, bool isExpanded, bool? isCollection)
		{
			string name = navigationLink.Name;
			DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecord;
			if (!this.TryGetDuplicationRecord(name, out duplicationRecord))
			{
				DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecord2 = new DuplicatePropertyNamesChecker.DuplicationRecord(DuplicatePropertyNamesChecker.DuplicationKind.NavigationProperty);
				DuplicatePropertyNamesChecker.ApplyNavigationLinkToDuplicationRecord(duplicationRecord2, navigationLink, isExpanded, isCollection);
				this.propertyNameCache.Add(name, duplicationRecord2);
				return null;
			}
			this.CheckNavigationLinkDuplicateNameForExistingDuplicationRecord(name, duplicationRecord);
			if (duplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.PropertyAnnotationSeen || (duplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.NavigationProperty && duplicationRecord.AssociationLink != null && duplicationRecord.NavigationLink == null))
			{
				DuplicatePropertyNamesChecker.ApplyNavigationLinkToDuplicationRecord(duplicationRecord, navigationLink, isExpanded, isCollection);
			}
			else if (this.allowDuplicateProperties)
			{
				duplicationRecord.DuplicationKind = DuplicatePropertyNamesChecker.DuplicationKind.NavigationProperty;
				DuplicatePropertyNamesChecker.ApplyNavigationLinkToDuplicationRecord(duplicationRecord, navigationLink, isExpanded, isCollection);
			}
			else
			{
				bool? isCollectionEffectiveValue = DuplicatePropertyNamesChecker.GetIsCollectionEffectiveValue(isExpanded, isCollection);
				if (isCollectionEffectiveValue == false || duplicationRecord.NavigationPropertyIsCollection == false)
				{
					throw new ODataException(Strings.DuplicatePropertyNamesChecker_MultipleLinksForSingleton(name));
				}
				if (isCollectionEffectiveValue != null)
				{
					duplicationRecord.NavigationPropertyIsCollection = isCollectionEffectiveValue;
				}
			}
			return duplicationRecord.AssociationLink;
		}

		// Token: 0x060011AF RID: 4527 RVA: 0x00041AC4 File Offset: 0x0003FCC4
		internal ODataNavigationLink CheckForDuplicateAssociationLinkNames(ODataAssociationLink associationLink)
		{
			string name = associationLink.Name;
			DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecord;
			if (!this.TryGetDuplicationRecord(name, out duplicationRecord))
			{
				this.propertyNameCache.Add(name, new DuplicatePropertyNamesChecker.DuplicationRecord(DuplicatePropertyNamesChecker.DuplicationKind.NavigationProperty)
				{
					AssociationLink = associationLink
				});
				return null;
			}
			if (duplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.PropertyAnnotationSeen || (duplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.NavigationProperty && duplicationRecord.AssociationLink == null))
			{
				duplicationRecord.DuplicationKind = DuplicatePropertyNamesChecker.DuplicationKind.NavigationProperty;
				duplicationRecord.AssociationLink = associationLink;
				return duplicationRecord.NavigationLink;
			}
			throw new ODataException(Strings.DuplicatePropertyNamesChecker_DuplicatePropertyNamesNotAllowed(name));
		}

		// Token: 0x060011B0 RID: 4528 RVA: 0x00041B3B File Offset: 0x0003FD3B
		internal void Clear()
		{
			if (this.propertyNameCache != null)
			{
				this.propertyNameCache.Clear();
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x00041B50 File Offset: 0x0003FD50
		internal void AddODataPropertyAnnotation(string propertyName, string annotationName, object annotationValue)
		{
			DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecordToAddPropertyAnnotation = this.GetDuplicationRecordToAddPropertyAnnotation(propertyName, annotationName);
			Dictionary<string, object> dictionary = duplicationRecordToAddPropertyAnnotation.PropertyODataAnnotations;
			if (dictionary == null)
			{
				dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
				duplicationRecordToAddPropertyAnnotation.PropertyODataAnnotations = dictionary;
			}
			else if (dictionary.ContainsKey(annotationName))
			{
				if (ODataJsonLightReaderUtils.IsAnnotationProperty(propertyName))
				{
					throw new ODataException(Strings.DuplicatePropertyNamesChecker_DuplicateAnnotationForInstanceAnnotationNotAllowed(annotationName, propertyName));
				}
				throw new ODataException(Strings.DuplicatePropertyNamesChecker_DuplicateAnnotationForPropertyNotAllowed(annotationName, propertyName));
			}
			dictionary.Add(annotationName, annotationValue);
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00041BB8 File Offset: 0x0003FDB8
		internal void AddCustomPropertyAnnotation(string propertyName, string annotationName)
		{
			DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecordToAddPropertyAnnotation = this.GetDuplicationRecordToAddPropertyAnnotation(propertyName, annotationName);
			HashSet<string> hashSet = duplicationRecordToAddPropertyAnnotation.PropertyCustomAnnotations;
			if (hashSet == null)
			{
				hashSet = new HashSet<string>(StringComparer.Ordinal);
				duplicationRecordToAddPropertyAnnotation.PropertyCustomAnnotations = hashSet;
			}
			else if (hashSet.Contains(annotationName))
			{
				throw new ODataException(Strings.DuplicatePropertyNamesChecker_DuplicateAnnotationForPropertyNotAllowed(annotationName, propertyName));
			}
			hashSet.Add(annotationName);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x00041C0C File Offset: 0x0003FE0C
		internal Dictionary<string, object> GetODataPropertyAnnotations(string propertyName)
		{
			DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecord;
			if (!this.TryGetDuplicationRecord(propertyName, out duplicationRecord))
			{
				return null;
			}
			DuplicatePropertyNamesChecker.ThrowIfPropertyIsProcessed(propertyName, duplicationRecord);
			return duplicationRecord.PropertyODataAnnotations;
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00041C34 File Offset: 0x0003FE34
		internal void MarkPropertyAsProcessed(string propertyName)
		{
			DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecord;
			if (!this.TryGetDuplicationRecord(propertyName, out duplicationRecord))
			{
				duplicationRecord = new DuplicatePropertyNamesChecker.DuplicationRecord(DuplicatePropertyNamesChecker.DuplicationKind.PropertyAnnotationSeen);
				this.propertyNameCache.Add(propertyName, duplicationRecord);
			}
			DuplicatePropertyNamesChecker.ThrowIfPropertyIsProcessed(propertyName, duplicationRecord);
			duplicationRecord.PropertyODataAnnotations = DuplicatePropertyNamesChecker.propertyAnnotationsProcessedToken;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00041C7C File Offset: 0x0003FE7C
		internal IEnumerable<string> GetAllUnprocessedProperties()
		{
			if (this.propertyNameCache == null)
			{
				return Enumerable.Empty<string>();
			}
			return from property in this.propertyNameCache.Where(new Func<KeyValuePair<string, DuplicatePropertyNamesChecker.DuplicationRecord>, bool>(DuplicatePropertyNamesChecker.IsPropertyUnprocessed))
			select property.Key;
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00041CD0 File Offset: 0x0003FED0
		private static void ThrowIfPropertyIsProcessed(string propertyName, DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecord)
		{
			if (!object.ReferenceEquals(duplicationRecord.PropertyODataAnnotations, DuplicatePropertyNamesChecker.propertyAnnotationsProcessedToken))
			{
				return;
			}
			if (ODataJsonLightReaderUtils.IsAnnotationProperty(propertyName) && !ODataJsonLightUtils.IsMetadataReferenceProperty(propertyName))
			{
				throw new ODataException(Strings.DuplicatePropertyNamesChecker_DuplicateAnnotationNotAllowed(propertyName));
			}
			throw new ODataException(Strings.DuplicatePropertyNamesChecker_DuplicatePropertyNamesNotAllowed(propertyName));
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x00041D0C File Offset: 0x0003FF0C
		private static bool IsPropertyUnprocessed(KeyValuePair<string, DuplicatePropertyNamesChecker.DuplicationRecord> property)
		{
			return !string.IsNullOrEmpty(property.Key) && !object.ReferenceEquals(property.Value.PropertyODataAnnotations, DuplicatePropertyNamesChecker.propertyAnnotationsProcessedToken);
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00041D38 File Offset: 0x0003FF38
		private static DuplicatePropertyNamesChecker.DuplicationKind GetDuplicationKind(ODataProperty property)
		{
			object value = property.Value;
			if (value == null || (!(value is ODataStreamReferenceValue) && !(value is ODataCollectionValue)))
			{
				return DuplicatePropertyNamesChecker.DuplicationKind.PotentiallyAllowed;
			}
			return DuplicatePropertyNamesChecker.DuplicationKind.Prohibited;
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x00041D64 File Offset: 0x0003FF64
		private static bool? GetIsCollectionEffectiveValue(bool isExpanded, bool? isCollection)
		{
			if (isExpanded)
			{
				return isCollection;
			}
			if (!(isCollection == true))
			{
				return null;
			}
			return new bool?(true);
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00041D9D File Offset: 0x0003FF9D
		private static void ApplyNavigationLinkToDuplicationRecord(DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecord, ODataNavigationLink navigationLink, bool isExpanded, bool? isCollection)
		{
			duplicationRecord.DuplicationKind = DuplicatePropertyNamesChecker.DuplicationKind.NavigationProperty;
			duplicationRecord.NavigationLink = navigationLink;
			duplicationRecord.NavigationPropertyIsCollection = DuplicatePropertyNamesChecker.GetIsCollectionEffectiveValue(isExpanded, isCollection);
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00041DBA File Offset: 0x0003FFBA
		private bool TryGetDuplicationRecord(string propertyName, out DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecord)
		{
			if (this.propertyNameCache == null)
			{
				this.propertyNameCache = new Dictionary<string, DuplicatePropertyNamesChecker.DuplicationRecord>(StringComparer.Ordinal);
				duplicationRecord = null;
				return false;
			}
			return this.propertyNameCache.TryGetValue(propertyName, out duplicationRecord);
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00041DE8 File Offset: 0x0003FFE8
		private void CheckNavigationLinkDuplicateNameForExistingDuplicationRecord(string propertyName, DuplicatePropertyNamesChecker.DuplicationRecord existingDuplicationRecord)
		{
			if (existingDuplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.NavigationProperty && existingDuplicationRecord.AssociationLink != null && existingDuplicationRecord.NavigationLink == null)
			{
				return;
			}
			if (existingDuplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.Prohibited || (existingDuplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.PotentiallyAllowed && !this.allowDuplicateProperties) || (existingDuplicationRecord.DuplicationKind == DuplicatePropertyNamesChecker.DuplicationKind.NavigationProperty && this.isResponse && !this.allowDuplicateProperties))
			{
				throw new ODataException(Strings.DuplicatePropertyNamesChecker_DuplicatePropertyNamesNotAllowed(propertyName));
			}
		}

		// Token: 0x060011BD RID: 4541 RVA: 0x00041E50 File Offset: 0x00040050
		private DuplicatePropertyNamesChecker.DuplicationRecord GetDuplicationRecordToAddPropertyAnnotation(string propertyName, string annotationName)
		{
			DuplicatePropertyNamesChecker.DuplicationRecord duplicationRecord;
			if (!this.TryGetDuplicationRecord(propertyName, out duplicationRecord))
			{
				duplicationRecord = new DuplicatePropertyNamesChecker.DuplicationRecord(DuplicatePropertyNamesChecker.DuplicationKind.PropertyAnnotationSeen);
				this.propertyNameCache.Add(propertyName, duplicationRecord);
			}
			if (object.ReferenceEquals(duplicationRecord.PropertyODataAnnotations, DuplicatePropertyNamesChecker.propertyAnnotationsProcessedToken))
			{
				throw new ODataException(Strings.DuplicatePropertyNamesChecker_PropertyAnnotationAfterTheProperty(annotationName, propertyName));
			}
			return duplicationRecord;
		}

		// Token: 0x04000676 RID: 1654
		private static readonly Dictionary<string, object> propertyAnnotationsProcessedToken = new Dictionary<string, object>(0, StringComparer.Ordinal);

		// Token: 0x04000677 RID: 1655
		private readonly bool allowDuplicateProperties;

		// Token: 0x04000678 RID: 1656
		private readonly bool isResponse;

		// Token: 0x04000679 RID: 1657
		private Dictionary<string, DuplicatePropertyNamesChecker.DuplicationRecord> propertyNameCache;

		// Token: 0x0200022D RID: 557
		private enum DuplicationKind
		{
			// Token: 0x0400067C RID: 1660
			PropertyAnnotationSeen,
			// Token: 0x0400067D RID: 1661
			Prohibited,
			// Token: 0x0400067E RID: 1662
			PotentiallyAllowed,
			// Token: 0x0400067F RID: 1663
			NavigationProperty
		}

		// Token: 0x0200022E RID: 558
		private sealed class DuplicationRecord
		{
			// Token: 0x060011C0 RID: 4544 RVA: 0x00041EAE File Offset: 0x000400AE
			public DuplicationRecord(DuplicatePropertyNamesChecker.DuplicationKind duplicationKind)
			{
				this.DuplicationKind = duplicationKind;
			}

			// Token: 0x170003D0 RID: 976
			// (get) Token: 0x060011C1 RID: 4545 RVA: 0x00041EBD File Offset: 0x000400BD
			// (set) Token: 0x060011C2 RID: 4546 RVA: 0x00041EC5 File Offset: 0x000400C5
			public DuplicatePropertyNamesChecker.DuplicationKind DuplicationKind { get; set; }

			// Token: 0x170003D1 RID: 977
			// (get) Token: 0x060011C3 RID: 4547 RVA: 0x00041ECE File Offset: 0x000400CE
			// (set) Token: 0x060011C4 RID: 4548 RVA: 0x00041ED6 File Offset: 0x000400D6
			public ODataNavigationLink NavigationLink { get; set; }

			// Token: 0x170003D2 RID: 978
			// (get) Token: 0x060011C5 RID: 4549 RVA: 0x00041EDF File Offset: 0x000400DF
			// (set) Token: 0x060011C6 RID: 4550 RVA: 0x00041EE7 File Offset: 0x000400E7
			public ODataAssociationLink AssociationLink { get; set; }

			// Token: 0x170003D3 RID: 979
			// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00041EF0 File Offset: 0x000400F0
			// (set) Token: 0x060011C8 RID: 4552 RVA: 0x00041EF8 File Offset: 0x000400F8
			public bool? NavigationPropertyIsCollection { get; set; }

			// Token: 0x170003D4 RID: 980
			// (get) Token: 0x060011C9 RID: 4553 RVA: 0x00041F01 File Offset: 0x00040101
			// (set) Token: 0x060011CA RID: 4554 RVA: 0x00041F09 File Offset: 0x00040109
			public Dictionary<string, object> PropertyODataAnnotations { get; set; }

			// Token: 0x170003D5 RID: 981
			// (get) Token: 0x060011CB RID: 4555 RVA: 0x00041F12 File Offset: 0x00040112
			// (set) Token: 0x060011CC RID: 4556 RVA: 0x00041F1A File Offset: 0x0004011A
			public HashSet<string> PropertyCustomAnnotations { get; set; }
		}
	}
}
