namespace BT.Social.WinFormsApp;

static class Program
{
  [STAThread]
  static void Main()
  {
    ApplicationConfiguration.Initialize();
    Application.Run(new Form1());
  }
}
