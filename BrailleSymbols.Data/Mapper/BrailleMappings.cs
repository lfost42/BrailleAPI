﻿using System;
using AutoMapper;
using BrailleSymbols.Data.Models;
using BrailleSymbols.Data.Models.Dtos;

namespace BrailleSymbols.Data.Mapper
{
    public class BrailleMappings : Profile
    {
        public BrailleMappings()
        {
            CreateMap<SpecialSymbolsModel, SpecialSymbolsModelDto>().ReverseMap();
        }
    }
}