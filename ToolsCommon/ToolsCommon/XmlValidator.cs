using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Microsoft.WindowsPhone.ImageUpdate.Tools.Common
{
	// Token: 0x02000037 RID: 55
	public class XmlValidator
	{
		// Token: 0x06000172 RID: 370 RVA: 0x0000899A File Offset: 0x00006B9A
		private static void OnSchemaValidationEvent(object sender, ValidationEventArgs e)
		{
			Console.WriteLine(e.Message);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x000089A7 File Offset: 0x00006BA7
		public XmlValidator() : this(new ValidationEventHandler(XmlValidator.OnSchemaValidationEvent))
		{
		}

		// Token: 0x06000174 RID: 372 RVA: 0x000089BC File Offset: 0x00006BBC
		public XmlValidator(ValidationEventHandler eventHandler)
		{
			this._validationEventHandler = eventHandler;
			this._xmlReaderSettings = new XmlReaderSettings();
			this._xmlReaderSettings.ValidationType = ValidationType.Schema;
			this._xmlReaderSettings.ValidationFlags |= (XmlSchemaValidationFlags.ReportValidationWarnings | XmlSchemaValidationFlags.ProcessIdentityConstraints);
			this._xmlReaderSettings.ValidationEventHandler += this._validationEventHandler;
		}

		// Token: 0x06000175 RID: 373 RVA: 0x00008A12 File Offset: 0x00006C12
		public void AddSchema(XmlSchema schema)
		{
			this._xmlReaderSettings.Schemas.Add(schema);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x00008A28 File Offset: 0x00006C28
		public void AddSchema(string xsdFile)
		{
			using (Stream stream = LongPathFile.OpenRead(xsdFile))
			{
				this.AddSchema(stream);
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x00008A60 File Offset: 0x00006C60
		public void AddSchema(Stream xsdStream)
		{
			this.AddSchema(XmlSchema.Read(xsdStream, this._validationEventHandler));
		}

		// Token: 0x06000178 RID: 376 RVA: 0x00008A74 File Offset: 0x00006C74
		public void Validate(string xmlFile)
		{
			using (Stream stream = LongPathFile.OpenRead(xmlFile))
			{
				this.Validate(stream);
			}
		}

		// Token: 0x06000179 RID: 377 RVA: 0x00008AAC File Offset: 0x00006CAC
		public void Validate(Stream xmlStream)
		{
			XmlReader xmlReader = XmlReader.Create(xmlStream, this._xmlReaderSettings);
			while (xmlReader.Read())
			{
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x00008AD0 File Offset: 0x00006CD0
		public void Validate(XElement element)
		{
			while (element != null && element.GetSchemaInfo() == null)
			{
				element = element.Parent;
			}
			if (element == null)
			{
				throw new ArgumentException("Argument has no SchemaInfo anywhere in the document. Validate the XDocument first.");
			}
			IXmlSchemaInfo schemaInfo = element.GetSchemaInfo();
			element.Validate(schemaInfo.SchemaElement, this._xmlReaderSettings.Schemas, this._validationEventHandler, true);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x00008B25 File Offset: 0x00006D25
		public void Validate(XDocument document)
		{
			document.Validate(this._xmlReaderSettings.Schemas, this._validationEventHandler, true);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00008B3F File Offset: 0x00006D3F
		public XmlReader GetXmlReader(string xmlFile)
		{
			return XmlReader.Create(LongPathFile.OpenRead(xmlFile), this._xmlReaderSettings);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00008B52 File Offset: 0x00006D52
		public XmlReader GetXmlReader(Stream xmlStream)
		{
			return XmlReader.Create(xmlStream, this._xmlReaderSettings);
		}

		// Token: 0x040000E1 RID: 225
		protected ValidationEventHandler _validationEventHandler;

		// Token: 0x040000E2 RID: 226
		protected XmlReaderSettings _xmlReaderSettings;
	}
}
