using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZooFormUI.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ZooFormUI
{
    public partial class UCFind : UserControl
    {
        private static Field<UCFind> _instanse;
        public static UCFind Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCFind();
                return _instanse.getInstance();
            }
            set => _instanse = value;
        }
        private string LastFindedOption { get; set; }
        private ListBox FindBox { get; set; }
        public UCFind()
        {
            InitializeComponent();
        }
        private void UCFind_Load(object sender, EventArgs e)
        {
            Label labelFind = new Label();
            labelFind.Location = new Point(20, 25);
            labelFind.Text = "Find:";
            labelFind.Width = 50;

            TextBox boxFind = new TextBox();
            boxFind.Name = "Find";
            boxFind.Height = 20;
            boxFind.Location = new Point(100, 20);
            boxFind.TextChanged += BoxFind_TextChanged;

            FindBox = new ListBox();
            FindBox.Name = "Box";
            FindBox.Width = 200;
            FindBox.Height = 250;
            FindBox.Location = new Point(250, 50);
            FindBox.Click += FindBox_Click;

            Button btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Click += (sender, e) => {
                MainMenu.Instanse.Width = 300;
                UCDBManager.Instanse.BringToFront();
            };

            Button btnEdit = new Button();
            btnEdit.Text = "Edit";
            btnEdit.Size = new Size(90, 50);
            btnEdit.Location = new Point(50, 300);
            btnEdit.Click += BtnEdit_Click;

            Button btnDelete = new Button();
            btnDelete.Text = "Delete";
            btnDelete.Size = new Size(90, 50);
            btnDelete.Location = new Point(160, 300);
            btnDelete.Click += BtnDelete_Click;

            Button btnAnimal = new Button();
            btnAnimal.Text = "Animals";
            btnAnimal.Size = new Size(90, 50);
            btnAnimal.Location = new Point(20, 60);
            btnAnimal.Click += (sender, e) => { ShowData("Animal"); };

            Button btnEmployee = new Button();
            btnEmployee.Text = "Employees";
            btnEmployee.Size = new Size(90, 50);
            btnEmployee.Location = new Point(20, 110);
            btnEmployee.Click += (sender, e) => { ShowData("Employee"); };

            Button btnFood = new Button();
            btnFood.Text = "Foods";
            btnFood.Size = new Size(90, 50);
            btnFood.Location = new Point(20, 160);
            btnFood.Click += (sender, e) => { ShowData("Food"); };

            Button btnAviary = new Button();
            btnAviary.Text = "Aviaries";
            btnAviary.Size = new Size(90, 50);
            btnAviary.Location = new Point(20, 210);
            btnAviary.Click += (sender, e) => { ShowData("Aviary"); };

            ShowData("Animal");

            this.Controls.Add(labelFind);
            this.Controls.Add(boxFind);
            this.Controls.Add(FindBox);
            this.Controls.AddRange(
                new[] { btnBack, btnEdit, btnDelete, btnAnimal, btnEmployee, btnFood, btnAviary }
                );
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            UCEdit.Instanse.BringToFrontOrCreate(LastFindedOption, FindBox.SelectedItem);
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (FindBox.SelectedItem == null)
                return;
            if(LastFindedOption.ToLower() == "animal")
                using (ZooDbContext db = new ZooDbContext())
                {
                    var entity = FindBox.SelectedItem as Animal;
                    var response = db.Animals
                        .Where(x => x.Id == entity.Id)
                        .FirstOrDefault();
                    db.Animals.Remove(response);
                    db.SaveChanges();
                }
            if (LastFindedOption.ToLower() == "employee")
                using (ZooDbContext db = new ZooDbContext())
                {
                    var entity = FindBox.SelectedItem as ZooKeeper;
                    var response = db.ZooKeepers
                        .Include(x => x.Animals)
                        .Where(x => x.Id == entity.Id)
                        .FirstOrDefault();
                    if(response.Animals.Count > 0)
                    {
                        Alert alert = new Alert();
                        var result = alert.ShowDialog(response);
                        if (result == DialogResult.Yes)
                        {
                            db.ZooKeepers.Remove(response);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        db.ZooKeepers.Remove(response);
                        db.SaveChanges();
                    }
                }
            if (LastFindedOption.ToLower() == "food")
                using (ZooDbContext db = new ZooDbContext())
                {
                    var entity = FindBox.SelectedItem as Food;
                    var response = db.Foods
                        .Where(x => x.Id == entity.Id)
                        .FirstOrDefault();
                    db.Foods.Remove(response);
                    db.SaveChanges();
                }
            if (LastFindedOption.ToLower() == "aviary")
                using (ZooDbContext db = new ZooDbContext())
                {
                    var entity = FindBox.SelectedItem as Aviary;
                    var response = db.Aviaries
                        .Where(x => x.Id == entity.Id)
                        .FirstOrDefault();
                    db.Aviaries.Remove(response);
                    db.SaveChanges();
                }
            ShowData(LastFindedOption);
        }
        public void ShowData(string findValue)
        {
            if (findValue == "")
                findValue = LastFindedOption;
            FindBox.BeginUpdate();
            FindBox.Items.Clear();
            switch (findValue)
            {
                case "Animal":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var all = db.Animals.ToList();
                        FindBox.Items.AddRange(all.Cast<object>().ToArray());
                    }
                    break;
                case "Employee":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var all = db.ZooKeepers.ToList();
                        FindBox.Items.AddRange(all.Cast<object>().ToArray());
                    }
                    break;
                case "Food":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var all = db.Foods.ToList();
                        FindBox.Items.AddRange(all.Cast<object>().ToArray());
                    }
                    break;
                case "Aviary":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var all = db.Aviaries.ToList();
                        FindBox.Items.AddRange(all.Cast<object>().ToArray());
                    }
                    break;
                default:
                    break;
            }
            FindBox.Refresh();
            FindBox.EndUpdate();
            LastFindedOption = findValue;
        }
        private void FindBox_Click(object sender, EventArgs e)
        {
            if (FindBox.SelectedItem == null)
                return;
            ListBox lb = sender as ListBox;

            ToolTip yourToolTip = new ToolTip();
            yourToolTip.ToolTipIcon = ToolTipIcon.Info;
            yourToolTip.ShowAlways = true;

            switch (LastFindedOption)
            {
                case "Employee":
                    string showInfo = FindPopUpInfo_Employee();
                    if(showInfo != "")
                        yourToolTip.SetToolTip(FindBox, showInfo);
                    break;
                default:
                    break;
            }
            

        }
        private string FindPopUpInfo_Employee()
        {
            ZooKeeper entity;
            using (ZooDbContext db = new ZooDbContext())
            {
                entity = db.ZooKeepers
                    .Include(x => x.Animals)
                    .Where(x => x.Name == FindBox.SelectedItem.ToString())
                    .FirstOrDefault();
            }
            string res = entity.Name + " have " + entity.Animals.Count + " animals: ";
            foreach (var animal in entity.Animals)
            {
                res += animal.Name.ToString() + ", ";
            }
            if (res.LastIndexOf(',') == -1)
                return res.Remove(res.LastIndexOf(':'));
            else
                return res.Remove(res.LastIndexOf(','));
        }
        private void BoxFind_TextChanged(object sender, EventArgs e)
        {
            List<string> items = new List<string>();
            foreach (var el in FindBox.Items)
                items.Add(el.ToString());
            items.Sort(delegate (string a, string b)
            {
                if (a == null && b == null) return 0;
                else if (a == null) return -1;
                else if (b == null) return 1;
                else
                {
                    int commonA = 0;
                    int commonB = 0;
                    for(int i = 0; i < Math.Min(a.Length, Controls["Find"].Text.Length); i++)
                    {
                        if (a[i].ToString().ToLower() == Controls["Find"].Text[i].ToString().ToLower())
                            commonA++;
                        else
                            commonA--;
                    }
                    commonA -= Math.Abs(Controls["Find"].Text.Length - a.Length);
                    for (int i = 0; i < Math.Min(b.Length, Controls["Find"].Text.Length); i++)
                    {
                        if (b[i].ToString().ToLower() == Controls["Find"].Text[i].ToString().ToLower())
                            commonB++;
                        else
                            commonB--;
                    }
                    commonB -= Math.Abs(Controls["Find"].Text.Length - b.Length);
                    return commonB.CompareTo(commonA);
                }
            });
            FindBox.Items.Clear();
            foreach (var el in items)
                FindBox.Items.Add(el);
        }
    }
}
