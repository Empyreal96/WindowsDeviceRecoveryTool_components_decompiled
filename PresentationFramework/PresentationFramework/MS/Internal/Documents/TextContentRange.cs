using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;

namespace MS.Internal.Documents
{
	// Token: 0x020006F3 RID: 1779
	internal sealed class TextContentRange
	{
		// Token: 0x0600723E RID: 29246 RVA: 0x0000326D File Offset: 0x0000146D
		internal TextContentRange()
		{
		}

		// Token: 0x0600723F RID: 29247 RVA: 0x0020A5D4 File Offset: 0x002087D4
		internal TextContentRange(int cpFirst, int cpLast, ITextContainer textContainer)
		{
			Invariant.Assert(cpFirst <= cpLast);
			Invariant.Assert(cpFirst >= 0);
			Invariant.Assert(textContainer != null);
			Invariant.Assert(cpLast <= textContainer.SymbolCount);
			this._cpFirst = cpFirst;
			this._cpLast = cpLast;
			this._size = 0;
			this._ranges = null;
			this._textContainer = textContainer;
		}

		// Token: 0x06007240 RID: 29248 RVA: 0x0020A63C File Offset: 0x0020883C
		internal void Merge(TextContentRange other)
		{
			Invariant.Assert(other != null);
			if (other._textContainer == null)
			{
				return;
			}
			if (this._textContainer == null)
			{
				this._cpFirst = other._cpFirst;
				this._cpLast = other._cpLast;
				this._textContainer = other._textContainer;
				this._size = other._size;
				if (this._size != 0)
				{
					Invariant.Assert(other._ranges != null);
					Invariant.Assert(other._ranges.Length >= other._size * 2);
					this._ranges = new int[this._size * 2];
					for (int i = 0; i < this._ranges.Length; i++)
					{
						this._ranges[i] = other._ranges[i];
					}
				}
			}
			else
			{
				Invariant.Assert(this._textContainer == other._textContainer);
				if (other.IsSimple)
				{
					this.Merge(other._cpFirst, other._cpLast);
				}
				else
				{
					for (int j = 0; j < other._size; j++)
					{
						this.Merge(other._ranges[j * 2], other._ranges[j * 2 + 1]);
					}
				}
			}
			this.Normalize();
		}

		// Token: 0x06007241 RID: 29249 RVA: 0x0020A764 File Offset: 0x00208964
		internal ReadOnlyCollection<TextSegment> GetTextSegments()
		{
			List<TextSegment> list;
			if (this._textContainer == null)
			{
				list = new List<TextSegment>();
			}
			else if (this.IsSimple)
			{
				list = new List<TextSegment>(1);
				list.Add(new TextSegment(this._textContainer.CreatePointerAtOffset(this._cpFirst, LogicalDirection.Forward), this._textContainer.CreatePointerAtOffset(this._cpLast, LogicalDirection.Backward), true));
			}
			else
			{
				list = new List<TextSegment>(this._size);
				for (int i = 0; i < this._size; i++)
				{
					list.Add(new TextSegment(this._textContainer.CreatePointerAtOffset(this._ranges[i * 2], LogicalDirection.Forward), this._textContainer.CreatePointerAtOffset(this._ranges[i * 2 + 1], LogicalDirection.Backward), true));
				}
			}
			return new ReadOnlyCollection<TextSegment>(list);
		}

