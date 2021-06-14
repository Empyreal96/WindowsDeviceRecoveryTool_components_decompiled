using System;
using System.Reflection;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Provides an abstract base class for various resource keys. </summary>
	// Token: 0x020000EC RID: 236
	[MarkupExtensionReturnType(typeof(ResourceKey))]
	public abstract class ResourceKey : MarkupExtension
	{
		/// <summary>Gets an assembly object that indicates which assembly's dictionary to look in for the value associated with this key. </summary>
		/// <returns>The retrieved assembly, as a reflection class.</returns>
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000869 RID: 2153
		public abstract Assembly Assembly { get; }

		/// <summary>Returns this <see cref="T:System.Windows.ResourceKey" />. Instances of this class are typically used as a key in a dictionary. </summary>
		/// <param name="serviceProvider">A service implementation that provides the desired value.</param>
		/// <returns>Calling this method always returns the instance itself.</returns>
		// Token: 0x0600086A RID: 2154 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}
	}
}
