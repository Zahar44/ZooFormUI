using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZooFormUI.Database
{
    public class Person
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FName { get; set; }
        public string SName { get; set; }
        public string MName { get; set; }
        public string FullName 
        {
            get
            {
                if (String.IsNullOrWhiteSpace(FName))
                    return "";
                return $"{FName} {SName} {MName}";
            }
            set
            {
                var strs = value.Split(' ');
                try
                {
                    FName = strs[0];
                }
                catch (Exception)
                {
                    FName = "";
                }
                try
                {
                    SName = strs[1];
                }
                catch (Exception)
                {
                    SName = "";
                }
                try
                {
                    MName = strs[2];
                }
                catch (Exception)
                {
                    MName = "";
                }
            }
        }
        public family Family { get; set; }
        public enum family { Male, Female, Unknown }
        public Person()
        {
            FName = "";
            SName = "";
            MName = "";
            Family = family.Unknown;
        }
        public override string ToString() => FullName;
    }
}
