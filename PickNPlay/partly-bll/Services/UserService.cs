using AutoMapper;
using PickNPlay.Exceptions;
using PickNPlay.picknplay_bll.Models.User;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Repositories;
using YourNamespaceHere.Controllers;

namespace PickNPlay.picknplay_bll.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userRepository = unitOfWork.UserRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserGet>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(_mapper.Map<UserGet>);
        }

        public async Task<UserGet> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            var userGet = _mapper.Map<UserGet>(user);
            if (userGet != null)
            {
                userGet.TransactionQuantity = _userRepository.TransactionCount(id);
            }
            return userGet;
        }

        public async Task<UserGet> AddAsync(UserPost userDto)
        {
            var user = _mapper.Map<User>(userDto);

            user.UserImage = "http://res.cloudinary.com/pick-n-play/image/upload/v1722061692/bn3cdybpwhe7z0b6nc6l.png";
            user.UserRoleId = 1;

            try
            {
                var record = _userRepository.CheckAbsenseByUsernameAndEmail(user);

                if (record)
                {
                    throw new ServiceException("The user with these email and/or username exists");
                }

                await _userRepository.AddAsync(user);
                var created = _userRepository.GetTheNewestUser();
                return _mapper.Map<UserGet>(created);

            }
            catch (ServiceException)
            {
                throw;
            }
            catch (DALException)
            {
                throw;
            }
        }

        public async Task UpdateAsync(int id, UserUpdate userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return;
            }

            _mapper.Map(userDto, existingUser);
            await _userRepository.UpdateAsync(existingUser);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<UserGet> GetByUsernameAndPasswordAsync(string? userName, string? password)
        {
            var entity = await _userRepository.GetByNameAndPassword(userName, password);
            if (entity == null)
                return null;
            else return _mapper.Map<UserGet>(entity);

        }

        public async Task<decimal> GetBalanceById(int userId)
        {
            return await _userRepository.GetBalanceById(userId);
        }

        public async Task<bool> UpdateBalance(int userId, decimal balance)
        {
            return await _userRepository.UpdateBalance(userId, balance);
        }

        public async Task<bool> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            return await _userRepository.UpdatePassword(userId, oldPassword, newPassword);
        }

        public async Task<bool> ChangeNickname(int userId, string nickname)
        {
            return await _userRepository.UpdateNickname(userId, nickname);
        }

        public async Task<bool> ChangePhoto(int userId, string? imageUrl)
        {
            return await _userRepository.UpdateImage(userId, imageUrl);
        }

        public async Task<string> GetEmailVerifToken(int userId)
        {
            try
            {
                return await _userRepository.GetEmailVerifToken(userId);
            }
            catch
            {
                throw;
            }
        }

        public async Task<string> GetPhoneNumberVerifToken(int userId)
        {
            try
            {
                return await _userRepository.GetPhoneNumberVerifToken(userId);
            }
            catch
            {
                throw;
            }

        }

        public async Task<bool> VerifyEmail(string verificationToken)
        {
            try
            {
                var isSuccess = await _userRepository.VerifyEmail(verificationToken);
                return isSuccess;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> VerifyPhoneNumber(int userId, string phoneVerificationToken)
        {
            try
            {
                var isSuccess = await _userRepository.VerifyPhoneNumber(userId, phoneVerificationToken);
                return isSuccess;
            }
            catch
            {
                throw;
            }
        }

        public async Task<(string, string)> GetPasswordResetTokenAndUsername(string email)
        {
            return await _userRepository.GetPasswordResetTokenAndUserName(email);
        }

        public async Task<bool> ResetPassword(ResetModel resetModel)
        {
            return await _userRepository.ResetPassword(resetModel);
        }

        public async Task<IEnumerable<object>> GetMostProfitableUsersAsync(int term)
        {
            return await _userRepository.GetMostProfitableUsersInPeriod(term);
        }

        public async Task<IEnumerable<UserReallyBriefInfoWithoutReviews>> GetLatestUsers(int amount)
        {
            var entities = await _userRepository.GetLatestRegisteredUsers(amount);

            return entities.Select(_mapper.Map<UserReallyBriefInfoWithoutReviews>);
        }

        public async Task<bool> ChangeDescription(string content, int userId)
        {
            return await _userRepository.ChangeDescription(content, userId);
        }

        public object GetAmountOfCreatedByMonth()
        {
            return _userRepository.GetAmountOfCreatedByMonth();
        }

        public object GetAmountOfCreatedByYear()
        {
            return _userRepository.GetAmountOfCreatedByYear();
        }
        public object GetSuccessfulCreatorsByMonth()
        {
            return _userRepository.GetSuccessfulCreatorsByMonth();
        }
        public object GetSuccessfulCreatorsByYear()
        {
            return _userRepository.GetSuccessfulCreatorsByYear();
        }

    }
}
