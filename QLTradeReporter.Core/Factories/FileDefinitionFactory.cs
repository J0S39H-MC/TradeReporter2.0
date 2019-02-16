using QLTradeReporter.Core.FileDefinitions;
using QLTradeReporter.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTradeReporter.Core.Factories
{
    public static class FileDefinitionFactory
    {
        public static IDelimitedFileDefinition GetFileDefinition(FileType fileType)
        {
            IDelimitedFileDefinition fileProcessor;

            switch (fileType)
            {
                case FileType.InputTradeFile:
                    fileProcessor = new InputTradeFileDefinition();
                    break;

                default:
                    throw new NotImplementedException("File Type Not Valid For Processing");
            }

            return fileProcessor;
        }
    }
}
