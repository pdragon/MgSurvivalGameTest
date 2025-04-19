using System;
using Desktop;

static class Program
{
    [STAThread]
    static void Main()
    {
        using var game = new DesktopGame();
        game.Run();
    }
}
