using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZooFormUI.Database;
using ZooFormUI.UserControls;

namespace ZooFormUI
{
    public partial class UCMain : UserControl
    {
        private static Field<UCMain> _instanse;
        public static UCMain Instanse
        {
            get 
            {
                if (_instanse == null)
                    _instanse = new UCMain();
               return _instanse.getInstance();
            } 
            set => _instanse = value;
        }

        private bool conn;
        //public event EventHandler<EventArgs> DBConnected;
        public UCMain()
        {
            conn = false;
            InitializeComponent();
            MainMenu.Instanse.Size = new Size(301, 401);
            MainMenu.Instanse.Size = new Size(300, 400);
        }
        private async void UCMain_Load(object sender, EventArgs e)
        {
            this.Width = 300;
            this.Height = 400;
            List<Button> buttons = new List<Button>();

            Button btnDBManager = await Desiner.MakeBigButtonAsync("Database", new Point(70, 50));
            ConnectingAnimation(btnDBManager);

            Button btnSettings = await Desiner.MakeBigButtonAsync("About", new Point(70, 50 + 15 * 1 + 80));
            btnSettings.Click += (sender, e) => UCAbout.Instanse.BringToFrontOrCreate();

            Button btnExit = await Desiner.MakeBigButtonAsync("Exit", new Point(70, 50 + 15 * 2 + 80 * 2));
            btnExit.Click += (sender, e) => MainMenu.Instanse.Close();

            this.Controls.AddRange(new Control[] { btnDBManager, btnSettings, btnExit });
        }
        public virtual void OnDBConnected(object sender, EventArgs e, bool _conn)
        {
            conn = _conn;
            if (!conn)
                throw new Exception("Failed when connected to database");
            Controls["Database"].Click += (sender, e) => UCDBManager.Instanse.BringToFrontOrCreate();
        }
        private async void ConnectingAnimation(Button btn)
        {
            int i = 0;
            while (!ZooDbContext.Connected)
            {
                switch (i++)
                {
                    case 0:
                        btn.Text = "Connecting |";
                        await Task.Delay(50);
                        break;
                    case 1:
                        btn.Text = "Connecting /";
                        break;
                    case 2:
                        btn.Text = "Connecting -";
                        break;
                    case 3:
                        btn.Text = "Connecting \\";
                        break;
                    case 4:
                        btn.Text = "Connecting -";
                        break;
                    default:
                        i = 0;
                        break;
                }
                await Task.Delay(50);
            }
            btn.Text = "Database";
        }
        private async Task<Button> MakeBtnAsync(string name, int pos)
        {
            return await Task.Run(() =>
            {
                return new Button
                {
                    Name = name,
                    Text = name,
                    Height = 80,
                    Width = 160,
                    Location = new Point(65, 47 + 95 * pos),
                };
            });
        }
    }
}
