using System;
using System.ComponentModel;
using System.Windows.Markup;
using MS.Internal;

namespace System.Windows
{
	/// <summary>Implements a markup extension that supports dynamic resource references made from XAML. </summary>
	// Token: 0x020000B6 RID: 182
	[TypeConverter(typeof(DynamicResourceExtensionConverter))]
	[MarkupExtensionReturnType(typeof(object))]
	public class DynamicResourceExtension : MarkupExtension
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.DynamicResourceExtension" /> class.</summary>
		// Token: 0x060003D4 RID: 980 RVA: 0x0000B03E File Offset: 0x0000923E
		public DynamicResourceExtension()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.DynamicResourceExtension" /> class, with the provided initial key.</summary>
		/// <param name="resourceKey">The key of the resource that this markup extension references.</param>
		// Token: 0x060003D5 RID: 981 RVA: 0x0000B046 File Offset: 0x00009246
		public DynamicResourceExtension(object resourceKey)
		{
			if (resourceKey == null)
			{
				throw new ArgumentNullException("resourceKey");
			}
			this._resourceKey = resourceKey;
		}

		/// <summary>Returns an object that should be set on the property where this extension is applied. For <see cref="T:System.Windows.DynamicResourceExtension" />, this is the object found in a resource dictionary in the current parent chain that is keyed by the <see cref="P:System.Windows.DynamicResourceExtension.ResourceKey" />.</summary>
		/// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
		/// <returns>The object to set on the property where the extension is applied. Rather than the actual value, this will be an expression that will be evaluated at a later time.</returns>
		/// <exception cref="T:System.InvalidOperationException">Attempted to provide a value for an extension that did not provide a <paramref name="resourceKey" />.</exception>
		// Token: 0x060003D6 RID: 982 RVA: 0x0000B064 File Offset: 0x00009264
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this.ResourceKey == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionResourceKey"));
			}
			if (serviceProvider != null)
			{
				DependencyObject dependencyObject;
				DependencyProperty dependencyProperty;
				Helper.CheckCanReceiveMarkupExtension(this, serviceProvider, out dependencyObject, out dependencyProperty);
			}
			return new ResourceReferenceExpression(this.ResourceKey);
		}

		/// <summary>Gets or sets the key specified by this dynamic resource reference. The key is used to lookup a resource in resource dictionaries, by means of an intermediate expression. </summary>
		/// <returns>The resource key that this dynamic resource reference specifies.</returns>
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000B0A2 File Offset: 0x000092A2
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0000B0AA File Offset: 0x000092AA
		[ConstructorArgument("resourceKey")]
		public object ResourceKey
		{
			get
			{
				return this._resourceKey;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._resourceKey = value;
			}
		}

		// Token: 0x04000615 RID: 1557
		private object _resourceKey;
	}
}
