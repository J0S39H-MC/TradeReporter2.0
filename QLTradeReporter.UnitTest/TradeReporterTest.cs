using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PostTradeMonitor.Core.Events;
using PostTradeMonitor.Core.FileDefinitions;
using PostTradeMonitor.Core.Interfaces;
using PostTradeMonitor.Core.Models;
using PostTradeMonitor.Core.Processor;
using PostTradeMonitor.Core.Services;

namespace PostTradeMonitor.UnitTest
{
    [TestClass]
    public class TradeReporterTest
    {
        //Mock<ITradeValidator> mockTradeValidator;
        ITradeValidator mockTradeValidator;
        ITradeDataService mockTradeDataService;
        ITradeMapper mockTradeMapper;
        IPostTradeNotifier postTradeTicker;
        TradeMonitor tradeMonitor;

        private TestContext testContext;

        public TestContext TestContext
        {
            get { return testContext; }
            set { testContext = value; }
        }

        public TradeReporterTest()
        {
            //mockTradeDataService = new Mock<ITradeDataService>();
            //mockTradeValidator = new Mock<ITradeValidator>();
            mockTradeDataService = new TradeDataService();
            mockTradeValidator = new TradeValidator(new InputTradeFileDefinition());
            mockTradeMapper = new TradeMapper(new InputTradeFileDefinition());
        }

        [TestMethod]
        public void TestTradeDataService()
        {
            //var source = Path.Combine(testContext.TestDeploymentDir, @"Input\input.csv");
            //var obsTrades = mockTradeDataService.GetObservableTradeData(@"C:\TestFiles\input.csv");
            //obsTrades.Subscribe((str) => OnNext(str), (e) => OnError(e), () => OnCompleted());
        }

        [TestMethod]
        public void Test_Validator_Returns_True_For_Valid_FieldArray()
        {
            bool isValid = mockTradeValidator.IsValid(new string[] { "51300375441", "cbe", "176", "78" });
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Test_Validator_Returns_False_For_Invalid_FieldArray()
        {
            bool isValid = mockTradeValidator.IsValid(new string[] { "cbe", "cbe", "176", "78" });
            Assert.IsFalse(isValid);

            isValid = mockTradeValidator.IsValid(new string[] { "51300375441", "51300375441", "176", "78" });
            Assert.IsFalse(isValid);


            isValid = mockTradeValidator.IsValid(new string[] { "51300375441", "cbe", "cbe", "78" });
            Assert.IsFalse(isValid);

            isValid = mockTradeValidator.IsValid(new string[] { "51300375441", "cbe", "176", "cbe" });
            Assert.IsFalse(isValid);

            isValid = mockTradeValidator.IsValid(new string[] { "51300375441", "cb2", "176", "78" });
            Assert.IsFalse(isValid);

        }

        [TestMethod]
        public void Test_Mapper_Returns_Trade_from_FieldArray()
        {
            Trade trade = mockTradeMapper.Map<Trade>(new string[] { "51300375441", "cbe", "176", "78" });
            Assert.AreEqual(51300375441, trade.TimeStamp);
            Assert.AreEqual("cbe", trade.Symbol);
            Assert.AreEqual(176, trade.Quantity);
            Assert.AreEqual(78, trade.Price);
        }

        [TestMethod]
        public void Test_Sample_Input_File()
        {
            postTradeTicker = new PostTradeNotifier();
            tradeMonitor = new TradeMonitor(postTradeTicker);

            List<Trade> trades = new List<Trade>();
            trades.Add(new Trade() { TimeStamp = 52924702,  Symbol = "aaa",  Quantity = 13, Price = 1136 });
            trades.Add(new Trade() { TimeStamp = 52924702, Symbol = "aac", Quantity = 20, Price = 477 });
            trades.Add(new Trade() { TimeStamp = 52925641, Symbol = "aab", Quantity = 31, Price = 907 });

            trades.Add(new Trade() { TimeStamp = 52927350, Symbol = "aab", Quantity = 29, Price = 724 });
            trades.Add(new Trade() { TimeStamp = 52927783, Symbol = "aac", Quantity = 21, Price = 638 });
            trades.Add(new Trade() { TimeStamp = 52930489, Symbol = "aaa", Quantity = 18, Price = 1222 });

            trades.Add(new Trade() { TimeStamp = 52931654, Symbol = "aaa", Quantity = 9, Price = 1077 });
            trades.Add(new Trade() { TimeStamp = 52933453, Symbol = "aab", Quantity = 9, Price = 756 });

            foreach(var trade in trades)
            {
                postTradeTicker.NotifyOnTradePosted(trade);
            }

            var tradeSummaries = tradeMonitor.TradeSummary;
            var group1 = tradeSummaries["aaa"];
            Assert.AreEqual(5787, group1.MaxTimeGap);
            Assert.AreEqual(40, group1.Volume);
            Assert.AreEqual(1161, group1.WeightedAveragePrice);
            Assert.AreEqual(1222, group1.MaxPrice);

            var group2 = tradeSummaries["aab"];
            Assert.AreEqual(6103, group2.MaxTimeGap);
            Assert.AreEqual(69, group2.Volume);
            Assert.AreEqual(810, group2.WeightedAveragePrice);
            Assert.AreEqual(907, group2.MaxPrice);


            var group3 = tradeSummaries["aac"];
            Assert.AreEqual(3081, group3.MaxTimeGap);
            Assert.AreEqual(41, group3.Volume);
            Assert.AreEqual(559, group3.WeightedAveragePrice);
            Assert.AreEqual(638, group3.MaxPrice);
        }

        private object OnError(Exception e)
        {
            return e;
        }

        private object OnCompleted()
        {
            return Unit.Default;
        }


        private object OnNext(string str)
        {
            return str;
        }

    }
}
