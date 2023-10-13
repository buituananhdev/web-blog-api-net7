using Microsoft.EntityFrameworkCore;
using WebBlog.Data.Models;
using WebBlog.Data.Data;

namespace WebBlog.Service.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly DataContext _context;
        public MessageService(DataContext context)
        {
            _context = context;
        }
        public async Task<Message?> DeleteMessage(string messageID)
        {
            var message = await _context.Messages.FindAsync(messageID);
            if (message is null)
            {
                return null;
            }

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return message;
        }

        /*public async Task<List<Message>> GetMessagesInConversation(string senderID, string receiverID)
        {
            var messages = await _context.Messages
            .Where(m => (m.SenderID == senderID && m.ReceiverID == receiverID) || (m.SenderID == receiverID && m.ReceiverID == senderID))
            .ToListAsync();
            return messages;
        }*/
        

        public async Task<Message> SendMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }
    }
}
