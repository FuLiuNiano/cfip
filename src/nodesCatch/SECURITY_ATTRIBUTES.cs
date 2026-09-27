using System;

namespace nodesCatch;

public struct SECURITY_ATTRIBUTES
{
	public uint nLength;

	public IntPtr lpSecurityDescriptor;

	public int bInheritHandle;
}
