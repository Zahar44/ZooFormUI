using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ZooFormUI.Database;
using ZooFormUI.Repos;

namespace ZooFormUI.Library
{
    class EntityProvider
    {
        private readonly Control from;
        private readonly string statement;
        private readonly IZooDbContext dbContext = new ZooDbContext();
        public EntityProvider(Control control, string _statement)
        {
            from = control;
            statement = _statement;
        }

        private Animal GetAnimal()
        {
            try
            {
                var panel = from.Controls["Panel"] as UserControl;
                return new Animal
                {
                    Name = panel.Controls["Name"].Text,
                    Age = Int32.Parse(panel.Controls["Age"].Text),
                    KindId = ((panel.Controls["Kind"] as ComboBox).SelectedItem as Kind).Id,
                    ZooKeeperId = ((panel.Controls["ZooKeeper"] as ComboBox).SelectedItem as ZooKeeper).Id,
                    IsPredator = (panel.Controls["Yes"] as RadioButton).Checked ? true : false
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private Kind GetKind()
        {
            var panel = from.Controls["Panel"] as UserControl;
            return new Kind
            {
                Name = panel.Controls["Name"].Text,
                IsWormBlooded = (panel.Controls["Worm"] as RadioButton).Checked ? true : false,
                Description = panel.Controls["Description"].Text,
                Сonditions = panel.Controls["Сonditions"].Text,
            };
        }

        private ZooKeeper GetEmployee()
        {
            var panel = from.Controls["Panel"] as UserControl;
            return new ZooKeeper
            {
                Name = panel.Controls["Name"].Text,
                Family = new Func<Person.family>(() => {
                    if (panel.Controls["Family"].Text == "Male")
                        return Person.family.male;
                    else if (panel.Controls["Family"].Text == "Female")
                        return Person.family.female;
                    else
                        return Person.family.unknown;
                })(),
                Salary = Int32.Parse(panel.Controls["Salary"].Text),
                Telephone = panel.Controls["Telephone"].Text,
                Address = panel.Controls["Address"].Text
            };
        }

        private Aviary GetAviary()
        {
            var panel = from.Controls["Panel"] as UserControl;
            return new Aviary
            {
                Address = panel.Controls["Address"].Text,
                MaxAnimalsSize = Int32.Parse(panel.Controls["MaxAnimal"].Text),
            };
        }

        private Food GetFood()
        {
            var panel = from.Controls["Panel"] as UserControl;
            return new Food
            {
                Name = panel.Controls["Name"].Text,
                Amount = Int32.Parse(panel.Controls["Amount"].Text),
                Category = (panel.Controls["Category"] as ComboBox).SelectedText,
                Freeze = (panel.Controls["Yes"] as RadioButton).Checked ? true : false,
                RotAt = DateTime.Parse(DateTime.Now.ToString()).AddDays(double.Parse(panel.Controls["Suitability"].Text.ToString())),
            };
        }

        public object GetRepository()
        {
            switch (statement)
            {
                case "Animal":
                    return new AnimalRepository(dbContext);
                case "Employee":
                    return new ZooKeeperRepository(dbContext);
                case "Aviary":
                    return new AviaryRepository(dbContext);
                case "Food":
                    return new FoodRepository(dbContext);
                case "Kind":
                    return new KindRepository(dbContext);
                default:
                    return null;
            }
        }
        public object GetEntity()
        {
            switch (statement)
            {
                case "Animal":
                    return GetAnimal();
                case "Employee":
                    return GetEmployee();
                case "Aviary":
                    return GetAviary();
                case "Food":
                    return GetFood();
                case "Kind":
                    return GetKind();
                default:
                    return null;
            }
        }

    }
}
