using System;
using System.Xml.Linq;

namespace Newtonsoft.Json.Converters
{
	// Token: 0x02000038 RID: 56
	internal class XProcessingInstructionWrapper : XObjectWrapper
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00008745 File Offset: 0x00006945
		private XProcessingInstruction ProcessingInstruction
		{
			get
			{
				return (XProcessingInstruction)base.WrappedNode;
			}
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00008752 File Offset: 0x00006952
		public XProcessingInstructionWrapper(XProcessingInstruction processingInstruction) : base(processingInstruction)
		{
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000875B File Offset: 0x0000695B
		public override string LocalName
		{
			get
			{
				return this.ProcessingInstruction.Target;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000222 RID: 546 RVA: 0x00008768 File Offset: 0x00006968
		// (set) Token: 0x06000223 RID: 547 RVA: 0x00008775 File Offset: 0x00006975
		public override string Value
		{
			get
			{
				return this.ProcessingInstruction.Data;
			}
			set
			{
				this.ProcessingInstruction.Data = value;
			}
		}
	}
}
