using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace MS.Internal.Markup
{
	// Token: 0x0200065A RID: 1626
	internal class StaticExtension : StaticExtension
	{
		// Token: 0x06006C04 RID: 27652 RVA: 0x001F17EE File Offset: 0x001EF9EE
		public StaticExtension()
		{
		}

		// Token: 0x06006C05 RID: 27653 RVA: 0x001F17F6 File Offset: 0x001EF9F6
		public StaticExtension(string member) : base(member)
		{
		}

		// Token: 0x06006C06 RID: 27654 RVA: 0x001F1800 File Offset: 0x001EFA00
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (base.Member == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionStaticMember"));
			}
			object obj;
			if (base.MemberType != null)
			{
				obj = SystemResourceKey.GetSystemResourceKey(base.MemberType.Name + "." + base.Member);
				if (obj != null)
				{
					return obj;
				}
			}
			else
			{
				obj = SystemResourceKey.GetSystemResourceKey(base.Member);
				if (obj != null)
				{
					return obj;
				}
				int num = base.Member.IndexOf('.');
				if (num < 0)
				{
					throw new ArgumentException(SR.Get("MarkupExtensionBadStatic", new object[]
					{
						base.Member
					}));
				}
				string text = base.Member.Substring(0, num);
				if (text == string.Empty)
				{
					throw new ArgumentException(SR.Get("MarkupExtensionBadStatic", new object[]
					{
						base.Member
					}));
				}
				if (serviceProvider == null)
				{
					throw new ArgumentNullException("serviceProvider");
				}
				IXamlTypeResolver xamlTypeResolver = serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;
				if (xamlTypeResolver == null)
				{
					throw new ArgumentException(SR.Get("MarkupExtensionNoContext", new object[]
					{
						base.GetType().Name,
						"IXamlTypeResolver"
					}));
				}
				base.MemberType = xamlTypeResolver.Resolve(text);
				base.Member = base.Member.Substring(num + 1, base.Member.Length - num - 1);
			}
			obj = CommandConverter.GetKnownControlCommand(base.MemberType, base.Member);
			if (obj != null)
			{
				return obj;
			}
			return base.ProvideValue(serviceProvider);
		}
	}
}
