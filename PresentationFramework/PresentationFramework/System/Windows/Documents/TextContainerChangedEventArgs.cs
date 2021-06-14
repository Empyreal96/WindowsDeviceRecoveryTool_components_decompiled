using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace System.Windows.Documents
{
	// Token: 0x020003ED RID: 1005
	internal class TextContainerChangedEventArgs : EventArgs
	{
		// Token: 0x060037B4 RID: 14260 RVA: 0x000F8D6A File Offset: 0x000F6F6A
		internal TextContainerChangedEventArgs()
		{
			this._changes = new SortedList<int, TextChange>();
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x000F8D7D File Offset: 0x000F6F7D
		internal void SetLocalPropertyValueChanged()
		{
			this._hasLocalPropertyValueChange = true;
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x000F8D88 File Offset: 0x000F6F88
		internal void AddChange(PrecursorTextChangeType textChange, int offset, int length, bool collectTextChanges)
		{
			if (textChange == PrecursorTextChangeType.ContentAdded || textChange == PrecursorTextChangeType.ElementAdded || textChange == PrecursorTextChangeType.ContentRemoved || textChange == PrecursorTextChangeType.ElementExtracted)
			{
				this._hasContentAddedOrRemoved = true;
			}
			if (!collectTextChanges)
			{
				return;
			}
			if (textChange == PrecursorTextChangeType.ElementAdded)
			{
				this.AddChangeToList(textChange, offset, 1);
				this.AddChangeToList(textChange, offset + length - 1, 1);
				return;
			}
			if (textChange == PrecursorTextChangeType.ElementExtracted)
			{
				this.AddChangeToList(textChange, offset + length - 1, 1);
				this.AddChangeToList(textChange, offset, 1);
				return;
			}
			if (textChange == PrecursorTextChangeType.PropertyModified)
			{
				return;
			}
			this.AddChangeToList(textChange, offset, length);
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x060037B7 RID: 14263 RVA: 0x000F8DF4 File Offset: 0x000F6FF4
		internal bool HasContentAddedOrRemoved
		{
			get
			{
				return this._hasContentAddedOrRemoved;
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x060037B8 RID: 14264 RVA: 0x000F8DFC File Offset: 0x000F6FFC
		internal bool HasLocalPropertyValueChange
		{
			get
			{
				return this._hasLocalPropertyValueChange;
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x060037B9 RID: 14265 RVA: 0x000F8E04 File Offset: 0x000F7004
		internal SortedList<int, TextChange> Changes
		{
			get
			{
				return this._changes;
			}
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x000F8E0C File Offset: 0x000F700C
		private void AddChangeToList(PrecursorTextChangeType textChange, int offset, int length)
		{
			int num = 0;
			bool flag = false;
			int num2 = this.Changes.IndexOfKey(offset);
			TextChange textChange2;
			if (num2 != -1)
			{
				textChange2 = this.Changes.Values[num2];
			}
			else
			{
				textChange2 = new TextChange();
				textChange2.Offset = offset;
				this.Changes.Add(offset, textChange2);
				num2 = this.Changes.IndexOfKey(offset);
			}
			if (textChange == PrecursorTextChangeType.ContentAdded || textChange == PrecursorTextChangeType.ElementAdded)
			{
				textChange2.AddedLength += length;
				num = length;
			}
			else if (textChange == PrecursorTextChangeType.ContentRemoved || textChange == PrecursorTextChangeType.ElementExtracted)
			{
				textChange2.RemovedLength += Math.Max(0, length - textChange2.AddedLength);
				textChange2.AddedLength = Math.Max(0, textChange2.AddedLength - length);
				num = -length;
				flag = true;
			}
			int i;
			if (num2 > 0 && textChange != PrecursorTextChangeType.PropertyModified)
			{
				i = num2 - 1;
				TextChange textChange3 = null;
				while (i >= 0)
				{
					TextChange textChange4 = this.Changes.Values[i];
					if (textChange4.Offset + textChange4.AddedLength >= offset && this.MergeTextChangeLeft(textChange4, textChange2, flag, length))
					{
						textChange3 = textChange4;
					}
					i--;
				}
				if (textChange3 != null)
				{
					textChange2 = textChange3;
				}
				num2 = this.Changes.IndexOfKey(textChange2.Offset);
			}
			i = num2 + 1;
			if (flag && i < this.Changes.Count)
			{
				while (i < this.Changes.Count && this.Changes.Values[i].Offset <= offset + length)
				{
					this.MergeTextChangeRight(this.Changes.Values[i], textChange2, offset, length);
				}
				num2 = this.Changes.IndexOfKey(textChange2.Offset);
			}
			if (num != 0)
			{
				SortedList<int, TextChange> sortedList = new SortedList<int, TextChange>(this.Changes.Count);
				for (i = 0; i < this.Changes.Count; i++)
				{
					TextChange textChange5 = this.Changes.Values[i];
					if (i > num2)
					{
						textChange5.Offset += num;
					}
					sortedList.Add(textChange5.Offset, textChange5);
				}
				this._changes = sortedList;
			}
			this.DeleteChangeIfEmpty(textChange2);
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x000F900E File Offset: 0x000F720E
		private void DeleteChangeIfEmpty(TextChange change)
		{
			if (change.AddedLength == 0 && change.RemovedLength == 0)
			{
				this.Changes.Remove(change.Offset);
			}
		}

		// Token: 0x060037BC RID: 14268 RVA: 0x000F9034 File Offset: 0x000F7234
		private bool MergeTextChangeLeft(TextChange oldChange, TextChange newChange, bool isDeletion, int length)
		{
			if (oldChange.Offset + oldChange.AddedLength >= newChange.Offset)
			{
				if (isDeletion)
				{
					int val = oldChange.AddedLength - (newChange.Offset - oldChange.Offset);
					int num = Math.Min(val, newChange.RemovedLength);
					oldChange.AddedLength -= num;
					oldChange.RemovedLength += length - num;
				}
				else
				{
					oldChange.AddedLength += length;
				}
				this.Changes.Remove(newChange.Offset);
				return true;
			}
			return false;
		}

		// Token: 0x060037BD RID: 14269 RVA: 0x000F90C0 File Offset: 0x000F72C0
		private void MergeTextChangeRight(TextChange oldChange, TextChange newChange, int offset, int length)
		{
			int num = (oldChange.AddedLength > 0) ? (offset + length - oldChange.Offset) : 0;
			if (num >= oldChange.AddedLength)
			{
				newChange.RemovedLength += oldChange.RemovedLength - oldChange.AddedLength;
				this.Changes.Remove(oldChange.Offset);
				return;
			}
			newChange.RemovedLength += oldChange.RemovedLength - num;
			newChange.AddedLength += oldChange.AddedLength - num;
			this.Changes.Remove(oldChange.Offset);
		}

		// Token: 0x04002569 RID: 9577
		private bool _hasContentAddedOrRemoved;

		// Token: 0x0400256A RID: 9578
		private bool _hasLocalPropertyValueChange;

		// Token: 0x0400256B RID: 9579
		private SortedList<int, TextChange> _changes;
	}
}
