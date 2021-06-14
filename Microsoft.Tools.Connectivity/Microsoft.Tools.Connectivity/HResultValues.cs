using System;
using System.ComponentModel;

namespace Microsoft.Tools.Connectivity
{
	// Token: 0x02000006 RID: 6
	internal enum HResultValues
	{
		// Token: 0x04000016 RID: 22
		[Description("The operation completed successfully.")]
		S_OK,
		// Token: 0x04000017 RID: 23
		[Description("This function is not valid on this platform.")]
		E_NOTIMPL = -2147467263,
		// Token: 0x04000018 RID: 24
		[Description("No such interface supported.")]
		E_NOINTERFACE,
		// Token: 0x04000019 RID: 25
		[Description("The pointer is invalid.")]
		E_POINTER,
		// Token: 0x0400001A RID: 26
		[Description("Operation aborted.")]
		E_ABORT,
		// Token: 0x0400001B RID: 27
		[Description("Unspecified error.")]
		E_FAIL,
		// Token: 0x0400001C RID: 28
		[Description("The handle is invalid.")]
		E_HANDLE,
		// Token: 0x0400001D RID: 29
		[Description("File not found.")]
		E_FILENOTFOUND = -2147024894,
		// Token: 0x0400001E RID: 30
		[Description("Access is denied.")]
		E_ACCESSDENIED = -2147024891,
		// Token: 0x0400001F RID: 31
		[Description("Not enough storage is available to complete this operation.")]
		E_OUTOFMEMORY = -2147024882,
		// Token: 0x04000020 RID: 32
		[Description("The file exists.")]
		E_FILEEXISTS = -2147024816,
		// Token: 0x04000021 RID: 33
		[Description("The argument is invalid.")]
		E_INVALIDARG = -2147024809,
		// Token: 0x04000022 RID: 34
		[Description("The application cannot be run in Win32 mode.")]
		E_CHILDNOTCOMPLETE = -2147024767,
		// Token: 0x04000023 RID: 35
		[Description("The data necessary to complete this operation is not yet available.")]
		E_PENDING = -2147483638,
		// Token: 0x04000024 RID: 36
		[Description("The directory name is invalid.")]
		E_DIRECTORY = -2147024629,
		// Token: 0x04000025 RID: 37
		[Description("This operation returned because the timeout period expired.")]
		E_TIMEOUT = -2147023436,
		// Token: 0x04000026 RID: 38
		[Description("This drive is locked by BitLocker Drive Encryption.")]
		FVE_E_LOCKED_VOLUME = -2144272384,
		// Token: 0x04000027 RID: 39
		[Description("A matching SirepClient.dll is not registered on your machine.")]
		E_CLASSNOTREGISTERED = -2147221164,
		// Token: 0x04000028 RID: 40
		[Description("The surrogate failed to load the DLL requested in the RPC call.")]
		RPC_E_SURROGATE_FAILED_DLL_LOAD = -1988934655,
		// Token: 0x04000029 RID: 41
		[Description("The surrogate loaded the reqested DLL, but failed to load the requested function in the DLL.")]
		RPC_E_SURROGATE_FAILED_FUNC_LOAD,
		// Token: 0x0400002A RID: 42
		[Description("Function name passed into surrogate has invalid characters preventing attempt to call GetProcAddress.")]
		RPC_E_SURROGATE_BAD_FUNCNAME,
		// Token: 0x0400002B RID: 43
		[Description("The surrogate successfully created but failed to init an RPC descriptor when getting ready to execute RPC call; possible bogus data passed in from desktop side.")]
		RPC_E_SURROGATE_RPCDESCRIPTOR_INIT,
		// Token: 0x0400002C RID: 44
		[Description("The surrogate could not create instance of RPCDescriptor (out of memory?).")]
		RPC_E_SURROGATE_NULL_RPCDESCRIPTOR,
		// Token: 0x0400002D RID: 45
		[Description("The surrogate had a problem calling TransactionManager->ReadObsolete().")]
		RPC_E_TM_READ_FAIL,
		// Token: 0x0400002E RID: 46
		[Description("The user asked RPC_DLL to call a function whose name has length 0.")]
		RPC_E_RPCDLL_ZERO_LENGTH_FUNCNAME,
		// Token: 0x0400002F RID: 47
		[Description("The user asked RPC_DLL to call a function in a dll with name of length 0.")]
		RPC_E_RPCDLL_ZERO_LENGTH_DLLNAME,
		// Token: 0x04000030 RID: 48
		[Description("The RPC_DLL could not create instance of RPCDescriptor (out of memory?).")]
		RPC_E_RPCDLL_NULL_RPCDESCRIPTOR,
		// Token: 0x04000031 RID: 49
		[Description("The RPC_DLL could not connect to the surrogate - Reason unknown.")]
		RPC_E_RPCDLL_COULDNOTCONNECT,
		// Token: 0x04000032 RID: 50
		[Description("The RPC_DLL tried to create request bytes for transmit, but they were NULL or of length 0.")]
		RPC_E_RPCDLL_MALFORMEDREQUEST,
		// Token: 0x04000033 RID: 51
		[Description("The RPC_DLL tried to do CreateTransaction to transmit an RPC request, but CreateTransaction failed.")]
		RPC_E_RPCDLL_CONNECTTRANSACTION,
		// Token: 0x04000034 RID: 52
		[Description("The RPC_DLL failed when trying to do a TransactionManager->Write() call.")]
		RPC_E_RPCDLL_TMWRITE_FAIL,
		// Token: 0x04000035 RID: 53
		[Description("The RPC_DLL has been disconnected.")]
		RPC_E_RPCDLL_DISCONNECTED,
		// Token: 0x04000036 RID: 54
		[Description("The SurrogateStatusTracker failed in AddDLLForEntry() - No empty slots?")]
		RPC_E_SST_ADDDLLFORENTRY_FAIL,
		// Token: 0x04000037 RID: 55
		[Description("SurrogateStatusTracker failed in GetDLLHInstance because no matching HINSTANCE found.")]
		RPC_E_SST_GETHINST_BADHINSTANCE,
		// Token: 0x04000038 RID: 56
		[Description("SurrogateStatusTracker failed in GetDLLHinstance because no matching DLLName found.")]
		RPC_E_SST_GETHINST_BADDLLNAME,
		// Token: 0x04000039 RID: 57
		[Description("The ClientConnectionTracker could not connect to surrogate, even though RPC bits were re-deployed successfully.")]
		RPC_E_CCT_COULDNOTCONNECT,
		// Token: 0x0400003A RID: 58
		[Description("The ClientConnectionTracker tried to deploy RPC bits and failed.")]
		RPC_E_CCT_DEPLOYRPC_FAIL,
		// Token: 0x0400003B RID: 59
		[Description("The ClientConnectionTracker could not figure out the path to the RPC bits for deployment.")]
		RPC_E_CCT_RESOLVEDEPLOYPATH_FAIL,
		// Token: 0x0400003C RID: 60
		[Description("The ClientConnectionTracker could not do CoCreateInstance to get IID_IConmanServer.")]
		RPC_E_CCT_DEPLOYRPC_CCINSTANCE,
		// Token: 0x0400003D RID: 61
		[Description("The ClientConnectionTracker in DeployRPCBits failed to do CoInitializeEx.")]
		RPC_E_CCT_DEPLOYRPC_CINITEX,
		// Token: 0x0400003E RID: 62
		[Description("No surrogate connections are registered in the ClientConnectionTracker for the DLL name passed as an argument.")]
		RPC_E_CCT_BADDLLNAME,
		// Token: 0x0400003F RID: 63
		[Description("The inArgs argument to RPC_Call may not be NULL if the inArgLength argument is greater than 0.")]
		RPC_E_RPCCALL_NULLINARGS,
		// Token: 0x04000040 RID: 64
		[Description("The path to the transports, as stored in the registry and used by ClientConnectionTracker, is too long.")]
		RPC_E_CCT_PATHTOTRANSPORT_TOOLONG,
		// Token: 0x04000041 RID: 65
		[Description("ClientConnectionTracker had an unexpected failure when accessing the registry.")]
		RPC_E_CCT_REGISTRY_ERROR,
		// Token: 0x04000042 RID: 66
		[Description("The RPC Server thread failed to launch the surrogate; ConmanSurrogate.exe is probably not on device.")]
		RPC_E_RPCSVR_START_SURROGATE_FAIL,
		// Token: 0x04000043 RID: 67
		[Description("Things that are not yet implemented can return this value.")]
		RPC_E_NOT_YET_IMPL,
		// Token: 0x04000044 RID: 68
		[Description("RPC Server cannot quit because it is activly starting.")]
		RPC_E_RPCSERVER_STARTING,
		// Token: 0x04000045 RID: 69
		[Description("RPC Server knows it should quit, but has not yet made it into the HasQuit state.")]
		RPC_E_RPCSERVER_SHOULDQUIT,
		// Token: 0x04000046 RID: 70
		[Description("Given a DLL name, no surrogate was found listening on a port that can field RPC calls into that DLL.")]
		RPC_E_RPCSERVER_NO_PORT_FOR_DLL,
		// Token: 0x04000047 RID: 71
		[Description("Although the OS says it launched a surrogate, its presence cannot be detected by the RPCServer.")]
		RPC_E_RPCSVR_DETECT_SURROGATE_FAIL,
		// Token: 0x04000048 RID: 72
		[Description("The RPC System was asked to use a port that is beyond the legal range.")]
		RPC_E_PORT_OUTOFRANGE,
		// Token: 0x04000049 RID: 73
		[Description("The SurrogateStatusEntry failed to find the requested DLL SLOT.")]
		RPC_E_SSE_DLLSLOT_NOTFOUND,
		// Token: 0x0400004A RID: 74
		[Description("The SurrogateStatusEntry failed to find an empty DLL SLOT.")]
		RPC_E_SSE_EMPTYSLOT_NOTFOUND,
		// Token: 0x0400004B RID: 75
		[Description("SurrogateStatusTracker failed invariant check.")]
		RPC_E_SST_INVARIANT,
		// Token: 0x0400004C RID: 76
		[Description("No more empty entries are left in the surrogatestatustracker - too many surrogates running?")]
		RPC_E_SST_NOMOREENTRIES,
		// Token: 0x0400004D RID: 77
		[Description("The SurrogateStatusTracker was unable to get a handle to to the shared memory file map view.")]
		RPC_E_SST_CANNOT_GET_MAPVIEW,
		// Token: 0x0400004E RID: 78
		[Description("Attempts to get access to the shared memory area failed for reasons unknown.")]
		RPC_E_SST_CANNOT_GET_SHAREDMEM,
		// Token: 0x0400004F RID: 79
		[Description("RPCServerREquestHandler failed its invariant check.")]
		RPC_E_SVRREQHANDLER_INVARIANT,
		// Token: 0x04000050 RID: 80
		[Description("ClientConnectionTracker failed its invariant check.")]
		RPC_E_CCT_INVARIANT,
		// Token: 0x04000051 RID: 81
		[Description("ClientCOnnectionTracker has no more slots available for cached TM connections.")]
		E_RPC_CCT_TM_CACHE_FULL,
		// Token: 0x04000052 RID: 82
		[Description("Surrogate could not create threads for the thread pool.")]
		E_RPC_SURROGATE_CREATETHREADS,
		// Token: 0x04000053 RID: 83
		[Description("Surrogate could not find a free slot for a guid representing an RPC call.")]
		E_RPC_NOEMPTYGUIDSLOT,
		// Token: 0x04000054 RID: 84
		[Description("SurrogateCommunicationManager flunked its invariant check.")]
		E_RPC_SURROGATECOMMMGR_INVARIANT,
		// Token: 0x04000055 RID: 85
		[Description("SurrogateComminicationManager failed to Init propertly (insuffecient resources?).")]
		E_RPC_SURROGATECOMMMGR_INIT,
		// Token: 0x04000056 RID: 86
		[Description("ClientConnectionTracker had a problem maniuplating the DataStore.")]
		E_RPC_CCT_DATASTORE_ACCESS,
		// Token: 0x04000057 RID: 87
		[Description("ClientConnectionTracker reports an invalid value in a DataStore entry.")]
		E_RPC_CCT_DATASTORE_CONTENTS,
		// Token: 0x04000058 RID: 88
		[Description("The RPC server cannot launch a surrogate.")]
		E_RPC_CANNOT_LAUNCH_SURROGATE,
		// Token: 0x04000059 RID: 89
		[Description("The GUIDKeyedList is empty, but user tried to access the first node.")]
		E_GUIDKEYEDLIST_EMPTY,
		// Token: 0x0400005A RID: 90
		[Description("Tried to find a GUID in a GuidKeyedList that was not found.")]
		E_GUIDKEYEDLIST_NODENOTFOUND,
		// Token: 0x0400005B RID: 91
		[Description("TMManager was doing AcceptTMConnection but was unable to launch its reader thread.")]
		E_TMMANAGER_THREADCREATION,
		// Token: 0x0400005C RID: 92
		[Description("Caller tried to call a method on a TMManager when it was in the wrong state, eg, do ConnectTransaction when not in Initialized_Connected.")]
		E_TMMANAGER_INVALIDSTATE,
		// Token: 0x0400005D RID: 93
		[Description("Guid Keyed List object failed its invariant check.")]
		E_GUIDKEYEDLIST_INVARIANT,
		// Token: 0x0400005E RID: 94
		[Description("TMManager detects packet out-of-sync error in the underlying transport layer.")]
		E_TMMANAGER_PACKETOUTOFSYNC,
		// Token: 0x0400005F RID: 95
		[Description("Tried to get the packet data from an empty PacketListNode that has no data in its buffer.")]
		E_PACKETLISTNODE_EMPTY,
		// Token: 0x04000060 RID: 96
		[Description("The ClientConnectionTracker can't find a cached connection for a surrogate (could mean need to launch surrogate / is expected error in that case).")]
		E_NOSURROGATECONNECTIONFOUND,
		// Token: 0x04000061 RID: 97
		[Description("RPC server cannot launch a surrogate because the corresponding target DLL is missing.")]
		E_RPC_TARGET_DLL_MISSING,
		// Token: 0x04000062 RID: 98
		[Description("There are device side RPC binaries missing.")]
		E_RPC_DEVICE_RPC_BINS_MISSING,
		// Token: 0x04000063 RID: 99
		[Description("Could not load the transport loader.")]
		RPC_E_SURROGATE_FAILED_TL_LOAD,
		// Token: 0x04000064 RID: 100
		[Description("Could not get the transport.")]
		RPC_E_TL_GETTRANSPORT,
		// Token: 0x04000065 RID: 101
		[Description("Could not acknowledge launch with transport loader.")]
		RPC_E_TL_ACKNOWLEDGE,
		// Token: 0x04000066 RID: 102
		[Description("Could not register callback.")]
		RPC_E_TL_REGISTERCALLBACK,
		// Token: 0x04000067 RID: 103
		[Description("The service has not been started.")]
		ERROR_SERVICE_NOT_ACTIVE = -2147023834,
		// Token: 0x04000068 RID: 104
		[Description("Device already connected.")]
		ERROR_DEVICE_ALREADY_ATTACHED = -2147024348,
		// Token: 0x04000069 RID: 105
		[Description("Connection refused.")]
		WSAECONNREFUSED = -2147014835,
		// Token: 0x0400006A RID: 106
		[Description("Network is down.")]
		WSAENETDOWN = -2147014846,
		// Token: 0x0400006B RID: 107
		[Description("Network dropped connection on reset.")]
		WSAENETRESET = -2147014844,
		// Token: 0x0400006C RID: 108
		[Description("Software caused connection abort.")]
		WSAECONNABORTED,
		// Token: 0x0400006D RID: 109
		[Description("Connection reset by peer.")]
		WSAECONNRESET,
		// Token: 0x0400006E RID: 110
		[Description("A connection attempt failed because the connected party did not properly respond after a period of time, or the established connection failed because the connected host has failed to respond.")]
		WSAETIMEOUT = -2147014836,
		// Token: 0x0400006F RID: 111
		[Description("A socket operation was attempted to an unreachable host.")]
		WSAENETUNREACH = -2147014845,
		// Token: 0x04000070 RID: 112
		[Description("The requested address is not valid in its context.")]
		WSAEADDRNOTAVAIL = -2147014847,
		// Token: 0x04000071 RID: 113
		[Description("A socket operation failed because the destination host is down.")]
		WSAEHOSTDOWN = -2147014832,
		// Token: 0x04000072 RID: 114
		[Description("ICMP destination unreachable received, which means host was unreachable.")]
		WSAEHOSTUNREACH,
		// Token: 0x04000073 RID: 115
		[Description("Invalid working foloder.")]
		E_WORKING_FOLDER = -1988886511,
		// Token: 0x04000074 RID: 116
		[Description("Source folder not allowed.")]
		E_SRC_FOLDER_NOT_ALLOWED = -1988886505,
		// Token: 0x04000075 RID: 117
		[Description("Source file not found.")]
		E_SRC_FILE_NOT_FOUND,
		// Token: 0x04000076 RID: 118
		[Description("Source path not found.")]
		E_SRC_PATH_NOT_FOUND = -1988886498,
		// Token: 0x04000077 RID: 119
		[Description("Destination folder not allowed.")]
		E_DST_FOLDER_NOT_ALLOWED = -1988886503,
		// Token: 0x04000078 RID: 120
		[Description("Destination path not found.")]
		E_DST_PATH_NOT_FOUND = -1988886497,
		// Token: 0x04000079 RID: 121
		[Description("Bad UNC path on destination.")]
		E_DST_BAD_UNC_PATH,
		// Token: 0x0400007A RID: 122
		[Description("Could not execute the command.")]
		E_COULD_NOT_EXECUTE = -1988886494,
		// Token: 0x0400007B RID: 123
		[Description("Bad path name on destination.")]
		E_DST_BAD_PATHNAME = -1988886490,
		// Token: 0x0400007C RID: 124
		[Description("Conflicting file exisits.")]
		E_CONFLICTING_FILE_EXISTS = -1988886486
	}
}
