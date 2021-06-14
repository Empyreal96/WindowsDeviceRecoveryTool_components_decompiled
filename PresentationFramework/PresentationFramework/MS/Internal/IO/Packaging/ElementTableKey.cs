using System;

namespace MS.Internal.IO.Packaging
{
	// Token: 0x0200065D RID: 1629
	internal class ElementTableKey
	{
		// Token: 0x06006C1C RID: 27676 RVA: 0x001F1CFD File Offset: 0x001EFEFD
		internal ElementTableKey(string xmlNamespace, string baseName)
		{
			if (xmlNamespace == null)
			{
				throw new ArgumentNullException("xmlNamespace");
			}
			if (baseName == null)
			{
				throw new ArgumentNullException("baseName");
			}
			this._xmlNamespace = xmlNamespace;
			this._baseName = baseName;
		}

		// Token: 0x06006C1D RID: 27677 RVA: 0x001F1D30 File Offset: 0x001EFF30
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return false;
			}
			if (other.GetType() != base.GetType())
			{
				return false;
			}
			ElementTableKey elementTableKey = (ElementTableKey)other;
			return string.CompareOrdinal(this.BaseName, elementTableKey.BaseName) == 0 && string.CompareOrdinal(this.XmlNamespace, elementTableKey.XmlNamespace) == 0;
		}

		// Token: 0x06006C1E RID: 27678 RVA: 0x001F1D87 File Offset: 0x001EFF87
		public override int GetHashCode()
		{
			return this.XmlNamespace.GetHashCode() ^ this.BaseName.GetHashCode();
		}

		// Token: 0x170019D9 RID: 6617
		// (get) Token: 0x06006C1F RID: 27679 RVA: 0x001F1DA0 File Offset: 0x001EFFA0
		internal string XmlNamespace
		{
			get
			{
				return this._xmlNamespace;
			}
		}

		// Token: 0x170019DA RID: 6618
		// (get) Token: 0x06006C20 RID: 27680 RVA: 0x001F1DA8 File Offset: 0x001EFFA8
		internal string BaseName
		{
			get
			{
				return this._baseName;
			}
		}

		// Token: 0x0400350D RID: 13581
		private string _baseName;

		// Token: 0x0400350E RID: 13582
		private string _xmlNamespace;

		// Token: 0x0400350F RID: 13583
		public static readonly string XamlNamespace = "http://schemas.microsoft.com/winfx/2006/xaml/presentation";

		// Token: 0x04003510 RID: 13584
		public static readonly string FixedMarkupNamespace = "http://schemas.microsoft.com/xps/2005/06";
	}
}
