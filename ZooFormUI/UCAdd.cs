using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public UCAdd() : base()
        {
        }
        public override async Task Set(string sender, object entity = null)
        {
            switch (sender)
            {
                case "Animal":
                    await Instanse.LoadAnimalAsync();
                    break;
                case "Employee":
                    await Instanse.LoadEmployeeAsync();
                    break;
                case "Aviary":
                    await Instanse.LoadAviaryAsync();
                    break;
                case "Food":
                    await Instanse.LoadFoodAsync();
                    break;
                case "Kind":
                    await Instanse.LoadKindAsync();
                    break;
                default:
                    throw new Exception("Add: wrong statement value");
            }
        }
        protected override void BtnBack_Click(object sender, EventArgs e) => UCDBManager.Instanse.BringToFrontOrCreate();
        protected override async void BtnAcceptAsync_Click(object sender, EventArgs e)
        {
            foreach (Control control in Controls["Panel"].Controls)
            {
                if (control is RadioButton)
                    continue;
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
                case "Food":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var entity = GetEntity() as Food;
                        db.Foods.Add(entity);
                        db.SaveChanges();
                    }
                    break;
                case "Kind":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var entity = GetEntity() as Kind;
                        db.Kinds.Add(entity);
                        db.SaveChanges();
                    }
                    break;
                default:
                    throw new Exception("Add: can't find Statement");
            }
            await Instanse.BringToFrontOrCreateAsync(Statement);
        }
    }
}
