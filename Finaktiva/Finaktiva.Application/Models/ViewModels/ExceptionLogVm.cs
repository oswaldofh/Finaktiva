namespace Finaktiva.Application.Models.ViewModels
{
    public class ExceptionLogVm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Json { get; set; }
    }
}
