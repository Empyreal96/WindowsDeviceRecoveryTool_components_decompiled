using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000723 RID: 1827
	internal sealed class DisplayMemberTemplateSelector : DataTemplateSelector
	{
		// Token: 0x06007509 RID: 29961 RVA: 0x00217A18 File Offset: 0x00215C18
		public DisplayMemberTemplateSelector(string displayMemberPath, string stringFormat)
		{
			this._displayMemberPath = displayMemberPath;
			this._stringFormat = stringFormat;
		}

		// Token: 0x0600750A RID: 29962 RVA: 0x00217A30 File Offset: 0x00215C30
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			if (SystemXmlHelper.IsXmlNode(item))
			{
				if (this._xmlNodeContentTemplate == null)
				{
					this._xmlNodeContentTemplate = new DataTemplate();
					FrameworkElementFactory frameworkElementFactory = ContentPresenter.CreateTextBlockFactory();
					Binding binding = new Binding();
					binding.XPath = this._displayMemberPath;
					binding.StringFormat = this._stringFormat;
					frameworkElementFactory.SetBinding(TextBlock.TextProperty, binding);
					this._xmlNodeContentTemplate.VisualTree = frameworkElementFactory;
					this._xmlNodeContentTemplate.Seal();
				}
				return this._xmlNodeContentTemplate;
			}
			if (this._clrNodeContentTemplate == null)
			{
				this._clrNodeContentTemplate = new DataTemplate();
				FrameworkElementFactory frameworkElementFactory2 = ContentPresenter.CreateTextBlockFactory();
				Binding binding2 = new Binding();
				binding2.Path = new PropertyPath(this._displayMemberPath, new object[0]);
				binding2.StringFormat = this._stringFormat;
				frameworkElementFactory2.SetBinding(TextBlock.TextProperty, binding2);
				this._clrNodeContentTemplate.VisualTree = frameworkElementFactory2;
				this._clrNodeContentTemplate.Seal();
			}
			return this._clrNodeContentTemplate;
		}

		// Token: 0x04003810 RID: 14352
		private string _displayMemberPath;

		// Token: 0x04003811 RID: 14353
		private string _stringFormat;

		// Token: 0x04003812 RID: 14354
		private DataTemplate _xmlNodeContentTemplate;

		// Token: 0x04003813 RID: 14355
		private DataTemplate _clrNodeContentTemplate;
	}
}
