using System;

namespace System.Windows
{
	// Token: 0x020000FF RID: 255
	internal struct ChildValueLookup
	{
		// Token: 0x06000959 RID: 2393 RVA: 0x00020C8C File Offset: 0x0001EE8C
		public override bool Equals(object value)
		{
			if (value is ChildValueLookup)
			{
				ChildValueLookup childValueLookup = (ChildValueLookup)value;
				if (this.LookupType == childValueLookup.LookupType && this.Property == childValueLookup.Property && this.Value == childValueLookup.Value)
				{
					if (this.Conditions == null && childValueLookup.Conditions == null)
					{
						return true;
					}
					if (this.Conditions == null || childValueLookup.Conditions == null)
					{
						return false;
					}
					if (this.Conditions.Length == childValueLookup.Conditions.Length)
					{
						for (int i = 0; i < this.Conditions.Length; i++)
						{
							if (!this.Conditions[i].TypeSpecificEquals(childValueLookup.Conditions[i]))
							{
								return false;
							}
						}
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600095A RID: 2394 RVA: 0x00020D45 File Offset: 0x0001EF45
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x0600095B RID: 2395 RVA: 0x00020D57 File Offset: 0x0001EF57
		public static bool operator ==(ChildValueLookup value1, ChildValueLookup value2)
		{
			return value1.Equals(value2);
		}

		// Token: 0x0600095C RID: 2396 RVA: 0x00020D6C File Offset: 0x0001EF6C
		public static bool operator !=(ChildValueLookup value1, ChildValueLookup value2)
		{
			return !value1.Equals(value2);
		}

		// Token: 0x040007FB RID: 2043
		internal ValueLookupType LookupType;

		// Token: 0x040007FC RID: 2044
		internal TriggerCondition[] Conditions;

		// Token: 0x040007FD RID: 2045
		internal DependencyProperty Property;

		// Token: 0x040007FE RID: 2046
		internal object Value;
	}
}
