using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Accessibility;

namespace System.Windows.Forms
{
	// Token: 0x02000109 RID: 265
	internal sealed class InternalAccessibleObject : StandardOleMarshalObject, UnsafeNativeMethods.IAccessibleInternal, IReflect, UnsafeNativeMethods.IServiceProvider, UnsafeNativeMethods.IAccessibleEx, UnsafeNativeMethods.IRawElementProviderSimple, UnsafeNativeMethods.IRawElementProviderFragment, UnsafeNativeMethods.IRawElementProviderFragmentRoot, UnsafeNativeMethods.IInvokeProvider, UnsafeNativeMethods.IValueProvider, UnsafeNativeMethods.IRangeValueProvider, UnsafeNativeMethods.IExpandCollapseProvider, UnsafeNativeMethods.IToggleProvider, UnsafeNativeMethods.ITableProvider, UnsafeNativeMethods.ITableItemProvider, UnsafeNativeMethods.IGridProvider, UnsafeNativeMethods.IGridItemProvider, UnsafeNativeMethods.IEnumVariant, UnsafeNativeMethods.IOleWindow, UnsafeNativeMethods.ILegacyIAccessibleProvider, UnsafeNativeMethods.ISelectionProvider, UnsafeNativeMethods.ISelectionItemProvider, UnsafeNativeMethods.IScrollItemProvider, UnsafeNativeMethods.IRawElementProviderHwndOverride
	{
		// Token: 0x0600052A RID: 1322 RVA: 0x0000F4D4 File Offset: 0x0000D6D4
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal InternalAccessibleObject(AccessibleObject accessibleImplemention)
		{
			this.publicIAccessible = accessibleImplemention;
			this.publicIEnumVariant = accessibleImplemention;
			this.publicIOleWindow = accessibleImplemention;
			this.publicIReflect = accessibleImplemention;
			this.publicIServiceProvider = accessibleImplemention;
			this.publicIAccessibleEx = accessibleImplemention;
			this.publicIRawElementProviderSimple = accessibleImplemention;
			this.publicIRawElementProviderFragment = accessibleImplemention;
			this.publicIRawElementProviderFragmentRoot = accessibleImplemention;
			this.publicIInvokeProvider = accessibleImplemention;
			this.publicIValueProvider = accessibleImplemention;
			this.publicIRangeValueProvider = accessibleImplemention;
			this.publicIExpandCollapseProvider = accessibleImplemention;
			this.publicIToggleProvider = accessibleImplemention;
			this.publicITableProvider = accessibleImplemention;
			this.publicITableItemProvider = accessibleImplemention;
			this.publicIGridProvider = accessibleImplemention;
			this.publicIGridItemProvider = accessibleImplemention;
			this.publicILegacyIAccessibleProvider = accessibleImplemention;
			this.publicISelectionProvider = accessibleImplemention;
			this.publicISelectionItemProvider = accessibleImplemention;
			this.publicIScrollItemProvider = accessibleImplemention;
			this.publicIRawElementProviderHwndOverride = accessibleImplemention;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0000F588 File Offset: 0x0000D788
		private object AsNativeAccessible(object accObject)
		{
			if (accObject is AccessibleObject)
			{
				return new InternalAccessibleObject(accObject as AccessibleObject);
			}
			return accObject;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0000F5A0 File Offset: 0x0000D7A0
		private object[] AsArrayOfNativeAccessibles(object[] accObjectArray)
		{
			if (accObjectArray != null && accObjectArray.Length != 0)
			{
				for (int i = 0; i < accObjectArray.Length; i++)
				{
					accObjectArray[i] = this.AsNativeAccessible(accObjectArray[i]);
				}
			}
			return accObjectArray;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0000F5CF File Offset: 0x0000D7CF
		void UnsafeNativeMethods.IAccessibleInternal.accDoDefaultAction(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIAccessible.accDoDefaultAction(childID);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0000F5E7 File Offset: 0x0000D7E7
		object UnsafeNativeMethods.IAccessibleInternal.accHitTest(int xLeft, int yTop)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.accHitTest(xLeft, yTop));
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000F606 File Offset: 0x0000D806
		void UnsafeNativeMethods.IAccessibleInternal.accLocation(out int l, out int t, out int w, out int h, object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIAccessible.accLocation(out l, out t, out w, out h, childID);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0000F624 File Offset: 0x0000D824
		object UnsafeNativeMethods.IAccessibleInternal.accNavigate(int navDir, object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.accNavigate(navDir, childID));
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0000F643 File Offset: 0x0000D843
		void UnsafeNativeMethods.IAccessibleInternal.accSelect(int flagsSelect, object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIAccessible.accSelect(flagsSelect, childID);
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0000F65C File Offset: 0x0000D85C
		object UnsafeNativeMethods.IAccessibleInternal.get_accChild(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.get_accChild(childID));
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0000F67A File Offset: 0x0000D87A
		int UnsafeNativeMethods.IAccessibleInternal.get_accChildCount()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.accChildCount;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0000F691 File Offset: 0x0000D891
		string UnsafeNativeMethods.IAccessibleInternal.get_accDefaultAction(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accDefaultAction(childID);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0000F6A9 File Offset: 0x0000D8A9
		string UnsafeNativeMethods.IAccessibleInternal.get_accDescription(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accDescription(childID);
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0000F6C1 File Offset: 0x0000D8C1
		object UnsafeNativeMethods.IAccessibleInternal.get_accFocus()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.accFocus);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0000F6DE File Offset: 0x0000D8DE
		string UnsafeNativeMethods.IAccessibleInternal.get_accHelp(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accHelp(childID);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0000F6F6 File Offset: 0x0000D8F6
		int UnsafeNativeMethods.IAccessibleInternal.get_accHelpTopic(out string pszHelpFile, object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accHelpTopic(out pszHelpFile, childID);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0000F70F File Offset: 0x0000D90F
		string UnsafeNativeMethods.IAccessibleInternal.get_accKeyboardShortcut(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accKeyboardShortcut(childID);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0000F727 File Offset: 0x0000D927
		string UnsafeNativeMethods.IAccessibleInternal.get_accName(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accName(childID);
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000F73F File Offset: 0x0000D93F
		object UnsafeNativeMethods.IAccessibleInternal.get_accParent()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.accParent);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0000F75C File Offset: 0x0000D95C
		object UnsafeNativeMethods.IAccessibleInternal.get_accRole(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accRole(childID);
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0000F774 File Offset: 0x0000D974
		object UnsafeNativeMethods.IAccessibleInternal.get_accSelection()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIAccessible.accSelection);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0000F791 File Offset: 0x0000D991
		object UnsafeNativeMethods.IAccessibleInternal.get_accState(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accState(childID);
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0000F7A9 File Offset: 0x0000D9A9
		string UnsafeNativeMethods.IAccessibleInternal.get_accValue(object childID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessible.get_accValue(childID);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0000F7C1 File Offset: 0x0000D9C1
		void UnsafeNativeMethods.IAccessibleInternal.set_accName(object childID, string newName)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIAccessible.set_accName(childID, newName);
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0000F7DA File Offset: 0x0000D9DA
		void UnsafeNativeMethods.IAccessibleInternal.set_accValue(object childID, string newValue)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIAccessible.set_accValue(childID, newValue);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0000F7F3 File Offset: 0x0000D9F3
		void UnsafeNativeMethods.IEnumVariant.Clone(UnsafeNativeMethods.IEnumVariant[] v)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIEnumVariant.Clone(v);
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0000F80B File Offset: 0x0000DA0B
		int UnsafeNativeMethods.IEnumVariant.Next(int n, IntPtr rgvar, int[] ns)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIEnumVariant.Next(n, rgvar, ns);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0000F825 File Offset: 0x0000DA25
		void UnsafeNativeMethods.IEnumVariant.Reset()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIEnumVariant.Reset();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0000F83C File Offset: 0x0000DA3C
		void UnsafeNativeMethods.IEnumVariant.Skip(int n)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIEnumVariant.Skip(n);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0000F854 File Offset: 0x0000DA54
		int UnsafeNativeMethods.IOleWindow.GetWindow(out IntPtr hwnd)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIOleWindow.GetWindow(out hwnd);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0000F86C File Offset: 0x0000DA6C
		void UnsafeNativeMethods.IOleWindow.ContextSensitiveHelp(int fEnterMode)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIOleWindow.ContextSensitiveHelp(fEnterMode);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0000F884 File Offset: 0x0000DA84
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
		{
			return this.publicIReflect.GetMethod(name, bindingAttr, binder, types, modifiers);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0000F898 File Offset: 0x0000DA98
		MethodInfo IReflect.GetMethod(string name, BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetMethod(name, bindingAttr);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0000F8A7 File Offset: 0x0000DAA7
		MethodInfo[] IReflect.GetMethods(BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetMethods(bindingAttr);
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0000F8B5 File Offset: 0x0000DAB5
		FieldInfo IReflect.GetField(string name, BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetField(name, bindingAttr);
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0000F8C4 File Offset: 0x0000DAC4
		FieldInfo[] IReflect.GetFields(BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetFields(bindingAttr);
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0000F8D2 File Offset: 0x0000DAD2
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetProperty(name, bindingAttr);
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0000F8E1 File Offset: 0x0000DAE1
		PropertyInfo IReflect.GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
		{
			return this.publicIReflect.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0000F8F7 File Offset: 0x0000DAF7
		PropertyInfo[] IReflect.GetProperties(BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetProperties(bindingAttr);
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0000F905 File Offset: 0x0000DB05
		MemberInfo[] IReflect.GetMember(string name, BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetMember(name, bindingAttr);
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0000F914 File Offset: 0x0000DB14
		MemberInfo[] IReflect.GetMembers(BindingFlags bindingAttr)
		{
			return this.publicIReflect.GetMembers(bindingAttr);
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0000F924 File Offset: 0x0000DB24
		object IReflect.InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			IntSecurity.UnmanagedCode.Demand();
			return this.publicIReflect.InvokeMember(name, invokeAttr, binder, this.publicIAccessible, args, modifiers, culture, namedParameters);
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000553 RID: 1363 RVA: 0x0000F958 File Offset: 0x0000DB58
		Type IReflect.UnderlyingSystemType
		{
			get
			{
				IReflect reflect = this.publicIReflect;
				return this.publicIReflect.UnderlyingSystemType;
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0000F978 File Offset: 0x0000DB78
		int UnsafeNativeMethods.IServiceProvider.QueryService(ref Guid service, ref Guid riid, out IntPtr ppvObject)
		{
			IntSecurity.UnmanagedCode.Assert();
			ppvObject = IntPtr.Zero;
			int num = this.publicIServiceProvider.QueryService(ref service, ref riid, out ppvObject);
			if (num >= 0)
			{
				ppvObject = Marshal.GetComInterfaceForObject(this, typeof(UnsafeNativeMethods.IAccessibleEx));
			}
			return num;
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0000F9BC File Offset: 0x0000DBBC
		object UnsafeNativeMethods.IAccessibleEx.GetObjectForChild(int idChild)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessibleEx.GetObjectForChild(idChild);
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0000F9D4 File Offset: 0x0000DBD4
		int UnsafeNativeMethods.IAccessibleEx.GetIAccessiblePair(out object ppAcc, out int pidChild)
		{
			IntSecurity.UnmanagedCode.Assert();
			ppAcc = this;
			pidChild = 0;
			return 0;
		}

		// Token: 0x06000557 RID: 1367 RVA: 0x0000F9E7 File Offset: 0x0000DBE7
		int[] UnsafeNativeMethods.IAccessibleEx.GetRuntimeId()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessibleEx.GetRuntimeId();
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0000F9FE File Offset: 0x0000DBFE
		int UnsafeNativeMethods.IAccessibleEx.ConvertReturnedElement(object pIn, out object ppRetValOut)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIAccessibleEx.ConvertReturnedElement(pIn, out ppRetValOut);
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000559 RID: 1369 RVA: 0x0000FA17 File Offset: 0x0000DC17
		UnsafeNativeMethods.ProviderOptions UnsafeNativeMethods.IRawElementProviderSimple.ProviderOptions
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIRawElementProviderSimple.ProviderOptions;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x0000FA2E File Offset: 0x0000DC2E
		UnsafeNativeMethods.IRawElementProviderSimple UnsafeNativeMethods.IRawElementProviderSimple.HostRawElementProvider
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIRawElementProviderSimple.HostRawElementProvider;
			}
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0000FA48 File Offset: 0x0000DC48
		object UnsafeNativeMethods.IRawElementProviderSimple.GetPatternProvider(int patternId)
		{
			IntSecurity.UnmanagedCode.Assert();
			object patternProvider = this.publicIRawElementProviderSimple.GetPatternProvider(patternId);
			if (patternProvider == null)
			{
				return null;
			}
			if (patternId == 10005)
			{
				return this;
			}
			if (patternId == 10002)
			{
				return this;
			}
			if (AccessibilityImprovements.Level3 && patternId == 10003)
			{
				return this;
			}
			if (patternId == 10015)
			{
				return this;
			}
			if (patternId == 10012)
			{
				return this;
			}
			if (patternId == 10013)
			{
				return this;
			}
			if (patternId == 10006)
			{
				return this;
			}
			if (patternId == 10007)
			{
				return this;
			}
			if (AccessibilityImprovements.Level3 && patternId == 10000)
			{
				return this;
			}
			if (AccessibilityImprovements.Level3 && patternId == 10018)
			{
				return this;
			}
			if (AccessibilityImprovements.Level3 && patternId == 10001)
			{
				return this;
			}
			if (AccessibilityImprovements.Level3 && patternId == 10010)
			{
				return this;
			}
			if (AccessibilityImprovements.Level3 && patternId == 10017)
			{
				return this;
			}
			return null;
		}

		// Token: 0x0600055C RID: 1372 RVA: 0x0000FB21 File Offset: 0x0000DD21
		object UnsafeNativeMethods.IRawElementProviderSimple.GetPropertyValue(int propertyID)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIRawElementProviderSimple.GetPropertyValue(propertyID);
		}

		// Token: 0x0600055D RID: 1373 RVA: 0x0000FB39 File Offset: 0x0000DD39
		object UnsafeNativeMethods.IRawElementProviderFragment.Navigate(UnsafeNativeMethods.NavigateDirection direction)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIRawElementProviderFragment.Navigate(direction));
		}

		// Token: 0x0600055E RID: 1374 RVA: 0x0000FB57 File Offset: 0x0000DD57
		int[] UnsafeNativeMethods.IRawElementProviderFragment.GetRuntimeId()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIRawElementProviderFragment.GetRuntimeId();
		}

		// Token: 0x0600055F RID: 1375 RVA: 0x0000FB6E File Offset: 0x0000DD6E
		object[] UnsafeNativeMethods.IRawElementProviderFragment.GetEmbeddedFragmentRoots()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsArrayOfNativeAccessibles(this.publicIRawElementProviderFragment.GetEmbeddedFragmentRoots());
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x0000FB8B File Offset: 0x0000DD8B
		void UnsafeNativeMethods.IRawElementProviderFragment.SetFocus()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIRawElementProviderFragment.SetFocus();
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000561 RID: 1377 RVA: 0x0000FBA2 File Offset: 0x0000DDA2
		NativeMethods.UiaRect UnsafeNativeMethods.IRawElementProviderFragment.BoundingRectangle
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIRawElementProviderFragment.BoundingRectangle;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x0000FBB9 File Offset: 0x0000DDB9
		UnsafeNativeMethods.IRawElementProviderFragmentRoot UnsafeNativeMethods.IRawElementProviderFragment.FragmentRoot
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				if (AccessibilityImprovements.Level3)
				{
					return this.publicIRawElementProviderFragment.FragmentRoot;
				}
				return this.AsNativeAccessible(this.publicIRawElementProviderFragment.FragmentRoot) as UnsafeNativeMethods.IRawElementProviderFragmentRoot;
			}
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0000FBEE File Offset: 0x0000DDEE
		object UnsafeNativeMethods.IRawElementProviderFragmentRoot.ElementProviderFromPoint(double x, double y)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIRawElementProviderFragmentRoot.ElementProviderFromPoint(x, y));
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0000FC0D File Offset: 0x0000DE0D
		object UnsafeNativeMethods.IRawElementProviderFragmentRoot.GetFocus()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIRawElementProviderFragmentRoot.GetFocus());
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000565 RID: 1381 RVA: 0x0000FC2A File Offset: 0x0000DE2A
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.DefaultAction
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicILegacyIAccessibleProvider.DefaultAction;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000566 RID: 1382 RVA: 0x0000FC41 File Offset: 0x0000DE41
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.Description
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicILegacyIAccessibleProvider.Description;
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000567 RID: 1383 RVA: 0x0000FC58 File Offset: 0x0000DE58
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.Help
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicILegacyIAccessibleProvider.Help;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000568 RID: 1384 RVA: 0x0000FC6F File Offset: 0x0000DE6F
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.KeyboardShortcut
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicILegacyIAccessibleProvider.KeyboardShortcut;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x0000FC86 File Offset: 0x0000DE86
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.Name
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicILegacyIAccessibleProvider.Name;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x0600056A RID: 1386 RVA: 0x0000FC9D File Offset: 0x0000DE9D
		uint UnsafeNativeMethods.ILegacyIAccessibleProvider.Role
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicILegacyIAccessibleProvider.Role;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0000FCB4 File Offset: 0x0000DEB4
		uint UnsafeNativeMethods.ILegacyIAccessibleProvider.State
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicILegacyIAccessibleProvider.State;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600056C RID: 1388 RVA: 0x0000FCCB File Offset: 0x0000DECB
		string UnsafeNativeMethods.ILegacyIAccessibleProvider.Value
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicILegacyIAccessibleProvider.Value;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0000FCE2 File Offset: 0x0000DEE2
		int UnsafeNativeMethods.ILegacyIAccessibleProvider.ChildId
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicILegacyIAccessibleProvider.ChildId;
			}
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x0000FCF9 File Offset: 0x0000DEF9
		void UnsafeNativeMethods.ILegacyIAccessibleProvider.DoDefaultAction()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicILegacyIAccessibleProvider.DoDefaultAction();
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x0000FD10 File Offset: 0x0000DF10
		IAccessible UnsafeNativeMethods.ILegacyIAccessibleProvider.GetIAccessible()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicILegacyIAccessibleProvider.GetIAccessible();
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x0000FD27 File Offset: 0x0000DF27
		object[] UnsafeNativeMethods.ILegacyIAccessibleProvider.GetSelection()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsArrayOfNativeAccessibles(this.publicILegacyIAccessibleProvider.GetSelection());
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x0000FD44 File Offset: 0x0000DF44
		void UnsafeNativeMethods.ILegacyIAccessibleProvider.Select(int flagsSelect)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicILegacyIAccessibleProvider.Select(flagsSelect);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0000FD5C File Offset: 0x0000DF5C
		void UnsafeNativeMethods.ILegacyIAccessibleProvider.SetValue(string szValue)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicILegacyIAccessibleProvider.SetValue(szValue);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0000FD74 File Offset: 0x0000DF74
		void UnsafeNativeMethods.IInvokeProvider.Invoke()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIInvokeProvider.Invoke();
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000574 RID: 1396 RVA: 0x0000FD8B File Offset: 0x0000DF8B
		bool UnsafeNativeMethods.IValueProvider.IsReadOnly
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIValueProvider.IsReadOnly;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000575 RID: 1397 RVA: 0x0000FDA2 File Offset: 0x0000DFA2
		string UnsafeNativeMethods.IValueProvider.Value
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIValueProvider.Value;
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x0000FDB9 File Offset: 0x0000DFB9
		void UnsafeNativeMethods.IValueProvider.SetValue(string newValue)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIValueProvider.SetValue(newValue);
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000577 RID: 1399 RVA: 0x0000FD8B File Offset: 0x0000DF8B
		bool UnsafeNativeMethods.IRangeValueProvider.IsReadOnly
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIValueProvider.IsReadOnly;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000578 RID: 1400 RVA: 0x0000FDD1 File Offset: 0x0000DFD1
		double UnsafeNativeMethods.IRangeValueProvider.LargeChange
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIRangeValueProvider.LargeChange;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000579 RID: 1401 RVA: 0x0000FDE8 File Offset: 0x0000DFE8
		double UnsafeNativeMethods.IRangeValueProvider.Maximum
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIRangeValueProvider.Maximum;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600057A RID: 1402 RVA: 0x0000FDFF File Offset: 0x0000DFFF
		double UnsafeNativeMethods.IRangeValueProvider.Minimum
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIRangeValueProvider.Minimum;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600057B RID: 1403 RVA: 0x0000FE16 File Offset: 0x0000E016
		double UnsafeNativeMethods.IRangeValueProvider.SmallChange
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIRangeValueProvider.SmallChange;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600057C RID: 1404 RVA: 0x0000FE2D File Offset: 0x0000E02D
		double UnsafeNativeMethods.IRangeValueProvider.Value
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIRangeValueProvider.Value;
			}
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0000FE44 File Offset: 0x0000E044
		void UnsafeNativeMethods.IRangeValueProvider.SetValue(double newValue)
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIRangeValueProvider.SetValue(newValue);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0000FE5C File Offset: 0x0000E05C
		void UnsafeNativeMethods.IExpandCollapseProvider.Expand()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIExpandCollapseProvider.Expand();
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0000FE73 File Offset: 0x0000E073
		void UnsafeNativeMethods.IExpandCollapseProvider.Collapse()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIExpandCollapseProvider.Collapse();
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x0000FE8A File Offset: 0x0000E08A
		UnsafeNativeMethods.ExpandCollapseState UnsafeNativeMethods.IExpandCollapseProvider.ExpandCollapseState
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIExpandCollapseProvider.ExpandCollapseState;
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0000FEA1 File Offset: 0x0000E0A1
		void UnsafeNativeMethods.IToggleProvider.Toggle()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIToggleProvider.Toggle();
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000582 RID: 1410 RVA: 0x0000FEB8 File Offset: 0x0000E0B8
		UnsafeNativeMethods.ToggleState UnsafeNativeMethods.IToggleProvider.ToggleState
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIToggleProvider.ToggleState;
			}
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0000FECF File Offset: 0x0000E0CF
		object[] UnsafeNativeMethods.ITableProvider.GetRowHeaders()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsArrayOfNativeAccessibles(this.publicITableProvider.GetRowHeaders());
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0000FEEC File Offset: 0x0000E0EC
		object[] UnsafeNativeMethods.ITableProvider.GetColumnHeaders()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsArrayOfNativeAccessibles(this.publicITableProvider.GetColumnHeaders());
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0000FF09 File Offset: 0x0000E109
		UnsafeNativeMethods.RowOrColumnMajor UnsafeNativeMethods.ITableProvider.RowOrColumnMajor
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicITableProvider.RowOrColumnMajor;
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0000FF20 File Offset: 0x0000E120
		object[] UnsafeNativeMethods.ITableItemProvider.GetRowHeaderItems()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsArrayOfNativeAccessibles(this.publicITableItemProvider.GetRowHeaderItems());
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000FF3D File Offset: 0x0000E13D
		object[] UnsafeNativeMethods.ITableItemProvider.GetColumnHeaderItems()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsArrayOfNativeAccessibles(this.publicITableItemProvider.GetColumnHeaderItems());
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0000FF5A File Offset: 0x0000E15A
		object UnsafeNativeMethods.IGridProvider.GetItem(int row, int column)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.AsNativeAccessible(this.publicIGridProvider.GetItem(row, column));
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000589 RID: 1417 RVA: 0x0000FF79 File Offset: 0x0000E179
		int UnsafeNativeMethods.IGridProvider.RowCount
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIGridProvider.RowCount;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600058A RID: 1418 RVA: 0x0000FF90 File Offset: 0x0000E190
		int UnsafeNativeMethods.IGridProvider.ColumnCount
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIGridProvider.ColumnCount;
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0000FFA7 File Offset: 0x0000E1A7
		int UnsafeNativeMethods.IGridItemProvider.Row
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIGridItemProvider.Row;
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600058C RID: 1420 RVA: 0x0000FFBE File Offset: 0x0000E1BE
		int UnsafeNativeMethods.IGridItemProvider.Column
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIGridItemProvider.Column;
			}
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0000FFD5 File Offset: 0x0000E1D5
		int UnsafeNativeMethods.IGridItemProvider.RowSpan
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIGridItemProvider.RowSpan;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x0000FFEC File Offset: 0x0000E1EC
		int UnsafeNativeMethods.IGridItemProvider.ColumnSpan
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicIGridItemProvider.ColumnSpan;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x00010003 File Offset: 0x0000E203
		UnsafeNativeMethods.IRawElementProviderSimple UnsafeNativeMethods.IGridItemProvider.ContainingGrid
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				if (AccessibilityImprovements.Level3)
				{
					return this.publicIGridItemProvider.ContainingGrid;
				}
				return this.AsNativeAccessible(this.publicIGridItemProvider.ContainingGrid) as UnsafeNativeMethods.IRawElementProviderSimple;
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00010038 File Offset: 0x0000E238
		object[] UnsafeNativeMethods.ISelectionProvider.GetSelection()
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicISelectionProvider.GetSelection();
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0001004F File Offset: 0x0000E24F
		bool UnsafeNativeMethods.ISelectionProvider.CanSelectMultiple
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicISelectionProvider.CanSelectMultiple;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000592 RID: 1426 RVA: 0x00010066 File Offset: 0x0000E266
		bool UnsafeNativeMethods.ISelectionProvider.IsSelectionRequired
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicISelectionProvider.IsSelectionRequired;
			}
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x0001007D File Offset: 0x0000E27D
		void UnsafeNativeMethods.ISelectionItemProvider.Select()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicISelectionItemProvider.Select();
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00010094 File Offset: 0x0000E294
		void UnsafeNativeMethods.ISelectionItemProvider.AddToSelection()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicISelectionItemProvider.AddToSelection();
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x000100AB File Offset: 0x0000E2AB
		void UnsafeNativeMethods.ISelectionItemProvider.RemoveFromSelection()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicISelectionItemProvider.RemoveFromSelection();
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x000100C2 File Offset: 0x0000E2C2
		bool UnsafeNativeMethods.ISelectionItemProvider.IsSelected
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicISelectionItemProvider.IsSelected;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000597 RID: 1431 RVA: 0x000100D9 File Offset: 0x0000E2D9
		UnsafeNativeMethods.IRawElementProviderSimple UnsafeNativeMethods.ISelectionItemProvider.SelectionContainer
		{
			get
			{
				IntSecurity.UnmanagedCode.Assert();
				return this.publicISelectionItemProvider.SelectionContainer;
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000100F0 File Offset: 0x0000E2F0
		void UnsafeNativeMethods.IScrollItemProvider.ScrollIntoView()
		{
			IntSecurity.UnmanagedCode.Assert();
			this.publicIScrollItemProvider.ScrollIntoView();
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x00010107 File Offset: 0x0000E307
		UnsafeNativeMethods.IRawElementProviderSimple UnsafeNativeMethods.IRawElementProviderHwndOverride.GetOverrideProviderForHwnd(IntPtr hwnd)
		{
			IntSecurity.UnmanagedCode.Assert();
			return this.publicIRawElementProviderHwndOverride.GetOverrideProviderForHwnd(hwnd);
		}

		// Token: 0x0400048D RID: 1165
		private IAccessible publicIAccessible;

		// Token: 0x0400048E RID: 1166
		private UnsafeNativeMethods.IEnumVariant publicIEnumVariant;

		// Token: 0x0400048F RID: 1167
		private UnsafeNativeMethods.IOleWindow publicIOleWindow;

		// Token: 0x04000490 RID: 1168
		private IReflect publicIReflect;

		// Token: 0x04000491 RID: 1169
		private UnsafeNativeMethods.IServiceProvider publicIServiceProvider;

		// Token: 0x04000492 RID: 1170
		private UnsafeNativeMethods.IAccessibleEx publicIAccessibleEx;

		// Token: 0x04000493 RID: 1171
		private UnsafeNativeMethods.IRawElementProviderSimple publicIRawElementProviderSimple;

		// Token: 0x04000494 RID: 1172
		private UnsafeNativeMethods.IRawElementProviderFragment publicIRawElementProviderFragment;

		// Token: 0x04000495 RID: 1173
		private UnsafeNativeMethods.IRawElementProviderFragmentRoot publicIRawElementProviderFragmentRoot;

		// Token: 0x04000496 RID: 1174
		private UnsafeNativeMethods.IInvokeProvider publicIInvokeProvider;

		// Token: 0x04000497 RID: 1175
		private UnsafeNativeMethods.IValueProvider publicIValueProvider;

		// Token: 0x04000498 RID: 1176
		private UnsafeNativeMethods.IRangeValueProvider publicIRangeValueProvider;

		// Token: 0x04000499 RID: 1177
		private UnsafeNativeMethods.IExpandCollapseProvider publicIExpandCollapseProvider;

		// Token: 0x0400049A RID: 1178
		private UnsafeNativeMethods.IToggleProvider publicIToggleProvider;

		// Token: 0x0400049B RID: 1179
		private UnsafeNativeMethods.ITableProvider publicITableProvider;

		// Token: 0x0400049C RID: 1180
		private UnsafeNativeMethods.ITableItemProvider publicITableItemProvider;

		// Token: 0x0400049D RID: 1181
		private UnsafeNativeMethods.IGridProvider publicIGridProvider;

		// Token: 0x0400049E RID: 1182
		private UnsafeNativeMethods.IGridItemProvider publicIGridItemProvider;

		// Token: 0x0400049F RID: 1183
		private UnsafeNativeMethods.ILegacyIAccessibleProvider publicILegacyIAccessibleProvider;

		// Token: 0x040004A0 RID: 1184
		private UnsafeNativeMethods.ISelectionProvider publicISelectionProvider;

		// Token: 0x040004A1 RID: 1185
		private UnsafeNativeMethods.ISelectionItemProvider publicISelectionItemProvider;

		// Token: 0x040004A2 RID: 1186
		private UnsafeNativeMethods.IScrollItemProvider publicIScrollItemProvider;

		// Token: 0x040004A3 RID: 1187
		private UnsafeNativeMethods.IRawElementProviderHwndOverride publicIRawElementProviderHwndOverride;
	}
}
