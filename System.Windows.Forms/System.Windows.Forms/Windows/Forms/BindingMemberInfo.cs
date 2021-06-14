using System;

namespace System.Windows.Forms
{
	/// <summary>Contains information that enables a <see cref="T:System.Windows.Forms.Binding" /> to resolve a data binding to either the property of an object or the property of the current object in a list of objects.</summary>
	// Token: 0x02000128 RID: 296
	public struct BindingMemberInfo
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.BindingMemberInfo" /> class.</summary>
		/// <param name="dataMember">A navigation path that resolves to either the property of an object or the property of the current object in a list of objects. </param>
		// Token: 0x06000807 RID: 2055 RVA: 0x00018174 File Offset: 0x00016374
		public BindingMemberInfo(string dataMember)
		{
			if (dataMember == null)
			{
				dataMember = "";
			}
			int num = dataMember.LastIndexOf(".");
			if (num != -1)
			{
				this.dataList = dataMember.Substring(0, num);
				this.dataField = dataMember.Substring(num + 1);
				return;
			}
			this.dataList = "";
			this.dataField = dataMember;
		}

		/// <summary>Gets the property name, or the period-delimited hierarchy of property names, that comes before the property name of the data-bound object.</summary>
		/// <returns>The property name, or the period-delimited hierarchy of property names, that comes before the data-bound object property name.</returns>
		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x000181CB File Offset: 0x000163CB
		public string BindingPath
		{
			get
			{
				if (this.dataList == null)
				{
					return "";
				}
				return this.dataList;
			}
		}

		/// <summary>Gets the property name of the data-bound object.</summary>
		/// <returns>The property name of the data-bound object. This can be an empty string ("").</returns>
		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x000181E1 File Offset: 0x000163E1
		public string BindingField
		{
			get
			{
				if (this.dataField == null)
				{
					return "";
				}
				return this.dataField;
			}
		}

		/// <summary>Gets the information that is used to specify the property name of the data-bound object.</summary>
		/// <returns>An empty string (""), a single property name, or a hierarchy of period-delimited property names that resolves to the property name of the final data-bound object.</returns>
		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x000181F7 File Offset: 0x000163F7
		public string BindingMember
		{
			get
			{
				if (this.BindingPath.Length <= 0)
				{
					return this.BindingField;
				}
				return this.BindingPath + "." + this.BindingField;
			}
		}

		/// <summary>Determines whether the specified object is equal to this <see cref="T:System.Windows.Forms.BindingMemberInfo" />.</summary>
		/// <param name="otherObject">The object to compare for equality.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="otherObject" /> is a <see cref="T:System.Windows.Forms.BindingMemberInfo" /> and both <see cref="P:System.Windows.Forms.BindingMemberInfo.BindingMember" /> strings are equal; otherwise <see langword="false" />.</returns>
		// Token: 0x0600080B RID: 2059 RVA: 0x00018224 File Offset: 0x00016424
		public override bool Equals(object otherObject)
		{
			if (otherObject is BindingMemberInfo)
			{
				BindingMemberInfo bindingMemberInfo = (BindingMemberInfo)otherObject;
				return string.Equals(this.BindingMember, bindingMemberInfo.BindingMember, StringComparison.OrdinalIgnoreCase);
			}
			return false;
		}

		/// <summary>Determines whether two <see cref="T:System.Windows.Forms.BindingMemberInfo" /> objects are equal.</summary>
		/// <param name="a">The first <see cref="T:System.Windows.Forms.BindingMemberInfo" /> to compare for equality.</param>
		/// <param name="b">The second <see cref="T:System.Windows.Forms.BindingMemberInfo" /> to compare for equality.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.BindingMemberInfo.BindingMember" /> strings for <paramref name="a" /> and <paramref name="b" /> are equal; otherwise <see langword="false" />.</returns>
		// Token: 0x0600080C RID: 2060 RVA: 0x00018255 File Offset: 0x00016455
		public static bool operator ==(BindingMemberInfo a, BindingMemberInfo b)
		{
			return a.Equals(b);
		}

		/// <summary>Determines whether two <see cref="T:System.Windows.Forms.BindingMemberInfo" /> objects are not equal.</summary>
		/// <param name="a">The first <see cref="T:System.Windows.Forms.BindingMemberInfo" /> to compare for inequality.</param>
		/// <param name="b">The second <see cref="T:System.Windows.Forms.BindingMemberInfo" /> to compare for inequality.</param>
		/// <returns>
		///     <see langword="true" /> if the <see cref="P:System.Windows.Forms.BindingMemberInfo.BindingMember" /> strings for <paramref name="a" /> and <paramref name="b" /> are not equal; otherwise <see langword="false" />.</returns>
		// Token: 0x0600080D RID: 2061 RVA: 0x0001826A File Offset: 0x0001646A
		public static bool operator !=(BindingMemberInfo a, BindingMemberInfo b)
		{
			return !a.Equals(b);
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Windows.Forms.BindingMemberInfo" />.</summary>
		/// <returns>The hash code for this <see cref="T:System.Windows.Forms.BindingMemberInfo" />.</returns>
		// Token: 0x0600080E RID: 2062 RVA: 0x00018282 File Offset: 0x00016482
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x04000610 RID: 1552
		private string dataList;

		// Token: 0x04000611 RID: 1553
		private string dataField;
	}
}
