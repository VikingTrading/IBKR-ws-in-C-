using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using IBApi;

namespace IBKR
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            string[] symbols = { "EUR", "GBP", "CAD", "CHF", "AUD"};
            List<string> symbolList = new List<string>();
            foreach (string a in symbols)
                Console.WriteLine(a);
            

            Samples.EWrapperImpl ibClient = new Samples.EWrapperImpl();

            ibClient.ClientSocket.eConnect("8.218.39.58", 7496, 2);

            /* ibClient.ClientSocket.eConnect("127.0.0.1", 7496, 0);*/

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

           

            List<Contract> contractList = new List<Contract>();





           /* Contract contract = new Contract();
            contract.Symbol = "CHF";
            contract.SecType = "CASH";
            contract.Exchange = "IDEALPRO";
            contract.Currency = "USD";*/

            Contract contract = new Contract();
            contract.Symbol = "XAUUSD";
            contract.SecType = "CMDTY";
            contract.Exchange = "smart";
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


            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);

            server.Start();
            Console.WriteLine("Server has started on 127.0.0.1:80.{0}Waiting for a connection...", Environment.NewLine);

            TcpClient client = server.AcceptTcpClient();

            Console.WriteLine("A client connected.");

        }
    }
}
