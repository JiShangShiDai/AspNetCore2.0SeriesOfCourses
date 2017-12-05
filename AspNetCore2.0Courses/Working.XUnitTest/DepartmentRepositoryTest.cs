using Moq;
using System;
using System.Data;
using Working.Models.Repository;
using Xunit;
using Dapper;
using Working.Models.DataModel;
using System.Collections.Generic;

namespace Working.XUnitTest
{
    /// <summary>
    /// ���Ųִ�����
    /// </summary>
    [Trait("���Ųִ���", "RoleRepository")]
    public class DepartmentRepositoryTest
    {
        /// <summary>
        /// ���ݿ�Mock����
        /// </summary>
        Mock<IWorkingDB> _dbMock;
        /// <summary>
        /// ���Ųִ�����
        /// </summary>
        IDepartmentRepository _departmentRepository;
        public DepartmentRepositoryTest()
        {
            _dbMock = new Mock<IWorkingDB>();
            _departmentRepository = new DepartmentRepository(_dbMock.Object);
        }
        /// <summary>
        /// ������ID��ѯ�и����ŵĲ���
        /// </summary>
        [Fact]
        public void GetAllPDepartment_Default_Return()
        {
            var list = new List<FullDepartment>() { new FullDepartment()};
            _dbMock.Setup(db => db.Query<FullDepartment>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);
            var departments = _departmentRepository.GetAllPDepartment();
            Assert.Single(departments);
        }

    }
}
