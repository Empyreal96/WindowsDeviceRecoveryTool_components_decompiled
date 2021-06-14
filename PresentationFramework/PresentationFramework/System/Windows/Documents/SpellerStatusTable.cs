using System;
using System.Collections;
using MS.Internal;

namespace System.Windows.Documents
{
	// Token: 0x020003DE RID: 990
	internal class SpellerStatusTable
	{
		// Token: 0x060035A4 RID: 13732 RVA: 0x000F3876 File Offset: 0x000F1A76
		internal SpellerStatusTable(ITextPointer textContainerStart, SpellerHighlightLayer highlightLayer)
		{
			this._highlightLayer = highlightLayer;
			this._runList = new ArrayList(1);
			this._runList.Add(new SpellerStatusTable.Run(textContainerStart, SpellerStatusTable.RunType.Dirty));
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x000F38A4 File Offset: 0x000F1AA4
		internal void OnTextChange(TextContainerChangeEventArgs e)
		{
			if (e.TextChange == TextChangeType.ContentAdded)
			{
				this.OnContentAdded(e);
			}
			else if (e.TextChange == TextChangeType.ContentRemoved)
			{
				this.OnContentRemoved(e.ITextPosition);
			}
			else
			{
				ITextPointer textPointer = e.ITextPosition.CreatePointer(e.Count);
				textPointer.Freeze();
				this.MarkDirtyRange(e.ITextPosition, textPointer);
			}
			this.DebugAssertRunList();
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x000F3904 File Offset: 0x000F1B04
		internal void GetFirstDirtyRange(ITextPointer searchStart, out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			int num = this.FindIndex(searchStart.CreateStaticPointer(), LogicalDirection.Forward);
			while (num >= 0 && num < this._runList.Count)
			{
				SpellerStatusTable.Run run = this.GetRun(num);
				if (run.RunType == SpellerStatusTable.RunType.Dirty)
				{
					start = TextPointerBase.Max(searchStart, run.Position);
					end = this.GetRunEndPositionDynamic(num);
					return;
				}
				num++;
			}
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x000F3966 File Offset: 0x000F1B66
		internal void MarkCleanRange(ITextPointer start, ITextPointer end)
		{
			this.MarkRange(start, end, SpellerStatusTable.RunType.Clean);
			this.DebugAssertRunList();
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x000F3977 File Offset: 0x000F1B77
		internal void MarkDirtyRange(ITextPointer start, ITextPointer end)
		{
			this.MarkRange(start, end, SpellerStatusTable.RunType.Dirty);
			this.DebugAssertRunList();
		}

		// Token: 0x060035A9 RID: 13737 RVA: 0x000F3988 File Offset: 0x000F1B88
		internal void MarkErrorRange(ITextPointer start, ITextPointer end)
		{
			int num = this.FindIndex(start.CreateStaticPointer(), LogicalDirection.Forward);
			SpellerStatusTable.Run run = this.GetRun(num);
			Invariant.Assert(run.RunType == SpellerStatusTable.RunType.Clean);
			Invariant.Assert(run.Position.CompareTo(start) <= 0);
			Invariant.Assert(this.GetRunEndPosition(num).CompareTo(end) >= 0);
			if (run.Position.CompareTo(start) == 0)
			{
				run.RunType = SpellerStatusTable.RunType.Error;
			}
			else
			{
				this._runList.Insert(num + 1, new SpellerStatusTable.Run(start, SpellerStatusTable.RunType.Error));
				num++;
			}
			if (this.GetRunEndPosition(num).CompareTo(end) > 0)
			{
				this._runList.Insert(num + 1, new SpellerStatusTable.Run(end, SpellerStatusTable.RunType.Clean));
			}
			this._highlightLayer.FireChangedEvent(start, end);
			this.DebugAssertRunList();
		}

		// Token: 0x060035AA RID: 13738 RVA: 0x000F3A58 File Offset: 0x000F1C58
		internal bool IsRunType(StaticTextPointer textPosition, LogicalDirection direction, SpellerStatusTable.RunType runType)
		{
			int num = this.FindIndex(textPosition, direction);
			return num >= 0 && this.GetRun(num).RunType == runType;
		}

		// Token: 0x060035AB RID: 13739 RVA: 0x000F3A84 File Offset: 0x000F1C84
		internal StaticTextPointer GetNextErrorTransition(StaticTextPointer textPosition, LogicalDirection direction)
		{
			StaticTextPointer staticTextPointer = StaticTextPointer.Null;
			int num = this.FindIndex(textPosition, direction);
			if (num != -1)
			{
				if (direction == LogicalDirection.Forward)
				{
					if (this.IsErrorRun(num))
					{
						staticTextPointer = this.GetRunEndPosition(num);
					}
					else
					{
						for (int i = num + 1; i < this._runList.Count; i++)
						{
							if (this.IsErrorRun(i))
							{
								staticTextPointer = this.GetRun(i).Position.CreateStaticPointer();
								break;
							}
						}
					}
				}
				else if (this.IsErrorRun(num))
				{
					staticTextPointer = this.GetRun(num).Position.CreateStaticPointer();
				}
				else
				{
					for (int i = num - 1; i > 0; i--)
					{
						if (this.IsErrorRun(i))
						{
							staticTextPointer = this.GetRunEndPosition(i);
							break;
						}
					}
				}
			}
			Invariant.Assert(staticTextPointer.IsNull || textPosition.CompareTo(staticTextPointer) != 0);
			return staticTextPointer;
		}

		// Token: 0x060035AC RID: 13740 RVA: 0x000F3B50 File Offset: 0x000F1D50
		internal bool GetError(StaticTextPointer textPosition, LogicalDirection direction, out ITextPointer start, out ITextPointer end)
		{
			start = null;
			end = null;
			int errorIndex = this.GetErrorIndex(textPosition, direction);
			if (errorIndex >= 0)
			{
				start = this.GetRun(errorIndex).Position;
				end = this.GetRunEndPositionDynamic(errorIndex);
			}
			return start != null;
		}

		// Token: 0x060035AD RID: 13741 RVA: 0x000F3B90 File Offset: 0x000F1D90
		internal bool GetRun(StaticTextPointer position, LogicalDirection direction, out SpellerStatusTable.RunType runType, out StaticTextPointer end)
		{
			int num = this.FindIndex(position, direction);
			runType = SpellerStatusTable.RunType.Clean;
			end = StaticTextPointer.Null;
			if (num < 0)
			{
				return false;
			}
			SpellerStatusTable.Run run = this.GetRun(num);
			runType = run.RunType;
			end = ((direction == LogicalDirection.Forward) ? this.GetRunEndPosition(num) : run.Position.CreateStaticPointer());
			return true;
		}

		// Token: 0x060035AE RID: 13742 RVA: 0x000F3BEC File Offset: 0x000F1DEC
		private int GetErrorIndex(StaticTextPointer textPosition, LogicalDirection direction)
		{
			int num = this.FindIndex(textPosition, direction);
			if (num >= 0)
			{
				SpellerStatusTable.Run run = this.GetRun(num);
				if (run.RunType == SpellerStatusTable.RunType.Clean || run.RunType == SpellerStatusTable.RunType.Dirty)
				{
					num = -1;
				}
			}
			return num;
		}

		// Token: 0x060035AF RID: 13743 RVA: 0x000F3C24 File Offset: 0x000F1E24
		private int FindIndex(StaticTextPointer position, LogicalDirection direction)
		{
			int num = -1;
			int i = 0;
			int num2 = this._runList.Count;
			while (i < num2)
			{
				num = (i + num2) / 2;
				SpellerStatusTable.Run run = this.GetRun(num);
				if ((direction == LogicalDirection.Forward && position.CompareTo(run.Position) < 0) || (direction == LogicalDirection.Backward && position.CompareTo(run.Position) <= 0))
				{
					num2 = num;
				}
				else
				{
					if ((direction != LogicalDirection.Forward || position.CompareTo(this.GetRunEndPosition(num)) < 0) && (direction != LogicalDirection.Backward || position.CompareTo(this.GetRunEndPosition(num)) <= 0))
					{
						break;
					}
					i = num + 1;
				}
			}
			if (i >= num2)
			{
				num = -1;
			}
			return num;
		}

		// Token: 0x060035B0 RID: 13744 RVA: 0x000F3CB4 File Offset: 0x000F1EB4
		private void MarkRange(ITextPointer start, ITextPointer end, SpellerStatusTable.RunType runType)
		{
			if (start.CompareTo(end) == 0)
			{
				return;
			}
			Invariant.Assert(runType == SpellerStatusTable.RunType.Clean || runType == SpellerStatusTable.RunType.Dirty);
			int num = this.FindIndex(start.CreateStaticPointer(), LogicalDirection.Forward);
			int num2 = this.FindIndex(end.CreateStaticPointer(), LogicalDirection.Backward);
			Invariant.Assert(num >= 0);
			Invariant.Assert(num2 >= 0);
			if (num + 1 < num2)
			{
				for (int i = num + 1; i < num2; i++)
				{
					this.NotifyHighlightLayerBeforeRunChange(i);
				}
				this._runList.RemoveRange(num + 1, num2 - num - 1);
				num2 = num + 1;
			}
			if (num == num2)
			{
				this.AddRun(num, start, end, runType);
				return;
			}
			Invariant.Assert(num == num2 - 1);
			this.AddRun(num, start, end, runType);
			num2 = this.FindIndex(end.CreateStaticPointer(), LogicalDirection.Backward);
			Invariant.Assert(num2 >= 0);
			this.AddRun(num2, start, end, runType);
		}

		// Token: 0x060035B1 RID: 13745 RVA: 0x000F3D88 File Offset: 0x000F1F88
		private void AddRun(int index, ITextPointer start, ITextPointer end, SpellerStatusTable.RunType runType)
		{
			Invariant.Assert(runType == SpellerStatusTable.RunType.Clean || runType == SpellerStatusTable.RunType.Dirty);
			Invariant.Assert(start.CompareTo(end) < 0);
			SpellerStatusTable.RunType runType2 = (runType == SpellerStatusTable.RunType.Clean) ? SpellerStatusTable.RunType.Dirty : SpellerStatusTable.RunType.Clean;
			SpellerStatusTable.Run run = this.GetRun(index);
			if (run.RunType == runType)
			{
				this.TryToMergeRunWithNeighbors(index);
				return;
			}
			if (run.RunType != runType2)
			{
				run.RunType = runType;
				ITextPointer position = run.Position;
				ITextPointer runEndPositionDynamic = this.GetRunEndPositionDynamic(index);
				this.TryToMergeRunWithNeighbors(index);
				this._highlightLayer.FireChangedEvent(position, runEndPositionDynamic);
				return;
			}
			if (run.Position.CompareTo(start) >= 0)
			{
				if (this.GetRunEndPosition(index).CompareTo(end) <= 0)
				{
					run.RunType = runType;
					this.TryToMergeRunWithNeighbors(index);
					return;
				}
				if (index > 0 && this.GetRun(index - 1).RunType == runType)
				{
					run.Position = end;
					return;
				}
				run.RunType = runType;
				SpellerStatusTable.Run value = new SpellerStatusTable.Run(end, runType2);
				this._runList.Insert(index + 1, value);
				return;
			}
			else
			{
				SpellerStatusTable.Run value;
				if (this.GetRunEndPosition(index).CompareTo(end) > 0)
				{
					value = new SpellerStatusTable.Run(start, runType);
					this._runList.Insert(index + 1, value);
					value = new SpellerStatusTable.Run(end, runType2);
					this._runList.Insert(index + 2, value);
					return;
				}
				if (index < this._runList.Count - 1 && this.GetRun(index + 1).RunType == runType)
				{
					this.GetRun(index + 1).Position = start;
					return;
				}
				value = new SpellerStatusTable.Run(start, runType);
				this._runList.Insert(index + 1, value);
				return;
			}
		}

		// Token: 0x060035B2 RID: 13746 RVA: 0x000F3F14 File Offset: 0x000F2114
		private void TryToMergeRunWithNeighbors(int index)
		{
			SpellerStatusTable.Run run = this.GetRun(index);
			if (index > 0 && this.GetRun(index - 1).RunType == run.RunType)
			{
				this._runList.RemoveAt(index);
				index--;
			}
			if (index < this._runList.Count - 1 && this.GetRun(index + 1).RunType == run.RunType)
			{
				this._runList.RemoveAt(index + 1);
			}
		}

		// Token: 0x060035B3 RID: 13747 RVA: 0x000F3F88 File Offset: 0x000F2188
		private void OnContentAdded(TextContainerChangeEventArgs e)
		{
			ITextPointer textPointer;
			if (e.ITextPosition.Offset > 0)
			{
				textPointer = e.ITextPosition.CreatePointer(-1);
			}
			else
			{
				textPointer = e.ITextPosition;
			}
			textPointer.Freeze();
			ITextPointer textPointer2;
			if (e.ITextPosition.Offset + e.Count < e.ITextPosition.TextContainer.SymbolCount - 1)
			{
				textPointer2 = e.ITextPosition.CreatePointer(e.Count + 1);
			}
			else
			{
				textPointer2 = e.ITextPosition.CreatePointer(e.Count);
			}
			textPointer2.Freeze();
			this.MarkRange(textPointer, textPointer2, SpellerStatusTable.RunType.Dirty);
		}

		// Token: 0x060035B4 RID: 13748 RVA: 0x000F401C File Offset: 0x000F221C
		private void OnContentRemoved(ITextPointer position)
		{
			int num = this.FindIndex(position.CreateStaticPointer(), LogicalDirection.Backward);
			if (num == -1)
			{
				num = 0;
			}
			SpellerStatusTable.Run run = this.GetRun(num);
			if (run.RunType != SpellerStatusTable.RunType.Dirty)
			{
				this.NotifyHighlightLayerBeforeRunChange(num);
				run.RunType = SpellerStatusTable.RunType.Dirty;
				if (num > 0 && this.GetRun(num - 1).RunType == SpellerStatusTable.RunType.Dirty)
				{
					this._runList.RemoveAt(num);
					num--;
				}
			}
			num++;
			int i;
			for (i = num; i < this._runList.Count; i++)
			{
				ITextPointer position2 = this.GetRun(i).Position;
				if (position2.CompareTo(position) > 0 && position2.CompareTo(this.GetRunEndPosition(i)) != 0)
				{
					break;
				}
			}
			this._runList.RemoveRange(num, i - num);
			if (num < this._runList.Count)
			{
				this.NotifyHighlightLayerBeforeRunChange(num);
				this._runList.RemoveAt(num);
				if (num < this._runList.Count && this.GetRun(num).RunType == SpellerStatusTable.RunType.Dirty)
				{
					this._runList.RemoveAt(num);
				}
			}
		}

		// Token: 0x060035B5 RID: 13749 RVA: 0x000F411C File Offset: 0x000F231C
		private void NotifyHighlightLayerBeforeRunChange(int index)
		{
			if (this.IsErrorRun(index))
			{
				ITextPointer position = this.GetRun(index).Position;
				ITextPointer runEndPositionDynamic = this.GetRunEndPositionDynamic(index);
				if (position.CompareTo(runEndPositionDynamic) != 0)
				{
					this._highlightLayer.FireChangedEvent(position, runEndPositionDynamic);
				}
			}
		}

		// Token: 0x060035B6 RID: 13750 RVA: 0x000F4160 File Offset: 0x000F2360
		private void DebugAssertRunList()
		{
			Invariant.Assert(this._runList.Count >= 1, "Run list should never be empty!");
			if (Invariant.Strict)
			{
				SpellerStatusTable.RunType runType = SpellerStatusTable.RunType.Clean;
				for (int i = 0; i < this._runList.Count; i++)
				{
					SpellerStatusTable.Run run = this.GetRun(i);
					if (this._runList.Count == 1)
					{
						Invariant.Assert(run.Position.CompareTo(run.Position.TextContainer.Start) == 0);
					}
					else
					{
						Invariant.Assert(run.Position.CompareTo(this.GetRunEndPosition(i)) <= 0, "Found negative width run!");
					}
					Invariant.Assert(i == 0 || this.GetRunEndPosition(i - 1).CompareTo(run.Position) <= 0, "Found overlapping runs!");
					if (!this.IsErrorRun(i))
					{
						Invariant.Assert(i == 0 || runType != run.RunType, "Found consecutive dirty/dirt or clean/clean runs!");
					}
					runType = run.RunType;
				}
			}
		}

		// Token: 0x060035B7 RID: 13751 RVA: 0x000F4266 File Offset: 0x000F2466
		private SpellerStatusTable.Run GetRun(int index)
		{
			return (SpellerStatusTable.Run)this._runList[index];
		}

		// Token: 0x060035B8 RID: 13752 RVA: 0x000F427C File Offset: 0x000F247C
		private ITextPointer GetRunEndPositionDynamic(int index)
		{
			return this.GetRunEndPosition(index).CreateDynamicTextPointer(LogicalDirection.Forward);
		}

		// Token: 0x060035B9 RID: 13753 RVA: 0x000F429C File Offset: 0x000F249C
		private StaticTextPointer GetRunEndPosition(int index)
		{
			StaticTextPointer result;
			if (index + 1 < this._runList.Count)
			{
				result = this.GetRun(index + 1).Position.CreateStaticPointer();
			}
			else
			{
				SpellerStatusTable.Run run = this.GetRun(index);
				ITextContainer textContainer = run.Position.TextContainer;
				result = textContainer.CreateStaticPointerAtOffset(textContainer.SymbolCount);
			}
			return result;
		}

		// Token: 0x060035BA RID: 13754 RVA: 0x000F42F4 File Offset: 0x000F24F4
		private bool IsErrorRun(int index)
		{
			SpellerStatusTable.Run run = this.GetRun(index);
			return run.RunType != SpellerStatusTable.RunType.Clean && run.RunType != SpellerStatusTable.RunType.Dirty;
		}

		// Token: 0x04002527 RID: 9511
		private readonly SpellerHighlightLayer _highlightLayer;

		// Token: 0x04002528 RID: 9512
		private readonly ArrayList _runList;

		// Token: 0x020008F5 RID: 2293
		internal enum RunType
		{
			// Token: 0x040042D9 RID: 17113
			Clean,
			// Token: 0x040042DA RID: 17114
			Dirty,
			// Token: 0x040042DB RID: 17115
			Error
		}

		// Token: 0x020008F6 RID: 2294
		private class Run
		{
			// Token: 0x06008598 RID: 34200 RVA: 0x00249FE0 File Offset: 0x002481E0
			internal Run(ITextPointer position, SpellerStatusTable.RunType runType)
			{
				this._position = position.GetFrozenPointer(LogicalDirection.Backward);
				this._runType = runType;
			}

			// Token: 0x17001E2C RID: 7724
			// (get) Token: 0x06008599 RID: 34201 RVA: 0x00249FFC File Offset: 0x002481FC
			// (set) Token: 0x0600859A RID: 34202 RVA: 0x0024A004 File Offset: 0x00248204
			internal ITextPointer Position
			{
				get
				{
					return this._position;
				}
				set
				{
					this._position = value;
				}
			}

			// Token: 0x17001E2D RID: 7725
			// (get) Token: 0x0600859B RID: 34203 RVA: 0x0024A00D File Offset: 0x0024820D
			// (set) Token: 0x0600859C RID: 34204 RVA: 0x0024A015 File Offset: 0x00248215
			internal SpellerStatusTable.RunType RunType
			{
				get
				{
					return this._runType;
				}
				set
				{
					this._runType = value;
				}
			}

			// Token: 0x040042DC RID: 17116
			private ITextPointer _position;

			// Token: 0x040042DD RID: 17117
			private SpellerStatusTable.RunType _runType;
		}
	}
}
