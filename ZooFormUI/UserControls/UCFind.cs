using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZooFormUI.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ZooFormUI.UserControls;
using System.Reflection;
using System.Collections;

namespace ZooFormUI
{
    public partial class UCFind : UserControl
    {
        private UCAddBase _lastFindedOption;

        private static Field<UCFind> _instanse;
        public static UCFind Instanse
        {
            get
            {
                if (_instanse == null)
                    _instanse = new UCFind();
                return _instanse.getInstance();
            }
            set => _instanse = value;
        }

        private Size _sizeOfPage;
        public Size SizeOfPage
        {
            get => _sizeOfPage;
            set
            {
                _sizeOfPage = value;
                MainMenu.Instanse.Size = new Size((Point)_sizeOfPage);
            }
        }
        private UCAddBase LastFindedOption {
            get => _lastFindedOption;
            set 
            {
                _lastFindedOption = value;
                ShowDataAsync();
            } 
        }

        private ListBox findBox = new ListBox();
        private DataGridView dataGridView = new DataGridView();
        private List<Control> findArea = new List<Control>();
        
        public UCFind()
        {
            SizeOfPage = new Size(500, 400);
            InitializeComponent();
            findBox.Name = "Box";
            findBox.Width = 200;
            findBox.Height = 250;
            findBox.Location = new Point(225, 85);
            findBox.Click += FindBox_Click;
            findBox.MouseDoubleClick += BtnEdit_Click;
            findBox.Font = new Font("", 14, FontStyle.Regular);
            
            dataGridView.Location = new Point(200, 50);
            dataGridView.Size = new Size(900, 275);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LastFindedOption = (UCAddBase)UCAddEmployee.Instanse;
        }
       
        private void UCFind_Load(object sender, EventArgs e)
        {
            _ = UCFind_LoadAsync(this, e);
        }
        private async Task UCFind_LoadAsync(object sender, EventArgs e)
        {
            Button btnExpand = await Desiner.MakeButtonAsync("Expand", new Point(50, 50));
            btnExpand.Size = new Size(120, 60);
            btnExpand.Click += BtnExpand_Click;

            Label lFind = await Desiner.MakeLabelAsync(LastFindedOption.Name, new Point(285, 15));
            lFind.Name = "Find:";
            lFind.Width = 80;
            lFind.TextAlign = ContentAlignment.MiddleCenter;

            TextBox tbFind = await Desiner.MakeTextBoxAsync("Find", new Point(265, 50));
            tbFind.Text = "Sort by name";
            tbFind.Enter += tbFind_Enter;
            tbFind.Leave += tbFind_Leave;
            tbFind.ForeColor = Color.Gray;
            tbFind.TextChanged += BoxFind_TextChanged;

            Button btnEdit = await Desiner.MakeButtonAsync("Edit", new Point(50, 120));
            btnEdit.Size = new Size(120, 60);
            btnEdit.Click += BtnEdit_Click;

            Button btnDelete = await Desiner.MakeButtonAsync("Delete", new Point(50, 193));
            btnDelete.Size = new Size(120, 60);
            btnDelete.Click += BtnDelete_Click;

            Button btnBack = await Desiner.MakeButtonAsync("Back", new Point(50, 263));
            btnBack.Size = new Size(120, 60);
            btnBack.Click += BtnBack_Click;
            
            Button btnLeft = new Button();
            btnLeft.Name = "Left";
            btnLeft.Image = Image.FromFile(@"D:\projects\ZooFormUI\ZooFormUI\Source\Left-Arrow.png").ResizeImage(25, 25);
            btnLeft.Size = new Size(25, 25);
            btnLeft.Location = new Point (lFind.Location.X - 25 - 5, lFind.Location.Y);
            btnLeft.Click += BtnLeft_Click;

            Button btnRight = new Button();
            btnRight.Name = "Right";
            btnRight.Image = Image.FromFile(@"D:\projects\ZooFormUI\ZooFormUI\Source\Right-Arrow.png").ResizeImage(25, 25);
            btnRight.Size = new Size(25, 25);
            btnRight.Location = new Point(lFind.Location.X + lFind.Width + 5, lFind.Location.Y);
            btnRight.Click += BtnRight_Click;

            findArea.AddRange(new Control[]
            {
                tbFind
            });
            Controls.AddRange(new Control[] 
            {   
                btnExpand, tbFind, findBox, 
                btnBack, btnEdit, btnDelete,
                lFind, btnLeft, btnRight
            });
        }

