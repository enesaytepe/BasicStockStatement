using Common.Enums;
using Common.Models;

namespace Common
{
    public class ExecutionResult<T>
    {
        public ExecutionResult()
        {
            //Modelin yaratılmasıyla mesaj listesi oluşturulur.
            MessageList = new List<ExecutionMessage>();
        }

        //Sonuçta döndürülecek data
        public T Data { get; set; }
        //Tüm işlemin başarı durumu
        public bool Success { get; set; }
        //Sonuç ile dönen mesaj listesi
        public List<ExecutionMessage> MessageList { get; set; }

        public void AddWarning(string message)
        {
            AddMessage(null, message, MessageCode.EE_00_00000, false, MessageType.Warning);
        }

        public void AddError(string message)
        {
            AddMessage(null, message, MessageCode.EE_00_00000, false, MessageType.Error);
        }

        public void AddError(Exception exception, string message)
        {
            AddMessage(exception, message, null, false, MessageType.Error);
        }

        public void AddError(Exception ex, string message, bool isPrivate)
        {
            AddMessage(ex, message, MessageCode.EE_00_00000, isPrivate);
        }

        public void AddMessage(Exception? ex, string? message, MessageCode? messageCode = MessageCode.EE_00_00000, bool isPrivate = false, MessageType messageType = MessageType.Information)
        {
            ExecutionMessage executionMessage = new ExecutionMessage();

            executionMessage.Exception = ex;
            executionMessage.MessageCode = messageCode ?? MessageCode.EE_00_00000;
            executionMessage.Time = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            executionMessage.Message = message ?? "";
            executionMessage.IsPrivate = isPrivate;
            executionMessage.MessageType = messageType;

            MessageList.Add(executionMessage);
        }
    }
}
