using System;

namespace Newtonsoft.Json.Bson
{
	// Token: 0x0200000D RID: 13
	internal class BsonValue : BsonToken
	{
		// Token: 0x06000084 RID: 132 RVA: 0x0000446F File Offset: 0x0000266F
		public BsonValue(object value, BsonType type)
		{
			this._value = value;
			this._type = type;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000085 RID: 133 RVA: 0x00004485 File Offset: 0x00002685
		public object Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000086 RID: 134 RVA: 0x0000448D File Offset: 0x0000268D
		public override BsonType Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x0400004C RID: 76
		private readonly object _value;

		// Token: 0x0400004D RID: 77
		private readonly BsonType _type;
	}
}
