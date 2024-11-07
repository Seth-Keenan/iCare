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

namespace Group2_iCare.Controllers
{
    public class ImportImageController : Controller
    {
        // GET: ImportImage
        public ActionResult Index(int PatientRecordID)
        {
            ViewBag.PatientRecordID = PatientRecordID;
            return View();
        }
        public ActionResult BrowseFiles(int PatientRecordID)
        {
            ViewBag.PatientRecordID = PatientRecordID;
            return View();
        }

        // GET: ImportImage/EditFile
        public ActionResult EditFile()
        {
            return View();
        }

        // GET: ImportImage/Scanner
        public ActionResult Scanner()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, int PatientRecordID)
        {
            ViewBag.PatientRecordID = PatientRecordID;

            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var fileExtension = Path.GetExtension(fileName).ToLower();

                if (fileExtension != ".pdf")
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

                // Redirect to ShowUploadedFile with fileName and PatientRecordID
                return RedirectToAction("ShowUploadedFile", new { fileName, PatientRecordID });
            }
            else
            {
                ViewBag.Message = "No file selected";
            }

            return View("Index");
        }

        public ActionResult ShowUploadedFile(string fileName, int PatientRecordID)
        {
            ViewBag.PatientRecordID = PatientRecordID;

            if (string.IsNullOrEmpty(fileName))
            {
                return HttpNotFound("File name is invalid.");
            }

            var directoryPath = Server.MapPath("~/Repository/UploadedFiles");
            if (directoryPath == null)
            {
                return HttpNotFound("Directory path is invalid.");
            }

            var path = Path.Combine(directoryPath, fileName);

            if (System.IO.File.Exists(path))
            {
                ViewBag.FilePath = Url.Content($"~/Repository/UploadedFiles/{fileName}");
                return View();
            }

            return HttpNotFound();
        }

    }
}