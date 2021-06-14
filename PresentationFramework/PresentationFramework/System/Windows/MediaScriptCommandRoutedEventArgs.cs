using System;

namespace System.Windows
{
	/// <summary>Provides data for the <see cref="E:System.Windows.Controls.MediaElement.ScriptCommand" /> and <see cref="E:System.Windows.Media.MediaPlayer.ScriptCommand" /> events.</summary>
	// Token: 0x020000A6 RID: 166
	public sealed class MediaScriptCommandRoutedEventArgs : RoutedEventArgs
	{
		// Token: 0x06000367 RID: 871 RVA: 0x00009B98 File Offset: 0x00007D98
		internal MediaScriptCommandRoutedEventArgs(RoutedEvent routedEvent, object sender, string parameterType, string parameterValue) : base(routedEvent, sender)
		{
			if (parameterType == null)
			{
				throw new ArgumentNullException("parameterType");
			}
			if (parameterValue == null)
			{
				throw new ArgumentNullException("parameterValue");
			}
			this._parameterType = parameterType;
			this._parameterValue = parameterValue;
		}

		/// <summary>Gets the type of script command that was raised.</summary>
		/// <returns>The type of script command that was raised.</returns>
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00009BCE File Offset: 0x00007DCE
		public string ParameterType
		{
			get
			{
				return this._parameterType;
			}
		}

		/// <summary>Gets the arguments associated with the script command type.</summary>
		/// <returns>The arguments associated with the script command type.</returns>
		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00009BD6 File Offset: 0x00007DD6
		public string ParameterValue
		{
			get
			{
				return this._parameterValue;
			}
		}

		// Token: 0x040005E8 RID: 1512
		private string _parameterType;

		// Token: 0x040005E9 RID: 1513
		private string _parameterValue;
	}
}
