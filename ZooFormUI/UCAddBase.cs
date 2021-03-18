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
    abstract public partial class UCAddBase : UserControl
    {
        protected string Statement { get; set; }
        protected object Entity { get; set; }
        private static int LabelCnt { get; set; } = 0;
        public abstract void Set(string sender, object entity = null);
        public UCAddBase()
        {
            AutoValidate = AutoValidate.Disable;
            InitializeComponent();
        }
        protected void LoadBase()
        {
            this.Width = 300;
            this.Height = 400;
            this.Controls.Clear();

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
            btnAccept.Click += BtnAccept_Click;

            this.Controls.AddRange(new[] { btnAccept, btnBack });
        }
        protected virtual Object GetEntity()
        {
            switch (Statement)
            {
                case "Animal":
                    return GetAnimal();
                case "Employee":
                    return GetEmployee();
                case "Aviary":
                    return GetAviary();
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
        protected void LoadAnimalBase()
        {
            Statement = "Animal";
            LoadBase();
            ErrorProvider errorProvider = new ErrorProvider();

            Label lName = MakeLabel("Name:");
            TextBox tbName = MakeTextBox("Name", 0);
            tbName.Validating += (sender, e) => TBName_Validating(sender, e, errorProvider);

            Label lAge = MakeLabel("Age:");
            TextBox tbAge = MakeTextBox("Age", 1);
            tbAge.Validating += (sender, e) => TBNum_Validating(sender, e, errorProvider);

            Label lKind = MakeLabel("Kind:");
            ComboBox boxKind = new ComboBox
            {
                Name = "Kind",
                Height = 20,
                Location = new Point(110, 100)
            };
            boxKind.Items.AddRange(FillBoxWithEntities(boxKind));
            if (boxKind.Items.Count > 0)
                boxKind.Text = boxKind.Items[0].ToString();
            boxKind.Validating += (sender, e) => Box_Validating(sender, e, errorProvider);

            Label lZooKeeper = MakeLabel("Keeper:");
            ComboBox boxZooKeeper = new ComboBox
            {
                Name = "ZooKeeper",
                Height = 20,
                Location = new Point(110, 140)
            };
            boxZooKeeper.Items.AddRange(FillBoxWithEntities(boxZooKeeper));
            if (boxZooKeeper.Items.Count > 0)
                boxZooKeeper.Text = boxZooKeeper.Items[0].ToString();
            boxZooKeeper.Validating += (sender, e) => Box_Validating(sender, e, errorProvider);

            Label lPredator = MakeLabel("Predator?");
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
            };

            Button btnFood = MakeButton("Food", 5);

            LabelCnt = 0;
            this.Controls.AddRange(new[] { lName, lZooKeeper, lKind, lAge, lPredator });
            this.Controls.AddRange(new[] { tbName, tbAge });
            this.Controls.AddRange(new[] { btnFood });
            this.Controls.AddRange(new[] { rbYes, rbNot });
            this.Controls.AddRange(new[] { boxZooKeeper, boxKind });
        }
        protected virtual void LoadAnimal() => LoadAnimalBase();
        private Animal GetAnimal()
        {
            return new Animal
            {
                Name = Controls["Name"].Text,
                Age = Int32.Parse(Controls["Age"].Text),
                Kind = (Controls["Kind"] as ComboBox).SelectedItem as Kind,
                ZooKeeper = (Controls["ZooKeeper"] as ComboBox).SelectedItem as ZooKeeper,
                IsPredator = (Controls["Yes"] as RadioButton).Checked ? true : false
            };
        }
        
        // ******Employee******
        protected void LoadEmployeeBase()
        {
            Statement = "Employee";
            LoadBase();

            Label labelName = new Label();
            labelName.Location = new Point(20, 20);
            labelName.Text = "Name:";
            labelName.Width = 50;
            TextBox boxName = new TextBox();
            boxName.Name = "Name";
            boxName.Height = 20;
            boxName.Location = new Point(100, 20);

            LabelCnt = 0;
            this.Controls.AddRange(new[] { labelName });
            this.Controls.AddRange(new[] { boxName });
        }
        protected virtual void LoadEmployee() => LoadEmployeeBase();
        private Employee GetEmployee()
        {
            return new Employee
            {
            };
        }

        // ******Aviary******
        protected void LoadAviaryBase()
        {
            Statement = "Aviary";
            LoadBase();
            ErrorProvider errorProvider = new ErrorProvider();

            Label lAdress = MakeLabel("Address:");
            TextBox tbAdress = MakeTextBox("Address", 0);
            tbAdress.Height = 50;
            tbAdress.Multiline = true;
            tbAdress.WordWrap = true;
            tbAdress.ScrollBars = ScrollBars.Both;
            tbAdress.Validating += (sender, e) => TBName_Validating(sender, e, errorProvider);

            Label lNone = MakeLabel("");

            Label lSize = MakeLabel("Size");
            TextBox tbSize = MakeTextBox("Size", 2);
            tbSize.Validating += (sender, e) => TBName_Validating(sender, e, errorProvider);

            Label lMaxAnimal = MakeLabel("Animals:");
            TextBox tbMaxAnimal = MakeTextBox("MaxAnimal", 3);
            tbMaxAnimal.Validating += (sender, e) => TBName_Validating(sender, e, errorProvider);

            Button btnType = MakeButton("Type", 4);

            LabelCnt = 0;
            Controls.AddRange(new[] { btnType });
            Controls.AddRange(new[] { lAdress, lSize, lMaxAnimal });
            Controls.AddRange(new[] { tbAdress, tbSize, tbMaxAnimal });
        }
        protected virtual void LoadAviary() => LoadAviaryBase();
        private Aviary GetAviary()
        {
            return new Aviary
            {
                Address = Controls["Address"].Text,
            };
        }

        // **Make * Area**
        private Label MakeLabel(string name, int pos = -1)
        {
            if (pos == -1)
                pos = LabelCnt;
            return new Label
            {
                Location = new Point(20, 20 + 40 * LabelCnt++),
                Text = name,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = 25,
                Width = 80
            };
        }
        private TextBox MakeTextBox(string name, int pos)
        {
            return new TextBox
            {
                Name = name,
                Location = new Point(110, 20 + 40 * pos++),
                Height = 20,
                Width = 120
            };
        }
        private Button MakeButton(string name, int pos)
        {
            return new Button
            {
                Location = new Point(100, 20 + 40 * pos++),
                Text = name,
                Size = new Size(90, 50)
            };
        }

        // ******Validating******
        private void TBNum_Validating(object sender, CancelEventArgs e, ErrorProvider errorProvider)
        {
            TextBox boxName = sender as TextBox;

            bool isNumber = boxName.Text.All(c => c >= '0' && c <= '9');

            if (string.IsNullOrWhiteSpace(boxName.Text) || !isNumber)
            {
                e.Cancel = true;
                boxName.Focus();
                errorProvider.SetError(boxName, "Age should be a number");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(boxName, "");
            }
        }
        private void TBName_Validating(object sender, CancelEventArgs e, ErrorProvider errorProvider)
        {
            TextBox boxName = sender as TextBox;

            if (string.IsNullOrWhiteSpace(boxName.Text))
            {
                e.Cancel = true;
                boxName.Focus();
                errorProvider.SetError(boxName, "Name should not be left blank!");
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
        protected object[] FillBoxWithEntities(object sender)
        {
            ComboBox box = sender as ComboBox;
            switch (box.Name)
            {
                case "ZooKeeper":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var entities = db.ZooKeepers.ToList();
                        return entities.Cast<object>().ToArray();
                    }
                case "Kind":
                    using (ZooDbContext db = new ZooDbContext())
                    {
                        var entities = db.Kinds.ToList();
                        return entities.Cast<object>().ToArray();
                    }
                default:
                    return null;
            }
            
        }
        protected abstract void BtnBack_Click(object sender, EventArgs e);
        protected abstract void BtnAccept_Click(object sender, EventArgs e);
        private void UCAddBase_Load(object sender, EventArgs e)
        {
        }
    }
}
