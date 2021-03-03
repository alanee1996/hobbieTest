using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using hobbie.Entities;
using hobbie.Models;

namespace hobbie.Repositories
{
    public interface IUserRepository : IDisposable
    {
        Task<List<UserSummaryView>> getAllUsers();
        Task<UserViewModel> findUserId(long id);
        Task<bool> create(UserViewModel user);
        Task<bool> update(UserViewModel user);
        Task<bool> delete(long id);
        Task<bool> deleteHobbie(long userId, long id);
        Task<bool> updateHobbie(HobbieViewModel model);
        Task<HobbieViewModel> addHobbie(HobbieViewModel model);
    }
}
