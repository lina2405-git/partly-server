using Microsoft.EntityFrameworkCore;
using PickNPlay.Exceptions;
using PickNPlay.picknplay_dal.Data;
using PickNPlay.picknplay_dal.Entities;
using YourNamespaceHere.Controllers;

namespace PickNPlay.picknplay_dal.Repositories
{

    public class UserRepository : IUserRepository
    {
        private readonly picknplayContext _context;

        public UserRepository(picknplayContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public int TransactionCount(int id)
        {
            var count = _context.Transactions
                .Where(t => t.SellerId == id)
                .Where(t => t.Status == "Completed")
                .Count();
            return count;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddAsync(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                entity.NumberVerificationToken = GeneratePhoneNumberVerifToken();
                entity.VerificationToken = GenerateEmailVerifToken();
                entity.PasswordResetToken = GenerateEmailVerifToken();

                entity.EmailApproved = false;
                entity.PhoneNumberApproved = false;

                await _context.Users.AddAsync(entity);

            }
            catch (Exception)
            {
                throw new DALException("exception while adding user");
            }
            finally
            {
                await _context.SaveChangesAsync(true);
            }

        }

        public async Task UpdateAsync(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> GetByNameAndPassword(string? userName, string? password)
        {
            return await _context.Users.FirstOrDefaultAsync(a => a.Username == userName && a.PasswordHash == password);
        }

        public bool CheckAbsenseByUsernameAndEmail(User user)
        {
            return _context.Users.Any(e => e.Username == user.Username || e.Email == user.Email);
        }

        public User GetTheNewestUser()
        {
            return _context.Users.Take(1).OrderByDescending(e => e.UserId).ToArray()[0];
        }

        public async Task<decimal> GetBalanceById(int userId)
        {
            var entity = await _context.Users.FirstAsync(e => e.UserId == userId) ?? throw new DALException($"User with id={userId} was not found");
            return entity.Balance;
        }

        public async Task<bool> UpdateBalance(int userId, decimal balance)
        {
            var entity = await _context.Users.FirstAsync(e => e.UserId == userId) ?? throw new DALException($"User with id={userId} was not found");

            entity.Balance += balance;
            if (entity.Balance < 0)
            {
                return false;
            }

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePassword(int userId, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId) ?? throw new DALException($"User with id={userId} does not exist");

            if (user.PasswordHash == oldPassword)
            {
                user.PasswordHash = newPassword;
                await UpdateAsync(user);
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateNickname(int userId, string nickname)
        {
            var user = await _context.Users.FindAsync(userId) ?? throw new DALException($"User with id={userId} does not exist");

            if (nickname.Length >= 5)
            {
                user.Username = nickname;
                await UpdateAsync(user);
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateImage(int userId, string? imageUrl)
        {
            var user = await _context.Users.FindAsync(userId) ?? throw new DALException($"User with id={userId} does not exist");

            user.UserImage = imageUrl;
            await UpdateAsync(user);
            return true;

        }

        public string GenerateEmailVerifToken()
        {
            return Guid.NewGuid().ToString().Substring(0, 10);
        }

        public string GeneratePhoneNumberVerifToken()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString();
        }

        public async Task<string> GetEmailVerifToken(int userId)
        {
            var entity = await _context.Users.FirstAsync(e => e.UserId == userId) ?? throw new DALException("This user does not exist");
            return entity.VerificationToken;
        }

        public async Task<string> GetPhoneNumberVerifToken(int userId)
        {
            var entity = await _context.Users.FirstAsync(e => e.UserId == userId) ?? throw new DALException("This user does not exist");
            return entity.NumberVerificationToken;
        }

        public async Task<bool> VerifyEmail(string verificationToken)
        {
            var entity = await _context.Users.FirstAsync(e => e.VerificationToken == verificationToken) ?? throw new DALException("User with this token was not found");

            if (entity.EmailApproved.Value)
            {
                throw new InvalidOperationException("User has already confirmed their email.");
            }

            if (entity.VerificationToken == verificationToken)
            {
                entity.EmailApproved = true;
                entity.EmailVerifiedAt = DateTime.UtcNow;
                entity.UserRoleId += 1;

                await _context.SaveChangesAsync(true);
                return true;
            }

            return false;
        }

        public async Task<bool> VerifyPhoneNumber(int userId, string phoneVerificationToken)
        {
            var entity = await _context.Users.FindAsync(userId) ?? throw new DALException("This user does not exist");

            if (entity.PhoneNumberApproved.Value)
            {
                throw new InvalidOperationException("User has already confirmed their number.");
            }

            if (entity.NumberVerificationToken == phoneVerificationToken)
            {
                entity.PhoneNumberApproved = true;
                entity.NumberVerifiedAt = DateTime.UtcNow;

                //entity.NumberVerificationToken = GeneratePhoneNumberVerifToken();

                await _context.SaveChangesAsync(true);
                return true;
            }

            return false;
        }

        public async Task<(string, string)> GetPasswordResetTokenAndUserName(string email)
        {
            //токен должен генерироваться каждый раз при попытке изменения пароля и срок его истечения должен устанавливаться
            var entity = await _context.Users.FirstAsync(e => e.Email == email)
                ?? throw new DALException("User with this email was not found");

            entity.PasswordResetToken = GenerateEmailVerifToken();
            entity.ResetTokenExpires = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync(true);

            return (entity.PasswordResetToken, entity.Username);

        }

        public async Task<bool> ResetPassword(ResetModel resetModel)
        {
            var user = await _context.Users.FirstAsync(e => e.PasswordResetToken == resetModel.ResetToken)
                ?? throw new DALException("User with this reset token was not found");

            if (user.ResetTokenExpires < DateTime.Now)
            {
                return false;
            }

            user.PasswordHash = resetModel.NewPassword;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync(true);
            return true;
        }

        public async Task<IEnumerable<object>> GetMostProfitableUsersInPeriod(int term)
        {
            var usernamesAndTransAmount = await _context.Users
                                    .Where(e=>e.CreatedAt >= DateTime.Now.AddDays(-term))
                                    .Select(e => new
                                    {
                                         e.Username,
                                         e.TransactionSellers.Count

                                    })
                                    .OrderByDescending(e=>e.Count)
                                    .ToListAsync();

            return usernamesAndTransAmount;
        }

        public async Task<IEnumerable<User>> GetLatestRegisteredUsers(int amount)
        {
            return await _context.Users.OrderByDescending(e => e.CreatedAt)
                                    .Take(amount).ToListAsync();
        }
        public async Task<bool> ChangeDescription(string content, int userId)
        {
            var entity = await _context.Users.FirstAsync(e => e.UserId == userId);
            
            if (entity == null)
            {
                throw new DALException("user not found");
            }

            entity.ProfileDescription = content;
            await _context.SaveChangesAsync(true);
            return true;
        }

        public object GetAmountOfCreatedByMonth()
        {

            var dateAndAmount = _context.Users
                             .Where(e => e.CreatedAt >= DateTime.Now.AddMonths(-1))
                             .GroupBy(e => new
                             {
                                 e.CreatedAt.Value.Year,
                                 e.CreatedAt.Value.Month,
                                 e.CreatedAt.Value.Day,
                             })
                             .Select(e => new
                             {
                                 Date = new DateOnly(e.Key.Year, e.Key.Month, e.Key.Day),
                                 Amount = e.Count()
                             })
                             .ToList();

            return new
            {
                TotalAmount = dateAndAmount.Sum(e => e.Amount),
                AmountByDays = dateAndAmount
            };
        }
        public object GetAmountOfCreatedByYear()
        {
            var dateAndAmount = _context.Users
                             .Where(e => e.CreatedAt >= DateTime.Now.AddYears(-1))
                             .GroupBy(e => new
                             {
                                 e.CreatedAt.Value.Year,
                                 e.CreatedAt.Value.Month,

                             })
                             .Select(e => new
                             {
                                 e.Key.Month,
                                 Amount = e.Count()
                             })
                             .ToList();

            return new
            {
                TotalAmount = dateAndAmount.Sum(e => e.Amount),
                AmountByMonths = dateAndAmount
            };
        }



        public object GetSuccessfulCreatorsByMonth()
        {
            return _context.Listings
                            .Where(e => e.CreatedAt >= DateTime.Now.AddMonths(-1))
                            .Join(_context.Users,
                                l => l.UserId,
                                u => u.UserId,
                                (l, u) => new { Listing = l, User = u })
                            .GroupBy(lu => lu.User.Username)
                            .Select(g => new
                            {
                                Username = g.Key,
                                CreatedAmount = g.Count()
                            })
                            .OrderByDescending(e => e.CreatedAmount)
                            .AsEnumerable();

        }

        public object GetSuccessfulCreatorsByYear()
        {
            return _context.Listings
                            .Where(e => e.CreatedAt >= DateTime.Now.AddYears(-1))
                            .Join(_context.Users,
                                l => l.UserId,
                                u => u.UserId,
                                (l, u) => new { Listing = l, User = u })
                            .GroupBy(lu => lu.User.Username)
                            .Select(g => new
                            {
                                Username = g.Key,
                                CreatedAmount = g.Count()
                            })
                            .OrderByDescending(e => e.CreatedAmount)
                            .AsEnumerable();

        }

    }
}