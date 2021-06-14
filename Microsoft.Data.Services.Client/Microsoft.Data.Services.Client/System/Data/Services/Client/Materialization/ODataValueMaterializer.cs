using System;
using Microsoft.Data.Edm;
using Microsoft.Data.OData;

namespace System.Data.Services.Client.Materialization
{
	// Token: 0x02000073 RID: 115
	internal sealed class ODataValueMaterializer : ODataMessageReaderMaterializer
	{
		// Token: 0x060003C9 RID: 969 RVA: 0x00010363 File Offset: 0x0000E563
		public ODataValueMaterializer(ODataMessageReader reader, IODataMaterializerContext materializerContext, Type expectedType, bool? singleResult) : base(reader, materializerContext, expectedType, singleResult)
		{
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060003CA RID: 970 RVA: 0x00010370 File Offset: 0x0000E570
		internal override object CurrentValue
		{
			get
			{
				return this.currentValue;
			}
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00010378 File Offset: 0x0000E578
		protected override void ReadWithExpectedType(IEdmTypeReference expectedClientType, IEdmTypeReference expectedReaderType)
		{
			object item = this.messageReader.ReadValue(expectedReaderType);
			this.currentValue = base.PrimitiveValueMaterializier.MaterializePrimitiveDataValue(base.ExpectedType, null, item);
		}

		// Token: 0x040002B6 RID: 694
		private object currentValue;
	}
}
