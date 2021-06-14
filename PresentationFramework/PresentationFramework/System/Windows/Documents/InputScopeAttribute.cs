using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Input;
using MS.Win32;

namespace System.Windows.Documents
{
	// Token: 0x0200037F RID: 895
	internal class InputScopeAttribute : UnsafeNativeMethods.ITfInputScope
	{
		// Token: 0x060030A9 RID: 12457 RVA: 0x000DB6C7 File Offset: 0x000D98C7
		internal InputScopeAttribute(InputScope inputscope)
		{
			this._inputScope = inputscope;
		}

		// Token: 0x060030AA RID: 12458 RVA: 0x000DB6D8 File Offset: 0x000D98D8
		[SecurityCritical]
		public void GetInputScopes(out IntPtr ppinputscopes, out int count)
		{
			if (this._inputScope != null)
			{
				int num = 0;
				count = this._inputScope.Names.Count;
				try
				{
					ppinputscopes = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)) * count);
				}
				catch (OutOfMemoryException)
				{
					throw new COMException(SR.Get("InputScopeAttribute_E_OUTOFMEMORY"), -2147024882);
				}
				for (int i = 0; i < count; i++)
				{
					Marshal.WriteInt32(ppinputscopes, num, (int)((InputScopeName)this._inputScope.Names[i]).NameValue);
					num += Marshal.SizeOf(typeof(int));
				}
				return;
			}
			ppinputscopes = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)));
			Marshal.WriteInt32(ppinputscopes, 0);
			count = 1;
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x000DB7AC File Offset: 0x000D99AC
		[SecurityCritical]
		public int GetPhrase(out IntPtr ppbstrPhrases, out int count)
		{
			count = ((this._inputScope == null) ? 0 : this._inputScope.PhraseList.Count);
			try
			{
				ppbstrPhrases = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(IntPtr)) * count);
			}
			catch (OutOfMemoryException)
			{
				throw new COMException(SR.Get("InputScopeAttribute_E_OUTOFMEMORY"), -2147024882);
			}
			int num = 0;
			for (int i = 0; i < count; i++)
			{
				IntPtr val;
				try
				{
					val = Marshal.StringToBSTR(((InputScopePhrase)this._inputScope.PhraseList[i]).Name);
				}
				catch (OutOfMemoryException)
				{
					num = 0;
					for (int j = 0; j < i; j++)
					{
						Marshal.FreeBSTR(Marshal.ReadIntPtr(ppbstrPhrases, num));
						num += Marshal.SizeOf(typeof(IntPtr));
					}
					throw new COMException(SR.Get("InputScopeAttribute_E_OUTOFMEMORY"), -2147024882);
				}
				Marshal.WriteIntPtr(ppbstrPhrases, num, val);
				num += Marshal.SizeOf(typeof(IntPtr));
			}
			if (count <= 0)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060030AC RID: 12460 RVA: 0x000DB8C4 File Offset: 0x000D9AC4
		public int GetRegularExpression(out string desc)
		{
			desc = null;
			if (this._inputScope != null)
			{
				desc = this._inputScope.RegularExpression;
			}
			if (desc == null)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060030AD RID: 12461 RVA: 0x000DB8E5 File Offset: 0x000D9AE5
		public int GetSRGC(out string desc)
		{
			desc = null;
			if (this._inputScope != null)
			{
				desc = this._inputScope.SrgsMarkup;
			}
			if (desc == null)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x000DB906 File Offset: 0x000D9B06
		public int GetXML(out string desc)
		{
			desc = null;
			return 1;
		}

		// Token: 0x04001E8C RID: 7820
		private InputScope _inputScope;
	}
}
