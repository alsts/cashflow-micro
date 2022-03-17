using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Cashflow.Common.Data.Enums;
using Cashflow.Common.Exceptions;
using MoneyService.Data.Models;
using MoneyService.Data.Repos.Interfaces;
using MoneyService.Services.interfaces;

namespace MoneyService.Services
{
    public class MoneyTasksService : IMoneyTasksService
    {
        private readonly ITaskRepo taskRepo;
        private readonly ITaskTransactionRepo taskTransactionRepo;
        private readonly IUsersService usersService;
        private readonly IMoneyUsersService moneyUsersService;
        private readonly IUserTransactionRepo userTransactionRepo;

        public MoneyTasksService(
            ITaskRepo taskRepo,
            IUsersService usersService,
            IMoneyUsersService moneyUsersService,
            ITaskTransactionRepo taskTransactionRepo,
            IUserTransactionRepo userTransactionRepo)
        {
            this.taskRepo = taskRepo;
            this.usersService = usersService;
            this.moneyUsersService = moneyUsersService;
            this.taskTransactionRepo = taskTransactionRepo;
            this.userTransactionRepo = userTransactionRepo;
        }

        public async Task<decimal> GetTaskBalance(string taskId)
        {
            var currentUser = await usersService.GetCurrentUser();
            var task = await GetTaskForUser(taskId, currentUser);
            return await taskTransactionRepo.GetTaskBalance(task.Id);
        }

        public async Task<(UserTransaction userTransaction, TaskTransaction taskTransaction)> AddMoneyToTaskBalance(string taskId, decimal amount)
        {
            var currentUser = await usersService.GetCurrentUser();
            var task = await GetTaskForUser(taskId, currentUser);

            var availableUserAdBalance = await moneyUsersService.GetAdBalanceForCurrentUser();
            if (availableUserAdBalance - amount <= 0)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Not enough money on user ad balance");
            }

            UserTransaction userTransaction = new UserTransaction
            {
                PublicId = Guid.NewGuid().ToString(),
                UserId = currentUser.Id,
                CreatedByUserId = currentUser.PublicId,
                Amount = -amount,
                TransactionStatus = (int)TransactionStatus.Completed,
                TransactionType = (int) TransactionType.AdBalance,
                Description = "MoneyTasksService:AddMoneyToTaskBalance"
            };
            await userTransactionRepo.Save(userTransaction);

            TaskTransaction taskTransaction = new TaskTransaction
            {
                PublicId = Guid.NewGuid().ToString(),
                TaskId = task.PublicId,
                CreatedByUserId = currentUser.PublicId,
                Amount = +amount,
                TransactionStatus = (int)TransactionStatus.Completed,
                Description = "MoneyTasksService:AddMoneyToTaskBalance"
            };
            await taskTransactionRepo.Save(taskTransaction);

            return (userTransaction, taskTransaction);
        }

        public async Task<(UserTransaction userTransaction, TaskTransaction taskTransaction)> ReturnMoneyFromTaskBalance(string taskId)
        {
            var currentUser = await usersService.GetCurrentUser();
            var task = await GetTaskForUser(taskId, currentUser);

            var taskAvailableBalance = await GetTaskBalance(taskId);
            if (taskAvailableBalance <= 0)
            {
                throw new HttpStatusException(HttpStatusCode.BadRequest, "Task does not have any money on it");
            }

            UserTransaction userTransaction = new UserTransaction()
            {
                PublicId = Guid.NewGuid().ToString(),
                UserId = currentUser.Id,
                CreatedByUserId = currentUser.PublicId,
                Amount = +taskAvailableBalance,
                TransactionStatus = (int)TransactionStatus.Completed,
                TransactionType = (int) TransactionType.AdBalance,
                Description = "MoneyTasksService:ReturnMoneyFromTaskBalance"
            };
            await userTransactionRepo.Save(userTransaction);

            TaskTransaction taskTransaction = new TaskTransaction
            {
                PublicId = Guid.NewGuid().ToString(),
                TaskId = task.PublicId,
                CreatedByUserId = currentUser.PublicId,
                Amount = -taskAvailableBalance,
                TransactionStatus = (int)TransactionStatus.Completed,
                Description = "MoneyTasksService:ReturnMoneyFromTaskBalance"
            };
            await taskTransactionRepo.Save(taskTransaction);
            
            return (userTransaction, taskTransaction);
        }

        public async Task<List<TaskTransaction>> GetTaskTransactionsHistoryForCurrentUser()
        {
            var currentUser = await usersService.GetCurrentUser();
            return await taskTransactionRepo.GetTransactionsHistory(currentUser.PublicId);
        }

        private async Task<Data.Models.Task> GetTaskForUser(string taskId, User user)
        {
            var task = await taskRepo.GetByPublicId(taskId);
            if (task == null)
            {
                throw new HttpStatusException(HttpStatusCode.NotFound, "Task does not exist");
            }

            if (task.UserId != user.PublicId)
            {
                throw new HttpStatusException(HttpStatusCode.Forbidden, "Not your task");
            }

            return task;
        }
    }
}
