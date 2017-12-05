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
    /// ������ִ�����
    /// </summary>
    [Trait("������ִ���", "WorkItemRepository")]
    public class WorkItemRepositoryTest
    {
        /// <summary>
        /// ���ݿ�Mock����
        /// </summary>
        Mock<IWorkingDB> _dbMock;
        /// <summary>
        /// ������ִ�����
        /// </summary>
        IWorkItemRepository _workItemRepository;
        public WorkItemRepositoryTest()
        {
            _dbMock = new Mock<IWorkingDB>();
            _workItemRepository = new WorkItemRepository(_dbMock.Object);
        }
        /// <summary>
        /// ���������û�ID��ѯ
        /// </summary>
        [Fact]
        public void GetWorkItemByYearMonth_Default_Return()
        {
            var list = new List<WorkItem>() { new WorkItem { CreateUserID = 1 } };
            _dbMock.Setup(db => db.Query<WorkItem>(It.IsAny<string>(), It.IsAny<object>(), null, true, null, null)).Returns(list);
            var workitems = _workItemRepository.GetWorkItemByYearMonth(2017, 10, 1);
            Assert.Equal(31, workitems.Count);
        }
        /// <summary>
        /// �׳�δ֪�쳣����
        /// </summary>
        [Fact]
        public void GetWorkItemByYearMonth_ThrowException_ReturnException()
        {
            var list = new List<WorkItem>() { new WorkItem { CreateUserID = 1 } };
            _dbMock.Setup(db => db.Query<WorkItem>(It.IsAny<string>(), It.IsAny<object>(), null, true, null, null)).Throws(new Exception("δ֪�쳣"));
            var exce = Assert.Throws<Exception>(() => _workItemRepository.GetWorkItemByYearMonth(2017, 10, 1));
            Assert.Equal("δ֪�쳣", exce.Message);
        }
        /// <summary>
        /// ��ӹ��������
        /// </summary>
        /// <param name="workItemID">������ID</param>
        /// <param name="returnResult">����ֵ</param>
        [Theory]
        [InlineData(1,1)]
        [InlineData(2,0)]
        public void AddWorkItem_Default_ReturnTrue(int workItemID ,int returnResult)
        {
            var list = new List<WorkItem>() { new WorkItem { CreateUserID = 1 } };
            _dbMock.Setup(db => db.Query<WorkItem>(It.IsAny<string>(), It.IsAny<object>(), null, true, null, null)).Returns(list);



            _dbMock.Setup(db => db.Execute(It.IsAny<string>(), It.IsAny<object>(),null,null,null)).Returns(value:returnResult);

            var result = _workItemRepository.AddWorkItem(new WorkItem { ID = workItemID }, 1);
            Assert.True(result==(workItemID==1));
        }
    }
}
