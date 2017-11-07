using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DSPMediaPlayer
{
    public partial class ImagePanel : UserControl
    {
        public ImagePanel()
        {
            InitializeComponent();

            // Set the value of the double-buffering style bits to true.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
              ControlStyles.UserPaint | ControlStyles.ResizeRedraw |
              ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            
        }
        
        int viewRectWidth, viewRectHeight; // view window width and height

        float zoom = 1.0f;
        public float Zoom
        {
            get { return zoom; }
            set
            {
                if (value < 0.001f) value = 0.001f;
                zoom = value;

                displayScrollbar();
                setScrollbarValues();
                Invalidate();
            }
        }

        Size canvasSize = new Size(60,40);
        public Size CanvasSize
        {
            get { return canvasSize; }
            set
            {
                canvasSize = value;
                //displayScrollbar();
                setScrollbarValues();
                Invalidate();
            }
        }

        Bitmap image;
        public Bitmap Image
        {
            get { return image; }
            set 
            {
                image = value;
                //displayScrollbar();
                setScrollbarValues(); 
                Invalidate();
            }
        }

        InterpolationMode interMode = InterpolationMode.HighQualityBicubic;
        public InterpolationMode InterpolationMode
        {
            get{return interMode;}
            set{interMode=value;}
        }

        protected override void OnLoad(EventArgs e)
        {
            //displayScrollbar();
            setScrollbarValues();
            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            //displayScrollbar();
            setScrollbarValues();
            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
             base.OnPaint(e);

            //draw image
            if(image!=null)
            {
                Rectangle srcRect,distRect;
                Point pt=new Point((int)(hScrollBar1.Value/zoom),(int)(vScrollBar1.Value/zoom));
                if (DSPMediaPlayer_Form.View_Mode.Equals(ViewMode.enOrigFrameSize))
                {
                    
                    srcRect = new Rectangle(pt, new Size((int)(image.Width / zoom), (int)(image.Height / zoom))); 
                    distRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height); // the center of apparent image is on origin


                    e.Graphics.DrawImage(image, distRect, srcRect, GraphicsUnit.Pixel);
                    displayScrollbar();
                   // setScrollbarValues();

                }
                else
                {
                    vScrollBar1.Visible = false;
                    hScrollBar1.Visible = false;

                    int NewHeight = this.image.Height * this.Width / this.image.Width;
                    int NewWidth = this.Width;
                    if (NewHeight > this.Height)
                    {
                        // Resize with height instead
                        NewWidth = this.image.Width * this.Height / this.image.Height;
                        NewHeight = this.Height;
                    }
                    srcRect = new Rectangle(0, 0, NewWidth, NewHeight);  
                   
                    distRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height); // the center of apparent image is on origin
                    System.Drawing.Image NewImage = this.image.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

                    e.Graphics.InterpolationMode = interMode;

                    e.Graphics.DrawImage(NewImage, (-srcRect.Width + this.Width) / 2, (-srcRect.Height + this.Height) / 2, NewWidth, NewHeight);
                }
            }

        }

        private void displayScrollbar()
        {
            viewRectWidth = this.Width;
            viewRectHeight = this.Height;

            if (image != null) canvasSize = image.Size;

            // If the zoomed image is wider than view window, show the HScrollBar and adjust the view window
            if (viewRectWidth > canvasSize.Width*zoom)
            {
                hScrollBar1.Visible = false;
                viewRectHeight = Height;
            }
            else
            {
                hScrollBar1.Visible = true;
                viewRectHeight = Height - hScrollBar1.Height;
            }

            // If the zoomed image is taller than view window, show the VScrollBar and adjust the view window
            if (viewRectHeight > canvasSize.Height*zoom)
            {
                vScrollBar1.Visible = false;
                viewRectWidth = Width;
            }
            else
            {
                vScrollBar1.Visible = true;
                viewRectWidth = Width - vScrollBar1.Width;
            }

            // Set up scrollbars
            hScrollBar1.Location = new Point(0, Height - hScrollBar1.Height);
            hScrollBar1.Width = viewRectWidth;
            vScrollBar1.Location = new Point(Width - vScrollBar1.Width, 0);
            vScrollBar1.Height = viewRectHeight;
        }

        private void setScrollbarValues()
        {
            // Set the Maximum, Minimum, LargeChange and SmallChange properties.
            this.vScrollBar1.Minimum = 0;
            this.hScrollBar1.Minimum = 0;

            // If the offset does not make the Maximum less than zero, set its value. 
            if ((canvasSize.Width * zoom - viewRectWidth) > 0)
            {
                this.hScrollBar1.Maximum =(int)( canvasSize.Width * zoom) - viewRectWidth;
            }
            // If the VScrollBar is visible, adjust the Maximum of the 
            // HSCrollBar to account for the width of the VScrollBar.  
            if (this.vScrollBar1.Visible)
            {
                this.hScrollBar1.Maximum += this.vScrollBar1.Width;
            }
            this.hScrollBar1.LargeChange = this.hScrollBar1.Maximum / 10;
            this.hScrollBar1.SmallChange = this.hScrollBar1.Maximum / 20;

            // Adjust the Maximum value to make the raw Maximum value 
            // attainable by user interaction.
            this.hScrollBar1.Maximum += this.hScrollBar1.LargeChange;

            // If the offset does not make the Maximum less than zero, set its value.    
            if ((canvasSize.Height * zoom - viewRectHeight) > 0)
            {
                this.vScrollBar1.Maximum = (int)(canvasSize.Height * zoom) - viewRectHeight;
            }

            // If the HScrollBar is visible, adjust the Maximum of the 
            // VSCrollBar to account for the width of the HScrollBar.
            if (this.hScrollBar1.Visible)
            {
                this.vScrollBar1.Maximum += this.hScrollBar1.Height;
            }
            this.vScrollBar1.LargeChange = this.vScrollBar1.Maximum / 10;
            this.vScrollBar1.SmallChange = this.vScrollBar1.Maximum / 20;

            // Adjust the Maximum value to make the raw Maximum value 
            // attainable by user interaction.
            this.vScrollBar1.Maximum += this.vScrollBar1.LargeChange;
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            this.Invalidate();
        }
    }
}
