using QLTradeReporter.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLTradeReporter.Core.Services
{
    public class TradeDataService : ITradeDataService
    {
        /// <summary>
        /// Gets stream of data from trade file. this allows us to stream the file lines without the need for an in memory collection.
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public IObservable<string> GetObservableTradeData(string dataSource)
        {
            if (string.IsNullOrWhiteSpace(dataSource) || !File.Exists(dataSource))
                return Observable.Empty<string>();

            // in this implementation there will be no need to loop through the file and read the contents out or encapsulate the parsing and reading logic within a using block.
            // the app will subscribe to this data stream and perfrom any necessary logic as the data is streamed from the file.
            return Observable.Using(() => File.OpenText(dataSource), stream => Observable.Generate(stream, state => !state.EndOfStream, state => state, state => state.ReadLine() ?? string.Empty));
        }

    }
}
