using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Forms.Binding" /> objects for a control.</summary>
	// Token: 0x0200012A RID: 298
	[DefaultEvent("CollectionChanged")]
	public class BindingsCollection : BaseCollection
	{
		// Token: 0x06000845 RID: 2117 RVA: 0x0001906B File Offset: 0x0001726B
		internal BindingsCollection()
		{
		}

		/// <summary>Gets the total number of bindings in the collection.</summary>
		/// <returns>The total number of bindings in the collection.</returns>
		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000846 RID: 2118 RVA: 0x00019073 File Offset: 0x00017273
		public override int Count
		{
			get
			{
				if (this.list == null)
				{
					return 0;
				}
				return base.Count;
			}
		}

		/// <summary>Gets the bindings in the collection as an object.</summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> containing all of the collection members.</returns>
		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x00019085 File Offset: 0x00017285
		protected override ArrayList List
		{
			get
			{
				if (this.list == null)
				{
					this.list = new ArrayList();
				}
				return this.list;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.Binding" /> at the specified index.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Forms.Binding" /> to find. </param>
		/// <returns>The <see cref="T:System.Windows.Forms.Binding" /> at the specified index.</returns>
		/// <exception cref="T:System.IndexOutOfRangeException">The collection doesn't contain an item at the specified index. </exception>
		// Token: 0x1700026C RID: 620
		public Binding this[int index]
		{
			get
			{
				return (Binding)this.List[index];
			}
		}

		/// <summary>Adds the specified binding to the collection.</summary>
		/// <param name="binding">The <see cref="T:System.Windows.Forms.Binding" /> to add to the collection. </param>
		// Token: 0x06000849 RID: 2121 RVA: 0x000190B4 File Offset: 0x000172B4
		protected internal void Add(Binding binding)
		{
			CollectionChangeEventArgs collectionChangeEventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Add, binding);
			this.OnCollectionChanging(collectionChangeEventArgs);
			this.AddCore(binding);
			this.OnCollectionChanged(collectionChangeEventArgs);
		}

		/// <summary>Adds a <see cref="T:System.Windows.Forms.Binding" /> to the collection.</summary>
		/// <param name="dataBinding">The <see cref="T:System.Windows.Forms.Binding" /> to add to the collection.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="dataBinding" /> argument was <see langword="null" />. </exception>
		// Token: 0x0600084A RID: 2122 RVA: 0x000190DE File Offset: 0x000172DE
		protected virtual void AddCore(Binding dataBinding)
		{
			if (dataBinding == null)
			{
				throw new ArgumentNullException("dataBinding");
			}
			this.List.Add(dataBinding);
		}

		/// <summary>Occurs when the collection is about to change.</summary>
		// Token: 0x1400003B RID: 59
		// (add) Token: 0x0600084B RID: 2123 RVA: 0x000190FB File Offset: 0x000172FB
		// (remove) Token: 0x0600084C RID: 2124 RVA: 0x00019114 File Offset: 0x00017314
		[SRDescription("collectionChangingEventDescr")]
		public event CollectionChangeEventHandler CollectionChanging
		{
			add
			{
				this.onCollectionChanging = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChanging, value);
			}
			remove
			{
				this.onCollectionChanging = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChanging, value);
			}
		}

		/// <summary>Occurs when the collection has changed.</summary>
		// Token: 0x1400003C RID: 60
		// (add) Token: 0x0600084D RID: 2125 RVA: 0x0001912D File Offset: 0x0001732D
		// (remove) Token: 0x0600084E RID: 2126 RVA: 0x00019146 File Offset: 0x00017346
		[SRDescription("collectionChangedEventDescr")]
		public event CollectionChangeEventHandler CollectionChanged
		{
			add
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Combine(this.onCollectionChanged, value);
			}
			remove
			{
				this.onCollectionChanged = (CollectionChangeEventHandler)Delegate.Remove(this.onCollectionChanged, value);
			}
		}

		/// <summary>Clears the collection of binding objects.</summary>
		// Token: 0x0600084F RID: 2127 RVA: 0x00019160 File Offset: 0x00017360
		protected internal void Clear()
		{
			CollectionChangeEventArgs collectionChangeEventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Refresh, null);
			this.OnCollectionChanging(collectionChangeEventArgs);
			this.ClearCore();
			this.OnCollectionChanged(collectionChangeEventArgs);
		}

		/// <summary>Clears the collection of any members.</summary>
		// Token: 0x06000850 RID: 2128 RVA: 0x00019189 File Offset: 0x00017389
		protected virtual void ClearCore()
		{
			this.List.Clear();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingsCollection.CollectionChanging" /> event. </summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CollectionChangeEventArgs" /> that contains event data.</param>
		// Token: 0x06000851 RID: 2129 RVA: 0x00019196 File Offset: 0x00017396
		protected virtual void OnCollectionChanging(CollectionChangeEventArgs e)
		{
			if (this.onCollectionChanging != null)
			{
				this.onCollectionChanging(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.BindingsCollection.CollectionChanged" /> event.</summary>
		/// <param name="ccevent">A <see cref="T:System.ComponentModel.CollectionChangeEventArgs" /> that contains the event data. </param>
		// Token: 0x06000852 RID: 2130 RVA: 0x000191AD File Offset: 0x000173AD
		protected virtual void OnCollectionChanged(CollectionChangeEventArgs ccevent)
		{
			if (this.onCollectionChanged != null)
			{
				this.onCollectionChanged(this, ccevent);
			}
		}

		/// <summary>Deletes the specified binding from the collection.</summary>
		/// <param name="binding">The Binding to remove from the collection. </param>
		// Token: 0x06000853 RID: 2131 RVA: 0x000191C4 File Offset: 0x000173C4
		protected internal void Remove(Binding binding)
		{
			CollectionChangeEventArgs collectionChangeEventArgs = new CollectionChangeEventArgs(CollectionChangeAction.Remove, binding);
			this.OnCollectionChanging(collectionChangeEventArgs);
			this.RemoveCore(binding);
			this.OnCollectionChanged(collectionChangeEventArgs);
		}

		/// <summary>Deletes the binding from the collection at the specified index.</summary>
		/// <param name="index">The index of the <see cref="T:System.Windows.Forms.Binding" /> to remove. </param>
		// Token: 0x06000854 RID: 2132 RVA: 0x000191EE File Offset: 0x000173EE
		protected internal void RemoveAt(int index)
		{
			this.Remove(this[index]);
		}

		/// <summary>Removes the specified <see cref="T:System.Windows.Forms.Binding" /> from the collection.</summary>
		/// <param name="dataBinding">The <see cref="T:System.Windows.Forms.Binding" /> to remove. </param>
		// Token: 0x06000855 RID: 2133 RVA: 0x000191FD File Offset: 0x000173FD
		protected virtual void RemoveCore(Binding dataBinding)
		{
			this.List.Remove(dataBinding);
		}

		/// <summary>Gets a value that indicates whether the collection should be serialized.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection count is greater than zero; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000856 RID: 2134 RVA: 0x0001920B File Offset: 0x0001740B
		protected internal bool ShouldSerializeMyAll()
		{
			return this.Count > 0;
		}

		// Token: 0x04000620 RID: 1568
		private ArrayList list;

		// Token: 0x04000621 RID: 1569
		private CollectionChangeEventHandler onCollectionChanging;

		// Token: 0x04000622 RID: 1570
		private CollectionChangeEventHandler onCollectionChanged;
	}
}
