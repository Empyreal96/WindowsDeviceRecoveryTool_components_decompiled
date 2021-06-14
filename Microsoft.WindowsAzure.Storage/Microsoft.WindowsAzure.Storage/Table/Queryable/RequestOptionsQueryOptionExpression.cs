using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Microsoft.WindowsAzure.Storage.Table.Queryable
{
	// Token: 0x02000128 RID: 296
	[DebuggerDisplay("RequestOptionsQueryOptionExpression {requestOptions}")]
	internal class RequestOptionsQueryOptionExpression : QueryOptionExpression
	{
		// Token: 0x060013BD RID: 5053 RVA: 0x0004B012 File Offset: 0x00049212
		internal RequestOptionsQueryOptionExpression(Type type, ConstantExpression requestOptions) : base((ExpressionType)10009, type)
		{
			this.requestOptions = requestOptions;
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x0004B027 File Offset: 0x00049227
		internal ConstantExpression RequestOptions
		{
			get
			{
				return this.requestOptions;
			}
		}

		// Token: 0x0400067C RID: 1660
		private ConstantExpression requestOptions;
	}
}
