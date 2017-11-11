using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MVCWebGridAppl.Models;
using System.Collections;
using System.Collections.Generic;

namespace MVCWebGridAppl.Controllers
{
    public class DBDataApiController : ApiController
    {
        public HttpResponseMessage GetDataFromDB()
        {
            HttpResponseMessage response;
            try
            {
                using (var ctx = new AdventureWorks2012Entities())
                {
                    IEnumerable<ResponseDataModel> query = (from d in ctx.Departments
                                                            join h in ctx.EmployeeDepartmentHistories on d.DepartmentID equals h.DepartmentID
                                                            join s in ctx.Shifts on h.ShiftID equals s.ShiftID
                                                            join e in ctx.Employees on h.BusinessEntityID equals e.BusinessEntityID
                                                            join p in ctx.People on h.BusinessEntityID equals p.BusinessEntityID
                                                            select new ResponseDataModel
                                                            {
                                                                Name = ((p.FirstName == null ? "" : p.FirstName) + " " + (p.MiddleName == null ? "" : p.MiddleName) + " " + (p.LastName == null ? "" : p.LastName)),
                                                                MaritalStatus = (e.MaritalStatus == "M" ? "Married" : "Single"),
                                                                Gender = (e.Gender == "M" ? "Male" : "FeMale"),
                                                                Designation = e.JobTitle,
                                                                Department = d.Name,
                                                                MajorBranch = d.GroupName
                                                            }).ToList();
                    if (query.Count() == 0)
                    {
                        response = Request.CreateResponse(HttpStatusCode.NotFound);
                        return response;
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, query);
                    return response;
                }
            }
            catch (Exception)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound);
                return response;
            }
            finally
            {

            }
        }
    }
}
