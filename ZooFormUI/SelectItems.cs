using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZooFormUI
{
    public partial class SelectItems : Form
    {
        private UCAddBase fromControl;
        private ICollection<object> fromHave;
        private UCAddBase toControl;

        public SelectItems(UCAddBase _fromControl, ICollection<object> _fromHave, UCAddBase _toControl)
        {
            StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            fromControl = _fromControl;
            toControl = _toControl;
            fromHave = _fromHave;
        }

        private async void SelectItems_Load(object sender, EventArgs e)
        {
            Size = new Size(400, 350);
            FormBorderStyle = FormBorderStyle.FixedSingle;

            Label lInfo = await Desiner.MakeLabelAsync("Info", new Point(10, 10));
            lInfo.Text = $"Select all { toControl.Name.ToLower() } witch are suitable for this { fromControl.Name.ToLower() }";
            lInfo.Size = new Size(380, 40);
            lInfo.TextAlign = ContentAlignment.MiddleCenter;

            ListBox lbAll = new ListBox
            {
                Name = "lbAll",
                Size = new Size(100, 200),
                Location = new Point(50, 100),
            };
            lbAll.Items.AddRange(SetLBAll().Cast<object>().ToArray());
            lbAll.DoubleClick += BtnRight_Click;

            Label lAll = await Desiner.MakeLabelAsync(
                $"All { toControl.Name.ToLower() }s",
                new Point(
                    lbAll.Location.X,
                    lbAll.Location.Y - 30)
                );
            lAll.Width = lbAll.Width;
            lAll.TextAlign = ContentAlignment.MiddleCenter;

            ListBox lbHave = new ListBox
            {
                Name = "lbHave",
                Size = new Size(100, 200),
                Location = new Point(250, 100),
            };
            if(fromHave != null)
                lbHave.Items.AddRange(fromHave.ToArray());
            lbHave.DoubleClick += BtnLeft_Click;

            Label lHave = await Desiner.MakeLabelAsync(
                $"{ toControl.Name }s in",
                new Point(
                    lbHave.Location.X,
                    lbHave.Location.Y - 30)
                );
            lHave.Width = lbAll.Width;
            lHave.TextAlign = ContentAlignment.MiddleCenter;

            Button btnRight = await Desiner.MakeButtonAsync("Right",
                new Point(180, 125));
            btnRight.Size = new Size(40, 40);
            btnRight.Text = "";
            btnRight.Image = Image.FromFile(@"D:\projects\ZooFormUI\ZooFormUI\Source\Right-Arrow.png").ResizeImage(40, 40);
            btnRight.Click += BtnRight_Click;

            Button btnLeft = await Desiner.MakeButtonAsync("Left",
                new Point(180, 180));
            btnLeft.Size = new Size(40, 40);
            btnLeft.Text = "";
            btnLeft.Image = Image.FromFile(@"D:\projects\ZooFormUI\ZooFormUI\Source\Left-Arrow.png").ResizeImage(40, 40);
            btnLeft.Click += BtnLeft_Click;

            Button btnAccept = await Desiner.MakeButtonAsync("Accept",
                new Point(180, 235));
            btnAccept.Size = new Size(40, 40);
            btnAccept.Text = "";
            btnAccept.Image = Image.FromFile(@"D:\projects\ZooFormUI\ZooFormUI\Source\Accept_Tick.png").ResizeImage(40, 40);
            btnAccept.Click += BtnAccept_Click;

            Controls.AddRange(new Control[] { lInfo, lbAll, lAll, lbHave, lHave, btnRight, btnLeft, btnAccept });
        }

        private ICollection<object> SetLBAll()
        {
            return toControl.GetAllEntity().Except(fromHave).ToArray();
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            var listBox = Controls["lbHave"] as ListBox;
            fromControl.SetRelationsForEntity(listBox.Items.Cast<object>().ToList());
            Close();
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            var lbAll = Controls["lbAll"] as ListBox;
            var lbHave = Controls["lbHave"] as ListBox;
            if (lbHave.SelectedItem == null)
                return;

            lbAll.Items.Add(lbHave.SelectedItem);
            lbHave.Items.Remove(lbHave.SelectedItem);
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            var lbAll = Controls["lbAll"] as ListBox;
            var lbHave = Controls["lbHave"] as ListBox;
            if (lbAll.SelectedItem == null)
                return;

            lbHave.Items.Add(lbAll.SelectedItem);
            lbAll.Items.Remove(lbAll.SelectedItem);
        }
    }
}
