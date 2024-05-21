
# TUTORIAL: Create a c# game that utilizes JSON features from SplashkitSDK

## A. What is JSON?


## B. JSON in Game Development
1. Benefit of JSON
- 
-
-

2. JSON usage
- Load game configuration:
- Saving and loading game process:
- Faciliate communication between server and client for multiplayer game.
## C. Game with JSON data implementation
1. Splashkit functions to interact with JSON.
    - Initialized JSON object
        ```
        //create empty object
        Json json_object = SplashKit.CreateJson();

        //create object from string 
        string jString = @"
        {
            ""window"": {
                ""width"": 800,
                ""height"": 600
            },
            ""user"": {
                ""name"": ""Player1""
            },
            ""game"": {
                ""lives"": 3
            }
        }";
        Json json_object = SplashKit.JsonFromString(jString);

        //create object from file
        Json json_object = SplashKit.JsonFromFile("data.json");
        ```

    - Interact with JSON object

        ```
        // Example of Json data from config.json file:
        
        {
            "window": {
                "name": "ROBOT DODGE WITH JSON",
                "width": 1200,
                "height": 1000
            },
            "player": {
                "playerImage":"Player.png",
                "heartImage":"Heart.png",
                "emptyHeartImage":"EmptyHeart.png",
                "lives": 7,
                "speed": 5,
                "moveForward": 273,
                "moveBackward": 274,
                "moveLeft": 276,
                "moveRight": 275
            },
            "robot": {
                "speed": 3
            }
        }
        ```

        ```
        //read data inside JSON object
        Json jsonConfig = SplashKit.JsonFromFile("config.json");
        Json windowConfig = SplashKit.JsonReadObject(jsonConfig, "window");

        ```
        ```
        //get data from JSON object
        int height = SplashKit.JsonReadNumberAsInt(windowConfig, "height");
        string windowName = SplashKit.JsonReadString(windowConfig, "name");

        // we can also read other type of data like array, boolean, number and double depend on the data type from your JSON file:
        SplashKit.JsonReadArray(Json j, string key, ref List<double> outResult);
        SplashKit.JsonReadBool(Json j, string key);
        SplashKit.JsonReadNumber(Json j, string key);
        SplashKit.JsonReadNumberAsDouble(Json j, string key);
        ```

        ```
        //insert data into JSON object
        // first create json object:
        Json json_object = SplashKit.CreateJson();

        // now we can set data for this new object:
        SplashKit.JsonSetNumber(json_object, "lives", _Player.Lives);
        SplashKit.JsonSetNumber(json_object, "active", active);

        // we can also pass this object inside another object:
        // we a new object
        Json json_object2 = SplashKit.CreateJson();
        // use JsonSetObject to insert the json_object to json_object2:

        SplashKit.JsonSetObject(json_object2, "player", json_object);
        SplashKit.JsonToFile(json_object2, "backup.json");

        ```



