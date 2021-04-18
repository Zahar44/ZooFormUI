using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZooFormUI.UserControls;

namespace ZooFormUI
{
    public static class Helper
    {
        public static void BringToFrontOrCreate(this UCMain instanse)
        {
            if (!MainMenu.Panel.Contains(UCMain.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCMain.Instanse);
                UCMain.Instanse.Dock = DockStyle.Fill;
            }
            UCMain.Instanse.BringToFront();
        }
        public static void BringToFrontOrCreate(this UCDBManager instanse)
        {
            MainMenu.Instanse.SetSizeSafe(new Size(300, 400));
            if (!MainMenu.Panel.Contains(UCDBManager.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCDBManager.Instanse);
                UCDBManager.Instanse.Dock = DockStyle.Fill;
            }
            UCDBManager.Instanse.BringToFront();
        }
        public static void BringToFrontOrCreate(this UCAbout instanse)
        {
            MainMenu.Instanse.SetSizeSafe(new Size(300, 400));
            if (!MainMenu.Panel.Contains(UCAbout.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCAbout.Instanse);
                UCAbout.Instanse.Dock = DockStyle.Fill;
            }
            UCAbout.Instanse.BringToFront();
        }
        public static async Task BringToFrontOrCreateAsync(this UCFind instanse) 
        {
            if (!MainMenu.Panel.Contains(UCFind.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCFind.Instanse);
                UCFind.Instanse.Dock = DockStyle.Fill;
            }
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() => MainMenu.Instanse.SetSizeSafe(UCFind.Instanse.SizeOfPage)));
            tasks.Add(Task.Run(() => UCFind.Instanse.ShowDataAsync()));
            UCFind.Instanse.BringToFront();
            
            await Task.WhenAll(tasks);
        }
        public static void BringToFrontOrCreate(this UCAdd instanse, UserControl sender)
        {
            if (!MainMenu.Panel.Contains(UCAdd.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCAdd.Instanse);
                UCAdd.Instanse.Dock = DockStyle.Fill;
            }
            if (!UCAdd.Panel.Contains(sender))
            {
                UCAdd.Panel.Controls.Add(sender);
                sender.Dock = DockStyle.Fill;
            }
            sender.BringToFront();
            UCAdd.Instanse.LastAddedControl = (UCAddBase)sender;
            UCAdd.Instanse.BringToFront();
        }
        public static void BringToFrontOrCreate(this UCEdit instanse, UserControl sender, object entity)
        {
            if (entity == null)
                return;
            if (!MainMenu.Panel.Contains(UCEdit.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCEdit.Instanse);
                UCEdit.Instanse.Dock = DockStyle.Fill;
            }
            if (!UCEdit.Panel.Contains(sender))
            {
                UCEdit.Panel.Controls.Add(sender);
                sender.Dock = DockStyle.Fill;
            }
            sender.BringToFront();
            UCEdit.Instanse.LastAddedControl = (UCAddBase)sender;
            UCEdit.Instanse.Entity = entity;
            MainMenu.Instanse.SetSizeSafe(new Size(300, 400));
            UCEdit.Instanse.BringToFront();
        }
        public static Bitmap ResizeImage(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
