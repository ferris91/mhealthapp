﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

//This page is to contain the specific exercise stats generated by the app. 

namespace CAPLab
{
    class ExerciseStats : ContentPage
    {
        public ExerciseStats()
        {
            var placeholderText = new Label
            {
                Text = "This page is to contain the specific exercise stats generated by the app."
            };
            Content = new StackLayout
            {
                Children =
                {
                    placeholderText
                }
            };
        }
    }
}