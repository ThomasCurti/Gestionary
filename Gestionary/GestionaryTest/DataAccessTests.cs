using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;

namespace GestionaryTest
{
    public class DataAccessTests
    {

        private GestionaryWebsite.DataAccess.GestionaryContext _context;

        private GestionaryWebsite.DataAccess.EmployeeRepository _employeeRepository;
        private GestionaryWebsite.DataAccess.StockRepository _stockRepository;
        private GestionaryWebsite.DataAccess.LogRepository _logRepository;

        private void Seed()
        {
            var roles = new[]
            {
                new GestionaryWebsite.DataAccess.EfModels.Roles { Id = 1, Name = "Admin" },
                new GestionaryWebsite.DataAccess.EfModels.Roles { Id = 2, Name = "Employee" },
            };

            var users = new[]
            {
                new GestionaryWebsite.DataAccess.EfModels.Users { Id = 1, PasswordHash = "test", RoleId = 1, Username = "AdminUser"},
                new GestionaryWebsite.DataAccess.EfModels.Users { Id = 2, PasswordHash = "test2", RoleId = 2, Username = "EmployeeUser"},
                new GestionaryWebsite.DataAccess.EfModels.Users { Id = 3, PasswordHash = "test3", RoleId = 1, Username = "AdminUser2"},
                new GestionaryWebsite.DataAccess.EfModels.Users { Id = 4, PasswordHash = "test4", RoleId = 3, Username = "randomUser"},
            };

            var types = new[]
            {
                new GestionaryWebsite.DataAccess.EfModels.Types { Id = 1, Name = "Type1"},
                new GestionaryWebsite.DataAccess.EfModels.Types { Id = 2, Name = "Type2"},
                new GestionaryWebsite.DataAccess.EfModels.Types { Id = 3, Name = "Type3"},
            };

            var items = new[]
            {
                new GestionaryWebsite.DataAccess.EfModels.Items { Id = 1, Name = "ItemName", PicName = "PicName.jpg", Price = 10, Stock = 20, TypeId = 1},
                new GestionaryWebsite.DataAccess.EfModels.Items { Id = 2, Name = "ItemName2", PicName = "PicName2.jpg", Price = 20, Stock = 40, TypeId = 2},
                new GestionaryWebsite.DataAccess.EfModels.Items { Id = 3, Name = "ItemName3", PicName = "PicName3.jpg", Price = 30, Stock = 60, TypeId = 3},
                new GestionaryWebsite.DataAccess.EfModels.Items { Id = 4, Name = "randomItem", PicName = "PicName4.jpg", Price = 40, Stock = 80, TypeId = 4},
            };


            _context.AddRange(users);
            _context.AddRange(items);
            _context.AddRange(roles);
            _context.AddRange(types);
            //can't test views yet, cf. https://github.com/dotnet/EntityFramework.Docs/issues/898
            _context.SaveChanges();
        }


        [SetUp]
        public void Setup()
        {

            //Mock the context -- Using InMemoryDatabase for speed purpose
            var options = new DbContextOptionsBuilder<GestionaryWebsite.DataAccess.GestionaryContext>()
                                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                                .Options;

            _context = new GestionaryWebsite.DataAccess.GestionaryContext(options);

            Seed();

            //Mock the ILogger
            var loggerEmployee = Mock.Of<ILogger<GestionaryWebsite.DataAccess.EmployeeRepository>>();
            var loggerStock = Mock.Of<ILogger<GestionaryWebsite.DataAccess.StockRepository>>();
            var loggerLogs = Mock.Of<ILogger<GestionaryWebsite.DataAccess.LogRepository>>();

            //Create the Automapper -- No need to mock it, we want to create a real AutoMapper with the GestionnaryWebsite's AutomapperProfile
            var mapperProfile = new GestionaryWebsite.DataAccess.AutomapperProfiles();
            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(mapperProfile));
            var mapper = new Mapper(mapperConfig);

            //Create Repositories
            _employeeRepository = new GestionaryWebsite.DataAccess.EmployeeRepository(_context, loggerEmployee, mapper);
            _stockRepository = new GestionaryWebsite.DataAccess.StockRepository(_context, loggerStock, mapper);
            _logRepository = new GestionaryWebsite.DataAccess.LogRepository(_context, loggerLogs, mapper);
        }

        [TearDown]
        public void Cleanup()
        {
            _employeeRepository = null;
            _stockRepository = null;
            _logRepository = null;
            _context.Dispose();
            _context = null;
        }

        [Test]
        public void GetAllEmployees()
        {
            var res = _employeeRepository.Get().Result.ToList();
            Assert.AreEqual(4, res.Count);
        }

        [Test]
        public void GetSpecificEmployee()
        {
            var employees = _employeeRepository.Get().Result.ToList();
            var res = employees.Where(employee => employee.Id == 1).FirstOrDefault();
            Assert.AreEqual(1, res.Id);
            Assert.AreEqual("test", res.PasswordHash);
            Assert.AreEqual(1, res.RoleId);
            Assert.AreEqual("AdminUser", res.Username);
        }

