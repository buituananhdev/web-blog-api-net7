using AutoMapper;
using WebBlog.Data.DTOs;

namespace WebBlog.API.Config
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Comment, CommentDTO>();
            CreateMap<CommentDTO, Comment>();

            CreateMap<Post, PostDTO>();
            CreateMap<PostDTO, Post>();

            CreateMap<Token, TokenDTO>();
            CreateMap<TokenDTO, Token>();
        }
    }
}
