using SplashKitSDK;

public class Player
{
    private Bitmap _PlayerBitmap;
    public double X {get; private set; }
    public double Y {get; private set; }
    public bool Quit {get; private set; }
    public int Width {
        get {
            return _PlayerBitmap.Width;
        }
    }
    public int Height {
        get {
            return _PlayerBitmap.Height;
        }
    }

    public Player(Window gameWindow)
    {
        _PlayerBitmap = new Bitmap("Player", "Player.png");
        X = (gameWindow.Width - Width)/2;
        Y = (gameWindow.Height - Height)/2;
        Quit = false;
    }

    public void Draw(){
        SplashKit.DrawBitmap(_PlayerBitmap, X, Y);
    }

    public enum Direction {
        None,
        Up,
        Down,
        Left,
        Right
    }
    public void HandleInput(){
        const int speed = 5;
        const int boost = 10;
        Direction pressKey;

        if (SplashKit.KeyDown(KeyCode.UpKey) ){
            pressKey = Direction.Up; //for the boost key to know the latest direction go click (in case user go 2 directions. Example: up and right)
            Y -= speed;
            if (SplashKit.KeyDown(KeyCode.QKey) && pressKey == Direction.Up) Y -= boost; 
        }
        if (SplashKit.KeyDown(KeyCode.DownKey)){
            pressKey = Direction.Down;
            Y += speed;
            if (SplashKit.KeyDown(KeyCode.QKey) && pressKey == Direction.Down) Y += boost;

        }
        if (SplashKit.KeyDown(KeyCode.RightKey)){
            pressKey = Direction.Right;
            X += speed;
            if (SplashKit.KeyDown(KeyCode.QKey) && pressKey == Direction.Right) X += boost;

        }
        if (SplashKit.KeyDown(KeyCode.LeftKey)) {
            pressKey = Direction.Left;
            X -= speed;
            if (SplashKit.KeyDown(KeyCode.QKey)  && pressKey == Direction.Left) X -= boost;
        }
        if (SplashKit.KeyDown(KeyCode.EscapeKey)) Quit = true;
    }

    public void StayOnWindow(Window gameWindow){
        const int GAP = 10;

        // check left side
        if (X < GAP) X = GAP;
        // check right side
        if (X > (gameWindow.Width - Width - GAP)) X = gameWindow.Width - Width - GAP;
        // check top
        if (Y < GAP) Y = GAP;
        // check bottom
        if (Y > gameWindow.Height - Height -GAP) Y = gameWindow.Height - Height - GAP;
    }

    public bool CollidedWith(Robot robot)
    {
        return _PlayerBitmap.CircleCollision(X, Y, robot.CollisionCircle);
    }

}

