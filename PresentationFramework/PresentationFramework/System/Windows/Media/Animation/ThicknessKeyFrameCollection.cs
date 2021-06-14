using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace System.Windows.Media.Animation
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> objects. </summary>
	// Token: 0x02000192 RID: 402
	public class ThicknessKeyFrameCollection : Freezable, IList, ICollection, IEnumerable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" /> class.</summary>
		// Token: 0x06001778 RID: 6008 RVA: 0x000730DA File Offset: 0x000712DA
		public ThicknessKeyFrameCollection()
		{
			this._keyFrames = new List<ThicknessKeyFrame>(2);
		}

		/// <summary> Gets an empty <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" />.  </summary>
		/// <returns>An empty <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" />.</returns>
		// Token: 0x17000545 RID: 1349
		// (get) Token: 0x06001779 RID: 6009 RVA: 0x000730F0 File Offset: 0x000712F0
		public static ThicknessKeyFrameCollection Empty
		{
			get
			{
				if (ThicknessKeyFrameCollection.s_emptyCollection == null)
				{
					ThicknessKeyFrameCollection thicknessKeyFrameCollection = new ThicknessKeyFrameCollection();
					thicknessKeyFrameCollection._keyFrames = new List<ThicknessKeyFrame>(0);
					thicknessKeyFrameCollection.Freeze();
					ThicknessKeyFrameCollection.s_emptyCollection = thicknessKeyFrameCollection;
				}
				return ThicknessKeyFrameCollection.s_emptyCollection;
			}
		}

		/// <summary>Creates a modifiable clone of this <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" />, making deep copies of this object's values. When copying dependency properties, this method copies resource references and data bindings (but they might no longer resolve) but not animations or their current values.</summary>
		/// <returns>A modifiable clone of the current object. The cloned object's <see cref="P:System.Windows.Freezable.IsFrozen" /> property will be <see langword="false" /> even if the source's <see cref="P:System.Windows.Freezable.IsFrozen" /> property was <see langword="true." /></returns>
		// Token: 0x0600177A RID: 6010 RVA: 0x00073127 File Offset: 0x00071327
		public new ThicknessKeyFrameCollection Clone()
		{
			return (ThicknessKeyFrameCollection)base.Clone();
		}

		/// <summary>Creates a new, frozen instance of <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" />.</summary>
		/// <returns>A frozen instance of <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" />.</returns>
		// Token: 0x0600177B RID: 6011 RVA: 0x00073134 File Offset: 0x00071334
		protected override Freezable CreateInstanceCore()
		{
			return new ThicknessKeyFrameCollection();
		}

		/// <summary>Makes this instance a deep copy of the specified <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" />. When copying dependency properties, this method copies resource references and data bindings (but they might no longer resolve) but not animations or their current values.</summary>
		/// <param name="sourceFreezable">The <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" /> to clone.</param>
		// Token: 0x0600177C RID: 6012 RVA: 0x0007313C File Offset: 0x0007133C
		protected override void CloneCore(Freezable sourceFreezable)
		{
			ThicknessKeyFrameCollection thicknessKeyFrameCollection = (ThicknessKeyFrameCollection)sourceFreezable;
			base.CloneCore(sourceFreezable);
			int count = thicknessKeyFrameCollection._keyFrames.Count;
			this._keyFrames = new List<ThicknessKeyFrame>(count);
			for (int i = 0; i < count; i++)
			{
				ThicknessKeyFrame thicknessKeyFrame = (ThicknessKeyFrame)thicknessKeyFrameCollection._keyFrames[i].Clone();
				this._keyFrames.Add(thicknessKeyFrame);
				base.OnFreezablePropertyChanged(null, thicknessKeyFrame);
			}
		}

		/// <summary>Makes this instance a modifiable deep copy of the specified <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" /> using current property values. Resource references, data bindings, and animations are not copied, but their current values are.</summary>
		/// <param name="sourceFreezable">The <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" /> to clone.</param>
		// Token: 0x0600177D RID: 6013 RVA: 0x000731A8 File Offset: 0x000713A8
		protected override void CloneCurrentValueCore(Freezable sourceFreezable)
		{
			ThicknessKeyFrameCollection thicknessKeyFrameCollection = (ThicknessKeyFrameCollection)sourceFreezable;
			base.CloneCurrentValueCore(sourceFreezable);
			int count = thicknessKeyFrameCollection._keyFrames.Count;
			this._keyFrames = new List<ThicknessKeyFrame>(count);
			for (int i = 0; i < count; i++)
			{
				ThicknessKeyFrame thicknessKeyFrame = (ThicknessKeyFrame)thicknessKeyFrameCollection._keyFrames[i].CloneCurrentValue();
				this._keyFrames.Add(thicknessKeyFrame);
				base.OnFreezablePropertyChanged(null, thicknessKeyFrame);
			}
		}

		/// <summary>Makes this instance a clone of the specified <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" /> object. </summary>
		/// <param name="sourceFreezable">The <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" /> object to clone.</param>
		// Token: 0x0600177E RID: 6014 RVA: 0x00073214 File Offset: 0x00071414
		protected override void GetAsFrozenCore(Freezable sourceFreezable)
		{
			ThicknessKeyFrameCollection thicknessKeyFrameCollection = (ThicknessKeyFrameCollection)sourceFreezable;
			base.GetAsFrozenCore(sourceFreezable);
			int count = thicknessKeyFrameCollection._keyFrames.Count;
			this._keyFrames = new List<ThicknessKeyFrame>(count);
			for (int i = 0; i < count; i++)
			{
				ThicknessKeyFrame thicknessKeyFrame = (ThicknessKeyFrame)thicknessKeyFrameCollection._keyFrames[i].GetAsFrozen();
				this._keyFrames.Add(thicknessKeyFrame);
				base.OnFreezablePropertyChanged(null, thicknessKeyFrame);
			}
		}

		/// <summary>Makes this instance a modifiable deep copy of the specified <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" /> using current property values. Resource references, data bindings, and animations are not copied, but their current values are.</summary>
		/// <param name="sourceFreezable">The <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" /> to clone.</param>
		// Token: 0x0600177F RID: 6015 RVA: 0x00073280 File Offset: 0x00071480
		protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
		{
			ThicknessKeyFrameCollection thicknessKeyFrameCollection = (ThicknessKeyFrameCollection)sourceFreezable;
			base.GetCurrentValueAsFrozenCore(sourceFreezable);
			int count = thicknessKeyFrameCollection._keyFrames.Count;
			this._keyFrames = new List<ThicknessKeyFrame>(count);
			for (int i = 0; i < count; i++)
			{
				ThicknessKeyFrame thicknessKeyFrame = (ThicknessKeyFrame)thicknessKeyFrameCollection._keyFrames[i].GetCurrentValueAsFrozen();
				this._keyFrames.Add(thicknessKeyFrame);
				base.OnFreezablePropertyChanged(null, thicknessKeyFrame);
			}
		}

		/// <summary>Makes this instance of <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" /> read-only or determines whether it can be made read-only.</summary>
		/// <param name="isChecking">
		///       <see langword="true" /> to check if this instance can be frozen; <see langword="false" /> to freeze this instance. </param>
		/// <returns>If <paramref name="isChecking" /> is true, this method returns <see langword="true" /> if this instance can be made read-only, or <see langword="false" /> if it cannot be made read-only. If <paramref name="isChecking" /> is false, this method returns <see langword="true" /> if this instance is now read-only, or <see langword="false" /> if it cannot be made read-only, with the side effect of having begun to change the frozen status of this object.</returns>
		// Token: 0x06001780 RID: 6016 RVA: 0x000732EC File Offset: 0x000714EC
		protected override bool FreezeCore(bool isChecking)
		{
			bool flag = base.FreezeCore(isChecking);
			int num = 0;
			while (num < this._keyFrames.Count && flag)
			{
				flag &= Freezable.Freeze(this._keyFrames[num], isChecking);
				num++;
			}
			return flag;
		}

		/// <summary> Returns an enumerator that can iterate through the collection. </summary>
		/// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can iterate through the collection.</returns>
		// Token: 0x06001781 RID: 6017 RVA: 0x00073331 File Offset: 0x00071531
		public IEnumerator GetEnumerator()
		{
			base.ReadPreamble();
			return this._keyFrames.GetEnumerator();
		}

		/// <summary>Gets the number of key frames contained in the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" />.</summary>
		/// <returns>The number of key frames contained in the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrameCollection" />. </returns>
		// Token: 0x17000546 RID: 1350
		// (get) Token: 0x06001782 RID: 6018 RVA: 0x00073349 File Offset: 0x00071549
		public int Count
		{
			get
			{
				base.ReadPreamble();
				return this._keyFrames.Count;
			}
		}

		/// <summary>Gets a value that indicates whether access to the collection is synchronized (thread-safe). </summary>
		/// <returns>
		///     <see langword="true" /> if access to the collection is synchronized (thread-safe); otherwise, <see langword="false" />.</returns>
		// Token: 0x17000547 RID: 1351
		// (get) Token: 0x06001783 RID: 6019 RVA: 0x0007335C File Offset: 0x0007155C
		public bool IsSynchronized
		{
			get
			{
				base.ReadPreamble();
				return base.IsFrozen || base.Dispatcher != null;
			}
		}

		/// <summary> Gets an object that can be used to synchronize access to the collection. </summary>
		/// <returns>An object that can be used to synchronize access to the collection.</returns>
		// Token: 0x17000548 RID: 1352
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x00073377 File Offset: 0x00071577
		public object SyncRoot
		{
			get
			{
				base.ReadPreamble();
				return ((ICollection)this._keyFrames).SyncRoot;
			}
		}

		/// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing. </param>
		/// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
		// Token: 0x06001785 RID: 6021 RVA: 0x0007338A File Offset: 0x0007158A
		void ICollection.CopyTo(Array array, int index)
		{
			base.ReadPreamble();
			((ICollection)this._keyFrames).CopyTo(array, index);
		}

		/// <summary>Copies all of the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> objects in a collection to a specified array. </summary>
		/// <param name="array">Identifies the array to which content is copied.</param>
		/// <param name="index">Index position in the array to which the contents of the collection are copied.</param>
		// Token: 0x06001786 RID: 6022 RVA: 0x0007339F File Offset: 0x0007159F
		public void CopyTo(ThicknessKeyFrame[] array, int index)
		{
			base.ReadPreamble();
			this._keyFrames.CopyTo(array, index);
		}

		/// <summary>Adds an item to the <see cref="T:System.Collections.IList" />.</summary>
		/// <param name="keyFrame">The object to add to the <see cref="T:System.Collections.IList" />.    </param>
		/// <returns>The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection.</returns>
		// Token: 0x06001787 RID: 6023 RVA: 0x000733B4 File Offset: 0x000715B4
		int IList.Add(object keyFrame)
		{
			return this.Add((ThicknessKeyFrame)keyFrame);
		}

		/// <summary>Adds a <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> to the end of the collection. </summary>
		/// <param name="keyFrame">The <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> to add to the end of the collection.</param>
		/// <returns>The index at which the <paramref name="keyFrame" /> was added.</returns>
		// Token: 0x06001788 RID: 6024 RVA: 0x000733C2 File Offset: 0x000715C2
		public int Add(ThicknessKeyFrame keyFrame)
		{
			if (keyFrame == null)
			{
				throw new ArgumentNullException("keyFrame");
			}
			base.WritePreamble();
			base.OnFreezablePropertyChanged(null, keyFrame);
			this._keyFrames.Add(keyFrame);
			base.WritePostscript();
			return this._keyFrames.Count - 1;
		}

		/// <summary>Removes all <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> objects from the collection. </summary>
		// Token: 0x06001789 RID: 6025 RVA: 0x00073400 File Offset: 0x00071600
		public void Clear()
		{
			base.WritePreamble();
			if (this._keyFrames.Count > 0)
			{
				for (int i = 0; i < this._keyFrames.Count; i++)
				{
					base.OnFreezablePropertyChanged(this._keyFrames[i], null);
				}
				this._keyFrames.Clear();
				base.WritePostscript();
			}
		}

		/// <summary>Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value.</summary>
		/// <param name="keyFrame">The object to locate in the <see cref="T:System.Collections.IList" />.    </param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="T:System.Object" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600178A RID: 6026 RVA: 0x0007345B File Offset: 0x0007165B
		bool IList.Contains(object keyFrame)
		{
			return this.Contains((ThicknessKeyFrame)keyFrame);
		}

		/// <summary>Gets a value that indicates whether the collection contains the specified <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" />. </summary>
		/// <param name="keyFrame">The <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> to locate in the collection.</param>
		/// <returns>
		///     <see langword="true" /> if the collection contains <paramref name="keyFrame" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x0600178B RID: 6027 RVA: 0x00073469 File Offset: 0x00071669
		public bool Contains(ThicknessKeyFrame keyFrame)
		{
			base.ReadPreamble();
			return this._keyFrames.Contains(keyFrame);
		}

		/// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.</summary>
		/// <param name="keyFrame">The object to locate in the <see cref="T:System.Collections.IList" />.</param>
		/// <returns>The index of <paramref name="value" /> if found in the list; otherwise, -1.</returns>
		// Token: 0x0600178C RID: 6028 RVA: 0x0007347D File Offset: 0x0007167D
		int IList.IndexOf(object keyFrame)
		{
			return this.IndexOf((ThicknessKeyFrame)keyFrame);
		}

		/// <summary> Searches for the specified <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> and returns the zero-based index of the first occurrence within the entire collection.</summary>
		/// <param name="keyFrame">The <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> to locate in the collection.</param>
		/// <returns>The zero-based index of the first occurrence of <paramref name="keyFrame" /> within the entire collection, if found; otherwise, -1.</returns>
		// Token: 0x0600178D RID: 6029 RVA: 0x0007348B File Offset: 0x0007168B
		public int IndexOf(ThicknessKeyFrame keyFrame)
		{
			base.ReadPreamble();
			return this._keyFrames.IndexOf(keyFrame);
		}

		/// <summary>Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.</summary>
		/// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted. </param>
		/// <param name="keyFrame">The object to insert into the <see cref="T:System.Collections.IList" />. </param>
		// Token: 0x0600178E RID: 6030 RVA: 0x0007349F File Offset: 0x0007169F
		void IList.Insert(int index, object keyFrame)
		{
			this.Insert(index, (ThicknessKeyFrame)keyFrame);
		}

		/// <summary>Inserts a <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> into a specific location within the collection. </summary>
		/// <param name="index">The index position at which the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> is inserted.</param>
		/// <param name="keyFrame">The <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> object to insert in the collection.</param>
		// Token: 0x0600178F RID: 6031 RVA: 0x000734AE File Offset: 0x000716AE
		public void Insert(int index, ThicknessKeyFrame keyFrame)
		{
			if (keyFrame == null)
			{
				throw new ArgumentNullException("keyFrame");
			}
			base.WritePreamble();
			base.OnFreezablePropertyChanged(null, keyFrame);
			this._keyFrames.Insert(index, keyFrame);
			base.WritePostscript();
		}

		/// <summary>Gets a value that indicates if the collection size can ever change.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is frozen; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001790 RID: 6032 RVA: 0x000734DF File Offset: 0x000716DF
		public bool IsFixedSize
		{
			get
			{
				base.ReadPreamble();
				return base.IsFrozen;
			}
		}

		/// <summary> Gets a value that indicates if the collection is read-only.</summary>
		/// <returns>
		///     <see langword="true" /> if the collection is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001791 RID: 6033 RVA: 0x000734DF File Offset: 0x000716DF
		public bool IsReadOnly
		{
			get
			{
				base.ReadPreamble();
				return base.IsFrozen;
			}
		}

		/// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.</summary>
		/// <param name="keyFrame">The object to remove from the <see cref="T:System.Collections.IList" />.</param>
		// Token: 0x06001792 RID: 6034 RVA: 0x000734ED File Offset: 0x000716ED
		void IList.Remove(object keyFrame)
		{
			this.Remove((ThicknessKeyFrame)keyFrame);
		}

		/// <summary>Removes a <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> object from the collection. </summary>
		/// <param name="keyFrame">Identifies the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> to remove from the collection.</param>
		// Token: 0x06001793 RID: 6035 RVA: 0x000734FB File Offset: 0x000716FB
		public void Remove(ThicknessKeyFrame keyFrame)
		{
			base.WritePreamble();
			if (this._keyFrames.Contains(keyFrame))
			{
				base.OnFreezablePropertyChanged(keyFrame, null);
				this._keyFrames.Remove(keyFrame);
				base.WritePostscript();
			}
		}

		/// <summary>Removes the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> at the specified index position from the collection. </summary>
		/// <param name="index">Index position of the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> to be removed.</param>
		// Token: 0x06001794 RID: 6036 RVA: 0x0007352C File Offset: 0x0007172C
		public void RemoveAt(int index)
		{
			base.WritePreamble();
			base.OnFreezablePropertyChanged(this._keyFrames[index], null);
			this._keyFrames.RemoveAt(index);
			base.WritePostscript();
		}

		/// <summary>Gets or sets the element at the specified index.</summary>
		/// <param name="index">The zero-based index of the element to get or set.    </param>
		/// <returns>The element at the specified index.</returns>
		// Token: 0x1700054B RID: 1355
		object IList.this[int index]
		{
			get
			{
				return this[index];
			}
			set
			{
				this[index] = (ThicknessKeyFrame)value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> at the specified index position.  </summary>
		/// <param name="index">The zero-based index of the <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> to get or set.</param>
		/// <returns>The <see cref="T:System.Windows.Media.Animation.ThicknessKeyFrame" /> at the specified index.</returns>
		/// <exception cref="T:System.InvalidOperationException">The attempt to modify the collection is invalid because the collection is frozen (its <see cref="P:System.Windows.Freezable.IsFrozen" /> property is true).</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="index" /> is less than zero.-or-
		///         <paramref name="index" /> is equal to or greater than <see cref="P:System.Windows.Media.Animation.ThicknessKeyFrameCollection.Count" />.</exception>
		// Token: 0x1700054C RID: 1356
		public ThicknessKeyFrame this[int index]
		{
			get
			{
				base.ReadPreamble();
				return this._keyFrames[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(string.Format(CultureInfo.InvariantCulture, "ThicknessKeyFrameCollection[{0}]", new object[]
					{
						index
					}));
				}
				base.WritePreamble();
				if (value != this._keyFrames[index])
				{
					base.OnFreezablePropertyChanged(this._keyFrames[index], value);
					this._keyFrames[index] = value;
					base.WritePostscript();
				}
			}
		}

		// Token: 0x040012BD RID: 4797
		private List<ThicknessKeyFrame> _keyFrames;

		// Token: 0x040012BE RID: 4798
		private static ThicknessKeyFrameCollection s_emptyCollection;
	}
}
