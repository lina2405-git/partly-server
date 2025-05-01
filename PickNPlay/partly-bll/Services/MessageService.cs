using AutoMapper;
using Microsoft.EntityFrameworkCore.Update.Internal;
using PickNPlay.picknplay_bll.Models.Game;
using PickNPlay.picknplay_bll.Models.Listing;
using PickNPlay.picknplay_bll.Models.Message;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;

namespace PickNPlay.picknplay_bll.Services
{
    public class MessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MessageGet>> GetAllAsync()
        {
            var messages = await _unitOfWork.MessageRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MessageGet>>(messages);
        }

        public async Task<MessageGet> GetByIdAsync(int id)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
            return _mapper.Map<MessageGet>(message);
        }

        public async Task<IEnumerable<MessageGet>> AddAsync(MessagePost messageDto, int senderId)
        {
            var message = _mapper.Map<Message>(messageDto);
            message.SenderId = senderId;
            message.CreatedAt = DateTime.Now;

            await _unitOfWork.MessageRepository.AddAsync(message);
            return await GetMessagesByListingIdAndUsers(messageDto.ListingId.Value, senderId, messageDto.ReceiverId, senderId);
        }

        public async Task UpdateAsync(int id, MessageUpdate messageDto)
        {
            var existingMessage = await _unitOfWork.MessageRepository.GetByIdAsync(id);

            _mapper.Map(messageDto, existingMessage);
            await _unitOfWork.MessageRepository.UpdateAsync(existingMessage);
        }

        public async Task DeleteAsync(int id)
        {
            await _unitOfWork.MessageRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ListingWithLastMessage>?> GetChatsForSeller(int userId)
        {
            var listings = await _unitOfWork.MessageRepository.GetChatsForSeller(userId);

            if (!listings.Any())
            {
                return Enumerable.Empty<ListingWithLastMessage>();
            }

            List<ListingWithLastMessage> result = new();

            foreach (var unmapped in listings)
            {
                var mapped = _mapper.Map<ListingGet>(unmapped);
                var unreadMessageCount = mapped.Messages.Count(e => e.isRead == false && e.ReceiverId == userId);

                result.Add(new()
                {
                    Listing =
                    {
                        FinalPrice = mapped.FinalPrice,
                        ListingId = mapped.ListingId,
                        User = mapped.User,
                        Title = mapped.Title,
                        UnreadMessageCount = unreadMessageCount
                    },
                    LastMessage = mapped.Messages.Any() ? mapped.Messages.TakeLast(1).ToArray()[0] : null

                });

            }

            if (!result.Any())
            {
                return null;
            }
            return result;
        }
        public async Task<IEnumerable<ListingWithLastMessage>> GetChatsForBuyer(int userId)
        {

            var listings = await _unitOfWork.MessageRepository.GetChatsForBuyer(userId);
            List<ListingWithLastMessage> result = new();

            if (!listings.Any())
            {
                return Enumerable.Empty<ListingWithLastMessage>();
            }

            foreach (var unmapped in listings)
            {
                var mapped = _mapper.Map<ListingGet>(unmapped);
                var unreadMessageCount = mapped.Messages.Count(e => e.isRead == false && e.ReceiverId == userId);
                result.Add(new()
                {
                    Listing =
                    {
                        FinalPrice = mapped.FinalPrice,
                        ListingId = mapped.ListingId,
                        User = mapped.User,
                        Title = mapped.Title,
                        UnreadMessageCount = unreadMessageCount
                    },
                    LastMessage = mapped.Messages.Any() ? mapped.Messages.TakeLast(1).ToArray()[0] : null

                });
            }

            if (!result.Any())
            {
                Enumerable.Empty<ListingWithLastMessage>();
            }
            return result;

        }

        private IEnumerable<MessageGet> MarkMessageIfTheSenderIsCurrentUser(int userId, IEnumerable<Message>? messages)
        {
            return messages.Select(e =>
            {
                var temp = _mapper.Map<MessageGet>(e);

                if (temp.SenderId == userId)
                {
                    temp.isSenderCurrentUser = true;
                }

                return temp;
            });
        }

        public async Task<IEnumerable<MessageGet>> GetMessagesByListingIdAndUsers(int listingId, int senderId, int receiverId, int currentUserId)
        {
            var messages = await _unitOfWork.MessageRepository.GetMessagesAndMarkAsRead(listingId, senderId, receiverId, currentUserId);

            if (!messages.Any())
            {
                return Enumerable.Empty<MessageGet>();
            }

            return MarkMessageIfTheSenderIsCurrentUser(currentUserId, messages);
        }

    }
}
