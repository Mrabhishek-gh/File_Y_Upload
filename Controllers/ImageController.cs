using ImagePreviewUpload.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImagePreviewUpload.Controllers
{
    public class ImageController : Controller
    {
        private static string ConnString = ConfigurationManager.ConnectionStrings["PatientDBconn"].ConnectionString;


        public ActionResult Upload()
        {
            return View();
        }

        public ActionResult UploadMultiple()
        {
            return View();
        }

        public ActionResult ImgFileSave(HttpPostedFileBase image)
        {
            try
            {
                if(image != null)
                {
                    string filename = image.FileName;
                    var ext = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();

                    if(ext == "jpeg" || ext == "jpg" || ext == "png")
                    {
                        string path = Server.MapPath("~/ImageUploads/" + filename);
                        image.SaveAs(path);
                        ViewBag.message = "Image uploaded successfully";
                    }
                    else
                    {
                        ViewBag.message = "Image format is not valid";

                    }

                }
                else 
                {
                    ViewBag.message = "Please select an image";
                }
            }
            catch(Exception exc) 
            {
                ViewBag.message = exc.Message;
            }

            return View("Upload");

        }

        [HttpPost]

        public ActionResult MultipleImageFileSave()
        {
           if(Request.Files.Count > 0)
           {
                try
                {
                    HttpFileCollectionBase files = Request.Files;

                    for(int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        fname = file.FileName;

                        fname = Path.Combine(Server.MapPath("~/ImageUploads/"), fname);

                        file.SaveAs(fname);


                    }
                    return Json("File uploaded Successfully ! ");

                }
                catch (Exception exc)
                {
                    return Json("Error occured" + exc.Message);
                }
           }
            else
            {
                return Json("No Files Selected");
            }


        }

        public ActionResult MediaPage()
        {
            return View();
        }


        [HttpPost]
        public ActionResult MediaPage(ImgViewModel imgViewModel)
        {
            ImgObj imgObj = new ImgObj();

            if (imgViewModel.FileAttach!= null)
            {
                imgObj.FileName = Path.GetFileName(imgViewModel.FileAttach.FileName);
                imgObj.FilePath = Server.MapPath("~/ImageUploads/" + imgObj.FileName);
                imgObj.UserName = imgViewModel.UserName;
                imgViewModel.FileAttach.SaveAs(imgObj.FilePath);

            }
         

            SqlConnection conn = new SqlConnection(ConnString);

            SqlCommand sqlCommand = new SqlCommand("sp_insert_file", conn);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@user_name", imgObj.FileName),
                new SqlParameter("@file_name",imgObj.FileName),
                new SqlParameter("@file_path",imgObj.FilePath)
            };
            
            sqlCommand.Parameters.AddRange(sp);

            SqlParameter sqlParameter = new SqlParameter()
            {
                ParameterName = "message",
                Size = int.MaxValue,
                Direction = System.Data.ParameterDirection.Output

            };

            sqlCommand.Parameters.Add(sqlParameter);

            conn.Open();
            sqlCommand.ExecuteNonQuery();
            ViewBag.message = sqlParameter.Value.ToString();
            conn.Close();

            return View();
            
        }

        public ActionResult ShowImages()
        {
            SqlConnection conn = new SqlConnection(ConnString);

            SortedList <int,string> final = new SortedList<int,string>(); 

            SqlCommand sqlCommand = new SqlCommand("sp_get_all_files", conn);
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

            conn.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

            if(sqlDataReader.HasRows)
            {
                while(sqlDataReader.Read())
                {
                    final.Add(Convert.ToInt32(sqlDataReader["FileID"]), sqlDataReader["FileName"].ToString());
                }
            }
            conn.Close();
            return View(final);
        }

   

        
    }
}