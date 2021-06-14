using System;

namespace System.Data.Services.Client
{
	// Token: 0x020000B0 RID: 176
	internal class InstancePrimitiveParserToken<T> : PrimitiveParserToken
	{
		// Token: 0x060005A6 RID: 1446 RVA: 0x00015968 File Offset: 0x00013B68
		internal InstancePrimitiveParserToken(T instance)
		{
			this.Instance = instance;
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00015977 File Offset: 0x00013B77
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x0001597F File Offset: 0x00013B7F
		internal T Instance { get; private set; }

		// Token: 0x060005A9 RID: 1449 RVA: 0x00015988 File Offset: 0x00013B88
		internal override object Materialize(Type clrType)
		{
			return this.Instance;
		}
	}
}