        [Test]
        public void InsertEmployee()
        {
            var emp = new GestionaryWebsite.Dbo.ClientUsers { Id = 5, PasswordHash = "testhash", RoleId = 2, Username = "usernameTest" };
            var res = _employeeRepository.Insert(emp).Result;
            Assert.AreEqual(5, res.Id);
            Assert.AreEqual("testhash", res.PasswordHash);
            Assert.AreEqual(2, res.RoleId);
            Assert.AreEqual("usernameTest", res.Username);
        }

        [Test]
        public void RemoveEmployee()
        {
            var res = _employeeRepository.Delete((long)2).Result;
            Assert.AreEqual(true, res);

            var res2 = _context.Users.Find((long)2);
            Assert.IsNull(res2);
        }

        [Test]
        public void RemoveEmployeeWithWrongId()
        {
            var res = _employeeRepository.Delete((long)6).Result;
            Assert.AreEqual(false, res);

            var res2 = _context.Users.Find((long)1);
            Assert.IsNotNull(res2);
            res2 = _context.Users.Find((long)2);
            Assert.IsNotNull(res2);
            res2 = _context.Users.Find((long)3);
            Assert.IsNotNull(res2);
            res2 = _context.Users.Find((long)4);
            Assert.IsNotNull(res2);
        }

        [Test]
        public void GetItems()
        {
            var res = _stockRepository.Get().Result.ToList();
            Assert.AreEqual(4, res.Count);
        }

        [Test]
        public void GetSpecificItem()
        {
            var items = _stockRepository.Get().Result.ToList();
            var res = items.Where(item => item.Id == 1).FirstOrDefault();
            Assert.AreEqual(1, res.Id);
            Assert.AreEqual("ItemName", res.Name);
            Assert.AreEqual("PicName.jpg", res.PicName);
            Assert.AreEqual(10, res.Price);
            Assert.AreEqual(20, res.Stock);
            Assert.AreEqual(1, res.TypeId);
        }

        [Test]
        public void InsertItem()
        {
            var item = new GestionaryWebsite.Dbo.ClientItems { Id = 5, Name = "ItemName", PicName = "PicName.jpg", Price = 10, Stock = 20, TypeId = 1 };
            var res = _stockRepository.Insert(item).Result;
            Assert.AreEqual(5, res.Id);
            Assert.AreEqual("ItemName", res.Name);
            Assert.AreEqual("PicName.jpg", res.PicName);
            Assert.AreEqual(10, res.Price);
            Assert.AreEqual(20, res.Stock);
            Assert.AreEqual(1, res.TypeId);
        }

        [Test]
        public void UpdateSpecificItem()
        {
            var item = new GestionaryWebsite.Dbo.ClientItems { Id = 1, Name = "ItemName", PicName = "PicName.jpg", Price = 10, Stock = 20, TypeId = 1 };
            var updated = _stockRepository.Update(item).Result;

            var res = _context.Items.Find((long)1);
            Assert.AreEqual(1, res.Id);
            Assert.AreEqual("ItemName", res.Name);
            Assert.AreEqual("PicName.jpg", res.PicName);
            Assert.AreEqual(10, res.Price);
            Assert.AreEqual(20, res.Stock);
            Assert.AreEqual(1, res.TypeId);
        }

        [Test]
        public void UpdateSpecificItemWithWrongId()
        {
            var item = new GestionaryWebsite.Dbo.ClientItems { Id = 7, Name = "ItemName", PicName = "PicName.jpg", Price = 10, Stock = 20, TypeId = 1 };
            var updated = _stockRepository.Update(item).Result;

            Assert.AreEqual(null, updated);
        }

        [Test]
        public void RemoveItem()
        {
            var res = _stockRepository.Delete((long)2).Result;
            Assert.AreEqual(true, res);

            var res2 = _context.Items.Find((long)2);
            Assert.IsNull(res2);
        }

        [Test]
        public void RemoveItemWithWrongId()
        {
            var res = _stockRepository.Delete((long)7).Result;
            Assert.AreEqual(false, res);

            var res2 = _context.Items.Find((long)1);
            Assert.IsNotNull(res2);
            res2 = _context.Items.Find((long)2);
            Assert.IsNotNull(res2);
            res2 = _context.Items.Find((long)3);
            Assert.IsNotNull(res2);
            res2 = _context.Items.Find((long)4);
            Assert.IsNotNull(res2);
        }

        [Test]
        public void InsertLog()
        {
            var log = new GestionaryWebsite.Dbo.ClientLogs { Class = "class", Date = DateTime.Now, Id = 1, Logtype = "LogType", Message = "Message", Type = "type" };

            var res = _logRepository.Insert(log);
            Assert.IsNotNull(res);

            var res2 = _context.Logs.Find((long)1);
            Assert.IsNotNull(res2);
        }

    }
}