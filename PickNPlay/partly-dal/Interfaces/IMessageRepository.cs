using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Interfaces;

public interface IMessageRepository : IRepository<Message>
{
    Task<IEnumerable<Listing>?> GetChatsForBuyer(int userId);
    Task<IEnumerable<Listing>?> GetChatsForSeller(int userId);
    Task<IEnumerable<Message>?> GetMessagesAndMarkAsRead(int listingId, int senderId, int receiverId, int currentUserId);
}