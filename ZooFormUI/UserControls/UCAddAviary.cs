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
    public partial class UCAddAviary : UCAddBase
    {
        public readonly IAviaryRepository Repository;

        private static Field<UserControl> _instanse;

        private Aviary Aviary { get; set; }
        private Aviary LastAviary { get; set; }
        public static UCAddBase Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCAddAviary();
                return (UCAddBase)_instanse.getInstance();
            }
            set => _instanse = value;
        }
        
        public UCAddAviary()
        {
            Repository = new AviaryRepository(new ZooDbContext());
            InitializeComponent();
            _ = LoadAviaryAsync();
            Name = "Aviary";
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
            Repository.Create(SetAviary());
        }
        public override void UpdateEntity()
        {
            LastAviary = Aviary.GetCopy();
            SetAviary();
            Aviary = Repository.Update(Aviary);
            UndoAction = UndoUpdating;
        }
        public override void DeleteEntity(object entity)
        {
            var response = Repository.Get((entity as Aviary).Id);
            LastAviary = response.GetCopy();
            Repository.Remove(response.Id);
            UndoAction = UndoDeleting;
        }
        public override void SetRelationsForEntity(ICollection<object> _arr)
        {
            SetAviary();
            var arr = _arr.ToList();

            if (Aviary.AviaryKinds == null)
                Aviary.AviaryKinds = new List<AviaryKind>();
            else
                Aviary.AviaryKinds.Clear();

            for (int i = 0; i < arr.Count; i++)
            {
                Aviary.AviaryKinds.Add(new AviaryKind
                {
                    AviaryId = Aviary.Id,
                    Kind = arr[i] as Kind,
                    KindId = (arr[i] as Kind).Id,
                });
            }
            Refresh(Aviary);
        }
        public override void Refresh(object _entity)
        {
            if (Controls.Count == 0)
                return;
            var entity = _entity as Aviary;
            Aviary = entity.Id != 0 ? Repository.Get(entity.Id) : entity;

            Controls["Address"].Text = Aviary.Address;
            Controls["Size"].Text = $"{ Aviary.Length } x { Aviary.Width } x { Aviary.Height }";
            Controls["MaxAnimal"].Text = Aviary.MaxAnimalsCount.ToString();

            Controls["Type"].Text =
                Aviary.AviaryKinds == null || Aviary.AviaryKinds.Count == 0 ?
                "Type" : $"Selected { Aviary.AviaryKinds.Count }";
        }
        public override void Refresh()
        {
            Refresh(new Aviary());
        }
        public override UCAddBase Next()
        {
            return (UCAddBase)UCAddEmployee.Instanse;
        }
        public override UCAddBase Prev()
        {
            return (UCAddBase)UCAddFood.Instanse;
        }
        public override string InfoToString(object _entity)
        {
            var entity = _entity as Aviary;
            entity = Repository.Get(entity.Id);
            StringBuilder res = new StringBuilder();

            res.Append($"Address: { entity.Address }\n" +
                $"Size: { entity.Length * entity.Height * entity.Width } m3\n" +
                $"Can keep { entity.MaxAnimalsCount } animals\n");

            return res.ToString();
        }

        private void BtnType_Click(object sender, EventArgs e)
        {
            var kinds = new List<Kind>();

            if (Aviary.AviaryKinds != null)
                foreach (var el in Aviary.AviaryKinds)
                {
                    kinds.Add(el.Kind);
                }

            var form = new SelectItems(this, kinds?.Cast<object>().ToList(), UCAddKind.Instanse);
            form.ShowDialog();
        }

        protected async Task LoadAviaryBaseAsync()
        {
            ValidateProvider validateProvider = new ValidateProvider(this);

            Label lAddress = await Desiner.MakeLabelAsync("Address:", 0);
            TextBox tbAdress = await Desiner.MakeTextBoxAsync("Address", 0);
            tbAdress.Height = 50;
            tbAdress.Multiline = true;
            tbAdress.WordWrap = true;
            tbAdress.ScrollBars = ScrollBars.Both;
            validateProvider.TBReq_Validating(tbAdress);

            Label lSize = await Desiner.MakeLabelAsync("Size:", 2);
            TextBox tbSize = await Desiner.MakeTextBoxAsync("Size", 2);
            validateProvider.TBReq_Validating(tbSize);

            Label lMaxAnimal = await Desiner.MakeLabelAsync("Animals:", 3);
            TextBox tbMaxAnimal = await Desiner.MakeTextBoxAsync("MaxAnimal", 3);
            validateProvider.TBReq_Validating(tbMaxAnimal);

            Button btnType = await Desiner.MakeButtonAsync("Type", 4);
            btnType.Click += BtnType_Click;

            Controls.AddRange(new Control[] {
                btnType, lAddress, lSize, lMaxAnimal,
                tbAdress, tbSize, tbMaxAnimal
            });
            Refresh();
        }
        protected async virtual Task LoadAviaryAsync() => await LoadAviaryBaseAsync();
        private Aviary SetAviary()
        {
            Aviary.Address = Controls["Address"].Text;
            Aviary.MaxAnimalsCount = Int32.Parse(Controls["MaxAnimal"].Text);
            var tmp = Controls["Size"].Text;
            var _size = tmp.Split('x');
            if(_size.Length > 1)
            {
                Aviary.Length = Int32.Parse(_size[0]);
                Aviary.Width  = Int32.Parse(_size[1]);
                Aviary.Height = Int32.Parse(_size[2]);
            }
            return Aviary;
        }

        protected override void UndoCreating()
        {
            throw new NotImplementedException();
        }
        protected override void UndoUpdating()
        {
            if (LastAviary == null)
                return;
            Aviary.SetNewValue(LastAviary);
            Repository.Update(Aviary);
        }
        protected override void UndoDeleting()
        {
            if (LastAviary == null)
                return;
            LastAviary.Id = 0;
            Repository.Create(LastAviary);
        }
    }
}
