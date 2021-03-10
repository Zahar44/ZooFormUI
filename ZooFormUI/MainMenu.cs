using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ZooFormUI
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }
        static MainMenu _obj;
        public static MainMenu Instanse
        {
            get
            {
                if (_obj == null)
                {
                    _obj = new MainMenu();
                }
                return _obj;
            }
        }
        static Panel panel;
        public static Panel panelContainer
        {
            get{ return panel; }
            set { panel = value; }
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            this.Width  = 300;
            this.Height = 400;

            _obj = this;
            panel = new Panel();
            panel.Width = Width;
            panel.Height = Height;
            panel.Location = new Point(0, 0);
            this.Controls.Add(panel);

            panel.Controls.Add(UCMain.Instanse);
            UCMain.Instanse.BringToFront();
        }
    }
}
