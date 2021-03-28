using EnterpriseWebProject.DAL;
using EnterpriseWebProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;
using EnterpriseWebProject.ViewModel;
using Newtonsoft.Json;
using PagedList;

namespace EnterpriseWebProject.Controllers
{
    [Authorize(Roles = "Manager")]
    public class ManagerController : Controller
    {
        EWContextDb db = new EWContextDb();

        public List<SelectListItem> AcaYearList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var year = db.AcademicYears.ToList();
            foreach (var item in year)
            {
                list.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.YearNo.ToString()});
            }
            return list;
        }
        //View Dashboard
        public ActionResult Dashboard()
        {
            //Pie chart
            var getFaculty = db.Faculties.ToList();
            var totalContri = db.Contributions.Count();
            List<int> contriNubmers = new List<int>();
            List<string> FacultyName1 = new List<string>();
            foreach (var fac in getFaculty)
            {
                int x;
                FacultyName1.Add(fac.Name);
                var getContriInFac = db.Contributions.Where(m => m.FacultyId == fac.Id).Count();
                //x = (getContriInFac / totalContri) * 100;
                x = (getContriInFac * 100) / totalContri;
                contriNubmers.Add(x);
            }
            ViewBag.Fac = JsonConvert.SerializeObject(FacultyName1);
            ViewBag.Contri = JsonConvert.SerializeObject(contriNubmers);

            //Pass Year data to drop down list
            var getYear = db.AcademicYears.ToList();
            List<SelectListItem> getYearNo = new List<SelectListItem>();
            foreach (var item in getYear)
            {
                getYearNo.Add(new SelectListItem { Value = item.YearNo.ToString(), Text = item.YearNo.ToString() });
            }
            ViewBag.getYear = getYearNo;

            //Get Faculty by Year
            int yearSelected = Convert.ToInt32(getYearNo.ElementAt(0).Value);
            var getMagazinebyYear = db.Magazines.Where(x => x.AcademicYear.YearNo == yearSelected).ToList();
            //Create FacultyName list and Count variable
            int count = 0;
            List<string> FacultyName = new List<string>();
            List<int> FacultyId = new List<int>();
            //Get all faculties contain in all magazine
            foreach (var item in getMagazinebyYear)
            {
                //Get Faculties in "item" Magazine
                var getMagaFac = db.Magazines_Faculties.Where(a => a.MagazineId == item.Id).ToList();
                //Pass data (Faculty Name) to list FacultyName 
                foreach (var faculty in getMagaFac)
                {
                    //If list FacultyName has not already contains the name of faculty => add to Faculty
                    if (FacultyName.Contains(faculty.Faculty.Name) == false)
                    {
                        FacultyName.Add(faculty.Faculty.Name);
                        FacultyId.Add(faculty.Faculty.Id);
                    }
                }
            }

            //Get all contribution by faculty in one year
            List<List<int>> ContributionbyFaculty = new List<List<int>>();
            //Get each faculty
            foreach (var item in FacultyId)
            {
                List<int> ContributionbyMonth = new List<int>();
                //Get contribution in each magazine (month) in "item" faculty
                foreach (var maga in getMagazinebyYear)
                {
                    //number of contribution in "maga" magazine (month) of "item" faculty
                    var getContri = db.Contributions.Where(m => m.MagazineId == maga.Id && m.FacultyId == item).ToList();
                    ContributionbyMonth.Add(getContri.Count);
                }
                ContributionbyFaculty.Add(ContributionbyMonth);
            }

            ViewBag.labels = JsonConvert.SerializeObject(FacultyName);
            ViewBag.lines = JsonConvert.SerializeObject(ContributionbyFaculty);


            return View();
        }

        [HttpPost]
        public ActionResult Dashboard(DashboardViewModel dashBoard)
        {
            //Pie chart
            var getFaculty = db.Faculties.ToList();
            var totalContri = db.Contributions.Count();
            List<int> contriNubmers = new List<int>();
            List<string> FacultyName1 = new List<string>();
            foreach (var fac in getFaculty)
            {
                int x;
                FacultyName1.Add(fac.Name);
                var getContriInFac = db.Contributions.Where(m => m.FacultyId == fac.Id).Count();
                //x = (getContriInFac / totalContri) * 100;
                x = (getContriInFac * 100) / totalContri;
                contriNubmers.Add(x);
            }
            ViewBag.Fac = JsonConvert.SerializeObject(FacultyName1);
            ViewBag.Contri = JsonConvert.SerializeObject(contriNubmers);

            //Pass Year data to drop down list
            var getYear = db.AcademicYears.ToList();
            List<SelectListItem> getYearNo = new List<SelectListItem>();
            foreach (var item in getYear)
            {
                getYearNo.Add(new SelectListItem { Value = item.YearNo.ToString(), Text = item.YearNo.ToString() });
            }
            ViewBag.getYear = getYearNo;

            //Get Faculty by Year
            int yearSelected = dashBoard.YearNo;
            var getMagazinebyYear = db.Magazines.Where(x => x.AcademicYear.YearNo == yearSelected).ToList();
            //Create FacultyName list and Count variable
            int count = 0;
            List<string> FacultyName = new List<string>();
            List<int> FacultyId = new List<int>();
            //Get all faculties contain in all magazine
            foreach (var item in getMagazinebyYear)
            {
                //Get Faculties in "item" Magazine
                var getMagaFac = db.Magazines_Faculties.Where(a => a.MagazineId == item.Id).ToList();
                //Pass data (Faculty Name) to list FacultyName 
                foreach (var faculty in getMagaFac)
                {
                    //If list FacultyName has not already contains the name of faculty => add to Faculty
                    if (FacultyName.Contains(faculty.Faculty.Name) == false)
                    {
                        FacultyName.Add(faculty.Faculty.Name);
                        FacultyId.Add(faculty.Faculty.Id);
                    }
                }
            }

            //Get all contribution by faculty in one year
            List<List<int>> ContributionbyFaculty = new List<List<int>>();
            //Get each faculty
            foreach (var item in FacultyId)
            {
                List<int> ContributionbyMonth = new List<int>();
                //Get contribution in each magazine (month) in "item" faculty
                foreach (var maga in getMagazinebyYear)
                {
                    //number of contribution in "maga" magazine (month) of "item" faculty
                    var getContri = db.Contributions.Where(m => m.MagazineId == maga.Id && m.FacultyId == item).ToList();
                    ContributionbyMonth.Add(getContri.Count);
                }
                ContributionbyFaculty.Add(ContributionbyMonth);
            }

            ViewBag.labels = JsonConvert.SerializeObject(FacultyName);
            ViewBag.lines = JsonConvert.SerializeObject(ContributionbyFaculty);
            return View();
        }

        //MANAGE: Magazine
        public ActionResult Magazine(int? page)
        {
            List<string> list = new List<string>();
            var maga_facList = db.Magazines_Faculties.ToList();
            foreach (var item in maga_facList)
            {
                list.Add(item.MagazineId.ToString());
            }
            ViewBag.list = list;

            IPagedList<Magazine> magazines = db.Magazines.ToList().ToPagedList(page ?? 1, 10);
            return View(magazines);
        }
        //GET: Create Magazine
        public ActionResult MagazineCreate()
        {
            ViewBag.list = AcaYearList();

            return View();
        }
        //GET: Edit Magazine
        public ActionResult MagazineEdit(int magazineId)
        {
            var id = db.Magazines.Find(magazineId);
            ViewBag.list = AcaYearList();
            return View(id);
        }
        //POST: Create Magazine
        [HttpPost]
        public ActionResult MagazineCreateOrEdit(Magazine magazine, HttpPostedFileBase PDFfile, HttpPostedFileBase ImgFile)
        {
            ViewBag.list = AcaYearList();
            if (ModelState.IsValid)
            {             
                Magazine obj = new Magazine();

                obj.Id = magazine.Id;
                obj.Name = magazine.Name;
                obj.OpenDate = magazine.OpenDate;
                obj.CloseDate = magazine.CloseDate;
                obj.AcademicYearID = magazine.AcademicYearID;
                obj.Description = magazine.Description;

                if (magazine.Id == 0)
                {
                    obj.Status = true;
                    db.Magazines.Add(obj);
                    db.SaveChanges();
                }
                else
                {
                    //upload file
                    if (PDFfile != null)
                    {
                        string pdf = Path.GetFileName(PDFfile.FileName);
                        string pdfPath = Path.Combine(Server.MapPath("~/Files/Doc/"), pdf);
                        PDFfile.SaveAs(pdfPath);

                        obj.PDFfile = pdf;
                    }
                    if (ImgFile != null)
                    {
                        string img = Path.GetFileName(ImgFile.FileName);
                        string imgPath = Path.Combine(Server.MapPath("~/Files/Image/"), img);
                        ImgFile.SaveAs(imgPath);

                        obj.ImgFile = img;
                    }
                    obj.Status = magazine.Status;
                    db.Entry(obj).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return Redirect("Magazine");
            }

            if (magazine.Id == 0)
            {
                return View("MagazineCreate", magazine);
            }
            return View("MagazineEdit", magazine);
           
        }
        //Delete Magazine
        public ActionResult MagazineDelete(int magazineId)
        {           

            var id = db.Magazines.Where(m => m.Id == magazineId).FirstOrDefault();
            db.Magazines.Remove(id);
            db.SaveChanges();

            return Redirect("Magazine");
        }

        //MANAGE: Magazine_Faculty
        //GET: Assign Faculty to Magazine
        public ActionResult FacToMagazine(int magazineId)
        {
            //get facultyId in Faculty_Magazine db and add to list
            var id = db.Magazines_Faculties.Where(m => m.MagazineId == magazineId).ToList();
            List<string> arr = new List<string>();
            foreach (var item in id)
            {
                arr.Add(item.FacultyId.ToString());
            }
            ViewBag.falId = arr;

            ////get Id of Faculty_Magazine
            //var conId = db.Contributions.ToList();
            //List<string> yes = new List<string>();
            //foreach (var item in conId)
            //{
            //    yes.Add(item.Magazine_Faculty.FacultyId.ToString());
            //}
            //ViewBag.conId = yes;


            List<SelectListItem> list = new List<SelectListItem>();
            var fac = db.Faculties.ToList();
            foreach (var item in fac)
            {
                list.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.Name.ToString() });
            }
            ViewBag.Fac = list;
            ViewBag.Id = magazineId;

            return View();
        }
        //POST: Assign Faculty to Magazine
        [HttpPost]
        public ActionResult FacToMagazine(int magazineId, int[] facultyID)
        {
            //Magazine_Faculty obj = new Magazine_Faculty();
            //List<SelectListItem> list = new List<SelectListItem>();
            //var fac = db.Faculties.ToList();
            //foreach (var item in fac)
            //{

            //}
            foreach (int item in facultyID)
            {
                Magazine_Faculty obj = new Magazine_Faculty();
                obj.MagazineId = magazineId;
                obj.FacultyId = item;

                db.Magazines_Faculties.Add(obj);
                db.SaveChanges();
            }

            return Redirect("Magazine");
        }

        //GET: ViewAssignFacultyToMagazine
        public ActionResult ViewFacInMagazine(int magazineId)
        {
            var gido = db.Magazines_Faculties.Where(x => x.MagazineId == magazineId).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in gido)
            {
                list.Add(new SelectListItem { Value = item.FacultyId.ToString(), Text = item.Faculty.Name });
            }
            ViewBag.list = list;

            //get Id of Faculty_Magazine
            var conId = db.Contributions.Where(x => x.Magazine_Faculty.MagazineId == magazineId).ToList();
            List<string> contri = new List<string>();
            foreach (var item in conId)
            {
                contri.Add(item.Magazine_Faculty.FacultyId.ToString());
            }
            ViewBag.conId = contri;

            var id = db.Magazines.Find(magazineId);           
            return View(id);
        }
        
        //POST: Delete
        public ActionResult DeleteFacInMag(int magazineId, int[] facultyId)
        {
            foreach (var item in facultyId)
            {
                var chodach = db.Magazines_Faculties.Where(x => x.MagazineId == magazineId && x.FacultyId == item).FirstOrDefault();
                db.Magazines_Faculties.Remove(chodach);
                db.SaveChanges();
            }
            return Redirect("Magazine");
        }

        //GET: ContributionPassed
        public ActionResult ContributionPassed(int? page)
        {
            IPagedList<EnterpriseWebProject.Models.File> list = db.Files.Where(m => m.Contribution.Status == true).ToList().ToPagedList(page ?? 1, 10);

            return View(list);
        }

        ////GET:
        //public ActionResult ContributionApproved(int magazineId)
        //{
        //    var fileInContri = db.Files.Where(m => m.Contribution.MagazineId == magazineId && m.Contribution.Status == true).ToList();
        //    ViewBag.Id = magazineId;
            
        //    return View(fileInContri);
        //}

        public FileResult DownloadFile(List<string> download)
        {
            //Models.File file = new Models.File();            
            //var file = db.Files.Where(m => m.Contribution.MagazineId == magazineId).ToList();
            //foreach (var item in file)
            //{
            //    var fileName1 = Path.GetFileName(item.FileType);
            //}

            List<Models.File> file = new List<Models.File>();

            foreach (var item in download)
            {
                var fileId = db.Files.Where(m => m.Id.ToString() == item).FirstOrDefault();
                file.Add(fileId);
            }

            var path1 = Server.MapPath("~/Files/Doc/");
            var path2 = Server.MapPath("~/Files/Image/");

            List<string> fileList = new List<string>();

            foreach (var item in file)
            {
                if (System.IO.File.Exists(Server.MapPath("~/Files/zipfiles/" + item.Name+".zip")))
                {
                    System.IO.File.Delete(Server.MapPath("~/Files/zipfiles/" + item.Name+".zip"));
                }

                ZipArchive zipDocImg = ZipFile.Open(Server.MapPath("~/Files/zipfiles/" + item.Name+".zip"), ZipArchiveMode.Create);

                zipDocImg.CreateEntryFromFile(path1 + item.FileType, item.FileType);
                zipDocImg.CreateEntryFromFile(path2 + item.FileName, item.FileName);

                zipDocImg.Dispose();

                fileList.Add(item.Name+".zip");

            }
            

            var zipPath = Server.MapPath("~/Files/zipfiles/");

            if (System.IO.File.Exists(Server.MapPath("~/Files/zipfiles/bundle.zip")))
            {
                System.IO.File.Delete(Server.MapPath("~/Files/zipfiles/bundle.zip"));
            }

            ZipArchive zip = ZipFile.Open(Server.MapPath("~/Files/zipfiles/bundle.zip"), ZipArchiveMode.Create);

            foreach (var item in fileList)
            {
                zip.CreateEntryFromFile(zipPath + item, item);
            }
            zip.Dispose();
            return File("~/Files/zipfiles/bundle.zip", "application/zip","Contribution.zip");
        }
    }
}