using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MS.Internal.Ink
{
	// Token: 0x02000695 RID: 1685
	internal class TextClipboardData : ElementsClipboardData
	{
		// Token: 0x06006E01 RID: 28161 RVA: 0x001FA5BF File Offset: 0x001F87BF
		internal TextClipboardData() : this(null)
		{
		}

		// Token: 0x06006E02 RID: 28162 RVA: 0x001FA5C8 File Offset: 0x001F87C8
		internal TextClipboardData(string text)
		{
			this._text = text;
		}

		// Token: 0x06006E03 RID: 28163 RVA: 0x001FA5D7 File Offset: 0x001F87D7
		internal override bool CanPaste(IDataObject dataObject)
		{
			return dataObject.GetDataPresent(DataFormats.UnicodeText, false) || dataObject.GetDataPresent(DataFormats.Text, false) || dataObject.GetDataPresent(DataFormats.OemText, false);
		}

		// Token: 0x06006E04 RID: 28164 RVA: 0x001FA603 File Offset: 0x001F8803
		protected override bool CanCopy()
		{
			return !string.IsNullOrEmpty(this._text);
		}

		// Token: 0x06006E05 RID: 28165 RVA: 0x001FA613 File Offset: 0x001F8813
		protected override void DoCopy(IDataObject dataObject)
		{
			dataObject.SetData(DataFormats.UnicodeText, this._text, true);
		}

		// Token: 0x06006E06 RID: 28166 RVA: 0x001FA628 File Offset: 0x001F8828
		protected override void DoPaste(IDataObject dataObject)
		{
			base.ElementList = new List<UIElement>();
			string text = dataObject.GetData(DataFormats.UnicodeText, true) as string;
			if (string.IsNullOrEmpty(text))
			{
				text = (dataObject.GetData(DataFormats.Text, true) as string);
			}
			if (!string.IsNullOrEmpty(text))
			{
				TextBox textBox = new TextBox();
				textBox.Text = text;
				textBox.TextWrapping = TextWrapping.Wrap;
				base.ElementList.Add(textBox);
			}
		}

		// Token: 0x04003620 RID: 13856
		private string _text;
	}
}
