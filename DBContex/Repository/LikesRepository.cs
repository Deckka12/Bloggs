using DBContex.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Collections.Specialized.BitVector32;

namespace DBContex.Repository
{

    public class LikesRepository : ILikesRepository
    {
        private readonly Context _context;

        public LikesRepository(Context context)
        {
            _context = context;
        }

        public async Task AddReactionAsync(Likes reaction)
        {
            await _context.Likes.AddAsync(reaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReactionAsync(int reactionId)
        {
            var reaction = await _context.Likes.FindAsync(reactionId);

            if (reaction != null)
            {
                _context.Likes.Remove(reaction);
           await _context.SaveChangesAsync();
            }
        }

        public async Task<Likes> GetReactionAsync(string userId, int articleId)
        {
            return await _context.Likes.FirstOrDefaultAsync(r => r.UserId == userId && r.ArticleId == articleId);
        }

        public async Task<int> GetLikesCountAsync(int articleId)
        {
            return await _context.Likes.CountAsync(r => r.ArticleId == articleId && r.Type == 1);
        }

        public async Task<int> GetDislikesCountAsync(int articleId)
        {
            return await _context.Likes.CountAsync(r => r.ArticleId == articleId && r.Type == 2);
        }
    }
}

