using System.Threading.Tasks;

namespace Demonstration
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            await new Pipeline().Generate(".\\Generated Tests", new[]
            {
                "..\\..\\..\\..\\Demonstration\\TestPurposeClass.cs",
                "..\\..\\..\\..\\TestsGeneratorLibrary\\TestsGenerator.cs"
            }, 2);
        }
    }
}