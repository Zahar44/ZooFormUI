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
        public override void Set(string sender, object entity = null)
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
        public UCAdd() : base()
        {
        }
        protected override void BtnBack_Click(object sender, EventArgs e) => UCDBManager.Instanse.BringToFrontOrCreate();
        protected override void BtnAccept_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.Focus();
                if (!Validate())
                {
                    return;
                }
            }
            switch (Statement)
            {
                case "Animal":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var entity = GetEntity() as Animal;
                        db.Animals.Add(entity);
                        db.SaveChanges();
                    }
                    break;
                case "Employee":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var entity = GetEntity() as ZooKeeper;
                        db.ZooKeepers.Add(entity);
                        db.SaveChanges();
                    }
                    break;
                case "Aviary":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var entity = GetEntity() as Aviary;
                        db.Aviaries.Add(entity);
                        db.SaveChanges();
                    }
                    break;
                default:
                    throw new Exception("Add: can't find Statement");
            }
        }
    }
}
