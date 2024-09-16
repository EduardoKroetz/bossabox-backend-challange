namespace bossabox_backend_challange.DTOs;

public class ResultDto<T>
{
    public T Data { get; set; }
    public string Error { get; set; }
}
