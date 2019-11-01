using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;
using System.Data.Entity;

namespace Vidly.Controllers.API
{
    public class CustomersController : ApiController 
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }



        //previous- GET/api/customers
        //public IEnumerable<CustomerDto> GetCustomers()
        //{
        //    return _context.Customers.ToList().Select(Mapper.Map<Customer,CustomerDto>);
        //}
        

        #region GET/api/customers
 
        public IHttpActionResult GetCustomers()
        {
            //var customerDtos = _context.Customers.ToList().Select(Mapper.Map<Customer, CustomerDto>);

            var customerDtos = _context.Customers
                .Include(c => c.MembershipType)
                .ToList()
                .Select(Mapper.Map<Customer, CustomerDto>);

            return Ok(customerDtos);
        }
        #endregion


        #region GET/api/customers/1

        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return NotFound();
            return Ok( Mapper.Map<Customer,CustomerDto>(customer));
        }
        #endregion
        

        #region POST /api/customers
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);
            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerDto.Id = customer.Id;
            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerDto);
        }

    #endregion
        

        //Previous- PUT/api/customers/1
//        [HttpPut]
//        public void UpdateCustomer(int id, CustomerDto customerDto)
//        {
//            if (!ModelState.IsValid)
//                throw new HttpResponseException(HttpStatusCode.BadRequest);

//            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

//            if (customerInDb == null)
//                throw new HttpResponseException(HttpStatusCode.NotFound);

//            Mapper.Map(customerDto,customerInDb);

//            _context.SaveChanges();

//;        }
        

        #region PUT /api/customers/1

        [HttpPut]
        public IHttpActionResult UpdateCustomer(int id, CustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);
            if (customerInDb == null)
                return NotFound();
            Mapper.Map(customerDto, customerInDb);
            _context.SaveChanges();
            return Ok();
        }

        #endregion


        //Previous- DELETE/api/customers/1

        //[HttpDelete]

        //public void DeleteCustomer(int id)
        //{
        //    var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

        //    if (customerInDb == null)
        //        throw new HttpResponseException(HttpStatusCode.NotFound);

        //    _context.Customers.Remove(customerInDb);
        //    _context.SaveChanges();

        //}


        #region // DELETE /api/customers/1

        [HttpDelete]
        public IHttpActionResult DeleteCustomer(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                return NotFound();
            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();
            return Ok();

        }

        #endregion
    }
}
