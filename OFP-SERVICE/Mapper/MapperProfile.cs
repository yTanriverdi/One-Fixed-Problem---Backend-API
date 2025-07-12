using AutoMapper;
using OFP_CORE.Entities;
using OFP_CORE.Entities.Likes;
using OFP_SERVICE.DTOs.CreateDTOs;
using OFP_SERVICE.DTOs.CreateDTOs.Likes;
using OFP_SERVICE.DTOs.CreateDTOs.Reports;
using OFP_SERVICE.DTOs.UpdateDTOs;
using OFP_SERVICE.DTOs.UpdateDTOs.Likes;
using OFP_SERVICE.DTOs.UpdateDTOs.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFP_SERVICE.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // PROBLEM
            CreateMap<ProblemCreateDTO, Problem>().ReverseMap();
            CreateMap<ProblemUpdateDTO, Problem>().ReverseMap();


            // ANSWER
            CreateMap<AnswerCreateDTO, Answer>().ReverseMap();
            CreateMap<AnswerUpdateDTO, Answer>().ReverseMap();


            // COMMENT
            CreateMap<CommentCreateDTO, Comment>().ReverseMap();
            CreateMap<CommentUpdateDTO, Comment>().ReverseMap();

            // SUGGEST
            CreateMap<SuggestCreateDTO, Suggest>().ReverseMap();

            // LIKES
            CreateMap<AnswerLikeCreateDTO, AnswerLike>().ReverseMap();
            CreateMap<AnswerLikeUpdateDTO, AnswerLike>().ReverseMap();

            CreateMap<CommentLikeCreateDTO, CommentLike>().ReverseMap();
            CreateMap<CommentLikeUpdateDTO, CommentLike>().ReverseMap();
            
            CreateMap<ProblemLikeCreateDTO, ProblemLike>().ReverseMap();
            CreateMap<ProblemLikeUpdateDTO, ProblemLike>().ReverseMap();

            // REPORTS
            CreateMap<ReportAnswerCreateDTO, ReportAnswer>().ReverseMap();
            CreateMap<ReportAnswerUpdateDTO, ReportAnswer>().ReverseMap();

            CreateMap<ReportCommentCreateDTO, ReportComment>().ReverseMap();
            CreateMap<ReportCommentUpdateDTO, ReportComment>().ReverseMap();

            // MAIL
            CreateMap<MailCreateDTO, Mail>().ReverseMap();


            // BASEUSER
            CreateMap<UserUpdateDTO, BaseUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ReverseMap();
        }
    }
}
