using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVSubTask.Services
{
    public class FileService
    {
        public bool isFileExist(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool isFileSupportedFormat(HttpPostedFileBase file)
        {
            if (file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                || file.ContentType == "application/pdf")
            {
                return true;
            }


            else
            {
                return false;
            }
        }

        public bool isFileSupportedSize(HttpPostedFileBase file)
        {
            if (file.ContentLength * 1048576 > 1)
            {
                return false ;
            }


            else
            {
                return true;
            }
        }


        public bool isImageSupported(HttpPostedFileBase file)
        {
            System.Drawing.Image sourceimage =
            System.Drawing.Image.FromStream(file.InputStream);

            var x = sourceimage.Width;
            var y = sourceimage.Height;

            if (x <= 256 && y <= 256)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}