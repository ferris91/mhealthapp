﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

/*
* File name: InitialLoginPage.cs
* 
* @description This class exists to render and control the logic of the initial login page of the primary app. 
*              This page is to contain the initial logic for validating a users login. 
* 
* @author Michael Miller
* @email miller.7594@osu.edu
* @version 03/18/2017
* 
*/


namespace CAPLab
{
    class InitialLoginPage : ContentPage
    {
        Entry osuUsernameField;
        Entry surveyConditionField;
        Label loginStatusMessage;
        User api_SuppliedUser;

        public InitialLoginPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            this.loginStatusMessage = new Label { Text = string.Empty };
            Icon = "logoutIcon.png";
            var title = new Label
            {
                Text = "OSU CAPLab",
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold
            };
            osuUsernameField = new Entry
            {
                Placeholder = "Enter your OSU name.#",
                Keyboard = Keyboard.Plain
            };

            surveyConditionField = new Entry
            {
                Placeholder = "Enter your survey condition here",
                Keyboard = Keyboard.Telephone
            };

            var loginButton = new Button
            {
                Text = "Login"
            };

            var registerButton = new Button
            {
                Text = "First time logging in?"
            };

            var logo = new Image { WidthRequest = 80, HeightRequest = 80 };
            logo.Source = ImageSource.FromFile("osuLoginIcon.png");

            loginButton.Clicked += OnLoginButtonClicked;
            registerButton.Clicked += OnRegisterButtonClicked;

            Content = new StackLayout
            {
                Padding = 30,
                Spacing = 10,
                Children =
                {
                    title,
                    logo,
                    osuUsernameField,
                    surveyConditionField,
                    loginButton,
                    registerButton,
                    loginStatusMessage
                }
            };
        }

        void OnLoginButtonClicked(object sender, EventArgs e)
        {
            if (osuUsernameField.Text == null || 
                osuUsernameField.Text == string.Empty ||
                surveyConditionField.Text == null ||
                surveyConditionField.Text == string.Empty)
            {
                loginStatusMessage.Text = "Login fields cannot be empty.";
            }
            else
            {
                var user = new User
                {
                    osuUsername = osuUsernameField.Text.ToLower(),
                    surveyCondition = surveyConditionField.Text,
                };

                var userIsValid = AreCredentialsCorrect(user);
                if (userIsValid)
                {
                    App.loggedIn = true;
                    if (user.osuUsername.Equals("caplab"))
                    {
                        //The line below creates a homepage behind the current one.
                        Navigation.InsertPageBefore(new HomepageNav(user), this);
                        //The line below removes the current page to reveal the newly created homepage.
                        Navigation.PopAsync();
                    }
                    else
                    {
                        //The line below creates a homepage behind the current one.
                        Navigation.InsertPageBefore(new HomepageNav(api_SuppliedUser), this);
                        //The line below removes the current page to reveal the newly created homepage.
                        Navigation.PopAsync();
                    }

                }
                else
                {
                    loginStatusMessage.Text = "Login failed, check spelling and try again.";
                    surveyConditionField.Text = string.Empty;
                }
            }
        }

        void OnRegisterButtonClicked(object sender, EventArgs e)
        { 
            Navigation.PushAsync(new RegistrationPage());
        }

        protected override bool OnBackButtonPressed()
        {
            //the closers below use the dependency service to ensure that the application exits regardless of platform. 
            //Info in dependency service: https://developer.xamarin.com/guides/xamarin-forms/dependency-service/introduction/
            var closer = DependencyService.Get<ICloseApplication>();
            closer.CloseApp();

            return true;
        }

        bool AreCredentialsCorrect(User user)
        {
            if (user.osuUsername.Equals("caplab"))
            {
                return user.osuUsername == Constants.testUsername && user.surveyCondition == Constants.testSurveyCondition;
            }
            else
            {
               var api_Connector = DependencyService.Get<IAPIConnector>();
               api_SuppliedUser = api_Connector.VerifyLogin(user).Result;
               if (api_SuppliedUser != null)
                {
                    api_SuppliedUser.retrievedFromServer = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
