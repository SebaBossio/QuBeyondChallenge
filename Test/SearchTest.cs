using Common.DTO;
using Common.Enums;
using Core.MediatorHandlers;
using Core.SearchAlgorithms;
using DataAccess;
using DBEntities.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    [TestClass]
    public class SearchTest
    {
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IConfiguration> _configurationMock;
        private CancellationToken _cancellation;
        private Mock<SearchRequestHandler> _searchRequestHandlerMock;

        public SearchTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(x => x.SearchesRepository.Add(It.IsAny<Searches>()));
            _unitOfWorkMock.Setup(x => x.SaveChanges());

            _configurationMock = new Mock<IConfiguration>();

            //var dictionaryMock = new Mock<IEnumerable<IConfigurationSection>>();
            //dictionaryMock.Setup(x => x.ToDictionary<string, string>(It.IsAny<string>(), It.IsAny<string>()))
            //var boyerMooreMock = new Mock<IConfigurationSection>();
            //boyerMooreMock.SetupGet(m => m[It.Is<string>(s => s == "Boyer-Moore")]).Returns("Core.SearchAlgorithms.Implementations.BoyerMoore");
            //var childresMock = new Mock<IConfigurationSection>();
            //childresMock.Setup(s => s.GetChildren()).Returns(new List<IConfigurationSection> { boyerMooreMock.Object });
            //_configurationMock.Setup(x => x.GetSection(It.Is<string>(s => s == "Algorithms"))).Returns(childresMock.Object);

            var mockedAlgorithmsDictionary = new Dictionary<string, string>();
            mockedAlgorithmsDictionary.Add("Boyer-Moore", "Core.SearchAlgorithms.Implementations.BoyerMoore");

            _searchRequestHandlerMock = new Mock<SearchRequestHandler>(_configurationMock.Object, _unitOfWorkMock.Object);
            _searchRequestHandlerMock.Setup(x => x.GetAlgorithmsDictionary()).Returns(mockedAlgorithmsDictionary);
        }

        [TestMethod]
        public async Task TestAlgorithms()
        {
            SearchRequestDTO searchRequestDTO = new SearchRequestDTO()
            { 
                UserName = "Test",
                AlgorithmKey = "Boyer-Moore",
                Matrix = new List<string>()
                {
                    "sagjodmasod", "aojsdtest2odjajd", "aojsajodjosatest1djosadjosoajd", "jastest2jdojosdjod", "jasdojasjdodod"
                },
                WordStream = new List<string>()
                {
                    "test1", "test2"
                }
            };

            var result = await _searchRequestHandlerMock.Object.Handle(searchRequestDTO, _cancellation);

            var expectedResult = new ResponseDTO()
            {
                Status = (int)eStatus.Success,
                Message = null,
                Value = new List<string>()
                {
                    "test2", "test1"
                }
            };

            Assert.AreEqual(expectedResult.Status, result.Status);
            Assert.AreEqual(expectedResult.Message, result.Message);
            CollectionAssert.AreEqual((List<string>)expectedResult.Value, (List<string>)result.Value);
        }
    }
}
