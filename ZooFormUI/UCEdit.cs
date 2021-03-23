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
    public partial class UCEdit : UCAddBase
    {
        private static Field<UCEdit> _instanse;
        protected object Entity { get; set; }
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
        public override async Task Set(string sender, object entity)
        {
            Entity = entity;
            switch (sender)
            {
                case "Animal":
                    await Instanse.LoadAnimalAsync();
                    break;
                case "Kind":
                    await Instanse.LoadKindAsync();
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
                default:
                    throw new Exception("Add: wrong statement value");
            }
        }
        protected override async Task LoadAnimalAsync()
        {
            await LoadAnimalBaseAsync();
            var _entity = Entity as Animal;
            var panel = Controls["Panel"] as UserControl;

            panel.Controls["Name"].Text = _entity.Name;
            panel.Controls["Age"].Text  = _entity.Age.ToString();
            if (_entity.IsPredator == true)
                (panel.Controls["Yes"] as RadioButton).Checked = true;
            else
                (panel.Controls["Not"] as RadioButton).Checked = true;

            using(ZooDbContext db = new ZooDbContext()) 
            {
                var _kind = db.Kinds
                            .Where(x => x.Id == _entity.KindId)
                            .FirstOrDefault();
                panel.Controls["Kind"].Text = _kind.ToString();
                var _zooKeeper = db.ZooKeepers
                            .Where(x => x.Id == _entity.ZooKeeperId)
                            .FirstOrDefault();
                panel.Controls["ZooKeeper"].Text = _zooKeeper.ToString();
            }
        }
        protected override async Task LoadKindAsync()
        {
            await LoadKindBaseAsync();
            var _entity = Entity as Kind;
            var panel = Controls["Panel"] as UserControl;

            panel.Controls["Name"].Text = _entity.Name;
            panel.Controls["Description"].Text = _entity.Description;
            panel.Controls["Сonditions"].Text = _entity.Сonditions;
            if (_entity.IsWormBlooded)
                (panel.Controls["Worm"] as RadioButton).Checked = true;
            else
                (panel.Controls["Cold"] as RadioButton).Checked = true;
        }
        protected override async Task LoadEmployeeAsync()
        {
            await LoadEmployeeBaseAsync();
            var _entity = Entity as ZooKeeper;
            var panel = Controls["Panel"] as UserControl;

            panel.Controls["Name"].Text       = _entity.Name;
            panel.Controls["Family"].Text     = _entity.Family.ToString();
            panel.Controls["Salary"].Text     = _entity.Salary.ToString();
            panel.Controls["Telephone"].Text  = _entity.Telephone;
            panel.Controls["Address"].Text    = _entity.Address;
        }
        protected override async Task LoadAviaryAsync()
        {
            await LoadAviaryBaseAsync();
            var _entity = Entity as Aviary;
            var panel = Controls["Panel"] as UserControl;

            panel.Controls["Address"].Text = _entity.Address;
            //panel.Controls["Size"].Text = _entity;
            panel.Controls["MaxAnimal"].Text = _entity.MaxAnimalsSize.ToString();
        }
        protected override async Task LoadFoodAsync()
        {
            await LoadFoodBaseAsync();
            var _entity = Entity as Food;
            var panel = Controls["Panel"] as UserControl;

            panel.Controls["Name"].Text = _entity.Name;
            panel.Controls["Amount"].Text = _entity.Amount.ToString();
            panel.Controls["Category"].Text = _entity.Category;
            panel.Controls["Suitability"].Text = (DateTime.Parse(_entity.RotAt.ToString()) - DateTime.Now).TotalDays.ToString().ToString();
            if(_entity.Freeze)
                (panel.Controls["Yes"] as RadioButton).Checked = true;
            else
                (panel.Controls["Not"] as RadioButton).Checked = true;
        }
        protected override void BtnBack_Click(object sender, EventArgs e) => _ = UCFind.Instanse.BringToFrontOrCreateAsync();
        protected override void BtnAcceptAsync_Click(object sender, EventArgs e)
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
            var ep = new EntityProvider(this, Statement);
            var entity = ep.GetEntity();
            var repo = ep.GetRepository();

            switch (Statement)
            {
                case "Animal":
                    (repo as AnimalRepository).Update(entity as Animal);
                    break;
                case "Employee":
                    (repo as ZooKeeperRepository).Update(entity as ZooKeeper);
                    break;
                case "Aviary":
                    (repo as AviaryRepository).Update(entity as Aviary);
                    break;
                case "Food":
                    (repo as FoodRepository).Update(entity as Food);
                    break;
                case "Kind":
                    (repo as KindRepository).Update(entity as Kind);
                    break;
                default:
                    throw new Exception("Add: can't find Statement");
            }
            /*
            switch (Statement)
            {
                case "Animal":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var _entity = GetEntity() as Animal;
                        var response = db.Animals
                                    .Where(x => x.Id == (Entity as Animal).Id)
                                    .FirstOrDefault();

                        if (response != null)
                        {
                            response.Name           = _entity.Name;
                            response.KindId         = _entity.KindId;
                            response.Kind           = _entity.Kind;
                            response.ZooKeeperId    = _entity.ZooKeeperId;
                            response.ZooKeeper      = _entity.ZooKeeper;
                            response.IsPredator     = _entity.IsPredator;
                            response.Aviary         = _entity.Aviary;
                        }

                        db.SaveChanges();
                    }
                    break;
                case "Employee":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var _entity = GetEntity() as ZooKeeper;
                        var response = db.ZooKeepers
                                    .Where(x => x.Id == (Entity as ZooKeeper).Id)
                                    .FirstOrDefault();

                        if (response != null)
                        {
                            response.Name       = _entity.Name;
                            response.Family     = _entity.Family;
                            response.Animals    = _entity.Animals;
                            response.Address    = _entity.Address;
                            response.Salary     = _entity.Salary;
                            response.Telephone  = _entity.Telephone;
                        }

                        db.SaveChanges();
                    }
                    break;
                case "Aviary":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var _entity = GetEntity() as Aviary;
                        var response = db.Aviaries
                                    .Where(x => x.Id == (Entity as Aviary).Id)
                                    .FirstOrDefault();

                        if (response != null)
                        {
                            response.Address        = _entity.Address;
                            response.Animals        = _entity.Animals;
                            response.CanHold        = _entity.CanHold;
                            response.MaxAnimalsSize = _entity.MaxAnimalsSize;
                            response.Height         = _entity.Height;
                            response.Length         = _entity.Length;
                            response.Width          = _entity.Width;
                        }

                        db.SaveChanges();
                    }
                    break;
                case "Food":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var _entity = GetEntity() as Food;
                        var response = db.Foods
                                    .Where(x => x.Id == (Entity as Food).Id)
                                    .FirstOrDefault();

                        if (response != null) 
                        {
                            response.Name        = _entity.Name;
                            response.RotAt       = _entity.RotAt;
                            response.Freeze      = _entity.Freeze;
                            response.Category    = _entity.Category;
                            response.Amount      = _entity.Amount;
                            response.AnimalFoods = _entity.AnimalFoods;
                        }
                        
                        db.SaveChanges();
                    }
                    break;
                default:
                    break;
            }
            */
            BtnBack_Click(sender, e);
        }
        private void UCEdit_Load(object sender, EventArgs e)
        {

        }
    }
}
