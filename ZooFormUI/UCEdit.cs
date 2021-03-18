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
    public partial class UCEdit : UCAddBase
    {
        private static Field<UCEdit> _instanse;
        public static UCEdit Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCEdit();
                return _instanse.getInstance();
            }
            set => _instanse = value;
        }
        public UCEdit()
        {
            InitializeComponent();
        }
        public override void Set(string sender, object entity)
        {
            switch (sender)
            {
                case "Animal":
                    Instanse.LoadAnimalBase();
                    break;
                case "Employee":
                    Instanse.LoadEmployeeBase();
                    break;
                case "Aviary":
                    Instanse.LoadAviaryBase();
                    break;
                default:
                    throw new Exception("Add: wrong statement value");
            }
        }
        protected override void LoadAnimal()
        {
            LoadAnimalBase();
            var _entity = Entity as Animal;
            Controls["Name"].Text = _entity.Name;

            using (ZooDbContext db = new ZooDbContext())
            {
                Controls["ZooKeeper"].Text 
                            = db.ZooKeepers
                            .Where(x => x.Id == _entity.ZooKeeperId)
                            .FirstOrDefault().ToString();
            }
        }
        protected override void LoadEmployee()
        {
            LoadEmployeeBase();

        }
        protected override void BtnBack_Click(object sender, EventArgs e) => UCFind.Instanse.BringToFrontOrCreate();
        protected override void BtnAccept_Click(object sender, EventArgs e)
        {

        }

        /*
        protected override void BtnAccept_Click_Animal(object sender, EventArgs e)
        {
            using (ZooDbContext db = new ZooDbContext())
            {
                var _entity = Entity as Animal;
                var animal = db.Animals.Where(x => x.Id == _entity.Id).FirstOrDefault();
                animal.Name = Controls["Name"].Text;

                animal.ZooKeeper = db.ZooKeepers
                            .Where(x => x.Id == ((Controls["ZooKeeper"] as ComboBox)
                            .SelectedItem as ZooKeeper).Id)
                            .FirstOrDefault();

                db.SaveChanges();
                LoadAnimalBase();
            }
            UCFind.Instanse.BringToFrontOrCreate();
        }
        protected override void BtnAccept_Click_Employee(object sender, EventArgs e)
        {
            UCFind.Instanse.BringToFrontOrCreate();
        }
        */
        private void UCEdit_Load(object sender, EventArgs e)
        {

        }
    }
}
