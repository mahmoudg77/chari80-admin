using Chair80CP.Models;
using Chair80CP.Libs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Chair80CP.BLL
{
    public class Images
    {

        public static string resize(string img, float BOXHEIGHT, float BOXWIDTH,string path)
        {

            Image image = Image.FromFile(img);
            
            float scaleHeight = (float)BOXHEIGHT / (float)image.Height;
            float scaleWidth = (float)BOXWIDTH / (float)image.Width;

            float scale = Math.Min(scaleHeight, scaleWidth);

            Image thumb = image.GetThumbnailImage((int)(image.Width * scale), (int)(image.Height * scale), () => false, IntPtr.Zero);

            System.IO.FileInfo f = new System.IO.FileInfo(img);

       
             

            thumb.Save(f.Directory.Parent.FullName + @"\" + path + @"\" + f.Name, image.RawFormat);

            thumb.Dispose();

            return f.Directory.Parent.FullName + @"\" + path + @"\" + f.Name;
        }
        public  static Image  resize(Image image, float BOXHEIGHT, float BOXWIDTH)
        {

 
            float scaleHeight = (float)BOXHEIGHT / (float)image.Height;
            float scaleWidth = (float)BOXWIDTH / (float)image.Width;

            float scale = Math.Min(scaleHeight, scaleWidth);

            Image thumb = image.GetThumbnailImage((int)(image.Width * scale), (int)(image.Height * scale), () => false, IntPtr.Zero);

            return thumb;
        }

        public static bool SaveImagesFromRequest(HttpRequestBase httpRequest, string lang, string model, int model_id, string model_tag = "main")
        {
 
            int success_count = 0;
            foreach (string file in httpRequest.Files)
            {
                var postedFile = httpRequest.Files[file];

                SaveImageFromFile(postedFile, model, model_id, model_tag);
                
            }
            return true;
        }


        public static bool SaveImageFromFile(HttpPostedFileBase postedFile,string model,int model_id,string model_tag = "main")
        {
            if (postedFile != null && postedFile.ContentLength > 0)
            {

                int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                List<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".jpeg", ".bmp" };
                var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                var extension = ext.ToLower();
                if (!AllowedFileExtensions.Contains(extension))
                {

                    var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                     return false;
                }
                else if (postedFile.ContentLength > MaxContentLength)
                {

                    var message = string.Format("Please Upload a file upto 1 mb.");

                    return false;
                }
                else
                {
                    Random random = new Random();
                    var code = random.Next(100000, 999999);

                    string fname = DateTime.Now.ToString("yyyyMMddHHmmss-")+ code  + postedFile.FileName.Replace(" ","-");
                    string online_original = "/Storage/Original/" + fname;
                    string online_thumb = online_original;// "/Storage/Thumb/" + fname;
                    string online_medium = online_original;// "/Storage/Medium/" + fname;
                    string online_large = online_original;// "/Storage/Large/" + fname;

                    string original =ConfigurationManager.AppSettings["mediaServer_Path"] + online_original.Replace("/","\\");// HttpContext.Current.Server.MapPath("~" + online_original);


                    postedFile.SaveAs(original);
                    postedFile = null;
                    //string thumb = BLL.Images.resize(original, 120, 120, "Thumb");
                    //string medium = BLL.Images.resize(original, 400, 400, "Medium");
                    //string large = BLL.Images.resize(original, 800, 800, "Large");


                    using (MainEntities ctx = new MainEntities())
                    {
                        tbl_images img = new tbl_images();
                        img.large = online_large;
                        img.meduim = online_medium;
                        img.original = online_original;
                        img.thumb = online_thumb;
                        img.model_name = model;
                        img.model_id = model_id;
                        img.model_tag = model_tag;

                        ctx.tbl_images.Add(img);
                        ctx.SaveChanges();

                        return true;
                    }


                }
            }
            return false;
        }

        /// <summary>
        /// Resize an image keeping its aspect ratio (cropping may occur).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image ResizeImageKeepAspectRatio(Image source, int width, int height)
        {
            Image result = null;

            try
            {
                if (source.Width != width || source.Height != height)
                {
                    // Resize image
                    float sourceRatio = (float)source.Width / source.Height;

                    using (var target = new Bitmap(width, height))
                    {
                        using (var g = System.Drawing.Graphics.FromImage(target))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            // Scaling
                            float scaling;
                            float scalingY = (float)source.Height / height;
                            float scalingX = (float)source.Width / width;
                            if (scalingX < scalingY) scaling = scalingX; else scaling = scalingY;

                            int newWidth = (int)(source.Width / scaling);
                            int newHeight = (int)(source.Height / scaling);

                            // Correct float to int rounding
                            if (newWidth < width) newWidth = width;
                            if (newHeight < height) newHeight = height;

                            // See if image needs to be cropped
                            int shiftX = 0;
                            int shiftY = 0;

                            if (newWidth > width)
                            {
                                shiftX = (newWidth - width) / 2;
                            }

                            if (newHeight > height)
                            {
                                shiftY = (newHeight - height) / 2;
                            }

                            // Draw image
                            g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
                        }

                        result = (Image)target.Clone();
                    }
                }
                else
                {
                    // Image size matched the given size
                    result = (Image)source.Clone();
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }
    }
}