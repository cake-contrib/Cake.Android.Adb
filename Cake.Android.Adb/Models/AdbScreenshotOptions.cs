using System;
namespace Cake.AndroidAdb
{
    public class AdbScreenshotOptions
    {
        public System.Threading.CancellationToken? RecordingCancelToken { get; set; }
        public TimeSpan? TimeLimit {get; set;}
        public int? BitrateMbps { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public bool Rotate { get; set; }
        public bool LogVerbose { get; set; }
    }
}
