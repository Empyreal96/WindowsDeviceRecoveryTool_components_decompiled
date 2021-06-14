using System;
using Microsoft.Data.Edm.Expressions;
using Microsoft.Data.Edm.Values;

namespace Microsoft.Data.Edm.Library.Values
{
	// Token: 0x02000190 RID: 400
	public class EdmBinaryConstant : EdmValue, IEdmBinaryConstantExpression, IEdmExpression, IEdmBinaryValue, IEdmPrimitiveValue, IEdmValue, IEdmElement
	{
		// Token: 0x060008C7 RID: 2247 RVA: 0x00018324 File Offset: 0x00016524
		public EdmBinaryConstant(byte[] value) : this(null, value)
		{
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0001832E File Offset: 0x0001652E
		public EdmBinaryConstant(IEdmBinaryTypeReference type, byte[] value) : base(type)
		{
			EdmUtil.CheckArgumentNull<byte[]>(value, "value");
			this.value = value;
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0001834A File Offset: 0x0001654A
		public byte[] Value
		{
			get
			{
				return this.value;
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x00018352 File Offset: 0x00016552
		public EdmExpressionKind ExpressionKind
		{
			get
			{
				return EdmExpressionKind.BinaryConstant;
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00018355 File Offset: 0x00016555
		public override EdmValueKind ValueKind
		{
			get
			{
				return EdmValueKind.Binary;
			}
		}

		// Token: 0x04000456 RID: 1110
		private readonly byte[] value;
	}
}
