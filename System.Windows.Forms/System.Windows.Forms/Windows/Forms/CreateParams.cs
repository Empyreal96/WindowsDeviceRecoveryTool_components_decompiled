using System;
using System.Text;

namespace System.Windows.Forms
{
	/// <summary>Encapsulates the information needed when creating a control.</summary>
	// Token: 0x02000164 RID: 356
	public class CreateParams
	{
		/// <summary>Gets or sets the name of the Windows class to derive the control from.</summary>
		/// <returns>The name of the Windows class to derive the control from.</returns>
		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x00039221 File Offset: 0x00037421
		// (set) Token: 0x06001037 RID: 4151 RVA: 0x00039229 File Offset: 0x00037429
		public string ClassName
		{
			get
			{
				return this.className;
			}
			set
			{
				this.className = value;
			}
		}

		/// <summary>Gets or sets the control's initial text.</summary>
		/// <returns>The control's initial text.</returns>
		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x00039232 File Offset: 0x00037432
		// (set) Token: 0x06001039 RID: 4153 RVA: 0x0003923A File Offset: 0x0003743A
		public string Caption
		{
			get
			{
				return this.caption;
			}
			set
			{
				this.caption = value;
			}
		}

		/// <summary>Gets or sets a bitwise combination of window style values.</summary>
		/// <returns>A bitwise combination of the window style values.</returns>
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x00039243 File Offset: 0x00037443
		// (set) Token: 0x0600103B RID: 4155 RVA: 0x0003924B File Offset: 0x0003744B
		public int Style
		{
			get
			{
				return this.style;
			}
			set
			{
				this.style = value;
			}
		}

		/// <summary>Gets or sets a bitwise combination of extended window style values.</summary>
		/// <returns>A bitwise combination of the extended window style values.</returns>
		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600103C RID: 4156 RVA: 0x00039254 File Offset: 0x00037454
		// (set) Token: 0x0600103D RID: 4157 RVA: 0x0003925C File Offset: 0x0003745C
		public int ExStyle
		{
			get
			{
				return this.exStyle;
			}
			set
			{
				this.exStyle = value;
			}
		}

		/// <summary>Gets or sets a bitwise combination of class style values.</summary>
		/// <returns>A bitwise combination of the class style values.</returns>
		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x00039265 File Offset: 0x00037465
		// (set) Token: 0x0600103F RID: 4159 RVA: 0x0003926D File Offset: 0x0003746D
		public int ClassStyle
		{
			get
			{
				return this.classStyle;
			}
			set
			{
				this.classStyle = value;
			}
		}

		/// <summary>Gets or sets the initial left position of the control.</summary>
		/// <returns>The numeric value that represents the initial left position of the control.</returns>
		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x00039276 File Offset: 0x00037476
		// (set) Token: 0x06001041 RID: 4161 RVA: 0x0003927E File Offset: 0x0003747E
		public int X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		/// <summary>Gets or sets the top position of the initial location of the control.</summary>
		/// <returns>The numeric value that represents the top position of the initial location of the control.</returns>
		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x00039287 File Offset: 0x00037487
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x0003928F File Offset: 0x0003748F
		public int Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		/// <summary>Gets or sets the initial width of the control.</summary>
		/// <returns>The numeric value that represents the initial width of the control.</returns>
		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001044 RID: 4164 RVA: 0x00039298 File Offset: 0x00037498
		// (set) Token: 0x06001045 RID: 4165 RVA: 0x000392A0 File Offset: 0x000374A0
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width = value;
			}
		}

		/// <summary>Gets or sets the initial height of the control.</summary>
		/// <returns>The numeric value that represents the initial height of the control.</returns>
		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001046 RID: 4166 RVA: 0x000392A9 File Offset: 0x000374A9
		// (set) Token: 0x06001047 RID: 4167 RVA: 0x000392B1 File Offset: 0x000374B1
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height = value;
			}
		}

		/// <summary>Gets or sets the control's parent.</summary>
		/// <returns>An <see cref="T:System.IntPtr" /> that contains the window handle of the control's parent.</returns>
		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001048 RID: 4168 RVA: 0x000392BA File Offset: 0x000374BA
		// (set) Token: 0x06001049 RID: 4169 RVA: 0x000392C2 File Offset: 0x000374C2
		public IntPtr Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;
			}
		}

		/// <summary>Gets or sets additional parameter information needed to create the control.</summary>
		/// <returns>The <see cref="T:System.Object" /> that holds additional parameter information needed to create the control.</returns>
		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x0600104A RID: 4170 RVA: 0x000392CB File Offset: 0x000374CB
		// (set) Token: 0x0600104B RID: 4171 RVA: 0x000392D3 File Offset: 0x000374D3
		public object Param
		{
			get
			{
				return this.param;
			}
			set
			{
				this.param = value;
			}
		}

		/// <summary>Returns a string that represents the current object.</summary>
		/// <returns>A string that represents the current object.</returns>
		// Token: 0x0600104C RID: 4172 RVA: 0x000392DC File Offset: 0x000374DC
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(64);
			stringBuilder.Append("CreateParams {'");
			stringBuilder.Append(this.className);
			stringBuilder.Append("', '");
			stringBuilder.Append(this.caption);
			stringBuilder.Append("', 0x");
			stringBuilder.Append(Convert.ToString(this.style, 16));
			stringBuilder.Append(", 0x");
			stringBuilder.Append(Convert.ToString(this.exStyle, 16));
			stringBuilder.Append(", {");
			stringBuilder.Append(this.x);
			stringBuilder.Append(", ");
			stringBuilder.Append(this.y);
			stringBuilder.Append(", ");
			stringBuilder.Append(this.width);
			stringBuilder.Append(", ");
			stringBuilder.Append(this.height);
			stringBuilder.Append("}");
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		// Token: 0x04000880 RID: 2176
		private string className;

		// Token: 0x04000881 RID: 2177
		private string caption;

		// Token: 0x04000882 RID: 2178
		private int style;

		// Token: 0x04000883 RID: 2179
		private int exStyle;

		// Token: 0x04000884 RID: 2180
		private int classStyle;

		// Token: 0x04000885 RID: 2181
		private int x;

		// Token: 0x04000886 RID: 2182
		private int y;

		// Token: 0x04000887 RID: 2183
		private int width;

		// Token: 0x04000888 RID: 2184
		private int height;

		// Token: 0x04000889 RID: 2185
		private IntPtr parent;

		// Token: 0x0400088A RID: 2186
		private object param;
	}
}
