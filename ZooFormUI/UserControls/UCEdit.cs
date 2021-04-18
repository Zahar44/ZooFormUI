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

namespace ZooFormUI
{
    public partial class UCEdit : UCAdd
    {
        private static Field<Panel> _panel;

        private static Field<UCEdit> _instanse;

        private static object _entity;

        public static new UCEdit Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCEdit();
                return _instanse.getInstance();
            }
            set => _instanse = value;
        }
        public static new Panel Panel
        {
            get
            {
                if (_panel == null)
                    _panel = new Panel();
                return _panel.getInstance();
            }
            set => _panel = value;
        }

        public object Entity {
            get => _entity;
            set 
            {
                _entity = value;
                LastAddedControl?.Refresh(_entity);
            }
        }

        public UCEdit()
        {
            InitializeComponent();
        }
        protected override void BtnBack_Click(object sender, EventArgs e) => _ = UCFind.Instanse.BringToFrontOrCreateAsync();
        protected override void BtnAccept_Click(object sender, EventArgs e)
        {
            if (!ValidateAll())
                return;

            LastAddedControl.UpdateEntity();

            BtnBack_Click(sender, e);
        }
        private void UCEdit_Load(object sender, EventArgs e)
        {
            Controls.Clear();

            LoadBase();

            Panel = new Panel
            {
                Name = "Panel",
                Size = new Size(300, 270),
                Location = new Point(0, 0),
            };
            Panel.BringToFront();

            Controls.AddRange(new Control[] { Panel });
        }
    }
}
