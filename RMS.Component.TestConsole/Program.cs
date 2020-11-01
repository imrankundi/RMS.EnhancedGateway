using PKPSAssets.Component.Common;
using PKPSAssets.Component.RestHelper;
using System;

namespace PKPSAssets.Component.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(5 * 1000);
                var res = ProcessMonitor.IsProcessRunning("PKPSAssets.Agent.WebApi");
                Console.WriteLine("Process Running {0}", res);
                //while(true)
                //{
                //    System.Threading.Thread.Sleep(5 * 1000);
                //    var res = ProcessMonitor.IsProcessRunning("PKPSAssets.Agent.WebApi");
                //    Console.WriteLine("Process Running {0}", res);

                //    if(!res)
                //    {
                //        try
                //        {
                //            (new System.Threading.Thread(() => CommandLine.RunExternalExe(@"D:\Source\DotNet\PKPSAssets\PKPSAssetsServices\PKPSAssets.Agent.WebApi\bin\Debug\PKPSAssets.Agent.WebApi.exe"))).Start();
                //        }
                //        catch (Exception ex)
                //        {
                //        }

                //        //Process process = Process.Start(@"D:\Source\DotNet\PKPSAssets\PKPSAssetsServices\PKPSAssets.Agent.WebApi\bin\Debug\PKPSAssets.Agent.WebApi.exe");
                //        //int id = process.Id;
                //        //Process tempProc = Process.GetProcessById(id);
                //        ////this.Visible = false;
                //        //tempProc.WaitForExit();
                //        //this.Visible = true;

                //    }
                //}
                Encrypt();
                Console.ReadKey();
            }
        }

        private static object syncLock = new object();
        static long count = 100001;
        public static long GetCount()
        {
            lock (syncLock)
            {
                return count++;
            }

        }


        public static void Encrypt()
        {
            EncryptionRequest request = new EncryptionRequest
            {
                RequestType = CryptoRequestType.Encrypt,
                EncryptionType = Server.Cryptography.DataTypes.EncryptionType.AES,
                KeyEntityId = Guid.Parse("78CD19D9-3D46-407C-81FC-ABB10118DDBA"),
                PlainText = "Hello World!"
            };
            RestClientFactory client = new RestClientFactory("CryptographyApi");
            var response = client.PostCall<EncryptionResponse, EncryptionRequest>
                    (client.apiConfiguration.Apis["Encrypt"], request);



        }
    }
}
