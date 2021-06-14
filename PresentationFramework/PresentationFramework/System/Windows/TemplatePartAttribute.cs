using System;

namespace System.Windows
{
	/// <summary>Represents an attribute that is applied to the class definition to identify the types of the named parts that are used for templating.</summary>
	// Token: 0x02000124 RID: 292
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class TemplatePartAttribute : Attribute
	{
		/// <summary>Gets or sets the pre-defined name of the part.</summary>
		/// <returns>The pre-defined name of the part.</returns>
		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000C23 RID: 3107 RVA: 0x0002D4E2 File Offset: 0x0002B6E2
		// (set) Token: 0x06000C24 RID: 3108 RVA: 0x0002D4EA File Offset: 0x0002B6EA
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		/// <summary>Gets or sets the type of the named part this attribute is identifying.</summary>
		/// <returns>The type of the named part this attribute is identifying.</returns>
		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000C25 RID: 3109 RVA: 0x0002D4F3 File Offset: 0x0002B6F3
		// (set) Token: 0x06000C26 RID: 3110 RVA: 0x0002D4FB File Offset: 0x0002B6FB
		public Type Type
		{
			get
			{
				return this._type;
			}
			set
			{
				this._type = value;
			}
		}

		// Token: 0x04000AD6 RID: 2774
		private string _name;

		// Token: 0x04000AD7 RID: 2775
		private Type _type;
	}
}
