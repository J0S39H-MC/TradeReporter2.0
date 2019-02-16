using QLTradeReporter.Core.Events;
using QLTradeReporter.Core.Factories;
using QLTradeReporter.Core.FileDefinitions;
using QLTradeReporter.Core.Interfaces;
using QLTradeReporter.Core.Models;
using QLTradeReporter.Core.Processor;
using QLTradeReporter.Core.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;


namespace QLTradeReporter.ConsoleApp
{
    class Program
    {
        static ITradeParser tradeParser;
        static ITradeMapper mapper;
        static ITradeValidator validator;
        static PostTradeNotifier postTradeTicker;
        static TradeMonitor tradeMonitor;
        static ITradeDataService tradeDataService;
        static IDisposable handle;

        static string outputPath = string.Empty;
        static FileInfo inputPath;
        static DirectoryInfo outputDir;

        static readonly Container container;

        static Program()
        {
            container = new Container();
            container.Register<ITradeDataService, TradeDataService>();
            container.Register<ITradeMapper, TradeMapper>();
            container.Register<ITradeParser, TradeParser>();
            container.Register<ITradeValidator, TradeValidator>();
            container.Register<PostTradeNotifier>();
            container.Register<IDelimitedFileDefinition, InputTradeFileDefinition>();
            container.Verify();
        }

        static void Main(string[] args)
        {
            if (args.Count() != 2 || args.Any(p => string.IsNullOrWhiteSpace(p))) throw new ArgumentNullException("Invalid arguments passed to app.");

            if (!File.Exists(args[0]))
            {
                throw new FileNotFoundException("input file not found");
            }

            if (!Directory.Exists(args[1]))
            {
                throw new DirectoryNotFoundException("output directory not found. valid output directory requried.");
            }           

            inputPath = new  FileInfo(args[0]);
            outputDir = new DirectoryInfo(args[1]);

            // todo use simple injector as DI container
            tradeDataService = container.GetInstance<ITradeDataService>();
            mapper = container.GetInstance<ITradeMapper>();
            validator = container.GetInstance<ITradeValidator>();
            tradeParser = container.GetInstance<ITradeParser>();
            postTradeTicker = container.GetInstance<PostTradeNotifier>();
            tradeMonitor = new TradeMonitor(postTradeTicker);

            var obsTrades = tradeDataService.GetObservableTradeData(inputPath.FullName);
            handle = obsTrades.Subscribe((str) => OnNext(str), (e) => OnError(e), () => OnCompleted());
        }

        private static object OnCompleted()
        {
            // output
            outputPath = Path.Combine(outputDir.FullName, "output.csv");
            FileInfo fileInfo = new FileInfo(outputPath);
            using (StreamWriter str = new StreamWriter(outputPath))
            {
                foreach (var item in tradeMonitor.TradeSummary.OrderBy(p => p.Key))
                {
                    str.WriteLine($"{item.Value.Symbol},{item.Value.MaxTimeGap},{item.Value.Volume},{item.Value.WeightedAveragePrice},{item.Value.MaxPrice}");
                }
            }
            return Unit.Default;
        }

        private static object OnError(Exception e)
        {
            //log trade parse error and line
            return Unit.Default;
        }

        private static object OnNext(string str)
        {
            // todo log trades not able to be parsed.
            // input
            if (tradeParser.TryParseTrade(str, out Trade trade))
                postTradeTicker.NotifyOnTradePosted(trade);
            
            return Unit.Default;
        }
    }
}
