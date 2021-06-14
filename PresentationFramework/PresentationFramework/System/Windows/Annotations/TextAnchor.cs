using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Documents;
using MS.Internal;

namespace System.Windows.Annotations
{
	/// <summary>Represents a selection of content that an annotation is anchored to.</summary>
	// Token: 0x020005C2 RID: 1474
	public sealed class TextAnchor
	{
		// Token: 0x0600624D RID: 25165 RVA: 0x001B8C88 File Offset: 0x001B6E88
		internal TextAnchor()
		{
		}

		// Token: 0x0600624E RID: 25166 RVA: 0x001B8C9C File Offset: 0x001B6E9C
		internal TextAnchor(TextAnchor anchor)
		{
			Invariant.Assert(anchor != null, "Anchor to clone is null.");
			foreach (TextSegment textSegment in anchor.TextSegments)
			{
				this._segments.Add(new TextSegment(textSegment.Start, textSegment.End));
			}
		}

		// Token: 0x0600624F RID: 25167 RVA: 0x001B8D20 File Offset: 0x001B6F20
		internal bool Contains(ITextPointer textPointer)
		{
			if (textPointer == null)
			{
				throw new ArgumentNullException("textPointer");
			}
			if (textPointer.TextContainer != this.Start.TextContainer)
			{
				throw new ArgumentException(SR.Get("NotInAssociatedTree", new object[]
				{
					"textPointer"
				}));
			}
			if (textPointer.CompareTo(this.Start) < 0)
			{
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Forward);
			}
			else if (textPointer.CompareTo(this.End) > 0)
			{
				textPointer = textPointer.GetInsertionPosition(LogicalDirection.Backward);
			}
			for (int i = 0; i < this._segments.Count; i++)
			{
				if (this._segments[i].Contains(textPointer))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006250 RID: 25168 RVA: 0x001B8DD0 File Offset: 0x001B6FD0
		internal void AddTextSegment(ITextPointer start, ITextPointer end)
		{
			Invariant.Assert(start != null, "Non-null start required to create segment.");
			Invariant.Assert(end != null, "Non-null end required to create segment.");
			TextSegment newSegment = TextAnchor.CreateNormalizedSegment(start, end);
			this.InsertSegment(newSegment);
		}

		/// <summary>Returns the hash code of the text anchor instance.</summary>
		/// <returns>The hash code of the text anchor instance.</returns>
		// Token: 0x06006251 RID: 25169 RVA: 0x001B8E08 File Offset: 0x001B7008
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Returns a value that indicates whether the text anchor is equal to the specified object. </summary>
		/// <param name="obj">The object to compare to.</param>
		/// <returns>
		///     <see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</returns>
		// Token: 0x06006252 RID: 25170 RVA: 0x001B8E10 File Offset: 0x001B7010
		public override bool Equals(object obj)
		{
			TextAnchor textAnchor = obj as TextAnchor;
			if (textAnchor == null)
			{
				return false;
			}
			if (textAnchor._segments.Count != this._segments.Count)
			{
				return false;
			}
			for (int i = 0; i < this._segments.Count; i++)
			{
				if (this._segments[i].Start.CompareTo(textAnchor._segments[i].Start) != 0 || this._segments[i].End.CompareTo(textAnchor._segments[i].End) != 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006253 RID: 25171 RVA: 0x001B8EBC File Offset: 0x001B70BC
		internal bool IsOverlapping(ICollection<TextSegment> textSegments)
		{
			Invariant.Assert(textSegments != null, "TextSegments must not be null.");
			textSegments = TextAnchor.SortTextSegments(textSegments, false);
			IEnumerator<TextSegment> enumerator = this._segments.GetEnumerator();
			IEnumerator<TextSegment> enumerator2 = textSegments.GetEnumerator();
			bool flag = enumerator.MoveNext();
			bool flag2 = enumerator2.MoveNext();
			while (flag && flag2)
			{
				TextSegment textSegment = enumerator.Current;
				TextSegment textSegment2 = enumerator2.Current;
				if (textSegment2.Start.CompareTo(textSegment2.End) == 0)
				{
					if (textSegment.Start.CompareTo(textSegment2.Start) == 0 && textSegment2.Start.LogicalDirection == LogicalDirection.Forward)
					{
						return true;
					}
					if (textSegment.End.CompareTo(textSegment2.End) == 0 && textSegment2.End.LogicalDirection == LogicalDirection.Backward)
					{
						return true;
					}
				}
				if (textSegment.Start.CompareTo(textSegment2.End) >= 0)
				{
					flag2 = enumerator2.MoveNext();
				}
				else
				{
					if (textSegment.End.CompareTo(textSegment2.Start) > 0)
					{
						return true;
					}
					flag = enumerator.MoveNext();
				}
			}
			return false;
		}

		// Token: 0x06006254 RID: 25172 RVA: 0x001B8FC8 File Offset: 0x001B71C8
		internal static TextAnchor ExclusiveUnion(TextAnchor anchor, TextAnchor otherAnchor)
		{
			Invariant.Assert(anchor != null, "anchor must not be null.");
			Invariant.Assert(otherAnchor != null, "otherAnchor must not be null.");
			foreach (TextSegment newSegment in otherAnchor.TextSegments)
			{
				anchor.InsertSegment(newSegment);
			}
			return anchor;
		}

		// Token: 0x06006255 RID: 25173 RVA: 0x001B9034 File Offset: 0x001B7234
		internal static TextAnchor TrimToRelativeComplement(TextAnchor anchor, ICollection<TextSegment> textSegments)
		{
			Invariant.Assert(anchor != null, "Anchor must not be null.");
			Invariant.Assert(textSegments != null, "TextSegments must not be null.");
			textSegments = TextAnchor.SortTextSegments(textSegments, true);
			IEnumerator<TextSegment> enumerator = textSegments.GetEnumerator();
			bool flag = enumerator.MoveNext();
			int num = 0;
			TextSegment textSegment = TextSegment.Null;
			while (num < anchor._segments.Count && flag)
			{
				bool condition;
				if (!textSegment.Equals(TextSegment.Null) && !textSegment.Equals(enumerator.Current))
				{
					ITextPointer end = textSegment.End;
					TextSegment textSegment2 = enumerator.Current;
					condition = (end.CompareTo(textSegment2.Start) <= 0);
				}
				else
				{
					condition = true;
				}
				Invariant.Assert(condition, "TextSegments are overlapping or not ordered.");
				TextSegment textSegment3 = anchor._segments[num];
				textSegment = enumerator.Current;
				if (textSegment3.Start.CompareTo(textSegment.End) >= 0)
				{
					flag = enumerator.MoveNext();
				}
				else if (textSegment3.Start.CompareTo(textSegment.Start) >= 0)
				{
					if (textSegment3.End.CompareTo(textSegment.End) <= 0)
					{
						anchor._segments.RemoveAt(num);
					}
					else
					{
						anchor._segments[num] = TextAnchor.CreateNormalizedSegment(textSegment.End, textSegment3.End);
						flag = enumerator.MoveNext();
					}
				}
				else
				{
					if (textSegment3.End.CompareTo(textSegment.Start) > 0)
					{
						anchor._segments[num] = TextAnchor.CreateNormalizedSegment(textSegment3.Start, textSegment.Start);
						if (textSegment3.End.CompareTo(textSegment.End) > 0)
						{
							anchor._segments.Insert(num + 1, TextAnchor.CreateNormalizedSegment(textSegment.End, textSegment3.End));
							flag = enumerator.MoveNext();
						}
					}
					num++;
				}
			}
			if (anchor._segments.Count > 0)
			{
				return anchor;
			}
			return null;
		}

		// Token: 0x06006256 RID: 25174 RVA: 0x001B921C File Offset: 0x001B741C
		internal static TextAnchor TrimToIntersectionWith(TextAnchor anchor, ICollection<TextSegment> textSegments)
		{
			Invariant.Assert(anchor != null, "Anchor must not be null.");
			Invariant.Assert(textSegments != null, "TextSegments must not be null.");
			textSegments = TextAnchor.SortTextSegments(textSegments, true);
			TextSegment textSegment = TextSegment.Null;
			int num = 0;
			IEnumerator<TextSegment> enumerator = textSegments.GetEnumerator();
			bool flag = enumerator.MoveNext();
			while (num < anchor._segments.Count && flag)
			{
				bool condition;
				if (!textSegment.Equals(TextSegment.Null) && !textSegment.Equals(enumerator.Current))
				{
					ITextPointer end = textSegment.End;
					TextSegment textSegment2 = enumerator.Current;
					condition = (end.CompareTo(textSegment2.Start) <= 0);
				}
				else
				{
					condition = true;
				}
				Invariant.Assert(condition, "TextSegments are overlapping or not ordered.");
				TextSegment textSegment3 = anchor._segments[num];
				textSegment = enumerator.Current;
				if (textSegment3.Start.CompareTo(textSegment.End) >= 0)
				{
					flag = enumerator.MoveNext();
				}
				else if (textSegment3.End.CompareTo(textSegment.Start) <= 0)
				{
					anchor._segments.RemoveAt(num);
				}
				else if (textSegment3.Start.CompareTo(textSegment.Start) < 0)
				{
					anchor._segments[num] = TextAnchor.CreateNormalizedSegment(textSegment.Start, textSegment3.End);
				}
				else
				{
					if (textSegment3.End.CompareTo(textSegment.End) > 0)
					{
						anchor._segments[num] = TextAnchor.CreateNormalizedSegment(textSegment3.Start, textSegment.End);
						anchor._segments.Insert(num + 1, TextAnchor.CreateNormalizedSegment(textSegment.End, textSegment3.End));
						flag = enumerator.MoveNext();
					}
					else if (textSegment3.End.CompareTo(textSegment.End) == 0)
					{
						flag = enumerator.MoveNext();
					}
					num++;
				}
			}
			if (!flag && num < anchor._segments.Count)
			{
				anchor._segments.RemoveRange(num, anchor._segments.Count - num);
			}
			if (anchor._segments.Count == 0)
			{
				return null;
			}
			return anchor;
		}

		/// <summary>Gets the beginning position of the text anchor.</summary>
		/// <returns>The beginning position of the text anchor.</returns>
		// Token: 0x170017A0 RID: 6048
		// (get) Token: 0x06006257 RID: 25175 RVA: 0x001B9435 File Offset: 0x001B7635
		public ContentPosition BoundingStart
		{
			get
			{
				return this.Start as ContentPosition;
			}
		}

		/// <summary>Gets the end position of the text anchor.</summary>
		/// <returns>The end position of the text anchor.</returns>
		// Token: 0x170017A1 RID: 6049
		// (get) Token: 0x06006258 RID: 25176 RVA: 0x001B9442 File Offset: 0x001B7642
		public ContentPosition BoundingEnd
		{
			get
			{
				return this.End as ContentPosition;
			}
		}

		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x06006259 RID: 25177 RVA: 0x001B9450 File Offset: 0x001B7650
		internal ITextPointer Start
		{
			get
			{
				if (this._segments.Count <= 0)
				{
					return null;
				}
				return this._segments[0].Start;
			}
		}

		// Token: 0x170017A3 RID: 6051
		// (get) Token: 0x0600625A RID: 25178 RVA: 0x001B9484 File Offset: 0x001B7684
		internal ITextPointer End
		{
			get
			{
				if (this._segments.Count <= 0)
				{
					return null;
				}
				return this._segments[this._segments.Count - 1].End;
			}
		}

		// Token: 0x170017A4 RID: 6052
		// (get) Token: 0x0600625B RID: 25179 RVA: 0x001B94C4 File Offset: 0x001B76C4
		internal bool IsEmpty
		{
			get
			{
				return this._segments.Count == 1 && this._segments[0].Start == this._segments[0].End;
			}
		}

		// Token: 0x170017A5 RID: 6053
		// (get) Token: 0x0600625C RID: 25180 RVA: 0x001B950C File Offset: 0x001B770C
		internal string Text
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this._segments.Count; i++)
				{
					stringBuilder.Append(TextRangeBase.GetTextInternal(this._segments[i].Start, this._segments[i].End));
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170017A6 RID: 6054
		// (get) Token: 0x0600625D RID: 25181 RVA: 0x001B956F File Offset: 0x001B776F
		internal ReadOnlyCollection<TextSegment> TextSegments
		{
			get
			{
				return this._segments.AsReadOnly();
			}
		}

		// Token: 0x0600625E RID: 25182 RVA: 0x001B957C File Offset: 0x001B777C
		private static ICollection<TextSegment> SortTextSegments(ICollection<TextSegment> textSegments, bool excludeZeroLength)
		{
			Invariant.Assert(textSegments != null, "TextSegments must not be null.");
			List<TextSegment> list = new List<TextSegment>(textSegments.Count);
			list.AddRange(textSegments);
			if (excludeZeroLength)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					TextSegment item = list[i];
					if (item.Start.CompareTo(item.End) >= 0)
					{
						list.Remove(item);
					}
				}
			}
			if (list.Count > 1)
			{
				list.Sort(new TextAnchor.TextSegmentComparer());
			}
			return list;
		}

		// Token: 0x0600625F RID: 25183 RVA: 0x001B95FC File Offset: 0x001B77FC
		private void InsertSegment(TextSegment newSegment)
		{
			int num = 0;
			while (num < this._segments.Count && newSegment.Start.CompareTo(this._segments[num].Start) >= 0)
			{
				num++;
			}
			if (num > 0 && newSegment.Start.CompareTo(this._segments[num - 1].End) < 0)
			{
				throw new InvalidOperationException(SR.Get("TextSegmentsMustNotOverlap"));
			}
			if (num < this._segments.Count && newSegment.End.CompareTo(this._segments[num].Start) > 0)
			{
				throw new InvalidOperationException(SR.Get("TextSegmentsMustNotOverlap"));
			}
			this._segments.Insert(num, newSegment);
		}

		// Token: 0x06006260 RID: 25184 RVA: 0x001B96CC File Offset: 0x001B78CC
		private static TextSegment CreateNormalizedSegment(ITextPointer start, ITextPointer end)
		{
			if (start.CompareTo(end) == 0)
			{
				if (!TextPointerBase.IsAtInsertionPosition(start, start.LogicalDirection))
				{
					start = start.GetInsertionPosition(start.LogicalDirection);
					end = start;
				}
			}
			else
			{
				if (!TextPointerBase.IsAtInsertionPosition(start, start.LogicalDirection))
				{
					start = start.GetInsertionPosition(LogicalDirection.Forward);
				}
				if (!TextPointerBase.IsAtInsertionPosition(end, start.LogicalDirection))
				{
					end = end.GetInsertionPosition(LogicalDirection.Backward);
				}
				if (start.CompareTo(end) >= 0)
				{
					if (start.LogicalDirection == LogicalDirection.Backward)
					{
						start = end.GetFrozenPointer(LogicalDirection.Backward);
					}
					end = start;
				}
			}
			return new TextSegment(start, end);
		}

		// Token: 0x0400318D RID: 12685
		private List<TextSegment> _segments = new List<TextSegment>(1);

		// Token: 0x020009FE RID: 2558
		private class TextSegmentComparer : IComparer<TextSegment>
		{
			// Token: 0x060089E3 RID: 35299 RVA: 0x002568FC File Offset: 0x00254AFC
			public int Compare(TextSegment x, TextSegment y)
			{
				if (x.Equals(TextSegment.Null))
				{
					if (y.Equals(TextSegment.Null))
					{
						return 0;
					}
					return -1;
				}
				else
				{
					if (y.Equals(TextSegment.Null))
					{
						return 1;
					}
					int num = x.Start.CompareTo(y.Start);
					if (num != 0)
					{
						return num;
					}
					return x.End.CompareTo(y.End);
				}
			}
		}
	}
}
