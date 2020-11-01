using Newtonsoft.Json;
using RMS.Services.Tests.RestTest;
using System;
using System.Threading.Tasks;

namespace RMS.Services.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //CommandController commandController = new CommandController();
            //commandController.Post(new Command.Requests.CommandRequest
            //{
            //    RequestType = Command.Requests.RequestType.MouseLeftClick
            //});

            //***************************************//

            //TouchlessController touchlessController = new TouchlessController();
            //var response = touchlessController.StartTouchlessTransaction();

            //***************************************//

            //QRImageGenerator qrImageGenerator = new QRImageGenerator();
            //qrImageGenerator.GenerateNewQRImage("PKPSAssets QR Image Test.");
            //Console.WriteLine("Image Saved.");
            //QRImageReader qrImageReader = new QRImageReader();
            //var decodedValue = qrImageReader.DecodeQRImage("D:\\Product Development\\QR Touchless\\QRImages\\PKPSAssets.QRImage.png");
            //Console.WriteLine(decodedValue);

            //***************************************//
            //Rest API Call Test

            //MainAsync().GetAwaiter().GetResult();

            //***************************************//
            //Json File Writer Test

            //JsonFileWriter jsonFileWriter = new JsonFileWriter();
            //AgentDataModel agentDataModel = new AgentDataModel
            //{
            //    IsTouchlessSession = true,
            //    QRCode = "asdf34asdas"
            //};
            //jsonFileWriter.WriteFile(agentDataModel);

            //Console.WriteLine("Done!");

            //***************************************//
            //QRImageForm

            //QRImageManager.LoadQRImageOnDialog();
            //Console.WriteLine("QR Image Loaded.");
            //Console.WriteLine("Press any key to ShowDialog.");
            //Console.ReadLine();
            //QRImageManager.EnableQRImageForm();
            //Console.WriteLine("Press any key to HideDialog.");
            //Console.ReadLine();
            //QRImageManager.HideQRImageForm();
            //Console.WriteLine("Press any key to ShowDialog.");
            //Console.ReadLine();
            //QRImageManager.LoadQRImageOnDialog();
            //QRImageManager.EnableQRImageForm();
            //Console.WriteLine("Press any key to HideDialog.");
            //Console.ReadLine();
            //QRImageManager.HideQRImageForm();
            //Console.WriteLine("Press any key to ShowDialog.");
            //Console.ReadLine();
            //QRImageManager.LoadQRImageOnDialog();
            //QRImageManager.EnableQRImageForm();
            //Console.WriteLine("Press any key to HideDialog.");
            //Console.ReadLine();
            //QRImageManager.HideQRImageForm();

            //***************************************//


            Console.ReadLine();
        }

        public static async Task MainAsync()
        {
            SampleRequest request = new SampleRequest
            {
                RequestType = 1
            };
            var res = await SampleModel.StartTouchlessTransaction(request);
            Console.WriteLine(JsonConvert.SerializeObject(res, Formatting.Indented));
        }
    }
}
