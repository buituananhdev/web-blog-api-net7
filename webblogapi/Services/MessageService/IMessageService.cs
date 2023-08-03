namespace webblogapi.Services.MessageService
{
    public interface IMessageService
    {
        Task<Message> SendMessage(Message message);
        Task<Message> DeleteMessage(string messageID);
    }
}
