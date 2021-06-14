using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x02000419 RID: 1049
	internal class TextServicesPropertyRanges
	{
		// Token: 0x06003C90 RID: 15504 RVA: 0x00118042 File Offset: 0x00116242
		internal TextServicesPropertyRanges(TextStore textstore, Guid guid)
		{
			this._guid = guid;
			this._textstore = textstore;
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x00002137 File Offset: 0x00000337
		[SecurityCritical]
		internal virtual void OnRange(UnsafeNativeMethods.ITfProperty property, int ecReadonly, UnsafeNativeMethods.ITfRange range)
		{
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x00118058 File Offset: 0x00116258
		[SecurityCritical]
		internal virtual void OnEndEdit(UnsafeNativeMethods.ITfContext context, int ecReadOnly, UnsafeNativeMethods.ITfEditRecord editRecord)
		{
			UnsafeNativeMethods.ITfProperty tfProperty = null;
			UnsafeNativeMethods.IEnumTfRanges propertyUpdate = this.GetPropertyUpdate(editRecord);
			UnsafeNativeMethods.ITfRange[] array = new UnsafeNativeMethods.ITfRange[1];
			int num;
			while (propertyUpdate.Next(1, array, out num) == 0)
			{
				ITextPointer textPointer;
				ITextPointer textPointer2;
				this.ConvertToTextPosition(array[0], out textPointer, out textPointer2);
				if (tfProperty == null)
				{
					context.GetProperty(ref this._guid, out tfProperty);
				}
				UnsafeNativeMethods.IEnumTfRanges enumTfRanges;
				if (tfProperty.EnumRanges(ecReadOnly, out enumTfRanges, array[0]) == 0)
				{
					UnsafeNativeMethods.ITfRange[] array2 = new UnsafeNativeMethods.ITfRange[1];
					while (enumTfRanges.Next(1, array2, out num) == 0)
					{
						this.OnRange(tfProperty, ecReadOnly, array2[0]);
						Marshal.ReleaseComObject(array2[0]);
					}
					Marshal.ReleaseComObject(enumTfRanges);
				}
				Marshal.ReleaseComObject(array[0]);
			}
			Marshal.ReleaseComObject(propertyUpdate);
			if (tfProperty != null)
			{
				Marshal.ReleaseComObject(tfProperty);
			}
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x00118100 File Offset: 0x00116300
		[SecurityCritical]
		protected void ConvertToTextPosition(UnsafeNativeMethods.ITfRange range, out ITextPointer start, out ITextPointer end)
		{
			UnsafeNativeMethods.ITfRangeACP tfRangeACP = range as UnsafeNativeMethods.ITfRangeACP;
			int num;
			int num2;
			tfRangeACP.GetExtent(out num, out num2);
			if (num2 < 0)
			{
				start = null;
				end = null;
				return;
			}
			start = this._textstore.CreatePointerAtCharOffset(num, LogicalDirection.Forward);
			end = this._textstore.CreatePointerAtCharOffset(num + num2, LogicalDirection.Forward);
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x0011814C File Offset: 0x0011634C
		[SecurityCritical]
		[UIPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		protected static object GetValue(int ecReadOnly, UnsafeNativeMethods.ITfProperty property, UnsafeNativeMethods.ITfRange range)
		{
			if (property == null)
			{
				return null;
			}
			object result;
			property.GetValue(ecReadOnly, range, out result);
			return result;
		}

		// Token: 0x06003C95 RID: 15509 RVA: 0x0011816C File Offset: 0x0011636C
		[SecurityCritical]
		private unsafe UnsafeNativeMethods.IEnumTfRanges GetPropertyUpdate(UnsafeNativeMethods.ITfEditRecord editRecord)
		{
			UnsafeNativeMethods.IEnumTfRanges result;
			fixed (Guid* ptr = &this._guid)
			{
				IntPtr intPtr = (IntPtr)((void*)ptr);
				editRecord.GetTextAndPropertyUpdates(0, ref intPtr, 1, out result);
			}
			return result;
		}

		// Token: 0x17000EFE RID: 3838
		// (get) Token: 0x06003C96 RID: 15510 RVA: 0x00118198 File Offset: 0x00116398
		protected Guid Guid
		{
			get
			{
				return this._guid;
			}
		}

		// Token: 0x17000EFF RID: 3839
		// (get) Token: 0x06003C97 RID: 15511 RVA: 0x001181A0 File Offset: 0x001163A0
		protected TextStore TextStore
		{
			get
			{
				return this._textstore;
			}
		}

		// Token: 0x0400262D RID: 9773
		private Guid _guid;

		// Token: 0x0400262E RID: 9774
		private TextStore _textstore;
	}
}
