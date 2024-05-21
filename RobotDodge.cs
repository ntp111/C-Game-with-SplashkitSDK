using SplashKitSDK;

public class RobotDodge
{
    private Player _Player;
    private Window _GameWindow;
    private List<Robot> _Robots;
    public bool Quit
    {
        get { return _Player.Quit;}
    }
    public RobotDodge(Window GameWindow)
    {
        _GameWindow = GameWindow;
        _Player = new Player(_GameWindow);
        _Robots = new List<Robot>();
    }
    public void HandleInput()
    {
        _Player.HandleInput();
        _Player.StayOnWindow(_GameWindow);

    }

    public void Draw()
    {
        _GameWindow.Clear(Color.White);
        foreach (Robot Robot in _Robots)
        {
            Robot.Draw();
        }
        _Player.Draw();
        _GameWindow.Refresh(60);
    }

    public void Update()
    {
        CheckCollisions();

        if (SplashKit.Rnd() < 0.03)
        {
            Robot randomRobot = RandomRobot();
            _Robots.Add(randomRobot);
        }
        foreach (Robot robot in _Robots)
        {
            robot.Update();
        }
    }
    private void CheckCollisions()
    {
        List<Robot>_RobotsToRemove = new List<Robot>();
        foreach (Robot robot in _Robots)
        {
            if (_Player.CollidedWith(robot) || robot.IsOffscreen(_GameWindow))
            {
                _RobotsToRemove.Add(robot);
            }
        }
        foreach (Robot robot in _RobotsToRemove)
        {
            _Robots.Remove(robot);
        }
    }
    public Robot RandomRobot()
    {
        return new Robot(_GameWindow, _Player);
    }
}


