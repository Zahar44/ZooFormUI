using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZooFormUI
{
    public partial class UCDBManager : UserControl
    {
        static private UCDBManager _instanse;
        static public UCDBManager Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCDBManager();
                return _instanse;
            }
        }
        public UCDBManager()
        {
            InitializeComponent();
            UCDBManager_Load();
        }

        private void UCDBManager_Load()
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
            buttons[0].Text = "Add";
            buttons[0].Click += btnAdd_Click;

            buttons[1].Text = "Find";
            buttons[1].Click += (sender, e) => {
                if (!MainMenu.panelContainer.Contains(UCFind.Instanse))
                {
                    MainMenu.panelContainer.Controls.Add(UCFind.Instanse);
                    UCFind.Instanse.Dock = DockStyle.Fill;
                }
                UCFind.FillAll();
                UCFind.Instanse.BringToFront();
            };

            buttons[2].Text = "Back";
            buttons[2].Click += (sender, e) => { UCMain.Instanse.BringToFront(); };

            this.Controls.AddRange(buttons.ToArray());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addContextMenu = new ContextMenuStrip();

            var animal = new ToolStripMenuItem("Animal");
            animal.Click += (sender, e) => {
                UCAdd.SetInstanse(sender.ToString());
                if (!MainMenu.panelContainer.Contains(UCAdd.Instanse))
                {
                    MainMenu.panelContainer.Controls.Add(UCAdd.Instanse);
                    UCAdd.Instanse.Dock = DockStyle.Fill;
                }
                UCAdd.Instanse.BringToFront();
            };

            var employee = new ToolStripMenuItem("Employee");
            addContextMenu.Items.AddRange(new[] { animal, employee });

            var _x = MainMenu.Instanse.Location.X + this.Controls[0].Location.X;
            var _y = MainMenu.Instanse.Location.Y + this.Controls[0].Location.Y;

            addContextMenu.Show(new Point(_x, _y));
        }
    }
}
