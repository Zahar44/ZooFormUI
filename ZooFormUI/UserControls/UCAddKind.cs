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
    public partial class UCAddKind : UCAddBase
    {
        private readonly IKindRepository Repository;

        private static Field<UserControl> _instanse;

        private Kind Kind { get; set; }
        private Kind LastKind { get; set; }
        public static UCAddBase Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCAddKind();
                return (UCAddBase)_instanse.getInstance();
            }
            set => _instanse = value;
        }
        
        public UCAddKind()
        {
            Repository = new KindRepository(new ZooDbContext());
            InitializeComponent();
            _ = LoadKindAsync();
            Name = "Kind";
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
            Repository.Create(SetKind());
        }
        public override void UpdateEntity()
        {
            LastKind = Kind.GetCopy();
            SetKind();
            Kind = Repository.Update(Kind);
            UndoAction = UndoUpdating;
        }
        public override void DeleteEntity(object entity)
        {
            var response = Repository.Get((entity as Kind).Id);
            LastKind = response.GetCopy();
            Repository.Remove(response.Id);
            UndoAction = UndoDeleting;
        }
        public override void Refresh(object _entity)
        {
            if (Controls.Count == 0)
                return;
            var entity = _entity as Kind;
            Kind = entity.Id != 0 ? Repository.Get(entity.Id) : entity;

            Controls["Name"].Text = Kind.Name;
            Controls["Description"].Text = Kind.Description;
            Controls["Сonditions"].Text = Kind.Сonditions;
            if (Kind.IsWormBlooded)
                (Controls["Worm"] as RadioButton).Checked = true;
            else
                (Controls["Cold"] as RadioButton).Checked = true;
        }
        public override void Refresh()
        {
            Refresh(new Kind());
        }
        public override UCAddBase Next()
        {
            return (UCAddBase)UCAddFood.Instanse;
        }
        public override UCAddBase Prev()
        {
            return (UCAddBase)UCAddAnimal.Instanse;
        }
        public override string InfoToString(object _entity)
        {
            var entity = _entity as Kind;
            entity = Repository.Get(entity.Id);
            StringBuilder res = new StringBuilder();

            res.Append($"{ entity.Name }\nConditions: { entity.Сonditions }\nDescriptions: { entity.Description }");

            return res.ToString();
        }

        protected async Task LoadKindBaseAsync()
        {
            ValidateProvider validateProvider = new ValidateProvider(this);

            Label lName = await Desiner.MakeLabelAsync("Name: ", 0);
            TextBox tbName = await Desiner.MakeTextBoxAsync("Name", 0);
            validateProvider.TBReq_Validating(tbName);

            Label lDescription = await Desiner.MakeLabelAsync("Description: ", 1);
            TextBox tbDescription = await Desiner.MakeTextBoxAsync("Description", 1);
            tbDescription.Height = 50;
            tbDescription.Multiline = true;
            tbDescription.WordWrap = true;
            tbDescription.ScrollBars = ScrollBars.Both;

            Label lСonditions = await Desiner.MakeLabelAsync("Сonditions: ", 3);
            TextBox tbСonditions = await Desiner.MakeTextBoxAsync("Сonditions", 3);
            tbСonditions.Height = 50;
            tbСonditions.Multiline = true;
            tbСonditions.WordWrap = true;
            tbСonditions.ScrollBars = ScrollBars.Both;

            Label lBlood = await Desiner.MakeLabelAsync("Blood type: ", 5);
            RadioButton rbWorm = new RadioButton
            {
                Location = new Point(110, 220),
                Width = 70,
                Text = "Worm",
                Name = "Worm",
            };
            RadioButton rbCold = new RadioButton
            {
                Location = new Point(180, 220),
                Width = 70,
                Text = "Cold",
                Name = "Cold",
            };
            rbCold.Checked = true;

            Controls.AddRange(new Control[] {
                lName, lDescription, lСonditions, lBlood,
                tbName, tbDescription, tbСonditions,
                rbWorm, rbCold
            });
            Refresh();
        }
        protected async virtual Task LoadKindAsync() => await LoadKindBaseAsync();
        private Kind SetKind()
        {
            Kind.Name = Controls["Name"].Text;
            Kind.IsWormBlooded = (Controls["Worm"] as RadioButton).Checked ? true : false;
            Kind.Description = Controls["Description"].Text;
            Kind.Сonditions = Controls["Сonditions"].Text;
            return Kind;
        }

        protected override void UndoCreating()
        {
            throw new NotImplementedException();
        }
        protected override void UndoUpdating()
        {
            if (LastKind == null)
                return;
            Kind.SetNewValue(LastKind);
            Repository.Update(Kind);
        }
        protected override void UndoDeleting()
        {
            if (LastKind == null)
                return;
            LastKind.Id = 0;
            Repository.Create(LastKind);
        }
    
        public override void SetRelationsForEntity(ICollection<object> ts)
        {
            throw new NotImplementedException();
        }
    }
}
