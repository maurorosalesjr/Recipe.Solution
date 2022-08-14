namespace Recipe.Models
{
  public class Box
    {       
        public int BoxId { get; set; }
        public int FormulaId { get; set; }
        public int TagId { get; set; }
        public virtual Formula Formula { get; set; }
        public virtual Tag Tag { get; set; }
    }
}