namespace ClanNewsTool
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            ApplicationConfiguration.Initialize();
            Localization.Initialize();
            Application.Run(new MainForm());
        }
    }
}