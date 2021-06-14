using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	/// <summary>Represents a <see cref="T:System.Windows.Forms.TextBox" /> control that is hosted in a <see cref="T:System.Windows.Forms.DataGridTextBoxColumn" />.</summary>
	// Token: 0x0200017C RID: 380
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ToolboxItem(false)]
	[DesignTimeVisible(false)]
	[DefaultProperty("GridEditName")]
	public class DataGridTextBox : TextBox
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.DataGridTextBox" /> class. </summary>
		// Token: 0x060014EF RID: 5359 RVA: 0x0004ECC2 File Offset: 0x0004CEC2
		public DataGridTextBox()
		{
			base.TabStop = false;
		}

		/// <summary>Sets the <see cref="T:System.Windows.Forms.DataGrid" /> to which this <see cref="T:System.Windows.Forms.TextBox" /> control belongs.</summary>
		/// <param name="parentGrid">The <see cref="T:System.Windows.Forms.DataGrid" /> control that hosts the control. </param>
		// Token: 0x060014F0 RID: 5360 RVA: 0x0004ECD8 File Offset: 0x0004CED8
		public void SetDataGrid(DataGrid parentGrid)
		{
			this.dataGrid = parentGrid;
		}

		/// <summary>Raises the <see cref="M:System.Windows.Forms.Control.WndProc(System.Windows.Forms.Message@)" /> event.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" /> that contains the event data. </param>
		// Token: 0x060014F1 RID: 5361 RVA: 0x0004ECE4 File Offset: 0x0004CEE4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 770 || m.Msg == 768 || m.Msg == 771)
			{
				this.IsInEditOrNavigateMode = false;
				this.dataGrid.ColumnStartedEditing(base.Bounds);
			}
			base.WndProc(ref m);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data. </param>
		// Token: 0x060014F2 RID: 5362 RVA: 0x0004ED37 File Offset: 0x0004CF37
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			this.dataGrid.TextBoxOnMouseWheel(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.KeyPress" /> event.</summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyPressEventArgs" /> that contains the event data. </param>
		// Token: 0x060014F3 RID: 5363 RVA: 0x0004ED48 File Offset: 0x0004CF48
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if (e.KeyChar == ' ' && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
			{
				return;
			}
			if (base.ReadOnly)
			{
				return;
			}
			if ((Control.ModifierKeys & Keys.Control) == Keys.Control && (Control.ModifierKeys & Keys.Alt) == Keys.None)
			{
				return;
			}
			this.IsInEditOrNavigateMode = false;
			this.dataGrid.ColumnStartedEditing(base.Bounds);
		}

		/// <summary>Indicates whether the key pressed is processed further (for example, to navigate, or escape). This property is read-only.</summary>
		/// <param name="m">A <see cref="T:System.Windows.Forms.Message" />, passed by reference, that contains the key data. </param>
		/// <returns>
		///     <see langword="true" /> if the key is consumed, <see langword="false" /> to if the key is further processed.</returns>
		// Token: 0x060014F4 RID: 5364 RVA: 0x0004EDBC File Offset: 0x0004CFBC
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected internal override bool ProcessKeyMessage(ref Message m)
		{
			Keys keys = (Keys)((long)m.WParam);
			Keys modifierKeys = Control.ModifierKeys;
			if ((keys | modifierKeys) == Keys.Return || (keys | modifierKeys) == Keys.Escape || (keys | modifierKeys) == (Keys.LButton | Keys.MButton | Keys.Back | Keys.Control))
			{
				return m.Msg == 258 || this.ProcessKeyPreview(ref m);
			}
			if (m.Msg == 258)
			{
				return keys == Keys.LineFeed || this.ProcessKeyEventArgs(ref m);
			}
			if (m.Msg == 257)
			{
				return true;
			}
			Keys keys2 = keys & Keys.KeyCode;
			if (keys2 <= Keys.Add)
			{
				if (keys2 <= Keys.Delete)
				{
					if (keys2 != Keys.Tab)
					{
						switch (keys2)
						{
						case Keys.Space:
							if (this.IsInEditOrNavigateMode && (Control.ModifierKeys & Keys.Shift) == Keys.Shift)
							{
								return m.Msg == 258 || this.ProcessKeyPreview(ref m);
							}
							return this.ProcessKeyEventArgs(ref m);
						case Keys.Prior:
						case Keys.Next:
							break;
						case Keys.End:
						case Keys.Home:
							if (this.SelectionLength == this.Text.Length)
							{
								return this.ProcessKeyPreview(ref m);
							}
							return this.ProcessKeyEventArgs(ref m);
						case Keys.Left:
							if (base.SelectionStart + this.SelectionLength == 0 || (this.IsInEditOrNavigateMode && this.SelectionLength == this.Text.Length))
							{
								return this.ProcessKeyPreview(ref m);
							}
							return this.ProcessKeyEventArgs(ref m);
						case Keys.Up:
							if (this.Text.IndexOf("\r\n") < 0 || base.SelectionStart + this.SelectionLength < this.Text.IndexOf("\r\n"))
							{
								return this.ProcessKeyPreview(ref m);
							}
							return this.ProcessKeyEventArgs(ref m);
						case Keys.Right:
							if (base.SelectionStart + this.SelectionLength == this.Text.Length)
							{
								return this.ProcessKeyPreview(ref m);
							}
							return this.ProcessKeyEventArgs(ref m);
						case Keys.Down:
						{
							int startIndex = base.SelectionStart + this.SelectionLength;
							if (this.Text.IndexOf("\r\n", startIndex) == -1)
							{
								return this.ProcessKeyPreview(ref m);
							}
							return this.ProcessKeyEventArgs(ref m);
						}
						case Keys.Select:
						case Keys.Print:
						case Keys.Execute:
						case Keys.Snapshot:
						case Keys.Insert:
							goto IL_317;
						case Keys.Delete:
							if (!this.IsInEditOrNavigateMode)
							{
								return this.ProcessKeyEventArgs(ref m);
							}
							if (this.ProcessKeyPreview(ref m))
							{
								return true;
							}
							this.IsInEditOrNavigateMode = false;
							this.dataGrid.ColumnStartedEditing(base.Bounds);
							return this.ProcessKeyEventArgs(ref m);
						default:
							goto IL_317;
						}
					}
					else
					{
						if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
						{
							return this.ProcessKeyPreview(ref m);
						}
						return this.ProcessKeyEventArgs(ref m);
					}
				}
				else if (keys2 != Keys.A)
				{
					if (keys2 != Keys.Add)
					{
						goto IL_317;
					}
				}
				else
				{
					if (this.IsInEditOrNavigateMode && (Control.ModifierKeys & Keys.Control) == Keys.Control)
					{
						return m.Msg == 258 || this.ProcessKeyPreview(ref m);
					}
					return this.ProcessKeyEventArgs(ref m);
				}
			}
			else if (keys2 <= Keys.F2)
			{
				if (keys2 != Keys.Subtract)
				{
					if (keys2 != Keys.F2)
					{
						goto IL_317;
					}
					this.IsInEditOrNavigateMode = false;
					base.SelectionStart = this.Text.Length;
					return true;
				}
			}
			else if (keys2 != Keys.Oemplus && keys2 != Keys.OemMinus)
			{
				goto IL_317;
			}
			if (this.IsInEditOrNavigateMode)
			{
				return this.ProcessKeyPreview(ref m);
			}
			return this.ProcessKeyEventArgs(ref m);
			IL_317:
			return this.ProcessKeyEventArgs(ref m);
		}

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.DataGridTextBox" /> is in a mode that allows either editing or navigating.</summary>
		/// <returns>
		///     <see langword="true" /> if the controls is in navigation mode, and editing has not begun; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x060014F5 RID: 5365 RVA: 0x0004F0E7 File Offset: 0x0004D2E7
		// (set) Token: 0x060014F6 RID: 5366 RVA: 0x0004F0EF File Offset: 0x0004D2EF
		public bool IsInEditOrNavigateMode
		{
			get
			{
				return this.isInEditOrNavigateMode;
			}
			set
			{
				this.isInEditOrNavigateMode = value;
				if (value)
				{
					base.SelectAll();
				}
			}
		}

		// Token: 0x04000A16 RID: 2582
		private bool isInEditOrNavigateMode = true;

		// Token: 0x04000A17 RID: 2583
		private DataGrid dataGrid;
	}
}
