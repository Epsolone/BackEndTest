using System.Collections.Generic;

namespace BackEndApplication.Models
{
    public class Page
    {
        public bool HasMore { get; set; }

        public List<Image> Pictures { get; set; }
    }
}
