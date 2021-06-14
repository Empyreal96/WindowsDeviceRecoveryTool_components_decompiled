using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x02000799 RID: 1945
	[Serializable]
	internal class ReturnEventSaver
	{
		// Token: 0x060079DE RID: 31198 RVA: 0x0000326D File Offset: 0x0000146D
		internal ReturnEventSaver()
		{
		}

		// Token: 0x060079DF RID: 31199 RVA: 0x00228900 File Offset: 0x00226B00
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void _Detach(PageFunctionBase pf)
		{
			if (pf._Return != null && pf._Saver == null)
			{
				Delegate[] invocationList = pf._Return.GetInvocationList();
				ReturnEventSaverInfo[] array = this._returnList = new ReturnEventSaverInfo[invocationList.Length];
				for (int i = 0; i < invocationList.Length; i++)
				{
					Delegate @delegate = invocationList[i];
					bool fSamePf = false;
					if (@delegate.Target == pf)
					{
						fSamePf = true;
					}
					MethodInfo method = @delegate.Method;
					ReturnEventSaverInfo returnEventSaverInfo = new ReturnEventSaverInfo(@delegate.GetType().AssemblyQualifiedName, @delegate.Target.GetType().AssemblyQualifiedName, method.Name, fSamePf);
					array[i] = returnEventSaverInfo;
				}
				pf._Saver = this;
			}
			pf._DetachEvents();
		}

		// Token: 0x060079E0 RID: 31200 RVA: 0x002289B4 File Offset: 0x00226BB4
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal void _Attach(object caller, PageFunctionBase child)
		{
			ReturnEventSaverInfo[] array = null;
			array = this._returnList;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (string.Compare(this._returnList[i]._targetTypeName, caller.GetType().AssemblyQualifiedName, StringComparison.Ordinal) != 0)
					{
						throw new NotSupportedException(SR.Get("ReturnEventHandlerMustBeOnParentPage"));
					}
					Delegate d;
					try
					{
						new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
						d = Delegate.CreateDelegate(Type.GetType(this._returnList[i]._delegateTypeName), caller, this._returnList[i]._delegateMethodName);
					}
					catch (Exception innerException)
					{
						throw new NotSupportedException(SR.Get("ReturnEventHandlerMustBeOnParentPage"), innerException);
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					child._AddEventHandler(d);
				}
			}
		}

		// Token: 0x040039A5 RID: 14757
		[SecurityCritical]
		private ReturnEventSaverInfo[] _returnList;
	}
}
