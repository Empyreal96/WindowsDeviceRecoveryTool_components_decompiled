using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Edm;

namespace Microsoft.Data.OData.Evaluation
{
	// Token: 0x02000103 RID: 259
	internal abstract class ODataEntryMetadataContext : IODataEntryMetadataContext
	{
		// Token: 0x06000701 RID: 1793 RVA: 0x00018428 File Offset: 0x00016628
		protected ODataEntryMetadataContext(ODataEntry entry, IODataFeedAndEntryTypeContext typeContext)
		{
			this.entry = entry;
			this.typeContext = typeContext;
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0001843E File Offset: 0x0001663E
		public ODataEntry Entry
		{
			get
			{
				return this.entry;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000703 RID: 1795 RVA: 0x00018446 File Offset: 0x00016646
		public IODataFeedAndEntryTypeContext TypeContext
		{
			get
			{
				return this.typeContext;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000704 RID: 1796
		public abstract string ActualEntityTypeName { get; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000705 RID: 1797
		public abstract ICollection<KeyValuePair<string, object>> KeyProperties { get; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000706 RID: 1798
		public abstract IEnumerable<KeyValuePair<string, object>> ETagProperties { get; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000707 RID: 1799
		public abstract IEnumerable<IEdmNavigationProperty> SelectedNavigationProperties { get; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000708 RID: 1800
		public abstract IDictionary<string, IEdmStructuralProperty> SelectedStreamProperties { get; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x06000709 RID: 1801
		public abstract IEnumerable<IEdmFunctionImport> SelectedAlwaysBindableOperations { get; }

		// Token: 0x0600070A RID: 1802 RVA: 0x0001844E File Offset: 0x0001664E
		internal static ODataEntryMetadataContext Create(ODataEntry entry, IODataFeedAndEntryTypeContext typeContext, ODataFeedAndEntrySerializationInfo serializationInfo, IEdmEntityType actualEntityType, IODataMetadataContext metadataContext, SelectedPropertiesNode selectedProperties)
		{
			if (serializationInfo != null)
			{
				return new ODataEntryMetadataContext.ODataEntryMetadataContextWithoutModel(entry, typeContext, serializationInfo);
			}
			return new ODataEntryMetadataContext.ODataEntryMetadataContextWithModel(entry, typeContext, actualEntityType, metadataContext, selectedProperties);
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x00018484 File Offset: 0x00016684
		private static object GetPrimitivePropertyClrValue(ODataEntry entry, string propertyName, string entityTypeName, bool isKeyProperty)
		{
			ODataProperty odataProperty = (entry.NonComputedProperties == null) ? null : entry.NonComputedProperties.SingleOrDefault((ODataProperty p) => p.Name == propertyName);
			if (odataProperty == null)
			{
				throw new ODataException(Strings.EdmValueUtils_PropertyDoesntExist(entry.TypeName, propertyName));
			}
			return ODataEntryMetadataContext.GetPrimitivePropertyClrValue(entityTypeName, odataProperty, isKeyProperty);
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x000184E4 File Offset: 0x000166E4
		private static object GetPrimitivePropertyClrValue(string entityTypeName, ODataProperty property, bool isKeyProperty)
		{
			object value = property.Value;
			if (value == null && isKeyProperty)
			{
				throw new ODataException(Strings.ODataEntryMetadataContext_NullKeyValue(property.Name, entityTypeName));
			}
			if (value is ODataValue)
			{
				throw new ODataException(Strings.ODataEntryMetadataContext_KeyOrETagValuesMustBePrimitiveValues(property.Name, entityTypeName));
			}
			return value;
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0001852B File Offset: 0x0001672B
		private static void ValidateEntityTypeHasKeyProperties(KeyValuePair<string, object>[] keyProperties, string actualEntityTypeName)
		{
			if (keyProperties == null || keyProperties.Length == 0)
			{
				throw new ODataException(Strings.ODataEntryMetadataContext_EntityTypeWithNoKeyProperties(actualEntityTypeName));
			}
		}

		// Token: 0x040002A5 RID: 677
		private static readonly KeyValuePair<string, object>[] EmptyProperties = new KeyValuePair<string, object>[0];

		// Token: 0x040002A6 RID: 678
		private readonly ODataEntry entry;

		// Token: 0x040002A7 RID: 679
		private readonly IODataFeedAndEntryTypeContext typeContext;

		// Token: 0x040002A8 RID: 680
		private KeyValuePair<string, object>[] keyProperties;

		// Token: 0x040002A9 RID: 681
		private IEnumerable<KeyValuePair<string, object>> etagProperties;

		// Token: 0x040002AA RID: 682
		private IEnumerable<IEdmNavigationProperty> selectedNavigationProperties;

		// Token: 0x040002AB RID: 683
		private IDictionary<string, IEdmStructuralProperty> selectedStreamProperties;

		// Token: 0x040002AC RID: 684
		private IEnumerable<IEdmFunctionImport> selectedAlwaysBindableOperations;

		// Token: 0x02000104 RID: 260
		private sealed class ODataEntryMetadataContextWithoutModel : ODataEntryMetadataContext
		{
			// Token: 0x0600070F RID: 1807 RVA: 0x0001854E File Offset: 0x0001674E
			internal ODataEntryMetadataContextWithoutModel(ODataEntry entry, IODataFeedAndEntryTypeContext typeContext, ODataFeedAndEntrySerializationInfo serializationInfo) : base(entry, typeContext)
			{
				this.serializationInfo = serializationInfo;
			}

			// Token: 0x170001B9 RID: 441
			// (get) Token: 0x06000710 RID: 1808 RVA: 0x0001855F File Offset: 0x0001675F
			public override ICollection<KeyValuePair<string, object>> KeyProperties
			{
				get
				{
					if (this.keyProperties == null)
					{
						this.keyProperties = ODataEntryMetadataContext.ODataEntryMetadataContextWithoutModel.GetPropertiesBySerializationInfoPropertyKind(this.entry, ODataPropertyKind.Key, this.ActualEntityTypeName);
						ODataEntryMetadataContext.ValidateEntityTypeHasKeyProperties(this.keyProperties, this.ActualEntityTypeName);
					}
					return this.keyProperties;
				}
			}

			// Token: 0x170001BA RID: 442
			// (get) Token: 0x06000711 RID: 1809 RVA: 0x00018598 File Offset: 0x00016798
			public override IEnumerable<KeyValuePair<string, object>> ETagProperties
			{
				get
				{
					IEnumerable<KeyValuePair<string, object>> result;
					if ((result = this.etagProperties) == null)
					{
						result = (this.etagProperties = ODataEntryMetadataContext.ODataEntryMetadataContextWithoutModel.GetPropertiesBySerializationInfoPropertyKind(this.entry, ODataPropertyKind.ETag, this.ActualEntityTypeName));
					}
					return result;
				}
			}

			// Token: 0x170001BB RID: 443
			// (get) Token: 0x06000712 RID: 1810 RVA: 0x000185CA File Offset: 0x000167CA
			public override string ActualEntityTypeName
			{
				get
				{
					if (string.IsNullOrEmpty(base.Entry.TypeName))
					{
						throw new ODataException(Strings.ODataFeedAndEntryTypeContext_ODataEntryTypeNameMissing);
					}
					return base.Entry.TypeName;
				}
			}

			// Token: 0x170001BC RID: 444
			// (get) Token: 0x06000713 RID: 1811 RVA: 0x000185F4 File Offset: 0x000167F4
			public override IEnumerable<IEdmNavigationProperty> SelectedNavigationProperties
			{
				get
				{
					return ODataEntryMetadataContext.ODataEntryMetadataContextWithoutModel.EmptyNavigationProperties;
				}
			}

			// Token: 0x170001BD RID: 445
			// (get) Token: 0x06000714 RID: 1812 RVA: 0x000185FB File Offset: 0x000167FB
			public override IDictionary<string, IEdmStructuralProperty> SelectedStreamProperties
			{
				get
				{
					return ODataEntryMetadataContext.ODataEntryMetadataContextWithoutModel.EmptyStreamProperties;
				}
			}

			// Token: 0x170001BE RID: 446
			// (get) Token: 0x06000715 RID: 1813 RVA: 0x00018602 File Offset: 0x00016802
			public override IEnumerable<IEdmFunctionImport> SelectedAlwaysBindableOperations
			{
				get
				{
					return ODataEntryMetadataContext.ODataEntryMetadataContextWithoutModel.EmptyOperations;
				}
			}

			// Token: 0x06000716 RID: 1814 RVA: 0x00018654 File Offset: 0x00016854
			private static KeyValuePair<string, object>[] GetPropertiesBySerializationInfoPropertyKind(ODataEntry entry, ODataPropertyKind propertyKind, string actualEntityTypeName)
			{
				KeyValuePair<string, object>[] result = ODataEntryMetadataContext.EmptyProperties;
				if (entry.NonComputedProperties != null)
				{
					result = (from p in entry.NonComputedProperties
					where p.SerializationInfo != null && p.SerializationInfo.PropertyKind == propertyKind
					select new KeyValuePair<string, object>(p.Name, ODataEntryMetadataContext.GetPrimitivePropertyClrValue(actualEntityTypeName, p, propertyKind == ODataPropertyKind.Key))).ToArray<KeyValuePair<string, object>>();
				}
				return result;
			}

			// Token: 0x040002AD RID: 685
			private static readonly IEdmNavigationProperty[] EmptyNavigationProperties = new IEdmNavigationProperty[0];

			// Token: 0x040002AE RID: 686
			private static readonly Dictionary<string, IEdmStructuralProperty> EmptyStreamProperties = new Dictionary<string, IEdmStructuralProperty>(StringComparer.Ordinal);

			// Token: 0x040002AF RID: 687
			private static readonly IEdmFunctionImport[] EmptyOperations = new IEdmFunctionImport[0];

			// Token: 0x040002B0 RID: 688
			private readonly ODataFeedAndEntrySerializationInfo serializationInfo;
		}

		// Token: 0x02000105 RID: 261
		private sealed class ODataEntryMetadataContextWithModel : ODataEntryMetadataContext
		{
			// Token: 0x06000718 RID: 1816 RVA: 0x000186E7 File Offset: 0x000168E7
			internal ODataEntryMetadataContextWithModel(ODataEntry entry, IODataFeedAndEntryTypeContext typeContext, IEdmEntityType actualEntityType, IODataMetadataContext metadataContext, SelectedPropertiesNode selectedProperties) : base(entry, typeContext)
			{
				this.actualEntityType = actualEntityType;
				this.metadataContext = metadataContext;
				this.selectedProperties = selectedProperties;
			}

			// Token: 0x170001BF RID: 447
			// (get) Token: 0x06000719 RID: 1817 RVA: 0x00018730 File Offset: 0x00016930
			public override ICollection<KeyValuePair<string, object>> KeyProperties
			{
				get
				{
					if (this.keyProperties == null)
					{
						IEnumerable<IEdmStructuralProperty> enumerable = this.actualEntityType.Key();
						if (enumerable != null)
						{
							this.keyProperties = (from p in enumerable
							select new KeyValuePair<string, object>(p.Name, ODataEntryMetadataContext.GetPrimitivePropertyClrValue(this.entry, p.Name, this.ActualEntityTypeName, true))).ToArray<KeyValuePair<string, object>>();
						}
						ODataEntryMetadataContext.ValidateEntityTypeHasKeyProperties(this.keyProperties, this.ActualEntityTypeName);
					}
					return this.keyProperties;
				}
			}

			// Token: 0x170001C0 RID: 448
			// (get) Token: 0x0600071A RID: 1818 RVA: 0x000187C0 File Offset: 0x000169C0
			public override IEnumerable<KeyValuePair<string, object>> ETagProperties
			{
				get
				{
					if (this.etagProperties == null)
					{
						IEnumerable<IEdmStructuralProperty> enumerable = this.actualEntityType.StructuralProperties();
						IEnumerable<KeyValuePair<string, object>> etagProperties;
						if (enumerable == null)
						{
							etagProperties = ODataEntryMetadataContext.EmptyProperties;
						}
						else
						{
							etagProperties = (from p in enumerable
							where p.ConcurrencyMode == EdmConcurrencyMode.Fixed
							select new KeyValuePair<string, object>(p.Name, ODataEntryMetadataContext.GetPrimitivePropertyClrValue(this.entry, p.Name, this.ActualEntityTypeName, false))).ToArray<KeyValuePair<string, object>>();
						}
						this.etagProperties = etagProperties;
					}
					return this.etagProperties;
				}
			}

			// Token: 0x170001C1 RID: 449
			// (get) Token: 0x0600071B RID: 1819 RVA: 0x00018837 File Offset: 0x00016A37
			public override string ActualEntityTypeName
			{
				get
				{
					return this.actualEntityType.FullName();
				}
			}

			// Token: 0x170001C2 RID: 450
			// (get) Token: 0x0600071C RID: 1820 RVA: 0x00018844 File Offset: 0x00016A44
			public override IEnumerable<IEdmNavigationProperty> SelectedNavigationProperties
			{
				get
				{
					IEnumerable<IEdmNavigationProperty> result;
					if ((result = this.selectedNavigationProperties) == null)
					{
						result = (this.selectedNavigationProperties = this.selectedProperties.GetSelectedNavigationProperties(this.actualEntityType));
					}
					return result;
				}
			}

			// Token: 0x170001C3 RID: 451
			// (get) Token: 0x0600071D RID: 1821 RVA: 0x00018878 File Offset: 0x00016A78
			public override IDictionary<string, IEdmStructuralProperty> SelectedStreamProperties
			{
				get
				{
					IDictionary<string, IEdmStructuralProperty> result;
					if ((result = this.selectedStreamProperties) == null)
					{
						result = (this.selectedStreamProperties = this.selectedProperties.GetSelectedStreamProperties(this.actualEntityType));
					}
					return result;
				}
			}

			// Token: 0x170001C4 RID: 452
			// (get) Token: 0x0600071E RID: 1822 RVA: 0x000188D8 File Offset: 0x00016AD8
			public override IEnumerable<IEdmFunctionImport> SelectedAlwaysBindableOperations
			{
				get
				{
					if (this.selectedAlwaysBindableOperations == null)
					{
						bool mustBeContainerQualified = this.metadataContext.OperationsBoundToEntityTypeMustBeContainerQualified(this.actualEntityType);
						this.selectedAlwaysBindableOperations = (from operation in this.metadataContext.GetAlwaysBindableOperationsForType(this.actualEntityType)
						where this.selectedProperties.IsOperationSelected(this.actualEntityType, operation, mustBeContainerQualified)
						select operation).ToArray<IEdmFunctionImport>();
					}
					return this.selectedAlwaysBindableOperations;
				}
			}

			// Token: 0x040002B1 RID: 689
			private readonly IEdmEntityType actualEntityType;

			// Token: 0x040002B2 RID: 690
			private readonly IODataMetadataContext metadataContext;

			// Token: 0x040002B3 RID: 691
			private readonly SelectedPropertiesNode selectedProperties;
		}
	}
}
