using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ZooFormUI.Database
{
    public class Kind
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsWormBlooded { get; set; }
        public string Description { get; set; }
        public string Сonditions { get; set; }

        public ICollection<AviaryKind> AviaryKind { get; set; }

        public Kind GetCopy()
        {
            return (Kind)this.MemberwiseClone();
            //return new Kind
            //{
            //    Id = this.Id,
            //    Name = this.Name,
            //    IsWormBlooded = this.IsWormBlooded,
            //    Description = this.Description,
            //    Сonditions = this.Сonditions,
            //    AviaryKind = this.AviaryKind,
            //};
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            return GetHashCode() == obj.GetHashCode();
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return Id * Name.GetHashCode();
        }
        public void SetNewValue(Kind other)
        {
            this.Id = other.Id;
            this.Name = other.Name;
            this.IsWormBlooded = other.IsWormBlooded;
            this.Description = other.Description;
            this.Сonditions = other.Сonditions;
            this.AviaryKind = other.AviaryKind;
        }
        public override string ToString() => Name;
    }
}
