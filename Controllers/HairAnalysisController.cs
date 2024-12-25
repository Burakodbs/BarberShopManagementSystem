using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using BarberShopManagementSystem.Models;
using BarberShopManagementSystem.ViewModels;

namespace BarberShopManagementSystem.Controllers
{
    public class HairAnalysisController : Controller
    {
        private readonly string _huggingFaceApiKey = "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"; // Hugging Face'den ücretsiz API key alabilirsiniz
        private readonly string _modelUrl = "https://api-inference.huggingface.co/models/Salesforce/blip-image-captioning-large"; // Örnek model

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AnalyzeHair(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("Dosya yüklenmedi");

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_huggingFaceApiKey}");

            // Dosyayı byte array'e çevir
            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            var imageBytes = ms.ToArray();

            // API'ye istek gönder
            var content = new ByteArrayContent(imageBytes);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var response = await client.PostAsync(_modelUrl, content);
            var result = await response.Content.ReadAsStringAsync();

            // AI sonuçlarını analiz et ve önerileri oluştur
            var viewModel = new HairAnalysisViewModel
            {
                RawApiResult = result,
                Recommendations = AnalyzeResults(result)
            };

            return View("Results", viewModel);
        }

        private HairRecommendations AnalyzeResults(string apiResult)
        {
            Console.Write(apiResult);
            // API çıktısı bir görüntü sınıflandırma ve öznitelik analizi yapar
            // Saç tipi, renk ve yüz şekli gibi özellikleri tespit eder

            var recommendations = new HairRecommendations();
            recommendations.RecommendedStyles = new List<string> { };
            {
                // API sonucunda saç tipi düz/dalgalı/kıvırcık olarak tespit edilirse:
                if (apiResult.Contains("beard"))
                {
                    recommendations.RecommendedStyles = new List<string> {
                        "Sakallarınızı biraz kısaltabilirsiniz." 
                        ,"Sizin önerilen hizmetimiz: Sakal Kesimi"
                    };
                }
                else if (apiResult.Contains("hair"))
                {
                    recommendations.RecommendedStyles = new List<string> {
                        "Saçlarınızın biraz bakıma ihtiyacı var gibi. Tam yerine geldiniz." ,
                        "Sizin için önerilen hizmetimiz: Saç Kesimi "
                    };
                        
                }
                if(apiResult.Contains("beard") && apiResult.Contains("hair"))
                {
                    recommendations.RecommendedStyles = new List<string> {
                    "Saçınız ve sakalınız birbirine karışacak. Güzel bir tıraşı hakediyorsunuz",
                    "Sizin için önerilen hizmetimiz: Saç ve Sakal Kesimi "
                    };
                }
                else
                {
                    recommendations.RecommendedStyles = new List<string> {
                    "Bence berberlerimizle görüşmelisiniz. Gözlerim biraz kötü görüyor da:)"};
                }


                return recommendations;
            }
        }


    }
}
