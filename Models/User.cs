﻿namespace bossabox_backend_challange.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Tool> Tools { get; set; }
}
