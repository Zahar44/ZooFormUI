using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZooFormUI.UserControls;

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
            //Thread thread = new Thread(new ThreadStart(LoadContentAsync));
            //thread.Start();
            InitializeComponent();
        }
        //private void LoadContentAsync()
        //{
        //    _ = UCAdd.Instanse.BringToFrontOrCreateAsync(UCAddAnimal.Instanse);
        //    _ = UCAdd.Instanse.BringToFrontOrCreateAsync(UCAddAviary.Instanse);
        //    _ = UCAdd.Instanse.BringToFrontOrCreateAsync(UCAddEmployee.Instanse);
        //    _ = UCAdd.Instanse.BringToFrontOrCreateAsync(UCAddFood.Instanse);
        //    _ = UCAdd.Instanse.BringToFrontOrCreateAsync(UCAddKind.Instanse);
        //}
        private async void UCDBManager_Load(object sender, EventArgs e)
        {
            this.Width = 300;
            this.Height = 400;

            Button btnAdd = await Desiner.MakeBigButtonAsync("Add", new Point(70, 50));
            btnAdd.Click += btnAdd_ClickContext;
            
            Button btnFind = await Desiner.MakeBigButtonAsync("Find", new Point(70, 50 + 15 * 1 + 80));
            btnFind.Click += async (sender, e) => {  await UCFind.Instanse.BringToFrontOrCreateAsync(); };
            
            Button btnBack = await Desiner.MakeBigButtonAsync("Back", new Point(70, 50 + 15 * 2 + 80 * 2));
            btnBack.Click += (sender, e) => { UCMain.Instanse.BringToFrontOrCreate(); };

            this.Controls.AddRange(new Control[] { btnAdd, btnFind, btnBack });
        }
        private void btnAdd_ClickContext(object sender, EventArgs e)
        {
            var addContextMenu = new ContextMenuStrip();

            var animal = new ToolStripMenuItem("Animal");
            animal.Click += (sender, e) => btnAdd_Click(sender, e, UCAddAnimal.Instanse);
            var kind = new ToolStripMenuItem("Kind");
            kind.Click += (sender, e) => btnAdd_Click(sender, e, UCAddKind.Instanse);
            var employee = new ToolStripMenuItem("Employee");
            employee.Click += (sender, e) => btnAdd_Click(sender, e, UCAddEmployee.Instanse);
            var aviary = new ToolStripMenuItem("Aviary");
            aviary.Click += (sender, e) => btnAdd_Click(sender, e, UCAddAviary.Instanse);
            var food = new ToolStripMenuItem("Food");
            food.Click += (sender, e) => btnAdd_Click(sender, e, UCAddFood.Instanse);

            addContextMenu.Items.AddRange(new[] { animal, kind, employee, aviary, food });
            addContextMenu.Show(Cursor.Position);
        }

        private void btnAdd_Click(object sender, EventArgs e, UCAddBase control)
        {
            control.Refresh();
            UCAdd.Instanse.BringToFrontOrCreate(control);
        }
    }
}
