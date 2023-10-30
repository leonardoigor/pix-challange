using Api.Repository.Base;
using Microsoft.AspNetCore.Mvc;
using QueueShared.Queues;
using Serilog;
using SharedLibrary.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public BaseRepository<Transaction, Guid> BaseRepository { get; }

        public TransactionController(BaseRepository<Transaction, Guid> baseRepository)
        {
            BaseRepository = baseRepository;

        }

        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<Transaction> Get()
        {
            return BaseRepository.Get().ToList();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public Transaction? Get(Guid id)
        {
            return BaseRepository.GetById(id).FirstOrDefault();
        }

        // POST api/<UserController>
        [HttpPost]
        public Transaction Post([FromBody] Transaction entity)
        {
            using (var transaction = BaseRepository.BeginTransaction(true))
            using (var queue = new ImpactTransaction(false))
            {
                try
                {
                    queue.Run();
                    entity = BaseRepository.Add(entity);
                    BaseRepository.SaveChanges();
                    queue.Send(entity.Id.ToString());

                    Log.Information($"Send to queue {nameof(ImpactTransaction)} {entity.Id}");
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
