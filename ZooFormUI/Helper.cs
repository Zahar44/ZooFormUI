using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
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
        public static void BringToFrontOrCreate(this UCFind instanse) 
        {
            Thread setSize = new Thread(new ParameterizedThreadStart(MainMenu.Instanse.SetNewSize));
            setSize.Start(new Size(500, 400));
            if (!MainMenu.Panel.Contains(UCFind.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCFind.Instanse);
                UCFind.Instanse.Dock = DockStyle.Fill;
            }
            UCFind.Instanse.ShowData("");
            UCFind.Instanse.BringToFront();
        }
        public static void BringToFrontOrCreate(this UCAdd instanse, object sender)
        {
            Thread setSize = new Thread(new ParameterizedThreadStart(MainMenu.Instanse.SetNewSize));
            setSize.Start(new Size(300, 400));
            UCAdd.SetInstanse(sender.ToString());
            if (!MainMenu.Panel.Contains(UCAdd.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCAdd.Instanse);
                UCAdd.Instanse.Dock = DockStyle.Fill;
            }
            UCAdd.Instanse.BringToFront();
        }
        public static void BringToFrontOrCreate(this UCEdit instanse, object sender, object entity)
        {
            Thread setSize = new Thread(new ParameterizedThreadStart(MainMenu.Instanse.SetNewSize));
            setSize.Start(new Size(300, 400));
            UCEdit.SetInstanse(sender.ToString(), entity);
            if (!MainMenu.Panel.Contains(UCEdit.Instanse))
            {
                MainMenu.Panel.Controls.Add(UCEdit.Instanse);
                UCEdit.Instanse.Dock = DockStyle.Fill;
            }
            UCEdit.Instanse.BringToFront();
        }
    }
}
