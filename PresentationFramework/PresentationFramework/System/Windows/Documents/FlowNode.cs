using System;

namespace System.Windows.Documents
{
	// Token: 0x02000370 RID: 880
	internal sealed class FlowNode : IComparable
	{
		// Token: 0x06002F7F RID: 12159 RVA: 0x000D62D7 File Offset: 0x000D44D7
		internal FlowNode(int scopeId, FlowNodeType type, object cookie)
		{
			this._scopeId = scopeId;
			this._type = type;
			this._cookie = cookie;
		}

		// Token: 0x06002F80 RID: 12160 RVA: 0x000D62F4 File Offset: 0x000D44F4
		public static bool IsNull(FlowNode flow)
		{
			return flow == null;
		}

		// Token: 0x06002F81 RID: 12161 RVA: 0x000D62FC File Offset: 0x000D44FC
		public override int GetHashCode()
		{
			return this._scopeId.GetHashCode() ^ this._fp.GetHashCode();
		}

		// Token: 0x06002F82 RID: 12162 RVA: 0x000D6324 File Offset: 0x000D4524
		public override bool Equals(object o)
		{
			if (o == null || base.GetType() != o.GetType())
			{
				return false;
			}
			FlowNode flowNode = (FlowNode)o;
			return this._fp == flowNode._fp;
		}

		// Token: 0x06002F83 RID: 12163 RVA: 0x000D6360 File Offset: 0x000D4560
		public int CompareTo(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o");
			}
			FlowNode flowNode = o as FlowNode;
			if (flowNode == null)
			{
				throw new ArgumentException(SR.Get("UnexpectedParameterType", new object[]
				{
					o.GetType(),
					typeof(FlowNode)
				}), "o");
			}
			if (this == flowNode)
			{
				return 0;
			}
			int num = this._fp - flowNode._fp;
			if (num == 0)
			{
				return 0;
			}
			if (num < 0)
			{
				return -1;
			}
			return 1;
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x000D63D6 File Offset: 0x000D45D6
		internal void SetFp(int fp)
		{
			this._fp = fp;
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x000D63DF File Offset: 0x000D45DF
		internal void IncreaseFp()
		{
			this._fp++;
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x000D63EF File Offset: 0x000D45EF
		internal void DecreaseFp()
		{
			this._fp--;
		}

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06002F87 RID: 12167 RVA: 0x000D63FF File Offset: 0x000D45FF
		internal int Fp
		{
			get
			{
				return this._fp;
			}
		}

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06002F88 RID: 12168 RVA: 0x000D6407 File Offset: 0x000D4607
		internal int ScopeId
		{
			get
			{
				return this._scopeId;
			}
		}

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06002F89 RID: 12169 RVA: 0x000D640F File Offset: 0x000D460F
		internal FlowNodeType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06002F8A RID: 12170 RVA: 0x000D6417 File Offset: 0x000D4617
		internal object Cookie
		{
			get
			{
				return this._cookie;
			}
		}

		// Token: 0x17000BF8 RID: 3064
		// (get) Token: 0x06002F8B RID: 12171 RVA: 0x000D641F File Offset: 0x000D461F
		// (set) Token: 0x06002F8C RID: 12172 RVA: 0x000D6427 File Offset: 0x000D4627
		internal FixedSOMElement[] FixedSOMElements
		{
			get
			{
				return this._elements;
			}
			set
			{
				this._elements = value;
			}
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x000D6430 File Offset: 0x000D4630
		internal void AttachElement(FixedElement fixedElement)
		{
			this._cookie = fixedElement;
		}

		// Token: 0x04001E42 RID: 7746
		private readonly int _scopeId;

		// Token: 0x04001E43 RID: 7747
		private readonly FlowNodeType _type;

		// Token: 0x04001E44 RID: 7748
		private int _fp;

		// Token: 0x04001E45 RID: 7749
		private object _cookie;

		// Token: 0x04001E46 RID: 7750
		private FixedSOMElement[] _elements;
	}
}
