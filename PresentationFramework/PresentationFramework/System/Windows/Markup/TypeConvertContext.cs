using System;
using System.ComponentModel;

namespace System.Windows.Markup
{
	// Token: 0x02000231 RID: 561
	internal class TypeConvertContext : ITypeDescriptorContext, IServiceProvider
	{
		// Token: 0x0600225F RID: 8799 RVA: 0x00002137 File Offset: 0x00000337
		public void OnComponentChanged()
		{
		}

		// Token: 0x06002260 RID: 8800 RVA: 0x0000B02A File Offset: 0x0000922A
		public bool OnComponentChanging()
		{
			return false;
		}

		// Token: 0x06002261 RID: 8801 RVA: 0x000AADF8 File Offset: 0x000A8FF8
		public virtual object GetService(Type serviceType)
		{
			if (serviceType == typeof(IUriContext))
			{
				return this._parserContext;
			}
			if (serviceType == typeof(string))
			{
				return this._attribStringValue;
			}
			ProvideValueServiceProvider provideValueProvider = this._parserContext.ProvideValueProvider;
			return provideValueProvider.GetService(serviceType);
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06002262 RID: 8802 RVA: 0x0000C238 File Offset: 0x0000A438
		public IContainer Container
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06002263 RID: 8803 RVA: 0x0000C238 File Offset: 0x0000A438
		public object Instance
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06002264 RID: 8804 RVA: 0x0000C238 File Offset: 0x0000A438
		public PropertyDescriptor PropertyDescriptor
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06002265 RID: 8805 RVA: 0x000AAE4A File Offset: 0x000A904A
		public ParserContext ParserContext
		{
			get
			{
				return this._parserContext;
			}
		}

		// Token: 0x06002266 RID: 8806 RVA: 0x000AAE52 File Offset: 0x000A9052
		public TypeConvertContext(ParserContext parserContext)
		{
			this._parserContext = parserContext;
		}

		// Token: 0x040019EE RID: 6638
		private ParserContext _parserContext;

		// Token: 0x040019EF RID: 6639
		private string _attribStringValue;
	}
}
