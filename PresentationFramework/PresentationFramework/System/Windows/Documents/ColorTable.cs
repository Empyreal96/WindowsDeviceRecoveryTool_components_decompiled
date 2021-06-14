using System;
using System.Collections;
using System.Windows.Media;

namespace System.Windows.Documents
{
	// Token: 0x020003C7 RID: 967
	internal class ColorTable : ArrayList
	{
		// Token: 0x06003402 RID: 13314 RVA: 0x000E7AA9 File Offset: 0x000E5CA9
		internal ColorTable() : base(20)
		{
			this._inProgress = false;
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x000E7ABA File Offset: 0x000E5CBA
		internal Color ColorAt(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return this.EntryAt(index).Color;
			}
			return Color.FromArgb(byte.MaxValue, 0, 0, 0);
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x000E7AE4 File Offset: 0x000E5CE4
		internal void FinishColor()
		{
			if (this._inProgress)
			{
				this._inProgress = false;
				return;
			}
			int index = this.AddColor(Color.FromArgb(byte.MaxValue, 0, 0, 0));
			this.EntryAt(index).IsAuto = true;
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x000E7B24 File Offset: 0x000E5D24
		internal int AddColor(Color color)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this.ColorAt(i) == color)
				{
					return i;
				}
			}
			this.Add(new ColorTableEntry
			{
				Color = color
			});
			return this.Count - 1;
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x000E7B70 File Offset: 0x000E5D70
		internal ColorTableEntry EntryAt(int index)
		{
			if (index >= 0 && index < this.Count)
			{
				return (ColorTableEntry)this[index];
			}
			return null;
		}

		// Token: 0x17000D5B RID: 3419
		// (set) Token: 0x06003407 RID: 13319 RVA: 0x000E7B90 File Offset: 0x000E5D90
		internal byte NewRed
		{
			set
			{
				ColorTableEntry inProgressEntry = this.GetInProgressEntry();
				if (inProgressEntry != null)
				{
					inProgressEntry.Red = value;
				}
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (set) Token: 0x06003408 RID: 13320 RVA: 0x000E7BB0 File Offset: 0x000E5DB0
		internal byte NewGreen
		{
			set
			{
				ColorTableEntry inProgressEntry = this.GetInProgressEntry();
				if (inProgressEntry != null)
				{
					inProgressEntry.Green = value;
				}
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (set) Token: 0x06003409 RID: 13321 RVA: 0x000E7BD0 File Offset: 0x000E5DD0
		internal byte NewBlue
		{
			set
			{
				ColorTableEntry inProgressEntry = this.GetInProgressEntry();
				if (inProgressEntry != null)
				{
					inProgressEntry.Blue = value;
				}
			}
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x000E7BF0 File Offset: 0x000E5DF0
		private ColorTableEntry GetInProgressEntry()
		{
			if (this._inProgress)
			{
				return this.EntryAt(this.Count - 1);
			}
			this._inProgress = true;
			ColorTableEntry colorTableEntry = new ColorTableEntry();
			this.Add(colorTableEntry);
			return colorTableEntry;
		}

		// Token: 0x040024BA RID: 9402
		private bool _inProgress;
	}
}
