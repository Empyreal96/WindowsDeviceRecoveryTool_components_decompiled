using System;
using System.ComponentModel;
using MS.Internal.Annotations;

namespace System.Windows.Annotations
{
	/// <summary>Represents an object that identifies an item of content.</summary>
	// Token: 0x020005CF RID: 1487
	public abstract class ContentLocatorBase : INotifyPropertyChanged2, INotifyPropertyChanged, IOwnedObject
	{
		// Token: 0x06006320 RID: 25376 RVA: 0x0000326D File Offset: 0x0000146D
		internal ContentLocatorBase()
		{
		}

		/// <summary>Creates a modifiable deep copy clone of this <see cref="T:System.Windows.Annotations.ContentLocatorBase" />.</summary>
		/// <returns>A modifiable deep copy clone of this <see cref="T:System.Windows.Annotations.ContentLocatorBase" />.</returns>
		// Token: 0x06006321 RID: 25377
		public abstract object Clone();

		/// <summary>For a description of this member, see <see cref="E:System.ComponentModel.INotifyPropertyChanged.PropertyChanged" />.</summary>
		// Token: 0x1400012C RID: 300
		// (add) Token: 0x06006322 RID: 25378 RVA: 0x001BE126 File Offset: 0x001BC326
		// (remove) Token: 0x06006323 RID: 25379 RVA: 0x001BE12F File Offset: 0x001BC32F
		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add
			{
				this._propertyChanged += value;
			}
			remove
			{
				this._propertyChanged -= value;
			}
		}

		// Token: 0x06006324 RID: 25380 RVA: 0x001BE138 File Offset: 0x001BC338
		internal void FireLocatorChanged(string name)
		{
			if (this._propertyChanged != null)
			{
				this._propertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x170017C9 RID: 6089
		// (get) Token: 0x06006325 RID: 25381 RVA: 0x001BE154 File Offset: 0x001BC354
		// (set) Token: 0x06006326 RID: 25382 RVA: 0x001BE15C File Offset: 0x001BC35C
		bool IOwnedObject.Owned
		{
			get
			{
				return this._owned;
			}
			set
			{
				this._owned = value;
			}
		}

		// Token: 0x06006327 RID: 25383
		internal abstract ContentLocatorBase Merge(ContentLocatorBase other);

		// Token: 0x1400012D RID: 301
		// (add) Token: 0x06006328 RID: 25384 RVA: 0x001BE168 File Offset: 0x001BC368
		// (remove) Token: 0x06006329 RID: 25385 RVA: 0x001BE1A0 File Offset: 0x001BC3A0
		private event PropertyChangedEventHandler _propertyChanged;

		// Token: 0x040031C7 RID: 12743
		private bool _owned;
	}
}
