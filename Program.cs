using System;
using SplashKitSDK;

namespace RobotDodgeGame
{
    public class Program
    {
        public static void Main()
        {
             // Load JSON data from file
            Json jsonConfig = SplashKit.JsonFromFile("config.json");

            // Check if the file was loaded successfully
            if (jsonConfig == null)
            {
                Console.WriteLine("Failed to load JSON file.");
                return;
            }
            Json windowConfig = SplashKit.JsonReadObject(jsonConfig, "gameConfig");


            // int Width = int.Parse(SplashKit.JsonReadString(windowConfig, "width"));
            // int Height = int.Parse(SplashKit.JsonReadString(windowConfig, "height"));

            Window robotDodgeWindow;
            robotDodgeWindow = new Window("Robot Dodge", 800, 600);
            RobotDodge robotDodge = new RobotDodge(robotDodgeWindow);
            while (!robotDodgeWindow.CloseRequested && !robotDodge.Quit)
            {
                SplashKit.ProcessEvents();
                robotDodge.HandleInput();
                robotDodge.Update();
                robotDodge.Draw();
            }
            robotDodgeWindow.Close();
        }

    }
}
