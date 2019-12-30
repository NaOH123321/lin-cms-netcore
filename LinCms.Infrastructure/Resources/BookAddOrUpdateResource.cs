using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LinCms.Infrastructure.Resources
{
    public class BookAddOrUpdateResource
    {
        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string Summary { get; set; } = null!;

        public string Image { get; set; } = null!;
    }
}
