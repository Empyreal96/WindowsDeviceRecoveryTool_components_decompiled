using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms.Layout;

namespace System.Windows.Forms
{
	/// <summary>Represents a Windows picture box control for displaying an image.</summary>
	// Token: 0x0200030A RID: 778
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[DefaultProperty("Image")]
	[DefaultBindingProperty("Image")]
	[Docking(DockingBehavior.Ask)]
	[Designer("System.Windows.Forms.Design.PictureBoxDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SRDescription("DescriptionPictureBox")]
	public class PictureBox : Control, ISupportInitialize
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Forms.PictureBox" /> class.</summary>
		// Token: 0x06002F1F RID: 12063 RVA: 0x000DA120 File Offset: 0x000D8320
		public PictureBox()
		{
			base.SetState2(2048, true);
			this.pictureBoxState = new BitVector32(12);
			base.SetStyle(ControlStyles.Opaque | ControlStyles.Selectable, false);
			base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
			this.TabStop = false;
			this.savedSize = base.Size;
		}

		/// <summary>Overrides the <see cref="P:System.Windows.Forms.Control.AllowDrop" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if drag-and-drop operations are allowed in the control; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06002F20 RID: 12064 RVA: 0x000B0BBD File Offset: 0x000AEDBD
		// (set) Token: 0x06002F21 RID: 12065 RVA: 0x000B0BC5 File Offset: 0x000AEDC5
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				base.AllowDrop = value;
			}
		}

