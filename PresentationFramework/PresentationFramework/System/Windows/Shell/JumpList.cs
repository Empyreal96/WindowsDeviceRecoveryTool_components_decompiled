using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Markup;
using MS.Internal;
using MS.Internal.AppModel;
using MS.Internal.Interop;
using MS.Win32;

namespace System.Windows.Shell
{
	/// <summary>Represents a list of items and tasks displayed as a menu on a Windows 7 taskbar button.</summary>
	// Token: 0x02000148 RID: 328
	[SecurityCritical]
	[ContentProperty("JumpItems")]
	[UIPermission(SecurityAction.LinkDemand, Window = UIPermissionWindow.AllWindows)]
	public sealed class JumpList : ISupportInitialize
	{
		/// <summary>Adds the specified item path to the Recent category of the Jump List.</summary>
		/// <param name="itemPath">The path to add to the Jump List.</param>
		// Token: 0x06000E5D RID: 3677 RVA: 0x00036F00 File Offset: 0x00035100
		[SecurityCritical]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public static void AddToRecentCategory(string itemPath)
		{
			Verify.FileExists(itemPath, "itemPath");
			itemPath = Path.GetFullPath(itemPath);
			NativeMethods2.SHAddToRecentDocs(itemPath);
		}

		/// <summary>Adds the specified jump path to the Recent category of the Jump List.</summary>
		/// <param name="jumpPath">The <see cref="T:System.Windows.Shell.JumpPath" /> to add to the Jump List.</param>
		// Token: 0x06000E5E RID: 3678 RVA: 0x00036F1B File Offset: 0x0003511B
		[SecurityCritical]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public static void AddToRecentCategory(JumpPath jumpPath)
		{
			Verify.IsNotNull<JumpPath>(jumpPath, "jumpPath");
			JumpList.AddToRecentCategory(jumpPath.Path);
		}

		/// <summary>Adds the specified jump task to the Recent category of the Jump List.</summary>
		/// <param name="jumpTask">The <see cref="T:System.Windows.Shell.JumpTask" /> to add to the Jump List.</param>
		// Token: 0x06000E5F RID: 3679 RVA: 0x00036F34 File Offset: 0x00035134
		[SecurityCritical]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public static void AddToRecentCategory(JumpTask jumpTask)
		{
			Verify.IsNotNull<JumpTask>(jumpTask, "jumpTask");
			if (Utilities.IsOSWindows7OrNewer)
			{
				IShellLinkW shellLinkW = JumpList.CreateLinkFromJumpTask(jumpTask, false);
				try
				{
					if (shellLinkW != null)
					{
						NativeMethods2.SHAddToRecentDocs(shellLinkW);
					}
				}
				finally
				{
					Utilities.SafeRelease<IShellLinkW>(ref shellLinkW);
				}
			}
		}

		/// <summary>Sets the <see cref="T:System.Windows.Shell.JumpList" /> object associated with an application.</summary>
		/// <param name="application">The application associated with the <see cref="T:System.Windows.Shell.JumpList" />.</param>
		/// <param name="value">The <see cref="T:System.Windows.Shell.JumpList" /> to associate with the application.</param>
		// Token: 0x06000E60 RID: 3680 RVA: 0x00036F80 File Offset: 0x00035180
		[SecuritySafeCritical]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public static void SetJumpList(Application application, JumpList value)
		{
			Verify.IsNotNull<Application>(application, "application");
			object obj = JumpList.s_lock;
			lock (obj)
			{
				JumpList jumpList;
				if (JumpList.s_applicationMap.TryGetValue(application, out jumpList) && jumpList != null)
				{
					jumpList._application = null;
				}
				JumpList.s_applicationMap[application] = value;
				if (value != null)
				{
					value._application = application;
				}
			}
			if (value != null)
			{
				value.ApplyFromApplication();
			}
		}

