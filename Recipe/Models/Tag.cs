using System.Collections.Generic;

namespace Recipe.Models
{
  public class Tag
    {
        public Tag()
        {
            this.JoinEntities = new HashSet<Box>();
        }

        public int TagId { get; set; }
        public string TagName { get; set; }
        public virtual ICollection<Box> JoinEntities { get; set; }
    }
}