using EnterpriseWebProject.DAL;
using EnterpriseWebProject.EmailForm;
using EnterpriseWebProject.Models;
using EnterpriseWebProject.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EnterpriseWebProject.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        EWContextDb db = new EWContextDb();

        // GET: Contribution
        public ActionResult Contribution(int? page)
        {
            var list = db.Contributions.Where(m => m.Account.Username == User.Identity.Name).ToList();
            List<Models.File> file = new List<Models.File>();
            foreach (var item in list)
            {
                file.Add(db.Files.Where(m => m.ContributionId == item.Id).FirstOrDefault());
            }

            file.Sort((m, n) => n.Id.CompareTo(m.Id));

            return View(file.ToPagedList(page ?? 1, 10));
        }

        //GET: Contribution with submit file Form
        public ActionResult ContributionSubmitFile()
        {
            return View();
        }
        //POST: Submit File with Contribution Id
        [HttpPost]
        public ActionResult ContributionSubmitFile(Contri_FileViewModel model, HttpPostedFileBase fileDoc,HttpPostedFileBase fileImage)
        {
            if (ModelState.IsValid)
            {
                //Contribution database
                Contribution objContri = new Contribution();
                var userLogged = db.Accounts.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
                var acc_facLogged = db.Accounts_Faculties.Where(m => m.Account.Username == User.Identity.Name).FirstOrDefault();
                var magazineId = db.Magazines.Where(x => x.Status == true).FirstOrDefault();
                var maga_facList = db.Magazines_Faculties.Where(m => m.MagazineId == magazineId.Id).ToList();

                objContri.AccountId = userLogged.Id;
                objContri.SubmitDate = DateTime.Now;
                objContri.Status = null;
                foreach (var item in maga_facList)
                {
                    if (item.FacultyId == acc_facLogged.FacultyId)
                    {
                        objContri.FacultyId = item.FacultyId;
                    }
                }
                objContri.MagazineId = magazineId.Id;

                db.Contributions.Add(objContri);
                db.SaveChanges();

                int contriId = objContri.Id;
                //File Database
                Models.File obj = new Models.File();
                string doc = null;
                string image = null;
                if (fileImage != null)
                {
                    image = System.IO.Path.GetFileName(fileImage.FileName);
                    string imagePath = System.IO.Path.Combine(Server.MapPath("~/Files/Image/"), image);
                    fileImage.SaveAs(imagePath);
                    obj.FileName = fileImage != null ? image : obj.FileName;

                }
                if (fileDoc != null)
                {
                    doc = System.IO.Path.GetFileName(fileDoc.FileName);                
                    string docPath = System.IO.Path.Combine(Server.MapPath("~/Files/Doc/"), doc);
                    
                    fileDoc.SaveAs(docPath);
                   
                    obj.ContributionId = contriId;
                    obj.Name = model.Name;
                    obj.FileType = fileDoc != null ? doc : obj.FileType;                    
                    obj.Description = model.Description;

                    db.Files.Add(obj);
                    db.SaveChanges();

                    var user = db.Accounts.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
                    var acc_fac = db.Accounts_Faculties.ToList();

                    var Coor = db.Accounts.Where(m => m.Role.Name == "Coordinator").ToList();
                    var facId = 0;
                    List<string> coorList = new List<string>();
                    foreach (var item in acc_fac)
                    {
                        if (item.AccountId == user.Id)
                        {
                            facId = item.FacultyId;
                        }
                    }

                    foreach (var item in Coor)
                    {
                        foreach (var item2 in item.Account_Faculties)
                        {
                            if (item2.FacultyId == facId)
                            {
                                coorList.Add(item2.Account.Email);
                            }
                        }
                    }
                    //string x = 
                    //var rand = new Random();
                    //int x = rand.
                    //string gmail = rand.Next(coorList.)

                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailForm/SendMailWithNewContriBution.html")))
                    {
                        body = reader.ReadToEnd();
                    }
                    body = body.Replace("{Student}", user.Email);
                    foreach (var item in coorList)
                    {
                        body = body.Replace("{Coordinator}", item);
                        bool IsSendEmail = SendEmail.GmailSend(item, "Notification", body, true);
                    }
                }

                return RedirectToAction("Contribution", "Student");
            }
            return View(model);
        }

        //GET: Edit File submitted in contribution 
        public ActionResult EditFileInContribution(int contriId)
        { 
            var file = db.Files.Where(m => m.Contribution.Id == contriId).FirstOrDefault();
            UpdateFileViewModel model = new UpdateFileViewModel();
            model.Name = file.Name;
            model.Description = file.Description;
            model.ContributionId = file.ContributionId;
            model.FileDoc = file.FileType;
            model.FileImg = file.FileName;
    
            return View(model);
        }
        //POST
        [HttpPost]
        public ActionResult EditFileInContribution(UpdateFileViewModel model, HttpPostedFileBase FileDoc, HttpPostedFileBase FileImg)
        {
            var file = db.Files.Where(m => m.Contribution.Id == model.ContributionId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                file.Name = model.Name;
                file.Description = model.Description;
                if (FileDoc != null)
                {
                    string doc = System.IO.Path.GetFileName(FileDoc.FileName);
                    string docPath = System.IO.Path.Combine(Server.MapPath("~/Files/Doc/"), doc);
                    FileDoc.SaveAs(docPath);
                    model.FileDoc = FileDoc != null ? doc : model.FileDoc;
                    file.FileType = model.FileDoc;

                }
                if (FileImg != null)
                {
                    string img = System.IO.Path.GetFileName(FileImg.FileName);
                    string imgPath = System.IO.Path.Combine(Server.MapPath("~/Files/Image/"), img);
                    FileImg.SaveAs(imgPath);
                    model.FileImg = FileImg != null ? img : model.FileImg;
                    file.FileName = model.FileImg;

                }
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("Contribution");
            }
            return View(model);
        }
        //DELETE Contribution
        public ActionResult DeleteContribution(int contriId)
        {
            var contri = db.Contributions.Where(m => m.Id == contriId).FirstOrDefault();
            var file = db.Files.Where(m => m.ContributionId == contriId).FirstOrDefault();
            var comment = db.Comments.Where(m => m.ContributionId == contriId).ToList();

            db.Contributions.Remove(contri);
            db.Files.Remove(file);
            foreach (var item in comment)
            {
                db.Comments.Remove(item);
            }
            db.SaveChanges();

            return Redirect("Contribution");
            
        }
        //GET: Comment page
        public ActionResult ViewContributionAndComment(int contriId, int numb)
        {
            CommentViewModel model = new CommentViewModel();
            var contri = db.Contributions.Find(contriId);
            var file = db.Files.Where(m => m.Contribution.Id == contriId).FirstOrDefault();
            var comment = db.Comments.Where(m => m.ContributionId == contriId).ToList();

            model.Number = numb;
            model.SubmitDate = contri.SubmitDate;
            model.Status = contri.Status;
            model.StudentName = contri.Account.Name;
            model.Avatar = contri.Account.Avatar;


            model.Name = file.Name;
            model.FileDoc = file.FileType;
            model.FileImg = file.FileName;
            model.Description = file.Description;

            model.ContributionId = contri.Id;
            model.AccountId = contri.AccountId;
            //List<List<string>> viewComment = new List<List<string>>();
            //foreach (var item in comment)
            //{
            //    List<string> AccountAndComment = new List<string>();
            //    AccountAndComment.Add(item.Account.Username);
            //    AccountAndComment.Add(item.Message);

            //    viewComment.Add(AccountAndComment);
            //}
            //ViewBag.List = viewComment;
            List<Comment> list = new List<Comment>();
            foreach (var item in comment)
            {
                list.Add(item);
            }
            ViewBag.list = list;

            return View(model);
        }
        [HttpPost]
        public ActionResult CommentSubmit(CommentViewModel model)
        {
            if(ModelState.IsValid)
            {
                Comment obj = new Comment();

                obj.Message = model.Message;
                obj.ContributionId = model.ContributionId;
                obj.AccountId = model.AccountId;
                obj.SubmitDate = DateTime.Now.ToString();

                db.Comments.Add(obj);
                db.SaveChanges();

                return RedirectToAction("ViewContributionAndComment", new { contriId = model.ContributionId, numb = model.Number });
            }
            return View();
        }
    }
}