		// Token: 0x06007242 RID: 29250 RVA: 0x0020A824 File Offset: 0x00208A24
		internal bool Contains(ITextPointer position, bool strict)
		{
			bool result = false;
			int offset = position.Offset;
			if (this.IsSimple)
			{
				if (offset >= this._cpFirst && offset <= this._cpLast)
				{
					result = true;
					if (strict && this._cpFirst != this._cpLast && ((offset == this._cpFirst && position.LogicalDirection == LogicalDirection.Backward) || (offset == this._cpLast && position.LogicalDirection == LogicalDirection.Forward)))
					{
						result = false;
					}
				}
			}
			else
			{
				int i = 0;
				while (i < this._size)
				{
					if (offset >= this._ranges[i * 2] && offset <= this._ranges[i * 2 + 1])
					{
						result = true;
						if (strict && ((offset == this._ranges[i * 2] && position.LogicalDirection == LogicalDirection.Backward) || (offset == this._ranges[i * 2 + 1] && position.LogicalDirection == LogicalDirection.Forward)))
						{
							result = false;
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			return result;
		}

		// Token: 0x17001B2E RID: 6958
		// (get) Token: 0x06007243 RID: 29251 RVA: 0x0020A900 File Offset: 0x00208B00
		internal ITextPointer StartPosition
		{
			get
			{
				ITextPointer result = null;
				if (this._textContainer != null)
				{
					result = this._textContainer.CreatePointerAtOffset(this.IsSimple ? this._cpFirst : this._ranges[0], LogicalDirection.Forward);
				}
				return result;
			}
		}

		// Token: 0x17001B2F RID: 6959
		// (get) Token: 0x06007244 RID: 29252 RVA: 0x0020A940 File Offset: 0x00208B40
		internal ITextPointer EndPosition
		{
			get
			{
				ITextPointer result = null;
				if (this._textContainer != null)
				{
					result = this._textContainer.CreatePointerAtOffset(this.IsSimple ? this._cpLast : this._ranges[(this._size - 1) * 2 + 1], LogicalDirection.Backward);
				}
				return result;
			}
		}

		// Token: 0x06007245 RID: 29253 RVA: 0x0020A988 File Offset: 0x00208B88
		private void Merge(int cpFirst, int cpLast)
		{
			if (!this.IsSimple)
			{
				int i;
				for (i = 0; i < this._size; i++)
				{
					if (cpLast < this._ranges[i * 2])
					{
						this.EnsureSize();
						for (int j = this._size * 2 - 1; j >= i * 2; j--)
						{
							this._ranges[j + 2] = this._ranges[j];
						}
						this._ranges[i * 2] = cpFirst;
						this._ranges[i * 2 + 1] = cpLast;
						this._size++;
						break;
					}
					if (cpFirst <= this._ranges[i * 2 + 1])
					{
						this._ranges[i * 2] = Math.Min(this._ranges[i * 2], cpFirst);
						this._ranges[i * 2 + 1] = Math.Max(this._ranges[i * 2 + 1], cpLast);
						while (this.MergeWithNext(i))
						{
						}
						break;
					}
				}
				if (i >= this._size)
				{
					this.EnsureSize();
					this._ranges[this._size * 2] = cpFirst;
					this._ranges[this._size * 2 + 1] = cpLast;
					this._size++;
				}
				return;
			}
			if (cpFirst <= this._cpLast && cpLast >= this._cpFirst)
			{
				this._cpFirst = Math.Min(this._cpFirst, cpFirst);
				this._cpLast = Math.Max(this._cpLast, cpLast);
				return;
			}
			this._size = 2;
			this._ranges = new int[8];
			if (cpFirst > this._cpLast)
			{
				this._ranges[0] = this._cpFirst;
				this._ranges[1] = this._cpLast;
				this._ranges[2] = cpFirst;
				this._ranges[3] = cpLast;
				return;
			}
			this._ranges[0] = cpFirst;
			this._ranges[1] = cpLast;
			this._ranges[2] = this._cpFirst;
			this._ranges[3] = this._cpLast;
		}

		// Token: 0x06007246 RID: 29254 RVA: 0x0020AB60 File Offset: 0x00208D60
		private bool MergeWithNext(int pos)
		{
			if (pos < this._size - 1 && this._ranges[pos * 2 + 1] >= this._ranges[(pos + 1) * 2])
			{
				this._ranges[pos * 2 + 1] = Math.Max(this._ranges[pos * 2 + 1], this._ranges[(pos + 1) * 2 + 1]);
				for (int i = (pos + 1) * 2; i < (this._size - 1) * 2; i++)
				{
					this._ranges[i] = this._ranges[i + 2];
				}
				this._size--;
				return true;
			}
			return false;
		}

		// Token: 0x06007247 RID: 29255 RVA: 0x0020ABFC File Offset: 0x00208DFC
		private void EnsureSize()
		{
			Invariant.Assert(this._size > 0);
			Invariant.Assert(this._ranges != null);
			if (this._ranges.Length < (this._size + 1) * 2)
			{
				int[] array = new int[this._ranges.Length * 2];
				for (int i = 0; i < this._size * 2; i++)
				{
					array[i] = this._ranges[i];
				}
				this._ranges = array;
			}
		}

		// Token: 0x06007248 RID: 29256 RVA: 0x0020AC6E File Offset: 0x00208E6E
		private void Normalize()
		{
			if (this._size == 1)
			{
				this._cpFirst = this._ranges[0];
				this._cpLast = this._ranges[1];
				this._size = 0;
				this._ranges = null;
			}
		}

		// Token: 0x17001B30 RID: 6960
		// (get) Token: 0x06007249 RID: 29257 RVA: 0x0020ACA3 File Offset: 0x00208EA3
		private bool IsSimple
		{
			get
			{
				return this._size == 0;
			}
		}

		// Token: 0x04003759 RID: 14169
		private int _cpFirst;

		// Token: 0x0400375A RID: 14170
		private int _cpLast;

		// Token: 0x0400375B RID: 14171
		private int _size;

		// Token: 0x0400375C RID: 14172
		private int[] _ranges;

		// Token: 0x0400375D RID: 14173
		private ITextContainer _textContainer;
	}
}
