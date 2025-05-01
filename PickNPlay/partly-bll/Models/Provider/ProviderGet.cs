using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PickNPlay.picknplay_bll.Models.Provider
{
    public class ProviderGet
    {
        public int ProviderId { get; set; }
        public string ProviderName { get; set; } = null!;
        public string ProviderLogoUrl { get; set; }


    }
}
