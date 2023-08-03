namespace webblogapi.Services.VoteService
{
    public interface IVoteService
    {
        Task<bool> VoteUp(Vote vote);
        Task<bool> VoteDown(Vote vote);
    }
}
