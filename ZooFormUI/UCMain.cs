using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZooFormUI
{
    public partial class UCMain : UserControl
    {
        static private UCMain _instanse;
        static public UCMain Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCMain();
                return _instanse;
            }
        }
        public UCMain()
        {
            InitializeComponent();
            UCMain_Load();
        }

        private void UCMain_Load()
        {
            this.Width = 300;
            this.Height = 400;
            List<Button> buttons = new List<Button>();

            for (int i = 0; i < 3; i++)
            {
                buttons.Add(new Button());
                buttons[i].Height = 70;
                buttons[i].Width = 140;
                buttons[i].Location = new Point((this.Width - buttons[i].Width) / 2 - 5, (buttons[i].Height * buttons.Count + 10 * i) - 20);
            }
            buttons[0].Text = "DBManager";
            buttons[0].Click += (sender, e) =>
            {
                if (!MainMenu.panelContainer.Contains(UCDBManager.Instanse))
                {
                    MainMenu.panelContainer.Controls.Add(UCDBManager.Instanse);
                    UCDBManager.Instanse.Dock = DockStyle.Fill;
                }
                UCDBManager.Instanse.BringToFront();
            };

            buttons[1].Text = "Settings";

            buttons[2].Text = "Exit";
            buttons[2].Click += (sender, e) => { MainMenu.Instanse.Close(); };

            this.Controls.AddRange(buttons.ToArray());
        }
    }
}
