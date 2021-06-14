using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Services.Client.Metadata;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000039 RID: 57
	internal abstract class ODataEntityMaterializer : ODataMaterializer
	{
		// Token: 0x060001C3 RID: 451 RVA: 0x000095C8 File Offset: 0x000077C8
		public ODataEntityMaterializer(IODataMaterializerContext materializerContext, EntityTrackingAdapter entityTrackingAdapter, QueryComponents queryComponents, Type expectedType, ProjectionPlan materializeEntryPlan) : base(materializerContext, expectedType)
		{
			this.materializeEntryPlan = (materializeEntryPlan ?? ODataEntityMaterializer.CreatePlan(queryComponents));
			this.EntityTrackingAdapter = entityTrackingAdapter;
			SimpleLazy<PrimitivePropertyConverter> lazyPrimitivePropertyConverter = new SimpleLazy<PrimitivePropertyConverter>(() => new PrimitivePropertyConverter(this.Format));
			this.entryValueMaterializerPolicy = new EntryValueMaterializationPolicy(base.MaterializerContext, this.EntityTrackingAdapter, lazyPrimitivePropertyConverter, this.nextLinkTable);
			this.entryValueMaterializerPolicy.CollectionValueMaterializationPolicy = base.CollectionValueMaterializationPolicy;
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000963F File Offset: 0x0000783F
		// (set) Token: 0x060001C5 RID: 453 RVA: 0x00009647 File Offset: 0x00007847
		internal EntityTrackingAdapter EntityTrackingAdapter { get; private set; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00009650 File Offset: 0x00007850
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x0000965D File Offset: 0x0000785D
		internal object TargetInstance
		{
			get
			{
				return this.EntityTrackingAdapter.TargetInstance;
			}
			set
			{
				this.EntityTrackingAdapter.TargetInstance = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000966B File Offset: 0x0000786B
		internal sealed override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00009673 File Offset: 0x00007873
		internal sealed override ProjectionPlan MaterializeEntryPlan
		{
			get
			{
				return this.materializeEntryPlan;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000967B File Offset: 0x0000787B
		protected EntryValueMaterializationPolicy EntryValueMaterializationPolicy
		{
			get
			{
				return this.entryValueMaterializerPolicy;
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00009684 File Offset: 0x00007884
		internal static IEnumerable<T> EnumerateAsElementType<T>(IEnumerable source)
		{
			IEnumerable<T> enumerable = source as IEnumerable<T>;
			if (enumerable != null)
			{
				return enumerable;
			}
			return ODataEntityMaterializer.EnumerateAsElementTypeInternal<T>(source);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000984C File Offset: 0x00007A4C
		internal static IEnumerable<T> EnumerateAsElementTypeInternal<T>(IEnumerable source)
		{
			foreach (object item in source)
			{
				yield return (T)((object)item);
			}
			yield break;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000986C File Offset: 0x00007A6C
		internal static List<TTarget> ListAsElementType<T, TTarget>(ODataEntityMaterializer materializer, IEnumerable<T> source) where T : TTarget
		{
			List<TTarget> list = source as List<TTarget>;
			if (list != null)
			{
				return list;
			}
			IList list2 = source as IList;
			List<TTarget> list3;
			if (list2 != null)
			{
				list3 = new List<TTarget>(list2.Count);
			}
			else
			{
				list3 = new List<TTarget>();
			}
			foreach (T t in source)
			{
				list3.Add((TTarget)((object)t));
			}
			DataServiceQueryContinuation value;
			if (materializer.nextLinkTable.TryGetValue(source, out value))
			{
				materializer.nextLinkTable[list3] = value;
			}
			return list3;
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000990C File Offset: 0x00007B0C
		internal static ProjectionPlan CreatePlanForDirectMaterialization(Type lastSegmentType)
		{
			return new ProjectionPlan
			{
				Plan = new Func<object, object, Type, object>(ODataEntityMaterializerInvoker.DirectMaterializePlan),
				ProjectedType = lastSegmentType,
				LastSegmentType = lastSegmentType
			};
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00009940 File Offset: 0x00007B40
		internal static ProjectionPlan CreatePlanForShallowMaterialization(Type lastSegmentType)
		{
			return new ProjectionPlan
			{
				Plan = new Func<object, object, Type, object>(ODataEntityMaterializerInvoker.ShallowMaterializePlan),
				ProjectedType = lastSegmentType,
				LastSegmentType = lastSegmentType
			};
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00009990 File Offset: 0x00007B90
		internal static bool ProjectionCheckValueForPathIsNull(MaterializerEntry entry, Type expectedType, ProjectionPath path)
		{
			if (path.Count == 0 || (path.Count == 1 && path[0].Member == null))
			{
				return entry.Entry == null;
			}
			bool result = false;
			IEnumerable<ODataNavigationLink> navigationLinks = entry.NavigationLinks;
			ClientEdmModel model = entry.EntityDescriptor.Model;
			for (int i = 0; i < path.Count; i++)
			{
				ProjectionPathSegment projectionPathSegment = path[i];
				if (projectionPathSegment.Member != null)
				{
					bool flag = i == path.Count - 1;
					string propertyName = projectionPathSegment.Member;
					if (projectionPathSegment.SourceTypeAs != null)
					{
						expectedType = projectionPathSegment.SourceTypeAs;
						if (!navigationLinks.Any((ODataNavigationLink p) => p.Name == propertyName))
						{
							return true;
						}
					}
					IEdmType orCreateEdmType = model.GetOrCreateEdmType(expectedType);
					ClientPropertyAnnotation property = model.GetClientTypeAnnotation(orCreateEdmType).GetProperty(propertyName, false);
					MaterializerNavigationLink propertyOrThrow = ODataEntityMaterializer.GetPropertyOrThrow(navigationLinks, propertyName);
					EntryValueMaterializationPolicy.ValidatePropertyMatch(property, propertyOrThrow.Link);
					if (propertyOrThrow.Feed != null)
					{
						result = false;
					}
					else
					{
						if (propertyOrThrow.Entry == null)
						{
							return true;
						}
						if (flag)
						{
							result = (propertyOrThrow.Entry.Entry == null);
						}
						else
						{
							entry = propertyOrThrow.Entry;
							navigationLinks = entry.NavigationLinks;
						}
					}
					expectedType = property.PropertyType;
				}
			}
			return result;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00009AEC File Offset: 0x00007CEC
		internal static IEnumerable ProjectionSelect(ODataEntityMaterializer materializer, MaterializerEntry entry, Type expectedType, Type resultType, ProjectionPath path, Func<object, object, Type, object> selector)
		{
			ClientEdmModel model = materializer.MaterializerContext.Model;
			ClientTypeAnnotation clientTypeAnnotation = entry.ActualType ?? model.GetClientTypeAnnotation(model.GetOrCreateEdmType(expectedType));
			IEnumerable enumerable = (IEnumerable)Util.ActivatorCreateInstance(typeof(List<>).MakeGenericType(new Type[]
			{
				resultType
			}), new object[0]);
			MaterializerNavigationLink materializerNavigationLink = null;
			ClientPropertyAnnotation clientPropertyAnnotation = null;
			for (int i = 0; i < path.Count; i++)
			{
				ProjectionPathSegment projectionPathSegment = path[i];
				if (projectionPathSegment.SourceTypeAs != null)
				{
					clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(projectionPathSegment.SourceTypeAs));
				}
				if (projectionPathSegment.Member != null)
				{
					string member = projectionPathSegment.Member;
					clientPropertyAnnotation = clientTypeAnnotation.GetProperty(member, false);
					materializerNavigationLink = ODataEntityMaterializer.GetPropertyOrThrow(entry.NavigationLinks, member);
					if (materializerNavigationLink.Entry != null)
					{
						entry = materializerNavigationLink.Entry;
						clientTypeAnnotation = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(clientPropertyAnnotation.PropertyType));
					}
				}
			}
			EntryValueMaterializationPolicy.ValidatePropertyMatch(clientPropertyAnnotation, materializerNavigationLink.Link);
			MaterializerFeed feed = MaterializerFeed.GetFeed(materializerNavigationLink.Feed);
			Action<object, object> addToCollectionDelegate = ClientTypeUtil.GetAddToCollectionDelegate(enumerable.GetType());
			foreach (ODataEntry arg in feed.Entries)
			{
				object arg2 = selector(materializer, arg, clientPropertyAnnotation.EntityCollectionItemType);
				addToCollectionDelegate(enumerable, arg2);
			}
			ProjectionPlan projectionPlan = new ProjectionPlan();
			projectionPlan.LastSegmentType = clientPropertyAnnotation.EntityCollectionItemType;
			projectionPlan.Plan = selector;
			projectionPlan.ProjectedType = resultType;
			materializer.EntryValueMaterializationPolicy.FoundNextLinkForCollection(enumerable, feed.NextPageLink, projectionPlan);
			return enumerable;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009CAC File Offset: 0x00007EAC
		internal static ODataEntry ProjectionGetEntry(MaterializerEntry entry, string name)
		{
			MaterializerNavigationLink propertyOrThrow = ODataEntityMaterializer.GetPropertyOrThrow(entry.NavigationLinks, name);
			MaterializerEntry entry2 = propertyOrThrow.Entry;
			if (entry2 == null)
			{
				throw new InvalidOperationException(Strings.AtomMaterializer_PropertyNotExpectedEntry(name));
			}
			ODataEntityMaterializer.CheckEntryToAccessNotNull(entry2, name);
			return entry2.Entry;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009D18 File Offset: 0x00007F18
		internal static object ProjectionInitializeEntity(ODataEntityMaterializer materializer, MaterializerEntry entry, Type expectedType, Type resultType, string[] properties, Func<object, object, Type, object>[] propertyValues)
		{
			if (entry.Entry == null)
			{
				throw new NullReferenceException(Strings.AtomMaterializer_EntryToInitializeIsNull(resultType.FullName));
			}
			if (!entry.EntityHasBeenResolved)
			{
				ODataEntityMaterializer.ProjectionEnsureEntryAvailableOfType(materializer, entry, resultType);
			}
			else if (!resultType.IsAssignableFrom(entry.ActualType.ElementType))
			{
				string message = Strings.AtomMaterializer_ProjectEntityTypeMismatch(resultType.FullName, entry.ActualType.ElementType.FullName, entry.Id);
				throw new InvalidOperationException(message);
			}
			object resolvedObject = entry.ResolvedObject;
			for (int i = 0; i < properties.Length; i++)
			{
				string propertyName = properties[i];
				ClientPropertyAnnotation property = entry.ActualType.GetProperty(propertyName, materializer.MaterializerContext.IgnoreMissingProperties);
				object obj = propertyValues[i](materializer, entry.Entry, expectedType);
				ODataProperty odataProperty = (from p in entry.Entry.Properties
				where p.Name == propertyName
				select p).FirstOrDefault<ODataProperty>();
				StreamDescriptor streamDescriptor;
				if (((odataProperty == null && entry.NavigationLinks != null) ? (from l in entry.NavigationLinks
				where l.Name == propertyName
				select l).FirstOrDefault<ODataNavigationLink>() : null) != null || odataProperty != null || entry.EntityDescriptor.TryGetNamedStreamInfo(propertyName, out streamDescriptor))
				{
					if (entry.ShouldUpdateFromPayload)
					{
						if (property.EdmProperty.Type.TypeKind() == EdmTypeKind.Entity)
						{
							materializer.EntityTrackingAdapter.MaterializationLog.SetLink(entry, property.PropertyName, obj);
						}
						if (!property.IsEntityCollection)
						{
							if (!property.IsPrimitiveOrComplexCollection)
							{
								property.SetValue(resolvedObject, obj, property.PropertyName, false);
							}
						}
						else
						{
							IEnumerable enumerable = (IEnumerable)obj;
							DataServiceQueryContinuation dataServiceQueryContinuation = materializer.nextLinkTable[enumerable];
							Uri nextLink = (dataServiceQueryContinuation == null) ? null : dataServiceQueryContinuation.NextLinkUri;
							ProjectionPlan plan = (dataServiceQueryContinuation == null) ? null : dataServiceQueryContinuation.Plan;
							materializer.MergeLists(entry, property, enumerable, nextLink, plan);
						}
					}
					else if (property.IsEntityCollection)
					{
						materializer.EntryValueMaterializationPolicy.FoundNextLinkForUnmodifiedCollection(property.GetValue(entry.ResolvedObject) as IEnumerable);
					}
				}
			}
			return resolvedObject;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00009F24 File Offset: 0x00008124
		internal static void ProjectionEnsureEntryAvailableOfType(ODataEntityMaterializer materializer, MaterializerEntry entry, Type requiredType)
		{
			if (!materializer.EntityTrackingAdapter.TryResolveAsExistingEntry(entry, requiredType))
			{
				materializer.EntryValueMaterializationPolicy.ResolveByCreatingWithType(entry, requiredType);
				return;
			}
			if (!requiredType.IsAssignableFrom(entry.ResolvedObject.GetType()))
			{
				throw Error.InvalidOperation(Strings.Deserialize_Current(requiredType, entry.ResolvedObject.GetType()));
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00009F78 File Offset: 0x00008178
		internal static object DirectMaterializePlan(ODataEntityMaterializer materializer, MaterializerEntry entry, Type expectedEntryType)
		{
			materializer.entryValueMaterializerPolicy.Materialize(entry, expectedEntryType, true);
			return entry.ResolvedObject;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00009F8E File Offset: 0x0000818E
		internal static object ShallowMaterializePlan(ODataEntityMaterializer materializer, MaterializerEntry entry, Type expectedEntryType)
		{
			materializer.entryValueMaterializerPolicy.Materialize(entry, expectedEntryType, false);
			return entry.ResolvedObject;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000A00C File Offset: 0x0000820C
		internal object ProjectionValueForPath(MaterializerEntry entry, Type expectedType, ProjectionPath path)
		{
			if (path.Count == 0 || (path.Count == 1 && path[0].Member == null))
			{
				if (!entry.EntityHasBeenResolved)
				{
					this.EntryValueMaterializationPolicy.Materialize(entry, expectedType, false);
				}
				return entry.ResolvedObject;
			}
			object obj = null;
			ICollection<ODataNavigationLink> collection = entry.NavigationLinks;
			IEnumerable<ODataProperty> source = entry.Entry.Properties;
			ClientEdmModel model = base.MaterializerContext.Model;
			for (int i = 0; i < path.Count; i++)
			{
				ProjectionPathSegment projectionPathSegment = path[i];
				if (projectionPathSegment.Member != null)
				{
					bool flag = i == path.Count - 1;
					string propertyName = projectionPathSegment.Member;
					expectedType = (projectionPathSegment.SourceTypeAs ?? expectedType);
					ClientPropertyAnnotation property = model.GetClientTypeAnnotation(model.GetOrCreateEdmType(expectedType)).GetProperty(propertyName, false);
					if (property.IsStreamLinkProperty)
					{
						StreamDescriptor streamDescriptor = (from sd in entry.EntityDescriptor.StreamDescriptors
						where sd.Name == propertyName
						select sd).SingleOrDefault<StreamDescriptor>();
						if (streamDescriptor == null)
						{
							if (projectionPathSegment.SourceTypeAs != null)
							{
								obj = WebUtil.GetDefaultValue<DataServiceStreamLink>();
								break;
							}
							throw new InvalidOperationException(Strings.AtomMaterializer_PropertyMissing(propertyName));
						}
						else
						{
							obj = streamDescriptor.StreamLink;
						}
					}
					else
					{
						if (projectionPathSegment.SourceTypeAs != null)
						{
							if (!collection.Any((ODataNavigationLink p) => p.Name == propertyName))
							{
								if (!source.Any((ODataProperty p) => p.Name == propertyName) && flag)
								{
									obj = WebUtil.GetDefaultValue(property.PropertyType);
									break;
								}
							}
						}
						ODataProperty odataProperty = (from p in source
						where p.Name == propertyName
						select p).FirstOrDefault<ODataProperty>();
						ODataNavigationLink odataNavigationLink;
						if (odataProperty != null || collection == null)
						{
							odataNavigationLink = null;
						}
						else
						{
							odataNavigationLink = (from p in collection
							where p.Name == propertyName
							select p).FirstOrDefault<ODataNavigationLink>();
						}
						ODataNavigationLink odataNavigationLink2 = odataNavigationLink;
						if (odataNavigationLink2 == null && odataProperty == null)
						{
							throw new InvalidOperationException(Strings.AtomMaterializer_PropertyMissing(propertyName));
						}
						if (odataNavigationLink2 != null)
						{
							EntryValueMaterializationPolicy.ValidatePropertyMatch(property, odataNavigationLink2);
							MaterializerNavigationLink link = MaterializerNavigationLink.GetLink(odataNavigationLink2);
							if (link.Feed != null)
							{
								MaterializerFeed feed = MaterializerFeed.GetFeed(link.Feed);
								Type implementationType = ClientTypeUtil.GetImplementationType(projectionPathSegment.ProjectionType, typeof(ICollection<>));
								if (implementationType == null)
								{
									implementationType = ClientTypeUtil.GetImplementationType(projectionPathSegment.ProjectionType, typeof(IEnumerable<>));
								}
								Type type = implementationType.GetGenericArguments()[0];
								Type type2 = projectionPathSegment.ProjectionType;
								if (type2.IsInterface() || ClientTypeUtil.IsDataServiceCollection(type2))
								{
									type2 = typeof(Collection<>).MakeGenericType(new Type[]
									{
										type
									});
								}
								IEnumerable enumerable = (IEnumerable)Util.ActivatorCreateInstance(type2, new object[0]);
								ODataEntityMaterializer.MaterializeToList(this, enumerable, type, feed.Entries);
								if (ClientTypeUtil.IsDataServiceCollection(projectionPathSegment.ProjectionType))
								{
									Type dataServiceCollectionOfT = WebUtil.GetDataServiceCollectionOfT(new Type[]
									{
										type
									});
									enumerable = (IEnumerable)Util.ActivatorCreateInstance(dataServiceCollectionOfT, new object[]
									{
										enumerable,
										TrackingMode.None
									});
								}
								ProjectionPlan plan = ODataEntityMaterializer.CreatePlanForShallowMaterialization(type);
								this.EntryValueMaterializationPolicy.FoundNextLinkForCollection(enumerable, feed.Feed.NextPageLink, plan);
								obj = enumerable;
							}
							else if (link.Entry != null)
							{
								MaterializerEntry entry2 = link.Entry;
								if (flag)
								{
									if (entry2.Entry != null && !entry2.EntityHasBeenResolved)
									{
										this.EntryValueMaterializationPolicy.Materialize(entry2, property.PropertyType, false);
									}
								}
								else
								{
									ODataEntityMaterializer.CheckEntryToAccessNotNull(entry2, propertyName);
								}
								source = entry2.Properties;
								collection = entry2.NavigationLinks;
								obj = entry2.ResolvedObject;
								entry = entry2;
							}
						}
						else
						{
							if (odataProperty.Value is ODataStreamReferenceValue)
							{
								obj = null;
								collection = ODataMaterializer.EmptyLinks;
								source = ODataMaterializer.EmptyProperties;
								goto IL_539;
							}
							EntryValueMaterializationPolicy.ValidatePropertyMatch(property, odataProperty);
							if (ClientTypeUtil.TypeOrElementTypeIsEntity(property.PropertyType))
							{
								throw Error.InvalidOperation(Strings.AtomMaterializer_InvalidEntityType(property.EntityCollectionItemType ?? property.PropertyType));
							}
							if (property.IsPrimitiveOrComplexCollection)
							{
								object obj2;
								if ((obj2 = obj) == null)
								{
									obj2 = (entry.ResolvedObject ?? base.CollectionValueMaterializationPolicy.CreateNewInstance(property.EdmProperty.Type.Definition.ToEdmTypeReference(true), expectedType));
								}
								object obj3 = obj2;
								base.ComplexValueMaterializationPolicy.ApplyDataValue(model.GetClientTypeAnnotation(model.GetOrCreateEdmType(obj3.GetType())), odataProperty, obj3);
								collection = ODataMaterializer.EmptyLinks;
								source = ODataMaterializer.EmptyProperties;
							}
							else if (odataProperty.Value is ODataComplexValue)
							{
								ODataComplexValue odataComplexValue = odataProperty.Value as ODataComplexValue;
								base.ComplexValueMaterializationPolicy.MaterializeComplexTypeProperty(property.PropertyType, odataComplexValue);
								source = odataComplexValue.Properties;
								collection = ODataMaterializer.EmptyLinks;
							}
							else
							{
								if (odataProperty.Value == null && !ClientTypeUtil.CanAssignNull(property.NullablePropertyType))
								{
									throw new InvalidOperationException(Strings.AtomMaterializer_CannotAssignNull(odataProperty.Name, property.NullablePropertyType));
								}
								base.ComplexValueMaterializationPolicy.MaterializePrimitiveDataValue(property.NullablePropertyType, odataProperty);
								collection = ODataMaterializer.EmptyLinks;
								source = ODataMaterializer.EmptyProperties;
							}
							obj = odataProperty.GetMaterializedValue();
						}
					}
					expectedType = property.PropertyType;
				}
				IL_539:;
			}
			return obj;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000A566 File Offset: 0x00008766
		internal sealed override void ClearLog()
		{
			this.EntityTrackingAdapter.MaterializationLog.Clear();
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000A578 File Offset: 0x00008778
		internal sealed override void ApplyLogToContext()
		{
			this.EntityTrackingAdapter.MaterializationLog.ApplyToContext();
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A58C File Offset: 0x0000878C
		internal void PropagateContinuation<T>(IEnumerable<T> from, DataServiceCollection<T> to)
		{
			DataServiceQueryContinuation dataServiceQueryContinuation;
			if (this.nextLinkTable.TryGetValue(from, out dataServiceQueryContinuation))
			{
				this.nextLinkTable.Add(to, dataServiceQueryContinuation);
				Util.SetNextLinkForCollection(to, dataServiceQueryContinuation);
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000A5C0 File Offset: 0x000087C0
		protected override bool ReadImplementation()
		{
			this.nextLinkTable.Clear();
			if (!this.ReadNextFeedOrEntry())
			{
				return false;
			}
			if (this.CurrentEntry == null && this.CurrentFeed != null && !this.ReadNextFeedOrEntry())
			{
				return false;
			}
			MaterializerEntry entry = MaterializerEntry.GetEntry(this.CurrentEntry);
			entry.ResolvedObject = this.TargetInstance;
			this.currentValue = this.materializeEntryPlan.Run(this, this.CurrentEntry, base.ExpectedType);
			return true;
		}

		// Token: 0x060001DC RID: 476
		protected abstract bool ReadNextFeedOrEntry();

		// Token: 0x060001DD RID: 477 RVA: 0x0000A633 File Offset: 0x00008833
		private static void CheckEntryToAccessNotNull(MaterializerEntry entry, string name)
		{
			if (entry.Entry == null)
			{
				throw new NullReferenceException(Strings.AtomMaterializer_EntryToAccessIsNull(name));
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000A64C File Offset: 0x0000884C
		private static ProjectionPlan CreatePlan(QueryComponents queryComponents)
		{
			LambdaExpression projection = queryComponents.Projection;
			ProjectionPlan projectionPlan;
			if (projection == null)
			{
				projectionPlan = ODataEntityMaterializer.CreatePlanForDirectMaterialization(queryComponents.LastSegmentType);
			}
			else
			{
				projectionPlan = ProjectionPlanCompiler.CompilePlan(projection, queryComponents.NormalizerRewrites);
				projectionPlan.LastSegmentType = queryComponents.LastSegmentType;
			}
			return projectionPlan;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000A68C File Offset: 0x0000888C
		private static void MaterializeToList(ODataEntityMaterializer materializer, IEnumerable list, Type nestedExpectedType, IEnumerable<ODataEntry> entries)
		{
			Action<object, object> addToCollectionDelegate = ClientTypeUtil.GetAddToCollectionDelegate(list.GetType());
			foreach (ODataEntry entry in entries)
			{
				MaterializerEntry entry2 = MaterializerEntry.GetEntry(entry);
				if (!entry2.EntityHasBeenResolved)
				{
					materializer.EntryValueMaterializationPolicy.Materialize(entry2, nestedExpectedType, false);
				}
				addToCollectionDelegate(list, entry2.ResolvedObject);
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000A720 File Offset: 0x00008920
		private static MaterializerNavigationLink GetPropertyOrThrow(IEnumerable<ODataNavigationLink> links, string propertyName)
		{
			ODataNavigationLink odataNavigationLink = null;
			if (links != null)
			{
				odataNavigationLink = (from p in links
				where p.Name == propertyName
				select p).FirstOrDefault<ODataNavigationLink>();
			}
			if (odataNavigationLink == null)
			{
				throw new InvalidOperationException(Strings.AtomMaterializer_PropertyMissing(propertyName));
			}
			return MaterializerNavigationLink.GetLink(odataNavigationLink);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000A778 File Offset: 0x00008978
		private void MergeLists(MaterializerEntry entry, ClientPropertyAnnotation property, IEnumerable list, Uri nextLink, ProjectionPlan plan)
		{
			if (entry.ShouldUpdateFromPayload && property.NullablePropertyType == list.GetType() && property.GetValue(entry.ResolvedObject) == null)
			{
				property.SetValue(entry.ResolvedObject, list, property.PropertyName, false);
				this.EntryValueMaterializationPolicy.FoundNextLinkForCollection(list, nextLink, plan);
				foreach (object target in list)
				{
					this.EntityTrackingAdapter.MaterializationLog.AddedLink(entry, property.PropertyName, target);
				}
				return;
			}
			this.EntryValueMaterializationPolicy.ApplyItemsToCollection(entry, property, list, nextLink, plan, false);
		}

		// Token: 0x0400020D RID: 525
		protected object currentValue;

		// Token: 0x0400020E RID: 526
		private readonly ProjectionPlan materializeEntryPlan;

		// Token: 0x0400020F RID: 527
		private readonly EntryValueMaterializationPolicy entryValueMaterializerPolicy;
	}
}
