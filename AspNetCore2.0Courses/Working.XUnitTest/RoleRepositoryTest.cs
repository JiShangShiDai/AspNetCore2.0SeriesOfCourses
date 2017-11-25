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
    /// ��ɫ�ִ�����
    /// </summary>
    [Trait("��ɫ�ִ���", "RoleRepository")]
    public class RoleRepositoryTest
    {
        /// <summary>
        /// ���ݿ�Mock����
        /// </summary>
        Mock<IWorkingDB> _dbMock;
        /// <summary>
        /// ��ɫ�ִ�����
        /// </summary>
        IRoleRepository _roleRepository;
        public RoleRepositoryTest()
        {
            _dbMock = new Mock<IWorkingDB>();
            _roleRepository = new RoleRepository(_dbMock.Object);
        }
        /// <summary>
        /// ������ID��ѯ�û�����
        /// </summary>
        [Fact]
        public void GetRoles_Default_Return()
        {
            var list = new List<Role>() { new Role(),new Role()};
            _dbMock.Setup(db => db.Query<Role>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);
            var roles = _roleRepository.GetRoles();
            Assert.Equal(2,roles.Count);
        }

    }
}
