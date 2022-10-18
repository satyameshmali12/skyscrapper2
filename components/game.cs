#region description
// Game
// this file control's the main screen for the game
// in other word's this file contain's the main game code
// all the game level all control from here
#endregion


namespace sky_scrapper2;
// this.target_x = this.devconfig._graphics.PreferredBackBufferWidth / 2 - block_width / 2 + 28;
// this.target_y = this.devconfig._graphics.PreferredBackBufferHeight / 2 + block_height + initial_y_spacing;
// var building_x = graphics.PreferredBackBufferWidth / 2 - building_width / 2;
// var building_y = graphics.PreferredBackBufferHeight - (building_height);

class Main_Game_Screen : Parent_Component
{


    public const int block_height = 100;
    public const int pathway_block_width = 186;
    public const int pathway_height = 40;
    public const int pathway_block_speed = 6;
    public const int block_fall_speed = 10;
    
    public const int initial_y_spacing = 124;  // this value is finded experimentally  to add the initial block in the building block's list.
    public const int f_b_x_moving_speed = 8;
    public const int f_b_y_moving_speed = 10;

    // loading the textures

    Assets assets;

    SoundEffectInstance sound1;
    SoundEffectInstance poof;

    Game1 devconfig;
    Basicfunc basf;


    SpriteFont sfont; // getting the font over here

    public int target_x, target_y;

    public int b_h_x, b_h_x_speed; // block holder x

    public int d_b_x, d_b_y, d_b_width;  // drop_box       in other word's the block which is falling down
    public bool is_d_b_is_settled;


    // all the block related varaible
    public int block_x, block_y, block_width;
    public bool is_to_show_block;


    public List<Building_Block> building_Blocks;

    // all the booleans
    public bool is_block_dropped, is_to_perfrom_transtion;
    public bool is_game_over;
    public int ini_length; // the number of initail blocks without any transition


    Modified_Text text1;




    int add_on_x,add_on_y,moving_block_x;

    public int score;

    // List<Texture2D> 
    public ArrayList back_y,backgrounds;

    Random rand;







    public int changed_height, y_changer;
    public Main_Game_Screen(Game1 game,int b_h_x)
    {
        devconfig = game;
        basf = new Basicfunc(this.devconfig);
        rand = new Random();

        assets = new Assets(devconfig);

        sound1 = basf.get_load_instance("assets/click",false);
        poof = basf.get_load_instance("assets/poof",false);


        sfont = basf.loadfont("assets/fonts/simple_font");


        block_width = pathway_block_width;

        var graphics = devconfig._graphics;

        target_x = graphics.PreferredBackBufferWidth / 2 - block_width / 2 + 28;
        target_y = graphics.PreferredBackBufferHeight / 2 + block_height + initial_y_spacing;


        this.b_h_x = b_h_x;
        b_h_x_speed = pathway_block_speed;

        d_b_x = 0;
        d_b_y = 0;
        d_b_width = 0;

        set_block_x();
        block_y = pathway_height;

        this.is_to_perfrom_transtion = false;
        this.is_d_b_is_settled = false;

        is_block_dropped = false;
        is_to_show_block = true;

        this.building_Blocks = new List<Building_Block>();

        changed_height = 0;
        y_changer = 0;


        is_game_over = false;
        ini_length = 2;


        text1 = new Modified_Text(devconfig,"assets/fonts/simple_font","No of Block's",20,graphics.PreferredBackBufferHeight-250,Color.DarkSlateBlue,Color.DarkCyan,Color.Aqua,150,2,2,is_to_fit_in_given_area:false);

        add_on_x = graphics.PreferredBackBufferWidth-310;
        add_on_y = graphics.PreferredBackBufferHeight-200;
        moving_block_x = add_on_x;

        score = 0;

        back_y = new ArrayList(){0};
        backgrounds = new ArrayList(){assets.background1};



    }

