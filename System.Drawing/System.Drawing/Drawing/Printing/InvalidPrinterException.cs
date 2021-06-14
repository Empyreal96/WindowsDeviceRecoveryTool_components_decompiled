using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace System.Drawing.Printing
{
	/// <summary>Represents the exception that is thrown when you try to access a printer using printer settings that are not valid.</summary>
	// Token: 0x02000054 RID: 84
	[Serializable]
	public class InvalidPrinterException : SystemException
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Printing.InvalidPrinterException" /> class.</summary>
		/// <param name="settings">A <see cref="T:System.Drawing.Printing.PrinterSettings" /> that specifies the settings for a printer. </param>
		// Token: 0x06000707 RID: 1799 RVA: 0x0001C9A0 File Offset: 0x0001ABA0
		public InvalidPrinterException(PrinterSettings settings) : base(InvalidPrinterException.GenerateMessage(settings))
		{
			this.settings = settings;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Drawing.Printing.InvalidPrinterException" /> class with serialized data.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="info" /> is <see langword="null" />. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is 0. </exception>
		// Token: 0x06000708 RID: 1800 RVA: 0x0001C9B5 File Offset: 0x0001ABB5
		protected InvalidPrinterException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.settings = (PrinterSettings)info.GetValue("settings", typeof(PrinterSettings));
		}

		/// <summary>Overridden. Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///         <paramref name="info" /> is <see langword="null" />. </exception>
		// Token: 0x06000709 RID: 1801 RVA: 0x0001C9DF File Offset: 0x0001ABDF
		[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			IntSecurity.AllPrinting.Demand();
			info.AddValue("settings", this.settings);
			base.GetObjectData(info, context);
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0001CA14 File Offset: 0x0001AC14
		private static string GenerateMessage(PrinterSettings settings)
		{
			if (settings.IsDefaultPrinter)
			{
				return SR.GetString("InvalidPrinterException_NoDefaultPrinter");
			}
			string @string;
			try
			{
				@string = SR.GetString("InvalidPrinterException_InvalidPrinter", new object[]
				{
					settings.PrinterName
				});
			}
			catch (SecurityException)
			{
				@string = SR.GetString("InvalidPrinterException_InvalidPrinter", new object[]
				{
					SR.GetString("CantTellPrinterName")
				});
			}
			return @string;
		}

		// Token: 0x0400060A RID: 1546
		private PrinterSettings settings;
	}
}
