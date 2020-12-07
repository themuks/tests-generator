using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TestsGeneratorLibrary;

namespace Demonstration
{
    internal class Pipeline
    {
        public Task Generate(string destFolder, string[] filenames, int maxPipelineTasks)
        {
            var execOptions = new ExecutionDataflowBlockOptions {MaxDegreeOfParallelism = maxPipelineTasks};
            var linkOptions = new DataflowLinkOptions {PropagateCompletion = true};

            Directory.CreateDirectory(destFolder);

            var loadFile = new TransformBlock<string, string>
            (
                async path => await File.ReadAllTextAsync(path),
                execOptions
            );
            var generateTests = new TransformManyBlock<string, TestUnit>
            (
                async sourceCode => await Task.Run(() => TestsGenerator.GenerateTests(sourceCode)),
                execOptions
            );
            var writeFile = new ActionBlock<TestUnit>
            (
                async filesContent =>
                {
                    await File.WriteAllTextAsync(destFolder + '\\' + filesContent.filename + ".cs",
                        filesContent.sourceCode);
                },
                execOptions
            );

            loadFile.LinkTo(generateTests, linkOptions);
            generateTests.LinkTo(writeFile, linkOptions);

            foreach (var filename in filenames) loadFile.Post(filename);

            loadFile.Complete();
            return writeFile.Completion;
        }
    }
}