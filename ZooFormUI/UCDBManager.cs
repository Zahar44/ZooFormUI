using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            var tasks = new List<Task>();
            tasks.Add(Task.Run(() => { var a = UCAdd.Instanse;  }));
            tasks.Add(Task.Run(() => { var a = UCFind.Instanse; }));

            InitializeComponent();
            Task.WhenAll(tasks);
        }
        private async void UCDBManager_Load(object sender, EventArgs e)
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

            Button btnAdd = await MakeBtnAsync("Add", 0);
            btnAdd.Click += btnAdd_ClickContext;
            
            Button btnFind = await MakeBtnAsync("Find", 1);
            btnFind.Click += async (sender, e) => {  await UCFind.Instanse.BringToFrontOrCreateAsync(); };
            
            Button btnBack = await MakeBtnAsync("Back", 2);
            btnBack.Click += (sender, e) => { UCMain.Instanse.BringToFrontOrCreate(); };

            this.Controls.AddRange(new Control[] { btnAdd, btnFind, btnBack });
        }
        private async Task<Button> MakeBtnAsync(string name, int pos)
        {
            return await Task.Run(() => {
                return new Button
                {
                    Name = name,
                    Text = name,
                    Height = 80,
                    Width = 160,
                    Location = new Point(65, 47 + 95 * pos),
                };
            });
        }
        private void btnAdd_ClickContext(object sender, EventArgs e)
        {
            var addContextMenu = new ContextMenuStrip();

            var animal = new ToolStripMenuItem("Animal");
            animal.Click += btnAddAsync_Click;
            var kind = new ToolStripMenuItem("Kind");
            kind.Click += btnAddAsync_Click;
            var employee = new ToolStripMenuItem("Employee");
            employee.Click += btnAddAsync_Click;
            var aviary = new ToolStripMenuItem("Aviary");
            aviary.Click += btnAddAsync_Click;
            var food = new ToolStripMenuItem("Food");
            food.Click += btnAddAsync_Click;

            addContextMenu.Items.AddRange(new[] { animal, kind, employee, aviary, food });

            var _x = MainMenu.Instanse.Location.X + this.Controls[0].Location.X;
            var _y = MainMenu.Instanse.Location.Y + this.Controls[0].Location.Y;

            addContextMenu.Show(new Point(_x, _y));
        }
        private async void btnAddAsync_Click(object sender, EventArgs e)
        {
            await UCAdd.Instanse.BringToFrontOrCreateAsync(sender);
        }
    }
}
