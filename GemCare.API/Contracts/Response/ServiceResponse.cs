namespace GemCare.API.Contracts.Response
{
    public class ServiceResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int Price { get; set; }     
    }
    public class AllServicesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }      
        public string ImageUrl { get; set; }
        public int Price { get; set; }
    }
}
