using BusinessLayer.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Entity;
using Microsoft.Extensions.Logging; // Add this
using ModelLayer.Entity;
using ModelLayer.RequestDTO;
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
        private readonly ILogger<PaymentProcessController> _logger; // Add this

        public PaymentProcessController(IPaymentProcessBL paymentProcessBL, ILogger<PaymentProcessController> logger) // Modify the constructor
        {
            _paymentProcessBL = paymentProcessBL;
            _logger = logger; // Initialize the logger
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
                _logger.LogError(ex, "An error occurred while processing the payment"); // Use the injected logger
                var response = new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while processing the payment"
                };
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            try
            {
                var result = await _paymentProcessBL.GetAllPayments();
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<PaymentModel>>
                    {
                        Success = true,
                        Message = "All payments retrieved successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new ResponseModel<PaymentModel>
                    {
                        Success = false,
                        Message = "No Payments found"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Payments");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<PaymentModel>
                {
                    Success = false,
                    Message = "An error occurred while retrieving Payments"
                });
            }
        }

        [HttpGet("getbycustomerid")]
        public async Task<IActionResult> GetPaymentById(int CustomerId)
        {
            try
            {
                var result = await _paymentProcessBL.GetPaymentById(CustomerId);
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<PaymentModel>>
                    {
                        Success = true,
                        Message = "All payments retrieved successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                else
                {
                    return BadRequest(new ResponseModel<PaymentModel>
                    {
                        Success = false,
                        Message = "No Payments found"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving Payments");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<PaymentModel>
                {
                    Success = false,
                    Message = "An error occurred while retrieving Payments"
                });
            }
        }

        [HttpGet("generateReceipt/{paymentId}")]
        public async Task<IActionResult> GetRecieptByPaymementId(int paymentId)
        {
            try
            {
                var result = await _paymentProcessBL.GetRecieptByPaymementId(paymentId);
                if (result != null)
                {
                    var response = new ResponseModel<IEnumerable<ReceiptDetails>>
                    {
                        Success = true,
                        Message = "Receipt generated successfully",
                        Data = result
                    };
                    return Ok(response);
                }
                else
                {
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "No receipt found"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the receipt");
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An error occurred while generating the receipt"
                });
            }
        }

        [HttpPost("calculate-premium")]
        public async Task<IActionResult> CalculatePremium([FromBody] PremiumCalculationRequest request)
        {
            try
            {
                var premium = await _paymentProcessBL.CalculatePremium(request.PolicyId, request.CustomerAge, request.CoverageAmount,request.PolicyType,request.paymentFrequency,request.TermYears);
                return Ok(new { Premium = premium });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating premium"); // Use the injected logger
                return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = "An error occurred while calculating premium" });
            }
        }
    }
}
