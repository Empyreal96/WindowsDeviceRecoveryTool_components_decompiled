using System;

namespace System.Windows
{
	/// <summary>Specifies that an attached property has a browsable scope that extends to child elements in the logical tree.</summary>
	// Token: 0x0200009F RID: 159
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class AttachedPropertyBrowsableForChildrenAttribute : AttachedPropertyBrowsableAttribute
	{
		/// <summary>Gets or sets a value that declares whether to use the deep mode for detection of parent elements on the attached property where this  .NET Framework attribute is applied.</summary>
		/// <returns>
		///     <see langword="true" /> if the attached property is browsable for all child elements in the logical tree of the parent element that owns the attached property. <see langword="false" /> if the attached property is only browsable for immediate child elements of a parent element that owns the attached property. The default is <see langword="false" />.</returns>
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060002FE RID: 766 RVA: 0x00008490 File Offset: 0x00006690
		// (set) Token: 0x060002FF RID: 767 RVA: 0x00008498 File Offset: 0x00006698
		public bool IncludeDescendants
		{
			get
			{
				return this._includeDescendants;
			}
			set
			{
				this._includeDescendants = value;
			}
		}

		/// <summary>Determines whether the current <see cref="T:System.Windows.AttachedPropertyBrowsableForChildrenAttribute" /> .NET Framework attribute is equal to a specified object.</summary>
		/// <param name="obj">The <see cref="T:System.Windows.AttachedPropertyBrowsableForChildrenAttribute" /> to compare to the current <see cref="T:System.Windows.AttachedPropertyBrowsableForChildrenAttribute" />.</param>
		/// <returns>
		///     <see langword="true" /> if the specified <see cref="T:System.Windows.AttachedPropertyBrowsableForChildrenAttribute" /> is equal to the current <see cref="T:System.Windows.AttachedPropertyBrowsableForChildrenAttribute" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000300 RID: 768 RVA: 0x000084A4 File Offset: 0x000066A4
		public override bool Equals(object obj)
		{
			AttachedPropertyBrowsableForChildrenAttribute attachedPropertyBrowsableForChildrenAttribute = obj as AttachedPropertyBrowsableForChildrenAttribute;
			return attachedPropertyBrowsableForChildrenAttribute != null && this._includeDescendants == attachedPropertyBrowsableForChildrenAttribute._includeDescendants;
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Windows.AttachedPropertyBrowsableForChildrenAttribute" /> .NET Framework attribute.</summary>
		/// <returns>An unsigned 32-bit integer value.</returns>
		// Token: 0x06000301 RID: 769 RVA: 0x000084CB File Offset: 0x000066CB
		public override int GetHashCode()
		{
			return this._includeDescendants.GetHashCode();
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000084D8 File Offset: 0x000066D8
		internal override bool IsBrowsable(DependencyObject d, DependencyProperty dp)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			if (dp == null)
			{
				throw new ArgumentNullException("dp");
			}
			DependencyObject dependencyObject = d;
			Type ownerType = dp.OwnerType;
			for (;;)
			{
				dependencyObject = FrameworkElement.GetFrameworkParent(dependencyObject);
				if (dependencyObject != null && ownerType.IsInstanceOfType(dependencyObject))
				{
					break;
				}
				if (!this._includeDescendants || dependencyObject == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040005C7 RID: 1479
		private bool _includeDescendants;
	}
}
