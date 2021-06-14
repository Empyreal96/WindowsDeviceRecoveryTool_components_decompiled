using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Data.Edm;
using Microsoft.Data.Edm.Csdl;
using Microsoft.Data.Edm.Validation;
using Microsoft.Data.OData.Atom;

namespace Microsoft.Data.OData
{
	// Token: 0x020001C9 RID: 457
	internal sealed class ODataMetadataOutputContext : ODataOutputContext
	{
		// Token: 0x06000E34 RID: 3636 RVA: 0x00031C40 File Offset: 0x0002FE40
		internal ODataMetadataOutputContext(ODataFormat format, Stream messageStream, Encoding encoding, ODataMessageWriterSettings messageWriterSettings, bool writingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver) : base(format, messageWriterSettings, writingResponse, synchronous, model, urlResolver)
		{
			try
			{
				this.messageOutputStream = messageStream;
				this.xmlWriter = ODataAtomWriterUtils.CreateXmlWriter(messageStream, messageWriterSettings, encoding);
			}
			catch (Exception e)
			{
				if (ExceptionUtils.IsCatchableExceptionType(e) && messageStream != null)
				{
					messageStream.Dispose();
				}
				throw;
			}
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00031C9C File Offset: 0x0002FE9C
		internal void Flush()
		{
			this.xmlWriter.Flush();
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00031CA9 File Offset: 0x0002FEA9
		internal override void WriteInStreamError(ODataError error, bool includeDebugInformation)
		{
			ODataAtomWriterUtils.WriteError(this.xmlWriter, error, includeDebugInformation, base.MessageWriterSettings.MessageQuotas.MaxNestingDepth);
			this.Flush();
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00031CD0 File Offset: 0x0002FED0
		internal override void WriteMetadataDocument()
		{
			base.Model.SaveODataAnnotations();
			IEnumerable<EdmError> enumerable;
			if (!EdmxWriter.TryWriteEdmx(base.Model, this.xmlWriter, EdmxTarget.OData, out enumerable))
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (EdmError edmError in enumerable)
				{
					stringBuilder.AppendLine(edmError.ToString());
				}
				throw new ODataException(Strings.ODataMetadataOutputContext_ErrorWritingMetadata(stringBuilder.ToString()));
			}
			this.Flush();
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00031D5C File Offset: 0x0002FF5C
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			try
			{
				if (this.xmlWriter != null)
				{
					this.xmlWriter.Flush();
					this.messageOutputStream.Dispose();
				}
			}
			finally
			{
				this.messageOutputStream = null;
				this.xmlWriter = null;
			}
		}

		// Token: 0x040004AE RID: 1198
		private Stream messageOutputStream;

		// Token: 0x040004AF RID: 1199
		private XmlWriter xmlWriter;
	}
}
