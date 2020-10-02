using Common.DTO;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.DTO
{
    public class SearchRequestDTO : IRequest<ResponseDTO>
    {
        public string UserName { get; set; }
        public string AlgorithmKey { get; set; }
        public IEnumerable<String> Matrix { get; set; }
        public IEnumerable<String> WordStream {get;set;}
    }

    public class SearchRequestDTOValidator : AbstractValidator<SearchRequestDTO>
    {
        public SearchRequestDTOValidator()
        {
            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} can not be null")
                .NotEmpty().WithMessage("{PropertyName} can not be empty");

            RuleFor(x => x.AlgorithmKey)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} can not be null")
                .NotEmpty().WithMessage("{PropertyName} can not be empty");

            RuleFor(x => x.Matrix)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} can not be null")
                .NotEmpty().WithMessage("{PropertyName} can not be empty")
                .Must(x => x.Count() > 0 && x.Count() < 4096);

            RuleForEach(x => x.Matrix)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("Matrix can't have null values")
                .NotEmpty().WithMessage("Matrix can't have empty values")
                .Must(x => x.Length <= 64).WithMessage("Matrix can't have rows with more than 64 characters");

            RuleFor(x => x.WordStream)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage("{PropertyName} can not be null")
                .NotEmpty().WithMessage("{PropertyName} can not be empty");
        }
    }
}
