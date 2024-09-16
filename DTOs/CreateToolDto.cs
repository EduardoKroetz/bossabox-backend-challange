namespace bossabox_backend_challange.DTOs;

public class CreateToolDto
{
    public string Title { get; set; }
    public string Link { get; set; }
    public string Description { get; set; }
    public string[] Tags { get; set; } = [];
}
