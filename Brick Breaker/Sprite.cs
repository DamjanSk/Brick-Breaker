using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brick_Breaker {
    class Sprite {
        private List<Bitmap> images; // The sprite's images.
        private Bitmap frame; // The current image (or frame).
        private int index; // The index of the current frame.
        private double x, y; // Horizontal and vertical position on the screen.
        private double width, height; // Width and height.
        private int opacity, old = 100; // Opacity. Old is for keeping track of whether it has changed.
        private bool destroyed; // Used for bricks.

        
        public Sprite(Bitmap image, int x, int y, int opacity) {
            images = new List<Bitmap>();

            images.Add(Utils.optimizedImage(image));
            frame = images[0];
            index = 0;
            this.x = x;
            this.y = y;
            this.width = image.Width;
            this.height = image.Height;
            this.opacity = opacity;
        }


        // Add a frame to images.
        public void addFrame(Bitmap image) {
            images.Add(image);
        }
        // Change the current frame.
        public void setFrame(int frame) {
            if(index != frame) {
                this.frame = images[frame];
                index = frame;
                old = 100;
            }
        }


        // This method check whether the sprite overlaps a point.
        public bool overlaps(double x, double y) {
            if(x > this.x && x < this.x+this.width && y > this.y && y < this.y+this.height)
                return true;

            return false;
        }
        // This method checks whether the sprite overlaps another sprite.
        public bool overlaps(Sprite s) {
            if(s.overlaps(x, y) || s.overlaps(x+width, y) || s.overlaps(x+width, y+height) || s.overlaps(x, y+height))
                return true;

            return false;
        }


        // Getters.
        public double getX() {
            return x;
        }
        public double getY() {
            return y;
        }
        public double getWidth() {
            return width;
        }
        public double getHeight() {
            return height;
        }
        public int getOpacity() {
            return opacity;
        }
        public bool isDestroyed() {
            return destroyed;
        }


        //Setters
        public void setOpacity(int opacity) {
            if(opacity > 100)
                this.opacity = 100;
            else if(opacity < 0)
                this.opacity = 0;
            else
                this.opacity = opacity;
        }
        public void setX(double x) {
            this.x = x;
        }
        public void setY(double y) {
            this.y = y;
        }
        public void setWidth(double width) {
            this.width = width;
        }
        public void setHeight(double height) {
            this.height = height;
        }
        public void setDestroyed(bool destroyed) {
            this.destroyed = destroyed;
        }


        // This method draws the sprite on the given graphics.
        public void draw(Graphics g) {
            // Only update opacity if it has been changed.
            if(opacity != old) {
                frame = (Bitmap) Utils.ChangeImageOpacity(images[index], opacity/100f);
                old = opacity;
            }

            g.DrawImage(frame, (int) x, (int) y, (int) width, (int) height);
        }
    }
}
