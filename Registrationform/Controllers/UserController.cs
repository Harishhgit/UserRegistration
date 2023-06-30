using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Registrationform.Models;
using Registrationform.Services;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Hosting;

namespace Registrationform.Controllers
{
    public class UserController : Controller
    {
        private readonly UserContext _usercontext;
        private readonly ISharedservices _sharedservice;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserController(UserContext usercontext,ISharedservices sharedservice,IWebHostEnvironment webHostEnvironment)
        {
            _usercontext = usercontext;
            _sharedservice = sharedservice;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public IActionResult Userregister()
        {
            //Countrylist Dropdown
            //List<Country> Countrylist = _sharedservice.GetCountry();
            //var country = new SelectList(CountryList, "CountryId", "CountryName");
            List<Country> CountryList = _usercontext.Countries.ToList();
            ViewBag.Country_table = new SelectList(CountryList, "CountryId", "CountryName");
            

            //Gender Dropdown
            List<SelectListItem> Gender = new(){
                                                new SelectListItem {Text = "Male" },
                                                new SelectListItem {Text = "Female"}
                                                };
            ViewBag.Gender = Gender;

            //SecurityQuestions Dropdown
            List<SecQuestions> QuestionList = _usercontext.Squestion.ToList();
            ViewBag.Question_table = new SelectList(QuestionList, "SquestionId", "Squestion");
            
            return View();
            

        }


        public JsonResult GetStates(int id)
        {
            List<State> sl = _sharedservice.GetState(id);
            return Json(new SelectList(sl, "StateId", "StateName"));
        }
 
        public JsonResult GetCities(int id)
        {     
            List<City> cl = _sharedservice.GetCity(id);
            return Json(new SelectList(cl,"CityId","CityName"));
        }

        
        
        [HttpPost]
        public IActionResult Userregister(Userdetails userdetails)
        {
            //Getting country list and passing it 
            List<Country> CountryList = _sharedservice.GetCountry().ToList();
            ///working code
            //List<Country> CountryList = _usercontext.Countries.ToList();
            //ViewBag.Country_table = new SelectList(CountryList, "CountryId", "CountryName");


            //List<Country> CountryList = _usercontext.Countries.ToList();
            ViewBag.Country_table = new SelectList(CountryList, "CountryId", "CountryName");


            List<SelectListItem> Gender = new(){
                                                    new SelectListItem {Text = "Male" },
                                                    new SelectListItem {Text = "Female"}
                                               };

                ViewBag.Gender = Gender;

            List<SecQuestions> QuestionList = _usercontext.Squestion.ToList();
            ViewBag.Question_table = new SelectList(QuestionList, "SquestionId", "Squestion");

            //Email verification for existing user 
            var emailexist = _usercontext.UserDetails.Where(m => m.Email == userdetails.Email).FirstOrDefault();
            var contactexist = _usercontext.UserDetails.Where(m => m.Contact == userdetails.Contact).FirstOrDefault();


            //if (emailexist != null)
            //{
            //    ViewBag.msg = "User account already exists, use another email !";
            //}
            if (emailexist == null && contactexist == null)
            {

                try
                {
                    //If user provide profile photo in registration form then it will be uploded to project directory and in database also
                    if (userdetails.ProfilePhoto != null)
                    {
                        //Profile photo directory where photos will be uploaded
                        string folderpath = "images/ProfilePhotos/";                         //Assigning unique name to uploaded photo
                        string filename = folderpath + Guid.NewGuid().ToString() + "_" + userdetails.ProfilePhoto.FileName;                         //Using IWebHostEnvironment creating actual path in hosted environment
                        string serverfolderpath = Path.Combine(_webHostEnvironment.WebRootPath, filename); userdetails.ProfilePhotoUrl = "/" + filename;                         //Creating copy of profile photo and storing in actual path
                        userdetails.ProfilePhoto.CopyTo(new FileStream(serverfolderpath, FileMode.Create));
                    }
                    else
                    {
                        //Else user not provide profile photo then from backend system will assign sample user avatar image 
                        userdetails.ProfilePhotoUrl = "/images/ProfilePhotos/user_avatar.jpg";
                    }
                    
                    _usercontext.Add(userdetails);
                    _usercontext.SaveChanges();
                    ViewBag.Message = "User account created successfully !!";

                    ModelState.Clear();
                }
                catch 
                {
                    ViewBag.msg = "Registration Failed!";
                }
            }

            else if (emailexist != null || contactexist != null)
            {
                if (emailexist != null)
                {
                    ViewBag.msg = "Email already registered";
                    ModelState.AddModelError("Email", "Enter new email");
                    //                   
                    
                    //

                }
                else
                {
                    ViewBag.msg = "Contact number already registered";
                    ModelState.AddModelError("Contact", "Enter new contact");
                }
            }
            else
            {
                ViewBag.msg = "Email and Contact already registered";
            }

            return View();

        }




        [HttpGet]

        public IActionResult Login() 
        { 
            return View(); 
        }


        [HttpPost]
        public IActionResult Login(Userdetails userd)
        {
            var user = _usercontext.UserDetails.Where(m => m.Email == userd.Email && m.Pwd == userd.Pwd).FirstOrDefault();


            if (userd.Email == "Admin@gmail.com" && userd.Pwd == "Admin@123")
            {                
                return RedirectToAction("Userslist", "User" , new { userid = userd.UserId });
            }
            else if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.FirstName);
                return RedirectToAction("Dashboard", "User", new { userid = user.UserId });
            }
            else
            {
                ViewBag.Message = "Invalid Credentials";
                ModelState.Clear();
            }

