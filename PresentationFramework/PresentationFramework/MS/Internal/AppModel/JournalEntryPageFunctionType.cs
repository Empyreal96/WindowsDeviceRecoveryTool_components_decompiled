using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace MS.Internal.AppModel
{
	// Token: 0x0200078D RID: 1933
	[Serializable]
	internal class JournalEntryPageFunctionType : JournalEntryPageFunctionSaver, ISerializable
	{
		// Token: 0x06007974 RID: 31092 RVA: 0x00227534 File Offset: 0x00225734
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal JournalEntryPageFunctionType(JournalEntryGroupState jeGroupState, PageFunctionBase pageFunction) : base(jeGroupState, pageFunction)
		{
			string assemblyQualifiedName = pageFunction.GetType().AssemblyQualifiedName;
			this._typeName = new SecurityCriticalDataForSet<string>(assemblyQualifiedName);
		}

		// Token: 0x06007975 RID: 31093 RVA: 0x00227561 File Offset: 0x00225761
		[SecurityCritical]
		protected JournalEntryPageFunctionType(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this._typeName = new SecurityCriticalDataForSet<string>(info.GetString("_typeName"));
		}

		// Token: 0x06007976 RID: 31094 RVA: 0x00227581 File Offset: 0x00225781
		[SecurityCritical]
		[SecurityPermission(SecurityAction.LinkDemand, SerializationFormatter = true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_typeName", this._typeName.Value);
		}

		// Token: 0x06007977 RID: 31095 RVA: 0x002275A1 File Offset: 0x002257A1
		internal override void SaveState(object contentObject)
		{
			base.SaveState(contentObject);
		}

		// Token: 0x06007978 RID: 31096 RVA: 0x002275AC File Offset: 0x002257AC
		[SecurityCritical]
		[SecurityTreatAsSafe]
		internal override PageFunctionBase ResumePageFunction()
		{
			Invariant.Assert(this._typeName.Value != null, "JournalEntry does not contain the Type for the PageFunction to be created");
			Type type = Type.GetType(this._typeName.Value);
			new ReflectionPermission(ReflectionPermissionFlag.MemberAccess).Assert();
			PageFunctionBase pageFunctionBase;
			try
			{
				pageFunctionBase = (PageFunctionBase)Activator.CreateInstance(type);
			}
			catch (Exception innerException)
			{
				throw new Exception(SR.Get("FailedResumePageFunction", new object[]
				{
					this._typeName.Value
				}), innerException);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			this.InitializeComponent(pageFunctionBase);
			this.RestoreState(pageFunctionBase);
			return pageFunctionBase;
		}

		// Token: 0x06007979 RID: 31097 RVA: 0x00227654 File Offset: 0x00225854
		private void InitializeComponent(PageFunctionBase pageFunction)
		{
			IComponentConnector componentConnector = pageFunction as IComponentConnector;
			if (componentConnector != null)
			{
				componentConnector.InitializeComponent();
			}
		}

		// Token: 0x04003985 RID: 14725
		private SecurityCriticalDataForSet<string> _typeName;
	}
}
