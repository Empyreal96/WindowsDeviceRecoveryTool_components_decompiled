using System;
using System.Windows;
using System.Windows.Data;

namespace MS.Internal.Data
{
	// Token: 0x02000708 RID: 1800
	internal class BindingExpressionUncommonField : UncommonField<BindingExpression>
	{
		// Token: 0x06007371 RID: 29553 RVA: 0x0021148D File Offset: 0x0020F68D
		internal new void SetValue(DependencyObject instance, BindingExpression bindingExpr)
		{
			base.SetValue(instance, bindingExpr);
			bindingExpr.Attach(instance);
		}

		// Token: 0x06007372 RID: 29554 RVA: 0x002114A0 File Offset: 0x0020F6A0
		internal new void ClearValue(DependencyObject instance)
		{
			BindingExpression value = base.GetValue(instance);
			if (value != null)
			{
				value.Detach();
			}
			base.ClearValue(instance);
		}
	}
}
