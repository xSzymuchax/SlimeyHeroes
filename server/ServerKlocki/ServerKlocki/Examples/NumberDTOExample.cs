using Swashbuckle.AspNetCore.Filters;
using ServerKlocki.DTOs;

namespace ServerKlocki.Examples
{
    public class NumberDTOExample : IExamplesProvider<NumberDTO>
    {
        public NumberDTO GetExamples()
        {
            return new NumberDTO { Number= 5 };
        }
    }
}
