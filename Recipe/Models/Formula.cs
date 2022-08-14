using System.Collections.Generic;

namespace Recipe.Models
{
    public class Formula
    {
        public Formula()
        {
            this.JoinEntities = new HashSet<Box>();
        }

        public int FormulaId { get; set; }
        public string FormulaName { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Box> JoinEntities { get;}
    }
}