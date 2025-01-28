using BarberShopManagementSystem.Models;

namespace BarberShopManagementSystem.ViewModels {
    public class HairAnalysisViewModel {
        public string RawApiResult { get; set; }
        public HairRecommendations Recommendations { get; set; }
        public string ProcessedImagePath { get; internal set; }
    }
}