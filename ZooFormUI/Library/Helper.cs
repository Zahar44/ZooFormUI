using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            if (!MainMenu.Panel.Contains(UCDBManager.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCDBManager.Instanse);
                UCDBManager.Instanse.Dock = DockStyle.Fill;
            }
            UCDBManager.Instanse.BringToFront();
        }
        public static async Task BringToFrontOrCreateAsync(this UCFind instanse) 
        {
            if (!MainMenu.Panel.Contains(UCFind.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCFind.Instanse);
                UCFind.Instanse.Dock = DockStyle.Fill;
            }
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() => MainMenu.Instanse.SetSizeSafe(new Size(500, 400))));
            tasks.Add(Task.Run(() => UCFind.Instanse.ShowDataAsync("")));
            UCFind.Instanse.BringToFront();
            
            await Task.WhenAll(tasks);
        }
        public static async Task BringToFrontOrCreateAsync(this UCAdd instanse, object sender)
        {
            await UCAdd.Instanse.Set(sender.ToString());
            if (!MainMenu.Panel.Contains(UCAdd.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCAdd.Instanse);
                UCAdd.Instanse.Dock = DockStyle.Fill;
            }
            UCAdd.Instanse.BringToFront();
        }
        public static async Task BringToFrontOrCreateAsync(this UCEdit instanse, object sender, object entity)
        {
            if (entity == null)
                return;
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() => MainMenu.Instanse.SetSizeSafe(new Size(300, 400))));
            tasks.Add(UCEdit.Instanse.Set(sender.ToString(), entity));

            if (!MainMenu.Panel.Contains(UCEdit.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCEdit.Instanse);
                UCEdit.Instanse.Dock = DockStyle.Fill;
            }
            UCEdit.Instanse.BringToFront();
            await Task.WhenAll(tasks);
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
