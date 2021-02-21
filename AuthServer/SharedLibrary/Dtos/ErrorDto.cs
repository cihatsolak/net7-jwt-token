using System.Collections.Generic;

namespace SharedLibrary.Dtos
{
    public class ErrorDto
    {
        public ErrorDto()
        {
            Errors = new List<string>();
        }

        public ErrorDto(string error, bool isShow = true)
        {
            Errors = new List<string>
            {
                error
            };
            IsShow = isShow;
        }

        public ErrorDto(List<string> errors, bool isShow)
        {
            Errors = errors;
            IsShow = isShow;
        }

        public List<string> Errors
        {
            get; private set;
        }
        public bool IsShow { get; private set; }
    }
}
