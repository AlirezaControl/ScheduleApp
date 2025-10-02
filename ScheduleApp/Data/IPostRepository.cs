using System.Collections.Generic;
using GuardScheduler.Models;

namespace GuardScheduler.Data
{
    public interface IPostRepository
    {
        List<Post> GetAll();
        Post GetById(int id);
        int Insert(Post p);
        void Update(Post p);
        void Delete(int id);
    }
}