using System;
using System.Drawing;

namespace System.Windows.Forms.ComponentModel.Com2Interop
{
	// Token: 0x020004AF RID: 1199
	internal class Com2PictureConverter : Com2DataTypeToManagedDataTypeConverter
	{
		// Token: 0x060050AB RID: 20651 RVA: 0x0014E278 File Offset: 0x0014C478
		public Com2PictureConverter(Com2PropertyDescriptor pd)
		{
			if (pd.DISPID == -522 || pd.Name.IndexOf("Icon") != -1)
			{
				this.pictureType = typeof(Icon);
			}
		}

		// Token: 0x170013E2 RID: 5090
		// (get) Token: 0x060050AC RID: 20652 RVA: 0x0014E2D6 File Offset: 0x0014C4D6
		public override Type ManagedType
		{
			get
			{
				return this.pictureType;
			}
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x0014E2E0 File Offset: 0x0014C4E0
		public override object ConvertNativeToManaged(object nativeValue, Com2PropertyDescriptor pd)
		{
			if (nativeValue == null)
			{
				return null;
			}
			UnsafeNativeMethods.IPicture picture = (UnsafeNativeMethods.IPicture)nativeValue;
			IntPtr handle = picture.GetHandle();
			if (this.lastManaged != null && handle == this.lastNativeHandle)
			{
				return this.lastManaged;
			}
			this.lastNativeHandle = handle;
			if (handle != IntPtr.Zero)
			{
				short num = picture.GetPictureType();
				if (num != 1)
				{
					if (num == 3)
					{
						this.pictureType = typeof(Icon);
						this.lastManaged = Icon.FromHandle(handle);
					}
				}
				else
				{
					this.pictureType = typeof(Bitmap);
					this.lastManaged = Image.FromHbitmap(handle);
				}
				this.pictureRef = new WeakReference(picture);
			}
			else
			{
				this.lastManaged = null;
				this.pictureRef = null;
			}
			return this.lastManaged;
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x0014E39C File Offset: 0x0014C59C
		public override object ConvertManagedToNative(object managedValue, Com2PropertyDescriptor pd, ref bool cancelSet)
		{
			cancelSet = false;
			if (this.lastManaged != null && this.lastManaged.Equals(managedValue) && this.pictureRef != null && this.pictureRef.IsAlive)
			{
				return this.pictureRef.Target;
			}
			this.lastManaged = managedValue;
			if (managedValue != null)
			{
				Guid guid = typeof(UnsafeNativeMethods.IPicture).GUID;
				NativeMethods.PICTDESC pictdesc = null;
				bool fOwn = false;
				if (this.lastManaged is Icon)
				{
					pictdesc = NativeMethods.PICTDESC.CreateIconPICTDESC(((Icon)this.lastManaged).Handle);
				}
				else if (this.lastManaged is Bitmap)
				{
					pictdesc = NativeMethods.PICTDESC.CreateBitmapPICTDESC(((Bitmap)this.lastManaged).GetHbitmap(), this.lastPalette);
					fOwn = true;
				}
				UnsafeNativeMethods.IPicture picture = UnsafeNativeMethods.OleCreatePictureIndirect(pictdesc, ref guid, fOwn);
				this.lastNativeHandle = picture.GetHandle();
				this.pictureRef = new WeakReference(picture);
				return picture;
			}
			this.lastManaged = null;
			this.lastNativeHandle = (this.lastPalette = IntPtr.Zero);
			this.pictureRef = null;
			return null;
		}

		// Token: 0x04003418 RID: 13336
		private object lastManaged;

		// Token: 0x04003419 RID: 13337
		private IntPtr lastNativeHandle;

		// Token: 0x0400341A RID: 13338
		private WeakReference pictureRef;

		// Token: 0x0400341B RID: 13339
		private IntPtr lastPalette = IntPtr.Zero;

		// Token: 0x0400341C RID: 13340
		private Type pictureType = typeof(Bitmap);
	}
}
