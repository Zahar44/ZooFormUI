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

namespace ZooFormUI.UserControls
{
    public partial class UCAddFood : UCAddBase
    {
        public readonly IFoodRepository Repository;
        
        private static Field<UserControl> _instanse;
        private Food LastFood { get; set; }
        private Food Food { get; set; }

        public static UCAddBase Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCAddFood();
                return (UCAddBase)_instanse.getInstance();
            }
            set => _instanse = value;
        }

        public UCAddFood()
        {
            Repository = new FoodRepository(new ZooDbContext());
            InitializeComponent();
            _ = LoadFoodAsync();
            Name = "Food";
        }
        public override ICollection<object> GetAllEntity()
        {
            return Repository.GetAll().Cast<object>().ToList();
        }
        public override object GetEntity(int ind)
        {
            return (object)Repository.Get(ind);
        }
        public override void CreateEntity()
        {
            Repository.Create(SetFood());
        }
        public override void UpdateEntity()
        {
            LastFood = Food.GetCopy();
            SetFood();
            Food = Repository.Update(Food);
            UndoAction = UndoUpdating;
        }
        public override void DeleteEntity(object entity)
        {
            var response = Repository.Get((entity as Food).Id);
            LastFood = response.GetCopy();
            Repository.Remove(response.Id);
            UndoAction = UndoDeleting;
        }
        public override void Refresh(object _entity)
        {
            if (Controls.Count == 0)
                return;
            var entity = _entity as Food;
            Food = entity.Id != 0 ? Repository.Get(entity.Id) : entity;

            Controls["Name"].Text = Food.Name;
            Controls["Amount"].Text = Food.Amount.ToString();
            (Controls["Category"] as ComboBox).Text = Food.Category;
            if (Food.Freeze)
                (Controls["Yes"] as RadioButton).Checked = true;
            else
                (Controls["Not"] as RadioButton).Checked = true;
            Controls["Suitability"].Text = ((int)(Food.RotAt - DateTime.Now).TotalDays).ToString();
        }
        public override void Refresh()
        {
            Refresh(new Food());
        }
        public override UCAddBase Next()
        {
            return (UCAddBase)UCAddAviary.Instanse;
        }
        public override UCAddBase Prev()
        {
            return (UCAddBase)UCAddKind.Instanse;
        }
        public override string InfoToString(object _entity)
        {
            var entity = _entity as Food;
            entity = Repository.Get(entity.Id);
            StringBuilder res = new StringBuilder();

            res.Append($"Name: { entity.Name }\nCategory: { entity.Category }\nAmount: { entity.Amount }\n");
            string frez = entity.Freeze ? "Needed to freeze\n" : "";
            res.Append($"{ frez }Rot at { entity.RotAt }");

            return res.ToString();
        }

        protected async Task LoadFoodBaseAsync()
        {
            ValidateProvider validateProvider = new ValidateProvider(this);

            Label lName = await Desiner.MakeLabelAsync("Name:", 0);
            TextBox tbName = await Desiner.MakeTextBoxAsync("Name", 0);
            validateProvider.TBReq_Validating(tbName);

            Label lAmount = await Desiner.MakeLabelAsync("Amount:", 1);
            TextBox tbAmount = await Desiner.MakeTextBoxAsync("Amount", 1);
            validateProvider.TBNum_Validating(tbAmount);

            Label lCategory = await Desiner.MakeLabelAsync("Category:", 2);
            ComboBox boxCategory = new ComboBox
            {
                Name = "Category",
                Height = 20,
                Location = new Point(110, 100)
            };
            boxCategory.Items.AddRange(new[] { "Meals", "Fruits", "Vegetables", "Other" });
            boxCategory.SelectedItem = "Other";
            validateProvider.Box_Validating(boxCategory);

            Label lRot = await Desiner.MakeLabelAsync("Suitability:", 3);
            TextBox tbRot = await Desiner.MakeTextBoxAsync("Suitability", 3);
            validateProvider.TBNum_Validating(tbRot);

            Label lFreeze = await Desiner.MakeLabelAsync("Freezed?", 4);
            RadioButton rbYes = new RadioButton
            {
                Location = new Point(110, 180),
                Width = 60,
                Text = "Yes",
                Name = "Yes"
            };
            RadioButton rbNot = new RadioButton
            {
                Location = new Point(180, 180),
                Width = 60,
                Text = "Not",
                Name = "Not"
            };
            rbNot.Checked = true;

            Controls.AddRange(new Control[] {
                lName, lAmount, lCategory, lFreeze, lRot,
                tbName, tbAmount, tbRot, rbYes, rbNot, boxCategory
            });
            Refresh();
        }
        protected async virtual Task LoadFoodAsync() => await LoadFoodBaseAsync();
        private Food SetFood()
        {
            Food.Name = Controls["Name"].Text;
            Food.Amount = Int32.Parse(Controls["Amount"].Text);
            Food.Category = (Controls["Category"] as ComboBox).SelectedText;
            Food.Freeze = (Controls["Yes"] as RadioButton).Checked ? true : false;
            Food.RotAt = DateTime.Parse(DateTime.Now.ToString()).AddDays(double.Parse(Controls["Suitability"].Text.ToString()));
            return Food;
        }

        public override void SetRelationsForEntity(ICollection<object> ts)
        {
            throw new NotImplementedException();
        }
        protected override void UndoCreating()
        {
            throw new NotImplementedException();
        }
        protected override void UndoUpdating()
        {
            if (LastFood == null)
                return;
            Food.SetNewValue(LastFood);
            Repository.Update(Food);
        }
        protected override void UndoDeleting()
        {
            if (LastFood == null)
                return;
            LastFood.Id = 0;
            Repository.Create(LastFood);
        }
    }
}
