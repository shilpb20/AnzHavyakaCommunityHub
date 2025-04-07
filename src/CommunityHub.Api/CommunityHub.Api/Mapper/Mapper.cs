using AutoMapper;
using CommunityHub.Core.Dtos;
using CommunityHub.Core.Models;
using CommunityHub.Infrastructure.Models;
using CommunityHub.Infrastructure.Models.Login;
using CommunityHub.Infrastructure.Models.Registration;
using Newtonsoft.Json;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<LoginResponse, LoginResponseDto>().ReverseMap();

        CreateMap<UserInfoCreateDto, UserInfo>().ReverseMap();
        CreateMap<UserInfoDto, UserInfo>().ReverseMap();
        CreateMap<UserInfoDto, UserInfoCreateDto>().ReverseMap();

        CreateMap<SpouseInfoCreateDto, SpouseInfo>().ReverseMap();
        CreateMap<SpouseInfoDto, SpouseInfo>().ReverseMap();
        CreateMap<SpouseInfoDto, SpouseInfoCreateDto>().ReverseMap();

        CreateMap<Child, ChildDto>().ReverseMap();
        CreateMap<Child, ChildCreateDto>().ReverseMap();
        CreateMap<ChildDto, ChildCreateDto>().ReverseMap();

        CreateMap<ContactForm, ContactFormCreateDto>().ReverseMap();
        CreateMap<ContactForm, ContactFormDto>().ReverseMap();

        CreateMap<RegistrationInfoCreateDto, RegistrationInfoDto>().ReverseMap();

        CreateMap<RegistrationInfo, RegistrationInfoDto>().ReverseMap();
        CreateMap<RegistrationInfo, RegistrationInfoCreateDto>().ReverseMap();

        CreateMap<RegistrationRequest, RegistrationRequestDto>()
            .ForMember(dest => dest.RegistrationInfo,
                f => f.MapFrom(src => string.IsNullOrEmpty(src.RegistrationInfo)
                    ? null : JsonConvert.DeserializeObject<RegistrationInfoDto>(src.RegistrationInfo)))
            .ForMember(dest => dest.CreatedAt, f => f.MapFrom(src => src.CreatedAt));

        CreateMap<RegistrationRequestDto, RegistrationRequest>()
            .ForMember(dest => dest.RegistrationInfo,
                f => f.MapFrom(src => src.RegistrationInfo == null
                    ? null : JsonConvert.SerializeObject(src.RegistrationInfo)))
            .ForMember(dest => dest.CreatedAt, f => f.MapFrom(src => src.CreatedAt));
    }
}
