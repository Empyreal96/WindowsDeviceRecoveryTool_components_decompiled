using System;

namespace System.Windows.Controls.Primitives
{
	/// <summary>Represents an implementation of a <see cref="T:System.Windows.Controls.Primitives.Thumb" /> control that enables a <see cref="T:System.Windows.Window" /> to change its size. </summary>
	// Token: 0x020005A2 RID: 1442
	public class ResizeGrip : Control
	{
		// Token: 0x06005F72 RID: 24434 RVA: 0x001AC09C File Offset: 0x001AA29C
		static ResizeGrip()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeGrip), new FrameworkPropertyMetadata(typeof(ResizeGrip)));
			ResizeGrip._dType = DependencyObjectType.FromSystemTypeInternal(typeof(ResizeGrip));
			Window.IWindowServiceProperty.OverrideMetadata(typeof(ResizeGrip), new FrameworkPropertyMetadata(new PropertyChangedCallback(ResizeGrip._OnWindowServiceChanged)));
		}

		// Token: 0x06005F74 RID: 24436 RVA: 0x001AC108 File Offset: 0x001AA308
		private static void _OnWindowServiceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ResizeGrip resizeGrip = d as ResizeGrip;
			resizeGrip.OnWindowServiceChanged(e.OldValue as Window, e.NewValue as Window);
		}

		// Token: 0x06005F75 RID: 24437 RVA: 0x001AC13A File Offset: 0x001AA33A
		private void OnWindowServiceChanged(Window oldWindow, Window newWindow)
		{
			if (oldWindow != null && oldWindow != newWindow)
			{
				oldWindow.ClearResizeGripControl(this);
			}
			if (newWindow != null)
			{
				newWindow.SetResizeGripControl(this);
			}
		}

		// Token: 0x17001700 RID: 5888
		// (get) Token: 0x06005F76 RID: 24438 RVA: 0x001AC154 File Offset: 0x001AA354
		internal override DependencyObjectType DTypeThemeStyleKey
		{
			get
			{
				return ResizeGrip._dType;
			}
		}

		// Token: 0x17001701 RID: 5889
		// (get) Token: 0x06005F77 RID: 24439 RVA: 0x000962DF File Offset: 0x000944DF
		internal override int EffectiveValuesInitialSize
		{
			get
			{
				return 28;
			}
		}

		// Token: 0x040030A5 RID: 12453
		private static DependencyObjectType _dType;
	}
}
