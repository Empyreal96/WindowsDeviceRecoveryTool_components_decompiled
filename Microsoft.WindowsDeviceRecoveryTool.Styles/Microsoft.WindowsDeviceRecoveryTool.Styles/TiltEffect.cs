using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Microsoft.WindowsDeviceRecoveryTool.Styles
{
	// Token: 0x0200001C RID: 28
	public class TiltEffect : DependencyObject
	{
		// Token: 0x06000095 RID: 149 RVA: 0x00003C60 File Offset: 0x00001E60
		public static bool GetIsTiltEnabled(DependencyObject source)
		{
			return (bool)source.GetValue(TiltEffect.IsTiltEnabledProperty);
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00003C82 File Offset: 0x00001E82
		public static void SetIsTiltEnabled(DependencyObject source, bool value)
		{
			source.SetValue(TiltEffect.IsTiltEnabledProperty, value);
		}

		// Token: 0x06000097 RID: 151 RVA: 0x00003C98 File Offset: 0x00001E98
		public static Vector3D GetRotationVector(DependencyObject source)
		{
			return (Vector3D)source.GetValue(TiltEffect.RotationVectorProperty);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00003CBA File Offset: 0x00001EBA
		public static void SetRotationVector(DependencyObject source, Vector3D value)
		{
			source.SetValue(TiltEffect.RotationVectorProperty, value);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00003CD0 File Offset: 0x00001ED0
		public static void OnIsTiltEnabledChanged(DependencyObject target, DependencyPropertyChangedEventArgs args)
		{
			FrameworkElement frameworkElement = target as FrameworkElement;
			if (frameworkElement != null)
			{
				if ((bool)args.NewValue)
				{
					frameworkElement.MouseEnter += TiltEffect.OnMouseEnter;
				}
				else
				{
					frameworkElement.MouseEnter -= TiltEffect.OnMouseEnter;
				}
			}
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00003D2E File Offset: 0x00001F2E
		private static void OnMouseEnter(object sender, MouseEventArgs args)
		{
			TiltEffect.TryStartTiltEffect(sender as FrameworkElement, args);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00003D40 File Offset: 0x00001F40
		private static void TryStartTiltEffect(FrameworkElement frameworkElement, MouseEventArgs args)
		{
			if (TiltEffect.currentTiltElement != null && !object.Equals(TiltEffect.currentTiltElement, frameworkElement))
			{
				TiltEffect.EndTiltEffect(TiltEffect.currentTiltElement);
			}
			TiltEffect.currentTiltElement = frameworkElement;
			TiltEffect.currentTiltElement.MouseMove += TiltEffect.OnCurrentTiltElementMouseMove;
			TiltEffect.currentTiltElement.MouseLeftButtonUp += TiltEffect.OnCurrentTiltElementMouseLeftButtonUp;
			TiltEffect.currentTiltElement.MouseLeave += TiltEffect.OnCurrentTiltElementMouseLeave;
			TiltEffect.currentTiltElementCenter = new Point(TiltEffect.currentTiltElement.ActualWidth / 2.0, TiltEffect.currentTiltElement.ActualHeight / 2.0);
			TiltEffect.ContinueTiltEffect(frameworkElement, args);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00003DFC File Offset: 0x00001FFC
		private static void ContinueTiltEffect(IInputElement element, MouseEventArgs args)
		{
			Point position = args.GetPosition(element);
			Point point = new Point(Math.Min(Math.Max(position.X / (TiltEffect.currentTiltElementCenter.X * 2.0), 0.0), 1.0), Math.Min(Math.Max(position.Y / (TiltEffect.currentTiltElementCenter.Y * 2.0), 0.0), 1.0));
			if (!double.IsNaN(point.X) && !double.IsNaN(point.Y))
			{
				double num = Math.Abs(point.X - 0.5);
				double num2 = Math.Abs(point.Y - 0.5);
				double num3 = (double)(-(double)Math.Sign(point.X - 0.5));
				double num4 = (double)Math.Sign(point.Y - 0.5);
				double num5 = num + num2;
				double num6 = (num + num2 > 0.0) ? (num / (num + num2)) : 0.0;
				double num7 = num5 * 0.3 * 180.0 / 3.141592653589793;
				double num8 = (1.0 - num5) * 3.0;
				double z = -num8;
				double num9 = num7 * num6 * num3;
				double x = num7 * (1.0 - num6) * num4;
				Vector3D value = new Vector3D(x, -num9, z);
				TiltEffect.SetRotationVector(TiltEffect.currentTiltElement, value);
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00003FB8 File Offset: 0x000021B8
		private static void EndTiltEffect(IInputElement element)
		{
			if (element != null)
			{
				element.MouseMove -= TiltEffect.OnCurrentTiltElementMouseMove;
				element.MouseLeftButtonUp -= TiltEffect.OnCurrentTiltElementMouseLeftButtonUp;
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00003FF6 File Offset: 0x000021F6
		private static void OnCurrentTiltElementMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			TiltEffect.EndTiltEffect(sender as FrameworkElement);
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004005 File Offset: 0x00002205
		private static void OnCurrentTiltElementMouseLeave(object sender, MouseEventArgs e)
		{
			TiltEffect.EndTiltEffect(sender as FrameworkElement);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004014 File Offset: 0x00002214
		private static void OnCurrentTiltElementMouseMove(object sender, MouseEventArgs args)
		{
			TiltEffect.ContinueTiltEffect(sender as FrameworkElement, args);
		}

		// Token: 0x04000022 RID: 34
		private const double MaxAngle = 0.3;

		// Token: 0x04000023 RID: 35
		private const double MaxDepression = 3.0;

		// Token: 0x04000024 RID: 36
		private static FrameworkElement currentTiltElement;

		// Token: 0x04000025 RID: 37
		private static Point currentTiltElementCenter;

		// Token: 0x04000026 RID: 38
		public static readonly DependencyProperty IsTiltEnabledProperty = DependencyProperty.RegisterAttached("IsTiltEnabled", typeof(bool), typeof(TiltEffect), new PropertyMetadata(new PropertyChangedCallback(TiltEffect.OnIsTiltEnabledChanged)));

		// Token: 0x04000027 RID: 39
		public static readonly DependencyProperty RotationVectorProperty = DependencyProperty.RegisterAttached("RotationVector", typeof(Vector3D), typeof(TiltEffect), null);
	}
}
