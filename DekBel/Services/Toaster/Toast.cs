using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dek.Bel.Services.Toaster
{
    public partial class Toast : Form
    {
        private enum AnimationStateEnum
        {
            Started = 0,
            Initializing,
            FadingIn1,
            FadingIn2,
            Middle,
            FadingOut1,
            FadingOut2,
            Ended
        }

        private AnimationStateEnum AnimationState;
        private int MiddleStateCounter = 100;
        private int InitializingStateCounter;

        private const int MinHeight = 5;
        private const int MinWidth = 11;
        private const int MaxHeight = 100;
        private const int MaxWidth = 313;

        private const int MaxInitializingStateCounter = 5;
        private const int MaxMiddleStateCounter = 500;
        private const int AnimationSpeed = 17;

        public Toast(string message, string text)
        {
            InitializeComponent();
            Height = MinHeight;
            Width = MinWidth;
            label3.Text = message;
            label2.Text = text;
            Visible = false;    
        }

        private void Toast_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (AnimationState)
            {
                case AnimationStateEnum.Started:
                    AnimationState = AnimationStateEnum.Initializing;
                    InitializingStateCounter = 0;
                    Visible = true;
                    break;
                case AnimationStateEnum.Initializing:
                    InitializingStateCounter++;
                    if (InitializingStateCounter > MaxInitializingStateCounter)
                    {
                        AnimationState = AnimationStateEnum.FadingIn2;
                    }
                    break;
                case AnimationStateEnum.FadingIn1:
                    Width += AnimationSpeed;
                    if (Width > MaxWidth)
                    {
                        Width = MaxWidth;
                        AnimationState = AnimationStateEnum.FadingIn2;
                    }

                    break;
                case AnimationStateEnum.FadingIn2:
                    Width = MaxWidth;
                    Height += AnimationSpeed;
                    if (Height > MaxHeight)
                    {
                        Height = MaxHeight;
                        AnimationState = AnimationStateEnum.Middle;
                        MiddleStateCounter = 0;
                    }

                    break;
                case AnimationStateEnum.Middle:
                    MiddleStateCounter += AnimationSpeed;
                    if(MiddleStateCounter > MaxMiddleStateCounter)
                    {
                        AnimationState = AnimationStateEnum.FadingOut1;
                    }
                    break;
                case AnimationStateEnum.FadingOut1:
                    Height -= AnimationSpeed;
                    if (Height < MinHeight)
                    {
                        Height = MinHeight;
                        AnimationState = AnimationStateEnum.Ended;
                    }
                    break;
                case AnimationStateEnum.FadingOut2:
                    Width -= AnimationSpeed;
                    if (Width < MinWidth)
                    {
                        AnimationState = AnimationStateEnum.Ended;
                    }
                    break;
                case AnimationStateEnum.Ended:
                    Close();
                    break;
                default:
                    Close();
                    break;
            }
        }

        private void Toast_Load(object sender, EventArgs e)
        {
            AnimationState = AnimationStateEnum.Started;
            timer1.Start();
            Height = MinHeight;
            Width = MinWidth;
        }
    }
}
