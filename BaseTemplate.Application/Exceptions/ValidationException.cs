using FluentValidation.Results;

namespace BaseTemplate.Application.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base("Ocorreram um ou mais erros de validação.")
        {
            Errors = failures
                .GroupBy(f => f.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray());
        }
    }
}
