namespace GraphQLServer.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public Items Item { get; set; } 
        public DateOnly ExpirationDate { get; set; }
    }
}
