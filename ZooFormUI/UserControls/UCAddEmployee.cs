using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZooFormUI.Database;
using ZooFormUI.Library;
using ZooFormUI.Repos;
using static ZooFormUI.Database.Person;

namespace ZooFormUI.UserControls
{
    public partial class UCAddEmployee : UCAddBase
    {
        private readonly IZooKeeperRepository Repository;

        private static Field<UserControl> _instanse;

        private ZooKeeper Employee { get; set; }
        private ZooKeeper LastEmployee { get; set; }

        public static UCAddBase Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCAddEmployee();
                return (UCAddBase)_instanse.getInstance();
            }
            set => _instanse = value;
        }
        

        public UCAddEmployee()
        {
            Repository = new ZooKeeperRepository(new ZooDbContext());
            InitializeComponent();
            _ =  LoadEmployeeAsync();
            Name = "Employee";
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
            SetEmployee();
            LastEmployee = Employee.GetCopy();
            Employee = Repository.Create(Employee);
            UndoAction = UndoCreating;
        }
        public override void UpdateEntity()
        {
            LastEmployee = Employee.GetCopy();
            SetEmployee();
            Employee = Repository.Update(Employee);
            UndoAction = UndoUpdating;
        }
        public override void DeleteEntity(object entity)
        {
            var response = Repository.Get((entity as ZooKeeper).Id);
            LastEmployee = response.GetCopy();
            if (response.Animals?.Count > 0)
            {
                Alert alert = new Alert();
                var result = alert.ShowDialog(response);
                if (result == DialogResult.Yes)
                {
                    Repository.Remove(response.Id);
                    LastEmployee.Animals = null;
                }
            }
            else
            {
                Repository.Remove(response.Id);
                LastEmployee.Animals = null;
            }
            UndoAction = UndoDeleting;
        }
        public override void Refresh(object _entity)
        {
            if (Controls.Count == 0)
                return;
            var entity = _entity as ZooKeeper;
            Employee = entity.Id != 0 ? Repository.Get(entity.Id) : entity;

            Controls["Name"].Text = Employee.FullName;
            Controls["Family"].Text = Employee.Family.ToString();
            Controls["Salary"].Text = Employee.Salary.ToString();
            Controls["Telephone"].Text = Employee.Telephone;
            Controls["Address"].Text = Employee.Address;
        }
        public override void Refresh()
        {
            Refresh(new ZooKeeper());
        }
        public override UCAddBase Next()
        {
            return (UCAddBase)UCAddAnimal.Instanse;
        }
        public override UCAddBase Prev()
        {
            return (UCAddBase)UCAddAviary.Instanse;
        }
        public override string InfoToString(object _entity)
        {
            var entity = _entity as ZooKeeper;
            entity = Repository.Get(entity.Id);
            StringBuilder res = new StringBuilder();

            res.Append($"{ entity.FullName }\nSalary: { entity.Salary }\nAnimals: ");

            int animalsSize = entity.Animals.Count;
            int size = animalsSize > 4 ? 4 : animalsSize;
            for (int i = 0; i < size; i++)
            {
                if(i != size - 1)
                {
                    res.Append($"{ entity.Animals.ElementAt(i).Name }, ");
                }
                else if(animalsSize > size)
                {
                    res.Append($" and other...");
                }
                else
                {
                    res.Append($"{ entity.Animals.ElementAt(i).Name }");
                }
            }
            if(animalsSize == 0)
            {
                res.Replace("Animals:", "This keeper doesn't have any animals");
            }
            res.Append($"\nTelephone: { entity.Telephone }");

            return res.ToString();
        }

        protected async Task LoadEmployeeBaseAsync()
        {
            ValidateProvider validateProvider = new ValidateProvider(this);

            Label lName = await Desiner.MakeLabelAsync("Name:", 0);
            TextBox tbName = await Desiner.MakeTextBoxAsync("Name", 0);
            validateProvider.TBReq_Validating(tbName);

            Label lFamily = await Desiner.MakeLabelAsync("Family:", 1);
            ComboBox boxFamily = new ComboBox
            {
                Name = "Family",
                Height = 20,
                Location = new Point(110, 60)
            };
            boxFamily.Items.AddRange(new[] { "Male", "Female", "Unknown" });
            validateProvider.Box_Validating(boxFamily);

            Label lSalary = await Desiner.MakeLabelAsync("Salary:", 2);
            TextBox tbSalary = await Desiner.MakeTextBoxAsync("Salary", 2);
            validateProvider.TBNum_Validating(tbSalary);

            Label lTel = await Desiner.MakeLabelAsync("Telephone:", 3);
            TextBox tbTel = await Desiner.MakeTextBoxAsync("Telephone", 3);
            validateProvider.TBTel_Validating(tbTel);
            validateProvider.TbTel_Leave(tbTel);

            Label lAddress = await Desiner.MakeLabelAsync("Address:", 4);
            TextBox tbAddress = await Desiner.MakeTextBoxAsync("Address", 4);
            tbAddress.Height = 50;
            tbAddress.Multiline = true;
            tbAddress.WordWrap = true;
            tbAddress.ScrollBars = ScrollBars.Both;
            validateProvider.TBReq_Validating(tbAddress);

            Controls.AddRange(new Control[] {
                lName, lFamily, lSalary, lTel, lAddress,
                tbName, tbSalary, tbTel, tbAddress, boxFamily
            });
            Refresh();
        }
        protected async virtual Task LoadEmployeeAsync() => await LoadEmployeeBaseAsync();
        private ZooKeeper SetEmployee()
        {
            Employee.FullName = Controls["Name"].Text;
            Employee.Family = new Func<Person.family>(() => {
                if (Controls["Family"].Text == "Male")
                    return Person.family.Male;
                else if (Controls["Family"].Text == "Female")
                    return family.Female;
                else
                    return Person.family.Unknown;
            })();
            Employee.Salary = Int32.Parse(Controls["Salary"].Text);
            Employee.Telephone = Controls["Telephone"].Text;
            Employee.Address = Controls["Address"].Text;

            return Employee;
        }

        public override void SetRelationsForEntity(ICollection<object> ts)
        {
            throw new NotImplementedException();
        }
    
        protected override void UndoCreating()
        {
            if (LastEmployee == null)
                return;
            Employee = LastEmployee;
            Repository.Remove(Employee.Id);
        }
        protected override void UndoUpdating()
        {
            if (LastEmployee == null)
                return;
            //LastEmployee.Animals = null;
            Employee.SetNewValue(LastEmployee);
            Repository.Update(Employee);
        }
        protected override void UndoDeleting()
        {
            if (LastEmployee == null)
                return;
            LastEmployee.Id = 0;
            Repository.Create(LastEmployee);
        }
    }
}
