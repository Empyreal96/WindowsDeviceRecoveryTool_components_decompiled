using System;
using System.Collections.ObjectModel;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.TriggerBase" /> objects.</summary>
	// Token: 0x02000135 RID: 309
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	public sealed class TriggerCollection : Collection<TriggerBase>
	{
		// Token: 0x06000CC8 RID: 3272 RVA: 0x0002FA5B File Offset: 0x0002DC5B
		internal TriggerCollection() : this(null)
		{
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x0002FA64 File Offset: 0x0002DC64
		internal TriggerCollection(FrameworkElement owner)
		{
			this._sealed = false;
			this._owner = owner;
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x0002FA7A File Offset: 0x0002DC7A
		protected override void ClearItems()
		{
			this.CheckSealed();
			this.OnClear();
			base.ClearItems();
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x0002FA8E File Offset: 0x0002DC8E
		protected override void InsertItem(int index, TriggerBase item)
		{
			this.CheckSealed();
			this.TriggerBaseValidation(item);
			this.OnAdd(item);
			base.InsertItem(index, item);
		}

		// Token: 0x06000CCC RID: 3276 RVA: 0x0002FAAC File Offset: 0x0002DCAC
		protected override void RemoveItem(int index)
		{
			this.CheckSealed();
			TriggerBase triggerBase = base[index];
			this.OnRemove(triggerBase);
			base.RemoveItem(index);
		}

		// Token: 0x06000CCD RID: 3277 RVA: 0x0002FAD5 File Offset: 0x0002DCD5
		protected override void SetItem(int index, TriggerBase item)
		{
			this.CheckSealed();
			this.TriggerBaseValidation(item);
			this.OnAdd(item);
			base.SetItem(index, item);
		}

		/// <summary>Gets a value that indicates whether this collection is read-only and cannot be changed.</summary>
		/// <returns>
		///     <see langword="true" /> if this collection is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x06000CCE RID: 3278 RVA: 0x0002FAF3 File Offset: 0x0002DCF3
		public bool IsSealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x0002FAFC File Offset: 0x0002DCFC
		internal void Seal()
		{
			this._sealed = true;
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Seal();
			}
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x0002FB2D File Offset: 0x0002DD2D
		internal FrameworkElement Owner
		{
			get
			{
				return this._owner;
			}
		}

		// Token: 0x06000CD1 RID: 3281 RVA: 0x0002FB35 File Offset: 0x0002DD35
		private void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"TriggerCollection"
				}));
			}
		}

		// Token: 0x06000CD2 RID: 3282 RVA: 0x0002FB5D File Offset: 0x0002DD5D
		private void TriggerBaseValidation(TriggerBase triggerBase)
		{
			if (triggerBase == null)
			{
				throw new ArgumentNullException("triggerBase");
			}
		}

		// Token: 0x06000CD3 RID: 3283 RVA: 0x0002FB6D File Offset: 0x0002DD6D
		private void OnAdd(TriggerBase triggerBase)
		{
			if (this.Owner != null && this.Owner.IsInitialized)
			{
				EventTrigger.ProcessOneTrigger(this.Owner, triggerBase);
			}
			InheritanceContextHelper.ProvideContextForObject(this.Owner, triggerBase);
		}

		// Token: 0x06000CD4 RID: 3284 RVA: 0x0002FB9C File Offset: 0x0002DD9C
		private void OnRemove(TriggerBase triggerBase)
		{
			if (this.Owner != null)
			{
				if (this.Owner.IsInitialized)
				{
					EventTrigger.DisconnectOneTrigger(this.Owner, triggerBase);
				}
				InheritanceContextHelper.RemoveContextFromObject(this.Owner, triggerBase);
			}
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x0002FBCC File Offset: 0x0002DDCC
		private void OnClear()
		{
			if (this.Owner != null)
			{
				if (this.Owner.IsInitialized)
				{
					EventTrigger.DisconnectAllTriggers(this.Owner);
				}
				for (int i = base.Count - 1; i >= 0; i--)
				{
					InheritanceContextHelper.RemoveContextFromObject(this.Owner, base[i]);
				}
			}
		}

		// Token: 0x04000B22 RID: 2850
		private bool _sealed;

		// Token: 0x04000B23 RID: 2851
		private FrameworkElement _owner;
	}
}
