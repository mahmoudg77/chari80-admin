using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Chair80CP.BLL;
using Chair80CP.Models;
namespace Chair80CP.Controllers
{
    
    public class ImagesController : Controller
    {
        private Models.MainEntities db = new Models.MainEntities();
        // GET: Images
        public ActionResult Index(int id, string model,string tag = "main",string style= "crope",string size="100x100")
        {
            ViewBag.style = style;
            ViewBag.size = size;
            var Images = db.tbl_images.Where(a => a.model_name == model && a.model_id == id && a.model_tag == tag).ToList();
            return PartialView(Images);
        }
        [LoginFilter]
        public ActionResult Delete(int id)
        {

            var img = db.tbl_images.Find(id);
            if (img != null)
            {
               string filePath = ConfigurationManager.AppSettings["mediaServer_Path"] + img.original.Replace("/", "\\");
                try
                {

                System.IO.File.Delete(filePath);
                }
                catch (Exception)
                {

                    
                }
                db.Entry(img).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }

            
                return Json(new { type="success",message="Deleted success"});
             

            
        }
        [LoginFilter]
        public ActionResult Upload(string model, int model_id, string model_tag = "main")
        {

            try
            {
                var httpRequest = Request;
                if(Images.SaveImagesFromRequest(httpRequest, "en", model, model_id, model_tag))
                {
                    return Json(new { type = "success", message = "Upload images success" });
                }

                return Json(new { type = "error", message = "Error while upload images" });

            }
            catch (Exception ex)
            {
                return Json(new { type = "error", message = ex.Message });
            }


        }
    }
}