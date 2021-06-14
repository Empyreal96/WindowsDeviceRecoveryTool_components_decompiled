using System;
using MS.Internal;

namespace System.Windows
{
	// Token: 0x02000116 RID: 278
	internal class DeferredResourceReference : DeferredReference
	{
		// Token: 0x06000BA3 RID: 2979 RVA: 0x0002ADD9 File Offset: 0x00028FD9
		internal DeferredResourceReference(ResourceDictionary dictionary, object key)
		{
			this._dictionary = dictionary;
			this._keyOrValue = key;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x0002ADF0 File Offset: 0x00028FF0
		internal override object GetValue(BaseValueSourceInternal valueSource)
		{
			if (this._dictionary != null)
			{
				bool flag;
				object value = this._dictionary.GetValue(this._keyOrValue, out flag);
				if (flag)
				{
					this._keyOrValue = value;
					this.RemoveFromDictionary();
				}
				bool flag2 = valueSource == BaseValueSourceInternal.ThemeStyle || valueSource == BaseValueSourceInternal.ThemeStyleTrigger || valueSource == BaseValueSourceInternal.Style || valueSource == BaseValueSourceInternal.TemplateTrigger || valueSource == BaseValueSourceInternal.StyleTrigger || valueSource == BaseValueSourceInternal.ParentTemplate || valueSource == BaseValueSourceInternal.ParentTemplateTrigger;
				if (flag2)
				{
					StyleHelper.SealIfSealable(value);
				}
				this.OnInflated();
				return value;
			}
			return this._keyOrValue;
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x0002AE64 File Offset: 0x00029064
		private void OnInflated()
		{
			if (this._inflatedList != null)
			{
				foreach (object obj in this._inflatedList)
				{
					ResourceReferenceExpression resourceReferenceExpression = (ResourceReferenceExpression)obj;
					resourceReferenceExpression.OnDeferredResourceInflated(this);
				}
			}
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0002AEA4 File Offset: 0x000290A4
		internal override Type GetValueType()
		{
			if (this._dictionary != null)
			{
				bool flag;
				return this._dictionary.GetValueType(this._keyOrValue, out flag);
			}
			if (this._keyOrValue == null)
			{
				return null;
			}
			return this._keyOrValue.GetType();
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0002AEE2 File Offset: 0x000290E2
		internal virtual void RemoveFromDictionary()
		{
			if (this._dictionary != null)
			{
				this._dictionary.DeferredResourceReferences.Remove(this);
				this._dictionary = null;
			}
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0002AF05 File Offset: 0x00029105
		internal virtual void AddInflatedListener(ResourceReferenceExpression listener)
		{
			if (this._inflatedList == null)
			{
				this._inflatedList = new WeakReferenceList(this);
			}
			this._inflatedList.Add(listener);
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0002AF28 File Offset: 0x00029128
		internal virtual void RemoveInflatedListener(ResourceReferenceExpression listener)
		{
			if (this._inflatedList != null)
			{
				this._inflatedList.Remove(listener);
			}
		}

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x06000BAA RID: 2986 RVA: 0x0002AF3F File Offset: 0x0002913F
		internal virtual object Key
		{
			get
			{
				return this._keyOrValue;
			}
		}

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06000BAB RID: 2987 RVA: 0x0002AF47 File Offset: 0x00029147
		// (set) Token: 0x06000BAC RID: 2988 RVA: 0x0002AF4F File Offset: 0x0002914F
		internal ResourceDictionary Dictionary
		{
			get
			{
				return this._dictionary;
			}
			set
			{
				this._dictionary = value;
			}
		}

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x06000BAD RID: 2989 RVA: 0x0002AF3F File Offset: 0x0002913F
		// (set) Token: 0x06000BAE RID: 2990 RVA: 0x0002AF58 File Offset: 0x00029158
		internal virtual object Value
		{
			get
			{
				return this._keyOrValue;
			}
			set
			{
				this._keyOrValue = value;
			}
		}

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x06000BAF RID: 2991 RVA: 0x0000B02A File Offset: 0x0000922A
		internal virtual bool IsUnset
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x06000BB0 RID: 2992 RVA: 0x0002AF61 File Offset: 0x00029161
		internal bool IsInflated
		{
			get
			{
				return this._dictionary == null;
			}
		}

		// Token: 0x04000AB2 RID: 2738
		private ResourceDictionary _dictionary;

		// Token: 0x04000AB3 RID: 2739
		protected object _keyOrValue;

		// Token: 0x04000AB4 RID: 2740
		private WeakReferenceList _inflatedList;
	}
}
