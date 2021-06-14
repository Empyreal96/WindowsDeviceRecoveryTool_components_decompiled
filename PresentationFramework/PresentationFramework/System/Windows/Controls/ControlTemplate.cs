using System;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace System.Windows.Controls
{
	/// <summary>Specifies the visual structure and behavioral aspects of a <see cref="T:System.Windows.Controls.Control" /> that can be shared across multiple instances of the control.</summary>
	// Token: 0x02000469 RID: 1129
	[Localizability(LocalizationCategory.None, Readability = Readability.Unreadable)]
	[DictionaryKeyProperty("TargetType")]
	public class ControlTemplate : FrameworkTemplate
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ControlTemplate" /> class.</summary>
		// Token: 0x060041DA RID: 16858 RVA: 0x0000A1F1 File Offset: 0x000083F1
		public ControlTemplate()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.ControlTemplate" /> class with the specified target type.</summary>
		/// <param name="targetType">The type this template is intended for.</param>
		// Token: 0x060041DB RID: 16859 RVA: 0x0012DEB4 File Offset: 0x0012C0B4
		public ControlTemplate(Type targetType)
		{
			this.ValidateTargetType(targetType, "targetType");
			this._targetType = targetType;
		}

		/// <summary>Checks the templated parent against a set of rules.</summary>
		/// <param name="templatedParent">The element this template is applied to.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="templatedParent" /> must not be <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">The <see cref="P:System.Windows.Controls.ControlTemplate.TargetType" /> of a <see cref="T:System.Windows.Controls.ControlTemplate" /> must match the type of the <see cref="T:System.Windows.Controls.Control" /> that it is being applied to.</exception>
		/// <exception cref="T:System.ArgumentException">You must associate the <see cref="T:System.Windows.Controls.ControlTemplate" /> with a <see cref="T:System.Windows.Controls.Control" /> by setting the <see cref="P:System.Windows.Controls.Control.Template" /> property before using the <see cref="T:System.Windows.Controls.ControlTemplate" /> on the <see cref="T:System.Windows.Controls.Control" />.</exception>
		// Token: 0x060041DC RID: 16860 RVA: 0x0012DED0 File Offset: 0x0012C0D0
		protected override void ValidateTemplatedParent(FrameworkElement templatedParent)
		{
			if (templatedParent == null)
			{
				throw new ArgumentNullException("templatedParent");
			}
			if (this._targetType != null && !this._targetType.IsInstanceOfType(templatedParent))
			{
				throw new ArgumentException(SR.Get("TemplateTargetTypeMismatch", new object[]
				{
					this._targetType.Name,
					templatedParent.GetType().Name
				}));
			}
			if (templatedParent.TemplateInternal != this)
			{
				throw new ArgumentException(SR.Get("MustNotTemplateUnassociatedControl"));
			}
		}

		/// <summary>Gets or sets the type for which this <see cref="T:System.Windows.Controls.ControlTemplate" /> is intended.</summary>
		/// <returns>The default value is <see langword="null" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <see cref="P:System.Windows.Controls.ControlTemplate.TargetType" /> property must not be <see langword="null" /> if the definition of the template has a <see cref="T:System.Windows.Controls.ContentPresenter" />.</exception>
		/// <exception cref="T:System.ArgumentException">The specified types are not valid. The <see cref="P:System.Windows.Controls.ControlTemplate.TargetType" /> of a <see cref="T:System.Windows.Controls.ControlTemplate" /> must be or inherit from a <see cref="T:System.Windows.Controls.Control" />, a <see cref="T:System.Windows.Controls.Page" />, or a <see cref="T:System.Windows.Navigation.PageFunctionBase" />.</exception>
		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x060041DD RID: 16861 RVA: 0x0012DF52 File Offset: 0x0012C152
		// (set) Token: 0x060041DE RID: 16862 RVA: 0x0012DF5A File Offset: 0x0012C15A
		[Ambient]
		[DefaultValue(null)]
		public Type TargetType
		{
			get
			{
				return this._targetType;
			}
			set
			{
				this.ValidateTargetType(value, "value");
				base.CheckSealed();
				this._targetType = value;
			}
		}

		/// <summary>Gets a collection of <see cref="T:System.Windows.TriggerBase" /> objects that apply property changes or perform actions based on specified conditions.</summary>
		/// <returns>A collection of <see cref="T:System.Windows.TriggerBase" /> objects. The default value is <see langword="null" />.</returns>
		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x060041DF RID: 16863 RVA: 0x0012DF75 File Offset: 0x0012C175
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

		// Token: 0x060041E0 RID: 16864 RVA: 0x0012DFA4 File Offset: 0x0012C1A4
		private void ValidateTargetType(Type targetType, string argName)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException(argName);
			}
			if (!typeof(Control).IsAssignableFrom(targetType) && !typeof(Page).IsAssignableFrom(targetType) && !typeof(PageFunctionBase).IsAssignableFrom(targetType))
			{
				throw new ArgumentException(SR.Get("InvalidControlTemplateTargetType", new object[]
				{
					targetType.Name
				}));
			}
		}

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x060041E1 RID: 16865 RVA: 0x0012E016 File Offset: 0x0012C216
		internal override Type TargetTypeInternal
		{
			get
			{
				if (this.TargetType != null)
				{
					return this.TargetType;
				}
				return ControlTemplate.DefaultTargetType;
			}
		}

		// Token: 0x060041E2 RID: 16866 RVA: 0x0012E032 File Offset: 0x0012C232
		internal override void SetTargetTypeInternal(Type targetType)
		{
			this.TargetType = targetType;
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x060041E3 RID: 16867 RVA: 0x0012E03B File Offset: 0x0012C23B
		internal override TriggerCollection TriggersInternal
		{
			get
			{
				return this.Triggers;
			}
		}

		// Token: 0x040027BA RID: 10170
		private Type _targetType;

		// Token: 0x040027BB RID: 10171
		private TriggerCollection _triggers;

		// Token: 0x040027BC RID: 10172
		internal static readonly Type DefaultTargetType = typeof(Control);
	}
}
