namespace sky_scrapper2
{
    public class Modified_Text
    {

        Game1 devconfig;
        Basicfunc basf;
        SpriteFont sfont;

        public string text;

        public int one_char_width;  // size of the text element 
        bool is_to_fit_in_given_area;

        bool is_hovered, is_clicked;
        Color initial_color, hover_color, clicked_color;

        public int x,y,width,height,y_changer,rotation;

        int height_range;


        ArrayList text_list;          // if user wanted to fit in the given area


        public Modified_Text(Game1 game, string url_to_font, string text, int x, int y, Color initial_color, Color hover_color, Color clicked_color, int width, int one_char_width = 1, int one_char_height = 1, bool is_to_fit_in_given_area = true, int spacing = 10)
        {

            this.devconfig = game;
            this.basf = new Basicfunc(this.devconfig);
            this.sfont = this.basf.loadfont(url_to_font);


            this.text = text;

            this.text_list = new ArrayList();


            this.x = x;
            this.y = y;
            this.height_range = (int)sfont.MeasureString(this.text).Y;

            this.one_char_width = one_char_width;
            this.height = one_char_height;

            this.rotation = 0;

            this.is_clicked = false;
            this.is_hovered = false;

            this.initial_color = initial_color;
            this.hover_color = hover_color;
            this.clicked_color = clicked_color;


            this.y_changer = 0;



            this.is_to_fit_in_given_area = is_to_fit_in_given_area;

            if (this.is_to_fit_in_given_area)
            {
                this.width = width;
                set_text_list();
            }
            else
            {
                // this.width = (int)sfont.MeasureString(this.text).X+(int)(this.text.Length*this.sfont.Spacing);
                this.width = (int)sfont.MeasureString(this.text).X;
            }


        }

        public void update(MouseState ms)
        {

            var l_but_pressed = ms.LeftButton == ButtonState.Pressed;
            is_hovered = false;
            is_clicked = false;


            if (this.is_mouse_over_button(ms) && !l_but_pressed)
            {
                is_hovered = true;
            }
            else if (this.is_mouse_over_button(ms) && l_but_pressed)
            {
                is_clicked = true;
            }

        }

        public void draw()
        {
            var initial_x = x;
            var d_b_c = (int)(width / text.Length);
            if (is_to_fit_in_given_area)
            {
                foreach (var item in text_list)
                {
                    draw_text(Convert.ToString(item), initial_x);
                    initial_x += d_b_c;
                }
            }
            else
            {
                // basf.beginblend(sfont,)
                draw_text((string)text, initial_x);
            }

        }


        private void set_text_list()
        {
            for (int i = 0; i < text.Length; i++)
            {
                text_list.Add(text[i]);
            }

        }

        private void draw_text(string text, int x_pos)
        {
            basf.drawtext(sfont, text, new Vector2(x_pos, this.y-this.y_changer), (is_hovered) ? hover_color : (is_clicked) ? clicked_color : initial_color, new Vector2(one_char_width, this.height),rotation:(float)this.rotation);
        }

        public bool is_mouse_over_button(MouseState ms)
        {
            return ms.X > x && ms.X < x + this.width && ms.Y > y && ms.Y < y + this.height_range;
        }

        // use this method if not finnded the rnage_height accurate
        // if using this mehtod you have to amnually or experimentally find the range_height
        public void set_range_height(int height_range){
            this.height_range = height_range;
        }

    }
}