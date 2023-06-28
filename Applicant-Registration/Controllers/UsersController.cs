using Applicant_Registration.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Applicant_Registration.Controllers
{

    public class UsersController : ApiController
    {
       

        private const string LocalLoginProvider = "Local";

        private ApplicationUserManager _userManager;


        public UsersController()

        {

        }


        public UsersController(ApplicationUserManager userManager)

        {

            UserManager = userManager;

            

        }



        public ApplicationUserManager UserManager

        {

            get

            {

                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            }

            private set

            {

                _userManager = value;

            }

        }


        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }


        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> Register(Users model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email
            };  

            try
            {
                var result = await UserManager.CreateAsync(user,model.Password);

                if (result.Succeeded)   
                {
                    

                    return Ok(result);
                }
                else
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions that may occur during the registration process
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Login(Users model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check user credentials
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null || !await UserManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized();
            }

            return Ok(user);
        }



        [HttpGet]
        [Route("userdetails")]
        [Authorize]
        public IHttpActionResult GetUserDetails()
        {
            // Retrieve the current  user
            var identity = User.Identity as ClaimsIdentity;
            var username = HttpContext.Current.User.Identity.Name;
            var usdr = UserManager.FindByName(username);

            if (identity != null)
            {
                // Retrieve the user's details from the claims
                var userDetails = new ApplicationUser
                {
                    UserName = identity.FindFirst(ClaimTypes.Name)?.Value,
                    Email = identity.FindFirst(ClaimTypes.Email)?.Value
                };

                return Ok(usdr);
            }

            return BadRequest("User details not found.");
        }

        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            // Clear authentication-related information to log out the user
            Request.GetOwinContext().Authentication.SignOut();

            return Ok();
        }               
    }
    }














