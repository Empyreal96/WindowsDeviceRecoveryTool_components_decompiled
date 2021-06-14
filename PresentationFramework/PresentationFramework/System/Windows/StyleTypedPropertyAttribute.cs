using System;

namespace System.Windows
{
	/// <summary>Represents an attribute that is applied to the class definition and determines the <see cref="P:System.Windows.Style.TargetType" />s of the properties that are of type <see cref="T:System.Windows.Style" />.</summary>
	// Token: 0x0200010C RID: 268
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class StyleTypedPropertyAttribute : Attribute
	{
		/// <summary>Gets or sets the name of the property that is of type <see cref="T:System.Windows.Style" />.</summary>
		/// <returns>The name of the property that is of type <see cref="T:System.Windows.Style" />.</returns>
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x0002101C File Offset: 0x0001F21C
		// (set) Token: 0x0600096B RID: 2411 RVA: 0x00021024 File Offset: 0x0001F224
		public string Property
		{
			get
			{
				return this._property;
			}
			set
			{
				this._property = value;
			}
		}

		/// <summary>Gets or sets the <see cref="P:System.Windows.Style.TargetType" /> of the <see cref="P:System.Windows.StyleTypedPropertyAttribute.Property" /> this attribute is specifying.</summary>
		/// <returns>The <see cref="P:System.Windows.Style.TargetType" /> of the <see cref="P:System.Windows.StyleTypedPropertyAttribute.Property" /> this attribute is specifying.</returns>
		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x0002102D File Offset: 0x0001F22D
		// (set) Token: 0x0600096D RID: 2413 RVA: 0x00021035 File Offset: 0x0001F235
		public Type StyleTargetType
		{
			get
			{
				return this._styleTargetType;
			}
			set
			{
				this._styleTargetType = value;
			}
		}

		// Token: 0x0400081D RID: 2077
		private string _property;

		// Token: 0x0400081E RID: 2078
		private Type _styleTargetType;
	}
}
