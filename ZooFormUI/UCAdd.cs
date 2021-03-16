using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZooFormUI.Database;

namespace ZooFormUI
{
    public partial class UCAdd : UCAddBase
    {
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
        static public void SetInstanse(string sender)
        {
            switch (sender)
            {
                case "Animal":
                    Instanse.LoadAnimalBase();
                    break;
                case "Employee":
                    Instanse.LoadEmployeeBase();
                    break;
                default:
                    throw new Exception("Add: wrong statement value");
                    break;
            }
        }
        public UCAdd() : base()
        {
        }
        protected override void BtnAccept_Click_Animal(object sender, EventArgs e)
        {
            using (ZooDbContext db = new ZooDbContext())
            {
                Animal animal = new Animal();
                animal.Name = Controls["Name"].Text;
                animal.ZooKeeper = db.ZooKeepers
                    .Where(x => x.Name == Controls["ZooKeeper"].Text)
                    .FirstOrDefault();

                db.Animals.Add(animal);
                db.SaveChanges();
                LoadAnimalBase();
            }
        }
        protected override void BtnAccept_Click_Employee(object sender, EventArgs e)
        {
            using (ZooDbContext db = new ZooDbContext())
            {
               ZooKeeper zooKeeper = new ZooKeeper();
               zooKeeper.Name = this.Controls["Name"].Text;

               db.ZooKeepers.Add(zooKeeper);
               db.SaveChanges();
               LoadEmployeeBase();
            }
        }
    }
}
