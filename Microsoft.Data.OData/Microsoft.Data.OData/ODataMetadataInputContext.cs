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
	// Token: 0x020001FF RID: 511
	internal sealed class ODataMetadataInputContext : ODataInputContext
	{
		// Token: 0x06000F7D RID: 3965 RVA: 0x000375C0 File Offset: 0x000357C0
		internal ODataMetadataInputContext(ODataFormat format, Stream messageStream, Encoding encoding, ODataMessageReaderSettings messageReaderSettings, ODataVersion version, bool readingResponse, bool synchronous, IEdmModel model, IODataUrlResolver urlResolver) : base(format, messageReaderSettings, version, readingResponse, synchronous, model, urlResolver)
		{
			ExceptionUtils.CheckArgumentNotNull<ODataFormat>(format, "format");
			ExceptionUtils.CheckArgumentNotNull<ODataMessageReaderSettings>(messageReaderSettings, "messageReaderSettings");
			try
			{
				this.baseXmlReader = ODataAtomReaderUtils.CreateXmlReader(messageStream, encoding, messageReaderSettings);
				this.xmlReader = new BufferingXmlReader(this.baseXmlReader, null, messageReaderSettings.BaseUri, false, messageReaderSettings.MessageQuotas.MaxNestingDepth, messageReaderSettings.ReaderBehavior.ODataNamespace);
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

		// Token: 0x06000F7E RID: 3966 RVA: 0x00037660 File Offset: 0x00035860
		internal override IEdmModel ReadMetadataDocument()
		{
			return this.ReadMetadataDocumentImplementation();
		}

		// Token: 0x06000F7F RID: 3967 RVA: 0x00037668 File Offset: 0x00035868
		protected override void DisposeImplementation()
		{
			try
			{
				if (this.baseXmlReader != null)
				{
					((IDisposable)this.baseXmlReader).Dispose();
				}
			}
			finally
			{
				this.baseXmlReader = null;
				this.xmlReader = null;
			}
		}

		// Token: 0x06000F80 RID: 3968 RVA: 0x000376AC File Offset: 0x000358AC
		private IEdmModel ReadMetadataDocumentImplementation()
		{
			IEdmModel edmModel;
			IEnumerable<EdmError> enumerable;
			if (!EdmxReader.TryParse(this.xmlReader, out edmModel, out enumerable))
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (EdmError edmError in enumerable)
				{
					stringBuilder.AppendLine(edmError.ToString());
				}
				throw new ODataException(Strings.ODataMetadataInputContext_ErrorReadingMetadata(stringBuilder.ToString()));
			}
			edmModel.LoadODataAnnotations(base.MessageReaderSettings.MessageQuotas.MaxEntityPropertyMappingsPerType);
			return edmModel;
		}

		// Token: 0x04000582 RID: 1410
		private XmlReader baseXmlReader;

		// Token: 0x04000583 RID: 1411
		private BufferingXmlReader xmlReader;
	}
}
