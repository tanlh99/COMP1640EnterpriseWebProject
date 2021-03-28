using EnterpriseWebProject.DAL;
using EnterpriseWebProject.EmailForm;
using EnterpriseWebProject.Models;
using EnterpriseWebProject.ViewModel;
using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace EnterpriseWebProject.Controllers
{
    public class HomeController : Controller
    {
        EWContextDb db = new EWContextDb();
        private Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "0123456789QWEWRTYUIOPASDFGHJKLLZXCVBNMqwertyuiopasdfghjklzxcvbnm";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public ActionResult Index()
        {
            //Random random = new Random();
            //var listFac = new List<string>();
            //int FacNo = 4; ////So luong Faculty
            //int MagaId = 31; ////Id cua magazine muon add contribution vao
            //int month = 2; ////Thang cua magazine
            //int year = 2021; ////Năm của magazine
            //int contriNumber = 10; ////Số lượng contribution muốn tạo


            ////Auto assign Faculty to Magazine (Assign Faculty ngẫu nhiên cho Magazine)
            //for (int i = 0; i < FacNo; i++)
            //{
            //    int ranFac = random.Next(1, 5);
            //    if (listFac.Contains(ranFac.ToString()) == false)
            //    {
            //        listFac.Add(ranFac.ToString());
            //        System.Diagnostics.Debug.WriteLine("INSERT INTO Magazine_Faculty([MagazineId],[FacultyId]) VALUES({0},{1})", MagaId, ranFac);
            //    }
            //}

            ////Create data for contribution in Magazine 
            //var getStdbyFac = db.Accounts.Where(std1 => std1.RoleId == 4).ToList();
            //for (int i = 0; i < contriNumber; i++)
            //{
            //    int StudentFac;
            //    int StudentId;
            //    int arrFacindex = random.Next(listFac.Count);
            //    int ranFac = Convert.ToInt32(listFac.ElementAt(arrFacindex));
            //    int day = random.Next(1, 30);
            //    var FacStd = db.Accounts_Faculties.Where(m => m.FacultyId == ranFac).ToList();
            //    Account_Faculty student = FacStd.ElementAt(random.Next(1, FacStd.Count));
            //    //var StdFac = db.Accounts_Faculties.Where(m => m.AccountId == student.Id).FirstOrDefault();
            //    StudentId = student.AccountId;
            //    StudentFac = student.FacultyId;
            //    System.Diagnostics.Debug.WriteLine("INSERT INTO Contributions([SubmitDate],[Status],[AccountId],[MagazineId],[FacultyId]) VALUES('0{4}-{2}-{5} 07:57:10','False','{0}','{3}','{1}');", StudentId, StudentFac, day, MagaId, month, year);
            //}

            ////Add file data to contribution
            //List<string> Description = new List<string>();
            //Description.Add("No thief, however skillful, can rob one of knowledge, and that is why knowledge is the best and safest treasure to acquire.");
            //Description.Add("Knowledge is seemingly endless, we cannot underestimate the importance of knowledge in today’s society. It can be said that each of us can contribute a part of personal knowledge to build a great tree so that we can exchange and learn from each other, by each student can write magazine articles.");
            //Description.Add("Lorem ipsum dolor sit , consectetur adipiscing elit, sed do iusmod tempor incididunt ut labore et dolore magna aliqua. Quis ipsum suspendisse ultrices.");
            //List<string> Name = new List<string>();
            //Name.Add("Good Contribution");
            //Name.Add("Nice Contribution");
            //Name.Add("Super Contribution");
            //List<string> FileType = new List<string>();
            //FileType.Add("MyContribution.docx");
            //FileType.Add("MyContributionA.docx");
            //FileType.Add("MyContributionB.docx");
            //List<string> FileName = new List<string>();
            //FileName.Add("catmeme.png");
            //FileName.Add("heheaa.png");
            //FileName.Add("haha.jpg");

            //var contri = db.Contributions.ToList();
            //for (int i = 1; i < 161; i++)
            //{

            //    //var getContribyMagaId = db.Contributions.Where(m => m.MagazineId == i).ToList(); //Lưu ý i là id của magazine
            //    foreach (var item in contri)
            //    {
            //        string name = Name.ElementAt(random.Next(0, 3));
            //        string filetype = FileType.ElementAt(random.Next(0, 3));
            //        string filename = FileName.ElementAt(random.Next(0, 3));
            //        string description = Description.ElementAt(random.Next(0, 3));
            //        System.Diagnostics.Debug.WriteLine("INSERT INTO Files([Name],[FileType],[FileName],[Description],[ContributionId]) " +
            //            "VALUES('{0}','{1}','{2}','{3}','{4}');", name, filetype, filename, description, item.Id);
            //    }
            //}

            //Auto assign faculty for student (Assign faculty ngẫu nhiên cho student ko co Faculty)
            //var StudentNoFac = db.Accounts.Where(m => m.RoleId == 4 && m.Account_Faculties.Count == 0).ToList();
            //foreach (var item in StudentNoFac)
            //{
            //    int ranFac = random.Next(1, FacNo + 1);
            //    System.Diagnostics.Debug.WriteLine("INSERT INTO Account_Faculty([AccountId],[FacultyId]) VALUES({0},{1})", item.Id, ranFac);
            //}

            //Auto assign faculty for coordi (Assign faculty ngẫu nhiên cho student ko co Faculty)
            //var CoorNoFac = db.Accounts.Where(m => m.RoleId == 3 && m.Account_Faculties.Count == 0).ToList();
            //foreach (var item in CoorNoFac)
            //{
            //    int ranFac = random.Next(1, FacNo + 1);
            //    System.Diagnostics.Debug.WriteLine("INSERT INTO Account_Faculty([AccountId],[FacultyId]) VALUES({0},{1})", item.Id, ranFac);
            //}
            return View();
        }
        public ActionResult Magazine(int? page)
        {
            var list = db.Magazines.Where(m => m.Status == false && m.PDFfile != null).ToList();
            list.Sort((m, n) => n.Id.CompareTo(m.Id));
            return View(list.ToPagedList(page ?? 1, 6));
        }       
        //Download Magazine
        public FileResult DownloadMagazine(int magaId)
        {
            var maga = db.Magazines.Where(m => m.Id == magaId).FirstOrDefault();
            var fileName = Path.GetFileName(maga.PDFfile);
            var path = Path.Combine(Server.MapPath("~/Files/Doc/"), fileName);

            return File(path, "pdf", maga.PDFfile);
        }

        //GET: Login page
        public ActionResult Login()
        {
            return View();
        }
        //POST: Login
        [HttpPost]
        public ActionResult LoginAccount(LoginViewModel model)
        {          
            if (ModelState.IsValid)
            {
                Account acc = db.Accounts.Where(a => a.Username == model.Username && a.Password == model.Password).FirstOrDefault();
                if (acc != null)
                {
                    FormsAuthentication.SetAuthCookie(acc.Username, model.rememberMe == false);
                    return RedirectToAction("Profile","Home");
                }
                else
                {
                    ViewBag.err = String.Format("Your Password or Username is incorrect!");
                }
            }          
            return View("Login", model);
        }
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("Index");
        }

        // MANAGE: invidual profile
        [Authorize]
        public ActionResult Profile()
        {
            var user = db.Accounts.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            return View(user);
        }
        //GET: change profile page
        [Authorize]
        [HttpGet]
        public ActionResult ChangeProfile()
        {
            UpdateProfileViewModel model = new UpdateProfileViewModel();
            var user = db.Accounts.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            model.Name = user.Name;
            model.DOB = user.DOB;
            model.Email = user.Email;
            model.Tel = user.Tel;

            model.Avatar = user.Avatar;
          
            return View(model);
        }
        ////POST: change profile
        [Authorize]
        [HttpPost]
        public ActionResult ChangeProfile(UpdateProfileViewModel model,HttpPostedFileBase avatar)
        {
            var user = db.Accounts.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
      
            if (ModelState.IsValid)
            {
                user.Name = model.Name;
                user.DOB = model.DOB;
                user.Tel = model.Tel;

                //change avatar in profile
                if (avatar != null)
                {
                    var img = Path.GetFileName(avatar.FileName);
                    var pathImg = Path.Combine(Server.MapPath("~/Files/Avatar/"), img);
                    avatar.SaveAs(pathImg);
                    user.Avatar = img;
                }
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("Profile");
            }
            user.Avatar = model.Avatar;
            user.Name = model.Name;
            return View("ChangeProfile", model);
        }

        //GET: change password page
        [Authorize]
        public ActionResult ChangePassword()
        {
            ChangePasswordModel model = new ChangePasswordModel();
            var user = db.Accounts.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            model.Name = user.Name;
            model.Avatar = user.Avatar;
            return View(model);
        }
        //POST: change password
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var curUser = db.Accounts.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            model.Name = curUser.Name;
            model.Avatar = curUser.Avatar;
            var user = db.Accounts.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            
            if (ModelState.IsValid)
            {
                if (user.Password.Contains(model.OldPassword))
                {
                    user.Password = model.NewPassword;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.Error = String.Format("Current password is not correct!");
                }
                return Redirect("Profile");
            }    

            return View(model);
        }

        //GET: ForgotPassword page
        public ActionResult ForgotPassword()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = db.Accounts.Where(m => m.Email == model.Email).FirstOrDefault();
                if (account == null)
                {
                    ViewBag.err = String.Format("Email is not correct!");                 
                }

                string code = RandomString(50);
                var callbackUrl = Url.Action("ResetPassword", "Home", new { userId = account.Id, code = code }, protocol: Request.Url.Scheme);
                string body = string.Empty;
                using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailForm/ForgotPasswordForm.html")))
                {
                    body = reader.ReadToEnd();
                }
                body = body.Replace("{ConfirmationLink}", callbackUrl);
                body = body.Replace("{UserName}", model.Email);
                bool IsSendEmail = SendEmail.GmailSend(model.Email, "Confirm your account", body, true);
            }
            return View();
        }

        //GET: ResetPassword page
        public ActionResult ResetPassword(string code, int userId)
        {
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            model.Id = userId;
            return code == null ? View("Error") : View(model);
        }
        //POST:ResetPassword
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Accounts.Where(m => m.Id == model.Id).FirstOrDefault();

                user.Password = model.Password;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("Login");
            }
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}