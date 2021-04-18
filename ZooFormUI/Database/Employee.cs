using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ZooFormUI.Database
{
    public class Employee : Person
    {
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }



        public Employee() : base()
        {
            Salary = 0;
            Address = "";
            Telephone = "(xxx)xxx-xxxx";
        }
    }
}
