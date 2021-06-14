using System;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Implements a Windows message.</summary>
	// Token: 0x020002E8 RID: 744
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public struct Message
	{
		/// <summary>Gets or sets the window handle of the message.</summary>
		/// <returns>The window handle of the message.</returns>
		// Token: 0x17000AE6 RID: 2790
		// (get) Token: 0x06002CE4 RID: 11492 RVA: 0x000D0D2F File Offset: 0x000CEF2F
		// (set) Token: 0x06002CE5 RID: 11493 RVA: 0x000D0D37 File Offset: 0x000CEF37
		public IntPtr HWnd
		{
			get
			{
				return this.hWnd;
			}
			set
			{
				this.hWnd = value;
			}
		}

		/// <summary>Gets or sets the ID number for the message.</summary>
		/// <returns>The ID number for the message.</returns>
		// Token: 0x17000AE7 RID: 2791
		// (get) Token: 0x06002CE6 RID: 11494 RVA: 0x000D0D40 File Offset: 0x000CEF40
		// (set) Token: 0x06002CE7 RID: 11495 RVA: 0x000D0D48 File Offset: 0x000CEF48
		public int Msg
		{
			get
			{
				return this.msg;
			}
			set
			{
				this.msg = value;
			}
		}

		/// <summary>Gets or sets the <see cref="P:System.Windows.Forms.Message.WParam" /> field of the message.</summary>
		/// <returns>The <see cref="P:System.Windows.Forms.Message.WParam" /> field of the message.</returns>
		// Token: 0x17000AE8 RID: 2792
		// (get) Token: 0x06002CE8 RID: 11496 RVA: 0x000D0D51 File Offset: 0x000CEF51
		// (set) Token: 0x06002CE9 RID: 11497 RVA: 0x000D0D59 File Offset: 0x000CEF59
		public IntPtr WParam
		{
			get
			{
				return this.wparam;
			}
			set
			{
				this.wparam = value;
			}
		}

		/// <summary>Specifies the <see cref="P:System.Windows.Forms.Message.LParam" /> field of the message.</summary>
		/// <returns>The <see cref="P:System.Windows.Forms.Message.LParam" /> field of the message.</returns>
		// Token: 0x17000AE9 RID: 2793
		// (get) Token: 0x06002CEA RID: 11498 RVA: 0x000D0D62 File Offset: 0x000CEF62
		// (set) Token: 0x06002CEB RID: 11499 RVA: 0x000D0D6A File Offset: 0x000CEF6A
		public IntPtr LParam
		{
			get
			{
				return this.lparam;
			}
			set
			{
				this.lparam = value;
			}
		}

		/// <summary>Specifies the value that is returned to Windows in response to handling the message.</summary>
		/// <returns>The return value of the message.</returns>
		// Token: 0x17000AEA RID: 2794
		// (get) Token: 0x06002CEC RID: 11500 RVA: 0x000D0D73 File Offset: 0x000CEF73
		// (set) Token: 0x06002CED RID: 11501 RVA: 0x000D0D7B File Offset: 0x000CEF7B
		public IntPtr Result
		{
			get
			{
				return this.result;
			}
			set
			{
				this.result = value;
			}
		}

		/// <summary>Gets the <see cref="P:System.Windows.Forms.Message.LParam" /> value and converts the value to an object.</summary>
		/// <param name="cls">The type to use to create an instance. This type must be declared as a structure type. </param>
		/// <returns>An <see cref="T:System.Object" /> that represents an instance of the class specified by the <paramref name="cls" /> parameter, with the data from the <see cref="P:System.Windows.Forms.Message.LParam" /> field of the message.</returns>
		// Token: 0x06002CEE RID: 11502 RVA: 0x000D0D84 File Offset: 0x000CEF84
		public object GetLParam(Type cls)
		{
			return UnsafeNativeMethods.PtrToStructure(this.lparam, cls);
		}

		/// <summary>Creates a new <see cref="T:System.Windows.Forms.Message" />.</summary>
		/// <param name="hWnd">The window handle that the message is for. </param>
		/// <param name="msg">The message ID. </param>
		/// <param name="wparam">The message <paramref name="wparam" /> field. </param>
		/// <param name="lparam">The message <paramref name="lparam" /> field. </param>
		/// <returns>A <see cref="T:System.Windows.Forms.Message" /> that represents the message that was created.</returns>
		// Token: 0x06002CEF RID: 11503 RVA: 0x000D0D94 File Offset: 0x000CEF94
		public static Message Create(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			return new Message
			{
				hWnd = hWnd,
				msg = msg,
				wparam = wparam,
				lparam = lparam,
				result = IntPtr.Zero
			};
		}

		/// <summary>Determines whether the specified object is equal to the current object.</summary>
		/// <param name="o">The object to compare with the current object.</param>
		/// <returns>
		///     <see langword="true" /> if the specified object is equal to the current object; otherwise, <see langword="false" />.</returns>
		// Token: 0x06002CF0 RID: 11504 RVA: 0x000D0DD8 File Offset: 0x000CEFD8
		public override bool Equals(object o)
		{
			if (!(o is Message))
			{
				return false;
			}
			Message message = (Message)o;
			return this.hWnd == message.hWnd && this.msg == message.msg && this.wparam == message.wparam && this.lparam == message.lparam && this.result == message.result;
		}

		/// <summary>Determines whether two instances of <see cref="T:System.Windows.Forms.Message" /> are not equal. </summary>
		/// <param name="a">A <see cref="T:System.Windows.Forms.Message" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">A <see cref="T:System.Windows.Forms.Message" /> to compare to <paramref name="a" />.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="a" /> and <paramref name="b" /> do not represent the same <see cref="T:System.Windows.Forms.Message" />; otherwise, <see langword="false" />. </returns>
		// Token: 0x06002CF1 RID: 11505 RVA: 0x000D0E50 File Offset: 0x000CF050
		public static bool operator !=(Message a, Message b)
		{
			return !a.Equals(b);
		}

		/// <summary>Determines whether two instances of <see cref="T:System.Windows.Forms.Message" /> are equal. </summary>
		/// <param name="a">A <see cref="T:System.Windows.Forms.Message" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">A <see cref="T:System.Windows.Forms.Message" /> to compare to <paramref name="a" />.</param>
		/// <returns>
		///     <see langword="true" /> if <paramref name="a" /> and <paramref name="b" /> represent the same <see cref="T:System.Windows.Forms.Message" />; otherwise, <see langword="false" />. </returns>
		// Token: 0x06002CF2 RID: 11506 RVA: 0x000D0E68 File Offset: 0x000CF068
		public static bool operator ==(Message a, Message b)
		{
			return a.Equals(b);
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		// Token: 0x06002CF3 RID: 11507 RVA: 0x000D0E7D File Offset: 0x000CF07D
		public override int GetHashCode()
		{
			return (int)this.hWnd << 4 | this.msg;
		}

		/// <summary>Returns a <see cref="T:System.String" /> that represents the current <see cref="T:System.Windows.Forms.Message" />.</summary>
		/// <returns>A <see cref="T:System.String" /> that represents the current <see cref="T:System.Windows.Forms.Message" />.</returns>
		// Token: 0x06002CF4 RID: 11508 RVA: 0x000D0E94 File Offset: 0x000CF094
		public override string ToString()
		{
			bool flag = false;
			try
			{
				IntSecurity.UnmanagedCode.Demand();
				flag = true;
			}
			catch (SecurityException)
			{
			}
			if (flag)
			{
				return MessageDecoder.ToString(this);
			}
			return base.ToString();
		}

		// Token: 0x04001327 RID: 4903
		private IntPtr hWnd;

		// Token: 0x04001328 RID: 4904
		private int msg;

		// Token: 0x04001329 RID: 4905
		private IntPtr wparam;

		// Token: 0x0400132A RID: 4906
		private IntPtr lparam;

		// Token: 0x0400132B RID: 4907
		private IntPtr result;
	}
}
