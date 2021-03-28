using EnterpriseWebProject.DAL;
using EnterpriseWebProject.Models;
using EnterpriseWebProject.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnterpriseWebProject.Controllers
{
    [Authorize(Roles = "Coordinator")]
    public class CoordinatorController : Controller
    {
        // GET: Coordinator
        EWContextDb db = new EWContextDb();
        public ActionResult ContributionSubmited(int? page)
        {
            var fac_acc = db.Accounts_Faculties.Where(m => m.Account.Username == User.Identity.Name).FirstOrDefault();
            var list = db.Files.Where(m => m.Contribution.FacultyId == fac_acc.FacultyId  && m.Contribution.Magazine_Faculty.Magazine.Status == true).ToList().ToPagedList(page ?? 1, 10 );
                         
            return View(list);
        }
        //GET: 
        public ActionResult ContributionDetail(int contriId, int numb)
        {
            CommentViewModel model = new CommentViewModel();
            var contri = db.Contributions.Find(contriId);
            var file = db.Files.Where(m => m.Contribution.Id == contriId).FirstOrDefault();
            var comment = db.Comments.Where(m => m.ContributionId == contriId).ToList();

            //get coordinator Id into comment with accountId
            var coordinator = db.Accounts.Where(m => m.Username == User.Identity.Name).FirstOrDefault();

            model.Number = numb;
            model.SubmitDate = contri.SubmitDate;
            model.Status = contri.Status;
            model.StudentName = contri.Account.Name;
            model.Avatar = coordinator.Avatar;

            model.Name = file.Name;
            model.FileDoc = file.FileType;
            model.FileImg = file.FileName;
            model.Description = file.Description;

            model.ContributionId = contri.Id;
            model.AccountId = coordinator.Id;
            //List<List<string>> viewComment = new List<List<string>>();
            //foreach (var item in comment)
            //{
            //    //model.ViewMessage = item.Message;
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
        //Download function
        public FileResult DownloadFile(int contriId)
        {
            var file = db.Files.Where(m => m.ContributionId == contriId).FirstOrDefault();
            var path = Server.MapPath("~/Files/Doc/");
            var fileDoc = Path.GetFileName(file.FileType);

            var fullPath = Path.Combine(path, fileDoc);
            return File(fullPath, "docx", file.FileType);
        }
        //POST: 
        [HttpPost]
        public ActionResult CoordinatorComment(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Comment obj = new Comment();
                obj.Message = model.Message;
                obj.AccountId = model.AccountId;
                obj.ContributionId = model.ContributionId;
                obj.SubmitDate = DateTime.Now.ToString();

                db.Comments.Add(obj);
                db.SaveChanges();

                return RedirectToAction("ContributionDetail", new { contriId = model.ContributionId, numb = model.Number });
            }
            return View();
        }
        //GET: 
        public ActionResult ApproveContribution(int contriId)
        {
            var findId = db.Contributions.Where(m => m.Id == contriId).FirstOrDefault();

            CommentViewModel model = new CommentViewModel();

            model.ContributionId = findId.Id;
            model.Status = findId.Status;
            
            return View(model);
        }
        //POST:
        [HttpPost]
        public ActionResult ApproveContribution(CommentViewModel model)
        {
            var obj = db.Contributions.Where(m => m.Id == model.ContributionId).FirstOrDefault();

            obj.Status = model.Status;

            db.Entry(obj).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect("ContributionSubmited");
        }
    }
}