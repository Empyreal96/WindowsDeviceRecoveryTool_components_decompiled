using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Windows.Forms
{
	/// <summary>Provides methods to place data on and retrieve data from the system Clipboard. This class cannot be inherited.</summary>
	// Token: 0x0200013F RID: 319
	public sealed class Clipboard
	{
		// Token: 0x06000A13 RID: 2579 RVA: 0x000027DB File Offset: 0x000009DB
		private Clipboard()
		{
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0001E6B9 File Offset: 0x0001C8B9
		private static bool IsFormatValid(DataObject data)
		{
			return Clipboard.IsFormatValid(data.GetFormats());
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0001E6C8 File Offset: 0x0001C8C8
		internal static bool IsFormatValid(string[] formats)
		{
			if (formats != null && formats.Length <= 4)
			{
				foreach (string a in formats)
				{
					if (!(a == "Text") && !(a == "UnicodeText") && !(a == "System.String") && !(a == "Csv"))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0001E72C File Offset: 0x0001C92C
		internal static bool IsFormatValid(FORMATETC[] formats)
		{
			if (formats != null && formats.Length <= 4)
			{
				for (int i = 0; i < formats.Length; i++)
				{
					short cfFormat = formats[i].cfFormat;
					if (cfFormat != 1 && cfFormat != 13 && (int)cfFormat != DataFormats.GetFormat("System.String").Id && (int)cfFormat != DataFormats.GetFormat("Csv").Id)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>Clears the Clipboard and then places nonpersistent data on it.</summary>
		/// <param name="data">The data to place on the Clipboard. </param>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">Data could not be placed on the Clipboard. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">The value of <paramref name="data" /> is <see langword="null" />. </exception>
		// Token: 0x06000A17 RID: 2583 RVA: 0x0001E78F File Offset: 0x0001C98F
		public static void SetDataObject(object data)
		{
			Clipboard.SetDataObject(data, false);
		}

		/// <summary>Clears the Clipboard and then places data on it and specifies whether the data should remain after the application exits.</summary>
		/// <param name="data">The data to place on the Clipboard. </param>
		/// <param name="copy">
		///       <see langword="true" /> if you want data to remain on the Clipboard after this application exits; otherwise, <see langword="false" />. </param>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">Data could not be placed on the Clipboard. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">The value of <paramref name="data" /> is <see langword="null" />. </exception>
		// Token: 0x06000A18 RID: 2584 RVA: 0x0001E798 File Offset: 0x0001C998
		public static void SetDataObject(object data, bool copy)
		{
			Clipboard.SetDataObject(data, copy, 10, 100);
		}

		/// <summary>Clears the Clipboard and then attempts to place data on it the specified number of times and with the specified delay between attempts, optionally leaving the data on the Clipboard after the application exits.</summary>
		/// <param name="data">The data to place on the Clipboard.</param>
		/// <param name="copy">
		///       <see langword="true" /> if you want data to remain on the Clipboard after this application exits; otherwise, <see langword="false" />.</param>
		/// <param name="retryTimes">The number of times to attempt placing the data on the Clipboard.</param>
		/// <param name="retryDelay">The number of milliseconds to pause between attempts. </param>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="data" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///         <paramref name="retryTimes" /> is less than zero.-or-
		///         <paramref name="retryDelay" /> is less than zero.</exception>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">Data could not be placed on the Clipboard. This typically occurs when the Clipboard is being used by another process.</exception>
		// Token: 0x06000A19 RID: 2585 RVA: 0x0001E7A8 File Offset: 0x0001C9A8
		[UIPermission(SecurityAction.Demand, Clipboard = UIPermissionClipboard.OwnClipboard)]
		public static void SetDataObject(object data, bool copy, int retryTimes, int retryDelay)
		{
			if (Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
			}
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			if (retryTimes < 0)
			{
				throw new ArgumentOutOfRangeException("retryTimes", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"retryTimes",
					retryTimes.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (retryDelay < 0)
			{
				throw new ArgumentOutOfRangeException("retryDelay", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"retryDelay",
					retryDelay.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			DataObject dataObject = null;
			if (!(data is IDataObject))
			{
				dataObject = new DataObject(data);
			}
			bool flag = false;
			try
			{
				IntSecurity.ClipboardRead.Demand();
			}
			catch (SecurityException)
			{
				flag = true;
			}
			if (flag)
			{
				if (dataObject == null)
				{
					dataObject = (data as DataObject);
				}
				if (!Clipboard.IsFormatValid(dataObject))
				{
					throw new SecurityException(SR.GetString("ClipboardSecurityException"));
				}
			}
			if (dataObject != null)
			{
				dataObject.RestrictedFormats = flag;
			}
			int num = retryTimes;
			IntSecurity.UnmanagedCode.Assert();
			try
			{
				int num2;
				do
				{
					if (data is IDataObject)
					{
						num2 = UnsafeNativeMethods.OleSetClipboard((IDataObject)data);
					}
					else
					{
						num2 = UnsafeNativeMethods.OleSetClipboard(dataObject);
					}
					if (num2 != 0)
					{
						if (num == 0)
						{
							Clipboard.ThrowIfFailed(num2);
						}
						num--;
						Thread.Sleep(retryDelay);
					}
				}
				while (num2 != 0);
				if (copy)
				{
					num = retryTimes;
					do
					{
						num2 = UnsafeNativeMethods.OleFlushClipboard();
						if (num2 != 0)
						{
							if (num == 0)
							{
								Clipboard.ThrowIfFailed(num2);
							}
							num--;
							Thread.Sleep(retryDelay);
						}
					}
					while (num2 != 0);
				}
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		/// <summary>Retrieves the data that is currently on the system Clipboard.</summary>
		/// <returns>An <see cref="T:System.Windows.Forms.IDataObject" /> that represents the data currently on the Clipboard, or <see langword="null" /> if there is no data on the Clipboard.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">Data could not be retrieved from the Clipboard. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode and the <see cref="P:System.Windows.Forms.Application.MessageLoop" /> property value is <see langword="true" />. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method. </exception>
		// Token: 0x06000A1A RID: 2586 RVA: 0x0001E948 File Offset: 0x0001CB48
		public static IDataObject GetDataObject()
		{
			IntSecurity.ClipboardRead.Demand();
			if (Application.OleRequired() == ApartmentState.STA)
			{
				return Clipboard.GetDataObject(10, 100);
			}
			if (Application.MessageLoop)
			{
				throw new ThreadStateException(SR.GetString("ThreadMustBeSTA"));
			}
			return null;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0001E980 File Offset: 0x0001CB80
		private static IDataObject GetDataObject(int retryTimes, int retryDelay)
		{
			IDataObject dataObject = null;
			int num = retryTimes;
			int num2;
			do
			{
				num2 = UnsafeNativeMethods.OleGetClipboard(ref dataObject);
				if (num2 != 0)
				{
					if (num == 0)
					{
						Clipboard.ThrowIfFailed(num2);
					}
					num--;
					Thread.Sleep(retryDelay);
				}
			}
			while (num2 != 0);
			if (dataObject == null)
			{
				return null;
			}
			if (dataObject is IDataObject && !Marshal.IsComObject(dataObject))
			{
				return (IDataObject)dataObject;
			}
			return new DataObject(dataObject);
		}

		/// <summary>Removes all data from the Clipboard.</summary>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A1C RID: 2588 RVA: 0x0001E9D4 File Offset: 0x0001CBD4
		public static void Clear()
		{
			Clipboard.SetDataObject(new DataObject());
		}

		/// <summary>Indicates whether there is data on the Clipboard in the <see cref="F:System.Windows.Forms.DataFormats.WaveAudio" /> format.</summary>
		/// <returns>
		///     <see langword="true" /> if there is audio data on the Clipboard; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A1D RID: 2589 RVA: 0x0001E9E0 File Offset: 0x0001CBE0
		public static bool ContainsAudio()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject != null && dataObject.GetDataPresent(DataFormats.WaveAudio, false);
		}

		/// <summary>Indicates whether there is data on the Clipboard that is in the specified format or can be converted to that format. </summary>
		/// <param name="format">The format of the data to look for. See <see cref="T:System.Windows.Forms.DataFormats" /> for predefined formats.</param>
		/// <returns>
		///     <see langword="true" /> if there is data on the Clipboard that is in the specified <paramref name="format" /> or can be converted to that format; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A1E RID: 2590 RVA: 0x0001EA04 File Offset: 0x0001CC04
		public static bool ContainsData(string format)
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject != null && dataObject.GetDataPresent(format, false);
		}

		/// <summary>Indicates whether there is data on the Clipboard that is in the <see cref="F:System.Windows.Forms.DataFormats.FileDrop" /> format or can be converted to that format.</summary>
		/// <returns>
		///     <see langword="true" /> if there is a file drop list on the Clipboard; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A1F RID: 2591 RVA: 0x0001EA24 File Offset: 0x0001CC24
		public static bool ContainsFileDropList()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject != null && dataObject.GetDataPresent(DataFormats.FileDrop, true);
		}

		/// <summary>Indicates whether there is data on the Clipboard that is in the <see cref="F:System.Windows.Forms.DataFormats.Bitmap" /> format or can be converted to that format.</summary>
		/// <returns>
		///     <see langword="true" /> if there is image data on the Clipboard; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A20 RID: 2592 RVA: 0x0001EA48 File Offset: 0x0001CC48
		public static bool ContainsImage()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject != null && dataObject.GetDataPresent(DataFormats.Bitmap, true);
		}

		/// <summary>Indicates whether there is data on the Clipboard in the <see cref="F:System.Windows.Forms.TextDataFormat.Text" /> or <see cref="F:System.Windows.Forms.TextDataFormat.UnicodeText" /> format, depending on the operating system.</summary>
		/// <returns>
		///     <see langword="true" /> if there is text data on the Clipboard; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A21 RID: 2593 RVA: 0x0001EA6C File Offset: 0x0001CC6C
		public static bool ContainsText()
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				return Clipboard.ContainsText(TextDataFormat.Text);
			}
			return Clipboard.ContainsText(TextDataFormat.UnicodeText);
		}

		/// <summary>Indicates whether there is text data on the Clipboard in the format indicated by the specified <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</summary>
		/// <param name="format">One of the <see cref="T:System.Windows.Forms.TextDataFormat" /> values.</param>
		/// <returns>
		///     <see langword="true" /> if there is text data on the Clipboard in the value specified for <paramref name="format" />; otherwise, <see langword="false" />.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="format" /> is not a valid <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</exception>
		// Token: 0x06000A22 RID: 2594 RVA: 0x0001EA9C File Offset: 0x0001CC9C
		public static bool ContainsText(TextDataFormat format)
		{
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			IDataObject dataObject = Clipboard.GetDataObject();
			return dataObject != null && dataObject.GetDataPresent(Clipboard.ConvertToDataFormats(format), false);
		}

		/// <summary>Retrieves an audio stream from the Clipboard.</summary>
		/// <returns>A <see cref="T:System.IO.Stream" /> containing audio data or <see langword="null" /> if the Clipboard does not contain any data in the <see cref="F:System.Windows.Forms.DataFormats.WaveAudio" /> format.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A23 RID: 2595 RVA: 0x0001EAE8 File Offset: 0x0001CCE8
		public static Stream GetAudioStream()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject != null)
			{
				return dataObject.GetData(DataFormats.WaveAudio, false) as Stream;
			}
			return null;
		}

		/// <summary>Retrieves data from the Clipboard in the specified format.</summary>
		/// <param name="format">The format of the data to retrieve. See <see cref="T:System.Windows.Forms.DataFormats" /> for predefined formats.</param>
		/// <returns>An <see cref="T:System.Object" /> representing the Clipboard data or <see langword="null" /> if the Clipboard does not contain any data that is in the specified <paramref name="format" /> or can be converted to that format.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A24 RID: 2596 RVA: 0x0001EB14 File Offset: 0x0001CD14
		public static object GetData(string format)
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject != null)
			{
				return dataObject.GetData(format);
			}
			return null;
		}

		/// <summary>Retrieves a collection of file names from the Clipboard. </summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.StringCollection" /> containing file names or <see langword="null" /> if the Clipboard does not contain any data that is in the <see cref="F:System.Windows.Forms.DataFormats.FileDrop" /> format or can be converted to that format.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A25 RID: 2597 RVA: 0x0001EB34 File Offset: 0x0001CD34
		public static StringCollection GetFileDropList()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			StringCollection stringCollection = new StringCollection();
			if (dataObject != null)
			{
				string[] array = dataObject.GetData(DataFormats.FileDrop, true) as string[];
				if (array != null)
				{
					stringCollection.AddRange(array);
				}
			}
			return stringCollection;
		}

		/// <summary>Retrieves an image from the Clipboard.</summary>
		/// <returns>An <see cref="T:System.Drawing.Image" /> representing the Clipboard image data or <see langword="null" /> if the Clipboard does not contain any data that is in the <see cref="F:System.Windows.Forms.DataFormats.Bitmap" /> format or can be converted to that format.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A26 RID: 2598 RVA: 0x0001EB70 File Offset: 0x0001CD70
		public static Image GetImage()
		{
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject != null)
			{
				return dataObject.GetData(DataFormats.Bitmap, true) as Image;
			}
			return null;
		}

		/// <summary>Retrieves text data from the Clipboard in the <see cref="F:System.Windows.Forms.TextDataFormat.Text" /> or <see cref="F:System.Windows.Forms.TextDataFormat.UnicodeText" /> format, depending on the operating system.</summary>
		/// <returns>The Clipboard text data or <see cref="F:System.String.Empty" /> if the Clipboard does not contain data in the <see cref="F:System.Windows.Forms.TextDataFormat.Text" /> or <see cref="F:System.Windows.Forms.TextDataFormat.UnicodeText" /> format, depending on the operating system.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		// Token: 0x06000A27 RID: 2599 RVA: 0x0001EB99 File Offset: 0x0001CD99
		public static string GetText()
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				return Clipboard.GetText(TextDataFormat.Text);
			}
			return Clipboard.GetText(TextDataFormat.UnicodeText);
		}

		/// <summary>Retrieves text data from the Clipboard in the format indicated by the specified <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</summary>
		/// <param name="format">One of the <see cref="T:System.Windows.Forms.TextDataFormat" /> values.</param>
		/// <returns>The Clipboard text data or <see cref="F:System.String.Empty" /> if the Clipboard does not contain data in the specified format.</returns>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="format" /> is not a valid <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</exception>
		// Token: 0x06000A28 RID: 2600 RVA: 0x0001EBC8 File Offset: 0x0001CDC8
		public static string GetText(TextDataFormat format)
		{
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			IDataObject dataObject = Clipboard.GetDataObject();
			if (dataObject != null)
			{
				string text = dataObject.GetData(Clipboard.ConvertToDataFormats(format), false) as string;
				if (text != null)
				{
					return text;
				}
			}
			return string.Empty;
		}

		/// <summary>Clears the Clipboard and then adds a <see cref="T:System.Byte" /> array in the <see cref="F:System.Windows.Forms.DataFormats.WaveAudio" /> format after converting it to a <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="audioBytes">A <see cref="T:System.Byte" /> array containing the audio data.</param>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="audioBytes" /> is <see langword="null" />.</exception>
		// Token: 0x06000A29 RID: 2601 RVA: 0x0001EC21 File Offset: 0x0001CE21
		public static void SetAudio(byte[] audioBytes)
		{
			if (audioBytes == null)
			{
				throw new ArgumentNullException("audioBytes");
			}
			Clipboard.SetAudio(new MemoryStream(audioBytes));
		}

		/// <summary>Clears the Clipboard and then adds a <see cref="T:System.IO.Stream" /> in the <see cref="F:System.Windows.Forms.DataFormats.WaveAudio" /> format.</summary>
		/// <param name="audioStream">A <see cref="T:System.IO.Stream" /> containing the audio data.</param>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="audioStream" /> is <see langword="null" />.</exception>
		// Token: 0x06000A2A RID: 2602 RVA: 0x0001EC3C File Offset: 0x0001CE3C
		public static void SetAudio(Stream audioStream)
		{
			if (audioStream == null)
			{
				throw new ArgumentNullException("audioStream");
			}
			IDataObject dataObject = new DataObject();
			dataObject.SetData(DataFormats.WaveAudio, false, audioStream);
			Clipboard.SetDataObject(dataObject, true);
		}

		/// <summary>Clears the Clipboard and then adds data in the specified format.</summary>
		/// <param name="format">The format of the data to set. See <see cref="T:System.Windows.Forms.DataFormats" /> for predefined formats.</param>
		/// <param name="data">An <see cref="T:System.Object" /> representing the data to add.</param>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="data" /> is <see langword="null" />.</exception>
		// Token: 0x06000A2B RID: 2603 RVA: 0x0001EC74 File Offset: 0x0001CE74
		public static void SetData(string format, object data)
		{
			IDataObject dataObject = new DataObject();
			dataObject.SetData(format, data);
			Clipboard.SetDataObject(dataObject, true);
		}

		/// <summary>Clears the Clipboard and then adds a collection of file names in the <see cref="F:System.Windows.Forms.DataFormats.FileDrop" /> format.</summary>
		/// <param name="filePaths">A <see cref="T:System.Collections.Specialized.StringCollection" /> containing the file names.</param>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="filePaths" /> is <see langword="null" />.</exception>
		/// <exception cref="T:System.ArgumentException">
		///         <paramref name="filePaths" /> does not contain any strings.-or-At least one of the strings in <paramref name="filePaths" /> is <see cref="F:System.String.Empty" />, contains only white space, contains one or more invalid characters as defined by <see cref="F:System.IO.Path.InvalidPathChars" />, is <see langword="null" />, contains a colon (:), or exceeds the system-defined maximum length.See the <see cref="P:System.Exception.InnerException" /> property of the <see cref="T:System.ArgumentException" /> for more information.</exception>
		// Token: 0x06000A2C RID: 2604 RVA: 0x0001EC98 File Offset: 0x0001CE98
		public static void SetFileDropList(StringCollection filePaths)
		{
			if (filePaths == null)
			{
				throw new ArgumentNullException("filePaths");
			}
			if (filePaths.Count == 0)
			{
				throw new ArgumentException(SR.GetString("CollectionEmptyException"));
			}
			foreach (string text in filePaths)
			{
				try
				{
					string fullPath = Path.GetFullPath(text);
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
					throw new ArgumentException(SR.GetString("Clipboard_InvalidPath", new object[]
					{
						text,
						"filePaths"
					}), ex);
				}
			}
			if (filePaths.Count > 0)
			{
				IDataObject dataObject = new DataObject();
				string[] array = new string[filePaths.Count];
				filePaths.CopyTo(array, 0);
				dataObject.SetData(DataFormats.FileDrop, true, array);
				Clipboard.SetDataObject(dataObject, true);
			}
		}

		/// <summary>Clears the Clipboard and then adds an <see cref="T:System.Drawing.Image" /> in the <see cref="F:System.Windows.Forms.DataFormats.Bitmap" /> format.</summary>
		/// <param name="image">The <see cref="T:System.Drawing.Image" /> to add to the Clipboard.</param>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="image" /> is <see langword="null" />.</exception>
		// Token: 0x06000A2D RID: 2605 RVA: 0x0001ED8C File Offset: 0x0001CF8C
		public static void SetImage(Image image)
		{
			if (image == null)
			{
				throw new ArgumentNullException("image");
			}
			IDataObject dataObject = new DataObject();
			dataObject.SetData(DataFormats.Bitmap, true, image);
			Clipboard.SetDataObject(dataObject, true);
		}

		/// <summary>Clears the Clipboard and then adds text data in the <see cref="F:System.Windows.Forms.TextDataFormat.Text" /> or <see cref="F:System.Windows.Forms.TextDataFormat.UnicodeText" /> format, depending on the operating system.</summary>
		/// <param name="text">The text to add to the Clipboard.</param>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="text" /> is <see langword="null" /> or <see cref="F:System.String.Empty" />.</exception>
		// Token: 0x06000A2E RID: 2606 RVA: 0x0001EDC1 File Offset: 0x0001CFC1
		public static void SetText(string text)
		{
			if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
			{
				Clipboard.SetText(text, TextDataFormat.Text);
				return;
			}
			Clipboard.SetText(text, TextDataFormat.UnicodeText);
		}

		/// <summary>Clears the Clipboard and then adds text data in the format indicated by the specified <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</summary>
		/// <param name="text">The text to add to the Clipboard.</param>
		/// <param name="format">One of the <see cref="T:System.Windows.Forms.TextDataFormat" /> values.</param>
		/// <exception cref="T:System.Runtime.InteropServices.ExternalException">The Clipboard could not be cleared. This typically occurs when the Clipboard is being used by another process.</exception>
		/// <exception cref="T:System.Threading.ThreadStateException">The current thread is not in single-threaded apartment (STA) mode. Add the <see cref="T:System.STAThreadAttribute" /> to your application's <see langword="Main" /> method.</exception>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="text" /> is <see langword="null" /> or <see cref="F:System.String.Empty" />.</exception>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">
		///         <paramref name="format" /> is not a valid <see cref="T:System.Windows.Forms.TextDataFormat" /> value.</exception>
		// Token: 0x06000A2F RID: 2607 RVA: 0x0001EDF4 File Offset: 0x0001CFF4
		public static void SetText(string text, TextDataFormat format)
		{
			if (string.IsNullOrEmpty(text))
			{
				throw new ArgumentNullException("text");
			}
			if (!ClientUtils.IsEnumValid(format, (int)format, 0, 4))
			{
				throw new InvalidEnumArgumentException("format", (int)format, typeof(TextDataFormat));
			}
			IDataObject dataObject = new DataObject();
			dataObject.SetData(Clipboard.ConvertToDataFormats(format), false, text);
			Clipboard.SetDataObject(dataObject, true);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0001EE58 File Offset: 0x0001D058
		private static string ConvertToDataFormats(TextDataFormat format)
		{
			switch (format)
			{
			case TextDataFormat.Text:
				return DataFormats.Text;
			case TextDataFormat.UnicodeText:
				return DataFormats.UnicodeText;
			case TextDataFormat.Rtf:
				return DataFormats.Rtf;
			case TextDataFormat.Html:
				return DataFormats.Html;
			case TextDataFormat.CommaSeparatedValue:
				return DataFormats.CommaSeparatedValue;
			default:
				return DataFormats.UnicodeText;
			}
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0001EEA4 File Offset: 0x0001D0A4
		private static void ThrowIfFailed(int hr)
		{
			if (hr != 0)
			{
				ExternalException ex = new ExternalException(SR.GetString("ClipboardOperationFailed"), hr);
				throw ex;
			}
		}
	}
}
