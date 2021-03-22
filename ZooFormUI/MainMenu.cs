using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ZooFormUI
{
    public partial class MainMenu : Form
    {
        private static Field<MainMenu> _instanse;
        public static MainMenu Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new MainMenu();
                return _instanse.getInstance();
            }
            set => _instanse = value;
        }

        private static Field<Panel> _panel;
        public static Panel Panel
        {
            get
            {
                if (_panel == null)
                    _panel = new Panel();
                return _panel.getInstance();
            }
            set => _panel = value;
        }
        public MainMenu()
        {
            InitializeComponent();
        }
        private void MainMenu_Load(object sender, EventArgs e)
        {
            this.Width = 300;
            this.Height = 400;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            _instanse = this;

            Panel.Dock = DockStyle.Fill;
            this.Controls.Add(Panel);
            Panel.Controls.Add(UCMain.Instanse);
        }

        delegate void SetNewSizeCallback(object size);
    }
}
