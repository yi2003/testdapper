using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dapper;

namespace TryDapper.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
         
            var customerEntities = new CustomerDb();
            var customers = customerEntities.GetCustomers();
            return customers.Select(c => c.Address).ToList();
        }

    // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }

    public class Post
    {
        public User Owner { get; set; }
    }

    public class User
    {
        
    }

    public class CustomerDb
    {
        public string connectionstring =
            @"Data Source=172.16.101.151;Initial Catalog=xloborelease;user id=XloboRelease;Password=123456;";



        public IList<Customer> GetCustomers()
        {
            using (System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(connectionstring))
            {
                sqlConnection.Open();
                var customer = sqlConnection.Query<Customer>("Select * from Customer");
                return customer.ToList();


              //  var data = sqlConnection.Query<Post, User, Post>("", (post, user) => { post.Owner = user; return post; });
              //  var post = data.First();
            }
        }
    }






}
