﻿using AutoMapper;
using EcomerceMVC.Data;
using EcomerceMVC.ViewModels;

namespace EcomerceMVC.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<RegisterVM, KhachHang>();
                //.ForMember(kh => kh.HoTen, option => option.MapFrom(RegisterVM => RegisterVM.HoTen))
                //.ReverseMap();

        }
    }
}
