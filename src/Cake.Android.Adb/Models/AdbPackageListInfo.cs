using System;
using Cake.Core.IO;

namespace Cake.AndroidAdb
{
	/// <summary>
	/// Android Package Information
	/// </summary>
	public class AdbPackageListInfo
	{
		/// <summary>
		/// Gets or sets the install path.
		/// </summary>
		/// <value>The install path.</value>
		public FilePath InstallPath { get; set; }

		/// <summary>
		/// Gets or sets the installer.
		/// </summary>
		/// <value>The installer.</value>
		public string Installer { get; set; }

		/// <summary>
		/// Gets or sets the name of the package.
		/// </summary>
		/// <value>The name of the package.</value>
		public string PackageName { get; set; }
	}
}
