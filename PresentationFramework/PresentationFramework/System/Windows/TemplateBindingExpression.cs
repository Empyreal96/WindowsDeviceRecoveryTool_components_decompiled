using System;
using System.ComponentModel;

namespace System.Windows
{
	/// <summary>Describes a run-time instance of a <see cref="T:System.Windows.TemplateBindingExtension" />.</summary>
	// Token: 0x0200011B RID: 283
	[TypeConverter(typeof(TemplateBindingExpressionConverter))]
	public class TemplateBindingExpression : Expression
	{
		// Token: 0x06000BC5 RID: 3013 RVA: 0x0002B222 File Offset: 0x00029422
		internal TemplateBindingExpression(TemplateBindingExtension templateBindingExtension)
		{
			this._templateBindingExtension = templateBindingExtension;
		}

		/// <summary>Gets the <see cref="T:System.Windows.TemplateBindingExtension" /> object of this expression instance.</summary>
		/// <returns>The template binding extension of this expression instance.</returns>
		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0002B231 File Offset: 0x00029431
		public TemplateBindingExtension TemplateBindingExtension
		{
			get
			{
				return this._templateBindingExtension;
			}
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0002B239 File Offset: 0x00029439
		internal override object GetValue(DependencyObject d, DependencyProperty dp)
		{
			return dp.GetDefaultValue(d.DependencyObjectType);
		}

		// Token: 0x04000AB8 RID: 2744
		private TemplateBindingExtension _templateBindingExtension;
	}
}