		/// <summary>Indicates the border style for the control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle" /> enumeration values. The default is <see cref="F:System.Windows.Forms.BorderStyle.None" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.BorderStyle" /> values. </exception>
		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06002F22 RID: 12066 RVA: 0x000DA182 File Offset: 0x000D8382
		// (set) Token: 0x06002F23 RID: 12067 RVA: 0x000DA18C File Offset: 0x000D838C
		[DefaultValue(BorderStyle.None)]
		[SRCategory("CatAppearance")]
		[DispId(-504)]
		[SRDescription("PictureBoxBorderStyleDescr")]
		public BorderStyle BorderStyle
		{
			get
			{
				return this.borderStyle;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 2))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(BorderStyle));
				}
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					base.RecreateHandle();
					this.AdjustSize();
				}
			}
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x000DA1DC File Offset: 0x000D83DC
		private Uri CalculateUri(string path)
		{
			Uri result;
			try
			{
				result = new Uri(path);
			}
			catch (UriFormatException)
			{
				path = Path.GetFullPath(path);
				result = new Uri(path);
			}
			return result;
		}

		/// <summary>Cancels an asynchronous image load.</summary>
		// Token: 0x06002F25 RID: 12069 RVA: 0x000DA218 File Offset: 0x000D8418
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxCancelAsyncDescr")]
		public void CancelAsync()
		{
			this.pictureBoxState[2] = true;
		}

		/// <summary>Overrides the <see cref="P:System.Windows.Forms.Control.CausesValidation" /> property.</summary>
		/// <returns>
		///     <see langword="true" /> if the control causes validation to be performed on any controls requiring validation when it receives focus; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06002F26 RID: 12070 RVA: 0x000DA227 File Offset: 0x000D8427
		// (set) Token: 0x06002F27 RID: 12071 RVA: 0x000DA22F File Offset: 0x000D842F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		/// <summary>Overrides the <see cref="E:System.Windows.Forms.Control.CausesValidationChanged" /> property.</summary>
		// Token: 0x14000234 RID: 564
		// (add) Token: 0x06002F28 RID: 12072 RVA: 0x000DA238 File Offset: 0x000D8438
		// (remove) Token: 0x06002F29 RID: 12073 RVA: 0x000DA241 File Offset: 0x000D8441
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler CausesValidationChanged
		{
			add
			{
				base.CausesValidationChanged += value;
			}
			remove
			{
				base.CausesValidationChanged -= value;
			}
		}

		/// <summary>Overrides the <see cref="P:System.Windows.Forms.Control.CreateParams" /> property.</summary>
		/// <returns>A <see cref="T:System.Windows.Forms.CreateParams" /> that contains the required creation parameters when the handle to the control is created.</returns>
		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06002F2A RID: 12074 RVA: 0x000DA24C File Offset: 0x000D844C
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams createParams = base.CreateParams;
				BorderStyle borderStyle = this.borderStyle;
				if (borderStyle != BorderStyle.FixedSingle)
				{
					if (borderStyle == BorderStyle.Fixed3D)
					{
						createParams.ExStyle |= 512;
					}
				}
				else
				{
					createParams.Style |= 8388608;
				}
				return createParams;
			}
		}

		/// <summary>Gets a value indicating the mode for Input Method Editor (IME) for the <see cref="T:System.Windows.Forms.PictureBox" />.</summary>
		/// <returns>Always <see cref="F:System.Windows.Forms.ImeMode.Disable" />.</returns>
		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06002F2B RID: 12075 RVA: 0x0001BB93 File Offset: 0x00019D93
		protected override ImeMode DefaultImeMode
		{
			get
			{
				return ImeMode.Disable;
			}
		}

		/// <summary>Gets the default size of the control.</summary>
		/// <returns>The default <see cref="T:System.Drawing.Size" /> of the control.</returns>
		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06002F2C RID: 12076 RVA: 0x000DA296 File Offset: 0x000D8496
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 50);
			}
		}

		/// <summary>Gets or sets the image to display when an error occurs during the image-loading process or if the image load is canceled.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> to display if an error occurs during the image-loading process or if the image load is canceled.</returns>
		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06002F2D RID: 12077 RVA: 0x000DA2A4 File Offset: 0x000D84A4
		// (set) Token: 0x06002F2E RID: 12078 RVA: 0x000DA30C File Offset: 0x000D850C
		[SRCategory("CatAsynchronous")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("PictureBoxErrorImageDescr")]
		public Image ErrorImage
		{
			get
			{
				if (this.errorImage == null && this.pictureBoxState[8])
				{
					if (this.defaultErrorImage == null)
					{
						if (PictureBox.defaultErrorImageForThread == null)
						{
							PictureBox.defaultErrorImageForThread = new Bitmap(typeof(PictureBox), "ImageInError.bmp");
						}
						this.defaultErrorImage = PictureBox.defaultErrorImageForThread;
					}
					this.errorImage = this.defaultErrorImage;
				}
				return this.errorImage;
			}
			set
			{
				if (this.ErrorImage != value)
				{
					this.pictureBoxState[8] = false;
				}
				this.errorImage = value;
			}
		}

		/// <summary>Overrides the <see cref="P:System.Windows.Forms.Control.ForeColor" /> property.</summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor" /> property.</returns>
		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06002F2F RID: 12079 RVA: 0x00012082 File Offset: 0x00010282
		// (set) Token: 0x06002F30 RID: 12080 RVA: 0x0001208A File Offset: 0x0001028A
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PictureBox.ForeColor" /> property changes.</summary>
		// Token: 0x14000235 RID: 565
		// (add) Token: 0x06002F31 RID: 12081 RVA: 0x00052766 File Offset: 0x00050966
		// (remove) Token: 0x06002F32 RID: 12082 RVA: 0x0005276F File Offset: 0x0005096F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ForeColorChanged
		{
			add
			{
				base.ForeColorChanged += value;
			}
			remove
			{
				base.ForeColorChanged -= value;
			}
		}

		/// <summary>Gets or sets the font of the text displayed by the control.</summary>
		/// <returns>The <see cref="T:System.Drawing.Font" /> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont" /> property.</returns>
		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06002F33 RID: 12083 RVA: 0x00012071 File Offset: 0x00010271
		// (set) Token: 0x06002F34 RID: 12084 RVA: 0x00012079 File Offset: 0x00010279
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PictureBox.Font" /> property changes.</summary>
		// Token: 0x14000236 RID: 566
		// (add) Token: 0x06002F35 RID: 12085 RVA: 0x00052778 File Offset: 0x00050978
		// (remove) Token: 0x06002F36 RID: 12086 RVA: 0x00052781 File Offset: 0x00050981
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler FontChanged
		{
			add
			{
				base.FontChanged += value;
			}
			remove
			{
				base.FontChanged -= value;
			}
		}

		/// <summary>Gets or sets the image that is displayed by <see cref="T:System.Windows.Forms.PictureBox" />.</summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> to display.</returns>
		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06002F37 RID: 12087 RVA: 0x000DA32B File Offset: 0x000D852B
		// (set) Token: 0x06002F38 RID: 12088 RVA: 0x000DA333 File Offset: 0x000D8533
		[SRCategory("CatAppearance")]
		[Localizable(true)]
		[Bindable(true)]
		[SRDescription("PictureBoxImageDescr")]
		public Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.InstallNewImage(value, PictureBox.ImageInstallationType.DirectlySpecified);
			}
		}

		/// <summary>Gets or sets the path or URL for the image to display in the <see cref="T:System.Windows.Forms.PictureBox" />.</summary>
		/// <returns>The path or URL for the image to display in the <see cref="T:System.Windows.Forms.PictureBox" />.</returns>
		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06002F39 RID: 12089 RVA: 0x000DA33D File Offset: 0x000D853D
		// (set) Token: 0x06002F3A RID: 12090 RVA: 0x000DA348 File Offset: 0x000D8548
		[SRCategory("CatAsynchronous")]
		[Localizable(true)]
		[DefaultValue(null)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("PictureBoxImageLocationDescr")]
		public string ImageLocation
		{
			get
			{
				return this.imageLocation;
			}
			set
			{
				this.imageLocation = value;
				this.pictureBoxState[32] = !string.IsNullOrEmpty(this.imageLocation);
				if (string.IsNullOrEmpty(this.imageLocation) && this.imageInstallationType != PictureBox.ImageInstallationType.DirectlySpecified)
				{
					this.InstallNewImage(null, PictureBox.ImageInstallationType.DirectlySpecified);
				}
				if (this.WaitOnLoad && !this.pictureBoxState[64] && !string.IsNullOrEmpty(this.imageLocation))
				{
					this.Load();
				}
				base.Invalidate();
			}
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06002F3B RID: 12091 RVA: 0x000DA3C4 File Offset: 0x000D85C4
		private Rectangle ImageRectangle
		{
			get
			{
				return this.ImageRectangleFromSizeMode(this.sizeMode);
			}
		}

		// Token: 0x06002F3C RID: 12092 RVA: 0x000DA3D4 File Offset: 0x000D85D4
		private Rectangle ImageRectangleFromSizeMode(PictureBoxSizeMode mode)
		{
			Rectangle result = LayoutUtils.DeflateRect(base.ClientRectangle, base.Padding);
			if (this.image != null)
			{
				switch (mode)
				{
				case PictureBoxSizeMode.Normal:
				case PictureBoxSizeMode.AutoSize:
					result.Size = this.image.Size;
					break;
				case PictureBoxSizeMode.CenterImage:
					result.X += (result.Width - this.image.Width) / 2;
					result.Y += (result.Height - this.image.Height) / 2;
					result.Size = this.image.Size;
					break;
				case PictureBoxSizeMode.Zoom:
				{
					Size size = this.image.Size;
					float num = Math.Min((float)base.ClientRectangle.Width / (float)size.Width, (float)base.ClientRectangle.Height / (float)size.Height);
					result.Width = (int)((float)size.Width * num);
					result.Height = (int)((float)size.Height * num);
					result.X = (base.ClientRectangle.Width - result.Width) / 2;
					result.Y = (base.ClientRectangle.Height - result.Height) / 2;
					break;
				}
				}
			}
			return result;
		}

		/// <summary>Gets or sets the image displayed in the <see cref="T:System.Windows.Forms.PictureBox" /> control when the main image is loading.</summary>
		/// <returns>The <see cref="T:System.Drawing.Image" /> displayed in the picture box control when the main image is loading.</returns>
		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06002F3D RID: 12093 RVA: 0x000DA538 File Offset: 0x000D8738
		// (set) Token: 0x06002F3E RID: 12094 RVA: 0x000DA5A0 File Offset: 0x000D87A0
		[SRCategory("CatAsynchronous")]
		[Localizable(true)]
		[RefreshProperties(RefreshProperties.All)]
		[SRDescription("PictureBoxInitialImageDescr")]
		public Image InitialImage
		{
			get
			{
				if (this.initialImage == null && this.pictureBoxState[4])
				{
					if (this.defaultInitialImage == null)
					{
						if (PictureBox.defaultInitialImageForThread == null)
						{
							PictureBox.defaultInitialImageForThread = new Bitmap(typeof(PictureBox), "PictureBox.Loading.bmp");
						}
						this.defaultInitialImage = PictureBox.defaultInitialImageForThread;
					}
					this.initialImage = this.defaultInitialImage;
				}
				return this.initialImage;
			}
			set
			{
				if (this.InitialImage != value)
				{
					this.pictureBoxState[4] = false;
				}
				this.initialImage = value;
			}
		}

		// Token: 0x06002F3F RID: 12095 RVA: 0x000DA5C0 File Offset: 0x000D87C0
		private void InstallNewImage(Image value, PictureBox.ImageInstallationType installationType)
		{
			this.StopAnimate();
			this.image = value;
			LayoutTransaction.DoLayoutIf(this.AutoSize, this, this, PropertyNames.Image);
			this.Animate();
			if (installationType != PictureBox.ImageInstallationType.ErrorOrInitial)
			{
				this.AdjustSize();
			}
			this.imageInstallationType = installationType;
			base.Invalidate();
			CommonProperties.xClearPreferredSizeCache(this);
		}

		/// <summary>Gets or sets the Input Method Editor(IME) mode supported by this control.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode" /> values.</returns>
		// Token: 0x17000B6A RID: 2922
		// (get) Token: 0x06002F40 RID: 12096 RVA: 0x00011FE4 File Offset: 0x000101E4
		// (set) Token: 0x06002F41 RID: 12097 RVA: 0x00011FEC File Offset: 0x000101EC
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PictureBox.ImeMode" /> property changes.</summary>
		// Token: 0x14000237 RID: 567
		// (add) Token: 0x06002F42 RID: 12098 RVA: 0x0001BF2C File Offset: 0x0001A12C
		// (remove) Token: 0x06002F43 RID: 12099 RVA: 0x0001BF35 File Offset: 0x0001A135
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				base.ImeModeChanged += value;
			}
			remove
			{
				base.ImeModeChanged -= value;
			}
		}

		/// <summary>Displays the image specified by the <see cref="P:System.Windows.Forms.PictureBox.ImageLocation" /> property of the <see cref="T:System.Windows.Forms.PictureBox" />.</summary>
		/// <exception cref="T:System.InvalidOperationException">
		///         <see cref="P:System.Windows.Forms.PictureBox.ImageLocation" /> is <see langword="null" /> or an empty string.</exception>
		// Token: 0x06002F44 RID: 12100 RVA: 0x000DA610 File Offset: 0x000D8810
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxLoad0Descr")]
		public void Load()
		{
			if (this.imageLocation == null || this.imageLocation.Length == 0)
			{
				throw new InvalidOperationException(SR.GetString("PictureBoxNoImageLocation"));
			}
			this.pictureBoxState[32] = false;
			PictureBox.ImageInstallationType installationType = PictureBox.ImageInstallationType.FromUrl;
			Image value;
			try
			{
				this.DisposeImageStream();
				Uri uri = this.CalculateUri(this.imageLocation);
				if (uri.IsFile)
				{
					this.localImageStreamReader = new StreamReader(uri.LocalPath);
					value = Image.FromStream(this.localImageStreamReader.BaseStream);
				}
				else
				{
					using (WebClient webClient = new WebClient())
					{
						this.uriImageStream = webClient.OpenRead(uri.ToString());
						value = Image.FromStream(this.uriImageStream);
					}
				}
			}
			catch
			{
				if (!base.DesignMode)
				{
					throw;
				}
				value = this.ErrorImage;
				installationType = PictureBox.ImageInstallationType.ErrorOrInitial;
			}
			this.InstallNewImage(value, installationType);
		}

		/// <summary>Sets the <see cref="P:System.Windows.Forms.PictureBox.ImageLocation" /> to the specified URL and displays the image indicated.</summary>
		/// <param name="url">The path for the image to display in the <see cref="T:System.Windows.Forms.PictureBox" />.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///         <paramref name="url" /> is <see langword="null" /> or an empty string.</exception>
		/// <exception cref="T:System.Net.WebException">
		///         <paramref name="url" /> refers to an image on the Web that cannot be accessed.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="url" /> refers to a file that is not an image.</exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		///         <paramref name="url" /> refers to a file that does not exist.</exception>
		// Token: 0x06002F45 RID: 12101 RVA: 0x000DA700 File Offset: 0x000D8900
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxLoad1Descr")]
		public void Load(string url)
		{
			this.ImageLocation = url;
			this.Load();
		}

		/// <summary>Loads the image asynchronously.</summary>
		// Token: 0x06002F46 RID: 12102 RVA: 0x000DA710 File Offset: 0x000D8910
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxLoadAsync0Descr")]
		public void LoadAsync()
		{
			if (this.imageLocation == null || this.imageLocation.Length == 0)
			{
				throw new InvalidOperationException(SR.GetString("PictureBoxNoImageLocation"));
			}
			if (this.pictureBoxState[1])
			{
				return;
			}
			this.pictureBoxState[1] = true;
			if ((this.Image == null || this.imageInstallationType == PictureBox.ImageInstallationType.ErrorOrInitial) && this.InitialImage != null)
			{
				this.InstallNewImage(this.InitialImage, PictureBox.ImageInstallationType.ErrorOrInitial);
			}
			this.currentAsyncLoadOperation = AsyncOperationManager.CreateOperation(null);
			if (this.loadCompletedDelegate == null)
			{
				this.loadCompletedDelegate = new SendOrPostCallback(this.LoadCompletedDelegate);
				this.loadProgressDelegate = new SendOrPostCallback(this.LoadProgressDelegate);
				this.readBuffer = new byte[4096];
			}
			this.pictureBoxState[32] = false;
			this.pictureBoxState[2] = false;
			this.contentLength = -1;
			this.tempDownloadStream = new MemoryStream();
			WebRequest state = WebRequest.Create(this.CalculateUri(this.imageLocation));
			new WaitCallback(this.BeginGetResponseDelegate).BeginInvoke(state, null, null);
		}

		// Token: 0x06002F47 RID: 12103 RVA: 0x000DA820 File Offset: 0x000D8A20
		private void BeginGetResponseDelegate(object arg)
		{
			WebRequest webRequest = (WebRequest)arg;
			webRequest.BeginGetResponse(new AsyncCallback(this.GetResponseCallback), webRequest);
		}

		// Token: 0x06002F48 RID: 12104 RVA: 0x000DA848 File Offset: 0x000D8A48
		private void PostCompleted(Exception error, bool cancelled)
		{
			AsyncOperation asyncOperation = this.currentAsyncLoadOperation;
			this.currentAsyncLoadOperation = null;
			if (asyncOperation != null)
			{
				asyncOperation.PostOperationCompleted(this.loadCompletedDelegate, new AsyncCompletedEventArgs(error, cancelled, null));
			}
		}

		// Token: 0x06002F49 RID: 12105 RVA: 0x000DA87C File Offset: 0x000D8A7C
		private void LoadCompletedDelegate(object arg)
		{
			AsyncCompletedEventArgs asyncCompletedEventArgs = (AsyncCompletedEventArgs)arg;
			Image value = this.ErrorImage;
			PictureBox.ImageInstallationType installationType = PictureBox.ImageInstallationType.ErrorOrInitial;
			if (!asyncCompletedEventArgs.Cancelled && asyncCompletedEventArgs.Error == null)
			{
				try
				{
					value = Image.FromStream(this.tempDownloadStream);
					installationType = PictureBox.ImageInstallationType.FromUrl;
				}
				catch (Exception error)
				{
					asyncCompletedEventArgs = new AsyncCompletedEventArgs(error, false, null);
				}
			}
			if (!asyncCompletedEventArgs.Cancelled)
			{
				this.InstallNewImage(value, installationType);
			}
			this.tempDownloadStream = null;
			this.pictureBoxState[2] = false;
			this.pictureBoxState[1] = false;
			this.OnLoadCompleted(asyncCompletedEventArgs);
		}

		// Token: 0x06002F4A RID: 12106 RVA: 0x000DA910 File Offset: 0x000D8B10
		private void LoadProgressDelegate(object arg)
		{
			this.OnLoadProgressChanged((ProgressChangedEventArgs)arg);
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x000DA920 File Offset: 0x000D8B20
		private void GetResponseCallback(IAsyncResult result)
		{
			if (this.pictureBoxState[2])
			{
				this.PostCompleted(null, true);
				return;
			}
			try
			{
				WebRequest webRequest = (WebRequest)result.AsyncState;
				WebResponse webResponse = webRequest.EndGetResponse(result);
				this.contentLength = (int)webResponse.ContentLength;
				this.totalBytesRead = 0;
				Stream responseStream = webResponse.GetResponseStream();
				responseStream.BeginRead(this.readBuffer, 0, 4096, new AsyncCallback(this.ReadCallBack), responseStream);
			}
			catch (Exception error)
			{
				this.PostCompleted(error, false);
			}
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x000DA9B4 File Offset: 0x000D8BB4
		private void ReadCallBack(IAsyncResult result)
		{
			if (this.pictureBoxState[2])
			{
				this.PostCompleted(null, true);
				return;
			}
			Stream stream = (Stream)result.AsyncState;
			try
			{
				int num = stream.EndRead(result);
				if (num > 0)
				{
					this.totalBytesRead += num;
					this.tempDownloadStream.Write(this.readBuffer, 0, num);
					stream.BeginRead(this.readBuffer, 0, 4096, new AsyncCallback(this.ReadCallBack), stream);
					if (this.contentLength != -1)
					{
						int progressPercentage = (int)(100f * ((float)this.totalBytesRead / (float)this.contentLength));
						if (this.currentAsyncLoadOperation != null)
						{
							this.currentAsyncLoadOperation.Post(this.loadProgressDelegate, new ProgressChangedEventArgs(progressPercentage, null));
						}
					}
				}
				else
				{
					this.tempDownloadStream.Seek(0L, SeekOrigin.Begin);
					if (this.currentAsyncLoadOperation != null)
					{
						this.currentAsyncLoadOperation.Post(this.loadProgressDelegate, new ProgressChangedEventArgs(100, null));
					}
					this.PostCompleted(null, false);
					Stream stream2 = stream;
					stream = null;
					stream2.Close();
				}
			}
			catch (Exception error)
			{
				this.PostCompleted(error, false);
				if (stream != null)
				{
					stream.Close();
				}
			}
		}

		/// <summary>Loads the image at the specified location, asynchronously.</summary>
		/// <param name="url">The path for the image to display in the <see cref="T:System.Windows.Forms.PictureBox" />.</param>
		// Token: 0x06002F4D RID: 12109 RVA: 0x000DAAE0 File Offset: 0x000D8CE0
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxLoadAsync1Descr")]
		public void LoadAsync(string url)
		{
			this.ImageLocation = url;
			this.LoadAsync();
		}

		/// <summary>Occurs when the asynchronous image-load operation is completed, been canceled, or raised an exception.</summary>
		// Token: 0x14000238 RID: 568
		// (add) Token: 0x06002F4E RID: 12110 RVA: 0x000DAAEF File Offset: 0x000D8CEF
		// (remove) Token: 0x06002F4F RID: 12111 RVA: 0x000DAB02 File Offset: 0x000D8D02
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxLoadCompletedDescr")]
		public event AsyncCompletedEventHandler LoadCompleted
		{
			add
			{
				base.Events.AddHandler(PictureBox.loadCompletedKey, value);
			}
			remove
			{
				base.Events.RemoveHandler(PictureBox.loadCompletedKey, value);
			}
		}

		/// <summary>Occurs when the progress of an asynchronous image-loading operation has changed.</summary>
		// Token: 0x14000239 RID: 569
		// (add) Token: 0x06002F50 RID: 12112 RVA: 0x000DAB15 File Offset: 0x000D8D15
		// (remove) Token: 0x06002F51 RID: 12113 RVA: 0x000DAB28 File Offset: 0x000D8D28
		[SRCategory("CatAsynchronous")]
		[SRDescription("PictureBoxLoadProgressChangedDescr")]
		public event ProgressChangedEventHandler LoadProgressChanged
		{
			add
			{
				base.Events.AddHandler(PictureBox.loadProgressChangedKey, value);
			}
			remove
			{
				base.Events.RemoveHandler(PictureBox.loadProgressChangedKey, value);
			}
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x000DAB3B File Offset: 0x000D8D3B
		private void ResetInitialImage()
		{
			this.pictureBoxState[4] = true;
			this.initialImage = this.defaultInitialImage;
		}

		// Token: 0x06002F53 RID: 12115 RVA: 0x000DAB56 File Offset: 0x000D8D56
		private void ResetErrorImage()
		{
			this.pictureBoxState[8] = true;
			this.errorImage = this.defaultErrorImage;
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x000DAB71 File Offset: 0x000D8D71
		private void ResetImage()
		{
			this.InstallNewImage(null, PictureBox.ImageInstallationType.DirectlySpecified);
		}

		/// <summary>Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left languages.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RightToLeft" /> values.</returns>
		// Token: 0x17000B6B RID: 2923
		// (get) Token: 0x06002F55 RID: 12117 RVA: 0x000DAB7B File Offset: 0x000D8D7B
		// (set) Token: 0x06002F56 RID: 12118 RVA: 0x000BDC35 File Offset: 0x000BBE35
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PictureBox.RightToLeft" /> property changes.</summary>
		// Token: 0x1400023A RID: 570
		// (add) Token: 0x06002F57 RID: 12119 RVA: 0x000DAB83 File Offset: 0x000D8D83
		// (remove) Token: 0x06002F58 RID: 12120 RVA: 0x000DAB8C File Offset: 0x000D8D8C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				base.RightToLeftChanged += value;
			}
			remove
			{
				base.RightToLeftChanged -= value;
			}
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x000DAB95 File Offset: 0x000D8D95
		private bool ShouldSerializeInitialImage()
		{
			return !this.pictureBoxState[4];
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x000DABA6 File Offset: 0x000D8DA6
		private bool ShouldSerializeErrorImage()
		{
			return !this.pictureBoxState[8];
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x000DABB7 File Offset: 0x000D8DB7
		private bool ShouldSerializeImage()
		{
			return this.imageInstallationType == PictureBox.ImageInstallationType.DirectlySpecified && this.Image != null;
		}

		/// <summary>Indicates how the image is displayed.</summary>
		/// <returns>One of the <see cref="T:System.Windows.Forms.PictureBoxSizeMode" /> values. The default is <see cref="F:System.Windows.Forms.PictureBoxSizeMode.Normal" />.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.PictureBoxSizeMode" /> values. </exception>
		// Token: 0x17000B6C RID: 2924
		// (get) Token: 0x06002F5C RID: 12124 RVA: 0x000DABCC File Offset: 0x000D8DCC
		// (set) Token: 0x06002F5D RID: 12125 RVA: 0x000DABD4 File Offset: 0x000D8DD4
		[DefaultValue(PictureBoxSizeMode.Normal)]
		[SRCategory("CatBehavior")]
		[Localizable(true)]
		[SRDescription("PictureBoxSizeModeDescr")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public PictureBoxSizeMode SizeMode
		{
			get
			{
				return this.sizeMode;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PictureBoxSizeMode));
				}
				if (this.sizeMode != value)
				{
					if (value == PictureBoxSizeMode.AutoSize)
					{
						this.AutoSize = true;
						base.SetStyle(ControlStyles.FixedWidth | ControlStyles.FixedHeight, true);
					}
					if (value != PictureBoxSizeMode.AutoSize)
					{
						this.AutoSize = false;
						base.SetStyle(ControlStyles.FixedWidth | ControlStyles.FixedHeight, false);
						this.savedSize = base.Size;
					}
					this.sizeMode = value;
					this.AdjustSize();
					base.Invalidate();
					this.OnSizeModeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>Occurs when <see cref="P:System.Windows.Forms.PictureBox.SizeMode" /> changes.</summary>
		// Token: 0x1400023B RID: 571
		// (add) Token: 0x06002F5E RID: 12126 RVA: 0x000DAC62 File Offset: 0x000D8E62
		// (remove) Token: 0x06002F5F RID: 12127 RVA: 0x000DAC75 File Offset: 0x000D8E75
		[SRCategory("CatPropertyChanged")]
		[SRDescription("PictureBoxOnSizeModeChangedDescr")]
		public event EventHandler SizeModeChanged
		{
			add
			{
				base.Events.AddHandler(PictureBox.EVENT_SIZEMODECHANGED, value);
			}
			remove
			{
				base.Events.RemoveHandler(PictureBox.EVENT_SIZEMODECHANGED, value);
			}
		}

		/// <summary>Gets or sets a value that indicates whether the user can give the focus to this control by using the TAB key.</summary>
		/// <returns>
		///     <see langword="true" /> if the user can give the focus to the control by using the TAB key; otherwise, <see langword="false" />. The default is <see langword="true" />.</returns>
		// Token: 0x17000B6D RID: 2925
		// (get) Token: 0x06002F60 RID: 12128 RVA: 0x000AA115 File Offset: 0x000A8315
		// (set) Token: 0x06002F61 RID: 12129 RVA: 0x000AA11D File Offset: 0x000A831D
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PictureBox.TabStop" /> property changes.</summary>
		// Token: 0x1400023C RID: 572
		// (add) Token: 0x06002F62 RID: 12130 RVA: 0x000AA126 File Offset: 0x000A8326
		// (remove) Token: 0x06002F63 RID: 12131 RVA: 0x000AA12F File Offset: 0x000A832F
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		/// <summary>Gets or sets the tab index value.</summary>
		/// <returns>The tab index value.</returns>
		// Token: 0x17000B6E RID: 2926
		// (get) Token: 0x06002F64 RID: 12132 RVA: 0x000AA0F2 File Offset: 0x000A82F2
		// (set) Token: 0x06002F65 RID: 12133 RVA: 0x000AA0FA File Offset: 0x000A82FA
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PictureBox.TabIndex" /> property changes.</summary>
		// Token: 0x1400023D RID: 573
		// (add) Token: 0x06002F66 RID: 12134 RVA: 0x000AA103 File Offset: 0x000A8303
		// (remove) Token: 0x06002F67 RID: 12135 RVA: 0x000AA10C File Offset: 0x000A830C
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TabIndexChanged
		{
			add
			{
				base.TabIndexChanged += value;
			}
			remove
			{
				base.TabIndexChanged -= value;
			}
		}

		/// <summary>Gets or sets the text of the <see cref="T:System.Windows.Forms.PictureBox" />.</summary>
		/// <returns>The text of the <see cref="T:System.Windows.Forms.PictureBox" />.</returns>
		// Token: 0x17000B6F RID: 2927
		// (get) Token: 0x06002F68 RID: 12136 RVA: 0x0001BFA5 File Offset: 0x0001A1A5
		// (set) Token: 0x06002F69 RID: 12137 RVA: 0x0001BFAD File Offset: 0x0001A1AD
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Bindable(false)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>Occurs when the value of the <see cref="P:System.Windows.Forms.PictureBox.Text" /> property changes.</summary>
		// Token: 0x1400023E RID: 574
		// (add) Token: 0x06002F6A RID: 12138 RVA: 0x0003E435 File Offset: 0x0003C635
		// (remove) Token: 0x06002F6B RID: 12139 RVA: 0x0003E43E File Offset: 0x0003C63E
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		/// <summary>Overrides the <see cref="E:System.Windows.Forms.Control.Enter" /> property.</summary>
		// Token: 0x1400023F RID: 575
		// (add) Token: 0x06002F6C RID: 12140 RVA: 0x000DAC88 File Offset: 0x000D8E88
		// (remove) Token: 0x06002F6D RID: 12141 RVA: 0x000DAC91 File Offset: 0x000D8E91
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Enter
		{
			add
			{
				base.Enter += value;
			}
			remove
			{
				base.Enter -= value;
			}
		}

		/// <summary>Occurs when a key is released when the control has focus. </summary>
		// Token: 0x14000240 RID: 576
		// (add) Token: 0x06002F6E RID: 12142 RVA: 0x000B0E8C File Offset: 0x000AF08C
		// (remove) Token: 0x06002F6F RID: 12143 RVA: 0x000B0E95 File Offset: 0x000AF095
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyUp
		{
			add
			{
				base.KeyUp += value;
			}
			remove
			{
				base.KeyUp -= value;
			}
		}

		/// <summary>Occurs when a key is pressed when the control has focus.</summary>
		// Token: 0x14000241 RID: 577
		// (add) Token: 0x06002F70 RID: 12144 RVA: 0x000B0E9E File Offset: 0x000AF09E
		// (remove) Token: 0x06002F71 RID: 12145 RVA: 0x000B0EA7 File Offset: 0x000AF0A7
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyEventHandler KeyDown
		{
			add
			{
				base.KeyDown += value;
			}
			remove
			{
				base.KeyDown -= value;
			}
		}

		/// <summary>Occurs when a key is pressed when the control has focus.</summary>
		// Token: 0x14000242 RID: 578
		// (add) Token: 0x06002F72 RID: 12146 RVA: 0x000B0EB0 File Offset: 0x000AF0B0
		// (remove) Token: 0x06002F73 RID: 12147 RVA: 0x000B0EB9 File Offset: 0x000AF0B9
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event KeyPressEventHandler KeyPress
		{
			add
			{
				base.KeyPress += value;
			}
			remove
			{
				base.KeyPress -= value;
			}
		}

		/// <summary>Occurs when input focus leaves the <see cref="T:System.Windows.Forms.PictureBox" />. </summary>
		// Token: 0x14000243 RID: 579
		// (add) Token: 0x06002F74 RID: 12148 RVA: 0x000DAC9A File Offset: 0x000D8E9A
		// (remove) Token: 0x06002F75 RID: 12149 RVA: 0x000DACA3 File Offset: 0x000D8EA3
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler Leave
		{
			add
			{
				base.Leave += value;
			}
			remove
			{
				base.Leave -= value;
			}
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x000DACAC File Offset: 0x000D8EAC
		private void AdjustSize()
		{
			if (this.sizeMode == PictureBoxSizeMode.AutoSize)
			{
				base.Size = base.PreferredSize;
				return;
			}
			base.Size = this.savedSize;
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x000DACD0 File Offset: 0x000D8ED0
		private void Animate()
		{
			this.Animate(!base.DesignMode && base.Visible && base.Enabled && this.ParentInternal != null);
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x000DACFC File Offset: 0x000D8EFC
		private void StopAnimate()
		{
			this.Animate(false);
		}

		// Token: 0x06002F79 RID: 12153 RVA: 0x000DAD08 File Offset: 0x000D8F08
		private void Animate(bool animate)
		{
			if (animate != this.currentlyAnimating)
			{
				if (animate)
				{
					if (this.image != null)
					{
						ImageAnimator.Animate(this.image, new EventHandler(this.OnFrameChanged));
						this.currentlyAnimating = animate;
						return;
					}
				}
				else if (this.image != null)
				{
					ImageAnimator.StopAnimate(this.image, new EventHandler(this.OnFrameChanged));
					this.currentlyAnimating = animate;
				}
			}
		}

		/// <summary>Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.PictureBox" /> and optionally releases the managed resources.</summary>
		/// <param name="disposing">
		///       <see langword="true" /> to release managed and unmanaged resources; <see langword="false" /> to release unmanaged resources only.</param>
		// Token: 0x06002F7A RID: 12154 RVA: 0x000DAD6E File Offset: 0x000D8F6E
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.StopAnimate();
			}
			this.DisposeImageStream();
			base.Dispose(disposing);
		}

		// Token: 0x06002F7B RID: 12155 RVA: 0x000DAD86 File Offset: 0x000D8F86
		private void DisposeImageStream()
		{
			if (this.localImageStreamReader != null)
			{
				this.localImageStreamReader.Dispose();
				this.localImageStreamReader = null;
			}
			if (this.uriImageStream != null)
			{
				this.uriImageStream.Dispose();
				this.localImageStreamReader = null;
			}
		}

		// Token: 0x06002F7C RID: 12156 RVA: 0x000DADBC File Offset: 0x000D8FBC
		internal override Size GetPreferredSizeCore(Size proposedSize)
		{
			if (this.image == null)
			{
				return CommonProperties.GetSpecifiedBounds(this).Size;
			}
			Size sz = this.SizeFromClientSize(Size.Empty) + base.Padding.Size;
			return this.image.Size + sz;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002F7D RID: 12157 RVA: 0x000DAE10 File Offset: 0x000D9010
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.Animate();
		}

		// Token: 0x06002F7E RID: 12158 RVA: 0x000DAE20 File Offset: 0x000D9020
		private void OnFrameChanged(object o, EventArgs e)
		{
			if (base.Disposing || base.IsDisposed)
			{
				return;
			}
			if (base.InvokeRequired && base.IsHandleCreated)
			{
				object obj = this.internalSyncObject;
				lock (obj)
				{
					if (this.handleValid)
					{
						base.BeginInvoke(new EventHandler(this.OnFrameChanged), new object[]
						{
							o,
							e
						});
					}
					return;
				}
			}
			base.Invalidate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleDestroyed" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002F7F RID: 12159 RVA: 0x000DAEAC File Offset: 0x000D90AC
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnHandleDestroyed(EventArgs e)
		{
			object obj = this.internalSyncObject;
			lock (obj)
			{
				this.handleValid = false;
			}
			base.OnHandleDestroyed(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002F80 RID: 12160 RVA: 0x000DAEF4 File Offset: 0x000D90F4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnHandleCreated(EventArgs e)
		{
			object obj = this.internalSyncObject;
			lock (obj)
			{
				this.handleValid = true;
			}
			base.OnHandleCreated(e);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.PictureBox.LoadCompleted" /> event.</summary>
		/// <param name="e">An <see cref="T:System.ComponentModel.AsyncCompletedEventArgs" /> that contains the event data. </param>
		// Token: 0x06002F81 RID: 12161 RVA: 0x000DAF3C File Offset: 0x000D913C
		protected virtual void OnLoadCompleted(AsyncCompletedEventArgs e)
		{
			AsyncCompletedEventHandler asyncCompletedEventHandler = (AsyncCompletedEventHandler)base.Events[PictureBox.loadCompletedKey];
			if (asyncCompletedEventHandler != null)
			{
				asyncCompletedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.PictureBox.LoadProgressChanged" /> event.</summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.ProgressChangedEventArgs" /> that contains the event data.</param>
		// Token: 0x06002F82 RID: 12162 RVA: 0x000DAF6C File Offset: 0x000D916C
		protected virtual void OnLoadProgressChanged(ProgressChangedEventArgs e)
		{
			ProgressChangedEventHandler progressChangedEventHandler = (ProgressChangedEventHandler)base.Events[PictureBox.loadProgressChangedKey];
			if (progressChangedEventHandler != null)
			{
				progressChangedEventHandler(this, e);
			}
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.</summary>
		/// <param name="pe">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data. </param>
		// Token: 0x06002F83 RID: 12163 RVA: 0x000DAF9C File Offset: 0x000D919C
		protected override void OnPaint(PaintEventArgs pe)
		{
			if (this.pictureBoxState[32])
			{
				try
				{
					if (this.WaitOnLoad)
					{
						this.Load();
					}
					else
					{
						this.LoadAsync();
					}
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsCriticalException(ex))
					{
						throw;
					}
					this.image = this.ErrorImage;
				}
			}
			if (this.image != null)
			{
				this.Animate();
				ImageAnimator.UpdateFrames(this.Image);
				Rectangle rect = (this.imageInstallationType == PictureBox.ImageInstallationType.ErrorOrInitial) ? this.ImageRectangleFromSizeMode(PictureBoxSizeMode.CenterImage) : this.ImageRectangle;
				pe.Graphics.DrawImage(this.image, rect);
			}
			base.OnPaint(pe);
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
		// Token: 0x06002F84 RID: 12164 RVA: 0x000DB044 File Offset: 0x000D9244
		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			this.Animate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002F85 RID: 12165 RVA: 0x000DB053 File Offset: 0x000D9253
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			this.Animate();
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.Control.Resize" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		// Token: 0x06002F86 RID: 12166 RVA: 0x000DB062 File Offset: 0x000D9262
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (this.sizeMode == PictureBoxSizeMode.Zoom || this.sizeMode == PictureBoxSizeMode.StretchImage || this.sizeMode == PictureBoxSizeMode.CenterImage || this.BackgroundImage != null)
			{
				base.Invalidate();
			}
			this.savedSize = base.Size;
		}

		/// <summary>Raises the <see cref="E:System.Windows.Forms.PictureBox.SizeModeChanged" /> event.</summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data. </param>
		// Token: 0x06002F87 RID: 12167 RVA: 0x000DB0A0 File Offset: 0x000D92A0
		protected virtual void OnSizeModeChanged(EventArgs e)
		{
			EventHandler eventHandler = base.Events[PictureBox.EVENT_SIZEMODECHANGED] as EventHandler;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>Returns a string that represents the current <see cref="T:System.Windows.Forms.PictureBox" /> control.</summary>
		/// <returns>A string that represents the current <see cref="T:System.Windows.Forms.PictureBox" />. </returns>
		// Token: 0x06002F88 RID: 12168 RVA: 0x000DB0D0 File Offset: 0x000D92D0
		public override string ToString()
		{
			string str = base.ToString();
			return str + ", SizeMode: " + this.sizeMode.ToString("G");
		}

		/// <summary>Gets or sets a value indicating whether an image is loaded synchronously.</summary>
		/// <returns>
		///     <see langword="true" /> if an image-loading operation is completed synchronously; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000B70 RID: 2928
		// (get) Token: 0x06002F89 RID: 12169 RVA: 0x000DB104 File Offset: 0x000D9304
		// (set) Token: 0x06002F8A RID: 12170 RVA: 0x000DB113 File Offset: 0x000D9313
		[SRCategory("CatAsynchronous")]
		[Localizable(true)]
		[DefaultValue(false)]
		[SRDescription("PictureBoxWaitOnLoadDescr")]
		public bool WaitOnLoad
		{
			get
			{
				return this.pictureBoxState[16];
			}
			set
			{
				this.pictureBoxState[16] = value;
			}
		}

		/// <summary>Signals the object that initialization is starting.</summary>
		// Token: 0x06002F8B RID: 12171 RVA: 0x000DB123 File Offset: 0x000D9323
		void ISupportInitialize.BeginInit()
		{
			this.pictureBoxState[64] = true;
		}

		/// <summary>Signals to the object that initialization is complete.</summary>
		// Token: 0x06002F8C RID: 12172 RVA: 0x000DB133 File Offset: 0x000D9333
		void ISupportInitialize.EndInit()
		{
			if (this.ImageLocation != null && this.ImageLocation.Length != 0 && this.WaitOnLoad)
			{
				this.Load();
			}
			this.pictureBoxState[64] = false;
		}

		// Token: 0x04001D56 RID: 7510
		private BorderStyle borderStyle;

		// Token: 0x04001D57 RID: 7511
		private Image image;

		// Token: 0x04001D58 RID: 7512
		private PictureBoxSizeMode sizeMode;

		// Token: 0x04001D59 RID: 7513
		private Size savedSize;

		// Token: 0x04001D5A RID: 7514
		private bool currentlyAnimating;

		// Token: 0x04001D5B RID: 7515
		private AsyncOperation currentAsyncLoadOperation;

		// Token: 0x04001D5C RID: 7516
		private string imageLocation;

		// Token: 0x04001D5D RID: 7517
		private Image initialImage;

		// Token: 0x04001D5E RID: 7518
		private Image errorImage;

		// Token: 0x04001D5F RID: 7519
		private int contentLength;

		// Token: 0x04001D60 RID: 7520
		private int totalBytesRead;

		// Token: 0x04001D61 RID: 7521
		private MemoryStream tempDownloadStream;

		// Token: 0x04001D62 RID: 7522
		private const int readBlockSize = 4096;

		// Token: 0x04001D63 RID: 7523
		private byte[] readBuffer;

		// Token: 0x04001D64 RID: 7524
		private PictureBox.ImageInstallationType imageInstallationType;

		// Token: 0x04001D65 RID: 7525
		private SendOrPostCallback loadCompletedDelegate;

		// Token: 0x04001D66 RID: 7526
		private SendOrPostCallback loadProgressDelegate;

		// Token: 0x04001D67 RID: 7527
		private bool handleValid;

		// Token: 0x04001D68 RID: 7528
		private object internalSyncObject = new object();

		// Token: 0x04001D69 RID: 7529
		private Image defaultInitialImage;

		// Token: 0x04001D6A RID: 7530
		private Image defaultErrorImage;

		// Token: 0x04001D6B RID: 7531
		[ThreadStatic]
		private static Image defaultInitialImageForThread = null;

		// Token: 0x04001D6C RID: 7532
		[ThreadStatic]
		private static Image defaultErrorImageForThread = null;

		// Token: 0x04001D6D RID: 7533
		private static readonly object defaultInitialImageKey = new object();

		// Token: 0x04001D6E RID: 7534
		private static readonly object defaultErrorImageKey = new object();

		// Token: 0x04001D6F RID: 7535
		private static readonly object loadCompletedKey = new object();

		// Token: 0x04001D70 RID: 7536
		private static readonly object loadProgressChangedKey = new object();

		// Token: 0x04001D71 RID: 7537
		private const int PICTUREBOXSTATE_asyncOperationInProgress = 1;

		// Token: 0x04001D72 RID: 7538
		private const int PICTUREBOXSTATE_cancellationPending = 2;

		// Token: 0x04001D73 RID: 7539
		private const int PICTUREBOXSTATE_useDefaultInitialImage = 4;

		// Token: 0x04001D74 RID: 7540
		private const int PICTUREBOXSTATE_useDefaultErrorImage = 8;

		// Token: 0x04001D75 RID: 7541
		private const int PICTUREBOXSTATE_waitOnLoad = 16;

		// Token: 0x04001D76 RID: 7542
		private const int PICTUREBOXSTATE_needToLoadImageLocation = 32;

		// Token: 0x04001D77 RID: 7543
		private const int PICTUREBOXSTATE_inInitialization = 64;

		// Token: 0x04001D78 RID: 7544
		private BitVector32 pictureBoxState;

		// Token: 0x04001D79 RID: 7545
		private StreamReader localImageStreamReader;

		// Token: 0x04001D7A RID: 7546
		private Stream uriImageStream;

		// Token: 0x04001D7B RID: 7547
		private static readonly object EVENT_SIZEMODECHANGED = new object();

		// Token: 0x020006FF RID: 1791
		private enum ImageInstallationType
		{
			// Token: 0x04004041 RID: 16449
			DirectlySpecified,
			// Token: 0x04004042 RID: 16450
			ErrorOrInitial,
			// Token: 0x04004043 RID: 16451
			FromUrl
		}
	}
}
