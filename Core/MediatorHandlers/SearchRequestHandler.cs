using Common.DTO;
using Common.Enums;
using Common.Exceptions;
using Core.SearchAlgorithms;
using DataAccess;
using DataAccess.Repositories;
using DBEntities.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.MediatorHandlers
{
    public class SearchRequestHandler : IRequestHandler<SearchRequestDTO, ResponseDTO>
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public SearchRequestHandler(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public Task<ResponseDTO> Handle(SearchRequestDTO request, CancellationToken cancellationToken)
        {
            ResponseDTO result = new ResponseDTO();
            try
            {
                var dicAlgorithms = GetAlgorithmsDictionary();

                ISearchAlgorithm algorithm = AlgorithmsCreator.GetAlgorithm(dicAlgorithms, request.AlgorithmKey, request.Matrix);
                var algorithmResult = algorithm.Find(request.WordStream);

                _unitOfWork.SearchesRepository.Add(new Searches()
                {
                    UserName = request.UserName,
                    AlgorithmKey = request.AlgorithmKey,
                    Matrix = request.Matrix.Aggregate((x, y) => x + "," + y),
                    WordStream = request.WordStream.Aggregate((x, y) => x + "," + y)
                });

                _unitOfWork.SaveChanges();

                result.Status = (int)eStatus.Success;
                result.Value = algorithmResult;
            }
            catch(WordFinderCustomException ex)
            {
                result.Status = (int)eStatus.Error;
                result.Message = ex.Message;
            }
            catch(Exception ex)
            {
                result.Status = (int)eStatus.Error;
                result.Message = "Unexpected error";
            }

            return Task.FromResult(result);
        }

        public virtual Dictionary<string, string> GetAlgorithmsDictionary()
        {
            return _configuration.GetSection("Algorithms").GetChildren().ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
