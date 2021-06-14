using System;

namespace System.Windows.Data
{
	/// <summary>Describes the status of a binding.</summary>
	// Token: 0x02000198 RID: 408
	public enum BindingStatus
	{
		/// <summary>The binding has not yet been attached to its target property.</summary>
		// Token: 0x040012D7 RID: 4823
		Unattached,
		/// <summary>The binding has not been activated.</summary>
		// Token: 0x040012D8 RID: 4824
		Inactive,
		/// <summary>The binding has been successfully activated. This means that the binding has been attached to its binding target (target) property and has located the binding source (source), resolved the <see langword="Path" /> and/or <see langword="XPath" />, and begun transferring values.</summary>
		// Token: 0x040012D9 RID: 4825
		Active,
		/// <summary>The binding has been detached from its target property.</summary>
		// Token: 0x040012DA RID: 4826
		Detached,
		/// <summary>The binding is waiting for an asynchronous operation to complete.</summary>
		// Token: 0x040012DB RID: 4827
		AsyncRequestPending,
		/// <summary>The binding was unable to resolve the source path.</summary>
		// Token: 0x040012DC RID: 4828
		PathError,
		/// <summary>The binding could not successfully return a source value to update the target value. For more information, see the remarks on <see cref="P:System.Windows.Data.BindingBase.FallbackValue" />.</summary>
		// Token: 0x040012DD RID: 4829
		UpdateTargetError,
		/// <summary>The binding was unable to send the value to the source property.</summary>
		// Token: 0x040012DE RID: 4830
		UpdateSourceError
	}
}
