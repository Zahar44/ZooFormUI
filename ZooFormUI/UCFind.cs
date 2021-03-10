using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZooFormUI.Database;
using System.Linq;

namespace ZooFormUI
{
    public partial class UCFind : UserControl
    {
        static private UCFind _instanse;
        static public UCFind Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCFind();
                return _instanse;
            }
        }
        static public void FillAll()
        {
            _instanse.BoxFind_fillAll();
        }
        public UCFind()
        {
            InitializeComponent();
            UCFind_Load();
        }
        private ListBox listBox { get; set; }
        private void UCFind_Load()
        {
            this.Width = 300;
            this.Height = 400;

            Label labelFind = new Label();
            labelFind.Location = new Point(20, 25);
            labelFind.Text = "Find:";
            labelFind.Width = 50;

            TextBox boxFind = new TextBox();
            boxFind.Name = "Find";
            boxFind.Height = 20;
            boxFind.Location = new Point(100, 20);
            boxFind.TextChanged += BoxFind_TextChanged;

            listBox = new ListBox();
            listBox.Name = "Box";
            listBox.Width = 200;
            listBox.Height = 250;
            listBox.Location = new Point(50, 50);

            Button btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Click += (sender, e) => { UCDBManager.Instanse.BringToFront(); };

            Button btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Height = 50;
            btnEdit.Width = 90;
            btnEdit.Location = new Point(50, 300);

            Button btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Height = 50;
            btnDelete.Width = 90;
            btnDelete.Location = new Point(160, 300);
            btnDelete.Click += BtnDelete_Click;

            BoxFind_fillAll();

            this.Controls.Add(labelFind);
            this.Controls.Add(boxFind);
            this.Controls.Add(listBox);
            this.Controls.AddRange(new[] { btnBack, btnEdit, btnDelete });
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            using (ZooDbContext db = new ZooDbContext())
            {
                var entity = listBox.SelectedItem;
                var response = db.Animals.Where(x => x.Name == entity.ToString()).FirstOrDefault();
                db.Animals.Remove(response);
                db.SaveChanges();
            }
            BoxFind_fillAll();
        }

        private void BoxFind_fillAll()
        {
            using (ZooDbContext db = new ZooDbContext())
            {
                listBox.Items.Clear();
                var all = db.Animals.Where(x => x.Id != 0).ToList();
                foreach (var el in all)
                    listBox.Items.Add(el.Name);
            }
        }
        private void BoxFind_TextChanged(object sender, EventArgs e)
        {
            List<string> items = new List<string>();
            foreach (var el in listBox.Items)
                items.Add(el.ToString());
            items.Sort(delegate (string a, string b)
            {
                if (a == null && b == null) return 0;
                else if (a == null) return -1;
                else if (b == null) return 1;
                else return a.CompareTo(Controls["Find"].Text + " ");
            });
            listBox.Items.Clear();
            foreach (var el in items)
                listBox.Items.Add(el);
        }
    }
}
