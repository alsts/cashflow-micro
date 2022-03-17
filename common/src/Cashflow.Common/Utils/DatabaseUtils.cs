#nullable enable
using System;
using System.Linq;
using Cashflow.Common.Data.DataObjects;
using Cashflow.Common.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cashflow.Common.Utils
{
    public static class DatabaseUtils
    {
        public static void ConfigureAddedEntitiesChanges(ChangeTracker changeTracker,  LoggedInUserDataHolder? loggedInUserDataHolder)
        {
            foreach (var entity in changeTracker
                .Entries()
                .Where(x => x.Entity is BaseEntity && x.State == EntityState.Added)
                .Select(x => x.Entity)
                .Cast<BaseEntity>())
            {
                entity.CreatedAt = DateTime.Now;
                entity.CreatedByUserId = loggedInUserDataHolder?.UserID ?? "";
            }
        }
        
        public static void ConfigureModifiedEntitiesChanges(ChangeTracker changeTracker,  LoggedInUserDataHolder? loggedInUserDataHolder)
        {
            foreach (var entity in changeTracker
                .Entries()
                .Where(x => x.Entity is BaseEntity && x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .Cast<BaseEntity>())
            {
                entity.Version += 1;
                entity.LastUpdatedAt = DateTime.Now;
                entity.LastUpdatedByUserId = loggedInUserDataHolder?.UserID ?? "";
            }
        }
    }
}
