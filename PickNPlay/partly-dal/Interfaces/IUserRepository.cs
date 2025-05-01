using PickNPlay.picknplay_dal.Entities;
using PickNPlay.picknplay_dal.Interfaces;
using YourNamespaceHere.Controllers;

namespace PickNPlay.picknplay_dal.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        bool CheckAbsenseByUsernameAndEmail(User user);
        int TransactionCount(int id);


        Task<User?> GetByNameAndPassword(string? userName, string? password);
        User GetTheNewestUser();
        Task<decimal> GetBalanceById(int userId);

        Task<string> GetEmailVerifToken(int userId);
        Task<string> GetPhoneNumberVerifToken(int userId);

        Task<bool> UpdateBalance(int userId, decimal balance);
        Task<bool> UpdatePassword(int userId, string oldPassword, string newPassword);
        Task<bool> UpdateNickname(int userId, string nickname);
        Task<bool> UpdateImage(int userId, string? imageUrl);


        Task<bool> VerifyEmail(string verificationToken);
        Task<bool> VerifyPhoneNumber(int userId, string phoneVerificationToken);
        Task<(string, string)> GetPasswordResetTokenAndUserName(string email);
        Task<bool> ResetPassword(ResetModel resetModel);
        Task<IEnumerable<object>> GetMostProfitableUsersInPeriod(int term);

        Task<IEnumerable<User>> GetLatestRegisteredUsers(int amount);
        Task<bool> ChangeDescription(string content, int userId);
        object GetAmountOfCreatedByMonth();
        object GetAmountOfCreatedByYear();
        object GetSuccessfulCreatorsByMonth();
        object GetSuccessfulCreatorsByYear();
    }
}