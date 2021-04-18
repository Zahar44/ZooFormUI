using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZooFormUI.Database;

namespace ZooFormUI
{
    public partial class Alert : Form
    {
        public Alert()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        private void Alert_Load(object sender, EventArgs e)
        {
            this.Width = 250;
            this.Height = 275;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            Button btnYes = new Button();
            btnYes.Text = "Yes";
            btnYes.Location = new Point(50 - 5, 175);
            btnYes.Size = new Size(75, 40);
            btnYes.Click += (sender, e) => {  DialogResult = DialogResult.Yes; };

            Button btnNot = new Button();
            btnNot.Text = "Not";
            btnNot.Location = new Point(125 - 5, 175);
            btnNot.Size = new Size(75, 40);
            btnNot.Click += (sender, e) => { DialogResult = DialogResult.No; };

            this.Controls.AddRange(new[] { btnYes, btnNot });
        }

        public DialogResult ShowDialog(ZooKeeper zooKeeper)
        {
            Label label = new Label();
            label.Size = new Size(250, 50);
            label.Dock = DockStyle.Top;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Text = "If you deleted " + zooKeeper.FullName + "\nalso will be deleted:";

            ListBox listBox = new ListBox();
            foreach (var animal in zooKeeper.Animals)
                listBox.Items.Add(animal.Name);
            listBox.Size = new Size(150, 125);
            listBox.Location = new Point(50 - 5, 50);

            this.Controls.Add(listBox);
            this.Controls.Add(label);
            return ShowDialog();
        }
    }
}
