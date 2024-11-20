﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyMayMoc.Model
{
    public class Loi : INotifyPropertyChanged
    {
        private string _masp;
        private string _tensp;
        private string _giaban;


        public string MaSanPham
        {
            get => _masp;
            set { _masp = value; OnPropertyChanged(nameof(MaSanPham)); }
        }

        public string TenSanPham
        {
            get => _tensp;
            set { _tensp = value; OnPropertyChanged(nameof(TenSanPham)); }
        }
        public string GiaBan
        {
            get => _giaban;
            set { _giaban = value; OnPropertyChanged(nameof(GiaBan)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
