using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace System.Drawing
{
	/// <summary>Allows you to specify an icon to represent a control in a container, such as the Microsoft Visual Studio Form Designer.</summary>
	// Token: 0x02000038 RID: 56
	[AttributeUsage(AttributeTargets.Class)]
	public class ToolboxBitmapAttribute : Attribute
	{
		/// <summary>Initializes a new <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object with an image from a specified file.</summary>
		/// <param name="imageFile">The name of a file that contains a 16 by 16 bitmap. </param>
		// Token: 0x06000586 RID: 1414 RVA: 0x000182A2 File Offset: 0x000164A2
		public ToolboxBitmapAttribute(string imageFile) : this(ToolboxBitmapAttribute.GetImageFromFile(imageFile, false, true), ToolboxBitmapAttribute.GetImageFromFile(imageFile, true, true))
		{
			this.imageFile = imageFile;
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object based on a 16 x 16 bitmap that is embedded as a resource in a specified assembly.</summary>
		/// <param name="t">A <see cref="T:System.Type" /> whose defining assembly is searched for the bitmap resource. </param>
		// Token: 0x06000587 RID: 1415 RVA: 0x000182C1 File Offset: 0x000164C1
		public ToolboxBitmapAttribute(Type t) : this(ToolboxBitmapAttribute.GetImageFromResource(t, null, false), ToolboxBitmapAttribute.GetImageFromResource(t, null, true))
		{
			this.imageType = t;
		}

		/// <summary>Initializes a new <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object based on a 16 by 16 bitmap that is embedded as a resource in a specified assembly.</summary>
		/// <param name="t">A <see cref="T:System.Type" /> whose defining assembly is searched for the bitmap resource. </param>
		/// <param name="name">The name of the embedded bitmap resource. </param>
		// Token: 0x06000588 RID: 1416 RVA: 0x000182E0 File Offset: 0x000164E0
		public ToolboxBitmapAttribute(Type t, string name) : this(ToolboxBitmapAttribute.GetImageFromResource(t, name, false), ToolboxBitmapAttribute.GetImageFromResource(t, name, true))
		{
			this.imageType = t;
			this.imageName = name;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x00018306 File Offset: 0x00016506
		private ToolboxBitmapAttribute(Image smallImage, Image largeImage)
		{
			this.smallImage = smallImage;
			this.largeImage = largeImage;
		}

		/// <summary>Indicates whether the specified object is a <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object and is identical to this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</summary>
		/// <param name="value">The <see cref="T:System.Object" /> to test. </param>
		/// <returns>This method returns <see langword="true" /> if <paramref name="value" /> is both a <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object and is identical to this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</returns>
		// Token: 0x0600058A RID: 1418 RVA: 0x0001831C File Offset: 0x0001651C
		public override bool Equals(object value)
		{
			if (value == this)
			{
				return true;
			}
			ToolboxBitmapAttribute toolboxBitmapAttribute = value as ToolboxBitmapAttribute;
			return toolboxBitmapAttribute != null && toolboxBitmapAttribute.smallImage == this.smallImage && toolboxBitmapAttribute.largeImage == this.largeImage;
		}

		/// <summary>Gets a hash code for this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</summary>
		/// <returns>The hash code for this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</returns>
		// Token: 0x0600058B RID: 1419 RVA: 0x00018359 File Offset: 0x00016559
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Gets the small <see cref="T:System.Drawing.Image" /> associated with this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</summary>
		/// <param name="component">If this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object does not already have a small image, this method searches for a bitmap resource in the assembly that defines the type of the object specified by the component parameter. For example, if you pass an object of type ControlA to the component parameter, then this method searches the assembly that defines ControlA. </param>
		/// <returns>The small <see cref="T:System.Drawing.Image" /> associated with this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</returns>
		// Token: 0x0600058C RID: 1420 RVA: 0x00018361 File Offset: 0x00016561
		public Image GetImage(object component)
		{
			return this.GetImage(component, true);
		}

		/// <summary>Gets the small or large <see cref="T:System.Drawing.Image" /> associated with this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</summary>
		/// <param name="component">If this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object does not already have a small image, this method searches for a bitmap resource in the assembly that defines the type of the object specified by the component parameter. For example, if you pass an object of type ControlA to the component parameter, then this method searches the assembly that defines ControlA. </param>
		/// <param name="large">Specifies whether this method returns a large image (<see langword="true" />) or a small image (<see langword="false" />). The small image is 16 by 16, and the large image is 32 by 32. </param>
		/// <returns>An <see cref="T:System.Drawing.Image" /> object associated with this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</returns>
		// Token: 0x0600058D RID: 1421 RVA: 0x0001836B File Offset: 0x0001656B
		public Image GetImage(object component, bool large)
		{
			if (component != null)
			{
				return this.GetImage(component.GetType(), large);
			}
			return null;
		}

		/// <summary>Gets the small <see cref="T:System.Drawing.Image" /> associated with this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</summary>
		/// <param name="type">If this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object does not already have a small image, this method searches for a bitmap resource in the assembly that defines the type specified by the type parameter. For example, if you pass typeof(ControlA) to the type parameter, then this method searches the assembly that defines ControlA. </param>
		/// <returns>The small <see cref="T:System.Drawing.Image" /> associated with this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</returns>
		// Token: 0x0600058E RID: 1422 RVA: 0x0001837F File Offset: 0x0001657F
		public Image GetImage(Type type)
		{
			return this.GetImage(type, false);
		}

		/// <summary>Gets the small or large <see cref="T:System.Drawing.Image" /> associated with this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</summary>
		/// <param name="type">If this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object does not already have a small image, this method searches for a bitmap resource in the assembly that defines the type specified by the component type. For example, if you pass typeof(ControlA) to the type parameter, then this method searches the assembly that defines ControlA. </param>
		/// <param name="large">Specifies whether this method returns a large image (<see langword="true" />) or a small image (<see langword="false" />). The small image is 16 by 16, and the large image is 32 by 32. </param>
		/// <returns>An <see cref="T:System.Drawing.Image" /> associated with this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</returns>
		// Token: 0x0600058F RID: 1423 RVA: 0x00018389 File Offset: 0x00016589
		public Image GetImage(Type type, bool large)
		{
			return this.GetImage(type, null, large);
		}

		/// <summary>Gets the small or large <see cref="T:System.Drawing.Image" /> associated with this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</summary>
		/// <param name="type">If this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object does not already have a small image, this method searches for an embedded bitmap resource in the assembly that defines the type specified by the component type. For example, if you pass typeof(ControlA) to the type parameter, then this method searches the assembly that defines ControlA. </param>
		/// <param name="imgName">The name of the embedded bitmap resource. </param>
		/// <param name="large">Specifies whether this method returns a large image (<see langword="true" />) or a small image (<see langword="false" />). The small image is 16 by 16, and the large image is 32 by 32. </param>
		/// <returns>An <see cref="T:System.Drawing.Image" /> associated with this <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object.</returns>
		// Token: 0x06000590 RID: 1424 RVA: 0x00018394 File Offset: 0x00016594
		public Image GetImage(Type type, string imgName, bool large)
		{
			if ((large && this.largeImage == null) || (!large && this.smallImage == null))
			{
				Image image;
				if (large)
				{
					image = this.largeImage;
				}
				else
				{
					image = this.smallImage;
				}
				if (image == null)
				{
					image = ToolboxBitmapAttribute.GetImageFromResource(type, imgName, large);
				}
				if (large && this.largeImage == null && this.smallImage != null)
				{
					image = new Bitmap((Bitmap)this.smallImage, ToolboxBitmapAttribute.largeSize.Width, ToolboxBitmapAttribute.largeSize.Height);
				}
				Bitmap bitmap = image as Bitmap;
				if (bitmap != null)
				{
					ToolboxBitmapAttribute.MakeBackgroundAlphaZero(bitmap);
				}
				if (image == null)
				{
					image = ToolboxBitmapAttribute.DefaultComponent.GetImage(type, large);
				}
				if (large)
				{
					this.largeImage = image;
				}
				else
				{
					this.smallImage = image;
				}
			}
			Image result = large ? this.largeImage : this.smallImage;
			if (this.Equals(ToolboxBitmapAttribute.Default))
			{
				this.largeImage = null;
				this.smallImage = null;
			}
			return result;
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00018480 File Offset: 0x00016680
		internal Bitmap GetOriginalBitmap()
		{
			if (this.originalBitmap != null)
			{
				return this.originalBitmap;
			}
			if (this.smallImage == null)
			{
				return null;
			}
			if (!DpiHelper.IsScalingRequired)
			{
				return null;
			}
			if (!string.IsNullOrEmpty(this.imageFile))
			{
				this.originalBitmap = (ToolboxBitmapAttribute.GetImageFromFile(this.imageFile, false, false) as Bitmap);
			}
			else if (this.imageType != null)
			{
				this.originalBitmap = (ToolboxBitmapAttribute.GetImageFromResource(this.imageType, this.imageName, false, false) as Bitmap);
			}
			return this.originalBitmap;
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00018508 File Offset: 0x00016708
		private static Image GetIconFromStream(Stream stream, bool large, bool scaled)
		{
			if (stream == null)
			{
				return null;
			}
			Icon original = new Icon(stream);
			Icon icon = new Icon(original, large ? ToolboxBitmapAttribute.largeSize : ToolboxBitmapAttribute.smallSize);
			Bitmap result = icon.ToBitmap();
			if (DpiHelper.IsScalingRequired && scaled)
			{
				DpiHelper.ScaleBitmapLogicalToDevice(ref result, 0);
			}
			return result;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00018550 File Offset: 0x00016750
		private static string GetFileNameFromBitmapSelector(string originalName)
		{
			if (originalName != ToolboxBitmapAttribute.lastOriginalFileName)
			{
				ToolboxBitmapAttribute.lastOriginalFileName = originalName;
				ToolboxBitmapAttribute.lastUpdatedFileName = BitmapSelector.GetFileName(originalName);
			}
			return ToolboxBitmapAttribute.lastUpdatedFileName;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00018578 File Offset: 0x00016778
		private static Image GetImageFromFile(string imageFile, bool large, bool scaled = true)
		{
			Image image = null;
			try
			{
				if (imageFile != null)
				{
					imageFile = ToolboxBitmapAttribute.GetFileNameFromBitmapSelector(imageFile);
					string extension = Path.GetExtension(imageFile);
					if (extension != null && string.Equals(extension, ".ico", StringComparison.OrdinalIgnoreCase))
					{
						FileStream fileStream = File.Open(imageFile, FileMode.Open);
						if (fileStream == null)
						{
							goto IL_64;
						}
						try
						{
							image = ToolboxBitmapAttribute.GetIconFromStream(fileStream, large, scaled);
							goto IL_64;
						}
						finally
						{
							fileStream.Close();
						}
					}
					if (!large)
					{
						image = Image.FromFile(imageFile);
						Bitmap bitmap = image as Bitmap;
						if (DpiHelper.IsScalingRequired && scaled)
						{
							DpiHelper.ScaleBitmapLogicalToDevice(ref bitmap, 0);
						}
					}
				}
				IL_64:;
			}
			catch (Exception ex)
			{
				if (ClientUtils.IsCriticalException(ex))
				{
					throw;
				}
			}
			return image;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00018618 File Offset: 0x00016818
		private static Image GetBitmapFromResource(Type t, string bitmapname, bool large, bool scaled)
		{
			if (bitmapname == null)
			{
				return null;
			}
			Image image = null;
			Stream resourceStream = BitmapSelector.GetResourceStream(t, bitmapname);
			if (resourceStream != null)
			{
				Bitmap bitmap = new Bitmap(resourceStream);
				image = bitmap;
				ToolboxBitmapAttribute.MakeBackgroundAlphaZero(bitmap);
				if (large)
				{
					image = new Bitmap(bitmap, ToolboxBitmapAttribute.largeSize.Width, ToolboxBitmapAttribute.largeSize.Height);
				}
				if (DpiHelper.IsScalingRequired && scaled)
				{
					bitmap = (Bitmap)image;
					DpiHelper.ScaleBitmapLogicalToDevice(ref bitmap, 0);
					image = bitmap;
				}
			}
			return image;
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00018685 File Offset: 0x00016885
		private static Image GetIconFromResource(Type t, string bitmapname, bool large, bool scaled)
		{
			if (bitmapname == null)
			{
				return null;
			}
			return ToolboxBitmapAttribute.GetIconFromStream(BitmapSelector.GetResourceStream(t, bitmapname), large, scaled);
		}

		/// <summary>Returns an <see cref="T:System.Drawing.Image" /> object based on a bitmap resource that is embedded in an assembly.</summary>
		/// <param name="t">This method searches for an embedded bitmap resource in the assembly that defines the type specified by the t parameter. For example, if you pass typeof(ControlA) to the t parameter, then this method searches the assembly that defines ControlA. </param>
		/// <param name="imageName">The name of the embedded bitmap resource. </param>
		/// <param name="large">Specifies whether this method returns a large image (true)or a small image (false). The small image is 16 by 16, and the large image is 32 x 32. </param>
		/// <returns>An <see cref="T:System.Drawing.Image" /> object based on the retrieved bitmap.</returns>
		// Token: 0x06000597 RID: 1431 RVA: 0x0001869A File Offset: 0x0001689A
		public static Image GetImageFromResource(Type t, string imageName, bool large)
		{
			return ToolboxBitmapAttribute.GetImageFromResource(t, imageName, large, true);
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000186A8 File Offset: 0x000168A8
		internal static Image GetImageFromResource(Type t, string imageName, bool large, bool scaled)
		{
			Image image = null;
			try
			{
				string text = null;
				string text2 = null;
				string text3 = null;
				if (imageName == null)
				{
					string text4 = t.FullName;
					int num = text4.LastIndexOf('.');
					if (num != -1)
					{
						text4 = text4.Substring(num + 1);
					}
					text = text4 + ".ico";
					text2 = text4 + ".bmp";
				}
				else if (string.Compare(Path.GetExtension(imageName), ".ico", true, CultureInfo.CurrentCulture) == 0)
				{
					text = imageName;
				}
				else if (string.Compare(Path.GetExtension(imageName), ".bmp", true, CultureInfo.CurrentCulture) == 0)
				{
					text2 = imageName;
				}
				else
				{
					text3 = imageName;
					text2 = imageName + ".bmp";
					text = imageName + ".ico";
				}
				if (text3 != null)
				{
					image = ToolboxBitmapAttribute.GetBitmapFromResource(t, text3, large, scaled);
				}
				if (image == null && text2 != null)
				{
					image = ToolboxBitmapAttribute.GetBitmapFromResource(t, text2, large, scaled);
				}
				if (image == null && text != null)
				{
					image = ToolboxBitmapAttribute.GetIconFromResource(t, text, large, scaled);
				}
			}
			catch (Exception ex)
			{
				t == null;
			}
			return image;
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x000187A0 File Offset: 0x000169A0
		private static void MakeBackgroundAlphaZero(Bitmap img)
		{
			Color pixel = img.GetPixel(0, img.Height - 1);
			img.MakeTransparent();
			Color color = Color.FromArgb(0, pixel);
			img.SetPixel(0, img.Height - 1, color);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x000187DC File Offset: 0x000169DC
		static ToolboxBitmapAttribute()
		{
			SafeNativeMethods.Gdip.DummyFunction();
			Bitmap img = null;
			Stream resourceStream = BitmapSelector.GetResourceStream(typeof(ToolboxBitmapAttribute), "DefaultComponent.bmp");
			if (resourceStream != null)
			{
				img = new Bitmap(resourceStream);
				ToolboxBitmapAttribute.MakeBackgroundAlphaZero(img);
			}
			ToolboxBitmapAttribute.DefaultComponent = new ToolboxBitmapAttribute(img, null);
		}

		// Token: 0x04000327 RID: 807
		private Image smallImage;

		// Token: 0x04000328 RID: 808
		private Image largeImage;

		// Token: 0x04000329 RID: 809
		private Bitmap originalBitmap;

		// Token: 0x0400032A RID: 810
		private string imageFile;

		// Token: 0x0400032B RID: 811
		private Type imageType;

		// Token: 0x0400032C RID: 812
		private string imageName;

		// Token: 0x0400032D RID: 813
		private static readonly Size largeSize = new Size(32, 32);

		// Token: 0x0400032E RID: 814
		private static readonly Size smallSize = new Size(16, 16);

		// Token: 0x0400032F RID: 815
		private static string lastOriginalFileName;

		// Token: 0x04000330 RID: 816
		private static string lastUpdatedFileName;

		/// <summary>A <see cref="T:System.Drawing.ToolboxBitmapAttribute" /> object that has its small image and its large image set to <see langword="null" />.</summary>
		// Token: 0x04000331 RID: 817
		public static readonly ToolboxBitmapAttribute Default = new ToolboxBitmapAttribute(null, null);

		// Token: 0x04000332 RID: 818
		private static readonly ToolboxBitmapAttribute DefaultComponent;
	}
}
