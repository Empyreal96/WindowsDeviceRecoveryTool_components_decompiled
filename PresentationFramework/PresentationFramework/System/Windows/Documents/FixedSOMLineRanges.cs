using System;
using System.Collections.Generic;

namespace System.Windows.Documents
{
	// Token: 0x0200035D RID: 861
	internal class FixedSOMLineRanges
	{
		// Token: 0x06002DD5 RID: 11733 RVA: 0x000CE398 File Offset: 0x000CC598
		public void AddRange(double start, double end)
		{
			int i = 0;
			while (i < this.Start.Count)
			{
				if (start > this.End[i] + 3.0)
				{
					i++;
				}
				else
				{
					if (end + 3.0 < this.Start[i])
					{
						this.Start.Insert(i, start);
						this.End.Insert(i, end);
						return;
					}
					if (this.Start[i] < start)
					{
						start = this.Start[i];
					}
					if (this.End[i] > end)
					{
						end = this.End[i];
					}
					this.Start.RemoveAt(i);
					this.End.RemoveAt(i);
				}
			}
			this.Start.Add(start);
			this.End.Add(end);
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x000CE480 File Offset: 0x000CC680
		public int GetLineAt(double line)
		{
			int num = 0;
			int i = this.Start.Count - 1;
			while (i > num)
			{
				int num2 = num + i >> 1;
				if (line > this.End[num2])
				{
					num = num2 + 1;
				}
				else
				{
					i = num2;
				}
			}
			if (num == i && line <= this.End[num] && line >= this.Start[num])
			{
				return num;
			}
			return -1;
		}

		// Token: 0x17000B76 RID: 2934
		// (get) Token: 0x06002DD8 RID: 11736 RVA: 0x000CE4EE File Offset: 0x000CC6EE
		// (set) Token: 0x06002DD7 RID: 11735 RVA: 0x000CE4E5 File Offset: 0x000CC6E5
		public double Line
		{
			get
			{
				return this._line;
			}
			set
			{
				this._line = value;
			}
		}

		// Token: 0x17000B77 RID: 2935
		// (get) Token: 0x06002DD9 RID: 11737 RVA: 0x000CE4F6 File Offset: 0x000CC6F6
		public List<double> Start
		{
			get
			{
				if (this._start == null)
				{
					this._start = new List<double>();
				}
				return this._start;
			}
		}

		// Token: 0x17000B78 RID: 2936
		// (get) Token: 0x06002DDA RID: 11738 RVA: 0x000CE511 File Offset: 0x000CC711
		public List<double> End
		{
			get
			{
				if (this._end == null)
				{
					this._end = new List<double>();
				}
				return this._end;
			}
		}

		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x06002DDB RID: 11739 RVA: 0x000CE52C File Offset: 0x000CC72C
		public int Count
		{
			get
			{
				return this.Start.Count;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x06002DDC RID: 11740 RVA: 0x000CE539 File Offset: 0x000CC739
		public static double MinLineSeparation
		{
			get
			{
				return 3.0;
			}
		}

		// Token: 0x04001DCD RID: 7629
		private double _line;

		// Token: 0x04001DCE RID: 7630
		private List<double> _start;

		// Token: 0x04001DCF RID: 7631
		private List<double> _end;

		// Token: 0x04001DD0 RID: 7632
		private const double _minLineSeparation = 3.0;
	}
}
