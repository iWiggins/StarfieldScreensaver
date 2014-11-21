
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using Screensavers;
using System.Collections.Generic;
using System.Threading;

class star
{
    public star()
    {
        Center = new Point();
        scale = 1;
        T = 1;
        MAX_SCALE = Screen.PrimaryScreen.Bounds.Height / 36;
    }

    public star(Point centerOfScreen,Point startingPoint)
    {
        setInitials(centerOfScreen, startingPoint);
        T = 1;
        MAX_SCALE = Screen.PrimaryScreen.Bounds.Height / 36;
    }

    public Rectangle boundingBox
    {
        get
        {
            return new Rectangle(Center, new Size(Scale, Scale));
        }
    }

    public void placeCenter(Point centerOfScreen, Point startingPoint)
    {
        setInitials(centerOfScreen, startingPoint);
    }

    public Point center
    {
        get
        {
            return Center;
        }
    }

    public int scale
    {
        set
        {
            Scale = (value > 0 ? value : 1);
        }
    }

    public int t
    {
        set
        {
            T = value;
            scale = scaler();
        }
    }

    private int scaler()
    {
        return (int)(System.Math.Pow(++T, 2)/100 + 2);
    }

    private void reset()
    {
        if (Scale >= MAX_SCALE)
        {
            Center = initialCenter;
            Scale = 1;
            T = 1; 
        }
    }

    public void move()
    {
        reset();
        scale = scaler();
        Center.X += (int)(T * cosTheta);
        Center.Y += (int)(T * sinTheta);
    }

    private void setInitials(Point centerOfScreen, Point startingPoint)
    {
        Center = initialCenter = startingPoint;

        if (startingPoint.X == centerOfScreen.X)
        {
            if (startingPoint.Y == centerOfScreen.Y)
            {
                Random myRandom = new Random();
                setInitials(centerOfScreen, new Point(centerOfScreen.X + (myRandom.Next(Screen.PrimaryScreen.Bounds.Width/2) * ((myRandom.Next(2) == 1) ? 1 : -1)), centerOfScreen.Y + (myRandom.Next(Screen.PrimaryScreen.Bounds.Height/2) * ((myRandom.Next(2) == 1) ? 1 : -1))));
            }

            else if (startingPoint.Y > centerOfScreen.Y)
            {
                theta = Math.PI / 2;
            }

            else
            {
                theta = 3 * Math.PI / 2;
            }
        }

        else if (startingPoint.Y == centerOfScreen.Y)
        {
            if (startingPoint.X > centerOfScreen.X)
            {
                theta = 0;
            }

            else
            {
                theta = Math.PI;
            }
        }

        else
        {
            theta = ((startingPoint.X-centerOfScreen.X < 0) ? Math.PI : 0) + System.Math.Atan((double)((double)((double)startingPoint.Y - (double)centerOfScreen.Y) / (double)((double)startingPoint.X - (double)centerOfScreen.X)));
        }

        sinTheta = Math.Sin(theta);
        cosTheta = Math.Cos(theta);
    }

    private int Scale;
    private double theta;
    private double cosTheta;
    private double sinTheta;
    private Point Center;
    private Point initialCenter;
    private int T;
    private int MAX_SCALE;
}

class MyCoolScreensaver : Screensaver
{
    public Point centerOfScreen
    {
        get
        {
            return new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
        }
    }

    public List<star> stars = new List<star>();
    public const int numberOfStars = 3000;

    public MyCoolScreensaver()
    {
        Initialize += new EventHandler(MyCoolScreensaver_Initialize);
        Update += new EventHandler(MyCoolScreensaver_Update);
        Exit += new EventHandler(MyCoolScreensaver_Exit);

        Random myRandom = new Random();

        for (int i = 0; i < numberOfStars; ++i)
        {
            stars.Add(new star());
        }
        
    }

    void MyCoolScreensaver_Initialize(object sender, EventArgs e)
    {
        Random myRandom = new Random();

        foreach (star S in stars)
        {
                S.placeCenter(centerOfScreen, new Point(centerOfScreen.X + (myRandom.Next(400) * ((myRandom.Next(2) == 1) ? 1 : -1)), centerOfScreen.Y + (myRandom.Next(400) * ((myRandom.Next(2) == 1) ? 1 : -1))));
                S.t = myRandom.Next(50);
        }

        //To avoid the "Starburst" starting bug.
        for (int i = 0; i < 10; i++)
        {
            foreach (star S in stars)
            {
                S.move();
            } 
        }
    }

    void MyCoolScreensaver_Update(object sender, EventArgs e)
    {
        //Thread.Sleep(1);

        //Black out the screen:
        Graphics0.Clear(Color.Black);

        //Print The stars as they are:

        SolidBrush paintbrush = new SolidBrush(Color.White);

        foreach (star S in stars)
        {
            Graphics0.FillEllipse(paintbrush, S.boundingBox);
        }

        //Update the stars:

        foreach (star S in stars)
        {
            S.move();
        }
    }

    void MyCoolScreensaver_Exit(object sender, EventArgs e)
    {}

    [STAThread]
    static void Main()
    {
        Screensaver ss = new MyCoolScreensaver();
        ss.Run();
    }
}
