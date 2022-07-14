using System;
using System.Collections.Generic;
using System.Threading;
using IBApi;

namespace IBKR
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            

            Samples.EWrapperImpl ibClient = new Samples.EWrapperImpl();

            ibClient.ClientSocket.eConnect("8.218.39.58", 7496, 0);

            var reader = new EReader(ibClient.ClientSocket, ibClient.Signal);
            reader.Start();

            new Thread(() => {
                while (ibClient.ClientSocket.IsConnected())
                {
                    ibClient.Signal.waitForSignal();
                    reader.processMsgs();
                }
            })
            { IsBackground = true }.Start();

            while (ibClient.NextOrderId <= 0) { }

            Contract contract = new Contract();
            contract.Symbol = "CHF";
            contract.SecType = "CASH";
            contract.Exchange = "IDEALPRO";
            contract.Currency = "USD";



            List<TagValue> mktDataOptions = new List<TagValue>();


            // Kick off the request for market data for this
            // contract.  reqMktData Parameters are:
            // tickerId           - A unique id to represent this request
            // contract           - The contract object specifying the financial instrument
            // genericTickList    - A string representing special tick values
            // snapshot           - When true obtains only the latest price tick
            //                      When false, obtains data in a stream
            // regulatory snapshot - API version 9.72 and higher. Remove for earlier versions of API
            // mktDataOptions   - TagValueList of options 
            ibClient.ClientSocket.reqMktData(1, contract, "", false, false, mktDataOptions);

            // Pause so we can view the output
            Console.ReadKey();

            // Cancel the subscription/request. Parameter is:
            // tickerId         - A unique id to represent the request 
            ibClient.ClientSocket.cancelMktData(1);

            // Disconnect from TWS
            ibClient.ClientSocket.eDisconnect();


            ibClient.ClientSocket.reqMktData(1, contract, "", false, false, mktDataOptions);

            // Pause so we can view the output
            Console.ReadKey();

            // Cancel the subscription/request. Parameter is:
            // tickerId         - A unique id to represent the request 
            ibClient.ClientSocket.cancelMktData(1);

            // Disconnect from TWS
            ibClient.ClientSocket.eDisconnect();

        }
    }
}
