using System;

namespace System.Windows.Baml2006
{
	// Token: 0x0200015D RID: 349
	internal class OptimizedStaticResource
	{
		// Token: 0x06000F9C RID: 3996 RVA: 0x0003C9F5 File Offset: 0x0003ABF5
		public OptimizedStaticResource(byte flags, short keyId)
		{
			this._isType = ((flags & OptimizedStaticResource.TypeExtensionValueMask) > 0);
			this._isStatic = ((flags & OptimizedStaticResource.StaticExtensionValueMask) > 0);
			this.KeyId = keyId;
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06000F9D RID: 3997 RVA: 0x0003CA24 File Offset: 0x0003AC24
		// (set) Token: 0x06000F9E RID: 3998 RVA: 0x0003CA2C File Offset: 0x0003AC2C
		public short KeyId { get; set; }

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06000F9F RID: 3999 RVA: 0x0003CA35 File Offset: 0x0003AC35
		// (set) Token: 0x06000FA0 RID: 4000 RVA: 0x0003CA3D File Offset: 0x0003AC3D
		public object KeyValue { get; set; }

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06000FA1 RID: 4001 RVA: 0x0003CA46 File Offset: 0x0003AC46
		public bool IsKeyStaticExtension
		{
			get
			{
				return this._isStatic;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06000FA2 RID: 4002 RVA: 0x0003CA4E File Offset: 0x0003AC4E
		public bool IsKeyTypeExtension
		{
			get
			{
				return this._isType;
			}
		}

		// Token: 0x040011A6 RID: 4518
		private bool _isStatic;

		// Token: 0x040011A7 RID: 4519
		private bool _isType;

		// Token: 0x040011A8 RID: 4520
		private static readonly byte TypeExtensionValueMask = 1;

		// Token: 0x040011A9 RID: 4521
		private static readonly byte StaticExtensionValueMask = 2;
	}
}
