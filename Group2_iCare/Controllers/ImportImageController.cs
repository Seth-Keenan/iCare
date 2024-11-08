using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;
using Group2_iCare.Models;
using System.Data.Entity.Core.Objects;

namespace Group2_iCare.Controllers
{
    public class ImportImageController : Controller
    {
        private Group2_iCAREDBEntities db = new Group2_iCAREDBEntities();

        // GET: ImportImage
        public ActionResult Index(int PatientRecordID)
        { // index
            ViewBag.PatientRecordID = PatientRecordID; // get the patient record id
            var files = db.Files.Where(f => f.UploadedByID == PatientRecordID.ToString()).ToList(); // get the files to list
            ViewBag.UploadedFiles = files;
            return View(); // return the view
        }
        public ActionResult BrowseFiles(int PatientRecordID)
        { // browse files
            ViewBag.PatientRecordID = PatientRecordID; // get the patient record id
            return View();
        }

        // GET: ImportImage/EditFile
        public ActionResult EditFile(int PatientRecordID)
        {
            ViewBag.PatientRecordID = PatientRecordID; // get the patient record id
            var files = db.Files.Where(f => f.UploadedByID == PatientRecordID.ToString()).ToList(); // list the files
            ViewBag.UploadedFiles = files;
            return View();
        }

        // GET: ImportImage/Scanner
        public ActionResult Scanner(int PatientRecordID)
        { // scanner
            ViewBag.PatientRecordID = PatientRecordID; // get the patient record id
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, int PatientRecordID)
        {
            ViewBag.PatientRecordID = PatientRecordID; // get the patient record id

            if (file != null && file.ContentLength > 0)
            { // if file is not null and is greater than 0
                var fileName = Path.GetFileName(file.FileName); // get the file name
                var fileExtension = Path.GetExtension(fileName).ToLower(); // get the file extension

                if (fileExtension != ".pdf") // if file extension is not pdf
                {
                    ViewBag.Message = "Only PDF files are allowed.";
                    return View("Index");
                }

                var directoryPath = Server.MapPath("~/Repository/UploadedFiles");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var path = Path.Combine(directoryPath, fileName);
                file.SaveAs(path);
                ViewBag.FileName = fileName; // Use fileName instead of path
                ViewBag.Message = "File uploaded successfully";

                byte[] fileData; // byte array for file data
                using (var binaryReader = new BinaryReader(file.InputStream)) // read file
                {
                    fileData = binaryReader.ReadBytes(file.ContentLength);
                }

                // Random ID because we will not be calling it 
                int newId; // new id for file
                Random random = new Random(); // random number
                do
                {
                    newId = random.Next(1, int.MaxValue);
                } while (db.Files.Any(f => f.ID == newId.ToString()));

                var currentUser = db.iCAREWorker.FirstOrDefault(); // get the current user

                db.Files.Add(new Files // add the file
                {
                    ID = newId.ToString(),
                    FileName = fileName,
                    ContentType = file.ContentType,
                    FilePath = $"~/Repository/UploadedFiles/{fileName}",
                    Data = fileData,
                    UploadedByID = PatientRecordID.ToString(),
                    UploadDate = DateTime.Now,
                    Descript = "Uploaded file"
                });

                db.SaveChanges(); // save changes

                return RedirectToAction("ShowUploadedFile", new { fileName, PatientRecordID }); // return the uploaded file
            }
            else
            {
                ViewBag.Message = "No file selected"; // if no file is selected
            }

            return View("Index"); // return the index
        }

        public ActionResult ShowUploadedFile(string fileName, int PatientRecordID)
        {
            ViewBag.PatientRecordID = PatientRecordID; // get the patient record id

            if (string.IsNullOrEmpty(fileName)) // if file name is not valid
            {
                return HttpNotFound("File name is invalid.");
            }

            var directoryPath = Server.MapPath("~/Repository/UploadedFiles"); // get the directory path
            if (directoryPath == null)
            {
                return HttpNotFound("Directory path is invalid.");
            }

            var path = Path.Combine(directoryPath, fileName); // combine the path

            if (System.IO.File.Exists(path))
            {
                ViewBag.FilePath = Url.Content($"~/Repository/UploadedFiles/{fileName}"); // get the file path
                return View();
            }

            return HttpNotFound();
        }

        public ActionResult DeleteFile(string fileName, int PatientRecordID)
        {
            ViewBag.PatientRecordID = PatientRecordID; // get the patient record id

            if (string.IsNullOrEmpty(fileName))
            {
                return HttpNotFound("File name is invalid.");
            }

            var directoryPath = Server.MapPath("~/Repository/UploadedFiles"); // get the directory path
            if (directoryPath == null)
            { // if directory path is invalid
                return HttpNotFound("Directory path is invalid.");
            }

            var path = Path.Combine(directoryPath, fileName); // combine the path

            if (System.IO.File.Exists(path)) // if file exists
            {
                System.IO.File.Delete(path); // delete the file
                ViewBag.Message = "File deleted successfully";

                var file = db.Files.FirstOrDefault(f => f.FileName == fileName); // get the file
                if (file != null)
                {
                    db.Files.Remove(file);
                    db.SaveChanges();
                }

                return RedirectToAction("Index", new { PatientRecordID }); // return to index
            }

            return HttpNotFound();
        }
    }
}