            return View();
        }


        [HttpGet]

        public IActionResult Dashboard(int userid)
        {
            //Store the passed email to variable and make sure without providing email no one access succes page
            if (userid > 0)
            {
                HttpContext.Session.GetString("UserName");

                //Get all the details of user who logged in and store it in variable

                var user = _usercontext.UserDetails.Where(m => m.UserId == userid).ToList();
                
                ViewBag.User = user[0].FirstName;
  
                return View(user);
            }

            else
            {
                return RedirectToAction("Login", "User");
            }

        }


        public IActionResult Logout() 
        {
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login","User");
        }


        [HttpGet]
        public IActionResult Forgotpassword() 
        {
            return View(); 
        }

        
        [HttpPost]
        public IActionResult Forgotpassword(ForgotPassword model)
        {
            var useraccount = _usercontext.UserDetails.Where(m => m.Email == model.Email).FirstOrDefault();


            //If email exists then send email else Show no account registered message
            if (useraccount != null)
            {
                //Generate reset code
                Random random = new Random();
                var resetcode = random.Next(1111, 9999);

                //bind generated reset code to model ResetCode Property
                model.ResetCode = resetcode;
                try
                {
                    
                    MailMessage mailm = new MailMessage("NationalResidentRegistration@test.com", model.Email, "Reset Password", "Reset Code for Password Update : "+ resetcode);
                    mailm.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "Give your server credentials";
                    smtp.Port = 25;
                    smtp.EnableSsl = false;

                    NetworkCredential nc = new NetworkCredential("Give your credentials", "test@123");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = nc;

                    smtp.Send(mailm);
                    ViewBag.message = "Email has been sent successfully";

                    _usercontext.Add(model);
                    _usercontext.SaveChanges();

                    //Passing email to Reset Password page to validate
                    TempData["UserReqResetPass"] = model.Email;

                }
                catch
                {
                    ViewBag.msg = "Error occured";
                }

                ModelState.Clear();
            }
            else
            {
                ViewBag.msg = "Email is not registered";
            }

            return View();
        }


