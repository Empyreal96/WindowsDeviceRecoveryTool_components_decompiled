using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Data.Services.Client
{
	// Token: 0x020000E9 RID: 233
	internal sealed class BindingGraph
	{
		// Token: 0x06000781 RID: 1921 RVA: 0x000203B7 File Offset: 0x0001E5B7
		public BindingGraph(BindingObserver observer)
		{
			this.observer = observer;
			this.graph = new BindingGraph.Graph();
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x000203D4 File Offset: 0x0001E5D4
		[MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
		public bool AddDataServiceCollection(object source, string sourceProperty, object collection, string collectionEntitySet)
		{
			if (this.graph.ExistsVertex(collection))
			{
				return false;
			}
			BindingGraph.Vertex vertex = this.graph.AddVertex(collection);
			vertex.IsDataServiceCollection = true;
			vertex.EntitySet = collectionEntitySet;
			ICollection collection2 = collection as ICollection;
			if (source != null)
			{
				vertex.Parent = this.graph.LookupVertex(source);
				vertex.ParentProperty = sourceProperty;
				this.graph.AddEdge(source, collection, sourceProperty);
				Type collectionEntityType = BindingUtils.GetCollectionEntityType(collection.GetType());
				if (!typeof(INotifyPropertyChanged).IsAssignableFrom(collectionEntityType))
				{
					throw new InvalidOperationException(Strings.DataBinding_NotifyPropertyChangedNotImpl(collectionEntityType));
				}
				typeof(BindingGraph).GetMethod("SetObserver", false, false).MakeGenericMethod(new Type[]
				{
					collectionEntityType
				}).Invoke(this, new object[]
				{
					collection2
				});
			}
			else
			{
				this.graph.Root = vertex;
			}
			this.AttachDataServiceCollectionNotification(collection);
			foreach (object target in collection2)
			{
				this.AddEntity(source, sourceProperty, target, collectionEntitySet, collection);
			}
			return true;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00020514 File Offset: 0x0001E714
		public void AddPrimitiveOrComplexCollection(object source, string sourceProperty, object collection, Type collectionItemType)
		{
			BindingGraph.Vertex parent = this.graph.LookupVertex(source);
			if (this.graph.LookupVertex(collection) != null)
			{
				throw new InvalidOperationException(Strings.DataBinding_CollectionAssociatedWithMultipleEntities(collection.GetType()));
			}
			BindingGraph.Vertex vertex = this.graph.AddVertex(collection);
			vertex.Parent = parent;
			vertex.ParentProperty = sourceProperty;
			vertex.IsPrimitiveOrComplexCollection = true;
			vertex.PrimitiveOrComplexCollectionItemType = collectionItemType;
			this.graph.AddEdge(source, collection, sourceProperty);
			if (!this.AttachPrimitiveOrComplexCollectionNotification(collection))
			{
				throw new InvalidOperationException(Strings.DataBinding_NotifyCollectionChangedNotImpl(collection.GetType()));
			}
			if (PrimitiveType.IsKnownNullableType(collectionItemType))
			{
				return;
			}
			if (!typeof(INotifyPropertyChanged).IsAssignableFrom(collectionItemType))
			{
				throw new InvalidOperationException(Strings.DataBinding_NotifyPropertyChangedNotImpl(collectionItemType));
			}
			this.AddComplexObjectsFromCollection(collection, (IEnumerable)collection);
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x000205DC File Offset: 0x0001E7DC
		public bool AddEntity(object source, string sourceProperty, object target, string targetEntitySet, object edgeSource)
		{
			BindingGraph.Vertex vertex = this.graph.LookupVertex(edgeSource);
			BindingGraph.Vertex vertex2 = null;
			bool flag = false;
			if (target != null)
			{
				vertex2 = this.graph.LookupVertex(target);
				if (vertex2 == null)
				{
					vertex2 = this.graph.AddVertex(target);
					vertex2.EntitySet = BindingEntityInfo.GetEntitySet(target, targetEntitySet, this.observer.Context.Model);
					if (!this.AttachEntityOrComplexObjectNotification(target))
					{
						throw new InvalidOperationException(Strings.DataBinding_NotifyPropertyChangedNotImpl(target.GetType()));
					}
					flag = true;
				}
				if (this.graph.ExistsEdge(edgeSource, target, vertex.IsDataServiceCollection ? null : sourceProperty))
				{
					throw new InvalidOperationException(Strings.DataBinding_EntityAlreadyInCollection(target.GetType()));
				}
				this.graph.AddEdge(edgeSource, target, vertex.IsDataServiceCollection ? null : sourceProperty);
			}
			if (!vertex.IsDataServiceCollection)
			{
				this.observer.HandleUpdateEntityReference(source, sourceProperty, vertex.EntitySet, target, (vertex2 == null) ? null : vertex2.EntitySet);
			}
			else
			{
				this.observer.HandleAddEntity(source, sourceProperty, (vertex.Parent != null) ? vertex.Parent.EntitySet : null, edgeSource as ICollection, target, vertex2.EntitySet);
			}
			if (flag)
			{
				this.AddFromProperties(target);
			}
			return flag;
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x00020728 File Offset: 0x0001E928
		public void RemoveDataServiceCollectionItem(object item, object parent, string parentProperty)
		{
			if (this.graph.LookupVertex(item) == null)
			{
				return;
			}
			if (parentProperty != null)
			{
				BindingEntityInfo.BindingPropertyInfo bindingPropertyInfo = BindingEntityInfo.GetObservableProperties(parent.GetType(), this.observer.Context.Model).Single((BindingEntityInfo.BindingPropertyInfo p) => p.PropertyInfo.PropertyName == parentProperty);
				parent = bindingPropertyInfo.PropertyInfo.GetValue(parent);
			}
			object source = null;
			string sourceProperty = null;
			string sourceEntitySet = null;
			string targetEntitySet = null;
			this.GetDataServiceCollectionInfo(parent, out source, out sourceProperty, out sourceEntitySet, out targetEntitySet);
			targetEntitySet = BindingEntityInfo.GetEntitySet(item, targetEntitySet, this.observer.Context.Model);
			this.observer.HandleDeleteEntity(source, sourceProperty, sourceEntitySet, parent as ICollection, item, targetEntitySet);
			this.graph.RemoveEdge(parent, item, null);
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x00020800 File Offset: 0x0001EA00
		public void RemoveComplexTypeCollectionItem(object item, object collection)
		{
			if (item == null)
			{
				return;
			}
			if (this.graph.LookupVertex(item) == null)
			{
				return;
			}
			this.graph.RemoveEdge(collection, item, null);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00020830 File Offset: 0x0001EA30
		public void RemoveCollection(object source)
		{
			BindingGraph.Vertex vertex = this.graph.LookupVertex(source);
			foreach (BindingGraph.Edge edge in vertex.OutgoingEdges.ToList<BindingGraph.Edge>())
			{
				this.graph.RemoveEdge(source, edge.Target.Item, null);
			}
			this.RemoveUnreachableVertices();
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x000208DC File Offset: 0x0001EADC
		public void RemoveRelation(object source, string relation)
		{
			BindingGraph.Edge edge = this.graph.LookupVertex(source).OutgoingEdges.SingleOrDefault((BindingGraph.Edge e) => e.Source.Item == source && e.Label == relation);
			if (edge != null)
			{
				this.graph.RemoveEdge(edge.Source.Item, edge.Target.Item, edge.Label);
			}
			this.RemoveUnreachableVertices();
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00020988 File Offset: 0x0001EB88
		public void RemoveNonTrackedEntities()
		{
			foreach (object item in from o in this.graph
			select BindingEntityInfo.IsEntityType(o.GetType(), this.observer.Context.Model) && !this.observer.IsContextTrackingEntity(o))
			{
				this.graph.ClearEdgesForVertex(this.graph.LookupVertex(item));
			}
			this.RemoveUnreachableVertices();
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x00020BC8 File Offset: 0x0001EDC8
		public IEnumerable<object> GetDataServiceCollectionItems(object collection)
		{
			BindingGraph.Vertex collectionVertex = this.graph.LookupVertex(collection);
			foreach (BindingGraph.Edge collectionEdge in collectionVertex.OutgoingEdges.ToList<BindingGraph.Edge>())
			{
				yield return collectionEdge.Target.Item;
			}
			yield break;
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00020BEC File Offset: 0x0001EDEC
		public void Reset()
		{
			this.graph.Reset(new Action<object>(this.DetachNotifications));
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x00020C05 File Offset: 0x0001EE05
		public void RemoveUnreachableVertices()
		{
			this.graph.RemoveUnreachableVertices(new Action<object>(this.DetachNotifications));
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00020C1E File Offset: 0x0001EE1E
		public void GetDataServiceCollectionInfo(object collection, out object source, out string sourceProperty, out string sourceEntitySet, out string targetEntitySet)
		{
			this.graph.LookupVertex(collection).GetDataServiceCollectionInfo(out source, out sourceProperty, out sourceEntitySet, out targetEntitySet);
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x00020C37 File Offset: 0x0001EE37
		public void GetPrimitiveOrComplexCollectionInfo(object collection, out object source, out string sourceProperty, out Type collectionItemType)
		{
			this.graph.LookupVertex(collection).GetPrimitiveOrComplexCollectionInfo(out source, out sourceProperty, out collectionItemType);
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00020C50 File Offset: 0x0001EE50
		public void GetAncestorEntityForComplexProperty(ref object entity, ref string propertyName, ref object propertyValue)
		{
			BindingGraph.Vertex vertex = this.graph.LookupVertex(entity);
			while (vertex.IsComplex || vertex.IsPrimitiveOrComplexCollection)
			{
				propertyName = vertex.IncomingEdges[0].Label;
				propertyValue = vertex.Item;
				entity = vertex.Parent.Item;
				vertex = vertex.Parent;
			}
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00020CAC File Offset: 0x0001EEAC
		public void AddComplexObject(object source, string sourceProperty, object target)
		{
			if (this.graph.LookupVertex(target) != null)
			{
				throw new InvalidOperationException(Strings.DataBinding_ComplexObjectAssociatedWithMultipleEntities(target.GetType()));
			}
			BindingGraph.Vertex parent = this.graph.LookupVertex(source);
			BindingGraph.Vertex vertex = this.graph.AddVertex(target);
			vertex.Parent = parent;
			vertex.IsComplex = true;
			if (!this.AttachEntityOrComplexObjectNotification(target))
			{
				throw new InvalidOperationException(Strings.DataBinding_NotifyPropertyChangedNotImpl(target.GetType()));
			}
			this.graph.AddEdge(source, target, sourceProperty);
			this.AddFromProperties(target);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00020D34 File Offset: 0x0001EF34
		public void AddComplexObjectsFromCollection(object collection, IEnumerable collectionItems)
		{
			foreach (object obj in collectionItems)
			{
				if (obj != null)
				{
					this.AddComplexObject(collection, null, obj);
				}
			}
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00020D88 File Offset: 0x0001EF88
		private void AddFromProperties(object entity)
		{
			foreach (BindingEntityInfo.BindingPropertyInfo bindingPropertyInfo in BindingEntityInfo.GetObservableProperties(entity.GetType(), this.observer.Context.Model))
			{
				object value = bindingPropertyInfo.PropertyInfo.GetValue(entity);
				if (value != null)
				{
					switch (bindingPropertyInfo.PropertyKind)
					{
					case BindingPropertyKind.BindingPropertyKindEntity:
						this.AddEntity(entity, bindingPropertyInfo.PropertyInfo.PropertyName, value, null, entity);
						break;
					case BindingPropertyKind.BindingPropertyKindDataServiceCollection:
						this.AddDataServiceCollection(entity, bindingPropertyInfo.PropertyInfo.PropertyName, value, null);
						break;
					case BindingPropertyKind.BindingPropertyKindPrimitiveOrComplexCollection:
						this.AddPrimitiveOrComplexCollection(entity, bindingPropertyInfo.PropertyInfo.PropertyName, value, bindingPropertyInfo.PropertyInfo.PrimitiveOrComplexCollectionItemType);
						break;
					default:
						this.AddComplexObject(entity, bindingPropertyInfo.PropertyInfo.PropertyName, value);
						break;
					}
				}
			}
		}

		// Token: 0x06000793 RID: 1939 RVA: 0x00020E78 File Offset: 0x0001F078
		private void AttachDataServiceCollectionNotification(object target)
		{
			INotifyCollectionChanged notifyCollectionChanged = target as INotifyCollectionChanged;
			notifyCollectionChanged.CollectionChanged -= this.observer.OnDataServiceCollectionChanged;
			notifyCollectionChanged.CollectionChanged += this.observer.OnDataServiceCollectionChanged;
		}

		// Token: 0x06000794 RID: 1940 RVA: 0x00020EBC File Offset: 0x0001F0BC
		private bool AttachPrimitiveOrComplexCollectionNotification(object collection)
		{
			INotifyCollectionChanged notifyCollectionChanged = collection as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= this.observer.OnPrimitiveOrComplexCollectionChanged;
				notifyCollectionChanged.CollectionChanged += this.observer.OnPrimitiveOrComplexCollectionChanged;
				return true;
			}
			return false;
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00020F04 File Offset: 0x0001F104
		private bool AttachEntityOrComplexObjectNotification(object target)
		{
			INotifyPropertyChanged notifyPropertyChanged = target as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged -= this.observer.OnPropertyChanged;
				notifyPropertyChanged.PropertyChanged += this.observer.OnPropertyChanged;
				return true;
			}
			return false;
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x00020F4C File Offset: 0x0001F14C
		private void DetachNotifications(object target)
		{
			this.DetachCollectionNotifications(target);
			INotifyPropertyChanged notifyPropertyChanged = target as INotifyPropertyChanged;
			if (notifyPropertyChanged != null)
			{
				notifyPropertyChanged.PropertyChanged -= this.observer.OnPropertyChanged;
			}
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x00020F84 File Offset: 0x0001F184
		private void DetachCollectionNotifications(object target)
		{
			INotifyCollectionChanged notifyCollectionChanged = target as INotifyCollectionChanged;
			if (notifyCollectionChanged != null)
			{
				notifyCollectionChanged.CollectionChanged -= this.observer.OnDataServiceCollectionChanged;
				notifyCollectionChanged.CollectionChanged -= this.observer.OnPrimitiveOrComplexCollectionChanged;
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x00020FCC File Offset: 0x0001F1CC
		private void SetObserver<T>(ICollection collection)
		{
			DataServiceCollection<T> dataServiceCollection = collection as DataServiceCollection<T>;
			dataServiceCollection.Observer = this.observer;
		}

		// Token: 0x040004A8 RID: 1192
		private BindingObserver observer;

		// Token: 0x040004A9 RID: 1193
		private BindingGraph.Graph graph;

		// Token: 0x020000EA RID: 234
		internal sealed class Graph
		{
			// Token: 0x0600079A RID: 1946 RVA: 0x00020FEC File Offset: 0x0001F1EC
			public Graph()
			{
				this.vertices = new Dictionary<object, BindingGraph.Vertex>(ReferenceEqualityComparer<object>.Instance);
			}

			// Token: 0x170001B0 RID: 432
			// (get) Token: 0x0600079B RID: 1947 RVA: 0x00021004 File Offset: 0x0001F204
			// (set) Token: 0x0600079C RID: 1948 RVA: 0x0002100C File Offset: 0x0001F20C
			public BindingGraph.Vertex Root
			{
				get
				{
					return this.root;
				}
				set
				{
					this.root = value;
				}
			}

			// Token: 0x0600079D RID: 1949 RVA: 0x00021018 File Offset: 0x0001F218
			public BindingGraph.Vertex AddVertex(object item)
			{
				BindingGraph.Vertex vertex = new BindingGraph.Vertex(item);
				this.vertices.Add(item, vertex);
				return vertex;
			}

			// Token: 0x0600079E RID: 1950 RVA: 0x0002103C File Offset: 0x0001F23C
			public void ClearEdgesForVertex(BindingGraph.Vertex v)
			{
				foreach (BindingGraph.Edge edge in v.OutgoingEdges.Concat(v.IncomingEdges).ToList<BindingGraph.Edge>())
				{
					this.RemoveEdge(edge.Source.Item, edge.Target.Item, edge.Label);
				}
			}

			// Token: 0x0600079F RID: 1951 RVA: 0x000210BC File Offset: 0x0001F2BC
			public bool ExistsVertex(object item)
			{
				BindingGraph.Vertex vertex;
				return this.vertices.TryGetValue(item, out vertex);
			}

			// Token: 0x060007A0 RID: 1952 RVA: 0x000210D8 File Offset: 0x0001F2D8
			public BindingGraph.Vertex LookupVertex(object item)
			{
				BindingGraph.Vertex result;
				this.vertices.TryGetValue(item, out result);
				return result;
			}

			// Token: 0x060007A1 RID: 1953 RVA: 0x000210F8 File Offset: 0x0001F2F8
			public BindingGraph.Edge AddEdge(object source, object target, string label)
			{
				BindingGraph.Vertex vertex = this.vertices[source];
				BindingGraph.Vertex vertex2 = this.vertices[target];
				BindingGraph.Edge edge = new BindingGraph.Edge
				{
					Source = vertex,
					Target = vertex2,
					Label = label
				};
				vertex.OutgoingEdges.Add(edge);
				vertex2.IncomingEdges.Add(edge);
				return edge;
			}

			// Token: 0x060007A2 RID: 1954 RVA: 0x00021158 File Offset: 0x0001F358
			public void RemoveEdge(object source, object target, string label)
			{
				BindingGraph.Vertex vertex = this.vertices[source];
				BindingGraph.Vertex vertex2 = this.vertices[target];
				BindingGraph.Edge item = new BindingGraph.Edge
				{
					Source = vertex,
					Target = vertex2,
					Label = label
				};
				vertex.OutgoingEdges.Remove(item);
				vertex2.IncomingEdges.Remove(item);
			}

			// Token: 0x060007A3 RID: 1955 RVA: 0x000211CC File Offset: 0x0001F3CC
			public bool ExistsEdge(object source, object target, string label)
			{
				BindingGraph.Edge e = new BindingGraph.Edge
				{
					Source = this.vertices[source],
					Target = this.vertices[target],
					Label = label
				};
				return this.vertices[source].OutgoingEdges.Any((BindingGraph.Edge r) => r.Equals(e));
			}

			// Token: 0x060007A4 RID: 1956 RVA: 0x00021239 File Offset: 0x0001F439
			public IList<object> Select(Func<object, bool> filter)
			{
				return this.vertices.Keys.Where(filter).ToList<object>();
			}

			// Token: 0x060007A5 RID: 1957 RVA: 0x00021254 File Offset: 0x0001F454
			public void Reset(Action<object> action)
			{
				foreach (object obj in this.vertices.Keys)
				{
					action(obj);
				}
				this.vertices.Clear();
			}

			// Token: 0x060007A6 RID: 1958 RVA: 0x000212B8 File Offset: 0x0001F4B8
			public void RemoveUnreachableVertices(Action<object> detachAction)
			{
				try
				{
					foreach (BindingGraph.Vertex vertex in this.UnreachableVertices())
					{
						this.ClearEdgesForVertex(vertex);
						detachAction(vertex.Item);
						this.vertices.Remove(vertex.Item);
					}
				}
				finally
				{
					foreach (BindingGraph.Vertex vertex2 in this.vertices.Values)
					{
						vertex2.Color = VertexColor.White;
					}
				}
			}

			// Token: 0x060007A7 RID: 1959 RVA: 0x00021388 File Offset: 0x0001F588
			private IEnumerable<BindingGraph.Vertex> UnreachableVertices()
			{
				Queue<BindingGraph.Vertex> queue = new Queue<BindingGraph.Vertex>();
				this.Root.Color = VertexColor.Gray;
				queue.Enqueue(this.Root);
				while (queue.Count != 0)
				{
					BindingGraph.Vertex vertex = queue.Dequeue();
					foreach (BindingGraph.Edge edge in vertex.OutgoingEdges)
					{
						if (edge.Target.Color == VertexColor.White)
						{
							edge.Target.Color = VertexColor.Gray;
							queue.Enqueue(edge.Target);
						}
					}
					vertex.Color = VertexColor.Black;
				}
				return (from v in this.vertices.Values
				where v.Color == VertexColor.White
				select v).ToList<BindingGraph.Vertex>();
			}

			// Token: 0x040004AA RID: 1194
			private Dictionary<object, BindingGraph.Vertex> vertices;

			// Token: 0x040004AB RID: 1195
			private BindingGraph.Vertex root;
		}

		// Token: 0x020000EB RID: 235
		internal sealed class Vertex
		{
			// Token: 0x060007A9 RID: 1961 RVA: 0x0002145C File Offset: 0x0001F65C
			public Vertex(object item)
			{
				this.Item = item;
				this.Color = VertexColor.White;
			}

			// Token: 0x170001B1 RID: 433
			// (get) Token: 0x060007AA RID: 1962 RVA: 0x00021472 File Offset: 0x0001F672
			// (set) Token: 0x060007AB RID: 1963 RVA: 0x0002147A File Offset: 0x0001F67A
			public object Item { get; private set; }

			// Token: 0x170001B2 RID: 434
			// (get) Token: 0x060007AC RID: 1964 RVA: 0x00021483 File Offset: 0x0001F683
			// (set) Token: 0x060007AD RID: 1965 RVA: 0x0002148B File Offset: 0x0001F68B
			public string EntitySet { get; set; }

			// Token: 0x170001B3 RID: 435
			// (get) Token: 0x060007AE RID: 1966 RVA: 0x00021494 File Offset: 0x0001F694
			// (set) Token: 0x060007AF RID: 1967 RVA: 0x0002149C File Offset: 0x0001F69C
			public bool IsDataServiceCollection { get; set; }

			// Token: 0x170001B4 RID: 436
			// (get) Token: 0x060007B0 RID: 1968 RVA: 0x000214A5 File Offset: 0x0001F6A5
			// (set) Token: 0x060007B1 RID: 1969 RVA: 0x000214AD File Offset: 0x0001F6AD
			public bool IsComplex { get; set; }

			// Token: 0x170001B5 RID: 437
			// (get) Token: 0x060007B2 RID: 1970 RVA: 0x000214B6 File Offset: 0x0001F6B6
			// (set) Token: 0x060007B3 RID: 1971 RVA: 0x000214BE File Offset: 0x0001F6BE
			public bool IsPrimitiveOrComplexCollection { get; set; }

			// Token: 0x170001B6 RID: 438
			// (get) Token: 0x060007B4 RID: 1972 RVA: 0x000214C7 File Offset: 0x0001F6C7
			// (set) Token: 0x060007B5 RID: 1973 RVA: 0x000214CF File Offset: 0x0001F6CF
			public Type PrimitiveOrComplexCollectionItemType { get; set; }

			// Token: 0x170001B7 RID: 439
			// (get) Token: 0x060007B6 RID: 1974 RVA: 0x000214D8 File Offset: 0x0001F6D8
			// (set) Token: 0x060007B7 RID: 1975 RVA: 0x000214E0 File Offset: 0x0001F6E0
			public BindingGraph.Vertex Parent { get; set; }

			// Token: 0x170001B8 RID: 440
			// (get) Token: 0x060007B8 RID: 1976 RVA: 0x000214E9 File Offset: 0x0001F6E9
			// (set) Token: 0x060007B9 RID: 1977 RVA: 0x000214F1 File Offset: 0x0001F6F1
			public string ParentProperty { get; set; }

			// Token: 0x170001B9 RID: 441
			// (get) Token: 0x060007BA RID: 1978 RVA: 0x000214FA File Offset: 0x0001F6FA
			public bool IsRootDataServiceCollection
			{
				get
				{
					return this.IsDataServiceCollection && this.Parent == null;
				}
			}

			// Token: 0x170001BA RID: 442
			// (get) Token: 0x060007BB RID: 1979 RVA: 0x0002150F File Offset: 0x0001F70F
			// (set) Token: 0x060007BC RID: 1980 RVA: 0x00021517 File Offset: 0x0001F717
			public VertexColor Color { get; set; }

			// Token: 0x170001BB RID: 443
			// (get) Token: 0x060007BD RID: 1981 RVA: 0x00021520 File Offset: 0x0001F720
			public IList<BindingGraph.Edge> IncomingEdges
			{
				get
				{
					if (this.incomingEdges == null)
					{
						this.incomingEdges = new List<BindingGraph.Edge>();
					}
					return this.incomingEdges;
				}
			}

			// Token: 0x170001BC RID: 444
			// (get) Token: 0x060007BE RID: 1982 RVA: 0x0002153B File Offset: 0x0001F73B
			public IList<BindingGraph.Edge> OutgoingEdges
			{
				get
				{
					if (this.outgoingEdges == null)
					{
						this.outgoingEdges = new List<BindingGraph.Edge>();
					}
					return this.outgoingEdges;
				}
			}

			// Token: 0x060007BF RID: 1983 RVA: 0x00021556 File Offset: 0x0001F756
			public void GetDataServiceCollectionInfo(out object source, out string sourceProperty, out string sourceEntitySet, out string targetEntitySet)
			{
				if (!this.IsRootDataServiceCollection)
				{
					source = this.Parent.Item;
					sourceProperty = this.ParentProperty;
					sourceEntitySet = this.Parent.EntitySet;
				}
				else
				{
					source = null;
					sourceProperty = null;
					sourceEntitySet = null;
				}
				targetEntitySet = this.EntitySet;
			}

			// Token: 0x060007C0 RID: 1984 RVA: 0x00021596 File Offset: 0x0001F796
			public void GetPrimitiveOrComplexCollectionInfo(out object source, out string sourceProperty, out Type collectionItemType)
			{
				source = this.Parent.Item;
				sourceProperty = this.ParentProperty;
				collectionItemType = this.PrimitiveOrComplexCollectionItemType;
			}

			// Token: 0x040004AD RID: 1197
			private List<BindingGraph.Edge> incomingEdges;

			// Token: 0x040004AE RID: 1198
			private List<BindingGraph.Edge> outgoingEdges;
		}

		// Token: 0x020000EC RID: 236
		internal sealed class Edge : IEquatable<BindingGraph.Edge>
		{
			// Token: 0x170001BD RID: 445
			// (get) Token: 0x060007C1 RID: 1985 RVA: 0x000215B5 File Offset: 0x0001F7B5
			// (set) Token: 0x060007C2 RID: 1986 RVA: 0x000215BD File Offset: 0x0001F7BD
			public BindingGraph.Vertex Source { get; set; }

			// Token: 0x170001BE RID: 446
			// (get) Token: 0x060007C3 RID: 1987 RVA: 0x000215C6 File Offset: 0x0001F7C6
			// (set) Token: 0x060007C4 RID: 1988 RVA: 0x000215CE File Offset: 0x0001F7CE
			public BindingGraph.Vertex Target { get; set; }

			// Token: 0x170001BF RID: 447
			// (get) Token: 0x060007C5 RID: 1989 RVA: 0x000215D7 File Offset: 0x0001F7D7
			// (set) Token: 0x060007C6 RID: 1990 RVA: 0x000215DF File Offset: 0x0001F7DF
			public string Label { get; set; }

			// Token: 0x060007C7 RID: 1991 RVA: 0x000215E8 File Offset: 0x0001F7E8
			public bool Equals(BindingGraph.Edge other)
			{
				return other != null && object.ReferenceEquals(this.Source, other.Source) && object.ReferenceEquals(this.Target, other.Target) && this.Label == other.Label;
			}
		}
	}
}
