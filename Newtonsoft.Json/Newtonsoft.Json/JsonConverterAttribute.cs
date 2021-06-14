using System;

namespace Newtonsoft.Json
{
	// Token: 0x02000048 RID: 72
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class JsonConverterAttribute : Attribute
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000AC7B File Offset: 0x00008E7B
		public Type ConverterType
		{
			get
			{
				return this._converterType;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002C1 RID: 705 RVA: 0x0000AC83 File Offset: 0x00008E83
		// (set) Token: 0x060002C2 RID: 706 RVA: 0x0000AC8B File Offset: 0x00008E8B
		public object[] ConverterParameters { get; private set; }

		// Token: 0x060002C3 RID: 707 RVA: 0x0000AC94 File Offset: 0x00008E94
		public JsonConverterAttribute(Type converterType)
		{
			if (converterType == null)
			{
				throw new ArgumentNullException("converterType");
			}
			this._converterType = converterType;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000ACB7 File Offset: 0x00008EB7
		public JsonConverterAttribute(Type converterType, params object[] converterParameters) : this(converterType)
		{
			this.ConverterParameters = converterParameters;
		}

		// Token: 0x040000E2 RID: 226
		private readonly Type _converterType;
	}
}
