using AutoMapper;
using WebBlog.Data.DTOs;

namespace WebBlog.API.Config
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
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
