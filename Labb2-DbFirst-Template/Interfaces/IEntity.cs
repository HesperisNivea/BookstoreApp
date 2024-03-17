namespace Labb2_DbFirst_Template.Interfaces;

public interface IEntity<TId>
{
    public TId Id { get; set; }
}