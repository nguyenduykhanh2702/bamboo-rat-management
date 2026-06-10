using System;
public class Cage : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsLocked { get; set; } = false;

    public CageType Type { get; set; }

    public ICollection<Rat> Rats { get; set; } = new List<Rat>();

    public ICollection<CageTransfer> OutgoingTransfers { get; set; } = new List<CageTransfer>();

    public ICollection<CageTransfer> IncomingTransfers { get; set; } = new List<CageTransfer>();
}