		/// <summary>Returns the <see cref="T:System.Windows.Shell.JumpList" /> object associated with an application.</summary>
		/// <param name="application">The application associated with the <see cref="T:System.Windows.Shell.JumpList" />.</param>
		/// <returns>The <see cref="T:System.Windows.Shell.JumpList" /> object associated with the specified application.</returns>
		// Token: 0x06000E61 RID: 3681 RVA: 0x00036FFC File Offset: 0x000351FC
		public static JumpList GetJumpList(Application application)
		{
			Verify.IsNotNull<Application>(application, "application");
			JumpList result;
			JumpList.s_applicationMap.TryGetValue(application, out result);
			return result;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Shell.JumpList" /> class.</summary>
		// Token: 0x06000E62 RID: 3682 RVA: 0x00037023 File Offset: 0x00035223
		[SecurityCritical]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public JumpList() : this(null, false, false)
		{
			this._initializing = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Windows.Shell.JumpList" /> class with the specified parameters.</summary>
		/// <param name="items">The collection of <see cref="T:System.Windows.Shell.JumpItem" /> objects that are displayed in the Jump List.</param>
		/// <param name="showFrequent">A value that indicates whether frequently used items are displayed in the Jump List.</param>
		/// <param name="showRecent">A value that indicates whether recently used items are displayed in the Jump List.</param>
		// Token: 0x06000E63 RID: 3683 RVA: 0x0003703A File Offset: 0x0003523A
		[SecurityCritical]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public JumpList(IEnumerable<JumpItem> items, bool showFrequent, bool showRecent)
		{
			if (items != null)
			{
				this._jumpItems = new List<JumpItem>(items);
			}
			else
			{
				this._jumpItems = new List<JumpItem>();
			}
			this.ShowFrequentCategory = showFrequent;
			this.ShowRecentCategory = showRecent;
			this._initializing = new bool?(false);
		}

		/// <summary>Gets or sets a value that indicates whether frequently used items are displayed in the Jump List.</summary>
		/// <returns>
		///     <see langword="true" /> if frequently used items are displayed in the Jump List; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06000E64 RID: 3684 RVA: 0x00037078 File Offset: 0x00035278
		// (set) Token: 0x06000E65 RID: 3685 RVA: 0x00037080 File Offset: 0x00035280
		public bool ShowFrequentCategory { [SecurityCritical] [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)] get; [SecurityCritical] [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)] set; }

