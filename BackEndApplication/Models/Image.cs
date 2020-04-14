using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BackEndApplication.Models
{
    public class Image
    {
        public string Id { get; set; }

        public string Author { get; set; }

        public string Camera { get; set; }

        public string Tags { get; set; }

        [JsonProperty("cropped_picture")]
        public string CroppedPicture { get; set; }

        [JsonProperty("full_picture")]
        public string FullPicture { get; set; }
    }
}
