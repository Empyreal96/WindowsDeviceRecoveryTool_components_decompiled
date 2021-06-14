using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows
{
	/// <summary>Converts to and from the <see cref="T:System.Nullable`1" /> type (using the <see cref="T:System.Boolean" /> type constraint on the generic). </summary>
	// Token: 0x020000DF RID: 223
	public class NullableBoolConverter : NullableConverter
	{
		/// <summary> Initializes a new instance of the <see cref="T:System.Windows.NullableBoolConverter" />  class. </summary>
		// Token: 0x0600079A RID: 1946 RVA: 0x00017BCB File Offset: 0x00015DCB
		public NullableBoolConverter() : base(typeof(bool?))
		{
		}

		/// <summary>Returns whether this object supports a standard set of values that can be picked from a list. </summary>
		/// <param name="context">Provides contextual information about a component, such as its container and property descriptor.</param>
		/// <returns>This implementation always returns <see langword="true" />.</returns>
		// Token: 0x0600079B RID: 1947 RVA: 0x00016748 File Offset: 0x00014948
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>Returns whether the collection of standard values returned from <see cref="M:System.Windows.NullableBoolConverter.GetStandardValues(System.ComponentModel.ITypeDescriptorContext)" /> is an exclusive list. </summary>
		/// <param name="context">Provides contextual information about a component, such as its container and property descriptor.</param>
		/// <returns>This implementation always returns <see langword="true" />.</returns>
		// Token: 0x0600079C RID: 1948 RVA: 0x00016748 File Offset: 0x00014948
		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		/// <summary>Returns a collection of standard values for the data type that this type converter is designed for. </summary>
		/// <param name="context">Provides contextual information about a component, such as its container and property descriptor. </param>
		/// <returns>A collection that holds a standard set of valid values. For this implementation, those values are <see langword="true" />, <see langword="false" />, and <see langword="null" />.</returns>
		// Token: 0x0600079D RID: 1949 RVA: 0x00017BE0 File Offset: 0x00015DE0
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			if (NullableBoolConverter._standardValues == null)
			{
				NullableBoolConverter._standardValues = new TypeConverter.StandardValuesCollection(new ArrayList(3)
				{
					true,
					false,
					null
				}.ToArray());
			}
			return NullableBoolConverter._standardValues;
		}

		// Token: 0x04000764 RID: 1892
		[ThreadStatic]
		private static TypeConverter.StandardValuesCollection _standardValues;
	}
}
