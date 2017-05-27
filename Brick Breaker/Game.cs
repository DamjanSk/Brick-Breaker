using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Brick_Breaker {
    class Game {
        // --- Variables ---
        private Frame frame; // A handle to the frame. Needed for mouse&keyboard input.
        private Graphics g; // A handle to the canvas graphics.
        private Bitmap dbBitmap; // Used for double buffering.
        private Graphics dbg; // Used for double buffering.
        private Thread gameThread; // The thread on which the game runs, in order not to freeze the  U.I.
        private long frames, start; // Used for measuring the FPS.
        private long target = 60, start2; // Used for controlling the FPS.
        private String layout, targetLayout; // The current layout. Affects which sprites are updated and drawn.
        private double ballSpeedTotal = 6, ballXSpeed = 4, ballYSpeed = -5, padSpeed = 4; // The speed of the ball and the pad.
        private int count = 0; // Counts the number of destroyed bricks.
        
        // --- Sprites ---
        private Sprite Fade;
        private Sprite BgMain;
        private Sprite BtnPlay;
        private Sprite BtnQuit;

        private Sprite BgPlay;
        private Sprite[,] Bricks;
        private Sprite Ball;
        private Sprite Pad;
        private Sprite Win;
        private Sprite Lose;
        private Sprite BtnBack;


        public Game(Frame frame, Graphics g) {
            this.frame = frame;
            this.g = g;
            gameThread = new Thread(new ThreadStart(run));
            gameThread.Start();
        }
        // This method closes the gameThread and the graphics.
        public void quit() {
            gameThread.Abort();
            g.Dispose();
        }


        private void run() {
            init(); // Initialize the sprites and variables.

            while(true) { // The game loop.
                if(Environment.TickCount - start2 > 1000/target) {
                    update();
                    draw();

                    frames++;
                    start2 += 1000/target;
                }

                // Measure the FPS.
                if(Environment.TickCount - start > 2000) {
                    Console.WriteLine("Current FPS: " + frames/2);
                    frames = 0;
                    start = Environment.TickCount;
                }
            }
        }
        // This method initializes the sprites and variables.
        private void init() {
            dbBitmap = new Bitmap(640, 480);
            dbg = Graphics.FromImage(dbBitmap);
            dbg.InterpolationMode = InterpolationMode.NearestNeighbor;

            frames = 0;
            start = Environment.TickCount;
            start2 = Environment.TickCount;

            layout = targetLayout = "Main";

            Fade = new Sprite(Properties.Resources.Fade, 0, 0, 100);
            BgMain = new Sprite(Properties.Resources.BgMain, 0, 0, 100);
            BtnPlay = new Sprite(Properties.Resources.BtnPlay, 245, 300, 100);
            BtnPlay.addFrame(Properties.Resources.BtnPlayH);
            BtnQuit = new Sprite(Properties.Resources.BtnQuit, 245, 350, 100);
            BtnQuit.addFrame(Properties.Resources.BtnQuitH);

            BgPlay = new Sprite(Properties.Resources.BgPlay, 0, 0, 100);
            Bricks = new Sprite[6,5];
            for(int x=0; x<6; x++) {
                for(int y=0; y<5; y++) {
                    Bricks[x,y] = new Sprite(Properties.Resources.Brick, 25 + x*100, 10 + y*30, 100);
                }
            }
            Ball = new Sprite(Properties.Resources.Ball, 310, 350, 100);
            Pad = new Sprite(Properties.Resources.Pad, 250, 450, 100);
            Win = new Sprite(Properties.Resources.Win, 182, 223, 0);
            Lose = new Sprite(Properties.Resources.Lose, 182, 223, 0);
            BtnBack = new Sprite(Properties.Resources.BtnBack, 240, 280, 0);
            BtnBack.addFrame(Properties.Resources.BtnBackH);

        }
        // This method controls the game logic.
        private void update() {
            if(layout.Equals("Main")) {
                
                // Highlighting buttons.
                if(BtnPlay.overlaps(frame.getMouseX(), frame.getMouseY()))
                    BtnPlay.setFrame(1);
                else
                    BtnPlay.setFrame(0);

                if(BtnQuit.overlaps(frame.getMouseX(), frame.getMouseY()))
                    BtnQuit.setFrame(1);
                else
                    BtnQuit.setFrame(0);

                // Clicking buttons.
                if(frame.getMouseClicked()) {
                    // Quit button.
                    if(BtnQuit.overlaps(frame.getMouseX(), frame.getMouseY()))
                        Environment.Exit(0);
                    
                    //Play button.
                    if(BtnPlay.overlaps(frame.getMouseX(), frame.getMouseY())) {
                        targetLayout = "Play";
                        BtnPlay.setDestroyed(true);
                    }
                }
                // Fancy onclick animation
                if(BtnPlay.isDestroyed() && BtnPlay.getHeight() > 0.4) {
                        BtnPlay.setWidth(BtnPlay.getWidth() + 2);
                        BtnPlay.setX(BtnPlay.getX() - 1);
                        double dist = BtnPlay.getHeight() - BtnPlay.getHeight()/1.2;
                        BtnPlay.setHeight(BtnPlay.getHeight()/1.2);
                        BtnPlay.setY(BtnPlay.getY() + dist/2);
                }

                // Layout transitioning.
                if(targetLayout.Equals("Main"))
                    Fade.setOpacity(Fade.getOpacity() - 4);
                else {
                    Fade.setOpacity(Fade.getOpacity() + 4);
                    if(Fade.getOpacity() >= 100)
                        layout = targetLayout;
                }
            }
            
            if(layout.Equals("Play")) {
                
                // Controlling the pad.
                if(frame.LeftDown())
                    Pad.setX(Pad.getX() - padSpeed);
                if(frame.RightDown())
                    Pad.setX(Pad.getX() + padSpeed);
                if(Pad.getX() < 6)
                    Pad.setX(6);
                if(Pad.getX() > 490)
                    Pad.setX(490);

                // Moving the ball.
                Ball.setX(Ball.getX() + ballXSpeed);
                Ball.setY(Ball.getY() + ballYSpeed);
                if(Ball.getX() < 6) {
                    Ball.setX(6);
                    ballXSpeed = -ballXSpeed;
                }
                if(Ball.getX() > 614) {
                    Ball.setX(614);
                    ballXSpeed = -ballXSpeed;
                }
                if(Ball.getY() < 6) {
                    Ball.setY(6);
                    ballYSpeed = -ballYSpeed;
                }
                if(Ball.getY() > 600) {
                    count = -1;
                }
                if(Ball.overlaps(Pad)) {
                    double d = ((Ball.getX() + Ball.getWidth()/2f) - (Pad.getX() + Pad.getWidth()/2f)) / (Pad.getWidth()/2f);
                    ballXSpeed = 0.7*ballSpeedTotal*d;
                    ballYSpeed = - Math.Sqrt(ballSpeedTotal*ballSpeedTotal - ballXSpeed*ballXSpeed);
                    Ball.setY(Pad.getY() - 20);
                }
                foreach(Sprite brick in Bricks) {
                    if(!brick.isDestroyed() && Ball.overlaps(brick)) {
                        brick.setDestroyed(true);
                        if((Ball.getX() < brick.getX() || Ball.getX()+Ball.getWidth() > brick.getX()+brick.getWidth()) && 
                            (Ball.getY() < brick.getY()+brick.getHeight()-4 && Ball.getY()+Ball.getHeight() > brick.getY()+4))
                            ballXSpeed = -ballXSpeed;
                        else
                            ballYSpeed = -ballYSpeed;
                        count++;
                        // Push the ball a bit.
                        Ball.setX(Ball.getX() + ballXSpeed);
                        Ball.setY(Ball.getY() + ballYSpeed);
                    }
                }

                // Fade destroyed bricks.
                foreach(Sprite brick in Bricks) {
                    if(brick.isDestroyed())
                        brick.setOpacity(brick.getOpacity() - 4);
                }

                // Check for victory.
                if(count == 30) {
                    ballXSpeed = 0;
                    ballYSpeed = 0;
                    Win.setOpacity(Win.getOpacity() + 1);
                    BtnBack.setOpacity(BtnBack.getOpacity() + 1);
                    Ball.setOpacity(Ball.getOpacity() - 1);
                }

                // Check for loss.
                if(count == -1) {
                    Lose.setOpacity(Lose.getOpacity() + 1);
                    BtnBack.setOpacity(BtnBack.getOpacity() + 1);
                }

                // Highlighting buttons.
                if(BtnBack.overlaps(frame.getMouseX(), frame.getMouseY()))
                    BtnBack.setFrame(1);
                else
                    BtnBack.setFrame(0);

                // Clicking buttons.
                if(frame.getMouseClicked()) {
                    if(BtnBack.overlaps(frame.getMouseX(), frame.getMouseY())) {
                        targetLayout = "Main";
                        // Reset the play button.
                        BtnPlay.setX(245);
                        BtnPlay.setY(300);
                        BtnPlay.setWidth(160);
                        BtnPlay.setHeight(34);
                        BtnPlay.setDestroyed(false);
                    }
                }

                // Layout transitioning.
                if(targetLayout.Equals("Play"))
                    Fade.setOpacity(Fade.getOpacity() - 4);
                else {
                    Fade.setOpacity(Fade.getOpacity() + 4);
                    if(Fade.getOpacity() >= 100) {
                        layout = targetLayout;
                        //Reset the play screen.
                        count = 0;
                        foreach(Sprite brick in Bricks) {
                            brick.setOpacity(100);
                            brick.setDestroyed(false);
                        }
                        Lose.setOpacity(0);
                        Win.setOpacity(0);
                        BtnBack.setOpacity(0);
                        Ball.setX(310);
                        Ball.setY(350);
                        ballXSpeed = 4;
                        ballYSpeed = -5;
                        Ball.setOpacity(100);
                    }
                }
            }
        }
        // This method draws the sprites to the canvas.
        private void draw() {
            if(layout.Equals("Main")) {
                BgMain.draw(dbg);
                BtnPlay.draw(dbg);
                BtnQuit.draw(dbg);
                Fade.draw(dbg);
            }
            if(layout.Equals("Play")) {
                BgPlay.draw(dbg);
                foreach(Sprite s in Bricks) {
                    s.draw(dbg);
                }
                Ball.draw(dbg);
                Pad.draw(dbg);
                Win.draw(dbg);
                Lose.draw(dbg);
                BtnBack.draw(dbg);
                Fade.draw(dbg);
            }

            g.DrawImage(dbBitmap, 0, 0);
        }
    }
}
