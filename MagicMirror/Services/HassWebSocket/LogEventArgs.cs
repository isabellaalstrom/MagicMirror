using System;

namespace MagicMirror.Services.HassWebSocket
{
    public class LogEventArgs
    {
        public string Text { get; set; }
        public Exception Exception { get; set; }
    }
}
