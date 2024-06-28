namespace core.Models;

public class Collaboration
{
    public int Id { get; set; }
    public Guid MusicId { get; set; }

    public string CollaboratorName { get; set; }

    public string CollaboratorDescription { get; set; }

    public Music Music { get; set; }
}