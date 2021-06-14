using System;

namespace System.Windows.Markup
{
	// Token: 0x020001CF RID: 463
	internal static class BamlRecordHelper
	{
		// Token: 0x06001DCB RID: 7627 RVA: 0x0008C739 File Offset: 0x0008A939
		internal static bool IsMapTableRecordType(BamlRecordType bamlRecordType)
		{
			return bamlRecordType - BamlRecordType.PIMapping <= 5;
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x0008C745 File Offset: 0x0008A945
		internal static bool IsDebugBamlRecordType(BamlRecordType recordType)
		{
			return recordType == BamlRecordType.LineNumberAndPosition || recordType == BamlRecordType.LinePosition;
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x0008C754 File Offset: 0x0008A954
		internal static bool HasDebugExtensionRecord(bool isDebugBamlStream, BamlRecord bamlRecord)
		{
			return isDebugBamlStream && bamlRecord.Next != null && BamlRecordHelper.IsDebugBamlRecordType(bamlRecord.Next.RecordType);
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x0008C778 File Offset: 0x0008A978
		internal static bool DoesRecordTypeHaveDebugExtension(BamlRecordType recordType)
		{
			switch (recordType)
			{
			case BamlRecordType.DocumentStart:
			case BamlRecordType.DocumentEnd:
			case BamlRecordType.PropertyCustom:
			case BamlRecordType.PropertyComplexEnd:
			case BamlRecordType.PropertyArrayEnd:
			case BamlRecordType.PropertyIListEnd:
			case BamlRecordType.PropertyIDictionaryEnd:
			case BamlRecordType.LiteralContent:
			case BamlRecordType.Text:
			case BamlRecordType.TextWithConverter:
			case BamlRecordType.RoutedEvent:
			case BamlRecordType.ClrEvent:
			case BamlRecordType.XmlAttribute:
			case BamlRecordType.ProcessingInstruction:
			case BamlRecordType.Comment:
			case BamlRecordType.DefTag:
			case BamlRecordType.DefAttribute:
			case BamlRecordType.EndAttributes:
			case BamlRecordType.AssemblyInfo:
			case BamlRecordType.TypeInfo:
			case BamlRecordType.TypeSerializerInfo:
			case BamlRecordType.AttributeInfo:
			case BamlRecordType.StringInfo:
			case BamlRecordType.PropertyStringReference:
			case BamlRecordType.DeferableContentStart:
			case BamlRecordType.DefAttributeKeyString:
			case BamlRecordType.DefAttributeKeyType:
			case BamlRecordType.KeyElementEnd:
			case BamlRecordType.ConstructorParametersStart:
			case BamlRecordType.ConstructorParametersEnd:
			case BamlRecordType.ConstructorParameterType:
			case BamlRecordType.NamedElementStart:
			case BamlRecordType.StaticResourceEnd:
			case BamlRecordType.StaticResourceId:
			case BamlRecordType.TextWithId:
			case BamlRecordType.LineNumberAndPosition:
			case BamlRecordType.LinePosition:
			case BamlRecordType.OptimizedStaticResource:
			case BamlRecordType.PropertyWithStaticResourceId:
				return false;
			case BamlRecordType.ElementStart:
			case BamlRecordType.ElementEnd:
			case BamlRecordType.Property:
			case BamlRecordType.PropertyComplexStart:
			case BamlRecordType.PropertyArrayStart:
			case BamlRecordType.PropertyIListStart:
			case BamlRecordType.PropertyIDictionaryStart:
			case BamlRecordType.XmlnsProperty:
			case BamlRecordType.PIMapping:
			case BamlRecordType.PropertyTypeReference:
			case BamlRecordType.PropertyWithExtension:
			case BamlRecordType.PropertyWithConverter:
			case BamlRecordType.KeyElementStart:
			case BamlRecordType.ConnectionId:
			case BamlRecordType.ContentProperty:
			case BamlRecordType.StaticResourceStart:
			case BamlRecordType.PresentationOptionsAttribute:
				return true;
			default:
				return false;
			}
		}
	}
}
