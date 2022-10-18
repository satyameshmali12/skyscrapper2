namespace sky_scrapper2;

class Home_Screen:Parent_Component
{
    Game1 devconfig;
    Basicfunc basf;
    
    // all the textures
    Assets assets;

    public int b_h_x;
    int block_speed;

    // SpriteFont sfont;

    public Home_Screen(Game1 game){
        devconfig = game;
        basf = new Basicfunc(devconfig);

        assets = new Assets(devconfig);

        
        
        b_h_x = 10;
        block_speed = 8;


    }

    public override void update(MouseState ms){
        b_h_x+=block_speed;
        if(b_h_x<=0){
            block_speed = 8;
        }
        else if(b_h_x+186>=devconfig._graphics.PreferredBackBufferWidth){
            block_speed = -8;
        }
    }

    public override void draw(){
        var graphics = devconfig._graphics;
        var s_width = graphics.PreferredBackBufferWidth;
        var s_height = graphics.PreferredBackBufferHeight;
        // basf.displayimgrect(background1,new Rectangle(0,0,s_width,s_height));
        basf.displayimgrect(assets.solid_Color,new Rectangle(0,0,s_width,s_height),Color.Black);


        var building_width = s_width / 2 + 200; // the value 200 here is finded experimentally
        var building_height = s_height / 2;
        var building_x = s_width / 2 - building_width / 2;
        var building_y = s_height - (building_height);

        var pathway_height = 40;
        var block_width = 186;


        basf.displayimgrect(assets.solid_Color,new Rectangle(b_h_x,0,block_width,s_height),Color.LightYellow);
        basf.displayimgrect(assets.city_texture, new Rectangle(building_x, building_y, building_width, building_height));

        basf.displayimgrect(assets.solid_Color, new Rectangle(0, 0, s_width, pathway_height), Color.LightGray);
        basf.displayimgrect(assets.solid_Color, new Rectangle(b_h_x, 0, block_width, pathway_height), Color.DarkCyan);

        basf.displayimgrect(assets.block_texture, new Rectangle(b_h_x,pathway_height,block_width,100));

        basf.displayimgrect(assets.title,new Rectangle(s_width/2-((s_width/2)/2),s_height/2-150,s_width/2,300));

        
        
    }
    
    
}