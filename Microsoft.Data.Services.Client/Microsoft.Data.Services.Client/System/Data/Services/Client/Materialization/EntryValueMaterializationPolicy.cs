using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Services.Client.Metadata;
using System.Linq;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000046 RID: 70
	internal class EntryValueMaterializationPolicy : StructuralValueMaterializationPolicy
	{
		// Token: 0x0600022F RID: 559 RVA: 0x0000B5C7 File Offset: 0x000097C7
		internal EntryValueMaterializationPolicy(IODataMaterializerContext context, EntityTrackingAdapter entityTrackingAdapter, SimpleLazy<PrimitivePropertyConverter> lazyPrimitivePropertyConverter, Dictionary<IEnumerable, DataServiceQueryContinuation> nextLinkTable) : base(context, lazyPrimitivePropertyConverter)
		{
			this.nextLinkTable = nextLinkTable;
			this.EntityTrackingAdapter = entityTrackingAdapter;
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000B5E0 File Offset: 0x000097E0
		// (set) Token: 0x06000231 RID: 561 RVA: 0x0000B5E8 File Offset: 0x000097E8
		internal EntityTrackingAdapter EntityTrackingAdapter { get; private set; }

		// Token: 0x06000232 RID: 562 RVA: 0x0000B5F1 File Offset: 0x000097F1
		internal static void ValidatePropertyMatch(ClientPropertyAnnotation property, ODataNavigationLink link)
		{
			EntryValueMaterializationPolicy.ValidatePropertyMatch(property, link, null, false);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000B5FD File Offset: 0x000097FD
		internal static void ValidatePropertyMatch(ClientPropertyAnnotation property, ODataProperty atomProperty)
		{
			EntryValueMaterializationPolicy.ValidatePropertyMatch(property, atomProperty, null, false);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000B608 File Offset: 0x00009808
		internal static Type ValidatePropertyMatch(ClientPropertyAnnotation property, ODataNavigationLink link, ClientEdmModel model, bool performEntityCheck)
		{
			Type type = null;
			if (link.IsCollection != null)
			{
				if (link.IsCollection.Value)
				{
					if (!property.IsEntityCollection)
					{
						throw Error.InvalidOperation(Strings.Deserialize_MismatchAtomLinkFeedPropertyNotCollection(property.PropertyName));
					}
					type = property.EntityCollectionItemType;
				}
				else
				{
					if (property.IsEntityCollection)
					{
						throw Error.InvalidOperation(Strings.Deserialize_MismatchAtomLinkEntryPropertyIsCollection(property.PropertyName));
					}
					type = property.PropertyType;
				}
			}
			if (type != null && performEntityCheck && !ClientTypeUtil.TypeIsEntity(type, model))
			{
				throw Error.InvalidOperation(Strings.AtomMaterializer_InvalidNonEntityType(type.ToString()));
			}
			return type;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000B6A0 File Offset: 0x000098A0
		internal static void ValidatePropertyMatch(ClientPropertyAnnotation property, ODataProperty atomProperty, ClientEdmModel model, bool performEntityCheck)
		{
			ODataFeed odataFeed = atomProperty.Value as ODataFeed;
			ODataEntry odataEntry = atomProperty.Value as ODataEntry;
			if (property.IsKnownType && (odataFeed != null || odataEntry != null))
			{
				throw Error.InvalidOperation(Strings.Deserialize_MismatchAtomLinkLocalSimple);
			}
			Type type = null;
			if (odataFeed != null)
			{
				if (!property.IsEntityCollection)
				{
					throw Error.InvalidOperation(Strings.Deserialize_MismatchAtomLinkFeedPropertyNotCollection(property.PropertyName));
				}
				type = property.EntityCollectionItemType;
			}
			if (odataEntry != null)
			{
				if (property.IsEntityCollection)
				{
					throw Error.InvalidOperation(Strings.Deserialize_MismatchAtomLinkEntryPropertyIsCollection(property.PropertyName));
				}
				type = property.PropertyType;
			}
			if (type != null && performEntityCheck && !ClientTypeUtil.TypeIsEntity(type, model))
			{
				throw Error.InvalidOperation(Strings.AtomMaterializer_InvalidNonEntityType(type.ToString()));
			}
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000B74C File Offset: 0x0000994C
		internal void Materialize(MaterializerEntry entry, Type expectedEntryType, bool includeLinks)
		{
			if (!this.EntityTrackingAdapter.TryResolveExistingEntity(entry, expectedEntryType))
			{
				ClientTypeAnnotation clientTypeAnnotation = base.MaterializerContext.ResolveTypeForMaterialization(expectedEntryType, entry.Entry.TypeName);
				this.ResolveByCreatingWithType(entry, clientTypeAnnotation.ElementType);
			}
			this.MaterializeResolvedEntry(entry, includeLinks);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000B8E4 File Offset: 0x00009AE4
		internal void ApplyItemsToCollection(MaterializerEntry entry, ClientPropertyAnnotation property, IEnumerable items, Uri nextLink, ProjectionPlan continuationPlan, bool isContinuation)
		{
			IEnumerable<object> enumerable = ODataEntityMaterializer.EnumerateAsElementType<object>(items);
			object obj = this.PopulateCollectionProperty(entry, property, enumerable, nextLink, continuationPlan);
			if (this.EntityTrackingAdapter.MergeOption == MergeOption.OverwriteChanges || this.EntityTrackingAdapter.MergeOption == MergeOption.PreserveChanges)
			{
				var source = from l in this.EntityTrackingAdapter.EntityTracker.GetLinks(entry.ResolvedObject, property.PropertyName)
				select new
				{
					l.Target,
					l.IsModified
				};
				if (obj != null && !property.IsDictionary)
				{
					object[] array = ODataEntityMaterializer.EnumerateAsElementType<object>((IEnumerable)obj).Except(from i in source
					select i.Target).Except(enumerable).ToArray<object>();
					foreach (object value in array)
					{
						property.RemoveValue(obj, value);
					}
				}
				if (!isContinuation)
				{
					IEnumerable<object> enumerable2;
					if (this.EntityTrackingAdapter.MergeOption == MergeOption.OverwriteChanges)
					{
						enumerable2 = from i in source
						select i.Target;
					}
					else
					{
						enumerable2 = from i in source
						where !i.IsModified
						select i.Target;
					}
					enumerable2 = enumerable2.Except(enumerable);
					foreach (object obj2 in enumerable2)
					{
						if (obj != null)
						{
							property.RemoveValue(obj, obj2);
						}
						this.EntityTrackingAdapter.MaterializationLog.RemovedLink(entry, property.PropertyName, obj2);
					}
				}
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000BAC4 File Offset: 0x00009CC4
		internal void FoundNextLinkForCollection(IEnumerable collection, Uri link, ProjectionPlan plan)
		{
			if (collection != null && !this.nextLinkTable.ContainsKey(collection))
			{
				DataServiceQueryContinuation dataServiceQueryContinuation = DataServiceQueryContinuation.Create(link, plan);
				this.nextLinkTable.Add(collection, dataServiceQueryContinuation);
				Util.SetNextLinkForCollection(collection, dataServiceQueryContinuation);
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000BAFE File Offset: 0x00009CFE
		internal void FoundNextLinkForUnmodifiedCollection(IEnumerable collection)
		{
			if (collection != null && !this.nextLinkTable.ContainsKey(collection))
			{
				this.nextLinkTable.Add(collection, null);
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000BB20 File Offset: 0x00009D20
		internal void ResolveByCreatingWithType(MaterializerEntry entry, Type type)
		{
			ClientEdmModel model = base.MaterializerContext.Model;
			entry.ActualType = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(type));
			entry.ResolvedObject = this.CreateNewInstance(entry.ActualType.EdmTypeReference, type);
			entry.CreatedByMaterializer = true;
			entry.ShouldUpdateFromPayload = true;
			entry.EntityHasBeenResolved = true;
			this.EntityTrackingAdapter.MaterializationLog.CreatedInstance(entry);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000BB8C File Offset: 0x00009D8C
		private static void ValidateCollectionElementTypeIsItemType(Type itemType, Type collectionElementType)
		{
			if (!collectionElementType.IsAssignableFrom(itemType))
			{
				string message = Strings.AtomMaterializer_EntryIntoCollectionMismatch(itemType.FullName, collectionElementType.FullName);
				throw new InvalidOperationException(message);
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000BBD4 File Offset: 0x00009DD4
		private static void ApplyLinkProperties(ClientTypeAnnotation actualType, MaterializerEntry entry)
		{
			if (entry.ShouldUpdateFromPayload)
			{
				foreach (ClientPropertyAnnotation clientPropertyAnnotation in from p in actualType.Properties()
				where p.PropertyType == typeof(DataServiceStreamLink)
				select p)
				{
					string propertyName = clientPropertyAnnotation.PropertyName;
					StreamDescriptor streamDescriptor;
					if (entry.EntityDescriptor.TryGetNamedStreamInfo(propertyName, out streamDescriptor))
					{
						clientPropertyAnnotation.SetValue(entry.ResolvedObject, streamDescriptor.StreamLink, propertyName, true);
					}
				}
			}
		}

		// Token: 0x0600023D RID: 573 RVA: 0x0000BC70 File Offset: 0x00009E70
		private object PopulateCollectionProperty(MaterializerEntry entry, ClientPropertyAnnotation property, IEnumerable<object> items, Uri nextLink, ProjectionPlan continuationPlan)
		{
			object obj = null;
			ClientEdmModel model = base.MaterializerContext.Model;
			ClientTypeAnnotation clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(property.EntityCollectionItemType));
			if (entry.ShouldUpdateFromPayload)
			{
				obj = this.GetOrCreateCollectionProperty(entry.ResolvedObject, property, entry.ForLoadProperty);
				foreach (object obj2 in items)
				{
					EntryValueMaterializationPolicy.ValidateCollectionElementTypeIsItemType(obj2.GetType(), clientTypeAnnotation.ElementType);
					property.SetValue(obj, obj2, property.PropertyName, true);
					this.EntityTrackingAdapter.MaterializationLog.AddedLink(entry, property.PropertyName, obj2);
				}
				this.FoundNextLinkForCollection(obj as IEnumerable, nextLink, continuationPlan);
			}
			else
			{
				foreach (object obj3 in items)
				{
					EntryValueMaterializationPolicy.ValidateCollectionElementTypeIsItemType(obj3.GetType(), clientTypeAnnotation.ElementType);
				}
				this.FoundNextLinkForUnmodifiedCollection(property.GetValue(entry.ResolvedObject) as IEnumerable);
			}
			return obj;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000BDA0 File Offset: 0x00009FA0
		private object GetOrCreateCollectionProperty(object instance, ClientPropertyAnnotation property, bool forLoadProperty)
		{
			object obj = property.GetValue(instance);
			if (obj == null)
			{
				Type type = property.PropertyType;
				if (forLoadProperty)
				{
					if (BindingEntityInfo.IsDataServiceCollection(type, base.MaterializerContext.Model))
					{
						obj = Activator.CreateInstance(WebUtil.GetDataServiceCollectionOfT(new Type[]
						{
							property.EntityCollectionItemType
						}), new object[]
						{
							null,
							TrackingMode.None
						});
					}
					else
					{
						Type type2 = typeof(List<>).MakeGenericType(new Type[]
						{
							property.EntityCollectionItemType
						});
						if (type.IsAssignableFrom(type2))
						{
							type = type2;
						}
						obj = Activator.CreateInstance(type);
					}
				}
				else
				{
					if (type.IsInterface())
					{
						type = typeof(Collection<>).MakeGenericType(new Type[]
						{
							property.EntityCollectionItemType
						});
					}
					obj = this.CreateNewInstance(property.EdmProperty.Type, type);
				}
				property.SetValue(instance, obj, property.PropertyName, false);
			}
			return obj;
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000BEA0 File Offset: 0x0000A0A0
		private void ApplyFeedToCollection(MaterializerEntry entry, ClientPropertyAnnotation property, ODataFeed feed, bool includeLinks)
		{
			ClientEdmModel model = base.MaterializerContext.Model;
			ClientTypeAnnotation clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(property.EntityCollectionItemType));
			IEnumerable<ODataEntry> entries = MaterializerFeed.GetFeed(feed).Entries;
			foreach (ODataEntry entry2 in entries)
			{
				this.Materialize(MaterializerEntry.GetEntry(entry2), clientTypeAnnotation.ElementType, includeLinks);
			}
			ProjectionPlan continuationPlan = includeLinks ? ODataEntityMaterializer.CreatePlanForDirectMaterialization(property.EntityCollectionItemType) : ODataEntityMaterializer.CreatePlanForShallowMaterialization(property.EntityCollectionItemType);
			this.ApplyItemsToCollection(entry, property, from e in entries
			select MaterializerEntry.GetEntry(e).ResolvedObject, feed.NextPageLink, continuationPlan, false);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000BF7C File Offset: 0x0000A17C
		private void MaterializeResolvedEntry(MaterializerEntry entry, bool includeLinks)
		{
			ClientTypeAnnotation actualType = entry.ActualType;
			if (!actualType.IsEntityType)
			{
				throw Error.InvalidOperation(Strings.AtomMaterializer_InvalidNonEntityType(actualType.ElementTypeName));
			}
			base.MaterializeDataValues(actualType, entry.Properties, base.MaterializerContext.IgnoreMissingProperties);
			if (entry.NavigationLinks != null)
			{
				foreach (ODataNavigationLink odataNavigationLink in entry.NavigationLinks)
				{
					ClientPropertyAnnotation property = actualType.GetProperty(odataNavigationLink.Name, true);
					if (property != null)
					{
						EntryValueMaterializationPolicy.ValidatePropertyMatch(property, odataNavigationLink, base.MaterializerContext.Model, true);
					}
				}
				if (includeLinks)
				{
					foreach (ODataNavigationLink odataNavigationLink2 in entry.NavigationLinks)
					{
						MaterializerNavigationLink link = MaterializerNavigationLink.GetLink(odataNavigationLink2);
						if (link != null)
						{
							ClientPropertyAnnotation property2 = actualType.GetProperty(odataNavigationLink2.Name, base.MaterializerContext.IgnoreMissingProperties);
							if (property2 != null)
							{
								if (link.Feed != null)
								{
									this.ApplyFeedToCollection(entry, property2, link.Feed, includeLinks);
								}
								else if (link.Entry != null)
								{
									MaterializerEntry entry2 = link.Entry;
									if (entry2.Entry != null)
									{
										this.Materialize(entry2, property2.PropertyType, includeLinks);
									}
									if (entry.ShouldUpdateFromPayload)
									{
										property2.SetValue(entry.ResolvedObject, entry2.ResolvedObject, odataNavigationLink2.Name, true);
										this.EntityTrackingAdapter.MaterializationLog.SetLink(entry, property2.PropertyName, entry2.ResolvedObject);
									}
								}
							}
						}
					}
				}
			}
			foreach (ODataProperty odataProperty in entry.Properties)
			{
				if (!(odataProperty.Value is ODataStreamReferenceValue))
				{
					ClientPropertyAnnotation property3 = actualType.GetProperty(odataProperty.Name, base.MaterializerContext.IgnoreMissingProperties);
					if (property3 != null && entry.ShouldUpdateFromPayload)
					{
						EntryValueMaterializationPolicy.ValidatePropertyMatch(property3, odataProperty, base.MaterializerContext.Model, true);
						base.ApplyDataValue(actualType, odataProperty, entry.ResolvedObject);
					}
				}
			}
			EntryValueMaterializationPolicy.ApplyLinkProperties(actualType, entry);
			base.MaterializerContext.ResponsePipeline.FireEndEntryEvents(entry);
		}

		// Token: 0x0400022E RID: 558
		private readonly Dictionary<IEnumerable, DataServiceQueryContinuation> nextLinkTable;
	}
}
