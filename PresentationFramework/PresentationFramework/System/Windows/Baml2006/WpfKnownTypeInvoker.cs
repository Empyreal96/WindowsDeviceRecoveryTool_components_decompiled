using System;
using System.Xaml.Schema;

namespace System.Windows.Baml2006
{
	// Token: 0x02000171 RID: 369
	internal class WpfKnownTypeInvoker : XamlTypeInvoker
	{
		// Token: 0x060015A5 RID: 5541 RVA: 0x0006A1AE File Offset: 0x000683AE
		public WpfKnownTypeInvoker(WpfKnownType type) : base(type)
		{
			this._type = type;
		}

		// Token: 0x060015A6 RID: 5542 RVA: 0x0006A1C0 File Offset: 0x000683C0
		public override object CreateInstance(object[] arguments)
		{
			if ((arguments == null || arguments.Length == 0) && this._type.DefaultConstructor != null)
			{
				return this._type.DefaultConstructor();
			}
			if (!this._type.IsMarkupExtension)
			{
				return base.CreateInstance(arguments);
			}
			Baml6ConstructorInfo baml6ConstructorInfo;
			if (!this._type.Constructors.TryGetValue(arguments.Length, out baml6ConstructorInfo))
			{
				throw new InvalidOperationException(SR.Get("PositionalArgumentsWrongLength"));
			}
			return baml6ConstructorInfo.Constructor(arguments);
		}

		// Token: 0x04001263 RID: 4707
		private WpfKnownType _type;
	}
}
