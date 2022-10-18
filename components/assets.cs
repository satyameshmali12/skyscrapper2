namespace sky_scrapper2;
public class Assets
{
    Basicfunc basf;
    public Texture2D background1;
    public Texture2D solid_Color;
    public Texture2D block_texture;
    public Texture2D city_texture;   // the texture of the initial building...
    public Texture2D title;
    public Texture2D[] back_imgs;
    public Assets(Game1 game){
        basf = new Basicfunc(game);
        solid_Color = basf.loadtexture("assets/solidcolor");
        block_texture = basf.loadtexture("assets/building");
        city_texture = basf.loadtexture("assets/city");
        title = basf.loadtexture("assets/game_title");

        var back_g = "assets/backgrounds";
        background1 = basf.loadtexture($"{back_g}/back1");
        back_imgs = new Texture2D[2]{basf.loadtexture($"{back_g}/back2"),basf.loadtexture($"{back_g}/back3")};
        
    }   
}