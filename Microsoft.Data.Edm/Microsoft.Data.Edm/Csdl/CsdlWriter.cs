using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Data.Edm.Csdl.Internal.Serialization;
using Microsoft.Data.Edm.Library;
using Microsoft.Data.Edm.Validation;

namespace Microsoft.Data.Edm.Csdl
{
	// Token: 0x020001B5 RID: 437
	public static class CsdlWriter
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x0001F2AC File Offset: 0x0001D4AC
		public static bool TryWriteCsdl(this IEdmModel model, XmlWriter writer, out IEnumerable<EdmError> errors)
		{
			return CsdlWriter.TryWriteCsdl(model, (string x) => writer, true, out errors);
		}

		// Token: 0x06000AA1 RID: 2721 RVA: 0x0001F2DA File Offset: 0x0001D4DA
		public static bool TryWriteCsdl(this IEdmModel model, Func<string, XmlWriter> writerProvider, out IEnumerable<EdmError> errors)
		{
			return CsdlWriter.TryWriteCsdl(model, writerProvider, false, out errors);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x0001F2E8 File Offset: 0x0001D4E8
		internal static bool TryWriteCsdl(IEdmModel model, Func<string, XmlWriter> writerProvider, bool singleFileExpected, out IEnumerable<EdmError> errors)
		{
			EdmUtil.CheckArgumentNull<IEdmModel>(model, "model");
			EdmUtil.CheckArgumentNull<Func<string, XmlWriter>>(writerProvider, "writerProvider");
			errors = model.GetSerializationErrors();
			if (errors.FirstOrDefault<EdmError>() != null)
			{
				return false;
			}
			IEnumerable<EdmSchema> schemas = new EdmModelSchemaSeparationSerializationVisitor(model).GetSchemas();
			if (schemas.Count<EdmSchema>() > 1 && singleFileExpected)
			{
				errors = new EdmError[]
				{
					new EdmError(new CsdlLocation(0, 0), EdmErrorCode.SingleFileExpected, Strings.Serializer_SingleFileExpected)
				};
				return false;
			}
			if (schemas.Count<EdmSchema>() == 0)
			{
				errors = new EdmError[]
				{
					new EdmError(new CsdlLocation(0, 0), EdmErrorCode.NoSchemasProduced, Strings.Serializer_NoSchemasProduced)
				};
				return false;
			}
			CsdlWriter.WriteSchemas(model, schemas, writerProvider);
			errors = Enumerable.Empty<EdmError>();
			return true;
		}

		// Token: 0x06000AA3 RID: 2723 RVA: 0x0001F39C File Offset: 0x0001D59C
		internal static void WriteSchemas(IEdmModel model, IEnumerable<EdmSchema> schemas, Func<string, XmlWriter> writerProvider)
		{
			Version edmVersion = model.GetEdmVersion() ?? EdmConstants.EdmVersionLatest;
			foreach (EdmSchema edmSchema in schemas)
			{
				XmlWriter xmlWriter = writerProvider(edmSchema.Namespace);
				if (xmlWriter != null)
				{
					EdmModelCsdlSerializationVisitor edmModelCsdlSerializationVisitor = new EdmModelCsdlSerializationVisitor(model, xmlWriter, edmVersion);
					edmModelCsdlSerializationVisitor.VisitEdmSchema(edmSchema, model.GetNamespacePrefixMappings());
				}
			}
		}
	}
}
