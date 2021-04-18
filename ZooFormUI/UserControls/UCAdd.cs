using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZooFormUI.Database;
using ZooFormUI.Library;
using ZooFormUI.Repos;
using ZooFormUI.UserControls;

namespace ZooFormUI
{
    public partial class UCAdd : UserControl
    {
        private static Field<Panel> _panel;

        private static Field<UCAdd> _instanse;


        public static UCAdd Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCAdd();
                return _instanse.getInstance();
            }
            set => _instanse = value;
        }
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
        public UCAddBase LastAddedControl { get; set; }
       
        public UCAdd()
        {
            AutoValidate = AutoValidate.Disable;
            InitializeComponent();
        }

        protected virtual void LoadBase()
        {
            Width = 300;
            Height = 400;

            Button btnBack = new Button
            {
                Name = "Back",
                Text = "Back",
                Location = new Point(50, 280),
                Size = new Size(90, 50)
            };
            btnBack.Click += BtnBack_Click;

            Button btnAccept = new Button
            {
                Name = "Accept",
                Text = "Accept",
                Location = new Point(150, 280),
                Size = new Size(90, 50)
            };
            btnAccept.Click += BtnAccept_Click;
            
            Controls.AddRange(new Control[] { btnAccept, btnBack });
        }
        protected virtual void Load_UCAdd(object sender, System.EventArgs e)
        {
            LoadBase();

            Panel = new Panel
            {
                Name = "Panel",
                Size = new Size(300, 270),
                Location = new Point(0, 0),
            };
            Panel.BringToFront();

            Controls.AddRange(new Control[] { Panel });
        }
        protected virtual bool ValidateAll()
        {
            foreach (Control control in LastAddedControl.Controls)
            {
                if (control is RadioButton)
                    continue;
                control.Focus();
                if (!Validate())
                {
                    return false;
                }
            }
            return true;
        }
        protected virtual void BtnBack_Click(object sender, EventArgs e) => UCDBManager.Instanse.BringToFrontOrCreate();
        protected virtual void BtnAccept_Click(object sender, EventArgs e)
        {
            if (!ValidateAll())
                return;

            LastAddedControl.CreateEntity();
            LastAddedControl.Refresh();
        }
    }
}
