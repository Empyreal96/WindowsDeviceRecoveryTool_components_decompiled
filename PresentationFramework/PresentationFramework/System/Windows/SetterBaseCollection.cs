using System;
using System.Collections.ObjectModel;

namespace System.Windows
{
	/// <summary>Represents a collection of <see cref="T:System.Windows.SetterBase" /> objects.</summary>
	// Token: 0x020000F4 RID: 244
	public sealed class SetterBaseCollection : Collection<SetterBase>
	{
		// Token: 0x060008A4 RID: 2212 RVA: 0x0001C1F9 File Offset: 0x0001A3F9
		protected override void ClearItems()
		{
			this.CheckSealed();
			base.ClearItems();
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0001C207 File Offset: 0x0001A407
		protected override void InsertItem(int index, SetterBase item)
		{
			this.CheckSealed();
			this.SetterBaseValidation(item);
			base.InsertItem(index, item);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0001C21E File Offset: 0x0001A41E
		protected override void RemoveItem(int index)
		{
			this.CheckSealed();
			base.RemoveItem(index);
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0001C22D File Offset: 0x0001A42D
		protected override void SetItem(int index, SetterBase item)
		{
			this.CheckSealed();
			this.SetterBaseValidation(item);
			base.SetItem(index, item);
		}

		/// <summary>Gets a value that indicates whether this object is in a read-only state.</summary>
		/// <returns>
		///     <see langword="true" /> if this object is in a read-only state and cannot be changed; otherwise, <see langword="false" />.</returns>
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x0001C244 File Offset: 0x0001A444
		public bool IsSealed
		{
			get
			{
				return this._sealed;
			}
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0001C24C File Offset: 0x0001A44C
		internal void Seal()
		{
			this._sealed = true;
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Seal();
			}
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x0001C27D File Offset: 0x0001A47D
		private void CheckSealed()
		{
			if (this._sealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"SetterBaseCollection"
				}));
			}
		}

		// Token: 0x060008AB RID: 2219 RVA: 0x0001C2A5 File Offset: 0x0001A4A5
		private void SetterBaseValidation(SetterBase setterBase)
		{
			if (setterBase == null)
			{
				throw new ArgumentNullException("setterBase");
			}
		}

		// Token: 0x040007B1 RID: 1969
		private bool _sealed;
	}
}