    public override void update(MouseState ms)
    {
        
        var last_elem_y = (int)back_y[back_y.Count-1];
        var s_height = devconfig._graphics.PreferredBackBufferHeight;
        if(last_elem_y+y_changer>=0){
            back_y.Add(last_elem_y-s_height);
            backgrounds.Add(assets.back_imgs[rand.Next(0,assets.back_imgs.Length-1)]);
        }
        text1.update(ms);
        
        var ks = Keyboard.GetState();
        if (ks.IsKeyDown(Keys.Space))
        {
            if(!is_block_dropped){
                this.is_block_dropped = true;
                sound1.Play();
            }
        }



        var graphics = this.devconfig._graphics;
        if (!is_to_perfrom_transtion)
        {
            if (!is_block_dropped)
            {
                if (b_h_x + pathway_block_width > graphics.PreferredBackBufferWidth || b_h_x < 0)
                {
                    b_h_x_speed = -b_h_x_speed;
                }
                b_h_x += b_h_x_speed;
                set_block_x();
            }
            else
            {
                if (block_y + block_height + block_fall_speed < target_y)
                {
                    block_y += block_fall_speed;
                }
                else
                {
                    var incre = target_y - (block_y + block_height);
                    block_y += (incre < 1) ? 0 : incre;
                    is_to_perfrom_transtion = true;
                    d_b_y = block_y;
                    if (block_x + block_width < target_x)
                    {
                        // this.devconfig.Exit();
                        is_game_over = true;
                    }
                    else if (block_x > target_x + block_width)
                    {
                        // this.devconfig.Exit();
                        is_game_over = true;
                    }
                    else if (block_x < target_x)
                    {
                        d_b_width = target_x - block_x;
                        block_width -= d_b_width;
                        d_b_x = block_x;
                        block_x = target_x;
                    }
                    else
                    {
                        d_b_width = block_x - target_x;
                        d_b_x = block_x + (block_width - d_b_width);
                        block_width -= d_b_width;
                        target_x += d_b_width;

                        if(block_width<0){
                            is_game_over = true;
                        }
                    }
                    if(!is_game_over){
                        score+=block_width/2;
                    }
                    building_Blocks.Add(new Building_Block(block_x, block_y, block_width));
                    is_to_show_block = false;
                    poof.Play();

                }
            }
        }
        else
        {
            d_b_y += block_fall_speed + 2;
            if (d_b_y > devconfig._graphics.PreferredBackBufferHeight)
            {
                if (building_Blocks.Count >= ini_length)
                {
                    var increment = block_height / 100 * 10;
                    if (changed_height + increment < block_height)
                    {
                        changed_height += increment;
                        increment_blocks_y(increment);
                    }
                    else
                    {
                        var lefted_amount = block_height - changed_height;
                        increment_blocks_y(lefted_amount);
                        changed_height = 0;
                        reset();
                    }
                }
                else
                {
                    target_y = block_y;
                    reset();
                }
            }
        }

    }
    public override void draw()
    {
        var graphics = this.devconfig._graphics;
        var s_width = graphics.PreferredBackBufferWidth;
        var s_height = graphics.PreferredBackBufferHeight;

        // basf.displayimgrect(assets.background1, new Rectangle(0, 0, s_width, s_height)); // displaying the initial background 
        for (var i = 0; i < backgrounds.Count; i++)
        {
            Texture2D item = (Texture2D)backgrounds[i];
            basf.displayimgrect(item, new Rectangle(0, (int)back_y[i]+y_changer, s_width, s_height));
        }

        basf.displayimgrect(assets.solid_Color, new Rectangle(0, 0, s_width, pathway_height), Color.LightGray);
        basf.displayimgrect(assets.solid_Color, new Rectangle(b_h_x, 0, pathway_block_width, pathway_height), Color.DarkCyan);

        var building_width = s_width / 2 + 200; // the value 200 here is finded experimentally
        var building_height = s_height / 2;
        var building_x = s_width / 2 - building_width / 2;
        var building_y = s_height - (building_height);

        if (!is_to_perfrom_transtion && is_to_show_block)
        {
            basf.displayimgrect(assets.solid_Color, new Rectangle(block_x, block_y, block_width, s_height), Color.LightYellow);
        }


        basf.displayimgrect(assets.city_texture, new Rectangle(building_x, building_y + y_changer, building_width, building_height));


        if (is_to_show_block)
        {
            basf.displayimgrect(assets.block_texture, new Rectangle(block_x, block_y, block_width, block_height));
        }
        // basf.displayimgrect(assets.block_texture,new Rectangle(target_x,target_y-block_height,block_width,block_height));

        if (is_to_perfrom_transtion)
        {
            basf.displayimgrect(assets.block_texture, new Rectangle(d_b_x, d_b_y, d_b_width, block_height));
        }

        foreach (var item in building_Blocks)
        {
            var x = item.x;
            var y = item.y;
            var width = item.width;
            basf.displayimgrect(assets.block_texture, new Rectangle(x, y, width, block_height));
        }


        text1.draw();

        var no_of_blocks = building_Blocks.Count;
        var text_x = 20+sfont.MeasureString("No of Blocks").X/2-sfont.MeasureString($"{building_Blocks.Count}").X/2;
        
        basf.drawtext(sfont,$"{no_of_blocks}",new Vector2(text_x,s_height-200),Color.Black,new Vector2(2.5f,2.5f));

        // to avoid the confusion of the add_on variables and other stuff the logic of the add_on is writtened in the draw method
        if(block_width>10){
            var a_o_x = add_on_x;
            var a_o_height = 50;

            var total_width = 300;

            moving_block_x = total_width/100*(block_x*100/s_width);

            var first_box_width = total_width/100*(target_x*100/s_width);
            basf.displayimgrect(assets.solid_Color,new Rectangle(a_o_x,add_on_y,first_box_width,a_o_height),Color.LightSkyBlue);
            a_o_x+=first_box_width;

            var is_over_the_block = (Math.Abs(add_on_x+moving_block_x-a_o_x)<5)?true:false;

            var second_box_width = total_width/100*(block_width*100/s_width);
            basf.displayimgrect(assets.solid_Color,new Rectangle(a_o_x,add_on_y,second_box_width,a_o_height),Color.LightGreen);
            a_o_x+=second_box_width;
            

            basf.displayimgrect(assets.solid_Color,new Rectangle(a_o_x,add_on_y,(total_width-(first_box_width+second_box_width)),a_o_height),Color.LightSkyBlue);

            basf.displayimgrect(assets.block_texture,new Rectangle(add_on_x+moving_block_x,add_on_y+a_o_height,second_box_width,a_o_height),(is_over_the_block)?Color.LightCoral:Color.Pink);



        }

        basf.drawtext(sfont,$"Score {score}",new Vector2(s_width-200,s_height-340),Color.Red,new Vector2(2,2)); // displaying the scorez


    }

    private void set_block_x()
    {
        block_x = b_h_x + pathway_block_width / 2 - block_width / 2;
    }

    public void reset()
    {
        is_to_perfrom_transtion = false;
        is_block_dropped = false;
        block_x = b_h_x;
        block_y = pathway_height;
        is_d_b_is_settled = false;
        is_to_perfrom_transtion = false;
        is_to_show_block = true;
    }

    private void increment_blocks_y(int value) // this function will chagne the position of the blocks
    {
        foreach (var item in building_Blocks)
        {
            item.y += value;
        }
        y_changer += value;
    }
    

}
