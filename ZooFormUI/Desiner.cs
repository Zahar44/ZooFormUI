using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZooFormUI
{
    public static class Desiner
    {
        public static async Task<Label> MakeLabelAsync(string name, int pos)
        {
            return await Task.Run(() => {
                return new Label
                {
                    Location = new Point(20, 20 + 40 * pos),
                    Text = name,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Height = 25,
                    Width = 80
                };
            });
        }
        public static async Task<TextBox> MakeTextBoxAsync(string name, int pos)
        {
            return await Task.Run(() => {
                return new TextBox
                {
                    Name = name,
                    Location = new Point(110, 20 + 40 * pos++),
                    Height = 20,
                    Width = 120
                };
            });
        }
        public static async Task<Button> MakeButtonAsync(string name, int pos)
        {
            return await Task.Run(() => {
                return new Button
                {
                    Location = new Point(100, 20 + 40 * pos++),
                    Text = name,
                    Size = new Size(90, 50)
                };
            });
        }
    }
}
