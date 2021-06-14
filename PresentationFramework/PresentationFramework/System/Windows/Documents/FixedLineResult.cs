using System;

namespace System.Windows.Documents
{
	// Token: 0x02000353 RID: 851
	internal sealed class FixedLineResult : IComparable
	{
		// Token: 0x06002D47 RID: 11591 RVA: 0x000CC6F7 File Offset: 0x000CA8F7
		internal FixedLineResult(FixedNode[] nodes, Rect layoutBox)
		{
			this._nodes = nodes;
			this._layoutBox = layoutBox;
		}

		// Token: 0x06002D48 RID: 11592 RVA: 0x000CC710 File Offset: 0x000CA910
		public int CompareTo(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (o.GetType() != typeof(FixedLineResult))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FixedLineResult)
				}), "o");
			}
			FixedLineResult fixedLineResult = (FixedLineResult)o;
			return this.BaseLine.CompareTo(fixedLineResult.BaseLine);
		}

		// Token: 0x17000B46 RID: 2886
		// (get) Token: 0x06002D49 RID: 11593 RVA: 0x000CC78E File Offset: 0x000CA98E
		internal FixedNode Start
		{
			get
			{
				return this._nodes[0];
			}
		}

		// Token: 0x17000B47 RID: 2887
		// (get) Token: 0x06002D4A RID: 11594 RVA: 0x000CC79C File Offset: 0x000CA99C
		internal FixedNode End
		{
			get
			{
				return this._nodes[this._nodes.Length - 1];
			}
		}

		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x06002D4B RID: 11595 RVA: 0x000CC7B3 File Offset: 0x000CA9B3
		internal FixedNode[] Nodes
		{
			get
			{
				return this._nodes;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x06002D4C RID: 11596 RVA: 0x000CC7BC File Offset: 0x000CA9BC
		internal double BaseLine
		{
			get
			{
				return this._layoutBox.Bottom;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06002D4D RID: 11597 RVA: 0x000CC7D7 File Offset: 0x000CA9D7
		internal Rect LayoutBox
		{
			get
			{
				return this._layoutBox;
			}
		}

		// Token: 0x04001DA3 RID: 7587
		private readonly FixedNode[] _nodes;

		// Token: 0x04001DA4 RID: 7588
		private readonly Rect _layoutBox;
	}
}
