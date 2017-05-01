using System;
using Cake.Core;

namespace Cake.AndroidAdb
{
    public static class AdbRunnerFactory
    {
        public static AdbTool GetAdbTool(ICakeContext context)
        {
            return new AdbTool(context, context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
        }

    }
}
