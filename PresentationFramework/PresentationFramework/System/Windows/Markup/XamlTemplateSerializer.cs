using System;
using System.Globalization;

namespace System.Windows.Markup
{
	// Token: 0x0200026C RID: 620
	internal class XamlTemplateSerializer : XamlSerializer
	{
		// Token: 0x0600237D RID: 9085 RVA: 0x000AD5D8 File Offset: 0x000AB7D8
		internal override object GetDictionaryKey(BamlRecord startRecord, ParserContext parserContext)
		{
			object obj = null;
			int num = 0;
			BamlRecord bamlRecord = startRecord;
			short num2 = 0;
			while (bamlRecord != null)
			{
				if (bamlRecord.RecordType == BamlRecordType.ElementStart)
				{
					BamlElementStartRecord bamlElementStartRecord = bamlRecord as BamlElementStartRecord;
					if (++num != 1)
					{
						break;
					}
					num2 = bamlElementStartRecord.TypeId;
				}
				else if (bamlRecord.RecordType == BamlRecordType.Property && num == 1)
				{
					BamlPropertyRecord bamlPropertyRecord = bamlRecord as BamlPropertyRecord;
					short num3;
					string a;
					BamlAttributeUsage bamlAttributeUsage;
					parserContext.MapTable.GetAttributeInfoFromId(bamlPropertyRecord.AttributeId, out num3, out a, out bamlAttributeUsage);
					if (num3 == num2)
					{
						if (a == "TargetType")
						{
							obj = parserContext.XamlTypeMapper.GetDictionaryKey(bamlPropertyRecord.Value, parserContext);
						}
						else if (a == "DataType")
						{
							object dictionaryKey = parserContext.XamlTypeMapper.GetDictionaryKey(bamlPropertyRecord.Value, parserContext);
							Exception ex = TemplateKey.ValidateDataType(dictionaryKey, null);
							if (ex != null)
							{
								this.ThrowException("TemplateBadDictionaryKey", parserContext.LineNumber, parserContext.LinePosition, ex);
							}
							obj = new DataTemplateKey(dictionaryKey);
						}
					}
				}
				else if (bamlRecord.RecordType == BamlRecordType.PropertyComplexStart || bamlRecord.RecordType == BamlRecordType.PropertyIListStart || bamlRecord.RecordType == BamlRecordType.ElementEnd)
				{
					break;
				}
				bamlRecord = bamlRecord.Next;
			}
			if (obj == null)
			{
				this.ThrowException("StyleNoDictionaryKey", parserContext.LineNumber, parserContext.LinePosition, null);
			}
			return obj;
		}

		// Token: 0x0600237E RID: 9086 RVA: 0x000AD714 File Offset: 0x000AB914
		private void ThrowException(string id, int lineNumber, int linePosition, Exception innerException)
		{
			string text = SR.Get(id);
			XamlParseException ex;
			if (lineNumber > 0)
			{
				text += " ";
				text += SR.Get("ParserLineAndOffset", new object[]
				{
					lineNumber.ToString(CultureInfo.CurrentUICulture),
					linePosition.ToString(CultureInfo.CurrentUICulture)
				});
				ex = new XamlParseException(text, lineNumber, linePosition);
			}
			else
			{
				ex = new XamlParseException(text);
			}
			throw ex;
		}

		// Token: 0x04001AA1 RID: 6817
		internal const string ControlTemplateTagName = "ControlTemplate";

		// Token: 0x04001AA2 RID: 6818
		internal const string DataTemplateTagName = "DataTemplate";

		// Token: 0x04001AA3 RID: 6819
		internal const string HierarchicalDataTemplateTagName = "HierarchicalDataTemplate";

		// Token: 0x04001AA4 RID: 6820
		internal const string ItemsPanelTemplateTagName = "ItemsPanelTemplate";

		// Token: 0x04001AA5 RID: 6821
		internal const string TargetTypePropertyName = "TargetType";

		// Token: 0x04001AA6 RID: 6822
		internal const string DataTypePropertyName = "DataType";

		// Token: 0x04001AA7 RID: 6823
		internal const string TriggersPropertyName = "Triggers";

		// Token: 0x04001AA8 RID: 6824
		internal const string ResourcesPropertyName = "Resources";

		// Token: 0x04001AA9 RID: 6825
		internal const string SettersPropertyName = "Setters";

		// Token: 0x04001AAA RID: 6826
		internal const string ItemsSourcePropertyName = "ItemsSource";

		// Token: 0x04001AAB RID: 6827
		internal const string ItemTemplatePropertyName = "ItemTemplate";

		// Token: 0x04001AAC RID: 6828
		internal const string ItemTemplateSelectorPropertyName = "ItemTemplateSelector";

