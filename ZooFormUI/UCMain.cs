using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ZooFormUI
{
    public partial class UCMain : UserControl
    {
        private static Field<UCMain> _instanse;
        public static UCMain Instanse
        {
            get 
            {
                if (_instanse == null)
                    _instanse = new UCMain();
               return _instanse.getInstance();
            } 
            set => _instanse = value;
        }
        public UCMain()
        {
            Thread create = new Thread(new ThreadStart(delegate {
                var instanse1 = UCDBManager.Instanse;
            }));
            create.Start();
            InitializeComponent();
        }
        private void UCMain_Load(object sender, EventArgs e)
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
            buttons[0].Click += (sender, e) => { UCDBManager.Instanse.BringToFrontOrCreate(); };

            buttons[1].Text = "Settings";

            buttons[2].Text = "Exit";
            buttons[2].Click += (sender, e) => { MainMenu.Instanse.Close(); };

            this.Controls.AddRange(buttons.ToArray());
        }
    }
}
