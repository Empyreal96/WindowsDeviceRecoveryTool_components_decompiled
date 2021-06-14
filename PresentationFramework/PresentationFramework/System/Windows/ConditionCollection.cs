using System;
using System.Collections.ObjectModel;

namespace System.Windows
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.Condition" /> objects.</summary>
	// Token: 0x020000A4 RID: 164
	public sealed class ConditionCollection : Collection<Condition>
	{
		// Token: 0x0600035C RID: 860 RVA: 0x00009A85 File Offset: 0x00007C85
		protected override void ClearItems()
		{
			this.CheckSealed();
			base.ClearItems();
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00009A93 File Offset: 0x00007C93
		protected override void InsertItem(int index, Condition item)
		{
			this.CheckSealed();
			this.ConditionValidation(item);
			base.InsertItem(index, item);
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00009AAA File Offset: 0x00007CAA
		protected override void RemoveItem(int index)
		{
			this.CheckSealed();
			base.RemoveItem(index);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00009AB9 File Offset: 0x00007CB9
		protected override void SetItem(int index, Condition item)
		{
			this.CheckSealed();
			this.ConditionValidation(item);
			base.SetItem(index, item);
		}

		/// <summary>Gets a value that indicates whether this trigger is read-only and cannot be changed .</summary>
		/// <returns>
		///     <see langword="true" /> if the trigger is read-only; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000360 RID: 864 RVA: 0x00009AD0 File Offset: 0x00007CD0
		public bool IsSealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00009AD8 File Offset: 0x00007CD8
		internal void Seal(ValueLookupType type)
		{
			this._sealed = true;
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Seal(type);
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00009B0A File Offset: 0x00007D0A
		private void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"ConditionCollection"
				}));
			}
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00009B34 File Offset: 0x00007D34
		private void ConditionValidation(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is Condition))
			{
				throw new ArgumentException(SR.Get("MustBeCondition"));
			}
		}

		// Token: 0x040005E6 RID: 1510
		private bool _sealed;
	}
}
