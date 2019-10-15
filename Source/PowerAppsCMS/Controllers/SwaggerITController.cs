using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using PowerAppsCMS.Models;
using Swashbuckle.Swagger.Annotations;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using PowerAppsCMS.Constants;

namespace PowerAppsCMS.Controllers
{
    /// <summary>
    /// Controller untuk mendapatkan user
    /// </summary>
    public class SwaggerITController : ApiController
    {
        [Route("api/GetInspectorUnitDetailIT")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, Description = "Get inspector unit detail", Type = typeof(ProductionPlanningUnitDetail))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Internal Server Error", Type = typeof(Exception))]
        [SwaggerOperation("GetInspectorUnitDetailIT")]

        public IHttpActionResult GetInspectorUnitDetailIT(int productID, int unitID)
        {
            using (PowerAppsCMSEntities db = new PowerAppsCMSEntities())
            {
                List<ProductionPlanningUnitDetail> listInspectorUnitDetail = new List<ProductionPlanningUnitDetail>();
                try
                {
                    foreach (Process itemProcess in db.Processes.Where(x => x.MasterProcess.ProductID == productID && x.UnitID == unitID))
                    {
                        ProductionPlanningUnitDetail inspectorPlanningUnitDetail = new ProductionPlanningUnitDetail();
                        inspectorPlanningUnitDetail.ProcessID = itemProcess.ID;
                        inspectorPlanningUnitDetail.ProcessGroupID = itemProcess.MasterProcess.ProcessGroupID;
                        inspectorPlanningUnitDetail.StatusID = itemProcess.Status;
                        var statusID = inspectorPlanningUnitDetail.StatusID;
                        if (statusID == (int)ProcessStatus.NotStarted)
                        {
                            inspectorPlanningUnitDetail.StatusName = "Not Started";
                        }
                        else if (statusID == (int)ProcessStatus.OnProcess)
                        {
                            inspectorPlanningUnitDetail.StatusName = "On Process";
                        }
                        else if (statusID == (int)ProcessStatus.ONProcessLate)
                        {
                            inspectorPlanningUnitDetail.StatusName = "On Process (Late)";
                        }
                        else if (statusID == (int)ProcessStatus.StopByOperator)
                        {
                            inspectorPlanningUnitDetail.StatusName = "Stopped By Operator";
                        }
                        else if (statusID == (int)ProcessStatus.WaitForQC)
                        {
                            inspectorPlanningUnitDetail.StatusName = "Wait For QC";
                        }
                        else if (statusID == (int)ProcessStatus.QCPassed)
                        {
                            inspectorPlanningUnitDetail.StatusName = "QC Passed";
                        }
                        else if (statusID == (int)ProcessStatus.QCNotGood)
                        {
                            inspectorPlanningUnitDetail.StatusName = "QC Not Good";
                        }
                        else if (statusID == (int)ProcessStatus.Finish)
                        {

                            inspectorPlanningUnitDetail.StatusName = "Finish";
                        }

                        inspectorPlanningUnitDetail.ProcessName = itemProcess.MasterProcess.Name;
                        inspectorPlanningUnitDetail.ManHour = itemProcess.MasterProcess.ManHour;
                        inspectorPlanningUnitDetail.ManPower = itemProcess.MasterProcess.ManPower;
                        inspectorPlanningUnitDetail.MasterProcessID = itemProcess.MasterProcessID;
                        inspectorPlanningUnitDetail.LastModifiedby = itemProcess.LastModifiedBy;
                        listInspectorUnitDetail.Add(inspectorPlanningUnitDetail);
                    }
                    return Ok(listInspectorUnitDetail);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }

        }
        /// <summary>
        /// Mengambil daftar operator yang masih aktif
        /// </summary>
        /// <returns>Daftar operator yang masih aktif</returns>

        /// <summary>
        /// Mengambil daftar operator PB
        /// </summary>
        /// <returns>Daftar operator PB</returns>
        [Route("api/GetGroupLeaderProductionPlanningIT")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "Get specific production planning",
            Type = typeof(ProductionPlanning))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                Description = "Internal Server Error",
               Type = typeof(Exception))]
        [SwaggerOperation("GetGroupLeaderProductionPlanningIT")]

