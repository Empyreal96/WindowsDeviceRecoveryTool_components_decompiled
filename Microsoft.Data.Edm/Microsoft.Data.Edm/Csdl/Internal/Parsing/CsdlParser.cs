using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Data.Edm.Csdl.Internal.Parsing.Ast;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl.Internal.Parsing
{
	// Token: 0x02000161 RID: 353
	internal class CsdlParser
	{
		// Token: 0x0600073D RID: 1853 RVA: 0x00013B30 File Offset: 0x00011D30
		public static bool TryParse(IEnumerable<XmlReader> csdlReaders, out CsdlModel entityModel, out IEnumerable<EdmError> errors)
		{
			EdmUtil.CheckArgumentNull<IEnumerable<XmlReader>>(csdlReaders, "csdlReaders");
			CsdlParser csdlParser = new CsdlParser();
			int num = 0;
			foreach (XmlReader xmlReader in csdlReaders)
			{
				if (xmlReader != null)
				{
					try
					{
						csdlParser.AddReader(xmlReader);
						goto IL_94;
					}
					catch (XmlException ex)
					{
						entityModel = null;
						errors = new EdmError[]
						{
							new EdmError(new CsdlLocation(ex.LineNumber, ex.LinePosition), EdmErrorCode.XmlError, ex.Message)
						};
						return false;
					}
					goto IL_6C;
					IL_94:
					num++;
					continue;
				}
				IL_6C:
				entityModel = null;
				errors = new EdmError[]
				{
					new EdmError(null, EdmErrorCode.NullXmlReader, Strings.CsdlParser_NullXmlReader)
				};
				return false;
			}
			if (num == 0)
			{
				entityModel = null;
				errors = new EdmError[]
				{
					new EdmError(null, EdmErrorCode.NoReadersProvided, Strings.CsdlParser_NoReadersProvided)
				};
				return false;
			}
			bool flag = csdlParser.GetResult(out entityModel, out errors);
			if (!flag)
			{
				entityModel = null;
			}
			return flag;
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x00013C4C File Offset: 0x00011E4C
		public bool AddReader(XmlReader csdlReader)
		{
			string documentPath = csdlReader.BaseURI ?? string.Empty;
			CsdlDocumentParser csdlDocumentParser = new CsdlDocumentParser(documentPath, csdlReader);
			csdlDocumentParser.ParseDocumentElement();
			this.success &= !csdlDocumentParser.HasErrors;
			this.errorsList.AddRange(csdlDocumentParser.Errors);
			if (csdlDocumentParser.Result != null)
			{
				this.result.AddSchema(csdlDocumentParser.Result.Value);
			}
			return this.success;
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x00013CC2 File Offset: 0x00011EC2
		public bool GetResult(out CsdlModel model, out IEnumerable<EdmError> errors)
		{
			model = this.result;
			errors = this.errorsList;
			return this.success;
		}

		// Token: 0x0400039C RID: 924
		private readonly List<EdmError> errorsList = new List<EdmError>();

		// Token: 0x0400039D RID: 925
		private readonly CsdlModel result = new CsdlModel();

		// Token: 0x0400039E RID: 926
		private bool success = true;
	}
}
