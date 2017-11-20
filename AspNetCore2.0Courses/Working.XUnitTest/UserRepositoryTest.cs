using Moq;
using System;
using System.Data;
using Working.Models.Repository;
using Xunit;
using Dapper;
using Working.Models.DataModel;
using System.Collections.Generic;
using DotNetCore.Moq.Dapper;
namespace Working.XUnitTest
{
    /// <summary>
    /// �û��ִ�����
    /// </summary>
    [Trait("�ִ���", "UserRepository")]
    public class UserRepositoryTest
    {
        Mock<IWorkingDB> _dbMock;
        UserRepository _userRepository;
        public UserRepositoryTest()
        {
            _dbMock = new Mock<IWorkingDB>();
            _userRepository = new UserRepository(_dbMock.Object);
        }

        #region ��¼����
        /// <summary>
        /// ���Ե�¼����ֵ
        /// </summary>
        [Fact]
        public void Login_Default_Return()
        {
            var list = new List<UserRole>() {
                new UserRole{ ID=1, Name="����ΰ", DepartmentID=1, Password="gsw", RoleID=1, RoleName="Leader", UserName="gsw" }
            };
           _dbMock.Setup(db => db.Query<UserRole>(It.IsAny<string>(), It.IsAny<object>(), null, true, null, null)).Returns(list);
            var userRole = _userRepository.Login("gsw", "gsw");

            Assert.NotNull(userRole);


        }
        /// <summary>
        /// ���Ե�¼�û������������
        /// </summary>
        [Fact]
        public void Login_Null_ThrowException()
        {
            var list = new List<UserRole>();
            _dbMock.Setup(db => db.Query<UserRole>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);
            var exc = Assert.Throws<Exception>(() => { _userRepository.Login("gsw", "gsw"); });

            Assert.Contains("�û������������", exc.Message);
        }
        /// <summary>
        /// ���Ե�¼�û������������
        /// </summary>
        [Fact]
        public void Login_Unkonow_ThrowException()
        {  
            var list = new List<UserRole>();
            _dbMock.Setup(db=>db.Query<UserRole>(It.IsAny<string>(), It.IsAny<object>(), null,true,null,null)).Throws(new Exception("δ֪"));
            var exc = Assert.Throws<Exception>(() => { _userRepository.Login("gsw", "gsw"); });
            Assert.Contains("δ֪", exc.Message);
        }
        #endregion


        /// <summary>
        /// �����쳣���
        /// </summary>
        [Fact]
        public void AddUser_NullUser_ThrowException()
        { 
            var exception =Assert.Throws<Exception>(()=> { _userRepository.AddUser(null); });
            Assert.Contains("��ӵ��û�����ΪNull", exception.Message);
        }

        /// <summary>
        /// �����쳣���
        /// </summary>
        [Fact]
        public void AddUser_Default_ReturnTrue()
        {
            _dbMock.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>(), null, null, null)).Returns(1);       
            var result = _userRepository.AddUser(new User { UserName = "test" });
            Assert.True(result);
        }
    }
}
