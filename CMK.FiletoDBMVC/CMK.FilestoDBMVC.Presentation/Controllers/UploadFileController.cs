using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using CMK.FilestoDBMVC.DataAccess.Models_Generated;
using CMK.FilestoDBMVC.Presentation.Models;

namespace CMK.FilestoDBMVC.Presentation.Controllers
{
    public class UploadFileController : Controller
    {

        public ActionResult Success()
        {
            return View();
        }

        // GET: UploadFile
        public ActionResult Index()
        {
            //IEnumerable<FilestoDb> listofFiles;
            //using (CustomersEntities db = new CustomersEntities())
            //{
            //    listofFiles = db.FilestoDbs.ToList();
            //}
            return View();
        }


        public ActionResult ProcessRequest(int id)
        {
            int Id = Convert.ToInt32(id.ToString());
            using (CustomersEntities db = new CustomersEntities())
            {
                var v = db.FilestoDbs.Where(x => x.Id.Equals(Id)).FirstOrDefault();
                if (v != null)
                {
                    Response.ContentType = v.ContentType;
                    Response.AddHeader("content-disposition", "attachment; filename=" + v.FileName);
                    Response.BinaryWrite(v.FileContent);
                    Response.Flush();
                    Response.End();
                }
            }

            //IEnumerable<FilestoDb> listofFiles;
            //using (CustomersEntities db = new CustomersEntities())
            //{
            //    listofFiles = db.FilestoDbs.ToList();
            //}
            return View("Index");
        }


        [HttpPost]
        public ActionResult Index(CertificateObject certificateObject)
        {

            foreach (var certificateFile in certificateObject.myfile)
            {
                if (certificateFile == null)
                {
                    ViewBag.Error = "File cannot be empty.";
                    return View();
                }
                else if (!IsValidContentType(certificateFile.ContentType))
                {
                    ViewBag.Error = "only pdf files allowed.";
                    return View();
                }
                else if (!IsValidFileSize(certificateFile.ContentLength))
                {
                    ViewBag.Error = "File too large";
                    return View();
                }
            }


            string result = AddFiletoDB(certificateObject);

            if (result != "success")
            {
                ViewBag.Error = result;
                return View();
            }

            ViewBag.Message = "Successful";

            //if (certificateObject == null)
            //{
            //    ViewBag.Error = "File cannot be empty.";
            //    //return View(listofFiles);
            //}
            //else if (!IsValidContentType(certificateObject.myfile.ContentType))
            //{
            //    ViewBag.Error = "only pdf files allowed.";
            //    //return View(listofFiles);
            //}
            //else if (!IsValidFileSize(certificateObject.myfile.ContentLength))
            //{
            //    ViewBag.Error = "File too large";
            //    //return View(listofFiles);
            //}
            //else
            //{
            //    ViewBag.Message = "Successful";

            //    string result = AddFiletoDB(certificateObject.myfile);

            //    if (result != "success")
            //    {
            //        ViewBag.Error = result;
            //    }

            //}

            return View();
        }


        [NonAction]
        private bool IsValidContentType(string contentType)
        {
            return contentType.Equals("application/pdf");
        }

        [NonAction]
        private string AddFiletoDB(CertificateObject fileList)
        {
            #region Old
            /*
            HttpFileCollection filesColl = Request.Files;
            using (CustomersEntities db = new CustomersEntities())
            {
                foreach (string uploader in filesColl)
                {
                    HttpPostedFile file = filesColl[uploader];

                    if (file.ContentLength > 0)
                    {
                        BinaryReader br = new BinaryReader(file.InputStream);

                        byte[] buffer = br.ReadBytes(file.ContentLength);

                        db.tblFilestoDbs.Add(new tblFilestoDb
                        {
                            FileName = file.FileName,
                            ContentType = file.ContentType,
                            FileExtension = Path.GetExtension(file.FileName),
                            FileSize = file.ContentLength,
                            FileContent = buffer
                        });
                    }
                }
                db.SaveChanges();
            }
            */
            #endregion

            List<FilestoDb> filestoAddtoDb = new List<FilestoDb>();

            string response = string.Empty;
            try
            {
                using (CustomersEntities db = new CustomersEntities())
                {
                    foreach (var fileObject in fileList.myfile)
                    {
                        if (fileObject.ContentLength > 0)
                        {
                            BinaryReader br = new BinaryReader(fileObject.InputStream);

                            byte[] buffer = br.ReadBytes(fileObject.ContentLength);

                            filestoAddtoDb.Add(new FilestoDb()
                            {
                                FileName = fileObject.FileName,
                                ContentType = fileObject.ContentType,
                                FileExtension = Path.GetExtension(fileObject.FileName),
                                FileSize = fileObject.ContentLength,
                                FileContent = buffer
                            });
                        }
                    }
                    db.FilestoDbs.AddRange(filestoAddtoDb);
                    db.SaveChanges();
                    response = "success";

                    return response;

                }
            }
            catch (Exception ex)
            {
                response = ex.Message;
                return response;
            }
        }

        [NonAction]
        private void PopulateResultsView()
        {

        }

        [NonAction]
        private bool IsValidFileSize(int contentLength)
        {
            return ((contentLength / 1024) / 1024) < 1;
        }
    }
}