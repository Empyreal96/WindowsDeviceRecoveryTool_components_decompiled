using System;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x0200035C RID: 860
	internal sealed class FixedSOMLineCollection
	{
		// Token: 0x06002DCC RID: 11724 RVA: 0x000CE15E File Offset: 0x000CC35E
		public FixedSOMLineCollection()
		{
			this._verticals = new List<FixedSOMLineRanges>();
			this._horizontals = new List<FixedSOMLineRanges>();
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000CE17C File Offset: 0x000CC37C
		public bool IsVerticallySeparated(double left, double top, double right, double bottom)
		{
			return this._IsSeparated(this._verticals, left, top, right, bottom);
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x000CE18F File Offset: 0x000CC38F
		public bool IsHorizontallySeparated(double left, double top, double right, double bottom)
		{
			return this._IsSeparated(this._horizontals, top, left, bottom, right);
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000CE1A2 File Offset: 0x000CC3A2
		public void AddVertical(Point point1, Point point2)
		{
			this._AddLineToRanges(this._verticals, point1.X, point1.Y, point2.Y);
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000CE1C5 File Offset: 0x000CC3C5
		public void AddHorizontal(Point point1, Point point2)
		{
			this._AddLineToRanges(this._horizontals, point1.Y, point1.X, point2.X);
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000CE1E8 File Offset: 0x000CC3E8
		private void _AddLineToRanges(List<FixedSOMLineRanges> ranges, double line, double start, double end)
		{
			if (start > end)
			{
				double num = start;
				start = end;
				end = num;
			}
			double num2 = 0.5 * FixedSOMLineRanges.MinLineSeparation;
			FixedSOMLineRanges fixedSOMLineRanges;
			for (int i = 0; i < ranges.Count; i++)
			{
				if (line < ranges[i].Line - num2)
				{
					fixedSOMLineRanges = new FixedSOMLineRanges();
					fixedSOMLineRanges.Line = line;
					fixedSOMLineRanges.AddRange(start, end);
					ranges.Insert(i, fixedSOMLineRanges);
					return;
				}
				if (line < ranges[i].Line + num2)
				{
					ranges[i].AddRange(start, end);
					return;
				}
			}
			fixedSOMLineRanges = new FixedSOMLineRanges();
			fixedSOMLineRanges.Line = line;
			fixedSOMLineRanges.AddRange(start, end);
			ranges.Add(fixedSOMLineRanges);
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000CE294 File Offset: 0x000CC494
		private bool _IsSeparated(List<FixedSOMLineRanges> lines, double parallelLowEnd, double perpLowEnd, double parallelHighEnd, double perpHighEnd)
		{
			int num = 0;
			int i = lines.Count;
			if (i == 0)
			{
				return false;
			}
			int num2 = 0;
			while (i > num)
			{
				num2 = num + i >> 1;
				if (lines[num2].Line < parallelLowEnd)
				{
					num = num2 + 1;
				}
				else
				{
					if (lines[num2].Line <= parallelHighEnd)
					{
						break;
					}
					i = num2;
				}
			}
			if (lines[num2].Line >= parallelLowEnd && lines[num2].Line <= parallelHighEnd)
			{
				do
				{
					num2--;
				}
				while (num2 >= 0 && lines[num2].Line >= parallelLowEnd);
				num2++;
				while (num2 < lines.Count && lines[num2].Line <= parallelHighEnd)
				{
					double num3 = (perpHighEnd - perpLowEnd) * 0.1;
					int lineAt = lines[num2].GetLineAt(perpLowEnd + num3);
					if (lineAt >= 0)
					{
						double num4 = lines[num2].End[lineAt];
						if (num4 >= perpHighEnd - num3)
						{
							return true;
						}
					}
					num2++;
				}
			}
			return false;
		}

		// Token: 0x17000B74 RID: 2932
		// (get) Token: 0x06002DD3 RID: 11731 RVA: 0x000CE385 File Offset: 0x000CC585
		public List<FixedSOMLineRanges> HorizontalLines
		{
			get
			{
				return this._horizontals;
			}
		}

		// Token: 0x17000B75 RID: 2933
		// (get) Token: 0x06002DD4 RID: 11732 RVA: 0x000CE38D File Offset: 0x000CC58D
		public List<FixedSOMLineRanges> VerticalLines
		{
			get
			{
				return this._verticals;
			}
		}

		// Token: 0x04001DCA RID: 7626
		private List<FixedSOMLineRanges> _horizontals;

		// Token: 0x04001DCB RID: 7627
		private List<FixedSOMLineRanges> _verticals;

		// Token: 0x04001DCC RID: 7628
		private const double _fudgeFactor = 0.1;
	}
}
