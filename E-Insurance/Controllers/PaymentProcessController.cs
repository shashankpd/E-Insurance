using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using Response;
using System;
using System.Threading.Tasks;

namespace E_Insurance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentProcessController : ControllerBase
    {
        private readonly IPaymentProcessBL _paymentProcessBL;

        public PaymentProcessController(IPaymentProcessBL paymentProcessBL)
        {
            _paymentProcessBL = paymentProcessBL;
        }

        [HttpPost("payment")]
        public async Task<IActionResult> AddPayment([FromBody] Payment payment)
        {
            try
            {
                var result = await _paymentProcessBL.AddPayment(payment);
                if (result)
                {
                    var response = new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Payment done successfully",
                        Data = "Payment has been processed."
                    };
                    return CreatedAtAction(nameof(AddPayment), response);
                }
                else
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid input or business rule violation"
                    });
                }
            }
            catch (PolicyAlreadyExistsException ex)
            {
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                
                ILogger<PaymentProcessController> logger =
                    new Logger<PaymentProcessController>(new LoggerFactory());
                logger.LogError(ex, "An error occurred while processing the payment");

               
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while processing the payment"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
