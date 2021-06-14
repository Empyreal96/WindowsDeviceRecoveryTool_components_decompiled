using System;
using System.Globalization;

namespace System.Windows.Markup
{
	// Token: 0x0200026B RID: 619
	internal class XamlStyleSerializer : XamlSerializer
	{
		// Token: 0x0600237A RID: 9082 RVA: 0x000AD478 File Offset: 0x000AB678
		internal override object GetDictionaryKey(BamlRecord startRecord, ParserContext parserContext)
		{
			Type result = Style.DefaultTargetType;
			bool flag = false;
			object obj = null;
			int num = 0;
			BamlRecord bamlRecord = startRecord;
			short ownerTypeId = 0;
			while (bamlRecord != null)
			{
				if (bamlRecord.RecordType == BamlRecordType.ElementStart)
				{
					BamlElementStartRecord bamlElementStartRecord = bamlRecord as BamlElementStartRecord;
					if (++num == 1)
					{
						ownerTypeId = bamlElementStartRecord.TypeId;
					}
					else if (num == 2)
					{
						result = parserContext.MapTable.GetTypeFromId(bamlElementStartRecord.TypeId);
						flag = true;
						break;
					}
				}
				else if (bamlRecord.RecordType == BamlRecordType.Property && num == 1)
				{
					BamlPropertyRecord bamlPropertyRecord = bamlRecord as BamlPropertyRecord;
					if (parserContext.MapTable.DoesAttributeMatch(bamlPropertyRecord.AttributeId, ownerTypeId, "TargetType"))
					{
						obj = parserContext.XamlTypeMapper.GetDictionaryKey(bamlPropertyRecord.Value, parserContext);
					}
				}
				else if (bamlRecord.RecordType == BamlRecordType.PropertyComplexStart || bamlRecord.RecordType == BamlRecordType.PropertyIListStart)
				{
					break;
				}
				bamlRecord = bamlRecord.Next;
			}
			if (obj == null)
			{
				if (!flag)
				{
					this.ThrowException("StyleNoDictionaryKey", parserContext.LineNumber, parserContext.LinePosition);
				}
				return result;
			}
			return obj;
		}

		// Token: 0x0600237B RID: 9083 RVA: 0x000AD56C File Offset: 0x000AB76C
		private void ThrowException(string id, int lineNumber, int linePosition)
		{
			string text = SR.Get(id);
			XamlParseException ex;
			if (lineNumber > 0)
			{
				text += " ";
				text += SR.Get("ParserLineAndOffset", new object[]
				{
					lineNumber.ToString(CultureInfo.CurrentCulture),
					linePosition.ToString(CultureInfo.CurrentCulture)
				});
				ex = new XamlParseException(text, lineNumber, linePosition);
			}
			else
			{
				ex = new XamlParseException(text);
			}
			throw ex;
		}

		// Token: 0x04001A88 RID: 6792
		internal const string StyleTagName = "Style";

		// Token: 0x04001A89 RID: 6793
		internal const string TargetTypePropertyName = "TargetType";

		// Token: 0x04001A8A RID: 6794
		internal const string BasedOnPropertyName = "BasedOn";

		// Token: 0x04001A8B RID: 6795
		internal const string VisualTriggersPropertyName = "Triggers";

		// Token: 0x04001A8C RID: 6796
		internal const string ResourcesPropertyName = "Resources";

		// Token: 0x04001A8D RID: 6797
		internal const string SettersPropertyName = "Setters";

		// Token: 0x04001A8E RID: 6798
		internal const string VisualTriggersFullPropertyName = "Style.Triggers";

		// Token: 0x04001A8F RID: 6799
		internal const string SettersFullPropertyName = "Style.Setters";

		// Token: 0x04001A90 RID: 6800
		internal const string ResourcesFullPropertyName = "Style.Resources";

		// Token: 0x04001A91 RID: 6801
		internal const string PropertyTriggerPropertyName = "Property";

		// Token: 0x04001A92 RID: 6802
		internal const string PropertyTriggerValuePropertyName = "Value";

		// Token: 0x04001A93 RID: 6803
		internal const string PropertyTriggerSourceName = "SourceName";

		// Token: 0x04001A94 RID: 6804
		internal const string PropertyTriggerEnterActions = "EnterActions";

		// Token: 0x04001A95 RID: 6805
		internal const string PropertyTriggerExitActions = "ExitActions";

		// Token: 0x04001A96 RID: 6806
		internal const string DataTriggerBindingPropertyName = "Binding";

		// Token: 0x04001A97 RID: 6807
		internal const string EventTriggerEventName = "RoutedEvent";

		// Token: 0x04001A98 RID: 6808
		internal const string EventTriggerSourceName = "SourceName";

		// Token: 0x04001A99 RID: 6809
		internal const string EventTriggerActions = "Actions";

		// Token: 0x04001A9A RID: 6810
		internal const string MultiPropertyTriggerConditionsPropertyName = "Conditions";

		// Token: 0x04001A9B RID: 6811
		internal const string SetterTagName = "Setter";

		// Token: 0x04001A9C RID: 6812
		internal const string SetterPropertyAttributeName = "Property";

		// Token: 0x04001A9D RID: 6813
		internal const string SetterValueAttributeName = "Value";

		// Token: 0x04001A9E RID: 6814
		internal const string SetterTargetAttributeName = "TargetName";

		// Token: 0x04001A9F RID: 6815
		internal const string SetterEventAttributeName = "Event";

		// Token: 0x04001AA0 RID: 6816
		internal const string SetterHandlerAttributeName = "Handler";
	}
}