        private void tbFind_Enter(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.Text == "Sort by name")
            {
                tb.ForeColor = Color.Black;
                tb.Text = "";
            }
        }
        private void tbFind_Leave(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb.Text.Length == 0)
            {
                tb.ForeColor = Color.Gray;
                tb.Text = "Sort by name";
            }
        }
        private void BtnExpand_Click(object sender, EventArgs e)
        {
            (sender as Button).Text = "Collapse";
            (sender as Button).Click -= BtnExpand_Click;
            (sender as Button).Click += BtnCollapse_Click;

            _expand();
            ShowDataAsync();

            SizeOfPage = new Size(1150, 400);
            MainMenu.Instanse.ReallyCenterToScreen();
        }
        private void BtnCollapse_Click(object sender, EventArgs e)
        {
            (sender as Button).Text = "Expand";
            (sender as Button).Click -= BtnCollapse_Click;
            (sender as Button).Click += BtnExpand_Click;

            _collapse();
            ShowDataAsync();

            SizeOfPage = new Size(500, 400);
        }
        private void BtnRight_Click(object sender, EventArgs e)
        {
            LastFindedOption = LastFindedOption.Next();
            MainMenu.Instanse.UndoKeyUpdate(_lastFindedOption);
            Controls["Find:"].Text = LastFindedOption.Name;
        }
        private void BtnLeft_Click(object sender, EventArgs e)
        {
            LastFindedOption = LastFindedOption.Prev();
            MainMenu.Instanse.UndoKeyUpdate(_lastFindedOption);
            Controls["Find:"].Text = LastFindedOption.Name;
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            BtnCollapse_Click(Controls["Expand"], e);
            MainMenu.Instanse.UndoKeyKill();
            UCDBManager.Instanse.BringToFrontOrCreate();
        }
        private void BtnEdit_Click(object sender, EventArgs e)
        {
            object item;
            if(!Contains(dataGridView))
            {
                item = findBox.SelectedItem;
            }
            else
            {
                item = EntityFromGrid();
            }

            UCEdit.Instanse.BringToFrontOrCreate(LastFindedOption, item);
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            object item;
            if (!Contains(dataGridView))
            {
                item = findBox.SelectedItem;
            }
            else
            {
                item = EntityFromGrid();
            }

            LastFindedOption.DeleteEntity(item);
            
            ShowDataAsync();
        }

        public void ShowDataAsync()
        {
            Action action1 = () => ShowDataAsyncForDataGridSafe();
            Action action2 = () => ShowDataAsyncForTableSafe();

            if (InvokeRequired)
            {
                Invoke(action1);
                Invoke(action2);
            }
            else
            {
                action1();
                action2();
            }
        }
        private void ShowDataAsyncForDataGridSafe()
        {
            var data = LastFindedOption.GetAllEntity();

            if (data?.Count <= 0)
                return;

            createDataGrid(data);
            fillDataGrid(data);
        }
        private void ShowDataAsyncForTableSafe()
        {
            findBox.BeginUpdate();
            findBox.Items.Clear();
            findBox.Items.AddRange(LastFindedOption.GetAllEntity().ToArray());
            findBox.Refresh();
            findBox.EndUpdate();

            MainMenu.Instanse.UndoKeyActivate();
            MainMenu.Instanse.UndoKeyUpdate(_lastFindedOption);
        }

        private void _collapse()
        {
            Controls.Remove(dataGridView);
            Controls.AddRange(findArea.ToArray());
            Controls.Add(findBox);
        }
        private void _expand()
        {
            Controls.Remove(findBox);
            foreach (var item in findArea)
            {
                Controls.Remove(item);
            }
            Controls.Add(dataGridView);
        }
        private object EntityFromGrid()
        {
            var cells = dataGridView.SelectedCells;
            if(cells.Count > 0)
            {
                return findBox.Items[cells[0].RowIndex];
            }
            return null;
        }
        private void fillDataGrid(ICollection<object> data)
        {
            int i = 0;
            var props = GetPropertyLists(data);
            dataGridView.Rows.Clear();

            foreach (var item in data)
            {
                dataGridView.Rows.Add();
                List<string> row = new List<string>();
                foreach (var propertyInfo in props)
                {
                    var value = propertyInfo.GetValue(item, null);
                    if (value == null) value = "";

                    row.Add(value.ToString());
                }
                dataGridView.Rows[i++].SetValues(row.ToArray());
            }
        }
        private void createDataGrid(ICollection<object> data)
        {
            if (data.Count == 0)
                return;
            var props = GetPropertyLists(data);
            dataGridView.Columns.Clear();

            foreach (var propertyInfo in props)
            {
                dataGridView.Columns.Add(propertyInfo.Name, propertyInfo.Name);
            }
        }
        private List<PropertyInfo> GetPropertyLists(ICollection<object> vs)
        {
            var props = new List<PropertyInfo>(
                vs.FirstOrDefault()
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance));

            List<PropertyInfo> toDelete = new List<PropertyInfo>();
            foreach (var propertyInfo in props)
            {
                var value = propertyInfo.GetValue(vs.First(), null);
                
                if (value == null || value is ICollection || propertyInfo.Name == "Id")
                {
                    toDelete.Add(propertyInfo);
                }
            }

            props = props.Except(toDelete).ToList();
            props = props.OrderBy(x => !x.Name.Contains("Name")).ToList();
            return props.OrderBy(x => x.Name != "Id").ToList();
        }
        private void FindBox_Click(object sender, EventArgs e)
        {
            if (findBox.SelectedItem == null)
                return;
            ListBox lb = sender as ListBox;

            ToolTip toolTip = new ToolTip();
            toolTip.ToolTipIcon = ToolTipIcon.Info;

            toolTip.SetToolTip(lb, _lastFindedOption.InfoToString(lb.SelectedItem));
        }
        private void BoxFind_TextChanged(object sender, EventArgs e)
        {
            List<object> items = new List<object>();
            foreach (var el in findBox.Items)
                items.Add(el);
            items.Sort(delegate (object _a, object _b)
            {
                string a = _a.ToString();
                string b = _b.ToString();
                if (a == null && b == null) return 0;
                else if (a == null) return -1;
                else if (b == null) return 1;
                else
                {
                    int commonA = 0;
                    int commonB = 0;
                    for(int i = 0; i < Math.Min(a.Length, Controls["Find"].Text.Length); i++)
                    {
                        if (a[i].ToString().ToLower() == Controls["Find"].Text[i].ToString().ToLower())
                            commonA++;
                        else
                            commonA--;
                    }
                    //commonA -= Math.Abs(Controls["Find"].Text.Length - a.Length);
                    for (int i = 0; i < Math.Min(b.Length, Controls["Find"].Text.Length); i++)
                    {
                        if (b[i].ToString().ToLower() == Controls["Find"].Text[i].ToString().ToLower())
                            commonB++;
                        else
                            commonB--;
                    }
                    //commonB -= Math.Abs(Controls["Find"].Text.Length - b.Length);
                    return commonB.CompareTo(commonA);
                }
            });
            findBox.Items.Clear();
            foreach (var el in items)
                findBox.Items.Add(el);
        }
    }
}
