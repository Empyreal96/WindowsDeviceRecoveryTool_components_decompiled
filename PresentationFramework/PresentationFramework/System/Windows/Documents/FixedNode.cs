using System;

namespace System.Windows.Documents
{
	// Token: 0x02000354 RID: 852
	internal struct FixedNode : IComparable
	{
		// Token: 0x06002D4E RID: 11598 RVA: 0x000CC7DF File Offset: 0x000CA9DF
		internal static FixedNode Create(int pageIndex, int childLevels, int level1Index, int level2Index, int[] childPath)
		{
			if (childLevels == 1)
			{
				return new FixedNode(pageIndex, level1Index);
			}
			if (childLevels != 2)
			{
				return FixedNode.Create(pageIndex, childPath);
			}
			return new FixedNode(pageIndex, level1Index, level2Index);
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000CC804 File Offset: 0x000CAA04
		internal static FixedNode Create(int pageIndex, int[] childPath)
		{
			int[] array = new int[childPath.Length + 1];
			array[0] = pageIndex;
			childPath.CopyTo(array, 1);
			return new FixedNode(array);
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000CC82E File Offset: 0x000CAA2E
		private FixedNode(int page, int level1Index)
		{
			this._path = new int[2];
			this._path[0] = page;
			this._path[1] = level1Index;
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000CC84E File Offset: 0x000CAA4E
		private FixedNode(int page, int level1Index, int level2Index)
		{
			this._path = new int[3];
			this._path[0] = page;
			this._path[1] = level1Index;
			this._path[2] = level2Index;
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000CC877 File Offset: 0x000CAA77
		private FixedNode(int[] path)
		{
			this._path = path;
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000CC880 File Offset: 0x000CAA80
		public int CompareTo(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			if (o.GetType() != typeof(FixedNode))
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FixedNode)
				}), "o");
			}
			FixedNode fixedNode = (FixedNode)o;
			return this.CompareTo(fixedNode);
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000CC8F4 File Offset: 0x000CAAF4
		public int CompareTo(FixedNode fixedNode)
		{
			int num = this.Page.CompareTo(fixedNode.Page);
			if (num == 0)
			{
				int num2 = 1;
				while (num2 <= this.ChildLevels && num2 <= fixedNode.ChildLevels)
				{
					int num3 = this[num2];
					int num4 = fixedNode[num2];
					if (num3 != num4)
					{
						return num3.CompareTo(num4);
					}
					num2++;
				}
			}
			return num;
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000CC95C File Offset: 0x000CAB5C
		internal int ComparetoIndex(int[] childPath)
		{
			int num = 0;
			while (num < childPath.Length && num < this._path.Length - 1)
			{
				if (childPath[num] != this._path[num + 1])
				{
					return childPath[num].CompareTo(this._path[num + 1]);
				}
				num++;
			}
			return 0;
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000CC9AB File Offset: 0x000CABAB
		public static bool operator <(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) < 0;
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000CC9B8 File Offset: 0x000CABB8
		public static bool operator <=(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) <= 0;
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000CC9C8 File Offset: 0x000CABC8
		public static bool operator >(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) > 0;
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x000CC9D5 File Offset: 0x000CABD5
		public static bool operator >=(FixedNode fp1, FixedNode fp2)
		{
			return fp1.CompareTo(fp2) >= 0;
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000CC9E5 File Offset: 0x000CABE5
		public override bool Equals(object o)
		{
			return o is FixedNode && this.Equals((FixedNode)o);
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000CC9FD File Offset: 0x000CABFD
		public bool Equals(FixedNode fixedp)
		{
			return this.CompareTo(fixedp) == 0;
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000CCA09 File Offset: 0x000CAC09
		public static bool operator ==(FixedNode fp1, FixedNode fp2)
		{
			return fp1.Equals(fp2);
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x000CCA13 File Offset: 0x000CAC13
		public static bool operator !=(FixedNode fp1, FixedNode fp2)
		{
			return !fp1.Equals(fp2);
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000CCA20 File Offset: 0x000CAC20
		public override int GetHashCode()
		{
			int num = 0;
			foreach (int num2 in this._path)
			{
				num = 43 * num + num2;
			}
			return num;
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06002D5F RID: 11615 RVA: 0x000CCA50 File Offset: 0x000CAC50
		internal int Page
		{
			get
			{
				return this._path[0];
			}
		}

		// Token: 0x17000B4C RID: 2892
		internal int this[int level]
		{
			get
			{
				return this._path[level];
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06002D61 RID: 11617 RVA: 0x000CCA64 File Offset: 0x000CAC64
		internal int ChildLevels
		{
			get
			{
				return this._path.Length - 1;
			}
		}

		// Token: 0x04001DA5 RID: 7589
		private readonly int[] _path;
	}
}