2. Implement JSON to a real game
    - Create a simple game: For the tutorial purpose, we will create a simple game call Robot Dodge. we will have a player that can be moved around dodging the robots.
    - Purpose of using JSON for this game: 
        - the game will load the game configuration from JSON file
        - when user close the game, the player information will be saved into a JSON file and when open the game again, the Player will start the game with the same lives from when they quit.
    - Create Main function to initialize the game:
        - Initial setup:
            - Create config.json file and place it inside this directory: yourproject/resources/json
                ```
                {
                    "window": {
                        "name": "ROBOT DODGE WITH JSON",
                        "width": 1200,
                        "height": 1000
                    },
                    "player": {
                        "playerImage":"Player.png",
                        "heartImage":"Heart.png",
                        "emptyHeartImage":"EmptyHeart.png",
                        "lives": 7,
                        "speed": 5,
                        "moveForward": 273,
                        "moveBackward": 274,
                        "moveLeft": 276,
                        "moveRight": 275
                    },
                    "robot": {
                        "speed": 3
                    }
                }
                ```
            - Create an empty backup.config file so we can save the gameplay process into, the file should also be stored in same directory with config.json.


        - Important logic:
            - initialized json object from file
            - get data from json object
            - called function to save game process
            ```
            


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
                        // Read different sections of the JSON config
                        Json windowConfig = SplashKit.JsonReadObject(jsonConfig, "window");
                        Json playerConfig = SplashKit.JsonReadObject(jsonConfig, "player");
                        Json robotConfig = SplashKit.JsonReadObject(jsonConfig, "robot");

                        // Retrieve window configuration from windowConfig object
                        int width = SplashKit.JsonReadNumberAsInt(windowConfig, "width");
                        int height = SplashKit.JsonReadNumberAsInt(windowConfig, "height");
                        string windowName = SplashKit.JsonReadString(windowConfig, "name");

                        Window robotDodgeWindow;
                        robotDodgeWindow = new Window(windowName, width, height);
                        // passed playerConfig and robotConfig Json object to RobotDodge to setup the player and robot
                        RobotDodge robotDodge = new RobotDodge(robotDodgeWindow, playerConfig, robotConfig);
                        while (!robotDodgeWindow.CloseRequested && !robotDodge.Quit)
                        {
                            SplashKit.ProcessEvents();
                            robotDodge.HandleInput();
                            robotDodge.Update();
                            robotDodge.Draw();
                        }
                        if (robotDodgeWindow.CloseRequested || robotDodge.Quit)
                        {
                            robotDodge.saveProgress();
                        }
                        robotDodgeWindow.Close();
                    }

                }
            }

            ```
    - In RobotDodge class, add a function to save the game process
        - Important logic:
            - create json properties and use it to create player and robot
            - create function to save the game process.
            ```
            // constructor should set the player and robot config
            // player config is used to create new player
            public RobotDodge(Window GameWindow, Json PlayerConfig, Json RobotConfig)
            {
                _GameWindow = GameWindow;
                _PlayerConfig = PlayerConfig;
                _RobotConfig = RobotConfig;

                _Player = new Player(_GameWindow, _PlayerConfig);
                _Robots = new List<Robot>();
                _Bullets = new List<Bullet>();
                myTimer = new SplashKitSDK.Timer("My Timer");
                myTimer.Start();
            }

            // pass the config json object when create robots
            public Robot RandomRobot()
            {
                return new Robot(_GameWindow, _Player, _RobotConfig);
            }

            // create function to save the gameplay process
            public void saveProgress(){
                int active = 0;
                if (_Player.Lives > 0)
                {
                    active = 1;
                }
                Json json_object = SplashKit.CreateJson();
                SplashKit.JsonSetNumber(json_object, "lives", _Player.Lives);
                SplashKit.JsonSetNumber(json_object, "active", active);

                Json json_object2 = SplashKit.CreateJson();
                SplashKit.JsonSetObject(json_object2, "player", json_object);
                SplashKit.JsonToFile(json_object2, "backup.json");

            }
            ```
        - In Player class, use the config to setup the player
            ```
            // constructor uses json object to set the player image, lives, etc..
            // constructor also check the "backup.config" to set the state of the player from backup instead of creating new player.
            public Player(Window gameWindow, Json playerConfig)
            {
                _playerConfig = playerConfig;
                _PlayerBitmap = new Bitmap("Player", SplashKit.JsonReadString(_playerConfig, "playerImage"));
                _PlayerLivesBitmap = new Bitmap("Heart", SplashKit.JsonReadString(_playerConfig, "heartImage"));
                _PlayerLostLivesBitmap = new Bitmap("EmptyHeart", SplashKit.JsonReadString(_playerConfig, "emptyHeartImage"));
                fullLives = SplashKit.JsonReadNumberAsInt(_playerConfig, "lives");
                Lives = SplashKit.JsonReadNumberAsInt(_playerConfig, "lives");

                //check backup to update the player info
                Json jsonBackup = SplashKit.JsonFromFile("backup.json");
                Json playerBackup = SplashKit.JsonReadObject(jsonBackup, "player");
                //active equals 1 mean the backup file is meant to be loaded for the game
                if (SplashKit.JsonReadNumberAsInt(playerBackup, "active") == 1)
                {
                    Lives = SplashKit.JsonReadNumberAsInt(playerBackup, "lives");
                }

                X = (gameWindow.Width - Width) / 2;
                Y = (gameWindow.Height - Height) / 2;
                Quit = false;
            }
            //set up controls of the player based on json file
            // We need to set (KeyCode to change the data from json to keycode input type)
            public void HandleInput()
            {

                int speed = SplashKit.JsonReadNumberAsInt(_playerConfig, "speed");

                if (SplashKit.KeyDown((KeyCode)SplashKit.JsonReadNumberAsInt(_playerConfig, "moveForward")))
                {
                    Y -= speed;
                }
                if (SplashKit.KeyDown((KeyCode)SplashKit.JsonReadNumberAsInt(_playerConfig, "moveBackward")))
                {
                    Y += speed;
                }
                if (SplashKit.KeyDown((KeyCode)SplashKit.JsonReadNumberAsInt(_playerConfig, "moveRight")))
                {
                    X += speed;
                }
                if (SplashKit.KeyDown((KeyCode)SplashKit.JsonReadNumberAsInt(_playerConfig, "moveLeft")))
                {
                    X -= speed;
                }
                if (Lives <= 0 || SplashKit.KeyDown(KeyCode.EscapeKey))
                {
                    Quit = true;
                }
            }
            ```
        - in Robot class, we will set up similarly
            ```
                int SPEED = SplashKit.JsonReadNumberAsInt(_robotConfig, "speed");
            ```


