﻿namespace bossabox_backend_challange.Models;

public class Tool
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Link { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<Tag> Tags { get; set; } = [];
}
