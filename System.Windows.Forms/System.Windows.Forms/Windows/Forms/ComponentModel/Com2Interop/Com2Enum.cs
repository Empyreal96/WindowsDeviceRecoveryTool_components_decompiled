using System;
using System.Globalization;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004A4 RID: 1188
	internal class Com2Enum
	{
		// Token: 0x06005052 RID: 20562 RVA: 0x0014CCCC File Offset: 0x0014AECC
		public Com2Enum(string[] names, object[] values, bool allowUnknownValues)
		{
			this.allowUnknownValues = allowUnknownValues;
			if (names == null || values == null || names.Length != values.Length)
			{
				throw new ArgumentException(SR.GetString("COM2NamesAndValuesNotEqual"));
			}
			this.PopulateArrays(names, values);
		}

		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x06005053 RID: 20563 RVA: 0x0014CD01 File Offset: 0x0014AF01
		public bool IsStrictEnum
		{
			get
			{
				return !this.allowUnknownValues;
			}
		}

		// Token: 0x170013D7 RID: 5079
		// (get) Token: 0x06005054 RID: 20564 RVA: 0x0014CD0C File Offset: 0x0014AF0C
		public virtual object[] Values
		{
			get
			{
				return (object[])this.values.Clone();
			}
		}

		// Token: 0x170013D8 RID: 5080
		// (get) Token: 0x06005055 RID: 20565 RVA: 0x0014CD1E File Offset: 0x0014AF1E
		public virtual string[] Names
		{
			get
			{
				return (string[])this.names.Clone();
			}
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x0014CD30 File Offset: 0x0014AF30
		public virtual object FromString(string s)
		{
			int num = -1;
			for (int i = 0; i < this.stringValues.Length; i++)
			{
				if (string.Compare(this.names[i], s, true, CultureInfo.InvariantCulture) == 0 || string.Compare(this.stringValues[i], s, true, CultureInfo.InvariantCulture) == 0)
				{
					return this.values[i];
				}
				if (num == -1 && string.Compare(this.names[i], s, true, CultureInfo.InvariantCulture) == 0)
				{
					num = i;
				}
			}
			if (num != -1)
			{
				return this.values[num];
			}
			if (!this.allowUnknownValues)
			{
				return null;
			}
			return s;
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x0014CDBC File Offset: 0x0014AFBC
		protected virtual void PopulateArrays(string[] names, object[] values)
		{
			this.names = new string[names.Length];
			this.stringValues = new string[names.Length];
			this.values = new object[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				this.names[i] = names[i];
				this.values[i] = values[i];
				if (values[i] != null)
				{
					this.stringValues[i] = values[i].ToString();
				}
			}
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x0014CE2C File Offset: 0x0014B02C
		public virtual string ToString(object v)
		{
			if (v != null)
			{
				if (this.values.Length != 0 && v.GetType() != this.values[0].GetType())
				{
					try
					{
						v = Convert.ChangeType(v, this.values[0].GetType(), CultureInfo.InvariantCulture);
					}
					catch
					{
					}
				}
				string text = v.ToString();
				for (int i = 0; i < this.values.Length; i++)
				{
					if (string.Compare(this.stringValues[i], text, true, CultureInfo.InvariantCulture) == 0)
					{
						return this.names[i];
					}
				}
				if (this.allowUnknownValues)
				{
					return text;
				}
			}
			return "";
		}

		// Token: 0x0400340C RID: 13324
		private string[] names;

		// Token: 0x0400340D RID: 13325
		private object[] values;

		// Token: 0x0400340E RID: 13326
		private string[] stringValues;

		// Token: 0x0400340F RID: 13327
		private bool allowUnknownValues;
	}
}
