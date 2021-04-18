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
    public partial class UCAddAnimal : UCAddBase
    {
        private readonly IAnimalRepository Repository;
        private readonly IFoodRepository FoodRepository;

        private static Field<UserControl> _instanse;

        private Animal Animal { get; set; }
        private Animal LastAnimal { get; set; }
        public static UCAddBase Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCAddAnimal();
                return (UCAddBase)_instanse.getInstance();
            }
            set => _instanse = value;
        }

        public UCAddAnimal()
        {
            Repository = new AnimalRepository(new ZooDbContext());
            FoodRepository = new FoodRepository(new ZooDbContext());
            InitializeComponent();
            Name = "Animal";
            _ = LoadAnimalAsync();
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
            Animal = Repository.Create(SetAnimal());
        }
        public override void UpdateEntity()
        {
            LastAnimal = Animal.GetCopy();
            SetAnimal();
            Animal = Repository.Update(Animal);
            UndoAction = UndoUpdating;
        }
        public override void DeleteEntity(object entity)
        {
            var response = Repository.Get((entity as Animal).Id);
            LastAnimal = response.GetCopy();
            Repository.Remove(response.Id);
            UndoAction = UndoDeleting;
        }
        public override void SetRelationsForEntity(ICollection<object> _arr)
        {
            Animal = SetAnimal();
            var arr = _arr.ToList();

            if (Animal.AnimalFoods == null)
                Animal.AnimalFoods = new List<AnimalFood>();
            else
                Animal.AnimalFoods.Clear();

            for (int i = 0; i < arr.Count; i++)
            {
                Animal.AnimalFoods.Add(new AnimalFood
                {
                    AnimalId = Animal.Id,
                    Food = arr[i] as Food,
                    FoodId = (arr[i] as Food).Id,
                });
            }
            Refresh(Animal);
        }
        public override void Refresh(object _entity)
        {
            if (Controls.Count == 0)
                return;
            var entity = _entity as Animal;
            Animal = entity.Id != 0 ? Repository.Get(entity.Id) : entity;

            (Controls["Kind"] as ComboBox).Items.Clear();
            (Controls["Kind"] as ComboBox).Items.AddRange(UCAddKind.Instanse.GetAllEntity().Cast<object>().ToArray());

            (Controls["ZooKeeper"] as ComboBox).Items.Clear();
            (Controls["ZooKeeper"] as ComboBox).Items.AddRange(UCAddEmployee.Instanse.GetAllEntity().Cast<object>().ToArray());

            Controls["Name"].Text = Animal.Name;
            Controls["Age"].Text = Animal.Age.ToString();
            (Controls["Kind"] as ComboBox).Text = 
                Animal.Kind != null ? Animal.Kind.ToString() : null;
            (Controls["ZooKeeper"] as ComboBox).Text = 
                Animal.ZooKeeper != null ? Animal.ZooKeeper.ToString() : null;
            if (Animal.IsPredator)
                (Controls["Yes"] as RadioButton).Checked = true;
            else
                (Controls["Not"] as RadioButton).Checked = true;


            Controls["Food"].Text = 
                Animal.AnimalFoods == null || Animal.AnimalFoods.Count == 0 ?
                "Food" : $"Selected { Animal.AnimalFoods.Count }";
        }
        public override void Refresh()
        {
            Refresh(new Animal());
        }
        public override UCAddBase Next()
        {
            return (UCAddBase)UCAddKind.Instanse;
        }
        public override UCAddBase Prev()
        {
            return (UCAddBase)UCAddEmployee.Instanse;
        }
        public override string InfoToString(object _entity)
        {
            var entity = _entity as Animal;
            entity = Repository.Get(entity.Id);
            StringBuilder res = new StringBuilder();

            res.Append($"Name: { entity.Name }\nAge: { entity.Age }\nKind: { entity.Kind.Name }\n");
            string blood = entity.Kind.IsWormBlooded ? "worm" : "cold";
            string preda = entity.IsPredator ? "Predator" : "Nonaggressive";
            res.Append($"Blood: { blood }\n{ preda }\nKeeper: { entity.ZooKeeper.FullName }\nEat: ");

            List<Food> foods = new List<Food>();
            foreach (var food in entity.AnimalFoods)
            {
                foods.Add(FoodRepository.Get(food.FoodId));
            }

            for (int i = 0; i < foods.Count; i++)
            {
                if(i != foods.Count - 1)
                {
                    res.Append($"{ foods[i].Name }, ");
                }
                else
                {
                    res.Append($"{ foods[i].Name }");
                }
            }
            if(foods.Count == 0)
            {
                res.Replace("Eat: ", $"Foods for this animal not set");
            }

            return res.ToString();
        }

        private void BtnFood_Click(object sender, EventArgs e)
        {
            var foods = new List<Food>();

            if(Animal.AnimalFoods != null)
                foreach (var el in Animal.AnimalFoods)
                {
                    foods.Add(FoodRepository.Get(el.FoodId));
                }

            var form = new SelectItems(this, foods?.Cast<object>().ToList(), UCAddFood.Instanse);
            form.ShowDialog();
        }

        protected async Task LoadAnimalBaseAsync()
        {
            ValidateProvider validateProvider = new ValidateProvider(this);

            Label lName = await Desiner.MakeLabelAsync("Name:", 0);
            TextBox tbName = await Desiner.MakeTextBoxAsync("Name", 0);
            validateProvider.TBReq_Validating(tbName);

            Label lAge = await Desiner.MakeLabelAsync("Age:", 1);
            TextBox tbAge = await Desiner.MakeTextBoxAsync("Age", 1);
            validateProvider.TBNum_Validating(tbAge);

            Label lKind = await Desiner.MakeLabelAsync("Kind:", 2);
            ComboBox boxKind = new ComboBox
            {
                Name = "Kind",
                Height = 20,
                Location = new Point(110, 100)
            };
            FillBoxWithEntities(boxKind);
            validateProvider.Box_Validating(boxKind);

            Label lZooKeeper = await Desiner.MakeLabelAsync("Keeper:", 3);
            ComboBox boxZooKeeper = new ComboBox
            {
                Name = "ZooKeeper",
                Height = 20,
                Location = new Point(110, 140)
            };
            FillBoxWithEntities(boxZooKeeper);
            validateProvider.Box_Validating(boxZooKeeper);

            Label lPredator = await Desiner.MakeLabelAsync("Predator?", 4);
            RadioButton rbYes = new RadioButton
            {
                Location = new Point(110, 180),
                Width = 60,
                Text = "Yes",
                Name = "Yes",
            };
            RadioButton rbNot = new RadioButton
            {
                Location = new Point(180, 180),
                Width = 60,
                Text = "Not",
                Name = "Not",
            };
            rbNot.Checked = true;

            Button btnFood = await Desiner.MakeButtonAsync("Food", 5);
            btnFood.Click += BtnFood_Click;

            Controls.AddRange(new Control[] {
                lName, lZooKeeper, lKind, lAge, lPredator,
                tbName, tbAge, btnFood, rbYes, rbNot,
                boxZooKeeper, boxKind
            });
            Refresh();
        }
        protected async virtual Task LoadAnimalAsync() => await LoadAnimalBaseAsync();
        protected virtual void FillBoxWithEntities(object sender)
        {
            ComboBox box = sender as ComboBox;
            switch (box.Name)
            {
                case "ZooKeeper":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var entities = db.ZooKeepers.ToList();
                        foreach (var el in entities)
                        {
                            box.Items.Add(el);
                        }
                    }
                    break;
                case "Kind":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var entities = db.Kinds.ToList();
                        foreach (var el in entities)
                        {
                            box.Items.Add(el);
                        }
                    }
                    break;
                default:
                    break;
            }

        }
        private Animal SetAnimal()
        {
            try
            {
                Animal.Name = Controls["Name"].Text;
                Animal.Age = Int32.Parse(Controls["Age"].Text);
                Animal.Kind = (Controls["Kind"] as ComboBox).SelectedItem as Kind;
                Animal.ZooKeeper = (Controls["ZooKeeper"] as ComboBox).SelectedItem as ZooKeeper;
                Animal.IsPredator = (Controls["Yes"] as RadioButton).Checked ? true : false;
            }
            catch (Exception)
            {
            }
            
            return Animal;
        }

        protected override void UndoCreating()
        {
            throw new NotImplementedException();
        }
        protected override void UndoUpdating()
        {
            if (LastAnimal == null)
                return;
            Animal.SetNewValue(LastAnimal);
            Repository.Update(Animal);
        }
        protected override void UndoDeleting()
        {
            if (LastAnimal == null)
                return;
            LastAnimal.Id = 0;
            Repository.Create(LastAnimal);
        }
    }
}
