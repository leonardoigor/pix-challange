using Api.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public BaseRepository<User, Guid> BaseRepository { get; }

        public UserController(BaseRepository<User, Guid> baseRepository)
        {
            BaseRepository = baseRepository;

        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return BaseRepository.Get().ToList();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public User? Get(Guid id)
        {
            return BaseRepository.GetById(id).FirstOrDefault();
        }

        // POST api/<UserController>
        [HttpPost]
        public User Post([FromBody] User entity)
        {
            using (var transaction = BaseRepository.BeginTransaction(true))
            {
                try
                {
                    entity = BaseRepository.Add(entity);
                    BaseRepository.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return entity;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
