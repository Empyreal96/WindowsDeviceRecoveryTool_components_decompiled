using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms.Internal;

namespace System.Windows.Forms
{
	// Token: 0x02000438 RID: 1080
	internal sealed class WindowsFormsUtils
	{
		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06004B34 RID: 19252 RVA: 0x0013640C File Offset: 0x0013460C
		public static Point LastCursorPoint
		{
			get
			{
				int messagePos = SafeNativeMethods.GetMessagePos();
				return new Point(NativeMethods.Util.SignedLOWORD(messagePos), NativeMethods.Util.SignedHIWORD(messagePos));
			}
		}

		// Token: 0x06004B35 RID: 19253 RVA: 0x00136430 File Offset: 0x00134630
		public static Graphics CreateMeasurementGraphics()
		{
			return Graphics.FromHdcInternal(WindowsGraphicsCacheManager.MeasurementGraphics.DeviceContext.Hdc);
		}

		// Token: 0x06004B36 RID: 19254 RVA: 0x00136448 File Offset: 0x00134648
		public static bool ContainsMnemonic(string text)
		{
			if (text != null)
			{
				int length = text.Length;
				int num = text.IndexOf('&', 0);
				if (num >= 0 && num <= length - 2)
				{
					int num2 = text.IndexOf('&', num + 1);
					if (num2 == -1)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06004B37 RID: 19255 RVA: 0x00136486 File Offset: 0x00134686
		internal static Rectangle ConstrainToScreenWorkingAreaBounds(Rectangle bounds)
		{
			return WindowsFormsUtils.ConstrainToBounds(Screen.GetWorkingArea(bounds), bounds);
		}

		// Token: 0x06004B38 RID: 19256 RVA: 0x00136494 File Offset: 0x00134694
		internal static Rectangle ConstrainToScreenBounds(Rectangle bounds)
		{
			return WindowsFormsUtils.ConstrainToBounds(Screen.FromRectangle(bounds).Bounds, bounds);
		}

		// Token: 0x06004B39 RID: 19257 RVA: 0x001364A8 File Offset: 0x001346A8
		internal static Rectangle ConstrainToBounds(Rectangle constrainingBounds, Rectangle bounds)
		{
			if (!constrainingBounds.Contains(bounds))
			{
				bounds.Size = new Size(Math.Min(constrainingBounds.Width - 2, bounds.Width), Math.Min(constrainingBounds.Height - 2, bounds.Height));
				if (bounds.Right > constrainingBounds.Right)
				{
					bounds.X = constrainingBounds.Right - bounds.Width;
				}
				else if (bounds.Left < constrainingBounds.Left)
				{
					bounds.X = constrainingBounds.Left;
				}
				if (bounds.Bottom > constrainingBounds.Bottom)
				{
					bounds.Y = constrainingBounds.Bottom - 1 - bounds.Height;
				}
				else if (bounds.Top < constrainingBounds.Top)
				{
					bounds.Y = constrainingBounds.Top;
				}
			}
			return bounds;
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x00136588 File Offset: 0x00134788
		internal static string EscapeTextWithAmpersands(string text)
		{
			if (text == null)
			{
				return null;
			}
			int i = text.IndexOf('&');
			if (i == -1)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Substring(0, i));
			while (i < text.Length)
			{
				if (text[i] == '&')
				{
					stringBuilder.Append("&");
				}
				if (i < text.Length)
				{
					stringBuilder.Append(text[i]);
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x001365FC File Offset: 0x001347FC
		internal static string GetControlInformation(IntPtr hwnd)
		{
			if (hwnd == IntPtr.Zero)
			{
				return "Handle is IntPtr.Zero";
			}
			return "";
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x00136623 File Offset: 0x00134823
		internal static string AssertControlInformation(bool condition, Control control)
		{
			if (condition)
			{
				return string.Empty;
			}
			return WindowsFormsUtils.GetControlInformation(control.Handle);
		}

		// Token: 0x06004B3D RID: 19261 RVA: 0x0013663C File Offset: 0x0013483C
		internal static int GetCombinedHashCodes(params int[] args)
		{
			int num = -757577119;
			for (int i = 0; i < args.Length; i++)
			{
				num = (args[i] ^ num) * -1640531535;
			}
			return num;
		}

		// Token: 0x06004B3E RID: 19262 RVA: 0x0013666C File Offset: 0x0013486C
		public static char GetMnemonic(string text, bool bConvertToUpperCase)
		{
			char result = '\0';
			if (text != null)
			{
				int length = text.Length;
				for (int i = 0; i < length - 1; i++)
				{
					if (text[i] == '&')
					{
						if (text[i + 1] == '&')
						{
							i++;
						}
						else
						{
							if (bConvertToUpperCase)
							{
								result = char.ToUpper(text[i + 1], CultureInfo.CurrentCulture);
								break;
							}
							result = char.ToLower(text[i + 1], CultureInfo.CurrentCulture);
							break;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004B3F RID: 19263 RVA: 0x001366E4 File Offset: 0x001348E4
		public static HandleRef GetRootHWnd(HandleRef hwnd)
		{
			IntPtr ancestor = UnsafeNativeMethods.GetAncestor(new HandleRef(hwnd, hwnd.Handle), 2);
			return new HandleRef(hwnd.Wrapper, ancestor);
		}

		// Token: 0x06004B40 RID: 19264 RVA: 0x00136717 File Offset: 0x00134917
		public static HandleRef GetRootHWnd(Control control)
		{
			return WindowsFormsUtils.GetRootHWnd(new HandleRef(control, control.Handle));
		}

		// Token: 0x06004B41 RID: 19265 RVA: 0x0013672C File Offset: 0x0013492C
		public static string TextWithoutMnemonics(string text)
		{
			if (text == null)
			{
				return null;
			}
			int i = text.IndexOf('&');
			if (i == -1)
			{
				return text;
			}
			StringBuilder stringBuilder = new StringBuilder(text.Substring(0, i));
			while (i < text.Length)
			{
				if (text[i] == '&')
				{
					i++;
				}
				if (i < text.Length)
				{
					stringBuilder.Append(text[i]);
				}
				i++;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004B42 RID: 19266 RVA: 0x00136798 File Offset: 0x00134998
		public static Point TranslatePoint(Point point, Control fromControl, Control toControl)
		{
			NativeMethods.POINT point2 = new NativeMethods.POINT(point.X, point.Y);
			UnsafeNativeMethods.MapWindowPoints(new HandleRef(fromControl, fromControl.Handle), new HandleRef(toControl, toControl.Handle), point2, 1);
			return new Point(point2.x, point2.y);
		}

		// Token: 0x06004B43 RID: 19267 RVA: 0x001367EA File Offset: 0x001349EA
		public static bool SafeCompareStrings(string string1, string string2, bool ignoreCase)
		{
			return string1 != null && string2 != null && string1.Length == string2.Length && string.Compare(string1, string2, ignoreCase, CultureInfo.InvariantCulture) == 0;
		}

		// Token: 0x06004B44 RID: 19268 RVA: 0x00136814 File Offset: 0x00134A14
		public static int RotateLeft(int value, int nBits)
		{
			nBits %= 32;
			return value << nBits | value >> 32 - nBits;
		}

		// Token: 0x06004B45 RID: 19269 RVA: 0x0013682C File Offset: 0x00134A2C
		public static string GetComponentName(IComponent component, string defaultNameValue)
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(defaultNameValue))
			{
				if (component.Site != null)
				{
					text = component.Site.Name;
				}
				if (text == null)
				{
					text = string.Empty;
				}
			}
			else
			{
				text = defaultNameValue;
			}
			return text;
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06004B46 RID: 19270 RVA: 0x00136869 File Offset: 0x00134A69
		internal static bool TargetsAtLeast_v4_5
		{
			get
			{
				return WindowsFormsUtils._targetsAtLeast_v4_5;
			}
		}

		// Token: 0x06004B47 RID: 19271 RVA: 0x00136870 File Offset: 0x00134A70
		[SecuritySafeCritical]
		[ReflectionPermission(SecurityAction.Assert, Unrestricted = true)]
		private static bool RunningOnCheck(string propertyName)
		{
			Type type;
			try
			{
				type = typeof(object).GetTypeInfo().Assembly.GetType("System.Runtime.Versioning.BinaryCompatibility", false);
			}
			catch (TypeLoadException)
			{
				return false;
			}
			if (type == null)
			{
				return false;
			}
			PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			return !(property == null) && (bool)property.GetValue(null);
		}

		// Token: 0x0400277D RID: 10109
		public static readonly Size UninitializedSize = new Size(-7199369, -5999471);

		// Token: 0x0400277E RID: 10110
		private static bool _targetsAtLeast_v4_5 = WindowsFormsUtils.RunningOnCheck("TargetsAtLeast_Desktop_V4_5");

		// Token: 0x0400277F RID: 10111
		public static readonly ContentAlignment AnyRightAlign = (ContentAlignment)1092;

		// Token: 0x04002780 RID: 10112
		public static readonly ContentAlignment AnyLeftAlign = (ContentAlignment)273;

		// Token: 0x04002781 RID: 10113
		public static readonly ContentAlignment AnyTopAlign = (ContentAlignment)7;

		// Token: 0x04002782 RID: 10114
		public static readonly ContentAlignment AnyBottomAlign = (ContentAlignment)1792;

		// Token: 0x04002783 RID: 10115
		public static readonly ContentAlignment AnyMiddleAlign = (ContentAlignment)112;

		// Token: 0x04002784 RID: 10116
		public static readonly ContentAlignment AnyCenterAlign = (ContentAlignment)546;

		// Token: 0x02000803 RID: 2051
		public static class EnumValidator
		{
			// Token: 0x06006E34 RID: 28212 RVA: 0x00193650 File Offset: 0x00191850
			public static bool IsValidContentAlignment(ContentAlignment contentAlign)
			{
				if (ClientUtils.GetBitCount((uint)contentAlign) != 1)
				{
					return false;
				}
				int num = 1911;
				return (num & (int)contentAlign) != 0;
			}

			// Token: 0x06006E35 RID: 28213 RVA: 0x00193674 File Offset: 0x00191874
			public static bool IsEnumWithinShiftedRange(Enum enumValue, int numBitsToShift, int minValAfterShift, int maxValAfterShift)
			{
				int num = Convert.ToInt32(enumValue, CultureInfo.InvariantCulture);
				int num2 = num >> numBitsToShift;
				return num2 << numBitsToShift == num && num2 >= minValAfterShift && num2 <= maxValAfterShift;
			}

			// Token: 0x06006E36 RID: 28214 RVA: 0x001936AC File Offset: 0x001918AC
			public static bool IsValidTextImageRelation(TextImageRelation relation)
			{
				return ClientUtils.IsEnumValid(relation, (int)relation, 0, 8, 1);
			}

			// Token: 0x06006E37 RID: 28215 RVA: 0x001936BD File Offset: 0x001918BD
			public static bool IsValidArrowDirection(ArrowDirection direction)
			{
				return direction <= ArrowDirection.Up || direction - ArrowDirection.Right <= 1;
			}
		}

		// Token: 0x02000804 RID: 2052
		public class ArraySubsetEnumerator : IEnumerator
		{
			// Token: 0x06006E38 RID: 28216 RVA: 0x001936CD File Offset: 0x001918CD
			public ArraySubsetEnumerator(object[] array, int count)
			{
				this.array = array;
				this.total = count;
				this.current = -1;
			}

			// Token: 0x06006E39 RID: 28217 RVA: 0x001936EA File Offset: 0x001918EA
			public bool MoveNext()
			{
				if (this.current < this.total - 1)
				{
					this.current++;
					return true;
				}
				return false;
			}

			// Token: 0x06006E3A RID: 28218 RVA: 0x0019370D File Offset: 0x0019190D
			public void Reset()
			{
				this.current = -1;
			}

			// Token: 0x170017D0 RID: 6096
			// (get) Token: 0x06006E3B RID: 28219 RVA: 0x00193716 File Offset: 0x00191916
			public object Current
			{
				get
				{
					if (this.current == -1)
					{
						return null;
					}
					return this.array[this.current];
				}
			}

			// Token: 0x0400423A RID: 16954
			private object[] array;

			// Token: 0x0400423B RID: 16955
			private int total;

			// Token: 0x0400423C RID: 16956
			private int current;
		}

		// Token: 0x02000805 RID: 2053
		internal class ReadOnlyControlCollection : Control.ControlCollection
		{
			// Token: 0x06006E3C RID: 28220 RVA: 0x00193730 File Offset: 0x00191930
			public ReadOnlyControlCollection(Control owner, bool isReadOnly) : base(owner)
			{
				this._isReadOnly = isReadOnly;
			}

			// Token: 0x06006E3D RID: 28221 RVA: 0x00193740 File Offset: 0x00191940
			public override void Add(Control value)
			{
				if (this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				this.AddInternal(value);
			}

			// Token: 0x06006E3E RID: 28222 RVA: 0x00193761 File Offset: 0x00191961
			internal virtual void AddInternal(Control value)
			{
				base.Add(value);
			}

			// Token: 0x06006E3F RID: 28223 RVA: 0x0019376A File Offset: 0x0019196A
			public override void Clear()
			{
				if (this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				base.Clear();
			}

			// Token: 0x06006E40 RID: 28224 RVA: 0x001740E2 File Offset: 0x001722E2
			internal virtual void RemoveInternal(Control value)
			{
				base.Remove(value);
			}

			// Token: 0x06006E41 RID: 28225 RVA: 0x0019378A File Offset: 0x0019198A
			public override void RemoveByKey(string key)
			{
				if (this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				base.RemoveByKey(key);
			}

			// Token: 0x170017D1 RID: 6097
			// (get) Token: 0x06006E42 RID: 28226 RVA: 0x001937AB File Offset: 0x001919AB
			public override bool IsReadOnly
			{
				get
				{
					return this._isReadOnly;
				}
			}

			// Token: 0x0400423D RID: 16957
			private readonly bool _isReadOnly;
		}

		// Token: 0x02000806 RID: 2054
		internal class TypedControlCollection : WindowsFormsUtils.ReadOnlyControlCollection
		{
			// Token: 0x06006E43 RID: 28227 RVA: 0x001937B3 File Offset: 0x001919B3
			public TypedControlCollection(Control owner, Type typeOfControl, bool isReadOnly) : base(owner, isReadOnly)
			{
				this.typeOfControl = typeOfControl;
				this.ownerControl = owner;
			}

			// Token: 0x06006E44 RID: 28228 RVA: 0x001937CB File Offset: 0x001919CB
			public TypedControlCollection(Control owner, Type typeOfControl) : base(owner, false)
			{
				this.typeOfControl = typeOfControl;
				this.ownerControl = owner;
			}

			// Token: 0x06006E45 RID: 28229 RVA: 0x001937E4 File Offset: 0x001919E4
			public override void Add(Control value)
			{
				Control.CheckParentingCycle(this.ownerControl, value);
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.IsReadOnly)
				{
					throw new NotSupportedException(SR.GetString("ReadonlyControlsCollection"));
				}
				if (!this.typeOfControl.IsAssignableFrom(value.GetType()))
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, SR.GetString("TypedControlCollectionShouldBeOfType", new object[]
					{
						this.typeOfControl.Name
					}), new object[0]), value.GetType().Name);
				}
				base.Add(value);
			}

			// Token: 0x0400423E RID: 16958
			private Type typeOfControl;

			// Token: 0x0400423F RID: 16959
			private Control ownerControl;
		}

		// Token: 0x02000807 RID: 2055
		internal struct DCMapping : IDisposable
		{
			// Token: 0x06006E46 RID: 28230 RVA: 0x0019387C File Offset: 0x00191A7C
			public DCMapping(HandleRef hDC, Rectangle bounds)
			{
				if (hDC.Handle == IntPtr.Zero)
				{
					throw new ArgumentNullException("hDC");
				}
				NativeMethods.POINT point = new NativeMethods.POINT();
				HandleRef handleRef = NativeMethods.NullHandleRef;
				this.translatedBounds = bounds;
				this.graphics = null;
				this.dc = DeviceContext.FromHdc(hDC.Handle);
				this.dc.SaveHdc();
				bool flag = SafeNativeMethods.GetViewportOrgEx(hDC, point);
				HandleRef handleRef2 = new HandleRef(null, SafeNativeMethods.CreateRectRgn(point.x + bounds.Left, point.y + bounds.Top, point.x + bounds.Right, point.y + bounds.Bottom));
				try
				{
					handleRef = new HandleRef(this, SafeNativeMethods.CreateRectRgn(0, 0, 0, 0));
					int clipRgn = SafeNativeMethods.GetClipRgn(hDC, handleRef);
					NativeMethods.POINT point2 = new NativeMethods.POINT();
					flag = SafeNativeMethods.SetViewportOrgEx(hDC, point.x + bounds.Left, point.y + bounds.Top, point2);
					if (clipRgn != 0)
					{
						NativeMethods.RECT rect = default(NativeMethods.RECT);
						NativeMethods.RegionFlags rgnBox = (NativeMethods.RegionFlags)SafeNativeMethods.GetRgnBox(handleRef, ref rect);
						if (rgnBox == NativeMethods.RegionFlags.SIMPLEREGION)
						{
							NativeMethods.RegionFlags regionFlags = (NativeMethods.RegionFlags)SafeNativeMethods.CombineRgn(handleRef2, handleRef2, handleRef, 1);
						}
					}
					else
					{
						SafeNativeMethods.DeleteObject(handleRef);
						handleRef = new HandleRef(null, IntPtr.Zero);
					}
					NativeMethods.RegionFlags regionFlags2 = (NativeMethods.RegionFlags)SafeNativeMethods.SelectClipRgn(hDC, handleRef2);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					this.dc.RestoreHdc();
					this.dc.Dispose();
				}
				finally
				{
					flag = SafeNativeMethods.DeleteObject(handleRef2);
					if (handleRef.Handle != IntPtr.Zero)
					{
						flag = SafeNativeMethods.DeleteObject(handleRef);
					}
				}
			}

			// Token: 0x06006E47 RID: 28231 RVA: 0x00193A2C File Offset: 0x00191C2C
			public void Dispose()
			{
				if (this.graphics != null)
				{
					this.graphics.Dispose();
					this.graphics = null;
				}
				if (this.dc != null)
				{
					this.dc.RestoreHdc();
					this.dc.Dispose();
					this.dc = null;
				}
			}

			// Token: 0x170017D2 RID: 6098
			// (get) Token: 0x06006E48 RID: 28232 RVA: 0x00193A78 File Offset: 0x00191C78
			public Graphics Graphics
			{
				get
				{
					if (this.graphics == null)
					{
						this.graphics = Graphics.FromHdcInternal(this.dc.Hdc);
						this.graphics.SetClip(new Rectangle(Point.Empty, this.translatedBounds.Size));
					}
					return this.graphics;
				}
			}

			// Token: 0x04004240 RID: 16960
			private DeviceContext dc;

			// Token: 0x04004241 RID: 16961
			private Graphics graphics;

			// Token: 0x04004242 RID: 16962
			private Rectangle translatedBounds;
		}
	}
}
