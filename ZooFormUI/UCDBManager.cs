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
    public partial class UCDBManager : UserControl
    {
        private static Field<UCDBManager> _instanse;
        public static UCDBManager Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCDBManager();
                return _instanse.getInstance();
            }
            set => _instanse = value;
        }
        public UCDBManager()
        {
            Thread create = new Thread(new ThreadStart(delegate {
                var instanse1 = UCAdd.Instanse;
                var instanse2 = UCFind.Instanse;
            }));
            create.Start();
            InitializeComponent();
            UCDBManager_Load(this, new EventArgs());
        }
        private void UCDBManager_Load(object sender, EventArgs e)
        {
            this.Width = 300;
            this.Height = 400;
            List<Button> buttons = new List<Button>();

            for (int i = 0; i < 3; i++)
            {
                buttons.Add(new Button());
                buttons[i].Height = 70;
                buttons[i].Width = 140;
                buttons[i].Location = new Point(
                    (this.Width - buttons[i].Width) / 2 - 5, (buttons[i].Height * buttons.Count + 10 * i) - 20
                    );
            }
            buttons[0].Text = "Add";
            buttons[0].Click += btnAdd_ClickContext;

            buttons[1].Text = "Find";
            buttons[1].Click += (sender, e) => { UCFind.Instanse.BringToFrontOrCreate(); };

            buttons[2].Text = "Back";
            buttons[2].Click += (sender, e) => { UCMain.Instanse.BringToFrontOrCreate(); };

            this.Controls.AddRange(buttons.ToArray());
        }
        private void btnAdd_ClickContext(object sender, EventArgs e)
        {
            var addContextMenu = new ContextMenuStrip();

            var animal = new ToolStripMenuItem("Animal");
            animal.Click += btnAdd_Click;
            var employee = new ToolStripMenuItem("Employee");
            employee.Click += btnAdd_Click;

            addContextMenu.Items.AddRange(new[] { animal, employee });

            var _x = MainMenu.Instanse.Location.X + this.Controls[0].Location.X;
            var _y = MainMenu.Instanse.Location.Y + this.Controls[0].Location.Y;

            addContextMenu.Show(new Point(_x, _y));
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            UCAdd.Instanse.BringToFrontOrCreate(sender);
        }
    }
}
