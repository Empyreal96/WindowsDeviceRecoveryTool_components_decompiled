using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using MS.Internal.Annotations.Anchoring;

namespace MS.Internal.Annotations.Component
{
	// Token: 0x020007E0 RID: 2016
	internal class AnnotationHighlightLayer : HighlightLayer
	{
		// Token: 0x06007C95 RID: 31893 RVA: 0x00230A1B File Offset: 0x0022EC1B
		internal AnnotationHighlightLayer()
		{
			this._segments = new List<AnnotationHighlightLayer.HighlightSegment>();
		}

		// Token: 0x06007C96 RID: 31894 RVA: 0x00230A30 File Offset: 0x0022EC30
		internal void AddRange(IHighlightRange highlightRange)
		{
			Invariant.Assert(highlightRange != null, "the owner is null");
			ITextPointer start = highlightRange.Range.Start;
			ITextPointer end = highlightRange.Range.End;
			if (start.CompareTo(end) == 0)
			{
				return;
			}
			if (this._segments.Count == 0)
			{
				object textContainer = start.TextContainer;
				this.IsFixedContainer = (textContainer is FixedTextContainer || textContainer is DocumentSequenceTextContainer);
			}
			ITextPointer start2;
			ITextPointer end2;
			this.ProcessOverlapingSegments(highlightRange, out start2, out end2);
			if (this.Changed != null && this.IsFixedContainer)
			{
				this.Changed(this, new AnnotationHighlightLayer.AnnotationHighlightChangedEventArgs(start2, end2));
			}
		}

		// Token: 0x06007C97 RID: 31895 RVA: 0x00230ACC File Offset: 0x0022ECCC
		internal void RemoveRange(IHighlightRange highlightRange)
		{
			if (highlightRange.Range.Start.CompareTo(highlightRange.Range.End) == 0)
			{
				return;
			}
			int num;
			int num2;
			this.GetSpannedSegments(highlightRange.Range.Start, highlightRange.Range.End, out num, out num2);
			ITextPointer start = this._segments[num].Segment.Start;
			ITextPointer end = this._segments[num2].Segment.End;
			int i = num;
			while (i <= num2)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[i];
				if (highlightSegment.RemoveOwner(highlightRange) == 0)
				{
					this._segments.Remove(highlightSegment);
					num2--;
				}
				else
				{
					i++;
				}
			}
			if (this.Changed != null && this.IsFixedContainer)
			{
				this.Changed(this, new AnnotationHighlightLayer.AnnotationHighlightChangedEventArgs(start, end));
			}
		}

		// Token: 0x06007C98 RID: 31896 RVA: 0x00230BB4 File Offset: 0x0022EDB4
		internal void ModifiedRange(IHighlightRange highlightRange)
		{
			Invariant.Assert(highlightRange != null, "null range data");
			if (highlightRange.Range.Start.CompareTo(highlightRange.Range.End) == 0)
			{
				return;
			}
			int num;
			int num2;
			this.GetSpannedSegments(highlightRange.Range.Start, highlightRange.Range.End, out num, out num2);
			for (int i = num; i < num2; i++)
			{
				this._segments[i].UpdateOwners();
			}
			ITextPointer start = this._segments[num].Segment.Start;
			ITextPointer end = this._segments[num2].Segment.End;
			if (this.Changed != null && this.IsFixedContainer)
			{
				this.Changed(this, new AnnotationHighlightLayer.AnnotationHighlightChangedEventArgs(start, end));
			}
		}

		// Token: 0x06007C99 RID: 31897 RVA: 0x00230C88 File Offset: 0x0022EE88
		internal void ActivateRange(IHighlightRange highlightRange, bool activate)
		{
			Invariant.Assert(highlightRange != null, "null range data");
			if (highlightRange.Range.Start.CompareTo(highlightRange.Range.End) == 0)
			{
				return;
			}
			int num;
			int num2;
			this.GetSpannedSegments(highlightRange.Range.Start, highlightRange.Range.End, out num, out num2);
			ITextPointer start = this._segments[num].Segment.Start;
			ITextPointer end = this._segments[num2].Segment.End;
			for (int i = num; i <= num2; i++)
			{
				if (activate)
				{
					this._segments[i].AddActiveOwner(highlightRange);
				}
				else
				{
					this._segments[i].RemoveActiveOwner(highlightRange);
				}
			}
			if (this.Changed != null && this.IsFixedContainer)
			{
				this.Changed(this, new AnnotationHighlightLayer.AnnotationHighlightChangedEventArgs(start, end));
			}
		}

