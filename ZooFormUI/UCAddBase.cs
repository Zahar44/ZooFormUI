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
    abstract public partial class UCAddBase : UserControl
    {

        public abstract Task Set(string sender, object entity = null);
        public UCAddBase()
        {
            AutoValidate = AutoValidate.Disable;
            LoadBase();
            InitializeComponent();
        }
        protected string Statement { get; set; }
        protected object Buffer { get; set; }
        protected void LoadBase()
        {
            Width = 300;
            Height = 400;
            Controls.Clear();

            Button btnBack = new Button
            {
                Name = "Back",
                Text = "Back",
                Location = new Point(50, 280),
                Size = new Size(90, 50)
            };
            btnBack.Click += BtnBack_Click;

            Button btnAccept = new Button
            {
                Name = "Accept",
                Text = "Accept",
                Location = new Point(150, 280),
                Size = new Size(90, 50)
            };
            btnAccept.Click += BtnAcceptAsync_Click;

            UserControl panel = new UserControl
            {
                Name = "Panel",
                Size = new Size(300, 270),
                Location = new Point(0, 0),
            };
            panel.BringToFront();
            
            Controls.AddRange(new Control[] { btnAccept, btnBack, panel });
        }
        protected virtual object GetEntity()
        {
            switch (Statement)
            {
                case "Animal":
                    return GetAnimal();
                case "Employee":
                    return GetEmployee();
                case "Aviary":
                    return GetAviary();
                case "Food":
                    return GetFood();
                case "Kind":
                    return GetKind();
                default:
                    return null;
            }
        }

        /*
        private PicturePanel MakeBackBtn()
        {
            Bitmap image = new Bitmap(Image.FromFile("..\\..\\..\\Source\\Back_Arrow.png"));
            image = image.ResizeImage(60, 60);
            PicturePanel btn = new PicturePanel();
            btn.Location = new Point(60, 280);
            btn.Click += (sender, e) => { UCDBManager.Instanse.BringToFrontOrCreate(); };
            btn.Size = image.Size;
            btn.BackgroundImage = image;
            return btn;
        }
        private PicturePanel MakeAcceptBtn()
        {
            Bitmap image = new Bitmap(Image.FromFile("..\\..\\..\\Source\\Accept_Tick.png"));
            image = image.ResizeImage(60, 60);
            PicturePanel btn = new PicturePanel();
            btn.Location = new Point(160, 280);
            btn.Click += BtnAccept_Click;
            btn.Size = image.Size;
            btn.BackgroundImage = image;
            return btn;
        }
        */

        // ******Animal******
        protected async Task LoadAnimalBaseAsync()
        {
            Statement = "Animal";
            var panel = Controls["Panel"] as UserControl;
            panel.Controls.Clear();
            ErrorProvider errorProvider = new ErrorProvider();

            Label lName = await Desiner.MakeLabelAsync("Name:", 0);
            TextBox tbName = await Desiner.MakeTextBoxAsync("Name", 0);
            tbName.Validating += (sender, e) => TBReq_Validating(sender, e, errorProvider);

            Label lAge = await Desiner.MakeLabelAsync("Age:", 1);
            TextBox tbAge = await Desiner.MakeTextBoxAsync("Age", 1);
            tbAge.Validating += (sender, e) => TBNum_Validating(sender, e, errorProvider);

            Label lKind = await Desiner.MakeLabelAsync("Kind:", 2);
            ComboBox boxKind = new ComboBox
            {
                Name = "Kind",
                Height = 20,
                Location = new Point(110, 100)
            };
            FillBoxWithEntities(boxKind);
            if (boxKind.Items.Count > 0)
                boxKind.Text = boxKind.Items[0].ToString();
            boxKind.Validating += (sender, e) => Box_Validating(sender, e, errorProvider);

            Label lZooKeeper = await Desiner.MakeLabelAsync("Keeper:", 3);
            ComboBox boxZooKeeper = new ComboBox
            {
                Name = "ZooKeeper",
                Height = 20,
                Location = new Point(110, 140)
            };
            FillBoxWithEntities(boxZooKeeper);
            if (boxZooKeeper.Items.Count > 0)
                boxZooKeeper.Text = boxZooKeeper.Items[0].ToString();
            boxZooKeeper.Validating += (sender, e) => Box_Validating(sender, e, errorProvider);

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

            panel.Controls.AddRange(new Control[] { 
                lName, lZooKeeper, lKind, lAge, lPredator,
                tbName, tbAge, btnFood, rbYes, rbNot,
                boxZooKeeper, boxKind
            });
        }
        protected async virtual Task LoadAnimalAsync() => await LoadAnimalBaseAsync();
        private Animal GetAnimal()
        {
            var panel = Controls["Panel"] as UserControl;
            return new Animal
            {
                Name = panel.Controls["Name"].Text,
                Age = Int32.Parse(panel.Controls["Age"].Text),
                KindId = ((panel.Controls["Kind"] as ComboBox).SelectedItem as Kind).Id,
                ZooKeeperId = ((panel.Controls["ZooKeeper"] as ComboBox).SelectedItem as ZooKeeper).Id,
                IsPredator = (panel.Controls["Yes"] as RadioButton).Checked ? true : false
            };
        }
        
        // ******Kind******
        protected async Task LoadKindBaseAsync()
        {
            Statement = "Kind";
            var panel = Controls["Panel"] as UserControl;
            panel.Controls.Clear();
            ErrorProvider errorProvider = new ErrorProvider();

            Label lName = await Desiner.MakeLabelAsync("Name: ", 0);
            TextBox tbName = await Desiner.MakeTextBoxAsync("Name", 0);
            tbName.Validating += (sender, e) => TBReq_Validating(sender, e, errorProvider);
           
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

            panel.Controls.AddRange(new Control[] { 
                lName, lDescription, lСonditions, lBlood,
                tbName, tbDescription, tbСonditions,
                rbWorm, rbCold
            });
        }
        protected async virtual Task LoadKindAsync() => await LoadKindBaseAsync();
        private Kind GetKind()
        {
            var panel = Controls["Panel"] as UserControl;
            return new Kind
            {
                Name = panel.Controls["Name"].Text,
                IsWormBlooded = (panel.Controls["Worm"] as RadioButton).Checked ? true : false,
                Description = panel.Controls["Description"].Text,
                Сonditions = panel.Controls["Сonditions"].Text,
            };
        }

        // ******Employee******
        protected async Task LoadEmployeeBaseAsync()
        {
            Statement = "Employee";
            var panel = Controls["Panel"] as UserControl;
            panel.Controls.Clear();
            ErrorProvider errorProvider = new ErrorProvider();
            ErrorProvider telEP = new ErrorProvider();
            CancelEventArgs telCancel = new CancelEventArgs();

            Label lName = await Desiner.MakeLabelAsync("Name:", 0);
            TextBox tbName = await Desiner.MakeTextBoxAsync("Name", 0);
            tbName.Validating += (sender, e) => TBReq_Validating(sender, e, errorProvider);

            Label lFamily = await Desiner.MakeLabelAsync("Family:", 1);
            ComboBox boxFamily = new ComboBox
            {
                Name = "Family",
                Height = 20,
                Location = new Point(110, 60)
            };
            boxFamily.Items.AddRange(new[] { "Male", "Female", "Unknown" });
            boxFamily.SelectedItem = "Unknown";
            boxFamily.Validating += (sender, e) => Box_Validating(sender, e, errorProvider);

            Label lSalary = await Desiner.MakeLabelAsync("Salary:", 2);
            TextBox tbSalary = await Desiner.MakeTextBoxAsync("Salary", 2);
            tbSalary.Validating += (sender, e) => TBNum_Validating(sender, e, errorProvider);

            Label lTel = await Desiner.MakeLabelAsync("Telephone:", 3);
            TextBox tbTel = await Desiner.MakeTextBoxAsync("Telephone", 3);
            tbTel.Validating += (sender, e) => TBNum_Validating(sender, e, errorProvider);
            tbTel.LostFocus += (sender, e) => TbTel_Leave(sender, telCancel, telEP);

            Label lAddress = await Desiner.MakeLabelAsync("Address:", 4);
            TextBox tbAddress = await Desiner.MakeTextBoxAsync("Address", 4);
            tbAddress.Height = 50;
            tbAddress.Multiline = true;
            tbAddress.WordWrap = true;
            tbAddress.ScrollBars = ScrollBars.Both;
            tbAddress.Validating += (sender, e) => TBReq_Validating(sender, e, errorProvider);

            panel.Controls.AddRange(new Control[] { 
                lName, lFamily, lSalary, lTel, lAddress,
                tbName, tbSalary, tbTel, tbAddress, boxFamily
            });
        }
        protected async virtual Task LoadEmployeeAsync() => await LoadEmployeeBaseAsync();
        private ZooKeeper GetEmployee()
        {
            var panel = Controls["Panel"] as UserControl;
            return new ZooKeeper
            {
                Name = panel.Controls["Name"].Text,
                Family = new Func<Person.family>(() => {
                    if (panel.Controls["Family"].Text == "Male")
                        return Person.family.male;
                    else if (panel.Controls["Family"].Text == "Female")
                        return Person.family.female;
                    else
                        return Person.family.unknown;
                })(),
                Salary = Int32.Parse(panel.Controls["Salary"].Text),
                Telephone = panel.Controls["Telephone"].Text,
                Address = panel.Controls["Address"].Text
            };
        }

        // ******Aviary******
        protected async Task LoadAviaryBaseAsync()
        {
            Statement = "Aviary";
            var panel = Controls["Panel"] as UserControl;
            panel.Controls.Clear();
            ErrorProvider errorProvider = new ErrorProvider();

            Label lAddress = await Desiner.MakeLabelAsync("Address:", 0);
            TextBox tbAdress = await Desiner.MakeTextBoxAsync("Address", 0);
            tbAdress.Height = 50;
            tbAdress.Multiline = true;
            tbAdress.WordWrap = true;
            tbAdress.ScrollBars = ScrollBars.Both;
            tbAdress.Validating += (sender, e) => TBReq_Validating(sender, e, errorProvider);

            Label lSize = await Desiner.MakeLabelAsync("Size", 2);
            TextBox tbSize = await Desiner.MakeTextBoxAsync("Size", 2);
            tbSize.Validating += (sender, e) => TBReq_Validating(sender, e, errorProvider);

            Label lMaxAnimal = await Desiner.MakeLabelAsync("Animals:", 3);
            TextBox tbMaxAnimal = await Desiner.MakeTextBoxAsync("MaxAnimal", 3);
            tbMaxAnimal.Validating += (sender, e) => TBReq_Validating(sender, e, errorProvider);

            Button btnType = await Desiner.MakeButtonAsync("Type", 4);

            panel.Controls.AddRange(new Control[] { 
                btnType, lAddress, lSize, lMaxAnimal,
                tbAdress, tbSize, tbMaxAnimal
            });
        }
        protected async virtual Task LoadAviaryAsync() => await LoadAviaryBaseAsync();
        private Aviary GetAviary()
        {
            var panel = Controls["Panel"] as UserControl;
            return new Aviary
            {
                Address = panel.Controls["Address"].Text,
                MaxAnimalsSize = Int32.Parse(panel.Controls["MaxAnimal"].Text),
            };
        }

        // ******Food******
        protected async Task LoadFoodBaseAsync()
        {
            Statement = "Food";
            var panel = Controls["Panel"] as UserControl;
            panel.Controls.Clear();
            ErrorProvider errorProvider = new ErrorProvider();

            Label lName = await Desiner.MakeLabelAsync("Name:", 0);
            TextBox tbName = await Desiner.MakeTextBoxAsync("Name", 0);
            tbName.Validating += (sender, e) => TBReq_Validating(sender, e, errorProvider);

            Label lAmount = await Desiner.MakeLabelAsync("Amount:", 1);
            TextBox tbAmount = await Desiner.MakeTextBoxAsync("Amount", 1);
            tbAmount.Validating += (sender, e) => TBNum_Validating(sender, e, errorProvider);

            Label lCategory = await Desiner.MakeLabelAsync("Category:", 2);
            ComboBox boxCategory = new ComboBox
            {
                Name = "Category",
                Height = 20,
                Location = new Point(110, 100)
            };
            boxCategory.Items.AddRange(new[] { "Meals", "Fruits", "Vegetables", "Other" });
            boxCategory.SelectedItem = "Other";
            boxCategory.Validating += (sender, e) => Box_Validating(sender, e, errorProvider);

            Label lRot = await Desiner.MakeLabelAsync("Suitability:", 3);
            TextBox tbRot = await Desiner.MakeTextBoxAsync("Suitability", 3);
            tbRot.Validating += (sender, e) => TBNum_Validating(sender, e, errorProvider);

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

            panel.Controls.AddRange(new Control[] { 
                lName, lAmount, lCategory, lFreeze, lRot,
                tbName, tbAmount, tbRot, rbYes, rbNot, boxCategory
            });
        }
        protected async virtual Task LoadFoodAsync() => await LoadFoodBaseAsync();
        private Food GetFood()
        {
            var panel = Controls["Panel"] as UserControl;
            return new Food
            {
                Name = panel.Controls["Name"].Text,
                Amount = Int32.Parse(panel.Controls["Amount"].Text),
                Category = (panel.Controls["Category"] as ComboBox).SelectedText,
                Freeze = (panel.Controls["Yes"] as RadioButton).Checked ? true : false,
                RotAt = DateTime.Parse(DateTime.Now.ToString()).AddDays(int.Parse(panel.Controls["Suitability"].Text.ToString())), 
            };
        }

        // ******Validating******
        private void TbTel_Leave(object sender, CancelEventArgs e, ErrorProvider errorProvider)
        {
            var tb = sender as TextBox;
            if (isFormat(tb.Text))
                return;
            if (TBTel_Validating(sender, e as CancelEventArgs, errorProvider))
                tb.Text = String.Format("{0:(###)###-####}", Int64.Parse(tb.Text));
            else
                Controls["Back"].Focus();
        }
        private bool isFormat(string str)
        {
            if (str.Length != 13)
                return false;
            try
            {

                var _a = str.Split('(');
                var _b = str.Split(')');
                var _c = str.Split('-');
                if (_a[1].Length == 12 &&
                    _b[1].Length == 8 &&
                    _c[1].Length == 4)
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        private void TBNum_Validating(object sender, CancelEventArgs e, ErrorProvider errorProvider)
        {
            TextBox boxName = sender as TextBox;
            if (isFormat(boxName.Text))
                return;
            bool isNumber = boxName.Text.All(x => x >= '0' && x <= '9');

            if (string.IsNullOrWhiteSpace(boxName.Text) || !isNumber)
            {
                e.Cancel = true;
                boxName.Focus();
                errorProvider.SetError(boxName, "Field should be a number");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(boxName, "");
            }
        }
        private bool TBTel_Validating(object sender, CancelEventArgs e, ErrorProvider errorProvider)
        {
            TextBox boxName = sender as TextBox;

            bool isTel = true;
            var str = boxName.Text;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '+')
                    continue;
                if (!(str[i] >= '0' && str[i] <= '9'))
                {
                    isTel = false;
                    break;
                }
            }
            if (str.Length != 10)
                isTel = false;

            var ic = new Icon(SystemIcons.Question, 16, 16).ToBitmap();
            var Hicon = ic.ResizeImage(16, 16).GetHicon();
            errorProvider.Icon = Icon.FromHandle(Hicon);
            
            if (!isTel)
            { 

                e.Cancel = true;
                boxName.Focus();
                errorProvider.SetError(boxName, "Cannot convert to property format.\nFormat should be: 1234567890");
                return false;
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(boxName, "");
                return true;
            }
        }
        private void TBReq_Validating(object sender, CancelEventArgs e, ErrorProvider errorProvider)
        {
            TextBox boxName = sender as TextBox;

            if (string.IsNullOrWhiteSpace(boxName.Text))
            {
                e.Cancel = true;
                boxName.Focus();
                errorProvider.SetError(boxName, "Field should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(boxName, "");
            }
        }
        private void Box_Validating(object sender, CancelEventArgs e, ErrorProvider errorProvider)
        {
            ComboBox box = sender as ComboBox;

            if (string.IsNullOrWhiteSpace(box.Text.ToString()))
            {
                e.Cancel = true;
                box.Focus();
                errorProvider.SetError(box, "Field can not be empty");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(box, "");
            }
        }

        // ******Other******
        protected void FillBoxWithEntities(object sender)
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
        protected abstract void BtnBack_Click(object sender, EventArgs e);
        protected abstract void BtnAcceptAsync_Click(object sender, EventArgs e);
        private void UCAddBase_Load(object sender, EventArgs e)
        {
        }
    }
}
