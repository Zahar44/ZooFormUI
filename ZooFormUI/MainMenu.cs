using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ZooFormUI
{
    public partial class MainMenu : Form
    {
        private static Field<Panel> _panel;
        
        private static Field<MainMenu> _instanse;

        private bool undoKeyActivated = false;

        private static UCAddBase control;

        private delegate void SafeCallDelegate(Size _size);
        public static MainMenu Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new MainMenu();
                return _instanse.getInstance();
            }
            set => _instanse = value;
        }
        public static Panel Panel
        {
            get
            {
                if (_panel == null)
                    _panel = new Panel();
                return _panel.getInstance();
            }
            set => _panel = value;
        }
        public MainMenu()
        {
            InitializeComponent();
            KeyPreview = true;
        }

        public void UndoKeyActivate()
        {
            if(!undoKeyActivated)
                KeyDown += UndoKey;
            undoKeyActivated = true;
        }
        public void UndoKeyKill()
        {
            KeyDown -= UndoKey;
            undoKeyActivated = false;
        }
        public void UndoKeyUpdate(UCAddBase _control)
        {
            control = _control;
        }

        private void UndoKey(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z && e.Modifiers == Keys.Control)
            {
                control.UndoLastChanges();
            }
        }

        public void ReallyCenterToScreen()
        {
            Screen screen = Screen.FromControl(this);

            Rectangle workingArea = screen.WorkingArea;
            this.Location = new Point()
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };
        }
        public void SetSizeSafe(Size _size)
        {
            Action action = () => { Size = _size; };
            if (this.InvokeRequired)
                Invoke(action);
            else
                action();
        }
        private void MainMenu_Load(object sender, EventArgs e)
        {
            this.Width = 300;
            this.Height = 400;
            FormBorderStyle = FormBorderStyle.FixedSingle;

            _instanse = this;

            Panel.Dock = DockStyle.Fill;
            this.Controls.Add(Panel);
            Panel.Controls.Add(UCMain.Instanse);
        }

        delegate void SetNewSizeCallback(object size);
    }
}
