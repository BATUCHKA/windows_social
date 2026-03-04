namespace BT.Social.Core.Interfaces
{
  public interface IShareable
  {
    void Share(Guid userId);
    int GetShareCount();
  }
}
