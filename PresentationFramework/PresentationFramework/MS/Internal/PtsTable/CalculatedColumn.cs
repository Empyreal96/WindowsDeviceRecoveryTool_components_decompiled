using System;
using System.Windows;
using MS.Internal.PtsHost.UnsafeNativeMethods;

namespace MS.Internal.PtsTable
{
	// Token: 0x0200060A RID: 1546
	internal struct CalculatedColumn
	{
		// Token: 0x06006710 RID: 26384 RVA: 0x001CD828 File Offset: 0x001CBA28
		internal void ValidateAuto(double durMinWidth, double durMaxWidth)
		{
			this._durMinWidth = durMinWidth;
			this._durMaxWidth = durMaxWidth;
			this.SetFlags(true, CalculatedColumn.Flags.ValidAutofit);
		}

		// Token: 0x170018DF RID: 6367
		// (get) Token: 0x06006711 RID: 26385 RVA: 0x001CD840 File Offset: 0x001CBA40
		internal int PtsWidthChanged
		{
			get
			{
				return PTS.FromBoolean(!this.CheckFlags(CalculatedColumn.Flags.ValidWidth));
			}
		}

		// Token: 0x170018E0 RID: 6368
		// (get) Token: 0x06006712 RID: 26386 RVA: 0x001CD851 File Offset: 0x001CBA51
		internal double DurMinWidth
		{
			get
			{
				return this._durMinWidth;
			}
		}

		// Token: 0x170018E1 RID: 6369
		// (get) Token: 0x06006713 RID: 26387 RVA: 0x001CD859 File Offset: 0x001CBA59
		internal double DurMaxWidth
		{
			get
			{
				return this._durMaxWidth;
			}
		}

		// Token: 0x170018E2 RID: 6370
		// (get) Token: 0x06006714 RID: 26388 RVA: 0x001CD861 File Offset: 0x001CBA61
		// (set) Token: 0x06006715 RID: 26389 RVA: 0x001CD869 File Offset: 0x001CBA69
		internal GridLength UserWidth
		{
			get
			{
				return this._userWidth;
			}
			set
			{
				if (this._userWidth != value)
				{
					this.SetFlags(false, CalculatedColumn.Flags.ValidAutofit);
				}
				this._userWidth = value;
			}
		}

		// Token: 0x170018E3 RID: 6371
		// (get) Token: 0x06006716 RID: 26390 RVA: 0x001CD888 File Offset: 0x001CBA88
		// (set) Token: 0x06006717 RID: 26391 RVA: 0x001CD890 File Offset: 0x001CBA90
		internal double DurWidth
		{
			get
			{
				return this._durWidth;
			}
			set
			{
				if (!DoubleUtil.AreClose(this._durWidth, value))
				{
					this.SetFlags(false, CalculatedColumn.Flags.ValidWidth);
				}
				this._durWidth = value;
			}
		}

		// Token: 0x170018E4 RID: 6372
		// (get) Token: 0x06006718 RID: 26392 RVA: 0x001CD8AF File Offset: 0x001CBAAF
		// (set) Token: 0x06006719 RID: 26393 RVA: 0x001CD8B7 File Offset: 0x001CBAB7
		internal double UrOffset
		{
			get
			{
				return this._urOffset;
			}
			set
			{
				this._urOffset = value;
			}
		}

		// Token: 0x0600671A RID: 26394 RVA: 0x001CD8C0 File Offset: 0x001CBAC0
		private void SetFlags(bool value, CalculatedColumn.Flags flags)
		{
			this._flags = (value ? (this._flags | flags) : (this._flags & ~flags));
		}

		// Token: 0x0600671B RID: 26395 RVA: 0x001CD8DE File Offset: 0x001CBADE
		private bool CheckFlags(CalculatedColumn.Flags flags)
		{
			return (this._flags & flags) == flags;
		}

		// Token: 0x04003345 RID: 13125
		private GridLength _userWidth;

		// Token: 0x04003346 RID: 13126
		private double _durWidth;

		// Token: 0x04003347 RID: 13127
		private double _durMinWidth;

		// Token: 0x04003348 RID: 13128
		private double _durMaxWidth;

		// Token: 0x04003349 RID: 13129
		private double _urOffset;

		// Token: 0x0400334A RID: 13130
		private CalculatedColumn.Flags _flags;

		// Token: 0x02000A1C RID: 2588
		[Flags]
		private enum Flags
		{
			// Token: 0x040046F5 RID: 18165
			ValidWidth = 1,
			// Token: 0x040046F6 RID: 18166
			ValidAutofit = 2
		}
	}
}