        public IHttpActionResult GetGroupLeaderProductionPlanningIT(int processGroupID, int ProductGroupID)
        {
            using (PowerAppsCMSEntities db = new PowerAppsCMSEntities())
            {
                List<ProductionPlanning> listProductionPlanning = new List<ProductionPlanning>();
                List<Process> selectedProcesslist = null;
                DateTime now = DateTime.Now;
                try
                {
                    var itemProcess = db.Processes.Where(x => x.MasterProcess.ProcessGroupID == processGroupID && x.Unit.IsHold == false && x.Unit.SFSDueDate <= x.Unit.MPSDueDate && x.Unit.Product.ProductSubGroups.ProductGroup.ID == ProductGroupID).Select(x => x.Unit).Distinct();

                    foreach (Unit itemUnit in itemProcess)
                    {
                        ProductionPlanning productionPlanning = new ProductionPlanning();
                        productionPlanning.UnitID = itemUnit.ID;
                        //productionPlanning.MasterProcessID = productionPlannings.MasterProcessID;
                        //productionPlanning.p = item.Processes;
                        productionPlanning.SerialNumber = itemUnit.SerialNumber;
                        productionPlanning.PRO = itemUnit.PRO.Number;
                        productionPlanning.Product = itemUnit.Product.Name;
                        productionPlanning.Customer = itemUnit.PRO.CustomerListInSODisplayText;// Customer;
                        productionPlanning.ProductID = itemUnit.ProductID;
                        //productionPlanning.PGID = itemUnit.Product.ProductSubGroups.ProductGroup.ID;
                        //productionPlanning.PGName = itemUnit.Product.ProductSubGroups.ProductGroup.Name;
                        var prs = db.Processes.Where(y => y.MasterProcess.ProductID == itemUnit.ProductID && y.Unit.ID == itemUnit.ID);
                        foreach (Process item in prs)
                        {
                            if (item.Status == 7)
                            {
                                productionPlanning.StatusProcess = 1;
                            }
                            else
                            {
                                productionPlanning.StatusProcess = 0;
                            }

                        }
                        selectedProcesslist = db.Processes.Where(x => x.MasterProcess.ProductID == productionPlanning.ProductID && x.UnitID == productionPlanning.UnitID && x.MasterProcess.ProcessGroupID == processGroupID).ToList();
                        int countSelectedProcessList = selectedProcesslist.Count();
                        if (selectedProcesslist.Where(x => x.PlanStartDate > now && x.Status == (int)ProcessStatus.NotStarted).Count() == countSelectedProcessList)
                        {
                            productionPlanning.StatusID = (int)UnitStatus.NotStarted;
                            productionPlanning.StatusName = "Not Started";
                        }
                        else if (selectedProcesslist.Where(x => (x.Status != (int)ProcessStatus.QCPassed || x.Status != (int)ProcessStatus.Finish) && (x.ProcessIssues.Count(y => y.Status == false) > 0)).Count() > 0)
                        {
                            productionPlanning.StatusID = (int)UnitStatus.Issue;
                            productionPlanning.StatusName = "Issue";
                        }
                        else if (selectedProcesslist.Where(x => now > x.PlanEndDate && (x.Status != (int)ProcessStatus.QCPassed || x.Status != (int)ProcessStatus.Finish)).Count() >= 1)
                        {
                            productionPlanning.StatusID = (int)UnitStatus.Issue;
                            productionPlanning.StatusName = "Process Late";
                        }
                        else if (selectedProcesslist.Where(x => x.Status == (int)ProcessStatus.QCNotGood).Count() >= 1)
                        {
                            productionPlanning.StatusID = (int)UnitStatus.QCNotGood;
                            productionPlanning.StatusName = "QC Not Good";
                        }
                        else
                        {
                            productionPlanning.StatusID = (int)UnitStatus.OnProcess;
                        }
                        listProductionPlanning.Add(productionPlanning);
                    }
                    return Ok(listProductionPlanning);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }



        //API LOGIN
        [Route("api/LoginIT")]
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK,
            Description = "Login using PIN",
            Type = typeof(UserModel))]
        [SwaggerResponse(HttpStatusCode.InternalServerError,
                Description = "Internal Server Error",
               Type = typeof(Exception))]
        [SwaggerOperation("LoginIT")]
        public IHttpActionResult LoginIT(string userName, string PIN)
        {
            PowerAppsCMSEntities db = new PowerAppsCMSEntities();
            try
            {
                var hashedPin = EncryptPin(PIN);
                var findUser = db.Users.Where(x => x.PIN == hashedPin && x.EmployeeNumber == userName && x.IsActive == true).SingleOrDefault();
                if (findUser != null)
                {
                    var user = new UserModel
                    {
                        LoginStatus = 1,
                        ID = findUser.ID,
                        Name = findUser.Name,
                        EmployeeNumber = findUser.EmployeeNumber,
                        RoleID = findUser.RoleID,
                        ProcessGroupID = findUser.ProcessGroupID == null ? 0 : findUser.ProcessGroupID,
                        ParentUserID = findUser.ParentUserID,
                        Email = findUser.Email,
                        IsActive = findUser.IsActive,
                        PersonID = findUser.PersonID,
                        ProcessGroupName = findUser.ProcessGroupID == null ? "NONAME" : findUser.ProcessGroup.Name,
                        RoleName = findUser.Role.Name,
                        PIN = findUser.PIN,
                        UserName = findUser.Username
                    };
                    return Ok(user);
                }

                else
                {
                    var user = new UserModel
                    {
                        LoginStatus = 0
                    };
                    return Ok(user);
                }

                //return null;
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public string EncryptPin(string pin)
        {
            MD5 md5 = MD5.Create();
            byte[] hashPin = md5.ComputeHash(Encoding.Default.GetBytes(pin));
            StringBuilder returnHashedPin = new StringBuilder();

            for (int i = 0; i < hashPin.Length; i++)
            {
                returnHashedPin.Append(hashPin[i].ToString());
            }

            return returnHashedPin.ToString();
        }
    }
}
