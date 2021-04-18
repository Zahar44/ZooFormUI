using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZooFormUI.Library
{
    class ValidateProvider
    {
        private UserControl control;

        private ErrorProvider errorProvider3 = new ErrorProvider(); // regular for TBTel_Validating
        private ErrorProvider errorProvider2 = new ErrorProvider(); // question
        private ErrorProvider errorProvider1 = new ErrorProvider(); // regular
        private CancelEventArgs cancelEventArgs = new CancelEventArgs();
        public ValidateProvider(UserControl _control)
        {
            var ic = new Icon(SystemIcons.Question, 16, 16).ToBitmap();
            var Hicon = ic.ResizeImage(16, 16).GetHicon();
            errorProvider2.Icon = Icon.FromHandle(Hicon);
            control = _control;
        }
        public void TbTel_Leave(Control item) => item.Leave += (sender, e) => _TbTel_Leave(sender, e, errorProvider2); 
        protected void _TbTel_Leave(object sender, EventArgs e, ErrorProvider errorProvider)
        {
            var tb = sender as TextBox;
            if (_TBTel_Validating(sender, cancelEventArgs, errorProvider, "Cannot convert to property format.\nPlease type 10 numbers"))
            {
                try
                {
                    tb.Text = String.Format("{0:(###)###-####}", Int64.Parse(tb.Text));
                }
                catch (Exception)
                {
                }
            }
        }
        protected bool isFormat(string str)
        {
            if(str.Length == 10)
            {
                return !str.All(x => x <= '0' || x >= '9');
            }

            if (str.Length != 13)
                return false;

            try
            {
                var _a = str.Split('(');
                var _b = str.Split(')');
                var _c = str.Split('-');
                if (_a[1].Length == 12 &&
                    _b[1].Length == 8 &&
                    _c[1].Length == 4)
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }
        public void TBNum_Validating(Control item) => item.Validating += (sender, e) => _TBNum_Validating(sender, e, errorProvider1);
        protected void _TBNum_Validating(object sender, CancelEventArgs e, ErrorProvider errorProvider)
        {
            TextBox boxName = sender as TextBox;
            if (isFormat(boxName.Text))
                return;
            bool isNumber = boxName.Text.All(x => x >= '0' && x <= '9' || x == '.');

            if (string.IsNullOrWhiteSpace(boxName.Text) || !isNumber)
            {
                e.Cancel = true;
                boxName.Focus();
                errorProvider.SetError(boxName, "Field should be a number");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(boxName, "");
            }
        }
        public void TBTel_Validating(Control item) => item.Validating += (sender, e) => _TBTel_Validating(sender, e, errorProvider3, "Type correct number please");
        protected bool _TBTel_Validating(object sender, CancelEventArgs e, ErrorProvider errorProvider, string msg)
        {
            TextBox boxName = sender as TextBox;

            bool isTel = isFormat(boxName.Text);

            if (!isTel)
            {
                e.Cancel = true;
                errorProvider3?.SetError(boxName, "");
                errorProvider2?.SetError(boxName, "");
                errorProvider.SetError(boxName, msg);
                return false;
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(boxName, "");
                return true;
            }
        }
        public void TBReq_Validating(Control item) => item.Validating += (sender, e) => _TBReq_Validating(sender, e, errorProvider1);
        protected void _TBReq_Validating(object sender, CancelEventArgs e, ErrorProvider errorProvider)
        {
            TextBox boxName = sender as TextBox;

            if (string.IsNullOrWhiteSpace(boxName.Text))
            {
                e.Cancel = true;
                boxName.Focus();
                errorProvider.SetError(boxName, "Field should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(boxName, "");
            }
        }
        public void Box_Validating(Control item) => item.Validating += (sender, e) => _Box_Validating(sender, e, errorProvider1);
        protected void _Box_Validating(object sender, CancelEventArgs e, ErrorProvider errorProvider)
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
    }
}
