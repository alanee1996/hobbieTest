using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hobbie.Entities;
using hobbie.Exceptions;
using hobbie.Models;
using hobbie.Utilis;
using Microsoft.EntityFrameworkCore;

namespace hobbie.Repositories
{
    public class UserRepository : IUserRepository
    {
        private bool disposedValue;
        private DBContext db;
        private Log log = Log.getInstance();

        public UserRepository(DBContext db)
        {
            this.db = db;
        }

        public async Task<bool> create(UserViewModel user)
        {
            await createValidation(user);
            try
            {
                User _user = new User();
                _user.id = user.id;
                _user.name = user.name;
                _user.isDeleted = false;
                if (user.hobbies != null && user.hobbies.Count > 0)
                {
                    _user.hobbies = user.hobbies.Select(c => new Hobbie
                    {
                        hobbie = c.hobbie,
                    }).ToList();
                }
                await db.AddAsync(_user);
                var result = await db.SaveChangesAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                log.error("Create user error - {0}", ex: ex, user);
                return false;
            }
        }

        public async Task<bool> delete(long id)
        {
            try
            {
                var user = await db.users.FirstOrDefaultAsync(c => c.id == id && c.isDeleted == false);
                if (user != null)
                {
                    user.isDeleted = true;
                    db.users.Update(user);
                    return (await db.SaveChangesAsync()) > 0;
                }
                throw new UserNotFound(id);
            }
            catch (UserNotFound ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                log.error("delete user error - id: {0}", ex: ex, id);
                return false;
            }
        }

        public async Task<List<UserSummaryView>> getAllUsers()
        {
            try
            {
                var users = db.users.Where(c => !c.isDeleted).Include("hobbies");
                if (users == null) throw new Exception("Find all users is null, it might have some connection issues");
                return (await users.ToListAsync()).Select(c => new UserSummaryView(c.id, c.name, c.hobbies?.Select(h => new HobbieViewModel
                {
                    id = h.id,
                    hobbie = h.hobbie
                }).ToList())).ToList();
            }
            catch (Exception ex)
            {
                log.error("Get all user have error", ex);
                return new List<UserSummaryView>();
            }

        }

        public async Task<bool> update(UserViewModel user)
        {
            await updateUserValidation(user);
            try
            {
                var _user = await db.users.FirstOrDefaultAsync(c => c.id == user.id && !c.isDeleted);
                if (_user == null) throw new UserNotFound(user.id);
                if (_user.name == user.name) return true;
                _user.name = user.name;
                return (await db.SaveChangesAsync()) > 0;
            }
            catch (Exception ex)
            {
                log.error("Update user error - {0}", ex: ex, user);
                return false;
            }
        }

        private async Task updateUserValidation(UserViewModel model)
        {
            if (model.id <= 0) throw new InvalidDataException("User Id cannot less than 0");
            if (!(await db.users.AnyAsync(c => c.id == model.id))) throw new InvalidDataException($"User not found");
            if (string.IsNullOrWhiteSpace(model.name)) throw new InvalidDataException("Name cannot be null or whitespace");
        }

        private async Task createValidation(UserViewModel model)
        {
            if (model.id <= 0) throw new InvalidDataException("User Id cannot less than 0");
            if (await db.users.AnyAsync(c => c.id == model.id)) throw new InvalidDataException($"User id {model.id} already exist, please use other Id");
            if (string.IsNullOrWhiteSpace(model.name)) throw new InvalidDataException("Name cannot be null or empty space");
            if (model.newHobbies != null)
            {
                if (model.newHobbies.Any(c => string.IsNullOrWhiteSpace(c.hobbie))) throw new InvalidDataException("Hobbies that you enter cannot be null");
            }
        }

        private async Task hobbieUpdateValidation(HobbieViewModel model)
        {
            if (model.id <= 0) throw new InvalidDataException("Invalid hobbie id");
            if (model.userId <= 0) throw new InvalidDataException("Invalid user id");
            if (!(await db.hobbies.AnyAsync(c => c.id == model.id && c.id == model.id))) throw new InvalidDataException($"Hobbie id {model.id} not found");
            if (string.IsNullOrWhiteSpace(model.hobbie)) throw new InvalidDataException("Hobbie cannot be null or empty space");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                db = null;
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UserRepository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async Task<UserViewModel> findUserId(long id)
        {
            try
            {
                var user = await db.users.Include("hobbies").FirstOrDefaultAsync(c => c.id == id && !c.isDeleted);
                if (user == null) throw new UserNotFound(id);
                return new UserViewModel
                {
                    id = user.id,
                    name = user.name,
                    hobbies = user.hobbies?.Select(c => new HobbieViewModel
                    {
                        id = c.id,
                        hobbie = c.hobbie
                    })?.ToList()
                };
            }
            catch (UserNotFound ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                log.error("Find user by id error : Id {0}", ex: ex, id);
                return null;
            }
        }

        public async Task<bool> deleteHobbie(long userId, long id)
        {
            try
            {
                var hobbie = await db.hobbies.FirstOrDefaultAsync(c => c.id == id && c.userId == userId);
                if (hobbie == null) throw new Exception("Hobbie not found");
                db.hobbies.Remove(hobbie);
                return (await db.SaveChangesAsync()) > 0;
            }
            catch (Exception ex)
            {
                log.error("Delete hobbie error userId : {0} , id : {1}", ex: ex, userId, id);
                return false;
            }
        }

        public async Task<bool> updateHobbie(HobbieViewModel model)
        {
            await hobbieUpdateValidation(model);
            try
            {
                var hobbie = await db.hobbies.FirstOrDefaultAsync(c => c.id == model.id && c.userId == model.userId);
                if (hobbie == null) throw new Exception("Hobbie not found");
                if (hobbie.hobbie == model.hobbie) return true;
                hobbie.hobbie = model.hobbie;
                return (await db.SaveChangesAsync()) > 0;
            }
            catch (Exception ex)
            {
                log.error("Delete hobbie error  - {0}", ex: ex, model);
                return false;
            }
        }

        private async Task addHobbieValidation(HobbieViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.hobbie)) throw new InvalidDataException("Hobbie cannot be null or whitespace");
            if (!(await db.users.AnyAsync(c => c.id == model.userId))) throw new InvalidDataException("User not found");
            if (await db.hobbies.AnyAsync(c => c.hobbie.ToLower().Trim() == model.hobbie.ToLower().Trim() && c.userId == model.userId)) throw new InvalidDataException($"Hobbie {model.hobbie} already exist in the record");
        }

        public async Task<HobbieViewModel> addHobbie(HobbieViewModel model)
        {
            await addHobbieValidation(model);
            try
            {
                var hobbie = new Hobbie()
                {
                    userId = model.userId,
                    hobbie = model.hobbie
                };
                await db.AddAsync(hobbie);
                var result = (await db.SaveChangesAsync()) > 0;
                if (!result) throw new Exception("Add hobbie null");
                return new HobbieViewModel
                {
                    id = hobbie.id,
                    userId = hobbie.userId,
                    hobbie = hobbie.hobbie
                };
            }
            catch (Exception ex)
            {
                log.error("Delete hobbie error  - {0}", ex: ex, model);
                return null;
            }
        }
    }
}
