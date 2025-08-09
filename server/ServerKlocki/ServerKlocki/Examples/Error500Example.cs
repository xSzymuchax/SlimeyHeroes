using Swashbuckle.AspNetCore.Filters;

namespace ServerKlocki.Examples
{
    public class Error500Example : IExamplesProvider<string>
    {
        public string GetExamples()
        {
            return "Internal server error - something bad happened";
        }
    }
}
