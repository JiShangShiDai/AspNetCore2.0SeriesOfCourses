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
        /// ������ID��ѯ�û�����
        /// </summary>
        [Fact]
        public void GetWorkItemByYearMonth_Default_Return()
        {
            var list = new List<WorkItem>() { new WorkItem { CreateUserID=1 } };
            _dbMock.Setup(db => db.Query<WorkItem>(It.IsAny<string>(),It.IsAny<object>(),null,true, null, null)).Returns(list);
            var workitems = _workItemRepository.GetWorkItemByYearMonth(2017,10,1);
            Assert.Equal(31,workitems.Count);
        }

    }
}
