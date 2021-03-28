using EnterpriseWebProject.DAL;
using EnterpriseWebProject.Models;
using EnterpriseWebProject.ViewModel;
using Newtonsoft.Json;
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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        EWContextDb db = new EWContextDb();

        public List<SelectListItem> RoleList()
        {
            List<SelectListItem> role = new List<SelectListItem>();
            var roleList = db.Roles.ToList();
            foreach(var item in roleList)
            {
                role.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            return role;
        }
        
        //Manage Account
        public ActionResult Account(int? page)
        {
            IPagedList<Account> acc = db.Accounts.ToList().ToPagedList(page ?? 1, 10);

            //Fk account Id in contribution database
            List<Contribution> contri = db.Contributions.ToList();
            List<int> contriId = new List<int>();
            foreach (var item in contri)
            {
                contriId.Add(item.AccountId);
            }
            ViewBag.contriId = contriId;
            //Fk account Id in Account_Faculty database
            List<Account_Faculty> accFac = db.Accounts_Faculties.ToList();
            List<int> accFacId = new List<int>();
            foreach (var item in accFac)
            {
                accFacId.Add(item.AccountId);
            }
            ViewBag.accFacId = accFacId;
            return View(acc);
        }
        //GET: Create Account page
        public ActionResult AccountCreate()
        {
            ViewBag.Roles = RoleList();
            return View();
        }
        //POST: Create Account
        [HttpPost]
        public ActionResult AccountCreate(Account account)
        {
            ViewBag.Roles = RoleList();
            if (ModelState.IsValid)
            {              
                Account accObj = new Account();

                accObj.Id = account.Id;

                accObj.RoleId = account.RoleId;
                accObj.Username = account.Username;
                accObj.Password = account.Password;
                accObj.Name = account.Name;
                accObj.DOB = account.DOB;
                accObj.Tel = account.Tel;
                accObj.Email = account.Email;

                ////upload avatar for account
                //string img = Path.GetFileName(avatar.FileName);
                //string pathImg = Path.Combine(Server.MapPath("~/Files/Avatar/"), img);
                //avatar.SaveAs(pathImg);
                //accObj.Avatar = img;

                var usernameExist = db.Accounts.Where(m => m.Username == account.Username).Any();
                var emailExist = db.Accounts.Where(m => m.Email == account.Email).Any();
                if (!usernameExist && !emailExist)
                {
                    db.Accounts.Add(accObj);
                    db.SaveChanges();
                }else if(emailExist)
                {
                    ViewBag.emailErr = String.Format("This Email is already exist!");
                    return View();
                }
                else if(usernameExist)
                {
                    ViewBag.usernameErr = String.Format("This Username is already exist!");
                    return View();
                }      
                return Redirect("Account");
            }

            return View(account);        
        }
        //GET: Edit Account page
        public ActionResult AccountEdit(int accountId)
        {
            AdminEditAccViewModel model = new AdminEditAccViewModel();

            var obj = db.Accounts.Where(m => m.Id == accountId).FirstOrDefault();

            model.NewRole = obj.RoleId;
            model.Username = obj.Username;
            model.NewPassword = obj.Password;

            ViewBag.Roles = RoleList();


            return View(model);
        }
        //POST: Edit Account
        [HttpPost]
        public ActionResult AccountEdit(AdminEditAccViewModel model)
        {
            ViewBag.Roles = RoleList();
            var obj = db.Accounts.Where(m => m.Username == model.Username).FirstOrDefault();

            if (ModelState.IsValid)
            {
                obj.RoleId = model.NewRole;
                obj.Password = model.NewPassword;

                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();

                return Redirect("Account");
            }
            return View(model);
        }
        
        //GET: Delete Account
        public ActionResult AccountDelete(int accId)
        {
            var accDel = db.Accounts.Where(a => a.Id == accId).FirstOrDefault();
            db.Accounts.Remove(accDel);
            db.SaveChanges();

            return Redirect("Account");
        }
        //GET: Assign Faculty to Account
        public ActionResult AssignFac(int accId)
        {
            var id = db.Accounts.Find(accId);

            //Get Account_faculty data in db
            List<string> facDb = new List<string>();
            var facList = db.Accounts_Faculties.Where(a => a.AccountId == accId).ToList();
            foreach(var item in facList)
            {
                facDb.Add(item.FacultyId.ToString());
            }
            ViewBag.FacDb = facDb;

            //Select Faculty Db List
            List<SelectListItem> list = new List<SelectListItem>();
            var fac = db.Faculties.ToList();
            foreach(var item in fac)
            {
                list.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name });
            }
            ViewBag.facList = list;

            return View(id);
        }
        //POST: Assign Faculty to Acc
        [HttpPost]
        public ActionResult AssignFac(int accId, int[] facId)
        {
           
            foreach(var item in facId)
            {
                Account_Faculty obj = new Account_Faculty();
                obj.AccountId = accId;
                obj.FacultyId = item;

                db.Accounts_Faculties.Add(obj);
                db.SaveChanges();
            }

            return Redirect("Account");
        }
        //GET: View Assign Faculty page
        public ActionResult ViewAssignFac(int accId)
        {
            var accFacList = db.Accounts_Faculties.Where(a => a.AccountId == accId).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach(var item in accFacList)
            {
                list.Add(new SelectListItem { Value = item.FacultyId.ToString(), Text = item.Faculty.Name});
            }
            ViewBag.list = list;

            var id = db.Accounts.Find(accId);
            return View(id);
        }
        //Delete Assign Faculty 
        public ActionResult DelAssignFac(int accId, int[] facId)
        {

            foreach(var item in facId)
            {
                var acc_facId = db.Accounts_Faculties.Where(a => a.AccountId == accId && a.FacultyId == item).FirstOrDefault();

                db.Accounts_Faculties.Remove(acc_facId);
                db.SaveChanges();
            }

            return Redirect("Account");
        }

        //MANAGE: Role
        public ActionResult Role()
        {
            List<Role> roles = db.Roles.ToList();
            return View(roles);
        }
        //GET: Create Role
        [HttpGet]
        public ActionResult RoleCreate()
        {
            return View();
        }
        [HttpGet]
        public ActionResult RoleEdit(int roleId)
        {
            var id = db.Roles.Find(roleId);
            return View(id);
        }
        //POST: Create or Edit Role
        [HttpPost]
        public ActionResult RoleCreateOrEdit(Role role)
        {
            if (ModelState.IsValid)
            {
                Role obj = new Role();
                var isExist = db.Roles.Where(m => m.Name == role.Name).Any();

                obj.Id = role.Id;
                obj.Name = role.Name;

                if (role.Id == 0)
                {
                    if (!isExist)
                    {
                        db.Roles.Add(obj);
                        db.SaveChanges();
                        return Redirect("Role");
                    }
                    else
                    {
                        ViewBag.error = String.Format("This role already exists!");
                    }
                }
                else
                {
                    if (!isExist)
                    {
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        return Redirect("Role");   
                    }
                    else
                    {
                        ViewBag.error = String.Format("This role already exists!");

                    }               
                }     
            }
            if (role.Id == 0)
            {
                return View("RoleCreate", role);
            }
            return View("RoleEdit", role);         
        }
        //GET: Delete Role
        public ActionResult RoleDelete(int roleId)
        {        
            int x = db.Accounts.Where(r => r.RoleId == roleId).Count();
            if (x == 0)
            {
                var roleDel = db.Roles.Where(r => r.Id == roleId).FirstOrDefault();
                db.Roles.Remove(roleDel);
                db.SaveChanges();
            }
            else
            {
                ViewBag.Err = String.Format("Cannot delete this row!!!");
                return View();
            }
            return Redirect("Role");
        }

        //MANAGE: Academic Year
        public ActionResult AcademicYear()
        {
            List<AcademicYear> list = db.AcademicYears.ToList();

            //check FK of AcademicId in Magazine
            List<Magazine> maga = db.Magazines.ToList();
            List<int> magaId = new List<int>();
            foreach (var item in maga)
            {
                 magaId.Add(item.Id);
            }
            ViewBag.magaId = magaId;
            

            return View(list);
        }
        //GET: Create AcaYear
        public ActionResult AcaYearCreate()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            int year = Convert.ToInt32(DateTime.Now.Year);
            for (int i = year; i < year + 5; i++)
            {
                list.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
            }
            
            ViewBag.Year = list;       
            return View();
        }
        //POST: Create AcaYear
        [HttpPost]
        public ActionResult AcaYearCreate(AcademicYear academic)
        {
            if (ModelState.IsValid)
            {
                List<SelectListItem> list = new List<SelectListItem>();
                int year = Convert.ToInt32(DateTime.Now.Year);
                for (int i = year; i < year + 5; i++)
                {
                    list.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                }

                ViewBag.Year = list;

                var isExist = db.AcademicYears.Where(m => m.YearNo == academic.YearNo).Any();
                if (!isExist)
                {
                    AcademicYear obj = new AcademicYear();
                    obj.YearNo = academic.YearNo;

                    db.AcademicYears.Add(obj);
                    db.SaveChanges();

                    return Redirect("AcademicYear");
                }
                else
                {
                    ViewBag.error = String.Format("This Academic year is already exist!");
                }
            }                   

            return View(academic);
        }
        //GET: Delte AcaYear
        public ActionResult AcaYearDelete(int yearId)
        {
            var id = db.AcademicYears.Where(y => y.Id == yearId).FirstOrDefault();
            db.AcademicYears.Remove(id);
            db.SaveChanges();

            return Redirect("AcademicYear");
        }

        //MANAGE: Faculty
        public ActionResult Faculty()
        {
            List<Faculty> faculties = db.Faculties.ToList();

            var acc_facList = db.Accounts_Faculties.ToList();
            var maga_facList = db.Magazines_Faculties.ToList();

            List<string> listAF = new List<string>();
            List<string> listMF = new List<string>();
            foreach (var item in acc_facList)
            {
                listAF.Add(item.FacultyId.ToString());
            }
            ViewBag.listAF = listAF;
            foreach (var item1 in maga_facList)
            {
                listMF.Add(item1.FacultyId.ToString());
            }
            ViewBag.listMF = listMF;

            return View(faculties);
        }
        //GET: Create Faculty
        public ActionResult FacultyCreate()
        {
            return View();
        }
        //GET: Edit Faculty
        public ActionResult FacultyEdit(int facId)
        {
            var id = db.Faculties.Find(facId);
            return View(id);
        }
        //POST: Create Fculty
        [HttpPost]
        public ActionResult FacultyCreateOrEdit(Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                Faculty obj = new Faculty();
                obj.Id = faculty.Id;
                obj.Name = faculty.Name;
                obj.Description = faculty.Description;

                var isExist = db.Faculties.Where(m => m.Name == faculty.Name).Any();
                if (!isExist)
                {
                    if (faculty.Id == 0)
                    {
                        db.Faculties.Add(obj);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                    return Redirect("Faculty");
                }
                else
                {
                    ViewBag.error = String.Format("This faculty is already exist!");
                }
                
            }

            if (faculty.Id == 0)
            {
                return View("FacultyCreate", faculty);
            }
            return View("FacultyEdit", faculty);

        }
        //DELETE Faculty
        public ActionResult FacultyDelete(int facId)
        {
            var id = db.Faculties.Where(m => m.Id == facId).FirstOrDefault();
            db.Faculties.Remove(id);
            db.SaveChanges();

            return Redirect("Faculty");
        }

    }
}