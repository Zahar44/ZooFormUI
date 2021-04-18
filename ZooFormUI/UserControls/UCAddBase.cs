using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZooFormUI.Database;

namespace ZooFormUI
{
    public abstract partial class UCAddBase : UserControl
    {
        /*
        private PicturePanel MakeBackBtn()
        {
            Bitmap image = new Bitmap(Image.FromFile("..\\..\\..\\Source\\Back_Arrow.png"));
            image = image.ResizeImage(60, 60);
            PicturePanel btn = new PicturePanel();
            btn.Location = new Point(60, 280);
            btn.Click += (sender, e) => { UCDBManager.Instanse.BringToFrontOrCreate(); };
            btn.Size = image.Size;
            btn.BackgroundImage = image;
            return btn;
        }
        private PicturePanel MakeAcceptBtn()
        {
            Bitmap image = new Bitmap(Image.FromFile("..\\..\\..\\Source\\Accept_Tick.png"));
            image = image.ResizeImage(60, 60);
            PicturePanel btn = new PicturePanel();
            btn.Location = new Point(160, 280);
            btn.Click += BtnAccept_Click;
            btn.Size = image.Size;
            btn.BackgroundImage = image;
            return btn;
        }
        */

        protected delegate void LastAction();
        protected LastAction UndoAction;
        protected abstract void UndoDeleting();
        protected abstract void UndoUpdating();
        protected abstract void UndoCreating();
        
        public abstract ICollection<object> GetAllEntity();
        public abstract object GetEntity(int ind);
        public abstract UCAddBase Next();
        public abstract UCAddBase Prev();
        public abstract void CreateEntity();
        public abstract void SetRelationsForEntity(ICollection<object> ts);
        public abstract void UpdateEntity();
        public abstract void DeleteEntity(object entity);
        public virtual void UndoLastChanges() 
        {
            if (UndoAction == null)
                return;
            try
            {
                UndoAction();
                UCFind.Instanse.ShowDataAsync();
                MessageBox.Show($"Successfully undo changes");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Cannot set last relationships");
            }
            UndoAction = null;
        }
        public abstract string InfoToString(object _entity);
        public abstract void Refresh(object entity);
    }
}
