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

            Button btnAccept = new Button();
            btnAccept.Height = 50;
            btnAccept.Width = 90;
            btnAccept.Text = "Accept";
            btnAccept.Click += BtnAccept_Click;
            btnAccept.Location = new Point(50, 280);

            Button btnBack = new Button();
            btnBack.Height = 50;
            btnBack.Width = 90;
            btnBack.Text = "Back";
            btnBack.Click += (sender, e) => { UCDBManager.Instanse.BringToFrontOrCreate(); };
            btnBack.Location = new Point(160, 280);

            this.Controls.AddRange(new[] { btnAccept, btnBack });
        }
        protected void LoadAnimalBase()
        {
            Statement = "Animal";
            LoadBase();
            ErrorProvider errorProvider = new ErrorProvider();

            Label labelName = new Label();
            labelName.Location = new Point(20, 20);
            labelName.Text = "Name:";
            labelName.Width = 50;
            TextBox boxName = new TextBox();

            boxName.Name = "Name";
            boxName.Height = 20;
            boxName.Location = new Point(100, 20);
            boxName.Validating += (sender, e) => {
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
            };

            Label labelZooKeeper = new Label();
            ErrorProvider errorProviderZK = new ErrorProvider();
            labelZooKeeper.Location = new Point(20, 60);
            labelZooKeeper.Text = "ZooKeeper:";
            labelZooKeeper.Width = 50;

            ComboBox boxZooKeeper = new ComboBox();
            boxZooKeeper.Name = "ZooKeeper";
            boxZooKeeper.Height = 20;
            boxZooKeeper.Location = new Point(100, 60);
            boxZooKeeper.Items.AddRange(FillBoxWithZooKeepers());
            if (boxZooKeeper.Items.Count > 0)
                boxZooKeeper.Text = boxZooKeeper.Items[0].ToString();
            boxZooKeeper.Validating += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(boxZooKeeper.Text))
                {
                    e.Cancel = true;
                    boxZooKeeper.Focus();
                    errorProvider.SetError(boxZooKeeper, "Animals can not be without zookeeper");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider.SetError(boxZooKeeper, "");
                }
            };

            this.Controls.AddRange(new[] { labelName, labelZooKeeper });
            this.Controls.AddRange(new[] { boxName });
            this.Controls.AddRange(new[] { boxZooKeeper });
        }
        protected virtual void LoadAnimal() => LoadAnimalBase();
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

            this.Controls.AddRange(new[] { labelName });
            this.Controls.AddRange(new[] { boxName });
        }
        protected virtual void LoadEmployee() => LoadEmployeeBase();
        protected object[] FillBoxWithZooKeepers()
        {
            using (ZooDbContext db = new ZooDbContext())
            {
                var entities = db.ZooKeepers.ToList();
                return entities.Cast<object>().ToArray();
            }
        }
        virtual protected void BtnAccept_Click(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                control.Focus();
                if (!Validate())
                {
                    return;
                }
            }
            switch (Statement)
            {
                case "Animal":
                    BtnAccept_Click_Animal(sender, e);
                    break;
                case "Employee":
                    BtnAccept_Click_Employee(sender, e);
                    break;
                default:
                    throw new Exception("Add: can't find Statement");
            }
        }
        abstract protected void BtnAccept_Click_Animal(object sender, EventArgs e);
        abstract protected void BtnAccept_Click_Employee(object sender, EventArgs e);
        private void UCAddBase_Load(object sender, EventArgs e)
        {
        }
    }
}
