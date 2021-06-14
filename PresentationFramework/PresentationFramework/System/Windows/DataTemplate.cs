using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Markup;

namespace System.Windows
{
	/// <summary>Describes the visual structure of a data object.</summary>
	// Token: 0x020000A9 RID: 169
	[DictionaryKeyProperty("DataTemplateKey")]
	public class DataTemplate : FrameworkTemplate
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.DataTemplate" /> class.</summary>
		// Token: 0x06000383 RID: 899 RVA: 0x0000A1F1 File Offset: 0x000083F1
		public DataTemplate()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.DataTemplate" /> class with the specified <see cref="P:System.Windows.DataTemplate.DataType" /> property.</summary>
		/// <param name="dataType">If the template is intended for object data, this is the Type name of the data object. </param>
		// Token: 0x06000384 RID: 900 RVA: 0x0000A1FC File Offset: 0x000083FC
		public DataTemplate(object dataType)
		{
			Exception ex = TemplateKey.ValidateDataType(dataType, "dataType");
			if (ex != null)
			{
				throw ex;
			}
			this._dataType = dataType;
		}

		/// <summary>Gets or sets the type for which this <see cref="T:System.Windows.DataTemplate" /> is intended. </summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000385 RID: 901 RVA: 0x0000A227 File Offset: 0x00008427
		// (set) Token: 0x06000386 RID: 902 RVA: 0x0000A230 File Offset: 0x00008430
		[DefaultValue(null)]
		[Ambient]
		public object DataType
		{
			get
			{
				return this._dataType;
			}
			set
			{
				Exception ex = TemplateKey.ValidateDataType(value, "value");
				if (ex != null)
				{
					throw ex;
				}
				base.CheckSealed();
				this._dataType = value;
			}
		}

		/// <summary>Gets a collection of triggers that apply property values or perform actions based on one or more conditions.</summary>
		/// <returns>A collection of trigger objects. The default value is <see langword="null" />.</returns>
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000387 RID: 903 RVA: 0x0000A25B File Offset: 0x0000845B
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[DependsOn("VisualTree")]
		[DependsOn("Template")]
		public TriggerCollection Triggers
		{
			get
			{
				if (this._triggers == null)
				{
					this._triggers = new TriggerCollection();
					if (base.IsSealed)
					{
						this._triggers.Seal();
					}
				}
				return this._triggers;
			}
		}

		/// <summary>Gets the default key of the <see cref="T:System.Windows.DataTemplate" />. </summary>
		/// <returns>The default key of the <see cref="T:System.Windows.DataTemplate" />.</returns>
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000388 RID: 904 RVA: 0x0000A289 File Offset: 0x00008489
		public object DataTemplateKey
		{
			get
			{
				if (this.DataType == null)
				{
					return null;
				}
				return new DataTemplateKey(this.DataType);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000389 RID: 905 RVA: 0x0000A2A0 File Offset: 0x000084A0
		internal override Type TargetTypeInternal
		{
			get
			{
				return DataTemplate.DefaultTargetType;
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000A2A7 File Offset: 0x000084A7
		internal override void SetTargetTypeInternal(Type targetType)
		{
			throw new InvalidOperationException(SR.Get("TemplateNotTargetType"));
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600038B RID: 907 RVA: 0x0000A2B8 File Offset: 0x000084B8
		internal override object DataTypeInternal
		{
			get
			{
				return this.DataType;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600038C RID: 908 RVA: 0x0000A2C0 File Offset: 0x000084C0
		internal override TriggerCollection TriggersInternal
		{
			get
			{
				return this.Triggers;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600038D RID: 909 RVA: 0x0000A2C8 File Offset: 0x000084C8
		internal static Type DefaultTargetType
		{
			get
			{
				return typeof(ContentPresenter);
			}
		}

		/// <summary>Checks the templated parent against a set of rules.</summary>
		/// <param name="templatedParent">The element this template is applied to.</param>
		// Token: 0x0600038E RID: 910 RVA: 0x0000A2D4 File Offset: 0x000084D4
		protected override void ValidateTemplatedParent(FrameworkElement templatedParent)
		{
			if (templatedParent == null)
			{
				throw new ArgumentNullException("templatedParent");
			}
			if (!(templatedParent is ContentPresenter))
			{
				throw new ArgumentException(SR.Get("TemplateTargetTypeMismatch", new object[]
				{
					"ContentPresenter",
					templatedParent.GetType().Name
				}));
			}
		}

		// Token: 0x040005EE RID: 1518
		private object _dataType;

		// Token: 0x040005EF RID: 1519
		private TriggerCollection _triggers;
	}
}
