using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace apitemplateio
{
	public class APITemplateIO
	{

		private string api_key { get; set; }

		public APITemplateIO(string api_key)
		{
			this.api_key = api_key;
		}


		private HttpClient CreateHttpClient()
		{
			var client = new HttpClient();
			client.DefaultRequestHeaders.Add("X-API-KEY", api_key);
			return client;
		}

		private async Task Download(string download_url, string save_to)
		{
			var client = new HttpClient();
			var download_response = await client.GetAsync(download_url);
			using (var stream = await download_response.Content.ReadAsStreamAsync())
			{
				using (Stream fileStream = File.Open(save_to, FileMode.Create))
				{
					await stream.CopyToAsync(fileStream);
				}
			}
		}

		public async Task<bool> CreatePDF(string template_id, string json, string pdf_file_path)
		{
			return await Create(template_id, json, pdf_file_path, null);
		}

		public async Task<bool> CreateImage(string template_id, string json, string jpeg_file_path, string png_file_path)
		{
			return await Create(template_id, json, jpeg_file_path, png_file_path);
		}


		public async Task<bool> CreatePDF(string template_id, object obj, string pdf_file_path)
		{
			return await Create(template_id, obj, pdf_file_path, null);
		}

		public async Task<bool> CreateImage(string template_id, object obj, string jpeg_file_path, string png_file_path)
		{
			return await Create(template_id, obj, jpeg_file_path, png_file_path);
		}

        protected async Task<bool> Create(string template_id, object obj, string save_to_1, string save_to_2)
		{
            var json_content = JsonSerializer.Serialize(obj);
            return await Create(template_id, json_content, save_to_1, save_to_2);
        }

		protected async Task<bool> Create(string template_id, string json_content, string save_to_1, string save_to_2)
		{
			var url = $"https://api.apitemplate.io/v1/create?template_id={template_id}";

			var buffer = System.Text.Encoding.UTF8.GetBytes(json_content);
			var byteContent = new ByteArrayContent(buffer);

			var client = CreateHttpClient();
			var response = await client.PostAsync(url, byteContent);
			var ret = await response.Content.ReadAsStringAsync();
			var returnContent = JsonSerializer.Deserialize<CreateResponse>(ret);

			if (returnContent.status == "success")
			{
				await Download(returnContent.download_url, save_to_1);
				if (!string.IsNullOrEmpty(returnContent.download_url_png) && save_to_2 != null)
				{
					await Download(returnContent.download_url_png, save_to_2);
				}
				return true;
			}
			return false;
		}

		public async Task<AccountInfoResponse> GetAccountInformation()
		{
			var url = $"https://api.apitemplate.io/v1/account-information";
			var client = CreateHttpClient();
			var response = await client.GetAsync(url);
			var ret = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<AccountInfoResponse>(ret);
		}

		public async Task<List<TemplateItemResponse>> ListTemplates()
		{
			var url = $"https://api.apitemplate.io/v1/list-templates";
			var client = CreateHttpClient();
			var response = await client.GetAsync(url);
			var ret = await response.Content.ReadAsStringAsync();
			Console.WriteLine(ret);
			return JsonSerializer.Deserialize<List<TemplateItemResponse>>(ret);
		}

	}
}
