namespace sky_scrapper2;

class Game_Over_Screen:Parent_Component
{
    Game1 devconfig;
    Basicfunc basf;

    const int changing_speed = 10;


    public int score;
    public int y_changer;
    // public int no_of_blocks;

    public int block_height = 100;

    // getting all the assets..
    Assets assets;


    bool is_high_score;

    SoundEffectInstance game_over_sound;
    

    SpriteFont sfont;

    public int change_y_position;

    public List<Building_Block> building_Blocks;

    public ArrayList back_y,backgrounds;
    // Random rand;
    
    // Main_Game_Screen game1;
    Random rand;

    
    public Game_Over_Screen(Game1 game){
        devconfig = game;

        basf = new Basicfunc(this.devconfig);
        rand = new Random();

        game_over_sound = basf.get_load_instance("assets/gameover");

        sfont = basf.loadfont("assets/fonts/simple_font");

        this.building_Blocks = new List<Building_Block>();
        y_changer = 0;

        assets = new Assets(devconfig);

        score = 0;
        is_high_score = false;

        change_y_position = 0;

        back_y = new ArrayList();
        backgrounds = new ArrayList();
        
    }
    public override void update(MouseState ms){
        var ks = Keyboard.GetState();
        if(ks.IsKeyDown(Keys.W)){
            change_y_position-=(change_y_position+changing_speed>0)?changing_speed:0;
        }
        else if(ks.IsKeyDown(Keys.S)){
            change_y_position+=(change_y_position+changing_speed<y_changer)?changing_speed:0;
        }

        if(back_y.Count>0){
            var last_elem_y = (int)back_y[back_y.Count-1];
            var s_height = devconfig._graphics.PreferredBackBufferHeight;
            if(last_elem_y+y_changer>=0){
                back_y.Add(last_elem_y-s_height);
                backgrounds.Add(assets.back_imgs[rand.Next(0,assets.back_imgs.Length-1)]);
            }
        }
    }

    public override void draw(){
        var graphics = devconfig._graphics;
        var s_width = graphics.PreferredBackBufferWidth;
        var s_height = graphics.PreferredBackBufferHeight;

        var building_width = s_width / 2 + 200; // the value 200 here is finded experimentally
        var building_height = s_height / 2;
        var building_x = s_width / 2 - building_width / 2;
        var building_y = s_height - (building_height);

        // basf.displayimgrect(assets.background1,new Rectangle(0,0,s_width,s_height));
        for (var i = 0; i < backgrounds.Count; i++)
        {
            Texture2D item = (Texture2D)backgrounds[i];
            basf.displayimgrect(item, new Rectangle(0, ((int)back_y[i]+y_changer)-change_y_position, s_width, s_height));
        }

        basf.displayimgrect(assets.city_texture,new Rectangle(building_x,(building_y+y_changer)-change_y_position,building_width,building_height));

        foreach (var item in building_Blocks)
        {
            if(building_Blocks.IndexOf(item)<building_Blocks.Count-1){
                var x = item.x;
                var y = item.y;
                var width = item.width;
                basf.displayimgrect(assets.block_texture,new Rectangle(x,y-(change_y_position),width,block_height));
            }
        }
        var text = $"Score is {score}";
        var text_size = sfont.MeasureString(text);
        basf.drawtext(sfont,text,new Vector2(s_width/2-110,s_height/2-(text_size.Y/2+40)),Color.DarkBlue,new Vector2(3,3));
        if(is_high_score){
            basf.drawtext(sfont,"HIGHT SCORE!",new Vector2(s_width/2-200,s_height/2-(text_size.Y/2+180)),Color.DarkGreen,new Vector2(4,3));
        }
    }

    public void set_score(int score){
        this.score = score;

        var data = File.ReadAllLines("data/high_score.txt");
        is_high_score = (Convert.ToInt32(data[0])<this.score)?true:false;
        if(is_high_score){
            File.WriteAllText("data/high_score.txt",$"{this.score}");
        }
    }

    public void play_defeat_song(){
        game_over_sound.Play();
    }
    
}