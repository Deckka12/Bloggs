using DBContex.Repository;
using DBContex.Models;
using Bloggs.Models.Request;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bloggs.Controllers
{
    public class LikesController : Controller
    {

        private readonly LikesRepository _reactionRepository;

        public LikesController(LikesRepository reactionRepository)
        {
            _reactionRepository = reactionRepository;
        }

        [HttpPost]
         public async Task<IActionResult> AddReaction(ReactionViewModel reactionViewModel)
         {
             var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
             var reaction = new Likes
             {
                 UserId = userId,
                 ArticleId = reactionViewModel.ArticleId,
                 Type = reactionViewModel.ReactionType
             };

             await _reactionRepository.AddReactionAsync(reaction);

             var likesCount = await _reactionRepository.GetLikesCountAsync(reactionViewModel.ArticleId);
             var dislikesCount = await _reactionRepository.GetDislikesCountAsync(reactionViewModel.ArticleId);

             return Json(new { success = true, likesCount, dislikesCount });
         }
    }
}
