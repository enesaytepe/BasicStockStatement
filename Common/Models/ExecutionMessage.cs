using Common.Enums;

namespace Common.Models
{
    public class ExecutionMessage
    {
        public Exception? Exception { get; set; }
        public MessageCode MessageCode { get; set; }
        public DateTime Time { get; set; }
        public string Message { get; set; }
        public bool IsPrivate { get; set; }
        public MessageType MessageType { get; set; }
    }
}
