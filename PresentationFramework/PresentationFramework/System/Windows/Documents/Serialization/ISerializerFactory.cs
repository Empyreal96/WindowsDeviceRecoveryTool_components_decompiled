using System;
using System.IO;

namespace System.Windows.Documents.Serialization
{
	/// <summary>Provides a means for creating a software component that can serialize any part of a Windows Presentation Foundation (WPF) application's content to a manufacturer's proprietary format. </summary>
	// Token: 0x02000449 RID: 1097
	public interface ISerializerFactory
	{
		/// <summary>Initializes an object derived from the abstract <see cref="T:System.Windows.Documents.Serialization.SerializerWriter" /> class for the specified <see cref="T:System.IO.Stream" />. </summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> to which the returned object writes.</param>
		/// <returns>An object of a class derived from <see cref="T:System.Windows.Documents.Serialization.SerializerWriter" />.</returns>
		// Token: 0x06003FEE RID: 16366
		SerializerWriter CreateSerializerWriter(Stream stream);

		/// <summary>Gets the public name of the manufacturer's serializing component. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the public name of the serializing component. </returns>
		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x06003FEF RID: 16367
		string DisplayName { get; }

		/// <summary>Gets the name of the serializing component's manufacturer. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the manufacturer's name. </returns>
		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x06003FF0 RID: 16368
		string ManufacturerName { get; }

		/// <summary>Gets the web address of the serializing component's manufacturer. </summary>
		/// <returns>A <see cref="T:System.Uri" /> representing the manufacturer's website.</returns>
		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x06003FF1 RID: 16369
		Uri ManufacturerWebsite { get; }

		/// <summary>Gets the default extension for files of the manufacturer's proprietary format. </summary>
		/// <returns>A <see cref="T:System.String" /> representing the proprietary format's default file extension.</returns>
		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x06003FF2 RID: 16370
		string DefaultFileExtension { get; }
	}
}
