using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.AndroidAdb
{
	public enum AdbTransport
	{
		Any = 0,
		Usb = 1,
		Local = 2
	}

	public enum AdbState
	{
		Device = 0,
		Recovery,
		Sideload,
		Bootloader
	}
}
