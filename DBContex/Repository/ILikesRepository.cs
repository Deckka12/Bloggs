using DBContex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DBContex.Repository
{
    internal interface ILikesRepository
    {
        Task AddReactionAsync(Likes reaction);
        Task DeleteReactionAsync(int reactionId);
        Task<Likes> GetReactionAsync(string userId, int articleId);
        Task<int> GetLikesCountAsync(int articleId);
        Task<int> GetDislikesCountAsync(int articleId);
    }
}
