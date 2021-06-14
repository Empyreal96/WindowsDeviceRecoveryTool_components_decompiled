using System;

namespace System.Drawing.Design
{
	/// <summary>Provides a callback mechanism that can create a <see cref="T:System.Drawing.Design.ToolboxItem" />.</summary>
	/// <param name="serializedObject">The object which contains the data to create a <see cref="T:System.Drawing.Design.ToolboxItem" /> for. </param>
	/// <param name="format">The name of the clipboard data format to create a <see cref="T:System.Drawing.Design.ToolboxItem" /> for. </param>
	/// <returns>The deserialized <see cref="T:System.Drawing.Design.ToolboxItem" /> object specified by <paramref name="serializedObject" />.</returns>
	// Token: 0x02000081 RID: 129
	// (Invoke) Token: 0x060008B3 RID: 2227
	public delegate ToolboxItem ToolboxItemCreatorCallback(object serializedObject, string format);
}
