using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Services.Client.Metadata;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.Data.Services.Client
{
	// Token: 0x020000ED RID: 237
	internal sealed class BindingObserver
	{
		// Token: 0x060007C9 RID: 1993 RVA: 0x0002162E File Offset: 0x0001F82E
		internal BindingObserver(DataServiceContext context, Func<EntityChangedParams, bool> entityChanged, Func<EntityCollectionChangedParams, bool> collectionChanged)
		{
			this.Context = context;
			this.Context.ChangesSaved += this.OnChangesSaved;
			this.EntityChanged = entityChanged;
			this.CollectionChanged = collectionChanged;
			this.bindingGraph = new BindingGraph(this);
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x0002166E File Offset: 0x0001F86E
		// (set) Token: 0x060007CB RID: 1995 RVA: 0x00021676 File Offset: 0x0001F876
		internal DataServiceContext Context { get; private set; }

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x0002167F File Offset: 0x0001F87F
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x00021687 File Offset: 0x0001F887
		internal bool AttachBehavior { get; set; }

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x00021690 File Offset: 0x0001F890
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x00021698 File Offset: 0x0001F898
		internal bool DetachBehavior { get; set; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x000216A1 File Offset: 0x0001F8A1
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x000216A9 File Offset: 0x0001F8A9
		internal Func<EntityChangedParams, bool> EntityChanged { get; private set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x000216B2 File Offset: 0x0001F8B2
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x000216BA File Offset: 0x0001F8BA
		internal Func<EntityCollectionChangedParams, bool> CollectionChanged { get; private set; }

		// Token: 0x060007D4 RID: 2004 RVA: 0x000216C4 File Offset: 0x0001F8C4
		internal void StartTracking<T>(DataServiceCollection<T> collection, string collectionEntitySet)
		{
			try
			{
				this.AttachBehavior = true;
				this.bindingGraph.AddDataServiceCollection(null, null, collection, collectionEntitySet);
			}
			finally
			{
				this.AttachBehavior = false;
			}
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x00021704 File Offset: 0x0001F904
		internal void StopTracking()
		{
			this.bindingGraph.Reset();
			this.Context.ChangesSaved -= this.OnChangesSaved;
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x00021728 File Offset: 0x0001F928
		internal bool LookupParent<T>(DataServiceCollection<T> collection, out object parentEntity, out string parentProperty)
		{
			string text;
			string text2;
			this.bindingGraph.GetDataServiceCollectionInfo(collection, out parentEntity, out parentProperty, out text, out text2);
			return parentEntity != null;
		}

		// Token: 0x060007D7 RID: 2007 RVA: 0x00021750 File Offset: 0x0001F950
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		internal void OnPropertyChanged(object source, PropertyChangedEventArgs eventArgs)
		{
			Util.CheckArgumentNull<object>(source, "source");
			Util.CheckArgumentNull<PropertyChangedEventArgs>(eventArgs, "eventArgs");
			string propertyName = eventArgs.PropertyName;
			if (string.IsNullOrEmpty(propertyName))
			{
				this.HandleUpdateEntity(source, null, null);
				return;
			}
			BindingEntityInfo.BindingPropertyInfo bindingPropertyInfo;
			ClientPropertyAnnotation clientPropertyAnnotation;
			object obj;
			if (BindingEntityInfo.TryGetPropertyValue(source, propertyName, this.Context.Model, out bindingPropertyInfo, out clientPropertyAnnotation, out obj))
			{
				if (bindingPropertyInfo != null)
				{
					this.bindingGraph.RemoveRelation(source, propertyName);
					switch (bindingPropertyInfo.PropertyKind)
					{
					case BindingPropertyKind.BindingPropertyKindEntity:
						this.bindingGraph.AddEntity(source, propertyName, obj, null, source);
						return;
					case BindingPropertyKind.BindingPropertyKindDataServiceCollection:
						if (obj == null)
						{
							return;
						}
						try
						{
							typeof(BindingUtils).GetMethod("VerifyObserverNotPresent", false, true).MakeGenericMethod(new Type[]
							{
								bindingPropertyInfo.PropertyInfo.EntityCollectionItemType
							}).Invoke(null, new object[]
							{
								obj,
								propertyName,
								source.GetType()
							});
						}
						catch (TargetInvocationException ex)
						{
							throw ex.InnerException;
						}
						try
						{
							this.AttachBehavior = true;
							this.bindingGraph.AddDataServiceCollection(source, propertyName, obj, null);
							return;
						}
						finally
						{
							this.AttachBehavior = false;
						}
						break;
					case BindingPropertyKind.BindingPropertyKindPrimitiveOrComplexCollection:
						break;
					default:
						if (obj != null)
						{
							this.bindingGraph.AddComplexObject(source, propertyName, obj);
						}
						this.HandleUpdateEntity(source, propertyName, obj);
						return;
					}
					if (obj != null)
					{
						this.bindingGraph.AddPrimitiveOrComplexCollection(source, propertyName, obj, bindingPropertyInfo.PropertyInfo.PrimitiveOrComplexCollectionItemType);
					}
					this.HandleUpdateEntity(source, propertyName, obj);
					return;
				}
				if (!clientPropertyAnnotation.IsStreamLinkProperty)
				{
					this.HandleUpdateEntity(source, propertyName, obj);
				}
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x000218EC File Offset: 0x0001FAEC
		internal void OnDataServiceCollectionChanged(object collection, NotifyCollectionChangedEventArgs eventArgs)
		{
			Util.CheckArgumentNull<object>(collection, "collection");
			Util.CheckArgumentNull<NotifyCollectionChangedEventArgs>(eventArgs, "eventArgs");
			object source;
			string sourceProperty;
			string text;
			string targetEntitySet;
			this.bindingGraph.GetDataServiceCollectionInfo(collection, out source, out sourceProperty, out text, out targetEntitySet);
			switch (eventArgs.Action)
			{
			case NotifyCollectionChangedAction.Add:
				this.OnAddToDataServiceCollection(eventArgs, source, sourceProperty, targetEntitySet, collection);
				return;
			case NotifyCollectionChangedAction.Remove:
				this.OnRemoveFromDataServiceCollection(eventArgs, source, sourceProperty, collection);
				return;
			case NotifyCollectionChangedAction.Replace:
				this.OnRemoveFromDataServiceCollection(eventArgs, source, sourceProperty, collection);
				this.OnAddToDataServiceCollection(eventArgs, source, sourceProperty, targetEntitySet, collection);
				return;
			case NotifyCollectionChangedAction.Move:
				return;
			case NotifyCollectionChangedAction.Reset:
				if (this.DetachBehavior)
				{
					this.RemoveWithDetachDataServiceCollection(collection);
					return;
				}
				this.bindingGraph.RemoveCollection(collection);
				return;
			default:
				throw new InvalidOperationException(Strings.DataBinding_DataServiceCollectionChangedUnknownActionCollection(eventArgs.Action));
			}
		}

		// Token: 0x060007D9 RID: 2009 RVA: 0x000219AC File Offset: 0x0001FBAC
		internal void OnPrimitiveOrComplexCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Util.CheckArgumentNull<object>(sender, "sender");
			Util.CheckArgumentNull<NotifyCollectionChangedEventArgs>(e, "e");
			object entity;
			string propertyName;
			Type type;
			this.bindingGraph.GetPrimitiveOrComplexCollectionInfo(sender, out entity, out propertyName, out type);
			if (!PrimitiveType.IsKnownNullableType(type))
			{
				switch (e.Action)
				{
				case NotifyCollectionChangedAction.Add:
					this.OnAddToComplexTypeCollection(sender, e.NewItems);
					break;
				case NotifyCollectionChangedAction.Remove:
					this.OnRemoveFromComplexTypeCollection(sender, e.OldItems);
					break;
				case NotifyCollectionChangedAction.Replace:
					this.OnRemoveFromComplexTypeCollection(sender, e.OldItems);
					this.OnAddToComplexTypeCollection(sender, e.NewItems);
					break;
				case NotifyCollectionChangedAction.Move:
					break;
				case NotifyCollectionChangedAction.Reset:
					this.bindingGraph.RemoveCollection(sender);
					break;
				default:
					throw new InvalidOperationException(Strings.DataBinding_CollectionChangedUnknownActionCollection(e.Action, sender.GetType()));
				}
			}
			this.HandleUpdateEntity(entity, propertyName, sender);
		}

		// Token: 0x060007DA RID: 2010 RVA: 0x00021A80 File Offset: 0x0001FC80
		internal void HandleAddEntity(object source, string sourceProperty, string sourceEntitySet, ICollection collection, object target, string targetEntitySet)
		{
			if (this.Context.ApplyingChanges)
			{
				return;
			}
			if (source != null && this.IsDetachedOrDeletedFromContext(source))
			{
				return;
			}
			EntityDescriptor entityDescriptor = this.Context.GetEntityDescriptor(target);
			bool flag = !this.AttachBehavior && (entityDescriptor == null || (source != null && !this.IsContextTrackingLink(source, sourceProperty, target) && entityDescriptor.State != EntityStates.Deleted));
			if (flag && this.CollectionChanged != null)
			{
				EntityCollectionChangedParams arg = new EntityCollectionChangedParams(this.Context, source, sourceProperty, sourceEntitySet, collection, target, targetEntitySet, NotifyCollectionChangedAction.Add);
				if (this.CollectionChanged(arg))
				{
					return;
				}
			}
			if (source != null && this.IsDetachedOrDeletedFromContext(source))
			{
				throw new InvalidOperationException(Strings.DataBinding_BindingOperation_DetachedSource);
			}
			entityDescriptor = this.Context.GetEntityDescriptor(target);
			if (source != null)
			{
				if (this.AttachBehavior)
				{
					if (entityDescriptor == null)
					{
						BindingUtils.ValidateEntitySetName(targetEntitySet, target);
						this.Context.AttachTo(targetEntitySet, target);
						this.Context.AttachLink(source, sourceProperty, target);
						return;
					}
					if (entityDescriptor.State != EntityStates.Deleted && !this.IsContextTrackingLink(source, sourceProperty, target))
					{
						this.Context.AttachLink(source, sourceProperty, target);
						return;
					}
				}
				else
				{
					if (entityDescriptor == null)
					{
						this.Context.AddRelatedObject(source, sourceProperty, target);
						return;
					}
					if (entityDescriptor.State != EntityStates.Deleted && !this.IsContextTrackingLink(source, sourceProperty, target))
					{
						this.Context.AddLink(source, sourceProperty, target);
						return;
					}
				}
			}
			else if (entityDescriptor == null)
			{
				BindingUtils.ValidateEntitySetName(targetEntitySet, target);
				if (this.AttachBehavior)
				{
					this.Context.AttachTo(targetEntitySet, target);
					return;
				}
				this.Context.AddObject(targetEntitySet, target);
			}
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00021C08 File Offset: 0x0001FE08
		internal void HandleDeleteEntity(object source, string sourceProperty, string sourceEntitySet, ICollection collection, object target, string targetEntitySet)
		{
			if (this.Context.ApplyingChanges)
			{
				return;
			}
			if (source != null && this.IsDetachedOrDeletedFromContext(source))
			{
				return;
			}
			bool flag = this.IsContextTrackingEntity(target) && !this.DetachBehavior;
			if (flag && this.CollectionChanged != null)
			{
				EntityCollectionChangedParams arg = new EntityCollectionChangedParams(this.Context, source, sourceProperty, sourceEntitySet, collection, target, targetEntitySet, NotifyCollectionChangedAction.Remove);
				if (this.CollectionChanged(arg))
				{
					return;
				}
			}
			if (source != null && !this.IsContextTrackingEntity(source))
			{
				throw new InvalidOperationException(Strings.DataBinding_BindingOperation_DetachedSource);
			}
			if (this.IsContextTrackingEntity(target))
			{
				if (this.DetachBehavior)
				{
					this.Context.Detach(target);
					return;
				}
				this.Context.DeleteObject(target);
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00021CBC File Offset: 0x0001FEBC
		internal void HandleUpdateEntityReference(object source, string sourceProperty, string sourceEntitySet, object target, string targetEntitySet)
		{
			if (this.Context.ApplyingChanges)
			{
				return;
			}
			if (this.IsDetachedOrDeletedFromContext(source))
			{
				return;
			}
			EntityDescriptor entityDescriptor = (target != null) ? this.Context.GetEntityDescriptor(target) : null;
			bool flag = !this.AttachBehavior && (entityDescriptor == null || !this.IsContextTrackingLink(source, sourceProperty, target));
			if (flag && this.EntityChanged != null)
			{
				EntityChangedParams arg = new EntityChangedParams(this.Context, source, sourceProperty, target, sourceEntitySet, targetEntitySet);
				if (this.EntityChanged(arg))
				{
					return;
				}
			}
			if (this.IsDetachedOrDeletedFromContext(source))
			{
				throw new InvalidOperationException(Strings.DataBinding_BindingOperation_DetachedSource);
			}
			entityDescriptor = ((target != null) ? this.Context.GetEntityDescriptor(target) : null);
			if (target != null)
			{
				if (entityDescriptor == null)
				{
					BindingUtils.ValidateEntitySetName(targetEntitySet, target);
					if (this.AttachBehavior)
					{
						this.Context.AttachTo(targetEntitySet, target);
					}
					else
					{
						this.Context.AddObject(targetEntitySet, target);
					}
					entityDescriptor = this.Context.GetEntityDescriptor(target);
				}
				if (!this.IsContextTrackingLink(source, sourceProperty, target))
				{
					if (!this.AttachBehavior)
					{
						this.Context.SetLink(source, sourceProperty, target);
						return;
					}
					if (entityDescriptor.State != EntityStates.Deleted)
					{
						this.Context.AttachLink(source, sourceProperty, target);
						return;
					}
				}
			}
			else
			{
				this.Context.SetLink(source, sourceProperty, null);
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00021DFC File Offset: 0x0001FFFC
		internal bool IsContextTrackingEntity(object entity)
		{
			return this.Context.GetEntityDescriptor(entity) != null;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00021E10 File Offset: 0x00020010
		private void HandleUpdateEntity(object entity, string propertyName, object propertyValue)
		{
			if (this.Context.ApplyingChanges)
			{
				return;
			}
			if (!BindingEntityInfo.IsEntityType(entity.GetType(), this.Context.Model))
			{
				this.bindingGraph.GetAncestorEntityForComplexProperty(ref entity, ref propertyName, ref propertyValue);
			}
			if (this.IsDetachedOrDeletedFromContext(entity))
			{
				return;
			}
			if (this.EntityChanged != null)
			{
				EntityChangedParams arg = new EntityChangedParams(this.Context, entity, propertyName, propertyValue, null, null);
				if (this.EntityChanged(arg))
				{
					return;
				}
			}
			if (this.IsContextTrackingEntity(entity))
			{
				this.Context.UpdateObject(entity);
			}
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00021E9C File Offset: 0x0002009C
		private void OnAddToDataServiceCollection(NotifyCollectionChangedEventArgs eventArgs, object source, string sourceProperty, string targetEntitySet, object collection)
		{
			if (eventArgs.NewItems != null)
			{
				foreach (object obj in eventArgs.NewItems)
				{
					if (obj == null)
					{
						throw new InvalidOperationException(Strings.DataBinding_BindingOperation_ArrayItemNull("Add"));
					}
					if (!BindingEntityInfo.IsEntityType(obj.GetType(), this.Context.Model))
					{
						throw new InvalidOperationException(Strings.DataBinding_BindingOperation_ArrayItemNotEntity("Add"));
					}
					this.bindingGraph.AddEntity(source, sourceProperty, obj, targetEntitySet, collection);
				}
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00021F40 File Offset: 0x00020140
		private void OnRemoveFromDataServiceCollection(NotifyCollectionChangedEventArgs eventArgs, object source, string sourceProperty, object collection)
		{
			if (eventArgs.OldItems != null)
			{
				this.DeepRemoveDataServiceCollection(eventArgs.OldItems, source ?? collection, sourceProperty, new Action<object>(this.ValidateDataServiceCollectionItem));
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x00021F6C File Offset: 0x0002016C
		private void RemoveWithDetachDataServiceCollection(object collection)
		{
			object obj = null;
			string sourceProperty = null;
			string text = null;
			string text2 = null;
			this.bindingGraph.GetDataServiceCollectionInfo(collection, out obj, out sourceProperty, out text, out text2);
			this.DeepRemoveDataServiceCollection(this.bindingGraph.GetDataServiceCollectionItems(collection), obj ?? collection, sourceProperty, null);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00021FB0 File Offset: 0x000201B0
		private void DeepRemoveDataServiceCollection(IEnumerable collection, object source, string sourceProperty, Action<object> itemValidator)
		{
			foreach (object obj in collection)
			{
				if (itemValidator != null)
				{
					itemValidator(obj);
				}
				List<BindingObserver.UnTrackingInfo> list = new List<BindingObserver.UnTrackingInfo>();
				this.CollectUnTrackingInfo(obj, source, sourceProperty, list);
				foreach (BindingObserver.UnTrackingInfo unTrackingInfo in list)
				{
					this.bindingGraph.RemoveDataServiceCollectionItem(unTrackingInfo.Entity, unTrackingInfo.Parent, unTrackingInfo.ParentProperty);
				}
			}
			this.bindingGraph.RemoveUnreachableVertices();
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00022078 File Offset: 0x00020278
		private void OnAddToComplexTypeCollection(object collection, IList newItems)
		{
			if (newItems != null)
			{
				this.bindingGraph.AddComplexObjectsFromCollection(collection, newItems);
			}
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x0002208C File Offset: 0x0002028C
		private void OnRemoveFromComplexTypeCollection(object collection, IList items)
		{
			if (items != null)
			{
				foreach (object item in items)
				{
					this.bindingGraph.RemoveComplexTypeCollectionItem(item, collection);
				}
				this.bindingGraph.RemoveUnreachableVertices();
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x000220F0 File Offset: 0x000202F0
		private void OnChangesSaved(object sender, SaveChangesEventArgs eventArgs)
		{
			this.bindingGraph.RemoveNonTrackedEntities();
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00022120 File Offset: 0x00020320
		private void CollectUnTrackingInfo(object currentEntity, object parentEntity, string parentProperty, IList<BindingObserver.UnTrackingInfo> entitiesToUnTrack)
		{
			foreach (EntityDescriptor entityDescriptor in from x in this.Context.Entities
			where x.ParentEntity == currentEntity && x.State == EntityStates.Added
			select x)
			{
				this.CollectUnTrackingInfo(entityDescriptor.Entity, entityDescriptor.ParentEntity, entityDescriptor.ParentPropertyForInsert, entitiesToUnTrack);
			}
			entitiesToUnTrack.Add(new BindingObserver.UnTrackingInfo
			{
				Entity = currentEntity,
				Parent = parentEntity,
				ParentProperty = parentProperty
			});
		}

		// Token: 0x060007E7 RID: 2023 RVA: 0x000221CC File Offset: 0x000203CC
		private bool IsContextTrackingLink(object source, string sourceProperty, object target)
		{
			return this.Context.GetLinkDescriptor(source, sourceProperty, target) != null;
		}

		// Token: 0x060007E8 RID: 2024 RVA: 0x000221E4 File Offset: 0x000203E4
		private bool IsDetachedOrDeletedFromContext(object entity)
		{
			EntityDescriptor entityDescriptor = this.Context.GetEntityDescriptor(entity);
			return entityDescriptor == null || entityDescriptor.State == EntityStates.Deleted;
		}

		// Token: 0x060007E9 RID: 2025 RVA: 0x0002220C File Offset: 0x0002040C
		private void ValidateDataServiceCollectionItem(object target)
		{
			if (target == null)
			{
				throw new InvalidOperationException(Strings.DataBinding_BindingOperation_ArrayItemNull("Remove"));
			}
			if (!BindingEntityInfo.IsEntityType(target.GetType(), this.Context.Model))
			{
				throw new InvalidOperationException(Strings.DataBinding_BindingOperation_ArrayItemNotEntity("Remove"));
			}
		}

		// Token: 0x040004BB RID: 1211
		private BindingGraph bindingGraph;

		// Token: 0x020000EE RID: 238
		private class UnTrackingInfo
		{
			// Token: 0x170001C5 RID: 453
			// (get) Token: 0x060007EA RID: 2026 RVA: 0x00022249 File Offset: 0x00020449
			// (set) Token: 0x060007EB RID: 2027 RVA: 0x00022251 File Offset: 0x00020451
			public object Entity { get; set; }

			// Token: 0x170001C6 RID: 454
			// (get) Token: 0x060007EC RID: 2028 RVA: 0x0002225A File Offset: 0x0002045A
			// (set) Token: 0x060007ED RID: 2029 RVA: 0x00022262 File Offset: 0x00020462
			public object Parent { get; set; }

			// Token: 0x170001C7 RID: 455
			// (get) Token: 0x060007EE RID: 2030 RVA: 0x0002226B File Offset: 0x0002046B
			// (set) Token: 0x060007EF RID: 2031 RVA: 0x00022273 File Offset: 0x00020473
			public string ParentProperty { get; set; }
		}
	}
}
