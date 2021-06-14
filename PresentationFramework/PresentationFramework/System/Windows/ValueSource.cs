using System;

namespace System.Windows
{
	/// <summary>Reports the information returned from <see cref="M:System.Windows.DependencyPropertyHelper.GetValueSource(System.Windows.DependencyObject,System.Windows.DependencyProperty)" />.</summary>
	// Token: 0x020000AF RID: 175
	public struct ValueSource
	{
		// Token: 0x060003AF RID: 943 RVA: 0x0000A87B File Offset: 0x00008A7B
		internal ValueSource(BaseValueSourceInternal source, bool isExpression, bool isAnimated, bool isCoerced, bool isCurrent)
		{
			this._baseValueSource = (BaseValueSource)source;
			this._isExpression = isExpression;
			this._isAnimated = isAnimated;
			this._isCoerced = isCoerced;
			this._isCurrent = isCurrent;
		}

		/// <summary>Gets a value of the <see cref="T:System.Windows.BaseValueSource" /> enumeration, which reports the source that provided the dependency property system with a value.</summary>
		/// <returns>A value of the enumeration.</returns>
		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0000A8A2 File Offset: 0x00008AA2
		public BaseValueSource BaseValueSource
		{
			get
			{
				return this._baseValueSource;
			}
		}

		/// <summary>Gets a value that declares whether this value resulted from an evaluated expression. This might be a <see cref="T:System.Windows.Data.BindingExpression" /> supporting a binding, or an internal expression such as those that support the DynamicResource Markup Extension.</summary>
		/// <returns>
		///     <see langword="true" /> if the value came from an evaluated expression; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x0000A8AA File Offset: 0x00008AAA
		public bool IsExpression
		{
			get
			{
				return this._isExpression;
			}
		}

		/// <summary>Gets a value that declares whether the property is being animated.</summary>
		/// <returns>
		///     <see langword="true" /> if the property is animated; otherwise, <see langword="false" />.</returns>
		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060003B2 RID: 946 RVA: 0x0000A8B2 File Offset: 0x00008AB2
		public bool IsAnimated
		{
			get
			{
				return this._isAnimated;
			}
		}

		/// <summary>Gets a value that declares whether this value resulted from a <see cref="T:System.Windows.CoerceValueCallback" /> implementation applied to a dependency property.</summary>
		/// <returns>
		///     <see langword="true" /> if the value resulted from a <see cref="T:System.Windows.CoerceValueCallback" /> implementation applied to a dependency property; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060003B3 RID: 947 RVA: 0x0000A8BA File Offset: 0x00008ABA
		public bool IsCoerced
		{
			get
			{
				return this._isCoerced;
			}
		}

		/// <summary>Gets whether the value was set by the <see cref="M:System.Windows.DependencyObject.SetCurrentValue(System.Windows.DependencyProperty,System.Object)" /> method.</summary>
		/// <returns>
		///     <see langword="true" /> if the value was set by the <see cref="M:System.Windows.DependencyObject.SetCurrentValue(System.Windows.DependencyProperty,System.Object)" /> method; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060003B4 RID: 948 RVA: 0x0000A8C2 File Offset: 0x00008AC2
		public bool IsCurrent
		{
			get
			{
				return this._isCurrent;
			}
		}

		/// <summary>Returns the hash code for this <see cref="T:System.Windows.ValueSource" />.</summary>
		/// <returns>A 32-bit unsigned integer hash code.</returns>
		// Token: 0x060003B5 RID: 949 RVA: 0x0000A8CA File Offset: 0x00008ACA
		public override int GetHashCode()
		{
			return this._baseValueSource.GetHashCode();
		}

		/// <summary>Returns a value indicating whether this <see cref="T:System.Windows.ValueSource" /> is equal to a specified object.</summary>
		/// <param name="o">The object to compare with this <see cref="T:System.Windows.ValueSource" />.</param>
		/// <returns>
		///     <see langword="true" /> if the provided object is equivalent to the current <see cref="T:System.Windows.ValueSource" />; otherwise, <see langword="false" />.</returns>
		// Token: 0x060003B6 RID: 950 RVA: 0x0000A8E0 File Offset: 0x00008AE0
		public override bool Equals(object o)
		{
			if (o is ValueSource)
			{
				ValueSource valueSource = (ValueSource)o;
				return this._baseValueSource == valueSource._baseValueSource && this._isExpression == valueSource._isExpression && this._isAnimated == valueSource._isAnimated && this._isCoerced == valueSource._isCoerced;
			}
			return false;
		}

		/// <summary>Determines whether two <see cref="T:System.Windows.ValueSource" /> instances have the same value.</summary>
		/// <param name="vs1">The first <see cref="T:System.Windows.ValueSource" /> to compare.</param>
		/// <param name="vs2">The second <see cref="T:System.Windows.ValueSource" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.ValueSource" /> instances are equivalent; otherwise, <see langword="false" />.</returns>
		// Token: 0x060003B7 RID: 951 RVA: 0x0000A938 File Offset: 0x00008B38
		public static bool operator ==(ValueSource vs1, ValueSource vs2)
		{
			return vs1.Equals(vs2);
		}

		/// <summary>Determines whether two <see cref="T:System.Windows.ValueSource" /> instances do not have the same value.</summary>
		/// <param name="vs1">The first <see cref="T:System.Windows.ValueSource" /> to compare.</param>
		/// <param name="vs2">The second <see cref="T:System.Windows.ValueSource" /> to compare.</param>
		/// <returns>
		///     <see langword="true" /> if the two <see cref="T:System.Windows.ValueSource" /> instances are not equivalent; otherwise, <see langword="false" />.</returns>
		// Token: 0x060003B8 RID: 952 RVA: 0x0000A94D File Offset: 0x00008B4D
		public static bool operator !=(ValueSource vs1, ValueSource vs2)
		{
			return !vs1.Equals(vs2);
		}

		// Token: 0x04000607 RID: 1543
		private BaseValueSource _baseValueSource;

		// Token: 0x04000608 RID: 1544
		private bool _isExpression;

		// Token: 0x04000609 RID: 1545
		private bool _isAnimated;

		// Token: 0x0400060A RID: 1546
		private bool _isCoerced;

		// Token: 0x0400060B RID: 1547
		private bool _isCurrent;
	}
}
