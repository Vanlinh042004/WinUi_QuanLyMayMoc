﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace QuanLyMayMoc
{
    public class Project
    {
        [JsonProperty("MaDuAn")]
        private string _id;

        [JsonProperty("TenDuAn")]
        private string _name;

        [JsonProperty("NgayThucHien")]
        DateTime _timeCreate;
        public string ID
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(ID)); }
        }
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        public DateTime TimeCreate
        {
            get => _timeCreate;
            set { _timeCreate = value; OnPropertyChanged(nameof(TimeCreate)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
