using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Windows.Forms;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public class ImageHandler

    {

        public static Image GetImageFromFile(string strFilename)
        {
            try
            {
                return  (Bitmap)Image.FromFile(strFilename);
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Get image function failed", exc);
                return null;
            }
        }

        public static void ShowImageFromFile(PictureBox pic, string strFilename)
        {
            try
            {
                if (strFilename.Trim() == String.Empty)
                    return;

                Bitmap OldBmp = (Bitmap)Image.FromFile(strFilename);
                Bitmap NewBmp = new Bitmap(pic.Width, pic.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics graphics = Graphics.FromImage(NewBmp);

                //this is where you set up InterpolationMode property for higher quality resizing.
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                graphics.DrawImage(OldBmp, 0, 0, pic.Width, pic.Height);

                pic.BackgroundImage = NewBmp;
                pic.Refresh();
            }
            catch (Exception exc)
            {
                pic.BackgroundImage = null;
                FormHandler.DispalyErrorMessage("Show picture function failed", exc);
            }
        }

        public static Image ResizeImage(Image img,int width, int height)
        {
            try
            {
                if (img == null)
                    return null;

                Bitmap OldBmp = (Bitmap)img;
                Bitmap NewBmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics graphics = Graphics.FromImage(NewBmp);

                //this is where you set up InterpolationMode property for higher quality resizing.
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                graphics.DrawImage(OldBmp, 0, 0, width, height);

                return NewBmp;
            }
            catch (Exception exc)
            {
                FormHandler.DispalyErrorMessage("Show picture function failed", exc);
                return null;
            }
        }

        

        public static byte[] GetBytesFromFile(string strFilename)
        {
            FileStream fsFile = new FileStream(strFilename, FileMode.Open, FileAccess.Read);
            byte[] bytData = new byte[fsFile.Length - 1];
            fsFile.Read(bytData, 0, bytData.Length);
            fsFile.Close();

            return bytData;
        }

        public static byte[] GetBytesFromImage(Image img)
        {
          
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            if (img != null)
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            return ms.ToArray();
        }
    }
}
