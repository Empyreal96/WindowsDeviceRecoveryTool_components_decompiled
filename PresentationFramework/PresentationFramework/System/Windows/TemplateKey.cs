using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>When used as a resource key for a data template, allows the data template to participate in the lookup process.</summary>
	// Token: 0x02000122 RID: 290
	[TypeConverter(typeof(TemplateKeyConverter))]
	public abstract class TemplateKey : ResourceKey, ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.TemplateKey" /> class with the specified template type. This constructor is protected.</summary>
		/// <param name="templateType">A <see cref="T:System.Windows.TemplateKey.TemplateType" /> value that specifies the type of this template.</param>
		// Token: 0x06000C0F RID: 3087 RVA: 0x0002CFCC File Offset: 0x0002B1CC
		protected TemplateKey(TemplateKey.TemplateType templateType)
		{
			this._dataType = null;
			this._templateType = templateType;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.TemplateKey" /> class with the specified parameters. This constructor is protected.</summary>
		/// <param name="templateType">A <see cref="T:System.Windows.TemplateKey.TemplateType" /> value that specifies the type of this template.</param>
		/// <param name="dataType">The type for which this template is designed.</param>
		// Token: 0x06000C10 RID: 3088 RVA: 0x0002CFE4 File Offset: 0x0002B1E4
		protected TemplateKey(TemplateKey.TemplateType templateType, object dataType)
		{
			Exception ex = TemplateKey.ValidateDataType(dataType, "dataType");
			if (ex != null)
			{
				throw ex;
			}
			this._dataType = dataType;
			this._templateType = templateType;
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06000C11 RID: 3089 RVA: 0x0002D016 File Offset: 0x0002B216
		void ISupportInitialize.BeginInit()
		{
			this._initializing = true;
		}

		/// <summary>This member supports the Windows Presentation Foundation (WPF) infrastructure and is not intended to be used directly from your code.</summary>
		// Token: 0x06000C12 RID: 3090 RVA: 0x0002D01F File Offset: 0x0002B21F
		void ISupportInitialize.EndInit()
		{
			if (this._dataType == null)
			{
				throw new InvalidOperationException(SR.Get("PropertyMustHaveValue", new object[]
				{
					"DataType",
					base.GetType().Name
				}));
			}
			this._initializing = false;
		}

		/// <summary>Gets or sets the type for which the template is designed. </summary>
		/// <returns>The <see cref="T:System.Type" /> that specifies the type of object that the template is used to display, or a string that specifies the XML tag name for the XML data that the template is used to display.</returns>
		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000C13 RID: 3091 RVA: 0x0002D05C File Offset: 0x0002B25C
		// (set) Token: 0x06000C14 RID: 3092 RVA: 0x0002D064 File Offset: 0x0002B264
		public object DataType
		{
			get
			{
				return this._dataType;
			}
			set
			{
				if (!this._initializing)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsInitializeOnly", new object[]
					{
						"DataType",
						base.GetType().Name
					}));
				}
				if (this._dataType != null && value != this._dataType)
				{
					throw new InvalidOperationException(SR.Get("PropertyIsImmutable", new object[]
					{
						"DataType",
						base.GetType().Name
					}));
				}
				Exception ex = TemplateKey.ValidateDataType(value, "value");
				if (ex != null)
				{
					throw ex;
				}
				this._dataType = value;
			}
		}

		/// <summary>Returns the hash code for this instance of <see cref="T:System.Windows.TemplateKey" />.</summary>
		/// <returns>The hash code for this instance of <see cref="T:System.Windows.TemplateKey" />.</returns>
		// Token: 0x06000C15 RID: 3093 RVA: 0x0002D0FC File Offset: 0x0002B2FC
		public override int GetHashCode()
		{
			int num = (int)this._templateType;
			if (this._dataType != null)
			{
				num += this._dataType.GetHashCode();
			}
			return num;
		}

		/// <summary>Returns a value that indicates whether the given instance is identical to this instance of <see cref="T:System.Windows.TemplateKey" />.</summary>
		/// <param name="o">The object to compare for equality.</param>
		/// <returns>
		///     <see langword="true" /> if the two instances are identical; otherwise, <see langword="false" />.</returns>
		// Token: 0x06000C16 RID: 3094 RVA: 0x0002D128 File Offset: 0x0002B328
		public override bool Equals(object o)
		{
			TemplateKey templateKey = o as TemplateKey;
			return templateKey != null && this._templateType == templateKey._templateType && object.Equals(this._dataType, templateKey._dataType);
		}

		/// <summary>Returns a string representation of this <see cref="T:System.Windows.TemplateKey" />.</summary>
		/// <returns>A string representation of this <see cref="T:System.Windows.TemplateKey" />.</returns>
		// Token: 0x06000C17 RID: 3095 RVA: 0x0002D164 File Offset: 0x0002B364
		public override string ToString()
		{
			Type type = this.DataType as Type;
			if (this.DataType == null)
			{
				return string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}(null)", new object[]
				{
					base.GetType().Name
				});
			}
			return string.Format(TypeConverterHelper.InvariantEnglishUS, "{0}({1})", new object[]
			{
				base.GetType().Name,
				this.DataType
			});
		}

		/// <summary>Gets or sets the assembly that contains the template definition.</summary>
		/// <returns>The assembly in which the template is defined.</returns>
		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000C18 RID: 3096 RVA: 0x0002D1D8 File Offset: 0x0002B3D8
		public override Assembly Assembly
		{
			get
			{
				Type type = this._dataType as Type;
				if (type != null)
				{
					return type.Assembly;
				}
				return null;
			}
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0002D204 File Offset: 0x0002B404
		internal static Exception ValidateDataType(object dataType, string argName)
		{
			Exception result = null;
			if (dataType == null)
			{
				result = new ArgumentNullException(argName);
			}
			else if (!(dataType is Type) && !(dataType is string))
			{
				result = new ArgumentException(SR.Get("MustBeTypeOrString", new object[]
				{
					dataType.GetType().Name
				}), argName);
			}
			else if (typeof(object).Equals(dataType))
			{
				result = new ArgumentException(SR.Get("DataTypeCannotBeObject"), argName);
			}
			return result;
		}

		// Token: 0x04000ACC RID: 2764
		private object _dataType;

		// Token: 0x04000ACD RID: 2765
		private TemplateKey.TemplateType _templateType;

		// Token: 0x04000ACE RID: 2766
		private bool _initializing;

		/// <summary>Describes the different types of templates that use <see cref="T:System.Windows.TemplateKey" />.</summary>
		// Token: 0x02000830 RID: 2096
		protected enum TemplateType
		{
			/// <summary>A type that is a <see cref="T:System.Windows.DataTemplate" />.</summary>
			// Token: 0x04003CB3 RID: 15539
			DataTemplate,
			/// <summary>A type that is a <see langword="TableTemplate" />. This is obsolete.</summary>
			// Token: 0x04003CB4 RID: 15540
			TableTemplate
		}
	}
}
