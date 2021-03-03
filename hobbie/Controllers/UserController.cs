using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hobbie.Exceptions;
using hobbie.Models;
using hobbie.Repositories;
using hobbie.Utilis;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace hobbie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository repository;
        private Log log = Log.getInstance();

        public UserController(IUserRepository repository)
        {
            this.repository = repository;
        }


        [Route("create")]
        [HttpPost]
        public async Task<JsonResponse> create([FromBody] UserViewModel createModel)
        {
            try
            {
                var result = await repository.create(createModel);
                if (!result) return JsonResponse.failed("Create user failed, please try again");
                return JsonResponse.success(message: "User create successful");
            }
            catch (InvalidDataException ex)
            {
                log.info("Invalid data found - {0}", ex.Message);
                return JsonResponse.failed(message: ex.Message);
            }
            catch (Exception ex)
            {
                log.error("Create user error - {0}", ex, createModel);
                return JsonResponse.failed(message: "Failed to create user");
            }
        }

        [Route("delete/{id}")]
        [HttpDelete]
        public async Task<JsonResponse> delete([FromRoute] long id)
        {
            try
            {
                var result = await repository.delete(id);
                if (!result) return JsonResponse.failed("Delete user failed, please try again");
                return JsonResponse.success(message: "Delete user successful");
            }
            catch (Exception ex)
            {
                log.error("Delete user error user id : {0}", ex, id);
                return JsonResponse.failed(message: "Failed to delete user");
            }
        }

        [Route("edit")]
        [HttpPost]
        public async Task<JsonResponse> edit([FromBody] UserViewModel model)
        {
            try
            {
                var result = await repository.update(model);
                if (!result) return JsonResponse.failed("Update user failed, please try again");
                return JsonResponse.success(message: "Update user successful");
            }
            catch (Exception ex)
            {
                log.error("Update user error - {0}", ex, model);
                return JsonResponse.failed(message: "Failed to update user");
            }
        }

        [Route("hobbie/add")]
        [HttpPost]
        public async Task<JsonResponse> addHobbie([FromBody] HobbieViewModel model)
        {
            try
            {
                var result = await repository.addHobbie(model);
                if (result == null) return JsonResponse.failed("Add hobbie failed, please try again");
                return JsonResponse.success(data: result);
            }
            catch (InvalidDataException ex)
            {
                log.info("Invalid data found - {0}", ex.Message);
                return JsonResponse.failed(message: ex.Message);
            }
            catch (Exception ex)
            {
                log.error("Add hobbie error - {0}", ex, model);
                return JsonResponse.failed(message: "Failed to add hobbie");
            }
        }

        [Route("hobbie/edit")]
        [HttpPut]
        public async Task<JsonResponse> editHobbie([FromBody] HobbieViewModel model)
        {
            try
            {
                var result = await repository.updateHobbie(model);
                if (!result) return JsonResponse.failed("Update hobbie failed, please try again");
                return JsonResponse.success();
            }
            catch (InvalidDataException ex)
            {
                log.info("Invalid data found - {0}", ex.Message);
                return JsonResponse.failed(message: ex.Message);
            }
            catch (Exception ex)
            {
                log.error("Update hobbie error - {0}", ex, model);
                return JsonResponse.failed(message: "Failed to update hobbie");
            }
        }


        [Route("hobbie/delete/{userid}/{id}")]
        [HttpDelete]
        public async Task<JsonResponse> deleteHobbie([FromRoute] long userid, [FromRoute] long id)
        {
            try
            {
                var result = await repository.deleteHobbie(userid, id);
                if (!result) return JsonResponse.failed("Delete hobbie failed, please try again");
                return JsonResponse.success();
            }
            catch (Exception ex)
            {
                log.error("Update user error userId: {0}, id : {1}", ex, userid, id);
                return JsonResponse.failed(message: "Failed to update hobbie");
            }
        }
    }
}
