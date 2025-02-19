﻿using Projekt_Dokumentenablage.Helper;
using Projekt_Dokumentenablage.Models;
using Projekt_Dokumentenablage.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekt_Dokumentenablage.ViewModels
{
    public class ActualMainVM : BaseViewModel, INotifyPropertyChanged
    {
        private BaseViewModel _selectedViewModel;

        private const string VERBINDUNG = @"Data Source=localhost\sqlexpress;Initial Catalog=DocumentFiling;User ID=sa;Password=P@ssword";
        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserName"));
            }
        }
        private string _passsword;
        public string Password
        {
            get { return _passsword; }
            set
            {
                _passsword = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _errorLogin;

        public string ErrorLogin
        {
            get { return _errorLogin; }
            set
            {
                _errorLogin = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ErrorLogin"));
            }

        }


        public ICommand UpdatePage { get; set; }

        public RelayCommand Login { get; set; }

        public ActualMainVM()
        {
            //UserHandler.Instance.Load();

            Login = new RelayCommand((o) =>
            {
                bool check = false;

                MD5 hash = MD5.Create();

                var schluessel1 = hash.ComputeHash(Encoding.UTF8.GetBytes(Password));
                Password = Convert.ToBase64String(schluessel1);

                SqlConnection con = new SqlConnection(VERBINDUNG);

                string sql = $"Select UserName, UserPassword from documentFilingPassword where UserName = '{UserName}' and UserPassword = '{Password}'";

                con.Open();

                SqlCommand com = new SqlCommand(sql, con);
                SqlDataReader reader = com.ExecuteReader();

                while (reader.Read())
                {
                    check = true;
                }

                reader.Close();
                com.Dispose();
                con.Close();

                if (check)
                {
                    Window start = new DocumentFilingView();
                    start.ShowDialog();
                }
                else
                {
                    ErrorLogin = "Wrong Username or Password";
                }

                //if (UserHandler.Instance.GetUsers().Find(u => u.UserName == UserName && u.Password == Password) != null)
                //{
                //    Window start = new DocumentFilingView();
                //    start.ShowDialog();
                //}
                //else
                //{
                //    ErrorLogin = "Wrong Username or Password";
                //}
            });

            UpdatePage = new UpdateViewCommand(this);
        }
    }
}