		/// <summary>Gets or sets a value that indicates whether recently used items are displayed in the Jump List.</summary>
		/// <returns>
		///     <see langword="true" /> if recently used items are displayed in the Jump List; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06000E66 RID: 3686 RVA: 0x00037089 File Offset: 0x00035289
		// (set) Token: 0x06000E67 RID: 3687 RVA: 0x00037091 File Offset: 0x00035291
		public bool ShowRecentCategory { [SecurityCritical] [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)] get; [SecurityCritical] [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)] set; }

		/// <summary>Gets the collection of <see cref="T:System.Windows.Shell.JumpItem" /> objects that are displayed in the Jump List.</summary>
		/// <returns>The collection of <see cref="T:System.Windows.Shell.JumpItem" /> objects displayed in the Jump List. The default is an empty collection.</returns>
		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06000E68 RID: 3688 RVA: 0x0003709A File Offset: 0x0003529A
		public List<JumpItem> JumpItems
		{
			get
			{
				return this._jumpItems;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06000E69 RID: 3689 RVA: 0x000370A2 File Offset: 0x000352A2
		private bool IsUnmodified
		{
			[SecurityCritical]
			get
			{
				return this._initializing == null && this.JumpItems.Count == 0 && !this.ShowRecentCategory && !this.ShowFrequentCategory;
			}
		}

		/// <summary>Signals the start of the <see cref="T:System.Windows.Shell.JumpList" /> initialization.</summary>
		/// <exception cref="T:System.InvalidOperationException">This call to <see cref="M:System.Windows.Shell.JumpList.BeginInit" /> is nested in a previous call to <see cref="M:System.Windows.Shell.JumpList.BeginInit" />.</exception>
		// Token: 0x06000E6A RID: 3690 RVA: 0x000370D1 File Offset: 0x000352D1
		[SecurityCritical]
		[SecurityTreatAsSafe]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public void BeginInit()
		{
			if (!this.IsUnmodified)
			{
				throw new InvalidOperationException(SR.Get("JumpList_CantNestBeginInitCalls"));
			}
			this._initializing = new bool?(true);
		}

		/// <summary>Signals the end of the <see cref="T:System.Windows.Shell.JumpList" /> initialization.</summary>
		/// <exception cref="T:System.NotSupportedException">This call to <see cref="M:System.Windows.Shell.JumpList.EndInit" /> is not paired with a call to <see cref="M:System.Windows.Shell.JumpList.BeginInit" />.</exception>
		// Token: 0x06000E6B RID: 3691 RVA: 0x000370F8 File Offset: 0x000352F8
		[SecurityCritical]
		[SecurityTreatAsSafe]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public void EndInit()
		{
			if (this._initializing != true)
			{
				throw new NotSupportedException(SR.Get("JumpList_CantCallUnbalancedEndInit"));
			}
			this._initializing = new bool?(false);
			this.ApplyFromApplication();
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06000E6C RID: 3692 RVA: 0x0003714C File Offset: 0x0003534C
		private static string _RuntimeId
		{
			[SecurityCritical]
			get
			{
				string result;
				HRESULT hrLeft = NativeMethods2.GetCurrentProcessExplicitAppUserModelID(out result);
				if (hrLeft == HRESULT.E_FAIL)
				{
					hrLeft = HRESULT.S_OK;
					result = null;
				}
				hrLeft.ThrowIfFailed();
				return result;
			}
		}

		/// <summary>Sends the <see cref="T:System.Windows.Shell.JumpList" /> to the Windows shell in its current state.</summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Windows.Shell.JumpList" /> is not completely initialized.</exception>
		// Token: 0x06000E6D RID: 3693 RVA: 0x00037180 File Offset: 0x00035380
		[SecurityCritical]
		[UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
		public void Apply()
		{
			if (this._initializing == true)
			{
				throw new InvalidOperationException(SR.Get("JumpList_CantApplyUntilEndInit"));
			}
			this._initializing = new bool?(false);
			this.ApplyList();
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x000371D0 File Offset: 0x000353D0
		[SecurityCritical]
		private void ApplyFromApplication()
		{
			if (this._initializing != true && !this.IsUnmodified)
			{
				this._initializing = new bool?(false);
			}
			if (this._application == Application.Current && this._initializing == false)
			{
				this.ApplyList();
			}
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00037248 File Offset: 0x00035448
		[SecurityCritical]
		private void ApplyList()
		{
			Verify.IsApartmentState(ApartmentState.STA);
			if (!Utilities.IsOSWindows7OrNewer)
			{
				this.RejectEverything();
				return;
			}
			List<List<JumpList._ShellObjectPair>> list = null;
			List<JumpList._ShellObjectPair> list2 = null;
			ICustomDestinationList customDestinationList = (ICustomDestinationList)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("77f10cf0-3db5-4966-b520-b7c54fd35ed6")));
			List<JumpItem> list3;
			List<JumpList._RejectedJumpItemPair> list4;
			try
			{
				string runtimeId = JumpList._RuntimeId;
				if (!string.IsNullOrEmpty(runtimeId))
				{
					customDestinationList.SetAppID(runtimeId);
				}
				Guid guid = new Guid("92CA9DCD-5622-4bba-A805-5E9F541BD8C9");
				uint num;
				IObjectArray shellObjects = (IObjectArray)customDestinationList.BeginList(out num, ref guid);
				list2 = JumpList.GenerateJumpItems(shellObjects);
				list3 = new List<JumpItem>(this.JumpItems.Count);
				list4 = new List<JumpList._RejectedJumpItemPair>(this.JumpItems.Count);
				list = new List<List<JumpList._ShellObjectPair>>
				{
					new List<JumpList._ShellObjectPair>()
				};
				foreach (JumpItem jumpItem in this.JumpItems)
				{
					if (jumpItem == null)
					{
						list4.Add(new JumpList._RejectedJumpItemPair
						{
							JumpItem = jumpItem,
							Reason = JumpItemRejectionReason.InvalidItem
						});
					}
					else
					{
						object obj = null;
						try
						{
							obj = JumpList.GetShellObjectForJumpItem(jumpItem);
							if (obj == null)
							{
								list4.Add(new JumpList._RejectedJumpItemPair
								{
									Reason = JumpItemRejectionReason.InvalidItem,
									JumpItem = jumpItem
								});
							}
							else if (JumpList.ListContainsShellObject(list2, obj))
							{
								list4.Add(new JumpList._RejectedJumpItemPair
								{
									Reason = JumpItemRejectionReason.RemovedByUser,
									JumpItem = jumpItem
								});
							}
							else
							{
								JumpList._ShellObjectPair item = new JumpList._ShellObjectPair
								{
									JumpItem = jumpItem,
									ShellObject = obj
								};
								if (string.IsNullOrEmpty(jumpItem.CustomCategory))
								{
									list[0].Add(item);
								}
								else
								{
									bool flag = false;
									foreach (List<JumpList._ShellObjectPair> list5 in list)
									{
										if (list5.Count > 0 && list5[0].JumpItem.CustomCategory == jumpItem.CustomCategory)
										{
											list5.Add(item);
											flag = true;
											break;
										}
									}
									if (!flag)
									{
										list.Add(new List<JumpList._ShellObjectPair>
										{
											item
										});
									}
								}
								obj = null;
							}
						}
						finally
						{
							Utilities.SafeRelease<object>(ref obj);
						}
					}
				}
				list.Reverse();
				if (this.ShowFrequentCategory)
				{
					customDestinationList.AppendKnownCategory(KDC.FREQUENT);
				}
				if (this.ShowRecentCategory)
				{
					customDestinationList.AppendKnownCategory(KDC.RECENT);
				}
				foreach (List<JumpList._ShellObjectPair> list6 in list)
				{
					if (list6.Count > 0)
					{
						string customCategory = list6[0].JumpItem.CustomCategory;
						JumpList.AddCategory(customDestinationList, customCategory, list6, list3, list4);
					}
				}
				customDestinationList.CommitList();
			}
			catch
			{
				if (TraceShell.IsEnabled)
				{
					TraceShell.Trace(TraceEventType.Error, TraceShell.RejectingJumpItemsBecauseCatastrophicFailure);
				}
				this.RejectEverything();
				return;
			}
			finally
			{
				Utilities.SafeRelease<ICustomDestinationList>(ref customDestinationList);
				if (list != null)
				{
					foreach (List<JumpList._ShellObjectPair> list7 in list)
					{
						JumpList._ShellObjectPair.ReleaseShellObjects(list7);
					}
				}
				JumpList._ShellObjectPair.ReleaseShellObjects(list2);
			}
			list3.Reverse();
			this._jumpItems = list3;
			EventHandler<JumpItemsRejectedEventArgs> jumpItemsRejected = this.JumpItemsRejected;
			EventHandler<JumpItemsRemovedEventArgs> jumpItemsRemovedByUser = this.JumpItemsRemovedByUser;
			if (list4.Count > 0 && jumpItemsRejected != null)
			{
				List<JumpItem> list8 = new List<JumpItem>(list4.Count);
				List<JumpItemRejectionReason> list9 = new List<JumpItemRejectionReason>(list4.Count);
				foreach (JumpList._RejectedJumpItemPair rejectedJumpItemPair in list4)
				{
					list8.Add(rejectedJumpItemPair.JumpItem);
					list9.Add(rejectedJumpItemPair.Reason);
				}
				jumpItemsRejected(this, new JumpItemsRejectedEventArgs(list8, list9));
			}
			if (list2.Count > 0 && jumpItemsRemovedByUser != null)
			{
				List<JumpItem> list10 = new List<JumpItem>(list2.Count);
				foreach (JumpList._ShellObjectPair shellObjectPair in list2)
				{
					if (shellObjectPair.JumpItem != null)
					{
						list10.Add(shellObjectPair.JumpItem);
					}
				}
				if (list10.Count > 0)
				{
					jumpItemsRemovedByUser(this, new JumpItemsRemovedEventArgs(list10));
				}
			}
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00037744 File Offset: 0x00035944
		[SecurityCritical]
		private static bool ListContainsShellObject(List<JumpList._ShellObjectPair> removedList, object shellObject)
		{
			if (removedList.Count == 0)
			{
				return false;
			}
			IShellItem shellItem = shellObject as IShellItem;
			if (shellItem != null)
			{
				foreach (JumpList._ShellObjectPair shellObjectPair in removedList)
				{
					IShellItem shellItem2 = shellObjectPair.ShellObject as IShellItem;
					if (shellItem2 != null && shellItem.Compare(shellItem2, SICHINT.CANONICAL | SICHINT.TEST_FILESYSPATH_IF_NOT_EQUAL) == 0)
					{
						return true;
					}
				}
				return false;
			}
			IShellLinkW shellLinkW = shellObject as IShellLinkW;
			if (shellLinkW != null)
			{
				foreach (JumpList._ShellObjectPair shellObjectPair2 in removedList)
				{
					IShellLinkW shellLinkW2 = shellObjectPair2.ShellObject as IShellLinkW;
					if (shellLinkW2 != null)
					{
						string a = JumpList.ShellLinkToString(shellLinkW2);
						string b = JumpList.ShellLinkToString(shellLinkW);
						if (a == b)
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00037844 File Offset: 0x00035A44
		[SecurityCritical]
		private static object GetShellObjectForJumpItem(JumpItem jumpItem)
		{
			JumpPath jumpPath = jumpItem as JumpPath;
			JumpTask jumpTask = jumpItem as JumpTask;
			if (jumpPath != null)
			{
				return JumpList.CreateItemFromJumpPath(jumpPath);
			}
			if (jumpTask != null)
			{
				return JumpList.CreateLinkFromJumpTask(jumpTask, true);
			}
			return null;
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x00037878 File Offset: 0x00035A78
		[SecurityCritical]
		private static List<JumpList._ShellObjectPair> GenerateJumpItems(IObjectArray shellObjects)
		{
			List<JumpList._ShellObjectPair> list = new List<JumpList._ShellObjectPair>();
			Guid guid = new Guid("00000000-0000-0000-C000-000000000046");
			uint count = shellObjects.GetCount();
			for (uint num = 0U; num < count; num += 1U)
			{
				object at = shellObjects.GetAt(num, ref guid);
				JumpItem jumpItem = null;
				try
				{
					jumpItem = JumpList.GetJumpItemForShellObject(at);
				}
				catch (Exception ex)
				{
					if (ex is NullReferenceException || ex is SEHException)
					{
						throw;
					}
				}
				list.Add(new JumpList._ShellObjectPair
				{
					ShellObject = at,
					JumpItem = jumpItem
				});
			}
			return list;
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x00037908 File Offset: 0x00035B08
		[SecurityCritical]
		private static void AddCategory(ICustomDestinationList cdl, string category, List<JumpList._ShellObjectPair> jumpItems, List<JumpItem> successList, List<JumpList._RejectedJumpItemPair> rejectionList)
		{
			JumpList.AddCategory(cdl, category, jumpItems, successList, rejectionList, true);
		}

		// Token: 0x06000E74 RID: 3700 RVA: 0x00037918 File Offset: 0x00035B18
		[SecurityCritical]
		private static void AddCategory(ICustomDestinationList cdl, string category, List<JumpList._ShellObjectPair> jumpItems, List<JumpItem> successList, List<JumpList._RejectedJumpItemPair> rejectionList, bool isHeterogenous)
		{
			IObjectCollection objectCollection = (IObjectCollection)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("2d3468c1-36a7-43b6-ac24-d3f02fd9607a")));
			foreach (JumpList._ShellObjectPair shellObjectPair in jumpItems)
			{
				objectCollection.AddObject(shellObjectPair.ShellObject);
			}
			HRESULT hrLeft;
			if (string.IsNullOrEmpty(category))
			{
				hrLeft = cdl.AddUserTasks(objectCollection);
			}
			else
			{
				hrLeft = cdl.AppendCategory(category, objectCollection);
			}
			if (hrLeft.Succeeded)
			{
				int num = jumpItems.Count;
				while (--num >= 0)
				{
					successList.Add(jumpItems[num].JumpItem);
				}
				return;
			}
			if (isHeterogenous && hrLeft == HRESULT.DESTS_E_NO_MATCHING_ASSOC_HANDLER)
			{
				if (TraceShell.IsEnabled)
				{
					TraceShell.Trace(TraceEventType.Error, TraceShell.RejectingJumpListCategoryBecauseNoRegisteredHandler(new object[]
					{
						category
					}));
				}
				Utilities.SafeRelease<IObjectCollection>(ref objectCollection);
				List<JumpList._ShellObjectPair> list = new List<JumpList._ShellObjectPair>();
				foreach (JumpList._ShellObjectPair shellObjectPair2 in jumpItems)
				{
					if (shellObjectPair2.JumpItem is JumpPath)
					{
						rejectionList.Add(new JumpList._RejectedJumpItemPair
						{
							JumpItem = shellObjectPair2.JumpItem,
							Reason = JumpItemRejectionReason.NoRegisteredHandler
						});
					}
					else
					{
						list.Add(shellObjectPair2);
					}
				}
				if (list.Count > 0)
				{
					JumpList.AddCategory(cdl, category, list, successList, rejectionList, false);
					return;
				}
			}
			else
			{
				foreach (JumpList._ShellObjectPair shellObjectPair3 in jumpItems)
				{
					rejectionList.Add(new JumpList._RejectedJumpItemPair
					{
						JumpItem = shellObjectPair3.JumpItem,
						Reason = JumpItemRejectionReason.InvalidItem
					});
				}
			}
		}

		// Token: 0x06000E75 RID: 3701 RVA: 0x00037AF4 File Offset: 0x00035CF4
		[SecurityCritical]
		private static IShellLinkW CreateLinkFromJumpTask(JumpTask jumpTask, bool allowSeparators)
		{
			if (string.IsNullOrEmpty(jumpTask.Title) && (!allowSeparators || !string.IsNullOrEmpty(jumpTask.CustomCategory)))
			{
				return null;
			}
			IShellLinkW shellLinkW = (IShellLinkW)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00021401-0000-0000-C000-000000000046")));
			IShellLinkW result;
			try
			{
				string path = JumpList._FullName;
				if (!string.IsNullOrEmpty(jumpTask.ApplicationPath))
				{
					path = jumpTask.ApplicationPath;
				}
				shellLinkW.SetPath(path);
				if (!string.IsNullOrEmpty(jumpTask.WorkingDirectory))
				{
					shellLinkW.SetWorkingDirectory(jumpTask.WorkingDirectory);
				}
				if (!string.IsNullOrEmpty(jumpTask.Arguments))
				{
					shellLinkW.SetArguments(jumpTask.Arguments);
				}
				if (jumpTask.IconResourceIndex != -1)
				{
					string pszIconPath = JumpList._FullName;
					if (!string.IsNullOrEmpty(jumpTask.IconResourcePath))
					{
						if (jumpTask.IconResourcePath.Length >= 260)
						{
							return null;
						}
						pszIconPath = jumpTask.IconResourcePath;
					}
					shellLinkW.SetIconLocation(pszIconPath, jumpTask.IconResourceIndex);
				}
				if (!string.IsNullOrEmpty(jumpTask.Description))
				{
					shellLinkW.SetDescription(jumpTask.Description);
				}
				IPropertyStore propertyStore = (IPropertyStore)shellLinkW;
				PROPVARIANT propvariant = new PROPVARIANT();
				try
				{
					PKEY pkey = default(PKEY);
					if (!string.IsNullOrEmpty(jumpTask.Title))
					{
						propvariant.SetValue(jumpTask.Title);
						pkey = PKEY.Title;
					}
					else
					{
						propvariant.SetValue(true);
						pkey = PKEY.AppUserModel_IsDestListSeparator;
					}
					propertyStore.SetValue(ref pkey, propvariant);
				}
				finally
				{
					Utilities.SafeDispose<PROPVARIANT>(ref propvariant);
				}
				propertyStore.Commit();
				IShellLinkW shellLinkW2 = shellLinkW;
				shellLinkW = null;
				result = shellLinkW2;
			}
			catch (Exception)
			{
				result = null;
			}
			finally
			{
				Utilities.SafeRelease<IShellLinkW>(ref shellLinkW);
			}
			return result;
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00037CB8 File Offset: 0x00035EB8
		[SecurityCritical]
		private static IShellItem2 CreateItemFromJumpPath(JumpPath jumpPath)
		{
			try
			{
				return ShellUtil.GetShellItemForPath(Path.GetFullPath(jumpPath.Path));
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00037CF0 File Offset: 0x00035EF0
		[SecurityCritical]
		private static JumpItem GetJumpItemForShellObject(object shellObject)
		{
			IShellItem2 shellItem = shellObject as IShellItem2;
			IShellLinkW shellLinkW = shellObject as IShellLinkW;
			if (shellItem != null)
			{
				return new JumpPath
				{
					Path = shellItem.GetDisplayName((SIGDN)2147647488U)
				};
			}
			if (shellLinkW != null)
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				shellLinkW.GetPath(stringBuilder, stringBuilder.Capacity, null, SLGP.RAWPATH);
				StringBuilder stringBuilder2 = new StringBuilder(1024);
				shellLinkW.GetArguments(stringBuilder2, stringBuilder2.Capacity);
				StringBuilder stringBuilder3 = new StringBuilder(1024);
				shellLinkW.GetDescription(stringBuilder3, stringBuilder3.Capacity);
				StringBuilder stringBuilder4 = new StringBuilder(260);
				int iconResourceIndex;
				shellLinkW.GetIconLocation(stringBuilder4, stringBuilder4.Capacity, out iconResourceIndex);
				StringBuilder stringBuilder5 = new StringBuilder(260);
				shellLinkW.GetWorkingDirectory(stringBuilder5, stringBuilder5.Capacity);
				JumpTask jumpTask = new JumpTask
				{
					ApplicationPath = stringBuilder.ToString(),
					Arguments = stringBuilder2.ToString(),
					Description = stringBuilder3.ToString(),
					IconResourceIndex = iconResourceIndex,
					IconResourcePath = stringBuilder4.ToString(),
					WorkingDirectory = stringBuilder5.ToString()
				};
				using (PROPVARIANT propvariant = new PROPVARIANT())
				{
					IPropertyStore propertyStore = (IPropertyStore)shellLinkW;
					PKEY title = PKEY.Title;
					propertyStore.GetValue(ref title, propvariant);
					jumpTask.Title = (propvariant.GetValue() ?? "");
				}
				return jumpTask;
			}
			return null;
		}

		// Token: 0x06000E78 RID: 3704 RVA: 0x00037E60 File Offset: 0x00036060
		[SecurityCritical]
		private static string ShellLinkToString(IShellLinkW shellLink)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			shellLink.GetPath(stringBuilder, stringBuilder.Capacity, null, SLGP.RAWPATH);
			string text = null;
			using (PROPVARIANT propvariant = new PROPVARIANT())
			{
				IPropertyStore propertyStore = (IPropertyStore)shellLink;
				PKEY title = PKEY.Title;
				propertyStore.GetValue(ref title, propvariant);
				text = (propvariant.GetValue() ?? "");
			}
			StringBuilder stringBuilder2 = new StringBuilder(1024);
			shellLink.GetArguments(stringBuilder2, stringBuilder2.Capacity);
			return stringBuilder.ToString().ToUpperInvariant() + text.ToUpperInvariant() + stringBuilder2.ToString();
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x00037F08 File Offset: 0x00036108
		private void RejectEverything()
		{
			EventHandler<JumpItemsRejectedEventArgs> jumpItemsRejected = this.JumpItemsRejected;
			if (jumpItemsRejected == null)
			{
				this._jumpItems.Clear();
				return;
			}
			if (this._jumpItems.Count > 0)
			{
				List<JumpItemRejectionReason> list = new List<JumpItemRejectionReason>(this._jumpItems.Count);
				for (int i = 0; i < this._jumpItems.Count; i++)
				{
					list.Add(JumpItemRejectionReason.InvalidItem);
				}
				JumpItemsRejectedEventArgs e = new JumpItemsRejectedEventArgs(this._jumpItems, list);
				this._jumpItems.Clear();
				jumpItemsRejected(this, e);
			}
		}

		/// <summary>Occurs when jump items are not successfully added to the Jump List by the Windows shell.</summary>
		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06000E7A RID: 3706 RVA: 0x00037F88 File Offset: 0x00036188
		// (remove) Token: 0x06000E7B RID: 3707 RVA: 0x00037FC0 File Offset: 0x000361C0
		public event EventHandler<JumpItemsRejectedEventArgs> JumpItemsRejected;

		/// <summary>Occurs when jump items previously in the Jump List are removed from the list by the user.</summary>
		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000E7C RID: 3708 RVA: 0x00037FF8 File Offset: 0x000361F8
		// (remove) Token: 0x06000E7D RID: 3709 RVA: 0x00038030 File Offset: 0x00036230
		public event EventHandler<JumpItemsRemovedEventArgs> JumpItemsRemovedByUser;

		// Token: 0x04001125 RID: 4389
		private static readonly object s_lock = new object();

		// Token: 0x04001126 RID: 4390
		private static readonly Dictionary<Application, JumpList> s_applicationMap = new Dictionary<Application, JumpList>();

		// Token: 0x04001127 RID: 4391
		private Application _application;

		// Token: 0x04001128 RID: 4392
		private bool? _initializing;

		// Token: 0x04001129 RID: 4393
		private List<JumpItem> _jumpItems;

		// Token: 0x0400112C RID: 4396
		[SecurityCritical]
		private static readonly string _FullName = UnsafeNativeMethods.GetModuleFileName(default(HandleRef));

		// Token: 0x0200083C RID: 2108
		private class _RejectedJumpItemPair
		{
			// Token: 0x17001D87 RID: 7559
			// (get) Token: 0x06007EE7 RID: 32487 RVA: 0x00237306 File Offset: 0x00235506
			// (set) Token: 0x06007EE8 RID: 32488 RVA: 0x0023730E File Offset: 0x0023550E
			public JumpItem JumpItem { get; set; }

			// Token: 0x17001D88 RID: 7560
			// (get) Token: 0x06007EE9 RID: 32489 RVA: 0x00237317 File Offset: 0x00235517
			// (set) Token: 0x06007EEA RID: 32490 RVA: 0x0023731F File Offset: 0x0023551F
			public JumpItemRejectionReason Reason { get; set; }
		}

		// Token: 0x0200083D RID: 2109
		private class _ShellObjectPair
		{
			// Token: 0x17001D89 RID: 7561
			// (get) Token: 0x06007EEC RID: 32492 RVA: 0x00237328 File Offset: 0x00235528
			// (set) Token: 0x06007EED RID: 32493 RVA: 0x00237330 File Offset: 0x00235530
			public JumpItem JumpItem { get; set; }

			// Token: 0x17001D8A RID: 7562
			// (get) Token: 0x06007EEE RID: 32494 RVA: 0x00237339 File Offset: 0x00235539
			// (set) Token: 0x06007EEF RID: 32495 RVA: 0x00237341 File Offset: 0x00235541
			public object ShellObject { get; set; }

			// Token: 0x06007EF0 RID: 32496 RVA: 0x0023734C File Offset: 0x0023554C
			[SecurityCritical]
			public static void ReleaseShellObjects(List<JumpList._ShellObjectPair> list)
			{
				if (list != null)
				{
					foreach (JumpList._ShellObjectPair shellObjectPair in list)
					{
						object shellObject = shellObjectPair.ShellObject;
						shellObjectPair.ShellObject = null;
						Utilities.SafeRelease<object>(ref shellObject);
					}
				}
			}
		}
	}
}