		// Token: 0x06007C9A RID: 31898 RVA: 0x00230D78 File Offset: 0x0022EF78
		internal override object GetHighlightValue(StaticTextPointer textPosition, LogicalDirection direction)
		{
			object result = DependencyProperty.UnsetValue;
			for (int i = 0; i < this._segments.Count; i++)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[i];
				if (highlightSegment.Segment.Start.CompareTo(textPosition) > 0 || (highlightSegment.Segment.Start.CompareTo(textPosition) == 0 && direction == LogicalDirection.Backward))
				{
					break;
				}
				if (highlightSegment.Segment.End.CompareTo(textPosition) > 0 || (highlightSegment.Segment.End.CompareTo(textPosition) == 0 && direction == LogicalDirection.Backward))
				{
					result = highlightSegment;
					break;
				}
			}
			return result;
		}

		// Token: 0x06007C9B RID: 31899 RVA: 0x00230E18 File Offset: 0x0022F018
		internal override bool IsContentHighlighted(StaticTextPointer staticTextPosition, LogicalDirection direction)
		{
			return this.GetHighlightValue(staticTextPosition, direction) != DependencyProperty.UnsetValue;
		}

		// Token: 0x06007C9C RID: 31900 RVA: 0x00230E2C File Offset: 0x0022F02C
		internal override StaticTextPointer GetNextChangePosition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			ITextPointer textPointer;
			if (direction == LogicalDirection.Forward)
			{
				textPointer = this.GetNextForwardPosition(textPosition);
			}
			else
			{
				textPointer = this.GetNextBackwardPosition(textPosition);
			}
			if (textPointer != null)
			{
				return textPointer.CreateStaticPointer();
			}
			return StaticTextPointer.Null;
		}

		// Token: 0x17001CF0 RID: 7408
		// (get) Token: 0x06007C9D RID: 31901 RVA: 0x00230E5E File Offset: 0x0022F05E
		internal override Type OwnerType
		{
			get
			{
				return typeof(HighlightComponent);
			}
		}

		// Token: 0x1400016E RID: 366
		// (add) Token: 0x06007C9E RID: 31902 RVA: 0x00230E6C File Offset: 0x0022F06C
		// (remove) Token: 0x06007C9F RID: 31903 RVA: 0x00230EA4 File Offset: 0x0022F0A4
		internal override event HighlightChangedEventHandler Changed;

		// Token: 0x06007CA0 RID: 31904 RVA: 0x00230EDC File Offset: 0x0022F0DC
		private void ProcessOverlapingSegments(IHighlightRange highlightRange, out ITextPointer invalidateStart, out ITextPointer invalidateEnd)
		{
			ReadOnlyCollection<TextSegment> textSegments = highlightRange.Range.TextSegments;
			invalidateStart = null;
			invalidateEnd = null;
			int num = 0;
			IEnumerator<TextSegment> enumerator = textSegments.GetEnumerator();
			TextSegment textSegment = enumerator.MoveNext() ? enumerator.Current : TextSegment.Null;
			while (num < this._segments.Count && !textSegment.IsNull)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[num];
				if (highlightSegment.Segment.Start.CompareTo(textSegment.Start) <= 0)
				{
					if (highlightSegment.Segment.End.CompareTo(textSegment.Start) > 0)
					{
						IList<AnnotationHighlightLayer.HighlightSegment> list = highlightSegment.Split(textSegment.Start, textSegment.End, highlightRange);
						if (!list.Contains(highlightSegment))
						{
							highlightSegment.ClearOwners();
						}
						this._segments.Remove(highlightSegment);
						this._segments.InsertRange(num, list);
						num = num + list.Count - 1;
						if (textSegment.End.CompareTo(highlightSegment.Segment.End) <= 0)
						{
							textSegment = (enumerator.MoveNext() ? enumerator.Current : TextSegment.Null);
						}
						else
						{
							textSegment = new TextSegment(highlightSegment.Segment.End, textSegment.End);
						}
						if (invalidateStart == null)
						{
							invalidateStart = highlightSegment.Segment.Start;
						}
					}
					else
					{
						num++;
					}
				}
				else
				{
					if (invalidateStart == null)
					{
						invalidateStart = textSegment.Start;
					}
					if (textSegment.End.CompareTo(highlightSegment.Segment.Start) > 0)
					{
						AnnotationHighlightLayer.HighlightSegment item = new AnnotationHighlightLayer.HighlightSegment(textSegment.Start, highlightSegment.Segment.Start, highlightRange);
						this._segments.Insert(num++, item);
						textSegment = new TextSegment(highlightSegment.Segment.Start, textSegment.End);
					}
					else
					{
						this._segments.Insert(num++, new AnnotationHighlightLayer.HighlightSegment(textSegment.Start, textSegment.End, highlightRange));
						textSegment = (enumerator.MoveNext() ? enumerator.Current : TextSegment.Null);
					}
				}
			}
			if (!textSegment.IsNull)
			{
				if (invalidateStart == null)
				{
					invalidateStart = textSegment.Start;
				}
				this._segments.Insert(num++, new AnnotationHighlightLayer.HighlightSegment(textSegment.Start, textSegment.End, highlightRange));
			}
			while (enumerator.MoveNext())
			{
				List<AnnotationHighlightLayer.HighlightSegment> segments = this._segments;
				int index = num++;
				TextSegment textSegment2 = enumerator.Current;
				ITextPointer start = textSegment2.Start;
				textSegment2 = enumerator.Current;
				segments.Insert(index, new AnnotationHighlightLayer.HighlightSegment(start, textSegment2.End, highlightRange));
			}
			if (invalidateStart != null)
			{
				if (num == this._segments.Count)
				{
					num--;
				}
				invalidateEnd = this._segments[num].Segment.End;
			}
		}

		// Token: 0x06007CA1 RID: 31905 RVA: 0x002311C0 File Offset: 0x0022F3C0
		private ITextPointer GetNextForwardPosition(StaticTextPointer pos)
		{
			for (int i = 0; i < this._segments.Count; i++)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[i];
				if (pos.CompareTo(highlightSegment.Segment.Start) < 0)
				{
					return highlightSegment.Segment.Start;
				}
				if (pos.CompareTo(highlightSegment.Segment.End) < 0)
				{
					return highlightSegment.Segment.End;
				}
			}
			return null;
		}

		// Token: 0x06007CA2 RID: 31906 RVA: 0x00231240 File Offset: 0x0022F440
		private ITextPointer GetNextBackwardPosition(StaticTextPointer pos)
		{
			for (int i = this._segments.Count - 1; i >= 0; i--)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[i];
				if (pos.CompareTo(highlightSegment.Segment.End) > 0)
				{
					return highlightSegment.Segment.End;
				}
				if (pos.CompareTo(highlightSegment.Segment.Start) > 0)
				{
					return highlightSegment.Segment.Start;
				}
			}
			return null;
		}

		// Token: 0x06007CA3 RID: 31907 RVA: 0x002312C4 File Offset: 0x0022F4C4
		private void GetSpannedSegments(ITextPointer start, ITextPointer end, out int startSeg, out int endSeg)
		{
			startSeg = -1;
			endSeg = -1;
			for (int i = 0; i < this._segments.Count; i++)
			{
				AnnotationHighlightLayer.HighlightSegment highlightSegment = this._segments[i];
				if (highlightSegment.Segment.Start.CompareTo(start) == 0)
				{
					startSeg = i;
				}
				if (highlightSegment.Segment.End.CompareTo(end) == 0)
				{
					endSeg = i;
					break;
				}
			}
			if (startSeg >= 0 && endSeg >= 0)
			{
				int num = startSeg;
				int num2 = endSeg;
			}
		}

		// Token: 0x17001CF1 RID: 7409
		// (get) Token: 0x06007CA4 RID: 31908 RVA: 0x00231342 File Offset: 0x0022F542
		// (set) Token: 0x06007CA5 RID: 31909 RVA: 0x0023134A File Offset: 0x0022F54A
		private bool IsFixedContainer
		{
			get
			{
				return this._isFixedContainer;
			}
			set
			{
				this._isFixedContainer = value;
			}
		}

		// Token: 0x04003A6C RID: 14956
		private List<AnnotationHighlightLayer.HighlightSegment> _segments;

		// Token: 0x04003A6D RID: 14957
		private bool _isFixedContainer;

		// Token: 0x02000B84 RID: 2948
		private class AnnotationHighlightChangedEventArgs : HighlightChangedEventArgs
		{
			// Token: 0x06008E54 RID: 36436 RVA: 0x0025BA80 File Offset: 0x00259C80
			internal AnnotationHighlightChangedEventArgs(ITextPointer start, ITextPointer end)
			{
				TextSegment[] list = new TextSegment[]
				{
					new TextSegment(start, end)
				};
				this._ranges = new ReadOnlyCollection<TextSegment>(list);
			}

			// Token: 0x17001FAB RID: 8107
			// (get) Token: 0x06008E55 RID: 36437 RVA: 0x0025BAB4 File Offset: 0x00259CB4
			internal override IList Ranges
			{
				get
				{
					return this._ranges;
				}
			}

			// Token: 0x17001FAC RID: 8108
			// (get) Token: 0x06008E56 RID: 36438 RVA: 0x00230E5E File Offset: 0x0022F05E
			internal override Type OwnerType
			{
				get
				{
					return typeof(HighlightComponent);
				}
			}

			// Token: 0x04004B8A RID: 19338
			private readonly ReadOnlyCollection<TextSegment> _ranges;
		}

		// Token: 0x02000B85 RID: 2949
		internal sealed class HighlightSegment : Shape
		{
			// Token: 0x06008E57 RID: 36439 RVA: 0x0025BABC File Offset: 0x00259CBC
			internal HighlightSegment(ITextPointer start, ITextPointer end, IHighlightRange owner)
			{
				List<IHighlightRange> list = new List<IHighlightRange>(1);
				list.Add(owner);
				this.Init(start, end, list);
				this._owners = list;
				this.UpdateOwners();
			}

			// Token: 0x06008E58 RID: 36440 RVA: 0x0025BB0C File Offset: 0x00259D0C
			internal HighlightSegment(ITextPointer start, ITextPointer end, IList<IHighlightRange> owners)
			{
				this.Init(start, end, owners);
				this._owners = new List<IHighlightRange>(owners.Count);
				this._owners.AddRange(owners);
				this.UpdateOwners();
			}

			// Token: 0x06008E59 RID: 36441 RVA: 0x0025BB64 File Offset: 0x00259D64
			private void Init(ITextPointer start, ITextPointer end, IList<IHighlightRange> owners)
			{
				for (int i = 0; i < owners.Count; i++)
				{
				}
				this._segment = new TextSegment(start, end);
				base.IsHitTestVisible = false;
				object textContainer = start.TextContainer;
				this._isFixedContainer = (textContainer is FixedTextContainer || textContainer is DocumentSequenceTextContainer);
				this.GetContent();
			}

			// Token: 0x06008E5A RID: 36442 RVA: 0x0025BBC0 File Offset: 0x00259DC0
			internal void AddOwner(IHighlightRange owner)
			{
				for (int i = 0; i < this._owners.Count; i++)
				{
					if (this._owners[i].Priority < owner.Priority)
					{
						this._owners.Insert(i, owner);
						this.UpdateOwners();
						return;
					}
				}
				this._owners.Add(owner);
				this.UpdateOwners();
			}

			// Token: 0x06008E5B RID: 36443 RVA: 0x0025BC24 File Offset: 0x00259E24
			internal int RemoveOwner(IHighlightRange owner)
			{
				if (this._owners.Contains(owner))
				{
					if (this._activeOwners.Contains(owner))
					{
						this._activeOwners.Remove(owner);
					}
					this._owners.Remove(owner);
					this.UpdateOwners();
				}
				return this._owners.Count;
			}

			// Token: 0x06008E5C RID: 36444 RVA: 0x0025BC78 File Offset: 0x00259E78
			internal void AddActiveOwner(IHighlightRange owner)
			{
				if (this._owners.Contains(owner))
				{
					this._activeOwners.Add(owner);
					this.UpdateOwners();
				}
			}

			// Token: 0x06008E5D RID: 36445 RVA: 0x0025BC9A File Offset: 0x00259E9A
			private void AddActiveOwners(List<IHighlightRange> owners)
			{
				this._activeOwners.AddRange(owners);
				this.UpdateOwners();
			}

			// Token: 0x06008E5E RID: 36446 RVA: 0x0025BCAE File Offset: 0x00259EAE
			internal void RemoveActiveOwner(IHighlightRange owner)
			{
				if (this._activeOwners.Contains(owner))
				{
					this._activeOwners.Remove(owner);
					this.UpdateOwners();
				}
			}

			// Token: 0x06008E5F RID: 36447 RVA: 0x0025BCD1 File Offset: 0x00259ED1
			internal void ClearOwners()
			{
				this._owners.Clear();
				this._activeOwners.Clear();
				this.UpdateOwners();
			}

			// Token: 0x06008E60 RID: 36448 RVA: 0x0025BCF0 File Offset: 0x00259EF0
			internal IList<AnnotationHighlightLayer.HighlightSegment> Split(ITextPointer ps, LogicalDirection side)
			{
				IList<AnnotationHighlightLayer.HighlightSegment> list = null;
				if (ps.CompareTo(this._segment.Start) == 0 || ps.CompareTo(this._segment.End) == 0)
				{
					if ((ps.CompareTo(this._segment.Start) == 0 && side == LogicalDirection.Forward) || (ps.CompareTo(this._segment.End) == 0 && side == LogicalDirection.Backward))
					{
						list = new List<AnnotationHighlightLayer.HighlightSegment>(1);
						list.Add(this);
					}
				}
				else if (this._segment.Contains(ps))
				{
					list = new List<AnnotationHighlightLayer.HighlightSegment>(2);
					list.Add(new AnnotationHighlightLayer.HighlightSegment(this._segment.Start, ps, this._owners));
					list.Add(new AnnotationHighlightLayer.HighlightSegment(ps, this._segment.End, this._owners));
					list[0].AddActiveOwners(this._activeOwners);
					list[1].AddActiveOwners(this._activeOwners);
				}
				return list;
			}

			// Token: 0x06008E61 RID: 36449 RVA: 0x0025BDDC File Offset: 0x00259FDC
			internal IList<AnnotationHighlightLayer.HighlightSegment> Split(ITextPointer ps1, ITextPointer ps2, IHighlightRange newOwner)
			{
				IList<AnnotationHighlightLayer.HighlightSegment> list = new List<AnnotationHighlightLayer.HighlightSegment>();
				if (ps1.CompareTo(ps2) == 0)
				{
					if (this._segment.Start.CompareTo(ps1) > 0 || this._segment.End.CompareTo(ps1) < 0)
					{
						return list;
					}
					if (this._segment.Start.CompareTo(ps1) < 0)
					{
						list.Add(new AnnotationHighlightLayer.HighlightSegment(this._segment.Start, ps1, this._owners));
					}
					list.Add(new AnnotationHighlightLayer.HighlightSegment(ps1, ps1, this._owners));
					if (this._segment.End.CompareTo(ps1) > 0)
					{
						list.Add(new AnnotationHighlightLayer.HighlightSegment(ps1, this._segment.End, this._owners));
					}
					using (IEnumerator<AnnotationHighlightLayer.HighlightSegment> enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AnnotationHighlightLayer.HighlightSegment highlightSegment = enumerator.Current;
							highlightSegment.AddActiveOwners(this._activeOwners);
						}
						goto IL_1A1;
					}
				}
				if (this._segment.Contains(ps1))
				{
					IList<AnnotationHighlightLayer.HighlightSegment> list2 = this.Split(ps1, LogicalDirection.Forward);
					for (int i = 0; i < list2.Count; i++)
					{
						if (list2[i].Segment.Contains(ps2))
						{
							IList<AnnotationHighlightLayer.HighlightSegment> list3 = list2[i].Split(ps2, LogicalDirection.Backward);
							for (int j = 0; j < list3.Count; j++)
							{
								list.Add(list3[j]);
							}
							if (!list3.Contains(list2[i]))
							{
								list2[i].Discard();
							}
						}
						else
						{
							list.Add(list2[i]);
						}
					}
				}
				else
				{
					list = this.Split(ps2, LogicalDirection.Backward);
				}
				IL_1A1:
				if (list != null && list.Count > 0 && newOwner != null)
				{
					if (list.Count == 3)
					{
						list[1].AddOwner(newOwner);
					}
					else if (list[0].Segment.Start.CompareTo(ps1) == 0 || list[0].Segment.End.CompareTo(ps2) == 0)
					{
						list[0].AddOwner(newOwner);
					}
					else
					{
						list[1].AddOwner(newOwner);
					}
				}
				return list;
			}

			// Token: 0x06008E62 RID: 36450 RVA: 0x0025C018 File Offset: 0x0025A218
			internal void UpdateOwners()
			{
				if (this._cachedTopOwner != this.TopOwner)
				{
					if (this._cachedTopOwner != null)
					{
						this._cachedTopOwner.RemoveChild(this);
					}
					this._cachedTopOwner = this.TopOwner;
					if (this._cachedTopOwner != null)
					{
						this._cachedTopOwner.AddChild(this);
					}
				}
				base.Fill = this.OwnerColor;
			}

			// Token: 0x06008E63 RID: 36451 RVA: 0x0025C073 File Offset: 0x0025A273
			internal void Discard()
			{
				if (this.TopOwner != null)
				{
					this.TopOwner.RemoveChild(this);
				}
				this._activeOwners.Clear();
				this._owners.Clear();
			}

			// Token: 0x06008E64 RID: 36452 RVA: 0x0025C0A0 File Offset: 0x0025A2A0
			private void GetSegmentGeometry(GeometryGroup geometry, TextSegment segment, ITextView parentView)
			{
				List<ITextView> documentPageTextViews = TextSelectionHelper.GetDocumentPageTextViews(segment);
				foreach (ITextView view in documentPageTextViews)
				{
					Geometry pageGeometry = this.GetPageGeometry(segment, view, parentView);
					if (pageGeometry != null)
					{
						geometry.Children.Add(pageGeometry);
					}
				}
			}

			// Token: 0x06008E65 RID: 36453 RVA: 0x0025C108 File Offset: 0x0025A308
			private Geometry GetPageGeometry(TextSegment segment, ITextView view, ITextView parentView)
			{
				if (!view.IsValid || !parentView.IsValid)
				{
					return null;
				}
				if (view.RenderScope == null || parentView.RenderScope == null)
				{
					return null;
				}
				Geometry tightBoundingGeometryFromTextPositions = view.GetTightBoundingGeometryFromTextPositions(segment.Start, segment.End);
				if (tightBoundingGeometryFromTextPositions != null && parentView != null)
				{
					Transform transform = (Transform)view.RenderScope.TransformToVisual(parentView.RenderScope);
					if (tightBoundingGeometryFromTextPositions.Transform != null)
					{
						tightBoundingGeometryFromTextPositions.Transform = new TransformGroup
						{
							Children = 
							{
								tightBoundingGeometryFromTextPositions.Transform,
								transform
							}
						};
					}
					else
					{
						tightBoundingGeometryFromTextPositions.Transform = transform;
					}
				}
				return tightBoundingGeometryFromTextPositions;
			}

			// Token: 0x06008E66 RID: 36454 RVA: 0x0025C1AC File Offset: 0x0025A3AC
			private void GetContent()
			{
				this._contentSegments.Clear();
				ITextPointer textPointer = this._segment.Start.CreatePointer();
				ITextPointer textPointer2 = null;
				while (textPointer.CompareTo(this._segment.End) < 0)
				{
					TextPointerContext pointerContext = textPointer.GetPointerContext(LogicalDirection.Forward);
					if (pointerContext == TextPointerContext.ElementStart)
					{
						Type elementType = textPointer.GetElementType(LogicalDirection.Forward);
						if (typeof(Run).IsAssignableFrom(elementType) || typeof(BlockUIContainer).IsAssignableFrom(elementType))
						{
							this.OpenSegment(ref textPointer2, textPointer);
						}
						else if (typeof(Table).IsAssignableFrom(elementType) || typeof(Floater).IsAssignableFrom(elementType) || typeof(Figure).IsAssignableFrom(elementType))
						{
							this.CloseSegment(ref textPointer2, textPointer, this._segment.End);
						}
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
						if (typeof(Run).IsAssignableFrom(elementType) || typeof(BlockUIContainer).IsAssignableFrom(elementType))
						{
							textPointer.MoveToElementEdge(ElementEdge.AfterEnd);
						}
					}
					else if (pointerContext == TextPointerContext.ElementEnd)
					{
						Type parentType = textPointer.ParentType;
						if (typeof(TableCell).IsAssignableFrom(parentType) || typeof(Floater).IsAssignableFrom(parentType) || typeof(Figure).IsAssignableFrom(parentType))
						{
							this.CloseSegment(ref textPointer2, textPointer, this._segment.End);
						}
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					else if (pointerContext == TextPointerContext.Text || pointerContext == TextPointerContext.EmbeddedElement)
					{
						this.OpenSegment(ref textPointer2, textPointer);
						textPointer.MoveToNextContextPosition(LogicalDirection.Forward);
					}
					else
					{
						Invariant.Assert(false, "unexpected TextPointerContext");
					}
				}
				this.CloseSegment(ref textPointer2, textPointer, this._segment.End);
			}

			// Token: 0x06008E67 RID: 36455 RVA: 0x0025C35B File Offset: 0x0025A55B
			private void OpenSegment(ref ITextPointer segmentStart, ITextPointer cursor)
			{
				if (segmentStart == null)
				{
					segmentStart = cursor.GetInsertionPosition(LogicalDirection.Forward);
				}
			}

			// Token: 0x06008E68 RID: 36456 RVA: 0x0025C36C File Offset: 0x0025A56C
			private void CloseSegment(ref ITextPointer segmentStart, ITextPointer cursor, ITextPointer end)
			{
				if (segmentStart != null)
				{
					if (cursor.CompareTo(end) > 0)
					{
						cursor = end;
					}
					ITextPointer insertionPosition = cursor.GetInsertionPosition(LogicalDirection.Backward);
					if (segmentStart.CompareTo(insertionPosition) < 0)
					{
						this._contentSegments.Add(new TextSegment(segmentStart, insertionPosition));
					}
					segmentStart = null;
				}
			}

			// Token: 0x17001FAD RID: 8109
			// (get) Token: 0x06008E69 RID: 36457 RVA: 0x0025C3B4 File Offset: 0x0025A5B4
			protected override Geometry DefiningGeometry
			{
				get
				{
					if (this._isFixedContainer)
					{
						return Geometry.Empty;
					}
					ITextView documentPageTextView = TextSelectionHelper.GetDocumentPageTextView(this.TopOwner.Range.Start.CreatePointer(LogicalDirection.Forward));
					GeometryGroup geometryGroup = new GeometryGroup();
					if (this.TopOwner.HighlightContent)
					{
						using (List<TextSegment>.Enumerator enumerator = this._contentSegments.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								TextSegment segment = enumerator.Current;
								this.GetSegmentGeometry(geometryGroup, segment, documentPageTextView);
							}
							goto IL_85;
						}
					}
					this.GetSegmentGeometry(geometryGroup, this._segment, documentPageTextView);
					IL_85:
					UIElement uielement = this.TopOwner as UIElement;
					if (uielement != null)
					{
						uielement.RenderTransform = Transform.Identity;
					}
					return geometryGroup;
				}
			}

			// Token: 0x17001FAE RID: 8110
			// (get) Token: 0x06008E6A RID: 36458 RVA: 0x0025C474 File Offset: 0x0025A674
			internal TextSegment Segment
			{
				get
				{
					return this._segment;
				}
			}

			// Token: 0x17001FAF RID: 8111
			// (get) Token: 0x06008E6B RID: 36459 RVA: 0x0025C47C File Offset: 0x0025A67C
			internal IHighlightRange TopOwner
			{
				get
				{
					if (this._activeOwners.Count != 0)
					{
						return this._activeOwners[0];
					}
					if (this._owners.Count <= 0)
					{
						return null;
					}
					return this._owners[0];
				}
			}

			// Token: 0x17001FB0 RID: 8112
			// (get) Token: 0x06008E6C RID: 36460 RVA: 0x0025C4B4 File Offset: 0x0025A6B4
			private Brush OwnerColor
			{
				get
				{
					if (this._activeOwners.Count != 0)
					{
						return new SolidColorBrush(this._activeOwners[0].SelectedBackground);
					}
					if (this._owners.Count <= 0)
					{
						return null;
					}
					return new SolidColorBrush(this._owners[0].Background);
				}
			}

			// Token: 0x04004B8B RID: 19339
			private TextSegment _segment;

			// Token: 0x04004B8C RID: 19340
			private List<TextSegment> _contentSegments = new List<TextSegment>(1);

			// Token: 0x04004B8D RID: 19341
			private readonly List<IHighlightRange> _owners;

			// Token: 0x04004B8E RID: 19342
			private List<IHighlightRange> _activeOwners = new List<IHighlightRange>();

			// Token: 0x04004B8F RID: 19343
			private IHighlightRange _cachedTopOwner;

			// Token: 0x04004B90 RID: 19344
			private bool _isFixedContainer;
		}
	}
}
