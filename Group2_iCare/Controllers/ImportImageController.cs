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

namespace Group2_iCare.Controllers
{
    public class ImportImageController : Controller
    {
        // GET: ImportImage
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
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
                ViewBag.FileName = path;
                ViewBag.Message = "File uploaded successfully";
            }
            else
            {
                ViewBag.Message = "No file selected";
            }

            return View("ShowUploadedFile");
        }

        public ActionResult ShowUploadedFile(string fileName)
        {
            var directoryPath = Server.MapPath("~/Repository/UploadedFiles");
            var path = Path.Combine(directoryPath, fileName);

            if (System.IO.File.Exists(path))
            {
                var imageBytes = System.IO.File.ReadAllBytes(path);
                return File(imageBytes, "application/pdf");
            }

            return HttpNotFound();
        }
    }
}