using System;
using System.Windows.Media;

namespace System.Windows.Markup
{
	// Token: 0x02000227 RID: 551
	internal class ProvideValueServiceProvider : IServiceProvider, IProvideValueTarget, IXamlTypeResolver, IUriContext, IFreezeFreezables
	{
		// Token: 0x0600220F RID: 8719 RVA: 0x000AA1EB File Offset: 0x000A83EB
		internal ProvideValueServiceProvider(ParserContext context)
		{
			this._context = context;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x0000326D File Offset: 0x0000146D
		internal ProvideValueServiceProvider()
		{
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x000AA1FA File Offset: 0x000A83FA
		internal void SetData(object targetObject, object targetProperty)
		{
			this._targetObject = targetObject;
			this._targetProperty = targetProperty;
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x000AA20C File Offset: 0x000A840C
		internal void ClearData()
		{
			this._targetObject = (this._targetProperty = null);
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x000AA229 File Offset: 0x000A8429
		Type IXamlTypeResolver.Resolve(string qualifiedTypeName)
		{
			return this._context.XamlTypeMapper.GetTypeFromBaseString(qualifiedTypeName, this._context, true);
		}

		// Token: 0x17000828 RID: 2088
		// (get) Token: 0x06002214 RID: 8724 RVA: 0x000AA243 File Offset: 0x000A8443
		object IProvideValueTarget.TargetObject
		{
			get
			{
				return this._targetObject;
			}
		}

		// Token: 0x17000829 RID: 2089
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x000AA24B File Offset: 0x000A844B
		object IProvideValueTarget.TargetProperty
		{
			get
			{
				return this._targetProperty;
			}
		}

		// Token: 0x1700082A RID: 2090
		// (get) Token: 0x06002216 RID: 8726 RVA: 0x000AA253 File Offset: 0x000A8453
		// (set) Token: 0x06002217 RID: 8727 RVA: 0x000AA260 File Offset: 0x000A8460
		Uri IUriContext.BaseUri
		{
			get
			{
				return this._context.BaseUri;
			}
			set
			{
				throw new NotSupportedException(SR.Get("ParserProvideValueCantSetUri"));
			}
		}

		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002218 RID: 8728 RVA: 0x000AA271 File Offset: 0x000A8471
		bool IFreezeFreezables.FreezeFreezables
		{
			get
			{
				return this._context.FreezeFreezables;
			}
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000AA27E File Offset: 0x000A847E
		bool IFreezeFreezables.TryFreeze(string value, Freezable freezable)
		{
			return this._context.TryCacheFreezable(value, freezable);
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x000AA28D File Offset: 0x000A848D
		Freezable IFreezeFreezables.TryGetFreezable(string value)
		{
			return this._context.TryGetFreezable(value);
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x000AA29C File Offset: 0x000A849C
		public object GetService(Type service)
		{
			if (service == typeof(IProvideValueTarget))
			{
				return this;
			}
			if (this._context != null)
			{
				if (service == typeof(IXamlTypeResolver))
				{
					return this;
				}
				if (service == typeof(IUriContext))
				{
					return this;
				}
				if (service == typeof(IFreezeFreezables))
				{
					return this;
				}
			}
			return null;
		}

		// Token: 0x040019CC RID: 6604
		private ParserContext _context;

		// Token: 0x040019CD RID: 6605
		private object _targetObject;

		// Token: 0x040019CE RID: 6606
		private object _targetProperty;
	}
}
