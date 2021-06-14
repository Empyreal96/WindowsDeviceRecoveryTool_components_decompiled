using System;

namespace Newtonsoft.Json.Utilities
{
	// Token: 0x020000F2 RID: 242
	internal class EnumValue<T> where T : struct
	{
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000B62 RID: 2914 RVA: 0x0002E2DF File Offset: 0x0002C4DF
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0002E2E7 File Offset: 0x0002C4E7
		public T Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x0002E2EF File Offset: 0x0002C4EF
		public EnumValue(string name, T value)
		{
			this._name = name;
			this._value = value;
		}

		// Token: 0x04000419 RID: 1049
		private readonly string _name;

		// Token: 0x0400041A RID: 1050
		private readonly T _value;
	}
}
