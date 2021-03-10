using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZooFormUI.Database;

namespace ZooFormUI
{
    public partial class UCAdd : UserControl
    {
        static private UCAdd _instanse;
        static public UCAdd Instanse
        {
            get
            {
                if (_instanse == null)
                    throw new NoNullAllowedException("");
                return _instanse;
            }
        }

        static public void SetInstanse(string sender)
        {
            if (_instanse == null)
                _instanse = new UCAdd();
            switch (sender)
            {
                case "Animal":
                    _instanse.UCAdd_LoadAnimal();
                    break;
                default:
                    break;
            }
        }
        public UCAdd()
        {
            InitializeComponent();
        }

        private void UCAdd_LoadBase()
        {
            this.Width = 300;
            this.Height = 400;
            Button btnAccept = new Button();
            btnAccept.Width = 100;
            btnAccept.Height = 50;
            btnAccept.Text = "Accept";
            btnAccept.Click += BtnAccept_Click;
            btnAccept.Location = new Point((this.Width - btnAccept.Width) / 2, this.Bottom - btnAccept.Height - 100);
            this.Controls.Add(btnAccept);
        }

        private void UCAdd_LoadAnimal()
        {
            UCAdd_LoadBase();

            Label labelKind = new Label();
            labelKind.Location = new Point(20, 25);
            labelKind.Text = "Name:";
            labelKind.Width = 50;
            TextBox boxName = new TextBox();
            boxName.Name = "Name";
            boxName.Height = 20;
            boxName.Location = new Point(100, 20);

            this.Controls.AddRange(new[] { labelKind});
            this.Controls.AddRange(new[] { boxName});
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            Animal animal = new Animal();
            animal.Name = this.Controls["Name"].Text;
            animal.ZooKeeper = new ZooKeeper();

            if(animal != null)
                using(ZooDbContext db = new ZooDbContext())
                {
                    db.Animals.Add(animal);
                    db.SaveChanges();
                }
            UCDBManager.Instanse.BringToFront();
        }
    }
}
