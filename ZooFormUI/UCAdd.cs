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
using ZooFormUI.Library;
using ZooFormUI.Repos;

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
            EntityProvider ep = new EntityProvider(this, Statement);
            var entity = ep.GetEntity();
            var repo = ep.GetRepository();

            switch (Statement)
            {
                case "Animal":
                    (repo as AnimalRepository).Create(entity as Animal);
                    break;
                case "Employee":
                    (repo as ZooKeeperRepository).Create(entity as ZooKeeper);
                    break;
                case "Aviary":
                    (repo as AviaryRepository).Create(entity as Aviary);
                    break;
                case "Food":
                    (repo as FoodRepository).Create(entity as Food);
                    break;
                case "Kind":
                    (repo as KindRepository).Create(entity as Kind);
                    break;
                default:
                    throw new Exception("Add: can't find Statement");
            }
            await Instanse.BringToFrontOrCreateAsync(Statement);
        }
    }
}
