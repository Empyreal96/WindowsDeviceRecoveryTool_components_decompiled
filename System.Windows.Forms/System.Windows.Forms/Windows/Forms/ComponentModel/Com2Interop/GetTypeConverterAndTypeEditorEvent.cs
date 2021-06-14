using System;
using System.ComponentModel;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004BC RID: 1212
	internal class GetTypeConverterAndTypeEditorEvent : EventArgs
	{
		// Token: 0x0600512D RID: 20781 RVA: 0x001500E6 File Offset: 0x0014E2E6
		public GetTypeConverterAndTypeEditorEvent(TypeConverter typeConverter, object typeEditor)
		{
			this.typeEditor = typeEditor;
			this.typeConverter = typeConverter;
		}

		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x0600512E RID: 20782 RVA: 0x001500FC File Offset: 0x0014E2FC
		// (set) Token: 0x0600512F RID: 20783 RVA: 0x00150104 File Offset: 0x0014E304
		public TypeConverter TypeConverter
		{
			get
			{
				return this.typeConverter;
			}
			set
			{
				this.typeConverter = value;
			}
		}

		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x06005130 RID: 20784 RVA: 0x0015010D File Offset: 0x0014E30D
		// (set) Token: 0x06005131 RID: 20785 RVA: 0x00150115 File Offset: 0x0014E315
		public object TypeEditor
		{
			get
			{
				return this.typeEditor;
			}
			set
			{
				this.typeEditor = value;
			}
		}

		// Token: 0x04003450 RID: 13392
		private TypeConverter typeConverter;

		// Token: 0x04003451 RID: 13393
		private object typeEditor;
	}
}
