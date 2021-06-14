using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Media.Animation
{
	/// <summary>A trigger action that begins a <see cref="T:System.Windows.Media.Animation.Storyboard" /> and distributes its animations to their targeted objects and properties.</summary>
	// Token: 0x02000180 RID: 384
	[RuntimeNameProperty("Name")]
	[ContentProperty("Storyboard")]
	public sealed class BeginStoryboard : TriggerAction
	{
		/// <summary>Gets or sets the <see cref="T:System.Windows.Media.Animation.Storyboard" /> that this <see cref="T:System.Windows.Media.Animation.BeginStoryboard" /> starts. </summary>
		/// <returns>The <see cref="T:System.Windows.Media.Animation.Storyboard" /> that the <see cref="T:System.Windows.Media.Animation.BeginStoryboard" /> starts. The default is <see langword="null" />.</returns>
		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x0007053A File Offset: 0x0006E73A
		// (set) Token: 0x06001686 RID: 5766 RVA: 0x0007054C File Offset: 0x0006E74C
		[DefaultValue(null)]
		public Storyboard Storyboard
		{
			get
			{
				return base.GetValue(BeginStoryboard.StoryboardProperty) as Storyboard;
			}
			set
			{
				this.ThrowIfSealed();
				base.SetValue(BeginStoryboard.StoryboardProperty, value);
			}
		}

		/// <summary>Gets or sets the proper hand-off behavior to start an animation clock in this storyboard </summary>
		/// <returns>One of the <see cref="T:System.Windows.Media.Animation.HandoffBehavior" /> enumeration values. The default value is <see cref="F:System.Windows.Media.Animation.HandoffBehavior.SnapshotAndReplace" />.</returns>
		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001687 RID: 5767 RVA: 0x00070560 File Offset: 0x0006E760
		// (set) Token: 0x06001688 RID: 5768 RVA: 0x00070568 File Offset: 0x0006E768
		[DefaultValue(HandoffBehavior.SnapshotAndReplace)]
		public HandoffBehavior HandoffBehavior
		{
			get
			{
				return this._handoffBehavior;
			}
			set
			{
				this.ThrowIfSealed();
				if (HandoffBehaviorEnum.IsDefined(value))
				{
					this._handoffBehavior = value;
					return;
				}
				throw new ArgumentException(SR.Get("Storyboard_UnrecognizedHandoffBehavior"));
			}
		}

		/// <summary>Gets or sets the name of the <see cref="T:System.Windows.Media.Animation.BeginStoryboard" /> object. By naming the <see cref="T:System.Windows.Media.Animation.BeginStoryboard" /> object, the <see cref="T:System.Windows.Media.Animation.Storyboard" /> can be controlled after it is started.</summary>
		/// <returns>The name of the <see cref="T:System.Windows.Media.Animation.BeginStoryboard" />. The default is <see langword="null" />.</returns>
		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06001689 RID: 5769 RVA: 0x0007058F File Offset: 0x0006E78F
		// (set) Token: 0x0600168A RID: 5770 RVA: 0x00070597 File Offset: 0x0006E797
		[DefaultValue(null)]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this.ThrowIfSealed();
				if (value != null && !NameValidationHelper.IsValidIdentifierName(value))
				{
					throw new ArgumentException(SR.Get("InvalidPropertyValue", new object[]
					{
						value,
						"Name"
					}));
				}
				this._name = value;
			}
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x000705D3 File Offset: 0x0006E7D3
		private void ThrowIfSealed()
		{
			if (base.IsSealed)
			{
				throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
				{
					"BeginStoryboard"
				}));
			}
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x000705FC File Offset: 0x0006E7FC
		internal override void Seal()
		{
			if (!base.IsSealed)
			{
				Storyboard storyboard = base.GetValue(BeginStoryboard.StoryboardProperty) as Storyboard;
				if (storyboard == null)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_StoryboardReferenceRequired"));
				}
				if (!storyboard.CanFreeze)
				{
					throw new InvalidOperationException(SR.Get("Storyboard_UnableToFreeze"));
				}
				if (!storyboard.IsFrozen)
				{
					storyboard.Freeze();
				}
				this.Storyboard = storyboard;
			}
			base.Seal();
			base.DetachFromDispatcher();
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00070670 File Offset: 0x0006E870
		internal sealed override void Invoke(FrameworkElement fe, FrameworkContentElement fce, Style targetStyle, FrameworkTemplate frameworkTemplate, long layer)
		{
			INameScope nameScope;
			if (targetStyle != null)
			{
				nameScope = targetStyle;
			}
			else
			{
				nameScope = frameworkTemplate;
			}
			this.Begin((fe != null) ? fe : fce, nameScope, layer);
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x00070699 File Offset: 0x0006E899
		internal sealed override void Invoke(FrameworkElement fe)
		{
			this.Begin(fe, null, Storyboard.Layers.ElementEventTrigger);
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x000706A8 File Offset: 0x0006E8A8
		private void Begin(DependencyObject targetObject, INameScope nameScope, long layer)
		{
			if (this.Storyboard == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_StoryboardReferenceRequired"));
			}
			if (this.Name != null)
			{
				this.Storyboard.BeginCommon(targetObject, nameScope, this._handoffBehavior, true, layer);
				return;
			}
			this.Storyboard.BeginCommon(targetObject, nameScope, this._handoffBehavior, false, layer);
		}

		/// <summary>Identifies the <see cref="P:System.Windows.Media.Animation.BeginStoryboard.Storyboard" /> dependency property.</summary>
		/// <returns>The identifier for the <see cref="P:System.Windows.Media.Animation.BeginStoryboard.Storyboard" /> dependency property.</returns>
		// Token: 0x040012A3 RID: 4771
		public static readonly DependencyProperty StoryboardProperty = DependencyProperty.Register("Storyboard", typeof(Storyboard), typeof(BeginStoryboard));

		// Token: 0x040012A4 RID: 4772
		private HandoffBehavior _handoffBehavior;

		// Token: 0x040012A5 RID: 4773
		private string _name;
	}
}
