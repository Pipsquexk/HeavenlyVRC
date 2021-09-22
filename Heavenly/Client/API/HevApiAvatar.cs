using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.Core;

namespace Heavenly.Client.API
{
    public class HevApiAvatar
    {
        public string name { get; set; }
        public string id { get; set; }
        public string authorName { get; set; }
        public string authorId { get; set; }
        public string thumbnailImageUrl { get; set; }
        public string assetUrl { get; set; }
        
        public HevApiAvatar(string name, string id, string authorId, string authorName, string thumbnailImageUrl, string assetUrl)
        {
            this.name = name;
            this.id = id;
            this.authorName = authorName;
            this.authorId = authorId;
            this.thumbnailImageUrl = thumbnailImageUrl;
            this.assetUrl = assetUrl;
        }
    }
}
