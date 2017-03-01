using System;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.AndroidAdb
{
	/// <summary>
	/// Tool Settings for Android ADB Aliases
	/// </summary>
	public class AdbToolSettings : ToolSettings
	{
	  
		/// <summary>
		/// Gets or sets the Android SDK HOME root path to invoke tools from.
		/// </summary>
		/// <value>The sdk root.</value>
		public DirectoryPath SdkRoot { get; set; }

		/// <summary>
		/// Gets or sets the serial of a specific device or emulator to target commands with.  This must be specified if multiple devices are seen by ADB, and only works on commands that are specific to a device.
		/// </summary>
		/// <value>The serial.</value>
		public string Serial { get; set; }
	}
}
