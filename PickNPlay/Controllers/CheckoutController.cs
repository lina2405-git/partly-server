using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PickNPlay.Exceptions;
using PickNPlay.Helpers;
using PickNPlay.picknplay_bll.Models.Deposit;
using PickNPlay.picknplay_bll.Models.Transaction;
using PickNPlay.picknplay_bll.Services;
using Stripe;
using Stripe.Checkout;

namespace PickNPlay.Controllers
{
    [ApiController]
    [Route("checkout")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class CheckoutController : ControllerBase
    {
        private readonly IConfiguration configuration;

        private int userId;
#if DEBUG
        string frontUrl = "http://localhost:4200";
#else
        string frontUrl = "http://localhost:4200";
#endif

        private readonly ListingService listingService;
        private readonly picknplay_bll.Services.TransactionService transactionService;
        private readonly UserService userService;
        private readonly DepositService depositService;
        private static string s_wasmClientUrl;

        public CheckoutController(IConfiguration configuration,
            ListingService listingService,
            picknplay_bll.Services.TransactionService transactionService,
            UserService userService,
            DepositService depositService)
        {
            this.configuration = configuration;
            this.listingService = listingService;
            this.transactionService = transactionService;
            this.userService = userService;
            this.depositService = depositService;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CheckoutOrder([FromQuery] int listingId,
                                                        [FromQuery] int amount,
                                                        [FromServices] IServiceProvider sp)
        {
            //keeping orderId
            userId = JWTTranslator.GetUserId(HttpContext) ?? throw new ArgumentException("Problem with the token");
            //retrieving the listing
            var listing = await listingService.GetByIdWithoutReviewsAsync(listingId) ?? throw new ArgumentException($"Listing with this id was not found");
            //checking if we have enough in stock
            if (listing.Amount < amount)
            {
                return Conflict("The requested amount is more than left in stock");
            }

            decimal fee = listing.CategoryId == 25 ? 1.05m : 1.1m;

            //counting the total amount to pay
            var totalAmountFinal = amount * listing.FinalPrice * fee;

            //counting total amount to give seller
            var totalAmountSeller = amount * listing.SellerPrice;


            //retrieving user balance
            var userBalance = await userService.GetBalanceById(userId);

            //checking if the user has enough cash to pay
            if (userBalance < totalAmountFinal)
            {
                //if not => creating deposit
                var paymentUrl = await CreateDepositForAccount(Math.Ceiling(totalAmountFinal - userBalance));
                return StatusCode(402, new { message = "Payment required", url = paymentUrl });
            }
            //if it is => creating transaction
            var model = new TransactionPost()
            {
                BuyerPaid = totalAmountFinal,
                SellerGet = totalAmountSeller,
                BuyerId = userId,
                SellerId = listing.UserId,
                ListingId = listingId,
                Amount = amount,
            };

            await transactionService.AddAsync(model);

            return Ok();

        }

        /// <summary>
        /// returns url to proceed the deposit to account
        /// просто пополняет счет
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("deposit")]
        public async Task<string> CreateDepositForAccount(decimal sum)
        {
            var temp = (sum + sum * 0.03m + 0.3m) * 100;
            var options = new SessionCreateOptions()
            {
                SuccessUrl = $"{frontUrl}/deposit/success?sessionId=" + "{CHECKOUT_SESSION_ID}",
                CancelUrl = $"{frontUrl}/deposit/failed?sessionId=" + "{CHECKOUT_SESSION_ID}",
                PaymentMethodTypes = new List<string>()
                {
                    "card"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new()
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)temp,
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Make deposit to account",
                                Description = $"You do not have enough cash({sum}$). Proceed the payment to add some cash to your balance. Be aware that payment system charges fee in amount of 3%.",
                                //Images = new List<string>(){ listing.GameLogoUrl }
                            },
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            DepositPost dep = new()
            {
                DepositAmount = sum,
                SessionId = session.Id,
                UserId = JWTTranslator.GetUserId(HttpContext) ?? throw new Exception("Token is broken")
            };


            await depositService.AddAsync(dep);

            return session.Url;

        }

        [Authorize]
        [HttpPut("deposit/success")]
        public async Task<ActionResult> CreateSuccessDeposit([FromQuery] string sessionId)
        {
            try
            {
                var sessionService = new SessionService();
                var session = await sessionService.GetAsync(sessionId) ?? throw new ArgumentException("Session does not exist");

                if (IsSessionSuccessed(session))
                {

                    var isCompleted = await depositService.TransferMoney(sessionId);
                    if (isCompleted)
                    {
                        return Ok("Deposit was succesfully made");
                    }
                    return BadRequest("Error occured. Check passed parameters.");
                }

                return StatusCode(402, "Money is not paid (ты кого хочешь ноебат?). Deposit failed");
            }
            catch (SecurityTokenException e)
            {
                return Unauthorized(e.Message);
            }
            catch (ArgumentException e)
            {
                return NotFound(e.Message);
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (DALException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        //[Authorize]
        //[HttpPost("outcome/card_info")]
        //public async Task<ActionResult> CreateOutcomeRequestByCardInfo(decimal sum, CardInfo cardInfo)
        //{
        //    try
        //    {
        //        //creating a token
        //        var options = new TokenCreateOptions
        //        {
        //            Card = new TokenCardOptions
        //            {
        //                Number = cardInfo.CardNumber,
        //                ExpMonth = cardInfo.Month.ToString(),
        //                ExpYear = cardInfo.Year.ToString(),
        //                Cvc = cardInfo.CVC.ToString(),
        //            },
        //        };
        //        var service = new Stripe.TokenService();
        //        //token itself
        //        Stripe.Token stripeToken = service.Create(options);

        //        //creating a connected account
        //        var accountOptions = new AccountCreateOptions
        //        {
        //            Type = "custom",
        //            BusinessType = "individual",
        //            Individual = new AccountIndividualOptions
        //            {
        //                FirstName = cardInfo.FirstName,
        //                LastName = cardInfo.LastName,
        //            },
        //            TosAcceptance = new AccountTosAcceptanceOptions
        //            {
        //                Date = DateTimeOffset.UtcNow.Date,
        //                Ip = "127.0.0.1", // Assumes you're using this IP address
        //            },
        //            ExternalAccount = stripeToken.Id,
        //        };

        //        var accountService = new AccountService();
        //        //account itself
        //        Account account = accountService.Create(accountOptions);

        //        //making a payout
        //        var payoutOptions = new TransferCreateOptions
        //        {
        //            Amount = Convert.ToInt64(sum * 100), // Amount in cents
        //            Currency = "usd",
        //            Destination = account.Id,
        //        };
        //        var transferService = new TransferService();
        //        Transfer transfer = transferService.Create(payoutOptions);

        //        return Ok(transfer);

        //    }
        //    catch
        //    {

        //        throw;
        //    }
        //}


        [HttpPost("charge")]
        public IActionResult Charge([FromBody] ChargeRequest request)
        {

            var options = new ChargeCreateOptions
            {
                Amount = request.Amount * 100,
                Currency = "usd",
                Source = request.Token,
                Description = "Test description",

            };

            var service = new ChargeService();
            Charge charge = service.Create(options);

            if (charge.Status == "succeeded")
            {
                return Ok(charge);
            }
            else
            {
                return BadRequest(request);
            }
        }
        public class ChargeRequest
        {
            public string Token { get; set; }
            public long Amount { get; set; }
        }

        //[HttpPost("payout")]
        //public ActionResult Charge([FromBody] ChargeRequest request, decimal sum)
        //{

        //    var options = new ChargeCreateOptions
        //    {
        //        Amount = Convert.ToInt64(sum * 100), // Сумма в центах
        //        Currency = "usd",
        //        Description = "Example charge",
        //        Source = request.Token, // Токен, полученный от клиента
        //    };

        //    var service = new ChargeService();
        //    Charge charge = service.Create(options);

        //    return Ok(charge);
        //}




        //[Authorize]
        //[HttpPost("outcome/paypal")]
        //public async Task<ActionResult> CreateOutcomeRequestViaPayPal(decimal sum, string cardNumber)
        //{
        //    var _config = ConfigManager.Instance.GetProperties();
        //    var _accessToken = new OAuthTokenCredential(_config).GetAccessToken();

        //    var user = await userService.GetByIdAsync(JWTTranslator.GetUserId(HttpContext).Value);

        //    var apiContext = new APIContext(_accessToken)
        //    {
        //        Config = ConfigManager.Instance.GetProperties(),
        //    };

        //    try
        //    {
        //        var payout = new PayPal.Api.Payout()
        //        {
        //            sender_batch_header = new PayoutSenderBatchHeader()
        //            {
        //                sender_batch_id = "batch_" + Guid.NewGuid().ToString(),
        //                email_subject = "You have a payout!"
        //            },
        //            items = new List<PayoutItem>()
        //            {
        //                new PayoutItem()
        //                {
        //                    recipient_type = PayoutRecipientType.EMAIL,
        //                    amount = new Currency()
        //                    {
        //                        value = sum.ToString(),
        //                        currency = "USD"
        //                    },
        //                    receiver = user.Email,
        //                    note = "Thanks for choosing us!",
        //                    sender_item_id = "item_" + Guid.NewGuid().ToString()
        //                }
        //            }
        //        };

        //        var createdPayout = payout.Create(apiContext);

        //        if (createdPayout.batch_header.batch_status == "SUCCESS")
        //        {
        //            //create a table "payouts" and save a transaction
        //            return Ok("Payout created successfully!");
        //        }
        //        else
        //        {
        //            return StatusCode(500, "Error creating payout.");
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}




        private bool IsSessionSuccessed(Session session)
        {
            if (session.PaymentStatus == "paid")
            {
                return true;
            }

            return false;
        }
    }
}
