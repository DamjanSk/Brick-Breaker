using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brick_Breaker {
    public partial class Frame : Form {
        private Game game; // The game object controls updating and drawing the scenes.
        private int mouseX, mouseY; // The mouse coordinates.
        private bool mouseClicked, leftDown, rightDown; // Denote if mouse was clicked/left/right arrow is pressed down.


        public Frame() {
            InitializeComponent();
        }


        private void Frame_Paint(object sender, PaintEventArgs e) {
            if(game == null) {
                game = new Game(this, canvas.CreateGraphics());
            }
        }
        private void Frame_FormClosed(object sender, FormClosedEventArgs e) {
            if(game != null) {
                game.quit();
            }
        }

        
        // This method gets the cursor coordinates.
        private void canvas_MouseMove(object sender, MouseEventArgs e) {
            mouseX = e.Location.X;
            mouseY = e.Location.Y;
        }
        // This method checks if the mouse has been clicked.
        private void canvas_MouseUp(object sender, MouseEventArgs e) {
            mouseClicked = true;
        }
        // This method checks if the left or right arrow has been pressed.
        private void Frame_KeyDown(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Left)
                leftDown = true;
            if(e.KeyCode == Keys.Right)
                rightDown = true;
        }
        // This method checks if the left or right arrow has been released.
        private void Frame_KeyUp(object sender, KeyEventArgs e) {
            if(e.KeyCode == Keys.Left)
                leftDown = false;
            if(e.KeyCode == Keys.Right)
                rightDown = false;
        }


        // Getters for the cursor.
        public int getMouseX() {
            return mouseX;
        }
        public int getMouseY() {
            return mouseY;
        }
        public bool getMouseClicked() {
            if(mouseClicked) {
                mouseClicked = false;
                return true;
            }
            return false;
        }


        // Getters for button presses.
        public bool LeftDown() {
            return leftDown;
        }
        public bool RightDown() {
            return rightDown;
        }
    }
}
