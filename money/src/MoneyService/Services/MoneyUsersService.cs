using System;
using System.Net;
using System.Threading.Tasks;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Exceptions;
using MoneyService.Data.Models;
using MoneyService.Data.Repos.Interfaces;
using MoneyService.Services.interfaces;

namespace MoneyService.Services
{
    public class MoneyUsersService : IMoneyUsersService
    {
        private readonly IUserRepo userRepo;
        private readonly IUserTransactionRepo userTransactionRepo;
        private readonly IUsersService usersService;

        public MoneyUsersService(
            IUsersService usersService,
            IUserTransactionRepo userTransactionRepo)
        {
            this.usersService = usersService;
            this.userTransactionRepo = userTransactionRepo;
        }

        public async Task<decimal> GetMainBalanceForUser()
        {
            var currentUser = await usersService.GetCurrentUser();
            return await userTransactionRepo.GetMainBalanceForUser(currentUser.Id);
        }

        public async Task<decimal> GetAdBalanceForCurrentUser()
        {
            var currentUser = await usersService.GetCurrentUser();
            return await userTransactionRepo.GetAdBalanceForUser(currentUser.Id);
        }

        public async Task<UserTransaction> DepositToUserMainBalance(decimal amount)
        {
            var currentUser = await usersService.GetCurrentUser();

            UserTransaction userTransaction = new UserTransaction()
            {
                PublicId = Guid.NewGuid().ToString(),
                UserId = currentUser.Id,
                CreatedByUserId = currentUser.PublicId,
                Amount = +amount,
                TransactionStatus = (int) TransactionStatus.Completed,
                TransactionType = (int) TransactionType.Deposit,
                Description = "MoneyUsersService:DepositToUserMainBalance"
            };
            await userTransactionRepo.Save(userTransaction);
            
            return userTransaction;
        }

        public async Task<(UserTransaction userMainBalanceTransaction, UserTransaction userAdBalanceTransaction)> AddMoneyToAdBalance(decimal amount)
        {
            var currentUser = await usersService.GetCurrentUser();

            var userMainBalance = await GetMainBalanceForUser();
            if (userMainBalance - amount <= 0)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "User does not have enough money on main balance");
            }
            
            UserTransaction userMainBalanceTransaction = new UserTransaction
            {
                PublicId = Guid.NewGuid().ToString(),
                UserId = currentUser.Id,
                CreatedByUserId = currentUser.PublicId,
                Amount = -amount,
                TransactionStatus = (int) TransactionStatus.Completed,
                TransactionType = (int) TransactionType.Deposit,
                Description = "MoneyUsersService:AddMoneyToAdBalance"
            };
            await userTransactionRepo.Save(userMainBalanceTransaction);

            UserTransaction userAdBalanceTransaction = new UserTransaction
            {
                PublicId = Guid.NewGuid().ToString(),
                UserId = currentUser.Id,
                CreatedByUserId = currentUser.PublicId,
                Amount = +amount,
                TransactionStatus = (int) TransactionStatus.Completed,
                TransactionType = (int) TransactionType.AdBalance,
                Description = "MoneyUsersService:AddMoneyToAdBalance"
            };
            await userTransactionRepo.Save(userAdBalanceTransaction);

            return (userMainBalanceTransaction, userAdBalanceTransaction);
        }

        public async Task<UserTransaction> WithdrawFromMainBalance(decimal amount)
        {
            var currentUser = await usersService.GetCurrentUser();

            var userMainBalance = await GetMainBalanceForUser();
            if (userMainBalance - amount <= 0)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "User does not have enough money on main balance");
            }
            
            UserTransaction userWithdrawalTransaction = new UserTransaction
            {
                UserId = currentUser.Id,
                CreatedByUserId = currentUser.PublicId,
                Amount = -amount,
                TransactionStatus = (int) TransactionStatus.Completed,
                TransactionType = (int) TransactionType.Withdrawal,
                Description = "MoneyUsersService:WithdrawFromMainBalance"
            };
            await userTransactionRepo.Save(userWithdrawalTransaction);

            return userWithdrawalTransaction;
        }
    }
}
