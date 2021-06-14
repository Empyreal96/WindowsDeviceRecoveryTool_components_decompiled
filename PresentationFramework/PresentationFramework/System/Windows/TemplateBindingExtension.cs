using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Implements a markup extension that supports the binding between the value of a property in a template and the value of some other exposed property on the templated control.</summary>
	// Token: 0x0200011D RID: 285
	[TypeConverter(typeof(TemplateBindingExtensionConverter))]
	[MarkupExtensionReturnType(typeof(object))]
	public class TemplateBindingExtension : MarkupExtension
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.TemplateBindingExtension" /> class.</summary>
		// Token: 0x06000BCB RID: 3019 RVA: 0x0000B03E File Offset: 0x0000923E
		public TemplateBindingExtension()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.TemplateBindingExtension" /> class with the specified dependency property that is the source of the binding.</summary>
		/// <param name="property">The identifier of the property being bound.</param>
		// Token: 0x06000BCC RID: 3020 RVA: 0x0002B2CA File Offset: 0x000294CA
		public TemplateBindingExtension(DependencyProperty property)
		{
			if (property != null)
			{
				this._property = property;
				return;
			}
			throw new ArgumentNullException("property");
		}

		/// <summary>Returns an object that should be set as the value on the target object's property for this markup extension. For <see cref="T:System.Windows.TemplateBindingExtension" />, this is an expression (<see cref="T:System.Windows.TemplateBindingExpression" />) that supports the binding. </summary>
		/// <param name="serviceProvider">An object that can provide services for the markup extension. May be <see langword="null" /> in this implementation.</param>
		/// <returns>The expression that supports the binding.</returns>
		// Token: 0x06000BCD RID: 3021 RVA: 0x0002B2E7 File Offset: 0x000294E7
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			if (this.Property == null)
			{
				throw new InvalidOperationException(SR.Get("MarkupExtensionProperty"));
			}
			return new TemplateBindingExpression(this);
		}

		/// <summary>Gets or sets the property being bound to. </summary>
		/// <returns>Identifier of the dependency property being bound.</returns>
		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06000BCE RID: 3022 RVA: 0x0002B307 File Offset: 0x00029507
		// (set) Token: 0x06000BCF RID: 3023 RVA: 0x0002B30F File Offset: 0x0002950F
		[ConstructorArgument("property")]
		public DependencyProperty Property
		{
			get
			{
				return this._property;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._property = value;
			}
		}

		/// <summary>Gets or sets the converter that interprets between source and target of a binding.</summary>
		/// <returns>The converter implementation. This value defaults to <see langword="null" /> and is typically provided as an optional parameter of the binding.</returns>
		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06000BD0 RID: 3024 RVA: 0x0002B326 File Offset: 0x00029526
		// (set) Token: 0x06000BD1 RID: 3025 RVA: 0x0002B32E File Offset: 0x0002952E
		[DefaultValue(null)]
		public IValueConverter Converter
		{
			get
			{
				return this._converter;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._converter = value;
			}
		}

		/// <summary>Gets or sets the parameter to pass to the converter.</summary>
		/// <returns>The parameter being bound as referenced by the converter implementation. The default value is <see langword="null" />.</returns>
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06000BD2 RID: 3026 RVA: 0x0002B345 File Offset: 0x00029545
		// (set) Token: 0x06000BD3 RID: 3027 RVA: 0x0002B34D File Offset: 0x0002954D
		[DefaultValue(null)]
		public object ConverterParameter
		{
			get
			{
				return this._parameter;
			}
			set
			{
				this._parameter = value;
			}
		}

		// Token: 0x04000AB9 RID: 2745
		private DependencyProperty _property;

		// Token: 0x04000ABA RID: 2746
		private IValueConverter _converter;

		// Token: 0x04000ABB RID: 2747
		private object _parameter;
	}
}
