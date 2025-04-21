using NUnit.Framework;
using Asset_Management.Dao;
using Asset_Management.Models;
using Asset_Management.Utils;
using Asset_Management.Exceptions;
using System;
using System.Data.SqlClient;

namespace Unit_Testing
{
    [TestFixture]
    public class AssetUnitTests
    {
        private AssetManagementServiceImpl service;

        [SetUp]
        public void Setup()
        {
            service = new AssetManagementServiceImpl();

            using var conn = DBConnUtil.GetConnection();
            conn.Open();
        }


        [Test]
        public void Asset_Creation()
        {
            int id = new Random().Next(1000, 9999);

            var asset = new Asset( "Unit Test Asset", "Monitor", "MON-" + id, DateTime.Now, "Lab Room 5", "in use", 1);

            bool result = service.AddAsset(asset);
            Assert.IsTrue(result);
        }

        [Test]
        public void Asset_Added_to_Maintenance()
        {
            bool result = service.PerformMaintenance(1, DateTime.Now, "Fan replaced", 1200);
            Assert.IsTrue(result);
        }

        [Test]
        public void Asset_Reservation()
        {
            using (var conn = DBConnUtil.GetConnection())
            {
                conn.Open();
                var assetCheckCmd = new SqlCommand("SELECT COUNT(1) FROM Assets WHERE Asset_Id = 1", conn);
                int assetExists = (int)assetCheckCmd.ExecuteScalar();
                Assert.AreEqual(1, assetExists, "Asset with ID 1 should exist.");

                var employeeCheckCmd = new SqlCommand("SELECT COUNT(1) FROM Employees WHERE Employee_Id = 1", conn);
                int employeeExists = (int)employeeCheckCmd.ExecuteScalar();
                Assert.AreEqual(1, employeeExists, "Employee with ID 1 should exist.");
            }

            bool result = service.ReserveAsset(1, 1, DateTime.Now, DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), "approved");
            Assert.IsTrue(result);
        }


        [Test]
        public void AssetNotFoundException_Check()
        {
            int missingAssetId = 9999;
            DateTime when = DateTime.Now;
            string reason = "Invalid asset";
            decimal cost = 999;

            var ex = Assert.Throws<AssetNotFoundException>(() =>
                service.PerformMaintenance(missingAssetId, when, reason, cost)
            );

            Assert.That(ex.Message, Does.Contain(missingAssetId.ToString()));
        }
    }
}