        [HttpGet]
        public IActionResult ResetPassword()
        {
            var email = TempData["UserReqResetPass"] as string;

            var ResetPassUser = _usercontext.UserDetails.Where(xe => xe.Email == email).FirstOrDefault();

            //if page doesn't contains email that was passed by forgot password page then it will redirect to forgot password page
            if (ResetPassUser != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forgotpassword");
            }
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                //check reset code user providing is valid or not
                //if it is valid then take the first row of ForgotPasswordUser table and store it in
                //forgotpassuser variable

                var forgotpassuser = _usercontext.ForgotPasswords.Where(m => m.ResetCode == model.ResetCode).FirstOrDefault();
                if (forgotpassuser != null)
                {
                    //check forgotpassuser's email matches with any registered user's email
                    //if it matches then take first row of UserDetails table and store it in userdetails variable

                    var userdetails = _usercontext.UserDetails.Where(e => e.Email == forgotpassuser.Email).FirstOrDefault();
                    if (userdetails != null)
                    {
                        //update password of this email in user table with new password provided here

                        userdetails.Pwd = model.NewPassword;
                        _usercontext.UserDetails.Update(userdetails);
                        _usercontext.ForgotPasswords.Remove(forgotpassuser);
                        _usercontext.SaveChanges();
                        TempData["ResetResponse"] = userdetails.Email;
                        return RedirectToAction("ResetResponse","User");
                    }
                    else
                    {
                        ViewBag.msg = "Email is not registered";
                    }
                }
                else
                {
                    ViewBag.msg = "Invalid Reset Code";
                }
            }
            ModelState.Clear();
            return View();
        }

        [HttpGet]
        public IActionResult ResetResponse() 
        {
            var response = TempData["ResetResponse"]as string;
            if (response != null)
            {
                ViewBag.Message = "Password has been updated successfully";

                //Send the confirmation mail to user that password has been updated
                try
                {
                    //Create new SmtpClient and give smtp server host and port
                    SmtpClient smtpClient = new SmtpClient("Give your server credentials", "here port number as int");
                    smtpClient.EnableSsl = false;                     //Provide credentials of email account from which we want to send mail in NetworkCredential
                    NetworkCredential nc = new NetworkCredential("Give your credentials", "test@123");
                    smtpClient.Credentials = nc;                     //Provide emailfrom, emailto, emailsubject, emailbody to Send method 
                    smtpClient.Send("NationalResidentRegistration@test.com", response, "Your Password has been updated", "Hi, we are confirming that your password has been updated successfully.");
                }
                catch
                {
                    ViewBag.msg = "Error occured";
                }
            }
            else 
            {
                return RedirectToAction("Forgotpassword", "User");
            }
            return View();
        }

        public IActionResult Userslist(int pg=1)
        {
            List<Userdetails> users = _usercontext.UserDetails.ToList();

            const int pageSize = 4;
            if (pg < 1)
                pg = 1;

            int recsCount = users.Count();

            var pager = new Pager(recsCount, pg, pageSize);

            int recSkip = (pg - 1) * pageSize;

            var data = users.Skip(recSkip).Take(pager.PageSize).ToList();

            this.ViewBag.Pager = pager;


            return View(data);

        }

        [HttpGet]
        public IActionResult Useredit(int id)
        {
            var data = _usercontext.UserDetails.Where(uid => uid.UserId == id).FirstOrDefault();
           
            return View(data);
        }

        [HttpPost]

        public IActionResult Useredit(int id,Userdetails userdetails)
        {
            var data = _usercontext.UserDetails.Where(uid => uid.UserId == id).FirstOrDefault();
            ///
            var userdata = data.FirstName == userdetails.FirstName && data.LastName == userdetails.LastName && data.DOB == userdetails.DOB && data.Contact == userdetails.Contact && data.Email == userdetails.Email;             //If user has made any change in details then save the recently changed details in database
            if (userdata != true)
            {
                data.FirstName = userdetails.FirstName;
                data.LastName = userdetails.LastName;
                data.DOB = userdetails.DOB;
                data.Contact = userdetails.Contact;

                data.Email = userdetails.Email;

                _usercontext.SaveChanges();                 //Send confirmation mail to user that profile has been updated
                try
                {
                    //Create new SmtpClient and give smtp server host and port
                    SmtpClient smtpClient = new SmtpClient("Give your server credentials", 25);
                    smtpClient.EnableSsl = false;                     //Provide credentials of email account from which we want to send mail in NetworkCredential
                    NetworkCredential nc = new NetworkCredential("Give your credentials", "test@123");
                    smtpClient.Credentials = nc;                     //Provide emailfrom, emailto, emailsubject, emailbody to Send method 
                    smtpClient.Send("NationalResidentRegistration@test.com", data.Email, "Your profile has been updated", "Hi, we are confirming that your profile has been successfully updated.");
                }
                catch
                {
                    TempData["msg"] = "Error occured";
                }

                TempData["EditResponse"] = "Your profile has been updated";

                return RedirectToAction("Dashboard", new { userid = data.UserId });
            }

            else
            {
                TempData["ErrorMessage"] = "Details have not been Changed";

                return View();
            }

            }

        public IActionResult Adminedit(int id)
        {
            var data = _usercontext.UserDetails.Where(uid => uid.UserId == id).FirstOrDefault();

            return View(data);
        }

        [HttpPost]
        public IActionResult Adminedit(int id, Userdetails userdetails)
        {
            var data = _usercontext.UserDetails.Where(uid => uid.UserId == id).FirstOrDefault();

            //return View();

            if (data != null)
            {
                data.FirstName = userdetails.FirstName;
                data.LastName = userdetails.LastName;
                data.Contact = userdetails.Contact;
                data.DOB = userdetails.DOB;

                _usercontext.SaveChanges();

                TempData["Message"] = "User details has been updated";

                return RedirectToAction("Userslist", new { userid = data.UserId });
            }

            return View();

        }




        public IActionResult Deleteuser(int id) 
        {
            var data= _usercontext.UserDetails.Where(uid => uid.UserId == id).FirstOrDefault();

            return View(data);
        }

        [HttpPost]
        public IActionResult Deleteuser(int id,Userdetails userdetails)
        {
            var data = _usercontext.UserDetails.Where(uid => uid.UserId == id).FirstOrDefault();

            if (data != null) 
            {
                _usercontext.UserDetails.Remove(data);
                _usercontext.SaveChanges();

                //ViewBag.delete = "User Record has been Deleted";

                return RedirectToAction("Userslist", new { userid = data.UserId});
            }

            return View();

        }

        [HttpGet]
        public IActionResult DeleteView() 
        {
            return View();
        }

    }
}
