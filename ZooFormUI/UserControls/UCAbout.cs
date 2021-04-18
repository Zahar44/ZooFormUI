using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ZooFormUI.UserControls
{
    public partial class UCAbout : UserControl
    {
        private static Field<UserControl> _instanse;
        public static UCAbout Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCAbout();
                return (UCAbout)_instanse.getInstance();
            }
            set => _instanse = value;
        }

        public UCAbout()
        {
            InitializeComponent();
        }

        private void UCAbout_Load(object sender, EventArgs e)
        {
            Label info = new Label();
            info.Size = new Size(270, 250);
            info.Location = new Point(10, 10);
            info.Font = new Font("", 14);

            info.Text = $"    This program is user friendly interfase for interaction to database\n\n" +
                $"    You can easily adding new entities, deleting and even edit existing";

            Button back = Desiner.MakeButton("Back", new Point(100, 290));
            back.Click += (sender, e) => UCMain.Instanse.BringToFrontOrCreate();
            
            Controls.AddRange(new Control[]
            {
                info, back
            });
        }
    }
}
