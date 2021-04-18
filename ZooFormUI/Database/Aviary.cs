using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ZooFormUI.Database
{
    public class Aviary
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int MaxAnimalsCount { get; set; }

        public ICollection<AviaryKind> AviaryKinds { get; set; }

        public Aviary()
        {
            Address = "";
            Width = 0;
            Length = 0;
            Height = 0;
            MaxAnimalsCount = 0;
        }
        public Aviary GetCopy() 
        {
            return (Aviary)this.MemberwiseClone();
            //return new Aviary
            //{
            //    Id = this.Id,
            //    Address = this.Address,
            //    Width = this.Width,
            //    Length = this.Length,
            //    Height = this.Height,
            //    MaxAnimalsCount = this.MaxAnimalsCount,
            //    AviaryKinds = this.AviaryKinds,
            //};
        }
        public void SetNewValue(Aviary other)
        {
            this.Id = other.Id;
            this.Address = other.Address;
            this.Width = other.Width;
            this.Length = other.Length;
            this.Height = other.Height;
            this.MaxAnimalsCount = other.MaxAnimalsCount;
            this.AviaryKinds = other.AviaryKinds;
        }
        public override string ToString() => Address;
    }
}
