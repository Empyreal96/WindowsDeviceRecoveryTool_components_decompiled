using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace System.Windows.Media.Animation
{
	/// <summary>Manipulates a <see cref="T:System.Windows.Media.Animation.Storyboard" /> that has been applied by a <see cref="T:System.Windows.Media.Animation.BeginStoryboard" /> action.</summary>
	// Token: 0x02000181 RID: 385
	public abstract class ControllableStoryboardAction : TriggerAction
	{
		// Token: 0x06001691 RID: 5777 RVA: 0x00070532 File Offset: 0x0006E732
		internal ControllableStoryboardAction()
		{
		}

		/// <summary>Gets or sets the <see cref="P:System.Windows.Media.Animation.BeginStoryboard.Name" /> of the <see cref="T:System.Windows.Media.Animation.BeginStoryboard" /> that began the <see cref="T:System.Windows.Media.Animation.Storyboard" /> you want to interactively control. </summary>
		/// <returns>The <see cref="P:System.Windows.Media.Animation.BeginStoryboard.Name" /> of the <see cref="T:System.Windows.Media.Animation.BeginStoryboard" /> that began the <see cref="T:System.Windows.Media.Animation.Storyboard" /> you want to interactively control. The default value is <see langword="null" />.</returns>
		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06001692 RID: 5778 RVA: 0x00070725 File Offset: 0x0006E925
		// (set) Token: 0x06001693 RID: 5779 RVA: 0x0007072D File Offset: 0x0006E92D
		[DefaultValue(null)]
		public string BeginStoryboardName
		{
			get
			{
				return this._beginStoryboardName;
			}
			set
			{
				if (base.IsSealed)
				{
					throw new InvalidOperationException(SR.Get("CannotChangeAfterSealed", new object[]
					{
						"ControllableStoryboardAction"
					}));
				}
				this._beginStoryboardName = value;
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x0007075C File Offset: 0x0006E95C
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
			this.Invoke(fe, fce, this.GetStoryboard(fe, fce, nameScope));
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00070786 File Offset: 0x0006E986
		internal sealed override void Invoke(FrameworkElement fe)
		{
			this.Invoke(fe, null, this.GetStoryboard(fe, null, null));
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x00002137 File Offset: 0x00000337
		internal virtual void Invoke(FrameworkElement containingFE, FrameworkContentElement containingFCE, Storyboard storyboard)
		{
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x0007079C File Offset: 0x0006E99C
		private Storyboard GetStoryboard(FrameworkElement fe, FrameworkContentElement fce, INameScope nameScope)
		{
			if (this.BeginStoryboardName == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_BeginStoryboardNameRequired"));
			}
			BeginStoryboard beginStoryboard = Storyboard.ResolveBeginStoryboardName(this.BeginStoryboardName, nameScope, fe, fce);
			Storyboard storyboard = beginStoryboard.Storyboard;
			if (storyboard == null)
			{
				throw new InvalidOperationException(SR.Get("Storyboard_BeginStoryboardNoStoryboard", new object[]
				{
					this.BeginStoryboardName
				}));
			}
			return storyboard;
		}

		// Token: 0x040012A6 RID: 4774
		private string _beginStoryboardName;
	}
}
