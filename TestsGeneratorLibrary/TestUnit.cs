namespace TestsGeneratorLibrary
{
    public class TestUnit
    {
        public TestUnit(string filename, string sourceCode)
        {
            this.filename = filename;
            this.sourceCode = sourceCode;
        }

        public string filename { get; }
        public string sourceCode { get; }
    }
}