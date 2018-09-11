using System.Threading.Tasks;
using CoreServices.Models;
using CoreServices.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CoreServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        [HttpGet]
        [Route("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _postRepository.GetCategories();
            if (categories == null)
            {
                return BadRequest();
            }

            return Ok(categories);
        }

        [HttpGet]
        [Route("GetPosts")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetPosts();
            if (posts == null)
            {
                return BadRequest();
            }

            return Ok(posts);
        }

        [HttpGet]
        [Route("GetPost/{postId}")]        
        public async Task<IActionResult> GetPost(int? postId)
        {
            if (postId == null)
            {
                return NotFound();
            }

            var post = await _postRepository.GetPost(postId);
            if (post == null)
            {
                return BadRequest();
            }

            return Ok(post);
        }

        [HttpPost]
        [Route("AddPost")]
        public async Task<IActionResult> AddPost([FromBody]Post model)
        {
            if (ModelState.IsValid)
            {
                var postId = await _postRepository.AddPost(model);
                if (postId > 0)
                {
                    return Ok(postId);
                }
                else
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }


        [HttpPost]
        [Route("DeletePost")]
        public async Task<IActionResult> DeletePost(int? postId)
        {
            if (postId == null)
            {
                return NotFound();
            }

            await _postRepository.DeletePost(postId);

            return Ok();
        }

        [HttpPost]
        [Route("UpdatePost")]
        public async Task<IActionResult> UpdatePost([FromBody]Post model)
        {
            if (ModelState.IsValid)
            {
                await _postRepository.UpdatePost(model);
                return Ok();
            }

            return BadRequest();
        }


    }
}