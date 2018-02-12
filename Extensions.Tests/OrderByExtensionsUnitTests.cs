namespace Extensions.Tests {

    #region Usings
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    #endregion

    [TestClass]
    public class OrderByExtensionsUnitTests {

        [TestMethod]
        public void Test_Between() {
            var persons = GetPersons();

            var carsQueryable = persons.SelectMany(x => x.Cars)
                .AsQueryable();

            var betweenCars = carsQueryable
                .Between(x => x.Price, 10001m, 15000m)
                .OrderBy(x => x.Person.Name)
                .ToList();

            Assert.AreEqual(expected: 2, actual: betweenCars.Count);
            Assert.AreEqual(expected: "FakeName1", actual: betweenCars[0].Person.Name);
            Assert.AreEqual(expected: "FakeName2", actual: betweenCars[1].Person.Name);

            betweenCars = carsQueryable
                .Between(x => x.Price, 10002m, 15000m)
                .OrderBy(x => x.Person.Name)
                .ToList();

            Assert.AreEqual(expected: 0, actual: betweenCars.Count);
        }

        [TestMethod]
        public void Test_OrderBy_Property_Names() {
            var persons = GetPersons();

            var carsQueryable = persons.SelectMany(x => x.Cars)
                .AsQueryable();

            var sortedOrderByCars = carsQueryable
                .OrderBy("Make")
                .ToList();

            Assert.AreEqual(expected: 4, actual: sortedOrderByCars.Count);
            Assert.AreEqual(expected: "FakeMake1", actual: sortedOrderByCars[0].Make);
            Assert.AreEqual(expected: "FakeMake1", actual: sortedOrderByCars[1].Make);
            Assert.AreEqual(expected: "FakeMake2", actual: sortedOrderByCars[2].Make);
            Assert.AreEqual(expected: "FakeMake2", actual: sortedOrderByCars[3].Make);

        }

        [TestMethod]
        public void Test_OrderBy_Parent_Property_Names() {
            var persons = GetPersons();

            var carsQueryable = persons.SelectMany(x => x.Cars)
                .AsQueryable();

            var sortedOrderByCars = carsQueryable
                .OrderBy("Person.Name")
                .ToList();

            Assert.AreEqual(expected: 4, actual: sortedOrderByCars.Count);
            Assert.AreEqual(expected: "FakeName1", actual: sortedOrderByCars[0].Person.Name);
            Assert.AreEqual(expected: "FakeName1", actual: sortedOrderByCars[1].Person.Name);
            Assert.AreEqual(expected: "FakeName2", actual: sortedOrderByCars[2].Person.Name);
            Assert.AreEqual(expected: "FakeName2", actual: sortedOrderByCars[3].Person.Name);

        }

        [TestMethod]
        public void Test_OrderByDescending_Property_Names() {
            var persons = GetPersons();

            var carsQueryable = persons.SelectMany(x => x.Cars)
                .AsQueryable();

            var sortedOrderByCars = carsQueryable
                .OrderByDescending("Make")
                .ToList();

            Assert.AreEqual(expected: 4, actual: sortedOrderByCars.Count);
            Assert.AreEqual(expected: "FakeMake2", actual: sortedOrderByCars[0].Make);
            Assert.AreEqual(expected: "FakeMake2", actual: sortedOrderByCars[1].Make);
            Assert.AreEqual(expected: "FakeMake1", actual: sortedOrderByCars[2].Make);
            Assert.AreEqual(expected: "FakeMake1", actual: sortedOrderByCars[3].Make);

        }

        [TestMethod]
        public void Test_OrderByDescending_Parent_Property_Names() {
            var persons = GetPersons();

            var carsQueryable = persons.SelectMany(x => x.Cars)
                .AsQueryable();

            var sortedOrderByCars = carsQueryable
                .OrderByDescending("Person.Name")
                .ToList();

            Assert.AreEqual(expected: 4, actual: sortedOrderByCars.Count);
            Assert.AreEqual(expected: "FakeName2", actual: sortedOrderByCars[0].Person.Name);
            Assert.AreEqual(expected: "FakeName2", actual: sortedOrderByCars[1].Person.Name);
            Assert.AreEqual(expected: "FakeName1", actual: sortedOrderByCars[2].Person.Name);
            Assert.AreEqual(expected: "FakeName1", actual: sortedOrderByCars[3].Person.Name);

        }

        [TestMethod]
        public void Test_OrderBy_IEnumerable_Ascending_Property_Names() {
            var persons = GetPersons();

            var cars = persons.SelectMany(x => x.Cars).ToList();
            var carsSortedByMake = cars
                .OrderBy("Make", "ASC");

            var carsSortedByMakeList = carsSortedByMake.ToList();

            Assert.AreEqual(expected: 4, actual: carsSortedByMakeList.Count);
            Assert.AreEqual(expected: "FakeMake1", actual: carsSortedByMakeList[0].Make);
            Assert.AreEqual(expected: "FakeMake1", actual: carsSortedByMakeList[1].Make);
            Assert.AreEqual(expected: "FakeMake2", actual: carsSortedByMakeList[2].Make);
            Assert.AreEqual(expected: "FakeMake2", actual: carsSortedByMakeList[3].Make);
        }

        [TestMethod]
        public void Test_OrderBy_IEnumerable_Descending_Property_Names() {
            var persons = GetPersons();

            var cars = persons.SelectMany(x => x.Cars).ToList();
            var carsSortedByMake = cars
                .OrderBy("Make", "DESC");

            var carsSortedByMakeList = carsSortedByMake.ToList();

            Assert.AreEqual(expected: 4, actual: carsSortedByMakeList.Count);
            Assert.AreEqual(expected: "FakeMake2", actual: carsSortedByMakeList[0].Make);
            Assert.AreEqual(expected: "FakeMake2", actual: carsSortedByMakeList[1].Make);
            Assert.AreEqual(expected: "FakeMake1", actual: carsSortedByMakeList[2].Make);
            Assert.AreEqual(expected: "FakeMake1", actual: carsSortedByMakeList[3].Make);
        }

        [TestMethod]
        public void Test_ThenBy_Property_Names() {
            var persons = GetPersons();

            var carsQueryable = persons.SelectMany(x => x.Cars)
                .AsQueryable();

            var sortedOrderByCars = carsQueryable
                .OrderBy("Make")
                .ThenBy("Year")
                .ToList();

            Assert.AreEqual(expected: 4, actual: sortedOrderByCars.Count);
            Assert.AreEqual(expected: "FakeMake1", actual: sortedOrderByCars[0].Make);
            Assert.AreEqual(expected: 2017, actual: sortedOrderByCars[0].Year);
            Assert.AreEqual(expected: "FakeMake1", actual: sortedOrderByCars[1].Make);
            Assert.AreEqual(expected: 2018, actual: sortedOrderByCars[1].Year);
            Assert.AreEqual(expected: "FakeMake2", actual: sortedOrderByCars[2].Make);
            Assert.AreEqual(expected: 2017, actual: sortedOrderByCars[2].Year);
            Assert.AreEqual(expected: "FakeMake2", actual: sortedOrderByCars[3].Make);
            Assert.AreEqual(expected: 2018, actual: sortedOrderByCars[3].Year);

        }

        [TestMethod]
        public void Test_ThenByDescending_Property_Names() {
            var persons = GetPersons();

            var carsQueryable = persons.SelectMany(x => x.Cars)
                .AsQueryable();

            var sortedThenByDescendingCarsYear = carsQueryable
                .OrderBy(x => x.Make)
                .ThenByDescending(x => x.Year)
                .ToList();

            Assert.AreEqual(expected: 4, actual: sortedThenByDescendingCarsYear.Count);
            Assert.AreEqual(expected: "FakeMake1", actual: sortedThenByDescendingCarsYear[0].Make);
            Assert.AreEqual(expected: 2018, actual: sortedThenByDescendingCarsYear[0].Year);
            Assert.AreEqual(expected: "FakeMake1", actual: sortedThenByDescendingCarsYear[1].Make);
            Assert.AreEqual(expected: 2017, actual: sortedThenByDescendingCarsYear[1].Year);
            Assert.AreEqual(expected: "FakeMake2", actual: sortedThenByDescendingCarsYear[2].Make);
            Assert.AreEqual(expected: 2018, actual: sortedThenByDescendingCarsYear[2].Year);
            Assert.AreEqual(expected: "FakeMake2", actual: sortedThenByDescendingCarsYear[3].Make);
            Assert.AreEqual(expected: 2017, actual: sortedThenByDescendingCarsYear[3].Year);

        }

        private List<Person> GetPersons() {

            var persons = new List<Person>();

            var person1 = new Person {
                Name = "FakeName1",
                Cars = new List<Car>() {
                     new Car {
                          Make = "FakeMake1",
                          Model = "FakeModel1",
                          Price = 10001m,
                          Year = 2017
                    },
                     new Car {
                          Make = "FakeMake2",
                          Model = "FakeModel2",
                          Price = 20002m,
                          Year = 2018
                    }
                }
            };

            person1.Cars.Each(x => x.Person = person1);

            var person2 = new Person {
                Name = "FakeName2",
                Cars = new List<Car>() {
                     new Car {
                          Make = "FakeMake1",
                          Model = "FakeModel1",
                          Price = 10001m,
                          Year = 2018
                    },
                     new Car {
                          Make = "FakeMake2",
                          Model = "FakeModel2",
                          Price = 20002m,
                          Year = 2017
                    }
                }
            };

            person2.Cars.Each(x => x.Person = person2);

            persons.Add(person1);
            persons.Add(person2);
            return persons;
        }

    }

    class Person {
        public string Name { get; set; }
        public List<Car> Cars { get; set; }
    }

    class Car {
        public Person Person { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }
    }

}
