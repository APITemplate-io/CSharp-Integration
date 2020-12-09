using System;
using apitemplateio;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace apitemplateio_cs_console
{
	class Program
	{
		static string api_key = "<your api key>";

		static async Task Main(string[] args)
		{
			await TestPDF();
			await TestImage();
			await TestAccountInformation();
		}

		static async Task TestPDF(){
			string template_id = "<your pdf template id>";
            string save_to = @"/home/dev/result.pdf";
            object data = new{
                name = "Hello world"
            };

            apitemplateio.APITemplateIO apitemplate = new APITemplateIO(api_key);
            await apitemplate.CreatePDF(template_id ,data,save_to);
		}

		static async Task TestImage(){
			string template_id = "<your image template id>";
			string save_to_jpeg = @"/home/dev/result.jpeg";
			string save_to_png = @"/home/dev/result.png";
			object data = new
			{
				overrides = new List<object>(){ new{
				name = "text_1",
				text = "hello"
			}}
			};

			apitemplateio.APITemplateIO apitemplate = new APITemplateIO(api_key);
			await apitemplate.CreateImage(template_id, data, save_to_jpeg, save_to_png);
		}

		static async Task TestAccountInformation(){
			apitemplateio.APITemplateIO apitemplate = new APITemplateIO(api_key);
            AccountInfoResponse res = await apitemplate.GetAccountInformation();
            Console.WriteLine(res.subscription_product);
		}

	}
}
