using System;

namespace System.Windows.Documents
{
	// Token: 0x02000434 RID: 1076
	internal class XamlRtfConverter
	{
		// Token: 0x06003F36 RID: 16182 RVA: 0x0000326D File Offset: 0x0000146D
		internal XamlRtfConverter()
		{
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x001210EC File Offset: 0x0011F2EC
		internal string ConvertXamlToRtf(string xamlContent)
		{
			if (xamlContent == null)
			{
				throw new ArgumentNullException("xamlContent");
			}
			string result = string.Empty;
			if (xamlContent != string.Empty)
			{
				XamlToRtfWriter xamlToRtfWriter = new XamlToRtfWriter(xamlContent);
				if (this.WpfPayload != null)
				{
					xamlToRtfWriter.WpfPayload = this.WpfPayload;
				}
				xamlToRtfWriter.Process();
				result = xamlToRtfWriter.Output;
			}
			return result;
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x00121144 File Offset: 0x0011F344
		internal string ConvertRtfToXaml(string rtfContent)
		{
			if (rtfContent == null)
			{
				throw new ArgumentNullException("rtfContent");
			}
			string result = string.Empty;
			if (rtfContent != string.Empty)
			{
				RtfToXamlReader rtfToXamlReader = new RtfToXamlReader(rtfContent);
				rtfToXamlReader.ForceParagraph = this.ForceParagraph;
				if (this.WpfPayload != null)
				{
					rtfToXamlReader.WpfPayload = this.WpfPayload;
				}
				rtfToXamlReader.Process();
				result = rtfToXamlReader.Output;
			}
			return result;
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06003F39 RID: 16185 RVA: 0x001211A8 File Offset: 0x0011F3A8
		// (set) Token: 0x06003F3A RID: 16186 RVA: 0x001211B0 File Offset: 0x0011F3B0
		internal bool ForceParagraph
		{
			get
			{
				return this._forceParagraph;
			}
			set
			{
				this._forceParagraph = value;
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06003F3B RID: 16187 RVA: 0x001211B9 File Offset: 0x0011F3B9
		// (set) Token: 0x06003F3C RID: 16188 RVA: 0x001211C1 File Offset: 0x0011F3C1
		internal WpfPayload WpfPayload
		{
			get
			{
				return this._wpfPayload;
			}
			set
			{
				this._wpfPayload = value;
			}
		}

		// Token: 0x04002710 RID: 10000
		internal const int RtfCodePage = 1252;

		// Token: 0x04002711 RID: 10001
		private bool _forceParagraph;

		// Token: 0x04002712 RID: 10002
		private WpfPayload _wpfPayload;
	}
}
