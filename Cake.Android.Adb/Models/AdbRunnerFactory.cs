using System;
using Cake.Core;

namespace Cake.AndroidAdb
{
    internal static class AdbRunnerFactory
    {
        internal static AdbTool GetAdbTool(ICakeContext context)
        {
            return new AdbTool(context, context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        }

    }
}
