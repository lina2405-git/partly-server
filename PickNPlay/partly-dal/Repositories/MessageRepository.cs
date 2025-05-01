using Microsoft.EntityFrameworkCore;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;

public class MessageRepository : IMessageRepository
{
    private readonly picknplayContext _context;

    public MessageRepository(picknplayContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _context.Messages.ToListAsync();
    }

    public async Task<Message> GetByIdAsync(int id)
    {
        return await _context.Messages.FindAsync(id);
    }

    public async Task AddAsync(Message entity)
    {
        if (entity.ReceiverId == entity.SenderId)
        {
            throw new ArgumentException("You cannot send message to yourself");
        }

        _context.Messages.Add(entity);
        _context.SaveChanges(true);
    }

    public async Task UpdateAsync(Message entity)
    {
        _context.Messages.Update(entity);
        await _context.SaveChangesAsync(true);
    }

    public async Task DeleteAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message != null)
        {
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<IEnumerable<Listing>?> GetChatsForBuyer(int userId)
    {
        var listings = _context.Listings
            .Where(l => l.UserId != userId)
            .Where(l => l.Messages.Any())
            .Include(l => l.Messages)
                .ThenInclude(m => m.Sender)
            .Include(l => l.Messages)
                .ThenInclude(m => m.Receiver)
            .AsEnumerable()
            .Select(l =>
            {
                l.Messages = l.Messages
                    .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                    .ToList();
                return l;
            })
            .OrderByDescending(e => e.Messages.Last().CreatedAt)
            .ToList();

        return listings;

    }

    public async Task<IEnumerable<Listing>?> GetChatsForSeller(int userId)
    {
        var listings = _context.Listings
           .Where(l => l.UserId == userId)
           .Where(l => l.Messages.Any())
           .Include(l => l.Messages)
               .ThenInclude(m => m.Sender)
           .Include(l => l.Messages)
               .ThenInclude(m => m.Receiver)
           // .Include(l => l.Game)
           .AsEnumerable()
           .Select(l =>
           {
               l.Messages = l.Messages
                   .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                   .ToList();
               return l;
           })
           .OrderByDescending(e => e.Messages.Last().CreatedAt)
           .ToList();

        return listings;
    }

    public async Task<IEnumerable<Message>?> GetMessagesAndMarkAsRead(int listingId, int senderId, int receiverId, int currentUserId)
    {
        var messages = _context.Messages
           .Where(m => m.ListingId == listingId)
           .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                       (m.SenderId == receiverId && m.ReceiverId == senderId))
           .Include(m => m.Sender)
           .Include(m => m.Receiver)
           .ToList();

        foreach (var message in messages)
        {
            if (message.ReceiverId == currentUserId && !message.isRead)
            {
                message.isRead = true;
            }
        }

        await _context.SaveChangesAsync();

        return messages;
    }
}