		// Token: 0x04001AAD RID: 6829
		internal const string ItemContainerStylePropertyName = "ItemContainerStyle";

		// Token: 0x04001AAE RID: 6830
		internal const string ItemContainerStyleSelectorPropertyName = "ItemContainerStyleSelector";

		// Token: 0x04001AAF RID: 6831
		internal const string ItemStringFormatPropertyName = "ItemStringFormat";

		// Token: 0x04001AB0 RID: 6832
		internal const string ItemBindingGroupPropertyName = "ItemBindingGroup";

		// Token: 0x04001AB1 RID: 6833
		internal const string AlternationCountPropertyName = "AlternationCount";

		// Token: 0x04001AB2 RID: 6834
		internal const string ControlTemplateTriggersFullPropertyName = "ControlTemplate.Triggers";

		// Token: 0x04001AB3 RID: 6835
		internal const string ControlTemplateResourcesFullPropertyName = "ControlTemplate.Resources";

		// Token: 0x04001AB4 RID: 6836
		internal const string DataTemplateTriggersFullPropertyName = "DataTemplate.Triggers";

		// Token: 0x04001AB5 RID: 6837
		internal const string DataTemplateResourcesFullPropertyName = "DataTemplate.Resources";

		// Token: 0x04001AB6 RID: 6838
		internal const string HierarchicalDataTemplateTriggersFullPropertyName = "HierarchicalDataTemplate.Triggers";

		// Token: 0x04001AB7 RID: 6839
		internal const string HierarchicalDataTemplateItemsSourceFullPropertyName = "HierarchicalDataTemplate.ItemsSource";

		// Token: 0x04001AB8 RID: 6840
		internal const string HierarchicalDataTemplateItemTemplateFullPropertyName = "HierarchicalDataTemplate.ItemTemplate";

		// Token: 0x04001AB9 RID: 6841
		internal const string HierarchicalDataTemplateItemTemplateSelectorFullPropertyName = "HierarchicalDataTemplate.ItemTemplateSelector";

		// Token: 0x04001ABA RID: 6842
		internal const string HierarchicalDataTemplateItemContainerStyleFullPropertyName = "HierarchicalDataTemplate.ItemContainerStyle";

		// Token: 0x04001ABB RID: 6843
		internal const string HierarchicalDataTemplateItemContainerStyleSelectorFullPropertyName = "HierarchicalDataTemplate.ItemContainerStyleSelector";

		// Token: 0x04001ABC RID: 6844
		internal const string HierarchicalDataTemplateItemStringFormatFullPropertyName = "HierarchicalDataTemplate.ItemStringFormat";

		// Token: 0x04001ABD RID: 6845
		internal const string HierarchicalDataTemplateItemBindingGroupFullPropertyName = "HierarchicalDataTemplate.ItemBindingGroup";

		// Token: 0x04001ABE RID: 6846
		internal const string HierarchicalDataTemplateAlternationCountFullPropertyName = "HierarchicalDataTemplate.AlternationCount";

		// Token: 0x04001ABF RID: 6847
		internal const string PropertyTriggerPropertyName = "Property";

		// Token: 0x04001AC0 RID: 6848
		internal const string PropertyTriggerValuePropertyName = "Value";

		// Token: 0x04001AC1 RID: 6849
		internal const string PropertyTriggerSourceName = "SourceName";

		// Token: 0x04001AC2 RID: 6850
		internal const string PropertyTriggerEnterActions = "EnterActions";

		// Token: 0x04001AC3 RID: 6851
		internal const string PropertyTriggerExitActions = "ExitActions";

		// Token: 0x04001AC4 RID: 6852
		internal const string DataTriggerBindingPropertyName = "Binding";

		// Token: 0x04001AC5 RID: 6853
		internal const string EventTriggerEventName = "RoutedEvent";

		// Token: 0x04001AC6 RID: 6854
		internal const string EventTriggerSourceName = "SourceName";

		// Token: 0x04001AC7 RID: 6855
		internal const string EventTriggerActions = "Actions";

		// Token: 0x04001AC8 RID: 6856
		internal const string MultiPropertyTriggerConditionsPropertyName = "Conditions";

		// Token: 0x04001AC9 RID: 6857
		internal const string SetterTagName = "Setter";

		// Token: 0x04001ACA RID: 6858
		internal const string SetterPropertyAttributeName = "Property";

		// Token: 0x04001ACB RID: 6859
		internal const string SetterValueAttributeName = "Value";

		// Token: 0x04001ACC RID: 6860
		internal const string SetterTargetAttributeName = "TargetName";

		// Token: 0x04001ACD RID: 6861
		internal const string SetterEventAttributeName = "Event";

		// Token: 0x04001ACE RID: 6862
		internal const string SetterHandlerAttributeName = "Handler";
	}
}
