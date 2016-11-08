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
        //public IEnumerable<string> Get()
        //{
         
        //    var customerEntities = new CustomerDb();
        //    var customers = customerEntities.GetCustomers();
        //    return customers.Select(c => c.Address).ToList();
        //}

    // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var customerEntities = new CustomerDb();
            return customerEntities.GetDog().Name;

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
            var customerEntities = new CustomerDb();
            customerEntities.createTableAnddrop();
        }

        [HttpGet]
        [Route("left")]
        public void Left()
        {
            var customerEntities = new CustomerDb();
            var c = customerEntities.LeftJoin();
        }
    }


    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public List<CustomerTitle> Titles { get; set; }
    }


    public class CustomerTitle
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CustomerId { get; set; }
    }


    public class Post
    {
        public User Owner { get; set; }
    }

    public class User
    {
        
    }

    public class Dog
    {
        public int? Age { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float? Weight { get; set; }

        public int IgnoredProperty { get { return 1; } }
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

        public Dog GetDog()
        {
            using (
                System.Data.SqlClient.SqlConnection sqlConnection =
                    new System.Data.SqlClient.SqlConnection(connectionstring))
            {
                var dog = sqlConnection.Query<Dog>("select name=@Name,Age = @Age, Id = @Id",
                    new
                    
                    {
                        
                        Name= "hello",
                        Age = (int?) null,
                        Id = Guid.NewGuid()
                    });
                return dog.SingleOrDefault();
            }
        }


        public void createTableAnddrop()
        {
            using (
                System.Data.SqlClient.SqlConnection sqlConnection =
                    new System.Data.SqlClient.SqlConnection(connectionstring))
            {

                sqlConnection.Execute(
                    "create table #t(i int);insert into #t select @a union all select @b ;drop table #t ",
                    new {a = "111", b = "222"});


            }
        }

        public IList<Customer> LeftJoin()
        {
            using (
                System.Data.SqlClient.SqlConnection sqlConnection =
                    new System.Data.SqlClient.SqlConnection(connectionstring))
            {

                var lookUp = new Dictionary<int, Customer>();
                sqlConnection.Query<Customer, CustomerTitle, Customer>(
                    "select c.CustomerId, c.FirstName, c.LastName, c.Address, c.City, " +
                    "ct.Id,ct.Title,ct.CustomerId from Customer c left join CustomerTitle ct on c.CustomerId = ct.CustomerId ",
                    (c, ct) =>
                    {

                        Customer h = null;
                        if (!lookUp.TryGetValue(c.CustomerId, out h))
                        {
                            h = c;
                            lookUp.Add(c.CustomerId, h);
                        }

                        if (h.Titles == null)
                        {
                            h.Titles = new List<CustomerTitle>();
                        }
                        if (ct != null)
                        {
                            h.Titles.Add(ct);
                        }
                        return h;
                    }
                    ).ToList();


                var customers = lookUp.Values.ToList();
                return customers;
            }
        }
    }






}
