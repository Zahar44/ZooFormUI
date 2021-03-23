using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ZooFormUI
{
    public class Field<T> where T: new()
    {
        private T instance;

        private Field()
        { }

        public T getInstance()
        {
            if (instance == null)
                instance = new T();
            return instance;
        }

        public static implicit operator Field<T>(T v)
        {
            var res = new Field<T>();
            res.instance = v;
            return res;
        }
    }
}
