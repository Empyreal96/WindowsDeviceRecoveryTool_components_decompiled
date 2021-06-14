using System;

namespace System.Drawing.Printing
{
	// Token: 0x02000071 RID: 113
	[Serializable]
	internal struct TriState
	{
		// Token: 0x0600081D RID: 2077 RVA: 0x00020BD3 File Offset: 0x0001EDD3
		private TriState(byte value)
		{
			this.value = value;
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x00020BDC File Offset: 0x0001EDDC
		public bool IsDefault
		{
			get
			{
				return this == TriState.Default;
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x00020BEE File Offset: 0x0001EDEE
		public bool IsFalse
		{
			get
			{
				return this == TriState.False;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000820 RID: 2080 RVA: 0x00020C00 File Offset: 0x0001EE00
		public bool IsNotDefault
		{
			get
			{
				return this != TriState.Default;
			}
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00020C12 File Offset: 0x0001EE12
		public bool IsTrue
		{
			get
			{
				return this == TriState.True;
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x00020C24 File Offset: 0x0001EE24
		public static bool operator ==(TriState left, TriState right)
		{
			return left.value == right.value;
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00020C34 File Offset: 0x0001EE34
		public static bool operator !=(TriState left, TriState right)
		{
			return !(left == right);
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x00020C40 File Offset: 0x0001EE40
		public override bool Equals(object o)
		{
			TriState triState = (TriState)o;
			return this.value == triState.value;
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00020C62 File Offset: 0x0001EE62
		public override int GetHashCode()
		{
			return (int)this.value;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00020C6A File Offset: 0x0001EE6A
		public static implicit operator TriState(bool value)
		{
			if (!value)
			{
				return TriState.False;
			}
			return TriState.True;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00020C7A File Offset: 0x0001EE7A
		public static explicit operator bool(TriState value)
		{
			if (value.IsDefault)
			{
				throw new InvalidCastException(SR.GetString("TriStateCompareError"));
			}
			return value == TriState.True;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00020CA0 File Offset: 0x0001EEA0
		public override string ToString()
		{
			if (this == TriState.Default)
			{
				return "Default";
			}
			if (this == TriState.False)
			{
				return "False";
			}
			return "True";
		}

		// Token: 0x040006FF RID: 1791
		private byte value;

		// Token: 0x04000700 RID: 1792
		public static readonly TriState Default = new TriState(0);

		// Token: 0x04000701 RID: 1793
		public static readonly TriState False = new TriState(1);

		// Token: 0x04000702 RID: 1794
		public static readonly TriState True = new TriState(2);
	}
}
