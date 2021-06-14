using System;

namespace System.Windows.Data
{
	/// <summary>Represents an attribute that allows the author of a value converter to specify the data types involved in the implementation of the converter.</summary>
	// Token: 0x020001BC RID: 444
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public sealed class ValueConversionAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Data.ValueConversionAttribute" /> class with the specified source type and target type.</summary>
		/// <param name="sourceType">The type this converter converts.</param>
		/// <param name="targetType">The type this converter converts to.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="sourceType" /> parameter cannot be <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="targetType" /> parameter cannot be <see langword="null" />.</exception>
		// Token: 0x06001CAE RID: 7342 RVA: 0x00086796 File Offset: 0x00084996
		public ValueConversionAttribute(Type sourceType, Type targetType)
		{
			if (sourceType == null)
			{
				throw new ArgumentNullException("sourceType");
			}
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			this._sourceType = sourceType;
			this._targetType = targetType;
		}

		/// <summary>Gets the type this converter converts.</summary>
		/// <returns>The type this converter converts.</returns>
		// Token: 0x170006BE RID: 1726
		// (get) Token: 0x06001CAF RID: 7343 RVA: 0x000867D4 File Offset: 0x000849D4
		public Type SourceType
		{
			get
			{
				return this._sourceType;
			}
		}

		/// <summary>Gets the type this converter converts to.</summary>
		/// <returns>The type this converter converts to.</returns>
		// Token: 0x170006BF RID: 1727
		// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x000867DC File Offset: 0x000849DC
		public Type TargetType
		{
			get
			{
				return this._targetType;
			}
		}

		/// <summary>Gets and sets the type of the optional value converter parameter object.</summary>
		/// <returns>The type of the optional value converter parameter object.</returns>
		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x000867E4 File Offset: 0x000849E4
		// (set) Token: 0x06001CB2 RID: 7346 RVA: 0x000867EC File Offset: 0x000849EC
		public Type ParameterType
		{
			get
			{
				return this._parameterType;
			}
			set
			{
				this._parameterType = value;
			}
		}

		/// <summary>Gets the unique identifier of this <see cref="T:System.Windows.Data.ValueConversionAttribute" /> instance.</summary>
		/// <returns>The unique identifier of this <see cref="T:System.Windows.Data.ValueConversionAttribute" /> instance.</returns>
		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x0001B7E3 File Offset: 0x000199E3
		public override object TypeId
		{
			get
			{
				return this;
			}
		}

		/// <summary>Returns the hash code for this instance of <see cref="T:System.Windows.Data.ValueConversionAttribute" />.</summary>
		/// <returns>The hash code for this instance of <see cref="T:System.Windows.Data.ValueConversionAttribute" />.</returns>
		// Token: 0x06001CB4 RID: 7348 RVA: 0x000867F5 File Offset: 0x000849F5
		public override int GetHashCode()
		{
			return this._sourceType.GetHashCode() + this._targetType.GetHashCode();
		}

		// Token: 0x040013EA RID: 5098
		private Type _sourceType;

		// Token: 0x040013EB RID: 5099
		private Type _targetType;

		// Token: 0x040013EC RID: 5100
		private Type _parameterType;
	}
}
