using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
	/// <summary>Provides data for the events defined on <see cref="T:System.Windows.Forms.HtmlDocument" /> and <see cref="T:System.Windows.Forms.HtmlElement" />.</summary>
	// Token: 0x0200026F RID: 623
	public sealed class HtmlElementEventArgs : EventArgs
	{
		// Token: 0x060025A6 RID: 9638 RVA: 0x000B4678 File Offset: 0x000B2878
		internal HtmlElementEventArgs(HtmlShimManager shimManager, UnsafeNativeMethods.IHTMLEventObj eventObj)
		{
			this.htmlEventObj = eventObj;
			this.shimManager = shimManager;
		}

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060025A7 RID: 9639 RVA: 0x000B468E File Offset: 0x000B288E
		private UnsafeNativeMethods.IHTMLEventObj NativeHTMLEventObj
		{
			get
			{
				return this.htmlEventObj;
			}
		}

		/// <summary>Gets the mouse button that was clicked during a <see cref="E:System.Windows.Forms.HtmlElement.MouseDown" /> or <see cref="E:System.Windows.Forms.HtmlElement.MouseUp" /> event.</summary>
		/// <returns>The mouse button that was clicked.</returns>
		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x060025A8 RID: 9640 RVA: 0x000B4698 File Offset: 0x000B2898
		public MouseButtons MouseButtonsPressed
		{
			get
			{
				MouseButtons mouseButtons = MouseButtons.None;
				int button = this.NativeHTMLEventObj.GetButton();
				if ((button & 1) != 0)
				{
					mouseButtons |= MouseButtons.Left;
				}
				if ((button & 2) != 0)
				{
					mouseButtons |= MouseButtons.Right;
				}
				if ((button & 4) != 0)
				{
					mouseButtons |= MouseButtons.Middle;
				}
				return mouseButtons;
			}
		}

		/// <summary>Gets or sets the position of the mouse cursor in the document's client area. </summary>
		/// <returns>The current position of the mouse cursor. </returns>
		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x060025A9 RID: 9641 RVA: 0x000B46DB File Offset: 0x000B28DB
		public Point ClientMousePosition
		{
			get
			{
				return new Point(this.NativeHTMLEventObj.GetClientX(), this.NativeHTMLEventObj.GetClientY());
			}
		}

		/// <summary>Gets or sets the position of the mouse cursor relative to the element that raises the event.</summary>
		/// <returns>The mouse position relative to the element that raises the event.</returns>
		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x000B46F8 File Offset: 0x000B28F8
		public Point OffsetMousePosition
		{
			get
			{
				return new Point(this.NativeHTMLEventObj.GetOffsetX(), this.NativeHTMLEventObj.GetOffsetY());
			}
		}

		/// <summary>Gets or sets the position of the mouse cursor relative to a relatively positioned parent element.</summary>
		/// <returns>The position of the mouse cursor relative to the upper-left corner of the parent of the element that raised the event, if the parent element is relatively positioned. </returns>
		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060025AB RID: 9643 RVA: 0x000B4715 File Offset: 0x000B2915
		public Point MousePosition
		{
			get
			{
				return new Point(this.NativeHTMLEventObj.GetX(), this.NativeHTMLEventObj.GetY());
			}
		}

		/// <summary>Gets or sets a value indicating whether the current event bubbles up through the element hierarchy of the HTML Document Object Model.</summary>
		/// <returns>
		///     <see langword="true" /> if the event bubbles; <see langword="false" /> if it does not. </returns>
		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060025AC RID: 9644 RVA: 0x000B4732 File Offset: 0x000B2932
		// (set) Token: 0x060025AD RID: 9645 RVA: 0x000B4742 File Offset: 0x000B2942
		public bool BubbleEvent
		{
			get
			{
				return !this.NativeHTMLEventObj.GetCancelBubble();
			}
			set
			{
				this.NativeHTMLEventObj.SetCancelBubble(!value);
			}
		}

		/// <summary>Gets the ASCII value of the keyboard character typed in a <see cref="E:System.Windows.Forms.HtmlElement.KeyPress" />, <see cref="E:System.Windows.Forms.HtmlElement.KeyDown" />, or <see cref="E:System.Windows.Forms.HtmlElement.KeyUp" /> event.</summary>
		/// <returns>The ASCII value of the composed keyboard entry.</returns>
		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060025AE RID: 9646 RVA: 0x000B4753 File Offset: 0x000B2953
		public int KeyPressedCode
		{
			get
			{
				return this.NativeHTMLEventObj.GetKeyCode();
			}
		}

		/// <summary>Indicates whether the ALT key was pressed when this event occurred.</summary>
		/// <returns>
		///     <see langword="true" /> is the ALT key was pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060025AF RID: 9647 RVA: 0x000B4760 File Offset: 0x000B2960
		public bool AltKeyPressed
		{
			get
			{
				return this.NativeHTMLEventObj.GetAltKey();
			}
		}

		/// <summary>Indicates whether the CTRL key was pressed when this event occurred.</summary>
		/// <returns>
		///     <see langword="true" /> if the CTRL key was pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x060025B0 RID: 9648 RVA: 0x000B476D File Offset: 0x000B296D
		public bool CtrlKeyPressed
		{
			get
			{
				return this.NativeHTMLEventObj.GetCtrlKey();
			}
		}

		/// <summary>Indicates whether the SHIFT key was pressed when this event occurred.</summary>
		/// <returns>
		///     <see langword="true" /> if the SHIFT key was pressed; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x060025B1 RID: 9649 RVA: 0x000B477A File Offset: 0x000B297A
		public bool ShiftKeyPressed
		{
			get
			{
				return this.NativeHTMLEventObj.GetShiftKey();
			}
		}

		/// <summary>Gets the name of the event that was raised.</summary>
		/// <returns>The name of the event. </returns>
		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x060025B2 RID: 9650 RVA: 0x000B4787 File Offset: 0x000B2987
		public string EventType
		{
			get
			{
				return this.NativeHTMLEventObj.GetEventType();
			}
		}

		/// <summary>Gets or sets the return value of the handled event. </summary>
		/// <returns>
		///     <see langword="true" /> if the event has been handled; otherwise, <see langword="false" />.</returns>
		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x060025B3 RID: 9651 RVA: 0x000B4794 File Offset: 0x000B2994
		// (set) Token: 0x060025B4 RID: 9652 RVA: 0x000B47B8 File Offset: 0x000B29B8
		public bool ReturnValue
		{
			get
			{
				object returnValue = this.NativeHTMLEventObj.GetReturnValue();
				return returnValue == null || (bool)returnValue;
			}
			set
			{
				object returnValue = value;
				this.NativeHTMLEventObj.SetReturnValue(returnValue);
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.HtmlElement" /> the mouse pointer is moving away from.</summary>
		/// <returns>The <see cref="T:System.Windows.Forms.HtmlElement" /> the mouse pointer is moving away from.</returns>
		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x060025B5 RID: 9653 RVA: 0x000B47D8 File Offset: 0x000B29D8
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public HtmlElement FromElement
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement fromElement = this.NativeHTMLEventObj.GetFromElement();
				if (fromElement != null)
				{
					return new HtmlElement(this.shimManager, fromElement);
				}
				return null;
			}
		}

		/// <summary>Gets the <see cref="T:System.Windows.Forms.HtmlElement" /> toward which the user is moving the mouse pointer.</summary>
		/// <returns>The element toward which the mouse pointer is moving. </returns>
		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x060025B6 RID: 9654 RVA: 0x000B4804 File Offset: 0x000B2A04
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public HtmlElement ToElement
		{
			get
			{
				UnsafeNativeMethods.IHTMLElement toElement = this.NativeHTMLEventObj.GetToElement();
				if (toElement != null)
				{
					return new HtmlElement(this.shimManager, toElement);
				}
				return null;
			}
		}

		// Token: 0x0400101A RID: 4122
		private UnsafeNativeMethods.IHTMLEventObj htmlEventObj;

		// Token: 0x0400101B RID: 4123
		private HtmlShimManager shimManager;
	}
}
