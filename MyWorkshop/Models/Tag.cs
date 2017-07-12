using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkshop.Models
{
    public class Tag
    {
        public Tag()
        {
            Posts = new List<Post>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual IList<Post> Posts { get; set; }
    }
}
