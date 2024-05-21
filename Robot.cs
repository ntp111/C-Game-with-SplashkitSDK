using SplashKitSDK;

public class Robot
{
    public double X {get; private set; }
    public double Y {get; private set; }
    private Vector2D Velocity {get; set; }
    public Color MainColor {get; set; }
    public Circle CollisionCircle
    {
        get { return SplashKit.CircleAt(X, Y, 20);}
    }

    public int Width
    {
        get { return 50; }
    }
    private int Height
    {
        get { return 50; }
    }
    public Robot(Window gameWindow, Player player)
    {
        MainColor = Color.RandomRGB(200);

        if (SplashKit.Rnd() < 0.5)
        {
            // pick top or bottom
            X = SplashKit.Rnd(gameWindow.Width);

            if (SplashKit.Rnd() < 0.5)
                Y = -Height;
            else
                Y = gameWindow.Height;
        }
        else
        {
            // pick left or right
            Y = SplashKit.Rnd(gameWindow.Height);

            if (SplashKit.Rnd() < 0.5)
                X = -Width;
            else
                X = gameWindow.Width;
        }

        // set up velocity
        const int SPEED = 4;
        Point2D fromPt = new Point2D()
        {
            X = X, Y = Y
        };

        Point2D toPt = new Point2D()
        {
            X = player.X, Y = player.Y
        };
        Vector2D dir;
        dir = SplashKit.UnitVector(SplashKit.VectorPointToPoint(fromPt, toPt));

        Velocity = SplashKit.VectorMultiply(dir, SPEED);
    }
    public void Update()
    {
        X += Velocity.X;
        Y += Velocity.Y;
    }
    public bool IsOffscreen(Window gameWindow)
    {
        int screenWidth = gameWindow.Width;
        int screenHeight = gameWindow.Height;
        return X < -Width || X > screenWidth || Y < -Height || Y > screenHeight;
    }
    public void Draw()
    {
        double leftX;
        double rightX;
        double eyeY;
        double mouthY;

        leftX = X + 12;
        rightX = X + 27;
        eyeY = Y + 10;
        mouthY = Y + 30;

        SplashKit.FillRectangle(Color.Gray, X, Y, Width, Height);
        SplashKit.FillRectangle(MainColor, leftX, eyeY, 10, 10);
        SplashKit.FillRectangle(MainColor, rightX, eyeY, 10, 10);
        SplashKit.FillRectangle(MainColor, leftX, mouthY, 25, 10);
        SplashKit.FillRectangle(MainColor, leftX + 2, mouthY + 2, 21, 6);
    }
}