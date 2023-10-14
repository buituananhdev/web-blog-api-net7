using Microsoft.Extensions.DependencyInjection;
using WebBlog.Service.Services.AuthService;
using WebBlog.Service.Services.CommentService;
using WebBlog.Service.Services.FileService;
using WebBlog.Service.Services.FollowerService;
using WebBlog.Service.Services.MessageService;
using WebBlog.Service.Services.PostService;
using WebBlog.Service.Services.UserService;
using WebBlog.Service.Services.VoteService;

namespace WebBlog.Service
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddWebBlogService(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFollowerService, FollowerService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IVoteService, VoteService>();
            return services;
        }
    }
}
