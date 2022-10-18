namespace sky_scrapper2;

public class Game1 : Game
{

    public const int width = 1300;
    public const int height = 800;

    Texture2D ball_drop;

    SoundEffectInstance background_music;


    public GraphicsDeviceManager _graphics;
    public SpriteBatch _spriteBatch;


    Main_Game_Screen game_Screen;
    Home_Screen home_Screen;
    Game_Over_Screen game_Over_Screen;


    Basicfunc basf;

    Screen screen;

    ArrayList drops = new ArrayList();

    Random rand = new Random();

    bool is_but_pressed;   // to synchronize the event one after the another

    SpriteFont sfont;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        this.basf = new Basicfunc(this, width, height, false);

        this.screen = Screen.home_screen;

        this.home_Screen = new Home_Screen(this);
        this.game_Screen = new Main_Game_Screen(this, home_Screen.b_h_x);
        this.game_Over_Screen = new Game_Over_Screen(this);
        // game_Over_Screen.set_score(100);
        game_Over_Screen.set_score(200);

        drops = new ArrayList();

        ball_drop = basf.loadtexture("assets/pdot");

        background_music = basf.get_load_instance("assets/audio/music", true);
        background_music.Play();

        is_but_pressed = false;

        sfont = basf.loadfont("assets/fonts/simple_font");

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        var ms = Mouse.GetState();
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();




        var ks = Keyboard.GetState();
        var ctrl_pressed = (ks.IsKeyDown(Keys.LeftControl) || ks.IsKeyDown(Keys.RightControl));

        if (ks.IsKeyDown(Keys.Space))
        {
            if (!is_but_pressed)
            {
                if (screen == Screen.home_screen)
                {
                    screen = Screen.game_screen;
                    game_Screen = new Main_Game_Screen(this, home_Screen.b_h_x);
                    is_but_pressed = true;
                }
                else if (screen == Screen.game_over_screen)
                {
                    screen = Screen.home_screen;
                    is_but_pressed = true;
                }
            }
        }
        else
        {
            is_but_pressed = false;
        }
        if (game_Screen.is_game_over || ctrl_pressed && ks.IsKeyDown(Keys.Q))
        {
            if (screen == Screen.game_screen)
            {
                screen = Screen.game_over_screen;
                game_Over_Screen.change_y_position = 0;
                game_Over_Screen.building_Blocks = game_Screen.building_Blocks;
                game_Over_Screen.y_changer = game_Screen.y_changer;
                game_Over_Screen.back_y = game_Screen.back_y;
                game_Over_Screen.backgrounds = game_Screen.backgrounds;
                game_Over_Screen.set_score(game_Screen.score);
                is_but_pressed = true;
                game_Over_Screen.play_defeat_song();
            }
        }




        if (screen == Screen.home_screen)
        {
            home_Screen.update(ms);
        }
        else if (screen == Screen.game_screen)
        {
            game_Screen.update(ms);
        }
        else if (screen == Screen.game_over_screen)
        {
            game_Over_Screen.update(ms);
        }
        // TODO: Add your update logic here



        // moving the drops
        foreach (Drop item in drops)
        {
            if (item.x > width || item.x < 0 || item.y > height)
            {
                drops.Remove(item);
                break;
            }
            item.x += 1;
            item.y += 1;
        }

        if (drops.Count < 20)
        {
            drops.Add(new Drop(rand.Next(0, width), rand.Next(-200, 0)));
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.DarkBlue);
        if (screen == Screen.home_screen)
        {
            this.home_Screen.draw();
        }
        else if (screen == Screen.game_screen)
        {
            this.game_Screen.draw();
        }
        else if (screen == Screen.game_over_screen)
        {
            this.game_Over_Screen.draw();
        }
        
        // displaying the message for the user in the screen is home_screen or game_over_screen 
        if(screen == Screen.home_screen || screen == Screen.game_over_screen){
            var message = "Press Space Bar To Start The Game";
            var text_size = sfont.MeasureString(message);
            basf.drawtext(sfont,message,new (_graphics.PreferredBackBufferWidth/2-text_size.X/2,_graphics.PreferredBackBufferHeight/2+100),Color.DarkOrange,new Vector2(1,1));
        }

        foreach (Drop item in drops)
        {
            basf.displayimgrect(ball_drop, new Rectangle(item.x, item.y, 10, 10), Color.LightPink);
        }


        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
